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

                Crimes.HitCarWithCar.IsCurrentlyViolating = false;
                Crimes.HitPedWithCar.IsCurrentlyViolating = false;
                Crimes.DrivingOnPavement.IsCurrentlyViolating = false;
                Crimes.DrivingAgainstTraffic.IsCurrentlyViolating = false;
                Crimes.NonRoadworthyVehicle.IsCurrentlyViolating = false;
                Crimes.FelonySpeeding.IsCurrentlyViolating = false;
                return;
            }

            if (PlayerState.IsBusted || PlayerState.IsDead)
                return;

            if (PlayerState.IsInVehicle && Game.LocalPlayer.Character.IsInAnyVehicle(false) && (PlayerState.IsInAutomobile || PlayerState.IsOnMotorcycle) && !PedSwap.JustTakenOver(10000))
            {
                float VehicleSpeedMPH = Game.LocalPlayer.Character.CurrentVehicle.Speed * 2.23694f;
                Vehicle CurrVehicle = Game.LocalPlayer.Character.CurrentVehicle;
                PlayersVehicleIsSuspicious = false;
                if (!CurrVehicle.IsRoadWorthy() || CurrVehicle.IsDamaged())
                    PlayersVehicleIsSuspicious = true;
                bool TreatAsCop = false;
                bool TrafficAnyPoliceCanSeePlayer = PedList.CopPeds.Any(x => x.CanSeePlayer && x.AssignedAgency != null && x.AssignedAgency.CanCheckTrafficViolations);

                if (General.MySettings.TrafficViolations.ExemptCode3 && CurrVehicle != null && CurrVehicle.IsPoliceVehicle && PlayerState.CurrentVehicle != null && !PlayerState.CurrentVehicle.WasReportedStolen)
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
                    Crimes.DrivingAgainstTraffic.IsCurrentlyViolating = true;
                }
                else
                {
                    Crimes.DrivingAgainstTraffic.IsCurrentlyViolating = false;
                }


                if (General.MySettings.TrafficViolations.DrivingOnPavement && !TreatAsCop && (HasBeenDrivingOnPavement || (Game.LocalPlayer.IsDrivingOnPavement && Game.LocalPlayer.Character.CurrentVehicle.Speed >= 10f)))
                {
                    Crimes.DrivingOnPavement.IsCurrentlyViolating = true;
                }
                else
                {
                    Crimes.DrivingOnPavement.IsCurrentlyViolating = false;
                }


                int TimeSincePlayerLastHitAnyPed = Game.LocalPlayer.TimeSincePlayerLastHitAnyPed;
                if (General.MySettings.TrafficViolations.HitPed && TimeSincePlayerLastHitAnyPed > -1 && TimeSincePlayerLastHitAnyPed <= 1000 && (PedList.Civilians.Any(x => x.DistanceToPlayer <= 10f) || PedList.CopPeds.Any(x => x.DistanceToPlayer <= 10f)))//needed for non humans that are returned from this native
                {
                    Crimes.HitPedWithCar.IsCurrentlyViolating = true;
                }
                else
                {
                    Crimes.HitPedWithCar.IsCurrentlyViolating = false;
                }

                int TimeSincePlayerLastHitAnyVehicle = Game.LocalPlayer.TimeSincePlayerLastHitAnyVehicle;
                if (General.MySettings.TrafficViolations.HitVehicle && TimeSincePlayerLastHitAnyVehicle > -1 && TimeSincePlayerLastHitAnyVehicle <= 1000)
                {
                    Crimes.HitCarWithCar.IsCurrentlyViolating = true;
                }
                else
                {
                    Crimes.HitCarWithCar.IsCurrentlyViolating = false;
                }

                if (General.MySettings.TrafficViolations.NotRoadworthy && !TreatAsCop && PlayersVehicleIsSuspicious)
                {
                    Crimes.NonRoadworthyVehicle.IsCurrentlyViolating = true;
                }
                else
                {
                    Crimes.NonRoadworthyVehicle.IsCurrentlyViolating = false;
                }


                if (General.MySettings.TrafficViolations.Speeding)
                {
                    float SpeedLimit = 60f;
                    if (PlayerLocation.PlayerCurrentStreet != null)
                        SpeedLimit = PlayerLocation.PlayerCurrentStreet.SpeedLimit;
                    PlayerIsSpeeding = VehicleSpeedMPH > SpeedLimit + General.MySettings.TrafficViolations.SpeedingOverLimitThreshold;

                    if (PlayerIsSpeeding && !TreatAsCop)
                    {
                        Crimes.FelonySpeeding.IsCurrentlyViolating = true;
                    }
                    else
                    {
                        Crimes.FelonySpeeding.IsCurrentlyViolating = false;
                    }
                }
                else
                {
                    Crimes.FelonySpeeding.IsCurrentlyViolating = false;
                    PlayerIsSpeeding = false;
                }
                //not implemented yet
                //if (LosSantosRED.MySettings.TrafficViolationsRunningRedLight)
                //{
                //    PlayerIsRunningRedLight = false;//CheckRedLight();
                //    if (PlayerIsRunningRedLight && TrafficAnyPoliceCanSeePlayer && Crimes.RunningARedLight.CanObserveCrime && !TreatAsCop)
                //    {
                //        Crimes.RunningARedLight.DispatchToPlay.Speed = VehicleSpeedMPH;
                //        Crimes.RunningARedLight.CrimeObserved();
                //    }
                //}
                //else
                //    PlayerIsRunningRedLight = false;
            }
            else
            {
                Crimes.HitCarWithCar.IsCurrentlyViolating = false;
                Crimes.HitPedWithCar.IsCurrentlyViolating = false;
                Crimes.DrivingOnPavement.IsCurrentlyViolating = false;
                Crimes.DrivingAgainstTraffic.IsCurrentlyViolating = false;
                Crimes.NonRoadworthyVehicle.IsCurrentlyViolating = false;
                Crimes.FelonySpeeding.IsCurrentlyViolating = false;
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
        VehicleExt MyVehicle = PlayerState.CurrentVehicle;
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

