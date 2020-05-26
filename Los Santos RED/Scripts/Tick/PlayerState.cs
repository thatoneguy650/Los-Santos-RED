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
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Extensions = ExtensionsMethods.Extensions;

public static class PlayerState
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
    private static uint PoliceLastSeenVehicleHandle;
    public static bool IsRunning { get;  set; }
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
    public static bool IsJacking
    {
        get
        {
            return isJacking;
        }
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
        get
        {
            return isInVehicle;
        }
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
        get
        {
            return isAimingInVehicle;
        }
        private set
        {
            if (isAimingInVehicle != value)
            {
                isAimingInVehicle = value;
                IsAimingInVehicleChanged();
            }
        }
    }
    public static bool IsGettingIntoAVehicle
    {
        get
        {
            return isGettingIntoVehicle;
        }
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
        get
        {
            return areStarsGreyedOut;
        }
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
            if (CarJacking.PlayerCarJacking || CarLockPicking.PlayerLockPicking)
                return true;
            else
                return false;
        }
    }
    public static bool IsNotWanted
    {
        get
        {
            return Game.LocalPlayer.WantedLevel == 0;
        }
    }
    public static bool IsWanted
    {
        get
        {
            return Game.LocalPlayer.WantedLevel > 0;
        }
    }
    public static int WantedLevel
    {
        get
        {
            return Game.LocalPlayer.WantedLevel;
        }
    }
    public static bool WasJustJacking
    {
        get
        {
            if (GameTimeLastStartedJacking == 0)
                return false;
            else
                return Game.GameTime - GameTimeLastStartedJacking >= 5000;
        }
    }
    public static bool StarsRecentlyGreyedOut
    {
        get
        {
            if (GameTimeLastStarsGreyedOut == 0)
                return false;
            else
                return Game.GameTime - GameTimeLastStarsGreyedOut <= 1500;
        }
    }
    public static bool IsHoldingEnter
    {
        get
        {
            if (GameTimeStartedHoldingEnter == 0)
                return false;
            else if (Game.GameTime - GameTimeStartedHoldingEnter >= 75)
                return true;
            else
                return false;
        }
    }
    public static bool RecentlyShot(int Duration)
    {
        if (GameTimeLastShot == 0)
            return false;
        else if (PedSwap.JustTakenOver(Duration))
            return false;
        else if (Game.GameTime - GameTimeLastShot <= Duration)//15000
            return true;
        else
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

        IntPtr MyPtr = Game.GetScriptGlobalVariableAddress(4);//the script id for respawn_controller
        Marshal.WriteInt32(MyPtr, 1);//setting it to 1 turns it off somehow?
        Game.TerminateAllScriptsWithName("respawn_controller");
    }
    public static void Dispose()
    {
        IsRunning = false;

        IntPtr MyPtr = Game.GetScriptGlobalVariableAddress(4);//the script id for respawn_controller
        Marshal.WriteInt32(MyPtr, 0);//setting it to 0 turns it on somehow?
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
        }
    }
    private static void UpdatePlayer()
    {
        IsInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
        if (IsInVehicle)
        {
            if (Game.LocalPlayer.Character.IsInAirVehicle || Game.LocalPlayer.Character.IsInSeaVehicle || Game.LocalPlayer.Character.IsOnBike || Game.LocalPlayer.Character.IsInHelicopter)
                IsInAutomobile = false;
            else
                IsInAutomobile = true;

            if (Game.LocalPlayer.Character.IsOnBike)
                IsOnMotorcycle = true;
            else
                IsOnMotorcycle = false;

            CurrentVehicle = UpdateCurrentVehicle();
            if (CurrentVehicle != null && CurrentVehicle.GameTimeEntered == 0)
                CurrentVehicle.GameTimeEntered = Game.GameTime;
        }
        else
        {
            IsOnMotorcycle = false;
            IsInAutomobile = false;
            CurrentVehicle = null;
        }

        if (Game.LocalPlayer.Character.IsShooting)
            GameTimeLastShot = Game.GameTime;

        IsGettingIntoAVehicle = Game.LocalPlayer.Character.IsGettingIntoVehicle;
        IsConsideredArmed = Game.LocalPlayer.Character.IsConsideredArmed();
        IsAimingInVehicle = IsInVehicle && Game.LocalPlayer.IsFreeAiming;
        WeaponDescriptor PlayerCurrentWeapon = Game.LocalPlayer.Character.Inventory.EquippedWeapon;

        if (PlayerCurrentWeapon != null)
            CurrentWeaponHash = PlayerCurrentWeapon.Hash;
        else
            CurrentWeaponHash = 0;

        if (CurrentWeaponHash != 0 && PlayerCurrentWeapon.Hash != LastWeaponHash)
            LastWeaponHash = PlayerCurrentWeapon.Hash;

        AreStarsGreyedOut = NativeFunction.CallByName<bool>("ARE_PLAYER_STARS_GREYED_OUT", Game.LocalPlayer);
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
        int HourOfDay = NativeFunction.CallByName<int>("GET_CLOCK_HOURS");
        int MinuteOfDay = NativeFunction.CallByName<int>("GET_CLOCK_MINUTES");
        if (HourOfDay >= 20 || (HourOfDay >= 19 && MinuteOfDay >= 30) || HourOfDay <= 5)
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

        if (WantedLevel > MaxWantedLastLife) // The max wanted level i saw in the last life, not just right before being busted
            MaxWantedLastLife = WantedLevel;
    }
    private static void AudioTick()
    {
        if (General.MySettings.Police.DisableAmbientScanner)
            NativeFunction.Natives.xB9EFD5C25018725A("PoliceScannerDisabled", true);
        if (General.MySettings.Police.WantedMusicDisable)
            NativeFunction.Natives.xB9EFD5C25018725A("WantedMusicDisabled", true);
    }
    private static void TrackedVehiclesTick()
    {
        TrackedVehicles.RemoveAll(x => !x.VehicleEnt.Exists());
        if (IsNotWanted)
        {
            foreach (VehicleExt StolenCar in TrackedVehicles.Where(x => x.NeedsToBeReportedStolen))
            {
                StolenCar.WasReportedStolen = true;
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.ReportStolenVehicle, 10)
                {
                    ResultsInStolenCarSpotted = true,
                    VehicleToReport = StolenCar
                });
            }
        }
        if (IsInVehicle && Game.LocalPlayer.Character.IsInAnyVehicle(false))//first check is cheaper, but second is required to verify
        {
            if (CurrentVehicle == null)
                return;

            if (Police.AnyCanSeePlayer && IsWanted && !AreStarsGreyedOut)
            {
                if (PoliceLastSeenVehicleHandle != 0 && PoliceLastSeenVehicleHandle != CurrentVehicle.VehicleEnt.Handle && !CurrentVehicle.HasBeenDescribedByDispatch)
                {
                    //GameTimeLastReportedSpotted = Game.GameTime;
                    DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.SuspectChangedVehicle, 21) { IsAmbient = true, VehicleToReport = CurrentVehicle });
                }


                PoliceLastSeenVehicleHandle = CurrentVehicle.VehicleEnt.Handle;
            }
            if (Police.AnyCanRecognizePlayer)
            {
                if (WantedLevel > 0 && !AreStarsGreyedOut)
                    UpdateVehicleDescription(CurrentVehicle);
            }
        }
    }
    private static void BustedEvent()
    {
        DiedInVehicle = IsInVehicle;
        IsBusted = true;
        BeingArrested = true;
        Game.LocalPlayer.Character.Tasks.Clear();
        General.TransitionToSlowMo();
        HandsAreUp = false;
        Surrender.SetArrestedAnimation(Game.LocalPlayer.Character, false);
        DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.SuspectArrested, 5));
        GameFiber HandleBusted = GameFiber.StartNew(delegate
        {
            GameFiber.Wait(1000);
            Menus.ShowBustedMenu();
        }, "HandleBusted");
        Debugging.GameFibers.Add(HandleBusted);
    }
    private static void DeathEvent()
    {
        DiedInVehicle = IsInVehicle;
        IsDead = true;
        Game.LocalPlayer.Character.Kill();
        Game.LocalPlayer.Character.Health = 0;
        Game.LocalPlayer.Character.IsInvincible = true;
        General.TransitionToSlowMo();
        if (Police.PreviousWantedLevel > 0 || PedList.CopPeds.Any(x => x.RecentlySeenPlayer()))
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.SuspectWasted, 5));
        GameFiber HandleDeath = GameFiber.StartNew(delegate
        {
            GameFiber.Wait(1000);
            Menus.ShowDeathMenu();
        }, "HandleDeath");
        Debugging.GameFibers.Add(HandleDeath);
    }
    private static void IsGettingIntoVehicleChanged()
    {
        if (IsGettingIntoAVehicle)
        {
            Vehicle TargetVeh = Game.LocalPlayer.Character.VehicleTryingToEnter;
            int SeatTryingToEnter = Game.LocalPlayer.Character.SeatIndexTryingToEnter;
            General.AttemptLockStatus(TargetVeh, (VehicleLockStatus)7);//Attempt to lock most car doors
            if ((int)TargetVeh.LockStatus == 7)
            {
                CarLockPicking.PickLock(TargetVeh, SeatTryingToEnter);
            }
            if (TargetVeh != null && SeatTryingToEnter == -1)
            {
                Ped Driver = TargetVeh.Driver;
                if (Driver != null && Driver.IsAlive)
                {
                    CarJacking.CarJack(TargetVeh, Driver, SeatTryingToEnter);
                }
            }
        }
        isGettingIntoVehicle = IsGettingIntoAVehicle;
    }
    private static void IsInVehicleChanged()
    {
        if (IsInVehicle)
        {
            UpdateStolenStatus();
        }
        Debugging.WriteToLog("ValueChecker", String.Format("IsInVehicle Changed to: {0}", IsInVehicle));
    }
    private static void IsAimingInVehicleChanged()
    {
        if (IsAimingInVehicle)
        {
            TrafficViolations.SetDriverWindow(true);
        }
        else
        {
            TrafficViolations.SetDriverWindow(false);
        }
        Debugging.WriteToLog("ValueChecker", String.Format("IsAimingInVehicle Changed to: {0}", IsAimingInVehicle));
    }
    private static void IsJackingChanged()
    {
        if (IsJacking)
        {
            GameTimeLastStartedJacking = Game.GameTime;
        }
        Debugging.WriteToLog("ValueChecker", String.Format("IsJacking Changed to: {0}", IsJacking));
    }
    private static void AreStarsGreyedOutChanged()
    {
        if (AreStarsGreyedOut)
        {
            GameTimeLastStarsGreyedOut = Game.GameTime;
        }
        else
        {
            foreach (Cop Cop in PedList.CopPeds)
            {
                Cop.AtWantedCenterDuringSearchMode = false;
            }
        }
        Debugging.WriteToLog("ValueChecker", String.Format("AreStarsGreyedOut Changed to: {0}", AreStarsGreyedOut));
    }
    private static void TrackCurrentVehicle()
    {
        Vehicle CurrVehicle = Game.LocalPlayer.Character.CurrentVehicle;
        bool IsStolen = true;
        if (PedSwap.OwnedCar != null && PedSwap.OwnedCar.Handle == CurrVehicle.Handle)
            IsStolen = false;

        CurrVehicle.IsStolen = IsStolen;
        bool AmStealingCarFromPrerson = IsJacking;
        Ped PreviousOwner;

        if (CurrVehicle.HasDriver && CurrVehicle.Driver.Handle != Game.LocalPlayer.Character.Handle)
            PreviousOwner = CurrVehicle.Driver;
        else
            PreviousOwner = CurrVehicle.GetPreviousPedOnSeat(-1);

        if (PreviousOwner != null && PreviousOwner.DistanceTo2D(Game.LocalPlayer.Character) <= 20f && PreviousOwner.Handle != Game.LocalPlayer.Character.Handle)
        {
            AmStealingCarFromPrerson = true;
        }
        LicensePlate MyPlate = new LicensePlate(CurrVehicle.LicensePlate, (uint)CurrVehicle.Handle, NativeFunction.CallByName<int>("GET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", CurrVehicle), false);
        VehicleExt MyNewCar = new VehicleExt(CurrVehicle, Game.GameTime, AmStealingCarFromPrerson, CurrVehicle.IsAlarmSounding, PreviousOwner, IsStolen, MyPlate);
        if (IsStolen && PreviousOwner.Exists())
        {
            PedExt MyPrevOwner = PedList.Civilians.FirstOrDefault(x => x.Pedestrian.Handle == PreviousOwner.Handle);
            if (MyPrevOwner != null)
            {
                WantedLevelScript.CurrentCrimes.GrandTheftAuto.DispatchToPlay.VehicleToReport = MyNewCar;
                MyPrevOwner.AddCrime(WantedLevelScript.CurrentCrimes.GrandTheftAuto, MyPrevOwner.Pedestrian.Position);
            }
        }
        TrackedVehicles.Add(MyNewCar);
    }
    private static VehicleExt UpdateCurrentVehicle()
    {
        if (!Game.LocalPlayer.Character.IsInAnyVehicle(false))
            return null;
        else
        {
            Vehicle CurrVehicle = Game.LocalPlayer.Character.CurrentVehicle;
            VehicleExt ToReturn = TrackedVehicles.Where(x => x.VehicleEnt.Handle == CurrVehicle.Handle).FirstOrDefault();
            if (ToReturn == null)
            {
                TrackCurrentVehicle();
                return TrackedVehicles.Where(x => x.VehicleEnt.Handle == CurrVehicle.Handle).FirstOrDefault();
            }
            else
            {
                return ToReturn;
            }
        }
    }
    public static void UpdateStolenStatus()
    {
        VehicleExt MyVehicle = UpdateCurrentVehicle();
        if (MyVehicle == null || MyVehicle.IsStolen)
            return;

        if (PedSwap.OwnedCar == null)
            MyVehicle.IsStolen = true;
        else if (MyVehicle.VehicleEnt.Handle != PedSwap.OwnedCar.Handle && !MyVehicle.IsStolen)
            MyVehicle.IsStolen = true;
    }
    public static void SetPlayerToLastWeapon()
    {
        if (Game.LocalPlayer.Character.Inventory.EquippedWeapon != null && LastWeaponHash != 0)
        {
            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)LastWeaponHash, true);
            Debugging.WriteToLog("SetPlayerToLastWeapon", LastWeaponHash.ToString());
        }
    }
    public static void DisplayPlayerNotification()
    {
        string NotifcationText = "Warrants: ~g~None~s~";
        if (WantedLevelScript.CurrentCrimes.CommittedAnyCrimes)
            NotifcationText = "Wanted For:" + WantedLevelScript.CurrentCrimes.PrintCrimes();

        VehicleExt MyCar = UpdateCurrentVehicle();
        if (MyCar != null && !MyCar.IsStolen)
        {
            Vehicles.VehicleInfo VehicleInformation = Vehicles.GetVehicleInfo(MyCar);
            string VehicleName = "";
            if (VehicleInformation != null)
                VehicleName = VehicleInformation.Manufacturer.ToString();
            string DisplayName = DispatchAudio.GetVehicleDisplayName(MyCar.VehicleEnt);
            if (DisplayName != "")
                VehicleName += " " + DisplayName;

            NotifcationText += string.Format("~n~Vehicle: ~p~{0}~s~", VehicleName);
            NotifcationText += string.Format("~n~Plate: ~p~{0}~s~", MyCar.CarPlate.PlateNumber);
        }
        Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~b~Personal Info", string.Format("~y~{0}", PedSwap.SuspectName), NotifcationText);
    }
    public static void GivePlayerRandomWeapon(GTAWeapon.WeaponCategory RandomWeaponCategory)
    {
        GTAWeapon myGun = Weapons.GetRandomRegularWeaponByCategory(RandomWeaponCategory);
        Game.LocalPlayer.Character.Inventory.GiveNewWeapon(myGun.Name, myGun.AmmoAmount, true);
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
        BeingArrested = false;
        TimesDied = 0;
        LastWeaponHash = 0;
        if(IncludeMaxWanted)
            MaxWantedLastLife = 0;//this might be a problem in here and might need to be removed
    }
    public static void StartArrestManual()
    {
        BeingArrested = true;
        if (!IsBusted)
            BustedEvent();
    }
}