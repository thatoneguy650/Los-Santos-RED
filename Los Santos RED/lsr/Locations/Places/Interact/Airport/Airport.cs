using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
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
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using System.Xml.Serialization;
using static RAGENativeUI.Elements.UIMenuStatsPanel;

[XmlInclude(typeof(YanktonAiport))]
[XmlInclude(typeof(CayoPericoAirport))]
public class Airport : InteractableLocation, ILocationSetupable
{
    private protected ICrimes Crimes;
    private protected INameProvideable Names;

    private protected List<Carrier> Carriers = new List<Carrier>();

    private LocationCamera StoreCamera;
    private protected ILocationInteractable Player;
    private IModItems ModItems;
    private protected IEntityProvideable World;
    private protected ISettingsProvideable Settings;
    private protected IWeapons Weapons;
    private ITimeControllable Time;
    private protected IPlacesOfInterest PlacesOfInterest;
    private bool IsFlyingToLocation;
   
    public Airport() : base()
    {

    }
    public override string TypeName { get; set; } = "Airport";
    public override int MapIcon { get; set; } = (int)BlipSprite.Airport;
    public override Color MapIconColor { get; set; } = Color.White;
    public override float MapIconScale { get; set; } = 1.0f;
    public override string ButtonPromptText { get; set; }


    

    public string AirportID { get; set; }
    public Vector3 ArrivalPosition { get; set; }
    public float ArrivalHeading { get; set; }



    public Vector3 AirArrivalPosition { get; set; }
    public float AirArrivalHeading { get; set; }

    public List<string> RequestIPLs { get; set; }
    public List<string> RemoveIPLs { get; set; }
    public List<AirportFlight> CommercialFlights { get; set; } = new List<AirportFlight>();
    public HashSet<RoadToggler> RoadToggels { get; set; } = new HashSet<RoadToggler>();
    public HashSet<string> ZonesToEnable { get; set; } = new HashSet<string>();

    public int FuelPrice { get; set; } = 6;

    public Airport(string airportID, Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        AirportID = airportID;
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
        PlacesOfInterest = placesOfInterest;



        if (CanInteract)
        {
            Player.ActivityManager.IsInteractingWithLocation = true;
            CanInteract = false;

            GameFiber.StartNew(delegate
            {
                IsFlyingToLocation = false;
                StoreCamera = new LocationCamera(this, Player);
                StoreCamera.Setup();
                CreateInteractionMenu();
                InteractionMenu.Visible = true;
                SetupMenu();
                ProcessInteractionMenu();
                DisposeInteractionMenu();

                if (IsFlyingToLocation)
                {
                    StoreCamera.StopImmediately();
                }
                else
                {
                    StoreCamera.Dispose();
                }
                Player.ActivityManager.IsInteractingWithLocation = false;
                CanInteract = true;
            }, "HotelInteract");
        }
    }

    public void OnSetDestination(ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        ModItems = modItems;
        World = world;
        Settings = settings;
        Weapons = weapons;
        Time = time;
        PlacesOfInterest = placesOfInterest;
    }

