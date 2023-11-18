using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class FireStation : GameLocation, ILicensePlatePreviewable
{
    private ShopMenu agencyMenu;
    public FireStation(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {

    }
    public FireStation() : base()
    {

    }
    public override string TypeName { get; set; } = "Fire Station";
    public override int MapIcon { get; set; } = 436;
    public string LicensePlatePreviewText { get; set; } = "UNIT1";
    public override void StoreData(IShopMenus shopMenus, IAgencies agencies, IGangs gangs, IZones zones, IJurisdictions jurisdictions, IGangTerritories gangTerritories, INameProvideable Names, ICrimes Crimes, IPedGroups PedGroups, IEntityProvideable world,
        IStreets streets, ILocationTypes locationTypes, ISettingsProvideable settings, IPlateTypes plateTypes, IOrganizations associations, IContacts contacts, IInteriors interiors)
    {
        base.StoreData(shopMenus, agencies, gangs, zones, jurisdictions, gangTerritories, Names, Crimes, PedGroups, world, streets, locationTypes, settings, plateTypes, associations, contacts, interiors);
        if (AssignedAgency == null)
        {
            AssignedAgency = zones.GetZone(EntrancePosition)?.AssignedFireAgency;
        }
    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Enter {Name}";
        return true;
    }
    public override void OnInteract(ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        ModItems = modItems;
        World = world;
        Settings = settings;
        Weapons = weapons;
        Time = time;
        if (IsLocationClosed())
        {
            return;
        }
        if (!CanInteract)
        {
            return;
        }
        if (AssignedAgency == null)
        {
            Game.DisplayHelp("No Agency Assigned");
            return;
        }
        Player.ActivityManager.IsInteractingWithLocation = true;
        CanInteract = false;
        Player.IsTransacting = true;
        GameFiber.StartNew(delegate
        {
            try
            {
                StoreCamera = new LocationCamera(this, Player, Settings, NoEntryCam);
                StoreCamera.Setup();
                CreateInteractionMenu();
                if (Player.IsFireFighter)
                {
                    InteractAsFireFighter(modItems, world, settings, weapons, time);
                }
                else
                {
                    InteractAsOther();
                }
                DisposeInteractionMenu();
                StoreCamera.Dispose();
                Player.IsTransacting = false;
                Player.ActivityManager.IsInteractingWithLocation = false;
                CanInteract = true;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "BarInteract");
        
    }
    private void InteractAsFireFighter(IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time)
    {
        agencyMenu = AssignedAgency.GenerateMenu(ModItems);
        Transaction = new Transaction(MenuPool, InteractionMenu, agencyMenu, this);
        Transaction.LicensePlatePreviewable = this;
        if ((VehicleDeliveryLocations == null || !VehicleDeliveryLocations.Any()) && PossibleVehicleSpawns?.Any() == true)
        {
            List<SpawnPlace> places = new List<SpawnPlace>();
            foreach (ConditionalLocation place in PossibleVehicleSpawns)
            {
                places.Add(new SpawnPlace(place.Location, place.Heading));
            }
            Transaction.VehicleDeliveryLocations = places;
        }
        else
        {
            Transaction.VehicleDeliveryLocations = VehicleDeliveryLocations;
        }
        Transaction.VehiclePreviewPosition = VehiclePreviewLocation;
        Transaction.IsFreeItems = true;
        Transaction.IsFreeWeapons = true;
        Transaction.IsFreeVehicles = true;
        Transaction.IsPurchasing = false;
        Transaction.RotateVehiclePreview = false;
        Transaction.CreateTransactionMenu(Player, modItems, world, settings, weapons, time);
        InteractionMenu.Visible = true;
        Transaction.ProcessTransactionMenu();
        Transaction.DisposeTransactionMenu();
    }
    private void InteractAsOther()
    {
        UIMenuItem addComplaintMenu = new UIMenuItem("Learn About Fire Safety", "Learn about fire safety from one of our most boring teachers!");
        addComplaintMenu.Activated += (sender, selectedItem) =>
        {
            InteractionMenu.Visible = false;
            Game.DisplaySubtitle("You feel smarter");
        };
        InteractionMenu.AddItem(addComplaintMenu);
        InteractionMenu.Visible = true;
        ProcessInteractionMenu();
    }
}

