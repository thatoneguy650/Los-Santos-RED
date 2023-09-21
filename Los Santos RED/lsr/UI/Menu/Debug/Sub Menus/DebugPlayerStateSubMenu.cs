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
        UIMenuItem GiveMoney = new UIMenuItem("Give Money", "Give the player $50K");
        GiveMoney.Activated += (menu, item) =>
        {
            Player.BankAccounts.GiveMoney(50000, false);
            menu.Visible = false;
        };
        UIMenuItem SetMoney = new UIMenuItem("Set Money", "Sets the current player money");
        SetMoney.Activated += (menu, item) =>
        {
            if (int.TryParse(NativeHelper.GetKeyboardInput(""), out int moneyToSet))
            {
                Player.BankAccounts.SetCash(moneyToSet);
            }
            menu.Visible = false;
        };
        UIMenuItem GetAllItems = new UIMenuItem("Get All Items", "Gets 10 of every item");
        GetAllItems.Activated += (menu, item) =>
        {
            foreach (ModItem modItem in ModItems.InventoryItems())
            {
                if (!modItem.ConsumeOnPurchase)
                {
                    Player.Inventory.Add(modItem, 10);
                }
            }
            menu.Visible = false;
        };
        UIMenuItem GetSomeItems = new UIMenuItem("Get Some Items", "Gets 10 of 30 random items");
        GetSomeItems.Activated += (menu, item) =>
        {
            foreach (ModItem modItem in ModItems.InventoryItems().OrderBy(x => RandomItems.MyRand.Next()).Take(30))
            {
                if (!modItem.ConsumeOnPurchase)
                {
                    Player.Inventory.Add(modItem, 10);
                }
            }
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
        UIMenuListScrollerItem<WeaponCategory> GetRandomWeapon = new UIMenuListScrollerItem<WeaponCategory>("Get Random Weapon", "Gives the Player a random weapon and ammo.", Enum.GetValues(typeof(WeaponCategory)).Cast<WeaponCategory>());
        GetRandomWeapon.Activated += (menu, item) =>
        {
            WeaponInformation myGun = Weapons.GetRandomRegularWeapon(GetRandomWeapon.SelectedItem);
            if (myGun != null)
            {
                Game.LocalPlayer.Character.Inventory.GiveNewWeapon(myGun.ModelName, myGun.AmmoAmount, true);
            }
            menu.Visible = false;
        };

        UIMenuListScrollerItem<WeaponCategory> GetRandomUpgradedWeapon = new UIMenuListScrollerItem<WeaponCategory>("Get Random Upgraded Weapon", "Gives the Player a random upgraded weapon and ammo.", Enum.GetValues(typeof(WeaponCategory)).Cast<WeaponCategory>());
        GetRandomUpgradedWeapon.Activated += (menu, item) =>
        {
            WeaponInformation myGun = Weapons.GetRandomRegularWeapon(GetRandomUpgradedWeapon.SelectedItem);
            if (myGun != null)
            {
                Game.LocalPlayer.Character.Inventory.GiveNewWeapon(myGun.ModelName, myGun.AmmoAmount, true);
                WeaponComponent bestMagazineUpgrade = myGun.PossibleComponents.Where(x => x.ComponentSlot == ComponentSlot.Magazine).OrderBy(x => x.Name == "Box Magazine" ? 1 : x.Name == "Drum Magazine" ? 2 : x.Name == "Extended Clip" ? 3 : 4).FirstOrDefault();
                if (bestMagazineUpgrade != null)
                {
                    myGun.AddComponent(Game.LocalPlayer.Character, bestMagazineUpgrade);
                }
            }
            menu.Visible = false;
        };
        UIMenuListScrollerItem<WeaponCategory> GetRandomSuppressedWeapon = new UIMenuListScrollerItem<WeaponCategory>("Get Random Suppressed Weapon", "Gives the Player a random suppressed weapon and ammo.", Enum.GetValues(typeof(WeaponCategory)).Cast<WeaponCategory>());
        GetRandomSuppressedWeapon.Activated += (menu, item) =>
        {
            WeaponInformation myGun = Weapons.GetRandomRegularWeapon(GetRandomSuppressedWeapon.SelectedItem);
            if (myGun != null)
            {
                Game.LocalPlayer.Character.Inventory.GiveNewWeapon(myGun.ModelName, myGun.AmmoAmount, true);
                WeaponComponent bestMagazineUpgrade = myGun.PossibleComponents.Where(x => x.ComponentSlot == ComponentSlot.Muzzle).OrderBy(x => x.Name == "Suppressed" ? 1 : 4).FirstOrDefault();
                if (bestMagazineUpgrade != null)
                {
                    myGun.AddComponent(Game.LocalPlayer.Character, bestMagazineUpgrade);
                }
            }
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

        UIMenuItem RemoveButtonPrompts = new UIMenuItem("Remove Prompts", "Removes all the button prompts");
        RemoveButtonPrompts.Activated += (menu, item) =>
        {
            Player.ButtonPrompts.Clear();
            menu.Visible = false;
        };


        //spawn taxi
        UIMenuItem TaxiSpawn = new UIMenuItem("Spawn Taxi", "Spawns a taxi in fron of player");
        TaxiSpawn.Activated += (menu, item) =>
        {
            TaxiDropOff TaxiDropOff = new TaxiDropOff(Game.LocalPlayer.Character.GetOffsetPositionFront(10f), Settings, Crimes, Weapons, Names, World, ModItems, null);
            TaxiDropOff.Setup();
            TaxiDropOff.Start();
            menu.Visible = false;
        };

        //spawn taxi
        UIMenuItem ScamText = new UIMenuItem("Scam Text", "Add a random scam text");
        ScamText.Activated += (menu, item) =>
        {
            Player.CellPhone.AddScamText();
            menu.Visible = false;
        };

        //spawn taxi
        UIMenuItem RandomizePhone = new UIMenuItem("Randomize Phone", "Randomize the phone settings");
        RandomizePhone.Activated += (menu, item) =>
        {
            Player.CellPhone.RandomizeSettings();
            menu.Visible = false;
        };
        //spawn taxi
        UIMenuItem ToggleInvisible = new UIMenuItem("Toggle Invisible", "Toggle player invisibility");
        ToggleInvisible.Activated += (menu, item) =>
        {
            Player.Character.IsVisible = !Player.Character.IsVisible;
            menu.Visible = false;
        };


        UIMenuListScrollerItem<string> SetArrested = new UIMenuListScrollerItem<string>("Set Arrested", "Set the player ped as arrested.", new List<string>() { "Stay Standing", "Kneeling" });
        SetArrested.Activated += (menu, item) =>
        {
            bool stayStanding = SetArrested.SelectedItem == "Stay Standing";
            Player.Arrest();
            Game.TimeScale = 1.0f;
            Player.Surrendering.SetArrestedAnimation(stayStanding);
            menu.Visible = false;
        };
        

        UIMenuItem UnSetArrested = new UIMenuItem("UnSet Arrested", "Release the player from an arrest.");
        UnSetArrested.Activated += (menu, item) =>
        {
            Game.TimeScale = 1.0f;
            Player.Reset(true, false, true, true, true, false, false, false, false, false, false, false, false, false, false, true, false, false);
            Player.Surrendering.UnSetArrestedAnimation();
            menu.Visible = false;
        };



        PlayerStateItemsMenu.AddItem(FillHealthAndArmor);
        PlayerStateItemsMenu.AddItem(SetHealth);
        PlayerStateItemsMenu.AddItem(KillPlayer);

        PlayerStateItemsMenu.AddItem(ToggleInvisible);

        PlayerStateItemsMenu.AddItem(GiveMoney);
        PlayerStateItemsMenu.AddItem(SetMoney);

        PlayerStateItemsMenu.AddItem(ResetNeeds);
        PlayerStateItemsMenu.AddItem(SetRandomNeeds);
        PlayerStateItemsMenu.AddItem(ForceSober);
        PlayerStateItemsMenu.AddItem(GetAllItems);
        PlayerStateItemsMenu.AddItem(GetSomeItems);
        PlayerStateItemsMenu.AddItem(GetRandomWeapon);
        PlayerStateItemsMenu.AddItem(GetRandomUpgradedWeapon);
        PlayerStateItemsMenu.AddItem(GetRandomSuppressedWeapon);
        PlayerStateItemsMenu.AddItem(GetDriversLicense);
        PlayerStateItemsMenu.AddItem(GetCCWLicense);
        PlayerStateItemsMenu.AddItem(GetPilotsLicense);
        PlayerStateItemsMenu.AddItem(AutoSetRadioStation);
        PlayerStateItemsMenu.AddItem(TaxiSpawn);
        PlayerStateItemsMenu.AddItem(ScamText);
        PlayerStateItemsMenu.AddItem(RandomizePhone);
        PlayerStateItemsMenu.AddItem(SetArrested);
        PlayerStateItemsMenu.AddItem(UnSetArrested);
        PlayerStateItemsMenu.AddItem(RemoveButtonPrompts);
    }
}

