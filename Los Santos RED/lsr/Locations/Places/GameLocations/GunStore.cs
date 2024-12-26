using LosSantosRED.lsr.Interface;
using Mod;
using Rage;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class GunStore : GameLocation
{
    private UIMenuItem completeTask;
    public GunStore() : base()
    {

    }

    public List<SpawnPlace> ParkingSpaces = new List<SpawnPlace>();
    public override bool ShowsOnDirectory { get; set; } = false;
    public override string TypeName { get; set; } = "Gun Store";
    public override int MapIcon { get; set; } = (int)BlipSprite.AmmuNation;
    public override string ButtonPromptText { get; set; }
    public int MoneyToUnlock { get; set; } = 0;
    public string ContactName { get; set; } = "";
    public override int RegisterCashMin { get; set; } = 1000;
    public override int RegisterCashMax { get; set; } = 2550;
    [XmlIgnore]
    public PhoneContact PhoneContact { get; set; }

    public GunStore(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        MenuID = menuID;
        ButtonPromptText = $"Shop at {Name}";
    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Shop At {Name}";
        return true;
    }
    public override void StoreData(IShopMenus shopMenus, IAgencies agencies, IGangs gangs, IZones zones, IJurisdictions jurisdictions, IGangTerritories gangTerritories, INameProvideable names, ICrimes crimes, 
        IPedGroups PedGroups, IEntityProvideable world, IStreets streets, ILocationTypes locationTypes, ISettingsProvideable settings, IPlateTypes plateTypes, IOrganizations associations, IContacts contacts, IInteriors interiors,
        ILocationInteractable player, IModItems modItems, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest, IIssuableWeapons issuableWeapons, IHeads heads, IDispatchablePeople dispatchablePeople)
    {
        PhoneContact = contacts.GetContactData(ContactName);
        base.StoreData(shopMenus, agencies, gangs, zones, jurisdictions, gangTerritories, names, crimes, PedGroups, world, streets, locationTypes, settings, plateTypes, associations, contacts, interiors, player, modItems, weapons, time, placesOfInterest, issuableWeapons, heads, dispatchablePeople);
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
        if (!CanInteract)
        {
            return;
        }
        if (Interior != null && Interior.IsTeleportEntry)
        {
            DoEntranceCamera(false);
            Interior.Teleport(Player, this, StoreCamera);
        }
        else
        {
            StandardInteract(null, false);
        }
    }
    protected override bool ShouldSpawnVendor() => PhoneContact == null || !Player.RelationshipManager.GetOrCreate(PhoneContact).IsHostile;
    protected override bool IsLocationClosed()
    {
        if (PhoneContact != null && Player.RelationshipManager.GetOrCreate(PhoneContact).IsHostile)
        {
            Game.DisplayHelp("Increase your reputation to access.");
            return true;
        }
        return base.IsLocationClosed();
    }
    public override void StandardInteract(LocationCamera locationCamera, bool isInside)
    {
        Player.ActivityManager.IsInteractingWithLocation = true;
        CanInteract = false;
        Player.IsTransacting = true;
        GameFiber.StartNew(delegate
        {
            try
            {
                SetupLocationCamera(locationCamera, isInside, false);
                CreateInteractionMenu();
                HandleVariableItems();
                Transaction = new Transaction(MenuPool, InteractionMenu, Menu, this);
                Transaction.UseAccounts = false;
                Transaction.CreateTransactionMenu(Player, ModItems, World, Settings, Weapons, Time);
                InteractionMenu.Visible = true;
                Transaction.ProcessTransactionMenu();
                Player.RelationshipManager.OnInteracted(PhoneContact, Transaction.MoneySpent, (Transaction.MoneySpent) / 5);
                Transaction.DisposeTransactionMenu();
                DisposeInteractionMenu();
                DisposeCamera(isInside);
                DisposeInterior();
                Player.ActivityManager.IsInteractingWithLocation = false;
                Player.IsTransacting = false;
                CanInteract = true;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "GangDenInteract");
    }
    public override void AddDistanceOffset(Vector3 offsetToAdd)
    {
        foreach (SpawnPlace sp in ParkingSpaces)
        {
            sp.AddDistanceOffset(offsetToAdd);
        }
        base.AddDistanceOffset(offsetToAdd);
    }
    public override void OnVendorKilledByPlayer(Merchant merchant, IViolateable player, IZones zones, IGangTerritories gangTerritories)
    {
        player.RelationshipManager.OnVendorKilledByPlayer(PhoneContact, merchant, player, zones, gangTerritories);
        base.OnVendorKilledByPlayer(merchant, player, zones, gangTerritories);
    }
    public override void OnVendorInjuredByPlayer(Merchant merchant, IViolateable player, IZones zones, IGangTerritories gangTerritories)
    {
        player.RelationshipManager.OnVendorInjuredByPlayer(PhoneContact, merchant, player, zones, gangTerritories);
        base.OnVendorInjuredByPlayer(merchant, player, zones, gangTerritories);
    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.GunStores.Add(this);
        base.AddLocation(possibleLocations);
    }
}

