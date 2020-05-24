﻿using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class TrafficViolations
{

    private static uint GameTimeStartedDrivingOnPavement;
    private static uint GameTimeStartedDrivingAgainstTraffic;
    private static bool PlayersVehicleIsSuspicious;
    private static List<Vehicle> CloseVehicles;

    public static bool IsRunning { get; set; }
    public static bool PlayerIsSpeeding { get; set; }
    public static bool PlayerIsRunningRedLight { get; set; }
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
        CloseVehicles = new List<Vehicle>();
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
            if (!General.MySettings.TrafficViolations.Enabled)//if (WantedLevel.CurrentPoliceState != WantedLevel.PoliceState.Normal || !LosSantosRED.MySettings.TrafficViolations)
            {
                GameTimeStartedDrivingOnPavement = 0;
                GameTimeStartedDrivingAgainstTraffic = 0;
                PlayerIsSpeeding = false;
                PlayerIsRunningRedLight = false;
                PlayersVehicleIsSuspicious = false;

                WantedLevelScript.CurrentCrimes.HitCarWithCar.IsCurrentlyViolating = false;
                WantedLevelScript.CurrentCrimes.HitPedWithCar.IsCurrentlyViolating = false;
                WantedLevelScript.CurrentCrimes.DrivingOnPavement.IsCurrentlyViolating = false;
                WantedLevelScript.CurrentCrimes.DrivingAgainstTraffic.IsCurrentlyViolating = false;
                WantedLevelScript.CurrentCrimes.NonRoadworthyVehicle.IsCurrentlyViolating = false;
                WantedLevelScript.CurrentCrimes.FelonySpeeding.IsCurrentlyViolating = false;
                return;
            }

            if (PlayerState.IsBusted || PlayerState.IsDead)
                return;

            if (PlayerState.IsInVehicle && Game.LocalPlayer.Character.IsInAnyVehicle(false) && (PlayerState.IsInAutomobile || PlayerState.IsOnMotorcycle) && !PedSwapping.JustTakenOver(10000))
            {
                float VehicleSpeedMPH = Game.LocalPlayer.Character.CurrentVehicle.Speed * 2.23694f;
                Vehicle CurrVehicle = Game.LocalPlayer.Character.CurrentVehicle;
                GTAVehicle MyCar = PlayerState.GetCurrentVehicle();
                PlayersVehicleIsSuspicious = false;
                if (!CurrVehicle.IsRoadWorthy() || CurrVehicle.IsDamaged())
                    PlayersVehicleIsSuspicious = true;
                bool TreatAsCop = false;
                bool TrafficAnyPoliceCanSeePlayer = PedList.CopPeds.Any(x => x.CanSeePlayer && x.AssignedAgency != null && x.AssignedAgency.CanCheckTrafficViolations);

                if (General.MySettings.TrafficViolations.ExemptCode3 && CurrVehicle != null && CurrVehicle.IsPoliceVehicle && MyCar != null && !MyCar.WasReportedStolen)
                {
                    if (CurrVehicle.IsSirenOn && !Police.AnyCanRecognizePlayer) //see thru ur disguise if ur too close
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


                if (General.MySettings.TrafficViolations.DrivingAgainstTraffic && !TreatAsCop && (HasBeenDrivingAgainstTraffic || (Game.LocalPlayer.IsDrivingAgainstTraffic && Game.LocalPlayer.Character.CurrentVehicle.Speed >= 10f)))
                {
                    WantedLevelScript.CurrentCrimes.DrivingAgainstTraffic.IsCurrentlyViolating = true;
                    if (TrafficAnyPoliceCanSeePlayer && WantedLevelScript.CurrentCrimes.DrivingAgainstTraffic.CanObserveCrime)
                    {
                        WantedLevelScript.CurrentCrimes.DrivingAgainstTraffic.DispatchToPlay.VehicleToReport = MyCar;
                        WantedLevelScript.CurrentCrimes.DrivingAgainstTraffic.CrimeObserved();
                    }
                }
                else
                {
                    WantedLevelScript.CurrentCrimes.DrivingAgainstTraffic.IsCurrentlyViolating = false;
                }


                if (General.MySettings.TrafficViolations.DrivingOnPavement && !TreatAsCop && (HasBeenDrivingOnPavement || (Game.LocalPlayer.IsDrivingOnPavement && Game.LocalPlayer.Character.CurrentVehicle.Speed >= 10f)))
                {
                    WantedLevelScript.CurrentCrimes.DrivingOnPavement.IsCurrentlyViolating = true;
                    if (TrafficAnyPoliceCanSeePlayer && WantedLevelScript.CurrentCrimes.DrivingOnPavement.CanObserveCrime)
                    {
                        WantedLevelScript.CurrentCrimes.DrivingOnPavement.DispatchToPlay.VehicleToReport = MyCar;
                        WantedLevelScript.CurrentCrimes.DrivingOnPavement.CrimeObserved();
                    }
                }
                else
                {
                    WantedLevelScript.CurrentCrimes.DrivingOnPavement.IsCurrentlyViolating = false;
                }


                int TimeSincePlayerLastHitAnyPed = Game.LocalPlayer.TimeSincePlayerLastHitAnyPed;
                if (General.MySettings.TrafficViolations.HitPed && TimeSincePlayerLastHitAnyPed > -1 && TimeSincePlayerLastHitAnyPed <= 1000 && (PedList.Civilians.Any(x => x.DistanceToPlayer <= 10f) || PedList.CopPeds.Any(x => x.DistanceToPlayer <= 10f)))//needed for non humans that are returned from this native
                {
                    WantedLevelScript.CurrentCrimes.HitPedWithCar.IsCurrentlyViolating = true;
                    if (TrafficAnyPoliceCanSeePlayer && WantedLevelScript.CurrentCrimes.HitPedWithCar.CanObserveCrime)
                    {
                        WantedLevelScript.CurrentCrimes.HitPedWithCar.DispatchToPlay.VehicleToReport = MyCar;
                        WantedLevelScript.CurrentCrimes.HitPedWithCar.CrimeObserved();
                    }
                }
                else
                {
                    WantedLevelScript.CurrentCrimes.HitPedWithCar.IsCurrentlyViolating = false;
                }

                int TimeSincePlayerLastHitAnyVehicle = Game.LocalPlayer.TimeSincePlayerLastHitAnyVehicle;
                if (General.MySettings.TrafficViolations.HitVehicle && TimeSincePlayerLastHitAnyVehicle > -1 && TimeSincePlayerLastHitAnyVehicle <= 1000)
                {
                    WantedLevelScript.CurrentCrimes.HitCarWithCar.IsCurrentlyViolating = true;
                    if (TrafficAnyPoliceCanSeePlayer && WantedLevelScript.CurrentCrimes.HitCarWithCar.CanObserveCrime)
                    {
                        WantedLevelScript.CurrentCrimes.HitCarWithCar.DispatchToPlay.VehicleToReport = MyCar;
                        WantedLevelScript.CurrentCrimes.HitCarWithCar.CrimeObserved();
                    }
                }
                else
                {
                    WantedLevelScript.CurrentCrimes.HitCarWithCar.IsCurrentlyViolating = false;
                }

                if (General.MySettings.TrafficViolations.NotRoadworthy && !TreatAsCop && PlayersVehicleIsSuspicious)
                {
                    WantedLevelScript.CurrentCrimes.NonRoadworthyVehicle.IsCurrentlyViolating = true;
                    if (TrafficAnyPoliceCanSeePlayer && WantedLevelScript.CurrentCrimes.NonRoadworthyVehicle.CanObserveCrime)
                    {
                        WantedLevelScript.CurrentCrimes.NonRoadworthyVehicle.DispatchToPlay.VehicleToReport = MyCar;
                        WantedLevelScript.CurrentCrimes.NonRoadworthyVehicle.CrimeObserved();
                    }
                }
                else
                {
                    WantedLevelScript.CurrentCrimes.NonRoadworthyVehicle.IsCurrentlyViolating = false;
                }


                if (General.MySettings.TrafficViolations.Speeding)
                {
                    float SpeedLimit = 60f;
                    if (PlayerLocation.PlayerCurrentStreet != null)
                        SpeedLimit = PlayerLocation.PlayerCurrentStreet.SpeedLimit;
                    PlayerIsSpeeding = VehicleSpeedMPH > SpeedLimit + General.MySettings.TrafficViolations.SpeedingOverLimitThreshold;

                    if (PlayerIsSpeeding && !TreatAsCop)
                    {
                        WantedLevelScript.CurrentCrimes.FelonySpeeding.IsCurrentlyViolating = true;
                        if (TrafficAnyPoliceCanSeePlayer && WantedLevelScript.CurrentCrimes.FelonySpeeding.CanObserveCrime)
                        {
                            WantedLevelScript.CurrentCrimes.FelonySpeeding.DispatchToPlay.VehicleToReport = MyCar;
                            WantedLevelScript.CurrentCrimes.FelonySpeeding.DispatchToPlay.Speed = VehicleSpeedMPH;
                            WantedLevelScript.CurrentCrimes.FelonySpeeding.CrimeObserved();
                        }
                    }
                    else
                    {
                        WantedLevelScript.CurrentCrimes.FelonySpeeding.IsCurrentlyViolating = false;
                    }
                }
                else
                {
                    WantedLevelScript.CurrentCrimes.FelonySpeeding.IsCurrentlyViolating = false;
                    PlayerIsSpeeding = false;
                }
                //not implemented yet
                //if (LosSantosRED.MySettings.TrafficViolationsRunningRedLight)
                //{
                //    PlayerIsRunningRedLight = false;//CheckRedLight();
                //    if (PlayerIsRunningRedLight && TrafficAnyPoliceCanSeePlayer && WantedLevelScript.CurrentCrimes.RunningARedLight.CanObserveCrime && !TreatAsCop)
                //    {
                //        WantedLevelScript.CurrentCrimes.RunningARedLight.DispatchToPlay.Speed = VehicleSpeedMPH;
                //        WantedLevelScript.CurrentCrimes.RunningARedLight.CrimeObserved();
                //    }
                //}
                //else
                //    PlayerIsRunningRedLight = false;
            }
            else
            {
                WantedLevelScript.CurrentCrimes.HitCarWithCar.IsCurrentlyViolating = false;
                WantedLevelScript.CurrentCrimes.HitPedWithCar.IsCurrentlyViolating = false;
                WantedLevelScript.CurrentCrimes.DrivingOnPavement.IsCurrentlyViolating = false;
                WantedLevelScript.CurrentCrimes.DrivingAgainstTraffic.IsCurrentlyViolating = false;
                WantedLevelScript.CurrentCrimes.NonRoadworthyVehicle.IsCurrentlyViolating = false;
                WantedLevelScript.CurrentCrimes.FelonySpeeding.IsCurrentlyViolating = false;
            }
        }
    }

    public static bool CheckRedLight()
    {
        if (!Game.LocalPlayer.Character.IsInAnyVehicle(false))
            return false;
        Entity[] MyEnts = World.GetEntities(Game.LocalPlayer.Character.Position, 25f, GetEntitiesFlags.ConsiderGroundVehicles | GetEntitiesFlags.ExcludePlayerVehicle);
        foreach(Vehicle MyCar in MyEnts.Where(x => x.Exists() && x is Vehicle))
        {
            if(!CloseVehicles.Any(x => x.Handle == MyCar.Handle))
            {
                CloseVehicles.Add(MyCar);
            }
        }
        float PlayerZ = Game.LocalPlayer.Character.CurrentVehicle.Position.Z;
        CloseVehicles.RemoveAll(x => !x.Exists() || x.DistanceTo2D(Game.LocalPlayer.Character) >= 35f || !x.Position.Z.IsWithin(PlayerZ - 1.0f,PlayerZ + 1.0f));//Dont care about cars below me stopping at red lights
        if(!CloseVehicles.Any())
        {
            return false;
        }
        else
        {
            foreach (Vehicle MyCar in CloseVehicles.Where(x => x.Exists()))
            {
                if(NativeFunction.CallByName<bool>("IS_VEHICLE_STOPPED_AT_TRAFFIC_LIGHTS", MyCar)) 
                {
                    if (World.GetClosestEntity(MyCar.GetOffsetPositionFront(2f), 1f, GetEntitiesFlags.ConsiderGroundVehicles) == null)// And no car in front of them!!!!
                    {
                        float AngleBetween = Extensions.Angle(MyCar.ForwardVector, Game.LocalPlayer.Character.ForwardVector);
                        float ForwardVectorDiff = Extensions.Angle(Vector3.Subtract(MyCar.Position, Game.LocalPlayer.Character.Position), Game.LocalPlayer.Character.ForwardVector);
                        if ((AngleBetween <= 35.0f || AngleBetween >= 155.0f) && !Game.LocalPlayer.Character.CurrentVehicle.IsInFront(MyCar) && ForwardVectorDiff > 110f)//Game.LocalPlayer.Character.CurrentVehicle.Speed >= 4.0f)//
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;  
    }
    public static void SetDriverWindow(bool RollDown)
    {
        if (Game.LocalPlayer.Character.CurrentVehicle == null)
            return;

        bool DriverWindowIntact = NativeFunction.CallByName<bool>("IS_VEHICLE_WINDOW_INTACT", Game.LocalPlayer.Character.CurrentVehicle, 0);
        GTAVehicle MyVehicle = PlayerState.GetCurrentVehicle();
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
            {
                if (MyVehicle != null && MyVehicle.ManuallyRolledDriverWindowDown)
                {
                    MyVehicle.ManuallyRolledDriverWindowDown = false;
                    NativeFunction.CallByName<bool>("ROLL_UP_WINDOW", Game.LocalPlayer.Character.CurrentVehicle, 0);
                }
            }
        }
    }
}

