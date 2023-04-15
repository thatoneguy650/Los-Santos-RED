using LSR.Vehicles;
using Rage;
using RAGENativeUI.Elements;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Interface;

public class VehicleInteractionMenu
{
    public bool IsShowingMenu { get; private set; } = false;
    private VehicleExt VehicleExt;
    private MenuPool MenuPool;
    private UIMenu VehicleInteractMenu;
    private VehicleDoorSeatData VehicleDoorSeatData;
    private IInteractionable Player;
    public VehicleInteractionMenu(VehicleExt vehicleExt)
    {
        VehicleExt = vehicleExt;
    }
    public void ShowInteractionMenu(IInteractionable player, IWeapons weapons, IModItems modItems, VehicleDoorSeatData vehicleDoorSeatData, IVehicleSeatAndDoorLookup vehicleSeatDoorData, IEntityProvideable world)
    {
        VehicleDoorSeatData = vehicleDoorSeatData;
        Player = player;
        CreateInteractionMenu();
        if (!player.IsInVehicle)
        {
            VehicleExt.VehicleBodyManager.CreateInteractionMenu(MenuPool, VehicleInteractMenu, vehicleSeatDoorData, world);
        }
        VehicleExt.WeaponStorage.CreateInteractionMenu(player, MenuPool, VehicleInteractMenu, weapons, modItems, !player.IsInVehicle);
        if(!player.IsInVehicle)
        {
            VehicleExt.CreateDoorMenu(MenuPool, VehicleInteractMenu);
        }
        VehicleExt.HandleRandomItems(modItems);
        VehicleExt.SimpleInventory.CreateInteractionMenu(player, MenuPool, VehicleInteractMenu, !player.IsInVehicle);
        VehicleInteractMenu.Visible = true;
        IsShowingMenu = true;
        ProcessMenu();
    }
    private void CreateInteractionMenu()
    {
        MenuPool = new MenuPool();
        VehicleInteractMenu = new UIMenu("Vehicle", "Select an Option");
        VehicleInteractMenu.SetBannerType(EntryPoint.LSRedColor);
        MenuPool.Add(VehicleInteractMenu);     
    }
    private void ProcessMenu()
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                while (EntryPoint.ModController.IsRunning && Player.IsAliveAndFree && MenuPool.IsAnyMenuOpen() && VehicleExt.Vehicle.Exists() && VehicleExt.Vehicle.DistanceTo2D(Game.LocalPlayer.Character) <= 10f)
                {
                    MenuPool.ProcessMenus();
                    GameFiber.Yield();
                }
                if(VehicleDoorSeatData != null)
                {
                    Player.ActivityManager.SetDoor(VehicleDoorSeatData.DoorID, true, false);
                }
                IsShowingMenu = false;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "VehicleInteraction");
    }
}

