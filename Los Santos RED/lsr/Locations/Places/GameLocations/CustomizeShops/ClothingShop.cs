using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class ClothingShop : GameLocation
{
    public ClothingShop() : base()
    {

    }
    public override string TypeName { get; set; } = "Clothing Store";
    public override int MapIcon { get; set; } = (int)BlipSprite.ClothesStore;
    public override string ButtonPromptText { get; set; }
    public Vector3 ChangingRoomLocation { get; set; }
    public override int RegisterCashMin { get; set; } = 300;
    public override int RegisterCashMax { get; set; } = 1550;
    public override int MinPriceRefreshHours { get; set; } = 12;
    public override int MaxPriceRefreshHours { get; set; } = 24;
    public override int MinRestockHours { get; set; } = 12;
    public override int MaxRestockHours { get; set; } = 24;



    public string PedClothingShopMenuID { get; set; } = "GenericClothesShop";
    [XmlIgnore]
    public PedClothingShopMenu PedClothingShopMenu { get; set; } = new PedClothingShopMenu();
    [XmlIgnore]
    public ClothingShopInterior ClothingShopInterior { get; set; }


    public ClothingShop(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID, Vector3 changingRoomLocation) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        MenuID = menuID;
        ChangingRoomLocation = changingRoomLocation;
    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Shop At {Name}";
        return true;
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
            ClothingShopInterior.SetClothingShop(this);
            Interior.Teleport(Player, this, StoreCamera);
        }
        else
        {
            StandardInteract(null, false);
        }
    }
    public override void StoreData(IShopMenus shopMenus, IAgencies agencies, IGangs gangs, IZones zones, IJurisdictions jurisdictions, IGangTerritories gangTerritories, INameProvideable names, ICrimes crimes, IPedGroups PedGroups, IEntityProvideable world, IStreets streets, ILocationTypes locationTypes, ISettingsProvideable settings, IPlateTypes plateTypes, IOrganizations associations, IContacts contacts, IInteriors interiors, ILocationInteractable player, IModItems modItems, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest, IIssuableWeapons issuableWeapons, IHeads heads, IDispatchablePeople dispatchablePeople, ModDataFileManager modDataFileManager)
    {
        if (HasInterior)
        {
            ClothingShopInterior = interiors.PossibleInteriors.ClothingShopInteriors.Where(x => x.LocalID == InteriorID).FirstOrDefault();
            interior = ClothingShopInterior;
            if (ClothingShopInterior != null)
            {
                ClothingShopInterior.SetClothingShop(this);
            }
        }
        PedClothingShopMenu = shopMenus.GetPedClothingShopMenu(PedClothingShopMenuID);
        base.StoreData(shopMenus, agencies, gangs, zones, jurisdictions, gangTerritories, names, crimes, PedGroups, world, streets, locationTypes, settings, plateTypes, associations, contacts, interiors, player, modItems, weapons, time, placesOfInterest, issuableWeapons, heads, dispatchablePeople, modDataFileManager);
    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.ClothingShops.Add(this);
        base.AddLocation(possibleLocations);
    }
}

