using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Player.ActiveTasks
{
    public class GangRacketeeringTask : IPlayerTask
    {
        private ITaskAssignable Player;
        private IGangs Gangs;
        private IGangTerritories GangTerritories;
        private IZones Zones;
        private PlayerTasks PlayerTasks;
        private IPlacesOfInterest PlacesOfInterest;
        private ISettingsProvideable Settings;
        private IEntityProvideable World;
        private ICrimes Crimes;
        private IShopMenus ShopMenus;
        private PlayerTask CurrentTask;
        private UIMenuItem collectMoney;

        private Gang HiringGang;
        private GangDen HiringGangDen;
        private GameLocation RacketeeringLocation;
        private PhoneContact PhoneContact;
        private GangTasks GangTasks;
        private int GameTimeToWaitBeforeComplications;
        private int MoneyToRecieve;
        private int MoneyToPickup;
        private bool HasAddedComplications;
        private bool WillAddComplications;
        private bool HasLocations => RacketeeringLocation != null && HiringGangDen != null;

        public GangRacketeeringTask(ITaskAssignable player, IGangs gangs, PlayerTasks playerTasks, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, IEntityProvideable world,
            ICrimes crimes, PhoneContact phoneContact, GangTasks gangTasks, IGangTerritories gangTerritories, IZones zones)
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
            if (RacketeeringLocation != null)
            {
                RacketeeringLocation.IsPlayerInterestedInLocation = false;
            }
        }
        public void Start(Gang ActiveGang)
        {
            HiringGang = ActiveGang;
            if (PlayerTasks.CanStartNewTask(HiringGang?.ContactName))
            {
                GetRacketeeringInformation();
                GetHiringDen();
                if (HasLocations)
                {
                    GetRequiredPayment();
                    SendInitialInstructionsMessage();
                    AddTask();
                    GameFiber PayoffFiber = GameFiber.StartNew(delegate
                    {
                        try
                        {
                            LocationLoop();
                            WatchLoop();
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
        private void LocationLoop()
        {
            while (true)
            {
                if (RacketeeringLocation != null && RacketeeringLocation.InteractionMenu != null)
                {
                    collectMoney = new UIMenuItem("Collect Protection Money", $"Protection isn't optional or cheap.") { RightLabel = $"${MoneyToPickup}" };
                    collectMoney.Activated += (sender, selectedItem) =>
                    {
                        CollectMoney();
                    };
                    RacketeeringLocation.InteractionMenu.AddItem(collectMoney);
                    break;
                }
                GameFiber.Sleep(250);
            }
        }
        private void WatchLoop()
        {
            while (true)
            {
                CurrentTask = PlayerTasks.GetTask(HiringGang.ContactName);
                if (CurrentTask == null || !CurrentTask.IsActive)
                {
                    break;
                }
                if (!RacketeeringLocation.IsPlayerInterestedInLocation)
                {
                    RacketeeringLocation.CheckIsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, 1);
                }
                if (!RacketeeringLocation.IsPlayerInterestedInLocation && !RacketeeringLocation.IsNearby)
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
                if (RacketeeringLocation != null)
                {
                    RacketeeringLocation.IsPlayerInterestedInLocation = false;
                }
                SendMoneyDropOffMessage();
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
        private void CollectMoney()
        {
            Player.BankAccounts.GiveMoney(MoneyToPickup, false);
            collectMoney.Enabled = false;
            RacketeeringLocation.IsPlayerInterestedInLocation = false;
            RacketeeringLocation.PlaySuccessSound();
            RacketeeringLocation.DisplayMessage("~g~Reply", "Take the fucking money and leave already.");
        }
        private void GetRacketeeringInformation()
        {
            if (GangTerritories.GetGangTerritory(HiringGang.ID) != null)
            {
                List<ZoneJurisdiction> totalTerritories = GangTerritories.GetGangTerritory(HiringGang.ID);
                if (totalTerritories != null && totalTerritories.Any())
                {
                    List<GameLocation> PossibleSpots = PlacesOfInterest.PossibleLocations.RobberyTaskLocations().Where(x => x.IsCorrectMap(World.IsMPMapLoaded)).ToList();
                    List<GameLocation> AvailableSpots = new List<GameLocation>();

                    foreach (ZoneJurisdiction zj in totalTerritories)
                    {
                        Zone gangZone = Zones.GetZone(zj.ZoneInternalGameName);
                        if (gangZone != null)
                        {
                            foreach (GameLocation possibleSpot in PossibleSpots)
                            {
                                Zone spotZone = Zones.GetZone(possibleSpot.EntrancePosition);
                                if (spotZone.InternalGameName == zj.ZoneInternalGameName && !possibleSpot.HasInterior)
                                {
                                    bool isNear = PlacesOfInterest.PossibleLocations.PoliceStations.Any(policeStation => possibleSpot.CheckIsNearby(policeStation.CellX, policeStation.CellY, 10));
                                    if (!isNear)
                                    {
                                        AvailableSpots.Add(possibleSpot);
                                    }
                                }
                            }
                        }
                    }

                    RacketeeringLocation = AvailableSpots.PickRandom();
                }
            }
        }
        private void GetHiringDen()
        {
            HiringGangDen = PlacesOfInterest.GetMainDen(HiringGang.ID, World.IsMPMapLoaded);
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
            GameTimeToWaitBeforeComplications = RandomItems.GetRandomNumberInt(3000, 10000);
            HasAddedComplications = false;
            WillAddComplications = false;
            if (RacketeeringLocation != null)
            {
                RacketeeringLocation.IsPlayerInterestedInLocation = true;
            }
            PlayerTasks.AddTask(HiringGang.Contact, MoneyToRecieve, 500, -1 * MoneyToPickup, -1000, 2, "Racketeering for Gang");
            CurrentTask = PlayerTasks.GetTask(HiringGang.ContactName);
            CurrentTask.FailOnStandardRespawn = true;
            HiringGangDen.ExpectedMoney = MoneyToPickup;
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
                                $"Go to {RacketeeringLocation.Name} and make the owner understand our protection isn’t optional. Payment: ${MoneyToPickup}.",
                                $"Visit {RacketeeringLocation.Name} and remind them that payment of ${MoneyToPickup} is due. We don’t take “no” for an answer.",
                                $"Go to {RacketeeringLocation.Name} and collect ${MoneyToPickup}. If they hesitate, make it clear what that means.",
                                $"Stop by {RacketeeringLocation.Name} and let them know they owe us ${MoneyToPickup}. We’re not known for our patience.",
                                $"Head to {RacketeeringLocation.Name} and propose a deal for ${MoneyToPickup}. Make sure they understand it’s non-negotiable.",
                                $"Check {RacketeeringLocation.Name} for debts. They need to pay up: ${MoneyToPickup}. We’re not a charity.",
                                $"Go to {RacketeeringLocation.Name} and remind them of their ${MoneyToPickup}. We don’t tolerate excuses.",
                                $"Swing by {RacketeeringLocation.Name} and let them know we expect ${MoneyToPickup}. We can help them remember if they forget.",
                                $"Visit {RacketeeringLocation.Name} and ensure they realize ${MoneyToPickup} is the least of their worries if they don’t pay.",
                                $"Stop by {RacketeeringLocation.Name} and make it clear that ${MoneyToPickup} is overdue. We don’t let things slide.",
                                $"Head to {RacketeeringLocation.Name} and check if they’re ready to pay ${MoneyToPickup}. They’d be wise to say yes.",
                                $"Go to {RacketeeringLocation.Name} and collect ${MoneyToPickup}. Remind them that we don’t take kindly to delays.",
                                $"Make a stop at {RacketeeringLocation.Name} and confirm ${MoneyToPickup} is settled.",
                                $"Visit {RacketeeringLocation.Name} and remind them that their ${MoneyToPickup} is now our priority.",
                                $"Go to {RacketeeringLocation.Name} and stress that ${MoneyToPickup} is not a request; it’s a requirement.",
                                $"Stop by {RacketeeringLocation.Name} to collect ${MoneyToPickup}. We’ll be waiting, and we don’t like to be kept waiting.",
                                $"Check out {RacketeeringLocation.Name} and ensure they’re ready to hand over ${MoneyToPickup}. No games.",
                                $"Go to {RacketeeringLocation.Name} and confirm that ${MoneyToPickup} is on the table. Or we’ll have to escalate matters.",
                                $"Visit {RacketeeringLocation.Name} and make sure they understand the importance of ${MoneyToPickup}. We don’t play nice.",
                            };



            Player.CellPhone.AddPhoneResponse(HiringGang.Contact.Name, HiringGang.Contact.IconName, Replies.PickRandom());
        }
    }
}
