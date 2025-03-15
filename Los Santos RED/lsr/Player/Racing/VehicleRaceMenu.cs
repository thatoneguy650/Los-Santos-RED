using LosSantosRED.lsr.Interface;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class VehicleRaceMenu
{
    private MenuPool MenuPool;
    private UIMenu RaceMenu;
    private PedExt PedExt;
    private IVehicleRaces VehicleRaces;

    public VehicleRaceMenu(MenuPool menuPool, UIMenu challengeToRaceSubMenu, PedExt conversingPed, IVehicleRaces vehicleRaces)
    {
        MenuPool = menuPool;
        RaceMenu = challengeToRaceSubMenu;
        PedExt = conversingPed;
        VehicleRaces = vehicleRaces;
    }

    public void Setup()
    {
        foreach(VehicleRace vehicleRace in VehicleRaces.VehicleRaceTypeManager.VehiclesRaces)
        {
            vehicleRace.AddToMenu(MenuPool,RaceMenu);
        }
    }
}

