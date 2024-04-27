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
    private IWeatherReportable WeatherReporter;
    private ITimeControllable Time;
    private IModItems ModItems;
    private IInteriors Interiors;

    public LocationDispatcher(IEntityProvideable world, IDispatchable player, IGangs gangs, ISettingsProvideable settings, IStreets streets, IZones zones, IGangTerritories gangTerritories, IWeapons weapons, INameProvideable names, 
        IPedGroups pedGroups, ICrimes crimes, IShopMenus shopMenus, IPlacesOfInterest placesOfInterest, IAgencies agencies, IJurisdictions jurisdictions, IWeatherReportable weatherReporter, ITimeControllable time, IModItems modItems, IInteriors interiors)
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
        WeatherReporter = weatherReporter;
        Time = time;
        ModItems = modItems;
        Interiors = interiors;
    }

    public void Dispatch()
    {
        foreach (GameLocation ps in World.Places.ActiveLocations.ToList().Where(x => x.IsEnabled && x.DistanceToPlayer <= x.ActivateDistance && x.IsNearby && !x.IsDispatchFilled && (x.PossibleGroupSpawns != null || x.PossiblePedSpawns != null || x.PossibleVehicleSpawns != null)).ToList())
        {
            if (ps.PossibleGroupSpawns != null)
            {
                foreach (ConditionalGroup cg in ps.PossibleGroupSpawns)
                {
                    //EntryPoint.WriteToConsole($"ATTEMPTING GROUP SPAWN AT {ps.Name}");
                    cg.AttemptSpawn(Player, Agencies, Gangs, Zones, Jurisdictions, GangTerritories, Settings, World, ps.AssociationID, Weapons, Names, Crimes, PedGroups, ShopMenus, WeatherReporter, Time, ModItems, ps);
                    GameFiber.Yield();
                }
            }
            GameFiber.Yield();
            if (ps.PossiblePedSpawns != null)
            {
                foreach (ConditionalLocation cl in ps.PossiblePedSpawns)
                {
                    //EntryPoint.WriteToConsole($"ATTEMPTING PED SPAWN AT {ps.Name}");
                    cl.AttemptSpawn(Player, true, false, Agencies, Gangs, Zones, Jurisdictions, GangTerritories, Settings, World, ps.AssociationID, Weapons, Names, Crimes, PedGroups,ShopMenus, WeatherReporter, Time, ModItems, ps);
                    GameFiber.Yield();
                }
            }
            GameFiber.Yield();
            if (ps.PossibleVehicleSpawns != null)
            {
                foreach (ConditionalLocation cl in ps.PossibleVehicleSpawns)
                {
                    //EntryPoint.WriteToConsole($"ATTEMPTING VEHICLE SPAWN AT {ps.Name}");
                    cl.AttemptSpawn(Player, false, false, Agencies, Gangs, Zones, Jurisdictions, GangTerritories, Settings, World, ps.AssociationID, Weapons, Names, Crimes, PedGroups, ShopMenus, WeatherReporter, Time, ModItems, ps);
                    GameFiber.Yield();
                }
            }
            ps.IsDispatchFilled = true;
            GameFiber.Yield();
        }
        GameFiber.Yield();
        foreach (GameLocation ps in PlacesOfInterest.InteractableLocations().Where(x => x.IsEnabled && !x.IsNearby && x.IsDispatchFilled).ToList())
        {
            //EntryPoint.WriteToConsole($"Location Dispatcher, CLEARED AT {ps.Name}");
            ps.IsDispatchFilled = false;
        }
        GameFiber.Yield();
        HandleServiceWorkerSpawns();
    }

    public void Reset()
    {
        foreach (GameLocation ps in PlacesOfInterest.InteractableLocations().Where(x => x.IsDispatchFilled).ToList())
        {
            ps.IsDispatchFilled = false;
        }
        foreach (GameLocation ps in PlacesOfInterest.InteractableLocations().Where(x => x.IsServiceFilled).ToList())
        {
            ps.IsServiceFilled = false;
        }
    }
    private void HandleServiceWorkerSpawns()
    {
        foreach (GameLocation ps in World.Places.ActiveLocations.ToList().Where(x => x.IsEnabled && x.DistanceToPlayer <= x.ActivateDistance && x.IsNearby && !x.IsServiceFilled).ToList())
        {
            ps.AttemptVendorSpawn(ps.IsOpen(Time.CurrentHour),Interiors,Settings,Crimes,Weapons,Time,World, false);
            ps.IsServiceFilled = true;
            GameFiber.Yield();
        }
        GameFiber.Yield();
        foreach (GameLocation ps in PlacesOfInterest.InteractableLocations().Where(x => x.IsEnabled && !x.IsNearby && x.IsServiceFilled).ToList())
        {
            ps.AttemptVendorDespawn();
            //EntryPoint.WriteToConsole($"VENDOR DESPAWN AT {ps.Name} ");
            ps.IsServiceFilled = false;
        }
    }
    public void SpawnInteriorServiceWorker(GameLocation ps)
    {
        if(ps == null)
        {
            return;
        }
        ps.AttemptVendorSpawn(ps.IsOpen(Time.CurrentHour), Interiors, Settings, Crimes, Weapons, Time, World, true);
    }

}

