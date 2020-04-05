using ExtensionsMethods;
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

    public static List<Vehicle> CloseVehicles;//tmp public

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
        MainLoop();
    }
    private static void MainLoop()
    {
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void CheckViolations()
    {
        if (!Settings.TrafficViolations)//if (Police.CurrentPoliceState != Police.PoliceState.Normal || !Settings.TrafficViolations)
        {
            GameTimeStartedDrivingOnPavement = 0;
            GameTimeStartedDrivingAgainstTraffic = 0;
            PlayerIsSpeeding = false;
            PlayerIsRunningRedLight = false;
            PlayersVehicleIsSuspicious = false;

            Police.CurrentCrimes.HitCarWithCar.IsCurrentlyViolating = false;
            Police.CurrentCrimes.HitPedWithCar.IsCurrentlyViolating = false;
            Police.CurrentCrimes.DrivingOnPavement.IsCurrentlyViolating = false;
            Police.CurrentCrimes.DrivingAgainstTraffic.IsCurrentlyViolating = false;
            Police.CurrentCrimes.NonRoadworthyVehicle.IsCurrentlyViolating = false;
            Police.CurrentCrimes.FelonySpeeding.IsCurrentlyViolating = false;
            return;
        }

        if (LosSantosRED.IsBusted || LosSantosRED.IsDead)
            return;

        if (LosSantosRED.PlayerInVehicle && Game.LocalPlayer.Character.IsInAnyVehicle(false) && (LosSantosRED.PlayerInAutomobile || LosSantosRED.PlayerOnMotorcycle) && !PedSwapping.JustTakenOver(10000))
        {
            float VehicleSpeedMPH = Game.LocalPlayer.Character.CurrentVehicle.Speed * 2.23694f;
            Vehicle CurrVehicle = Game.LocalPlayer.Character.CurrentVehicle;
            GTAVehicle MyCar = LosSantosRED.GetPlayersCurrentTrackedVehicle();
            PlayersVehicleIsSuspicious = false;
            if (!CurrVehicle.IsRoadWorthy() || CurrVehicle.IsDamaged())
                PlayersVehicleIsSuspicious = true;
            bool TreatAsCop = false;
            bool TrafficAnyPoliceCanSeePlayer = PedScanning.CopPeds.Any(x => x.CanSeePlayer && x.AssignedAgency.CanCheckTrafficViolations);

            if (Settings.TrafficViolationsExemptCode3 && CurrVehicle != null && CurrVehicle.IsPoliceVehicle && MyCar != null && !MyCar.WasReportedStolen)
            {
                if (CurrVehicle.IsSirenOn && !Police.AnyPoliceCanRecognizePlayer) //see thru ur disguise if ur too close
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


            if (Settings.TrafficViolationsDrivingAgainstTraffic && !TreatAsCop && (HasBeenDrivingAgainstTraffic || (Game.LocalPlayer.IsDrivingAgainstTraffic && Game.LocalPlayer.Character.CurrentVehicle.Speed >= 10f)))
            {
                Police.CurrentCrimes.DrivingAgainstTraffic.IsCurrentlyViolating = true;
                if (TrafficAnyPoliceCanSeePlayer && Police.CurrentCrimes.DrivingAgainstTraffic.CanObserveCrime)
                {
                    Police.CurrentCrimes.DrivingAgainstTraffic.DispatchToPlay.VehicleToReport = MyCar;
                    Police.CurrentCrimes.DrivingAgainstTraffic.CrimeObserved();
                }
            }
            else
            {
                Police.CurrentCrimes.DrivingAgainstTraffic.IsCurrentlyViolating = false;
            }


            if (Settings.TrafficViolationsDrivingOnPavement && !TreatAsCop && (HasBeenDrivingOnPavement || (Game.LocalPlayer.IsDrivingOnPavement && Game.LocalPlayer.Character.CurrentVehicle.Speed >= 10f)))
            {
                Police.CurrentCrimes.DrivingOnPavement.IsCurrentlyViolating = true;
                if (TrafficAnyPoliceCanSeePlayer && Police.CurrentCrimes.DrivingOnPavement.CanObserveCrime)
                {
                    Police.CurrentCrimes.DrivingOnPavement.DispatchToPlay.VehicleToReport = MyCar;
                    Police.CurrentCrimes.DrivingOnPavement.CrimeObserved();
                }
            }
            else
            {
                Police.CurrentCrimes.DrivingOnPavement.IsCurrentlyViolating = false;
            }


            int TimeSincePlayerLastHitAnyPed = Game.LocalPlayer.TimeSincePlayerLastHitAnyPed;
            if (Settings.TrafficViolationsHitPed && TimeSincePlayerLastHitAnyPed > -1 && TimeSincePlayerLastHitAnyPed <= 1000)
            {
                Police.CurrentCrimes.HitPedWithCar.IsCurrentlyViolating = true;
                if (TrafficAnyPoliceCanSeePlayer && Police.CurrentCrimes.HitPedWithCar.CanObserveCrime)
                {
                    Police.CurrentCrimes.HitPedWithCar.DispatchToPlay.VehicleToReport = MyCar;
                    Police.CurrentCrimes.HitPedWithCar.CrimeObserved();
                }
            }
            else
            {
                Police.CurrentCrimes.HitPedWithCar.IsCurrentlyViolating = false;
            }

            int TimeSincePlayerLastHitAnyVehicle = Game.LocalPlayer.TimeSincePlayerLastHitAnyVehicle;
            if (Settings.TrafficViolationsHitVehicle && TimeSincePlayerLastHitAnyVehicle > -1 && TimeSincePlayerLastHitAnyVehicle <= 1000 && PedScanning.Civilians.Any(x => x.DistanceToPlayer <= 10f))//otherwise we get non humans
            {
                Police.CurrentCrimes.HitCarWithCar.IsCurrentlyViolating = true;
                if (TrafficAnyPoliceCanSeePlayer && Police.CurrentCrimes.HitCarWithCar.CanObserveCrime)
                {
                    Police.CurrentCrimes.HitCarWithCar.DispatchToPlay.VehicleToReport = MyCar;
                    Police.CurrentCrimes.HitCarWithCar.CrimeObserved();
                }
            }
            else
            {
                Police.CurrentCrimes.HitCarWithCar.IsCurrentlyViolating = false;
            }

            if (Settings.TrafficViolationsNotRoadworthy && !TreatAsCop && PlayersVehicleIsSuspicious)
            {
                Police.CurrentCrimes.NonRoadworthyVehicle.IsCurrentlyViolating = true;
                if (TrafficAnyPoliceCanSeePlayer && Police.CurrentCrimes.NonRoadworthyVehicle.CanObserveCrime)
                {
                    Police.CurrentCrimes.NonRoadworthyVehicle.DispatchToPlay.VehicleToReport = MyCar;
                    Police.CurrentCrimes.NonRoadworthyVehicle.CrimeObserved();
                }
            }
            else
            {
                Police.CurrentCrimes.NonRoadworthyVehicle.IsCurrentlyViolating = false;
            }


            if (Settings.TrafficViolationsSpeeding)
            {
                float SpeedLimit = 60f;
                if (PlayerLocation.PlayerCurrentStreet != null)
                    SpeedLimit = PlayerLocation.PlayerCurrentStreet.SpeedLimit;
                PlayerIsSpeeding = VehicleSpeedMPH > SpeedLimit + Settings.TrafficViolationsSpeedingOverLimitThreshold;

                if (PlayerIsSpeeding && !TreatAsCop)
                {
                    Police.CurrentCrimes.FelonySpeeding.IsCurrentlyViolating = true;
                    if (TrafficAnyPoliceCanSeePlayer && Police.CurrentCrimes.FelonySpeeding.CanObserveCrime)
                    {
                        Police.CurrentCrimes.FelonySpeeding.DispatchToPlay.VehicleToReport = MyCar;
                        Police.CurrentCrimes.FelonySpeeding.DispatchToPlay.Speed = VehicleSpeedMPH;
                        Police.CurrentCrimes.FelonySpeeding.CrimeObserved();
                    }
                }
                else
                {
                    Police.CurrentCrimes.FelonySpeeding.IsCurrentlyViolating = false;
                }
            }
            else
            {
                Police.CurrentCrimes.FelonySpeeding.IsCurrentlyViolating = false;
                PlayerIsSpeeding = false;
            }
            //not implemented yet
            //if (Settings.TrafficViolationsRunningRedLight)
            //{
            //    PlayerIsRunningRedLight = false;//CheckRedLight();
            //    if (PlayerIsRunningRedLight && TrafficAnyPoliceCanSeePlayer && Police.CurrentCrimes.RunningARedLight.CanObserveCrime && !TreatAsCop)
            //    {
            //        Police.CurrentCrimes.RunningARedLight.DispatchToPlay.Speed = VehicleSpeedMPH;
            //        Police.CurrentCrimes.RunningARedLight.CrimeObserved();
            //    }
            //}
            //else
            //    PlayerIsRunningRedLight = false;
        }
        else
        {
            Police.CurrentCrimes.HitCarWithCar.IsCurrentlyViolating = false;
            Police.CurrentCrimes.HitPedWithCar.IsCurrentlyViolating = false;
            Police.CurrentCrimes.DrivingOnPavement.IsCurrentlyViolating = false;
            Police.CurrentCrimes.DrivingAgainstTraffic.IsCurrentlyViolating = false;
            Police.CurrentCrimes.NonRoadworthyVehicle.IsCurrentlyViolating = false;
            Police.CurrentCrimes.FelonySpeeding.IsCurrentlyViolating = false;
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
        GTAVehicle MyVehicle = LosSantosRED.GetPlayersCurrentTrackedVehicle();
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

