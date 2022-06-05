using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

public class DebugMenu : Menu
{
    private UIMenu DispatcherMenu;
    private UIMenu Debug;
    private UIMenuListItem AutoSetRadioStation;
    private UIMenuItem GiveMoney;
    private UIMenuItem SetMoney;
    private UIMenuItem FillHealth;
    private UIMenuItem FillHealthAndArmor;
    private UIMenuItem ForceSober;
    private UIMenuListScrollerItem<Agency> SpawnAgencyFoot;
    private UIMenuListScrollerItem<Agency> SpawnAgencyVehicle;
    private UIMenuNumericScrollerItem<int> FastForwardTime;
    private UIMenuItem GoToReleaseSettings;
    private UIMenuItem GoToHardCoreSettings;
    private UIMenuItem StartRandomCrime;
    private UIMenuItem KillPlayer;
    private UIMenuItem LogCameraPositionMenu;
    private UIMenuItem LogInteriorMenu;
    private UIMenuItem LogLocationMenu;
    private UIMenuItem LogLocationSimpleMenu;
    private UIMenuListItem GetRandomWeapon;
    private UIMenuListItem GetRandomUpgradedWeapon;
    private UIMenuListItem TeleportToPOI;
    private UIMenuItem DefaultGangRep;
    private UIMenuItem RandomGangRep;
    private UIMenuItem SetDateToToday;
    private UIMenuItem Holder1;
    private IActionable Player;
    private RadioStations RadioStations;
    private int RandomWeaponCategory;
    private IWeapons Weapons;
    private IPlacesOfInterest PlacesOfInterest;
    private int PlaceOfInterestSelected;
    private ISettingsProvideable Settings;
    private ITimeControllable Time;
    private Camera FreeCam;
    private float FreeCamScale = 1.0f;
    private UIMenuItem FreeCamMenu;
    private UIMenuItem LoadSPMap;
    private UIMenuItem LoadMPMap;
    private IEntityProvideable World;
    private UIMenuItem HostileGangRep;
    private UIMenuItem FriendlyGangRep;
    private UIMenuItem RandomSingleGangRep;
    private ITaskerable Tasker;
    private MenuPool MenuPool;
    private Dispatcher Dispatcher;
    private IAgencies Agencies;
    private UIMenuListScrollerItem<Gang> SpawnGangFoot;
    private UIMenuListScrollerItem<Gang> SpawnGangVehicle;
    private UIMenuItem SpawnRockblock;
    private IGangs Gangs;
    private UIMenuListScrollerItem<Gang> SetGangRepDefault;
    private UIMenuListScrollerItem<Gang> SetGangRepFriendly;
    private UIMenuListScrollerItem<Gang> SetGangRepHostile;
    private UIMenuItem GetAllItems;
    private IModItems ModItems;
    private int RandomUpgradedWeaponCategory;

    public DebugMenu(MenuPool menuPool, IActionable player, IWeapons weapons, RadioStations radioStations, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, ITimeControllable time, IEntityProvideable world, ITaskerable tasker, Dispatcher dispatcher, IAgencies agencies, IGangs gangs, IModItems modItems)
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
        Debug = new UIMenu("Debug", "Debug Settings");
        Debug.SetBannerType(EntryPoint.LSRedColor);
        menuPool.Add(Debug);

        
        Debug.OnItemSelect += DebugMenuSelect;
        Debug.OnListChange += OnListChange;
        CreateDebugMenu();
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

        DispatcherMenu = MenuPool.AddSubMenu(Debug, "Dispatcher");
        DispatcherMenu.SetBannerType(EntryPoint.LSRedColor);
        DispatcherMenu.OnItemSelect += DispatcherMenuSelect;

        SpawnAgencyFoot = new UIMenuListScrollerItem<Agency>("Cop Random On-Foot Spawn", "Spawn a random agency ped on foot", Agencies.GetAgencies());
        SpawnAgencyVehicle = new UIMenuListScrollerItem<Agency>("Cop Random Vehicle Spawn", "Spawn a random agency ped with a vehicle", Agencies.GetAgencies());

