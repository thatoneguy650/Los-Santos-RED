using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class BarberShop : GameLocation
{
    public BarberShop(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {

    }
    public BarberShop() : base()
    {

    }
    public override string TypeName { get; set; } = "Barber Shop";
    public override int MapIcon { get; set; } = (int)BlipSprite.Barber;
    public bool AllowsHaircuts { get; set; } = true;
    public bool AllowsBeards { get; set; } = true;
    public bool AllowsMakeup { get; set; } = true;
    public string PedVariationShopMenuID { get; set; } = "GenericBarberShop";
    [XmlIgnore]
    public PedVariationShopMenu PedVariationShopMenu { get; set; } = new PedVariationShopMenu();
    [XmlIgnore]
    public BarberShopInterior BarberShopInterior { get; set; }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Shop At {Name}";
        return true;
    }
    public override void StoreData(IShopMenus shopMenus, IAgencies agencies, IGangs gangs, IZones zones, IJurisdictions jurisdictions, IGangTerritories gangTerritories, INameProvideable names, ICrimes crimes, IPedGroups PedGroups, IEntityProvideable world, IStreets streets, ILocationTypes locationTypes, ISettingsProvideable settings, IPlateTypes plateTypes, IOrganizations associations, IContacts contacts, IInteriors interiors, ILocationInteractable player, IModItems modItems, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest, IIssuableWeapons issuableWeapons, IHeads heads, IDispatchablePeople dispatchablePeople)
    {
        if (HasInterior)
        {
            BarberShopInterior = interiors.PossibleInteriors.BarberShopInteriors.Where(x => x.LocalID == InteriorID).FirstOrDefault();
            interior = BarberShopInterior;
            if (BarberShopInterior != null)
            {
                BarberShopInterior.SetBarberShop(this);
            }
        }
        PedVariationShopMenu = shopMenus.GetPedVariationMenu(PedVariationShopMenuID);
        base.StoreData(shopMenus, agencies, gangs, zones, jurisdictions, gangTerritories, names, crimes, PedGroups, world, streets, locationTypes, settings, plateTypes, associations, contacts, interiors, player, modItems, weapons, time, placesOfInterest, issuableWeapons, heads, dispatchablePeople);
    }
    public override void OnInteract()//ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest)
    {
        if (IsLocationClosed())
        {
            return;
        }
        if (!CanInteract)
        {
            return;
        }
        if (Interior != null && Interior.IsTeleportEntry)
        {
            DoEntranceCamera(true);
            BarberShopInterior.SetBarberShop(this);
            Interior.Teleport(Player, this, StoreCamera);
        }
        else
        {
            StandardInteract(null, false);
        }
    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.BarberShops.Add(this);
        base.AddLocation(possibleLocations);
    }
}

