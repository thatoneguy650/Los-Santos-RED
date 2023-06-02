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

    private SpawnLocation SpawnLocation;
    private DispatchableVehicle VehicleType;
    private DispatchablePerson PersonType;

    public LocationDispatcher(IEntityProvideable world, IDispatchable player, IGangs gangs, ISettingsProvideable settings, IStreets streets, IZones zones, IGangTerritories gangTerritories, IWeapons weapons, INameProvideable names, 
        IPedGroups pedGroups, ICrimes crimes, IShopMenus shopMenus, IPlacesOfInterest placesOfInterest, IAgencies agencies, IJurisdictions jurisdictions, IWeatherReportable weatherReporter, ITimeControllable time, IModItems modItems)
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
    }

    public void Dispatch()
    {
        foreach (InteractableLocation ps in World.Places.ActiveInteractableLocations.ToList().Where(x => x.IsEnabled && x.DistanceToPlayer <= x.ActivateDistance && x.IsNearby && !x.IsDispatchFilled && (x.PossibleGroupSpawns != null || x.PossiblePedSpawns != null || x.PossibleVehicleSpawns != null)).ToList())
        {
            if (ps.PossibleGroupSpawns != null)
            {
                foreach (ConditionalGroup cg in ps.PossibleGroupSpawns)
                {
                    cg.AttemptSpawn(Player, Agencies, Gangs, Zones, Jurisdictions, GangTerritories, Settings, World, ps.AssociationID, Weapons, Names, Crimes, PedGroups, ShopMenus, WeatherReporter, Time, ModItems);
                }
            }


            List<string> ForcedGroups = new List<string>();
            //EntryPoint.WriteToConsole($"Location Dispatcher, SPAWNED AT {ps.Name}");
            if (ps.PossiblePedSpawns != null)
            {

                foreach (ConditionalLocation cl in ps.PossiblePedSpawns)
                {
                    bool hasStuff = ForcedGroups.Contains(cl.GroupID);
                    cl.AttemptSpawn(Player, true, hasStuff, Agencies, Gangs, Zones, Jurisdictions, GangTerritories, Settings, World, ps.AssociationID, Weapons, Names, Crimes, PedGroups,ShopMenus, WeatherReporter, Time, ModItems);
                    if(cl.AttemptedSpawn && !string.IsNullOrEmpty(cl.GroupID))
                    {
                        //EntryPoint.WriteToConsoleTestLong($"ADDED FORCED GROUP {cl.GroupID}");
                        ForcedGroups.Add(cl.GroupID);
                    }
                    GameFiber.Yield();
                }
            }
            GameFiber.Yield();
            if (ps.PossibleVehicleSpawns != null)
            {
                foreach (ConditionalLocation cl in ps.PossibleVehicleSpawns)
                {
                    EntryPoint.WriteToConsole($"ATTEMPTING VEHICLE SPAWN AT {ps.Name} FOR {cl.GroupID}");


                    bool hasStuff = ForcedGroups.Contains(cl.GroupID);
                    //EntryPoint.WriteToConsoleTestLong($"CHECKED FORCE GROUP {cl.GroupID} FORCING:{hasStuff}");
                    cl.AttemptSpawn(Player, false, hasStuff, Agencies, Gangs, Zones, Jurisdictions, GangTerritories, Settings, World, ps.AssociationID, Weapons, Names, Crimes, PedGroups, ShopMenus, WeatherReporter, Time, ModItems);
                    
                    if (cl.AttemptedSpawn && !string.IsNullOrEmpty(cl.GroupID))
                    {
                        //EntryPoint.WriteToConsoleTestLong($"ADDED FORCED GROUP {cl.GroupID}");
                        ForcedGroups.Add(cl.GroupID);
                    }
                    GameFiber.Yield();
                }
            }
            ps.IsDispatchFilled = true;
            GameFiber.Yield();
        }
    
        foreach (InteractableLocation ps in PlacesOfInterest.InteractableLocations().Where(x => x.IsEnabled && !x.IsNearby && x.IsDispatchFilled).ToList())
        {
            //EntryPoint.WriteToConsole($"Location Dispatcher, CLEARED AT {ps.Name}");
            ps.IsDispatchFilled = false;
        }
    }
    public void Reset()
    {
        foreach (InteractableLocation ps in PlacesOfInterest.InteractableLocations().Where(x => x.IsDispatchFilled).ToList())
        {
            ps.IsDispatchFilled = false;
        }
    }
}

