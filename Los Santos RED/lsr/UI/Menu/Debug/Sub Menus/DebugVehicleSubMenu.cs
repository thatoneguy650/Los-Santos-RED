using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DebugVehicleSubMenu : DebugSubMenu
{
    private UIMenu vehicleItemsMenu;
    private IPlateTypes PlateTypes;
    public DebugVehicleSubMenu(UIMenu debug, MenuPool menuPool, IActionable player, IPlateTypes plateTypes) : base(debug, menuPool, player)
    {
        PlateTypes = plateTypes;
    }
    public override void AddItems()
    {
        vehicleItemsMenu = MenuPool.AddSubMenu(Debug, "Vehicle Menu");
        vehicleItemsMenu.SetBannerType(EntryPoint.LSRedColor);
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Change various vehicle items.";
        Update();
    }
    public override void Update()
    {
        vehicleItemsMenu.Clear();
        if (Player.CurrentVehicle == null || !Player.CurrentVehicle.Vehicle.Exists())
        {
            return;
        }
        CreatePlateMenuItem();
        CreateLiveryMenuItem();
        CreateExtraMenuItem();
        CreateColorMenuItem();
        CreateInfoMenuItem();
    }
    private void CreateExtraMenuItem()
    {
        UIMenuNumericScrollerItem<int> VehicleExtraMenuItem = new UIMenuNumericScrollerItem<int>("Set Extra", "Set the vehicle Extra", 1, 14, 1);
        VehicleExtraMenuItem.Activated += (menu, item) =>
        {
            if (Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
            {
                bool isOn = NativeFunction.Natives.IS_VEHICLE_EXTRA_TURNED_ON<bool>(Player.CurrentVehicle.Vehicle, VehicleExtraMenuItem.Value);
                NativeFunction.Natives.SET_VEHICLE_EXTRA(Player.CurrentVehicle.Vehicle, VehicleExtraMenuItem.Value, isOn);
                Game.DisplaySubtitle($"SET EXTRA {VehicleExtraMenuItem.Value} Disabled:{isOn}");
            }
        };
        vehicleItemsMenu.AddItem(VehicleExtraMenuItem);
    }
    private void CreateColorMenuItem()//CreateColorMenuItem
    {
        UIMenuNumericScrollerItem<int> VehicleColorMenuItem = new UIMenuNumericScrollerItem<int>("Set Color", "Set the vehicle color", 0, 159, 1);
        VehicleColorMenuItem.Activated += (menu, item) =>
        {
            if (Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
            {
                NativeFunction.Natives.SET_VEHICLE_COLOURS(Player.CurrentVehicle.Vehicle, VehicleColorMenuItem.Value, VehicleColorMenuItem.Value);
                Game.DisplaySubtitle($"SET COLOR {VehicleColorMenuItem.Value}");
            }
        };
        vehicleItemsMenu.AddItem(VehicleColorMenuItem);
    }
    private void CreateLiveryMenuItem()
    {
        int Total = NativeFunction.Natives.GET_VEHICLE_LIVERY_COUNT<int>(Player.CurrentVehicle.Vehicle);
        if (Total == -1)
        {
            return;
        }
        UIMenuNumericScrollerItem<int> LogLocationMenu = new UIMenuNumericScrollerItem<int>("Set Livery", "Set the vehicle Livery", 0, Total - 1, 1);
        LogLocationMenu.Activated += (menu, item) =>
        {
            if (Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
            {
                NativeFunction.Natives.SET_VEHICLE_LIVERY(Player.CurrentVehicle.Vehicle, LogLocationMenu.Value);
                Game.DisplaySubtitle($"SET LIVERY {LogLocationMenu.Value}");
            }

        };
        vehicleItemsMenu.AddItem(LogLocationMenu);
    }
    private void CreatePlateMenuItem()
    {
        UIMenuListScrollerItem<PlateType> plateIndex = new UIMenuListScrollerItem<PlateType>("Plate Type", "Select Plate Type to change", PlateTypes.PlateTypeManager.PlateTypeList);
        plateIndex.Activated += (menu, item) =>
        {
            if (Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
            {
                PlateType NewType = PlateTypes.GetPlateType(plateIndex.SelectedItem.Index);
                if (NewType != null)
                {
                    string NewPlateNumber = NewType.GenerateNewLicensePlateNumber();
                    if (NewPlateNumber != "")
                    {
                        Player.CurrentVehicle.Vehicle.LicensePlate = NewPlateNumber;
                    }
                    NativeFunction.CallByName<int>("SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", Player.CurrentVehicle.Vehicle, NewType.Index);
                    Game.DisplaySubtitle($" PlateIndex: {plateIndex.SelectedItem.Index}, Index: {NewType.Index}, State: {NewType.StateID}, Description: {NewType.Description}");
                }
                else
                {
                    Game.DisplaySubtitle($" PlateIndex: {plateIndex.SelectedItem.Index} None Found");
                }
            }

        };
        vehicleItemsMenu.AddItem(plateIndex);
    }
    private void CreateInfoMenuItem()
    {
        UIMenuItem vehInfoMenu = new UIMenuItem("Get Info", "Print info");
        vehInfoMenu.Activated += (menu, item) =>
        {
            if (Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
            {
                string toDisplay = $"PlateNumber:{Player.CurrentVehicle.CarPlate.PlateNumber} PlateType:{Player.CurrentVehicle.CarPlate.PlateType} IsWanted:{Player.CurrentVehicle.CarPlate.IsWanted} " +
                $"PlateNumber2:{Player.CurrentVehicle.OriginalLicensePlate.PlateNumber} PlateType2:{Player.CurrentVehicle.OriginalLicensePlate.PlateType} IsWanted2:{Player.CurrentVehicle.OriginalLicensePlate.IsWanted}";
                Game.DisplaySubtitle(toDisplay);
                EntryPoint.WriteToConsole(toDisplay);
            }
        };
        vehicleItemsMenu.AddItem(vehInfoMenu);
    }
}

