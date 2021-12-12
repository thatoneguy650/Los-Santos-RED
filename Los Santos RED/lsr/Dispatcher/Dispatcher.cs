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
    private IWeapons Weapons;
    private INameProvideable Names;
    public Dispatcher(IEntityProvideable world, IDispatchable player, IAgencies agencies, ISettingsProvideable settings, IStreets streets, IZones zones, IJurisdictions jurisdictions, IWeapons weapons, INameProvideable names)
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
    }
    public void Dispatch()
    {
        if (!LEDispatcher.Dispatch())
        {
            if (!EMSDispatcher.Dispatch())
            {
                FireDispatcher.Dispatch();
            }
        }
    }
    public void Recall()
    {
        LEDispatcher.Recall();
        EMSDispatcher.Recall();
        FireDispatcher.Recall();
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

