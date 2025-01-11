using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        private Gang EnemyGang;
        private Zone SelectedZone;
        private List<GameLocation> RacketeeringLocations = new List<GameLocation>();

        private PhoneContact PhoneContact;
        private GangTasks GangTasks;
        private int MoneyToReceive;
        private int MoneyToPickup;
        private bool WillExtortEnemyTurf;
        private bool ExtortionComplications;
        private bool RegularComplications;

        private bool HasLocations => RacketeeringLocations != null && RacketeeringLocations.Any() && HiringGangDen != null;
        public bool IsInterestingInLocations { get; private set; } = false;
        public PlayerTask PlayerTask => CurrentTask;

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
            if (HiringGangDen != null) { HiringGangDen.ExpectedMoney = 0; }
        }
        public void Start(Gang ActiveGang)
        {
            HiringGang = ActiveGang;
            WillExtortEnemyTurf = RandomItems.RandomPercent(Settings.SettingsManager.TaskSettings.GangRacketeeringExtortionPercentage);

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
                bool collectionFinished = RacketeeringLocations.All(loc => loc.ProtectionMoneyDue == 0);

                if (collectionFinished)
                {
                    CurrentTask.OnReadyForPayment(false);
                    break;
                }

                //RacketeeringLocations.ForEach(location =>
                //    {
                //       // if(location.SpawnedVendors.Any(x=> x.HasMenu))



                //        if (location.InteractionMenu != null && location.IsPlayerInterestedInLocation)
                //        {
                //            if (!location.InteractionMenu.MenuItems.Contains(collectMoney))
                //            {
                //                collectMoney = new UIMenuItem("Collect Protection Money", $"Protection isn't optional or cheap.") { RightLabel = $"${location.ProtectionMoneyDue}" };
                //                collectMoney.Activated += (sender, selectedItem) =>
                //                {
                //                    CollectMoney(location);
                //                };
                //                location.InteractionMenu.AddItem(collectMoney);
                //            }
                //        }
                //    });

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

            List<string> Replies = new List<string>() {
                                $"Here’s the money, just take it and leave me alone.",
                                $"Here’s what you asked for. Hope this keeps things smooth between us.",
                                $"Here’s the payout. Let’s call it even, and no one gets hurt.",
                                $"Please, take the money and go. I can’t afford to mess with you.",
                                $"Here’s everything I’ve made today—just keep ~p~{SelectedZone.DisplayName}~s~ peaceful",
                                $"Just take it, and we can all go about our business.",
                                $"I’m not looking to be your enemy. Here’s what you asked for.",
                                $"Here’s what I owe you. I’d prefer we avoid any further problems.",
                                $"I’ve paid my dues. No need for any more violence, alright?",
                                };
            ExtortionComplications = RandomItems.RandomPercent(Settings.SettingsManager.TaskSettings.GangRacketeeringExtortionComplicationsPercentage);
            RegularComplications = RandomItems.RandomPercent(Settings.SettingsManager.TaskSettings.GangRacketeeringComplicationsPercentage);
            if (WillExtortEnemyTurf)
            {
                if (ExtortionComplications)
                {
                    Replies = new List<string>() {
                                $"The cash is yours. But don’t think you’re the only one in this game.",
                                $"Take the money. But is {HiringGang.ColorPrefix}{HiringGang.ShortName}~s~ really sending just you to handle this?",
                                $"You’re alone for this? I thought {HiringGang.ColorPrefix}{HiringGang.ShortName}~s~ had better muscle than that.",
                                $"What a joke.",
                                $"Here’s what you came for. But I gotta ask—where’s the rest of {HiringGang.ColorPrefix}{HiringGang.ShortName}~s~?",                                $"The cash is yours. But don’t think you’re the only one in this game.",
                                $"Here’s your money. Don’t push your luck, {EnemyGang.ColorPrefix}{SelectedZone.DisplayName}~s~ isn’t yours.",
                                $"You think you can take over {EnemyGang.ColorPrefix}{SelectedZone.DisplayName}~s~? Think again.",
                                };
                    Player.Dispatcher.GangDispatcher.DispatchHitSquad(SelectedZone.AssignedGang);
                    EntryPoint.WriteToConsole($"{location.Name}:{ExtortionComplications}");
                }
            }
            else if (RegularComplications)
            {
                EntryPoint.WriteToConsole($"{location.Name}:{RegularComplications}");
                Replies = new List<string>() {
                                $"Here’s the money, my friend. But please, don't rush off. There’s always time for a small conversation between us, yes?",
                                $"The payment is yours. Before you leave, do you want to buy anything?",
                                $"The money is yours. But stay a while longer, my friend.",
                                $"I’ve given you the money. Just a moment more, if you please.",
                                $"Take the cash. But before you go, I have some information about {HiringGang.EnemyGangs.PickRandom()} you'd like to hear.",
                                };
                if (Player.IsNotWanted) { Player.AddCrime(Crimes.CrimeList?.FirstOrDefault(x => x.ID == "SuspiciousActivity"), false, Player.Character.Position, Player.CurrentVehicle, Player.WeaponEquipment.CurrentWeapon, true, true, true); }
            }
            location.DisplayMessage("~g~Reply", Replies.PickRandom());
        }
        private void GetRacketeeringSpots()
        {
            if(GangTerritories.GetGangTerritory(HiringGang.ID) == null)
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
            if (WillExtortEnemyTurf)
            {
                //availableTerritories = hiringGangTerritories.Where(zj => zj.Priority != 0).ToList();
                if(HiringGang.EnemyGangs != null && HiringGang.EnemyGangs.Any())
                {
                    EnemyGang = Gangs.GetGang(HiringGang.EnemyGangs.PickRandom());
                }
                if(EnemyGang == null)
                {
                    EnemyGang = Gangs.GetAllGangs().Where(x=> x.ID.ToLower() != HiringGang.ID.ToLower()).PickRandom();
                }
                if(EnemyGang != null)
                {
                    availableTerritories = GangTerritories.GetGangTerritory(EnemyGang.ID).ToList();
                }
            }
            else
            {
                availableTerritories = hiringGangTerritories.ToList();
            }
            if (!availableTerritories.Any() && WillExtortEnemyTurf)//fallback to friendly turf 
            {
                availableTerritories = hiringGangTerritories.ToList();
                WillExtortEnemyTurf = false;
            }
            if (!availableTerritories.Any())
            {
                return;
            }
            ZoneJurisdiction selectedTerritory = availableTerritories.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
            if (WillExtortEnemyTurf && EnemyGang == null)
            {
                EnemyGang = Zones.GetZone(availableTerritories.FirstOrDefault().ZoneInternalGameName).AssignedGang;
            }
            if (Zones.GetZone(selectedTerritory.ZoneInternalGameName) != null)
            {
                SelectedZone = Zones.GetZone(selectedTerritory.ZoneInternalGameName);
                foreach (GameLocation possibleSpot in PossibleSpots)
                {
                    Zone spotZone = Zones.GetZone(possibleSpot.EntrancePosition);
                    bool isNear = PlacesOfInterest.PossibleLocations.PoliceStations.Any(policeStation => policeStation.IsSameState(Player.CurrentLocation) && possibleSpot.EntrancePosition.DistanceTo2D(policeStation.EntrancePosition) < 100f);
                    if (spotZone.InternalGameName == SelectedZone.InternalGameName && !isNear)// && !possibleSpot.HasVendor)
                    {
                        AvailableSpots.Add(possibleSpot);
                    }
                }
            }
            RacketeeringLocations = AvailableSpots.OrderBy(x => Guid.NewGuid()).Take(RandomItems.GetRandomNumberInt(1, 5)).ToList();//new Random().Next(1, AvailableSpots.Count + 1)).ToList();
        }
        private void GetHiringDen()
        {
            HiringGangDen = PlacesOfInterest.GetMainDen(HiringGang.ID, World.IsMPMapLoaded, Player.CurrentLocation);
        }
        private void GetRequiredPayment()
        {
            float PercentCut = 0;
            foreach (GameLocation location in RacketeeringLocations)
            {
                int PaymentAmount = 0;
                PaymentAmount = location.GetRacketeeringPaymentAmount();

                if (PaymentAmount > 1000)
                {
                    PaymentAmount = PaymentAmount.Round(100);
                }
                else
                {
                    PaymentAmount = PaymentAmount.Round(50);
                }



                //if (location.GetType().ToString().Equals("Bank"))
                //{
                //    PaymentAmount = RandomItems.GetRandomNumberInt(10000, 20000).Round(100);
                //}
                //else if (location.GetType().ToString().Equals("Dealership"))
                //{
                //    PaymentAmount = RandomItems.GetRandomNumberInt(5000, 10000).Round(100);
                //}
                //else if (location.GetType().ToString().Equals("Hotel"))
                //{
                //    PaymentAmount = RandomItems.GetRandomNumberInt(2000, 5000).Round(100);
                //}
                //else 
                //{ 
                //    PaymentAmount = RandomItems.GetRandomNumberInt(500, 1000).Round(50);
                //}

                if (Zones.GetZone(location.EntrancePosition).Economy == eLocationEconomy.Rich)// .ToString().Equals("Rich")) 
                { 
                    PaymentAmount *= 5; 
                }
                else if (Zones.GetZone(location.EntrancePosition).Economy == eLocationEconomy.Middle)//  .ToString().Equals("Middle")) 
                { 
                    PaymentAmount *= 2; 
                }

                MoneyToPickup += PaymentAmount;
                location.ProtectionMoneyDue = PaymentAmount;
            }
            if (WillExtortEnemyTurf)
            {
                PercentCut = (float)MoneyToPickup / 4;
            }
            else
            {
                PercentCut = (float)MoneyToPickup / 10;
            }
            MoneyToReceive = (int)PercentCut;
            MoneyToReceive = MoneyToReceive.Round(100);
        }
        private void AddTask()
        {
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
                                $"Take the money to {HiringGang.DenName} on {HiringGangDen.FullStreetAddress}",
                                $"Now bring me the money, don't get lost. {HiringGang.DenName} on {HiringGangDen.FullStreetAddress}",
                                $"Remember that's MY MONEY you're holding. Drop it off at {HiringGang.DenName} on {HiringGangDen.FullStreetAddress}",
                                $"Drop the money off at {HiringGang.DenName} on {HiringGangDen.FullStreetAddress}",
                                $"Bring the stuff back to {HiringGang.DenName} on {HiringGangDen.FullStreetAddress}~s~. Don't take long.",  };
            Player.CellPhone.AddScheduledText(PhoneContact, Replies.PickRandom(), 0, true);
        }
        private void SendInitialInstructionsMessage()
        {
            string locationsList = RacketeeringLocations.Count > 5 ? $"these locations in ~p~{SelectedZone.DisplayName}~s~" : $"~b~{string.Join(", ", RacketeeringLocations.Select(loc => loc.Name))}~s~";
            List<string> Replies;
            if (WillExtortEnemyTurf)
            {
                Replies = new List<string>() {
                    $"Make your presence felt at {locationsList}. Show them who’s really in charge. ~g~${MoneyToReceive}~s~ for you.",
                    $"Head over to {locationsList} and remind them that {EnemyGang.ColorPrefix}{EnemyGang.ShortName}~s~ ain't shit. Bigger cut this time: ~g~${MoneyToReceive}~s~.",
                    $"Visit {locationsList} and let them know we offer better protection than {EnemyGang.ColorPrefix}{EnemyGang.ShortName}~s~. Take ~g~${MoneyToReceive}~s~ for your efforts.",
                    $"Go to {locationsList} and ensure they understand they’re on our turf now. You’ll earn ~g~${MoneyToReceive}~s~ for this.",
                    $"Tell {locationsList} that partnering with {EnemyGang.ColorPrefix}{EnemyGang.ShortName}~s~ is a big mistake. Bigger cut this time: ~g~${MoneyToReceive}~s~.",
                    $"Intimidate the owners at {locationsList} and assert our dominance. You'll get ~g~${MoneyToReceive}~s~."
                };
            }
            else
            {
                Replies = new List<string>() {
                    $"Go to {locationsList} and make the owners understand our protection isn’t optional. ~g~${MoneyToReceive}~s~ for you, 10% cut like usual.",
                    $"Visit {locationsList} and remind them of their overdue payments. You'll get ~g~${MoneyToReceive}~s~.",
                    $"Make sure to check in at {locationsList} and collect the protection tax. ~g~${MoneyToReceive}~s~ for you.",
                    $"Head to {locationsList} and let them know they owe us. We’re not known for our patience; you’ll get ~g~${MoneyToReceive}~s~ for your trouble.",
                    $"Stop by {locationsList} and ensure they realize they'll need to pay up. Your cut: ~g~${MoneyToReceive}~s~.",
                    $"Remind the owners at {locationsList} that they need to settle their debts. You'll get ~g~${MoneyToReceive}~s~."
                };
            }
            Player.CellPhone.AddPhoneResponse(HiringGang.Contact.Name, HiringGang.Contact.IconName, Replies.PickRandom());
        }

        public void OnInteractionMenuCreated(GameLocation gameLocation, MenuPool menuPool, UIMenu interactionMenu)
        {
            EntryPoint.WriteToConsole("Gang Racketerering Task OnTransactionMenuCreated Start");
            if(interactionMenu == null)
            {
                return;
            }
            if(gameLocation == null || !gameLocation.IsPlayerInterestedInLocation)
            {
                return;
            }    
            if (interactionMenu.MenuItems.Contains(collectMoney))
            {
                return;

            }
            collectMoney = new UIMenuItem("Collect Protection Money", $"Protection isn't optional or cheap.") { RightLabel = $"${gameLocation.ProtectionMoneyDue}" };
            collectMoney.Activated += (sender, selectedItem) =>
            {
                CollectMoney(gameLocation);
            };
            interactionMenu.AddItem(collectMoney);
            EntryPoint.WriteToConsole("Gang Racketerering Task OnTransactionMenuCreated CREATED");
        }
    }
}