        SpawnGangFoot = new UIMenuListScrollerItem<Gang>("Gang Random On-Foot Spawn", "Spawn a random gang ped on foot", Gangs.GetAllGangs());
        SpawnGangVehicle = new UIMenuListScrollerItem<Gang>("Gang Random Vehicle Spawn", "Spawn a random gang ped with a vehicle", Gangs.GetAllGangs());
        SpawnRockblock = new UIMenuItem("Spawn Roadblock","Spawn roadblock");

        DispatcherMenu.AddItem(SpawnAgencyFoot);
        DispatcherMenu.AddItem(SpawnAgencyVehicle);


        DispatcherMenu.AddItem(SpawnGangFoot);
        DispatcherMenu.AddItem(SpawnGangVehicle);
        DispatcherMenu.AddItem(SpawnRockblock);


        SetGangRepDefault = new UIMenuListScrollerItem<Gang>("Set Gang Default", "Sets the selected gang to the default reputation", Gangs.GetAllGangs());
        SetGangRepFriendly = new UIMenuListScrollerItem<Gang>("Set Gang Friendly", "Sets the selected gang to a friendly reputation", Gangs.GetAllGangs());
        SetGangRepHostile = new UIMenuListScrollerItem<Gang>("Set Gang Hostile", "Sets the selected gang to a hostile reputation", Gangs.GetAllGangs());




        FastForwardTime = new UIMenuNumericScrollerItem<int>("Fast Forward Time", "Fast forward time.", 1, 24, 1) { Formatter = v => v + " Hours"};
        GoToReleaseSettings = new UIMenuItem("Quick Set Release Settings", "Set some release settings quickly.");


        GoToHardCoreSettings = new UIMenuItem("Quick Set Hardcore Settings", "Set the very difficult settings.");


        StartRandomCrime = new UIMenuItem("Start Random Crime", "Trigger a random crime around the map.");
        KillPlayer = new UIMenuItem("Kill Player", "Immediatly die and ragdoll");
        GetRandomWeapon = new UIMenuListItem("Get Random Weapon", "Gives the Player a random weapon and ammo.", Enum.GetNames(typeof(WeaponCategory)).ToList());

        GetRandomUpgradedWeapon = new UIMenuListItem("Get Random Upgraded Weapon", "Gives the Player a random upgraded weapon and ammo.", Enum.GetNames(typeof(WeaponCategory)).ToList());


        GiveMoney = new UIMenuItem("Get Money", "Give you some cash");
        SetMoney = new UIMenuItem("Set Money", "Sets your cash");
        GetAllItems = new UIMenuItem("Get All Items", "Gets 5 of every item");

        FillHealth = new UIMenuItem("Fill Health", "Refill health only");
        FillHealthAndArmor = new UIMenuItem("Fill Health and Armor", "Get loaded for bear");

        ForceSober = new UIMenuItem("Become Sober", "Froces a sober state on the player (if intoxicated)");

        AutoSetRadioStation = new UIMenuListItem("Auto-Set Station", "Will auto set the station any time the radio is on", RadioStations.RadioStationList);
        LogLocationMenu = new UIMenuItem("Log Game Location", "Location Type, Then Name");
        LogLocationSimpleMenu = new UIMenuItem("Log Game Location (Simple)", "Location Type, Then Name");
        LogInteriorMenu = new UIMenuItem("Log Game Interior", "Interior Name");
        LogCameraPositionMenu = new UIMenuItem("Log Camera Position", "Logs current rendering cam post direction and rotation");
        FreeCamMenu = new UIMenuItem("Free Cam", "Start Free Camera Mode");

        LoadSPMap = new UIMenuItem("Load SP Map", "Loads the SP map if you have the MP map enabled");
        LoadMPMap = new UIMenuItem("Load MP Map", "Load the MP map if you have the SP map enabled");


       // TeleportToPOI = new UIMenuListItem("Teleport To POI", "Teleports to A POI on the Map", PlacesOfInterest.GetAllPlaces());


