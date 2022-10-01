using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class CityHall : InteractableLocation
{
    private LocationCamera StoreCamera;

    private ILocationInteractable Player;
    private IModItems ModItems;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private IWeapons Weapons;
    private ITimeControllable Time;
    private UIMenu GovernmentSubMenu;


    private UIMenuItem ChangeNameMenu;
    private UIMenuItem DriversLicenseMenu;
    private UIMenuItem CCWLicenseMenu;
    public CityHall() : base()
    {

    }
    public override string TypeName { get; set; } = "City Hall";
    public override int MapIcon { get; set; } = (int)BlipSprite.GangAttackPackage;
    public override Color MapIconColor { get; set; } = Color.White;
    public override float MapIconScale { get; set; } = 1.0f;
    public override string ButtonPromptText { get; set; }
    public int NameChangeFee { get; set; } = 500;
    public int DriversLicenseFee { get; set; } = 150;
    public int CCWLicenseFee { get; set; } = 1500;
    public CityHall(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {

    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Enter {Name}";
        return true;
    }
    public override void OnInteract(ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time)
    {
        Player = player;
        ModItems = modItems;
        World = world;
        Settings = settings;
        Weapons = weapons;
        Time = time;

        if (CanInteract)
        {
            Player.IsInteractingWithLocation = true;
            CanInteract = false;

            GameFiber.StartNew(delegate
            {
                StoreCamera = new LocationCamera(this, Player);
                StoreCamera.Setup();
                CreateInteractionMenu();
                InteractionMenu.Visible = true;
                InteractionMenu.OnItemSelect += InteractionMenu_OnItemSelect;

                GenerateCityHallMenu();

                ProcessInteractionMenu();
                DisposeInteractionMenu();
                StoreCamera.Dispose();
                Player.IsInteractingWithLocation = false;
                CanInteract = true;
            }, "HotelInteract");
        }
    }
    private void GenerateCityHallMenu()
    {
        ChangeNameMenu = new UIMenuItem("Change Name", NameDescription()) { RightLabel = $"{NameChangeFee:C0}" };
        DriversLicenseMenu = new UIMenuItem("Renew/Purchase Drivers License", DriversLicenseDescription()) { RightLabel = $"{DriversLicenseFee:C0}" };
        CCWLicenseMenu = new UIMenuItem("Renew/Purchase CCW License", CCWLicenseDescription()) { RightLabel = $"{CCWLicenseFee:C0}" };
        InteractionMenu.AddItem(ChangeNameMenu);
        InteractionMenu.AddItem(DriversLicenseMenu);
        InteractionMenu.AddItem(CCWLicenseMenu);
    }
    private string DriversLicenseDescription()
    {
        string DriversLicenseDescription;
        if (Player.Licenses.HasValidDriversLicense(Time))
        {
            DriversLicenseDescription = $"Extend ~p~Drivers License~s~~n~Issue Date: {Player.Licenses.DriversLicense.IssueDate:d}~n~Expiration Date: ~g~{Player.Licenses.DriversLicense.ExpirationDate:d}~s~";
        }
        else
        {
            if (Player.Licenses.HasDriversLicense)
            {
                DriversLicenseDescription = $"Renew your expired ~p~Drivers License~s~ ~n~Issue Date: {Player.Licenses.DriversLicense.IssueDate:d}~n~Expiration Date: ~r~{Player.Licenses.DriversLicense.ExpirationDate:d}~s~";
            }
            else
            {
                DriversLicenseDescription = "Purchase a new ~p~Drivers License~s~";
            }
        }
        return DriversLicenseDescription;
    }
    private string CCWLicenseDescription()
    {
        string CCWLicenseDescription;
        if (Player.Licenses.HasValidCCWLicense(Time))
        {
            CCWLicenseDescription = $"Extend ~p~CCW License~s~~n~Issue Date: {Player.Licenses.CCWLicense.IssueDate:d}~n~Expiration Date: ~g~{Player.Licenses.CCWLicense.ExpirationDate:d}~s~";
        }
        else
        {
            if (Player.Licenses.HasCCWLicense)
            {
                CCWLicenseDescription = $"Renew your expired ~p~CCW License~s~ ~n~Issue Date: {Player.Licenses.CCWLicense.IssueDate:d}~n~Expiration Date: ~r~{Player.Licenses.CCWLicense.ExpirationDate:d}~s~";
            }
            else
            {
                CCWLicenseDescription = "Purchase a new ~p~CCW License~s~";
            }
        }
        return CCWLicenseDescription;
    }
    private string NameDescription()
    {
        return $"Change your characters name. ~n~Legal Name: ~p~{Player.PlayerName}~s~";
    }
    private void InteractionMenu_OnItemSelect(RAGENativeUI.UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if(selectedItem == ChangeNameMenu)
        {
            if(Player.BankAccounts.Money >= NameChangeFee)
            {
                string NewName = NativeHelper.GetKeyboardInput("");
                if(NewName != "")
                {
                    Player.BankAccounts.GiveMoney(-1 * NameChangeFee);
                    Player.ChangeName(NewName);
                    ChangeNameMenu.Description = NameDescription();
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~g~Purchase", $"You have successfully changed your name to {NewName}");
                }
                else
                {
                    Game.DisplayNotification("CHAR_BLOCKED", "CHAR_BLOCKED", Name, "~r~Cancelled", "Name change cancelled, you have not been charged");
                } 
            }
            else
            {
                Game.DisplayNotification("CHAR_BLOCKED", "CHAR_BLOCKED", Name, "~r~Insufficient Funds", "We are sorry, we are unable to complete this transation, as you do not have the required funds");
            }
        }
        else if (selectedItem == DriversLicenseMenu)
        {
            if(Player.BankAccounts.Money >= DriversLicenseFee)
            {
                if (Player.Licenses.HasValidDriversLicense(Time))
                {
                    Player.BankAccounts.GiveMoney(-1 * DriversLicenseFee);
                    Player.Licenses.DriversLicense.IssueLicense(Time, 12);
                    DriversLicenseMenu.Description = DriversLicenseDescription();
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~g~Purchase", $"You have updated your drivers license.~n~Issue Date: {Player.Licenses.DriversLicense.IssueDate:d}~n~Expiration Date: {Player.Licenses.DriversLicense.ExpirationDate:d}");
                }
                else
                {
                    Player.BankAccounts.GiveMoney(-1 * DriversLicenseFee);
                    Player.Licenses.DriversLicense = new DriversLicense();
                    Player.Licenses.DriversLicense.IssueLicense(Time, 12);
                    DriversLicenseMenu.Description = DriversLicenseDescription();
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~g~Purchase", $"Drivers license issued.~n~Issue Date: {Player.Licenses.DriversLicense.IssueDate:d}~n~Expiration Date: {Player.Licenses.DriversLicense.ExpirationDate:d}");
                }
            }
            else
            {
                Game.DisplayNotification("CHAR_BLOCKED", "CHAR_BLOCKED", Name, "~r~Insufficient Funds", "We are sorry, we are unable to complete this transation, as you do not have the required funds");
            }
        }
        else if (selectedItem == CCWLicenseMenu)
        {
            if (Player.BankAccounts.Money >= CCWLicenseFee)
            {
                if (Player.Licenses.HasCCWLicense && Player.Licenses.CCWLicense.IsValid(Time))
                {
                    Player.BankAccounts.GiveMoney(-1 * CCWLicenseFee);
                    Player.Licenses.CCWLicense.IssueLicense(Time, 12);
                    CCWLicenseMenu.Description = CCWLicenseDescription();
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~g~Purchase", $"You have updated your CCW license.~n~Issue Date: {Player.Licenses.CCWLicense.IssueDate:d}~n~Expiration Date: {Player.Licenses.CCWLicense.ExpirationDate:d}");
                }
                else
                {
                    Player.BankAccounts.GiveMoney(-1 * CCWLicenseFee);
                    Player.Licenses.CCWLicense = new CCWLicense();
                    Player.Licenses.CCWLicense.IssueLicense(Time, 12);
                    CCWLicenseMenu.Description = CCWLicenseDescription();
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~g~Purchase", $"CCW license issued.~n~Issue Date: {Player.Licenses.CCWLicense.IssueDate:d}~n~Expiration Date: {Player.Licenses.CCWLicense.ExpirationDate:d}");
                }
            }
            else
            {
                Game.DisplayNotification("CHAR_BLOCKED", "CHAR_BLOCKED", Name, "~r~Insufficient Funds", "We are sorry, we are unable to complete this transation, as you do not have the required funds");
            }
        }
    }
}

