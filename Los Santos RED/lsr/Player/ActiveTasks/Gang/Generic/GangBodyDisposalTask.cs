using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Player.ActiveTasks
{
    public class GangBodyDisposalTask : GangTask, IPlayerTask
    {
        private GangDen HiringGangDen;
        private PedExt DeadBody;
        private bool HasEnteredVehicle;
        private CarCrusher CrusherLocation;
        private GameLocation SpawnLocation;

        public VehicleExt DeadBodyVehicle { get; private set; }

        public GangBodyDisposalTask(ITaskAssignable player, ITimeReportable time, IGangs gangs, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, IEntityProvideable world,
            ICrimes crimes, IWeapons weapons, INameProvideable names, IPedGroups pedGroups, IShopMenus shopMenus, IModItems modItems, PlayerTasks playerTasks, GangTasks gangTasks, PhoneContact hiringContact, Gang hiringGang) : base(player, time, gangs, placesOfInterest, settings, world, crimes, weapons, names, pedGroups, shopMenus, modItems, playerTasks, gangTasks, hiringContact, hiringGang)
        {
            DebugName = "Body Disposal";
            RepOnCompletion = 2000;
            DebtOnFail = 0;
            RepOnFail = -500;
            DaysToComplete = 2;
        }
        public override void Dispose()
        {
            DeleteCar();
            DeleteBody();
            if (CrusherLocation != null)
            {
                CrusherLocation.IsPlayerInterestedInLocation = false;
            }
            if(SpawnLocation != null)
            {
                SpawnLocation.IsPlayerInterestedInLocation = false;
            }
            base.Dispose();
        }
        protected override void GetPayment()
        {
            PaymentAmount = RandomItems.GetRandomNumberInt(HiringGang.BodyDisposalPaymentMin, HiringGang.BodyDisposalPaymentMax).Round(500);
            if (PaymentAmount <= 0)
            {
                PaymentAmount = 500;
            }
        }
        protected override void SendInitialInstructionsMessage()
        {
            List<string> Replies = new List<string>() {
                $"I got a {DeadBodyVehicle.FullName(false)} Plate #{DeadBodyVehicle.CarPlate.PlateNumber} with some unwanted guests. Pickup near ~p~{SpawnLocation.Name}~s~ on ~y~{SpawnLocation.FullStreetAddress}~s~ and take it to ~p~{CrusherLocation.Name}~s~ on ~y~{CrusherLocation.FullStreetAddress}~s~. ${PaymentAmount}"
                };
            Player.CellPhone.AddPhoneResponse(HiringGang.Contact.Name, HiringGang.Contact.IconName, Replies.PickRandom());
        }
        protected override bool GetTaskData()
        {
            HiringGangDen = PlacesOfInterest.GetMainDen(HiringGang.ID, World.IsMPMapLoaded, Player.CurrentLocation);
            if (HiringGangDen == null)
            {
                return false;
            }
            CrusherLocation = PlacesOfInterest.PossibleLocations.CarCrushers.Where(x => x.IsCorrectMap(World.IsMPMapLoaded) && x.IsSameState(Player.CurrentLocation?.CurrentZone?.GameState)).PickRandom();
            if (CrusherLocation == null)
            {
                return false;
            }
            SpawnLocation = PlacesOfInterest.PossibleLocations.GenericTaskLocations().Where(x=> x.IsCorrectMap(World.IsMPMapLoaded) && x.IsSameState(Player.CurrentLocation?.CurrentZone?.GameState)).PickRandom();
            if (SpawnLocation == null)
            {
                return false;
            }
            if (!SpawnCar() || !SpawnAndLoadBody())
            {
                return false;
            }
            return true;
        }
        protected override void AddTask()
        {
            if (CrusherLocation != null)
            {
                CrusherLocation.IsPlayerInterestedInLocation = true;
            }
            if (SpawnLocation != null)
            {
                SpawnLocation.IsPlayerInterestedInLocation = true;
            }
            base.AddTask();
        }
        protected override void Loop()
        {
            EntryPoint.WriteToConsole("Body Disposal Loop Start");
            while (true)
            {
                CurrentTask = PlayerTasks.GetTask(HiringGang.ContactName);
                if (CurrentTask == null || !CurrentTask.IsActive)
                {
                    break;
                }
                if (DeadBody == null || DeadBodyVehicle == null)
                {
                    break;
                }
                if (DeadBody.WasCrushed && DeadBodyVehicle.WasCrushed)
                {
                    CurrentTask.OnReadyForPayment(true);
                    break;
                }
                if (!DeadBodyVehicle.Vehicle.Exists())
                {
                    EntryPoint.WriteToConsole("Body Disposal Task: Vehicle Does Not Exist and wasnt crushed.");
                    break;
                }
                if (!DeadBody.Pedestrian.Exists())
                {
                    EntryPoint.WriteToConsole("Body Disposal Task: Ped Does Not Exist and wasnt crushed.");
                    break;
                }
                if (!HasEnteredVehicle && DeadBodyVehicle != null && DeadBodyVehicle.Vehicle.Exists() && Player.CurrentVehicle != null && Player.CurrentVehicle.Handle == DeadBodyVehicle.Handle)
                {
                    OnEnteredImpoundedVehicle();
                }
                GameFiber.Sleep(1000);
            }
        }
        private void OnEnteredImpoundedVehicle()
        {
            HasEnteredVehicle = true;
            SendGetRidMessage();
        }
        private void SendGetRidMessage()
        {
            List<string> Replies = new List<string>() {
                $"Get rid of that thing and then get back to the {HiringGang.DenName} on {HiringGangDen.FullStreetAddress} for payment."
                    };
            Player.CellPhone.AddScheduledText(HiringContact, Replies.PickRandom(), 0, true);
        }
        private bool SpawnCar()
        {
            string CarModel = new List<string>() { "stanier", "buffalo", "voodoo2" }.PickRandom();
            DispatchableVehicle dispatchableVehicle = new DispatchableVehicle(CarModel, 1, 1); //HiringGang.GetRandomVehicle(0, false, false, false, "", Settings);
            if (dispatchableVehicle == null)
            {
                return false;
            }
            SpawnLocation toSpawn = new SpawnLocation(SpawnLocation.EntrancePosition);
            toSpawn.Heading = Player.Character.Heading;
            toSpawn.GetClosestStreet(false);
            toSpawn.GetClosestSideOfRoad();


            toSpawn.GetRoadBoundaryPosition();
            if(toSpawn.HasRoadBoundaryPosition)
            {
                toSpawn.StreetPosition = toSpawn.RoadBoundaryPosition;
            }

            GangSpawnTask gmSpawn = new GangSpawnTask(HiringGang, toSpawn, dispatchableVehicle, null, false, Settings, Weapons, Names, false, Crimes, PedGroups, ShopMenus, World, ModItems, false, false, false);
            gmSpawn.AllowAnySpawn = true;
            gmSpawn.AddEmptyVehicleBlip = true;

            gmSpawn.AttemptSpawn();
            gmSpawn.CreatedVehicles.ForEach(x => x.AddVehicleToList(World));//World.Vehicles.AddEntity(x, ResponseType.None));
            DeadBodyVehicle = gmSpawn.CreatedVehicles.FirstOrDefault();
            if (DeadBodyVehicle == null || !DeadBodyVehicle.Vehicle.Exists())
            {
                return false;
            }
            DeadBodyVehicle.Vehicle.IsPersistent = true;
            DeadBodyVehicle.SetRandomPlate();
            DeadBodyVehicle.HasUpdatedPlateType = true;
            DeadBodyVehicle.CanHavePlateRandomlyUpdated = false;
            DeadBodyVehicle.WasModSpawned = true;
            DeadBodyVehicle.WasSpawnedEmpty = true;
            DeadBodyVehicle.IsManualCleanup = true;
            DeadBodyVehicle.IsAlwaysOpenForPlayer = true;
            return DeadBodyVehicle != null && DeadBodyVehicle.Vehicle.Exists();
        }
        private bool SpawnAndLoadBody()
        {
            if (DeadBodyVehicle == null || !DeadBodyVehicle.Vehicle.Exists())
            {
                return false;
            }
            DispatchablePerson dispatchablePerson = HiringGang.GetRandomPed(0, "");
            if (dispatchablePerson == null)
            {
                return false;
            }
            SpawnLocation toSpawn = new SpawnLocation(DeadBodyVehicle.Vehicle.Position.Around2D(10f));
            toSpawn.Heading = DeadBodyVehicle.Vehicle.Heading;
            GangSpawnTask gangSpawnTask = new GangSpawnTask(HiringGang, toSpawn, null, dispatchablePerson, false, Settings, Weapons, Names, true, Crimes, PedGroups, ShopMenus, World, ModItems, false, false, false);// Settings.SettingsManager.Police.SpawnedAmbientPoliceHaveBlip);
            gangSpawnTask.AllowAnySpawn = true;
            gangSpawnTask.AllowBuddySpawn = false;
            gangSpawnTask.AttemptSpawn();
           //gangSpawnTask.CreatedPeople.ForEach(x => { World.Pedestrians.AddEntity(x); });
            DeadBody = gangSpawnTask.CreatedPeople.FirstOrDefault();
            if (DeadBody == null || !DeadBody.Pedestrian.Exists())
            {
                return false;
            }
            DeadBody.Pedestrian.IsPersistent = true;
            DeadBody.IsDead = true;
            DeadBody.CanBeTasked = false;
            DeadBody.CanBeAmbientTasked = false;
            DeadBody.WasKilledByPlayer = true;
            DeadBody.HasBeenHurtByPlayer = true;
            DeadBody.IsManuallyDeleted = true;
            foreach (PedExt created in gangSpawnTask.CreatedPeople)
            {
                World.Pedestrians.DeadPeds.Add(created);
            }
            Player.Violations.DamageViolations.AddFakeKilled(DeadBody);
            if (!DeadBodyVehicle.VehicleBodyManager.LoadBody(DeadBody, new VehicleDoorSeatData("Trunk", "boot", 5, -2), false, World))
            {
                return false;
            }
            return DeadBody != null && DeadBody.Pedestrian.Exists();
        }
        private void DeleteCar()
        {
            if (DeadBodyVehicle != null && DeadBodyVehicle.Vehicle.Exists())
            {
                DeadBodyVehicle.FullyDelete();
            }
        }
        private void DeleteBody()
        {
            if (DeadBody != null && DeadBody.Pedestrian.Exists())
            {
                DeadBody.IsManuallyDeleted = false;
                DeadBody.DeleteBlip();
                DeadBody.Pedestrian.Delete();
            }
        }
        private void CleanupPed()
        {
            if (DeadBody == null || !DeadBody.Pedestrian.Exists())
            {
                return;
            }
            DeadBody.Pedestrian.IsPersistent = false;
            DeadBody.IsManuallyDeleted = false;//this seems to delete it, w/e
        }
        private void CleanupCar()
        {
            if (DeadBodyVehicle == null || !DeadBodyVehicle.Vehicle.Exists())
            {
                return;
            }
            DeadBodyVehicle.WasSpawnedEmpty = false;
            DeadBodyVehicle.IsManualCleanup = false;
            DeadBodyVehicle.Vehicle.IsPersistent = false;
            DeadBodyVehicle.Vehicle.LockStatus = (VehicleLockStatus)10;
        }
        protected override void OnTaskCompletedOrFailed()//called on failed and disposed?
        {
            CleanupPed();
            CleanupCar();
            if (CrusherLocation != null)
            {
                CrusherLocation.IsPlayerInterestedInLocation = false;
            }
            if (SpawnLocation != null)
            {
                SpawnLocation.IsPlayerInterestedInLocation = false;
            }
        }

    }
}
