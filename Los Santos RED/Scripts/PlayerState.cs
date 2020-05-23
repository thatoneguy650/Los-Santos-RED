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
    private static bool PrevPlayerIsGettingIntoVehicle;
    private static bool PrevPlayerInVehicle;
    private static bool PrevPlayerAimingInVehicle;
    private static uint GameTimeStartedHoldingEnter;
    public static bool IsRunning { get; set; }
    public static bool IsDead { get; set; }
    public static bool IsBusted { get; set; }
    public static bool BeingArrested { get; set; }
    public static bool DiedInVehicle { get; set; }
    public static bool PlayerIsConsideredArmed { get; set; }
    public static int TimesDied { get; set; }
    public static bool HandsAreUp { get; set; }
    public static int MaxWantedLastLife { get; set; }
    public static WeaponHash LastWeapon { get; set; }
    public static bool PlayerInVehicle { get; set; }
    public static bool PlayerInAutomobile { get; set; }
    public static GTAVehicle PlayersCurrentTrackedVehicle { get; set; }
    public static bool PlayerOnMotorcycle { get; set; }
    public static bool PlayerAimingInVehicle { get; set; }
    public static bool PlayerIsGettingIntoVehicle { get; set; }
    public static WeaponHash PlayerCurrentWeaponHash { get; set; }
    public static List<GTAVehicle> TrackedVehicles { get; set; }

    public static uint GameTimePlayerLastShot { get; set; }
    public static bool PlayerBreakingIntoCar
    {
        get
        {
            if (CarJacking.PlayerCarJacking || LockPicking.PlayerLockPicking)
                return true;
            else
                return false;
        }
    }
    public static bool IsCurrentVehicleTracked
    {
        get
        {
            if (Game.LocalPlayer.Character.IsInAnyVehicle(false))
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
    public static bool PlayerRecentlyShot(int Duration)
    {
        if (GameTimePlayerLastShot == 0)
            return false;
        else if (PedSwapping.JustTakenOver(Duration))
            return false;
        else if (Game.GameTime - GameTimePlayerLastShot <= Duration)//15000
            return true;
        else
            return false;
    }
    public static bool PlayerIsNotWanted
    {
        get
        {
            return Game.LocalPlayer.WantedLevel == 0;
        }
    }
    public static bool PlayerIsWanted
    {
        get
        {
            return Game.LocalPlayer.WantedLevel > 0;
        }
    }
    public static int PlayerWantedLevel
    {
        get
        {
            return Game.LocalPlayer.WantedLevel;
        }
    }
    public static bool PlayerHoldingEnter
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
    public static void Initialize()
    {
        IsRunning = true;
        IsDead = false;
        IsBusted = false;
        BeingArrested = false;
        DiedInVehicle = false;
        PlayerIsConsideredArmed = false;
        TimesDied = 0;
        HandsAreUp = false;
        MaxWantedLastLife = 0;
        LastWeapon = 0;
        PlayerInVehicle = false;
        PlayerInAutomobile = false;
        PlayerOnMotorcycle = false;
        PlayerAimingInVehicle = false;
        PlayerIsGettingIntoVehicle = false;
        PlayerCurrentWeaponHash = 0;
        TrackedVehicles = new List<GTAVehicle>();
        GameTimePlayerLastShot = 0;
        GameTimeStartedHoldingEnter = 0;
        Game.LocalPlayer.Character.CanBePulledOutOfVehicles = true;  
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void Tick()
    {
        if (IsRunning)
        {
            UpdatePlayer();
            StateTick();
            AudioTick();
        }
    }
    private static void UpdatePlayer()
    {
        PlayerInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
        if (PlayerInVehicle)
        {
            if (Game.LocalPlayer.Character.IsInAirVehicle || Game.LocalPlayer.Character.IsInSeaVehicle || Game.LocalPlayer.Character.IsOnBike || Game.LocalPlayer.Character.IsInHelicopter)
                PlayerInAutomobile = false;
            else
                PlayerInAutomobile = true;

            if (Game.LocalPlayer.Character.IsOnBike)
                PlayerOnMotorcycle = true;
            else
                PlayerOnMotorcycle = false;

            PlayersCurrentTrackedVehicle = GetPlayersCurrentTrackedVehicle();
            if (PlayersCurrentTrackedVehicle != null && PlayersCurrentTrackedVehicle.GameTimeEntered == 0)
                PlayersCurrentTrackedVehicle.GameTimeEntered = Game.GameTime;
        }
        else
        {
            PlayerOnMotorcycle = false;
            PlayerInAutomobile = false;
            PlayersCurrentTrackedVehicle = null;
        }

        if (Game.LocalPlayer.Character.IsShooting)
            GameTimePlayerLastShot = Game.GameTime;
        PlayerIsGettingIntoVehicle = Game.LocalPlayer.Character.IsGettingIntoVehicle;
        PlayerIsConsideredArmed = Game.LocalPlayer.Character.IsConsideredArmed();
        PlayerAimingInVehicle = PlayerInVehicle && Game.LocalPlayer.IsFreeAiming;
        WeaponDescriptor PlayerCurrentWeapon = Game.LocalPlayer.Character.Inventory.EquippedWeapon;

        if (PlayerCurrentWeapon != null)
            PlayerCurrentWeaponHash = PlayerCurrentWeapon.Hash;
        else
            PlayerCurrentWeaponHash = 0;

        if (PrevPlayerIsGettingIntoVehicle != PlayerIsGettingIntoVehicle)
            PlayerIsGettingIntoVehicleChanged();

        if (PlayerInVehicle && PlayersCurrentTrackedVehicle == null)//!IsCurrentVehicleTracked)
            TrackCurrentVehicle();

        if (PlayerCurrentWeaponHash != 0 && PlayerCurrentWeapon.Hash != LastWeapon)
            LastWeapon = PlayerCurrentWeapon.Hash;

        if (PrevPlayerAimingInVehicle != PlayerAimingInVehicle)
            PlayerAimingInVehicleChanged();

        if (PrevPlayerInVehicle != PlayerInVehicle)
            PlayerInVehicleChanged();

        if (Game.IsControlPressed(2, GameControl.Enter))
        {
            if (GameTimeStartedHoldingEnter == 0)
                GameTimeStartedHoldingEnter = Game.GameTime;
        }
        else
        {
            GameTimeStartedHoldingEnter = 0;
        }

    }
    private static void StateTick()
    {
        if (Game.LocalPlayer.Character.IsDead && !IsDead)
            PlayerDeathEvent();

        if (NativeFunction.CallByName<bool>("IS_PLAYER_BEING_ARRESTED", 0))
            BeingArrested = true;
        if (NativeFunction.CallByName<bool>("IS_PLAYER_BEING_ARRESTED", 1))
        {
            BeingArrested = true;
            Game.LocalPlayer.Character.Tasks.Clear();
        }

        if (BeingArrested && !IsBusted)
            PlayerBustedEvent();

        if (PlayerWantedLevel > MaxWantedLastLife) // The max wanted level i saw in the last life, not just right before being busted
            MaxWantedLastLife = PlayerWantedLevel;
    }
    private static void AudioTick()
    {
        if (General.MySettings.Police.DisableAmbientScanner)
            NativeFunction.Natives.xB9EFD5C25018725A("PoliceScannerDisabled", true);
        if (General.MySettings.Police.WantedMusicDisable)
            NativeFunction.Natives.xB9EFD5C25018725A("WantedMusicDisabled", true);
    }
    private static void PlayerBustedEvent()
    {
        DiedInVehicle = PlayerInVehicle;
        IsBusted = true;
        BeingArrested = true;
        Game.LocalPlayer.Character.Tasks.Clear();
        General.TransitionToSlowMo();
        HandsAreUp = false;
        Surrendering.SetArrestedAnimation(Game.LocalPlayer.Character, false);
        DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.SuspectArrested, 5));
        GameFiber HandleBusted = GameFiber.StartNew(delegate
        {
            GameFiber.Wait(1000);
            Menus.ShowBustedMenu();
        }, "HandleBusted");
        Debugging.GameFibers.Add(HandleBusted);
    }
    private static void PlayerDeathEvent()
    {
        DiedInVehicle = PlayerInVehicle;
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
    private static void PlayerIsGettingIntoVehicleChanged()
    {
        if (PlayerIsGettingIntoVehicle)
        {
            EnterVehicleEvent();
        }
        PrevPlayerIsGettingIntoVehicle = PlayerIsGettingIntoVehicle;
    }
    private static void PlayerInVehicleChanged()
    {
        if (PlayerInVehicle)
        {
            UpdateStolenStatus();
        }
        PrevPlayerInVehicle = PlayerInVehicle;
        Debugging.WriteToLog("ValueChecker", String.Format("PlayerInVehicle Changed to: {0}", PlayerInVehicle));
    }
    private static void PlayerAimingInVehicleChanged()
    {
        if (PlayerAimingInVehicle)
        {
            TrafficViolations.SetDriverWindow(true);
        }
        else
        {
            TrafficViolations.SetDriverWindow(false);
        }
        PrevPlayerAimingInVehicle = PlayerAimingInVehicle;
        Debugging.WriteToLog("ValueChecker", String.Format("PlayerAimingInVehicle Changed to: {0}", PlayerAimingInVehicle));
    }
    private static void TrackCurrentVehicle()
    {
        Vehicle CurrVehicle = Game.LocalPlayer.Character.CurrentVehicle;
        bool IsStolen = true;
        if (PedSwapping.OwnedCar != null && PedSwapping.OwnedCar.Handle == CurrVehicle.Handle)
            IsStolen = false;

        CurrVehicle.IsStolen = IsStolen;
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
        GTAVehicle MyNewCar = new GTAVehicle(CurrVehicle, Game.GameTime, AmStealingCarFromPrerson, CurrVehicle.IsAlarmSounding, PreviousOwner, IsStolen, MyPlate);
        if (IsStolen && PreviousOwner.Exists())
        {
            GTAPed MyPrevOwner = PedList.Civilians.FirstOrDefault(x => x.Pedestrian.Handle == PreviousOwner.Handle);
            if (MyPrevOwner != null)
            {
                Police.CurrentCrimes.GrandTheftAuto.DispatchToPlay.VehicleToReport = MyNewCar;
                MyPrevOwner.AddCrime(Police.CurrentCrimes.GrandTheftAuto, MyPrevOwner.Pedestrian.Position);
            }
        }
        TrackedVehicles.Add(MyNewCar);
    }
    public static void UpdateStolenStatus()
    {
        GTAVehicle MyVehicle = GetPlayersCurrentTrackedVehicle();
        if (MyVehicle == null || MyVehicle.IsStolen)
            return;

        if (PedSwapping.OwnedCar == null)
            MyVehicle.IsStolen = true;
        else if (MyVehicle.VehicleEnt.Handle != PedSwapping.OwnedCar.Handle && !MyVehicle.IsStolen)
            MyVehicle.IsStolen = true;
    }
    public static void EnterVehicleEvent()
    {
        Vehicle TargetVeh = Game.LocalPlayer.Character.VehicleTryingToEnter;
        int SeatTryingToEnter = Game.LocalPlayer.Character.SeatIndexTryingToEnter;

        General.AttemptLockStatus(TargetVeh, (VehicleLockStatus)7);//Attempt to lock most car doors
        if ((int)TargetVeh.LockStatus == 7)
        {
            LockPicking.PickLock(TargetVeh, SeatTryingToEnter);
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
    public static void SetPlayerToLastWeapon()
    {
        if (Game.LocalPlayer.Character.Inventory.EquippedWeapon != null && LastWeapon != 0)
        {
            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)LastWeapon, true);
            Debugging.WriteToLog("SetPlayerToLastWeapon", LastWeapon.ToString());
        }
    }
    public static void DisplayPlayerNotification()
    {
        string NotifcationText = "Warrants: ~g~None~s~";
        if (Police.CurrentCrimes.CommittedAnyCrimes)
            NotifcationText = "Wanted For:" + Police.CurrentCrimes.PrintCrimes();

        GTAVehicle MyCar = GetPlayersCurrentTrackedVehicle();
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

        Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~b~Personal Info", string.Format("~y~{0}", PedSwapping.SuspectName), NotifcationText);

    }
    public static void GivePlayerRandomWeapon(GTAWeapon.WeaponCategory RandomWeaponCategory)
    {
        GTAWeapon myGun = GTAWeapons.GetRandomWeapon(RandomWeaponCategory);
        Game.LocalPlayer.Character.Inventory.GiveNewWeapon(myGun.Name, myGun.AmmoAmount, true);
    }

}