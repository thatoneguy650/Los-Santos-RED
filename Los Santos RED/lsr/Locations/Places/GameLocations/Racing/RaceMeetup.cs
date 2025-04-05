using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class RaceMeetup : GameLocation
{
    private UIMenu RaceSubMenu;
    private IPlacesOfInterest PlacesOfInterest;
    private IDispatchablePeople DispatchablePeople;
    private IDispatchableVehicles DispatchableVehicles;
    public RaceMeetup() : base()
    {

    }
    public override string TypeName { get; set; } = "Race Meetup";
    public override int MapIcon { get; set; } = 38;
    public override string ButtonPromptText { get; set; }


    public List<string> SupportedTracks { get;set; } = new List<string>();
    public RaceMeetup(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        MenuID = menuID;
    }

    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Race at {Name}";
        return true;
    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.RaceMeetups.Add(this);
        base.AddLocation(possibleLocations);
    }
    public override void StoreData(IShopMenus shopMenus, IAgencies agencies, IGangs gangs, IZones zones, IJurisdictions jurisdictions, IGangTerritories gangTerritories, INameProvideable names, ICrimes crimes, IPedGroups PedGroups, IEntityProvideable world, IStreets streets, ILocationTypes locationTypes, ISettingsProvideable settings, IPlateTypes plateTypes, IOrganizations associations, IContacts contacts, IInteriors interiors, ILocationInteractable player, IModItems modItems, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest, IIssuableWeapons issuableWeapons, IHeads heads, IDispatchablePeople dispatchablePeople, ModDataFileManager modDataFileManager)
    {
        PlacesOfInterest = placesOfInterest;
        DispatchablePeople = dispatchablePeople;
        DispatchableVehicles = modDataFileManager.DispatchableVehicles;
        base.StoreData(shopMenus, agencies, gangs, zones, jurisdictions, gangTerritories, names, crimes, PedGroups, world, streets, locationTypes, settings, plateTypes, associations, contacts, interiors, player, modItems, weapons, time, placesOfInterest, issuableWeapons, heads, dispatchablePeople, modDataFileManager);
    }
    public override void OnInteract()
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
            Interior.Teleport(Player, this, StoreCamera);
        }
        else
        {
            StandardInteract(null, false);
        }
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
                SetupLocationCamera(locationCamera, isInside, true);
                CreateInteractionMenu();
                InteractionMenu.Visible = true;
                GenerateBodyExportMenu();
                ProcessInteractionMenu();
                DisposeInteractionMenu();
                DisposeCamera(isInside);
                DisposeInterior();
                Player.ActivityManager.IsInteractingWithLocation = false;
                CanInteract = true;
                Player.IsTransacting = false;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "RaceMeetupInteract");
    }
    private void GenerateBodyExportMenu()
    {
        RaceSubMenu = MenuPool.AddSubMenu(InteractionMenu, "Find a Race");
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = "Find a race and setup the items.";
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Barber;
        if (HasBannerImage)
        {
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BannerImagePath}");
            RaceSubMenu.SetBannerType(BannerImage);
        }
        VehicleRacesMenu vehicleRaceMenu = new VehicleRacesMenu(MenuPool, RaceSubMenu, null, ModDataFileManager.VehicleRaces, PlacesOfInterest, World, Player, false, null, DispatchableVehicles, SupportedTracks);
        vehicleRaceMenu.Setup();
    }

}

