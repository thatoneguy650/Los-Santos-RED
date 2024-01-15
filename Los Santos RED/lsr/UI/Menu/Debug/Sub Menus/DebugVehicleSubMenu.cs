using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
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
    private uint LastVehicleHandle;
    private int selectedPrimaryColor;
    private int selectedSecondaryColor;

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

        if (Player.InterestedVehicle == null || !Player.InterestedVehicle.Vehicle.Exists())
        {
            vehicleItemsMenu.Clear();
            LastVehicleHandle = 0;
            return;
        }
        if (LastVehicleHandle != Player.InterestedVehicle.Handle)
        {

            vehicleItemsMenu.Clear();
            CreateMenu();
        }
    }
    private void CreateMenu()
    {
        LastVehicleHandle = Player.InterestedVehicle.Handle;
        CreatePlateMenuItem();
        CreateLiveryMenuItem();
        CreateLivery2MenuItem();
        CreateExtraMenuItem();
        CreateColorMenuItem();
        CreateInfoMenuItem();
        CreateModificationItem();
    }
    private void CreateModificationItem()
    {

        UIMenuNumericScrollerItem<int> VehicleExtraMenuItem = new UIMenuNumericScrollerItem<int>("Set Extra", "Set the vehicle Extra", 1, 12, 1) { Value = 1 };
        VehicleExtraMenuItem.Activated += (menu, item) =>
        {
            if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
            {
               if(!NativeFunction.Natives.DOES_EXTRA_EXIST<bool>(Player.InterestedVehicle.Vehicle, VehicleExtraMenuItem.Value))
                {
                    Game.DisplaySubtitle($"EXTRA {VehicleExtraMenuItem.Value} DOES NOT EXIST");
                    return;
                }
                bool isOn = NativeFunction.Natives.IS_VEHICLE_EXTRA_TURNED_ON<bool>(Player.InterestedVehicle.Vehicle, VehicleExtraMenuItem.Value);
                NativeFunction.Natives.SET_VEHICLE_EXTRA(Player.InterestedVehicle.Vehicle, VehicleExtraMenuItem.Value, isOn);
                Game.DisplaySubtitle($"SET EXTRA {VehicleExtraMenuItem.Value} Disabled:{isOn}");
            }
        };
        vehicleItemsMenu.AddItem(VehicleExtraMenuItem);


        List<int> possibleToggles = new List<int>() { 17,18,19,20,21,22 };

        UIMenuListScrollerItem<int> VehicleToggleMenuItem = new UIMenuListScrollerItem<int>("Set Toggle", "Set the vehicle toggle", possibleToggles);
        VehicleToggleMenuItem.Activated += (menu, item) =>
        {
            if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
            {
                NativeFunction.Natives.SET_VEHICLE_MOD_KIT(Player.InterestedVehicle.Vehicle, 0);
                bool isOn = NativeFunction.Natives.IS_TOGGLE_MOD_ON<bool>(Player.InterestedVehicle.Vehicle, VehicleToggleMenuItem.SelectedItem);
                NativeFunction.Natives.TOGGLE_VEHICLE_MOD(Player.InterestedVehicle.Vehicle, VehicleToggleMenuItem.SelectedItem, isOn);
                Game.DisplaySubtitle($"SET Toggle {VehicleToggleMenuItem.SelectedItem} Disabled:{isOn}");
            }
        };
        vehicleItemsMenu.AddItem(VehicleToggleMenuItem);


        UIMenuNumericScrollerItem<int> VehicleModMenuItem = new UIMenuNumericScrollerItem<int>("Set Mod", "Set the vehicle mod", 1, 50, 1) { Value = 1 };
        VehicleModMenuItem.Activated += (menu, item) =>
        {
            if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
            {
                if (!int.TryParse(NativeHelper.GetKeyboardInput("-1"), out int modID))
                {
                    return;
                }
                if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
                {
                    NativeFunction.Natives.SET_VEHICLE_MOD_KIT(Player.InterestedVehicle.Vehicle, 0);
                    NativeFunction.Natives.SET_VEHICLE_MOD(Player.InterestedVehicle.Vehicle, VehicleModMenuItem.Value, modID, false);
                    Game.DisplaySubtitle($"SET MOD {VehicleModMenuItem.Value} modID:{modID}");
                }
            }
        };
        vehicleItemsMenu.AddItem(VehicleModMenuItem);

        /*VehicleExtras = new List<VehicleExtra>()
                    {
                        new VehicleExtra(0,false),
                        new VehicleExtra(1,false),
                        new VehicleExtra(2,false),
                        new VehicleExtra(3,false),
                        new VehicleExtra(4,false),
                        new VehicleExtra(5,false),
                        new VehicleExtra(6,false),
                        new VehicleExtra(7,false),
                        new VehicleExtra(8,false),
                        new VehicleExtra(9,false),
                        new VehicleExtra(10,false),
                        new VehicleExtra(11,false),
                        new VehicleExtra(12,false),
                        new VehicleExtra(13,false),
                        new VehicleExtra(14,false),
                        new VehicleExtra(15,false),
                    },
                VehicleToggles = new List<VehicleToggle>()
                    {
                        new VehicleToggle(17,false),
                        new VehicleToggle(18,false),
                        new VehicleToggle(19,false),
                        new VehicleToggle(20,false),
                        new VehicleToggle(21,false),
                        new VehicleToggle(22,false),
                    },
                VehicleMods = new List<VehicleMod>()
                    {
                        new VehicleMod(0,8),
                        new VehicleMod(1,0),
                        new VehicleMod(2,-1),
                        new VehicleMod(3,0),
                        new VehicleMod(4,-1),
                        new VehicleMod(5,1),
                        new VehicleMod(6,0),
                        new VehicleMod(7,2),
                        new VehicleMod(8,-1),
                        new VehicleMod(9,0),
                        new VehicleMod(10,0),
                        new VehicleMod(11,3),
                        new VehicleMod(12,-1),
                        new VehicleMod(13,-1),
                        new VehicleMod(14,-1),
                        new VehicleMod(15,-1),
                        new VehicleMod(16,-1),
                        new VehicleMod(23,12),
                        new VehicleMod(24,-1),
                        new VehicleMod(25,-1),
                        new VehicleMod(26,-1),
                        new VehicleMod(27,-1),
                        new VehicleMod(28,-1),
                        new VehicleMod(29,-1),
                        new VehicleMod(30,-1),
                        new VehicleMod(31,-1),
                        new VehicleMod(32,-1),
                        new VehicleMod(33,-1),
                        new VehicleMod(34,-1),
                        new VehicleMod(35,-1),
                        new VehicleMod(36,-1),
                        new VehicleMod(37,-1),
                        new VehicleMod(38,-1),
                        new VehicleMod(39,-1),
                        new VehicleMod(40,-1),
                        new VehicleMod(41,-1),
                        new VehicleMod(42,-1),
                        new VehicleMod(43,1),
                        new VehicleMod(44,-1),
                        new VehicleMod(45,-1),
                        new VehicleMod(46,-1),
                        new VehicleMod(47,-1),
                        new VehicleMod(48,-1),
                        new VehicleMod(49,-1),
                        new VehicleMod(50,-1),
                    },*/
    }

    private void CreateExtraMenuItem()
    {
        //UIMenuNumericScrollerItem<int> VehicleExtraMenuItem = new UIMenuNumericScrollerItem<int>("Set Extra", "Set the vehicle Extra", 1, 15, 1) { Value = 1 };
        //VehicleExtraMenuItem.Activated += (menu, item) =>
        //{
        //    if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
        //    {
        //        bool isOn = NativeFunction.Natives.IS_VEHICLE_EXTRA_TURNED_ON<bool>(Player.InterestedVehicle.Vehicle, VehicleExtraMenuItem.Value);
        //        NativeFunction.Natives.SET_VEHICLE_EXTRA(Player.InterestedVehicle.Vehicle, VehicleExtraMenuItem.Value, isOn);
        //        Game.DisplaySubtitle($"SET EXTRA {VehicleExtraMenuItem.Value} Disabled:{isOn}");
        //    }
        //};
        //vehicleItemsMenu.AddItem(VehicleExtraMenuItem);
    }
    private void CreateColorMenuItem()//CreateColorMenuItem
    {
        UIMenuNumericScrollerItem<int> VehicleColorMenuItem = new UIMenuNumericScrollerItem<int>("Set Both Color", "Set both the vehicle colors the same", 0, 159, 1);
        VehicleColorMenuItem.Value = VehicleColorMenuItem.Minimum;
        VehicleColorMenuItem.Activated += (menu, item) =>
        {
            if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
            {
                NativeFunction.Natives.SET_VEHICLE_COLOURS(Player.InterestedVehicle.Vehicle, VehicleColorMenuItem.Value, VehicleColorMenuItem.Value);
                Game.DisplaySubtitle($"SET COLOR {VehicleColorMenuItem.Value}");
            }
        };
        vehicleItemsMenu.AddItem(VehicleColorMenuItem);


        selectedPrimaryColor = 0;
        selectedSecondaryColor = 0;
        UIMenuNumericScrollerItem<int> vehiclePrimaryColorScroller = new UIMenuNumericScrollerItem<int>("Primary Color", "Set the primary color selected", 0, 159, 1);
        vehiclePrimaryColorScroller.Value = vehiclePrimaryColorScroller.Minimum;
        vehiclePrimaryColorScroller.Activated += (menu, item) =>
        {
            selectedPrimaryColor = vehiclePrimaryColorScroller.Value;
            if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
            {
                NativeFunction.Natives.SET_VEHICLE_COLOURS(Player.InterestedVehicle.Vehicle, selectedPrimaryColor, selectedSecondaryColor);
                Game.DisplaySubtitle($"SET COLOR {VehicleColorMenuItem.Value}");
            }
        };
        vehicleItemsMenu.AddItem(vehiclePrimaryColorScroller);


        UIMenuNumericScrollerItem<int> vehicleSecondaryColorScroller = new UIMenuNumericScrollerItem<int>("Secondary Color", "Set the secondary color selected", 0, 159, 1);
        vehicleSecondaryColorScroller.Value = vehicleSecondaryColorScroller.Minimum;
        vehicleSecondaryColorScroller.Activated += (sender, selectedItem) =>
        {
            selectedSecondaryColor = vehicleSecondaryColorScroller.Value;
            if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
            {
                NativeFunction.Natives.SET_VEHICLE_COLOURS(Player.InterestedVehicle.Vehicle, selectedPrimaryColor, selectedSecondaryColor);
                Game.DisplaySubtitle($"SET COLOR {VehicleColorMenuItem.Value}");
            }
        };
        vehicleItemsMenu.AddItem(vehicleSecondaryColorScroller);


        UIMenuItem setWHiteMenu = new UIMenuItem("Set Color White", "Set Color White");
        setWHiteMenu.Activated += (menu, item) =>
        {
            if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
            {
                NativeFunction.Natives.SET_VEHICLE_COLOURS(Player.InterestedVehicle.Vehicle, 134, 134);
            }
        };
        vehicleItemsMenu.AddItem(setWHiteMenu);








        UIMenuNumericScrollerItem<float> dirtsetscroller = new UIMenuNumericScrollerItem<float>("Set Dirtyness", "Set how dirty the car is", 0.0f, 15.0f, 1.0f);
        dirtsetscroller.Value = dirtsetscroller.Minimum;
        dirtsetscroller.Activated += (menu, item) =>
        {
            if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
            {
                NativeFunction.Natives.SET_VEHICLE_DIRT_LEVEL(Player.InterestedVehicle.Vehicle, dirtsetscroller.Value);
                Game.DisplaySubtitle($"SET DIRT {dirtsetscroller.Value}");
            }
        };
        vehicleItemsMenu.AddItem(dirtsetscroller);

    }


    private void CreateLiveryMenuItem()
    {
        int Total = NativeFunction.Natives.GET_VEHICLE_LIVERY_COUNT<int>(Player.InterestedVehicle.Vehicle);
        if (Total == -1)
        {
            return;
        }
        UIMenuNumericScrollerItem<int> LogLocationMenu = new UIMenuNumericScrollerItem<int>("Set Livery", "Set the vehicle Livery", 0, Total - 1, 1) ;
        LogLocationMenu.Value = LogLocationMenu.Minimum;
        LogLocationMenu.Activated += (menu, item) =>
        {
            if (Player.InterestedVehicle.Vehicle != null && Player.InterestedVehicle.Vehicle.Exists())
            {
                NativeFunction.Natives.SET_VEHICLE_LIVERY(Player.InterestedVehicle.Vehicle, LogLocationMenu.Value);
                Game.DisplaySubtitle($"SET LIVERY {LogLocationMenu.Value}");
            }
            
        };
        vehicleItemsMenu.AddItem(LogLocationMenu);
    }
    private void CreateLivery2MenuItem()
    {
        int Total = NativeFunction.Natives.GET_VEHICLE_LIVERY2_COUNT<int>(Player.InterestedVehicle.Vehicle);
        if (Total == -1)
        {
            return;
        }
        UIMenuNumericScrollerItem<int> LogLocationMenu = new UIMenuNumericScrollerItem<int>("Set Livery 2", "Set the vehicle Livery 2", 0, Total - 1, 1);
        LogLocationMenu.Value = LogLocationMenu.Minimum;
        LogLocationMenu.Activated += (menu, item) =>
        {
            if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
            {
                NativeFunction.Natives.SET_VEHICLE_LIVERY2(Player.InterestedVehicle.Vehicle, LogLocationMenu.Value);
                Game.DisplaySubtitle($"SET LIVERY 2 {LogLocationMenu.Value}");
            }

        };
        vehicleItemsMenu.AddItem(LogLocationMenu);
    }
    private void CreatePlateMenuItem()
    {
        UIMenuListScrollerItem<PlateType> plateIndex = new UIMenuListScrollerItem<PlateType>("Plate Type", "Select Plate Type to change", PlateTypes.PlateTypeManager.PlateTypeList);
        plateIndex.Activated += (menu, item) =>
        {
            if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
            {
                PlateType NewType = PlateTypes.GetPlateType(plateIndex.SelectedItem.Index);
                if (NewType != null)
                {
                    string NewPlateNumber = NewType.GenerateNewLicensePlateNumber();
                    if (NewPlateNumber != "")
                    {
                        Player.InterestedVehicle.Vehicle.LicensePlate = NewPlateNumber;
                    }
                    NativeFunction.CallByName<int>("SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", Player.InterestedVehicle.Vehicle, NewType.Index);
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
            if (Player.InterestedVehicle != null && Player.InterestedVehicle.Vehicle.Exists())
            {
                string toDisplay = $"PlateNumber:{Player.InterestedVehicle.CarPlate.PlateNumber} PlateType:{Player.InterestedVehicle.CarPlate.PlateType} IsWanted:{Player.InterestedVehicle.CarPlate.IsWanted} " +
                $"PlateNumber2:{Player.InterestedVehicle.OriginalLicensePlate.PlateNumber} PlateType2:{Player.InterestedVehicle.OriginalLicensePlate.PlateType} IsWanted2:{Player.InterestedVehicle.OriginalLicensePlate.IsWanted}";
                Game.DisplaySubtitle(toDisplay);
                EntryPoint.WriteToConsole(toDisplay);
            }
        };
        vehicleItemsMenu.AddItem(vehInfoMenu);
    }
}

