using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Player.ActiveTasks
{
    public class RivalGangAmbushTask : IPlayerTask
    {
        private ITaskAssignable Player;
        private ITimeReportable Time;
        private IGangs Gangs;
        private IGangTerritories GangTerritories;
        private IZones Zones;
        private PlayerTasks PlayerTasks;
        private IPlacesOfInterest PlacesOfInterest;
        private List<DeadDrop> ActiveDrops = new List<DeadDrop>();
        private ISettingsProvideable Settings;
        private IEntityProvideable World;
        private ICrimes Crimes;
        private Gang HiringGang;
        private GangDen HiringGangDen;
        private Gang TargetGang;
        private Zone TargetZone;
        private PlayerTask CurrentTask;
        private int GameTimeToWaitBeforeComplications;
        private int MoneyToRecieve;
        private bool HasAddedComplications;
        private bool WillAddComplications;
        private int KilledMembersAtStart;
        private int ExternalZoneKills = 0;
        private int InternalZoneKills = 0;
        private PhoneContact PhoneContact;
        private GangTasks GangTasks;
        public int KillRequirement { get; set; } = 1;
        private bool HasConditions => HiringGangDen != null && TargetZone != null;

        public bool JoinGangOnComplete { get; set; } = false;

        public RivalGangAmbushTask(ITaskAssignable player, ITimeReportable time, IGangs gangs, PlayerTasks playerTasks, IPlacesOfInterest placesOfInterest, List<DeadDrop> activeDrops, ISettingsProvideable settings, IEntityProvideable world, ICrimes crimes
            ,PhoneContact phoneContact, GangTasks gangTasks, Gang targetGang, int killRequirement, IGangTerritories gangTerritories, IZones zones)
        {
            Player = player;
            Time = time;
            Gangs = gangs;
            PlayerTasks = playerTasks;
            PlacesOfInterest = placesOfInterest;
            ActiveDrops = activeDrops;
            Settings = settings;
            World = world;
            Crimes = crimes;
            PhoneContact = phoneContact;
            GangTasks = gangTasks;
            TargetGang = targetGang;
            KillRequirement = killRequirement;
            GangTerritories = gangTerritories;
            Zones = zones;
        }
        public void Setup()
        {

        }
        public void Dispose()
        {

        }
        public void Start(Gang ActiveGang)
        {
            HiringGang = ActiveGang;
            if (PlayerTasks.CanStartNewTask(ActiveGang?.ContactName))
            {
                GetTargetZone();
                GetHiringDen();
                if (HasConditions)
                {
                    GetPayment();
                    SendInitialInstructionsMessage();
                    AddTask();
                    GameFiber PayoffFiber = GameFiber.StartNew(delegate
                    {
                        try
                        {
                            Loop();
                            FinishTask();
                        }
                        catch (Exception ex)
                        {
                            EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                            EntryPoint.ModController.CrashUnload();
                        }
                    }, "PayoffFiber");
                }
                else
                {
                    GangTasks.SendGenericTooSoonMessage(PhoneContact);
                }
            }
        }
        private void Loop()
        {
            while (true)
            {
                CurrentTask = PlayerTasks.GetTask(HiringGang.ContactName);
                if (CurrentTask == null || !CurrentTask.IsActive)
                {
                    break;
                }
                if (Zones.GetZone(Player.Character.Position) != TargetZone)
                {
                    ExternalZoneKills = Player.RelationshipManager.GangRelationships.GetReputation(TargetGang).MembersKilled - KilledMembersAtStart - InternalZoneKills;
                }
                else
                {
                    InternalZoneKills = Player.RelationshipManager.GangRelationships.GetReputation(TargetGang).MembersKilled - KilledMembersAtStart - ExternalZoneKills;
                }
                if (InternalZoneKills >= KillRequirement)
                {
                    CurrentTask.OnReadyForPayment(true);
                    break;
                }
                GameFiber.Sleep(1000);
            }
        }
        private void FinishTask()
        {
            if (CurrentTask != null && CurrentTask.IsActive && CurrentTask.IsReadyForPayment)
            {
                //GameFiber.Sleep(RandomItems.GetRandomNumberInt(5000, 15000));
                SendMoneyPickupMessage();
            }
        }
        private void GetTargetZone()
        {
            if (TargetGang == null)
            {
                return;
            }
            List<ZoneJurisdiction> totalTerritories = GangTerritories.GetGangTerritory(TargetGang.ID)?.Where(x=> x.Priority == 0).ToList();
            if(totalTerritories == null || !totalTerritories.Any())
            {
                return;
            }
            ZoneJurisdiction selectedTerritory = totalTerritories.PickRandom();
            if (selectedTerritory != null)
            {
                TargetZone = Zones.GetZone(selectedTerritory.ZoneInternalGameName);
            }
        }
        private void GetHiringDen()
        {
            HiringGangDen = PlacesOfInterest.GetMainDen(HiringGang.ID, World.IsMPMapLoaded, Player.CurrentLocation);
        }
        private void GetPayment()
        {
            MoneyToRecieve = RandomItems.GetRandomNumberInt(HiringGang.HitPaymentMin, HiringGang.HitPaymentMax).Round(500);
            MoneyToRecieve *= KillRequirement;
            if (MoneyToRecieve <= 0)
            {
                MoneyToRecieve = 500;
            }
        }
        private void AddTask()
        {
            GameTimeToWaitBeforeComplications = RandomItems.GetRandomNumberInt(3000, 10000);
            HasAddedComplications = false;
            WillAddComplications = false;// RandomItems.RandomPercent(Settings.SettingsManager.TaskSettings.RivalGangHitComplicationsPercentage);
            GangReputation gr = Player.RelationshipManager.GangRelationships.GetReputation(TargetGang);
            KilledMembersAtStart = gr.MembersKilled;
            PlayerTasks.AddTask(HiringGang.Contact, MoneyToRecieve, 2000, 0, -500, 7, "Rival Gang Ambush");
        }
        private void SendInitialInstructionsMessage()
        {
            List<string> Replies = new List<string>() {
                $"They think they can push us around? Not today. Get to {TargetGang.ColorPrefix}{TargetZone.DisplayName}~s~ and take out {KillRequirement} of those {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ bitches. Return to {HiringGang.DenName} on {HiringGangDen.FullStreetAddress} when you're done, and I'll make sure you’re paid ~g~${MoneyToRecieve}~s~.",
                $"Those {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ think they can fuck with me? Go to {TargetGang.ColorPrefix}{TargetZone.DisplayName}~s~ and give {KillRequirement} of those pricks a dirt nap. Once you are done come back to the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress}. ~g~${MoneyToRecieve}~s~ to you",
                $"We’re sending a clear message to {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~. Go to {TargetGang.ColorPrefix}{TargetZone.DisplayName}~s~ and deal with {KillRequirement} of their men. After the job, swing by {HiringGang.DenName} on {HiringGangDen.FullStreetAddress} for your ~g~${MoneyToRecieve}~s~.",
                $"{TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ decided to make some moves against us. Go over to {TargetGang.ColorPrefix}{TargetZone.DisplayName}~s~ and let them know we don't approve by sending {KillRequirement} of those assholes to the other side. I'll have ~g~${MoneyToRecieve}~s~ waiting for you.",
                $"Go to {TargetGang.ColorPrefix}{TargetZone.DisplayName}~s~ and find {KillRequirement} of those {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ pricks. Make sure they won't ever talk to anyone again. Come back to the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress} for your payment of ~g~${MoneyToRecieve}~s~",
                    };
            Player.CellPhone.AddPhoneResponse(HiringGang.Contact.Name, HiringGang.Contact.IconName, Replies.PickRandom());
        }
        private void SendMoneyPickupMessage()
        {
            List<string> Replies = new List<string>() {
                                $"Seems like that thing we discussed is done? Come by the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress} to collect the ~g~${MoneyToRecieve}~s~",
                                $"Word got around that you are done with that thing for us, Come back to the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress} for your payment of ~g~${MoneyToRecieve}~s~",
                                $"Get back to the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress} for your payment of ~g~${MoneyToRecieve}~s~",
                                $"{HiringGangDen.FullStreetAddress} for ~g~${MoneyToRecieve}~s~",
                                $"Heard you were done, see you at the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress}. We owe you ~g~${MoneyToRecieve}~s~",
                                };
            Player.CellPhone.AddScheduledText(PhoneContact, Replies.PickRandom(), 1, false);
        }
    }
}
