using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

public class TaxiServiceInteraction : IContactMenuInteraction
{
    private IContactInteractable Player;
    private UIMenu TaxiServiceMenu;
    private MenuPool MenuPool;
    private IGangs Gangs;
    private IPlacesOfInterest PlacesOfInterest;
    private ISettingsProvideable Settings;
    private IModItems ModItems;
    private UIMenu LocationsSubMenu;
    private UIMenu PickupSubMenu;
    private UIMenu JobsSubMenu;
    private TaxiServiceContact TaxiServiceContact;
    private UIMenu RequestSubMenu;

    private ICrimes Crimes;
    private IWeapons Weapons;
    private INameProvideable Names;
    private IShopMenus ShopMenus;
    private IEntityProvideable World;
    private TaxiFirm TaxiFirm;

    public TaxiServiceInteraction(IContactInteractable player, IGangs gangs, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, IModItems modItems, TaxiServiceContact taxiServiceContact, ICrimes crimes, IWeapons weapons,
        INameProvideable names, IShopMenus shopMenus, IEntityProvideable world, TaxiFirm taxiFirm)
    {
        Player = player;
        Gangs = gangs;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        ModItems = modItems;
        TaxiServiceContact = taxiServiceContact;
        Crimes = crimes;
        Weapons = weapons;
        Names = names;
        ShopMenus = shopMenus;
        World = world;
        TaxiFirm = taxiFirm;
        MenuPool = new MenuPool();
    }
    public void Start(PhoneContact contact)
    {
        if (TaxiServiceContact == null)
        {
            return;
        }
        TaxiServiceMenu = new UIMenu("", "Select an Option");
        TaxiServiceMenu.RemoveBanner();
        MenuPool.Add(TaxiServiceMenu);
        AddGeneralItems();
        //AddLocationItems();
        TaxiServiceMenu.Visible = true;
        InteractionLoop();
    }
    private void AddGeneralItems()
    {
        TaxiRide ExistingRide = Player.TaxiManager.ActiveRides.FirstOrDefault(x => x.RequestedFirm != null && x.RequestedFirm.ID == TaxiFirm.ID);
        if(ExistingRide == null)
        {
            AddNewRideItems();
        }
        else
        {
            AddExistingRideItems(ExistingRide);
        }
    }
    private void AddNewRideItems()
    {
        string MenuRequestHeader = "Request Taxi";
        string MenuRequestDescription = "Ask for a taxi to be dispatched.";
        string MenuRequestQuickHeader = "Request Taxi (Quick)";
        string MenuRequestQuickDescription = "Ask for a taxi to be dispatched and have it immediately arrive.";
        if (TaxiFirm != null && TaxiFirm.IsRideShare)
        {
            MenuRequestHeader = "Request Driver";
            MenuRequestQuickHeader = "Request Driver (Quick)";
            MenuRequestDescription = "Ask for a driver to be dispatched.";
            MenuRequestQuickDescription = "Ask for a driver to be dispatched and have them immediately arrive.";
        }
        UIMenuItem requestTaxiMenuItem = new UIMenuItem(MenuRequestHeader, MenuRequestDescription);
        requestTaxiMenuItem.Activated += (sender, selectedItem) =>
        {
            string fullText = "";
            if (Player.TaxiManager.RequestService(TaxiFirm, false))
            {
                fullText = $"{TaxiServiceContact.Name} is en route to ";
                fullText += Player.CurrentLocation?.GetStreetAndZoneString();
                TaxiRide taxiRide = Player.TaxiManager.ActiveRides.Where(x => x.RequestedFirm != null && x.RequestedFirm.ID == TaxiFirm.ID).FirstOrDefault();
                if (taxiRide != null && taxiRide.RespondingVehicle != null)
                {
                    fullText += $"~s~~n~~n~Vehicle: {taxiRide.RespondingVehicle.FullName(TaxiFirm.IsRideShare)}";
                }
            }
            else
            {
                fullText = "No service available to your current location. Please try again later.";
            }
            Player.CellPhone.AddPhoneResponse(TaxiServiceContact.Name, TaxiServiceContact.IconName, fullText);
            sender.Visible = false;
        };
        TaxiServiceMenu.AddItem(requestTaxiMenuItem);



        UIMenuItem requestQuickTaxiMenuItem = new UIMenuItem(MenuRequestQuickHeader, MenuRequestQuickDescription);
        requestQuickTaxiMenuItem.Activated += (sender, selectedItem) =>
        {

            string fullText = "";
            if (Player.TaxiManager.RequestService(TaxiFirm, false))
            {
                sender.Visible = false;
                Player.TaxiManager.ActiveRides.FirstOrDefault(x => x.RequestedFirm.ID == TaxiFirm.ID)?.TeleportToPickup();
            }
            else
            {
                fullText = "No service available to your current location. Please try again later.";
                Player.CellPhone.AddPhoneResponse(TaxiServiceContact.Name, TaxiServiceContact.IconName, fullText);
            }

            sender.Visible = false;
        };
        TaxiServiceMenu.AddItem(requestQuickTaxiMenuItem);


    }
    private void AddExistingRideItems(TaxiRide ExistingRide)
    {
        if(ExistingRide == null || ExistingRide.HasPickedUpPlayer)
        {
            return;
        }
        AddPickupItems(ExistingRide);
        UIMenuItem cancelTaxiMenu = new UIMenuItem("Cancel Ride", "Cancel the current ride.");
        cancelTaxiMenu.Activated += (sender, selectedItem) =>
        {
            Player.TaxiManager.CancelRide(ExistingRide, false);
            string fullText = "Ride has been cancelled";
            Player.CellPhone.AddPhoneResponse(TaxiServiceContact.Name, TaxiServiceContact.IconName, fullText);
            sender.Visible = false;
        };
        TaxiServiceMenu.AddItem(cancelTaxiMenu);
    }

