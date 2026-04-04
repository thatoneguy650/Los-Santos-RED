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
        private GameLocation DealingLocation;
        private bool HasArrivedNearMeetup;
        private bool HasCompletedTransaction;
        private List<GangMember> SpawnedMembers = new List<GangMember>();
        private GangMember PrimaryGangMember;
        private bool IsAmbush = false;
        private bool HasSetViolent = false;
        private int UnitPrice;

        public bool IsPlayerSellingDrugs { get; set; } = true;

        public GangDrugMeetTask(ITaskAssignable player, ITimeReportable time, IGangs gangs, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, IEntityProvideable world,
            ICrimes crimes, IWeapons weapons, INameProvideable names, IPedGroups pedGroups, IShopMenus shopMenus, IModItems modItems, PlayerTasks playerTasks, GangTasks gangTasks, 
            PhoneContact hiringContact, Gang hiringGang, ModItem modItem, int quantity, Gang dealingGang, GameLocation dealingLocation) : base(player, time, gangs, placesOfInterest, settings, world, crimes, weapons, names, pedGroups, shopMenus, modItems, playerTasks, gangTasks, hiringContact, hiringGang)
        {
            DebugName = "Drug Meet";
            RepOnCompletion = 200;
            DebtOnFail = 0;
            RepOnFail = -100;
            DaysToComplete = 2;
            ModItem = modItem;
            Quantity = quantity;
            DealingGang = dealingGang;
            DealingLocation = dealingLocation;
        }
        public override void Dispose()
        {
            CleanupPeds();
            if (DealingLocation != null)
            {
               
                DealingLocation.IsPlayerInterestedInLocation = false;
            }
            base.Dispose();
        }
        protected override void GetPayment()
        {
            PaymentAmount = 0;
        }
        protected override void SendInitialInstructionsMessage()
        {
            List<string> Replies = new List<string>() { };

            if(IsPlayerSellingDrugs)
            {
                Replies.Add($"Meet set with {DealingGang.ColorPrefix}{DealingGang.ShortName}~s~. They will be around {DealingLocation.FullStreetAddress} with {Quantity} {ModItem.MeasurementName} of {ModItem.Name}. Should be ${UnitPrice * Quantity} to you.");
            }
            else
            {
                Replies.Add($"Meet set with {DealingGang.ColorPrefix}{DealingGang.ShortName}~s~. They will be around {DealingLocation.FullStreetAddress} with ${UnitPrice * Quantity} to buy {Quantity} {ModItem.MeasurementName} of {ModItem.Name}.");
            }
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
            //DealingLocation = PlacesOfInterest.PossibleLocations.IllicitMarketplaces.Where(x => x.IsCorrectMap(World.IsMPMapLoaded) && x.IsSameState(Player.CurrentLocation?.CurrentZone?.GameState)).PickRandom();
            if (DealingLocation == null)
            {
                return false;
            }
            GangReputation gr = Player.RelationshipManager.GangRelationships.GetReputation(DealingGang);
            if(gr.IsEnemy || gr.GangRelationship == GangRespect.Hostile)
            {
                IsAmbush = true;
            }
            else if (gr.IsMember)
            {
                return false;
            }
            else if (gr.GangRelationship == GangRespect.Neutral)
            {
                IsAmbush = RandomItems.RandomPercent(Settings.SettingsManager.TaskSettings.DrugMeetAmbushPercentageNeutral);
            }
            else if(gr.GangRelationship == GangRespect.Friendly)
            {
                IsAmbush = RandomItems.RandomPercent(Settings.SettingsManager.TaskSettings.DrugMeetAmbushPercentageFriendly);
            }
            if (IsPlayerSellingDrugs)
            {
                UnitPrice = (int)Math.Round(ShopMenus.GetAverageStreetSalesPrice(ModItem) * RandomItems.GetRandomNumber(Settings.SettingsManager.TaskSettings.DrugMeetPriceScalarMin, Settings.SettingsManager.TaskSettings.DrugMeetPriceScalarMax));
            }
            else
            {
                UnitPrice = (int)Math.Round(ShopMenus.GetAverageStreetPurchasePrice(ModItem) * RandomItems.GetRandomNumber(Settings.SettingsManager.TaskSettings.DrugMeetPriceScalarMin, Settings.SettingsManager.TaskSettings.DrugMeetPriceScalarMax));
            }
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
                if(!HasArrivedNearMeetup && DealingLocation.DistanceToPlayer<= 200f && !HasSetViolent)
                {
                    OnArrivedNearMeetup();
                }
                if(HasArrivedNearMeetup && DealingLocation.DistanceToPlayer >= 275f && !HasSetViolent)
                {
                    OnWentAwayFromMeetup();
                }




                if (!IsAmbush && HasArrivedNearMeetup && SpawnedMembers.Any(x => x.WasKilledByPlayer || x.HasBeenHurtByPlayer))
                {
                    CurrentTask.OnReadyForPayment(false);
                    break;
                }

                if (!IsAmbush && HasCompletedSale())
                {
                    CurrentTask.OnReadyForPayment(false);
                    break;
                }



                if(IsAmbush && HasSetViolent && (DealingLocation.DistanceToPlayer >= 275f || SpawnedMembers.All(x => x.IsDead || x.IsUnconscious || !x.Pedestrian.Exists())))
                {
                    CurrentTask.OnReadyForPayment(false);
                    break;
                }
                if (IsAmbush && HasArrivedNearMeetup && !HasSetViolent && SpawnedMembers.Any(x=> x.PlayerPerception.CanRecognizeTarget || x.HasSeenPlayerCommitCrime || (x.CanSeePlayer && x.DistanceToPlayer <= 45f))) // PrimaryGangMember != null && PrimaryGangMember.PlayerPerception.CanRecognizeTarget)
                {
                    HasSetViolent = true;
                    OnSetGangMembersViolent();
                }
                
                GameFiber.Sleep(1000);
            }
        }

        private void OnSetGangMembersViolent()
        {
            foreach (GangMember gm in SpawnedMembers)
            {
                gm.IsHitSquad = true;
                gm.WillFight = true;
                gm.WillAlwaysFightPolice = true;
                gm.WillFightPolice = true;
            }
            Player.Dispatcher.GangDispatcher.DispatchHitSquad(DealingGang, true);    
            Game.DisplaySubtitle("Ambush, get out of there");      
            EntryPoint.WriteToConsole("DRUG MEETUP SET GANG MEMBERS VIOLENT! THEY RECOGNIZE YOU");
        }

        protected override void FinishTask()
        {
            if (CurrentTask != null && CurrentTask.IsActive && CurrentTask.IsReadyForPayment)
            {
                PlayerTasks.CompleteTask(HiringContact, true);
                OnTaskCompletedOrFailed();
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
        private bool HasCompletedSale()
        {
            if(PrimaryGangMember == null)
            {
                return false;
            }
            if(!PrimaryGangMember.Pedestrian.Exists())
            {
                return false;
            }
            if (IsPlayerSellingDrugs)
            {
                DesiredItem di = PrimaryGangMember.ItemDesires?.Get(ModItem);
                if (di != null && Quantity == di.ItemsPurchasedFromPlayer)
                {
                    EntryPoint.WriteToConsole($"YOU HAVE COMPLETED THE SALE Quantity{Quantity} ItemsPurchasedFromPlayer{di.ItemsPurchasedFromPlayer}");
                    return true;
                }
            }
            else
            {
                DesiredItem di = PrimaryGangMember.ItemDesires?.Get(ModItem);
                if (di != null && Quantity == di.ItemsSoldToPlayer)
                {
                    EntryPoint.WriteToConsole($"YOU HAVE COMPLETED THE SALE Quantity{Quantity} ItemsSoldToPlayer{di.ItemsSoldToPlayer}");
                    return true;
                }
            }
            return false;
        }
        private void OnWentAwayFromMeetup()
        {
            EntryPoint.WriteToConsole("DRUG MEETUP PLAYER IS WENT AWAY FROM LOCATION");
            HasArrivedNearMeetup = false;
            CleanupPeds();
        }
        private void OnArrivedNearMeetup()
        {
            EntryPoint.WriteToConsole($"DRUG MEETUP PLAYER IS NEARBY LOCATION {IsAmbush}");
            HasArrivedNearMeetup = true;
            if(HasCompletedTransaction || HasSetViolent)
            {
                return;
            }
            SpawnDealers();
        }
        private void SpawnDealers()
        {
            EntryPoint.WriteToConsole("DRUG MEETUP START SPAWN DEALERS");
            SpawnLocation spawnLocation = new SpawnLocation(DealingLocation.EntrancePosition);
            //spawnLocation.GetClosestStreet(false);
            //spawnLocation.GetClosestSidewalk(); ;


            spawnLocation.StreetPosition = DealingLocation.EntrancePosition;
            spawnLocation.Heading = DealingLocation.EntranceHeading;

            GangSpawnTask gangSpawnTask = new GangSpawnTask(DealingGang, spawnLocation,null,DealingGang.GetRandomPed(0,""), Settings.SettingsManager.GangSettings.ShowSpawnedBlip, Settings,Weapons,Names,false,Crimes,PedGroups,ShopMenus,World,ModItems,true,true,true);
            gangSpawnTask.PlacePedOnGround = true;
            gangSpawnTask.AllowAnySpawn = true;
            gangSpawnTask.AllowBuddySpawn = true;
            gangSpawnTask.IsHitSquad = IsAmbush;
            gangSpawnTask.SpawnRequirement = TaskRequirements.Guard;
            gangSpawnTask.AttemptSpawn();
            PrimaryGangMember = (GangMember)gangSpawnTask.CreatedPeople.FirstOrDefault();
            if(PrimaryGangMember == null)
            {
                return;
            }

            if (!IsAmbush)
            {
                EntryPoint.WriteToConsole($"DRUG MEETUP SPAWNED DEALERS {PrimaryGangMember.Handle} {ModItem.Name} BuyingUnitPrice:{UnitPrice}");
                if (IsPlayerSellingDrugs)
                {
                    PrimaryGangMember.SetupTransactionItems(new ShopMenu("testmenu", "testmenu2", new List<MenuItem>() { new MenuItem(ModItem.Name, -1, UnitPrice) {
                    ModItem = ModItem,
                    IsIllicilt = true,
                    NumberOfItemsToPurchaseFromPlayer = Quantity,
                    NumberOfItemsToSellToPlayer = 0 } }), true);
                    PrimaryGangMember.Money = UnitPrice * Quantity;//make sure he has enough on him to buy it
                }
                else
                {
                    PrimaryGangMember.SetupTransactionItems(new ShopMenu("testmenu", "testmenu2", new List<MenuItem>() { new MenuItem(ModItem.Name, UnitPrice, -1) {
                    ModItem = ModItem,
                    IsIllicilt = true,
                    NumberOfItemsToPurchaseFromPlayer = 0,
                    NumberOfItemsToSellToPlayer = Quantity } }), true);
                }
            }
            SpawnedMembers = new List<GangMember>();
            foreach (GangMember gm in gangSpawnTask.CreatedPeople)
            {
                gm.IsHitSquad = false;
                SpawnedMembers.Add(gm);
            }


            List<SpawnLocation> OtherSpawnLocations = new List<SpawnLocation>();
            foreach(ConditionalLocation conditionalLocation in DealingLocation?.PossiblePedSpawns)
            {
                OtherSpawnLocations.Add(new SpawnLocation(conditionalLocation.Location, conditionalLocation.Heading));
            }
            foreach (ConditionalGroup conditionalGroup in DealingLocation?.PossibleGroupSpawns)
            {
                foreach (ConditionalLocation conditionalLocation in conditionalGroup?.PossiblePedSpawns)
                {
                    OtherSpawnLocations.Add(new SpawnLocation(conditionalLocation.Location, conditionalLocation.Heading));
                }
            }
            if(!OtherSpawnLocations.Any())// && IsAmbush)
            {
                SpawnLocation fallbackspawnLocation = new SpawnLocation(DealingLocation.EntrancePosition.Around2D(50f));
                fallbackspawnLocation.GetClosestStreet(false);
                fallbackspawnLocation.GetClosestSidewalk();
                OtherSpawnLocations.Add(fallbackspawnLocation);
            }

            foreach (SpawnLocation otherspawnLocation in OtherSpawnLocations)
            {
                if(!RandomItems.RandomPercent(Settings.SettingsManager.TaskSettings.DugMeetContactGangBackupSpawnPercentage))
                {
                    continue;
                }
                GangSpawnTask gangAdditionalSpawnTask = new GangSpawnTask(DealingGang, otherspawnLocation, null, DealingGang.GetRandomPed(0, ""), Settings.SettingsManager.GangSettings.ShowSpawnedBlip, Settings, Weapons, Names, false, Crimes, PedGroups, ShopMenus, World, ModItems, true, true, true);
                gangAdditionalSpawnTask.PlacePedOnGround = true;
                gangAdditionalSpawnTask.AllowAnySpawn = true;
                gangAdditionalSpawnTask.AllowBuddySpawn = true;
                gangAdditionalSpawnTask.IsHitSquad = IsAmbush;
                gangAdditionalSpawnTask.SpawnRequirement = TaskRequirements.Guard;
                gangAdditionalSpawnTask.AttemptSpawn();
                foreach (GangMember gm in gangAdditionalSpawnTask.CreatedPeople)
                {
                    gm.IsHitSquad = false;
                    SpawnedMembers.Add(gm);
                }
                EntryPoint.WriteToConsole("SPAWNED ADDITIONAL PEDS");
            }
        }
        private void CleanupPeds()
        {
            foreach(GangMember gm in SpawnedMembers)
            {
                if(gm.Pedestrian.Exists() && gm.DistanceToPlayer >= 200f)
                {
                    gm.Pedestrian.IsPersistent = false;
                }
            }
        }
        protected override void OnTaskCompletedOrFailed()//called on failed and disposed?
        {
            CleanupPeds();
            if (DealingLocation != null)
            {
                DealingLocation.IsPlayerInterestedInLocation = false;
            }
        }
    }
}
