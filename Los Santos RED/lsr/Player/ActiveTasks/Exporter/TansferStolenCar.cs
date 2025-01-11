using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Media.Animation;

namespace LosSantosRED.lsr.Player.ActiveTasks
{
    public class TansferStolenCar : IPlayerTask
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
        private PlayerTask CurrentTask;
        private bool hasGottenInCar;
        private uint GameTimeGotInCar;
        private int GameTimeToWaitBeforeComplications;
        private bool HasAddedComplications;
        private bool WillAddComplications;
        private bool hasSpawnedCar;
        private Vehicle SpawnedVehicle = null;
        private VehicleExt SpawnedVehicleExt;
        private VehicleExporter DropOffStore;
        private VehicleExporter PickUpStore;
        private int MoneyToRecieve;
        private PhoneContact Contact;
        private string CarName;
        private uint GameTimeLastDisplayedWarning;

        private bool IsSpawnedVehicleDestroyed => !SpawnedVehicle.Exists() || SpawnedVehicle.Health <= 300 || SpawnedVehicle.EngineHealth <= 300;
        private bool IsSpawnedVehicleParkedAtDestination => SpawnedVehicle.Exists() && NativeHelper.IsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, DropOffStore.CellX, DropOffStore.CellY, 2) && !SpawnedVehicle.HasOccupants && SpawnedVehicle.DistanceTo2D(DropOffStore.EntrancePosition) <= 50f;
        private bool IsPlayerDrivingSpawnedVehicle => SpawnedVehicle.Exists() && SpawnedVehicle.Driver?.Handle == Player.Character.Handle;
        private bool IsPlayerFarAwayFromSpawnedVehicle => SpawnedVehicle.Exists() && SpawnedVehicle.DistanceTo2D(Player.Character) >= 850f;
        private bool IsPlayerNearbyPickupStore => NativeHelper.IsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, PickUpStore.CellX, PickUpStore.CellY, 5);
        public TansferStolenCar(ITaskAssignable player, ITimeReportable time, IGangs gangs, PlayerTasks playerTasks, IPlacesOfInterest placesOfInterest, List<DeadDrop> activeDrops, ISettingsProvideable settings, IEntityProvideable world, ICrimes crimes, IModItems modItems, VehicleExporterContact contact)
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
            Contact = contact;
        }
        public void Setup()
        {

        }
        public void Dispose()
        {
            if (SpawnedVehicle.Exists())
            {
                SpawnedVehicleExt?.RemoveBlip();
                SpawnedVehicle.IsPersistent = false;
                SpawnedVehicle.Delete();
            }
            if (PickUpStore != null)
            {
                PickUpStore.IsPlayerInterestedInLocation = false;
            }
            if (DropOffStore != null)
            {
                DropOffStore.IsPlayerInterestedInLocation = false;
            }
        }
        public void Start()
        {
            if (Contact == null || !PlayerTasks.CanStartNewTask(Contact.Name))
            {
                return;
            }
            GetShops();
            if(DropOffStore == null || PickUpStore == null)
            {
                SendTaskAbortMessage();
                return;
            }
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
            }, "GunPickupFiber");
        }
        private void SetCompleted()
        {
            
            if (PickUpStore != null)
            {
                PickUpStore.IsPlayerInterestedInLocation = false;
            }
            if (DropOffStore != null)
            {
                DropOffStore.IsPlayerInterestedInLocation = false;
            }
            PlayerTasks.CompleteTask(Contact, true);
            SendCompletedMessage();
        }
        private void SetFailed()
        {
            if (SpawnedVehicle.Exists())
            {
                SpawnedVehicleExt?.RemoveBlip();
                SpawnedVehicle.IsPersistent = false;
            }
            if (PickUpStore != null)
            {
                PickUpStore.IsPlayerInterestedInLocation = false;
            }
            if (DropOffStore != null)
            {
                DropOffStore.IsPlayerInterestedInLocation = false;
            }
            PlayerTasks.CancelTask(Contact);
        }
        private void FinishTask()
        {
            if (CurrentTask != null && CurrentTask.IsActive && CurrentTask.IsReadyForPayment)
            {
                SetCompleted();
            }
            else if (CurrentTask != null && CurrentTask.IsActive)
            {
                SetFailed();
            }
            else
            {
                Dispose();
            }
        }
        private void Loop()
        {
            while (true)
            {
                if (CurrentTask == null || !CurrentTask.IsActive)
                {
                    //EntryPoint.WriteToConsoleTestLong($"Task Inactive for {StaticStrings.UndergroundGunsContactName}");
                    break;
                }
                else if (hasSpawnedCar && IsSpawnedVehicleDestroyed)
                {
                    //EntryPoint.WriteToConsoleTestLong($"Task Inactive for {StaticStrings.UndergroundGunsContactName}, the spawned vehicle was destroyed");
                    Game.DisplayHelp($"{Contact.Name} Vehicle Destroyed");
                    break;
                }
                else if (hasSpawnedCar && hasGottenInCar && IsPlayerFarAwayFromSpawnedVehicle)
                {
                    //EntryPoint.WriteToConsoleTestLong($"Task Inactive for {StaticStrings.UndergroundGunsContactName}, you ran away from the car");
                    break;
                }
                if (!hasSpawnedCar && IsPlayerNearbyPickupStore)
                {
                    hasSpawnedCar = SpawnVehicle(PickUpStore);
                }
                if (hasSpawnedCar && !hasGottenInCar && IsPlayerDrivingSpawnedVehicle)
                {
                    OnGotInCar();
                }
                else if (hasSpawnedCar && hasGottenInCar && Player.IsNotWanted && IsSpawnedVehicleParkedAtDestination && SpawnedVehicleExt != null && SpawnedVehicleExt.IsDamaged())
                {
                    EntryPoint.WriteToConsole("ARRIVED WITH DAMAGED CAR");
                    OnArrivedAtDestinationWithBrokenCar();
                }
                else if (hasSpawnedCar && hasGottenInCar && Player.IsNotWanted && IsSpawnedVehicleParkedAtDestination && SpawnedVehicleExt != null && !SpawnedVehicleExt.IsDamaged())
                {
                    EntryPoint.WriteToConsole("ARRIVED WITH MINT CAR");
                    OnArrivedAtDestination();
                    break;
                }
                if (WillAddComplications && hasSpawnedCar && hasGottenInCar && !HasAddedComplications && Game.GameTime - GameTimeGotInCar >= GameTimeToWaitBeforeComplications)
                {
                    AddComplications();
                }

               // EntryPoint.WriteToConsole($"hasSpawnedCar{hasSpawnedCar} hasGottenInCar{hasGottenInCar} IsSpawnedVehicleParkedAtDestination{IsSpawnedVehicleParkedAtDestination} IsDamaged{SpawnedVehicleExt?.IsDamaged()}");
                GameFiber.Sleep(1000);
            }
        }
        private void OnArrivedAtDestination()
        {
            if (SpawnedVehicle.Exists())
            {
                SpawnedVehicle.IsPersistent = false;
            }
            CurrentTask.OnReadyForPayment(true);
            Player.LastFriendlyVehicle = null;
            SpawnedVehicle.LockStatus = (VehicleLockStatus)10;
            Game.DisplayHelp($"{Contact.Name} You have arrived, leave the vehicle");
        }
        private void OnArrivedAtDestinationWithBrokenCar()
        {
            if (Game.GameTime - GameTimeLastDisplayedWarning >= 25000)
            {
                Game.DisplayHelp($"{Contact.Name} The vehicle is too damaged, get it repaired!");
                GameTimeLastDisplayedWarning = Game.GameTime;
            }
        }
        private void OnGotInCar()
        {
            SendDropoffInstructionsMessage();
            hasGottenInCar = true;
            GameTimeGotInCar = Game.GameTime;
        }
        private void AddComplications()
        {
            GangReputation gr = Player.RelationshipManager.GangRelationships.GangReputations.Where(x => x.GangRelationship == GangRespect.Hostile).PickRandom();
            if (gr != null && gr.Gang != null && SpawnedVehicleExt != null)
            {
                SendGangSabotageMessage(gr.Gang);
                GameFiber.Sleep(RandomItems.GetRandomNumberInt(5000, 8000));
                //SpawnedVehicleExt.CarPlate.IsWanted = true;
                //SpawnedVehicleExt.OriginalLicensePlate.IsWanted = true;
                Player.AddCrime(Crimes.CrimeList?.FirstOrDefault(x => x.ID == "GrandTheftAuto"), false, Player.Character.Position, SpawnedVehicleExt, null, true, true, true);
                //EntryPoint.WriteToConsoleTestLong("GUNS CONTACT, COMPLICATIONS ADDED!");
            }
            HasAddedComplications = true;
        }
        private void AddTask()
        {
            PlayerTasks.AddTask(Contact, MoneyToRecieve, 2000, 0, -500, 7, "Vehicle Transport");
            CurrentTask = PlayerTasks.GetTask(Contact.Name);
            hasGottenInCar = false;
            hasSpawnedCar = false;
            GameTimeGotInCar = 0;
            GameTimeToWaitBeforeComplications = RandomItems.GetRandomNumberInt(3000, 10000);
            HasAddedComplications = false;
            WillAddComplications = RandomItems.RandomPercent(Settings.SettingsManager.TaskSettings.VehicleExporterTransferComplicationsPercentage);
        }
        private void GetPayment()
        {
            MoneyToRecieve = RandomItems.GetRandomNumberInt(Settings.SettingsManager.TaskSettings.VehicleExporterTransferPaymentMin, Settings.SettingsManager.TaskSettings.VehicleExporterTransferPaymentMax).Round(500);
            if (MoneyToRecieve <= 0)
            {
                MoneyToRecieve = 500;
            }
        }
        private void GetShops()
        {
            DropOffStore = PlacesOfInterest.PossibleLocations.VehicleExporters.Where(x => x.ContactName == Contact.Name && x.IsEnabled && x.IsCorrectMap(World.IsMPMapLoaded) && x.IsSameState(Player.CurrentLocation?.CurrentZone?.GameState)).PickRandom();
            PickUpStore = null;
            if (DropOffStore != null)
            {
                PickUpStore = PlacesOfInterest.PossibleLocations.VehicleExporters.Where(x => x.ContactName == Contact.Name && x.Name != DropOffStore.Name && x.ParkingSpaces.Any() && x.IsCorrectMap(World.IsMPMapLoaded) && x.IsSameState(Player.CurrentLocation?.CurrentZone?.GameState)).PickRandom();
            }
            if (PickUpStore != null)
            {
                PickUpStore.IsPlayerInterestedInLocation = true;
            }
            if (DropOffStore != null)
            {
                DropOffStore.IsPlayerInterestedInLocation = true;
            }
        }
        private bool SpawnVehicle(VehicleExporter PickUpStore)
        {
            SpawnLocation SpawnLocation = new SpawnLocation(PickUpStore.EntrancePosition);
            SpawnPlace ParkingSpot = PickUpStore.ParkingSpaces.PickRandom();// TR NOTE REMOVED ENTITY CHECK
            if (ParkingSpot == null)
            {
                return false;
            }
            SpawnLocation.StreetPosition = ParkingSpot.Position;
            SpawnLocation.Heading = ParkingSpot.Heading;
            if (SpawnLocation.StreetPosition == Vector3.Zero)
            {
                return false;
            }
            List<VehicleItem> PossibleVehicleItems = new List<VehicleItem>();
            foreach(ModItem modint in PickUpStore.Menu.Items.Where(x=> x.ModItem != null).Select(x=>x.ModItem))
            {
                VehicleItem vi = ModItems.PossibleItems.VehicleItems.FirstOrDefault(x => modint.Name == x.Name);
                if(vi != null && (!vi.IsDLC || Settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles))
                {
                    PossibleVehicleItems.Add(vi);
                }
            }
            VehicleItem selectedItem = PossibleVehicleItems.PickRandom();
            if (selectedItem == null || selectedItem.ModelItem == null)
            {
                return false;
            }
            uint modelHash = selectedItem.ModelItem.ModelHash;
            World.Vehicles.CleanupAmbient();
            SpawnedVehicle = new Vehicle(modelHash, SpawnLocation.StreetPosition, SpawnLocation.Heading);
            GameFiber.Yield();
            if (!SpawnedVehicle.Exists())
            {
                return false;
            }
            SpawnedVehicle.IsPersistent = true;
            Player.LastFriendlyVehicle = null;
            Player.LastFriendlyVehicle = SpawnedVehicle;
            SpawnedVehicleExt = new VehicleExt(SpawnedVehicle, Settings);
            World.Vehicles.AddCivilian(SpawnedVehicleExt);
            SpawnedVehicleExt.CanHavePlateRandomlyUpdated = false;
            SpawnedVehicleExt.SetRandomPlate();
            SpawnedVehicleExt.AddBlip();
            SpawnedVehicleExt.SetRandomColor();
            SpawnedVehicleExt.CarPlate.IsWanted = true;
            SpawnedVehicleExt.IsStolen = true;
            SpawnedVehicleExt.CanBeExported = false;      
            CarName = SpawnedVehicleExt.GetCarName();
            SendVehicleSpawnedMessage();
            return true;
        }
        private void SendDropoffInstructionsMessage()
        {
            List<string> Replies = new List<string>() {
                $"Take it to the shop on {DropOffStore.FullStreetAddress}. Make sure it is in perfect condition.",
                $"Get to {DropOffStore.FullStreetAddress}. Don't fuck up the car, it needs to be mint",
                $"Bring it to {DropOffStore.FullStreetAddress}, don't loiter. Make sure it gets there in perfect condition.",
                $"Bring it to {DropOffStore.FullStreetAddress}, don't loiter. Do NOT fuck up the car.",
                    };
            Player.CellPhone.AddScheduledText(Contact, Replies.PickRandom(),0, true);
        }
        private void SendVehicleSpawnedMessage()
        {
            List<string> PickupMessage = new List<string>() {
                        $"The ~p~{CarName}~s~ is parked out front, plate number is {SpawnedVehicle.LicensePlate}. Keys should be in it.",
                        $"Use the ~p~{CarName}~s~ out front. Should be unlocked and ready to go.",
                        $"Drive the ~p~{CarName}~s~ parked in front of the shop. Keys are under the visor.",
                        $"Use the ~p~{CarName}~s~, plate number is {SpawnedVehicle.LicensePlate}. Keys should be in it.",
                        $"Take the ~p~{CarName}~s~. It is ready to go.",
                        };
            Player.CellPhone.AddScheduledText(Contact, PickupMessage.PickRandom(), 0, true);
        }
        private void SendInitialInstructionsMessage()
        {
            List<string> Replies = new List<string>() {
                $"Need you to transfer a hot car from our shop on {PickUpStore.FullStreetAddress} and bring it to the shop on {DropOffStore.FullStreetAddress}. The car is out front. ${MoneyToRecieve} when you are done",
                $"Go get the hot car from {PickUpStore.FullStreetAddress} and take it to {DropOffStore.FullStreetAddress}. ${MoneyToRecieve} on completion",
                $"There is a hot car in front of {PickUpStore.FullStreetAddress}, go get it and bring it to {DropOffStore.FullStreetAddress}. Don't draw attention. Payment is ${MoneyToRecieve}",
                    };
            Player.CellPhone.AddPhoneResponse(Contact.Name, Replies.PickRandom());
        }
        private void SendCompletedMessage()
        {
            List<string> Replies = new List<string>() {
                        $"Seems like that thing we discussed is done? Sending you ${MoneyToRecieve}",
                        $"Word got around that you are done with that thing for us, sending your payment of ${MoneyToRecieve}",
                        $"Sending your payment of ${MoneyToRecieve}",
                        $"Sending ${MoneyToRecieve}",
                        $"Heard you were done. We owe you ${MoneyToRecieve}",
                        };
            Player.CellPhone.AddScheduledText(Contact, Replies.PickRandom(), 1, false);
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
            Player.CellPhone.AddPhoneResponse(Contact.Name, Replies.PickRandom());
        }
        private void SendGangSabotageMessage(Gang gang)
        {
            List<string> Replies2 = new List<string>() {
                                        $"Seems the police might be looking for that car. Wonder who could have informed them.",
                                        $"You've got some balls, driving a hot car. I let the LSPD know, and they think so too.",

                                        $"Seems like a car was just called in as stolen, must be a coincidence.",
                                        $"LSPD gonna fuck you and your car up prick.",
                                        $"Cops already know about the car, good luck dickhead.",
                                        $"Trying to move some hot cars I see? LSPD will be thrilled to know.",

                                        $"Did you think you could keep this from us? Would be unfortunate to get caught in that hot car.",
                                        $"Enjoy your time in Bolingbroke SHU",
                                            };
            Player.CellPhone.AddScheduledText(gang.Contact, Replies2.PickRandom(), 0, true);
        }
    }
}
