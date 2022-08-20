using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace LosSantosRED.lsr.Player.ActiveTasks
{
    public class GunPickupTask
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
        private PlayerTask CurrentTask;
        private bool hasGottenInCar;
        private uint GameTimeGotInCar;
        private int GameTimeToWaitBeforeComplications;
        private bool HasAddedComplications;
        private bool WillAddComplications;
        private bool hasSpawnedCar;
        private Vehicle SpawnedVehicle = null;
        private VehicleExt SpawnedVehicleExt;
        private Rage.Object GunProp;


        private GunStore DropOffStore;
        private GunStore PickUpStore;




        private int MoneyToRecieve;
        private bool IsSpawnedVehicleDestroyed => !SpawnedVehicle.Exists() || SpawnedVehicle.Health <= 300 || SpawnedVehicle.EngineHealth <= 300;
        private bool IsSpawnedVehicleParkedAtDestination => SpawnedVehicle.Exists() && NativeHelper.IsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, DropOffStore.CellX, DropOffStore.CellY, 2) && !SpawnedVehicle.HasOccupants && SpawnedVehicle.DistanceTo2D(DropOffStore.EntrancePosition) <= 50f;
        private bool IsPlayerDrivingSpawnedVehicle => SpawnedVehicle.Exists() && SpawnedVehicle.Driver?.Handle == Player.Character.Handle;
        private bool IsPlayerFarAwayFromSpawnedVehicle => SpawnedVehicle.Exists() && SpawnedVehicle.DistanceTo2D(Player.Character) >= 850f;
        private bool IsPlayerNearbyPickupStore => NativeHelper.IsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, PickUpStore.CellX, PickUpStore.CellY, 5);
        public GunPickupTask(ITaskAssignable player, ITimeReportable time, IGangs gangs, PlayerTasks playerTasks, IPlacesOfInterest placesOfInterest, List<DeadDrop> activeDrops, ISettingsProvideable settings, IEntityProvideable world, ICrimes crimes)
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
            if (SpawnedVehicle.Exists())
            {
                Blip attachedBlip = SpawnedVehicle.GetAttachedBlip();
                if (attachedBlip.Exists())
                {
                    attachedBlip.Delete();
                }
                SpawnedVehicle.IsPersistent = false;
                SpawnedVehicle.Delete();
            }
            if (GunProp.Exists())
            {
                GunProp.Delete();
            }
        }
        public void Start()
        {
            if (PlayerTasks.CanStartNewTask(EntryPoint.UndergroundGunsContactName))
            {
                GetShops();
                if (DropOffStore != null && PickUpStore != null)
                {
                    GetPayment();
                    SendInitialInstructionsMessage();
                    EntryPoint.WriteToConsole($"Starting Underground Guns Pickup Guns Task");
                    AddTask();
                    GameFiber PayoffFiber = GameFiber.StartNew(delegate
                    {
                        Loop();
                        FinishTask();
                    }, "GunPickupFiber");
                }
                else
                {
                    SendTaskAbortMessage();
                }
            }
        }
        private void SetCompleted()
        {
            SendCompletedMessage();
            PlayerTasks.CompleteTask(EntryPoint.UndergroundGunsContactName, true);
        }
        private void SetInactive()
        {
            if (SpawnedVehicle.Exists())
            {
                Blip attachedBlip = SpawnedVehicle.GetAttachedBlip();
                if (attachedBlip.Exists())
                {
                    attachedBlip.Delete();
                }
                SpawnedVehicle.IsPersistent = false;
            }
            if (GunProp.Exists())
            {
                GunProp.Delete();
            }
            PlayerTasks.CancelTask(EntryPoint.UndergroundGunsContactName);
        }
        private void FinishTask()
        {
            if (CurrentTask != null && CurrentTask.IsActive && CurrentTask.IsReadyForPayment)
            {
                SetCompleted();
            }
            else if (CurrentTask != null && CurrentTask.IsActive)
            {
                SetInactive();
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
                    EntryPoint.WriteToConsole($"Task Inactive for {EntryPoint.UndergroundGunsContactName}");
                    break;
                }
                else if (hasSpawnedCar && IsSpawnedVehicleDestroyed)
                {
                    EntryPoint.WriteToConsole($"Task Inactive for {EntryPoint.UndergroundGunsContactName}, the spawned vehicle was destroyed");
                    Game.DisplayHelp($"{EntryPoint.UndergroundGunsContactName} Vehicle Destroyed");
                    break;
                }
                else if (hasSpawnedCar && hasGottenInCar && IsPlayerFarAwayFromSpawnedVehicle)
                {
                    EntryPoint.WriteToConsole($"Task Inactive for {EntryPoint.UndergroundGunsContactName}, you ran away from the car");
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
                else if (hasSpawnedCar && hasGottenInCar && Player.IsNotWanted && IsSpawnedVehicleParkedAtDestination)
                {
                    OnArrivedAtDestination();
                    break;
                }
                if(WillAddComplications && hasSpawnedCar && hasGottenInCar && !HasAddedComplications && Game.GameTime - GameTimeGotInCar >= GameTimeToWaitBeforeComplications)
                {
                    AddComplications();
                }


                GameFiber.Sleep(1000);
            }
        }
        private void OnArrivedAtDestination()
        {
            if (SpawnedVehicle.Exists())
            {
                SpawnedVehicle.IsPersistent = false;
            }
            if(GunProp.Exists())
            {
                GunProp.IsPersistent = false;
            }
            CurrentTask.IsReadyForPayment = true;
            Player.LastFriendlyVehicle = null;
            SpawnedVehicle.SetLock((VehicleLockStatus)10);
            EntryPoint.WriteToConsole($"You ARRIVED! so it is now ready for payment!, doors are locked!");

            Game.DisplayHelp($"{EntryPoint.UndergroundGunsContactName} You have arrived, leave the vehicle");

            //Game.DisplayHelp($"You have arrived, leave the vehicle");
        }
        private void OnGotInCar()
        {
            SendDropoffInstructionsMessage();
            hasGottenInCar = true;
            GameTimeGotInCar = Game.GameTime;
        }



        private void AddComplications()
        {
            GangReputation gr = Player.GangRelationships.GangReputations.Where(x => x.GangRelationship == GangRespect.Hostile).PickRandom();
            if (gr != null && gr.Gang != null && SpawnedVehicleExt != null)
            {
                SendGangSabotageMessage(gr.Gang);
                GameFiber.Sleep(RandomItems.GetRandomNumberInt(5000, 8000));
                SpawnedVehicleExt.CarPlate.IsWanted = true;
                SpawnedVehicleExt.OriginalLicensePlate.IsWanted = true;
                Player.AddCrime(Crimes.CrimeList?.FirstOrDefault(x => x.ID == "GrandTheftAuto"), false, Player.Character.Position, SpawnedVehicleExt, null, true, true, true);
                EntryPoint.WriteToConsole("GUNS CONTACT, COMPLICATIONS ADDED!");
            }
            HasAddedComplications = true;
        }
        private void AddTask()
        {
            PlayerTasks.AddTask(EntryPoint.UndergroundGunsContactName, MoneyToRecieve, 2000, 0, -500, 7,"Gun Transport");
            CurrentTask = PlayerTasks.GetTask(EntryPoint.UndergroundGunsContactName);
            hasGottenInCar = false;
            hasSpawnedCar = false;
            GameTimeGotInCar = 0;
            GameTimeToWaitBeforeComplications = RandomItems.GetRandomNumberInt(3000, 10000);
            HasAddedComplications = false;


            WillAddComplications = RandomItems.RandomPercent(Settings.SettingsManager.TaskSettings.UndergroundGunsGunPickupComplicationsPercentage);

        }
        private void GetPayment()
        {
            MoneyToRecieve = RandomItems.GetRandomNumberInt(Settings.SettingsManager.TaskSettings.UndergroundGunsGunPickupPaymentMin, Settings.SettingsManager.TaskSettings.UndergroundGunsGunPickupPaymentMax).Round(500);
            if (MoneyToRecieve <= 0)
            {
                MoneyToRecieve = 500;
            }
        }
        private void GetShops()
        {
            DropOffStore = PlacesOfInterest.PossibleLocations.GunStores.Where(x => x.ContactName == EntryPoint.UndergroundGunsContactName && x.IsEnabled).PickRandom();
            PickUpStore = null;
            if (DropOffStore != null)
            {
                PickUpStore = PlacesOfInterest.PossibleLocations.GunStores.Where(x => x.ContactName == EntryPoint.UndergroundGunsContactName && x.Name != DropOffStore.Name && x.ParkingSpaces.Any()).PickRandom();
            }
        }
        private bool SpawnVehicle(GunStore PickUpStore)
        {
            SpawnLocation SpawnLocation = new SpawnLocation(PickUpStore.EntrancePosition);
            SpawnPlace ParkingSpot = null;
            foreach (SpawnPlace sp in PickUpStore.ParkingSpaces)
            {
                if (!Rage.World.GetEntities(sp.Position, 10f, GetEntitiesFlags.ConsiderAllVehicles).Any())
                {
                    ParkingSpot = sp;
                    break;
                }
            }
            if(ParkingSpot == null)
            {
                return false;
            }
            SpawnLocation.StreetPosition = ParkingSpot.Position;
            SpawnLocation.Heading = ParkingSpot.Heading;
            if (SpawnLocation.StreetPosition != Vector3.Zero)
            {
                SpawnedVehicle = new Vehicle("burrito3", SpawnLocation.StreetPosition, SpawnLocation.Heading);
                GameFiber.Yield();
                if (SpawnedVehicle.Exists())
                {
                    SpawnedVehicle.PrimaryColor = Color.Navy;
                    SpawnedVehicle.IsPersistent = true;
                    Blip myBlip = SpawnedVehicle.AttachBlip();
                    myBlip.Color = Color.DarkRed;
                    myBlip.Scale = 0.3f;
                    Player.LastFriendlyVehicle = null;
                    Player.LastFriendlyVehicle = SpawnedVehicle;
                    GunProp = new Rage.Object("gr_prop_gr_gunsmithsupl_03a", PickUpStore.EntrancePosition);
                    if (GunProp.Exists())
                    {
                        GunProp.AttachTo(SpawnedVehicle, SpawnedVehicle.GetBoneIndex("chassis_dummy"), new Vector3(0f, -1f, -0.3f), new Rotator(0f, 0f, 0f));
                    }
                    World.Vehicles.AddEntity(SpawnedVehicle);
                    SpawnedVehicleExt = World.Vehicles.GetVehicleExt(SpawnedVehicle.Handle);
                    if (SpawnedVehicleExt != null)
                    {
                        EntryPoint.WriteToConsole("Spawned Guns Task Burrito, FOUND VEHICLE ");
                        // World.Vehicles.UpdatePlate(SpawnedVehicleExt, true);
                        string NewPlateNumber = RandomItems.RandomString(8);
                        SpawnedVehicleExt.Vehicle.LicensePlate = NewPlateNumber;
                        SpawnedVehicleExt.OriginalLicensePlate.PlateNumber = NewPlateNumber;
                        SpawnedVehicleExt.CarPlate.PlateNumber = NewPlateNumber;

                    }
                    else
                    {
                        EntryPoint.WriteToConsole("Spawned Guns Task Burrito, DIDNT FIND VEHCILE :( ");
                    }
                    SendVehicleSpawnedMessage();
                    return true;
                }
            }
            return false;
        }
        private void SendDropoffInstructionsMessage()
        {
            List<string> Replies = new List<string>() {
                $"Take the van to the shop on {DropOffStore.FullStreetAddress}.",
                $"Get to {DropOffStore.FullStreetAddress}.",
                $"Bring the van to {DropOffStore.FullStreetAddress}, don't loiter.",
                    };
            Player.CellPhone.AddPhoneResponse(EntryPoint.UndergroundGunsContactName, Replies.PickRandom());
        }
        private void SendVehicleSpawnedMessage()
        {
            List<string> PickupMessage = new List<string>() {
                        $"The Dark Blue ~p~Burrito Van~s~ is parked out front, plate number is {SpawnedVehicle.LicensePlate}. Keys should be in it.",
                        $"Use the Dark Blue ~p~Burrito Van~s~ out front. Should be unlocked and ready to go.",
                        $"Drive the guns with the Dark Blue ~p~Burrito Van~s~ parked in front of the shop. Keys are under the visor.",
                        $"The guns are loaded in the Dark Blue ~p~Burrito Van~s~, plate number is {SpawnedVehicle.LicensePlate}. Keys should be in it.",
                        $"Take the Dark Blue ~p~Burrito Van~s~. It is already loaded and ready to go.",
                        };
            Player.CellPhone.AddScheduledText(EntryPoint.UndergroundGunsContactName, "CHAR_BLANK_ENTRY", PickupMessage.PickRandom(), 1);
        }
        private void SendInitialInstructionsMessage()
        {
            List<string> Replies = new List<string>() {
                $"Need you to pickup some guns from our shop on {PickUpStore.FullStreetAddress} and bring them to the shop on {DropOffStore.FullStreetAddress}. They are loaded in the ~p~Burrito Van~s~ out front. ${MoneyToRecieve} when you are done",
                $"Go get the van from {PickUpStore.FullStreetAddress} and take it to {DropOffStore.FullStreetAddress}. ${MoneyToRecieve} on completion",
                $"There is a van in front of {PickUpStore.FullStreetAddress}, go get it and bring it to {DropOffStore.FullStreetAddress}. Some sensitive stuff in the back, don't draw attention. Payment is ${MoneyToRecieve}",
                    };
            Player.CellPhone.AddPhoneResponse(EntryPoint.UndergroundGunsContactName, Replies.PickRandom());
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
            Player.CellPhone.AddScheduledText(EntryPoint.UndergroundGunsContactName, "CHAR_BLANK_ENTRY", Replies.PickRandom(), 0);
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
            Player.CellPhone.AddPhoneResponse(EntryPoint.UndergroundGunsContactName, Replies.PickRandom());
        }
        private void SendGangSabotageMessage(Gang gang)
        {
            List<string> Replies2 = new List<string>() {
                                        $"Seems the police might be looking for that van. Wonder who could have informed them.",
                                        $"You've got some balls, driving a hot van full of guns. I let the LSPD know, and they think so too.",

                                        $"Seems like a van was just called in as stolen, must be a coincidence.",
                                        $"LSPD gonna fuck you and your van full of guns up prick.",
                                        $"Cops already know about the van, good luck dickhead.",
                                        $"Trying to move some hot guns I see? LSPD will be thrilled to know.",

                                        $"Did you think you could keep this from us? Would be unfortunate to get caught with all those guns.",
                                        $"Enjoy your time in Bolingbroke SHU",
                                            };
            Player.CellPhone.AddScheduledText(gang.ContactName, gang.ContactIcon, Replies2.PickRandom(), 0);
        }
    }
}
