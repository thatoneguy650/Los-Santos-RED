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

    public TaxiServiceInteraction(IContactInteractable player, IGangs gangs, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, IModItems modItems, TaxiServiceContact taxiServiceContact, ICrimes crimes, IWeapons weapons, INameProvideable names, IShopMenus shopMenus, IEntityProvideable world)
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
        //AddJobItems();
        AddRequestItems();
        //AddLocationItems();
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


            AmbientSpawner ambientSpawner = new AmbientSpawner(new DispatchableVehicle("taxi", 100, 100), new DispatchablePerson("a_m_m_socenlat_01", 100, 100),Player.Position,Settings,Crimes,Weapons,Names,World,ModItems,ShopMenus);
            ambientSpawner.SetPersistent = true;


            ambientSpawner.Start();

            string fullText = "";
            if (ambientSpawner.SpawnedItems)
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
        };
        RequestSubMenu.AddItem(requestTaxiMenuItem);
    }
    //private void AddLocationItems()
    //{
    //    LocationsSubMenu = MenuPool.AddSubMenu(TaxiServiceMenu, "Locations");
    //    LocationsSubMenu.RemoveBanner();

    //    foreach (VehicleExporter gl in PlacesOfInterest.PossibleLocations.VehicleExporters.Where(x => x.ContactName == TaxiServiceContact.Name))
    //    {
    //        if (!gl.IsEnabled)
    //        {
    //            continue;
    //        }
    //        UIMenu locationsubMenu = MenuPool.AddSubMenu(LocationsSubMenu, gl.Name);
    //        locationsubMenu.RemoveBanner();

    //        UIMenuItem storeAddressRequest = new UIMenuItem("Request Directions", gl.Name + "~n~" + gl.Description + "~n~Address: " + gl.FullStreetAddress);
    //        storeAddressRequest.Activated += (sender, selectedItem) =>
    //        {
    //            RequestLocations(gl);
    //            sender.Visible = false;
    //        };
    //        locationsubMenu.AddItem(storeAddressRequest);

    //        UIMenu locationListsubMenu = MenuPool.AddSubMenu(locationsubMenu, "Vehicle List");
    //        locationListsubMenu.RemoveBanner();
    //        gl.AddPriceListItems(locationListsubMenu, ModItems);
    //    }
    //}

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
    //private void RequestLocations(VehicleExporter exporter)
    //{
    //    if (exporter != null)
    //    {
    //        Player.GPSManager.AddGPSRoute(exporter.Name, exporter.EntrancePosition);
    //        List<string> Replies = new List<string>() {
    //                $"Our shop is located on {exporter.FullStreetAddress} come see us.",
    //                $"Come check out our shop on {exporter.FullStreetAddress}.",
    //                $"You can find our shop on {exporter.FullStreetAddress}.",
    //                $"{exporter.FullStreetAddress}.",
    //                $"It's on {exporter.FullStreetAddress} come see us.",
    //                $"The shop? It's on {exporter.FullStreetAddress}.",

    //                };
    //        Player.CellPhone.AddPhoneResponse(TaxiServiceContact.Name, TaxiServiceContact.IconName, Replies.PickRandom());
    //    }
    //}

    //private void AddJobItems()
    //{
    //    JobsSubMenu = MenuPool.AddSubMenu(TaxiServiceMenu, "Jobs");
    //    JobsSubMenu.RemoveBanner();

    //    UIMenuItem TaskCancel = new UIMenuItem("Cancel Task", "Tell the gun dealer you can't complete the task.") { RightLabel = "~o~$?~s~" };
    //    TaskCancel.Activated += (sender, selectedItem) =>
    //    {
    //        Player.PlayerTasks.CancelTask(TaxiServiceContact.Name);
    //        sender.Visible = false;
    //    };
    //    if (Player.PlayerTasks.HasTask(TaxiServiceContact.Name))
    //    {
    //        JobsSubMenu.AddItem(TaskCancel);
    //        return;
    //    }
    //    UIMenuItem TransferCars = new UIMenuItem("Transfer", "Transfer some hot vehicles.") { RightLabel = $"~HUD_COLOUR_GREENDARK~{Settings.SettingsManager.TaskSettings.VehicleExporterTransferPaymentMin:C0}-{Settings.SettingsManager.TaskSettings.VehicleExporterTransferPaymentMax:C0}~s~" };
    //    TransferCars.Activated += (sender, selectedItem) =>
    //    {
    //        Player.PlayerTasks.VehicleExporterTasks.TansferStolenCar.Start(TaxiServiceContact);
    //        sender.Visible = false;
    //    };
    //    JobsSubMenu.AddItem(TransferCars);
    //}

}

