using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using NAudio.Wave;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Player.ActiveTasks
{
    public class GangDeliveryTask : IPlayerTask
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
        private IModItems ModItems;
        private IShopMenus ShopMenus;
        private Gang HiringGang;
        private PlayerTask CurrentTask;
        private int GameTimeToWaitBeforeComplications;
        private bool HasAddedComplications;
        private bool WillAddComplications;
        private int MoneyToRecieve;
        private GangDen HiringGangDen;
        private ModItem ItemToDeliver;
        private int NumberOfItemsToDeliver;
        private PhoneContact PhoneContact;
        private GangTasks GangTasks;
        private string ModItemNameToDeliver;
        private bool HasDen => HiringGangDen != null;

        public GangDeliveryTask(ITaskAssignable player, ITimeReportable time, IGangs gangs, PlayerTasks playerTasks, IPlacesOfInterest placesOfInterest, List<DeadDrop> activeDrops, ISettingsProvideable settings, IEntityProvideable world, ICrimes crimes, IModItems modItems, 
            IShopMenus shopMenus, PhoneContact phoneContact, GangTasks gangTasks, string modItemNameToDeliver)
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
            ModItems = modItems;
            ShopMenus = shopMenus;
            PhoneContact = phoneContact;
            GangTasks = gangTasks;
            ModItemNameToDeliver = modItemNameToDeliver;
        }
        public void Setup()
        {

        }
        public void Dispose()
        {
            if (HiringGangDen != null)
            {
                HiringGangDen.ExpectedItem = null;
                HiringGangDen.ExpectedItemAmount = 0;
            }
        }
        public void Start(Gang ActiveGang)
        {
            HiringGang = ActiveGang;
            if (PlayerTasks.CanStartNewTask(HiringGang?.ContactName))
            {
                GetHiringDen();
                if (HasDen)
                {
                    GetRequiredPayment();
                    if (ItemToDeliver != null)
                    {
                        SendInitialInstructionsMessage();
                        AddTask();
                        WatchTaskLoop();
                    }
                    else
                    {
                        Game.DisplayHelp("Could not find item");
                    }
                }
                else
                {
                    GangTasks.SendGenericTooSoonMessage(PhoneContact);
                }
            }
        }
        private void WatchTaskLoop()
        {
            GameFiber PayoffFiber = GameFiber.StartNew(delegate
            {
                try
                {
                    while (true)
                    {
                        if (CurrentTask == null || !CurrentTask.IsActive)
                        {
                            break;
                        }
                        GameFiber.Sleep(1000);
                    }
                    Dispose();
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "PayoffFiber");
        }
        private void GetHiringDen()
        {
            HiringGangDen = PlacesOfInterest.GetMainDen(HiringGang.ID,World.IsMPMapLoaded, Player.CurrentLocation);
        }
        private void GetRequiredPayment()
        {
            int PaymentAmount = RandomItems.GetRandomNumberInt(HiringGang.DeliveryPaymentMin, HiringGang.DeliveryPaymentMax).Round(100);
            ItemToDeliver = null;
            if(HiringGangDen.Menu.Items.Any(x => x.Purchaseable && x.ModItemName == ModItemNameToDeliver))
            {
                return;
            }
            if (ModItemNameToDeliver != "")
            {
                ItemToDeliver = ModItems.Get(ModItemNameToDeliver);
            }
            if(ItemToDeliver != null)
            {
                Tuple<int, int> Prices = ShopMenus.GetPrices(ItemToDeliver.Name);
                float MoreThaMax = (float)Prices.Item2 * 1.5f;
                NumberOfItemsToDeliver = (int)(PaymentAmount / MoreThaMax);
                MoneyToRecieve = PaymentAmount;
                //EntryPoint.WriteToConsoleTestLong($"GANG DELIVERY Item: {ItemToDeliver.Name} Number: {NumberOfItemsToDeliver} Lowest: {Prices.Item1}  Highest {Prices.Item2} Payment: {MoneyToRecieve}");
            }
            if (MoneyToRecieve <= 0)
            {
                MoneyToRecieve = 500;
            }
            MoneyToRecieve = MoneyToRecieve.Round(10);
        }
        private void AddTask()
        {
            PlayerTasks.AddTask(HiringGang.Contact, MoneyToRecieve, 500, 0, -1000, 4, "Pickup for Gang");
            HiringGangDen.ExpectedItem = ItemToDeliver;
            HiringGangDen.ExpectedItemAmount = NumberOfItemsToDeliver;

            CurrentTask = PlayerTasks.GetTask(HiringGang.ContactName);
            CurrentTask.OnReadyForPayment(false);


            GameTimeToWaitBeforeComplications = RandomItems.GetRandomNumberInt(3000, 10000);
            HasAddedComplications = false;
            WillAddComplications = false;// RandomItems.RandomPercent(Settings.SettingsManager.TaskSettings.RivalGangHitComplicationsPercentage);
        }
        private void SendInitialInstructionsMessage()
        {
            List<string> Replies = new List<string>() {
                    $"I want you to find us {NumberOfItemsToDeliver} {ItemToDeliver.MeasurementName}(s) of {ItemToDeliver.Name}. We need it quick. Bring it to the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress}. ${MoneyToRecieve}",
                    $"Go get {NumberOfItemsToDeliver} {ItemToDeliver.MeasurementName}(s) of {ItemToDeliver.Name} from somewhere, I don't wanna know. Don't take too long. Bring it to the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress}. ${MoneyToRecieve} to you when you drop it off",
                    $"We need you to find {NumberOfItemsToDeliver} {ItemToDeliver.MeasurementName}(s) of {ItemToDeliver.Name}. We can't wait all week. Take it to the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress}. You'll get ${MoneyToRecieve} when I get my item.",
                    };
            Player.CellPhone.AddPhoneResponse(HiringGang.Contact.Name, HiringGang.Contact.IconName, Replies.PickRandom());
        }
    }
}
