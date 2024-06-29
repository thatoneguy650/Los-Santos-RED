using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TrafficViolations
{
    private IViolateable Player;
    private Violations Violations;
    private ISettingsProvideable Settings;
    private ITimeReportable Time;
    private IEntityProvideable World;

    private uint GameTimeStartedDrivingAgainstTraffic;
    private uint GameTimeStartedDrivingOnPavement;
    private uint GameTimeLastRanRed;
    private uint GameTimeStartedSpeeding;
    private uint GameTimeStartedFelonySpeeding;
    private int TimeSincePlayerHitPed;
    private int TimeSincePlayerHitVehicle;
    private bool VehicleIsSuspicious;
    private bool SentRecentCrash = false;
    private bool isRegularSpeeding;
    private bool isFelonySpeeding;
    private bool isDrivingSuspiciously;
    private uint GameTimeLastSentCrash;

    public TrafficViolations(IViolateable player, Violations violations, ISettingsProvideable settings, ITimeReportable time, IEntityProvideable world)
    {
        Player = player;
        Violations = violations;
        Settings = settings;
        Time = time;
        World = world;
    }
    public bool IsRunningRedLight => GameTimeLastRanRed != 0 && Game.GameTime - GameTimeLastRanRed <= Settings.SettingsManager.ViolationSettings.RecentlyRanRedLightTime;
    public bool IsFelonySpeeding { get; set; }
    public bool IsRegularSpeeding { get; set; }
    public bool IsViolatingAnyTrafficLaws => HasBeenDrivingAgainstTraffic || HasBeenDrivingOnPavement || IsRunningRedLight || HasBeenSpeeding || HasBeenFelonySpeeding || VehicleIsSuspicious;
    private bool HasBeenDrivingAgainstTraffic => GameTimeStartedDrivingAgainstTraffic != 0 && Game.GameTime - GameTimeStartedDrivingAgainstTraffic >= Settings.SettingsManager.ViolationSettings.RecentlyDrivingAgainstTrafficTime;
    private bool HasBeenDrivingOnPavement => GameTimeStartedDrivingOnPavement != 0 && Game.GameTime - GameTimeStartedDrivingOnPavement >= Settings.SettingsManager.ViolationSettings.RecentlyDrivingOnPavementTime;
    private bool HasBeenSpeeding => GameTimeStartedSpeeding != 0 && Game.GameTime - GameTimeStartedSpeeding >= Settings.SettingsManager.ViolationSettings.RecentlySpeedingTime;
    private bool HasBeenFelonySpeeding => GameTimeStartedFelonySpeeding != 0 && Game.GameTime - GameTimeStartedFelonySpeeding >= Settings.SettingsManager.ViolationSettings.RecentlySpeedingTime;
    private bool RecentlyHitPed => TimeSincePlayerHitPed > 0 && TimeSincePlayerHitPed <= Settings.SettingsManager.ViolationSettings.RecentlyHitPedTime;
    private bool RecentlyHitVehicle => TimeSincePlayerHitVehicle > 0 && TimeSincePlayerHitVehicle <= Settings.SettingsManager.ViolationSettings.RecentlyHitVehicleTime;
    private bool ShouldCheckTrafficViolations => !Violations.CanIgnoreAllTrafficLaws && Player.IsInVehicle && (Player.IsInAutomobile || Player.IsOnMotorcycle) && !Player.RecentlyStartedPlaying && World.TotalWantedLevel <= 2 ;
    private bool IsFastEnoughToCheckViolations => Player.VehicleSpeedMPH >= Settings.SettingsManager.ViolationSettings.MinTrafficViolationSpeed;
    public void Setup()
    {
    }
    public void Dispose()
    {

    }
    public void Reset()
    {
        IsFelonySpeeding = false;
        IsRegularSpeeding = false;
        VehicleIsSuspicious = false;
        GameTimeLastRanRed = 0;
        TimeSincePlayerHitPed = 0;
        TimeSincePlayerHitVehicle = 0;
        Violations.CrimesViolating.RemoveAll(x => x.IsTrafficViolation);
    }
    public void Update()
    {
        Violations.CrimesViolating.RemoveAll(x => x.IsTrafficViolation);
        VehicleIsSuspicious = false;
        IsFelonySpeeding = false;
        IsRegularSpeeding = false;
        if(!Player.IsAliveAndFree || !Player.ShouldCheckViolations || !ShouldCheckTrafficViolations)
        {
            return;
        }
        CheckTrafficViolations(); 
    }
    private void CheckTrafficViolations()
    {
        isDrivingSuspiciously = false;
        UpdateTrafficStats();
        UpdateVehicleCollisionsViolations();
        UpdateGeneralViolations();
    }
    private void UpdateGeneralViolations()
    {
        if(Player.IsInPoliceVehicle || Player.IsGeneralTrafficLawImmune)
        {
            return;
        }
        if ((HasBeenDrivingAgainstTraffic || Game.LocalPlayer.IsDrivingAgainstTraffic) && IsFastEnoughToCheckViolations)
        {
            isDrivingSuspiciously = true;
            Violations.AddViolating(StaticStrings.DrivingAgainstTrafficCrimeID);
        }
        if ((HasBeenDrivingOnPavement || Game.LocalPlayer.IsDrivingOnPavement) && IsFastEnoughToCheckViolations)
        {
            isDrivingSuspiciously = true;
            Violations.AddViolating(StaticStrings.DrivingOnPavementCrimeID);
        }
        if (VehicleIsSuspicious)
        {
            isDrivingSuspiciously = true;
            Violations.AddViolating(StaticStrings.NonRoadworthyVehicleCrimeID);
        }
        if (HasBeenFelonySpeeding)// IsFelonySpeeding)
        {
            isDrivingSuspiciously = true;
            Violations.AddViolating(StaticStrings.FelonySpeedingCrimeID);
        }
        if (HasBeenSpeeding && IsFastEnoughToCheckViolations) //if (IsRegularSpeeding && IsFastEnoughToCheckViolations)
        {
            Violations.AddViolating(StaticStrings.SpeedingCrimeID);
        }
        if (IsRunningRedLight && IsFastEnoughToCheckViolations)
        {
            isDrivingSuspiciously = true;
            Violations.AddViolating(StaticStrings.RunningARedLightCrimeID);
        }
        if (Player.Intoxication.IsIntoxicated && isDrivingSuspiciously)// DrivingAgainstTraffic.IsCurrentlyViolating || DrivingOnPavement.IsCurrentlyViolating || FelonySpeeding.IsCurrentlyViolating || RunningARedLight.IsCurrentlyViolating || HitPedWithCar.IsCurrentlyViolating || HitCarWithCar.IsCurrentlyViolating))
        {
            Violations.AddViolating(StaticStrings.DrunkDrivingCrimeID);
        }
    }
    private void UpdateVehicleCollisionsViolations()
    {
        if (RecentlyHitPed && IsFastEnoughToCheckViolations)
        {
            isDrivingSuspiciously = true;
            Violations.AddViolating(StaticStrings.HitPedWithCarCrimeID);
        }
        if (RecentlyHitVehicle && IsFastEnoughToCheckViolations)
        {
            isDrivingSuspiciously = true;
            if (Game.GameTime - GameTimeLastSentCrash >= 2000)// !SentRecentCrash)
            {
                //SentRecentCrash = true;
                Player.OnVehicleCrashed();
                GameTimeLastSentCrash = Game.GameTime;
            }
            Violations.AddViolating(StaticStrings.HitCarWithCarCrimeID);
        }
    }
    private void UpdateTrafficStats()
    {
        VehicleIsSuspicious = false;
        IsFelonySpeeding = false;
        if(Player.CurrentVehicle == null || !Player.CurrentVehicle.Vehicle.Exists() || !Player.IsDriver)
        {
            return;
        }
        VehicleStatusUpdate();
        RunningRedUpdate();
        RecklessDrivingUpdate();
        SpeedingUpdate();
    }
    private void SpeedingUpdate()
    {
        float SpeedLimit = 60f;
        if (Player.CurrentLocation.CurrentStreet != null)
        {
            SpeedLimit = Player.CurrentLocation.CurrentStreet.SpeedLimitMPH;
        }
        IsFelonySpeeding = Player.VehicleSpeedMPH > SpeedLimit + Settings.SettingsManager.ViolationSettings.OverLimitFelonySpeedingAmount;
        IsRegularSpeeding = Player.VehicleSpeedMPH > SpeedLimit + Settings.SettingsManager.ViolationSettings.OverLimitSpeedingAmount;
        if (isRegularSpeeding != IsRegularSpeeding)
        {
            if (IsRegularSpeeding)
            {
                GameTimeStartedSpeeding = Game.GameTime;
                //EntryPoint.WriteToConsole("STARTED REGULAR SPEEDING");
            }
            else
            {
                GameTimeStartedSpeeding = 0;
                //EntryPoint.WriteToConsole("ENDED REGULAR SPEEDING");
            }
            isRegularSpeeding = IsRegularSpeeding;
        }

        if (isFelonySpeeding != IsFelonySpeeding)
        {
            if (IsFelonySpeeding)
            {
                GameTimeStartedFelonySpeeding = Game.GameTime;
                //EntryPoint.WriteToConsole("STARTED FELONY SPEEDING");
            }
            else
            {
                GameTimeStartedFelonySpeeding = 0;
                //EntryPoint.WriteToConsole("ENDED FELONY SPEEDING");
            }
            isFelonySpeeding = IsFelonySpeeding;
        }
    }
    private void RecklessDrivingUpdate()
    {
        if (Game.LocalPlayer.IsDrivingOnPavement && GameTimeStartedDrivingOnPavement == 0)
        {
            GameTimeStartedDrivingOnPavement = Game.GameTime;
        }
        else
        {
            GameTimeStartedDrivingOnPavement = 0;
        }
        if (Game.LocalPlayer.IsDrivingAgainstTraffic && GameTimeStartedDrivingAgainstTraffic == 0)
        {
            GameTimeStartedDrivingAgainstTraffic = Game.GameTime;
        }
        else
        {
            GameTimeStartedDrivingAgainstTraffic = 0;
        }
        TimeSincePlayerHitPed = Game.LocalPlayer.TimeSincePlayerLastHitAnyPed;
        TimeSincePlayerHitVehicle = Game.LocalPlayer.TimeSincePlayerLastHitAnyVehicle;
    }
    private void RunningRedUpdate()
    {
        if (Player.CurrentVehicle?.Indicators.LeftBlinkerOn == true || Player.CurrentVehicle?.Indicators.RightBlinkerOn == true)
        {
            return;
        }
        if (NativeFunction.Natives.GET_IS_PLAYER_DRIVING_WRECKLESS<bool>(Game.LocalPlayer, 1))
        {
            GameTimeLastRanRed = Game.GameTime;
        }       
    }
    private void VehicleStatusUpdate()
    {
        if (!Player.CurrentVehicle.IsRoadWorthy(Time) || Player.CurrentVehicle.IsVisiblyDamaged(Time))
        {
            VehicleIsSuspicious = true;
        }
    }
 
}

