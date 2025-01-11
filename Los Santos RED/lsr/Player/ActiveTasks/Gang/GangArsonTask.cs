using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.ArrayExtensions;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Player.ActiveTasks
{
    public class GangArsonTask : IPlayerTask
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
        private GameLocation TorchLocation;
        private Gang EnemyGang;
        private Zone SelectedZone;
        private PlayerTask CurrentTask;
        private int GameTimeToWaitBeforeComplications;
        private int MoneyToReceive;
        private bool HasAddedComplications;
        private bool WillAddComplications;
        private PhoneContact PhoneContact;
        private bool hasExploded = false;
        private GangTasks GangTasks;
        private bool WillTorchEnemyTurf;
        private bool HasConditions => TorchLocation != null && HiringGangDen != null;


        public GangArsonTask(ITaskAssignable player, ITimeReportable time, IGangs gangs, PlayerTasks playerTasks, IPlacesOfInterest placesOfInterest, List<DeadDrop> activeDrops, ISettingsProvideable settings, IEntityProvideable world, ICrimes crimes, 
            PhoneContact phoneContact, GangTasks gangTasks, IGangTerritories gangTerritories, IZones zones)
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
            GangTerritories = gangTerritories;
            Zones = zones;
        }
        public void Setup()
        {
            hasExploded = false;
        }
        public void Dispose()
        {
            if (TorchLocation != null) { TorchLocation.IsPlayerInterestedInLocation = false; }
        }
        public void Start(Gang ActiveGang)
        {
            HiringGang = ActiveGang;
            WillTorchEnemyTurf = RandomItems.RandomPercent(Settings.SettingsManager.TaskSettings.GangArsonEnemyTurfPercentage);

            if (PlayerTasks.CanStartNewTask(ActiveGang?.ContactName))
            {
                GetTorchLocation();
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
                bool isExplosion = NativeFunction.Natives.IS_EXPLOSION_IN_SPHERE<bool>(-1, TorchLocation.EntrancePosition.X, TorchLocation.EntrancePosition.Y, TorchLocation.EntrancePosition.Z, 50f) &&
                    Player.WeaponEquipment?.CurrentWeapon?.Hash == 615608432;
                if(isExplosion)
                {
                    hasExploded = true;
                }
                if (hasExploded)
                {
                    CurrentTask.OnReadyForPayment(false);
                    break;
                }
                GameFiber.Sleep(250);
            }
        }
        private void FinishTask()
        {
            if (CurrentTask != null && CurrentTask.IsActive && CurrentTask.IsReadyForPayment)
            {
                TorchLocation.IsPlayerInterestedInLocation = false;
                SendMoneyPickupMessage();
            }
            else if (CurrentTask != null && !CurrentTask.IsActive)
            {
                Dispose();
            }
            else
            {
                Dispose();
            }

        }
        private void GetTorchLocation()
        {
            if (GangTerritories.GetGangTerritory(HiringGang.ID) == null)
            {
                return;
            }
            List<ZoneJurisdiction> hiringGangTerritories = GangTerritories.GetGangTerritory(HiringGang.ID);
            if (hiringGangTerritories == null || !hiringGangTerritories.Any())
            {
                return;
            }
            List<GameLocation> PossibleSpots = PlacesOfInterest.PossibleLocations.RacketeeringTaskLocations().Where(x => x.IsCorrectMap(World.IsMPMapLoaded) && x.IsSameState(Player.CurrentLocation?.CurrentZone?.GameState)).ToList();
            List<GameLocation> AvailableSpots = new List<GameLocation>();
            List<ZoneJurisdiction> availableTerritories = new List<ZoneJurisdiction>();
            EnemyGang = null;
            if (WillTorchEnemyTurf)
            {
                //availableTerritories = hiringGangTerritories.Where(zj => zj.Priority != 0).ToList();
                if (HiringGang.EnemyGangs != null && HiringGang.EnemyGangs.Any())
                {
                    EnemyGang = Gangs.GetGang(HiringGang.EnemyGangs.PickRandom());
                }
                if (EnemyGang == null)
                {
                    EnemyGang = Gangs.GetAllGangs().Where(x => x.ID.ToLower() != HiringGang.ID.ToLower()).PickRandom();
                }
                if (EnemyGang != null)
                {
                    availableTerritories = GangTerritories.GetGangTerritory(EnemyGang.ID).ToList();
                }
            }
            else
            {
                availableTerritories = hiringGangTerritories.ToList();
            }
            if (!availableTerritories.Any() && WillTorchEnemyTurf)//fallback to friendly turf 
            {
                availableTerritories = hiringGangTerritories.ToList();
                WillTorchEnemyTurf = false;
            }
            if (!availableTerritories.Any())
            {
                return;
            }
            ZoneJurisdiction selectedTerritory = availableTerritories.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
            if (WillTorchEnemyTurf && EnemyGang == null)
            {
                EnemyGang = Zones.GetZone(availableTerritories.FirstOrDefault().ZoneInternalGameName).AssignedGang;
            }
            if (Zones.GetZone(selectedTerritory.ZoneInternalGameName) != null)
            {
                SelectedZone = Zones.GetZone(selectedTerritory.ZoneInternalGameName);
                foreach (GameLocation possibleSpot in PossibleSpots)
                {
                    Zone spotZone = Zones.GetZone(possibleSpot.EntrancePosition);
                    bool isNear = PlacesOfInterest.PossibleLocations.PoliceStations.Any(policeStation => possibleSpot.EntrancePosition.DistanceTo2D(policeStation.EntrancePosition) < 100f);
                    if (spotZone.InternalGameName == SelectedZone.InternalGameName && !isNear)// && !possibleSpot.HasVendor)
                    {
                        AvailableSpots.Add(possibleSpot);
                    }
                }
            }
            TorchLocation = AvailableSpots.PickRandom();
        }
        private void GetHiringDen()
        {
            HiringGangDen = PlacesOfInterest.GetMainDen(HiringGang.ID, World.IsMPMapLoaded, Player.CurrentLocation);
        }
        private void GetPayment()
        {
            MoneyToReceive = RandomItems.GetRandomNumberInt(HiringGang.ArsonPaymentMin, HiringGang.ArsonPaymentMax).Round(500);
            if (MoneyToReceive <= 0)
            {
                MoneyToReceive = 100;
            }
            if (WillTorchEnemyTurf)
            {
                MoneyToReceive *= 5;
            }
        }
        private void AddTask()
        {
            PlayerTasks.AddTask(HiringGang.Contact, MoneyToReceive, 500, 0, -2000, 7, "Arson");
            TorchLocation.IsPlayerInterestedInLocation = true;
            CurrentTask = PlayerTasks.GetTask(HiringGang.ContactName);
            CurrentTask.FailOnStandardRespawn = true;
        }
        private void SendInitialInstructionsMessage()
        {
            List<string> Replies;
            if (WillTorchEnemyTurf)
            {
                Replies = new List<string>() {
                $"Set fire to {TorchLocation.Name}. Make them feel the heat of our dominance. ~g~${MoneyToReceive}~s~ for your trouble.",
                $"Burn down {TorchLocation.Name} and show {EnemyGang.ColorPrefix}{EnemyGang.ShortName}~s~ they’re out of their league. Bigger payout this time: ~g~${MoneyToReceive}~s~.",
                $"Torch {TorchLocation.Name} and let them know we run this city. Take ~g~${MoneyToReceive}~s~ for your efforts.",
                $"Light up {TorchLocation.Name} and remind them that this is our territory. You’ll earn ~g~${MoneyToReceive}~s~ for the job.",
                $"Turn {TorchLocation.Name} into ashes and show {EnemyGang.ColorPrefix}{EnemyGang.ShortName}~s~ who really calls the shots. Your reward: ~g~${MoneyToReceive}~s~.",
                $"Set {TorchLocation.Name} ablaze and send a message that we’re in charge. Your cut: ~g~${MoneyToReceive}~s~."
                };
            }
            else
            {
                Replies = new List<string>() {
                $"Burn down {TorchLocation.Name} and remind them they can’t ignore us. You’ll get ~g~${MoneyToReceive}~s~ for your work, 10% cut like usual.",
                $"Set fire to {TorchLocation.Name} to show them we’re not messing around. Your reward: ~g~${MoneyToReceive}~s~.",
                $"Torch {TorchLocation.Name} and let them feel the consequences of defying us. ~g~${MoneyToReceive}~s~ for you.",
                $"Light {TorchLocation.Name} on fire and make them regret their unpaid debts. You’ll get ~g~${MoneyToReceive}~s~ for your trouble.",
                $"Burn {TorchLocation.Name} to the ground and send a message that we’re not here to negotiate. Your cut: ~g~${MoneyToReceive}~s~.",
                $"Set {TorchLocation.Name} on fire to remind them they can’t skip out on us. You’ll pocket ~g~${MoneyToReceive}~s~ for the job."
                };
            }


            Player.CellPhone.AddPhoneResponse(HiringGang.Contact.Name, HiringGang.Contact.IconName, Replies.PickRandom());
        }
        private void SendMoneyPickupMessage()
        {
            List<string> Replies = new List<string>() {
                                $"Seems like that thing we discussed is done? Come by the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress} to collect the ~g~${MoneyToReceive}~s~",
                                $"Word got around that you are done with that thing for us, Come back to the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress} for your payment of ~g~${MoneyToReceive}~s~",
                                $"Get back to the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress} for your payment of ~g~${MoneyToReceive}~s~",
                                $"{HiringGangDen.FullStreetAddress} for ~g~${MoneyToReceive}~s~",
                                $"Heard you were done, see you at the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress}. We owe you ~g~${MoneyToReceive}~s~",
                                };
            Player.CellPhone.AddScheduledText(PhoneContact, Replies.PickRandom(), 1, false);
        }
    }
}
