using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ExtensionsMethods;
using LSR.Vehicles;
using Rage;
using Rage.Native;

public static class PlayerStateManager
{
    private static bool isGettingIntoVehicle;
    private static bool isInVehicle;
    private static bool isAimingInVehicle;
    private static bool isJacking;
    private static bool areStarsGreyedOut;
    private static uint GameTimeStartedHoldingEnter;
    private static uint GameTimeLastShot;
    private static uint GameTimeLastStartedJacking;
    private static uint GameTimeLastStarsGreyedOut;
    private static uint GameTimeLastStarsNotGreyedOut;
    private static uint GameTimeLastDied;
    private static uint GameTimeLastBusted;
    private static uint GameTimePoliceNoticedVehicleChange;
    private static uint GameTimeLastMoved;
    private static uint PoliceLastSeenVehicleHandle;
    public static bool IsRunning { get; set; }
    public static int TimesDied { get; set; }
    public static bool HandsAreUp { get; set; }
    public static int MaxWantedLastLife { get; set; }
    public static WeaponHash LastWeaponHash { get; set; }
    public static WeaponHash CurrentWeaponHash { get; set; }
    public static bool IsDead { get; private set; }
    public static bool IsBusted { get; private set; }
    public static bool BeingArrested { get; private set; }
    public static bool DiedInVehicle { get; private set; }
    public static bool IsConsideredArmed { get; private set; }
    public static List<VehicleExt> TrackedVehicles { get; private set; }
    public static VehicleExt CurrentVehicle { get; private set; }
    public static WeaponInformation CurrentWeapon { get; private set; }
    public static WeaponCategory CurrentWeaponCategory
    {
        get
        {
            if (CurrentWeapon != null)
                return CurrentWeapon.Category;
            return WeaponCategory.Unknown;
        }
    }