        DefaultGangRep = new UIMenuItem("Set Gang Rep Default", "Sets the player reputation to each gang to the default value");
        RandomGangRep = new UIMenuItem("Set Gang Rep Random", "Sets the player reputation to each gang to a randomized number");
        RandomSingleGangRep = new UIMenuItem("Set Single Gang Rep Random", "Sets the player reputation to random gang to a randomized number");

        HostileGangRep = new UIMenuItem("Set Gang Rep Hostile", "Sets the player reputation to each gang to hostile");
        FriendlyGangRep = new UIMenuItem("Set Gang Rep Friendly", "Sets the player reputation to each gang to friendly");
        SetDateToToday = new UIMenuItem("Set Game Date Current", "Sets the game date the same as system date");



        Holder1 = new UIMenuItem("Placeholder", "Placeholder nullsub");



        Debug.AddItem(KillPlayer);
        Debug.AddItem(GetRandomWeapon);
        Debug.AddItem(GetRandomUpgradedWeapon);
        Debug.AddItem(GiveMoney);
        Debug.AddItem(SetMoney);
        Debug.AddItem(GetAllItems);
        Debug.AddItem(FillHealth);
        Debug.AddItem(FillHealthAndArmor);
        Debug.AddItem(FastForwardTime);
        Debug.AddItem(GoToReleaseSettings);
        Debug.AddItem(GoToHardCoreSettings);

        Debug.AddItem(ForceSober);

        Debug.AddItem(AutoSetRadioStation);
        Debug.AddItem(StartRandomCrime);
       // Debug.AddItem(TeleportToPOI);
        Debug.AddItem(DefaultGangRep);
        Debug.AddItem(RandomGangRep);
        Debug.AddItem(RandomSingleGangRep);

        Debug.AddItem(SetGangRepDefault);
        Debug.AddItem(SetGangRepFriendly);
        Debug.AddItem(SetGangRepHostile);


        //Debug.AddItem(HostileGangRep);
        //Debug.AddItem(FriendlyGangRep);

        Debug.AddItem(FreeCamMenu);
        Debug.AddItem(LogLocationMenu);
        Debug.AddItem(LogLocationSimpleMenu);
        Debug.AddItem(LogInteriorMenu);
        Debug.AddItem(LogCameraPositionMenu);
        Debug.AddItem(SetDateToToday);
        Debug.AddItem(LoadSPMap);
        Debug.AddItem(LoadMPMap);



        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To ScrapYard", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.ScrapYards));
        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To Hotel", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.Hotels));
        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To GunStores", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.GunStores));
        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To Gang Den", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.GangDens));
        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To Dead Drops", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.DeadDrops));
        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To Residence", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.Residences));
        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To City Hall", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.CityHalls));
        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To PoliceStation", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.PoliceStations));
        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To FireStation", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.FireStations));
        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To Hospital", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.Hospitals));
        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To Restaurant", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.Restaurants));
        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To Bank", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.Banks));
        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To BeautyShop", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.BeautyShops));
        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To Dispensary", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.Dispensaries));
        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To HardwardStore", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.HardwareStores));
        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To HeadShop", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.HeadShops));
        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To PawnShop", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.PawnShops));
        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To Pharmacy", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.Pharmacies));
        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To Stadium", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.Stadiums));
        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To ConvenienceStore", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.ConvenienceStores));
        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To LiquorStore", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.LiquorStores));
        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To GasStation", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.GasStations));
        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To Bar", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.Bars));
        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To FoodStand", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.FoodStands));
        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To CarDealership", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.CarDealerships));
        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To DriveThru", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.DriveThrus));
























