using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class FightClub : GameLocation
{
    private UIMenu FightSubMenu;
    private IGangs Gangs;
    public FightClub() : base()
    {

    }
    public FightClub(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        MenuID = menuID;
    }
    public override string TypeName { get; set; } = "Fight Club";
    public override int MapIcon { get; set; } = (int)BlipSprite.Rampage;
    public override string ButtonPromptText { get; set; }
    public int MaxBet { get; set; } = 5000;

    public FightClubArena FightClubArena { get; set; }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Fight at {Name}";
        return true;
    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.FightClubs.Add(this);
        base.AddLocation(possibleLocations);
    }
    public override void StoreData(IShopMenus shopMenus, IAgencies agencies, IGangs gangs, IZones zones, IJurisdictions jurisdictions, IGangTerritories gangTerritories, INameProvideable names, ICrimes crimes, IPedGroups PedGroups, IEntityProvideable world, IStreets streets, ILocationTypes locationTypes, ISettingsProvideable settings, IPlateTypes plateTypes, IOrganizations associations, IContacts contacts, IInteriors interiors, ILocationInteractable player, IModItems modItems, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest, IIssuableWeapons issuableWeapons, IHeads heads, IDispatchablePeople dispatchablePeople, ModDataFileManager modDataFileManager)
    {
        Gangs = gangs;
       // PlacesOfInterest = placesOfInterest;
       // DispatchablePeople = dispatchablePeople;
        //DispatchableVehicles = modDataFileManager.DispatchableVehicles;
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
                GenerateFightMenu();
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
    private void GenerateFightMenu()
    {
        FightSubMenu = MenuPool.AddSubMenu(InteractionMenu, "Find a Fight");
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = "Find a fight and setup the items.";
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Alert;
        if (HasBannerImage)
        {
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BannerImagePath}");
            FightSubMenu.SetBannerType(BannerImage);
        }
        FightClubsMenu fightClubsMenu = new FightClubsMenu(MenuPool, FightSubMenu, World, Player, EntryPoint.ModController.Player, EntryPoint.ModController.Player, this, Gangs);
        fightClubsMenu.Setup();
    }
}
