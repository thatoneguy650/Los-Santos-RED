using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Dispatcher
{
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
    private GangDispatcher GangDispatcher;
    private IWeapons Weapons;
    private INameProvideable Names;
    //private List<RandomHeadData> RandomHeadList;

    private ICrimes Crimes;
    private IPedGroups PedGroups;
    private IGangs Gangs;
    private IGangTerritories GangTerritories;
    private IShopMenus ShopMenus;
    private IPlacesOfInterest PlacesOfInterest;

    public Dispatcher(IEntityProvideable world, IDispatchable player, IAgencies agencies, ISettingsProvideable settings, IStreets streets, IZones zones, IJurisdictions jurisdictions, IWeapons weapons, INameProvideable names, ICrimes crimes, IPedGroups pedGroups, IGangs gangs, IGangTerritories gangTerritories, IShopMenus shopMenus, IPlacesOfInterest placesOfInterest)
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
    }
    public void Setup()
    {
        LEDispatcher = new LEDispatcher(World, Player, Agencies, Settings, Streets, Zones, Jurisdictions, Weapons, Names);
        EMSDispatcher = new EMSDispatcher(World, Player, Agencies, Settings, Streets, Zones, Jurisdictions, Weapons, Names);
        FireDispatcher = new FireDispatcher(World, Player, Agencies, Settings, Streets, Zones, Jurisdictions, Weapons, Names);
        ZombieDispatcher = new ZombieDispatcher(World, Player, Settings, Streets, Zones, Jurisdictions, Weapons, Names, Crimes);
        GangDispatcher = new GangDispatcher(World, Player, Gangs, Settings, Streets, Zones, GangTerritories, Weapons, Names, PedGroups, Crimes, ShopMenus, PlacesOfInterest);
    }
    public void Dispatch()
    {
        if (!LEDispatcher.Dispatch())
        {
            if (!EMSDispatcher.Dispatch())
            {
                if(!FireDispatcher.Dispatch())
                {
                    GangDispatcher.Dispatch();
                }
            }
        }
        if(World.IsZombieApocalypse)
        {
            GameFiber.Yield();
            ZombieDispatcher.Dispatch();
        }
    }
    public void Recall()
    {
        LEDispatcher.Recall();
        EMSDispatcher.Recall();
        FireDispatcher.Recall();
        if (World.IsZombieApocalypse)
        {
            GameFiber.Yield();
            ZombieDispatcher.Recall();
        }
        GangDispatcher.Recall();
    }
    public void Dispose()
    {
        LEDispatcher.Dispose();
    }
    public void SpawnRoadblock()
    {
        LEDispatcher.SpawnRoadblock();
    }
    public void RemoveRoadblock()
    {
        LEDispatcher.RemoveRoadblock();
    }
    public void DebugSpawnCop()
    {
        LEDispatcher.DebugSpawnCop("",false);
    }
    public void DebugSpawnCop(string agencyID, bool onFoot)
    {
        LEDispatcher.DebugSpawnCop(agencyID,onFoot);
    }
    public void DebugSpawnGang()
    {
        GangDispatcher.ForceDispatch("",false);
    }
    public void DebugSpawnGang(string agencyID, bool onFoot)
    {
        GangDispatcher.ForceDispatch(agencyID, onFoot);
    }
    public void DebugSpawnEMT(string agencyID, bool onFoot)
    {
        EMSDispatcher.DebugSpawnEMT(agencyID, onFoot);
    }
}

