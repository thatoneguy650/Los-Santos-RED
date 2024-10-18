using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;

public class DebugPlayerStateSubMenu : DebugSubMenu
{
    private ISettingsProvideable Settings;
    private ICrimes Crimes;
    private ITaskerable Tasker;
    private IEntityProvideable World;
    private IWeapons Weapons;
    private IModItems ModItems;
    private ITimeControllable Time;
    private IRadioStations RadioStations;
    private INameProvideable Names;
    public DebugPlayerStateSubMenu(UIMenu debug, MenuPool menuPool, IActionable player, ISettingsProvideable settings, ICrimes crimes, ITaskerable tasker, IEntityProvideable world, IWeapons weapons, IModItems modItems, ITimeControllable time, IRadioStations radioStations, INameProvideable names) : base(debug, menuPool, player)
    {
        Settings = settings;
        Crimes = crimes;
        Tasker = tasker;
        World = world;
        Weapons = weapons;
        ModItems = modItems;
        Time = time;
        RadioStations = radioStations;
        Names = names;
    }
    public override void AddItems()
    {
        UIMenu PlayerStateItemsMenu = MenuPool.AddSubMenu(Debug, "Player General Menu");
        PlayerStateItemsMenu.SetBannerType(EntryPoint.LSRedColor);
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Change various player items.";

        UIMenuItem KillPlayer = new UIMenuItem("Kill Player", "Immediatly die and ragdoll");
        KillPlayer.Activated += (menu, item) =>
        {
            Game.LocalPlayer.Character.Kill();
            menu.Visible = false;
        };
        UIMenuItem FillHealth = new UIMenuItem("Fill Health", "Refill health only");
        FillHealth.Activated += (menu, item) =>
        {
            Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
            menu.Visible = false;
        };
        UIMenuItem FillHealthAndArmor = new UIMenuItem("Fill Health and Armor", "Get loaded for bear");
        FillHealthAndArmor.Activated += (menu, item) =>
        {
            Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
            Game.LocalPlayer.Character.Armor = 100;
            menu.Visible = false;
        };
        UIMenuItem ForceSober = new UIMenuItem("Become Sober", "Forces a sober state on the player (if intoxicated)");
        ForceSober.Activated += (menu, item) =>
        {
            Player.Intoxication.Dispose();
            menu.Visible = false;
        };
        UIMenuItem SetRandomNeeds = new UIMenuItem("Set Random Needs", "Sets the player needs level random");
        SetRandomNeeds.Activated += (menu, item) =>
        {
            Player.HumanState.SetRandom();
            menu.Visible = false;
        };
        UIMenuItem ResetNeeds = new UIMenuItem("Reset Needs", "Sets the player needs to the default level");
        ResetNeeds.Activated += (menu, item) =>
        {
            Player.HumanState.Reset();
            menu.Visible = false;
        };

        UIMenuNumericScrollerItem<int> SetHealth = new UIMenuNumericScrollerItem<int>("Set Health", "Sets the player health", 0, Player.Character.MaxHealth, 1);
        SetHealth.Value = Player.Character.MaxHealth;
        SetHealth.Activated += (menu, item) =>
        {
            Player.HealthManager.SetHealth(SetHealth.Value);
            menu.Visible = false;
        };
        UIMenuListScrollerItem<RadioStation> AutoSetRadioStation = new UIMenuListScrollerItem<RadioStation>("Auto-Set Station", "Will auto set the station any time the radio is on", RadioStations.RadioStationList);
        AutoSetRadioStation.Activated += (menu, item) =>
        {
            Settings.SettingsManager.VehicleSettings.AutoTuneRadioStation = AutoSetRadioStation.SelectedItem.InternalName;
            menu.Visible = false;
        };
        AutoSetRadioStation.IndexChanged += (UIMenuScrollerItem sender, int oldIndex, int newIndex) =>
        {
            Settings.SettingsManager.VehicleSettings.AutoTuneRadioStation = AutoSetRadioStation.SelectedItem.InternalName;
        };
        UIMenuItem GetDriversLicense = new UIMenuItem("Get Drivers License", "Get a drivers license");
        GetDriversLicense.Activated += (menu, item) =>
        {
            string stateID = Player.CurrentLocation?.CurrentZone?.StateID;
            if (string.IsNullOrEmpty(stateID))
            {
                stateID = StaticStrings.SanAndreasStateID;
            }
            Player.Licenses.DriversLicense = new DriversLicense();
            Player.Licenses.DriversLicense.IssueLicense(Time, 12, stateID);
            menu.Visible = false;
        };
        UIMenuItem GetCCWLicense = new UIMenuItem("Get CCW License", "Get a ccw license");
        GetCCWLicense.Activated += (menu, item) =>
        {
            string stateID = Player.CurrentLocation?.CurrentZone?.StateID;
            if (string.IsNullOrEmpty(stateID))
            {
                stateID = StaticStrings.SanAndreasStateID;
            }
            Player.Licenses.CCWLicense = new CCWLicense();
            Player.Licenses.CCWLicense.IssueLicense(Time, 12, stateID);
            menu.Visible = false;
        };

        UIMenuItem GetPilotsLicense = new UIMenuItem("Get Pilots License", "Get a pilots license");
        GetPilotsLicense.Activated += (menu, item) =>
        {
            string stateID = Player.CurrentLocation?.CurrentZone?.StateID;
            if (string.IsNullOrEmpty(stateID))
            {
                stateID = StaticStrings.SanAndreasStateID;
            }
            Player.Licenses.PilotsLicense = new PilotsLicense();
            Player.Licenses.PilotsLicense.IssueLicense(Time, 12, stateID);
            Player.Licenses.PilotsLicense.IsFixedWingEndorsed = true;
            Player.Licenses.PilotsLicense.IsRotaryEndorsed = true;
            Player.Licenses.PilotsLicense.IsLighterThanAirEndorsed = true;
            menu.Visible = false;
        };

        UIMenuItem ScamText = new UIMenuItem("Scam Text", "Add a random scam text");
        ScamText.Activated += (menu, item) =>
        {
            Player.CellPhone.AddScamText();
            menu.Visible = false;
        };

        UIMenuItem RandomizePhone = new UIMenuItem("Randomize Phone", "Randomize the phone settings");
        RandomizePhone.Activated += (menu, item) =>
        {
            Player.CellPhone.RandomizeSettings();
            menu.Visible = false;
        };

        UIMenuItem ToggleInvisible = new UIMenuItem("Toggle Invisible", "Toggle player invisibility");
        ToggleInvisible.Activated += (menu, item) =>
        {
            Player.Character.IsVisible = !Player.Character.IsVisible;
            menu.Visible = false;
        };

        PlayerStateItemsMenu.AddItem(FillHealthAndArmor);
        PlayerStateItemsMenu.AddItem(SetHealth);
        PlayerStateItemsMenu.AddItem(KillPlayer);
        PlayerStateItemsMenu.AddItem(ToggleInvisible);
        PlayerStateItemsMenu.AddItem(ResetNeeds);
        PlayerStateItemsMenu.AddItem(SetRandomNeeds);
        PlayerStateItemsMenu.AddItem(ForceSober);
        PlayerStateItemsMenu.AddItem(GetDriversLicense);
        PlayerStateItemsMenu.AddItem(GetCCWLicense);
        PlayerStateItemsMenu.AddItem(GetPilotsLicense);
        PlayerStateItemsMenu.AddItem(AutoSetRadioStation);
        PlayerStateItemsMenu.AddItem(ScamText);
        PlayerStateItemsMenu.AddItem(RandomizePhone);

        UIMenuItem setUnCuffed = new UIMenuItem("Set Uncuffed", "Set Uncuffed");
        setUnCuffed.Activated += (menu, item) =>
        {
            Player.CuffManager.SetPlayerHandcuffsRemoved();
            menu.Visible = false;
        };
        PlayerStateItemsMenu.AddItem(setUnCuffed);

        UIMenuItem SayItem = new UIMenuItem("Say", "Have Player Say");
        SayItem.Activated += (menu, item) =>
        {
            
            menu.Visible = false;
            Player.PlayerVoice.DebugSayRandom();


        };
        PlayerStateItemsMenu.AddItem(SayItem);
    }
}