    private void SetupMenu()
    {
        InteractionMenu.SubtitleText = "Pick an Option";
        AddCommercialMenu();
        AddPrivateMenu();
    }
    private void AddCommercialMenu()
    {
        UIMenu commercialSubMenu = MenuPool.AddSubMenu(InteractionMenu, "Commercial Flights");
        commercialSubMenu.SubtitleText = "Pick a Destination";
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = "Need to get away? Chose one of our fine commercial carriers to get you where you need to go";

        foreach (string groupedAirportID in CommercialFlights.GroupBy(x => x.ToAirportID).Select(x => x.Key).Distinct().OrderBy(x => x))
        {
            string DestinationName = groupedAirportID;
            string DestinationDescription = groupedAirportID;

            Airport airportGroup = PlacesOfInterest.PossibleLocations.Airports.FirstOrDefault(x => x.AirportID == groupedAirportID);
            if (airportGroup != null)
            {
                DestinationName = airportGroup.Name;
                DestinationDescription = airportGroup.Description;
            }

            UIMenu destinationSubMenu = MenuPool.AddSubMenu(commercialSubMenu, DestinationName);
            destinationSubMenu.SubtitleText = "Destination: " + DestinationName;
            commercialSubMenu.MenuItems[commercialSubMenu.MenuItems.Count() - 1].Description = DestinationDescription;


            int? minCost = CommercialFlights.Where(x => x.ToAirportID == groupedAirportID)?.Min(x => x.Cost);
            if (minCost.HasValue)
            {
                commercialSubMenu.MenuItems[commercialSubMenu.MenuItems.Count() - 1].RightLabel = $"From ${minCost}";
            }


            foreach (AirportFlight flight in CommercialFlights.Where(x => x.ToAirportID == groupedAirportID))
            {
                bool canFly = false;
                Airport destinationAiport = PlacesOfInterest.PossibleLocations.Airports.FirstOrDefault(x => x.AirportID == flight.ToAirportID);
                if (destinationAiport != null && destinationAiport.IsEnabled)
                {
                    canFly = true;
                }


                Carrier carrier = Carriers.Where(x => x.CarrierID == flight.CarrierID).FirstOrDefault();
                string title = flight.CarrierID;
                string description = flight.Description;
                if (carrier != null)
                {
                    title = carrier.Name;
                    description = "~p~'" + carrier.Description + "'~s~" + "~n~~n~" + flight.Description;
                }
                description += $"~n~~n~Cost: ~r~{flight.Cost:C0}~s~";
                description += $"~n~Flight Time: ~y~{flight.FlightTime}~s~ hour(s)";
                UIMenuItem destinationMenu = new UIMenuItem(title, description) { RightLabel = $"{flight.Cost:C0} - {flight.FlightTime} hour(s)", Enabled = canFly };
                destinationMenu.Activated += (sender, selectedItem) =>
                {
                    if (Player.BankAccounts.Money >= flight.Cost)
                    {
                        Player.BankAccounts.GiveMoney(-1 * flight.Cost);
                        IsFlyingToLocation = true;
                        Game.FadeScreenOut(1000, true);
                        sender.Visible = false;
                        FlyToAirport(destinationAiport, flight.FlightTime);
                    }
                    else
                    {
                        Game.DisplayNotification("CHAR_BLOCKED", "CHAR_BLOCKED", Name, "~r~Insufficient Funds", "We are sorry, we are unable to complete this transation, as you do not have the required funds");
                    }
                };
                destinationSubMenu.AddItem(destinationMenu);

            }
        }
    }
    private void AddPrivateMenu()
    {
        UIMenu privateSubMenu = MenuPool.AddSubMenu(InteractionMenu, "Private Flights");
        privateSubMenu.SubtitleText = "Pick a Destination";
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = "Have your own license and plane?";

        if(!Player.Licenses.HasValidPilotsLicense(Time) && Settings.SettingsManager.WorldSettings.AirportsRequireLicenseForPrivateFlights)
        {
            InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Enabled = false;
            InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = "~n~~n~~r~Invalid License~s~";
        }

        foreach (string groupedAirportID in CommercialFlights.GroupBy(x => x.ToAirportID).Select(x => x.Key).Distinct().OrderBy(x => x))
        {
            string DestinationName = groupedAirportID;
            string DestinationDescription = groupedAirportID;
            AirportFlight commercialFlight = CommercialFlights.Where(x => x.ToAirportID == groupedAirportID).OrderBy(x => x.FlightTime).FirstOrDefault();

            int FlightTime = 2;
            float FuelUsed = 20.0f;
            if(commercialFlight != null) 
            {
                FlightTime = commercialFlight.FlightTime;
                FuelUsed = commercialFlight.FlightTime * 10.0f;
                if(FuelUsed >= 60.0f)
                {
                    FuelUsed = 60.0f;
                }
            }


            Airport airportGroup = PlacesOfInterest.PossibleLocations.Airports.FirstOrDefault(x => x.AirportID == groupedAirportID);
            if (airportGroup != null)
            {
                DestinationName = airportGroup.Name;
                DestinationDescription = airportGroup.Description;
            }
            UIMenu destinationSubMenu = MenuPool.AddSubMenu(privateSubMenu, DestinationName);
            destinationSubMenu.SubtitleText = "Destination: " + DestinationName;
            privateSubMenu.MenuItems[privateSubMenu.MenuItems.Count() - 1].Description = DestinationDescription;
            bool added = false;
            foreach (VehicleExt owned in Player.VehicleOwnership.OwnedVehicles)
            {
                if (owned != null && owned.Vehicle.Exists() && owned.IsAircraft && (!Settings.SettingsManager.WorldSettings.AirportsRequireOwnedPlanesLocal || owned.Vehicle.DistanceTo2D(Player.Character) <= Settings.SettingsManager.WorldSettings.AirportsOwnedPlanesLocalDistance))
                {
                    string VehicleName = owned.FullName(false);
                    string VehicleDescription = owned.FullDescription();
                    UIMenu planeSubMenu = MenuPool.AddSubMenu(destinationSubMenu, VehicleName);
                    planeSubMenu.SubtitleText = "Destination: " + DestinationName;
                    destinationSubMenu.MenuItems[destinationSubMenu.MenuItems.Count() - 1].Description = VehicleDescription;
                    destinationSubMenu.MenuItems[destinationSubMenu.MenuItems.Count() - 1].Enabled = airportGroup?.IsEnabled == true;
                    planeSubMenu.OnMenuOpen += (sender) =>
                    {
                        StoreCamera.HighlightEntity(owned.Vehicle);
                    };
                    planeSubMenu.OnMenuClose += (sender) =>
                    {
                        StoreCamera.ReHighlightStoreWithCamera();
                    };

                    string StartFlightDescription = $"Estimated Flight Time: ~y~{FlightTime}~s~ hour(s)";
                    StartFlightDescription += $"~n~Estimated Fuel Use: ~r~{FuelUsed}~s~%";
                    UIMenuItem startFlightMenu = new UIMenuItem("Depart", StartFlightDescription + "~n~" + VehicleDescription) { Enabled = airportGroup?.IsEnabled == true };
                    startFlightMenu.Activated += (sender, selectedItem) =>
                    {
                        IsFlyingToLocation = true;
                        Game.FadeScreenOut(1000, true);
                        sender.Visible = false;

                        FlyInToAirport(airportGroup, owned, FlightTime, FuelUsed);
                        
                    };
                    planeSubMenu.AddItem(startFlightMenu);

                    Refueling refueling = new Refueling(Player, Name, FuelPrice, owned, Settings);
                    refueling.Setup();
                    float AmountToFill = refueling.UnitsOfFuelNeeded * FuelPrice;
                    string MenuString = $"~n~Price Per Gallon: ~r~${FuelPrice}~s~~n~Fuel Capacity: ~y~{refueling.VehicleToFillFuelTankCapacity}~s~ Gallons~n~Fuel Needed: ~y~{refueling.UnitsOfFuelNeeded}~s~ Gallons";
                    UIMenuNumericScrollerItem<int> AddSomeMenuItem = new UIMenuNumericScrollerItem<int>("Refuel", "Add gasoline by the gallon" + MenuString, 1, refueling.UnitsOfFuelNeeded, 1) { Formatter = v => v + " Gallons - " + (v * FuelPrice).ToString("C0"), Enabled = refueling.UnitsOfFuelNeeded > 1 };
                    AddSomeMenuItem.Value = refueling.UnitsOfFuelNeeded;
                    AddSomeMenuItem.Activated += (sender, selectedItem) =>
                    {
                        if (owned.Vehicle.Exists())
                        {
                            refueling.RefuelQuick(AddSomeMenuItem.Value);
                            refueling.GetFuelStatus();
                            if (refueling.UnitsOfFuelNeeded > 0)
                            {
                                AddSomeMenuItem.Value = refueling.UnitsOfFuelNeeded;
                                AddSomeMenuItem.Maximum = refueling.UnitsOfFuelNeeded;      
                            }
                            else
                            {
                                AddSomeMenuItem.Value = 1;
                                AddSomeMenuItem.Maximum = 1;
                                AddSomeMenuItem.Enabled = false;
                            }
                            AddSomeMenuItem.Description = "Add gasoline by the gallon" + $"~n~Price Per Gallon: ~r~${FuelPrice}~s~~n~Fuel Capacity: ~y~{refueling.VehicleToFillFuelTankCapacity}~s~ Gallons~n~Fuel Needed: ~y~{refueling.UnitsOfFuelNeeded}~s~ Gallons";
                        }
                    };
                    planeSubMenu.AddItem(AddSomeMenuItem);
                    added = true;
                   
                }
            }
            if(!added)
            {
                UIMenuItem noPlanesMenu = new UIMenuItem("No Planes", "No valid planes available") { Enabled = false };
                destinationSubMenu.AddItem(noPlanesMenu);
            }
        }
    }
    public virtual void OnArrive(bool setPos)
    {
        if (RequestIPLs != null)
        {
            foreach (string requestIPL in RequestIPLs)
            {
                NativeFunction.Natives.REQUEST_IPL(requestIPL);
            }
        }
        if (RoadToggels != null)
        {
            foreach (RoadToggler rt in RoadToggels)
            {
                rt.SetRoad(true);
            }
        }
        if (ZonesToEnable != null)
        {
            foreach (string ze in ZonesToEnable)
            {
                NativeFunction.Natives.SET_ZONE_ENABLED(NativeFunction.Natives.GET_ZONE_FROM_NAME_ID<int>(ze), true);
            }
        }
        if (setPos)
        {
            Player.Character.Position = ArrivalPosition;
            Player.Character.Heading = ArrivalHeading;
        }
    }
    public virtual void OnDepart()
    {
        if (RemoveIPLs != null)
        {
            foreach (string removeIPL in RemoveIPLs)
            {
                NativeFunction.Natives.REMOVE_IPL(removeIPL);
            }
        }
        if(RoadToggels != null)
        {
            foreach (RoadToggler rt in RoadToggels)
            {
                rt.SetRoad(false);
            }
        }
        if(ZonesToEnable != null)
        {
            foreach(string ze in ZonesToEnable)
            {
                NativeFunction.Natives.SET_ZONE_ENABLED(NativeFunction.Natives.GET_ZONE_FROM_NAME_ID<int>(ze), false);
            }
        }
    }
    private void FlyToAirport(Airport destinationAiport, int flightTime)
    {
        GameFiber.StartNew(delegate
        {
            Game.FadeScreenOut(1500, true);
            OnDepart();
            destinationAiport.OnSetDestination(Player, ModItems, World, Settings, Weapons, Time, PlacesOfInterest);
            GameFiber.Sleep(1000);
            destinationAiport.OnArrive(true);
            Time.SetDateTime(Time.CurrentDateTime.AddHours(flightTime));
            GameFiber.Sleep(3000);//do the whole shebang ehre
            Game.FadeScreenIn(1500, true);
        }, "DestinationGoTo");
    }
    private void FlyInToAirport(Airport destinationAiport, VehicleExt plane, int flightTime, float fuelSpent)
    {
        GameFiber.StartNew(delegate
        {
            Game.FadeScreenOut(1500, true);
            OnDepart();
            destinationAiport.OnSetDestination(Player, ModItems, World, Settings, Weapons, Time, PlacesOfInterest);
            GameFiber.Sleep(1000);
            destinationAiport.OnArrive(false);
            Time.SetDateTime(Time.CurrentDateTime.AddHours(flightTime));
            if (plane != null && plane.Vehicle.Exists())
            {
                plane.Vehicle.Position = destinationAiport.AirArrivalPosition;
                plane.Vehicle.Heading = destinationAiport.AirArrivalHeading;
                Player.Character.WarpIntoVehicle(plane.Vehicle, -1);
                plane.Vehicle.ApplyForce(new Vector3(0.0f, 500.0f, 0.0f), Vector3.Zero, true, true);
                plane.Vehicle.IsEngineOn = true;
                NativeFunction.Natives.SET_VEHICLE_FORWARD_SPEED(plane.Vehicle, 90f);
                NativeFunction.Natives.SET_VEHICLE_ENGINE_ON(plane.Vehicle, true, true, false);
                NativeFunction.Natives.SET_HELI_BLADES_FULL_SPEED(plane.Vehicle);
                NativeFunction.Natives.CONTROL_LANDING_GEAR(plane.Vehicle, 3);
                if (plane.Vehicle.FuelLevel - fuelSpent < 0.0f)
                {
                    plane.Vehicle.FuelLevel = 0.0f;
                }
                else
                {
                    plane.Vehicle.FuelLevel -= fuelSpent;
                }
            }
            else
            {
                Player.Character.Position = destinationAiport.ArrivalPosition;
                Player.Character.Heading = destinationAiport.ArrivalHeading;
            }
            GameFiber.Sleep(500);//do the whole shebang ehre
            Game.FadeScreenIn(1500, true);
        }, "DestinationGoTo");
    }
    public void Setup(ICrimes crimes, INameProvideable names)
    {
        Crimes = crimes;
        Names = names;
        Carriers = new List<Carrier>()
        {
            new Carrier(StaticStrings.AirHerlerCarrierID,"Air Herler", "For the utmost in luxury"),
            new Carrier(StaticStrings.CaipiraAirwaysCarrierID,"Caipira Airways", "You'll get there when you get there"),
            new Carrier(StaticStrings.SanFierroAirCarrierID,"San Fierro Air", "Its the San Fierro Treat!"),
            new Carrier(StaticStrings.LosSantosAirCarrierID,"Los Santos Air", "Short Flights, Tall Terror"),
            new Carrier(StaticStrings.FlyUSCarrierID,"FlyUS", "Live A Little, Fly With US"),
            new Carrier(StaticStrings.AdiosAirlinesCarrierID,"Adios Airlines", "Say your goodbyes!"),
        };
    }
}

