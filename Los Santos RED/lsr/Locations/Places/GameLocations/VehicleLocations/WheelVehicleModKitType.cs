using LosSantosRED.lsr.Interface;
using Rage.Native;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class WheelVehicleModKitType : VehicleModKitType
{
    private UIMenu WheelSubMenu;

    //public int WheelTypeID { get; private set; }
    public WheelVehicleModKitType(string name, int id, ModShopMenu modShopMenu, UIMenu interactionMenu, MenuPool menuPool, ILocationInteractable player) : base(name, id, modShopMenu, interactionMenu, menuPool, player)
    {

    }
    public override void AddToMenu()
    {
        if (ModdingVehicle == null || !ModdingVehicle.Vehicle.Exists() || CurrentVariation == null)
        {
            return;
        }

        int TotalMods = NativeFunction.Natives.GET_NUM_VEHICLE_MODS<int>(ModdingVehicle.Vehicle, TypeID);
        if (TotalMods == 0)
        {
            return;
        }

        WheelSubMenu = MenuPool.AddSubMenu(InteractionMenu, TypeName);
        WheelSubMenu.SetBannerType(EntryPoint.LSRedColor);

        // Restore original wheels when exiting Wheels menu
        WheelSubMenu.OnMenuClose += (sender) =>
        {
            NativeFunction.Natives.SET_VEHICLE_WHEEL_TYPE(ModdingVehicle.Vehicle, CurrentVariation.WheelType);

            // Reset wheel mod to saved value (or -1 if none)
            VehicleMod wheelMod = CurrentVariation.VehicleMods.FirstOrDefault(x => x.ID == TypeID);
            int savedMod = wheelMod?.Output ?? -1;
            NativeFunction.Natives.SET_VEHICLE_MOD(ModdingVehicle.Vehicle, TypeID, savedMod, false);
        };

        AddWheelTypeSubMenus();
    }
    private void AddWheelTypeSubMenus()
    {
        List<WheelVehicleClassModKitType> list = new List<WheelVehicleClassModKitType>() 
        { 
            new WheelVehicleClassModKitType("Sport",0,TypeID, ModShopMenu,WheelSubMenu,MenuPool,Player),
            new WheelVehicleClassModKitType("Muscle",1,TypeID, ModShopMenu,WheelSubMenu,MenuPool,Player),
            new WheelVehicleClassModKitType("Lowrider",2,TypeID, ModShopMenu,WheelSubMenu,MenuPool,Player),
            new WheelVehicleClassModKitType("SUV",3,TypeID, ModShopMenu,WheelSubMenu,MenuPool,Player),
            new WheelVehicleClassModKitType("Off-Road",4,TypeID, ModShopMenu,WheelSubMenu,MenuPool,Player),
            new WheelVehicleClassModKitType("Tuner",5,TypeID,ModShopMenu,WheelSubMenu,MenuPool,Player),
            new WheelVehicleClassModKitType("Bike",6,TypeID, ModShopMenu,WheelSubMenu,MenuPool,Player),
            new WheelVehicleClassModKitType("High End",7,TypeID,ModShopMenu,WheelSubMenu,MenuPool,Player),
            new WheelVehicleClassModKitType("Benny's Originals",8,TypeID,ModShopMenu,WheelSubMenu,MenuPool,Player),
            new WheelVehicleClassModKitType("Benny's Bespoke",9,TypeID,ModShopMenu,WheelSubMenu,MenuPool,Player),
            new WheelVehicleClassModKitType("Racing",10,TypeID,ModShopMenu,WheelSubMenu,MenuPool,Player),
            new WheelVehicleClassModKitType("Street",11,TypeID,ModShopMenu,WheelSubMenu,MenuPool,Player),
            new WheelVehicleClassModKitType("Track",12,TypeID,ModShopMenu,WheelSubMenu,MenuPool,Player),

        };
        foreach (WheelVehicleClassModKitType menu in list)
        {
            menu.AddWheelTypeMenus();
        }

        if (ModdingVehicle == null || !ModdingVehicle.Vehicle.Exists())
        {
            return;
        }
        NativeFunction.Natives.SET_VEHICLE_WHEEL_TYPE(ModdingVehicle.Vehicle, CurrentVariation.WheelType);
    }

}

