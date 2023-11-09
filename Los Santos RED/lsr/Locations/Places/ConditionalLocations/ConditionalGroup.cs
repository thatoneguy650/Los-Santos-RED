using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class ConditionalGroup
{
    protected IAgencies Agencies;
    protected IGangs Gangs;
    protected IJurisdictions Jurisdictions;
    protected IGangTerritories GangTerritories;
    protected IZones Zones;
    protected ISettingsProvideable Settings;
    protected IEntityProvideable World;
    protected IWeapons Weapons;
    protected INameProvideable Names;
    protected IDispatchable Player;
    protected ICrimes Crimes;
    protected IPedGroups PedGroups;
    protected IShopMenus ShopMenus;
    protected IWeatherReportable WeatherReporter;
    protected ITimeControllable Time;
    protected IModItems ModItems;
    protected SpawnTask SpawnTask;
    protected SpawnLocation SpawnLocation;
    protected DispatchablePerson DispatchablePerson;
    protected DispatchableVehicle DispatchableVehicle;
    protected GameLocation GameLocation;
    public string Name { get; set; }
    public float Percentage { get; set; }
    public float OverrideNightPercentage { get; set; } = -1.0f;
    public float OverrideDayPercentage { get; set; } = -1.0f;
    public float OverridePoorWeatherPercentage { get; set; } = -1.0f;
    public int MinHourSpawn { get; set; } = 0;
    public int MaxHourSpawn { get; set; } = 24;
    public int MinWantedLevelSpawn { get; set; } = 0;
    public int MaxWantedLevelSpawn { get; set; } = 3;
    public List<ConditionalLocation> PossiblePedSpawns { get; set; }
    public List<ConditionalLocation> PossibleVehicleSpawns { get; set; }
    [XmlIgnore]
    public bool AttemptedSpawn { get; private set; }
    public virtual void AttemptSpawn(IDispatchable player, IAgencies agencies, IGangs gangs, IZones zones, IJurisdictions jurisdictions, IGangTerritories gangTerritories, ISettingsProvideable settings, IEntityProvideable world, string masterAssociationID, 
        IWeapons weapons, INameProvideable names, ICrimes crimes, IPedGroups pedGroups, IShopMenus shopMenus, IWeatherReportable weatherReporter, ITimeControllable time, IModItems modItems, GameLocation gameLocation)
    {
        Player = player;
        Agencies = agencies;
        Gangs = gangs;
        Zones = zones;
        Jurisdictions = jurisdictions;
        GangTerritories = gangTerritories;
        Settings = settings;
        World = world;
        Weapons = weapons;
        Names = names;
        Crimes = crimes;
        PedGroups = pedGroups;
        ShopMenus = shopMenus;
        WeatherReporter = weatherReporter;
        Time = time;
        ModItems = modItems;
        GameLocation = gameLocation;
        AttemptedSpawn = DetermineRun();
        if (!AttemptedSpawn)
        {
            return;
        }
        if (PossiblePedSpawns != null)
        {
            foreach (ConditionalLocation cl in PossiblePedSpawns)
            {
                cl.AttemptSpawn(Player, true, true, Agencies, Gangs, Zones, Jurisdictions, GangTerritories, Settings, World, masterAssociationID, Weapons, Names, Crimes, PedGroups, ShopMenus, WeatherReporter, Time, ModItems, GameLocation);
                GameFiber.Yield();
            }
        }
        GameFiber.Yield();
        if (PossibleVehicleSpawns != null)
        {
            foreach (ConditionalLocation cl in PossibleVehicleSpawns)
            {
                cl.AttemptSpawn(Player, false, true, Agencies, Gangs, Zones, Jurisdictions, GangTerritories, Settings, World, masterAssociationID, Weapons, Names, Crimes, PedGroups, ShopMenus, WeatherReporter, Time, ModItems, GameLocation);
                GameFiber.Yield();
            }
        }
    }
    private bool IsValidTimeToSpawn()
    {
        // EntryPoint.WriteToConsole($"IsValidTimeToSpawn Time:{Time.CurrentHour} {MinHourSpawn} {MaxHourSpawn} IsMinLess:{MinHourSpawn <= MaxHourSpawn}");
        if (MinHourSpawn <= MaxHourSpawn)
        {
            if (Time.CurrentHour >= MinHourSpawn && Time.CurrentHour <= MaxHourSpawn)
            {
                return true;
            }
        }
        else
        {
            if (Time.CurrentHour >= MinHourSpawn || Time.CurrentHour <= MaxHourSpawn)
            {
                return true;
            }
        }
        return false;
    }
    public virtual bool DetermineRun()
    {
        if (!IsValidTimeToSpawn())
        {
            // EntryPoint.WriteToConsole($"NOT IsValidTimeToSpawn Time:{Time.CurrentHour} {MinHourSpawn} {MaxHourSpawn} IsMinLess:{MinHourSpawn <= MaxHourSpawn}");
            return false;
        }
        if (World.TotalWantedLevel < MinWantedLevelSpawn || World.TotalWantedLevel > MaxWantedLevelSpawn)
        {
            return false;
        }
        if (WeatherReporter.IsPoorWeather && OverridePoorWeatherPercentage != -1.0f)
        {
            return RandomItems.RandomPercent(OverridePoorWeatherPercentage);
        }
        else if (Time.IsNight && OverrideNightPercentage != -1.0f)
        {
            return RandomItems.RandomPercent(OverrideNightPercentage);
        }
        else if (!Time.IsNight && OverrideDayPercentage != -1.0f)
        {
            return RandomItems.RandomPercent(OverrideDayPercentage);
        }
        return RandomItems.RandomPercent(Percentage);
    }


}

