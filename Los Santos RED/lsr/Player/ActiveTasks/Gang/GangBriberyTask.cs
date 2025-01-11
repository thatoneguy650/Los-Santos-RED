using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI.Elements;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LosSantosRED.lsr.Player.ActiveTasks
{
    public class GangBriberyTask : IPlayerTask
    {
        private ITaskAssignable Player;
        private IGangs Gangs;
        private PlayerTasks PlayerTasks;
        private IPlacesOfInterest PlacesOfInterest;
        private ISettingsProvideable Settings;
        private IEntityProvideable World;
        private ICrimes Crimes;
        private Gang HiringGang;
        private GangDen HiringGangDen;
        private PlayerTask CurrentTask;
        private int GameTimeToWaitBeforeComplications;
        private int MoneyToRecieve;
        private int HushMoney;
        private bool HasAddedComplications;
        private bool WillAddComplications;
        private int KilledMembersAtStart;
        private PhoneContact PhoneContact;
        private GangTasks GangTasks;
        private IGangTerritories GangTerritories;
        private IZones Zones;
        private UIMenuItem depositMoney;
        private Zone SelectedZone;
        private GameLocation DepositLocation;
        public int KillRequirement { get; set; } = 1;
        private bool HasConditions => DepositLocation != null && HiringGangDen != null && SelectedZone != null;

        public PlayerTask PlayerTask => CurrentTask;

        public GangBriberyTask(ITaskAssignable player, IGangs gangs, PlayerTasks playerTasks, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, IEntityProvideable world, ICrimes crimes
            , PhoneContact phoneContact, GangTasks gangTasks, IGangTerritories gangTerritories, IZones zones)
        {
            Player = player;
            Gangs = gangs;
            PlayerTasks = playerTasks;
            PlacesOfInterest = placesOfInterest;
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

        }
        public void Dispose()
        {
            if (DepositLocation != null) { DepositLocation.IsPlayerInterestedInLocation = false; }
            if (HiringGangDen != null) { HiringGangDen.PickupMoney = 0; }
        }
        public void Start(Gang ActiveGang)
        {
            HiringGang = ActiveGang;
            if (PlayerTasks.CanStartNewTask(ActiveGang?.ContactName))
            {
                GetTargetBank();
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
                            PickupLoop();
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
        private void PickupLoop()
        {
            while (true)
            {
                CurrentTask = PlayerTasks.GetTask(HiringGang.ContactName);
                if (CurrentTask == null || !CurrentTask.IsActive)
                {
                    //EntryPoint.WriteToConsoleTestLong($"Task Inactive for {HiringGang.ContactName}");
                    break;
                }
                if (HiringGangDen.PickupMoney == 0)
                {
                    SendDropOffMessage();
                    DepositLocation.IsPlayerInterestedInLocation = true;
                    break;
                }
                GameFiber.Sleep(1000);
            }
        }
        private void Loop()
        {
            while (true)
            {
                CurrentTask = PlayerTasks.GetTask(HiringGang.ContactName);
                if (CurrentTask == null || !CurrentTask.IsActive || HushMoney == 0)
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
                if (DepositLocation != null) { DepositLocation.IsPlayerInterestedInLocation = false; }
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
        private void DepositMoney(GameLocation location)
        {
            if (Player.BankAccounts.GetMoney(false) >= HushMoney)
            {
                Player.BankAccounts.GiveMoney(-1 * HushMoney, false);
                HushMoney = 0;
                depositMoney.Enabled = false;
                location.IsPlayerInterestedInLocation = false;
                location.PlaySuccessSound();
                location.DisplayMessage("~g~Reply", "Deposit Successful.");
            }
            else
            {
                location.PlayErrorSound();
                location.DisplayMessage("~r~Reply", "Come back when you actually have the cash.");
            }
        }
        private void GetTargetBank()
        {
            DepositLocation = PlacesOfInterest.PossibleLocations.Banks.Where(x=>x.IsSameState(Player.CurrentLocation?.CurrentZone?.GameState)).PickRandom();     
            if(DepositLocation  == null)
            {
                return;
            }
            SelectedZone = Zones.GetZone(DepositLocation.EntrancePosition);
        }
        private void GetHiringDen()
        {
            HiringGangDen = PlacesOfInterest.GetMainDen(HiringGang.ID, World.IsMPMapLoaded, Player.CurrentLocation);
        }
        private void GetPayment()
        {
            MoneyToRecieve = RandomItems.GetRandomNumberInt(HiringGang.BriberyPaymentMin, HiringGang.BriberyPaymentMax).Round(500);
            if (MoneyToRecieve <= 0)
            {
                MoneyToRecieve = 1000;
            }
            HushMoney = MoneyToRecieve * 5;
            HiringGangDen.PickupMoney = HushMoney;
        }
        private void AddTask()
        {
            PlayerTasks.AddTask(HiringGang.Contact, MoneyToRecieve, 2000, HiringGangDen.PickupMoney * -5, -2500, 7, "Gang Bribery");
            CurrentTask = PlayerTasks.GetTask(HiringGang.ContactName);
            CurrentTask.FailOnStandardRespawn = true;
        }
        private void SendInitialInstructionsMessage()
        {
            List<string> Replies = new List<string>() {
            $"Pick up ~g~{HushMoney:C0}~s~ at {HiringGang.DenName} at {HiringGangDen.FullStreetAddress}. Need you to make a quick deposit.",
            $"Grab ~g~{HushMoney:C0}~s~ from {HiringGang.DenName} at {HiringGangDen.FullStreetAddress}. After that, make sure to drop it off.",
            $"Go to {HiringGang.DenName} at {HiringGangDen.FullStreetAddress} and collect ~g~{HushMoney:C0}~s~. Then, make the deposit ASAP.",
            $"Head to {HiringGang.DenName} at {HiringGangDen.FullStreetAddress} to pick up ~g~{HushMoney:C0}~s~. Quick deposit needed afterward.",
            $"Collect ~g~{HushMoney:C0}~s~ from {HiringGang.DenName}, {HiringGangDen.FullStreetAddress}. Make the deposit right after.",
            $"Stop by {HiringGang.DenName} at {HiringGangDen.FullStreetAddress} to grab ~g~{HushMoney:C0}~s~ and drop it off at the bank when you're done.",
            $"Head over to {HiringGang.DenName}, {HiringGangDen.FullStreetAddress}, and pick up ~g~{HushMoney:C0}~s~. Need it deposited right after.",
            $"Pick up ~g~{HushMoney:C0}~s~ at {HiringGang.DenName}, {HiringGangDen.FullStreetAddress}. Then, take care of the deposit." 
            };

            Player.CellPhone.AddPhoneResponse(HiringGang.Contact.Name, HiringGang.Contact.IconName, Replies.PickRandom());
        }
        private void SendDropOffMessage()
        {
            List<string> Replies = new List<string>() {
            $"The {SelectedZone.AssignedLEAgency.ColorPrefix}{SelectedZone.AssignedLEAgency.ShortName}~s~ chief’s been getting a little too curious. A quiet deposit at {DepositLocation.Name} in {SelectedZone.DisplayName} should keep him quiet. Leave ~g~{HushMoney:C0}~s~ there.",
            $"Word’s spreading that the {SelectedZone.DisplayName} chief’s getting too nosy. Make a discreet deposit at {DepositLocation.Name} with ~g~{HushMoney:C0}~s~ to shut him up.",
            $"The chief in {SelectedZone.DisplayName} is starting to raise eyebrows. Smooth things over with a small gesture at {DepositLocation.Name}. Leave ~g~{HushMoney:C0}~s~ and walk away clean.",
            $"The {SelectedZone.AssignedLEAgency.ColorPrefix}{SelectedZone.AssignedLEAgency.ShortName}~s~ chief’s poking around. Make sure he’s properly compensated. Head to {DepositLocation.Name} in {SelectedZone.DisplayName} and drop off ~g~{HushMoney:C0}~s~.",
            $"The chief's been asking questions in {SelectedZone.DisplayName}. A little cash at {DepositLocation.Name} should keep him distracted. Leave ~g~{HushMoney:C0}~s~.",
            $"That chief in {SelectedZone.DisplayName} is digging too deep. Drop off ~g~{HushMoney:C0}~s~ at {DepositLocation.Name} and keep things quiet.",
            $"The {SelectedZone.AssignedLEAgency.ColorPrefix}{SelectedZone.AssignedLEAgency.ShortName}~s~ chief’s starting to get suspicious. A quiet deposit at {DepositLocation.Name} with ~g~{HushMoney:C0}~s~ should calm him down.",
            $"In {SelectedZone.DisplayName}, the chief’s been sniffing around. Head to {DepositLocation.Name} and leave ~g~{HushMoney:C0}~s~ to keep him from asking any more questions.",
            $"Too much noise from the {SelectedZone.AssignedLEAgency.ColorPrefix}{SelectedZone.AssignedLEAgency.ShortName}~s~ chief. Make a discreet deposit at {DepositLocation.Name} in {SelectedZone.DisplayName}, leave ~g~{HushMoney:C0}~s~ and everything will be fine.",
            $"The chief in {SelectedZone.DisplayName} is causing trouble. A quick stop at {DepositLocation.Name} with ~g~{HushMoney:C0}~s~ will smooth things over.",
            };
            Player.CellPhone.AddPhoneResponse(HiringGang.Contact.Name, HiringGang.Contact.IconName, Replies.PickRandom());
        }
        private void SendMoneyPickupMessage()
        {
            List<string> Replies = new List<string>() {
                                $"Seems like that thing we discussed is done? Come by the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress} to collect the ${MoneyToRecieve}",
                                $"Word got around that you are done with that thing for us, Come back to the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress} for your payment of ${MoneyToRecieve}",
                                $"Get back to the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress} for your payment of ${MoneyToRecieve}",
                                $"{HiringGangDen.FullStreetAddress} for ${MoneyToRecieve}",
                                $"Heard you were done, see you at the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress}. We owe you ${MoneyToRecieve}",
                                };
            Player.CellPhone.AddScheduledText(PhoneContact, Replies.PickRandom(), 1, false);
        }
        public void OnInteractionMenuCreated(GameLocation gameLocation, MenuPool menuPool, UIMenu interactionMenu)
        {
            EntryPoint.WriteToConsole("Gang Bribery Task OnTransactionMenuCreated Start");
            if (interactionMenu == null)
            {
                return;
            }
            if (gameLocation == null || !gameLocation.IsPlayerInterestedInLocation)
            {
                return;
            }
            if (interactionMenu.MenuItems.Contains(depositMoney))
            {
                return;
            }
            depositMoney = new UIMenuItem("Deposit Hush Money", $"Make a quiet deposit to keep things under the radar.") { RightLabel = $"${HushMoney}" };
            depositMoney.Activated += (sender, selectedItem) =>
            {
                DepositMoney(gameLocation);
            };
            interactionMenu.AddItem(depositMoney);
            EntryPoint.WriteToConsole("Gang Bribery Task OnTransactionMenuCreated CREATED");
        }
    }
}
