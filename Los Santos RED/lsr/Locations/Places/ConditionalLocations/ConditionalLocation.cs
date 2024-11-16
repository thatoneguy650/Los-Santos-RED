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
[XmlInclude(typeof(MerchantConditionalLocation))]
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
    protected IModItems ModItems;
    protected SpawnTask SpawnTask;
    protected SpawnLocation SpawnLocation;
    protected DispatchablePerson DispatchablePerson;
    protected DispatchableVehicle DispatchableVehicle;
    protected string MasterAssociationID;
    protected bool IsPerson;
    protected GameLocation GameLocation;
    public ConditionalLocation()
    {
    }
    public ConditionalLocation(Vector3 location, float heading, float percentage)
    {
        Location = location;
        Heading = heading;
        Percentage = percentage;
    }
    public bool HasDispatchablePerson => DispatchablePerson != null;


    public bool IsEmptyVehicleSpawn => DispatchablePerson == null;
    public Vector3 Location { get; set; }
    public float Heading { get; set; }
    public float Percentage { get; set; }
    public string AssociationID { get; set; }
    public string RequiredPedGroup { get; set; }
    public string RequiredVehicleGroup { get; set; }
    public bool IsEmpty { get; set; } = true;
    public bool AllowAirVehicle { get; set; } = false;
    public bool AllowBoat { get; set; } = false;
    public TaskRequirements TaskRequirements { get; set; } = TaskRequirements.None;
    public List<string> ForcedScenarios { get; set; }
    public float OverrideNightPercentage { get; set; } = -1.0f;
    public float OverrideDayPercentage { get; set; } = -1.0f;
    public float OverridePoorWeatherPercentage { get; set; } = -1.0f;
    public int MinHourSpawn { get; set; } = 0;
    public int MaxHourSpawn { get; set; } = 24;
    public virtual int MinWantedLevelSpawn { get; set; } = 0;
    public virtual int MaxWantedLevelSpawn { get; set; } = 3;
    public bool LongGunAlwaysEquipped { get; set; } = false;
    public bool ForceMelee { get; set; } = false;
    public bool ForceSidearm { get; set; } = false;
    public bool ForceLongGun { get; set; } = false;

    public bool ForceVehicleGroup { get; set; } = false;

    [XmlIgnore]
    public bool AttemptedSpawn { get; private set; }
    [XmlIgnore]
    public bool Ignore { get; private set; }

    //public virtual void Setup(IAgencies agencies, IGangs gangs, IZones zones, IJurisdictions jurisdictions, IGangTerritories gangTerritories, ISettingsProvideable settings, IEntityProvideable world, string masterAssociationID, IWeapons weapons, INameProvideable names, ICrimes crimes, IPedGroups pedGroups, IShopMenus shopMenus, ITimeControllable time, IModItems modItems)
    //{
    //    Agencies = agencies;
    //    Gangs = gangs;
    //    Zones = zones;
    //    Jurisdictions = jurisdictions;
    //    GangTerritories = gangTerritories;
    //    Settings = settings;
    //    World = world;
    //    MasterAssociationID = masterAssociationID;
    //    Weapons = weapons;
    //    Names = names;
    //    Crimes = crimes;
    //    PedGroups = pedGroups;
    //    ShopMenus = shopMenus;
    //    Time = time;
    //    ModItems = modItems;
    //    GetDispatchableGenerator();
    //}
    //public override void StoreData()
    //{

    //}
    public virtual void AttemptSpawn(IDispatchable player, bool isPerson, bool force, IAgencies agencies, IGangs gangs, IZones zones, IJurisdictions jurisdictions, IGangTerritories gangTerritories, ISettingsProvideable settings, 
        IEntityProvideable world, string masterAssociationID, IWeapons weapons, INameProvideable names, ICrimes crimes, IPedGroups pedGroups, IShopMenus shopMenus, IWeatherReportable weatherReporter, ITimeControllable time,
        IModItems modItems, GameLocation gameLocation)
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
        ModItems = modItems;
        GameLocation = gameLocation;
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

    public virtual void ForceSpawn(IDispatchable player, bool isPerson, bool force, IAgencies agencies, IGangs gangs, IZones zones, IJurisdictions jurisdictions, IGangTerritories gangTerritories, ISettingsProvideable settings,
    IEntityProvideable world, string masterAssociationID, IWeapons weapons, INameProvideable names, ICrimes crimes, IPedGroups pedGroups, IShopMenus shopMenus, IWeatherReportable weatherReporter, ITimeControllable time,
    IModItems modItems, GameLocation gameLocation)
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
        ModItems = modItems;
        GameLocation = gameLocation;
        GenerateSpawnLocation();
        GetDispatchableGenerator();


       // GenerateSpawnTypes();
        RunSpawnTask();
    }
    private bool IsValidTimeToSpawn()
    {
       // EntryPoint.WriteToConsole($"IsValidTimeToSpawn Time:{Time.CurrentHour} {MinHourSpawn} {MaxHourSpawn} IsMinLess:{MinHourSpawn <= MaxHourSpawn}");
        //if(MinHourSpawn <= MaxHourSpawn)
        //{
        //    if(Time.CurrentHour >= MinHourSpawn && Time.CurrentHour <= MaxHourSpawn)
        //    {
        //        return true;
        //    }
        //}
        //else
        //{
        //    if(Time.CurrentHour >= MinHourSpawn || Time.CurrentHour <= MaxHourSpawn)
        //    {
        //        return true;
        //    }
        //}


        if (MaxHourSpawn == 24 && MinHourSpawn == 0)
        {
            return true;
        }//18 & 4, NEW BAD
        //8 & 20, OLD GOOD
        if (MinHourSpawn < MaxHourSpawn)
        {
            return (Time.CurrentHour >= MinHourSpawn && Time.CurrentHour <= MaxHourSpawn);
        }
        else
        {
            return (Time.CurrentHour >= MinHourSpawn || Time.CurrentHour <= MaxHourSpawn);
        }



        //return false;
    }
    public virtual void OnLocationDeactivated()
    {

    }
    public virtual bool DetermineRun(bool force)
    {
        if(force)
        {
            return true;
        }
        if(!IsValidTimeToSpawn())
        {
           // EntryPoint.WriteToConsole($"NOT IsValidTimeToSpawn Time:{Time.CurrentHour} {MinHourSpawn} {MaxHourSpawn} IsMinLess:{MinHourSpawn <= MaxHourSpawn}");
            return false;
        }
        if(World.TotalWantedLevel < MinWantedLevelSpawn || World.TotalWantedLevel > MaxWantedLevelSpawn)
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
        if (LongGunAlwaysEquipped)
        {
            ped.AlwaysHasLongGun = true;
        }
        ped.LocationTaskRequirements.TaskRequirements = TaskRequirements;
        ped.LocationTaskRequirements.ForcedScenarios.Clear();
        if (ForcedScenarios != null && ForcedScenarios.Any())
        {
            ped.LocationTaskRequirements.ForcedScenarios.AddRange(ForcedScenarios.ToList());
        }
        //EntryPoint.WriteToConsoleTestLong("ADDED LOCATION REQUIREMENTS");
    }

    public void AddDistanceOffset(Vector3 offsetToAdd)
    {
        Location += offsetToAdd;
    }
    public void SetVehicle(DispatchableVehicle dv)
    {
        DispatchableVehicle = dv;
    }
}

