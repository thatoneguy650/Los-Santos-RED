using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class VehicleLicensePlateMenu
{
    private MenuPool MenuPool;
    private UIMenu VehicleHeaderMenu;
    private ILocationInteractable Player;
    private VehicleExt ModdingVehicle;
    private VehicleVariation CurrentVariation;
    private GameLocation GameLocation;
    private ModShopMenu ModShopMenu;
    private IPlateTypes PlateTypes;
    private UIMenu LicensePlatesMenu;
    private UIMenu LicensePlatesStyleMenu;
    private List<LicensePlateMenuItem> MenuItems = new List<LicensePlateMenuItem>();

    public VehicleLicensePlateMenu(MenuPool menuPool, UIMenu vehicleHeaderMenu, ILocationInteractable player, VehicleExt moddingVehicle, ModShopMenu modShopMenu, VehicleVariation currentVariation, GameLocation gameLocation, IPlateTypes plateTypes)
    {
        MenuPool = menuPool;
        VehicleHeaderMenu = vehicleHeaderMenu;
        Player = player;
        ModdingVehicle = moddingVehicle;
        CurrentVariation = currentVariation;
        GameLocation = gameLocation;
        ModShopMenu = modShopMenu;
        PlateTypes = plateTypes;
    }
    public void Setup()
    {
        LicensePlatesMenu = MenuPool.AddSubMenu(VehicleHeaderMenu, "License Plates");
        LicensePlatesMenu.SubtitleText = "LICENSE PLATES";
        VehicleHeaderMenu.MenuItems[VehicleHeaderMenu.MenuItems.Count() - 1].Description = "Pick License Plates";
        AddLicensePlates();
    }

    private void AddLicensePlates()
    {
        if (ModdingVehicle == null || !ModdingVehicle.Vehicle.Exists())
        {
            return;
        }

        LicensePlatesStyleMenu = MenuPool.AddSubMenu(LicensePlatesMenu, "Style");
        LicensePlatesStyleMenu.SubtitleText = "STYLE";
        LicensePlatesMenu.MenuItems[LicensePlatesMenu.MenuItems.Count() - 1].Description = "Pick License Plate Style";




        UIMenuItem setPlateNumber = new UIMenuItem("Plate Number", "Set plate number from input");
        int CustomTextPrice = 200;
        setPlateNumber.RightLabel = $"~r~${CustomTextPrice}";

        setPlateNumber.Activated += (menu, item) =>
        {
            string newplateText = NativeHelper.GetKeyboardInput("");
            if (string.IsNullOrEmpty(newplateText))
            {
                return;
            }
            if(newplateText.Length > 8)
            {
                ModShopMenu.DisplayMessage("Plate text needs to be 8 or less characters");
                return;
            }

            if (Player.BankAccounts.GetMoney(true) < CustomTextPrice)
            {
                ModShopMenu.DisplayInsufficientFundsMessage(CustomTextPrice);
                return;
            }
            ModShopMenu.DisplayPurchasedMessage(CustomTextPrice);
            Player.BankAccounts.GiveMoney(-1 * CustomTextPrice, true);



            if (ModdingVehicle != null && ModdingVehicle.Vehicle.Exists())
            {
                ModdingVehicle.Vehicle.LicensePlate = newplateText;
                CurrentVariation.LicensePlate.PlateNumber = newplateText;
            }
        };
        LicensePlatesMenu.AddItem(setPlateNumber);



        LicensePlatesStyleMenu.OnMenuOpen += (sender) =>
        {
            LicensePlateMenuItem licensePlateMenuItem = MenuItems.Where(x => x.Index == LicensePlatesStyleMenu.CurrentSelection).FirstOrDefault();
            if (licensePlateMenuItem == null)
            {
                return;
            }
            SetLicensePlateStyle(licensePlateMenuItem.PlateType, false);
        };
        LicensePlatesStyleMenu.OnMenuClose += (sender) =>
        {
            ResetLicensePlateStyle();
        };
        LicensePlatesStyleMenu.OnIndexChange += (sender, newIndex) =>
        {
            LicensePlateMenuItem licensePlateMenuItem = MenuItems.Where(x => x.Index == newIndex).FirstOrDefault();
            if (licensePlateMenuItem == null)
            {
                EntryPoint.WriteToConsole($"OnIndexChange NEWINDEX:{newIndex}");
                return;
            }
            SetLicensePlateStyle(licensePlateMenuItem.PlateType, false);
        };
        int counter = 0;
        foreach (PlateType plateType in PlateTypes.PlateTypeManager.PlateTypeList)
        {
            UIMenuItem plateTypeItem = new UIMenuItem(plateType.Description,plateType.Description);
            MenuItems.Add(new LicensePlateMenuItem(plateTypeItem, plateType, counter));
            bool isCurrent = CurrentVariation.LicensePlate.PlateType == plateType.Index;
            if (isCurrent)
            {
                plateTypeItem.RightLabel = "";
                plateTypeItem.RightBadge = UIMenuItem.BadgeStyle.Tick;
            }
            else
            {
                plateTypeItem.RightLabel = $"~r~${GetPrice(plateType.Index)}~s~";
                plateTypeItem.RightBadge = UIMenuItem.BadgeStyle.None;
            }
            plateTypeItem.Activated += (sender, e) =>
            {
                int totalPrice = GetPrice(plateType.Index);
                bool isInstalled = CurrentVariation.LicensePlate.PlateType == plateType.Index;
                if (isInstalled)
                {
                    ModShopMenu.DisplayMessage("You already have this mod installed");
                    return;
                }
                if (Player.BankAccounts.GetMoney(true) < totalPrice)
                {
                    ModShopMenu.DisplayInsufficientFundsMessage(totalPrice);
                    return;
                }
                ModShopMenu.DisplayPurchasedMessage(totalPrice);
                Player.BankAccounts.GiveMoney(-1 * totalPrice, true);

                //apply the modkit permanently
                if (ModdingVehicle == null || !ModdingVehicle.Vehicle.Exists())
                {
                    return;
                }





                SetLicensePlateStyle(plateType, true);
            };
            counter++;


            LicensePlatesStyleMenu.AddItem(plateTypeItem);
        }
    }
    protected void SyncMenuItems()
    {

        foreach (LicensePlateMenuItem item in MenuItems)
        {
            bool isCurrent = CurrentVariation.LicensePlate.PlateType == item.PlateType.Index;
            if (isCurrent)
            {
                item.UIMenuItem.RightLabel = "";
                item.UIMenuItem.RightBadge = UIMenuItem.BadgeStyle.Tick;
            }
            else
            {
                item.UIMenuItem.RightLabel = $"~r~${GetPrice(item.PlateType.Index)}~s~";
                item.UIMenuItem.RightBadge = UIMenuItem.BadgeStyle.None;
            }
        }
    }

    private int GetPrice(int index)
    {
        return 450;
    }

    private void ResetLicensePlateStyle()
    {
        if(ModdingVehicle == null || !ModdingVehicle.Vehicle.Exists() || CurrentVariation == null || CurrentVariation.LicensePlate == null)
        {
            return;
        }
        int setPlateTypeID = CurrentVariation.LicensePlate.PlateType;
        ModdingVehicle.Vehicle.LicensePlate = CurrentVariation.LicensePlate.PlateNumber;
        NativeFunction.Natives.SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX(ModdingVehicle.Vehicle, setPlateTypeID);
    }

    private void SetLicensePlateStyle(PlateType plateType, bool updateVariation)
    {
        if (ModdingVehicle == null || !ModdingVehicle.Vehicle.Exists() || CurrentVariation == null || CurrentVariation.LicensePlate == null)
        {
            return;
        }
        string newPlateText = plateType.GenerateNewLicensePlateNumber();
        if (CurrentVariation.LicensePlate.PlateType == plateType.Index && !updateVariation)
        {
            newPlateText = CurrentVariation.LicensePlate.PlateNumber;
        }
        
        if (updateVariation)
        {
            CurrentVariation.LicensePlate.PlateType = plateType.Index;
            CurrentVariation.LicensePlate.PlateNumber = newPlateText;
        }
        NativeFunction.Natives.SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX(ModdingVehicle.Vehicle, plateType.Index);
        ModdingVehicle.Vehicle.LicensePlate = newPlateText;
        SyncMenuItems();
    }

    protected class LicensePlateMenuItem
    {
        public LicensePlateMenuItem(UIMenuItem uIMenuItem, PlateType plateType, int index)
        {
            UIMenuItem = uIMenuItem;
            PlateType = plateType;
            Index = index;
        }

        public UIMenuItem UIMenuItem { get; set; }
        public PlateType PlateType { get; set; }
        public int Index { get; set; }
    }
}

