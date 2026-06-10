using LosSantosRED.lsr.Interface;
using Mod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GangTerritoryManager
{
    private IGangTerritoryManageable Player;
    private ISettingsProvideable Settings;
    private IEntityProvideable World;
    private IGangTerritories GangTerritories;
    private IPlacesOfInterest PlacesOfInterest;
    private ITimeReportable Time;

    public GangTerritoryManager(IGangTerritoryManageable player, ISettingsProvideable settings, IEntityProvideable world, IGangTerritories gangTerritories, IPlacesOfInterest placesOfInterest, ITimeReportable time)
    {
        Player = player;
        Settings = settings;
        World = world;
        GangTerritories = gangTerritories;
        PlacesOfInterest = placesOfInterest;
    }

    public void Dispose()
    {
        
    }

    public void Setup()
    {
        
    }

    public void Update()
    {
        
    }
    public bool SetTookOverZone(Zone zone)
    {
        if(Player.CurrentGang == null)
        {
            return false;
        }
        if(zone == null)
        {
            return false;
        }
        bool updated = GangTerritories.UpdateTerritory(Player.CurrentGang.ID, zone);
        if(updated)
        {
            zone.UpdateGangItems(GangTerritories);

            List<GangDen> densToUpdate = PlacesOfInterest.PossibleLocations.GangDens.Where(x=> x.ZoneID == zone.InternalGameName).ToList();
            foreach(GangDen dens in densToUpdate)
            {
                dens.AssociatedGang = Player.CurrentGang;
                dens.AssignedAssociationID = Player.CurrentGang.ID;
                dens.IsAvailableForPlayer = false;
                dens.IsDispatchFilled = false;
                dens.DeactivateBlip();
                dens.ActivateBlip(Time, World);
            }

        }
        return updated;
    }
}

