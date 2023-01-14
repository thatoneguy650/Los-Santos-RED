﻿using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player.Activity;
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
    private string FreeCamString = "Regular";
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
    private INameProvideable Names;

    private IPlateTypes PlateTypes;

    private Vector3 Offset;
    private Rotator Rotation;
    private bool isPrecise;
    private bool isRunning;
    private uint GameTimeLastAttached;
    private UIMenu vehicleItemsMenu;

    public DebugMenu(MenuPool menuPool, IActionable player, IWeapons weapons, RadioStations radioStations, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, ITimeControllable time, 
        IEntityProvideable world, ITaskerable tasker, Dispatcher dispatcher, IAgencies agencies, IGangs gangs, IModItems modItems, ICrimes crimes, IPlateTypes plateTypes, INameProvideable names)
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
        PlateTypes = plateTypes;
        Names = names;
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
            UpdateVehicleItems();
            Debug.Visible = true;
        }
    }
    public override void Toggle()
    {
        if (!Debug.Visible)
        {
            UpdateVehicleItems();
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
        CreateHelperItems();
        CreateRelationshipsMenu();
        CreateVehicleMenu();
    }
    private void UpdateVehicleItems()
    {
        vehicleItemsMenu.Clear();
        if (Player.CurrentVehicle == null || !Player.CurrentVehicle.Vehicle.Exists())
        {
            return;
        }
        CreatePlateMenuItem();
        CreateLiveryMenuItem();
    }
    private void CreateVehicleMenu()
    {
        vehicleItemsMenu = MenuPool.AddSubMenu(Debug, "Vehicle Menu");
        vehicleItemsMenu.SetBannerType(EntryPoint.LSRedColor);
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Change various vehicle items.";

        UpdateVehicleItems();



    }

    private void CreateLiveryMenuItem()
    {
        int Total = NativeFunction.Natives.GET_VEHICLE_LIVERY_COUNT<int>(Player.CurrentVehicle.Vehicle);
        if (Total == -1)
        {
            return;
        }
        UIMenuNumericScrollerItem<int> LogLocationMenu = new UIMenuNumericScrollerItem<int>("Set Livery", "Set the vehicle Livery", 0, Total, 1);
        LogLocationMenu.Activated += (menu, item) =>
        {
            if (Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
            {
                NativeFunction.Natives.SET_VEHICLE_LIVERY(Player.CurrentVehicle.Vehicle, LogLocationMenu.Value);
                Game.DisplaySubtitle($"SET LIVERY {LogLocationMenu.Value}");
            }

        };
        vehicleItemsMenu.AddItem(LogLocationMenu);
    }

    private void CreatePlateMenuItem()
    {
        UIMenuListScrollerItem<PlateType> plateIndex = new UIMenuListScrollerItem<PlateType>("Plate Type","Select Plate Type to change",PlateTypes.PlateTypeManager.PlateTypeList);
        plateIndex.Activated += (menu, item) =>
        {
            if (Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
            {
                PlateType NewType = PlateTypes.GetPlateType(plateIndex.SelectedItem.Index);
                if (NewType != null)
                {
                    string NewPlateNumber = NewType.GenerateNewLicensePlateNumber();
                    if (NewPlateNumber != "")
                    {
                        Player.CurrentVehicle.Vehicle.LicensePlate = NewPlateNumber;
                    }
                    NativeFunction.CallByName<int>("SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", Player.CurrentVehicle.Vehicle, NewType.Index);
                    Game.DisplaySubtitle($" PlateIndex: {plateIndex.SelectedItem.Index}, Index: {NewType.Index}, State: {NewType.State}, Description: {NewType.Description}");
                }
                else
                {
                    Game.DisplaySubtitle($" PlateIndex: {plateIndex.SelectedItem.Index} None Found");
                }
            }

        };
        vehicleItemsMenu.AddItem(plateIndex);
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
            TaxiDropOff TaxiDropOff = new TaxiDropOff(Game.LocalPlayer.Character.GetOffsetPositionFront(10f), Settings, Crimes, Weapons, Names, World);
            TaxiDropOff.Setup();
            TaxiDropOff.Start();
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

        PlayerStateItemsMenu.AddItem(RemoveButtonPrompts);
        PlayerStateItemsMenu.AddItem(TaxiSpawn);

    }
    private void CreateRelationshipsMenu()
    {
        UIMenu PlayerStateItemsMenu = MenuPool.AddSubMenu(Debug, "Relationships Menu");
        PlayerStateItemsMenu.SetBannerType(EntryPoint.LSRedColor);
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Change various relationship items.";


        UIMenuItem AddOfficerFriendly = new UIMenuItem("Add Officer Friendly", "Add officer friendly contact and set relationship to friendly");
        AddOfficerFriendly.Activated += (menu, item) =>
        {
            Player.RelationshipManager.OfficerFriendlyRelationship.Reset(false);
            Player.RelationshipManager.OfficerFriendlyRelationship.SetReputation(Player.RelationshipManager.OfficerFriendlyRelationship.RepMaximum,false);
            Player.RelationshipManager.OfficerFriendlyRelationship.SetMoneySpent(90000,false);
            Player.CellPhone.AddContact(new CorruptCopContact(StaticStrings.OfficerFriendlyContactName), false);
            menu.Visible = false;
        };
        UIMenuItem AddUndergroundGuns = new UIMenuItem("Add Underground Guns", "Add underground guns contact and set relationship to friendly");
        AddUndergroundGuns.Activated += (menu, item) =>
        {
            Player.RelationshipManager.GunDealerRelationship.Reset(false);
            Player.RelationshipManager.GunDealerRelationship.SetReputation(Player.RelationshipManager.GunDealerRelationship.RepMaximum, false);
            Player.RelationshipManager.GunDealerRelationship.SetMoneySpent(90000, false);
            Player.CellPhone.AddContact(new GunDealerContact(StaticStrings.UndergroundGunsContactName), false);
            menu.Visible = false;
        };
        
        PlayerStateItemsMenu.AddItem(AddOfficerFriendly);
        PlayerStateItemsMenu.AddItem(AddUndergroundGuns);
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

        UIMenuListScrollerItem<int>  SetWantedLevel = new UIMenuListScrollerItem<int>("Set Wanted Level", "Set wanted at the desired level", new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        SetWantedLevel.Activated += (menu, item) =>
        {
            if (SetWantedLevel.SelectedItem <= Settings.SettingsManager.PoliceSettings.MaxWantedLevel)
            {
                Player.SetWantedLevel(SetWantedLevel.SelectedItem, "Debug Menu", true);
                menu.Visible = false;
            }
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

        UIMenuItem SetNearestWanted = new UIMenuItem("Set Nearest Wanted", "Set the nearest ped wanted");
        SetNearestWanted.Activated += (menu, item) =>
        {
            SetNearestPedWanted();
            menu.Visible = false;
        };


        UIMenuItem ToggleCopTasking = new UIMenuItem("Toggle Cop Tasking", "Toggle player cop as taskable or not");
        ToggleCopTasking.Activated += (menu, item) =>
        {
            Player.ToggleCopTaskable();
            menu.Visible = false;
        };



        CrimeItemsMenu.AddItem(SetWantedLevel);
        CrimeItemsMenu.AddItem(ToggleInvestigation);
        CrimeItemsMenu.AddItem(SpawnGunAttackersMenu);
        CrimeItemsMenu.AddItem(SpawnNoGunAttackersMenu);
        CrimeItemsMenu.AddItem(StartRandomCrime);
        CrimeItemsMenu.AddItem(GiveClosesetGun);
        CrimeItemsMenu.AddItem(SetNearestWanted);
        CrimeItemsMenu.AddItem(ToggleCopTasking);
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








        //UIMenuItem GoToReleaseSettings = new UIMenuItem("Quick Set Release Settings", "Set some release settings quickly.");
        //GoToReleaseSettings.Activated += (menu, item) =>
        //{
        //    Settings.SetRelease();

        //    menu.Visible = false;
        //};
        ////OtherItemsMenu.AddItem(GoToReleaseSettings);
        //UIMenuItem GoToHardCoreSettings = new UIMenuItem("Quick Set Hardcore Settings", "Set the very difficult settings.");
        //GoToHardCoreSettings.Activated += (menu, item) =>
        //{
        //    Settings.SetHard();
        //    menu.Visible = false;
        //};
        ////OtherItemsMenu.AddItem(GoToHardCoreSettings);


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
            Player.Reset(true, false, true, true, true, false, false, false, false, false, false, false, false, false, false, true);
            Player.Surrendering.UnSetArrestedAnimation();
            menu.Visible = false;
        };
        OtherItemsMenu.AddItem(UnSetArrested);


        UIMenuListScrollerItem<ModTaskGroup> taskGroups = new UIMenuListScrollerItem<ModTaskGroup>("Enable Task Groups", "Enable or disable task groups.", EntryPoint.ModController.TaskGroups);
        taskGroups.Activated += (menu, item) =>
        {
            taskGroups.SelectedItem.IsRunning = !taskGroups.SelectedItem.IsRunning;

            taskGroups.Items = EntryPoint.ModController.TaskGroups;
            taskGroups.Reformat();

            //item.Text = taskGroups.SelectedItem.ToString();
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

        UIMenuNumericScrollerItem<int> logLevelChange = new UIMenuNumericScrollerItem<int>("Log Level", "Sets the log level of the mod", 0,5,1);
        logLevelChange.Value = EntryPoint.LogLevel;
        logLevelChange.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            EntryPoint.LogLevel = logLevelChange.Value;
        };
        OtherItemsMenu.AddItem(logLevelChange);

        //HighlightProp()


    }
    private void CreateHelperItems()
    {
        UIMenu HelperMenuItem = MenuPool.AddSubMenu(Debug, "Helper Menu");
        HelperMenuItem.SetBannerType(EntryPoint.LSRedColor);
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Various helper items";



        UIMenuItem propAttachMenu = new UIMenuItem("Prop Attachment", "Do some prop attachments");
        propAttachMenu.Activated += (menu, item) =>
        {
            SetPropAttachment();
            menu.Visible = false;
        };
        HelperMenuItem.AddItem(propAttachMenu);

        UIMenuItem weaponAliasMenu = new UIMenuItem("Weapon Alias Attachment", "Do some weapon alias prop attachments");
        weaponAliasMenu.Activated += (menu, item) =>
        {
            SetWeaponAliasAttachment();
            menu.Visible = false;
        };
        HelperMenuItem.AddItem(weaponAliasMenu);

        UIMenuItem particleAttachMenu = new UIMenuItem("Particle Attachment", "Get some particle offsets");
        particleAttachMenu.Activated += (menu, item) =>
        {
            SetParticleAttachment();
            menu.Visible = false;
        };
        HelperMenuItem.AddItem(particleAttachMenu);

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
                Dispatcher.DebugSpawnEMT(SpawnAgencyFoot.SelectedItem.ID, true, false);
            }
            else
            {
                Dispatcher.DebugSpawnCop(SpawnAgencyFoot.SelectedItem.ID, true, false);
            }
            menu.Visible = false;
        };
        UIMenuListScrollerItem<Agency> SpawnAgencyVehicle = new UIMenuListScrollerItem<Agency>("Cop Random Vehicle Spawn", "Spawn a random agency ped with a vehicle", Agencies.GetAgencies());
        SpawnAgencyVehicle.Activated += (menu, item) =>
        {
            if (SpawnAgencyVehicle.SelectedItem.Classification == Classification.EMS)
            {
                Dispatcher.DebugSpawnEMT(SpawnAgencyVehicle.SelectedItem.ID, false, false);
            }
            else
            {
                Dispatcher.DebugSpawnCop(SpawnAgencyVehicle.SelectedItem.ID, false, false);
            }
            menu.Visible = false;
        };
        UIMenuListScrollerItem<Agency> SpawnEmptyAgencyVehicle = new UIMenuListScrollerItem<Agency>("Cop Random Empty Vehicle Spawn", "Spawn a random agency empty vehicle", Agencies.GetAgencies());
        SpawnEmptyAgencyVehicle.Activated += (menu, item) =>
        {
            if (SpawnEmptyAgencyVehicle.SelectedItem.Classification == Classification.EMS)
            {
                Dispatcher.DebugSpawnEMT(SpawnEmptyAgencyVehicle.SelectedItem.ID, false, true);
            }
            else
            {
                Dispatcher.DebugSpawnCop(SpawnEmptyAgencyVehicle.SelectedItem.ID, false, true);
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
        UIMenuNumericScrollerItem<float> SpawnRockblock = new UIMenuNumericScrollerItem<float>("Spawn Roadblock", "Spawn roadblock",10f,200f,10f);
        SpawnRockblock.Activated += (menu, item) =>
        {
            Dispatcher.DebugSpawnRoadblock(SpawnRockblock.Value);
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
            Player.Scanner.ForceRandomDispatch();
            menu.Visible = false;
        };

        UIMenuItem RemoveCops = new UIMenuItem("Remove Cops", "Removes all the police");
        RemoveCops.Activated += (menu, item) =>
        {
            World.Pedestrians.ClearPolice();
            World.Vehicles.ClearPolice();
            menu.Visible = false;
        };




        DispatcherMenu.AddItem(SpawnAgencyFoot);
        DispatcherMenu.AddItem(SpawnAgencyVehicle);
        DispatcherMenu.AddItem(SpawnEmptyAgencyVehicle);
        DispatcherMenu.AddItem(SpawnGangFoot);
        DispatcherMenu.AddItem(SpawnGangVehicle);
        DispatcherMenu.AddItem(SpawnRockblock);
        DispatcherMenu.AddItem(DespawnRockblock);
        DispatcherMenu.AddItem(PlayScanner);
        DispatcherMenu.AddItem(RemoveCops);
    }
    private void Frecam()
    {
        GameFiber.StartNew(delegate
        {
            try
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
                            FreeCamString = "Slow";
                            FreeCamScale = 0.25f;
                        }
                        else if (FreeCamScale == 0.25f)
                        {
                            FreeCamString = "Super Slow";
                            FreeCamScale = 0.05f;
                        }
                        else
                        {
                            FreeCamString = "Regular";
                            FreeCamScale = 1.0f;
                        }
                        GameFiber.Sleep(100);
                    }

                    if (Game.IsKeyDownRightNow(Keys.J))
                    {
                        Game.LocalPlayer.Character.Position = FreeCam.Position;
                        Game.LocalPlayer.Character.Heading = FreeCam.Heading;
                    }

                    //string FreeCamString = FreeCamScale == 1.0f ? "Regular Scale" : "Slow Scale";
                    Game.DisplayHelp($"Press P to Exit~n~Press O To Change Scale Current: {FreeCamString}~n~Press J To Move Player to Position");
                    GameFiber.Yield();
                }
                FreeCam.Active = false;
                Game.LocalPlayer.HasControl = true;
                NativeFunction.Natives.CLEAR_FOCUS();
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
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
            try
            {
                bool isInvince = true;
                Ped coolguy = new Ped(Game.LocalPlayer.Character.GetOffsetPositionRight(10f).Around2D(10f));
                GameFiber.Yield();
                if (coolguy.Exists())
                {
                    coolguy.BlockPermanentEvents = true;
                    coolguy.KeepTasks = true;

                    coolguy.Inventory.GiveNewWeapon(WeaponHash.Pistol, 50, true);
                    //coolguy.IsInvincible = true;
                    //if (RandomItems.RandomPercent(30))
                    //{
                    //    coolguy.Inventory.GiveNewWeapon(WeaponHash.Pistol, 50, true);
                    //}
                    //else if (RandomItems.RandomPercent(30))
                    //{
                    //    coolguy.Inventory.GiveNewWeapon(WeaponHash.Bat, 1, true);
                    //}
                    coolguy.Tasks.FightAgainstClosestHatedTarget(250f, -1);
                    PedExt pedExt = new PedExt(coolguy, Settings, true, false, false, false, "Test1", Crimes, Weapons, "CRIMINAL", World, true);
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
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
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
    private void SetNearestPedWanted()
    {
        PedExt toChoose = World.Pedestrians.PedExts.Where(x=> x.Handle != Player.Handle).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
        if (toChoose != null && toChoose.Pedestrian.Exists())
        {
            toChoose.SetWantedLevel(3);
            EntryPoint.WriteToConsole($"Gave {toChoose.Pedestrian.Handle} Weapon");
        }

    }
    private void SpawnNoGunAttackers()
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                Ped coolguy = new Ped(Game.LocalPlayer.Character.GetOffsetPositionFront(10f).Around2D(10f));
                GameFiber.Yield();
                if (coolguy.Exists())
                {
                    coolguy.BlockPermanentEvents = true;
                    coolguy.KeepTasks = true;
                    //coolguy.IsInvincible = true;
                    PedExt pedExt = new PedExt(coolguy, Settings, true, false, false, false, "Test1", Crimes, Weapons, "CRIMINAL", World, true);
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
                    //coolguy.Tasks.FightAgainstClosestHatedTarget(250f, -1);
                    coolguy.Tasks.FightAgainst(Game.LocalPlayer.Character);
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
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
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
    private void SetPropAttachment()
    {
        string PropName = NativeHelper.GetKeyboardInput("prop_holster_01");
        try
        {
            Rage.Object SmokedItem = new Rage.Object(Game.GetHashKey(PropName), Player.Character.GetOffsetPositionUp(50f));
            GameFiber.Yield();

            string headBoneName = "BONETAG_HEAD";
            string handRBoneName = "BONETAG_R_PH_HAND";
            string handLBoneName = "BONETAG_L_PH_HAND";

            string boneName = headBoneName;



            string thighRBoneName = "BONETAG_R_THIGH";
            string thighLBoneName = "BONETAG_L_THIGH";
            string pelvisBoneName = "BONETAG_PELVIS";
            string spineRootBoneName = "BONETAG_SPINE_ROOT";
            string spineBoneName = "BONETAG_SPINE";
            string wantedBone = NativeHelper.GetKeyboardInput("Head");
            if (wantedBone == "RHand")
            {
                boneName = handRBoneName;
            }
            else if (wantedBone == "LHand")
            {
                boneName = handLBoneName;
            }
            else if (wantedBone == "LThigh")
            {
                boneName = thighLBoneName;
            }
            else if (wantedBone == "RThigh")
            {
                boneName = thighRBoneName;
            }
            else if (wantedBone == "Pelvis")
            {
                boneName = pelvisBoneName;
            }
            else if (wantedBone == "SpineRoot")
            {
                boneName = spineRootBoneName;
            }
            else if (wantedBone == "Spine")
            {
                boneName = spineBoneName;
            }
            else
            {
                boneName = wantedBone;
            }



            uint GameTimeLastAttached = 0;
            Offset = new Vector3();
            Rotation = new Rotator();
            isPrecise = false;
            if (SmokedItem.Exists())
            {
                //Specific for umbrella

                //AnimationDictionary.RequestAnimationDictionay("doors@");
                // NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "doors@", "door_sweep_l_hand_medium", 4.0f, -4.0f, -1, (int)(AnimationFlags.StayInEndFrame | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask), 0, false, false, false);//-1

                //string dictionary = "amb@world_human_binoculars@male@base";
                //string animation = "base";



                string dictionary = NativeHelper.GetKeyboardInput("move_strafe@melee_small_weapon_fps");
                string animation = NativeHelper.GetKeyboardInput("idle");

                AnimationDictionary.RequestAnimationDictionay(dictionary);
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, dictionary, animation, 4.0f, -4.0f, -1, (int)(AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask), 0, false, false, false);//-1


                isRunning = true;

                AttachItem(SmokedItem, boneName, new Vector3(0.0f, 0.0f, 0f), new Rotator(0f, 0f, 0f));



                GameFiber.StartNew(delegate
                {
                    try
                    {
                        while (!Game.IsKeyDownRightNow(Keys.Space) && SmokedItem.Exists())
                        {
                            if (Game.GameTime - GameTimeLastAttached >= 100 && CheckAttachmentkeys())
                            {
                                AttachItem(SmokedItem, boneName, Offset, Rotation);
                                GameTimeLastAttached = Game.GameTime;
                            }


                            if (Game.IsKeyDown(Keys.B))
                            {
                                EntryPoint.WriteToConsole($"Item {PropName} Attached to  {boneName} new Vector3({Offset.X}f,{Offset.Y}f,{Offset.Z}f),new Rotator({Rotation.Pitch}f, {Rotation.Roll}f, {Rotation.Yaw}f)");
                                GameFiber.Sleep(500);
                            }
                            if (Game.IsKeyDown(Keys.N))
                            {
                                isPrecise = !isPrecise;
                                GameFiber.Sleep(500);
                            }
                            if (Game.IsKeyDown(Keys.D0))
                            {
                                isRunning = !isRunning;
                                NativeFunction.Natives.SET_ENTITY_ANIM_SPEED(Player.Character, dictionary, animation, isRunning ? 1.0f : 0.0f);
                                GameFiber.Sleep(500);
                            }
                            float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, dictionary, animation);
                            Game.DisplayHelp($"Press SPACE to Stop~n~Press T-P to Increase~n~Press G=; to Decrease~n~Press B to print~n~Press N Toggle Precise {isPrecise} ~n~Press 0 Pause{isRunning}");
                            Game.DisplaySubtitle($"{Offset.X}f,{Offset.Y}f,{Offset.Z}f -- {Rotation.Pitch}f, {Rotation.Roll}f, {Rotation.Yaw}f");
                            //Game.DisplaySubtitle($"Current Animation Time {AnimationTime}");
                            GameFiber.Yield();
                        }

                        if (SmokedItem.Exists())
                        {
                            SmokedItem.Delete();
                        }
                    }
                    catch (Exception ex)
                    {
                        EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                        EntryPoint.ModController.CrashUnload();
                    }
                }, "Run Debug Logic");
            }
        }
        catch (Exception e)
        {
            Game.DisplayNotification("ERROR DEBUG");
        }
    }
    private void AttachItem(Rage.Object SmokedItem, string boneName, Vector3 offset, Rotator rotator)
    {
        if (SmokedItem.Exists())
        {
            SmokedItem.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, boneName), offset, rotator);

        }
    }
    private bool CheckAttachmentkeys()
    {

        float adderOffset = isPrecise ? 0.001f : 0.01f;
        float rotatorOFfset = isPrecise ? 1.0f : 10f;
        if (Game.IsKeyDownRightNow(Keys.T))//X UP?
        {
            Offset = new Vector3(Offset.X + adderOffset, Offset.Y, Offset.Z);
            return true;
        }
        else if (Game.IsKeyDownRightNow(Keys.G))//X Down?
        {
            Offset = new Vector3(Offset.X - adderOffset, Offset.Y, Offset.Z);
            return true;
        }
        else if (Game.IsKeyDownRightNow(Keys.Y))//Y UP?
        {
            Offset = new Vector3(Offset.X, Offset.Y + adderOffset, Offset.Z);
            return true;
        }
        else if (Game.IsKeyDownRightNow(Keys.H))//Y Down?
        {
            Offset = new Vector3(Offset.X, Offset.Y - adderOffset, Offset.Z);
            return true;
        }
        else if (Game.IsKeyDownRightNow(Keys.U))//Z Up?
        {
            Offset = new Vector3(Offset.X, Offset.Y, Offset.Z + adderOffset);
            return true;
        }
        else if (Game.IsKeyDownRightNow(Keys.J))//Z Down?
        {
            Offset = new Vector3(Offset.X, Offset.Y, Offset.Z - adderOffset);
            return true;
        }

        else if (Game.IsKeyDownRightNow(Keys.I))//XR Up?
        {
            Rotation = new Rotator(Rotation.Pitch + rotatorOFfset, Rotation.Roll, Rotation.Yaw);
            return true;
        }
        else if (Game.IsKeyDownRightNow(Keys.K))//XR Down?
        {
            Rotation = new Rotator(Rotation.Pitch - rotatorOFfset, Rotation.Roll, Rotation.Yaw);
            return true;
        }
        else if (Game.IsKeyDownRightNow(Keys.O))//YR Up?
        {
            Rotation = new Rotator(Rotation.Pitch, Rotation.Roll + rotatorOFfset, Rotation.Yaw);
            return true;
        }
        else if (Game.IsKeyDownRightNow(Keys.L) || Game.IsKeyDownRightNow(Keys.OemPeriod))//YR Down?
        {
            Rotation = new Rotator(Rotation.Pitch, Rotation.Roll - rotatorOFfset, Rotation.Yaw);
            return true;
        }
        else if (Game.IsKeyDownRightNow(Keys.P))//ZR Up?
        {
            Rotation = new Rotator(Rotation.Pitch, Rotation.Roll, Rotation.Yaw + rotatorOFfset);
            return true;
        }
        else if (Game.IsKeyDownRightNow(Keys.OemSemicolon))//ZR Down?
        {
            Rotation = new Rotator(Rotation.Pitch, Rotation.Roll, Rotation.Yaw - rotatorOFfset);
            return true;
        }
        return false;
    }
    private void SetWeaponAliasAttachment()
    {
        //shovel replacing baseball bat?
        string propName = NativeHelper.GetKeyboardInput("gr_prop_gr_hammer_01");
        string weaponHashText = NativeHelper.GetKeyboardInput("1317494643");
        if (uint.TryParse(weaponHashText, out uint WeaponHash))
        {
            NativeFunction.Natives.GIVE_WEAPON_TO_PED(Game.LocalPlayer.Character, WeaponHash, 200, false, false);
            NativeFunction.Natives.SET_CURRENT_PED_WEAPON(Game.LocalPlayer.Character, WeaponHash, true);
            if (Game.LocalPlayer.Character.Inventory.EquippedWeaponObject.Exists())
            {
                Game.LocalPlayer.Character.Inventory.EquippedWeaponObject.IsVisible = false;
            }
            //BONETAG_R_PH_HAND new Vector3(-0.03f,-0.2769999f,-0.06200002f),new Rotator(20f, -101f, 81f)
            Rage.Object weaponObject = null;
            try
            {
                weaponObject = new Rage.Object(propName, Player.Character.GetOffsetPositionUp(50f));
                string HandBoneName = "BONETAG_R_PH_HAND";
                Offset = new Vector3(0f, 0f, 0f);
                Rotation = new Rotator(0f, 0f, 0f);
                if (weaponObject.Exists())
                {
                    AttachItem(weaponObject, HandBoneName, new Vector3(0.0f, 0.0f, 0f), new Rotator(0f, 0f, 0f));

                    GameFiber.StartNew(delegate
                    {
                        try
                        {
                            while (!Game.IsKeyDownRightNow(Keys.Space))
                            {
                                uint currentWeapon;
                                NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(Game.LocalPlayer.Character, out currentWeapon, true);
                                if (currentWeapon != WeaponHash)
                                {
                                    break;
                                }
                                if (Game.GameTime - GameTimeLastAttached >= 100 && CheckAttachmentkeys())
                                {
                                    AttachItem(weaponObject, HandBoneName, Offset, Rotation);
                                    GameTimeLastAttached = Game.GameTime;
                                }
                                if (Game.IsKeyDown(Keys.B))
                                {
                                    EntryPoint.WriteToConsole($"Item {weaponObject} Attached to  {HandBoneName} new Vector3({Offset.X}f,{Offset.Y}f,{Offset.Z}f),new Rotator({Rotation.Pitch}f, {Rotation.Roll}f, {Rotation.Yaw}f)");
                                    GameFiber.Sleep(500);
                                }
                                if (Game.IsKeyDown(Keys.N))
                                {
                                    isPrecise = !isPrecise;
                                    GameFiber.Sleep(500);
                                }
                                Game.DisplaySubtitle($"{Offset.X}f,{Offset.Y}f,{Offset.Z}f -- {Rotation.Pitch}f, {Rotation.Roll}f, {Rotation.Yaw}f");
                                Game.DisplayHelp($"Press SPACE to Stop~n~Press T-P to Increase~n~Press G=; to Decrease~n~Press B to print~n~Press N Toggle Precise {isPrecise}");
                                GameFiber.Yield();
                            }
                            if (weaponObject.Exists())
                            {
                                weaponObject.Delete();
                            }
                            if (Game.LocalPlayer.Character.Inventory.EquippedWeaponObject.Exists())
                            {
                                Game.LocalPlayer.Character.Inventory.EquippedWeaponObject.IsVisible = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                            EntryPoint.ModController.CrashUnload();
                        }
                    }, "Run Debug Logic");
                }
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole($"Error Spawning Model {ex.Message} {ex.StackTrace}");
            }
        }
    }
    private void SetParticleAttachment()
    {
        //shovel replacing baseball bat?
        string propName = NativeHelper.GetKeyboardInput("p_cs_lighter_01");
        string particleGroupName = NativeHelper.GetKeyboardInput("core");
        string particleName = NativeHelper.GetKeyboardInput("ent_anim_cig_smoke");
        Rage.Object weaponObject = null;
        try
        {
            weaponObject = new Rage.Object(propName, Player.Character.GetOffsetPositionUp(50f));
            string HandBoneName = "BONETAG_L_PH_HAND";

            Offset = new Vector3(0.0f, 0.0f, 0.0f);
            Rotation = new Rotator(0f, 0f, 0f);


            Vector3 CoolOffset = new Vector3(0.13f, 0.02f, 0.02f);
            Rotator CoolRotation = new Rotator(-93f, 40f, 0f);
            if (weaponObject.Exists())
            {
                string dictionary = "anim@amb@casino@hangout@ped_male@stand_withdrink@01a@base";
                string animation = "base";

                AnimationDictionary.RequestAnimationDictionay(dictionary);
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, dictionary, animation, 4.0f, -4.0f, -1, (int)(AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask), 0, false, false, false);//-1



                AttachItem(weaponObject, HandBoneName, CoolOffset, CoolRotation);
                LoopedParticle particle = new LoopedParticle(particleGroupName, particleName, weaponObject, new Vector3(0.0f, 0.0f, 0f), Rotator.Zero, 1.5f);
                GameFiber.StartNew(delegate
                {
                    try
                    {
                        while (!Game.IsKeyDownRightNow(Keys.Space))
                        {
                            if (Game.GameTime - GameTimeLastAttached >= 100 && CheckAttachmentkeys())
                            {
                                particle.Stop();
                                particle = new LoopedParticle(particleGroupName, particleName, weaponObject, Offset, Rotation, 1.5f);
                                //AttachItem(weaponObject, HandBoneName, Offset, Rotation);
                                GameTimeLastAttached = Game.GameTime;
                            }
                            if (Game.IsKeyDown(Keys.B))
                            {
                                EntryPoint.WriteToConsole($"Item {weaponObject} Attached to  {HandBoneName} new Vector3({Offset.X}f,{Offset.Y}f,{Offset.Z}f),new Rotator({Rotation.Pitch}f, {Rotation.Roll}f, {Rotation.Yaw}f)");
                                GameFiber.Sleep(500);
                            }
                            if (Game.IsKeyDown(Keys.N))
                            {
                                isPrecise = !isPrecise;
                                GameFiber.Sleep(500);
                            }
                            Game.DisplaySubtitle($"{Offset.X}f,{Offset.Y}f,{Offset.Z}f -- {Rotation.Pitch}f, {Rotation.Roll}f, {Rotation.Yaw}f");
                            Game.DisplayHelp($"Press SPACE to Stop~n~Press T-P to Increase~n~Press G=; to Decrease~n~Press B to print~n~Press N Toggle Precise {isPrecise}");
                            GameFiber.Yield();
                        }
                        if (weaponObject.Exists())
                        {
                            weaponObject.Delete();
                        }
                        if (particle != null)
                        {
                            particle.Stop();
                        }
                    }
                    catch (Exception ex)
                    {
                        EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                        EntryPoint.ModController.CrashUnload();
                    }
                }, "Run Debug Logic");
            }
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"Error Spawning Model {ex.Message} {ex.StackTrace}");
        }

    }
}