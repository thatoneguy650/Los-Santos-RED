using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TaxiManager
{
    private bool HasRequestedService = false;
    private ITaxiRideable Player;
    private IEntityProvideable World;
    private IPlacesOfInterest PlacesOfInterest;
    private ISettingsProvideable Settings;

    public TaxiManager(ITaxiRideable player, IEntityProvideable world, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings)
    {
        Player = player;
        World = world;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
    }


    public bool RequestService(TaxiFirm taxiFirm)
    {
        if(HasRequestedService)
        {
            return false;
        }
        if(taxiFirm == null)
        {
            return false;
        }

        TaxiRide taxiRide = new TaxiRide(World, Player, taxiFirm, Player.Position);
        taxiRide.Setup();

        if(!taxiRide.CanStart)
        {
            return false;
        }

        return true;
    }

}

