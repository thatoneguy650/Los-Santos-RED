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


public class EmergencyServicesInteraction
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

    private int CostToClearWanted
    {
        get
        {
            return Player.WantedLevel * 10000;
        }
    }
    public EmergencyServicesInteraction(IContactInteractable player, IGangs gangs, IPlacesOfInterest placesOfInterest, IJurisdictions jurisdictions)
    {
        Player = player;
        Gangs = gangs;
        PlacesOfInterest = placesOfInterest;
        MenuPool = new MenuPool();
        Jurisdictions = jurisdictions;
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
        //EmergencyServicesMenu.AddItem(RequestFire);
        EmergencyServicesMenu.AddItem(RequestEMS);
        EmergencyServicesMenu.Visible = true;

        GameFiber.StartNew(delegate
        {
            while (MenuPool.IsAnyMenuOpen())
            {
                GameFiber.Yield();
            }
            Player.CellPhone.Close(250);
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
            Agency main = Jurisdictions.GetMainAgency(Player.CurrentLocation.CurrentZone.InternalGameName, ResponseType.LawEnforcement);
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
        Player.CellPhone.CallPolice();
    }
    private void RequestFireAssistance()
    {
        string fullText = "";
        if (Player.CurrentLocation != null)
        {
            Agency main = Jurisdictions.GetMainAgency(Player.CurrentLocation.CurrentZone.InternalGameName, ResponseType.Fire);
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

        //fullText = "Apologies, ~r~firefighting service~s~ is unavailable due to budget cuts.";
        Player.CellPhone.AddPhoneResponse(StaticStrings.EmergencyServicesContactName, "CHAR_CALL911", fullText);
        Player.CellPhone.CallFire();
    }
    private void RequestEMSAssistance()
    {
        string fullText = "";
        if (Player.CurrentLocation != null)
        {
            Agency main = Jurisdictions.GetMainAgency(Player.CurrentLocation.CurrentZone.InternalGameName, ResponseType.Fire);
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
        Player.CellPhone.CallEMS();
    }

}

