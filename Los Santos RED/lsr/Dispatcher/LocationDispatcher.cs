using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class LocationDispatcher
{
    private readonly IGangs Gangs;
    private readonly IDispatchable Player;
    private readonly ISettingsProvideable Settings;
    private readonly IStreets Streets;
    private readonly IEntityProvideable World;
    private readonly IZones Zones;
    private IWeapons Weapons;
    private INameProvideable Names;
    private IGangTerritories GangTerritories;
    private IPedGroups PedGroups;
    private ICrimes Crimes;
    private IShopMenus ShopMenus;
    private IPlacesOfInterest PlacesOfInterest;
    private IAgencies Agencies;
    private IJurisdictions Jurisdictions;



    private SpawnLocation SpawnLocation;
    private DispatchableVehicle VehicleType;
    private DispatchablePerson PersonType;

    public LocationDispatcher(IEntityProvideable world, IDispatchable player, IGangs gangs, ISettingsProvideable settings, IStreets streets, IZones zones, IGangTerritories gangTerritories, IWeapons weapons, INameProvideable names, IPedGroups pedGroups, ICrimes crimes, IShopMenus shopMenus, IPlacesOfInterest placesOfInterest, IAgencies agencies, IJurisdictions jurisdictions)
    {
        Player = player;
        World = world;
        Gangs = gangs;
        Settings = settings;
        Streets = streets;
        Zones = zones;
        GangTerritories = gangTerritories;
        Weapons = weapons;
        Names = names;
        PedGroups = pedGroups;
        Crimes = crimes;
        ShopMenus = shopMenus;
        PlacesOfInterest = placesOfInterest;
        Agencies = agencies;
        Jurisdictions = jurisdictions;
    }

    public void Dispatch()
    {
        foreach (InteractableLocation ps in World.Places.ActiveInteractableLocations.ToList().Where(x => x.IsEnabled && x.DistanceToPlayer <= 225f && x.IsNearby && !x.IsDispatchFilled && (x.PossiblePedSpawns != null || x.PossibleVehicleSpawns != null)).ToList())
        {
            //EntryPoint.WriteToConsole($"Location Dispatcher, SPAWNED AT {ps.Name}");
            if (ps.PossiblePedSpawns != null)
            {
                foreach (ConditionalLocation cl in ps.PossiblePedSpawns)
                {
                    cl.AttemptSpawn(Player, true, false, Agencies, Gangs, Zones, Jurisdictions, GangTerritories, Settings, World, ps.AssignedAgencyID, Weapons, Names, Crimes, PedGroups,ShopMenus);
                    GameFiber.Yield();
                }
            }
            if (ps.PossibleVehicleSpawns != null)
            {
                foreach (ConditionalLocation cl in ps.PossibleVehicleSpawns)
                {
                    cl.AttemptSpawn(Player, false, false, Agencies, Gangs, Zones, Jurisdictions, GangTerritories, Settings, World, ps.AssignedAgencyID, Weapons, Names, Crimes, PedGroups, ShopMenus);
                    GameFiber.Yield();
                }
            }
            ps.IsDispatchFilled = true;
            //GameFiber.Yield();
        }
    
        foreach (InteractableLocation ps in PlacesOfInterest.InteractableLocations().Where(x => x.IsEnabled && !x.IsNearby && x.IsDispatchFilled).ToList())
        {
            //EntryPoint.WriteToConsole($"Location Dispatcher, CLEARED AT {ps.Name}");
            ps.IsDispatchFilled = false;
        }
    }
}

