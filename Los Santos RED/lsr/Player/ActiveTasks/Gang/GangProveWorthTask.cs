using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Player.ActiveTasks
{
    public class GangProveWorthTask
    {
        private ITaskAssignable Player;
        private ITimeReportable Time;
        private IGangs Gangs;
        private PlayerTasks PlayerTasks;
        private IPlacesOfInterest PlacesOfInterest;
        private List<DeadDrop> ActiveDrops = new List<DeadDrop>();
        private ISettingsProvideable Settings;
        private IEntityProvideable World;
        private ICrimes Crimes;
        private Gang HiringGang;
        private GangDen HiringGangDen;
        private Gang TargetGang;
        private GangDen TargetGangDen;
        private PlayerTask CurrentTask;
        private int GameTimeToWaitBeforeComplications;
        private int MoneyToRecieve;
        private bool HasAddedComplications;
        private bool WillAddComplications;
        private int KilledMembersAtStart;
        private GangContact Contact;
        private bool hasExploded = false;
        public int KillRequirement { get; set; } = 1;
        private bool HasTargetGangAndHiringDen => TargetGang != null && HiringGangDen != null && TargetGangDen != null;

        public bool JoinGangOnComplete { get; set; } = false;

        public GangProveWorthTask(ITaskAssignable player, ITimeReportable time, IGangs gangs, PlayerTasks playerTasks, IPlacesOfInterest placesOfInterest, List<DeadDrop> activeDrops, ISettingsProvideable settings, IEntityProvideable world, ICrimes crimes)
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
        }
        public void Setup()
        {
            hasExploded = false;
        }
        public void Dispose()
        {

        }
        public void Start(Gang ActiveGang)
        {
            HiringGang = ActiveGang;
            Contact = new GangContact(HiringGang.ContactName, HiringGang.ContactIcon);
            if (PlayerTasks.CanStartNewTask(ActiveGang?.ContactName))
            {
                GetTargetGang();
                GetHiringDen();
                if (HasTargetGangAndHiringDen)
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
                    SendTaskAbortMessage();
                }
            }
        }
        private void Loop()
        {
            uint GameTimeLastCheckedStatus = 0;
            while (true)
            {
                bool isExplosion = NativeFunction.Natives.IS_EXPLOSION_IN_SPHERE<bool>(-1, TargetGangDen.EntrancePosition.X, TargetGangDen.EntrancePosition.Y, TargetGangDen.EntrancePosition.Z, 50f);
                if(isExplosion)
                {
                    hasExploded = true;
                }
                if (Game.GameTime - GameTimeLastCheckedStatus >= 1000)
                {
                    CurrentTask = PlayerTasks.GetTask(HiringGang.ContactName);
                    if (CurrentTask == null || !CurrentTask.IsActive)
                    {
                        break;
                    }
                    if (Player.RelationshipManager.GangRelationships.GetReputation(TargetGang)?.MembersKilled >= KilledMembersAtStart + KillRequirement && hasExploded)
                    {
                        CurrentTask.OnReadyForPayment(false);
                        break;
                    }
                    GameTimeLastCheckedStatus = Game.GameTime;
                }
                GameFiber.Yield();
            }
        }
        private void FinishTask()
        {
            if (CurrentTask != null && CurrentTask.IsActive && CurrentTask.IsReadyForPayment)
            {
                PlayerTasks.CompleteTask(HiringGang.ContactName, true);
            }
            else
            {
                Dispose();
            }
        }
        private void GetTargetGang()
        {
            TargetGang = null;
            if (HiringGang.EnemyGangs != null && HiringGang.EnemyGangs.Any())
            {
                TargetGang = Gangs.GetGang(HiringGang.EnemyGangs.PickRandom());
            }
            if (TargetGang == null)
            {
                TargetGang = Gangs.GetAllGangs().Where(x => x.ID != HiringGang.ID).PickRandom();
            }
            if(TargetGang != null)
            {
                TargetGangDen = PlacesOfInterest.PossibleLocations.GangDens.FirstOrDefault(x => x.AssociatedGang?.ID == TargetGang?.ID && x.IsPrimaryGangDen);
            }
        }
        private void GetHiringDen()
        {
            HiringGangDen = PlacesOfInterest.GetMainDen(HiringGang.ID, World.IsMPMapLoaded);
        }
        private void GetPayment()
        {
            MoneyToRecieve = 0;
        }
        private void AddTask()
        {
            GameTimeToWaitBeforeComplications = RandomItems.GetRandomNumberInt(3000, 10000);
            HasAddedComplications = false;
            WillAddComplications = false;
            GangReputation gr = Player.RelationshipManager.GangRelationships.GetReputation(TargetGang);
            KilledMembersAtStart = gr.MembersKilled;
            PlayerTasks.AddTask(HiringGang.ContactName, MoneyToRecieve, 2000, 0, -50000, 7, "Rival Gang Hit", JoinGangOnComplete);
        }
        private void SendInitialInstructionsMessage()
        {
            List<string> Replies = new List<string>() {
                $"Need to send a message to {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~. Get to the {TargetGang.DenName} on {TargetGangDen.FullStreetAddress} and send them an explosive surprise. Make sure its right outside the {TargetGang.DenName}. Be sure to also get rid of {KillRequirement} members for good measure.",
                    };
            Player.CellPhone.AddPhoneResponse(HiringGang.ContactName, HiringGang.ContactIcon, Replies.PickRandom());
        }

        private void SendTaskAbortMessage()
        {
            List<string> Replies = new List<string>() {
                    "Nothing yet, I'll let you know",
                    "I've got nothing for you yet",
                    "Give me a few days",
                    "Not a lot to be done right now",
                    "We will let you know when you can do something for us",
                    "Check back later.",
                    };
            Player.CellPhone.AddPhoneResponse(HiringGang.ContactName, Replies.PickRandom());
        }
    }
}
