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
    //Player State
    public static bool isDead = false;
    public static bool isBusted = false;
    public static bool BeingArrested = false;
    private static bool DiedInVehicle = false;
    public static bool PlayerIsConsideredArmed = false;
    public static int TimesDied;
    public static bool areHandsUp = false;
    public static int MaxWantedLastLife;
    private static WeaponHash LastWeapon = 0;
    public static bool PlayerIsOffroad;

    public static bool PlayerInVehicle = false;
    public static int PlayerWantedLevel = 0;
    public static int PreviousWantedLevel;
    private static bool PlayerIsGettingIntoVehicle;
    private static uint GameTimeLastTriedCarJacking;
    private static Police.PoliceState HandsUpPreviousPoliceState;

    public static List<GTALicensePlate> SpareLicensePlates = new List<GTALicensePlate>();
    public static List<GTAVehicle> TrackedVehicles = new List<GTAVehicle>();
    public static Vehicle OwnedCar = null;

    public static Street PlayerCurrentStreet;
    public static Street PlayerCurrentCrossStreet;
    public static Zone PlayerCurrentZone;
    private static uint GameTimeUpdatedLocation;

    //Event Checkers
    private static bool PrevPlayerInVehicle = false;
    private static bool PrevPlayerIsGettingIntoVehicle;

    //Other
    private static List<long> FrameTimes = new List<long>();
    public static List<Rage.Object> CreatedObjects = new List<Rage.Object>();
    private static bool IsRunning { get; set; } = true;
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


    //Header Items
    static InstantAction()
    {
        rnd = new Random();
    }
    public static void Initialize()
    {

        Police.CopModel.LoadAndWait();
        Police.CopModel.LoadCollisionAndWait();
        Game.LocalPlayer.Character.CanBePulledOutOfVehicles = true;
        while (Game.IsLoading)
            GameFiber.Yield();
        SetupLicensePlates();

        Settings.Initialize();
        Menus.Intitialize();//SOmewhat the procees each tick is taking frames
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
        MainLoop();
    }
    public static void MainLoop()
    {
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
                        WriteToLog("InstantActionTick", string.Format("Tick took {0} ms", stopwatch.ElapsedMilliseconds));
                    stopwatch.Reset();
                    GameFiber.Yield();
                }
            }
            catch (Exception e)
            {
                Dispose();
                WriteToLog("Error", e.Message + " : " + e.StackTrace);
            }

        });

        GameFiber.StartNew(delegate
        {
            try
            {
                while (IsRunning)
                {
                    DebugLoop();
                    GameFiber.Yield();
                }
            }
            catch (Exception e)
            {
                Dispose();
                WriteToLog("Error", e.Message + " : " + e.StackTrace);
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
    }

    //Ticks
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

        if (PlayerInVehicle)
            TrackCurrentVehicle();

        Police.CheckRecognition();

        if (!Police.AnyPoliceSeenPlayerThisWanted && Police.AnyPoliceRecentlySeenPlayer)
            Police.AnyPoliceSeenPlayerThisWanted = true;

        if (Game.LocalPlayer.Character.Inventory.EquippedWeapon != null && Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash != LastWeapon)
            LastWeapon = Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash;

        if (PrevPlayerInVehicle != PlayerInVehicle)
            PlayerInVehicleChanged(PlayerInVehicle);

        if (Game.GameTime - GameTimeUpdatedLocation >= 500)
        {
            UpdateLocation();
            GameTimeUpdatedLocation = Game.GameTime;
        }

    }
    private static void StateTick()
    {
        //Dead
        if (Game.LocalPlayer.Character.IsDead && !isDead)
            HandleDeath();

        // Busted
        if (NativeFunction.CallByName<bool>("IS_PLAYER_BEING_ARRESTED", 0))
            BeingArrested = true;
        if (NativeFunction.CallByName<bool>("IS_PLAYER_BEING_ARRESTED", 1))
        {
            BeingArrested = true;
            Game.LocalPlayer.Character.Tasks.Clear();
        }

        if (BeingArrested && !isBusted)
            HandleBusted();

        if (PlayerWantedLevel > PreviousWantedLevel)
            PreviousWantedLevel = PlayerWantedLevel;

        if (PlayerWantedLevel > MaxWantedLastLife) // The max wanted level i saw in the last life, not just right before being busted
            MaxWantedLastLife = PlayerWantedLevel;

        if (PedSwapping.JustTakenOver(1000) && PlayerWantedLevel > 0)//Right when you takeover a ped they might become wanted for some weird reason, this stops that
        {
            Police.SetWantedLevel(0);
            WriteToLog("StateTick", "Setting wanted to 0 after takeover");
        }
    }
    private static void ControlTick()
    {
        if (Game.IsKeyDownRightNow(Settings.SurrenderKey) && !Game.LocalPlayer.IsFreeAiming && (!Game.LocalPlayer.Character.IsInAnyVehicle(false) || Game.LocalPlayer.Character.CurrentVehicle.Speed < 2.5f))
        {
            if (!areHandsUp && !isBusted)
            {
                SetPedUnarmed(Game.LocalPlayer.Character, false);
                HandsUpPreviousPoliceState = Police.CurrentPoliceState;
                areHandsUp = true;
                RaiseHands();
                if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Speed <= 10f)
                    Game.LocalPlayer.Character.CurrentVehicle.IsDriveable = false;
            }
        }
        else
        {
            if (areHandsUp && !isBusted)
            {
                areHandsUp = false; // You put your hands down
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
    private static void TrackCurrentVehicle()
    {
        Vehicle CurrVehicle = Game.LocalPlayer.Character.CurrentVehicle;
        if (!TrackedVehicles.Any(x => x.VehicleEnt.Handle == CurrVehicle.Handle)) //Not Tracking Current Vehicle
        {
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
    }

    private static void HandleBusted()
    {
        DiedInVehicle = PlayerInVehicle; //Game.LocalPlayer.Character.IsInAnyVehicle(false);
        isBusted = true;
        BeingArrested = true;
        Game.LocalPlayer.Character.Tasks.Clear();
        NativeFunction.Natives.x2206BF9A37B7F724("DeathFailOut", 0, 0);//_START_SCREEN_EFFECT
        Game.TimeScale = 0.4f;
        areHandsUp = false;
        Menus.bustedMenu.Visible = true;
        SetArrestedAnimation(Game.LocalPlayer.Character, false);
        DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspectArrested, 5, false));
    }
    private static void HandleDeath()
    {
        DiedInVehicle = PlayerInVehicle;//Game.LocalPlayer.Character.IsInAnyVehicle(false);
        isDead = true;
        NativeFunction.Natives.x2206BF9A37B7F724("DeathFailOut", 0, 0);//_START_SCREEN_EFFECT
        Game.LocalPlayer.Character.Kill();
        Game.LocalPlayer.Character.Health = 0;
        Game.LocalPlayer.Character.IsInvincible = true;

        Police.SetWantedLevel(0);
        Game.TimeScale = 0.4f;
        Menus.deathMenu.Visible = true;

        if (Police.PrevWantedLevel > 0 || PoliceScanning.CopPeds.Any(x => x.isTasked))
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspectWasted, 5, false));
    }

    public static string GetCurrentStreet(Vector3 Position)
    {
        int StreetHash = 0;
        int CrossingHash = 0;
        unsafe
        {
            NativeFunction.CallByName<uint>("GET_STREET_NAME_AT_COORD", Position.X, Position.Y, Position.Z, &StreetHash, &CrossingHash);
        }
        string StreetName = string.Empty;
        string CrossStreetName = string.Empty;
        if (StreetHash != 0)
        {
            unsafe
            {
                IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_STREET_NAME_FROM_HASH_KEY", StreetHash);

                StreetName = Marshal.PtrToStringAnsi(ptr);
            }
        }
        return StreetName;
    }
    public static float GetSpeedLimit(Vector3 Position)
    {
        string StreetName = GetCurrentStreet(Position);
        Street MyStreet = Streets.GetStreetFromName(StreetName);
        if (MyStreet == null)
            return 50f;
        else
            return MyStreet.SpeedLimit;
    }
    private static float GetSpeedLimit(string StreetName)
    {
        Street MyStreet = Streets.GetStreetFromName(StreetName);
        if (MyStreet == null)
            return 50f;
        else
            return MyStreet.SpeedLimit;
    }
    private static void UpdateLocation()
    {
        PlayerCurrentZone = Zones.GetZoneName(Game.LocalPlayer.Character.Position);
        if (World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position).DistanceTo2D(Game.LocalPlayer.Character) >= 25f)
        {
            PlayerIsOffroad = true;
        }
        else
        {
            PlayerIsOffroad = false;
        }
        if(PlayerIsOffroad)
        {
            PlayerCurrentStreet = null;
            PlayerCurrentCrossStreet = null;
            //PlayerCurrentStreetSpeedLimit = 200f;
            //PlayerCurrentCrossStreetSpeedLimit = 200f;
            //PlayerCurrentZone = null;
            return;
        }

        Vector3 PlayerPos = Game.LocalPlayer.Character.Position;
        int StreetHash = 0;
        int CrossingHash = 0;
        string PlayerCurrentStreetName;
        string PlayerCurrentCrossStreetName;
        unsafe
        {
            NativeFunction.CallByName<uint>("GET_STREET_NAME_AT_COORD", PlayerPos.X, PlayerPos.Y, PlayerPos.Z, &StreetHash, &CrossingHash);
        }
        string StreetName = string.Empty;
        if (StreetHash != 0)
        {
            unsafe
            {
                IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_STREET_NAME_FROM_HASH_KEY", StreetHash);
                StreetName = Marshal.PtrToStringAnsi(ptr);
            }
            PlayerCurrentStreetName = StreetName;
        }
        else
            PlayerCurrentStreetName = "";

        string CrossStreetName = string.Empty;
        if (CrossingHash != 0)
        {
            unsafe
            {
                IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_STREET_NAME_FROM_HASH_KEY", CrossingHash);
                CrossStreetName = Marshal.PtrToStringAnsi(ptr);
            }
            PlayerCurrentCrossStreetName = CrossStreetName;
        }
        else
            PlayerCurrentCrossStreetName = "";


        PlayerCurrentStreet = Streets.GetStreetFromName(PlayerCurrentStreetName);
        PlayerCurrentCrossStreet = Streets.GetStreetFromName(PlayerCurrentCrossStreetName);

    }

    //Player Events
    private static void PlayerIsGettingIntoVehicleChanged()
    {
        if (PlayerIsGettingIntoVehicle)
        {
            Vehicle TargetVeh = Game.LocalPlayer.Character.VehicleTryingToEnter;
            int SeatTryingToEnter = Game.LocalPlayer.Character.SeatIndexTryingToEnter;


            LockCarDoor(TargetVeh);

            int LockStatus = (int)TargetVeh.LockStatus;


            if(LockStatus == 7)
            {
                UnlockCarDoor(TargetVeh, SeatTryingToEnter);
            }

            if (TargetVeh != null && SeatTryingToEnter == -1)
            {
                Ped Driver = TargetVeh.Driver;
                if (Driver != null && Driver.IsAlive)
                {
                    //GivePlayerLastWeaponIfUnarmed();
                    CarJackPedWithWeapon(TargetVeh,Driver, SeatTryingToEnter);
                    WriteToLog("EnterVehicle", "CarJacking");
                }
                else
                {
                    WriteToLog("EnterVehicle", "Regular Enter No Driver");
                }
            }
            else
            {
                WriteToLog("EnterVehicle", "Regular Enter");
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

        WriteToLog("ValueChecker", String.Format("playerInVehicle Changed to: {0}", playerInVehicle));
    }

    //Surrendering
    private static void RaiseHands()
    {
        if (Game.LocalPlayer.WantedLevel > 0 && Police.CopsKilledByPlayer < 5)
            Police.CurrentPoliceState = Police.PoliceState.ArrestedWait;



        bool inVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
        var sDict = (inVehicle) ? "veh@busted_std" : "ped";
        RequestAnimationDictionay(sDict);
        if (inVehicle)
        {
            //NativeFunction.CallByName<bool>("ROLL_DOWN_WINDOW", Game.LocalPlayer.Character.CurrentVehicle, 0);
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, sDict, "stay_in_car_crim", 2.0f, -2.0f, -1, 50, 0, true, false, true);
            //GameFiber.Sleep(250);
            //NativeFunction.CallByName<bool>("SET_ENTITY_ANIM_CURRENT_TIME", Game.LocalPlayer.Character, sDict, "stay_in_car_crim", 0.5f);
        }
        else
        {
            //if (!DroppingWeapon && Game.LocalPlayer.Character.isConsideredArmed())
            //    DropWeapon();

            //while (DroppingWeapon)
            //    GameFiber.Sleep(100);

            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, sDict, "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);
        }

    }
    public static void SetArrestedAnimation(Ped PedToArrest, bool MarkAsNoLongerNeeded)
    {
        GameFiber.StartNew(delegate
        {
            RequestAnimationDictionay("veh@busted_std");
            RequestAnimationDictionay("busted");
            RequestAnimationDictionay("ped");

            if (!PedToArrest.Exists())
                return;

            while (PedToArrest.IsRagdoll || PedToArrest.IsStunned)
                GameFiber.Yield();

            if (!PedToArrest.Exists())
                return;

            if (PedToArrest.IsInAnyVehicle(false))
            {
                Vehicle oldVehicle = PedToArrest.CurrentVehicle;
                //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "veh@busted_std", "get_out_car_crim", 2.0f, -2.0f, 2500, 50, 0, false, false, false);
                //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "veh@busted_std", "get_out_car_crim", 8.0f, -8.0f, -1, 50, 0, false, false, false);

                //GameFiber.Sleep(6000);
                if (PedToArrest.Exists() && oldVehicle.Exists())
                {
                    WriteToLog("SetArrestedAnimation", "Tasked to leave the vehicle");
                    NativeFunction.CallByName<uint>("TASK_LEAVE_VEHICLE", PedToArrest, oldVehicle, 256);
                    GameFiber.Sleep(2500);
                }
            }
            if (PedToArrest == Game.LocalPlayer.Character && !isBusted)
                return;


            if (MaxWantedLastLife < 3)
            {
                NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "ped", "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);
            }
            else
            {
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "busted", "idle_2_hands_up", 2.0f, -8.0f, 5000, 2, 0, false, false, false);
                GameFiber.Sleep(5000);
                if (!PedToArrest.Exists() || (PedToArrest == Game.LocalPlayer.Character && !isBusted))
                    return;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "busted", "idle_a", 8.0f, -8.0f, -1, 1, 0, false, false, false);
            }
            PedToArrest.KeepTasks = true;

            if (MarkAsNoLongerNeeded)
                PedToArrest.IsPersistent = false;
        });

    }
    private static void UnSetArrestedAnimation(Ped PedToArrest)
    {
        GameFiber.StartNew(delegate
        {
            RequestAnimationDictionay("random@arrests");
            RequestAnimationDictionay("busted");
            RequestAnimationDictionay("ped");

            if (NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_a", 1) || NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_2_hands_up", 1))
            {
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "random@arrests", "kneeling_arrest_escape", 8.0f, -8.0f, -1, 120, 0, 0, 1, 0);//"random@arrests", "kneeling_arrest_escape", 8.0f, -8.0f, -1, 4096, 0, 0, 1, 0);
            }
            else if (NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "ped", "handsup_enter", 1))
            {
                PedToArrest.Tasks.Clear();
            }
        });
    }

    //Weapon
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

    //Suicide
    public static void CommitSuicide(Ped PedToSuicide)
    {
        GameFiber.StartNew(delegate
        {
            if (!PedToSuicide.IsInAnyVehicle(false))
            {
                RequestAnimationDictionay("mp_suicide");

                GTAWeapon CurrentGun = null;
                if (PedToSuicide.Inventory.EquippedWeapon != null)
                    CurrentGun = GTAWeapons.WeaponsList.Where(x => (WeaponHash)x.Hash == PedToSuicide.Inventory.EquippedWeapon.Hash && x.CanPistolSuicide).FirstOrDefault();

                if (CurrentGun != null)
                {
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToSuicide, "mp_suicide", "pistol", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                    GameFiber.Wait(750);
                    Vector3 HeadCoordinated = PedToSuicide.GetBonePosition(PedBoneId.Head);
                    NativeFunction.CallByName<bool>("SET_PED_SHOOTS_AT_COORD", PedToSuicide, HeadCoordinated.X, HeadCoordinated.Y, HeadCoordinated.Z, true);
                }
                else
                {
                    NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", PedToSuicide, (uint)2725352035, true);
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToSuicide, "mp_suicide", "pill", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                    GameFiber.Wait(6000);
                }
            }
            PedToSuicide.Kill();
        });
    }

    //Car Stealing
    private static void UnlockCarDoor(Vehicle ToEnter, int SeatTryingToEnter)
    {
        if (!Game.IsControlPressed(2, GameControl.Enter))//holding enter go thru normal
            return;

        if (ToEnter.Exists() && (ToEnter.IsBike || ToEnter.IsBoat || ToEnter.IsHelicopter || ToEnter.IsPlane || ToEnter.IsBicycle))
            return;

        try
        {
            GameFiber.StartNew(delegate
            {

                SetPedUnarmed(Game.LocalPlayer.Character, false);

                bool Continue = true;
                ToEnter.MustBeHotwired = true;

                Vector3 GameEntryPosition = GetHandlePosition(ToEnter);
                if (GameEntryPosition == Vector3.Zero)
                    return;
                string AnimationName = "std_force_entry_ds";
                int DoorIndex = 0;
                int WaitTime = 1750;

                if (ToEnter.HasBone("door_dside_f") && ToEnter.HasBone("door_pside_f"))
                {
                    if(Game.LocalPlayer.Character.DistanceTo2D(ToEnter.GetBonePosition("door_dside_f")) > Game.LocalPlayer.Character.DistanceTo2D(ToEnter.GetBonePosition("door_pside_f")))
                    {
                        AnimationName = "std_force_entry_ps";
                        DoorIndex = 1;
                        WaitTime = 2200;
                    }
                    else
                    {
                        AnimationName = "std_force_entry_ds";
                        DoorIndex = 0;
                        WaitTime = 1750;
                    }
                }

                WriteToLog("UnlockCarDoor", string.Format("DoorIndex: {0},AnimationName: {1}", DoorIndex, AnimationName));
                Rage.Object Screwdriver = AttachScrewdriverToPed(Game.LocalPlayer.Character);
                RequestAnimationDictionay("veh@break_in@0h@p_m_one@");
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "veh@break_in@0h@p_m_one@", AnimationName, 2.0f, -2.0f, -1, 0, 0, false, false, false);

                Police.PlayerBreakingIntoCar = true;

                uint GameTimeStarted = Game.GameTime;
                while (Game.GameTime - GameTimeStarted <= WaitTime)
                {
                    GameFiber.Yield();
                    if (Extensions.IsMoveControlPressed())
                    {
                        Continue = false;
                        break;
                    }
                }

                if (!Continue)
                {
                    Game.LocalPlayer.Character.Tasks.Clear();
                    Screwdriver.Delete();
                    Police.PlayerBreakingIntoCar = false;
                    return;
                }

                ToEnter.LockStatus = VehicleLockStatus.Unlocked;
                ToEnter.Doors[DoorIndex].Open(true, false);

                //GameFiber.Sleep(500);

                WriteToLog("UnlockCarDoor", string.Format("Open Door: {0}", DoorIndex));
                GameTimeStarted = Game.GameTime;
                Game.LocalPlayer.Character.Tasks.EnterVehicle(ToEnter, SeatTryingToEnter);
                while (!Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.GameTime - GameTimeStarted <= 10000)
                {
                    GameFiber.Yield();
                    if (Extensions.IsMoveControlPressed())
                    {
                        Continue = false;
                        break;
                    }
                }
                if (ToEnter.Doors[DoorIndex].IsValid())
                    NativeFunction.CallByName<bool>("SET_VEHICLE_DOOR_CONTROL", ToEnter, DoorIndex, 4, 0f);

                GameFiber.Sleep(5000);
                Screwdriver.Delete();
                Police.PlayerBreakingIntoCar = false;
                WriteToLog("UnlockCarDoor", string.Format("Made it to the end: {0}", SeatTryingToEnter));
            });
        }
        catch (Exception e)
        {
            foreach (Rage.Object obj in CreatedObjects.Where(x => x.Exists()))
                obj.Delete();
            CreatedObjects.Clear();
            Police.PlayerBreakingIntoCar = false;
            WriteToLog("UnlockCarDoor", e.Message);
        }


    }
    private static void LockCarDoor(Vehicle ToLock)
    {
        WriteToLog("LockCarDoor", string.Format("Go To Start, Lock Status {0}", ToLock.LockStatus));
        if (ToLock.LockStatus != (VehicleLockStatus)1) //unlocked
            return;
        WriteToLog("LockCarDoor", "1");
        if (ToLock.HasDriver)//If they have a driver 
            return;
        WriteToLog("LockCarDoor", "2");
        foreach (VehicleDoor myDoor in ToLock.GetDoors())
        {
            if (!myDoor.IsValid() || myDoor.IsOpen)
                return;//invalid doors make the car not locked
        }
        WriteToLog("LockCarDoor", "3");
        if (!NativeFunction.CallByName<bool>("ARE_ALL_VEHICLE_WINDOWS_INTACT", ToLock))
            return;//broken windows == not locked
        WriteToLog("LockCarDoor", "4");
        if (TrackedVehicles.Any(x => x.VehicleEnt.Handle == ToLock.Handle))
            return; //previously entered vehicle arent locked
        if (ToLock.IsConvertible && ToLock.ConvertibleRoofState == VehicleConvertibleRoofState.Lowered)
            return;
        if (ToLock.IsBike || ToLock.IsPlane || ToLock.IsHelicopter)
            return;

        WriteToLog("LockCarDoor", "Locked");
        ToLock.LockStatus = (VehicleLockStatus)7;
    }

    //Car Jacking
    public static void CarJackPedWithWeapon(Vehicle TargetVehicle, Ped Driver, int SeatTryingToEnter)
    {
        if (!Game.IsControlPressed(2, GameControl.Enter))//holding enter go thru normal
            return;
        if (Game.GameTime - GameTimeLastTriedCarJacking <= 5000)
            return;
        try
        {
            if (SeatTryingToEnter != -1)
                return;

            GTAWeapon myGun = GetCurrentWeapon();
            if (myGun == null)
                return;

            GameFiber.StartNew(delegate
            {
                SetPlayerToLastWeapon();
                NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", Driver, TargetVehicle, 27, -1);
                Driver.BlockPermanentEvents = true;

                Vector3 GameEntryPosition = GetHandlePosition(TargetVehicle);
                float DesiredHeading = TargetVehicle.Heading - 90f;


                string dict = "";
                string PerpAnim = "";
                string VictimAnim = "";
                int BoneIndexSpine = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Driver, 57597);//11816
                Vector3 DriverSeatCoordinates = NativeFunction.CallByName<Vector3>("GET_PED_BONE_COORDS", Driver, BoneIndexSpine, 0f, 0f, 0f);

                GameTimeLastTriedCarJacking = Game.GameTime;

                if (!GetCarjackingAnimations(TargetVehicle, DriverSeatCoordinates, myGun, ref dict, ref PerpAnim, ref VictimAnim))//couldnt find animations
                {
                    Game.LocalPlayer.Character.Tasks.ClearImmediately();
                    GameFiber.Sleep(200);
                    Game.LocalPlayer.Character.Tasks.EnterVehicle(TargetVehicle, SeatTryingToEnter);
                    return;
                }

                //if (!MovePedToCarPosition(TargetVehicle, Game.LocalPlayer.Character, DesiredHeading, GameEntryPosition, true))
                //{
                //    Game.LocalPlayer.Character.Tasks.Clear();
                //    return;
                //}


                RequestAnimationDictionay(dict);

                float DriverHeading = Driver.Heading;
                int Scene1 = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", GameEntryPosition.X, GameEntryPosition.Y, Game.LocalPlayer.Character.Position.Z, 0.0f, 0.0f, DesiredHeading, 2);//270f //old
                NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", Scene1, false);
                NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", Game.LocalPlayer.Character, Scene1, dict, PerpAnim, 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);//std_perp_ds_a
                NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", Scene1, 0.0f);

                int Scene2 = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", DriverSeatCoordinates.X, DriverSeatCoordinates.Y, DriverSeatCoordinates.Z, 0.0f, 0.0f, DriverHeading, 2);//270f
                NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", Scene2, false);
                NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", Driver, Scene2, dict, VictimAnim, 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);
                NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", Scene2, 0.0f);

                Police.PlayerBreakingIntoCar = true;
                bool locOpenDoor = false;
                bool Cancel = false;
                Vector3 OriginalCarPosition = TargetVehicle.Position;

                while (NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", Scene1) < 0.75f)
                {
                    float ScenePhase = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", Scene1);
                    float Scene2Phase = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", Scene2);
                    GameFiber.Yield();
                    if (ScenePhase <= 0.4f && Extensions.IsMoveControlPressed())
                    {
                        Cancel = true;
                        break;
                    }

                    if (!locOpenDoor && ScenePhase > 0.05f && TargetVehicle.Doors[0].IsValid() && !TargetVehicle.Doors[0].IsFullyOpen)
                    {
                        locOpenDoor = true;
                        TargetVehicle.Doors[0].Open(false, false);
                    }
                    if(TargetVehicle.DistanceTo2D(OriginalCarPosition) >= 0.1f)
                    {
                        Cancel = true;
                        break;
                    }
                    if (Game.LocalPlayer.Character.isConsideredArmed() && Game.IsControlPressed(2, GameControl.Attack))
                    {
                        Vector3 TargetCoordinate = Driver.GetBonePosition(PedBoneId.Head);
                        NativeFunction.CallByName<bool>("SET_PED_SHOOTS_AT_COORD", Game.LocalPlayer.Character, TargetCoordinate.X, TargetCoordinate.Y, TargetCoordinate.Z, true);
                        Police.PlayerArtificiallyShooting = true;

                        if (ScenePhase <= 0.35f)
                        {
                            Driver.WarpIntoVehicle(TargetVehicle, -1);
                            Game.LocalPlayer.Character.Tasks.Clear();
                            NativeFunction.CallByName<bool>("SET_PLAYER_FORCED_AIM", Game.LocalPlayer.Character, true);
                            break;
                        }
                    }
                    if (Game.LocalPlayer.Character.isConsideredArmed() && Game.IsControlJustPressed(2, GameControl.Aim))
                    {
                        if (NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", Scene1) <= 0.4f)
                        {
                            Driver.WarpIntoVehicle(TargetVehicle, -1);
                            Game.LocalPlayer.Character.Tasks.Clear();
                            NativeFunction.CallByName<bool>("SET_PLAYER_FORCED_AIM", Game.LocalPlayer.Character, true);
                            break;
                        }
                    }
                }

                Police.PlayerArtificiallyShooting = false;

                float FinalScenePhase = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", Scene1);

                if (FinalScenePhase <= 0.4f)
                {
                    if (Cancel || Driver.IsDead)
                    {
                        Driver.BlockPermanentEvents = false;
                        Driver.WarpIntoVehicle(TargetVehicle, -1);
                        Game.LocalPlayer.Character.Tasks.Clear();
                    }
                }
                else
                {
                    if (Cancel)
                    {
                        Driver.BlockPermanentEvents = false;
                        Driver.WarpIntoVehicle(TargetVehicle, -1);
                        Game.LocalPlayer.Character.Tasks.Clear();
                    }
                    else
                        Game.LocalPlayer.Character.WarpIntoVehicle(TargetVehicle, -1);
                }

                if (Cancel)
                {
                    Police.PlayerBreakingIntoCar = false;
                    return;
                }
                    

                if (TargetVehicle.Doors[0].IsValid())
                    NativeFunction.CallByName<bool>("SET_VEHICLE_DOOR_CONTROL", TargetVehicle, 0, 4, 0f);

                WriteToLog("CarJackPedWithWeapon", string.Format("Scene1 Phase: {0}", FinalScenePhase));

                if (Driver.IsInAnyVehicle(false))
                {
                    WriteToLog("CarjackAnimation", "Driver In Vehicle");
                    //return; 
                }
                else
                {
                    WriteToLog("CarjackAnimation", "Driver Out of Vehicle");
                    // GameFiber.Sleep(1000);




                    //float? GroundZ = World.GetGroundZ(Driver.Position, true, false);
                    //if (GroundZ != null && Driver.Position.Z-1.0f <= GroundZ)
                    //    Driver.Position = new Vector3(Driver.Position.X, Driver.Position.Y, (float)GroundZ + 2f);
                    Driver.Tasks.ClearImmediately();

                    Driver.IsRagdoll = false;
                    Driver.BlockPermanentEvents = false;

                    if (rnd.Next(1, 11) >= 11)
                    {
                        GiveGunAndAttackPlayer(Driver);
                    }
                    else
                    {
                        Driver.Tasks.Flee(Game.LocalPlayer.Character, 100f, 30000);
                    }

                }
                GameFiber.Sleep(5000);

                Police.PlayerBreakingIntoCar = false;
            });
        }
        catch (Exception e)
        {
            foreach (Rage.Object obj in CreatedObjects.Where(x => x.Exists()))
                obj.Delete();
            CreatedObjects.Clear();
            Police.PlayerBreakingIntoCar = false;
            WriteToLog("UnlockCarDoor", e.Message);
        }
    }
    public static bool GetCarjackingAnimations(Vehicle TargetVehicle,Vector3 DriverSeatCoordinates, GTAWeapon MyGun, ref string Dictionary,ref string PerpAnimation,ref string VictimAnimation)
    {
        if (MyGun == null || (!MyGun.IsTwoHanded && !MyGun.IsOneHanded))
            return false;

        int intVehicleClass = NativeFunction.CallByName<int>("GET_VEHICLE_CLASS", TargetVehicle);
        Vehicles.VehicleClass VehicleClass = (Vehicles.VehicleClass)intVehicleClass;


        if (!TargetVehicle.Doors[0].IsValid())
            return false;

        float? GroundZ = World.GetGroundZ(DriverSeatCoordinates, true, false);
        if (GroundZ == null)
            GroundZ = 0f;
        float DriverDistanceToGround = DriverSeatCoordinates.Z - (float)GroundZ;
        WriteToLog("GetCarjackingAnimations", string.Format("VehicleClass {0},DriverSeatCoordinates: {1},GroundZ: {2}, PedHeight: {3}", VehicleClass, DriverSeatCoordinates, GroundZ, DriverDistanceToGround));
        if (VehicleClass == Vehicles.VehicleClass.Vans)
        {
            if (MyGun.IsTwoHanded)
            {
                Dictionary = "veh@jacking@2h";
                PerpAnimation = "van_perp_ds_a";
                VictimAnimation = "van_victim_ds_a";
            }
            else if (MyGun.IsOneHanded)
            {
                Dictionary = "veh@jacking@1h";
                PerpAnimation = "van_perp_ds_a";
                VictimAnimation = "van_victim_ds_a";
            }
        }
        else if (VehicleClass == Vehicles.VehicleClass.Helicopters)
        {
            if (MyGun.IsTwoHanded)
            {
                Dictionary = "veh@jacking@2h";
                PerpAnimation = "heli_perp_ds_a";
                VictimAnimation = "heli_victim_ds_a";
            }
            else if (MyGun.IsOneHanded)
            {
                Dictionary = "veh@jacking@1h";
                PerpAnimation = "heli_perp_ds_a";
                VictimAnimation = "heli_victim_ds_a";
            }
        }
        else if (VehicleClass == Vehicles.VehicleClass.Commercial)
        {
            if (MyGun.IsTwoHanded)
            {
                Dictionary = "veh@jacking@2h";
                PerpAnimation = "truck_perp_ds_a";
                VictimAnimation = "truck_victim_ds_a";
            }
            else if (MyGun.IsOneHanded)
            {
                Dictionary = "veh@jacking@1h";
                PerpAnimation = "truck_perp_ds_a";
                VictimAnimation = "truck_victim_ds_a";
            }
        }
        else if (DriverDistanceToGround > 1.75f)
        {
            if (MyGun.IsTwoHanded)
            {
                Dictionary = "veh@jacking@2h";
                PerpAnimation = "truck_perp_ds_a";
                VictimAnimation = "truck_victim_ds_a";
            }
            else if (MyGun.IsOneHanded)
            {
                Dictionary = "veh@jacking@1h";
                PerpAnimation = "truck_perp_ds_a";
                VictimAnimation = "truck_victim_ds_a";
            }
        }
        else if (DriverDistanceToGround <0.5f)
        {
            if (MyGun.IsTwoHanded)
            {
                Dictionary = "veh@jacking@2h";
                PerpAnimation = "low_perp_ds_a";
                VictimAnimation = "low_victim_ds_a";
            }
            else if (MyGun.IsOneHanded)
            {
                Dictionary = "veh@jacking@1h";
                PerpAnimation = "low_perp_ds_a";
                VictimAnimation = "low_victim_ds_a";
            }
        }
        else if(VehicleClass == Vehicles.VehicleClass.Motorcycles)
        {
            return false;
        }
        else
        {
            if (MyGun.IsTwoHanded)
            {
                Dictionary = "veh@jacking@2h";
                PerpAnimation = "std_perp_ds_a";
                VictimAnimation = "std_victim_ds_a";
            }
            else if (MyGun.IsOneHanded)
            {
                Dictionary = "veh@jacking@1h";
                PerpAnimation = "std_perp_ds";
                VictimAnimation = "std_victim_ds";
            }
        }
        return true;
    }

    //License Plate Changing
    public static void RemoveNearestLicensePlate()
    {
        Vehicle[] NearbyVehicles = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 10f, GetEntitiesFlags.ConsiderAllVehicles).Where(x => x is Vehicle).ToArray(), (x => (Vehicle)x));
        Vehicle ClosestVehicle = NearbyVehicles.Where(x => x.LicensePlate != "        ").OrderBy(x => GetLicensePlateChangePosition(x).DistanceTo2D(Game.LocalPlayer.Character.Position)).FirstOrDefault();

        if (ClosestVehicle != null)
        {
            GTAVehicle VehicleToChange = TrackedVehicles.Where(x => x.VehicleEnt.Handle == ClosestVehicle.Handle).FirstOrDefault();
            if (VehicleToChange == null)
            {
                VehicleToChange = new GTAVehicle(ClosestVehicle, false, false, new GTALicensePlate(ClosestVehicle.LicensePlate, (uint)ClosestVehicle.Handle, NativeFunction.CallByName<int>("GET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", ClosestVehicle), false));
                TrackedVehicles.Add(VehicleToChange);
            }

            if (ClosestVehicle.HasDriver)
            {
                PedReactToThreatening(ClosestVehicle.Driver);
            }
            ChangeLicensePlateAnimation(VehicleToChange, false);
        }
    }
    public static void ChangeNearestLicensePlate()
    {
        if (!SpareLicensePlates.Any())
            return;


        Vehicle[] NearbyVehicles = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 10f, GetEntitiesFlags.ConsiderAllVehicles).Where(x => x is Vehicle).ToArray(), (x => (Vehicle)x));
        Vehicle ClosestVehicle = NearbyVehicles.OrderBy(x => GetLicensePlateChangePosition(x).DistanceTo2D(Game.LocalPlayer.Character.Position)).FirstOrDefault();

        if (ClosestVehicle != null)
        {
            GTAVehicle VehicleToChange = TrackedVehicles.Where(x => x.VehicleEnt.Handle == ClosestVehicle.Handle).FirstOrDefault();
            if (VehicleToChange == null)
            {
                VehicleToChange = new GTAVehicle(ClosestVehicle, false, false, new GTALicensePlate(ClosestVehicle.LicensePlate, (uint)ClosestVehicle.Handle, NativeFunction.CallByName<int>("GET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", ClosestVehicle), false));
                TrackedVehicles.Add(VehicleToChange);
            }

            ChangeLicensePlateAnimation(VehicleToChange, true);
        }
    }
    public static void ChangeLicensePlateAnimation(GTAVehicle VehicleToChange, bool ChangePlates)
    {
        if (!ChangePlates && VehicleToChange.VehicleEnt.LicensePlate == "        ")// Plate already removed
            return;

        GameFiber.StartNew(delegate
        {
            try
            {
                Vector3 CarPosition = VehicleToChange.VehicleEnt.Position;
                Vector3 ChangeSpot = GetLicensePlateChangePosition(VehicleToChange.VehicleEnt);
                if (ChangeSpot == Vector3.Zero)
                    return;

                SetPedUnarmed(Game.LocalPlayer.Character, false);
                if (!MovePedToCarPosition(VehicleToChange.VehicleEnt, Game.LocalPlayer.Character, VehicleToChange.VehicleEnt.Heading, ChangeSpot, true))
                    return;

                Police.PlayerChangingPlate = true;
                Rage.Object Screwdriver = AttachScrewdriverToPed(Game.LocalPlayer.Character);
                Rage.Object LicensePlate = null;
                if(ChangePlates)
                    LicensePlate = AttachLicensePlateToPed(Game.LocalPlayer.Character);

                RequestAnimationDictionay("mp_car_bomb");
                uint GameTimeStartedAnimation = Game.GameTime;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "mp_car_bomb", "car_bomb_mechanic", 2.0f, -2.0f, 5000, 0, 0, false, false, false);
                bool Continue = true;
                while (Game.LocalPlayer.Character.IsAlive)//CarPosition.DistanceTo2D(VehicleToChange.VehicleEnt.Position) <= 0.5f && Game.LocalPlayer.Character.DistanceTo2D(ChangeSpot) <= 0.5f && Game.LocalPlayer.Character.IsAlive)
                {
                    if (Extensions.IsMoveControlPressed())
                    {
                        Continue = false;
                        break;
                    }
                    if (Game.GameTime - GameTimeStartedAnimation >= 2000)
                        break;
                    GameFiber.Yield();
                }

                if (Continue && CarPosition.DistanceTo2D(VehicleToChange.VehicleEnt.Position) <= 1f && Game.LocalPlayer.Character.DistanceTo2D(ChangeSpot) <= 2f && !Game.LocalPlayer.Character.IsDead)
                {
                    if (ChangePlates)
                        ChangePlate(VehicleToChange);
                    else
                        RemovePlate(VehicleToChange);
                }
                else
                {
                    Game.LocalPlayer.Character.Tasks.Clear();
                }

                if(LicensePlate != null)
                    LicensePlate.Delete();
                GameFiber.Sleep(750);
                Screwdriver.Delete();
                CreatedObjects.Clear();
                Police.PlayerChangingPlate = false;
            }
            catch (Exception e)
            {
                foreach (Rage.Object obj in CreatedObjects.Where(x => x.Exists()))
                    obj.Delete();
                CreatedObjects.Clear();
                Police.PlayerChangingPlate = false;
                WriteToLog("ChangeLicensePlate", e.StackTrace);
            }
        });
    }
    public static bool RemovePlate(GTAVehicle VehicleToChange)
    {
        if (VehicleToChange.VehicleEnt.Exists())
        {
            SpareLicensePlates.Add(VehicleToChange.CarPlate);
            Menus.UpdateLists();
            VehicleToChange.CarPlate = null;
            VehicleToChange.VehicleEnt.LicensePlate = "        ";
            VehicleToChange.CarPlate = null;
            return true;
        }
        return false;
    }
    public static bool ChangePlate(GTAVehicle VehicleToChange)
    {
        if (VehicleToChange.VehicleEnt.Exists())
        {
            GTALicensePlate PlateToAdd = SpareLicensePlates[Menus.ChangePlateIndex];
            GTALicensePlate PlateToRemove = VehicleToChange.CarPlate;
            SpareLicensePlates.RemoveAt(Menus.ChangePlateIndex);
            if (PlateToRemove != null)
            {
                SpareLicensePlates.Add(PlateToRemove);
            }

            VehicleToChange.CarPlate = PlateToAdd;
            VehicleToChange.VehicleEnt.LicensePlate = PlateToAdd.PlateNumber;
            NativeFunction.CallByName<int>("SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", VehicleToChange.VehicleEnt, PlateToAdd.PlateType);
            Menus.UpdateLists();
            return true;
        }
        return false;
    }

    //General Car Items
    public static bool MovePedToCarPosition(Vehicle TargetVehicle, Ped PedToMove, float DesiredHeading, Vector3 PositionToMoveTo, bool StopDriver)
    {
        bool Continue = true;
        bool isPlayer = false;
        if (PedToMove == Game.LocalPlayer.Character)
            isPlayer = true;
        Ped Driver = TargetVehicle.Driver;
        Vector3 CarPosition = TargetVehicle.Position;
        NativeFunction.CallByName<uint>("TASK_PED_SLIDE_TO_COORD", PedToMove, PositionToMoveTo.X, PositionToMoveTo.Y, PositionToMoveTo.Z, DesiredHeading, -1);

        while (!(PedToMove.DistanceTo2D(PositionToMoveTo) <= 0.15f && PedToMove.Heading.IsWithin(DesiredHeading - 5f, DesiredHeading + 5f)))
        {
            GameFiber.Yield();
            if (isPlayer && Extensions.IsMoveControlPressed())
            {
                Continue = false;
                break;
            }
            if (StopDriver && TargetVehicle.Driver != null)
                NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", Driver, TargetVehicle, 27, -1);
        }
        if (!Continue)
        {
            PedToMove.Tasks.Clear();
            return false;
        }
        return true;
    }
    public static Vector3 GetHandlePosition(Vehicle TargetVehicle)
    {
        Vector3 GameEntryPosition = Vector3.Zero;
        if (TargetVehicle.HasBone("handle_dside_f") && 1 == 0)
        {
            GameEntryPosition = TargetVehicle.GetBonePosition("handle_dside_f");
            WriteToLog("CarJackPedWithWeapon", string.Format("Handle Pos: {0}", GameEntryPosition));
        }
        else
        {
            GameEntryPosition = NativeFunction.CallByHash<Vector3>(0xC0572928C0ABFDA3, TargetVehicle, 0);
            WriteToLog("CarJackPedWithWeapon", string.Format("Game Entry Pos: {0}", GameEntryPosition));
        }
        return GameEntryPosition;
    }
    public static Vector3 GetLicensePlateChangePosition(Vehicle VehicleToChange)
    {
        Vector3 Position;
        Vector3 Right;
        Vector3 Forward;
        Vector3 Up;

        if (VehicleToChange.HasBone("numberplate"))
        {
            Position = VehicleToChange.GetBonePosition("numberplate");
            VehicleToChange.GetBoneAxes("numberplate", out Right, out Forward, out Up);
            return Vector3.Add(Forward * -1.0f, Position);
        }

        else if (VehicleToChange.HasBone("boot"))
        {
            Position = VehicleToChange.GetBonePosition("boot");
            VehicleToChange.GetBoneAxes("boot", out Right, out Forward, out Up);
            return Vector3.Add(Forward * -1.75f, Position);
        }
        else if (VehicleToChange.IsBike)
        {
            return VehicleToChange.GetOffsetPositionFront(-1.5f);
        }
        else if (VehicleToChange.HasBone("bumper_r"))
        {
            Position = VehicleToChange.GetBonePosition("bumper_r");
            VehicleToChange.GetBoneAxes("bumper_r", out Right, out Forward, out Up);
            Position = Vector3.Add(Forward * -1.0f, Position);
            return Vector3.Add(Right * 0.25f, Position);
        }
        else
            return Vector3.Zero;
    }

    //Respawn
    public static void BribePolice(int Amount)
    {
        if (Game.LocalPlayer.Character.GetCash(Settings.MainCharacterToAlias) < Amount)
            return;

        if (Amount < PreviousWantedLevel * Settings.PoliceBribeWantedLevelScale)
        {
            Game.DisplayNotification("Thats it? Thanks for the cash, but you're going downtown.");
            Game.LocalPlayer.Character.GiveCash(-1 * Amount, Settings.MainCharacterToAlias);
            return;
        }
        else
        {
            BeingArrested = false;
            isBusted = false;
            Game.DisplayNotification("Thanks for the cash, now beat it.");
            Game.LocalPlayer.Character.GiveCash(-1 * Amount, Settings.MainCharacterToAlias);
        }






        Police.PlayerIsPersonOfInterest = false;
        Police.CurrentPoliceState = Police.PoliceState.Normal;
        UnSetArrestedAnimation(Game.LocalPlayer.Character);
        NativeFunction.CallByName<bool>("RESET_PLAYER_ARREST_STATE", Game.LocalPlayer);
        //if (Game.LocalPlayer.Character.LastVehicle.Exists())
        //    NativeFunction.CallByName<bool>("SET_VEHICLE_HAS_BEEN_OWNED_BY_PLAYER", Game.LocalPlayer.Character.LastVehicle, true);
        ResetPlayer(true, false);

        //PoliceScanning.UntaskAll(true);

    }
    public static void BribeAnimation(Ped Cop,Ped Briber)
    {
        GameFiber.StartNew(delegate
        {
            RequestAnimationDictionay("mp_common");
            Vector3 BriberPos = Briber.Position;
            Vector3 CopPos = Cop.Position;

            WriteToLog("Bribe Animation", string.Format("Briber Pos: {0}", BriberPos));

            int scene1 = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", BriberPos.X-0.5f, BriberPos.Y, BriberPos.Z - 1.0f, 0.0f, 0.0f, 180f, 2);
            NativeFunction.CallByName<int>("SET_SYNCHRONIZED_SCENE_LOOPED", scene1, true);
            NativeFunction.CallByName<int>("TASK_SYNCHRONIZED_SCENE", Cop, scene1, "mp_common", "givetake1_a", 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);
            NativeFunction.CallByName<int>("TASK_SYNCHRONIZED_SCENE", Briber, scene1, "mp_common", "givetake1_b", 1000.0f, -4.0f, 1, 0, 0x447a0000, 0);
            NativeFunction.CallByName<int>("SET_SYNCHRONIZED_SCENE_PHASE", scene1, 0.0f);

            NativeFunction.CallByHash<bool>(0xCD9CC7E200A52A6F, scene1);
            GameFiber.Sleep(1000);

            WriteToLog("Bribe Animation", string.Format("Briber New Pos: {0}", Briber.Position));


            //RequestAnimationDictionay("mp_arresting");
            //Vector3 PlayerPos = Game.LocalPlayer.Character.Position;
            //int scene1 = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", PlayerPos.X, PlayerPos.Y, PlayerPos.Z - 1f, 0.0f, 0.0f, 180.0f, 2);
            //NativeFunction.CallByName<int>("SET_SYNCHRONIZED_SCENE_LOOPED", scene1, false);
            //NativeFunction.CallByName<int>("TASK_SYNCHRONIZED_SCENE", Cop, scene1, "mp_arresting", "a_uncuff", 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);
            //NativeFunction.CallByName<int>("TASK_SYNCHRONIZED_SCENE", Briber, scene1, "mp_arresting", "b_uncuff", 1000.0f, -4.0f, 1, 0, 0x447a0000, 0);
            //NativeFunction.CallByName<int>("SET_SYNCHRONIZED_SCENE_PHASE", scene1, 0.0f);


            //NativeFunction.CallByHash<bool>(0xCD9CC7E200A52A6F, scene1);




            //RequestAnimationDictionay("mp_arrest_paired");
            //Vector3 PlayerPos = Game.LocalPlayer.Character.Position;
            //int scene1 = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", PlayerPos.X, PlayerPos.Y, PlayerPos.Z, 0.0f, 0.0f, 180.0f, 2);
            //NativeFunction.CallByName<int>("SET_SYNCHRONIZED_SCENE_LOOPED", scene1, false);
            //NativeFunction.CallByName<int>("TASK_SYNCHRONIZED_SCENE", Cop, scene1, "mp_arrest_paired", "cop_p2_fwd", 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);
            //NativeFunction.CallByName<int>("TASK_SYNCHRONIZED_SCENE", Briber, scene1, "mp_arrest_paired", "crook_p2", 1000.0f, -4.0f, 1, 0, 0x447a0000, 0);
            //NativeFunction.CallByName<int>("SET_SYNCHRONIZED_SCENE_PHASE", scene1, 0.0f);


            //NativeFunction.CallByHash<bool>(0xCD9CC7E200A52A6F, scene1);



            //RequestAnimationDictionay("mp_arresting");
            //Vector3 BriberPos = Briber.Position;
            //Vector3 CopPos = Cop.Position;
            //int scene1 = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", BriberPos.X, BriberPos.Y - 1.1f, BriberPos.Z, 0.0f, 0.0f, 0f, 2);
            //NativeFunction.CallByName<int>("SET_SYNCHRONIZED_SCENE_LOOPED", scene1, true);
            //NativeFunction.CallByName<int>("TASK_SYNCHRONIZED_SCENE", Cop, scene1, "mp_arresting", "a_uncuff", 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);
            //NativeFunction.CallByName<int>("SET_SYNCHRONIZED_SCENE_PHASE", scene1, 0.0f);

            //int scene2 = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", BriberPos.X, BriberPos.Y, BriberPos.Z, 0.0f, 0.0f, 0f, 2);
            //NativeFunction.CallByName<int>("SET_SYNCHRONIZED_SCENE_LOOPED", scene2, true);
            //NativeFunction.CallByName<int>("TASK_SYNCHRONIZED_SCENE", Briber, scene2, "mp_arresting", "b_uncuff", 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);
            //NativeFunction.CallByName<int>("SET_SYNCHRONIZED_SCENE_PHASE", scene2, 0.0f);



            //RequestAnimationDictionay("mp_arresting");
            //Vector3 BriberPos = Briber.Position;
            //Vector3 CopPos = Cop.Position;
            //int scene1 = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", CopPos.X, CopPos.Y, CopPos.Z, 0.0f, 0.0f, 0f, 2);
            //NativeFunction.CallByName<int>("SET_SYNCHRONIZED_SCENE_LOOPED", scene1, true);
            //NativeFunction.CallByName<int>("TASK_SYNCHRONIZED_SCENE", Cop, scene1, "mp_arresting", "arresting_cop_shove_left_short", 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);
            //NativeFunction.CallByName<int>("SET_SYNCHRONIZED_SCENE_PHASE", scene1, 0.0f);

            //int scene2 = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", CopPos.X, CopPos.Y + 0.9f, CopPos.Z, 0.0f, 0.0f, 0f, 2);
            //NativeFunction.CallByName<int>("SET_SYNCHRONIZED_SCENE_LOOPED", scene2, true);
            //NativeFunction.CallByName<int>("TASK_SYNCHRONIZED_SCENE", Briber, scene2, "mp_arresting", "arrested_spin_l_0", 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);
            //NativeFunction.CallByName<int>("SET_SYNCHRONIZED_SCENE_PHASE", scene2, 0.0f);

            //NativeFunction.CallByHash<bool>(0xCD9CC7E200A52A6F, scene1);

            //NativeFunction.CallByHash<bool>(0xCD9CC7E200A52A6F, scene2);

            //RequestAnimationDictionay("random@struggle_thief@flee");
            //Vector3 PlayerPos = Game.LocalPlayer.Character.Position;
            //int scene1 = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", PlayerPos.X, PlayerPos.Y, PlayerPos.Z - 1f, 0.0f, 0.0f, 180.0f, 2);
            //NativeFunction.CallByName<int>("SET_SYNCHRONIZED_SCENE_LOOPED", scene1, false);
            //NativeFunction.CallByName<int>("TASK_SYNCHRONIZED_SCENE", Cop, scene1, "random@struggle_thief@flee", "flee_backward_shopkeeper", 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);
            //NativeFunction.CallByName<int>("TASK_SYNCHRONIZED_SCENE", Briber, scene1, "random@struggle_thief@flee", "flee_backward_thief", 1000.0f, -4.0f, 1, 0, 0x447a0000, 0);
            //NativeFunction.CallByName<int>("SET_SYNCHRONIZED_SCENE_PHASE", scene1, 0.0f);
        });
    }
    public static void RespawnAtHospital()
    {
        Game.FadeScreenOut(1500);
        GameFiber.Wait(1500);
        isDead = false;
        isBusted = false;
        Police.CurrentPoliceState = Police.PoliceState.Normal;
        Police.PlayerIsPersonOfInterest = false;
        ResetPlayer(true, true);

        Game.LocalPlayer.Character.Inventory.Weapons.Clear();
        RespawnInPlace(false);
        Police.SetWantedLevel(0);
        Location ClosestHospital = Locations.GetClosestLocationByType(Game.LocalPlayer.Character.Position, Location.LocationType.Hospital);

        Game.LocalPlayer.Character.Position = ClosestHospital.LocationPosition;
        Game.LocalPlayer.Character.Heading = ClosestHospital.Heading;

        GameFiber.Wait(1500);
        Game.FadeScreenIn(1500);
        Game.DisplayNotification(string.Format("You have been charged ~r~${0} ~s~in Hospital fees.", Settings.HospitalFee));
        Game.LocalPlayer.Character.GiveCash(-1 * Settings.HospitalFee, Settings.MainCharacterToAlias);     
    }
    public static void ResistArrest()
    {
        isBusted = false;
        BeingArrested = false;
        areHandsUp = false;
        Police.CurrentPoliceState = Police.PoliceState.DeadlyChase;
        UnSetArrestedAnimation(Game.LocalPlayer.Character);
        NativeFunction.CallByName<uint>("RESET_PLAYER_ARREST_STATE", Game.LocalPlayer);
        ResetPlayer(false, false);
        Tasking.UntaskAll(true);
    }
    public static void Surrender()
    {
        Game.FadeScreenOut(1500);
        GameFiber.Wait(1500);

        int bailMoney = MaxWantedLastLife * Settings.PoliceBailWantedLevelScale;
        BeingArrested = false;
        isBusted = false;
        Police.SetWantedLevel(0);
        Police.PlayerIsPersonOfInterest = false;
        RaiseHands();
        ResetPlayer(true, true);
        NativeFunction.CallByName<bool>("RESET_PLAYER_ARREST_STATE", Game.LocalPlayer);
        Location ClosestPolice = Locations.GetClosestLocationByType(Game.LocalPlayer.Character.Position, Location.LocationType.Police);

        Game.LocalPlayer.Character.Position = ClosestPolice.LocationPosition;
        Game.LocalPlayer.Character.Heading = ClosestPolice.Heading;

        Game.LocalPlayer.Character.Tasks.ClearImmediately();
        Game.LocalPlayer.Character.Inventory.Weapons.Clear();
        Game.LocalPlayer.Character.Inventory.GiveNewWeapon((WeaponHash)2725352035, -1, true);
        ResetPlayer(true, true);
        Police.CurrentPoliceState = Police.PoliceState.Normal;
        GameFiber.Wait(1500);
        Game.FadeScreenIn(1500);   
        Game.DisplayNotification(string.Format("You have been charged ~r~${0} ~s~in bail money, try to stay out of trouble.", bailMoney));
        Game.LocalPlayer.Character.GiveCash(-1 * bailMoney, Settings.MainCharacterToAlias);
       // PoliceScanning.UntaskAll(true);
    }
    public static void ResetPlayer(bool ClearWanted, bool ResetHealth)
    {
        isDead = false;
        isBusted = false;
        BeingArrested = false;

        NativeFunction.CallByName<bool>("NETWORK_REQUEST_CONTROL_OF_ENTITY", Game.LocalPlayer.Character);
        NativeFunction.Natives.xC0AA53F866B3134D();
        Game.TimeScale = 1f;
        if (ClearWanted)
        {
            Police.SetWantedLevel(0);
            Police.ResetPoliceStats();
            TrafficViolations.ResetTrafficViolations();
            Police.ResetPersonOfInterest();
        }

        //ResetCamera();
        NativeFunction.Natives.xB4EDDC19532BFB85(); //_STOP_ALL_SCREEN_EFFECTS;
        if (ResetHealth)
            Game.LocalPlayer.Character.Health = 100;
    }
    public static void RespawnInPlace(bool AsOldCharacter)
    {
        try
        {
            isDead = false;
            isBusted = false;
            BeingArrested = false;
            Game.LocalPlayer.Character.Health = 100;
            if (DiedInVehicle)
            {
                NativeFunction.Natives.xB69317BF5E782347(Game.LocalPlayer.Character);//"NETWORK_REQUEST_CONTROL_OF_ENTITY" 
                NativeFunction.Natives.xEA23C49EAA83ACFB(Game.LocalPlayer.Character.Position.X + 10f, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z, 0, false, false);//"NETWORK_RESURRECT_LOCAL_PLAYER"
                NativeFunction.Natives.xC0AA53F866B3134D();//_RESET_LOCALPLAYER_STATE
                if (Game.LocalPlayer.Character.LastVehicle.Exists() && Game.LocalPlayer.Character.LastVehicle.IsDriveable)
                {
                    Game.LocalPlayer.Character.WarpIntoVehicle(Game.LocalPlayer.Character.LastVehicle, -1);
                }
                NativeFunction.Natives.xC0AA53F866B3134D();//_RESET_LOCALPLAYER_STATE
            }
            else
            {
                NativeFunction.Natives.xB69317BF5E782347(Game.LocalPlayer.Character);//"NETWORK_REQUEST_CONTROL_OF_ENTITY"
                NativeFunction.Natives.xEA23C49EAA83ACFB(Game.LocalPlayer.Character.Position.X, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z, 0, false, false);//"NETWORK_RESURRECT_LOCAL_PLAYER"
                NativeFunction.Natives.xC0AA53F866B3134D();//_RESET_LOCALPLAYER_STATE
            }
            if (AsOldCharacter)
            {
                Police.SetWantedLevel(MaxWantedLastLife);
                ++TimesDied;
               // DispatchAudioSystem.AbortAllAudio();
            }
            else
            {
                Game.LocalPlayer.Character.Inventory.Weapons.Clear();
                Game.LocalPlayer.Character.Inventory.GiveNewWeapon(2725352035, 0, true);
                PreviousWantedLevel = 0;
                Police.SetWantedLevel(0);
                TimesDied = 0;
                MaxWantedLastLife = 0;
                Police.ResetPoliceStats();
                DispatchAudio.ResetReportedItems();
                Police.ResetPersonOfInterest();

            }
            Game.TimeScale = 1f;
            DiedInVehicle = false;
            NativeFunction.Natives.xB4EDDC19532BFB85(); //_STOP_ALL_SCREEN_EFFECTS
            ResetPlayer(false, false);
            Game.HandleRespawn();
            NativeFunction.Natives.xB9EFD5C25018725A("DISPLAY_HUD", true);
        }
        catch (Exception e)
        {
            Game.LogTrivial(e.Message);
            // UI.Notify(e.Message);
        }
    }

    //Attachment
    public static Rage.Object AttachScrewdriverToPed(Ped Pedestrian)
    {
        Rage.Object Screwdriver = new Rage.Object("prop_tool_screwdvr01", Pedestrian.GetOffsetPositionUp(50f));
        CreatedObjects.Add(Screwdriver);
        int BoneIndexRightHand = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, 57005);
        Screwdriver.AttachTo(Pedestrian, BoneIndexRightHand, new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f));
        return Screwdriver;
    }
    public static Rage.Object AttachLicensePlateToPed(Ped Pedestrian)
    {
        Rage.Object LicensePlate = new Rage.Object("p_num_plate_01", Pedestrian.GetOffsetPositionUp(55f));
        CreatedObjects.Add(LicensePlate);
        LicensePlate.IsVisible = true;
        int BoneIndexLeftHand = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, 18905);
        LicensePlate.AttachTo(Game.LocalPlayer.Character, BoneIndexLeftHand, new Vector3(0.19f, 0.08f, 0.0f), new Rotator(-57.2f, 90f, -173f));
        
        return LicensePlate;
    }

    //Ped Tasking
    public static void GiveGunAndAttackPlayer(Ped Attacker)
    {
        GTAWeapon GunToGive = GTAWeapons.WeaponsList.Where(x => x.Category == GTAWeapon.WeaponCategory.Pistol).PickRandom();
        Attacker.Inventory.GiveNewWeapon(GunToGive.Name, GunToGive.AmmoAmount, true);
        Attacker.Tasks.FightAgainst(Game.LocalPlayer.Character);
        Attacker.BlockPermanentEvents = true;
        Attacker.KeepTasks = true;
    }
    public static void PedReactToThreatening(Ped Attacker)
    {
        int RandomNum = rnd.Next(1, 20);
        if (RandomNum == 1) //Murder
        {
            GTAWeapon GunToGive = GTAWeapons.GetRandomWeaponByCategory(GTAWeapon.WeaponCategory.Pistol);
            Attacker.Inventory.GiveNewWeapon(GunToGive.Name, GunToGive.AmmoAmount, true);
            Attacker.Tasks.FightAgainst(Game.LocalPlayer.Character);
            Attacker.BlockPermanentEvents = true;
            Attacker.KeepTasks = true;
        }
        //else if (RandomNum == 2) //Run Away
        //{
        //    Attacker.Tasks.Cower(30000);
        //    Attacker.BlockPermanentEvents = true;
        //    Attacker.KeepTasks = true;
        //}
        else //Flee
        {
            Attacker.Tasks.Flee(Game.LocalPlayer.Character, 100f, 30000);
            Attacker.BlockPermanentEvents = true;
            Attacker.KeepTasks = true;
        }
    }

    //Weapon Variations
    public static WeaponVariation GetWeaponVariation(Ped WeaponOwner, uint WeaponHash)
    {
        int Tint = NativeFunction.CallByName<int>("GET_PED_WEAPON_TINT_INDEX", WeaponOwner, WeaponHash);
        GTAWeapon MyGun = GTAWeapons.GetWeaponFromHash(WeaponHash);
        if (MyGun == null)
            return new WeaponVariation(Tint);


        List<WeaponVariation.WeaponComponent> Components = GTAWeapons.GetWeaponVariations(MyGun.Name);

        if (!Components.Any())
            return new WeaponVariation(Tint);

        foreach (WeaponVariation.WeaponComponent PossibleComponent in Components)
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
    private static void SetupLicensePlates()
    {
        List<string> StartingPlateOptions = new List<string> { "BRNEBRO", "IMWITHER", "JOE30303", "JEBSGUAC", "MAGA2020", "YNGGANG", "POCAHNTS", "NOTPHOON", "LYINTED" };
        SpareLicensePlates.Add(new GTALicensePlate(StartingPlateOptions.PickRandom(), 1, 1, false));
    }

    //Other
    public static void RequestAnimationDictionay(String sDict)
    {
        NativeFunction.CallByName<bool>("REQUEST_ANIM_DICT", sDict);
        while (!NativeFunction.CallByName<bool>("HAS_ANIM_DICT_LOADED", sDict))
            GameFiber.Yield();
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
    public static void SetPedUnarmed(Ped Pedestrian,bool SetCantChange)
    {
        if (!(Pedestrian.Inventory.EquippedWeapon == null))
        {
            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Pedestrian, (uint)2725352035, true); //Unequip weapon so you don't get shot
            if(SetCantChange)
                NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Pedestrian, false);
        }
    }

    //Test Code
    public static Vehicle GetClosestVehicleToPlayer()
    {
        Vehicle[] NearbyVehicles = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 10f, GetEntitiesFlags.ConsiderAllVehicles).Where(x => x is Vehicle).ToArray(), (x => (Vehicle)x));
        return NearbyVehicles.OrderBy(x => x.DistanceTo2D(Game.LocalPlayer.Character.Position)).FirstOrDefault();
    }
    private static void UnlockCarDoorOld(Vehicle ToEnter, int SeatTryingToEnter)
    {
        if (!Game.IsControlPressed(2, GameControl.Enter))//holding enter go thru normal
            return;
        try
        {
            GameFiber.StartNew(delegate
            {
                if (SeatTryingToEnter != -1)
                    return;


                SetPedUnarmed(Game.LocalPlayer.Character, false);

                bool Continue = true;
                ToEnter.MustBeHotwired = true;



                //Vector3 GameEntryPosition = NativeFunction.CallByHash<Vector3>(0xC0572928C0ABFDA3, ToEnter, 0); //old
                Vector3 GameEntryPosition = ToEnter.GetBonePosition("handle_dside_f");


                Vector3 CarPosition = ToEnter.Position;
                //Game.LocalPlayer.Character.Tasks.GoStraightToPosition(GameEntryPosition, 1f, ToEnter.Heading, 1f, 10000);
                //uint GameTimeStartedWalking = Game.GameTime;
                //while (Game.LocalPlayer.Character.DistanceTo2D(GameEntryPosition) >= 0.05f && Game.GameTime - GameTimeStartedWalking < 10000 && CarPosition == ToEnter.Position && Game.LocalPlayer.Character.Speed > 0.5f)
                //{
                //    Rage.Debug.DrawArrowDebug(new Vector3(GameEntryPosition.X, GameEntryPosition.Y, GameEntryPosition.Z), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Yellow);
                //    GameFiber.Sleep(100);
                //}
                float DesiredHeading = ToEnter.Heading - 90f;
                NativeFunction.CallByName<uint>("TASK_PED_SLIDE_TO_COORD", Game.LocalPlayer.Character, GameEntryPosition.X, GameEntryPosition.Y, GameEntryPosition.Z, DesiredHeading, 3000);
                // GameFiber.Sleep(3000);



                uint GameTimeStarted = Game.GameTime;

                while (Game.GameTime - GameTimeStarted <= 1000)
                {
                    GameFiber.Yield();
                    if (Game.IsControlJustPressed(2, GameControl.MoveUp) || Game.IsControlJustPressed(2, GameControl.MoveRight) || Game.IsControlJustPressed(2, GameControl.MoveDown) || Game.IsControlJustPressed(2, GameControl.MoveLeft))
                    {
                        Continue = false;
                        break;
                    }
                }

                Rage.Object Screwdriver = new Rage.Object("prop_tool_screwdvr01", Game.LocalPlayer.Character.GetOffsetPositionUp(2f));
                CreatedObjects.Add(Screwdriver);
                int BoneIndexRightHand = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, 57005);
                Screwdriver.AttachTo(Game.LocalPlayer.Character, BoneIndexRightHand, new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f));
                Game.LocalPlayer.Character.Tasks.Clear();

                NativeFunction.CallByName<int>("TASK_ACHIEVE_HEADING", Game.LocalPlayer.Character, DesiredHeading, 1500);
                uint GameTimeStartedHeading = Game.GameTime;
                while (Game.LocalPlayer.Character.Heading != DesiredHeading && Game.GameTime - GameTimeStartedHeading < 700)
                {
                    GameFiber.Yield();
                    if (Game.IsControlJustPressed(2, GameControl.MoveUp) || Game.IsControlJustPressed(2, GameControl.MoveRight) || Game.IsControlJustPressed(2, GameControl.MoveDown) || Game.IsControlJustPressed(2, GameControl.MoveLeft))
                    {
                        Continue = false;
                        break;
                    }
                }

                RequestAnimationDictionay("veh@break_in@0h@p_m_one@");
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "veh@break_in@0h@p_m_one@", "std_force_entry_ds", 2.0f, -2.0f, -1, 0, 0, false, false, false);

                Police.PlayerBreakingIntoCar = true;

                GameTimeStarted = Game.GameTime;
                while (Game.GameTime - GameTimeStarted <= 1750)
                {
                    GameFiber.Yield();
                    if (Game.IsControlJustPressed(2, GameControl.MoveUp) || Game.IsControlJustPressed(2, GameControl.MoveRight) || Game.IsControlJustPressed(2, GameControl.MoveDown) || Game.IsControlJustPressed(2, GameControl.MoveLeft))
                    {
                        Continue = false;
                        break;
                    }
                }
                //GameFiber.Sleep(1750);

                if (!Continue)
                {
                    Game.LocalPlayer.Character.Tasks.Clear();
                    Screwdriver.Delete();
                    Police.PlayerBreakingIntoCar = false;
                    return;
                }

                ToEnter.LockStatus = VehicleLockStatus.Unlocked;
                ToEnter.Doors[SeatTryingToEnter + 1].Open(true, false);

                GameTimeStarted = Game.GameTime;
                while (Game.GameTime - GameTimeStarted <= 1000)
                {
                    GameFiber.Yield();
                    if (Game.IsControlJustPressed(2, GameControl.MoveUp) || Game.IsControlJustPressed(2, GameControl.MoveRight) || Game.IsControlJustPressed(2, GameControl.MoveDown) || Game.IsControlJustPressed(2, GameControl.MoveLeft))
                    {
                        Continue = false;
                        break;
                    }
                }

                if (!Continue)
                {
                    Game.LocalPlayer.Character.Tasks.Clear();
                    Screwdriver.Delete();
                    Police.PlayerBreakingIntoCar = false;
                    return;
                }

                //GameFiber.Sleep(1000);
                Game.LocalPlayer.Character.Tasks.EnterVehicle(ToEnter, SeatTryingToEnter);
                GameFiber.Sleep(5000);
                Screwdriver.Delete();
                Police.PlayerBreakingIntoCar = false;
            });
        }
        catch (Exception e)
        {
            foreach (Rage.Object obj in CreatedObjects.Where(x => x.Exists()))
                obj.Delete();
            CreatedObjects.Clear();
            Police.PlayerBreakingIntoCar = false;
            WriteToLog("UnlockCarDoor", e.Message);
        }


    }
    private static bool IsRunningRedNew()
    {

        //Vector3 PlayerPos = Game.LocalPlayer.Character.Position;
        //int NodeID = NativeFunction.CallByName<int>("GET_NTH_CLOSEST_VEHICLE_NODE_ID", PlayerPos.X, PlayerPos.Y, PlayerPos.Z, 1, 0, 300f, 300f);


        return false;

        //List<Ped> CloseDrivers = PoliceScanning.Civilians.Where(x => x.Exists() && x.IsAlive && x.DistanceTo2D(Game.LocalPlayer.Character) <= 20f && x.IsInAnyVehicle(false)).ToList();
        //Vehicle PlayersVehicle = Game.LocalPlayer.Character.CurrentVehicle;

        //foreach (Ped TrafficPed in CloseDrivers)
        //{
        //    Vehicle TrafficeVehicle = TrafficPed.CurrentVehicle;
        //    bool StoppedAtRed = NativeFunction.CallByHash<bool>(0x2959F696AE390A99, TrafficeVehicle);
        //    if (StoppedAtRed)
        //    {
        //        Rage.Debug.DrawArrowDebug(new Vector3(TrafficeVehicle.Position.X, TrafficeVehicle.Position.Y, TrafficeVehicle.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Red);
        //        if ((PlayersVehicle.FacingSameDirection(TrafficeVehicle) || PlayersVehicle.FacingOppositeDirection(TrafficeVehicle)) && PlayersVehicle.InFrontOf(TrafficeVehicle) && PlayersVehicle.Speed >= 3f)
        //        {
        //            return true;
        //        }
        //    }
        //}
        //return false;














        //Vehicle[] MyVehciles = World.GetAllVehicles();

        //Vehicle TrafficeVehicle = (Vehicle)World.GetClosestEntity(Game.LocalPlayer.Character.Position, 20f, GetEntitiesFlags.ConsiderGroundVehicles | GetEntitiesFlags.ExcludePlayerVehicle);
        // if (TrafficeVehicle == null)
        //    return false;
        //Vehicle[] TrafficeVehicles = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 20f, GetEntitiesFlags.ConsiderGroundVehicles | GetEntitiesFlags.ExcludePlayerVehicle).Where(x => x is Vehicle).ToArray(), (x => (Vehicle)x));
        // foreach (Vehicle TrafficeVehicle in TrafficeVehicles)
        // {

        // Vehicle PlayersVehicle = Game.LocalPlayer.Character.CurrentVehicle;
        //bool FacingSameDirection = PlayersVehicle.FacingSameDirection(TrafficeVehicle);
        //bool FacingOppositeDirection = PlayersVehicle.FacingOppositeDirection(TrafficeVehicle);

        //if (FacingSameDirection)
        //    Rage.Debug.DrawArrowDebug(new Vector3(TrafficeVehicle.Position.X, TrafficeVehicle.Position.Y, TrafficeVehicle.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Green);
        //if (FacingOppositeDirection)
        //    Rage.Debug.DrawArrowDebug(new Vector3(TrafficeVehicle.Position.X, TrafficeVehicle.Position.Y, TrafficeVehicle.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Red);

        //    // bool StoppedAtRed = NativeFunction.CallByHash<bool>(0x2959F696AE390A99, TrafficeVehicle);
        //    if (StoppedAtRed)
        //{
        //    if (PlayersVehicle.FacingSameDirection(TrafficeVehicle) || PlayersVehicle.FacingOppositeDirection(TrafficeVehicle) && PlayersVehicle.InFrontOf(TrafficeVehicle) && PlayersVehicle.Speed >= 3f)
        //    {
        //        return true;
        //    }
        //}
        //// }
        //return false;
        //this.vehs = World.GetNearbyVehicles(Game.Player.Character.Position, 9f);
        //if (this.vehs.Length == 0)
        //    return false;
        //for (int index = 0; index < this.vehs.Length; ++index)
        //{
        //    if (this.autolista.Count == 0)
        //        this.autolista.Add(this.vehs[index]);
        //    else if (!this.autolista.Contains(this.vehs[index]))
        //        this.autolista.Add(this.vehs[index]);
        //}
        //if (this.autolista.Count == 0)
        //    return false;
        //for (int index = 0; index < this.autolista.Count; ++index)
        //{
        //    if ((double)Vector3.Distance(this.autolista[index].Position, Game.Player.Character.Position) > 35.0)
        //        this.autolista.Remove(this.autolista[index]);
        //}


        //for (int index = 0; index < this.autolista.Count; ++index)
        //{
        //    if (Function.Call<bool>(Hash._0x2959F696AE390A99, (InputArgument)this.autolista[index]))
        //    {
        //        float num = Vector3.Angle(this.autolista[index].ForwardVector, Game.Player.Character.ForwardVector);
        //        if ((double)num <= 35.0 && ((double)num <= 35.0 && (double)Vector3.Distance(this.autolista[index].Position, Game.Player.Character.Position) > 30.0 && (double)Vector3.Angle(this.autolista[index].Position - Game.Player.Character.Position, Game.Player.Character.ForwardVector) > 150.0))
        //            return true;
        //    }
        //}
        //return false;

    }
    private static bool RunningRedLight()
    {

        Vehicle[] Vehicles = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 10f, GetEntitiesFlags.ConsiderAllVehicles | GetEntitiesFlags.ExcludePlayerVehicle).Where(x => x is Vehicle).ToArray(), (x => (Vehicle)x));
        foreach (Vehicle vehicle in Vehicles)
        {
            if (NativeFunction.CallByName<bool>("IS_VEHICLE_STOPPED_AT_TRAFFIC_LIGHTS", vehicle))
            {
                if (GetPedStreetName(vehicle.Driver) == GetPedStreetName(Game.LocalPlayer.Character) && vehicle.PlayerVehicleIsBehind() && Game.LocalPlayer.Character.CurrentVehicle.Speed >= 10f) // We are on the same street and they are stopped
                {
                    //WriteToLog("TrafficViolationsTick", string.Format("Vehicle stopped on same street: {0}", true));
                    return true;
                }
                //if(vehicle.PlayerVehicleIsBehind())
                //{
                //    return true;
                //}
            }
        }
        return false;



        //this.vehs = World.GetNearbyVehicles(((Entity)Game.get_Player().get_Character()).get_Position(), 9f);
        //if (this.vehs.Length == 0)
        //    return false;
        //for (int index = 0; index < this.vehs.Length; ++index)
        //{
        //    if (this.autolista.Count == 0)
        //        this.autolista.Add(this.vehs[index]);
        //    else if (!this.autolista.Contains(this.vehs[index]))
        //        this.autolista.Add(this.vehs[index]);
        //}
        //if (this.autolista.Count == 0)
        //    return false;
        //for (int index = 0; index < this.autolista.Count; ++index)
        //{
        //    if ((double)Vector3.Distance(((Entity)this.autolista[index]).get_Position(), ((Entity)Game.get_Player().get_Character()).get_Position()) > 35.0)
        //        this.autolista.Remove(this.autolista[index]);
        //}
        //for (int index = 0; index < this.autolista.Count; ++index)
        //{
        //    if (Function.Call<bool>((Hash)2979683755510794905L, new InputArgument[1]
        //    {
        //  InputArgument.op_Implicit(this.autolista[index])
        //    }) != 0)
        //    {
        //        float num = Vector3.Angle(((Entity)this.autolista[index]).get_ForwardVector(), ((Entity)Game.get_Player().get_Character()).get_ForwardVector());
        //        if ((double)num <= 35.0 && ((double)num <= 35.0 && (double)Vector3.Distance(((Entity)this.autolista[index]).get_Position(), ((Entity)Game.get_Player().get_Character()).get_Position()) > 30.0 && (double)Vector3.Angle(Vector3.op_Subtraction(((Entity)this.autolista[index]).get_Position(), ((Entity)Game.get_Player().get_Character()).get_Position()), ((Entity)Game.get_Player().get_Character()).get_ForwardVector()) > 150.0))
        //            return true;
        //    }
        //}
        //return false;
    }
    private static void RunningRedTick()
    {
        Vector3 PlayerPos = Game.LocalPlayer.Character.Position;
        Vector3 MyOutput;
        int Density;
        int Flag;
        int NodeID = 0;

        unsafe
        {
            NativeFunction.CallByName<bool>("GET_VEHICLE_NODE_PROPERTIES", PlayerPos.X, PlayerPos.Y, PlayerPos.Z, &Density, &Flag);
        }



        string s = Convert.ToString(Flag, 2); //Convert to binary in a string

        int[] bits = s.PadLeft(10, '0') // Add 0's from left
                     .Select(c => int.Parse(c.ToString())) // convert each char to int
                     .ToArray(); // Convert IEnumerable from select to Array



        UI.Text(string.Format("Node Flag: {0}, {1}", Flag, string.Join("", bits)), 0.5f, 0.5f, 0.5f, true, Color.Yellow, UI.eFont.FontChaletComprimeCologne);


        NodeID = NativeFunction.CallByName<int>("GET_NTH_CLOSEST_VEHICLE_NODE_ID", PlayerPos.X, PlayerPos.Y, PlayerPos.Z, 1, 0, 300f, 300f);
        unsafe
        {
            NativeFunction.CallByName<bool>("GET_VEHICLE_NODE_POSITION", NodeID, &MyOutput);
        }

        if (Flag > 128)// intersection like stuff? seems to be a bit that add up? 2 lanes + 128 for intersection = 130?
        {
            Rage.Debug.DrawArrowDebug(new Vector3(MyOutput.X, MyOutput.Y, MyOutput.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Red);
        }
        else
        {
            Rage.Debug.DrawArrowDebug(new Vector3(MyOutput.X, MyOutput.Y, MyOutput.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Green);
        }



        //Vector3 PlayerPos = Game.LocalPlayer.Character.Position;

        //int NodeID = 0;

        //Vector3 MyOutput;

        //for (int i = 1; i < 101; i++)
        //{
        //    NodeID = NativeFunction.CallByName<int>("GET_NTH_CLOSEST_VEHICLE_NODE_ID", PlayerPos.X, PlayerPos.Y, PlayerPos.Z, i, 0, 300f, 300f);
        //    int Density;
        //    int Flag;

        //    unsafe
        //    {
        //        NativeFunction.CallByName<bool>("GET_VEHICLE_NODE_POSITION", NodeID, &MyOutput);
        //        NativeFunction.CallByName<bool>("GET_VEHICLE_NODE_PROPERTIES", MyOutput.X, MyOutput.Y, MyOutput.Z, &Density,&Flag);
        //    }
        //    if(Flag == 4 || Flag == 128 || Flag == 512)
        //    {
        //        Rage.Debug.DrawArrowDebug(new Vector3(MyOutput.X, MyOutput.Y, MyOutput.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Orange);
        //    }
        //}





        //Vector3 MyOutput;

        //unsafe
        //{
        //    NativeFunction.CallByName<bool>("GET_VEHICLE_NODE_POSITION", NodeID, &MyOutput);
        //}



    }
    private static string GetPedStreetName(Ped MyPed)
    {
        Vector3 MyPedPos = MyPed.Position;
        int StreetHash = 0;
        int CrossingHash = 0;
        unsafe
        {
            NativeFunction.CallByName<uint>("GET_STREET_NAME_AT_COORD", MyPedPos.X, MyPedPos.Y, MyPedPos.Z, &StreetHash, &CrossingHash);
        }
        string StreetName = string.Empty;
        string CrossStreetName = string.Empty;
        Vector3 Position = MyPed.Position;
        if (StreetHash != 0)
        {
            unsafe
            {
                IntPtr ptr = Rage.Native.NativeFunction.CallByName<IntPtr>("GET_STREET_NAME_FROM_HASH_KEY", StreetHash);

                StreetName = Marshal.PtrToStringAnsi(ptr);
            }
        }
        return StreetName;
    }
    private static void CreatePassengerCop(GTACop Cop)
    {
        if (Cop.CopPed.CurrentVehicle.IsSeatFree(0))
        {
            Ped CreatedCop = new Ped("s_m_y_cop_01", new Vector3(0f, 0f, 0f), Cop.CopPed.Heading);
            CreatedCop.WarpIntoVehicle(Cop.CopPed.CurrentVehicle, 0);
        }
    }
    public static bool RadioIn()
    {
        GTACop Cop = Police.PrimaryPursuer;
        if (Cop == null)
            return false;
        else
        {
            GameFiber TaskFiber =
            GameFiber.StartNew(delegate
            {
                RequestAnimationDictionay("random@arrests");
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Cop.CopPed, "random@arrests", "generic_radio_enter", 2.0f, -2.0f, 2500, 16, 0, false, false, false);

            });
            GameFiber AudioFiber =
            GameFiber.StartNew(delegate
            {
                Cop.CopPed.PlayAmbientSpeech("GENERIC_FRIGHTENED_MED");
            });

            Cop.isTasked = false; //Doing the animation clears out his other tasks?
            while (AudioFiber.IsAlive && TaskFiber.IsAlive)
                GameFiber.Yield();

            if (Cop.CopPed.Exists() && !Cop.CopPed.IsDead)
                return true; //successfully radioed it in
            else
                return false; //died beforehand
        }

    }

    //Debug 
    public static void WriteToLog(String ProcedureString, String TextToLog)
    {
        // if (!Logging)
        //     return;
        //if (ProcedureString != "GetCarjackingAnimations")
        //    return;
        //StringBuilder sb = new StringBuilder();
        //sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ": " + ProcedureString + ": " + TextToLog + System.Environment.NewLine);
        //File.AppendAllText("Plugins\\InstantAction\\" + "log.txt", sb.ToString());
        //sb.Clear();


        if(ProcedureString == "Error")
        {
            Game.DisplayNotification("Instant Action has Crashed and needs to be restarted");
        }

        if(Settings.Logging)
            Game.Console.Print(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ": " + ProcedureString + ": " + TextToLog);
    }
    private static void DebugNonInvincible()
    {
        Game.LocalPlayer.Character.IsInvincible = false;
        Game.LocalPlayer.Character.Health = 100;
        WriteToLog("KeyDown", "You are NOT invicible");
    }
    private static void DebugInvincible()
    {
        Game.LocalPlayer.Character.IsInvincible = true;
        Game.LocalPlayer.Character.Health = 100;
        WriteToLog("KeyDown", "You are invicible");
    }
    private static void DebugCopReset()
    {
        Police.CurrentPoliceState = Police.PoliceState.Normal;
        Game.LocalPlayer.WantedLevel = 0;
        Tasking.UntaskAll(true);


        foreach (GTACop Cop in PoliceScanning.K9Peds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.CopPed.IsInHelicopter))
        {
            Cop.CopPed.Delete();
        }
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.CopPed.IsInAnyVehicle(false) && !x.CopPed.IsInHelicopter))
        {
            Cop.CopPed.Delete();
        }

        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && x.CopPed.IsInAnyVehicle(false) && !x.CopPed.IsInHelicopter))
        {
            Cop.CopPed.CurrentVehicle.Delete();
            Cop.CopPed.Delete();
        }


        Ped[] closestPed = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 400f, GetEntitiesFlags.ExcludePlayerPed | GetEntitiesFlags.ConsiderAnimalPeds).Where(x => x is Ped).ToArray(), (x => (Ped)x));
        foreach (Ped dog in closestPed)
        {
            dog.Delete();
        }

        Game.TimeScale = 1f;
        isBusted = false;
        BeingArrested = false;
        NativeFunction.Natives.xB4EDDC19532BFB85();


        PoliceSpawning.Dispose();
    }
    private static void DebugNumpad0()
    {
        DebugNonInvincible();
    }
    private static void DebugNumpad1()
    {
        DebugInvincible();
    }
    private static void DebugNumpad2()
    {
        if (Game.LocalPlayer.WantedLevel > 0)
            Game.LocalPlayer.WantedLevel = 0;
        else
            Game.LocalPlayer.WantedLevel = 2;
    }
    private static void DebugNumpad3()
    {
        DebugCopReset();
    }
    private static void DebugNumpad4()
    {
        Settings.Logging = true;

        //TestStreetCall();


        //PoliceScanning.RemoveAllCreatedEntities();


        Tasking.RetaskAllRandomSpawns();
        return;



        PoliceSpawning.SpawnCop(Agencies.SAHP, Game.LocalPlayer.Character.GetOffsetPositionFront(10f));




        GTACop MyCop = PoliceScanning.CopPeds.OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
        if (MyCop != null)
        {

            MyCop.CopPed.Position = Game.LocalPlayer.Character.GetOffsetPositionFront(10f).Around2D(10f);



            Tasking.AddItemToQueue(new PoliceTask(MyCop, PoliceTask.Task.RandomSpawnIdle));

            return;




            MyCop.CopPed.Tasks.StandStill(-1);

            Ped PedToMove = Game.LocalPlayer.Character;
            Vector3 PositionToMoveTo = MyCop.CopPed.GetOffsetPositionFront(1f);

           // 
            bool Continue = true;
            bool isPlayer = true;
            //Vector3 Resultant = Vector3.Subtract(PositionToMoveTo, MyCop.CopPed.Position);
            //float DesiredHeading = NativeFunction.CallByName<float>("GET_HEADING_FROM_VECTOR_2D", Resultant.X, Resultant.Y);





            //NativeFunction.CallByName<uint>("TASK_PED_SLIDE_TO_COORD", PedToMove, PositionToMoveTo.X, PositionToMoveTo.Y, PositionToMoveTo.Z, DesiredHeading, -1);

            Game.LocalPlayer.Character.Tasks.GoToOffsetFromEntity(MyCop.CopPed, -1, 1f, 0f, 2f);


            while (!(PedToMove.DistanceTo2D(PositionToMoveTo) <= 0.2f))
            {
                GameFiber.Yield();
                if (isPlayer && Extensions.IsMoveControlPressed())
                {
                    Continue = false;
                    break;
                }
            }
            if (!Continue)
            {
                PedToMove.Tasks.Clear();
            }



            GameFiber.Sleep(1000);

            BribeAnimation(MyCop.CopPed, Game.LocalPlayer.Character);
            //MyCop.CopPed.Tasks.LeaveVehicle(MyCop.CopPed.CurrentVehicle, LeaveVehicleFlags.None);
            //GameFiber.Sleep(4000);
            //PoliceScanning.Untask(MyCop);

            GameFiber.Sleep(15000);

            //PoliceScanning.RandomSpawnIdle(MyCop);

            if (MyCop.CopPed.CurrentVehicle.Exists())
                MyCop.CopPed.CurrentVehicle.Delete();
            if (MyCop.CopPed.LastVehicle.Exists())
                MyCop.CopPed.LastVehicle.Delete();
            MyCop.CopPed.Delete();

        }






        //GameFiber.StartNew(delegate
        //{
        //    VehicleInfo myLookup = Vehicles.Where(x => x.VehicleClass != VehicleLookup.VehicleClass.Utility).PickRandom();
        //    Vehicle MyCar = new Vehicle(myLookup.Name, Game.LocalPlayer.Character.GetOffsetPositionFront(4f));
        //    Ped Driver = new Ped("a_m_y_hipster_01", Game.LocalPlayer.Character.Position.Around2D(5f), 0f);
        //    PoliceScanning.CreatedEntities.Add(MyCar);
        //    PoliceScanning.CreatedEntities.Add(Driver);
        //    Driver.WarpIntoVehicle(MyCar, -1);
        //    uint GameTimeStarted = Game.GameTime;
        //    //while (!Game.LocalPlayer.Character.IsGettingIntoVehicle)
        //    //  GameFiber.Yield();

        //    WriteToLog("Bones", string.Format("Driver Position: {0}", Driver.Position));
        //    WriteToLog("Bones", string.Format("MyCar Position: {0}", MyCar.Position));

        //    // CarJackPedWithWeapon(MyCar, Driver, -1);
        //    while(Game.GameTime - GameTimeStarted <= 20000)
        //    {
        //        //Text(myLookup.VehicleClass.ToString(), 0.5f, 0.5f, 0.75f, true, Color.Black);
        //        GameFiber.Yield();
        //    }

        //    if (Driver.Exists())
        //        Driver.Delete();

        //    if (MyCar.Exists())
        //        MyCar.Delete();

        //});

        //GameFiber.StartNew(delegate
        //{
        //    VehicleInfo myLookup = Vehicles.Where(x => x.VehicleClass == VehicleLookup.VehicleClass.Coupe || x.VehicleClass == VehicleLookup.VehicleClass.Sedan || x.VehicleClass == VehicleLookup.VehicleClass.Sports || x.VehicleClass == VehicleLookup.VehicleClass.SUV || x.VehicleClass == VehicleLookup.VehicleClass.Compact).PickRandom();
        //    Vehicle MyCar = new Vehicle(myLookup.Name, Game.LocalPlayer.Character.GetOffsetPositionFront(4f));
        //    Ped Driver = new Ped("a_m_y_hipster_01", Game.LocalPlayer.Character.Position.Around2D(5f), 0f);
        //    PoliceScanning.CreatedEntities.Add(MyCar);
        //    PoliceScanning.CreatedEntities.Add(Driver);
        //    Driver.WarpIntoVehicle(MyCar, -1);
        //    uint GameTimeStarted = Game.GameTime;
        //    //while (!Game.LocalPlayer.Character.IsGettingIntoVehicle)
        //    //  GameFiber.Yield();

        //    WriteToLog("Bones", string.Format("Driver Position: {0}", Driver.Position));
        //    WriteToLog("Bones", string.Format("MyCar Position: {0}", MyCar.Position));

        //    // CarJackPedWithWeapon(MyCar, Driver, -1);
        //    GameFiber.Sleep(20000);
        //    if (Driver.Exists())
        //        Driver.Delete();

        //    if (MyCar.Exists())
        //        MyCar.Delete();

        //});






        //Vehicle[] NearbyVehicles = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 10f, GetEntitiesFlags.ConsiderAllVehicles).Where(x => x is Vehicle).ToArray(), (x => (Vehicle)x));
        //Vehicle ClosestVehicle = NearbyVehicles.OrderBy(x => x.DistanceTo2D(Game.LocalPlayer.Character.Position)).FirstOrDefault();
        //if (ClosestVehicle != null)
        //{
        //    ClosestVehicle.LockStatus = (Rage.VehicleLockStatus)7;
        //}






        //DispatchAudioSystem.AbortAllAudio();







        //Vehicle MyCar = new Vehicle("gauntlet", Game.LocalPlayer.Character.GetOffsetPositionFront(4f));
        //Ped Driver = new Ped("u_m_y_hippie_01", Game.LocalPlayer.Character.Position.Around2D(5f), 0f);
        //Driver.BlockPermanentEvents = true;

        //Driver.WarpIntoVehicle(MyCar, -1);

        ////uint GameTimeStarted = Game.GameTime;
        ////while (Game.GameTime - GameTimeStarted <= 10000)
        ////{
        ////    Vector3 Resultant = Vector3.Subtract(Game.LocalPlayer.Character.Position, Driver.Position);
        ////    Driver.Heading = NativeFunction.CallByName<float>("GET_HEADING_FROM_VECTOR_2D", Resultant.X, Resultant.Y);
        ////    GameFiber.Yield();
        ////}
        //GameFiber.Sleep(3000);

        //int BoneIndexSpine = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Driver, 11816);
        //Vector3 DriverSeatCoordinates = NativeFunction.CallByName<Vector3>("GET_PED_BONE_COORDS", Driver, BoneIndexSpine, 0f, 0f, 0f);



        //uint GameTimeStarted = Game.GameTime;
        ////while (Game.GameTime - GameTimeStarted <= 10000)
        ////{
        ////    Vector3 Resultant = Vector3.Subtract(Game.LocalPlayer.Character.Position, Driver.Position);
        ////    Driver.Heading = NativeFunction.CallByName<float>("GET_HEADING_FROM_VECTOR_2D", Resultant.X, Resultant.Y);
        ////    GameFiber.Yield();
        ////}



        //Driver.Position = DriverSeatCoordinates;

        //GameFiber.Sleep(3000);

        //Driver.WarpIntoVehicle(MyCar, -1);

        //GameFiber.Sleep(3000);

        ////GameFiber.Sleep(3000);

        //if (MyCar.Exists())
        //    MyCar.Delete();

        //if (Driver.Exists())
        //    Driver.Delete();



        //foreach (DroppedWeapon MyOldGuns in DroppedWeapons)
        //{

        //    WriteToLog("WeaponInventoryChanged", string.Format("Dropped Gun {0},OldAmmo: {1}", MyOldGuns.Weapon.Hash, MyOldGuns.Ammo));

        //}




        //List<string> Bones = new List<string> { "SKEL_ROOT", "skel_root", "SKEL_Pelvis", "SKEL_PELVIS", "skel_pelvis", "SKEL_Spine_Root", "SKEL_SPINE_ROOT", "skel_spine_root", "SKEL_Spine0","SKEL_SPINE0","skel_spine0" };


        //foreach(string Stuff in Bones)
        //{
        //    if(Game.LocalPlayer.Character.HasBone(Stuff))
        //    {
        //        WriteToLog("Bones", string.Format("I have bone: {0}", Stuff));
        //    }
        //}


        //int BoneIndexSpine = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, 0);
        //Vector3 MyPosition = NativeFunction.CallByName<Vector3>("GET_PED_BONE_COORDS", Game.LocalPlayer.Character, BoneIndexSpine, 0f, 0f, 0f);
        // WriteToLog("Bones", string.Format("Spine Bone?: {0}", MyPosition));

        //Vehicle[] NearbyVehicles = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 10f, GetEntitiesFlags.ConsiderAllVehicles).Where(x => x is Vehicle).ToArray(), (x => (Vehicle)x));
        //Vehicle ClosestVehicle = NearbyVehicles.OrderBy(x => x.DistanceTo2D(Game.LocalPlayer.Character.Position)).FirstOrDefault();
        //if (ClosestVehicle != null)
        //{
        //    ClosestVehicle.LockStatus = (Rage.VehicleLockStatus)7;




        //    //Vector3 GameEntryPosition = NativeFunction.CallByHash<Vector3>(0xC0572928C0ABFDA3, ClosestVehicle, 0);
        //    //Vector3 CarPosition = ClosestVehicle.Position;
        //    //float DesiredHeading = ClosestVehicle.Heading - 90f;
        //    ////NativeFunction.CallByName<uint>("TASK_PED_SLIDE_TO_COORD", Game.LocalPlayer.Character, GameEntryPosition.X, GameEntryPosition.Y, GameEntryPosition.Z, DesiredHeading, 3000);

        //    //uint GameTimeStarted = Game.GameTime;

        //    //while (Game.GameTime - GameTimeStarted <= 10000)
        //    //{
        //    //    Rage.Debug.DrawArrowDebug(new Vector3(GameEntryPosition.X, GameEntryPosition.Y, GameEntryPosition.Z), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Yellow);
        //    //    GameFiber.Yield();
        //    //}


        //   // GameFiber.Sleep(3000);

        //}






    }
    private static void DebugNumpad5()
    {

        Settings.Logging = true;

        Vector3 CurrentWantedLevelPosition = NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
        float DistanceToPlayer = Game.LocalPlayer.Character.DistanceTo2D(CurrentWantedLevelPosition);
        float DistanceToPlacePlayerLastSeen = Game.LocalPlayer.Character.DistanceTo2D(Police.PlacePlayerLastSeen);
        WriteToLog("WantedLevel", string.Format("CenterPosition: {0},DistanceToPlayer: {1},PlacePlayerLastSeen: {2},DistanceToPlacePlayerLastSeen: {3}", CurrentWantedLevelPosition,DistanceToPlayer, Police.PlacePlayerLastSeen, DistanceToPlacePlayerLastSeen));

        WriteToLog("WantedLevel2", string.Format("LastWantedCenterPosition: {0}", Police.LastWantedCenterPosition));

       // DispatchAudioSystem.ReportSuspiciousActivity();



        if (!Game.LocalPlayer.Character.IsInAnyVehicle(false))
            return;
        DispatchAudio.ReportFelonySpeeding(GetPlayersCurrentTrackedVehicle(), 110f);
        GTAVehicle VehicleDescription = TrackedVehicles.Where(x => x.VehicleEnt.Handle == Game.LocalPlayer.Character.CurrentVehicle.Handle).FirstOrDefault();
        Vehicle myCar = VehicleDescription.VehicleEnt;

        //if (myCar.Health <= 500 || myCar.EngineHealth <= 300)
        WriteToLog("RoadWorthyness", string.Format("CurrentCar: Health: {0},Engine Health {1}", myCar.Health, myCar.EngineHealth));

        //if (!NativeFunction.CallByName<bool>("ARE_ALL_VEHICLE_WINDOWS_INTACT", myCar))
        WriteToLog("RoadWorthyness", string.Format("CurrentCar: ARE_ALL_VEHICLE_WINDOWS_INTACT: {0}", NativeFunction.CallByName<bool>("ARE_ALL_VEHICLE_WINDOWS_INTACT", myCar)));

        VehicleDoor[] CarDoors = myCar.GetDoors();

        foreach (VehicleDoor myDoor in CarDoors)
        {
            //if (myDoor.IsDamaged)
            WriteToLog("RoadWorthyness", string.Format("CurrentCar: door {0} Is Damaged: {1}", myDoor.Index, myDoor.IsDamaged));
        }


        bool LightsOn;
        bool HighbeamsOn;

        unsafe
        {
            NativeFunction.CallByName<bool>("GET_VEHICLE_LIGHTS_STATE", myCar, &LightsOn, &HighbeamsOn);
        }

        WriteToLog("RoadWorthyness", string.Format("CurrentCar: IsStolen: {0},IsRoadWorthy: {1}, .CarPlate.IsWanted: {2},ColorMatchesDescription: {3},MatchesOriginalDescription: {4}", VehicleDescription.IsStolen, myCar.IsRoadWorthy(), VehicleDescription.CarPlate.IsWanted, VehicleDescription.ColorMatchesDescription, VehicleDescription.MatchesOriginalDescription));
        WriteToLog("RoadWorthyness", string.Format("CurrentCar: Night: {0},LightsOn: {1}, HighbeamsOn: {2},RightHeadlightDamaged: {3},LeftHeadlightDmaaged: {4}",Police.IsNightTime, LightsOn, HighbeamsOn, NativeFunction.CallByName<bool>("GET_IS_RIGHT_VEHICLE_HEADLIGHT_DAMAGED", myCar), NativeFunction.CallByName<bool>("GET_IS_LEFT_VEHICLE_HEADLIGHT_DAMAGED", myCar)));

        //ReportGrandTheftAuto();
        // ReportSuspiciousVehicle(VehicleDescription);


        // WriteToLog("RoadWorthyness", string.Format("CurrentCar: CurrentLicensePlateWanted: {0},IsStolen:{1},IsWanted:{2}", VehicleDescription.CurrentLicensePlateWanted,VehicleDescription.IsStolen,VehicleDescription.IsWanted));


        WriteToLog("RoadWorthyness", string.Format("CurrentCar: IS_VEHICLE_TYRE_BURST 0: {0}", NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar, 0, false)));
        WriteToLog("RoadWorthyness", string.Format("CurrentCar: IS_VEHICLE_TYRE_BURST 1: {0}", NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar, 1, false)));
        WriteToLog("RoadWorthyness", string.Format("CurrentCar: IS_VEHICLE_TYRE_BURST 2: {0}", NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar, 2, false)));
        WriteToLog("RoadWorthyness", string.Format("CurrentCar: IS_VEHICLE_TYRE_BURST 3: {0}", NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar, 3, false)));
        WriteToLog("RoadWorthyness", string.Format("CurrentCar: IS_VEHICLE_TYRE_BURST 4: {0}", NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar, 4, false)));
        WriteToLog("RoadWorthyness", string.Format("CurrentCar: IS_VEHICLE_TYRE_BURST 5: {0}", NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar, 5, false)));


        //ReportStolenVehicle(GetPlayersCurrentTrackedVehicle());

        WriteToLog("Civilians", string.Format("Total Civilians: {0}", PoliceScanning.Civilians.Count()));






                //if (!Game.LocalPlayer.Character.IsInAnyVehicle(false))
                //    return;

                //GTAVehicle VehicleDescription = EnteredVehicles.Where(x => x.VehicleEnt.Handle == Game.LocalPlayer.Character.CurrentVehicle.Handle).FirstOrDefault();

                //Color BaseColor = GetBaseColor(VehicleDescription.OriginalColor);
                //ColorLookup LookupColor = ColorLookups.Where(x => x.BaseColor == BaseColor).PickRandom();
                //VehicleInfo VehicleInformation = InstantAction.GetVehicleInfo(VehicleDescription);
                //string ManufacturerScannerFile;
                //if (VehicleInformation != null)
                //{
                //    ManufacturerScannerFile = GetManufacturerScannerFile(VehicleInformation.Manufacturer);
                //    WriteToLog("", string.Format("Name: {0},Manufac {1}, ModelScanner {2},Color {3}", VehicleInformation.Name, VehicleInformation.Manufacturer, VehicleInformation.ModelScannerFile, LookupColor.BaseColor));
                //}
                //else
                //{
                //    WriteToLog("", string.Format("Hash: {0},Name {1}", VehicleDescription.VehicleEnt.Model.Hash, VehicleDescription.VehicleEnt.Model.Name));
                //}




                //VehicleInfo ToReturn = VehicleLookup.Vehicles.Where(x => x.Name.ToUpper() == "CAVALCADE2").FirstOrDefault();
                //WriteToLog("", string.Format("CAVALCADE2: Hash: {0},Name {1}", ToReturn.Hash, ToReturn.Name));

                //ReportStolenVehicle(VehicleDescription);

                //Rage.Object camera = new Rage.Object("prop_ing_camera_01", Game.LocalPlayer.Character.GetOffsetPosition(Vector3.RelativeFront * 2));



                //Rage.Object FoodBag = new Rage.Object("prop_tool_screwdvr01", Game.LocalPlayer.Character.GetOffsetPositionFront(2f));

                //int BoneIndex = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, 57005);



                ////FoodBag.AttachTo(Game.LocalPlayer.Character, BoneIndex, new Vector3(0.1f, -0.1f, -0.1f), new Rotator(120.0f, 0.0f, 0.0f));
                //FoodBag.AttachTo(Game.LocalPlayer.Character, BoneIndex, new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f));
                ////camera.AttachTo(Game.LocalPlayer.Character, 28252, Vector3.Zero, Rotator.Zero);
                //GameFiber.Sleep(5000);

                //FoodBag.Delete();
                //camera.Delete();
    }
    private static void DebugNumpad6()
    {
        try
        {
            // Smoking.startPTFX("core", "ent_dst_concrete_large");

            //Smoking.Start();
            //if(!Smoking.PlayersCurrentCigarette.Exists())
            //{
            //    return;
            //}

            string PTFX = "core";
            string FX = "ent_anim_cig_exhale_mth_car";

            if (!NativeFunction.CallByName<bool>("HAS_NAMED_PTFX_ASSET_LOADED", PTFX))
            {
                NativeFunction.CallByName<bool>("REQUEST_NAMED_PTFX_ASSET", PTFX);
                while (!NativeFunction.CallByName<bool>("HAS_NAMED_PTFX_ASSET_LOADED", PTFX))
                    GameFiber.Sleep(50);
            }


            //NativeFunction.CallByName<bool>("REQUEST_NAMED_PTFX_ASSET", PTFX);
            //GameFiber.Sleep(200);
            NativeFunction.CallByHash<bool>(0x6C38AF3693A69A91, PTFX);
            //NativeFunction.CallByName<bool>("START_PARTICLE_FX_NON_LOOPED_AT_COORD", FX, offset.X, offset.Y, offset.Z, 0f, 0f, 0f, 1.0f, false, false, false);
            //NativeFunction.CallByName<bool>("START_PARTICLE_FX_LOOPED_ON_PED_BONE", FX, Game.LocalPlayer.Character, 0f, 0f, 0f, 0f, 0f, 0f, 57005, 1f, false, false, false);
            //NativeFunction.CallByName<bool>("START_PARTICLE_FX_LOOPED_ON_ENTITY", FX, Smoking.PlayersCurrentCigarette, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 2.0f, false, false, false);
            WriteToLog("DebugNumpad6", "StartLoop");
            int Particle = 0;

                Particle = NativeFunction.CallByName<int>("START_PARTICLE_FX_LOOPED_ON_PED_BONE", FX, Game.LocalPlayer.Character, 0f, 0f, 0f, 0f, 0f, 0f, 57005, 2.0f, false, false, false);
                GameFiber.Sleep(2500);
            
            NativeFunction.CallByName<int>("STOP_PARTICLE_FX_LOOPED", Particle, true);
            WriteToLog("DebugNumpad6", "StopLoop");


            //uint TimeStarted = Game.GameTime;
            //GameFiber.StartNew(delegate
            //{
            //    while (Game.GameTime - TimeStarted <= 5000)
            //    {
            //        NativeFunction.CallByName<bool>("START_PARTICLE_FX_LOOPED_ON_PED_BONE", FX, Game.LocalPlayer.Character, 0f, 0f, 0f, 0f, 0f, 0f, 57005, 2.0f, false, false, false);
            //       // NativeFunction.CallByName<bool>("START_PARTICLE_FX_NON_LOOPED_AT_COORD", "ent_dst_concrete_large", offset.X, offset.Y, offset.Z, 0f, 0f, 0f, 10.0f, false, false, false);
            //        GameFiber.Yield();
            //    }
            //});


            //GTAWeapon CurrentWeapon = Weapons.Where(x => x.Hash == (uint)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash).First();

            //if (CurrentWeapon != null)
            //{
            //    WeaponVariation.WeaponComponent myComponent = WeaponComponentsLookup.Where(x => x.BaseWeapon == CurrentWeapon.Name && x.Name == "Suppressor").FirstOrDefault();
            //    if (myComponent == null)
            //    {
            //        WriteToLog("DebugNumpad6", "No Component Found");
            //        return;
            //    }

            //    WeaponVariation Cool = new WeaponVariation(0);
            //    Cool.Components.Add(myComponent);

            //    ApplyWeaponVariation(Game.LocalPlayer.Character, (uint)CurrentWeapon.Hash, Cool);
            //}

            //WeaponVariation DroppedGunVariation = GetWeaponVariation(Game.LocalPlayer.Character, (uint)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash);
            //foreach (WeaponVariation.WeaponComponent Comp in DroppedGunVariation.Components)
            //{
            //    WriteToLog("GetWeaponVariation", string.Format("Name: {0},HashKey: {1},Hash: {2}", Comp.Name, Comp.HashKey, Comp.Hash));
            //}
            //WriteToLog("GetWeaponVariation", string.Format("Tint: {0}", DroppedGunVariation.Tint));
        }
        catch (Exception e)
        {
            WriteToLog("DebugApplyPoliceVariation", e.Message);
        }
    }
    private static void DebugNumpad7()
    {
        Settings.Logging = true;
        //Settings.Debug = true;
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.CopPed.Exists() && x.CopPed.IsAlive))
        {
            WriteToLog("Debug", string.Format("Cop: {0},Model.Name:{1},isTasked: {2},canSeePlayer: {3},DistanceToPlayer: {4},HurtByPlayer: {5},IssuedHeavyWeapon {6},TaskIsQueued: {7},TaskType: {8},WasRandomSpawn: {9},TaskFiber: {10},CurrentTaskStatus: {11},Agency: {12}",
                    Cop.CopPed.Handle, Cop.CopPed.Model.Name, Cop.isTasked, Cop.canSeePlayer, Cop.DistanceToPlayer, Cop.HurtByPlayer, Cop.IssuedHeavyWeapon, Cop.TaskIsQueued, Cop.TaskType, Cop.WasRandomSpawn, Cop.TaskFiber, Cop.CopPed.Tasks.CurrentTaskStatus, Cop.AssignedAgency.Initials));
        }
    }
    private static void DebugNumpad8()
    {
        if (PlayerCurrentStreet == null)
        {
            WriteToLog("PlayerCurrentStreet", "No STreet");
        }
        else
        {
            WriteToLog("PlayerCurrentStreet", PlayerCurrentStreet.Name);

        }
        
       // Smoking.Start();
    }
    private static void DebugNumpad9()
    {
        Game.DisplayNotification("Instant Action Deactivated");
        Dispose();
    }
    private static void DebugLoop()
    { 
        if (Game.IsKeyDown(Keys.NumPad0))
        {
            DebugNumpad0();
        }
        if (Game.IsKeyDown(Keys.NumPad1))
        {
            DebugNumpad1();
        }
        if (Game.IsKeyDown(Keys.NumPad2))
        {
            DebugNumpad2();
        }
        if (Game.IsKeyDown(Keys.NumPad3))
        {
            DebugNumpad3();
        }
        if (Game.IsKeyDown(Keys.NumPad4))
        {
            DebugNumpad4();
        }
        if (Game.IsKeyDown(Keys.NumPad5))
        {
            DebugNumpad5();
        }
        if (Game.IsKeyDown(Keys.NumPad6))
        {
            DebugNumpad6();
        }
        if (Game.IsKeyDown(Keys.NumPad7))
        {
            DebugNumpad7();
        }
        if (Game.IsKeyDown(Keys.NumPad8))
        {
            DebugNumpad8();
        }
        if (Game.IsKeyDown(Keys.NumPad9))
        {
            DebugNumpad9();
        }


        if (Settings.Debug)
        {
            foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead))
            {
                if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.InProgress)
                    Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Green);
                else if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.Interrupted)
                    Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Purple);
                else if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.None)
                    Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.White);
                else if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask)
                    Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Orange);
                else if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.Preparing)
                    Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Red);
                else if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.Unknown)
                    Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Black);
                else
                    Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Yellow);
            }
            //if (Game.LocalPlayer.WantedLevel > 0)
            //{
            //    Vector3 CurrentWantedLevelPosition = NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
            //    Rage.Debug.DrawArrowDebug(new Vector3(CurrentWantedLevelPosition.X, CurrentWantedLevelPosition.Y, CurrentWantedLevelPosition.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Blue);
            //}
        }

    }


}