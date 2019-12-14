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

    private static bool ViolationDrivingAgainstTraffic;
    private static bool ViolationDrivingOnPavement;
    private static bool ViolationHitPed;
    private static bool ViolationHitVehicle;
    private static bool ViolationSpeedLimit;
    private static bool ViolationNonRoadworthy;
    private static bool ViolationRunningRed;
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
        ViolationDrivingAgainstTraffic = false;
        ViolationDrivingOnPavement = false;
        ViolationHitPed = false;
        ViolationHitVehicle = false;
        ViolationSpeedLimit = false;
        ViolationNonRoadworthy = false;
        ViolationRunningRed = false;
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
        GameFiber.StartNew(delegate
        {
            try
            {
                while (IsRunning)
                {
                    CheckViolations();
                    GameFiber.Sleep(500);
                }
            }
            catch (Exception e)
            {
                Debugging.WriteToLog("TrafficViolations",e.Message + " : " + e.StackTrace);
            }
        });  
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    private static void CheckViolations()
    {
        if (Police.CurrentPoliceState != Police.PoliceState.Normal || !Settings.TrafficViolations)
        {
            GameTimeStartedDrivingOnPavement = 0;
            GameTimeStartedDrivingAgainstTraffic = 0;
            PlayerIsSpeeding = false;
            PlayerIsRunningRedLight = false;
            PlayersVehicleIsSuspicious = false;
            return;
        }

        InstantAction.PlayerInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
        if (!InstantAction.PlayerInVehicle)
        {
            ViolationSpeedLimit = false;
            ViolationRunningRed = false;
        }
        
        if (InstantAction.PlayerInVehicle && InstantAction.PlayerInAutomobile && !PedSwapping.JustTakenOver(5000))
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


            bool TrafficAnyPoliceCanSeePlayer = PoliceScanning.CopPeds.Any(x => x.canSeePlayer && x.AssignedAgency.CanCheckTrafficViolations);

            if (Settings.TrafficViolationsDrivingAgainstTraffic && TrafficAnyPoliceCanSeePlayer && !ViolationDrivingAgainstTraffic && !TreatAsCop && (HasBeenDrivingAgainstTraffic || (Game.LocalPlayer.IsDrivingAgainstTraffic && Game.LocalPlayer.Character.CurrentVehicle.Speed >= 10f)))
            {
                ViolationDrivingAgainstTraffic = true;
                Police.SetWantedLevel(1,"Driving Against Traffic");
                DispatchAudio.DispatchQueueItem RecklessDriver = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportRecklessDriver, 10, false, true, MyCar)
                {
                    IsTrafficViolation = true
                };
                DispatchAudio.AddDispatchToQueue(RecklessDriver);
            }
            if (Settings.TrafficViolationsDrivingOnPavement && TrafficAnyPoliceCanSeePlayer && !ViolationDrivingOnPavement && !TreatAsCop && (HasBeenDrivingOnPavement || (Game.LocalPlayer.IsDrivingOnPavement && Game.LocalPlayer.Character.CurrentVehicle.Speed >= 10f)))
            {
                ViolationDrivingOnPavement = true;
                Police.SetWantedLevel(1,"Driving On Pavement");
                DispatchAudio.DispatchQueueItem RecklessDriver = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportRecklessDriver, 10, false, true, MyCar)
                {
                    IsTrafficViolation = true
                };
                DispatchAudio.AddDispatchToQueue(RecklessDriver);
            }
            int TimeSincePlayerLastHitAnyPed = Game.LocalPlayer.TimeSincePlayerLastHitAnyPed;
            if (Settings.TrafficViolationsHitPed && TrafficAnyPoliceCanSeePlayer && !ViolationHitPed && TimeSincePlayerLastHitAnyPed > -1 && TimeSincePlayerLastHitAnyPed <= 1000)
            {
                ViolationHitPed = true;
                Police.SetWantedLevel(2,"Hit a Pedestrian");
                DispatchAudio.DispatchQueueItem PedHitAndRun = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportPedHitAndRun, 8, false, true, MyCar)
                {
                    IsTrafficViolation = true
                };
                DispatchAudio.AddDispatchToQueue(PedHitAndRun);
            }
            int TimeSincePlayerLastHitAnyVehicle = Game.LocalPlayer.TimeSincePlayerLastHitAnyVehicle;
            if (Settings.TrafficViolationsHitVehicle && TrafficAnyPoliceCanSeePlayer && !ViolationHitVehicle && TimeSincePlayerLastHitAnyVehicle > -1 && TimeSincePlayerLastHitAnyVehicle <= 1000)
            {
                ViolationHitVehicle = true;
                Police.SetWantedLevel(1,"Hit a vehicle");
                DispatchAudio.DispatchQueueItem VehicleHitAndRun = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportVehicleHitAndRun, 9, false, true, MyCar)
                {
                    IsTrafficViolation = true
                };
                DispatchAudio.AddDispatchToQueue(VehicleHitAndRun);
            }
            if (Settings.TrafficViolationsNotRoadworthy && TrafficAnyPoliceCanSeePlayer && !ViolationNonRoadworthy && !TreatAsCop && PlayersVehicleIsSuspicious)
            {
                ViolationNonRoadworthy = true;
                Police.SetWantedLevel(1,"Driving a non-roadworthy vehicle");
                DispatchAudio.DispatchQueueItem NonRoadWorthy = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspiciousVehicle, 10, false, true, MyCar)
                {
                    IsTrafficViolation = true
                };
                DispatchAudio.AddDispatchToQueue(NonRoadWorthy);
            }
            if (Settings.TrafficViolationsSpeeding)
            {
                float SpeedLimit = 60f;
                if (PlayerLocation.PlayerCurrentStreet != null)
                    SpeedLimit = PlayerLocation.PlayerCurrentStreet.SpeedLimit;
                PlayerIsSpeeding = VehicleSpeedMPH > SpeedLimit + Settings.TrafficViolationsSpeedingOverLimitThreshold;

                if (PlayerIsSpeeding && TrafficAnyPoliceCanSeePlayer && !ViolationSpeedLimit && !TreatAsCop)
                {
                    ViolationSpeedLimit = true;
                    if (VehicleSpeedMPH > SpeedLimit + (Settings.TrafficViolationsSpeedingOverLimitThreshold * 2))//going 2 times over the threshold = 3 stars
                        Police.SetWantedLevel(3, "Going 2 over Speed limit");
                    else if (VehicleSpeedMPH > SpeedLimit + (Settings.TrafficViolationsSpeedingOverLimitThreshold * 1.5))//going 1.5 times over the threshold = 2 stars
                        Police.SetWantedLevel(2, "Going 1.5 over Speed limit");
                    else
                        Police.SetWantedLevel(1, "Going over speed limit");

                    DispatchAudio.DispatchQueueItem FelonySpeeding = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportFelonySpeeding, 10, false, true, MyCar)
                    {
                        Speed = VehicleSpeedMPH,
                        IsTrafficViolation = true
                    };
                    DispatchAudio.AddDispatchToQueue(FelonySpeeding);
                }
            }
            if (Settings.TrafficViolationsRunningRedLight)
            {
                PlayerIsRunningRedLight = CheckRedLight();
                if (PlayerIsRunningRedLight && TrafficAnyPoliceCanSeePlayer && !ViolationRunningRed && !TreatAsCop)
                {
                    ViolationRunningRed = true;
                    Police.SetWantedLevel(1, "Running a Red Light");
                    DispatchAudio.DispatchQueueItem RunningRed = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportRunningRed, 10, false, true, MyCar)
                    {
                        IsTrafficViolation = true
                    };
                    DispatchAudio.AddDispatchToQueue(RunningRed);
                }
            }
            else
                PlayerIsRunningRedLight = false;
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
                        //float ForwardVectorDiff = Extensions.Angle(Vector3.Subtract(MyCar.Position, Game.LocalPlayer.Character.Position), Game.LocalPlayer.Character.ForwardVector);
                        if ((AngleBetween <= 35.0f || AngleBetween >= 155.0f) && !Game.LocalPlayer.Character.CurrentVehicle.IsInFront(MyCar) && Game.LocalPlayer.Character.CurrentVehicle.Speed >= 4.0f)//ForwardVectorDiff > 110f)
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
        GTAVehicle MyVehicle = InstantAction.GetPlayersCurrentTrackedVehicle();
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
    public static void ResetTrafficViolations()
    {
        ViolationDrivingOnPavement = false;
        ViolationDrivingAgainstTraffic = false;
        ViolationHitPed = false;
        ViolationSpeedLimit = false;
        ViolationHitVehicle = false;
        ViolationNonRoadworthy = false;
        ViolationRunningRed = false;
    }
}

