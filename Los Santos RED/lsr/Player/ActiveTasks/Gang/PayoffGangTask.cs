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
    public class PayoffGangTask
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
        private DeadDrop DeadDrop;
        private PlayerTask CurrentTask;
        private int GameTimeToWaitBeforeComplications;
        private GangReputation HiringGangReputation;
        private int CostToPayoff;
        private int RepToNextLevel;
        private bool HasAddedComplications;
        private bool WillAddComplications;
        private GangContact Contact;

        private bool HasDeadDrop => DeadDrop != null;

        public PayoffGangTask(ITaskAssignable player, ITimeReportable time, IGangs gangs, PlayerTasks playerTasks, IPlacesOfInterest placesOfInterest, List<DeadDrop> activeDrops, ISettingsProvideable settings, IEntityProvideable world, ICrimes crimes)
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
        }
        public void Dispose()
        {
            if(DeadDrop != null)
            {
                DeadDrop.Deactivate(true);
            }
        }
        public void Start(Gang ActiveGang)
        {
            HiringGang = ActiveGang;
            Contact = new GangContact(HiringGang.ContactName, HiringGang.ContactIcon);
            if (PlayerTasks.CanStartNewTask(HiringGang?.ContactName))
            {
                GetDeadDrop();
                if (HasDeadDrop)
                {
                    GetRequiredPayment();
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
            while (true)
            {
                CurrentTask = PlayerTasks.GetTask(HiringGang.ContactName);
                if (CurrentTask == null || !CurrentTask.IsActive)
                {
                    //EntryPoint.WriteToConsoleTestLong($"Task Inactive for {HiringGang.ContactName}");
                    break;
                }
                if (DeadDrop.InteractionComplete)
                {
                    DeadDrop.CheckIsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, 5);
                }
                if (DeadDrop.InteractionComplete && !DeadDrop.IsNearby)
                {
                    CurrentTask.OnReadyForPayment(false);
                    break;           
                }
                GameFiber.Sleep(1000);
            }
        }
        private void FinishTask()
        {
            if (CurrentTask != null && CurrentTask.IsActive && CurrentTask.IsReadyForPayment)
            {
                SetComplete();
            }
            else
            {
                Dispose();
            }
        }
        private void SetComplete()
        {
            SendCompletedMessage();
            PlayerTasks.CompleteTask(HiringGang.ContactName,false);         
        }
        private void GetDeadDrop()
        {
            DeadDrop = PlacesOfInterest.GetUsableDeadDrop(World.IsMPMapLoaded);
        }
        private void GetRequiredPayment()
        {
            HiringGangReputation = Player.RelationshipManager.GangRelationships.GetReputation(HiringGang);
            if(HiringGangReputation != null)
            {
                if(HiringGangReputation.PlayerDebt > 0)
                {
                    CostToPayoff = HiringGangReputation.PlayerDebt;
                    RepToNextLevel = 0;
                }
                else
                {
                    CostToPayoff = HiringGangReputation.CostToPayoff;
                    RepToNextLevel = HiringGangReputation.RepToNextLevel;
                }
            }
            else
            {
                CostToPayoff = 0;
                RepToNextLevel = 0;
            }
        }
        private void AddTask()
        {
            PlayerTasks.AddTask(HiringGang.ContactName, 0, RepToNextLevel, 0, -200, 2, "Dead Drop");
            DeadDrop.SetupDrop(CostToPayoff, true);
            ActiveDrops.Add(DeadDrop);
            GameTimeToWaitBeforeComplications = RandomItems.GetRandomNumberInt(3000, 10000);
            HasAddedComplications = false;
            WillAddComplications = false;// RandomItems.RandomPercent(Settings.SettingsManager.TaskSettings.RivalGangHitComplicationsPercentage);
        }
        private void SendCompletedMessage()
        {
            List<string> Replies;
            if(HiringGangReputation.PlayerDebt > 0)
            {
                Replies = new List<string>() {
                        "I guess we are even now",
                        "Consider your debt paid",
                        "Debt wiped",
                        "You can stop looking over your shoulder now",
                        "We are square",
                        };
            }
            else if (Player.RelationshipManager.GangRelationships.IsHostile(HiringGang))// CurrentTask.RepAmountOnCompletion <= 0)
            {
                Replies = new List<string>() {
                                "I guess we can forget about that shit.",
                                "No problem man, all is forgiven",
                                "That shit before? Forget about it.",
                                "We are square",
                                "You are off the hit list",
                                "This doesn't make us friends prick, just associates",
                                };
            }
            else
            {
                Replies = new List<string>() {
                                "Nice to get some respect from you finally, give us a call soon",
                                "Well this certainly smooths things over, come by to discuss things",
                                "I always liked you",
                                "Thanks for that, I'll remember it",
                                "Ah you got me my favorite thing! I owe you a thing or two",
                                };
            }
            Player.CellPhone.AddScheduledText(Contact, Replies.PickRandom(), 0);
        }
        private void SendInitialInstructionsMessage()
        {
            List<string> Replies = new List<string>() {
                $"Drop ${CostToPayoff} on {DeadDrop.FullStreetAddress}, its {DeadDrop.Description}. My guy won't pick it up if you are around.",
                $"Place ${CostToPayoff} in {DeadDrop.Description}, address is {DeadDrop.FullStreetAddress}. Don't hang around either, drop it off and leave.",
                $"Drop off ${CostToPayoff} to {DeadDrop.Description} on {DeadDrop.FullStreetAddress}. Once you drop the cash off, get out of the area.",
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
