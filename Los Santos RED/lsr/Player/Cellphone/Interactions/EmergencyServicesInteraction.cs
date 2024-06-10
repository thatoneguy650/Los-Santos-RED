using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class EmergencyServicesInteraction : IContactMenuInteraction
{
    private IContactInteractable Player;

    private MenuPool MenuPool;
    private UIMenu CopMenu;
    private PhoneContact LastAnsweredContact;
    private UIMenuItem RequestPolice;
    private UIMenuItem RequestFire;
    private UIMenuItem RequestEMS;
    private IGangs Gangs;
    private IPlacesOfInterest PlacesOfInterest;
    private UIMenu EmergencyServicesMenu;
    private string playerCurrentFormattedStreetName;
    private string playerCurrentFormattedZoneName;
    private IJurisdictions Jurisdictions;
    private ISettingsProvideable Settings;
    private ICrimes Crimes;
    private IEntityProvideable World;
    private int CostToClearWanted
    {
        get
        {
            return Player.WantedLevel * 10000;
        }
    }
    public EmergencyServicesInteraction(IContactInteractable player, IGangs gangs, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, IJurisdictions jurisdictions, ICrimes crimes, IEntityProvideable world)
    {
        Player = player;
        Gangs = gangs;
        PlacesOfInterest = placesOfInterest;
        MenuPool = new MenuPool();
        Jurisdictions = jurisdictions;
        Settings = settings;
        Crimes = crimes;
        World = world;
    }
    public void Start(PhoneContact contact)
    {
        EmergencyServicesMenu = new UIMenu(StaticStrings.EmergencyServicesContactName, "Select an Option");
        EmergencyServicesMenu.RemoveBanner();
        MenuPool.Add(EmergencyServicesMenu);
        EmergencyServicesMenu.OnItemSelect += OnEmergencyServicesSelect;
        RequestPolice = new UIMenuItem("Police Assistance");
        RequestFire = new UIMenuItem("Fire Assistance");
        RequestEMS = new UIMenuItem("Medical Service");
        EmergencyServicesMenu.AddItem(RequestPolice);
        EmergencyServicesMenu.AddItem(RequestFire);
        EmergencyServicesMenu.AddItem(RequestEMS);
        EmergencyServicesMenu.Visible = true;

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
    private void OnEmergencyServicesSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == RequestPolice)
        {
            RequestPoliceAssistance();
        }
        else if (selectedItem == RequestFire)
        {
            RequestFireAssistance();
        }
        else if (selectedItem == RequestEMS)
        {
            RequestEMSAssistance();
        }
        sender.Visible = false;
    }
    private void RequestPoliceAssistance()
    {
        string fullText = "";
        if (Player.CurrentLocation != null)
        {
            Agency main = Jurisdictions.GetRespondingAgency(Player.CurrentLocation.CurrentZone.InternalGameName, Player.CurrentLocation.CurrentZone.CountyID, ResponseType.LawEnforcement);
            if (main != null)
            {
                fullText = $"The {main.ColorPrefix}{main.FullName}~s~";
            }
        }
        if (fullText == "")
        {
            fullText = $"An officer";
        }
        fullText += " is en route to ";
        fullText += Player.CurrentLocation?.GetStreetAndZoneString();
        Player.CellPhone.AddPhoneResponse(StaticStrings.EmergencyServicesContactName, "CHAR_CALL911", fullText);
        CallPolice();
    }
    private void RequestFireAssistance()
    {
        string fullText = "";
        if (Player.CurrentLocation != null)
        {
            Agency main = Jurisdictions.GetRespondingAgency(Player.CurrentLocation.CurrentZone.InternalGameName, Player.CurrentLocation.CurrentZone.CountyID, ResponseType.Fire);
            if (main != null)
            {
                fullText = $"The {main.ColorPrefix}{main.FullName}~s~";
            }
        }
        if (fullText == "")
        {
            fullText = $"The fire department";
        }
        fullText += " is en route to ";
        fullText += Player.CurrentLocation?.GetStreetAndZoneString();
        Player.CellPhone.AddPhoneResponse(StaticStrings.EmergencyServicesContactName, "CHAR_CALL911", fullText);
        CallFire();
    }
    private void RequestEMSAssistance()
    {
        string fullText = "";
        if (Player.CurrentLocation != null)
        {
            Agency main = Jurisdictions.GetRespondingAgency(Player.CurrentLocation.CurrentZone.InternalGameName, Player.CurrentLocation.CurrentZone.CountyID, ResponseType.EMS);
            if (main != null)
            {
                fullText = $"The {main.ColorPrefix}{main.FullName}~s~";
            }
        }
        if (fullText == "")
        {
            fullText = $"Emergency medical services";
        }
        fullText += " is en route to ";
        fullText += Player.CurrentLocation?.GetStreetAndZoneString();
       // fullText = "We are sorry, all our ~w~ambulances~s~ are busy. Please try again later.";
        Player.CellPhone.AddPhoneResponse(StaticStrings.EmergencyServicesContactName, "CHAR_CALL911", fullText);
        CallEMS();
    }


    public void CallEMS()
    {
        if (Settings.SettingsManager.EMSSettings.ManageDispatching && Settings.SettingsManager.EMSSettings.ManageTasking)// && World.TotalWantedLevel <= 1)
        {
            Player.Scanner.Reset();
            Player.Investigation.Start(Player.Position, false, false, true, false, Player.CurrentLocation.CurrentInterior);
            Player.Dispatcher.EMSDispatcher.OnMedicalServicesRequested();
            Player.Scanner.OnMedicalServicesRequested();
        }
    }
    public void CallFire()
    {
        if (Settings.SettingsManager.FireSettings.ManageDispatching && Settings.SettingsManager.FireSettings.ManageTasking)//World.TotalWantedLevel <= 1)
        {
            Player.Scanner.Reset();
            Player.Investigation.Start(Player.Position, false, false, false, true, Player.CurrentLocation.CurrentInterior);
            Player.Dispatcher.FireDispatcher.OnFirefightingServicesRequested();
            Player.Scanner.OnFirefightingServicesRequested();
        }
    }
    public void CallPolice()
    {
        Crime ToCallIn = Crimes.CrimeList.FirstOrDefault(x => x.ID == StaticStrings.OfficersNeededCrimeID);
        PedExt violatingCiv = World.Pedestrians.Citizens.Where(x => x.DistanceToPlayer <= 200f).OrderByDescending(x => x.CurrentlyViolatingWantedLevel).FirstOrDefault();
        CrimeSceneDescription description;
        if (violatingCiv != null && violatingCiv.Pedestrian.Exists() && violatingCiv.CrimesCurrentlyViolating.Any())
        {
            description = new CrimeSceneDescription(!violatingCiv.IsInVehicle, Player.IsCop, violatingCiv.Pedestrian.Position, false) { VehicleSeen = null, WeaponSeen = null };
            ToCallIn = violatingCiv.CrimesCurrentlyViolating.OrderBy(x => x.Priority).FirstOrDefault();
        }
        else
        {
            description = new CrimeSceneDescription(false, Player.IsCop, Player.Position) { InteriorSeen = Player.CurrentLocation.CurrentInterior };
        }
        if (Player.IsCop)
        {
            Player.Scanner.Reset();
            Player.Scanner.AnnounceCrime(ToCallIn, description);
            Player.Investigation.Start(Player.Position, false, true, false, false, Player.CurrentLocation.CurrentInterior);
        }
        else
        {
            Player.AddCrime(ToCallIn, false, description.PlaceSeen, description.VehicleSeen, description.WeaponSeen, false, true, false);
        }
    }


}

