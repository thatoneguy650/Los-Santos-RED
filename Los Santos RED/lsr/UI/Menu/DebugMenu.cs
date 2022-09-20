using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

public class DebugMenu : Menu
{

    private UIMenu Debug;
    private IActionable Player;
    private RadioStations RadioStations;
    private Camera FreeCam;
    private float FreeCamScale = 1.0f;
    private IWeapons Weapons;
    private IPlacesOfInterest PlacesOfInterest;
    private ISettingsProvideable Settings;
    private ITimeControllable Time;
    private IEntityProvideable World;
    private ITaskerable Tasker;
    private MenuPool MenuPool;
    private Dispatcher Dispatcher;
    private IAgencies Agencies;
    private IGangs Gangs;
    private IModItems ModItems;
    private ICrimes Crimes;

    public DebugMenu(MenuPool menuPool, IActionable player, IWeapons weapons, RadioStations radioStations, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, ITimeControllable time, IEntityProvideable world, ITaskerable tasker, Dispatcher dispatcher, IAgencies agencies, IGangs gangs, IModItems modItems, ICrimes crimes)
    {
        Gangs = gangs;
        Dispatcher = dispatcher;
        Agencies = agencies;
        MenuPool = menuPool;
        Player = player;
        Weapons = weapons;
        RadioStations = radioStations;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        Time = time;
        World = world;
        Tasker = tasker;
        ModItems = modItems;
        Crimes = crimes;
        Debug = new UIMenu("Debug", "Debug Settings");
        Debug.SetBannerType(EntryPoint.LSRedColor);
        menuPool.Add(Debug);      
        
    }
    public override void Hide()
    {
        Debug.Visible = false;
    }
    public override void Show()
    {
        if (!Debug.Visible)
        {
            Debug.Visible = true;
        }
    }
    public override void Toggle()
    {
        if (!Debug.Visible)
        {
            Debug.Visible = true;
        }
        else
        {
            Debug.Visible = false;
        }
    }
    private void CreateDebugMenu()
    {
        CreateDispatcherMenu();
        CreateGangItemsMenu();
        CreateTimeMenu();
        CreateCrimeMenu();
        CreatePlayerStateMenu();
        CreateLocationMenu();
        CreateTeleportMenu();
        CreateOtherItems();
    }
    private void CreateLocationMenu()
    {
        UIMenu LocationItemsMenu = MenuPool.AddSubMenu(Debug, "Location Menu");
        LocationItemsMenu.SetBannerType(EntryPoint.LSRedColor);
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Change various location items.";
        UIMenuItem LogLocationMenu = new UIMenuItem("Log Game Location", "Location Type, Then Name");
        LogLocationMenu.Activated += (menu, item) =>
        {
            LogGameLocation();
            menu.Visible = false;
        };
        UIMenuItem LogSpawnPositionMenu = new UIMenuItem("Log Ped or Vehicle Spawn", "Logs a point for spawning");
        LogSpawnPositionMenu.Activated += (menu, item) =>
        {
            LogSpawnPosition();
            menu.Visible = false;
        };
        UIMenuItem LogLocationSimpleMenu = new UIMenuItem("Log Game Location (Simple)", "Location Type, Then Name");
        LogLocationSimpleMenu.Activated += (menu, item) =>
        {
            LogGameLocationSimple();
            menu.Visible = false;
        };
        UIMenuItem LogInteriorMenu = new UIMenuItem("Log Game Interior", "Interior Name");
        LogInteriorMenu.Activated += (menu, item) =>
        {
            LogGameInterior();
            menu.Visible = false;
        };
        UIMenuItem LogCameraPositionMenu = new UIMenuItem("Log Camera Position", "Logs current rendering cam post direction and rotation");
        LogCameraPositionMenu.Activated += (menu, item) =>
        {
            LogCameraPosition();
            menu.Visible = false;
        };
        UIMenuItem FreeCamMenu = new UIMenuItem("Free Cam", "Start Free Camera Mode");
        FreeCamMenu.Activated += (menu, item) =>
        {
            Frecam();
            menu.Visible = false;
        };
        UIMenuItem LoadSPMap = new UIMenuItem("Load SP Map", "Loads the SP map if you have the MP map enabled");
        LoadSPMap.Activated += (menu, item) =>
        {
            World.LoadSPMap();
            menu.Visible = false;
        };
        UIMenuItem LoadMPMap = new UIMenuItem("Load MP Map", "Load the MP map if you have the SP map enabled");
        LoadMPMap.Activated += (menu, item) =>
        {
            World.LoadMPMap();
            menu.Visible = false;
        };
        UIMenuItem AddAllBlips = new UIMenuItem("Add All Blips", "Add all blips to the map");
        AddAllBlips.Activated += (menu, item) =>
        {
            World.Places.StaticPlaces.AddAllBlips();
            menu.Visible = false;
        };
        UIMenuItem RemoveAllBlips = new UIMenuItem("Remove All Blips", "Remove all blips from the map");
        RemoveAllBlips.Activated += (menu, item) =>
        {
            World.RemoveBlips();
            menu.Visible = false;
        };




        LocationItemsMenu.AddItem(LogSpawnPositionMenu);
        LocationItemsMenu.AddItem(LogLocationMenu);
        LocationItemsMenu.AddItem(LogLocationSimpleMenu);
        LocationItemsMenu.AddItem(LogInteriorMenu);
        LocationItemsMenu.AddItem(LogCameraPositionMenu);
        LocationItemsMenu.AddItem(FreeCamMenu);
        LocationItemsMenu.AddItem(LoadSPMap);
        LocationItemsMenu.AddItem(LoadMPMap);
        LocationItemsMenu.AddItem(AddAllBlips);
        LocationItemsMenu.AddItem(RemoveAllBlips);
    }
    private void CreateTeleportMenu()
    {
        UIMenu LocationItemsMenu = MenuPool.AddSubMenu(Debug, "Teleport Menu");
        LocationItemsMenu.SetBannerType(EntryPoint.LSRedColor);
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Teleport to various locations";
        List<BasicLocation> DirectoryLocations = PlacesOfInterest.AllLocations().ToList();
        foreach (string typeName in DirectoryLocations.OrderBy(x => x.TypeName).Select(x => x.TypeName).Distinct())
        {
            UIMenuListScrollerItem<BasicLocation> myLocationType = new UIMenuListScrollerItem<BasicLocation>($"{typeName}", "Teleports to A POI on the Map", DirectoryLocations.Where(x => x.TypeName == typeName));
            myLocationType.Activated += (menu, item) =>
            {
                BasicLocation toTele = myLocationType.SelectedItem;
                if (toTele != null)
                {
                    Game.LocalPlayer.Character.Position = toTele.EntrancePosition;
                    Game.LocalPlayer.Character.Heading = toTele.EntranceHeading;
                }
                menu.Visible = false;
            };
            LocationItemsMenu.AddItem(myLocationType);
        }
    }
    private void CreatePlayerStateMenu()
    {
        UIMenu PlayerStateItemsMenu = MenuPool.AddSubMenu(Debug, "Player State Menu");
        PlayerStateItemsMenu.SetBannerType(EntryPoint.LSRedColor);
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Change various time items.";
        UIMenuItem KillPlayer = new UIMenuItem("Kill Player", "Immediatly die and ragdoll");
        KillPlayer.Activated += (menu, item) =>
        {
            Game.LocalPlayer.Character.Kill();
            menu.Visible = false;
        };
        UIMenuItem GiveMoney = new UIMenuItem("Give Money", "Give the player $50K");
        GiveMoney.Activated += (menu, item) =>
        {
            Player.BankAccounts.GiveMoney(50000);
            menu.Visible = false;
        };
        UIMenuItem SetMoney = new UIMenuItem("Set Money", "Sets the current player money");
        SetMoney.Activated += (menu, item) =>
        {
            if (int.TryParse(NativeHelper.GetKeyboardInput(""), out int moneyToSet))
            {
                Player.BankAccounts.SetMoney(moneyToSet);
            }
            menu.Visible = false;
        };
        UIMenuItem GetAllItems = new UIMenuItem("Get All Items", "Gets 10 of every item");
        GetAllItems.Activated += (menu, item) =>
        {
            foreach (ModItem modItem in ModItems.Items)
            {
                if (modItem.ItemType == ItemType.Drinks || modItem.ItemType == ItemType.Drugs || modItem.ItemType == ItemType.Food || modItem.ItemType == ItemType.Tools)
                {
                    if (!modItem.ConsumeOnPurchase)
                    {
                        Player.Inventory.Add(modItem, 10);
                    }
                }
            }
            menu.Visible = false;
        };
        UIMenuItem GetSomeItems = new UIMenuItem("Get Some Items", "Gets 10 of 30 random items");
        GetSomeItems.Activated += (menu, item) =>
        {
            foreach (ModItem modItem in ModItems.Items.OrderBy(x => RandomItems.MyRand.Next()).Take(30))
            {
                if (modItem.ItemType == ItemType.Drinks || modItem.ItemType == ItemType.Drugs || modItem.ItemType == ItemType.Food || modItem.ItemType == ItemType.Tools)
                {
                    if (!modItem.ConsumeOnPurchase)
                    {
                        Player.Inventory.Add(modItem, 10);
                    }
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

        UIMenuNumericScrollerItem<int> SetHealth = new UIMenuNumericScrollerItem<int>("Set Health", "Sets the player health",0,Player.Character.MaxHealth,1);
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
            Player.Licenses.DriversLicense = new DriversLicense();
            Player.Licenses.DriversLicense.IssueLicense(Time, 12);
            menu.Visible = false;
        };
        UIMenuItem GetCCWLicense = new UIMenuItem("Get CCW License", "Get a ccw license");
        GetCCWLicense.Activated += (menu, item) =>
        {
            Player.Licenses.CCWLicense = new CCWLicense();
            Player.Licenses.CCWLicense.IssueLicense(Time, 12);
            menu.Visible = false;
        };

        UIMenuItem GetPilotsLicense = new UIMenuItem("Get Pilots License", "Get a pilots license");
        GetPilotsLicense.Activated += (menu, item) =>
        {
            Player.Licenses.PilotsLicense = new PilotsLicense();
            Player.Licenses.PilotsLicense.IssueLicense(Time, 12);
            Player.Licenses.PilotsLicense.IsFixedWingEndorsed = true;
            Player.Licenses.PilotsLicense.IsRotaryEndorsed = true;
            Player.Licenses.PilotsLicense.IsLighterThanAirEndorsed = true;
            menu.Visible = false;
        };


        PlayerStateItemsMenu.AddItem(KillPlayer);
        PlayerStateItemsMenu.AddItem(GiveMoney);
        PlayerStateItemsMenu.AddItem(SetMoney);
        PlayerStateItemsMenu.AddItem(ForceSober);
        PlayerStateItemsMenu.AddItem(GetAllItems);
        PlayerStateItemsMenu.AddItem(GetSomeItems);
        PlayerStateItemsMenu.AddItem(FillHealthAndArmor);
        PlayerStateItemsMenu.AddItem(GetRandomWeapon);
        PlayerStateItemsMenu.AddItem(GetRandomUpgradedWeapon);
        PlayerStateItemsMenu.AddItem(SetRandomNeeds);
        PlayerStateItemsMenu.AddItem(SetHealth);
        PlayerStateItemsMenu.AddItem(ResetNeeds);
        PlayerStateItemsMenu.AddItem(AutoSetRadioStation);


        PlayerStateItemsMenu.AddItem(GetDriversLicense);
        PlayerStateItemsMenu.AddItem(GetCCWLicense);
        PlayerStateItemsMenu.AddItem(GetPilotsLicense);

    }

    public void Setup()
    {
        CreateDebugMenu();
    }

    private void CreateCrimeMenu()
    {
        UIMenu CrimeItemsMenu = MenuPool.AddSubMenu(Debug, "Crime Menu");
        CrimeItemsMenu.SetBannerType(EntryPoint.LSRedColor);
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Change various crime items.";
        UIMenuListScrollerItem<int>  SetWantedLevel = new UIMenuListScrollerItem<int>("Set Wanted Level", "Set wanted at the desired level", new List<int>() { 0, 1, 2, 3, 4, 5, 6 });
        SetWantedLevel.Activated += (menu, item) =>
        {
            Player.SetWantedLevel(SetWantedLevel.SelectedItem, "Debug Menu", true);
            menu.Visible = false;
        };
        UIMenuItem ToggleInvestigation = new UIMenuItem("Toggle Investigation", "Start or stop an investigation.");
        ToggleInvestigation.Activated += (menu, item) =>
        {
            if (Player.IsNotWanted)
            {
                if (Player.Investigation.IsActive)
                {
                    Player.Investigation.Start(Player.Character.Position, false, true, false, false);
                }
                else
                {
                    Player.Investigation.Expire();
                }
            }
            menu.Visible = false;
        };
        UIMenuItem SpawnGunAttackersMenu = new UIMenuItem("Spawn Gun Attacker", "spawns some peds with guns that will attack you");
        SpawnGunAttackersMenu.Activated += (menu, item) =>
        {
            SpawnGunAttackers();
            menu.Visible = false;
        };
        UIMenuItem SpawnNoGunAttackersMenu = new UIMenuItem("Spawn No Gun Attackers", "spawns some peds without guins that will attack you");
        SpawnNoGunAttackersMenu.Activated += (menu, item) =>
        {
            SpawnNoGunAttackers();
            menu.Visible = false;
        };
        UIMenuItem StartRandomCrime = new UIMenuItem("Start Random Crime", "Trigger a random crime around the map.");
        StartRandomCrime.Activated += (menu, item) =>
        {
            Tasker.CreateCrime();
            menu.Visible = false;
        };


        UIMenuItem GiveClosesetGun = new UIMenuItem("Give Gun", "Give a gun to the closest ped");
        GiveClosesetGun.Activated += (menu, item) =>
        {
            GiveClosestGun();
            menu.Visible = false;
        };



        CrimeItemsMenu.AddItem(SetWantedLevel);
        CrimeItemsMenu.AddItem(ToggleInvestigation);
        CrimeItemsMenu.AddItem(SpawnGunAttackersMenu);
        CrimeItemsMenu.AddItem(SpawnNoGunAttackersMenu);
        CrimeItemsMenu.AddItem(StartRandomCrime);
        CrimeItemsMenu.AddItem(GiveClosesetGun);
    }
    private void CreateOtherItems()
    {
        UIMenu OtherItemsMenu = MenuPool.AddSubMenu(Debug, "Other Menu");
        OtherItemsMenu.SetBannerType(EntryPoint.LSRedColor);
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Change various time items.";








        UIMenuItem highlightProp = new UIMenuItem("Highlight Prop", "Get some info about the nearest prop.");
        highlightProp.Activated += (menu, item) =>
        {
            HighlightProp();
            menu.Visible = false;
        };
        OtherItemsMenu.AddItem(highlightProp);








        UIMenuItem GoToReleaseSettings = new UIMenuItem("Quick Set Release Settings", "Set some release settings quickly.");
        GoToReleaseSettings.Activated += (menu, item) =>
        {
            Settings.SetRelease();
            menu.Visible = false;
        };
        OtherItemsMenu.AddItem(GoToReleaseSettings);
        UIMenuItem GoToHardCoreSettings = new UIMenuItem("Quick Set Hardcore Settings", "Set the very difficult settings.");
        GoToHardCoreSettings.Activated += (menu, item) =>
        {
            Settings.SetHard();
            menu.Visible = false;
        };
        OtherItemsMenu.AddItem(GoToHardCoreSettings);


        UIMenuItem PrintEntities = new UIMenuItem("Print Persistent Entities", "Prints a list of all persistent and spawned entities to the log.");
        PrintEntities.Activated += (menu, item) =>
        {
            PrintPersistentEntities();
            menu.Visible = false;
        };
        OtherItemsMenu.AddItem(PrintEntities);

        UIMenuItem PrintEntities2 = new UIMenuItem("Print Entities", "Prints a list of all entities to the log.");
        PrintEntities2.Activated += (menu, item) =>
        {
            PrintAllEntities();
            menu.Visible = false;
        };
        OtherItemsMenu.AddItem(PrintEntities2);



        UIMenuListScrollerItem<string> SetArrested = new UIMenuListScrollerItem<string>("Set Arrested", "Set the player ped as arrested.", new List<string>() { "Stay Standing", "Kneeling" });
        SetArrested.Activated += (menu, item) =>
        {
            bool stayStanding = SetArrested.SelectedItem == "Stay Standing";
            Player.Arrest();
            Game.TimeScale = 1.0f;
            Player.Surrendering.SetArrestedAnimation(stayStanding);
            menu.Visible = false;
        };
        OtherItemsMenu.AddItem(SetArrested);

        UIMenuItem UnSetArrested = new UIMenuItem("UnSet Arrested", "Release the player from an arrest.");
        UnSetArrested.Activated += (menu, item) =>
        {
            Game.TimeScale = 1.0f;
            Player.Reset(true, false, true, true, true, false, false, false, false, false, false, false, false, false, false);
            Player.Surrendering.UnSetArrestedAnimation();
            menu.Visible = false;
        };
        OtherItemsMenu.AddItem(UnSetArrested);


        UIMenuListScrollerItem<ModTaskGroup> taskGroups = new UIMenuListScrollerItem<ModTaskGroup>("Enable Task Groups", "Enable or disable task groups.", EntryPoint.ModController.TaskGroups);
        taskGroups.Activated += (menu, item) =>
        {
            taskGroups.SelectedItem.IsRunning = !taskGroups.SelectedItem.IsRunning;
            Game.DisplaySubtitle($"{taskGroups.SelectedItem.ToString()}");
            //menu.Visible = false;
        };
        OtherItemsMenu.AddItem(taskGroups);

        //TaskGroups



        UIMenuCheckboxItem runUI = new UIMenuCheckboxItem("Run UI", EntryPoint.ModController.RunUI);
        runUI.CheckboxEvent += (sender, Checked) =>
        {
            EntryPoint.ModController.RunUI = Checked;
            Game.DisplaySubtitle($"UI Running: {EntryPoint.ModController.RunUI}");
        };
        OtherItemsMenu.AddItem(runUI);

        UIMenuCheckboxItem runMenuOnly = new UIMenuCheckboxItem("Run Menu", EntryPoint.ModController.RunMenuOnly);
        runMenuOnly.CheckboxEvent += (sender, Checked) =>
        {
            EntryPoint.ModController.RunMenuOnly = Checked;
            Game.DisplaySubtitle($"UI Running: {EntryPoint.ModController.RunMenuOnly}");
        };
        OtherItemsMenu.AddItem(runMenuOnly);


        UIMenuCheckboxItem runVanilla = new UIMenuCheckboxItem("Run Vanilla", EntryPoint.ModController.RunVanilla);
        runVanilla.CheckboxEvent += (sender, Checked) =>
        {
            EntryPoint.ModController.RunVanilla = Checked;
            Game.DisplaySubtitle($"Vanilla Running: {EntryPoint.ModController.RunVanilla}");
        };
        OtherItemsMenu.AddItem(runVanilla);

        UIMenuCheckboxItem runInput = new UIMenuCheckboxItem("Run Input", EntryPoint.ModController.RunInput);
        runInput.CheckboxEvent += (sender, Checked) =>
        {
            EntryPoint.ModController.RunInput = Checked;
            Game.DisplaySubtitle($"Input Running: {EntryPoint.ModController.RunInput}");
        };
        OtherItemsMenu.AddItem(runInput);

        UIMenuCheckboxItem runOther = new UIMenuCheckboxItem("Run Other", EntryPoint.ModController.RunOther);
        runOther.CheckboxEvent += (sender, Checked) =>
        {
            EntryPoint.ModController.RunOther = Checked;
            Game.DisplaySubtitle($"Other Running: {EntryPoint.ModController.RunOther}");
        };
        OtherItemsMenu.AddItem(runOther);



        //HighlightProp()


    }

    private void RunUI_CheckboxEvent(UIMenuCheckboxItem sender, bool Checked)
    {
        throw new NotImplementedException();
    }

    private void CreateTimeMenu()
    {
        UIMenu TimeItems = MenuPool.AddSubMenu(Debug, "Time Menu");
        TimeItems.SetBannerType(EntryPoint.LSRedColor);
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Change various time items.";
        UIMenuNumericScrollerItem<int> FastForwardTime = new UIMenuNumericScrollerItem<int>("Fast Forward Time", "Fast forward time.", 1, 24, 1) { Formatter = v => v + " Hours" };
        FastForwardTime.Activated += (menu, item) =>
        {
            Time.FastForward(FastForwardTime.Value);
            menu.Visible = false;
        };
        TimeItems.AddItem(FastForwardTime);
        UIMenuItem SetDateToToday = new UIMenuItem("Set Game Date Current", "Sets the game date the same as system date");
        SetDateToToday.Activated += (menu, item) =>
        {
            Time.SetDateToToday();
            menu.Visible = false;
        };
        TimeItems.AddItem(SetDateToToday);
        UIMenuNumericScrollerItem<int> SetDateYear = new UIMenuNumericScrollerItem<int>("Set Game Date Year", "Sets the game date to the year selected (August 1st at 1 PM)", 1970, 2030, 1);
        SetDateYear.Activated += (menu, item) =>
        {
            DateTime toSet = new DateTime(SetDateYear.Value, 8, 1, 13, 0, 0);
            Time.SetDateTime(toSet);
            Game.DisplayHelp($"Date Set to {toSet}");
            menu.Visible = false;
        };
        TimeItems.AddItem(SetDateYear);



        UIMenuNumericScrollerItem<int> AdvanceHours = new UIMenuNumericScrollerItem<int>("Advance Hours", "Moves the game time forwards by the set hours", 1, 48, 1);
        AdvanceHours.Activated += (menu, item) =>
        {
            Time.SetDateTime(Time.CurrentDateTime.AddHours(AdvanceHours.Value));
            Game.DisplayHelp($"Date Set to {Time.CurrentDateTime.AddHours(AdvanceHours.Value)}");
            menu.Visible = false;
        };
        TimeItems.AddItem(AdvanceHours);


    }
    private void CreateGangItemsMenu()
    {
        UIMenu GangItems = MenuPool.AddSubMenu(Debug, "Gang Items");
        GangItems.SetBannerType(EntryPoint.LSRedColor);
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Debug Gang Items.";

        UIMenuListScrollerItem<Gang> SetAsGangMember = new UIMenuListScrollerItem<Gang>("Become Gang Member", "Become a gang member of the selected gang", Gangs.GetAllGangs());
        SetAsGangMember.Activated += (menu, item) =>
        {
            Player.RelationshipManager.GangRelationships.ResetGang(true);
            Player.RelationshipManager.GangRelationships.SetGang(SetAsGangMember.SelectedItem, true);
            menu.Visible = false;
        };
        UIMenuItem LeaveGang = new UIMenuItem("Leave Gang", "Leave your current gang");
        LeaveGang.Activated += (menu, item) =>
        {
            Player.RelationshipManager.GangRelationships.ResetGang(true);
            menu.Visible = false;
        };
        UIMenuListScrollerItem<Gang> SetGangRepDefault = new UIMenuListScrollerItem<Gang>("Set Gang Default", "Sets the selected gang to the default reputation", Gangs.GetAllGangs());
        SetGangRepDefault.Activated += (menu, item) =>
        {
            Player.RelationshipManager.GangRelationships.SetReputation(SetGangRepDefault.SelectedItem, 200, false);
            menu.Visible = false;
        };
        UIMenuListScrollerItem<Gang> SetGangRepFriendly = new UIMenuListScrollerItem<Gang>("Set Gang Friendly", "Sets the selected gang to a friendly reputation", Gangs.GetAllGangs());
        SetGangRepFriendly.Activated += (menu, item) =>
        {
            Player.RelationshipManager.GangRelationships.SetReputation(SetGangRepFriendly.SelectedItem, 5000, false);
            menu.Visible = false;
        };
        UIMenuListScrollerItem<Gang> SetGangRepHostile = new UIMenuListScrollerItem<Gang>("Set Gang Hostile", "Sets the selected gang to a hostile reputation", Gangs.GetAllGangs());
        SetGangRepHostile.Activated += (menu, item) =>
        {
            Player.RelationshipManager.GangRelationships.SetReputation(SetGangRepHostile.SelectedItem, -5000, false);
            menu.Visible = false;
        };
        UIMenuItem DefaultGangRep = new UIMenuItem("Set Gang Rep Default", "Sets the player reputation to each gang to the default value");
        DefaultGangRep.Activated += (menu, item) =>
        {
            Player.RelationshipManager.GangRelationships.Reset();
            menu.Visible = false;
        };
        UIMenuItem RandomGangRep = new UIMenuItem("Set Gang Rep Random", "Sets the player reputation to each gang to a randomized number");
        RandomGangRep.Activated += (menu, item) =>
        {
            Player.RelationshipManager.GangRelationships.SetAllRandomReputations();
            menu.Visible = false;
        };
        UIMenuItem RandomSingleGangRep = new UIMenuItem("Set Single Gang Rep Random", "Sets the player reputation to random gang to a randomized number");
        RandomSingleGangRep.Activated += (menu, item) =>
        {
            Player.RelationshipManager.GangRelationships.SetSingleRandomReputation();
            menu.Visible = false;
        };
        UIMenuItem HostileGangRep = new UIMenuItem("Set Gang Rep Hostile", "Sets the player reputation to each gang to hostile");
        HostileGangRep.Activated += (menu, item) =>
        {
            Player.RelationshipManager.GangRelationships.SetHostileReputations();
            menu.Visible = false;
        };
        UIMenuItem FriendlyGangRep = new UIMenuItem("Set Gang Rep Friendly", "Sets the player reputation to each gang to friendly");
        FriendlyGangRep.Activated += (menu, item) =>
        {
            Player.RelationshipManager.GangRelationships.SetFriendlyReputations();
            menu.Visible = false;
        };

        GangItems.AddItem(SetAsGangMember);
        GangItems.AddItem(LeaveGang);
        GangItems.AddItem(SetGangRepDefault);
        GangItems.AddItem(SetGangRepFriendly);
        GangItems.AddItem(SetGangRepHostile);
        GangItems.AddItem(DefaultGangRep);
        GangItems.AddItem(RandomGangRep);
        GangItems.AddItem(RandomSingleGangRep);
        GangItems.AddItem(HostileGangRep);
        GangItems.AddItem(FriendlyGangRep);

    }
    private void CreateDispatcherMenu()
    {
        UIMenu DispatcherMenu = MenuPool.AddSubMenu(Debug, "Dispatcher");
        DispatcherMenu.SetBannerType(EntryPoint.LSRedColor);
        UIMenuListScrollerItem<Agency> SpawnAgencyFoot = new UIMenuListScrollerItem<Agency>("Cop Random On-Foot Spawn", "Spawn a random agency ped on foot", Agencies.GetAgencies());
        SpawnAgencyFoot.Activated += (menu, item) =>
        {
            if (SpawnAgencyFoot.SelectedItem.Classification == Classification.EMS)
            {
                Dispatcher.DebugSpawnEMT(SpawnAgencyFoot.SelectedItem.ID, true);
            }
            else
            {
                Dispatcher.DebugSpawnCop(SpawnAgencyFoot.SelectedItem.ID, true);
            }
            menu.Visible = false;
        };
        UIMenuListScrollerItem<Agency> SpawnAgencyVehicle = new UIMenuListScrollerItem<Agency>("Cop Random Vehicle Spawn", "Spawn a random agency ped with a vehicle", Agencies.GetAgencies());
        SpawnAgencyVehicle.Activated += (menu, item) =>
        {
            if (SpawnAgencyVehicle.SelectedItem.Classification == Classification.EMS)
            {
                Dispatcher.DebugSpawnEMT(SpawnAgencyVehicle.SelectedItem.ID, false);
            }
            else
            {
                Dispatcher.DebugSpawnCop(SpawnAgencyVehicle.SelectedItem.ID, false);
            }
            menu.Visible = false;
        };
        UIMenuListScrollerItem<Gang> SpawnGangFoot = new UIMenuListScrollerItem<Gang>("Gang Random On-Foot Spawn", "Spawn a random gang ped on foot", Gangs.GetAllGangs());
        SpawnGangFoot.Activated += (menu, item) =>
        {
            Dispatcher.DebugSpawnGang(SpawnGangFoot.SelectedItem.ID, true);
            menu.Visible = false;
        };
        UIMenuListScrollerItem<Gang> SpawnGangVehicle = new UIMenuListScrollerItem<Gang>("Gang Random Vehicle Spawn", "Spawn a random gang ped with a vehicle", Gangs.GetAllGangs());
        SpawnGangVehicle.Activated += (menu, item) =>
        {
            Dispatcher.DebugSpawnGang(SpawnGangVehicle.SelectedItem.ID, false);
            menu.Visible = false;
        };
        UIMenuItem SpawnRockblock = new UIMenuItem("Spawn Roadblock", "Spawn roadblock");
        SpawnRockblock.Activated += (menu, item) =>
        {
            Dispatcher.DebugSpawnRoadblock();
            menu.Visible = false;
        };

        UIMenuItem DespawnRockblock = new UIMenuItem("Despawn Roadblock", "Despawn roadblock");
        DespawnRockblock.Activated += (menu, item) =>
        {
            Dispatcher.DebugRemoveRoadblock();
            menu.Visible = false;
        };

        UIMenuItem PlayScanner = new UIMenuItem("Play Scanner", "Play some random scanner audio");
        PlayScanner.Activated += (menu, item) =>
        {
            Player.Scanner.DebugPlayDispatch();
            menu.Visible = false;
        };






        DispatcherMenu.AddItem(SpawnAgencyFoot);
        DispatcherMenu.AddItem(SpawnAgencyVehicle);
        DispatcherMenu.AddItem(SpawnGangFoot);
        DispatcherMenu.AddItem(SpawnGangVehicle);
        DispatcherMenu.AddItem(SpawnRockblock);
        DispatcherMenu.AddItem(DespawnRockblock);
        DispatcherMenu.AddItem(PlayScanner);
    }
    private void Frecam()
    {
        GameFiber.StartNew(delegate
        {
            FreeCam = new Camera(false);
            FreeCam.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
            FreeCam.Position = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
            Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
            FreeCam.Rotation = new Rotator(r.X, r.Y, r.Z);
            FreeCam.Active = true;
            Game.LocalPlayer.HasControl = false;
            //This is all adapted from https://github.com/CamxxCore/ScriptCamTool/blob/master/GTAV_ScriptCamTool/PositionSelector.cs#L59
            while (!Game.IsKeyDownRightNow(Keys.P))
            {
                if (Game.IsKeyDownRightNow(Keys.W))
                {
                    FreeCam.Position += NativeHelper.GetCameraDirection(FreeCam, FreeCamScale);
                }
                else if (Game.IsKeyDownRightNow(Keys.S))
                {
                    FreeCam.Position -= NativeHelper.GetCameraDirection(FreeCam, FreeCamScale);
                }
                if (Game.IsKeyDownRightNow(Keys.A))
                {
                    FreeCam.Position = NativeHelper.GetOffsetPosition(FreeCam.Position, FreeCam.Rotation.Yaw, -1.0f * FreeCamScale);
                }
                else if (Game.IsKeyDownRightNow(Keys.D))
                {
                    FreeCam.Position = NativeHelper.GetOffsetPosition(FreeCam.Position, FreeCam.Rotation.Yaw, 1.0f * FreeCamScale);
                }
                FreeCam.Rotation += new Rotator(NativeFunction.Natives.GET_CONTROL_NORMAL<float>(2, 221) * -4f, 0, NativeFunction.Natives.GET_CONTROL_NORMAL<float>(2, 220) * -5f) * FreeCamScale;

                NativeFunction.Natives.SET_FOCUS_POS_AND_VEL(FreeCam.Position.X, FreeCam.Position.Y, FreeCam.Position.Z, 0f, 0f, 0f);

                if (Game.IsKeyDownRightNow(Keys.O))
                {
                    if (FreeCamScale == 1.0f)
                    {
                        FreeCamScale = 0.25f;
                    }
                    else
                    {
                        FreeCamScale = 1.0f;
                    }
                }

                if (Game.IsKeyDownRightNow(Keys.J))
                {
                    Game.LocalPlayer.Character.Position = FreeCam.Position;
                    Game.LocalPlayer.Character.Heading = FreeCam.Heading;
                }

                string FreeCamString = FreeCamScale == 1.0f ? "Regular Scale" : "Slow Scale";
                Game.DisplayHelp($"Press P to Exit~n~Press O To Change Scale Current: {FreeCamString}~n~Press J To Move Player to Position");
                GameFiber.Yield();
            }
            FreeCam.Active = false;
            Game.LocalPlayer.HasControl = true;
            NativeFunction.Natives.CLEAR_FOCUS();
        }, "Run Debug Logic");
    }
    private void LogGameLocation()
    {
        Vector3 pos = Game.LocalPlayer.Character.Position;
        float Heading = Game.LocalPlayer.Character.Heading;
        string text1 = NativeHelper.GetKeyboardInput("LocationType");
        string text2 = NativeHelper.GetKeyboardInput("Name");
        WriteToLogLocations($"new GameLocation(new Vector3({pos.X}f, {pos.Y}f, {pos.Z}f), {Heading}f,new Vector3({pos.X}f, {pos.Y}f, {pos.Z}f), {Heading}f, LocationType.{text1}, \"{text2}\", \"{text2}\"),");
    }
    private void LogGameLocationSimple()
    {
        Vector3 pos = Game.LocalPlayer.Character.Position;
        float Heading = Game.LocalPlayer.Character.Heading;
        string text1 = NativeHelper.GetKeyboardInput("LocationType");
        string text2 = NativeHelper.GetKeyboardInput("Name");
        string text3 = NativeHelper.GetKeyboardInput("Description");
        WriteToLogLocations($"new Vector3({pos.X}f, {pos.Y}f, {pos.Z}f), {Heading}f, \"{text2}\", \"{text3}\"),  //{text1}");
    }
    private void LogSpawnPosition()
    {
        Vector3 pos = Game.LocalPlayer.Character.Position;
        float Heading = Game.LocalPlayer.Character.Heading;
        WriteToLogLocations($"new ConditionalLocation(new Vector3({pos.X}f, {pos.Y}f, {pos.Z}f), {Heading}f, 75f),");
    }
    private void LogCameraPosition()
    {

        if (FreeCam.Active)
        {
            Vector3 pos = FreeCam.Position;
            Rotator r = FreeCam.Rotation;
            Vector3 direction = NativeHelper.GetCameraDirection(FreeCam);
            WriteToLogCameraPosition($", CameraPosition = new Vector3({pos.X}f, {pos.Y}f, {pos.Z}f), CameraDirection = new Vector3({direction.X}f, {direction.Y}f, {direction.Z}f), CameraRotation = new Rotator({r.Pitch}f, {r.Roll}f, {r.Yaw}f);");
        }
        else
        {
            uint CameraHAndle = NativeFunction.Natives.GET_RENDERING_CAM<uint>();
            Vector3 pos = NativeFunction.Natives.GET_CAM_COORD<Vector3>(CameraHAndle);
            Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
            Vector3 direction = NativeHelper.GetGameplayCameraDirection();
            WriteToLogCameraPosition($", CameraPosition = new Vector3({pos.X}f, {pos.Y}f, {pos.Z}f), CameraDirection = new Vector3({direction.X}f, {direction.Y}f, {direction.Z}f), CameraRotation = new Rotator({r.X}f, {r.Y}f, {r.Z}f);");
        }
    }
    private void LogGameInterior()
    {
        string text1 = NativeHelper.GetKeyboardInput("Name");
        string toWrite = $"new Interior({Player.CurrentLocation?.CurrentInterior?.ID}, \"{text1}\"),";
        WriteToLogInteriors(toWrite);
    }
    private void WriteToLogCameraPosition(String TextToLog)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(TextToLog + System.Environment.NewLine);
        File.AppendAllText("Plugins\\LosSantosRED\\" + "CameraPositions.txt", sb.ToString());
        sb.Clear();
    }
    private void WriteToLogLocations(String TextToLog)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(TextToLog + System.Environment.NewLine);
        File.AppendAllText("Plugins\\LosSantosRED\\" + "StoredLocations.txt", sb.ToString());
        sb.Clear();
    }
    private void WriteToLogInteriors(String TextToLog)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(TextToLog + System.Environment.NewLine);
        File.AppendAllText("Plugins\\LosSantosRED\\" + "StoredInteriors.txt", sb.ToString());
        sb.Clear();
    }
    private void SpawnGunAttackers()
    {
        GameFiber.StartNew(delegate
        {
            bool isInvince = true;
            Ped coolguy = new Ped(Game.LocalPlayer.Character.GetOffsetPositionRight(10f).Around2D(10f));
            GameFiber.Yield();
            if (coolguy.Exists())
            {
                coolguy.BlockPermanentEvents = true;
                coolguy.KeepTasks = true;

                coolguy.Inventory.GiveNewWeapon(WeaponHash.Pistol, 50, true);
                coolguy.IsInvincible = true;
                //if (RandomItems.RandomPercent(30))
                //{
                //    coolguy.Inventory.GiveNewWeapon(WeaponHash.Pistol, 50, true);
                //}
                //else if (RandomItems.RandomPercent(30))
                //{
                //    coolguy.Inventory.GiveNewWeapon(WeaponHash.Bat, 1, true);
                //}
                coolguy.Tasks.FightAgainstClosestHatedTarget(250f,-1);
                PedExt pedExt = new PedExt(coolguy, Settings, true, false, false, false, "Test1", Crimes, Weapons, "CRIMINA", World, true);
                pedExt.WasEverSetPersistent = true;
                World.Pedestrians.AddEntity(pedExt);
            }
            while (coolguy.Exists() && !Game.IsKeyDownRightNow(Keys.P))
            {
                Game.DisplayHelp($"Attackers Spawned! ~n~P to Delete ~n~O to Flee~n~L to Toggle Invincible");
                if (Game.IsKeyDownRightNow(Keys.L))
                {
                    isInvince = !isInvince;
                    coolguy.IsInvincible = isInvince;
                    Game.DisplaySubtitle($"isInvince {isInvince}");
                }
                if (Game.IsKeyDownRightNow(Keys.O))
                {
                    coolguy.Tasks.Clear();
                    coolguy.Tasks.Flee(Game.LocalPlayer.Character, 1000f, -1);
                }
                GameFiber.Sleep(25);
            }
            if (coolguy.Exists())
            {
                coolguy.Delete();
            }
        }, "Run Debug Logic");
    }


    private void GiveClosestGun()
    {
        PedExt toChoose = World.Pedestrians.PedExts.OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
        if(toChoose != null && toChoose.Pedestrian.Exists())
        {
            toChoose.Pedestrian.Inventory.GiveNewWeapon(WeaponHash.Pistol, 50, true);
            EntryPoint.WriteToConsole($"Gave {toChoose.Pedestrian.Handle} Weapon");
        }

    }

    private void SpawnNoGunAttackers()
    {
        GameFiber.StartNew(delegate
        {
            Ped coolguy = new Ped(Game.LocalPlayer.Character.GetOffsetPositionFront(10f).Around2D(10f));
            GameFiber.Yield();
            if (coolguy.Exists())
            {
                coolguy.BlockPermanentEvents = true;
                coolguy.KeepTasks = true;
                coolguy.IsInvincible = true;
                PedExt pedExt = new PedExt(coolguy, Settings,true,false,false,false,"Test1", Crimes, Weapons, "CRIMINA", World, true);
                pedExt.WasEverSetPersistent = true;
                World.Pedestrians.AddEntity(pedExt);
                NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", coolguy, 281, true);//Can Writhe
                NativeFunction.CallByName<bool>("SET_PED_DIES_WHEN_INJURED", coolguy, false);

                if (RandomItems.RandomPercent(30))
                {
                    coolguy.Inventory.GiveNewWeapon(WeaponHash.Bat, 1, true);
                }
                else if (RandomItems.RandomPercent(30))
                {
                    coolguy.Inventory.GiveNewWeapon(WeaponHash.Knife, 1, true);
                }
                coolguy.Tasks.FightAgainstClosestHatedTarget(250f, -1);
                //coolguy.Tasks.FightAgainst(Game.LocalPlayer.Character);
            }
            while (coolguy.Exists() && !Game.IsKeyDownRightNow(Keys.P))
            {
                 Game.DisplayHelp($"Attackers Spawned! Press P to Delete O to Flee");


                if (Game.IsKeyDownRightNow(Keys.O))
                {
                    coolguy.Tasks.Clear();
                    coolguy.Tasks.Flee(Game.LocalPlayer.Character, 1000f, -1);
                }
                GameFiber.Sleep(25);
            }
            if (coolguy.Exists())
            {
                coolguy.Delete();
            }

        }, "Run Debug Logic");
    }
    private void PrintPersistentEntities()
    {
        int TotalEntities = 0;
        EntryPoint.WriteToConsole($"SPAWNED ENTITIES ===============================", 0);
        foreach (Entity ent in EntryPoint.SpawnedEntities)
        {
            if (ent.Exists())
            {
                TotalEntities++;
                EntryPoint.WriteToConsole($"SPAWNED ENTITY STILL EXISTS {ent.Handle} {ent.GetType()} {ent.Model.Name} Dead: {ent.IsDead} Position: {ent.Position}", 0);
            }
        }
        EntryPoint.WriteToConsole($"SPAWNED ENTITIES =============================== TOTAL: {TotalEntities}", 0);

        TotalEntities = 0;

        List<Entity> AllEntities = Rage.World.GetAllEntities().ToList();
        EntryPoint.WriteToConsole($"PERSISTENT ENTITIES ===============================", 0);
        foreach (Entity ent in AllEntities)
        {
            if (ent.Exists() && ent.IsPersistent)
            {
                TotalEntities++;
                EntryPoint.WriteToConsole($"PERSISTENT ENTITY STILL EXISTS {ent.Handle} {ent.GetType()}  {ent.Model.Name} Dead: {ent.IsDead} Position: {ent.Position}", 0);
            }
        }
        EntryPoint.WriteToConsole($"PERSISTENT ENTITIES =============================== TOTAL: {TotalEntities}", 0);
    }
    private void PrintAllEntities()
    {
        int TotalEntities = 0;
        List<Entity> AllEntities = Rage.World.GetAllEntities().ToList();
        EntryPoint.WriteToConsole($"ENTITIES ===============================", 0);
        foreach (Entity ent in AllEntities)
        {
            if (ent.Exists())
            {
                TotalEntities++;
                EntryPoint.WriteToConsole($"ENTITY {ent.Handle} {ent.GetType()}  {ent.Model.Name} Dead: {ent.IsDead} Position: {ent.Position} Heading {ent.Heading}", 0);
            }
        }
        EntryPoint.WriteToConsole($"ENTITIES =============================== TOTAL: {TotalEntities}", 0);
    }
    private void HighlightProp()
    {

        Entity ClosestEntity = Rage.World.GetClosestEntity(Game.LocalPlayer.Character.GetOffsetPositionFront(2f), 2f, GetEntitiesFlags.ConsiderAllObjects | GetEntitiesFlags.ExcludePlayerPed);
        if (ClosestEntity.Exists())
        {
            Vector3 DesiredPos = ClosestEntity.GetOffsetPositionFront(-0.5f);
            EntryPoint.WriteToConsole($"Closest Object = {ClosestEntity.Model.Name} {ClosestEntity.Model.Hash}", 5);
            EntryPoint.WriteToConsole($"Closest Object X {ClosestEntity.Model.Dimensions.X} Y {ClosestEntity.Model.Dimensions.Y} Z {ClosestEntity.Model.Dimensions.Z}", 5);
            uint GameTimeStartedDisplaying = Game.GameTime;
            while (Game.GameTime - GameTimeStartedDisplaying <= 5000)
            {
                Rage.Debug.DrawArrowDebug(DesiredPos + new Vector3(0f, 0f, 0.5f), Vector3.Zero, Rotator.Zero, 1f, Color.Yellow);
                GameFiber.Yield();
            }

        }
    }

}