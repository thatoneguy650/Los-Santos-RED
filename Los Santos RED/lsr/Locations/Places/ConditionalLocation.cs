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

    public Vector3 Location { get; set; }
    public float Heading { get; set; }
    public float Percentage { get; set; }
    public string AssociationID { get; set; }
    public string RequiredPedGroup { get; set; }
    public string RequiredVehicleGroup { get; set; }
    public bool IsEmpty { get; set; } = true;
    public TaskRequirements SpawnRequirement { get; set; } = TaskRequirements.None;



    public virtual void AttemptSpawn(IDispatchable player, bool isPerson, bool force, IAgencies agencies, IGangs gangs, IZones zones, IJurisdictions jurisdictions, IGangTerritories gangTerritories, ISettingsProvideable settings, IEntityProvideable world, string masterAssociationID, IWeapons weapons, INameProvideable names, ICrimes crimes, IPedGroups pedGroups, IShopMenus shopMenus)
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
        if(!DetermineRun(force))
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
        if (!force && !RandomItems.RandomPercent(Percentage))
        {
            return false;
        }
        return true;
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


}

