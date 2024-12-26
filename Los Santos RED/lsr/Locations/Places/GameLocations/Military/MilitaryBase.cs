using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


public class MilitaryBase : GameLocation, ILocationRespawnable,  ILocationAreaRestrictable, IAssaultSpawnable, ILEDispatchableLocation
{
    public MilitaryBase(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {

    }
    public MilitaryBase() : base()
    {

    }
    public override string TypeName { get; set; } = "Military Base";
    public override int MapIcon { get; set; } = 176;
    public Vector3 RespawnLocation { get; set; }
    public float RespawnHeading { get; set; }
    public int MaxAssaultSpawns { get; set; } = 150;
    public List<SpawnPlace> AssaultSpawnLocations { get; set; }
    public bool RestrictAssaultSpawningUsingPedSpawns { get; set; } = false;
    public float AssaultSpawnHeavyWeaponsPercent { get; set; } = 100f;
    public override void StoreData(IShopMenus shopMenus, IAgencies agencies, IGangs gangs, IZones zones, IJurisdictions jurisdictions, IGangTerritories gangTerritories, INameProvideable Names, ICrimes Crimes, IPedGroups PedGroups, IEntityProvideable world,
    IStreets streets, ILocationTypes locationTypes, ISettingsProvideable settings, IPlateTypes plateTypes, IOrganizations associations, IContacts contacts, IInteriors interiors,
    ILocationInteractable player, IModItems modItems, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest, IIssuableWeapons issuableWeapons, IHeads heads, IDispatchablePeople dispatchablePeople)
    {
        base.StoreData(shopMenus, agencies, gangs, zones, jurisdictions, gangTerritories, Names, Crimes, PedGroups, world, streets, locationTypes, settings, plateTypes, associations, contacts, interiors, player, modItems, weapons, time, placesOfInterest, issuableWeapons, heads, dispatchablePeople);
        if (AssignedAgency == null)
        {
            AssignedAgency = zones.GetZone(EntrancePosition)?.AssignedLEAgency;
        }

        if (AssignedAgency == null)
        {
            Zone assignedZone = zones.GetZone(EntrancePosition);
            if (assignedZone != null)
            {
                AssignedAgency = assignedZone.AssignedLEAgency;
            }
            if (AssignedAgency == null && assignedZone != null)
            {
                EntryPoint.WriteToConsole("MILITARY BASE FALLBACK TO COUNTY AGENCY");
                AssignedAgency = jurisdictions.GetRespondingAgency(null, assignedZone.CountyID, ResponseType.LawEnforcement);
            }
        }

    }
    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        MaxAssaultSpawns = 15;
        AssaultSpawnHeavyWeaponsPercent = 80f;
    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.MilitaryBases.Add(this);
        base.AddLocation(possibleLocations);
    }

}

