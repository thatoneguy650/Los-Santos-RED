using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Player.ActiveTasks
{
    public class GangGetCarOutOfImpoundTask
    {
        private bool SpawnedVehicle = false;
        private ITaskAssignable Player;
        private ITimeReportable Time;
        private IGangs Gangs;
        private IPlacesOfInterest PlacesOfInterest;
        private ISettingsProvideable Settings;
        private IEntityProvideable World;
        private ICrimes Crimes;
        private PlayerTasks PlayerTasks;
        private IWeapons Weapons;
        private INameProvideable Names;
        private IPedGroups PedGroups;
        private IShopMenus ShopMenus;
        private IModItems ModItems;

        private Gang HiringGang;
        private GangDen HiringGangDen;
        private PlayerTask CurrentTask;
        private GangContact Contact;
        private int MoneyToRecieve;
        private PoliceStation ImpoundLocation;
        private VehicleExt ImpoundedVehicle;

        private bool HasTaskData => HiringGangDen != null && ImpoundLocation != null && SpawnedVehicle;
        public GangGetCarOutOfImpoundTask(ITaskAssignable player, ITimeReportable time, IGangs gangs, PlayerTasks playerTasks, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, IEntityProvideable world, 
            ICrimes crimes,IWeapons weapons, INameProvideable names, IPedGroups pedGroups, IShopMenus shopMenus, IModItems modItems)
        {
            Player = player;
            Time = time;
            Gangs = gangs;
            PlayerTasks = playerTasks;
            PlacesOfInterest = placesOfInterest;
            Settings = settings;
            World = world;
            Crimes = crimes;
            Weapons = weapons;
            Names = names;
            PedGroups = pedGroups;
            ShopMenus = shopMenus;
            ModItems = modItems;
        }
        public void Setup()
        {

        }
        public void Dispose()
        {
            if (ImpoundLocation != null)
            {
                ImpoundLocation.IsPlayerInterestedInLocation = false;
            }
            Delete();
        }
        public void Start(Gang ActiveGang)
        {
            HiringGang = ActiveGang;
            Contact = new GangContact(HiringGang.ContactName, HiringGang.ContactIcon);
            if(!PlayerTasks.CanStartNewTask(ActiveGang?.ContactName))
            {
                return;
            }
            GetTaskData();
            if (!HasTaskData)
            {
                SendTaskAbortMessage();
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
            }, "PayoffFiber");       
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
                if(ImpoundedVehicle == null || !ImpoundedVehicle.Vehicle.Exists())
                {
                    EntryPoint.WriteToConsole("Impound Theft Task: Vehicle Does Not Exist.");
                    break;
                }
                if (ImpoundedVehicle.Vehicle.Speed <= 0.5f && ImpoundedVehicle.Vehicle.DistanceTo2D(HiringGangDen.EntrancePosition) <= 70f)//Player.RelationshipManager.GangRelationships.GetReputation(TargetGang)?.MembersKilled >= KilledMembersAtStart + KillRequirement && hasExploded)
                {
                    CurrentTask.OnReadyForPayment(true);
                    break;
                }
                GameFiber.Sleep(1000);
            }
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
        private void SetCompleted()
        {
            Cleanup();
            if (ImpoundLocation != null)
            {
                ImpoundLocation.IsPlayerInterestedInLocation = false;
            }
            PlayerTasks.CompleteTask(HiringGang.ContactName, true);
        }
        private void SetFailed()
        {
            Cleanup();
            if (ImpoundLocation != null)
            {
                ImpoundLocation.IsPlayerInterestedInLocation = false;
            }
            SendFailMessage();
            PlayerTasks.FailTask(HiringGang.ContactName);
        }
        private void GetTaskData()
        {
            HiringGangDen = PlacesOfInterest.GetMainDen(HiringGang.ID, World.IsMPMapLoaded);
            ImpoundLocation = PlacesOfInterest.PossibleLocations.PoliceStations.Where(x => x.HasImpoundLot).PickRandom();
            if(SpawnCar())
            {
                SpawnedVehicle = true;
            }
        }

        private void GetPayment()
        {
            MoneyToRecieve = RandomItems.GetRandomNumberInt(HiringGang.ImpoundTheftPaymentMin, HiringGang.ImpoundTheftPaymentMax).Round(500);
            if (MoneyToRecieve <= 0)
            {
                MoneyToRecieve = 500;
            }
        }
        private void AddTask()
        {
            if (ImpoundLocation != null)
            {
                ImpoundLocation.IsPlayerInterestedInLocation = true;
            }
            PlayerTasks.AddTask(HiringGang.ContactName, MoneyToRecieve, 2000, 0, -500, 3, "Lockup Theft", false);
        }
        private void SendInitialInstructionsMessage()
        {
            List<string> Replies = new List<string>() {
                $"Need you to get my {ImpoundedVehicle.FullName(false)} Plate # {ImpoundedVehicle.CarPlate.PlateNumber} out of the impound lot at ~p~{ImpoundLocation.Name}~s~ and bring it back to {HiringGang.DenName} on {HiringGangDen.FullStreetAddress} for your payment of ${MoneyToRecieve}.",
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
        private void SendFailMessage()
        {
            List<string> Replies = new List<string>() {
                        $"You fucked that up pretty bad.",
                        };
            Player.CellPhone.AddScheduledText(Contact, Replies.PickRandom(), 1);
        }

        private bool SpawnCar()
        {
            DispatchableVehicle dispatchableVehicle = HiringGang.GetRandomVehicle(0, false, false, true, "", Settings);
            if(dispatchableVehicle == null)
            {
                return false;
            }
            SpawnPlace existingSpawn = ImpoundLocation.VehicleImpoundLot.ParkingSpots.PickRandom();
            if(existingSpawn == null)
            {
                return false;
            }
            SpawnLocation toSpawn = new SpawnLocation(existingSpawn.Position);
            toSpawn.Heading = existingSpawn.Heading;
            GangSpawnTask gmSpawn = new GangSpawnTask(HiringGang, toSpawn, dispatchableVehicle, null, false, Settings, Weapons, Names, false, Crimes, PedGroups, ShopMenus, World, ModItems, false, false, false);
            gmSpawn.AllowAnySpawn = true;
            gmSpawn.AllowBuddySpawn = false;
            gmSpawn.AttemptSpawn();
            ImpoundedVehicle = gmSpawn.CreatedVehicles.FirstOrDefault();
            if(ImpoundedVehicle == null || !ImpoundedVehicle.Vehicle.Exists())
            {
                return false;
            }
            ImpoundedVehicle.Vehicle.IsPersistent = true;
            ImpoundedVehicle.SetRandomPlate();
            ImpoundedVehicle.WasModSpawned = true;
            ImpoundedVehicle.WasSpawnedEmpty = true;
            if(!ImpoundLocation.VehicleImpoundLot.ImpoundVehicle(ImpoundedVehicle, Time))
            {
                return false;
            }
            return true;
        }
        private void Cleanup()
        {
            if (ImpoundedVehicle == null || !ImpoundedVehicle.Vehicle.Exists())
            {
                return;
            }
            if (ImpoundedVehicle != null && ImpoundedVehicle.Vehicle.Exists())
            {
                ImpoundedVehicle.WasSpawnedEmpty = false;
                ImpoundedVehicle.Vehicle.IsPersistent = false;
            }
        }
        private void Delete()
        {
            if (ImpoundedVehicle == null || !ImpoundedVehicle.Vehicle.Exists())
            {
                return;
            }
            if (ImpoundedVehicle != null && ImpoundedVehicle.Vehicle.Exists())
            {
                ImpoundedVehicle.WasSpawnedEmpty = false;
                ImpoundedVehicle.Vehicle.IsPersistent = false;
            }
        }
    }
}
