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
    public class GangPickupTask : IPlayerTask
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
        private bool HasAddedComplications;
        private bool WillAddComplications;
        private int MoneyToRecieve;
        private int MoneyToPickup;
        private GangDen HiringGangDen;
        private PhoneContact PhoneContact;
        private GangTasks GangTasks;

        private bool HasDeadDropAndDen => DeadDrop != null && HiringGangDen != null;

        public GangPickupTask(ITaskAssignable player, ITimeReportable time, IGangs gangs, PlayerTasks playerTasks, IPlacesOfInterest placesOfInterest, List<DeadDrop> activeDrops, ISettingsProvideable settings, IEntityProvideable world,
            ICrimes crimes, PhoneContact phoneContact, GangTasks gangTasks)
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
        }
        public void Setup()
        {

        }
        public void Dispose()
        {
            if (HiringGang != null)
            {
                HiringGangDen.ExpectedMoney = 0;
            }
            DeadDrop?.Reset();
            DeadDrop?.Deactivate(true);
        }
        public void Start(Gang ActiveGang)
        {
            HiringGang = ActiveGang;
            if (PlayerTasks.CanStartNewTask(HiringGang?.ContactName))
            {
                GetDeadDrop();
                GetHiringDen();
                if (HasDeadDropAndDen)
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
                DeadDrop?.Reset();
                DeadDrop?.Deactivate(true);
                SendMoneyDropOffMessage();
            }
            else if (CurrentTask != null && !CurrentTask.IsActive)
            {
                Dispose();
            }
            else
            {
                Dispose();//the failing messages are handled above if they cancel
            }
        }
        private void GetDeadDrop()
        {
            DeadDrop = PlacesOfInterest.GetUsableDeadDrop(World.IsMPMapLoaded, Player.CurrentLocation);
        }
        private void GetHiringDen()
        {
            HiringGangDen = PlacesOfInterest.GetMainDen(HiringGang.ID, World.IsMPMapLoaded, Player.CurrentLocation);
        }
        private void GetRequiredPayment()
        {
            int PaymentAmount = RandomItems.GetRandomNumberInt(HiringGang.PickupPaymentMin, HiringGang.PickupPaymentMax).Round(100);
            MoneyToPickup = PaymentAmount * 10;
            float TenPercent = (float)MoneyToPickup / 10;
            MoneyToRecieve = (int)TenPercent;
            if (MoneyToRecieve <= 0)
            {
                MoneyToRecieve = 500;
            }
            MoneyToRecieve = MoneyToRecieve.Round(10);
        }
        private void AddTask()
        {
            PlayerTasks.AddTask(HiringGang.Contact, MoneyToRecieve, 500, -1 * MoneyToPickup, -1000, 2, "Pickup for Gang");
            DeadDrop.SetupDrop(MoneyToPickup, false);
            ActiveDrops.Add(DeadDrop);
            HiringGangDen.ExpectedMoney = MoneyToPickup;
            GameTimeToWaitBeforeComplications = RandomItems.GetRandomNumberInt(3000, 10000);
            HasAddedComplications = false;
            WillAddComplications = false;// RandomItems.RandomPercent(Settings.SettingsManager.TaskSettings.RivalGangHitComplicationsPercentage);
        }
        private void SendMoneyDropOffMessage()
        {
            List<string> Replies = new List<string>() {
                                "Take the money to the designated place.",
                                "Now bring me the money, don't get lost",
                                "Remeber that is MY MONEY you are just holding it. Drop it off where we agreed.",
                                "Drop the money off at the designated place",
                                "Take the money where it needs to go",
                                "Bring the stuff back to us. Don't take long.",  };
            Player.CellPhone.AddScheduledText(PhoneContact, Replies.PickRandom(), 0, true);
        }
        private void SendInitialInstructionsMessage()
        {
            List<string> Replies = new List<string>() {
                    $"Pickup ${MoneyToPickup} from {DeadDrop.FullStreetAddress}, its {DeadDrop.Description}. Bring it to the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress}. You get 10% on completion",
                    $"Go get ${MoneyToPickup} from {DeadDrop.Description}, address is {DeadDrop.FullStreetAddress}. Bring it to the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress}. 10% to you when you drop it off",
                    $"Make a pickup of ${MoneyToPickup} from {DeadDrop.Description} on {DeadDrop.FullStreetAddress}. Take it to the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress}. You'll get 10% when I get my money.",
                    };
            Player.CellPhone.AddPhoneResponse(HiringGang.Contact.Name, HiringGang.Contact.IconName, Replies.PickRandom());
        }
    }
}
