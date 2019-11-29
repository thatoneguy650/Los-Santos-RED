using ExtensionsMethods;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

public static class InstantAction
{
    private static Random rnd;
    private static Police.PoliceState HandsUpPreviousPoliceState;
    private static bool PlayerIsGettingIntoVehicle;
    private static bool PrevPlayerInVehicle = false;
    private static bool PrevPlayerIsGettingIntoVehicle;
    private static bool IsRunning { get; set; } = true;
    public static bool isDead { get; set; } = false;
    public static bool isBusted { get; set; } = false;
    public static bool BeingArrested { get; set; } = false;
    public static bool DiedInVehicle { get; set; } = false;
    public static bool PlayerIsConsideredArmed { get; set; } = false;
    public static int TimesDied { get; set; } = 0;
    public static bool HandsAreUp { get; set; } = false;
    public static int MaxWantedLastLife { get; set; }
    public static WeaponHash LastWeapon { get; set; } = 0;
    public static bool PlayerInVehicle { get; set; } = false;
    public static int PlayerWantedLevel { get; set; } = 0;
    public static List<GTAVehicle> TrackedVehicles { get; set; } = new List<GTAVehicle>() ;
    public static Vehicle OwnedCar { get; set; } = null;
    public static List<Rage.Object> CreatedObjects { get; set; } = new List<Rage.Object>();
    public static bool IsHardToSeeInWeather
    {
        get
        {
            WeatherType TheWeather = World.Weather;
            if (TheWeather == WeatherType.Blizzard || TheWeather == WeatherType.Foggy || TheWeather == WeatherType.Rain || TheWeather == WeatherType.Snow || TheWeather == WeatherType.Snowlight || TheWeather == WeatherType.Thunder || TheWeather == WeatherType.Xmas)
                return true;
            else
                return false;
        }
    }
    public static bool IsCurrentVehicleTracked
    {
        get
        {
            if(Game.LocalPlayer.Character.IsInAnyVehicle(false))
            {
                PoolHandle Handle = Game.LocalPlayer.Character.CurrentVehicle.Handle;
                return TrackedVehicles.Any(x => x.VehicleEnt.Handle == Handle);
            }
            else
            {
                return false;
            }    
        }
    }
    static InstantAction()
    {
        rnd = new Random();
    }
    public static void Initialize()
    {
        while (Game.IsLoading)
            GameFiber.Yield();
        LoadInteriors();

        Police.Initialize();
        LicensePlateChanging.Initialize();
        Settings.Initialize();
        Menus.Intitialize();//Somewhat the procees each tick is taking frames
        RespawnStopper.Initialize(); //maye some slowness
        PoliceScanning.Initialize();
        DispatchAudio.Initialize();//slow? moved to 500 ms
        PoliceSpeech.Initialize();//slow? moved to 500 ms
        Vehicles.Initialize();
        VehicleEngine.Initialize();
        Smoking.Initialize();
        Tasking.Initialize();
        Agencies.Initialize();
        Locations.Initialize();
        GTAWeapons.Initialize();
        Speed.Initialize();
        WeaponDropping.Initialize();
        Streets.Initialize();
        UI.Initialize();
        Debugging.Initialize();
        PlayerLocation.Initialize();
        MainLoop();
    }
    public static void MainLoop()
    {
        Game.LocalPlayer.Character.CanBePulledOutOfVehicles = true;
        var stopwatch = new Stopwatch();
        GameFiber.StartNew(delegate
        {
            try
            {
                while (IsRunning)
                {
                    stopwatch.Start();
                    UpdatePlayer();
                    StateTick();
                    ControlTick();
                    AudioTick();
                    Police.Tick();
                    TrafficViolations.Tick();
                    stopwatch.Stop();
                    if (stopwatch.ElapsedMilliseconds >= 16)
                        Debugging.WriteToLog("InstantActionTick", string.Format("Tick took {0} ms", stopwatch.ElapsedMilliseconds));
                    stopwatch.Reset();
                    GameFiber.Yield();
                }
            }
            catch (Exception e)
            {
                Dispose();
                Debugging.WriteToLog("Error", e.Message + " : " + e.StackTrace);
            }
        });
    }
    public static void Dispose()
    {
        IsRunning = false;
        foreach (Blip myBlip in Police.CreatedBlips)
        {
            if (myBlip.Exists())
                myBlip.Delete();
        }
        LicensePlateChanging.Dispose();
        Settings.Dispose();
        Menus.Dispose();
        RespawnStopper.Dispose(); //maye some slowness
        PoliceScanning.Dispose();
        DispatchAudio.Dispose();
        PoliceSpeech.Dispose();
        Vehicles.Dispose();
        VehicleEngine.Dispose();
        Smoking.Dispose();
        Tasking.Dispose();
        Agencies.Dispose();
        Locations.Dispose();
        GTAWeapons.Dispose();
        Speed.Dispose();
        WeaponDropping.Dispose();
        Streets.Dispose();
        UI.Dispose();
        Debugging.Dispose();
        PlayerLocation.Dispose();
    }

