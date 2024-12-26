using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class Prison : GameLocation, ILocationRespawnable, ILocationAreaRestrictable, IAssaultSpawnable, ILEDispatchableLocation
{
    public Prison(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {

    }
    public Prison() : base()
    {

    }
    public override string TypeName { get; set; } = "Prison";
    public override int MapIcon { get; set; } = 188;
    public Vector3 RespawnLocation { get; set; }
    public float RespawnHeading { get; set; }

    public int MaxAssaultSpawns { get; set; } = 15;
    public List<SpawnPlace> AssaultSpawnLocations { get; set; }

    public bool RestrictAssaultSpawningUsingPedSpawns { get; set; } = false;
    public float AssaultSpawnHeavyWeaponsPercent { get; set; } = 80f;
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Enter {Name}";
        return true;
    }
    public override void StoreData(IShopMenus shopMenus, IAgencies agencies, IGangs gangs, IZones zones, IJurisdictions jurisdictions, IGangTerritories gangTerritories, INameProvideable names, ICrimes crimes, IPedGroups PedGroups, IEntityProvideable world,
        IStreets streets, ILocationTypes locationTypes, ISettingsProvideable settings, IPlateTypes plateTypes, IOrganizations associations, IContacts contacts, IInteriors interiors,
        ILocationInteractable player, IModItems modItems, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest, IIssuableWeapons issuableWeapons, IHeads heads, IDispatchablePeople dispatchablePeople)
    {
        base.StoreData(shopMenus, agencies, gangs, zones, jurisdictions, gangTerritories, names, crimes, PedGroups, world, streets, locationTypes, settings, plateTypes, associations, contacts, interiors, player, modItems, weapons, time, placesOfInterest, issuableWeapons, heads, dispatchablePeople);
        if (AssignedAgency == null)
        {
            Zone assignedZone = zones.GetZone(EntrancePosition);
            if (assignedZone != null)
            {
                AssignedAgency = assignedZone.AssignedLEAgency;
            }
            if (AssignedAgency == null && assignedZone != null)
            {
                EntryPoint.WriteToConsole("PRISON FALLBACK TO COUNTY AGENCY");
                AssignedAgency = jurisdictions.GetRespondingAgency(null, assignedZone.CountyID, ResponseType.LawEnforcement);
            }
        }
    }
    public override void OnInteract()//ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest)
    {
        //Player = player;
        //ModItems = modItems;
        //World = world;
        //Settings = settings;
        //Weapons = weapons;
        //Time = time;
        if (IsLocationClosed())
        {
            return;
        }
        if (CanInteract)
        {
            Game.DisplayHelp("Police Personnel Only.~r~WIP~s~");
        }
    }
    public override void AddDistanceOffset(Vector3 offsetToAdd)
    {
        if (RespawnLocation != Vector3.Zero)
        {
            RespawnLocation += offsetToAdd;
        }
        base.AddDistanceOffset(offsetToAdd);
    }
    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        MaxAssaultSpawns = 15;
        AssaultSpawnHeavyWeaponsPercent = 80f;
    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.Prisons.Add(this);
        base.AddLocation(possibleLocations);
    }
}

