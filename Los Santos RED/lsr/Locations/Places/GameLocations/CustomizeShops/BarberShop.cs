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
    public int StandardMaleHaircutPrice { get; set; } = 25;
    public int StandardFemaleHaircutPrice { get; set; } = 35;
    public int StandardBeardTrimPrice { get; set; } = 30;
    public int StandardHairColoringPrice { get; set; } = 45;
    public int StandardMakeupPrice { get; set; } = 55;
    public int PremiumHaircutExtra { get; set; } = 15;
    public int PremiumBearTrimExtra { get; set; } = 10;
    public int PremiumColoringExtra { get; set; } = 25;
    public int PremiumMakeupExtra { get; set; } = 25;
    public bool AllowsMakeup { get; set; } = true;
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
        base.StoreData(shopMenus, agencies, gangs, zones, jurisdictions, gangTerritories, names, crimes, PedGroups, world, streets, locationTypes, settings, plateTypes, associations, contacts, interiors, player, modItems, weapons, time, placesOfInterest, issuableWeapons, heads, dispatchablePeople);
    }
    public override void StandardInteract(LocationCamera locationCamera, bool isInside)
    {
        FakeCustomizeInterior fakeCustomizeInterior = new FakeCustomizeInterior(Player,World, Settings, ModItems, World.ModDataFileManager.ClothesNames, this,
            new Vector3(124.8145f, -747.5364f, 241.152f),//ANim Stuff
            new Vector3(0f, 0f, -388.969f),

            new Vector3(124.8145f, -747.5364f, 242.152f),//Chair Pos
             69.98466f,//239.2449f,//252.4096f

            new Vector3(123.7631f, -745.737f, 242.152f),//Player Pos
            216.5336f,

            new Vector3(122.921f, -748.2304f, 241.652f),//Barber Pos
            305.9934f,

            new Vector3(125.9692f, -748.0427f, 242.6188f), //Camera
            new Vector3(-0.8899927f, 0.4485282f, -0.08206987f),
            new Rotator(-4.707552f, 4.283317E-06f, 63.25334f)
            );
        fakeCustomizeInterior.Start();
    }
}