    private void AddPickupItems(TaxiRide ExistingRide)
    {
        if (ExistingRide == null || ExistingRide.HasPickedUpPlayer)
        {
            return;
        }
        PickupSubMenu = MenuPool.AddSubMenu(TaxiServiceMenu, "Pickup Location");
        PickupSubMenu.RemoveBanner();

        UIMenuItem updatePickupRegular = new UIMenuItem("Update", "Update the pickup location to nearby street based on your current position.");
        updatePickupRegular.Activated += (sender, selectedItem) =>
        {
            ExistingRide.UpdatePickupLocation();
            string fullText = "Pickup Location Updated";
            Player.CellPhone.AddPhoneResponse(TaxiServiceContact.Name, TaxiServiceContact.IconName, fullText);
            //sender.Visible = false;
        };
        PickupSubMenu.AddItem(updatePickupRegular);


        UIMenuItem updatePickupHere = new UIMenuItem("Set Here", "Set the pickup location as your current position. Be sure there is vehicle access at your current position.");
        updatePickupHere.Activated += (sender, selectedItem) =>
        {
            ExistingRide.SetPickupLocationAtPlayer();
            string fullText = "Pickup Location Updated";
            Player.CellPhone.AddPhoneResponse(TaxiServiceContact.Name, TaxiServiceContact.IconName, fullText);
            //sender.Visible = false;
        };
        PickupSubMenu.AddItem(updatePickupHere);


        UIMenuItem updateSetAtPickup = new UIMenuItem("Quick Pickup", "Teleport the driver to the current pickup location.");
        updateSetAtPickup.Activated += (sender, selectedItem) =>
        {
            //ExistingRide.SetPickupLocationAtPlayer();
            //string fullText = "Pickup Location Updated";
            //Player.CellPhone.AddPhoneResponse(TaxiServiceContact.Name, TaxiServiceContact.IconName, fullText);
            ExistingRide.TeleportToPickup();
            sender.Visible = false;
        };
        PickupSubMenu.AddItem(updateSetAtPickup);


    }

    private void AddLocationItems()
    {
        LocationsSubMenu = MenuPool.AddSubMenu(TaxiServiceMenu, "Locations");
        LocationsSubMenu.RemoveBanner();

        foreach (GameLocation gl in PlacesOfInterest.PossibleLocations.Landmarks.Where(x => 1==0))// x.ContactName == TaxiServiceContact.Name))
        {
            if (!gl.IsEnabled)
            {
                continue;
            }
            UIMenu locationsubMenu = MenuPool.AddSubMenu(LocationsSubMenu, gl.Name);
            locationsubMenu.RemoveBanner();
            UIMenuItem storeAddressRequest = new UIMenuItem("Request Directions", gl.Name + "~n~" + gl.Description + "~n~Address: " + gl.FullStreetAddress);
            storeAddressRequest.Activated += (sender, selectedItem) =>
            {
                RequestLocations(gl);
                sender.Visible = false;
            };
            locationsubMenu.AddItem(storeAddressRequest);
        }
    }
    private void InteractionLoop()
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                while (MenuPool.IsAnyMenuOpen())
                {
                    GameFiber.Yield();
                }
                Player.CellPhone.Close(250);
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "CellPhone");
    }
    public void Update()
    {
        MenuPool.ProcessMenus();
    }
    private void RequestLocations(GameLocation locationName)
    {
        if (locationName != null)
        {
            Player.GPSManager.AddGPSRoute(locationName.Name, locationName.EntrancePosition);
            List<string> Replies = new List<string>() {
                    $"Our shop is located on {locationName.FullStreetAddress} come see us.",
                    $"Come check out our shop on {locationName.FullStreetAddress}.",
                    $"You can find our shop on {locationName.FullStreetAddress}.",
                    $"{locationName.FullStreetAddress}.",
                    $"It's on {locationName.FullStreetAddress} come see us.",
                    $"The shop? It's on {locationName.FullStreetAddress}.",

                    };
            Player.CellPhone.AddPhoneResponse(TaxiServiceContact.Name, TaxiServiceContact.IconName, Replies.PickRandom());
        }
    }
}

