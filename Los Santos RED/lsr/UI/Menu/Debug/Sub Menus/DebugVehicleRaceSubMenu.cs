using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DebugVehicleRaceSubMenu : DebugSubMenu
{
    private List<VehicleRace> PossibleRaces = new List<VehicleRace>();
    private IVehicleRaces VehicleRaces;
    public DebugVehicleRaceSubMenu(UIMenu debug, MenuPool menuPool, IActionable player, IVehicleRaces vehicleRaces) : base(debug, menuPool, player)
    {
        VehicleRaces = vehicleRaces;
    }

    public override void AddItems()
    {
        SubMenu = MenuPool.AddSubMenu(Debug, "Racing Menu");
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Do vehicle racing";
        SubMenu.SetBannerType(EntryPoint.LSRedColor);
        CreateMenu();
    }
    public override void Update()
    {

    }
    private void CreateMenu()
    {
        SubMenu.Clear();
        foreach(VehicleRaceTrack vrt in VehicleRaces.VehicleRaceTypeManager.VehicleRaceTracks)
        {
            UIMenuItem generalOne = new UIMenuItem(vrt.Name, "Start a the specific race.");
            generalOne.Activated += (sender, selectedItem) =>
            {
                VehicleRace vr = new VehicleRace(vrt.Name, vrt, Player.CurrentVehicle);
                Player.RacingManager.StartDebugRace(vr);

                sender.Visible = false;
            };
            SubMenu.AddItem(generalOne);
        }
    }
}

