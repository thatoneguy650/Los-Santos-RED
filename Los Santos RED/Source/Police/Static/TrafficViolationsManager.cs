using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionsMethods;
using LosSantosRED.lsr;

public static class TrafficViolationsManager
{
    private static uint GameTimeLastRanRed;
    private static uint GameTimeStartedDrivingOnPavement;
    private static uint GameTimeStartedDrivingAgainstTraffic;
    private static int TimeSincePlayerHitPed;
    private static int TimeSincePlayerHitVehicle;
    private static bool PlayersVehicleIsSuspicious;
    private static bool TreatAsCop;
    private static float CurrentSpeed;
    public static bool IsRunning { get; set; }
    public static bool PlayerIsSpeeding { get; set; }
    public static bool PlayerIsRunningRedLight { get; set; }
    private static bool ShouldCheckViolations
    {
        get
        {
            if (Mod.Player.IsInVehicle && Game.LocalPlayer.Character.IsInAnyVehicle(false) && (Mod.Player.IsInAutomobile || Mod.Player.IsOnMotorcycle) && !PedSwapManager.RecentlyTakenOver)
                return true;
            else
                return false;
        }
    }
    public static bool RecentlyRanRed
    {
        get
        {
            if (GameTimeLastRanRed == 0)
                return false;
            else if (Game.GameTime - GameTimeLastRanRed <= 1000)
                return true;
            else
                return false;
        }
    }
    public static bool RecentlyHitPed
    {
        get
        {
            if (TimeSincePlayerHitPed > -1 && TimeSincePlayerHitPed <= 1000)
                return true;
            else
                return false;
        }
    }
    public static bool RecentlyHitVehicle
    {
        get
        {
            if (TimeSincePlayerHitVehicle > -1 && TimeSincePlayerHitVehicle <= 1000)
                return true;
            else
                return false;
        }
    }
    public static bool HasBeenDrivingAgainstTraffic
    {
        get
        {
            if (GameTimeStartedDrivingAgainstTraffic == 0)
                return false;
            else if (Game.GameTime - GameTimeStartedDrivingAgainstTraffic >= 1000)
                return true;
            else
                return false;
        }
    }
    public static bool HasBeenDrivingOnPavement
    {
        get
        {
            if (GameTimeStartedDrivingOnPavement == 0)
                return false;
            else if (Game.GameTime - GameTimeStartedDrivingOnPavement >= 1000)
                return true;
            else
                return false;
        }
    }
    public static bool ViolatingTrafficLaws
    {
        get
        {
            if (HasBeenDrivingAgainstTraffic || HasBeenDrivingOnPavement || PlayerIsRunningRedLight || PlayerIsSpeeding || PlayersVehicleIsSuspicious)
                return true;
            else
                return false;
        }
    }
    public static void Initialize()
    {
        GameTimeStartedDrivingOnPavement = 0;
        GameTimeStartedDrivingAgainstTraffic = 0;
        PlayersVehicleIsSuspicious = false;
        IsRunning = true;
        PlayerIsSpeeding = false;
        PlayerIsRunningRedLight = false;
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void Tick()
    {
        if (IsRunning)
        {
            if (!SettingsManager.MySettings.TrafficViolations.Enabled || Mod.Player.IsBusted || Mod.Player.IsDead)
            {
                ResetViolations();
                return;
            }

            if (ShouldCheckViolations)
            {
                UpdateTrafficStats();
                CheckViolations();
            }
            else
            {
                ResetViolations();
            }
        }
    }
    private static void UpdateTrafficStats()
    {
        CurrentSpeed = Game.LocalPlayer.Character.CurrentVehicle.Speed * 2.23694f;
        PlayersVehicleIsSuspicious = false;
        TreatAsCop = false;
        PlayerIsSpeeding = false;

        if (!Mod.Player.CurrentVehicle.VehicleEnt.IsRoadWorthy() || Mod.Player.CurrentVehicle.VehicleEnt.IsDamaged())
            PlayersVehicleIsSuspicious = true;

        if (SettingsManager.MySettings.TrafficViolations.ExemptCode3 && Mod.Player.CurrentVehicle.VehicleEnt != null && Mod.Player.CurrentVehicle.VehicleEnt.IsPoliceVehicle && Mod.Player.CurrentVehicle != null && !Mod.Player.CurrentVehicle.WasReportedStolen)
        {
            if (Mod.Player.CurrentVehicle.VehicleEnt.IsSirenOn && !Mod.PolicePerception.AnyCanRecognizePlayer) //see thru ur disguise if ur too close
            {
                TreatAsCop = true;//Cops dont have to do traffic laws stuff if ur running code3?
            }
        }


        //Streets.ResetStreets();
        PlayerIsRunningRedLight = false;

        foreach (PedExt Civilian in Mod.PedManager.Civilians.Where(x => x.Pedestrian.Exists()).OrderBy(x => x.DistanceToPlayer))
        {
            Civilian.IsWaitingAtTrafficLight = false;
            Civilian.IsFirstWaitingAtTrafficLight = false;
            Civilian.PlaceCheckingInfront = Vector3.Zero;
            if (Civilian.DistanceToPlayer <= 250f && Civilian.IsInVehicle)
            {
                if (Civilian.Pedestrian.IsInAnyVehicle(false) && Civilian.Pedestrian.CurrentVehicle != null)
                {
                    Vehicle PedCar = Civilian.Pedestrian.CurrentVehicle;
                    if (NativeFunction.CallByName<bool>("IS_VEHICLE_STOPPED_AT_TRAFFIC_LIGHTS", PedCar))
                    {
                        Civilian.IsWaitingAtTrafficLight = true;

                        if (Extensions.FacingSameOrOppositeDirection(Civilian.Pedestrian, Game.LocalPlayer.Character) && Game.LocalPlayer.Character.InFront(Civilian.Pedestrian) && Civilian.DistanceToPlayer <= 10f && Game.LocalPlayer.Character.Speed >= 3f)
                        {
                            GameTimeLastRanRed = Game.GameTime;
                            PlayerIsRunningRedLight = true;
                        }
                    }
                }
            }
        }


        // UI.DebugLine = string.Format("PlayerIsRunningRedLight {0}", PlayerIsRunningRedLight);



        if (Game.LocalPlayer.IsDrivingOnPavement)
        {
            if (GameTimeStartedDrivingOnPavement == 0)
                GameTimeStartedDrivingOnPavement = Game.GameTime;
        }
        else
            GameTimeStartedDrivingOnPavement = 0;

        if (Game.LocalPlayer.IsDrivingAgainstTraffic)
        {
            if (GameTimeStartedDrivingAgainstTraffic == 0)
                GameTimeStartedDrivingAgainstTraffic = Game.GameTime;
        }
        else
            GameTimeStartedDrivingAgainstTraffic = 0;


        TimeSincePlayerHitPed = Game.LocalPlayer.TimeSincePlayerLastHitAnyPed;
        TimeSincePlayerHitVehicle = Game.LocalPlayer.TimeSincePlayerLastHitAnyVehicle;

        float SpeedLimit = 60f;
        if (PlayerLocationManager.PlayerCurrentStreet != null)
            SpeedLimit = PlayerLocationManager.PlayerCurrentStreet.SpeedLimit;

        PlayerIsSpeeding = CurrentSpeed > SpeedLimit + SettingsManager.MySettings.TrafficViolations.SpeedingOverLimitThreshold;
    }
    private static void CheckViolations()
    {
        if (SettingsManager.MySettings.TrafficViolations.HitPed && RecentlyHitPed && (PedDamageManager.RecentlyHurtCivilian || PedDamageManager.RecentlyHurtCop) && (Mod.PedManager.Civilians.Any(x => x.DistanceToPlayer <= 10f) || Mod.PedManager.Cops.Any(x => x.DistanceToPlayer <= 10f)))//needed for non humans that are returned from this native
        {
            CrimeManager.HitPedWithCar.IsCurrentlyViolating = true;
        }
        else
        {
            CrimeManager.HitPedWithCar.IsCurrentlyViolating = false;
        }
        if (SettingsManager.MySettings.TrafficViolations.HitVehicle && RecentlyHitVehicle)
        {
            CrimeManager.HitCarWithCar.IsCurrentlyViolating = true;
        }
        else
        {
            CrimeManager.HitCarWithCar.IsCurrentlyViolating = false;
        }
        if (!TreatAsCop)
        {
            if (SettingsManager.MySettings.TrafficViolations.DrivingAgainstTraffic && (HasBeenDrivingAgainstTraffic || (Game.LocalPlayer.IsDrivingAgainstTraffic && Game.LocalPlayer.Character.CurrentVehicle.Speed >= 10f)))
            {
                CrimeManager.DrivingAgainstTraffic.IsCurrentlyViolating = true;
            }
            else
            {
                CrimeManager.DrivingAgainstTraffic.IsCurrentlyViolating = false;
            }
            if (SettingsManager.MySettings.TrafficViolations.DrivingOnPavement && (HasBeenDrivingOnPavement || (Game.LocalPlayer.IsDrivingOnPavement && Game.LocalPlayer.Character.CurrentVehicle.Speed >= 10f)))
            {
                CrimeManager.DrivingOnPavement.IsCurrentlyViolating = true;
            }
            else
            {
                CrimeManager.DrivingOnPavement.IsCurrentlyViolating = false;
            }

            if (SettingsManager.MySettings.TrafficViolations.NotRoadworthy && PlayersVehicleIsSuspicious)
            {
                CrimeManager.NonRoadworthyVehicle.IsCurrentlyViolating = true;
            }
            else
            {
                CrimeManager.NonRoadworthyVehicle.IsCurrentlyViolating = false;
            }
            if (SettingsManager.MySettings.TrafficViolations.Speeding && PlayerIsSpeeding)
            {
                CrimeManager.FelonySpeeding.IsCurrentlyViolating = true;
            }
            else
            {
                CrimeManager.FelonySpeeding.IsCurrentlyViolating = false;
            }
            if (SettingsManager.MySettings.TrafficViolations.RunningRedLight && RecentlyRanRed)
            {
                // Crimes.RunningARedLight.IsCurrentlyViolating = true;//turned off for now until i fix it
            }
            else
            {
                CrimeManager.RunningARedLight.IsCurrentlyViolating = false;
            }
        }

    }
    private static void ResetViolations()
    {
        GameTimeStartedDrivingOnPavement = 0;
        GameTimeStartedDrivingAgainstTraffic = 0;

        TreatAsCop = false;
        PlayerIsSpeeding = false;
        PlayerIsRunningRedLight = false;
        PlayersVehicleIsSuspicious = false;
        CurrentSpeed = 0f;

        CrimeManager.HitCarWithCar.IsCurrentlyViolating = false;
        CrimeManager.HitPedWithCar.IsCurrentlyViolating = false;
        CrimeManager.DrivingOnPavement.IsCurrentlyViolating = false;
        CrimeManager.DrivingAgainstTraffic.IsCurrentlyViolating = false;
        CrimeManager.NonRoadworthyVehicle.IsCurrentlyViolating = false;
        CrimeManager.FelonySpeeding.IsCurrentlyViolating = false;
    }
}