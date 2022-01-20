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

public class CellPhone
{
    private ICellPhoneable Player;
    private int ContactIndex = 40;
    private UIMenu EmergencyServicesMenu;
    private MenuPool MenuPool;
    private UIMenuItem RequestPolice;
    private UIMenuItem RequestFire;
    private UIMenuItem RequestEMS;
    private IJurisdictions Jurisdictions;

    public CustomiFruit CustomiFruit { get; private set; }
    public CellPhone (ICellPhoneable player, IJurisdictions jurisdictions)
    {
        Player = player;
        CustomiFruit = new CustomiFruit();
        MenuPool = new MenuPool();
        Jurisdictions = jurisdictions;
    }
    public void Setup()
    {
        BreakPoliceCall();
        AddContact("Vagos Boss", ContactIcon.MP_MexBoss);
        AddContact("LOST MC Boss", ContactIcon.MP_BikerBoss);
    }
    public void Update()
    {
        CustomiFruit.Update();
        MenuPool.ProcessMenus();
    }
    public void AddContact(string Name, ContactIcon contactIcon)
    {
        // New contact (wait 4 seconds (4000ms) before picking up the phone)
        iFruitContact contactA = new iFruitContact(Name, ContactIndex);
        contactA.Answered += ContactAnswered;   // Linking the Answered event with our function
        contactA.DialTimeout = 4000;            // Delay before answering
        contactA.Active = true;                 // true = the contact is available and will answer the phone
        contactA.Icon = contactIcon;      // Contact's icon
        CustomiFruit.Contacts.Add(contactA);         // Add the contact to the phone
        ContactIndex++;
    }

    private void ContactAnswered(iFruitContact contact)
    {
        // The contact has answered, we can execute our code
        Game.DisplayNotification("The contact has answered.");

        // We need to close the phone at a moment.
        // We can close it as soon as the contact pick up calling _iFruit.Close().
        // Here, we will close the phone in 5 seconds (5000ms).
        CustomiFruit.Close(5000);
    }
    private void BreakPoliceCall()
    {
        iFruitContact contactA = new iFruitContact("Emergency Services ", 10);
        contactA.Answered += PoliceAnswered;   // Linking the Answered event with our function
        contactA.DialTimeout = 4000;            // Delay before answering
        contactA.Active = true;                 // true = the contact is available and will answer the phone
        contactA.Icon = ContactIcon.Emergency;      // Contact's icon
        CustomiFruit.Contacts.Add(contactA);         // Add the contact to the phone
       // ContactIndex++;
    }
    private void PoliceAnswered(iFruitContact contact)
    {
        // The contact has answered, we can execute our code
        //Game.DisplayNotification("CHAR_CALL911", "CHAR_CALL911", "Emergency Services", "~r~Please Wait", "Your call is important to us, please stay on the line! ~n~~n~If you are being killed, yell a description of your attacker into the phone. Otherwise enjoy some smooth jazz.");

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
            CustomiFruit.Close(2000);
        }, "CellPhone");

        // We need to close the phone at a moment.
        // We can close it as soon as the contact pick up calling _iFruit.Close().
        // Here, we will close the phone in 5 seconds (5000ms).     
    }
    private void OnEmergencyServicesSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {

        string streetName = "";
        string zoneName = "";

        if(Player.CurrentLocation.CurrentStreet != null)
        {
            streetName = $"~HUD_COLOUR_YELLOWLIGHT~{Player.CurrentLocation.CurrentStreet.Name}~s~";
            if(Player.CurrentLocation.CurrentCrossStreet != null)
            {
                streetName += " at ~HUD_COLOUR_YELLOWLIGHT~" + Player.CurrentLocation.CurrentCrossStreet.Name + "~s~ ";
            }
            else
            {
                streetName += " ";
            }
        }
        if (Player.CurrentLocation.CurrentZone != null)
        {
            zoneName = Player.CurrentLocation.CurrentZone.IsSpecificLocation ? "near ~p~" : "in ~p~" +  Player.CurrentLocation.CurrentZone.DisplayName + "~s~";
        }
        string fullText = "";
        if (selectedItem == RequestPolice)
        {     
            if (Player.CurrentLocation != null)
            {
                Agency main = Jurisdictions.GetMainAgency(Player.CurrentLocation.CurrentZone.InternalGameName, ResponseType.LawEnforcement);
                if(main != null)
                {
                    fullText = $"The {main.ColorPrefix}{main.FullName}~s~";
                    //Game.DisplayNotification("CHAR_CALL911", "CHAR_CALL911", "Emergency Services", "~b~Police Service", $"An officer from {main.FullName} is now en route to .");
                }
            }
            if(fullText == "")
            {
                fullText = $"An officer";
            }
            fullText += " is en route to ";
            fullText += streetName;
            fullText += zoneName;
            Game.DisplayNotification("CHAR_CALL911", "CHAR_CALL911", "Emergency Services", "~b~Police Service", fullText);
            Player.CallPolice();
        }
        else if (selectedItem == RequestFire)
        {
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
            fullText += streetName;
            fullText += zoneName;
            Game.DisplayNotification("CHAR_CALL911", "CHAR_CALL911", "Emergency Services", "~r~Fire Service", fullText);
            Player.CallPolice();
        }
        else if (selectedItem == RequestEMS)
        {
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
            fullText += streetName;
            fullText += zoneName;
            Game.DisplayNotification("CHAR_CALL911", "CHAR_CALL911", "Emergency Services", "~h~Medical Service", fullText);
            Player.CallPolice();
        }
        sender.Visible = false;
    }
}
