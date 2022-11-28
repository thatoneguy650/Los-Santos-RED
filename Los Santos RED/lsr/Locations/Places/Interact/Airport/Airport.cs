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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using System.Xml.Serialization;
[XmlInclude(typeof(YanktonAiport))]
[XmlInclude(typeof(CayoPericoAirport))]
public class Airport : InteractableLocation
{

    private LocationCamera StoreCamera;
    private ILocationInteractable Player;
    private IModItems ModItems;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private IWeapons Weapons;
    private ITimeControllable Time;
    private IPlacesOfInterest PlacesOfInterest;
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
    public List<string> RequestIPLs { get; set; }
    public List<string> RemoveIPLs { get; set; }
    public List<AirportFlights> Flights { get; set; } = new List<AirportFlights>();
    public HashSet<RoadToggler> RoadToggels { get; set; } = new HashSet<RoadToggler>();
    public HashSet<string> ZonesToEnable { get; set; } = new HashSet<string>();

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
        InteractionMenu.SubtitleText = "Pick a Destination";




        foreach (string groupedAirportID in Flights.GroupBy(x => x.ToAirportID).Select(x => x.Key).Distinct().OrderBy(x => x))
        {
            string DestinationName = groupedAirportID;
            string DestinationDescription = groupedAirportID; 

            Airport airportGroup = PlacesOfInterest.PossibleLocations.Airports.FirstOrDefault(x => x.AirportID == groupedAirportID);
            if(airportGroup != null) 
            {
                DestinationName = airportGroup.Name;
                DestinationDescription = airportGroup.Description;
            }

            UIMenu destinationSubMenu = MenuPool.AddSubMenu(InteractionMenu, DestinationName);
            destinationSubMenu.SubtitleText = "Destination: " + DestinationName;
            InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = DestinationDescription;


            int? minCost = Flights.Where(x => x.ToAirportID == groupedAirportID)?.Min(x => x.Cost);
            if (minCost.HasValue)
            {
                InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].RightLabel = $"From ${minCost}";
            }


            foreach (AirportFlights flight in Flights.Where(x=> x.ToAirportID == groupedAirportID))
            {
                bool canFly = false;
                Airport destinationAiport = PlacesOfInterest.PossibleLocations.Airports.FirstOrDefault(x => x.AirportID == flight.ToAirportID);
                if (destinationAiport != null && destinationAiport.IsEnabled) 
                {
                    canFly = true; 
                }
                UIMenuItem destinationMenu = new UIMenuItem(flight.Airline, flight.Description) { RightLabel = $"{flight.Cost:C0} - {flight.FlightTime} hour(s)", Enabled = canFly };
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
    public virtual void OnArrive()
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
        Player.Character.Position = ArrivalPosition;
        Player.Character.Heading = ArrivalHeading;
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
            Game.FadeScreenOut(1000, true);
            OnDepart();
            destinationAiport.OnSetDestination(Player, ModItems, World, Settings, Weapons, Time, PlacesOfInterest);
            GameFiber.Sleep(1000);
            destinationAiport.OnArrive();
            Time.SetDateTime(Time.CurrentDateTime.AddHours(flightTime));
            GameFiber.Sleep(1000);//do the whole shebang ehre
            Game.FadeScreenIn(1000, true);
        }, "DestinationGoTo");
    }
}

