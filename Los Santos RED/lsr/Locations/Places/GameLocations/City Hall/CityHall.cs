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

public class CityHall : GameLocation
{
    //private LocationCamera StoreCamera;

    //private ILocationInteractable Player;
    //private IModItems ModItems;
    //private IEntityProvideable World;
    //private ISettingsProvideable Settings;
    //private IWeapons Weapons;
    //private ITimeControllable Time;
    private UIMenu GovernmentSubMenu;


    private UIMenuItem ChangeNameMenu;
    private UIMenuItem DriversLicenseMenu;
    private UIMenuItem CCWLicenseMenu;
    private UIMenuItem PilotsLicenseMenu;

    public CityHall() : base()
    {

    }
    public override string TypeName { get; set; } = "City Hall";
    public override int MapIcon { get; set; } = (int)BlipSprite.GangAttackPackage;
    public override string ButtonPromptText { get; set; }
    public int NameChangeFee { get; set; } = 500;
    public int DriversLicenseFee { get; set; } = 150;
    public int CCWLicenseFee { get; set; } = 1500;
    public int PilotsLicenseFee { get; set; } = 2500;
    public CityHall(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {

    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Enter {Name}";
        return true;
    }
    public override void OnInteract()//ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest)
    {
        //Player = player;
        //ModItems = modItems;
        //World = world;
        //Settings = settings;
        //Weapons = weapons;
        //Time = time;

        if (IsLocationClosed())
        {
            return;
        }
        if (!CanInteract)
        {
            return;
        }
        if (Interior != null && Interior.IsTeleportEntry)
        {
            DoEntranceCamera(true);
            Interior.Teleport(Player, this, StoreCamera);
        }
        else
        {
            StandardInteract(null, false);
        }
    }
    public override void StandardInteract(LocationCamera locationCamera, bool isInside)
    {
        Player.ActivityManager.IsInteractingWithLocation = true;
        CanInteract = false;
        Player.IsTransacting = true;
        GameFiber.StartNew(delegate
        {
            try
            {
                SetupLocationCamera(locationCamera, isInside, true);
                CreateInteractionMenu();
                InteractionMenu.Visible = true;
                InteractionMenu.OnItemSelect += InteractionMenu_OnItemSelect;
                GenerateCityHallMenu();
                ProcessInteractionMenu();
                DisposeInteractionMenu();
                DisposeCamera(isInside);
                DisposeInterior();
                Player.ActivityManager.IsInteractingWithLocation = false;
                CanInteract = true;
                Player.IsTransacting = false;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }

        }, "CityHallInteract");
    }
    private void GenerateCityHallMenu()
    {
        ChangeNameMenu = new UIMenuItem("Change Name", NameDescription()) { RightLabel = $"{NameChangeFee:C0}" };
        DriversLicenseMenu = new UIMenuItem("Renew/Purchase Drivers License", DriversLicenseDescription()) { RightLabel = $"{DriversLicenseFee:C0}" };
        CCWLicenseMenu = new UIMenuItem("Renew/Purchase CCW License", CCWLicenseDescription()) { RightLabel = $"{CCWLicenseFee:C0}" };
        PilotsLicenseMenu = new UIMenuItem("Renew/Purchase Pilots License", PilotsLicenseDescription()) { RightLabel = $"{PilotsLicenseFee:C0}" };

        InteractionMenu.AddItem(ChangeNameMenu);
        InteractionMenu.AddItem(DriversLicenseMenu);
        InteractionMenu.AddItem(CCWLicenseMenu);
        InteractionMenu.AddItem(PilotsLicenseMenu);


        GenerateTests();
    }

    private void GenerateTests()
    {
        //List<LicenseQuestion> Questions = new List<LicenseQuestion>
        //{
        //    new LicenseQuestion("If you want to get off from a freeway, but you missed your exit, you should:", 1, 
        //        new List<LicenseAnswer>() {
        //            new LicenseAnswer("Go to the next exit, and get off the freeway there", true),
        //            new LicenseAnswer("Make a U-turn through the median", false),
        //            new LicenseAnswer("Pull onto the shoulder and back your car to the exit", false),
        //            new LicenseAnswer("Flag down a police officer for an escort back to your exit", false), 
        //        }),


        //    new LicenseQuestion("Minimum speed signs are designed to", 1,
        //        new List<LicenseAnswer>() {               
        //            new LicenseAnswer("Show current local road conditions", false),
        //            new LicenseAnswer("Test future traffic signal needs", false),
        //            new LicenseAnswer("Keep traffic flowing smoothly", true),
        //            new LicenseAnswer("Assure pedestrian safety", false),
        //        }),

        //    new LicenseQuestion("A double solid yellow line means:", 1,
        //        new List<LicenseAnswer>() {       
        //            new LicenseAnswer("You may cross over the lines to pass slower traffic on the right.", false),
        //            new LicenseAnswer("You may not turn across the line to enter or exit a roadway.", false),
        //            new LicenseAnswer("You may turn across the line to enter or exit a roadway.", true),
        //        }),
        //    new LicenseQuestion("If using your high beams, you must dim your lights when oncoming traffic is within:", 1,
        //        new List<LicenseAnswer>() {
        //            new LicenseAnswer("500 feet.", true),
        //            new LicenseAnswer("450 feet.", false),
        //            new LicenseAnswer("400 feet", false),
        //            new LicenseAnswer("200 feet", false),
        //        }),
        //};
            


        //LicenseTest lt = new LicenseTest("Drivers Test", Questions);

        //lt.StartTest(DriversLicenseMenu);
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

    private string PilotsLicenseDescription()
    {
        string PilotsLicenseDescription;
        if (Player.Licenses.HasValidPilotsLicense(Time))
        {
            PilotsLicenseDescription = $"Extend ~p~Pilots License~s~~n~Issue Date: {Player.Licenses.PilotsLicense.IssueDate:d}~n~Expiration Date: ~g~{Player.Licenses.PilotsLicense.ExpirationDate:d}~s~";
        }
        else
        {
            if (Player.Licenses.HasPilotsLicense)
            {
                PilotsLicenseDescription = $"Renew your expired ~p~Pilots License~s~ ~n~Issue Date: {Player.Licenses.PilotsLicense.IssueDate:d}~n~Expiration Date: ~r~{Player.Licenses.PilotsLicense.ExpirationDate:d}~s~";
            }
            else
            {
                PilotsLicenseDescription = "Purchase a new ~p~Pilots License~s~";
            }
        }
        return PilotsLicenseDescription;
    }


    private string NameDescription()
    {
        return $"Change your characters name. ~n~Legal Name: ~p~{Player.PlayerName}~s~";
    }
    private void InteractionMenu_OnItemSelect(RAGENativeUI.UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if(selectedItem == ChangeNameMenu)
        {
            if(Player.BankAccounts.GetMoney(true) >= NameChangeFee)
            {
                string NewName = NativeHelper.GetKeyboardInput("");
                if(NewName != "")
                {
                    Player.BankAccounts.GiveMoney(-1 * NameChangeFee, true);
                    Player.ChangeName(NewName);
                    ChangeNameMenu.Description = NameDescription();
                    PlaySuccessSound();
                    DisplayMessage("~g~Purchase", $"You have successfully changed your name to {NewName}");
                }
                else
                {
                    PlayErrorSound();
                    DisplayMessage("~r~Cancelled", "Name change cancelled, you have not been charged");
                } 
            }
            else
            {
                PlayErrorSound();
                DisplayMessage("~r~Insufficient Funds", "We are sorry, we are unable to complete this transation, as you do not have the required funds");
            }
        }
        else if (selectedItem == DriversLicenseMenu)
        {
            if(Player.BankAccounts.GetMoney(true) >= DriversLicenseFee)
            {
                if (Player.Licenses.HasValidDriversLicense(Time))
                {
                    Player.BankAccounts.GiveMoney(-1 * DriversLicenseFee, true);
                    Player.Licenses.DriversLicense.IssueLicense(Time, 12, StateID);
                    DriversLicenseMenu.Description = DriversLicenseDescription();
                    PlaySuccessSound();
                    DisplayMessage("~g~Purchase", $"You have updated your drivers license.~n~Issue Date: {Player.Licenses.DriversLicense.IssueDate:d}~n~Expiration Date: {Player.Licenses.DriversLicense.ExpirationDate:d}");
                }
                else
                {
                    Player.BankAccounts.GiveMoney(-1 * DriversLicenseFee, true);
                    Player.Licenses.DriversLicense = new DriversLicense();
                    Player.Licenses.DriversLicense.IssueLicense(Time, 12, StateID);
                    DriversLicenseMenu.Description = DriversLicenseDescription();
                    PlaySuccessSound();
                    DisplayMessage("~g~Purchase", $"Drivers license issued.~n~Issue Date: {Player.Licenses.DriversLicense.IssueDate:d}~n~Expiration Date: {Player.Licenses.DriversLicense.ExpirationDate:d}");
                }
            }
            else
            {
                PlayErrorSound();
                DisplayMessage("~r~Insufficient Funds", "We are sorry, we are unable to complete this transation, as you do not have the required funds");
            }
        }
        else if (selectedItem == CCWLicenseMenu)
        {
            if (Player.BankAccounts.GetMoney(true) >= CCWLicenseFee)
            {
                if (Player.Licenses.HasCCWLicense && Player.Licenses.CCWLicense.IsValid(Time))
                {
                    Player.BankAccounts.GiveMoney(-1 * CCWLicenseFee, true);
                    Player.Licenses.CCWLicense.IssueLicense(Time, 12, StateID);
                    CCWLicenseMenu.Description = CCWLicenseDescription();
                    PlaySuccessSound();
                    DisplayMessage("~g~Purchase", $"You have updated your CCW license.~n~Issue Date: {Player.Licenses.CCWLicense.IssueDate:d}~n~Expiration Date: {Player.Licenses.CCWLicense.ExpirationDate:d}");
                }
                else
                {
                    Player.BankAccounts.GiveMoney(-1 * CCWLicenseFee, true);
                    Player.Licenses.CCWLicense = new CCWLicense();
                    Player.Licenses.CCWLicense.IssueLicense(Time, 12, StateID);
                    CCWLicenseMenu.Description = CCWLicenseDescription();
                    PlaySuccessSound();
                    DisplayMessage("~g~Purchase", $"CCW license issued.~n~Issue Date: {Player.Licenses.CCWLicense.IssueDate:d}~n~Expiration Date: {Player.Licenses.CCWLicense.ExpirationDate:d}");
                }
            }
            else
            {
                PlayErrorSound();
                DisplayMessage("~r~Insufficient Funds", "We are sorry, we are unable to complete this transation, as you do not have the required funds");
            }
        }
        else if (selectedItem == PilotsLicenseMenu)
        {
            if (Player.BankAccounts.GetMoney(true) >= PilotsLicenseFee)
            {
                if (Player.Licenses.HasPilotsLicense && Player.Licenses.PilotsLicense.IsValid(Time))
                {
                    Player.BankAccounts.GiveMoney(-1 * PilotsLicenseFee, true);
                    Player.Licenses.PilotsLicense.IssueLicense(Time, 12, StateID);
                    PilotsLicenseMenu.Description = PilotsLicenseDescription();
                    PlaySuccessSound();
                    DisplayMessage("~g~Purchase", $"You have updated your Pilots license.~n~Issue Date: {Player.Licenses.PilotsLicense.IssueDate:d}~n~Expiration Date: {Player.Licenses.PilotsLicense.ExpirationDate:d}");
                }
                else
                {
                    Player.BankAccounts.GiveMoney(-1 * PilotsLicenseFee, true);
                    Player.Licenses.PilotsLicense = new PilotsLicense();
                    Player.Licenses.PilotsLicense.IssueLicense(Time, 12, StateID);
                    PilotsLicenseMenu.Description = PilotsLicenseDescription();
                    PlaySuccessSound();
                    DisplayMessage("~g~Purchase", $"Pilots license issued.~n~Issue Date: {Player.Licenses.PilotsLicense.IssueDate:d}~n~Expiration Date: {Player.Licenses.PilotsLicense.ExpirationDate:d}");
                }
            }
            else
            {
                PlayErrorSound();
                DisplayMessage("~r~Insufficient Funds", "We are sorry, we are unable to complete this transation, as you do not have the required funds");
            }
        }

    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.CityHalls.Add(this);
        base.AddLocation(possibleLocations);
    }
}

