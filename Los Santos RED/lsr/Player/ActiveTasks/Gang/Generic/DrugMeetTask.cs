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
    public class GangDrugMeetTask : GangTask, IPlayerTask
    {
        private GangDen HiringGangDen;
        private ModItem ModItem;
        private int Quantity;
        private Gang DealingGang;
        private IllicitMarketplace DealingLocation;
        private bool HasArrivedNearMeetup;
        private bool HasCompletedTransaction;

        public GangDrugMeetTask(ITaskAssignable player, ITimeReportable time, IGangs gangs, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, IEntityProvideable world,
            ICrimes crimes, IWeapons weapons, INameProvideable names, IPedGroups pedGroups, IShopMenus shopMenus, IModItems modItems, PlayerTasks playerTasks, GangTasks gangTasks, 
            PhoneContact hiringContact, Gang hiringGang, ModItem modItem, int quantity, Gang dealingGang) : base(player, time, gangs, placesOfInterest, settings, world, crimes, weapons, names, pedGroups, shopMenus, modItems, playerTasks, gangTasks, hiringContact, hiringGang)
        {
            DebugName = "Drug Meet";
            RepOnCompletion = 2000;
            DebtOnFail = 0;
            RepOnFail = -500;
            DaysToComplete = 2;
            ModItem = modItem;
            Quantity = quantity;
            DealingGang = dealingGang;
        }
        public override void Dispose()
        {
            DeleteCar();
            DeleteBody();
            if (DealingLocation != null)
            {
                DealingLocation.IsPlayerInterestedInLocation = false;
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
                $"TBA ${PaymentAmount}"
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
            if(ModItem == null)
            {
                return false; 
            }
            if(DealingGang == null)
            {
                return false;
            }
            if(Quantity == 0)
            {
                return false;
            }
            DealingLocation = PlacesOfInterest.PossibleLocations.IllicitMarketplaces.Where(x => x.IsCorrectMap(World.IsMPMapLoaded) && x.IsSameState(Player.CurrentLocation?.CurrentZone?.GameState)).PickRandom();
            if (DealingLocation == null)
            {
                return false;
            }
            //if (!SpawnCar() || !SpawnAndLoadBody())
            //{
            //    return false;
            //}
            return true;
        }
        protected override void AddTask()
        {
            if (DealingLocation != null)
            {
                DealingLocation.IsPlayerInterestedInLocation = true;
            }
            base.AddTask();
        }
        protected override void Loop()
        {
            EntryPoint.WriteToConsole("Drug Meet Loop Start");
            while (true)
            {
                CurrentTask = PlayerTasks.GetTask(HiringGang.ContactName);
                if (CurrentTask == null || !CurrentTask.IsActive)
                {
                    break;
                }


                if(!HasArrivedNearMeetup && DealingLocation.DistanceToPlayer<= 200f)
                {
                    OnArrivedNearMeetup();
                }
                if(HasArrivedNearMeetup && DealingLocation.DistanceToPlayer >= 275f)
                {
                    OnWentAwayFromMeetup();
                }


                //if (DeadBody == null || DeadBodyVehicle == null)
                //{
                //    break;
                //}
                //if (DeadBody.WasCrushed && DeadBodyVehicle.WasCrushed)
                //{
                //    CurrentTask.OnReadyForPayment(true);
                //    break;
                //}
                //if (!DeadBodyVehicle.Vehicle.Exists())
                //{
                //    EntryPoint.WriteToConsole("Body Disposal Task: Vehicle Does Not Exist and wasnt crushed.");
                //    break;
                //}
                //if (!DeadBody.Pedestrian.Exists())
                //{
                //    EntryPoint.WriteToConsole("Body Disposal Task: Ped Does Not Exist and wasnt crushed.");
                //    break;
                //}
                //if (!HasEnteredVehicle && DeadBodyVehicle != null && DeadBodyVehicle.Vehicle.Exists() && Player.CurrentVehicle != null && Player.CurrentVehicle.Handle == DeadBodyVehicle.Handle)
                //{
                //    OnEnteredImpoundedVehicle();
                //}
                GameFiber.Sleep(1000);
            }
        }

        private void OnWentAwayFromMeetup()
        {
            EntryPoint.WriteToConsole("DRUG MEETUP PLAYER IS WENT AWAY FROM LOCATION");
            HasArrivedNearMeetup = false;
            CleanupDealer();
        }

        private void CleanupDealer()
        {

        }

        private void OnArrivedNearMeetup()
        {
            EntryPoint.WriteToConsole("DRUG MEETUP PLAYER IS NEARBY LOCATION");
            HasArrivedNearMeetup = true;
            if(HasCompletedTransaction)
            {
                return;
            }
            SpawnDealers();
        }

        private void SpawnDealers()
        {
            EntryPoint.WriteToConsole("DRUG MEETUP START SPAWN DEALERS");
            SpawnLocation spawnLocation = new SpawnLocation(DealingLocation.EntrancePosition);
            spawnLocation.GetClosestStreet(false);
            spawnLocation.GetClosestSidewalk(); ;
            GangSpawnTask gangSpawnTask = new GangSpawnTask(DealingGang, spawnLocation,null,DealingGang.GetRandomPed(0,""),true,Settings,Weapons,Names,false,Crimes,PedGroups,ShopMenus,World,ModItems,true,true,true);
            gangSpawnTask.PlacePedOnGround = true;
            gangSpawnTask.AllowAnySpawn = true;
            gangSpawnTask.AllowBuddySpawn = true;
            gangSpawnTask.SpawnRequirement = TaskRequirements.Guard;
            gangSpawnTask.AttemptSpawn();


            GangMember gangMember = (GangMember)gangSpawnTask.CreatedPeople.FirstOrDefault();
            if(gangMember == null)
            {
                return;
            }

            EntryPoint.WriteToConsole($"DRUG MEETUP SPAWNED DEALERS {gangMember.Handle}");

            //gangMember.ItemDesires.DesiredItems.Clear();
            //gangMember.ItemDesires.DesiredItems.Add(new DesiredItem(ModItem, -1, Quantity));

            gangMember.SetupTransactionItems(new ShopMenu("testmenu", "testmenu2", new List<MenuItem>() { new MenuItem(ModItem.Name, -1, 500) { ModItem = ModItem, NumberOfItemsToPurchaseFromPlayer = Quantity, NumberOfItemsToSellToPlayer = 0 } }),true);


            //foreach(DesiredItem di in gangMember.ItemDesires.DesiredItems)
            //{
            //    EntryPoint.WriteToConsole($" {di.ModItem?.Name} NumberOfItemsToPurchaseFromPlayer{di.NumberOfItemsToPurchaseFromPlayer} NumberOfItemsToSellToPlayer{di.NumberOfItemsToSellToPlayer}");
            //}

        }

        private void DeleteCar()
        {
            //if (DeadBodyVehicle != null && DeadBodyVehicle.Vehicle.Exists())
            //{
            //    DeadBodyVehicle.FullyDelete();
            //}
        }
        private void DeleteBody()
        {
            //if (DeadBody != null && DeadBody.Pedestrian.Exists())
            //{
            //    DeadBody.IsManuallyDeleted = false;
            //    DeadBody.DeleteBlip();
            //    DeadBody.Pedestrian.Delete();
            //}
        }
        private void CleanupPed()
        {
            //if (DeadBody == null || !DeadBody.Pedestrian.Exists())
            //{
            //    return;
            //}
            //DeadBody.Pedestrian.IsPersistent = false;
            //DeadBody.IsManuallyDeleted = false;//this seems to delete it, w/e
        }
        private void CleanupCar()
        {
            //if (DeadBodyVehicle == null || !DeadBodyVehicle.Vehicle.Exists())
            //{
            //    return;
            //}
            //DeadBodyVehicle.WasSpawnedEmpty = false;
            //DeadBodyVehicle.IsManualCleanup = false;
            //DeadBodyVehicle.Vehicle.IsPersistent = false;
            //DeadBodyVehicle.Vehicle.LockStatus = (VehicleLockStatus)10;
        }
        protected override void OnTaskCompletedOrFailed()//called on failed and disposed?
        {
            CleanupPed();
            CleanupCar();
            if (DealingLocation != null)
            {
                DealingLocation.IsPlayerInterestedInLocation = false;
            }
        }

    }
}