        //foreach (LocationType lt in (LocationType[])Enum.GetValues(typeof(LocationType)))
        //{
        //    Debug.AddItem(new UIMenuListScrollerItem<GameLocation>($"Teleport To {lt}", "Teleports to A POI on the Map", PlacesOfInterest.GetLocations(lt)));
        //}



    }

    private void DispatcherMenuSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if(selectedItem == SpawnAgencyFoot)
        {
            EntryPoint.WriteToConsole($"SpawnAgencyFoot SELECTED {SpawnAgencyFoot.SelectedItem.ID}");

            if(SpawnAgencyFoot.SelectedItem.Classification == Classification.EMS)
            {
                Dispatcher.DebugSpawnEMT(SpawnAgencyFoot.SelectedItem.ID, true);
            }
            else 
            {

                Dispatcher.DebugSpawnCop(SpawnAgencyFoot.SelectedItem.ID, true);
            }
        }
        else if (selectedItem == SpawnAgencyVehicle)
        {
            EntryPoint.WriteToConsole($"SpawnAgencyVehicle SELECTED {SpawnAgencyVehicle.SelectedItem.ID}");
            if (SpawnAgencyVehicle.SelectedItem.Classification == Classification.EMS)
            {
                Dispatcher.DebugSpawnEMT(SpawnAgencyVehicle.SelectedItem.ID, false);
            }
            else
            {
                Dispatcher.DebugSpawnCop(SpawnAgencyVehicle.SelectedItem.ID, false);
            }
        }


        else if (selectedItem == SpawnGangFoot)
        {
            EntryPoint.WriteToConsole($"SpawnGangFoot SELECTED {SpawnGangFoot.SelectedItem.ID}");
            Dispatcher.DebugSpawnGang(SpawnGangFoot.SelectedItem.ID, true);
        }
        else if (selectedItem == SpawnGangVehicle)
        {
            EntryPoint.WriteToConsole($"SpawnGangVehicle SELECTED {SpawnGangVehicle.SelectedItem.ID}");
            Dispatcher.DebugSpawnGang(SpawnGangVehicle.SelectedItem.ID, false);
        }


        else if(selectedItem == SpawnRockblock)
        {
            Dispatcher.SpawnRoadblock();
        }


        sender.Visible = false;
    }

    private void DebugMenuSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == KillPlayer)
        {
            Game.LocalPlayer.Character.Kill();
        }
        else if (selectedItem == GoToReleaseSettings)
        {
            Settings.SetRelease();
            
        }
        else if (selectedItem == GoToHardCoreSettings)
        {
            Settings.SetHard();
        }
        else if (selectedItem == FastForwardTime)
        {
            Time.FastForward(FastForwardTime.Value);
        }
        else if (selectedItem == GetRandomWeapon)
        {
            WeaponInformation myGun = Weapons.GetRandomRegularWeapon((WeaponCategory)RandomWeaponCategory);
            if (myGun != null)
            {
                Game.LocalPlayer.Character.Inventory.GiveNewWeapon(myGun.ModelName, myGun.AmmoAmount, true);
            }
        }
        else if (selectedItem == GetRandomUpgradedWeapon)
        {
            WeaponInformation myGun = Weapons.GetRandomRegularWeapon((WeaponCategory)RandomUpgradedWeaponCategory);
            if (myGun != null)
            {
                Game.LocalPlayer.Character.Inventory.GiveNewWeapon(myGun.ModelName, myGun.AmmoAmount, true);
                WeaponComponent bestMagazineUpgrade = myGun.PossibleComponents.Where(x => x.ComponentSlot == ComponentSlot.Magazine).OrderBy(x=> x.Name == "Box Magazine" ? 1 : x.Name == "Drum Magazine" ? 2 : x.Name == "Extended Clip" ? 3: 4).FirstOrDefault();
                if (bestMagazineUpgrade != null)
                {
                    myGun.AddComponent(Game.LocalPlayer.Character, bestMagazineUpgrade);
                }
            }
        }
        //else if (selectedItem == TeleportToPOI)
        //{
        //    GameLocation ToTeleportTo = PlacesOfInterest.GetAllPlaces()[PlaceOfInterestSelected];
        //    if(ToTeleportTo != null)
        //    {
        //        Game.LocalPlayer.Character.Position = ToTeleportTo.EntrancePosition;
        //        Game.LocalPlayer.Character.Heading = ToTeleportTo.EntranceHeading;
        //    }
        //}
        else if (selectedItem == GiveMoney)
        {
            Player.GiveMoney(50000);
        }
        else if (selectedItem == SetMoney)
        {
            if (int.TryParse(NativeHelper.GetKeyboardInput(""), out int moneyToSet))
            {
                Player.SetMoney(moneyToSet);
            }
        }
        else if (selectedItem == FillHealth)
        {
            Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
        }
        else if (selectedItem == FillHealthAndArmor)
        {
            Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
            Game.LocalPlayer.Character.Armor = 100;
        }
        else if (selectedItem == StartRandomCrime)
        {
            Tasker.CreateCrime();
        }
        else if (selectedItem == ForceSober)
        {
            Player.Intoxication.Dispose();
        }
        else if (selectedItem == LogLocationMenu)
        {
            LogGameLocation();
        }
        else if (selectedItem == LogLocationSimpleMenu)
        {
            LogGameLocationSimple();
        }
        else if (selectedItem == LogCameraPositionMenu)
        {
            LogCameraPosition();
        }
        else if (selectedItem == LogInteriorMenu)
        {
            LogGameInterior();
        }
        else if (selectedItem == SetDateToToday)
        {
            Time.SetDateToToday();
        }
        else if (selectedItem == FreeCamMenu)
        {
            Frecam();
        }
        else if (selectedItem == Holder1)
        {

        }
        else if (selectedItem == LoadMPMap)
        {
            World.LoadMPMap();
        }
        else if (selectedItem == LoadSPMap)
        {
            World.LoadSPMap();
        }

        else if (selectedItem == GetAllItems)
        {

            foreach (ModItem modItem in ModItems.Items)
            {
                Player.Inventory.Add(modItem, 5);
            }
        }

        if(selectedItem == SetGangRepHostile)
        {
            Player.GangRelationships.SetReputation(SetGangRepHostile.SelectedItem, -5000, false);
        }
        if (selectedItem == SetGangRepDefault)
        {
            Player.GangRelationships.SetReputation(SetGangRepDefault.SelectedItem, 200, false);
        }
        if (selectedItem == SetGangRepFriendly)
        {
            Player.GangRelationships.SetReputation(SetGangRepFriendly.SelectedItem, 5000, false);
        }
        else if (selectedItem == RandomGangRep)
        {
            Player.GangRelationships.SetAllRandomReputations();
        }
        else if (selectedItem == RandomSingleGangRep)
        {
            Player.GangRelationships.SetSingleRandomReputation();
        }
        else if (selectedItem == DefaultGangRep)
        {
            Player.GangRelationships.ResetAllReputations();
        }
        else if (selectedItem == HostileGangRep)
        {
            Player.GangRelationships.SetHostileReputations();
        }
        else if (selectedItem == FriendlyGangRep)
        {
            Player.GangRelationships.SetFriendlyReputations();
        }

        //if (selectedItem.GetType() == typeof(UIMenuListScrollerItem<GameLocation>))
        //{
        //    UIMenuListScrollerItem<GameLocation> myItem = (UIMenuListScrollerItem<GameLocation>)selectedItem;
        //    if (myItem.SelectedItem != null)
        //    {
        //        Game.LocalPlayer.Character.Position = myItem.SelectedItem.EntrancePosition;
        //        Game.LocalPlayer.Character.Heading = myItem.SelectedItem.EntranceHeading;
        //    }
        //}
        if (selectedItem.GetType() == typeof(UIMenuListScrollerItem<BasicLocation>))
        {
            UIMenuListScrollerItem<BasicLocation> myItem = (UIMenuListScrollerItem<BasicLocation>)selectedItem;
            if (myItem.SelectedItem != null)
            {
                Game.LocalPlayer.Character.Position = myItem.SelectedItem.EntrancePosition;
                Game.LocalPlayer.Character.Heading = myItem.SelectedItem.EntranceHeading;
            }
        }

        Debug.Visible = false;
    }
    private void OnListChange(UIMenu sender, UIMenuListItem list, int index)
    {
        if (list == GetRandomWeapon)
        {
            RandomWeaponCategory = index;
        }
        if (list == AutoSetRadioStation)
        {
            Settings.SettingsManager.VehicleSettings.AutoTuneRadioStation = RadioStations.RadioStationList[index].InternalName;
        }
        if (list == TeleportToPOI)
        {
            PlaceOfInterestSelected = index;
        }
        if(list == GetRandomUpgradedWeapon)
        {
            RandomUpgradedWeaponCategory = index;
        }
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

}