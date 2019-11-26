﻿using ExtensionsMethods;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class TrafficViolations
{

    private static bool ViolationDrivingAgainstTraffic = false;
    private static bool ViolationDrivingOnPavement = false;
    private static bool ViolationHitPed = false;
    private static bool ViolationHitVehicle = false;
    private static bool ViolationsSpeeding = false;
    private static bool ViolationSpeedLimit = false;
    private static bool ViolationNonRoadworthy = false;
    public static bool ReportedStolenVehicle;
    private static bool PlayerIsRunningRedLight = false;
    private static uint GameTimeStartedDrivingOnPavement = 0;
    private static uint GameTimeStartedDrivingAgainstTraffic = 0;
    public static bool PlayerIsSpeeding = false;
    public static float CurrentSpeedLimit;
    public static bool PlayersVehicleIsSuspicious = false;
    private static uint GameTimeTrafficViolations;
    public static bool HasBeenDrivingAgainstTraffic//seconds
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
    public static bool HasBeenDrivingOnPavement//seconds
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
    public static bool ViolatingTrafficLaws//seconds
    {
        get
        {
            if (HasBeenDrivingAgainstTraffic || HasBeenDrivingOnPavement || PlayerIsRunningRedLight || PlayerIsSpeeding || PlayersVehicleIsSuspicious)
                return true;
            else
                return false;
        }
    }
    public static void Tick()
    {
        if (Game.GameTime > GameTimeTrafficViolations + 500)
        {
            if (Police.CurrentPoliceState != Police.PoliceState.Normal)
            {
                GameTimeStartedDrivingOnPavement = 0;
                GameTimeStartedDrivingAgainstTraffic = 0;
                PlayerIsSpeeding = false;
                return;
            }

            if (!Settings.TrafficViolations)
                return;

            InstantAction.PlayerInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);

            if (!InstantAction.PlayerInVehicle)
            {
                ViolationSpeedLimit = false;
            }

            if (InstantAction.PlayerInVehicle && !PedSwapping.JustTakenOver(5000))
            {
                float VehicleSpeedMPH = Game.LocalPlayer.Character.CurrentVehicle.Speed * 2.23694f;
                Vehicle CurrVehicle = Game.LocalPlayer.Character.CurrentVehicle;
                GTAVehicle MyCar = InstantAction.GetPlayersCurrentTrackedVehicle();
                PlayersVehicleIsSuspicious = false;
                if (!CurrVehicle.IsRoadWorthy() || CurrVehicle.IsDamaged())
                    PlayersVehicleIsSuspicious = true;
                bool TreatAsCop = false;

                if (Settings.TrafficViolationsExemptCode3 && CurrVehicle != null && CurrVehicle.IsPoliceVehicle && MyCar != null && !MyCar.WasReportedStolen)
                {
                    if (CurrVehicle.IsSirenOn && Police.AnyPoliceCanRecognizePlayer && PoliceScanning.CopPeds.Any(x => x.DistanceToPlayer <= 10f)) //see thru ur disguise if ur too close
                    {
                        TreatAsCop = true;//Cops dont have to do traffic laws stuff if ur running code3?
                    }
                }

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

                if (Settings.TrafficViolationsDrivingAgainstTraffic && Police.AnyPoliceCanSeePlayer && !ViolationDrivingAgainstTraffic && !TreatAsCop && (HasBeenDrivingAgainstTraffic || (Game.LocalPlayer.IsDrivingAgainstTraffic && Game.LocalPlayer.Character.CurrentVehicle.Speed >= 10f)))
                {
                    ViolationDrivingAgainstTraffic = true;
                    Police.SetWantedLevel(1);
                    DispatchAudio.DispatchQueueItem RecklessDriver = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportRecklessDriver, 10, false, true, MyCar);
                    RecklessDriver.IsTrafficViolation = true;
                    DispatchAudio.AddDispatchToQueue(RecklessDriver);
                    InstantAction.WriteToLog("TrafficViolationsTick", "ViolationDrivingAgainstTraffic");
                }
                if (Settings.TrafficViolationsDrivingOnPavement && Police.AnyPoliceCanSeePlayer && !ViolationDrivingOnPavement && !TreatAsCop && (HasBeenDrivingOnPavement || (Game.LocalPlayer.IsDrivingOnPavement && Game.LocalPlayer.Character.CurrentVehicle.Speed >= 10f)))
                {
                    ViolationDrivingOnPavement = true;
                    Police.SetWantedLevel(1);
                    DispatchAudio.DispatchQueueItem RecklessDriver = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportRecklessDriver, 10, false, true, MyCar);
                    RecklessDriver.IsTrafficViolation = true;
                    DispatchAudio.AddDispatchToQueue(RecklessDriver);
                    InstantAction.WriteToLog("TrafficViolationsTick", "ViolationDrivingOnPavement");
                }
                int TimeSincePlayerLastHitAnyPed = Game.LocalPlayer.TimeSincePlayerLastHitAnyPed;
                if (Settings.TrafficViolationsHitPed && Police.AnyPoliceCanSeePlayer && !ViolationHitPed && TimeSincePlayerLastHitAnyPed > -1 && TimeSincePlayerLastHitAnyPed <= 1000)
                {
                    ViolationHitPed = true;
                    Police.SetWantedLevel(2);
                    DispatchAudio.DispatchQueueItem PedHitAndRun = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportPedHitAndRun, 8, false, true, MyCar);
                    PedHitAndRun.IsTrafficViolation = true;
                    DispatchAudio.AddDispatchToQueue(PedHitAndRun);
                    InstantAction.WriteToLog("TrafficViolationsTick", "PedHitAndRun");
                }
                int TimeSincePlayerLastHitAnyVehicle = Game.LocalPlayer.TimeSincePlayerLastHitAnyVehicle;
                if (Settings.TrafficViolationsHitVehicle && Police.AnyPoliceCanSeePlayer && !ViolationHitVehicle && TimeSincePlayerLastHitAnyVehicle > -1 && TimeSincePlayerLastHitAnyVehicle <= 1000)
                {
                    ViolationHitVehicle = true;
                    Police.SetWantedLevel(1);
                    DispatchAudio.DispatchQueueItem VehicleHitAndRun = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportVehicleHitAndRun, 9, false, true, MyCar);
                    VehicleHitAndRun.IsTrafficViolation = true;
                    DispatchAudio.AddDispatchToQueue(VehicleHitAndRun);
                    InstantAction.WriteToLog("TrafficViolationsTick", "VehicleHitAndRun");
                }
                if (Settings.TrafficViolationsNotRoadworthy && Police.AnyPoliceCanSeePlayer && !ViolationNonRoadworthy && !TreatAsCop && PlayersVehicleIsSuspicious)
                {
                    ViolationNonRoadworthy = true;
                    Police.SetWantedLevel(1);
                    DispatchAudio.DispatchQueueItem NonRoadWorthy = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspiciousVehicle, 10, false, true, MyCar);
                    NonRoadWorthy.IsTrafficViolation = true;
                    DispatchAudio.AddDispatchToQueue(NonRoadWorthy);
                    InstantAction.WriteToLog("TrafficViolationsTick", "NonRoadWorthy");
                }

                float SpeedLimit = InstantAction.GetSpeedLimit(Game.LocalPlayer.Character.Position);
                bool ViolationSpeedLimit = VehicleSpeedMPH > SpeedLimit + Settings.TrafficViolationsSpeedingOverLimitThreshold;
                PlayerIsSpeeding = ViolationSpeedLimit;
                CurrentSpeedLimit = SpeedLimit;

                if (ViolationSpeedLimit)
                    ViolationSpeedLimit = true;
                else
                    ViolationSpeedLimit = false;

                if (Settings.TrafficViolationsSpeeding && Police.AnyPoliceCanSeePlayer && !ViolationsSpeeding && !TreatAsCop && ViolationSpeedLimit)
                {
                    ViolationsSpeeding = true;
                    if (VehicleSpeedMPH > SpeedLimit + (Settings.TrafficViolationsSpeedingOverLimitThreshold * 1.5))//going 1.5 times the over the threshold = 2 stars
                        Police.SetWantedLevel(2);
                    else
                        Police.SetWantedLevel(1);

                    DispatchAudio.DispatchQueueItem FelonySpeeding = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportFelonySpeeding, 10, false, true, MyCar);
                    FelonySpeeding.Speed = VehicleSpeedMPH;
                    FelonySpeeding.IsTrafficViolation = true;
                    DispatchAudio.AddDispatchToQueue(FelonySpeeding);
                    InstantAction.WriteToLog("TrafficViolationsTick", "Speeding");
                }
            }
            GameTimeTrafficViolations = Game.GameTime;
        }
    }
    public static void ResetTrafficViolations()
    {
        ViolationDrivingOnPavement = false;
        ViolationDrivingAgainstTraffic = false;
        ViolationHitPed = false;
        ViolationsSpeeding = false;
        ViolationHitVehicle = false;
        ViolationNonRoadworthy = false;
    }
}
