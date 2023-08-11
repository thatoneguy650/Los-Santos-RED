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
using System.Xml.Serialization;

public class VehicleExporter : GameLocation
{
    private UIMenu ExportSubMenu;
    private bool HasExported;
    private UIMenu ExportListSubMenu;
    private List<Tuple<UIMenuItem, VehicleExt>> MenuLookups = new List<Tuple<UIMenuItem, VehicleExt>>();
    public VehicleExporter() : base()
    {

    }
    public override string TypeName { get; set; } = "Vehicle Exporter";
    public override int MapIcon { get; set; } = (int)123;
    public override float MapIconScale { get; set; } = 1.0f;
    public override string ButtonPromptText { get; set; }
    public override bool ShowsOnDirectory => false;
    public float VehiclePickupDistance { get; set; } = 25f;
    public int BodyDamageLimit { get; set; } = 200;
    public int EngineDamageLimit { get; set; } = 200;
    public string ContactName { get; set; } = "";
    public VehicleExporter(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string _Menu) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        MenuID = _Menu;
    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Export Vehicle At {Name}";
        return true;
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
        if (CanInteract)
        {
            Player.ActivityManager.IsInteractingWithLocation = true;
            CanInteract = false;
            GameFiber.StartNew(delegate
            {
                try
                {
                    StoreCamera = new LocationCamera(this, Player, Settings);
                    StoreCamera.Setup();
                    CreateInteractionMenu();
                    InteractionMenu.Visible = true;

                    GenerateExportMenu();

                    ProcessInteractionMenu();

                    if (HasExported && ContactName == StaticStrings.VehicleExporter)
                    {
                        Player.CellPhone.AddContact(new VehicleExporterContact(StaticStrings.VehicleExporter), true);
                    }

                    DisposeInteractionMenu();
                    StoreCamera.Dispose();
                    Player.ActivityManager.IsInteractingWithLocation = false;
                    CanInteract = true;
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "VehicleExporterInteract");
        }
    }
    public void AddPriceListItems(UIMenu toAdd)
    {
        foreach (MenuItem menuItem1 in Menu.Items.OrderBy(x => x.SalesPrice))
        {
            if (menuItem1.ModItem == null)
            {
                continue;
            }
            UIMenuItem vehicleToExportItem = new UIMenuItem(menuItem1.ModItem.DisplayName, menuItem1.ModItem.DisplayDescription) { RightLabel = menuItem1.SalesPrice.ToString("C0") };
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
        AddPriceListItems(ExportListSubMenu);



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
                continue;
            }
            string CarName = veh.GetCarName();
            bool CanExport = false;
            bool IsDamaged = false;
            int ExportAmount = 0;
            MenuItem menuItem = Menu.Items.FirstOrDefault(x => x.ModItemName == vehicleItem.Name);
            if(menuItem != null)
            {
                CanExport = true;
                ExportAmount = menuItem.SalesPrice;
            }
            if(veh.Vehicle.Health <= veh.Vehicle.MaxHealth - BodyDamageLimit || veh.Vehicle.EngineHealth <= veh.Vehicle.EngineHealth - EngineDamageLimit)
            {
                IsDamaged = true;
                CanExport = false;
            }
            UIMenuItem vehicleCrusherItem = new UIMenuItem(CarName, veh.GetCarDescription()) { RightLabel = ExportAmount.ToString("C0") };


            MenuLookups.Add(new Tuple<UIMenuItem,VehicleExt>(vehicleCrusherItem, veh));


            if (!CanExport)
            {
                vehicleCrusherItem.Enabled = false;
                vehicleCrusherItem.RightLabel = "";
            }
            if(IsDamaged)
            {
                vehicleCrusherItem.Description += "~n~~r~TOO DAMAGED TO EXPORT~s~";
            }
            vehicleCrusherItem.Activated += (sender, e) =>
            {
                ExportVehicle(veh, 1 * ExportAmount);
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
        Tuple<UIMenuItem,VehicleExt> lookupTuple = MenuLookups.FirstOrDefault(x=> x.Item1 == coolmen);
        if(lookupTuple == null || lookupTuple.Item2 == null || !lookupTuple.Item2.Vehicle.Exists())
        {
            return;
        }
        StoreCamera.HighlightEntity(lookupTuple.Item2.Vehicle);        
    }


    //private VehicleExt GetVehicle(string menuEntry)
    //{
    //    VehicleExt carToExport = null;
    //    foreach (VehicleExt veh in World.Vehicles.AllVehicleList)
    //    {
    //        if (!IsValidForExporting(veh))
    //        {
    //            continue;
    //        }
    //        string CarName = veh.GetCarName();
    //        if (menuEntry == CarName)
    //        {
    //            carToExport = veh;
    //        }

    //    }
    //    return carToExport;
    //}

    private void ExportVehicle(VehicleExt toExport, int Price)
    {
        if (toExport == null || !toExport.Vehicle.Exists() || toExport.VehicleBodyManager.StoredBodies.Any() || toExport.Vehicle.HasOccupants || !toExport.HasBeenEnteredByPlayer)
        {
            PlayErrorSound();
            DisplayMessage("~r~Exporting Failed", "We are unable to complete this export.");
            return;
        }
        Game.FadeScreenOut(1000, true);
        string CarName = toExport.GetCarName();
        toExport.WasCrushed = true;
        toExport.Vehicle.Delete();
        ExportSubMenu.MenuItems.RemoveAll(x => x.Text == CarName);
        ExportSubMenu.RefreshIndex();
        ExportSubMenu.Close(true);
        Game.FadeScreenIn(1000, true);
        Player.BankAccounts.GiveMoney(Price);
        PlaySuccessSound();
        DisplayMessage("~g~Exported", $"Thank you for exporting a ~p~{CarName}~s~ at ~y~{Name}~s~");
        HasExported = true;
    }

    private bool IsValidForExporting(VehicleExt toExport)
    {
        if (!toExport.Vehicle.Exists() || toExport.Vehicle.DistanceTo2D(EntrancePosition) > VehiclePickupDistance || toExport.VehicleBodyManager.StoredBodies.Any() || toExport.Vehicle.HasOccupants || !toExport.HasBeenEnteredByPlayer)
        {
            return false;
        }
        return true;
    }
}

