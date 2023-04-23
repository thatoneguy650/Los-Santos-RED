using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Dispatcher
{
    private uint GameTimeStartedDispatch;
    private readonly IAgencies Agencies;
    private readonly IDispatchable Player;
    private readonly ISettingsProvideable Settings;
    private readonly IStreets Streets;
    private readonly IEntityProvideable World;
    private readonly IJurisdictions Jurisdictions;
    private readonly IZones Zones;
    private LEDispatcher LEDispatcher;
    private EMSDispatcher EMSDispatcher;
    private FireDispatcher FireDispatcher;
    private ZombieDispatcher ZombieDispatcher;
    private SecurityDispatcher SecurityDispatcher;
    private GangDispatcher GangDispatcher;
    private LocationDispatcher LocationDispatcher;
    private IWeapons Weapons;
    private INameProvideable Names;
    private IWeatherReportable WeatherReporter;
    private ITimeControllable Time;
    //private List<RandomHeadData> RandomHeadList;

    private ICrimes Crimes;
    private IPedGroups PedGroups;
    private IGangs Gangs;
    private IGangTerritories GangTerritories;
    private IShopMenus ShopMenus;
    private IPlacesOfInterest PlacesOfInterest;
    private IModItems ModItems;
    private bool hasLocationDispatched;

    public Dispatcher(IEntityProvideable world, IDispatchable player, IAgencies agencies, ISettingsProvideable settings, IStreets streets, IZones zones, IJurisdictions jurisdictions, IWeapons weapons, INameProvideable names, ICrimes crimes,
        IPedGroups pedGroups, IGangs gangs, IGangTerritories gangTerritories, IShopMenus shopMenus, IPlacesOfInterest placesOfInterest, IWeatherReportable weatherReporter, ITimeControllable time, IModItems modItems)
    {
        Player = player;
        World = world;
        Agencies = agencies;
        Settings = settings;
        Streets = streets;
        Zones = zones;
        Jurisdictions = jurisdictions;
        Weapons = weapons;
        Names = names;
        Crimes = crimes;
        Gangs = gangs;
        PedGroups = pedGroups;
        GangTerritories = gangTerritories;
        ShopMenus = shopMenus;
        PlacesOfInterest = placesOfInterest;
        WeatherReporter = weatherReporter;
        Time = time;
        ModItems = modItems;
    }
    public void Setup()
    {
        GameTimeStartedDispatch = Game.GameTime;
        LEDispatcher = new LEDispatcher(World, Player, Agencies, Settings, Streets, Zones, Jurisdictions, Weapons, Names, PlacesOfInterest, ModItems);
        EMSDispatcher = new EMSDispatcher(World, Player, Agencies, Settings, Streets, Zones, Jurisdictions, Weapons, Names, PlacesOfInterest, ModItems);
        SecurityDispatcher = new SecurityDispatcher(World, Player, Agencies, Settings, Streets, Zones, Jurisdictions, Weapons, Names, PlacesOfInterest, Crimes, ModItems);
        FireDispatcher = new FireDispatcher(World, Player, Agencies, Settings, Streets, Zones, Jurisdictions, Weapons, Names, PlacesOfInterest, ModItems, ShopMenus);
        ZombieDispatcher = new ZombieDispatcher(World, Player, Settings, Streets, Zones, Jurisdictions, Weapons, Names, Crimes);
        GangDispatcher = new GangDispatcher(World, Player, Gangs, Settings, Streets, Zones, GangTerritories, Weapons, Names, PedGroups, Crimes, ShopMenus, PlacesOfInterest, ModItems);
        LocationDispatcher = new LocationDispatcher(World, Player, Gangs, Settings, Streets, Zones, GangTerritories, Weapons, Names, PedGroups, Crimes, ShopMenus, PlacesOfInterest, Agencies, Jurisdictions, WeatherReporter, Time, ModItems);
    }
    public void Dispatch()
    {
        if(Player.IsCustomizingPed)
        {
            return;
        }
        if(Player.RecentlyStartedPlaying)
        {
            EntryPoint.WriteToConsole("RECENTLY STARTED PLAYING NO DISPTACH");
            return;
        }
        if(Game.GameTime - GameTimeStartedDispatch <= 10000)
        {
            EntryPoint.WriteToConsole("RECENTLY STARTED PLAYING222 NO DISPTACH");
            return;
        }
        if (!LEDispatcher.Dispatch())
        {
            if (!EMSDispatcher.Dispatch())
            {
                if(!FireDispatcher.Dispatch())
                {
                    if(!SecurityDispatcher.Dispatch())
                    {

                    }
                }
            }
        }
        GameFiber.Yield();
        GangDispatcher.Dispatch();
        if (World.IsZombieApocalypse)
        {
            GameFiber.Yield();
            ZombieDispatcher.Dispatch();
        }
        GameFiber.Yield();
        LocationDispatcher.Dispatch();
    }
    public void Recall()
    {
        LEDispatcher.Recall();
        EMSDispatcher.Recall();
        GameFiber.Yield();
        FireDispatcher.Recall();
        SecurityDispatcher.Recall();
        if (World.IsZombieApocalypse)
        {
            GameFiber.Yield();
            ZombieDispatcher.Recall();
        }
        GameFiber.Yield();
        GangDispatcher.Recall();
    }
    public void Dispose()
    {
        LEDispatcher.Dispose();
    }
    public void DebugSpawnRoadblock(float distance)
    {
        LEDispatcher.SpawnRoadblock(true, distance);
    }
    public void DebugRemoveRoadblock()
    {
        LEDispatcher.RemoveRoadblock();

        World.Vehicles.ClearSpawned(true);
        World.Pedestrians.ClearSpawned();
    }
    public void DebugSpawnCop()
    {
        LEDispatcher.DebugSpawnCop("",false, false, null, null, false);
    }
    public void DebugSpawnCop(string agencyID, bool onFoot, bool isEmpty, DispatchableVehicle dispatchableVehicle, DispatchablePerson dispatchablePerson)
    {
        LEDispatcher.DebugSpawnCop(agencyID, onFoot, isEmpty, dispatchableVehicle, dispatchablePerson, false);
    }
    public void DebugSpawnCop(string agencyID, bool onFoot, bool isEmpty)
    {
        LEDispatcher.DebugSpawnCop(agencyID,onFoot, isEmpty, null, null, false);
    }
    public void DebugSpawnK9Cop(string agencyID)
    {
        LEDispatcher.DebugSpawnCop(agencyID, false, false, null, null, true);
    }
    public void DebugSpawnGang()
    {
        GangDispatcher.DebugSpawnGangMember("",false, false);
    }
    public void DebugSpawnGang(string agencyID, bool onFoot, bool isEmpty)
    {
        GangDispatcher.DebugSpawnGangMember(agencyID, onFoot, isEmpty);
    }
    public void DebugSpawnEMT(string agencyID, bool onFoot, bool isEmpty)
    {
        EMSDispatcher.DebugSpawnEMT(agencyID, onFoot, isEmpty);
    }

    public void DebugSpawnSecurityGuard(string agencyID, bool onFoot, bool isEmpty)
    {
        SecurityDispatcher.DebugSpawnSecurity(agencyID, onFoot, isEmpty);
    }
}