    private static void UpdatePlayer()
    {
        PlayerInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
        PlayerIsGettingIntoVehicle = Game.LocalPlayer.Character.IsGettingIntoVehicle;
        PlayerWantedLevel = Game.LocalPlayer.WantedLevel;
        Police.PlayerStarsGreyedOut = NativeFunction.CallByName<bool>("ARE_PLAYER_STARS_GREYED_OUT", Game.LocalPlayer);
        PlayerIsConsideredArmed = Game.LocalPlayer.Character.isConsideredArmed();
        Police.PlayerIsJacking = Game.LocalPlayer.Character.IsJacking;

        if (PrevPlayerIsGettingIntoVehicle != PlayerIsGettingIntoVehicle)
            PlayerIsGettingIntoVehicleChanged();

        if (PlayerInVehicle && !IsCurrentVehicleTracked)
            TrackCurrentVehicle();

        Police.CheckRecognition();

        if (Game.LocalPlayer.Character.Inventory.EquippedWeapon != null && Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash != LastWeapon)
            LastWeapon = Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash;

        if (PrevPlayerInVehicle != PlayerInVehicle)
            PlayerInVehicleChanged(PlayerInVehicle);
    }
    private static void StateTick()
    {
        //Dead
        if (Game.LocalPlayer.Character.IsDead && !isDead)
            PlayerDeathEvent();

        // Busted
        if (NativeFunction.CallByName<bool>("IS_PLAYER_BEING_ARRESTED", 0))
            BeingArrested = true;
        if (NativeFunction.CallByName<bool>("IS_PLAYER_BEING_ARRESTED", 1))
        {
            BeingArrested = true;
            Game.LocalPlayer.Character.Tasks.Clear();
        }

        if (BeingArrested && !isBusted)
            PlayerBustedEvent();

        //if (PlayerWantedLevel > PreviousWantedLevel)
        //    PreviousWantedLevel = PlayerWantedLevel;

        if (PlayerWantedLevel > MaxWantedLastLife) // The max wanted level i saw in the last life, not just right before being busted
            MaxWantedLastLife = PlayerWantedLevel;

        if (PedSwapping.JustTakenOver(1000) && PlayerWantedLevel > 0)//Right when you takeover a ped they might become wanted for some weird reason, this stops that
        {
            Police.SetWantedLevel(0);
            Debugging.WriteToLog("StateTick", "Setting wanted to 0 after takeover");
        }
    }
    private static void TrackCurrentVehicle()
    {
        Vehicle CurrVehicle = Game.LocalPlayer.Character.CurrentVehicle;
        bool stolen = true;
        if (OwnedCar != null && OwnedCar.Handle == CurrVehicle.Handle)
            stolen = false;

        CurrVehicle.IsStolen = stolen;
        bool AmStealingCarFromPrerson = Police.PlayerIsJacking;
        Ped PreviousOwner;

        if (CurrVehicle.HasDriver && CurrVehicle.Driver.Handle != Game.LocalPlayer.Character.Handle)
            PreviousOwner = CurrVehicle.Driver;
        else
            PreviousOwner = CurrVehicle.GetPreviousPedOnSeat(-1);

        if (PreviousOwner != null && PreviousOwner.DistanceTo2D(Game.LocalPlayer.Character) <= 20f && PreviousOwner.Handle != Game.LocalPlayer.Character.Handle)
        {
            AmStealingCarFromPrerson = true;
        }
        GTALicensePlate MyPlate = new GTALicensePlate(CurrVehicle.LicensePlate, (uint)CurrVehicle.Handle, NativeFunction.CallByName<int>("GET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", CurrVehicle), false);
        TrackedVehicles.Add(new GTAVehicle(CurrVehicle, Game.GameTime, AmStealingCarFromPrerson, CurrVehicle.IsAlarmSounding, PreviousOwner, !stolen, stolen, MyPlate));
    }
    private static void PlayerBustedEvent()
    {
        DiedInVehicle = PlayerInVehicle; //Game.LocalPlayer.Character.IsInAnyVehicle(false);
        isBusted = true;
        BeingArrested = true;
        Game.LocalPlayer.Character.Tasks.Clear();
        NativeFunction.Natives.x2206BF9A37B7F724("DeathFailMPIn", 0, 0);//_START_SCREEN_EFFECT
        Game.TimeScale = 0.4f;
        HandsAreUp = false;
        Surrendering.SetArrestedAnimation(Game.LocalPlayer.Character, false);
        DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspectArrested, 5, false));
        GameFiber HandleBusted = GameFiber.StartNew(delegate
        {
            GameFiber.Wait(1500);
            Menus.ShowBustedMenu();
        }, "HandleBusted");
        Debugging.GameFibers.Add(HandleBusted);
    }
    private static void PlayerDeathEvent()
    {
        DiedInVehicle = PlayerInVehicle;//Game.LocalPlayer.Character.IsInAnyVehicle(false);
        isDead = true;
        NativeFunction.Natives.x2206BF9A37B7F724("DeathFailOut", 0, 0);//_START_SCREEN_EFFECT
        Game.LocalPlayer.Character.Kill();
        Game.LocalPlayer.Character.Health = 0;
        Game.LocalPlayer.Character.IsInvincible = true;
        Police.SetWantedLevel(0);
        Game.TimeScale = 0.4f;
        if (Police.PreviousWantedLevel > 0 || PoliceScanning.CopPeds.Any(x => x.isTasked))
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspectWasted, 5, false));
        GameFiber HandleDeath = GameFiber.StartNew(delegate
        {
            GameFiber.Wait(1500);
            Menus.ShowDeathMenu();
        }, "HandleDeath");
        Debugging.GameFibers.Add(HandleDeath);
    }
    private static void PlayerIsGettingIntoVehicleChanged()
    {
        if (PlayerIsGettingIntoVehicle)
        {
            Vehicle TargetVeh = Game.LocalPlayer.Character.VehicleTryingToEnter;
            int SeatTryingToEnter = Game.LocalPlayer.Character.SeatIndexTryingToEnter;
            CarStealing.LockCarDoor(TargetVeh);//Attempt to lock most car doors
            int LockStatus = (int)TargetVeh.LockStatus;//Get the result of the function
            if(LockStatus == 7)//Locked but can be broken into
            {
                CarStealing.UnlockCarDoor(TargetVeh, SeatTryingToEnter);
            }

            if (TargetVeh != null && SeatTryingToEnter == -1)
            {
                Ped Driver = TargetVeh.Driver;
                if (Driver != null && Driver.IsAlive)
                {
                    CarStealing.CarJackPedWithWeapon(TargetVeh,Driver, SeatTryingToEnter);
                    Debugging.WriteToLog("EnterVehicle", "CarJacking");
                }
                else
                {
                    Debugging.WriteToLog("EnterVehicle", "Regular Enter No Driver");
                }
            }
            else
            {
                Debugging.WriteToLog("EnterVehicle", "Regular Enter");
            }

        }
        PrevPlayerIsGettingIntoVehicle = PlayerIsGettingIntoVehicle;
    }
    private static void PlayerInVehicleChanged(bool playerInVehicle)
    {
        if (playerInVehicle)
        {
            GTAVehicle MyVehicle = GetPlayersCurrentTrackedVehicle();
            if (MyVehicle == null || MyVehicle.IsStolen)
                return;

            if(OwnedCar == null)
                MyVehicle.IsStolen = true;
            else if (MyVehicle.VehicleEnt.Handle != OwnedCar.Handle && !MyVehicle.IsStolen)
                MyVehicle.IsStolen = true;
        }
        else
        {

        }
        PlayerInVehicle = playerInVehicle;
        PrevPlayerInVehicle = playerInVehicle;

        Debugging.WriteToLog("ValueChecker", String.Format("playerInVehicle Changed to: {0}", playerInVehicle));
    }

    private static void ControlTick()
    {
        if (Game.IsKeyDownRightNow(Settings.SurrenderKey) && !Game.LocalPlayer.IsFreeAiming && (!Game.LocalPlayer.Character.IsInAnyVehicle(false) || Game.LocalPlayer.Character.CurrentVehicle.Speed < 2.5f))
        {
            if (!HandsAreUp && !isBusted)
            {
                SetPedUnarmed(Game.LocalPlayer.Character, false);
                HandsUpPreviousPoliceState = Police.CurrentPoliceState;
                HandsAreUp = true;
                Surrendering.RaiseHands();
                if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Speed <= 10f)
                    Game.LocalPlayer.Character.CurrentVehicle.IsDriveable = false;
            }
        }
        else
        {
            if (HandsAreUp && !isBusted)
            {
                HandsAreUp = false; // You put your hands down
                Police.CurrentPoliceState = HandsUpPreviousPoliceState;
                Game.LocalPlayer.Character.Tasks.Clear();
                if (Game.LocalPlayer.Character.IsInAnyVehicle(false))
                    Game.LocalPlayer.Character.CurrentVehicle.IsDriveable = true;
            }
        }

    }
    private static void AudioTick()
    {
        if (Settings.DisableAmbientScanner)
            NativeFunction.Natives.xB9EFD5C25018725A("PoliceScannerDisabled", true);
        if (Settings.WantedMusicDisable)
            NativeFunction.Natives.xB9EFD5C25018725A("WantedMusicDisabled", true);
    }


    public static GTAWeapon GetCurrentWeapon()
    {
        ulong myHash = (ulong)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash;
        GTAWeapon CurrentGun = GTAWeapons.GetWeaponFromHash(myHash);//Weapons.Where(x => (WeaponHash)x.Hash == MyWeapon.Hash).FirstOrDefault();
        if (CurrentGun != null)
            return CurrentGun;
        else
            return null;
    }
    public static void SetPlayerToLastWeapon()
    {
        if (Game.LocalPlayer.Character.Inventory.EquippedWeapon != null && LastWeapon != 0)
        {
            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)LastWeapon, true);
        }
    }
    public static GTAVehicle GetPlayersCurrentTrackedVehicle()
    {
        if (!Game.LocalPlayer.Character.IsInAnyVehicle(false))
            return null;
        else
        {
            Vehicle CurrVehicle = Game.LocalPlayer.Character.CurrentVehicle;
            return TrackedVehicles.Where(x => x.VehicleEnt.Handle == CurrVehicle.Handle).FirstOrDefault();
        }

    }
    public static void SetPedUnarmed(Ped Pedestrian, bool SetCantChange)
    {
        if (!(Pedestrian.Inventory.EquippedWeapon == null))
        {
            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Pedestrian, (uint)2725352035, true); //Unequip weapon so you don't get shot
            if (SetCantChange)
                NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Pedestrian, false);
        }
    }
    public static WeaponVariation GetWeaponVariation(Ped WeaponOwner, uint WeaponHash)
    {
        int Tint = NativeFunction.CallByName<int>("GET_PED_WEAPON_TINT_INDEX", WeaponOwner, WeaponHash);
        GTAWeapon MyGun = GTAWeapons.GetWeaponFromHash(WeaponHash);
        if (MyGun == null)
            return new WeaponVariation(Tint);

        List<WeaponVariation.WeaponComponent> Components = new List<WeaponVariation.WeaponComponent>();
        List<WeaponVariation.WeaponComponent> PossibleComponents = GTAWeapons.GetWeaponVariations(MyGun.Name);

        if (!Components.Any())
            return new WeaponVariation(Tint);

        foreach (WeaponVariation.WeaponComponent PossibleComponent in PossibleComponents)
        {
            if (NativeFunction.CallByName<bool>("HAS_PED_GOT_WEAPON_COMPONENT", WeaponOwner, WeaponHash, PossibleComponent.Hash))
            {
                Components.Add(new WeaponVariation.WeaponComponent(PossibleComponent.Name, PossibleComponent.HashKey, PossibleComponent.Hash, true));
            }

        }
        return new WeaponVariation(Tint, Components);

    }
    public static void ApplyWeaponVariation(Ped WeaponOwner, uint WeaponHash, WeaponVariation _WeaponVariation)
    {
        if (_WeaponVariation == null)
            return;
        NativeFunction.CallByName<bool>("SET_PED_WEAPON_TINT_INDEX", WeaponOwner, WeaponHash, _WeaponVariation.Tint);
        GTAWeapon LookupGun = GTAWeapons.GetWeaponFromHash(WeaponHash);//Weapons.Where(x => x.Hash == WeaponHash).FirstOrDefault();
        if (LookupGun == null)
            return;
        List<WeaponVariation.WeaponComponent> PossibleComponents = GTAWeapons.GetWeaponVariations(LookupGun.Name);//WeaponComponentsLookup.Where(x => x.BaseWeapon == LookupGun.Name).ToList();
        foreach (WeaponVariation.WeaponComponent ToRemove in PossibleComponents)
        {
            NativeFunction.CallByName<bool>("REMOVE_WEAPON_COMPONENT_FROM_PED", WeaponOwner, WeaponHash, ToRemove.Hash);
        }


        foreach (WeaponVariation.WeaponComponent ToAdd in _WeaponVariation.Components)
        {
            NativeFunction.CallByName<bool>("GIVE_WEAPON_COMPONENT_TO_PED", WeaponOwner, WeaponHash, ToAdd.Hash);
        }
    }
    public static void RequestAnimationDictionay(String sDict)
    {
        NativeFunction.CallByName<bool>("REQUEST_ANIM_DICT", sDict);
        while (!NativeFunction.CallByName<bool>("HAS_ANIM_DICT_LOADED", sDict))
            GameFiber.Yield();
    }
    public static Rage.Object AttachScrewdriverToPed(Ped Pedestrian)
    {
        Rage.Object Screwdriver = new Rage.Object("prop_tool_screwdvr01", Pedestrian.GetOffsetPositionUp(50f));
        CreatedObjects.Add(Screwdriver);
        int BoneIndexRightHand = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, 57005);
        Screwdriver.AttachTo(Pedestrian, BoneIndexRightHand, new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f));
        return Screwdriver;
    }
    public static void LoadInteriors()
    {
        //Pillbox hill hospital?
        NativeFunction.CallByName<bool>("REMOVE_IPL", "RC12B_Destroyed");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "RC12B_HospitalInterior");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "RC12B_Default");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "RC12B_Fixed");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "RC12B_Default");//state 1 normal

        //Lifeinvader
        NativeFunction.CallByName<bool>("REQUEST_IPL", "facelobby");  // lifeinvader
        NativeFunction.CallByName<bool>("REMOVE_IPL", "facelobbyfake");
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, -340230128, -1042.518f, -240.6915f, 38.11796f, true, 0.0f, 0.0f, -1.0f);//_DOOR_CONTROL

        //    FIB Lobby      
        NativeFunction.CallByName<bool>("REQUEST_IPL", "FIBlobby");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "FIBlobbyfake");
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, -1517873911, 106.3793f, -742.6982f, 46.51962f, false, 0.0f, 0.0f, 0.0f);
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, -90456267, 105.7607f, -746.646f, 46.18266f, false, 0.0f, 0.0f, 0.0f);

        //Paleto Sheriff Office
        NativeFunction.CallByName<bool>("DISABLE_INTERIOR", NativeFunction.CallByName<int>("GET_INTERIOR_AT_COORDS", -444.89068603515625f, 6013.5869140625f, 30.7164f), false);
        NativeFunction.CallByName<bool>("CAP_INTERIOR", NativeFunction.CallByName<int>("GET_INTERIOR_AT_COORDS", -444.89068603515625f, 6013.5869140625f, 30.7164f), false);
        NativeFunction.CallByName<bool>("REQUEST_IPL", "v_sheriff2");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "cs1_16_sheriff_cap");
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, -1501157055, -444.4985f, 6017.06f, 31.86633f, false, 0.0f, 0.0f, 0.0f);
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, -1501157055, -442.66f, 6015.222f, 31.86633f, false, 0.0f, 0.0f, 0.0f);

        //Sheriffs Office Sandy Shores
        NativeFunction.CallByName<bool>("DISABLE_INTERIOR", NativeFunction.CallByName<int>("GET_INTERIOR_AT_COORDS", 1854.2537841796875f, 3686.738525390625f, 33.2671012878418f), false);
        NativeFunction.CallByName<bool>("CAP_INTERIOR", NativeFunction.CallByName<bool>("GET_INTERIOR_AT_COORDS", 1854.2537841796875f, 3686.738525390625f, 33.2671012878418f), false);
        NativeFunction.CallByName<bool>("REQUEST_IPL", "v_sheriff");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "sheriff_cap");
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, -1765048490, 1855.685f, 3683.93f, 34.59282f, false, 0.0f, 0.0f, 0.0f);

        //    Tequila la       
        NativeFunction.CallByName<bool>("DISABLE_INTERIOR", NativeFunction.CallByName<bool>("GET_INTERIOR_AT_COORDS", -556.5089111328125f, 286.318115234375f, 81.1763f), false);
        NativeFunction.CallByName<bool>("CAP_INTERIOR", NativeFunction.CallByName<bool>("GET_INTERIOR_AT_COORDS", -556.5089111328125f, 286.318115234375f, 81.1763f), false);
        NativeFunction.CallByName<bool>("REQUEST_IPL", "v_rockclub");
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, 993120320, -565.1712f, 276.6259f, 83.28626f, false, 0.0f, 0.0f, 0.0f);// front door
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, 993120320, -561.2866f, 293.5044f, 87.77851f, false, 0.0f, 0.0f, 0.0f);// back door

    }

}