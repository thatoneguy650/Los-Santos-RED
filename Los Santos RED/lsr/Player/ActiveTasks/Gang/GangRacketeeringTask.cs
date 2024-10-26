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
        private List<GameLocation> RacketeeringLocations = new List<GameLocation>();

        private PhoneContact PhoneContact;
        private GangTasks GangTasks;
        private int GameTimeToWaitBeforeComplications;
        private int MoneyToReceive;
        private int MoneyToPickup;
        private bool HasAddedComplications;
        private bool WillAddComplications;

        private bool HasLocations => RacketeeringLocations != null && RacketeeringLocations.Any() && HiringGangDen != null;

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
            foreach (GameLocation location in RacketeeringLocations)
            {
                if (location != null) { location.ProtectionMoneyDue = 0; location.IsPlayerInterestedInLocation = false; }
            }
            HiringGangDen.ExpectedMoney = 0;
        }
        public void Start(Gang ActiveGang)
        {
            HiringGang = ActiveGang;
            WillAddComplications = RandomItems.RandomPercent(Settings.SettingsManager.TaskSettings.GangRacketeeringComplicationsPercentage);

            if (PlayerTasks.CanStartNewTask(HiringGang?.ContactName))
            {
                GetRacketeeringSpots();
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
                GameLocation currentLocation = RacketeeringLocations.FirstOrDefault(loc => loc.IsPlayerInterestedInLocation);
                bool collectionFinished = RacketeeringLocations.All(loc => loc.ProtectionMoneyDue == 0);

                if (collectionFinished)
                {
                    CurrentTask.OnReadyForPayment(false);
                    break;
                }

                RacketeeringLocations.ToList().ForEach(location =>
                    {
                        if (location.InteractionMenu != null && location.IsPlayerInterestedInLocation)
                        {
                            if (!location.InteractionMenu.MenuItems.Contains(collectMoney))
                            {
                                collectMoney = new UIMenuItem("Collect Protection Money", $"Protection isn't optional or cheap.") { RightLabel = $"${location.ProtectionMoneyDue}" };
                                collectMoney.Activated += (sender, selectedItem) =>
                                {
                                    CollectMoney(location);
                                };
                                location.InteractionMenu.AddItem(collectMoney);
                            }
                        }
                    });

                GameFiber.Sleep(250);
            }
        }
        private void FinishTask()
        {
            if (CurrentTask != null && CurrentTask.IsActive && CurrentTask.IsReadyForPayment)
            {
                foreach (GameLocation location in RacketeeringLocations)
                {
                    if (location != null) { location.IsPlayerInterestedInLocation = false; }
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
        private void CollectMoney(GameLocation location)
        {
            Player.BankAccounts.GiveMoney(location.ProtectionMoneyDue, false);
            collectMoney.Enabled = false;
            location.IsPlayerInterestedInLocation = false;
            location.ProtectionMoneyDue = 0;
            location.PlaySuccessSound();
            location.DisplayMessage("~g~Reply", "Take the fucking money and leave already.");
        }
        private void GetRacketeeringSpots()
        {
            if (GangTerritories.GetGangTerritory(HiringGang.ID) != null)
            {
                List<ZoneJurisdiction> totalTerritories = GangTerritories.GetGangTerritory(HiringGang.ID);
                if (totalTerritories != null && totalTerritories.Any())
                {
                    List<GameLocation> PossibleSpots = PlacesOfInterest.PossibleLocations.RacketeeringTaskLocations().Where(x => x.IsCorrectMap(World.IsMPMapLoaded)).ToList();
                    List<GameLocation> AvailableSpots = new List<GameLocation>();

                    foreach (ZoneJurisdiction zj in totalTerritories)
                    {
                        Zone gangZone = Zones.GetZone(zj.ZoneInternalGameName);
                        if (gangZone != null)
                        {
                            foreach (GameLocation possibleSpot in PossibleSpots)
                            {
                                Zone spotZone = Zones.GetZone(possibleSpot.EntrancePosition);
                                bool isNear = PlacesOfInterest.PossibleLocations.PoliceStations.Any(policeStation => possibleSpot.CheckIsNearby(policeStation.CellX, policeStation.CellY, 10));

                                if (spotZone.InternalGameName == gangZone.InternalGameName && !possibleSpot.HasInterior && !isNear)
                                {
                                    if (WillAddComplications)
                                    {
                                        if (zj.Priority != 0)
                                        {
                                            AvailableSpots.Add(possibleSpot);
                                        }
                                    }
                                    else if (zj.Priority == 0)
                                    {
                                        AvailableSpots.Add(possibleSpot);
                                    }
                                }
                            }
                        }
                    }
                    RacketeeringLocations = AvailableSpots.OrderBy(x => Guid.NewGuid()).Take(new Random().Next(1, AvailableSpots.Count)).ToList();
                }
            }
        }
        private void GetHiringDen()
        {
            HiringGangDen = PlacesOfInterest.GetMainDen(HiringGang.ID, World.IsMPMapLoaded);
        }
        private void GetRequiredPayment()
        {
            float TenPercent = 0;
            foreach (GameLocation location in RacketeeringLocations)
            {
                int PaymentAmount = 0;
                if (location.GetType().ToString().Equals("Bank"))
                {
                    PaymentAmount = RandomItems.GetRandomNumberInt(10000, 20000).Round(100);
                }
                else if (location.GetType().ToString().Equals("Dealership"))
                {
                    PaymentAmount = RandomItems.GetRandomNumberInt(5000, 10000).Round(100);
                }
                else if (location.GetType().ToString().Equals("Hotel"))
                {
                    PaymentAmount = RandomItems.GetRandomNumberInt(2000, 5000).Round(100);
                }
                else 
                { 
                    PaymentAmount = RandomItems.GetRandomNumberInt(500, 1000).Round(50);
                }

                if (Zones.GetZone(location.EntrancePosition).Economy.ToString().Equals("Rich")) { PaymentAmount *= 5; }
                else if (Zones.GetZone(location.EntrancePosition).Economy.ToString().Equals("Middle")) { PaymentAmount *= 2; }

                MoneyToPickup += PaymentAmount;

                TenPercent = (float)PaymentAmount / 10;
                MoneyToReceive += (int)TenPercent;

                location.ProtectionMoneyDue = PaymentAmount;
            }
            MoneyToReceive = MoneyToReceive.Round(100);
        }
        private void AddTask()
        {
            GameTimeToWaitBeforeComplications = RandomItems.GetRandomNumberInt(3000, 10000);
            HasAddedComplications = false;
            foreach (GameLocation location in RacketeeringLocations)
            {
                if (location != null) { location.IsPlayerInterestedInLocation = true; }
            }

            PlayerTasks.AddTask(HiringGang.Contact, MoneyToReceive, 500, -1 * MoneyToPickup, -1000, 2, "Racketeering for Gang");
            CurrentTask = PlayerTasks.GetTask(HiringGang.ContactName);
            CurrentTask.FailOnStandardRespawn = true;
            HiringGangDen.ExpectedMoney = MoneyToPickup;
        }
        private void SendMoneyDropOffMessage()
        {
            List<string> Replies = new List<string>() {
                                $"Take the money to {HiringGangDen.Name}.",
                                $"Now bring me the money, don't get lost. {HiringGangDen.Name}",
                                $"Remeber that is MY MONEY you are just holding it. Drop it off at {HiringGangDen.Name}.",
                                $"Drop the money off at {HiringGangDen.Name}",
                                $"Bring the stuff back to {HiringGangDen.Name}. Don't take long.",  };
            Player.CellPhone.AddScheduledText(PhoneContact, Replies.PickRandom(), 0, true);
        }
        private void SendInitialInstructionsMessage()
        {
            string locationsList = string.Join(", ", RacketeeringLocations.Select(loc => loc.Name));
            List<string> Replies;
            if (WillAddComplications)
            {
                Replies = new List<string>() {
                $"Make your presence felt at {locationsList}. Show them who’s really in charge. ${MoneyToReceive} for you.",
                $"Head over to {locationsList} and remind them that we don't take kindly to trespassers. Your cut: ${MoneyToReceive}.",
                $"Visit {locationsList} and let them know they’re not safe here. Collect ${MoneyToReceive} for your efforts.",
                $"Go to {locationsList} and make sure they understand they’re on our turf now. You’ll earn ${MoneyToReceive} for this.",
                $"Intimidate the owners at {locationsList} and assert our dominance. You'll get ${MoneyToReceive}."
            };
            }
            else
            {
                Replies = new List<string>() {
                $"Go to the following locations: {locationsList} and make the owners understand our protection isn’t optional. ${MoneyToReceive} 10% cut like usual",
                $"Visit {locationsList} and remind them of their overdue payment. You'll get ${MoneyToReceive}",
                $"Make sure to check in at {locationsList} and collect the protection tax. ${MoneyToReceive} for you",
                $"Head to {locationsList} and let them know they owe us. We’re not known for our patience. ${MoneyToReceive} for you",
                $"Stop by {locationsList} and ensure they realize they'll need to pay up. Your cut: ${MoneyToReceive}",
                $"Remind the owners at {locationsList} that they need to settle their debts. You'll get ${MoneyToReceive}"
            };
            }
            Player.CellPhone.AddPhoneResponse(HiringGang.Contact.Name, HiringGang.Contact.IconName, Replies.PickRandom());
        }
    }
}
