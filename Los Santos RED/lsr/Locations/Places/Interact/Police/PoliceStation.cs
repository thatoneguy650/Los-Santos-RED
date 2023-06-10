using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Mod;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class PoliceStation : GameLocation, ILocationRespawnable, ILicensePlatePreviewable, ILocationImpoundable, ILocationAreaRestrictable
{
    private ShopMenu agencyMenu;
    private UIMenu ImpoundSubMenu;
    public PoliceStation(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {

    }
    public PoliceStation() : base()
    {

    }
    public string LicensePlatePreviewText { get; set; } = "UNIT1";
    public override string TypeName { get; set; } = "Police Station";
    public override int MapIcon { get; set; } = (int)BlipSprite.PoliceStation;
    public Vector3 RespawnLocation { get; set; }
    public float RespawnHeading { get; set; }
    public Vector3 ItemPreviewPosition { get; set; } = Vector3.Zero;
    public float ItemPreviewHeading { get; set; } = 0f;
    public VehicleImpoundLot VehicleImpoundLot { get; set; }
    public bool HasImpoundLot => VehicleImpoundLot != null;
    public List<SpawnPlace> ItemDeliveryLocations { get; set; } = new List<SpawnPlace>();
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Enter {Name}";
        return true;
    }
    public override void StoreData(IShopMenus shopMenus, IAgencies agencies, IGangs gangs, IZones zones, IJurisdictions jurisdictions, IGangTerritories gangTerritories, INameProvideable Names, ICrimes Crimes, IPedGroups PedGroups, IEntityProvideable world, 
        IStreets streets, ILocationTypes locationTypes, ISettingsProvideable settings)
    {
        base.StoreData(shopMenus, agencies, gangs, zones, jurisdictions, gangTerritories, Names, Crimes, PedGroups, world, streets,locationTypes, settings);
        VehicleImpoundLot?.Setup(this);
        if (AssignedAgency == null)
        {
            AssignedAgency = zones.GetZone(EntrancePosition)?.AssignedLEAgency;
        }
    }
    public override void OnInteract(ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        ModItems = modItems;
        World = world;
        Settings = settings;
        Weapons = weapons;
        Time = time;
        if (IsLocationClosed())
        {
            return;
        }
        if (!CanInteract)
        {

            return;        
        }
        if(AssignedAgency == null)
        {
            Game.DisplayHelp("No Agency Assigned");
            return;
        }
        Player.ActivityManager.IsInteractingWithLocation = true;
        CanInteract = false;
        Player.IsTransacting = true;
        GameFiber.StartNew(delegate
        {
            try
            {
                StoreCamera = new LocationCamera(this, Player);
                StoreCamera.ItemPreviewPosition = ItemPreviewPosition;
                StoreCamera.ItemPreviewHeading = ItemPreviewHeading;

                if (!Player.IsCop)
                {
                    StoreCamera.ForceRegularCamera = true;
                }
                StoreCamera.Setup();         
                CreateInteractionMenu();
                if(Player.IsCop)
                {
                    GenerateMenu();
                    InteractAsCop(modItems,world,settings,weapons,time);
                }
                else
                {
                    InteractAsOther();
                }
                DisposeInteractionMenu();
                StoreCamera.Dispose();
                Player.ActivityManager.IsInteractingWithLocation = false;
                Player.IsTransacting = false;
                CanInteract = true;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "PoliceStationInteract");
    }

    private void InteractAsCop(IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time)
    {
        Transaction = new Transaction(MenuPool, InteractionMenu, agencyMenu, this);
        Transaction.LicensePlatePreviewable = this;


        if((ItemDeliveryLocations == null || !ItemDeliveryLocations.Any()) && PossibleVehicleSpawns?.Any() == true)
        {
            List<SpawnPlace> places = new List<SpawnPlace>();
            foreach(ConditionalLocation place in PossibleVehicleSpawns)
            {
                places.Add(new SpawnPlace(place.Location, place.Heading));
            }
            Transaction.ItemDeliveryLocations = places;
        }
        else
        {
            Transaction.ItemDeliveryLocations = ItemDeliveryLocations;
        }

        
        Transaction.ItemPreviewPosition = ItemPreviewPosition;
        Transaction.ItemPreviewHeading = ItemPreviewHeading;
        Transaction.IsFreeItems = true;
        Transaction.IsFreeWeapons = true;
        Transaction.IsFreeVehicles = true;
        Transaction.IsPurchasing = false;
        Transaction.RotatePreview = false;
        Transaction.CreateTransactionMenu(Player, modItems, world, settings, weapons, time);

        InteractionMenu.Visible = true;
        InteractionMenu.OnItemSelect += InteractionMenu_OnItemSelect;
        Transaction.ProcessTransactionMenu();

        Transaction.DisposeTransactionMenu();
    }
    private void InteractAsOther()
    {
        UIMenuItem addComplaintMenu = new UIMenuItem("File Complaint","File a complaint about the conduct of the officers. After all, you pay their salary!");
        addComplaintMenu.Activated += (sender,selectedItem) =>
        {      
            InteractionMenu.Visible = false;
            Game.DisplaySubtitle("Go Fuck Yourself Prick.");
            Player.SetAngeredCop();
        };
        InteractionMenu.AddItem(addComplaintMenu);


        UIMenuItem PayBailFees = new UIMenuItem("Pay Bail Fees", "Pay your outstanding bail fees.") { RightLabel = $"${Player.Respawning.PastDueBailFees}" };
        PayBailFees.Activated += (sender, selectedItem) =>
        {
            if (Player.BankAccounts.Money <= Player.Respawning.PastDueBailFees)
            {
                new GTANotification(Name, "~r~Insufficient Funds", "We are sorry, we are unable to complete this transaction.").Display();
                NativeHelper.PlayErrorSound();
                return;
            }
            Player.BankAccounts.GiveMoney(-1 * Player.Respawning.PastDueBailFees);
            new GTANotification(Name, "~g~Accepted", $"Your bail fees have been paid.").Display();
            Player.Respawning.PayPastDueBail();
            PayBailFees.Enabled = Player.Respawning.PastDueBailFees > 0;
        };
        PayBailFees.Enabled = Player.Respawning.PastDueBailFees > 0;
        InteractionMenu.AddItem(PayBailFees);

        List<VehicleExt> ImpoundedVehicles = Player.VehicleOwnership.OwnedVehicles.Where(x => x.IsImpounded && x.Vehicle.Exists() && x.ImpoundedLocation == Name).ToList();// x.Vehicle.DistanceTo2D(EntrancePosition) <= 300f).ToList();
        if (HasImpoundLot && ImpoundedVehicles.Any())
        {       
            ImpoundSubMenu = MenuPool.AddSubMenu(InteractionMenu, "Impounded Vehicles");
            if (HasBannerImage)
            {
                ImpoundSubMenu.SetBannerType(BannerImage);
            }
            foreach (VehicleExt impoundedVehicle in ImpoundedVehicles)
            {
                EntryPoint.WriteToConsole("ADDING VEHICLE TO IMPOUND MENU");
                impoundedVehicle.AddToImpoundMenu(this,ImpoundSubMenu, Player, Time);
            }
        }
       
        InteractionMenu.Visible = true;
        InteractionMenu.OnItemSelect += InteractionMenu_OnItemSelect;
        ProcessInteractionMenu();
    }
    private void GenerateMenu()
    {
        List<MenuItem> menuItems = new List<MenuItem>();
        //Convert Dispatchable Vehicles to ShopMenu
        foreach (DispatchableVehicle dv in AssignedAgency.Vehicles)
        {
            VehicleItem vehicleItem = ModItems.GetVehicle(dv.ModelName);
            if (vehicleItem == null)
            {
                continue;
            }
            EntryPoint.WriteToConsole($"ADDED {dv.ModelName} TO MENU");
            MenuItem existingMenuItem = new MenuItem(vehicleItem.Name, 0, -1) { ModItem = vehicleItem };
            existingMenuItem.SetFree();
            menuItems.Add(existingMenuItem);
        }
        //Convert Issuable Weapons to ShopMenu
        List<IssuableWeapon> AllWeapons = new List<IssuableWeapon>();
        AllWeapons.AddRange(AssignedAgency.LessLethalWeapons);
        AllWeapons.AddRange(AssignedAgency.SideArms);
        AllWeapons.AddRange(AssignedAgency.LongGuns);
        foreach (IssuableWeapon issuableWeapon in AllWeapons)
        {
            WeaponItem weaponItem = ModItems.GetWeapon(issuableWeapon.ModelName);
            if(weaponItem == null)
            {
                continue;
            }
            MenuItem existingMenuItem = menuItems.FirstOrDefault(x => x.ModItemName == weaponItem.Name);
            if (existingMenuItem == null)
            {
                existingMenuItem = new MenuItem(weaponItem.Name, 0, -1) { SubPrice = 0, ModItem = weaponItem };
                existingMenuItem.SetFree();
                menuItems.Add(existingMenuItem);
            }
            foreach(WeaponComponent stuff in issuableWeapon.Variation?.Components)
            {
                if(!existingMenuItem.Extras.Any(x=> x.ExtraName == stuff.Name))
                {
                    existingMenuItem.Extras.Add(new MenuItemExtra(stuff.Name, 0));
                }
            }
            EntryPoint.WriteToConsole($"ADDED {issuableWeapon.ModelName} TO MENU");
        }
        agencyMenu = new ShopMenu(AssignedAgency.ID + "Menu", AssignedAgency.ID + "Menu", menuItems);
    }
    private void InteractionMenu_OnItemSelect(RAGENativeUI.UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem.Text == "Buy" || selectedItem.Text == "Take")
        {
            Transaction?.SellMenu?.Dispose();
            Transaction?.PurchaseMenu?.Show();
        }
        else if (selectedItem.Text == "Sell")
        {
            Transaction?.PurchaseMenu?.Dispose();
            Transaction?.SellMenu?.Show();
        }
    }
    public override void AddDistanceOffset(Vector3 offsetToAdd)
    {
        if (RespawnLocation != Vector3.Zero)
        {
            RespawnLocation += offsetToAdd;
        }
        VehicleImpoundLot?.AddDistanceOffset(offsetToAdd);
        base.AddDistanceOffset(offsetToAdd);
    }
}

