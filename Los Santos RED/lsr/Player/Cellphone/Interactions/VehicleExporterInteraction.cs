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

public class VehicleExporterInteraction : IContactMenuInteraction
{
    private IContactInteractable Player;
    private UIMenu VehicleExporterMenu;
    private MenuPool MenuPool;
    private IGangs Gangs;
    private IPlacesOfInterest PlacesOfInterest;
    private PhoneContact AnsweredContact;
    private ISettingsProvideable Settings;
    private IModItems ModItems;
    private UIMenu LocationsSubMenu;

    public VehicleExporterInteraction(IContactInteractable player, IGangs gangs, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, IModItems modItems)
    {
        Player = player;
        Gangs = gangs;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        ModItems = modItems;
        MenuPool = new MenuPool();
    }
    public void Start(PhoneContact contact)
    {
        AnsweredContact = contact;
        if (contact == null)
        {
            return;
        }
        VehicleExporterMenu = new UIMenu("", "Select an Option");
        VehicleExporterMenu.RemoveBanner();
        MenuPool.Add(VehicleExporterMenu);


        UIMenuItem currentVehicleQuestion = new UIMenuItem("Current Vehicle", "Ask if the current vehicle is exportable");
        currentVehicleQuestion.Activated += (sender, selectedItem) =>
        {
            VehicleExt vehicleToQuestion = null;
            if(Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
            {
                vehicleToQuestion = Player.CurrentVehicle;
            }
            else if (Player.CurrentLookedAtVehicle != null & Player.CurrentLookedAtVehicle.Vehicle.Exists())
            {
                vehicleToQuestion = Player.CurrentLookedAtVehicle;
            }

            if (vehicleToQuestion == null || !vehicleToQuestion.Vehicle.Exists())
            {
                Game.DisplaySubtitle("No Vehicle Detected");
                return;
            }
            VehicleItem vehicleItem = ModItems.GetVehicle(vehicleToQuestion.Vehicle.Model.Name);
            if(vehicleItem == null)
            {
                Game.DisplaySubtitle("Vehicle Not Found");
                return;
            }
            bool foundMenuItem = false;
            string Response = $"{vehicleItem.Name}~n~";
            foreach (VehicleExporter gl in PlacesOfInterest.PossibleLocations.VehicleExporters.Where(x=> x.ContactName == AnsweredContact.Name))
            {
                MenuItem mi = gl.Menu.Items.FirstOrDefault(x => x.ModItemName == vehicleItem.Name);
                if (mi == null)
                {
                    continue;
                }
                foundMenuItem = true;
                Response += $"{gl.Name} - {mi.SalesPrice.ToString("C0")}~n~";
            }
            if(!foundMenuItem)
            {
                Response = "Vehicle not available for export.";
            }
            Player.CellPhone.AddPhoneResponse(AnsweredContact.Name, AnsweredContact.IconName, Response);
            //sender.Visible = false;
        };
        VehicleExporterMenu.AddItem(currentVehicleQuestion);



        LocationsSubMenu = MenuPool.AddSubMenu(VehicleExporterMenu, "Locations");
        LocationsSubMenu.RemoveBanner();

        foreach (VehicleExporter gl in PlacesOfInterest.PossibleLocations.VehicleExporters.Where(x => x.ContactName == AnsweredContact.Name))
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

            UIMenu locationListsubMenu = MenuPool.AddSubMenu(locationsubMenu, "Vehicle List");
            locationListsubMenu.RemoveBanner();
            gl.AddPriceListItems(locationListsubMenu);
        }
        VehicleExporterMenu.Visible = true;
        InteractionLoop();
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
    private void RequestLocations(VehicleExporter exporter)
    {
        //VehicleExporter exporter = PlacesOfInterest.PossibleLocations.VehicleExporters.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
        if (exporter != null)
        {
            Player.GPSManager.AddGPSRoute(exporter.Name, exporter.EntrancePosition);
            List<string> Replies = new List<string>() {
                    $"Our shop is located on {exporter.FullStreetAddress} come see us.",
                    $"Come check out our shop on {exporter.FullStreetAddress}.",
                    $"You can find our shop on {exporter.FullStreetAddress}.",
                    $"{exporter.FullStreetAddress}.",
                    $"It's on {exporter.FullStreetAddress} come see us.",
                    $"The shop? It's on {exporter.FullStreetAddress}.",

                    };
            Player.CellPhone.AddPhoneResponse(AnsweredContact.Name, AnsweredContact.IconName, Replies.PickRandom());
        }
    }
}

