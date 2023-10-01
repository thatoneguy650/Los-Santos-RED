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
        AddRequestItems();
        AddLocationItems();
        TaxiServiceMenu.Visible = true;
        InteractionLoop();
    }
    private void AddRequestItems()
    {
        RequestSubMenu = MenuPool.AddSubMenu(TaxiServiceMenu, "Request Taxi");
        RequestSubMenu.RemoveBanner();
        UIMenuItem requestTaxiMenuItem = new UIMenuItem("Request Taxi", "Ask for a taxi to be dispatched.");
        requestTaxiMenuItem.Activated += (sender, selectedItem) =>
        {

            string fullText = "";
            if (Player.TaxiManager.RequestService(TaxiFirm))
            {
                fullText = $"{TaxiServiceContact.Name} is en route to ";
                fullText += Player.CurrentLocation?.GetStreetAndZoneString();
            }
            else
            {
                fullText = "No service available in your current location.";
            }
            Player.CellPhone.AddPhoneResponse(TaxiServiceContact.Name, TaxiServiceContact.IconName, fullText);
            sender.Visible = false;

            //AmbientSpawner ambientSpawner = new AmbientSpawner(new DispatchableVehicle("taxi", 100, 100), new DispatchablePerson("a_m_m_socenlat_01", 100, 100),Player.Position,Settings,Crimes,Weapons,Names,World,ModItems,ShopMenus);
            //ambientSpawner.SetPersistent = true;
            //ambientSpawner.Start();
            //string fullText = "";
            //if (ambientSpawner.SpawnedItems)
            //{        
            //    fullText = $"{TaxiServiceContact.Name} is en route to ";
            //    fullText += Player.CurrentLocation?.GetStreetAndZoneString();
            //}
            //else
            //{
            //    fullText = "No service available in your current location.";
            //}
            //Player.CellPhone.AddPhoneResponse(TaxiServiceContact.Name, TaxiServiceContact.IconName, fullText);
            //sender.Visible = false;
        };
        RequestSubMenu.AddItem(requestTaxiMenuItem);
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