    public static Vector3 CurrentPosition => Game.LocalPlayer.Character.Position;
    public static bool IsJacking
    {
        get => isJacking;
        private set
        {
            if (isJacking != value)
            {
                isJacking = value;
                IsJackingChanged();
            }
        }
    }
    public static bool IsInVehicle
    {
        get => isInVehicle;
        private set
        {
            if (isInVehicle != value)
            {
                isInVehicle = value;
                IsInVehicleChanged();
            }
        }
    }
    public static bool IsInAutomobile { get; private set; }
    public static bool IsOnMotorcycle { get; private set; }
    public static bool IsAimingInVehicle
    {
        get => isAimingInVehicle;
        private set
        {
            if (isAimingInVehicle != value)
            {
                isAimingInVehicle = value;
                IsAimingInVehicleChanged();
            }
        }
    }
    public static bool IsAttemptingToSurrender
    {
        get
        {
            if (HandsAreUp && !WantedLevelManager.IsWeaponsFree)
                return true;
            else
                return false;
        }
    }
    public static bool IsGettingIntoAVehicle
    {
        get => isGettingIntoVehicle;
        private set
        {
            if (isGettingIntoVehicle != value)
            {
                isGettingIntoVehicle = value;
                IsGettingIntoVehicleChanged();
            }
        }
    }
    public static bool AreStarsGreyedOut
    {
        get => areStarsGreyedOut;
        private set
        {
            if (areStarsGreyedOut != value)
            {
                areStarsGreyedOut = value;
                AreStarsGreyedOutChanged();
            }
        }
    }
    public static bool IsNightTime { get; private set; }
    public static bool IsBreakingIntoCar
    {
        get
        {
            if (CarJackingManager.PlayerCarJacking || CarLockPickingManager.PlayerLockPicking)
                return true;
            return false;
        }
    }
    public static bool IsNotWanted => Game.LocalPlayer.WantedLevel == 0;
    public static bool IsWanted => Game.LocalPlayer.WantedLevel > 0;
    public static int WantedLevel => Game.LocalPlayer.WantedLevel;
    public static bool WasJustJacking
    {
        get
        {
            if (GameTimeLastStartedJacking == 0)
                return false;
            return Game.GameTime - GameTimeLastStartedJacking >= 5000;
        }
    }
    public static bool IsStationary
    {
        get
        {
            if (GameTimeLastMoved == 0)
                return true;
            return Game.GameTime - GameTimeLastMoved >= 1500;
        }
    }
    public static bool StarsRecentlyGreyedOut
    {
        get
        {
            if (GameTimeLastStarsGreyedOut == 0)
                return false;
            return Game.GameTime - GameTimeLastStarsGreyedOut <= 1500;
        }
    }
    public static bool StarsRecentlyActive
    {
        get
        {
            if (GameTimeLastStarsNotGreyedOut == 0)
                return false;
            return Game.GameTime - GameTimeLastStarsNotGreyedOut <= 1500;
        }
    }
    public static bool RecentlyDied
    {
        get
        {
            if (GameTimeLastDied == 0)
                return false;
            return Game.GameTime - GameTimeLastDied <= 5000;
        }
    }
    public static bool RecentlyBusted
    {
        get
        {
            if (GameTimeLastBusted == 0)
                return false;
            return Game.GameTime - GameTimeLastBusted <= 5000;
        }
    }
    public static bool IsBustable
    {
        get
        {
            if (WantedLevelManager.HasBeenWantedFor <= 3000)
                return true;
            if (SurrenderManager.IsCommitingSuicide)
                return true;
            if (RecentlyBusted)
                return true;
            return true;
        }
    }
    public static bool PoliceRecentlyNoticedVehicleChange
    {
        get
        {
            if (GameTimePoliceNoticedVehicleChange == 0)
                return false;
            return Game.GameTime - GameTimePoliceNoticedVehicleChange <= 15000;
        }
    }
    public static List<VehicleExt> ReportedStolenVehicles
    {
        get { return TrackedVehicles.Where(x => x.NeedsToBeReportedStolen).ToList(); }
    }
    public static bool IsHoldingEnter
    {
        get
        {
            if (GameTimeStartedHoldingEnter == 0)
                return false;
            if (Game.GameTime - GameTimeStartedHoldingEnter >= 75)
                return true;
            return false;
        }
    }
    public static bool RecentlyShot(int duration)
    {
        if (GameTimeLastShot == 0)
            return false;
        if (PedSwapManager.RecentlyTakenOver)
            return false;
        if (RespawnManager.RecentlyRespawned)
            return false;
        if (Game.GameTime - GameTimeLastShot <= duration) //15000
            return true;
        return false;
    }
    public static void Initialize()
    {
        IsRunning = true;
        IsDead = false;
        IsBusted = false;
        BeingArrested = false;
        DiedInVehicle = false;
        IsConsideredArmed = false;
        TimesDied = 0;
        HandsAreUp = false;
        MaxWantedLastLife = 0;
        LastWeaponHash = 0;
        IsInVehicle = false;
        IsInAutomobile = false;
        IsOnMotorcycle = false;
        IsAimingInVehicle = false;
        IsGettingIntoAVehicle = false;
        IsJacking = false;
        CurrentWeaponHash = 0;
        TrackedVehicles = new List<VehicleExt>();
        GameTimeLastShot = 0;
        GameTimeStartedHoldingEnter = 0;
        PoliceLastSeenVehicleHandle = 0;

        Game.LocalPlayer.Character.CanBePulledOutOfVehicles = true;

        var MyPtr = Game.GetScriptGlobalVariableAddress(4); //the script id for respawn_controller
        Marshal.WriteInt32(MyPtr, 1); //setting it to 1 turns it off somehow?
        Game.TerminateAllScriptsWithName("respawn_controller");
    }
    public static void Dispose()
    {
        IsRunning = false;

        var MyPtr = Game.GetScriptGlobalVariableAddress(4); //the script id for respawn_controller
        Marshal.WriteInt32(MyPtr, 0); //setting it to 0 turns it on somehow?
        Game.StartNewScript("respawn_controller");
        Game.StartNewScript("selector");
    }
    public static void Tick()
    {
        if (IsRunning)
        {
            UpdatePlayer();
            StateTick();
            TurnOffRespawnScripts();
            AudioTick();
            TrackedVehiclesTick();
        }
    }
    private static void UpdatePlayer()
    {
        IsInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
        if (IsInVehicle)
        {
            if (Game.LocalPlayer.Character.IsInAirVehicle || Game.LocalPlayer.Character.IsInSeaVehicle ||
                Game.LocalPlayer.Character.IsOnBike || Game.LocalPlayer.Character.IsInHelicopter)
                IsInAutomobile = false;
            else
                IsInAutomobile = true;

            if (Game.LocalPlayer.Character.IsOnBike)
                IsOnMotorcycle = true;
            else
                IsOnMotorcycle = false;

            CurrentVehicle = UpdateCurrentVehicle();


            if (Extensions.IsMoveControlPressed() || Game.LocalPlayer.Character.Speed >= 0.1f)
                GameTimeLastMoved = Game.GameTime;
        }
        else
        {
            IsOnMotorcycle = false;
            IsInAutomobile = false;
            CurrentVehicle = null;
            if (Game.LocalPlayer.Character.Speed >= 0.2f)
                GameTimeLastMoved = Game.GameTime;
        }

        if (Game.LocalPlayer.Character.IsShooting)
            GameTimeLastShot = Game.GameTime;

        IsGettingIntoAVehicle = Game.LocalPlayer.Character.IsGettingIntoVehicle;
        IsConsideredArmed = Game.LocalPlayer.Character.IsConsideredArmed();
        IsAimingInVehicle = IsInVehicle && Game.LocalPlayer.IsFreeAiming;
        var PlayerCurrentWeapon = Game.LocalPlayer.Character.Inventory.EquippedWeapon;
        CurrentWeapon = General.GetCurrentWeapon(Game.LocalPlayer.Character);

        if (PlayerCurrentWeapon != null)
            CurrentWeaponHash = PlayerCurrentWeapon.Hash;
        else
            CurrentWeaponHash = 0;

        if (CurrentWeaponHash != 0 && PlayerCurrentWeapon.Hash != LastWeaponHash)
            LastWeaponHash = PlayerCurrentWeapon.Hash;

        AreStarsGreyedOut = SearchModeManager.IsInSearchMode;//NativeFunction.CallByName<bool>("ARE_PLAYER_STARS_GREYED_OUT", Game.LocalPlayer);
        IsJacking = Game.LocalPlayer.Character.IsJacking;

        if (Game.IsControlPressed(2, GameControl.Enter))
        {
            if (GameTimeStartedHoldingEnter == 0)
                GameTimeStartedHoldingEnter = Game.GameTime;
        }
        else
        {
            GameTimeStartedHoldingEnter = 0;
        }


        IsNightTime = false;
        var HourOfDay = NativeFunction.CallByName<int>("GET_CLOCK_HOURS");
        var MinuteOfDay = NativeFunction.CallByName<int>("GET_CLOCK_MINUTES");
        if (HourOfDay >= 19 || HourOfDay <= 6)//7pm to 6 am lights need to be on
            IsNightTime = true;
    }
    private static void TurnOffRespawnScripts()
    {
        Game.DisableAutomaticRespawn = true;
        Game.FadeScreenOutOnDeath = false;
        Game.TerminateAllScriptsWithName("selector");
        NativeFunction.Natives.x1E0B4DC0D990A4E7(false);
        NativeFunction.Natives.x21FFB63D8C615361(true);
    }
    private static void StateTick()
    {
        if (Game.LocalPlayer.Character.IsDead && !IsDead)
            DeathEvent();

        if (NativeFunction.CallByName<bool>("IS_PLAYER_BEING_ARRESTED", 0))
            BeingArrested = true;
        if (NativeFunction.CallByName<bool>("IS_PLAYER_BEING_ARRESTED", 1))
        {
            BeingArrested = true;
            Game.LocalPlayer.Character.Tasks.Clear();
        }

        if (BeingArrested && !IsBusted)
            BustedEvent();

        if (WantedLevel > MaxWantedLastLife
        ) // The max wanted level i saw in the last life, not just right before being busted
            MaxWantedLastLife = WantedLevel;
    }
    private static void AudioTick()
    {
        if (SettingsManager.MySettings.Police.DisableAmbientScanner)
            NativeFunction.Natives.xB9EFD5C25018725A("PoliceScannerDisabled", true);
        if (SettingsManager.MySettings.Police.WantedMusicDisable)
            NativeFunction.Natives.xB9EFD5C25018725A("WantedMusicDisabled", true);
    }
    private static void TrackedVehiclesTick()
    {
        TrackedVehicles.RemoveAll(x => !x.VehicleEnt.Exists());
        if (IsInVehicle && Game.LocalPlayer.Character.IsInAnyVehicle(false)
        ) //first check is cheaper, but second is required to verify
        {
            if (CurrentVehicle == null)
                return;

            if (PolicePedManager.AnyCanSeePlayer && IsWanted && !AreStarsGreyedOut)
            {
                if (PoliceLastSeenVehicleHandle != 0 &&
                    PoliceLastSeenVehicleHandle != CurrentVehicle.VehicleEnt.Handle &&
                    !CurrentVehicle.HasBeenDescribedByDispatch)
                {
                    GameTimePoliceNoticedVehicleChange = Game.GameTime;
                    Debugging.WriteToLog("PlayerState",
                        string.Format("PoliceRecentlyNoticedVehicleChange {0}", GameTimePoliceNoticedVehicleChange));
                }

                PoliceLastSeenVehicleHandle = CurrentVehicle.VehicleEnt.Handle;
            }

            if (PolicePedManager.AnyCanRecognizePlayer)
                if (WantedLevel > 0 && !AreStarsGreyedOut)
                    UpdateVehicleDescription(CurrentVehicle);
        }
    }
    private static void BustedEvent()
    {
        DiedInVehicle = IsInVehicle;
        IsBusted = true;
        BeingArrested = true;
        GameTimeLastBusted = Game.GameTime;
       // Game.LocalPlayer.Character.Tasks.Clear();
       // General.TransitionToMediumMo();//was slowmo slowmo lets try this
        HandsAreUp = false;
        SurrenderManager.SetArrestedAnimation(Game.LocalPlayer.Character, false, WantedLevel <= 2);
        var HandleBusted = GameFiber.StartNew(delegate
        {
            GameFiber.Wait(1000);
            MenuManager.ShowBustedMenu();
        }, "HandleBusted");
        Debugging.GameFibers.Add(HandleBusted);
        Game.LocalPlayer.HasControl = false;
    }
    private static void DeathEvent()
    {
        ClockManager.PauseTime();
        DiedInVehicle = IsInVehicle;
        IsDead = true;
        GameTimeLastDied = Game.GameTime;
        Game.LocalPlayer.Character.Kill();
        Game.LocalPlayer.Character.Health = 0;
        Game.LocalPlayer.Character.IsInvincible = true;
        General.TransitionToSlowMo();
        var HandleDeath = GameFiber.StartNew(delegate
        {
            GameFiber.Wait(1000);
            MenuManager.ShowDeathMenu();
        }, "HandleDeath");
        Debugging.GameFibers.Add(HandleDeath);
    }
    private static void IsGettingIntoVehicleChanged()
    {
        if (IsGettingIntoAVehicle)
        {
            var TargetVeh = Game.LocalPlayer.Character.VehicleTryingToEnter;
            var SeatTryingToEnter = Game.LocalPlayer.Character.SeatIndexTryingToEnter;
            General.AttemptLockStatus(TargetVeh, (VehicleLockStatus)7); //Attempt to lock most car doors
            if ((int)TargetVeh.LockStatus == 7) CarLockPickingManager.PickLock(TargetVeh, SeatTryingToEnter);
            if (TargetVeh != null && SeatTryingToEnter == -1)
            {
                var Driver = TargetVeh.Driver;
                if (Driver != null && Driver.IsAlive) CarJackingManager.CarJack(TargetVeh, Driver, SeatTryingToEnter);
            }
        }

        isGettingIntoVehicle = IsGettingIntoAVehicle;
    }
    private static void IsInVehicleChanged()
    {
        if (IsInVehicle) UpdateStolenStatus();
        Debugging.WriteToLog("ValueChecker", string.Format("IsInVehicle Changed to: {0}", IsInVehicle));
    }
    private static void IsAimingInVehicleChanged()
    {
        if (IsAimingInVehicle)
            SetDriverWindow(true);
        else
            SetDriverWindow(false);
        Debugging.WriteToLog("ValueChecker", string.Format("IsAimingInVehicle Changed to: {0}", IsAimingInVehicle));
    }
    private static void IsJackingChanged()
    {
        if (IsJacking) GameTimeLastStartedJacking = Game.GameTime;
        Debugging.WriteToLog("ValueChecker", string.Format("IsJacking Changed to: {0}", IsJacking));
    }
    private static void AreStarsGreyedOutChanged()
    {
        if (AreStarsGreyedOut)
            GameTimeLastStarsGreyedOut = Game.GameTime;
        else
            GameTimeLastStarsNotGreyedOut = Game.GameTime;
        //foreach (Cop Cop in PedList.CopPeds)
        //{
        //    Cop.AtWantedCenterDuringSearchMode = false;
        //}
        Debugging.WriteToLog("ValueChecker", string.Format("AreStarsGreyedOut Changed to: {0}", AreStarsGreyedOut));
    }
    private static void TrackCurrentVehicle()
    {
        var CurrVehicle = Game.LocalPlayer.Character.CurrentVehicle;
        var IsStolen = true;
        if (PedSwapManager.OwnedCar != null && PedSwapManager.OwnedCar.Handle == CurrVehicle.Handle)
            IsStolen = false;

        CurrVehicle.IsStolen = IsStolen;
        var AmStealingCarFromPrerson = IsJacking;
        Ped PreviousOwner;

        if (CurrVehicle.HasDriver && CurrVehicle.Driver.Handle != Game.LocalPlayer.Character.Handle)
            PreviousOwner = CurrVehicle.Driver;
        else
            PreviousOwner = CurrVehicle.GetPreviousPedOnSeat(-1);

        if (PreviousOwner != null && PreviousOwner.DistanceTo2D(Game.LocalPlayer.Character) <= 20f &&
            PreviousOwner.Handle != Game.LocalPlayer.Character.Handle) AmStealingCarFromPrerson = true;
        var MyPlate = new LicensePlate(CurrVehicle.LicensePlate, CurrVehicle.Handle,
            NativeFunction.CallByName<int>("GET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", CurrVehicle), false);
        var MyNewCar = new VehicleExt(CurrVehicle, Game.GameTime, AmStealingCarFromPrerson, CurrVehicle.IsAlarmSounding,
            IsStolen, MyPlate);
        if (IsStolen && PreviousOwner.Exists())
        {
            var MyPrevOwner = PedManager.Civilians.FirstOrDefault(x => x.Pedestrian.Handle == PreviousOwner.Handle);
            if (MyPrevOwner != null) MyPrevOwner.AddCrime(CrimeManager.GrandTheftAuto, MyPrevOwner.Pedestrian.Position);
        }

        TrackedVehicles.Add(MyNewCar);
    }
    private static VehicleExt UpdateCurrentVehicle()
    {
        if (!Game.LocalPlayer.Character.IsInAnyVehicle(false)) return null;

        var CurrVehicle = Game.LocalPlayer.Character.CurrentVehicle;
        var ToReturn = TrackedVehicles.Where(x => x.VehicleEnt.Handle == CurrVehicle.Handle).FirstOrDefault();
        if (ToReturn == null)
        {
            TrackCurrentVehicle();
            return TrackedVehicles.Where(x => x.VehicleEnt.Handle == CurrVehicle.Handle).FirstOrDefault();
        }

        ToReturn.SetAsEntered();
        return ToReturn;
    }
    private static void SetDriverWindow(bool RollDown)
    {
        if (Game.LocalPlayer.Character.CurrentVehicle == null)
            return;

        var DriverWindowIntact =
            NativeFunction.CallByName<bool>("IS_VEHICLE_WINDOW_INTACT", Game.LocalPlayer.Character.CurrentVehicle, 0);
        var MyVehicle = CurrentVehicle;
        if (DriverWindowIntact)
        {
            if (RollDown)
            {
                NativeFunction.CallByName<bool>("ROLL_DOWN_WINDOW", Game.LocalPlayer.Character.CurrentVehicle, 0);
                MyVehicle.ManuallyRolledDriverWindowDown = true;
            }
            else
            {
                MyVehicle.ManuallyRolledDriverWindowDown = false;
                NativeFunction.CallByName<bool>("ROLL_UP_WINDOW", Game.LocalPlayer.Character.CurrentVehicle, 0);
            }
        }
        else
        {
            if (!RollDown)
                if (MyVehicle != null && MyVehicle.ManuallyRolledDriverWindowDown)
                {
                    MyVehicle.ManuallyRolledDriverWindowDown = false;
                    NativeFunction.CallByName<bool>("ROLL_UP_WINDOW", Game.LocalPlayer.Character.CurrentVehicle, 0);
                }
        }
    }
    public static void UpdateStolenStatus()
    {
        var MyVehicle = UpdateCurrentVehicle();
        if (MyVehicle == null || MyVehicle.IsStolen)
            return;

        if (PedSwapManager.OwnedCar == null || MyVehicle.VehicleEnt.Handle != PedSwapManager.OwnedCar.Handle)
            MyVehicle.IsStolen = true;
    }
    public static void SetPlayerToLastWeapon()
    {
        if (Game.LocalPlayer.Character.Inventory.EquippedWeapon != null && LastWeaponHash != 0)
        {
            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)LastWeaponHash,
                true);
            Debugging.WriteToLog("SetPlayerToLastWeapon", LastWeaponHash.ToString());
        }
    }
    public static void DisplayPlayerNotification()
    {
        var NotifcationText = "Warrants: ~g~None~s~";
        if (WantedLevelManager.CurrentCrimes.CommittedAnyCrimes)
            NotifcationText = "Wanted For:" + WantedLevelManager.CurrentCrimes.PrintCrimes();

        var MyCar = UpdateCurrentVehicle();
        if (MyCar != null && !MyCar.IsStolen)
        {
            var Make = MyCar.MakeName();
            var Model = MyCar.ModelName();
            var VehicleName = "";
            if (Make != "")
                VehicleName = Make;
            if (Model != "")
                VehicleName += " " + Model;

            NotifcationText += string.Format("~n~Vehicle: ~p~{0}~s~", VehicleName);
            NotifcationText += string.Format("~n~Plate: ~p~{0}~s~", MyCar.CarPlate.PlateNumber);
        }

        Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~b~Personal Info",
            string.Format("~y~{0}", PedSwapManager.SuspectName), NotifcationText);
    }
    public static void GivePlayerRandomWeapon(WeaponCategory RandomWeaponCategory)
    {
        var myGun = WeaponManager.GetRandomRegularWeapon(RandomWeaponCategory);
        Game.LocalPlayer.Character.Inventory.GiveNewWeapon(myGun.ModelName, myGun.AmmoAmount, true);
    }
    private static void UpdateVehicleDescription(VehicleExt MyVehicle)
    {
        if (MyVehicle.VehicleEnt.Exists())
            MyVehicle.DescriptionColor = MyVehicle.VehicleEnt.PrimaryColor;
        if (MyVehicle.CarPlate != null)
            MyVehicle.CarPlate.IsWanted = true;
        if (MyVehicle.IsStolen && !MyVehicle.WasReportedStolen)
            MyVehicle.WasReportedStolen = true;
    }
    public static void PlayerShotArtificially()
    {
        GameTimeLastShot = Game.GameTime;
    }
    public static void ResetState(bool IncludeMaxWanted)
    {
        IsDead = false;
        IsBusted = false;
        Game.LocalPlayer.HasControl = true;
        BeingArrested = false;
        TimesDied = 0;
        LastWeaponHash = 0;
        if (IncludeMaxWanted)
            MaxWantedLastLife = 0; //this might be a problem in here and might need to be removed
    }
    public static void StartManualArrest()
    {
        BeingArrested = true;
        if (!IsBusted)
            BustedEvent();
    }
}