using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DebugTrunkSubMenu : DebugSubMenu
{
    public DebugTrunkSubMenu(UIMenu debug, MenuPool menuPool, IActionable player) : base(debug, menuPool, player)
    {

    }

    public override void AddItems()
    {
        SubMenu = MenuPool.AddSubMenu(Debug, "Trunk Menu");
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Change trunk attach items.";
        SubMenu.SetBannerType(EntryPoint.LSRedColor);
        UpdateMenu();
    }
    public override void Update()
    {
        UpdateMenu();
    }
    private void UpdateMenu()
    {
        SubMenu.Clear();

        UIMenuItem reattachMenuItem = new UIMenuItem("Reattach Ped");
        reattachMenuItem.Activated += (sender, e) =>
        {
            if (Player.InterestedVehicle == null || !Player.InterestedVehicle.Vehicle.Exists())
            {
                Game.DisplaySubtitle("No Vehicle");
                return;
            }
            StoredBody storedBody = Player.InterestedVehicle.VehicleBodyManager.StoredBodies.FirstOrDefault(x => x.VehicleDoorSeatData != null && x.VehicleDoorSeatData.SeatID == -2);
            if (storedBody == null)
            {
                Game.DisplaySubtitle("No Trunk Ped");
                return;
            }
            storedBody.ReAttach();
        };
        SubMenu.AddItem(reattachMenuItem);





    }
}

