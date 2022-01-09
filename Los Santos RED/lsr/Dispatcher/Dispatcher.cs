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
    public Dispatcher(IEntityProvideable world, IDispatchable player, IAgencies agencies, ISettingsProvideable settings, IStreets streets, IZones zones, IJurisdictions jurisdictions, IWeapons weapons, INameProvideable names, ICrimes crimes, IPedGroups pedGroups, IGangs gangs, IGangTerritories gangTerritories, IShopMenus shopMenus)
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
        LEDispatcher = new LEDispatcher(World, Player, Agencies, Settings, Streets, Zones, Jurisdictions, Weapons, Names);
        EMSDispatcher = new EMSDispatcher(World, Player, Agencies, Settings, Streets, Zones, Jurisdictions, Weapons, Names);
        FireDispatcher = new FireDispatcher(World, Player, Agencies, Settings, Streets, Zones, Jurisdictions, Weapons, Names);
        ZombieDispatcher = new ZombieDispatcher(World, Player, Settings, Streets, Zones, Jurisdictions, Weapons, Names, crimes);
        GangDispatcher = new GangDispatcher(World, Player, gangs, Settings, Streets, Zones, gangTerritories, Weapons, Names, pedGroups, crimes, shopMenus);
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
        //GangDispatcher.Dispatch();
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
        LEDispatcher.DebugSpawnCop();
    }
}

