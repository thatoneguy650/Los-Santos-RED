using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

public class VehicleExporter : GameLocation
{
    private UIMenu ExportSubMenu;
    private bool HasExported;
    private UIMenu ExportListSubMenu;
    private List<MenuVehicleMap> MenuLookups = new List<MenuVehicleMap>();
    private List<ExportedVehicle> ExportedVehicles = new List<ExportedVehicle>();
    public VehicleExporter() : base()
    {

    }
    public override string TypeName { get; set; } = "Vehicle Exporter";
    public override int MapIcon { get; set; } = 794;// (int)123;
    public override string ButtonPromptText { get; set; }
    public override bool ShowsOnDirectory => false;
    public float VehiclePickupDistance { get; set; } = 25f;
    public int BodyDamageLimit { get; set; } = 200;
    public int EngineDamageLimit { get; set; } = 200;
    public string ContactName { get; set; } = "";
    public int HoursBetweenExports { get; set; } = 3;
    public List<SpawnPlace> ParkingSpaces { get; set; } = new List<SpawnPlace>();
    [XmlIgnore]
    public PhoneContact PhoneContact { get; set; }
    public VehicleExporter(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string _Menu) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        MenuID = _Menu;
        ExportedVehicles = new List<ExportedVehicle>();
    }
    public override void Reset()
    {
        ExportedVehicles.Clear();
        base.Reset();
    }
    public override void StoreData(IShopMenus shopMenus, IAgencies agencies, IGangs gangs, IZones zones, IJurisdictions jurisdictions, IGangTerritories gangTerritories, INameProvideable names, ICrimes crimes,
    IPedGroups PedGroups, IEntityProvideable world, IStreets streets, ILocationTypes locationTypes, ISettingsProvideable settings, IPlateTypes plateTypes, IOrganizations associations, IContacts contacts, IInteriors interiors,
        ILocationInteractable player, IModItems modItems, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest, IIssuableWeapons issuableWeapons, IHeads heads, IDispatchablePeople dispatchablePeople)
    {
        PhoneContact = contacts.GetContactData(ContactName);
        base.StoreData(shopMenus, agencies, gangs, zones, jurisdictions, gangTerritories, names, crimes, PedGroups, world, streets, locationTypes, settings, plateTypes, associations, contacts, interiors, player, modItems, weapons, time, placesOfInterest, issuableWeapons,heads,dispatchablePeople);
    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Export Vehicle At {Name}";
        return true;
    }
    public override void OnInteract()//ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest)
    {
        //Player = player;
        //ModItems = modItems;
        //World = world;
        //Settings = settings;
        //Weapons = weapons;
        //Time = time;
        if (IsLocationClosed())
        {
            return;
        }
        if (!CanInteract)
        {
            return;
        }
        if (Interior != null && Interior.IsTeleportEntry)
        {
            DoEntranceCamera(true);
            Interior.Teleport(Player, this, StoreCamera);
        }
        else
        {
            StandardInteract(null, false);
        }
    }
    protected override bool ShouldSpawnVendor() => PhoneContact == null || !Player.RelationshipManager.GetOrCreate(PhoneContact).IsHostile;
    protected override bool IsLocationClosed()
    {
        if (PhoneContact != null && Player.RelationshipManager.GetOrCreate(PhoneContact).IsHostile)
        {
            Game.DisplayHelp("Increase your reputation to access.");
            return true;
        }
        return base.IsLocationClosed();
    }

    public override void StandardInteract(LocationCamera locationCamera, bool isInside)
    {
        Player.ActivityManager.IsInteractingWithLocation = true;
        CanInteract = false;
        Player.IsTransacting = true;
        GameFiber.StartNew(delegate
        {
            try
            {
                SetupLocationCamera(locationCamera, isInside, true);
                CreateInteractionMenu();
                InteractionMenu.Visible = true;
                GenerateExportMenu();
                ProcessInteractionMenu();
                if (HasExported)
                {
                    Organization Association = Associations.GetOrganizationByContact(ContactName);
                    EntryPoint.WriteToConsole($"ContactName {ContactName} NO Association FOUND: {Association == null}");
                    if (Association != null && Association.PhoneContact != null)
                    {
                        EntryPoint.WriteToConsole($"Association.PhoneContact.Name: {Association.PhoneContact.Name}");
                        Player.CellPhone.AddContact(Association.PhoneContact, true);
                    }
                    //Player.CellPhone.AddContact(new VehicleExporterContact(ContactName), true);
                }
                DisposeInteractionMenu();
                DisposeCamera(isInside);
                DisposeInterior();
                Player.ActivityManager.IsInteractingWithLocation = false;
                CanInteract = true;
                Player.IsTransacting = false;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "VehicleExporterInteract");
    }
    public void AddPriceListItems(UIMenu toAdd, IModItems modItems)
    {
        foreach (MenuItem menuItem1 in Menu.Items.OrderBy(x => x.SalesPrice))
        {
            if (menuItem1.ModItem == null)
            {
                continue;
            }
            VehicleItem vehicleItem = modItems.PossibleItems.VehicleItems.Where(x => x.Name == menuItem1.ModItem?.Name).FirstOrDefault();
            if (vehicleItem != null && vehicleItem.IsDLC && !Settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles)
            {
                continue;
            }
            UIMenuItem vehicleToExportItem = new UIMenuItem(menuItem1.ModItem.DisplayName, menuItem1.ModItem.FullDescription(Settings)) { RightLabel = menuItem1.SalesPrice.ToString("C0") };
            toAdd.AddItem(vehicleToExportItem);
        }
    }
    private void GenerateExportMenu()
    {
        HasExported = false;
        ExportListSubMenu = MenuPool.AddSubMenu(InteractionMenu, "List Exportable Vehicles");
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = "Get a list of exportable vehicles. Exported vehicles need to be near mint condition.";
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Car;
        if (HasBannerImage)
        {
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BannerImagePath}");
            ExportListSubMenu.SetBannerType(BannerImage);
        }

        AddPriceListItems(ExportListSubMenu, ModItems);
        MenuLookups.Clear();
        ExportSubMenu = MenuPool.AddSubMenu(InteractionMenu, "Export A Vehicle");
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = "Select a vehicle to export.";
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Car;
        if (HasBannerImage)
        {
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BannerImagePath}");
            ExportSubMenu.SetBannerType(BannerImage);
        }
        ExportSubMenu.OnIndexChange += ExportSubMenu_OnIndexChange;
        ExportSubMenu.OnMenuOpen += ExportSubMenu_OnMenuOpen;
        ExportSubMenu.OnMenuClose += ExportSubMenu_OnMenuClose;
        foreach (VehicleExt veh in World.Vehicles.AllVehicleList)
        {
            if (!IsValidForExporting(veh))
            {
                continue;
            }
            VehicleItem vehicleItem = ModItems.GetVehicle(veh.Vehicle.Model.Name);
            if (vehicleItem == null)
            {
                vehicleItem = ModItems.GetVehicle(veh.Vehicle.Model.Hash);
            }
            if (vehicleItem == null)
            {
                continue;
            }
            string CarName = veh.GetCarName();
            bool CanExport = false;
            bool IsDamaged = false;
            int ExportAmount = 0;
            MenuItem menuItem = Menu.Items.FirstOrDefault(x => x.ModItemName == vehicleItem.Name);
            ExportedVehicle exportedStats = null;
            if (menuItem != null)
            {
                CanExport = true;
                ExportAmount = menuItem.SalesPrice;
                exportedStats = ExportedVehicles?.FirstOrDefault(x => x.MenuItem?.ModItemName == menuItem?.ModItemName);
            }    
            string TimeBeforeExportAllowed = "";
            bool hasTimeRestriction = false; 
            if(exportedStats != null)
            {
                TimeBeforeExportAllowed = exportedStats.TimeLastExported.AddHours(HoursBetweenExports).ToString("dd MMM yyyy hh:mm tt");
                if (DateTime.Compare(Time.CurrentDateTime,exportedStats.TimeLastExported.AddHours(HoursBetweenExports)) < 0)//DateTime.Compare(Time.CurrentDateTime, residence.DateRentalPaymentDue) >= 0
                {
                    CanExport = false;
                }
                hasTimeRestriction = true;    
            }
            if(veh.IsDamaged(BodyDamageLimit, EngineDamageLimit))
            {
                IsDamaged = true;
                CanExport = false;
            }
            UIMenuItem vehicleCrusherItem = new UIMenuItem(CarName, veh.GetCarDescription()) { RightLabel = ExportAmount.ToString("C0") };
            MenuLookups.Add(new MenuVehicleMap(menuItem,vehicleCrusherItem, veh));
            if (!CanExport)
            {
                vehicleCrusherItem.Enabled = false;
                vehicleCrusherItem.RightLabel = "";
            }
            if(IsDamaged)
            {
                vehicleCrusherItem.Description += "~n~~r~TOO DAMAGED TO EXPORT~s~";
            }
            if(hasTimeRestriction && !CanExport)
            {
                vehicleCrusherItem.Description += $"~n~Next Available Drop Off: {TimeBeforeExportAllowed}";
            }
            vehicleCrusherItem.Activated += (sender, e) =>
            {
                ExportVehicle(veh, 1 * ExportAmount, menuItem, vehicleCrusherItem);
            };

            ExportSubMenu.AddItem(vehicleCrusherItem);
        }
    }
    private void ExportSubMenu_OnMenuClose(UIMenu sender)
    {
        StoreCamera.ReHighlightStoreWithCamera();
    }
    private void ExportSubMenu_OnMenuOpen(UIMenu sender)
    {
        ExportSubMenu_OnIndexChange(sender, sender.CurrentSelection);
    }
    private void ExportSubMenu_OnIndexChange(UIMenu sender, int newIndex)
    {
        if(sender == null && sender.MenuItems == null || !sender.MenuItems.Any() || newIndex == -1)
        {
            return;
        }
        UIMenuItem coolmen = sender.MenuItems[newIndex];
        MenuVehicleMap lookupTuple = MenuLookups.FirstOrDefault(x=> x.UIMenuItem == coolmen);
        if(lookupTuple == null || lookupTuple.VehicleExt == null || !lookupTuple.VehicleExt.Vehicle.Exists())
        {
            return;
        }
        StoreCamera.HighlightEntity(lookupTuple.VehicleExt.Vehicle);        
    }
    private void ExportVehicle(VehicleExt toExport, int Price, MenuItem menuItem, UIMenuItem vehicleCrusherItem)
    {
        if (toExport == null || !toExport.Vehicle.Exists() || toExport.VehicleBodyManager.StoredBodies.Any() || toExport.Vehicle.HasOccupants || !toExport.HasBeenEnteredByPlayer)
        {
            PlayErrorSound();
            DisplayMessage("~r~Exporting Failed", "We are unable to complete this export.");
            return;
        }
        Game.FadeScreenOut(1000, true);
        string CarName = toExport.GetCarName();
       // toExport.WasCrushed = true;
        toExport.Vehicle.Delete();
        ExportSubMenu.MenuItems.Remove(vehicleCrusherItem);
        ExportSubMenu.RefreshIndex();
        ExportSubMenu.Close(true);
        Game.FadeScreenIn(1000, true);
        Player.BankAccounts.GiveMoney(Price, false);
        PlaySuccessSound();
        DisplayMessage("~g~Exported", $"Thank you for exporting ~p~{CarName}~s~ at ~y~{Name}~s~");
        HasExported = true;
        AddToExportedList(menuItem);
        UpdateMenuRestrictions();
    }
    private void AddToExportedList(MenuItem menuItem)
    {
        ExportedVehicle current = ExportedVehicles.Where(x => x.MenuItem.ModItemName == menuItem.ModItemName).FirstOrDefault();
        if(current == null)
        {
            current = new ExportedVehicle(menuItem, 1, Time.CurrentDateTime);
            ExportedVehicles.Add(current);
        }
        else
        {
            current.TimeLastExported = Time.CurrentDateTime;
            current.TotalExports++;
        }
    }
    private bool IsValidForExporting(VehicleExt toExport)
    {
        if (!toExport.Vehicle.Exists() || toExport.Vehicle.DistanceTo2D(EntrancePosition) > VehiclePickupDistance || toExport.VehicleBodyManager.StoredBodies.Any() || toExport.Vehicle.HasOccupants || !toExport.HasBeenEnteredByPlayer || !toExport.CanBeExported)
        {
            return false;
        }
        return true;
    }
    private void UpdateMenuRestrictions()
    {
        foreach(MenuVehicleMap test in MenuLookups)
        {
            string TimeBeforeExportAllowed = "";
            bool CanExport = true;
            ExportedVehicle exportedStats = ExportedVehicles?.FirstOrDefault(x => x.MenuItem?.ModItemName == test.MenuItem?.ModItemName);
            if(exportedStats != null)
            {
                TimeBeforeExportAllowed = exportedStats.TimeLastExported.AddHours(HoursBetweenExports).ToString("dd MMM yyyy hh:mm tt");
                if (DateTime.Compare(Time.CurrentDateTime, exportedStats.TimeLastExported.AddHours(HoursBetweenExports)) < 0)
                {
                    CanExport = false;
                }
            }

            if (!CanExport && test.UIMenuItem.Enabled)
            {
                test.UIMenuItem.Enabled = false;
                test.UIMenuItem.Description += $"~n~Next Export Time: {TimeBeforeExportAllowed}";
            }
        }
    }
    public override void AddDistanceOffset(Vector3 offsetToAdd)
    {
        foreach (SpawnPlace sp in ParkingSpaces)
        {
            sp.AddDistanceOffset(offsetToAdd);
        }
        base.AddDistanceOffset(offsetToAdd);
    }
    public string GenerateTextItem(VehicleItem vehicleItem)
    {
        if(vehicleItem == null)
        {
            return "";
        }
        EntryPoint.WriteToConsole($"GenerateTextItem vehicleItem.Name:{vehicleItem.Name}");
        MenuItem mi = Menu.Items.FirstOrDefault(x => x.ModItemName == vehicleItem.Name);
        if (mi == null)
        {
            return "";
        }
        EntryPoint.WriteToConsole($"GenerateTextItem menuItem:{mi.ModItemName}");
        bool CanExport = true;
        ExportedVehicle exportedStats = ExportedVehicles.FirstOrDefault(x => x.MenuItem?.ModItemName == vehicleItem?.Name);
        string TimeBeforeExportAllowed = "";
        if (exportedStats != null && DateTime.Compare(Time.CurrentDateTime, exportedStats.TimeLastExported.AddHours(HoursBetweenExports)) < 0)
        {
            CanExport = false;
            TimeBeforeExportAllowed = exportedStats.TimeLastExported.AddHours(HoursBetweenExports).ToString("hh:mm tt");
        } 
        string finalString = $"{Name} - {mi.SalesPrice.ToString("C0")}";
        if(!CanExport)
        {
            finalString += $" - ~r~Next Export Time: {TimeBeforeExportAllowed}~s~";
        }
        finalString += "~n~";
        return finalString;
    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.VehicleExporters.Add(this);
        base.AddLocation(possibleLocations);
    }
}

