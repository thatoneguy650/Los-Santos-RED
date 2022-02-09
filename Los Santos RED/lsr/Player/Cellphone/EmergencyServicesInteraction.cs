using ExtensionsMethods;
using iFruitAddon2;
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
    private iFruitContact LastAnsweredContact;
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
    public void Start(iFruitContact contact)
    {
        EmergencyServicesMenu = new UIMenu("Emergency Services", "Select an Option");
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
            while (EmergencyServicesMenu.Visible)
            {
                GameFiber.Yield();
            }
            Player.CellPhone.Close(2000);
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
        fullText += playerCurrentFormattedStreetName;
        fullText += playerCurrentFormattedZoneName;

        Player.CellPhone.AddPhoneResponse("Emergency Services", "CHAR_CALL911", fullText);


        //Game.DisplayNotification("CHAR_CALL911", "CHAR_CALL911", "Emergency Services", "~o~Response", fullText);
        Player.CallPolice();
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
                //Game.DisplayNotification("CHAR_CALL911", "CHAR_CALL911", "Emergency Services", "~b~Police Service", $"An officer from {main.FullName} is now en route to .");
            }
        }
        if (fullText == "")
        {
            fullText = $"The fire department";
        }
        fullText += " is en route to ";
        fullText += playerCurrentFormattedStreetName;
        fullText += playerCurrentFormattedZoneName;

        fullText = "Apologies, ~r~firefighting service~s~ is unavailable due to budget cuts.";
        Player.CellPhone.AddPhoneResponse("Emergency Services", "CHAR_CALL911", fullText);
        //Game.DisplayNotification("CHAR_CALL911", "CHAR_CALL911", "Emergency Services", "~r~Fire Service", fullText);
        //Player.CallPolice();
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
                //Game.DisplayNotification("CHAR_CALL911", "CHAR_CALL911", "Emergency Services", "~b~Police Service", $"An officer from {main.FullName} is now en route to .");
            }
        }
        if (fullText == "")
        {
            fullText = $"Emergency medical services";
        }
        fullText += " is en route to ";
        fullText += playerCurrentFormattedStreetName;
        fullText += playerCurrentFormattedZoneName;

        fullText = "We are sorry, all our ~w~ambulances~s~ are busy. Please try again later.";
        Player.CellPhone.AddPhoneResponse("Emergency Services", "CHAR_CALL911", fullText);
        //Game.DisplayNotification("CHAR_CALL911", "CHAR_CALL911", "Emergency Services", "~h~Medical Service", fullText);
        //Player.CallPolice();
    }
}

