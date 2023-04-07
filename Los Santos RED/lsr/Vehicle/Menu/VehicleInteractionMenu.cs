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
    private VehicleExt VehicleExt;
    private MenuPool MenuPool;
    private UIMenu VehicleInteractMenu;

    public VehicleInteractionMenu(VehicleExt vehicleExt)
    {
        VehicleExt = vehicleExt;
    }
    public void ShowInteractionMenu(bool isInvehicle, IWeapons weapons, IModItems modItems)
    {
        CreateInteractionMenu();
        if (!isInvehicle)
        {
            VehicleExt.VehicleBodyManager.CreateInteractionMenu(MenuPool, VehicleInteractMenu);
        }
        VehicleExt.WeaponStorage.CreateInteractionMenu(MenuPool, VehicleInteractMenu, weapons, modItems);
        if(!isInvehicle)
        {
            VehicleExt.CreateDoorMenu(MenuPool, VehicleInteractMenu);
        }
        VehicleInteractMenu.Visible = true;
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
                while (EntryPoint.ModController.IsRunning && MenuPool.IsAnyMenuOpen())
                {
                    MenuPool.ProcessMenus();
                    GameFiber.Yield();
                }
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "VehicleInteraction");
    }
}

