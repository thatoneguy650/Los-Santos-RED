using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

[XmlInclude(typeof(LEConditionalLocation))]
[XmlInclude(typeof(SecurityConditionalLocation))]
[XmlInclude(typeof(GangConditionalLocation))]
[XmlInclude(typeof(EMSConditionalLocation))]
[XmlInclude(typeof(FireConditionalLocation))]
public class ConditionalLocation
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
    protected SpawnTask SpawnTask;
    protected SpawnLocation SpawnLocation;
    protected DispatchablePerson DispatchablePerson;
    protected DispatchableVehicle DispatchableVehicle;
    protected string MasterAssociationID;
    protected bool IsPerson;

    public ConditionalLocation()
    {
    }
    public ConditionalLocation(Vector3 location, float heading, float percentage)
    {
        Location = location;
        Heading = heading;
        Percentage = percentage;
    }
    [XmlIgnore]
    public bool AttemptedSpawn { get; private set; }

    public Vector3 Location { get; set; }
    public float Heading { get; set; }
    public float Percentage { get; set; }
    public string AssociationID { get; set; }
    public string RequiredPedGroup { get; set; }
    public string RequiredVehicleGroup { get; set; }
    public bool IsEmpty { get; set; } = true;
    public string GroupID { get; set; } = "";
    public TaskRequirements TaskRequirements { get; set; } = TaskRequirements.None;
    public List<string> ForcedScenarios { get; set; }
    public float OverrideNightPercentage { get; set; } = -1.0f;
    public float OverrideDayPercentage { get; set; } = -1.0f;
    public float OverridePoorWeatherPercentage { get; set; } = -1.0f;
    public int MinHourSpawn { get; set; } = 0;
    public int MaxHourSpawn { get; set; } = 24;
    public virtual void AttemptSpawn(IDispatchable player, bool isPerson, bool force, IAgencies agencies, IGangs gangs, IZones zones, IJurisdictions jurisdictions, IGangTerritories gangTerritories, ISettingsProvideable settings, IEntityProvideable world, string masterAssociationID, IWeapons weapons, INameProvideable names, ICrimes crimes, IPedGroups pedGroups, IShopMenus shopMenus, IWeatherReportable weatherReporter, ITimeControllable time)
    {
        Player = player;
        IsPerson = isPerson;
        Agencies = agencies;
        Gangs = gangs;
        Zones = zones;
        Jurisdictions = jurisdictions;
        GangTerritories = gangTerritories;
        Settings = settings;
        World = world;
        MasterAssociationID = masterAssociationID;
        Weapons = weapons;
        Names = names;
        Crimes = crimes;
        PedGroups = pedGroups;
        ShopMenus = shopMenus;
        WeatherReporter = weatherReporter;
        Time = time;
        AttemptedSpawn = DetermineRun(force);
        if (!AttemptedSpawn)
        {
            return;
        }
        GenerateSpawnLocation();
        GetDispatchableGenerator();
        GenerateSpawnTypes();
        RunSpawnTask();
    }
    public virtual bool DetermineRun(bool force)
    {
        if(force)
        {
            return true;
        }
        if (Time.CurrentHour < MinHourSpawn || Time.CurrentHour > MaxHourSpawn)
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
    public virtual void GenerateSpawnLocation()
    {
        SpawnLocation = new SpawnLocation(Location);
        SpawnLocation.Heading = Heading;
        SpawnLocation.StreetPosition = Location;
        SpawnLocation.SidewalkPosition = Location;
    }
    public virtual void GetDispatchableGenerator()
    {

    }
    public virtual void GenerateSpawnTypes()
    {

    }
    public virtual void RunSpawnTask()
    {

    }
    public virtual void AddLocationRequirements(PedExt ped)
    {
        if(ped == null || ped.LocationTaskRequirements == null)
        {
            return;
        }
        ped.LocationTaskRequirements.TaskRequirements = TaskRequirements;
        ped.LocationTaskRequirements.ForcedScenarios.Clear();
        if (ForcedScenarios != null && ForcedScenarios.Any())
        {
            ped.LocationTaskRequirements.ForcedScenarios.AddRange(ForcedScenarios.ToList());
        }
        EntryPoint.WriteToConsole("ADDED LOCATION REQUIREMENTS");
    }


}

