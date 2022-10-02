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
   // private bool IsRunningRedLight;
    
    private int TimeSincePlayerHitPed;
    private int TimeSincePlayerHitVehicle;
    private bool TreatAsCop;
    private bool VehicleIsSuspicious;
 

    private bool SentRecentCrash = false;


    public TrafficViolations(IViolateable player, Violations violations, ISettingsProvideable settings, ITimeReportable time, IEntityProvideable world)
    {
        Player = player;
        Violations = violations;
        Settings = settings;
        Time = time;
        World = world;
    }
    public bool IsRunningRedLight => GameTimeLastRanRed != 0 && Game.GameTime - GameTimeLastRanRed <= 2000;
    public bool IsSpeeding { get; set; }
    public bool IsViolatingAnyTrafficLaws => HasBeenDrivingAgainstTraffic || HasBeenDrivingOnPavement || IsRunningRedLight || IsSpeeding || VehicleIsSuspicious;
    private bool HasBeenDrivingAgainstTraffic => GameTimeStartedDrivingAgainstTraffic != 0 && Game.GameTime - GameTimeStartedDrivingAgainstTraffic >= Settings.SettingsManager.ViolationSettings.RecentlyDrivingAgainstTrafficTime;
    private bool HasBeenDrivingOnPavement => GameTimeStartedDrivingOnPavement != 0 && Game.GameTime - GameTimeStartedDrivingOnPavement >= Settings.SettingsManager.ViolationSettings.RecentlyDrivingOnPavementTime;
    private bool RecentlyHitPed => TimeSincePlayerHitPed > 0 && TimeSincePlayerHitPed <= Settings.SettingsManager.ViolationSettings.RecentlyHitPedTime;
    private bool RecentlyHitVehicle => TimeSincePlayerHitVehicle > 0 && TimeSincePlayerHitVehicle <= Settings.SettingsManager.ViolationSettings.RecentlyHitVehicleTime;
    private bool ShouldCheckTrafficViolations => Player.IsInVehicle && (Player.IsInAutomobile || Player.IsOnMotorcycle) && !Player.RecentlyStartedPlaying;
    public void Setup()
    {

    }
    public void Dispose()
    {

    }
    public void Reset()
    {
        IsSpeeding = false;
        VehicleIsSuspicious = false;
        GameTimeLastRanRed = 0;
        //IsRunningRedLight = false;
        TimeSincePlayerHitPed = 0;
        TimeSincePlayerHitVehicle = 0;
        Violations.CrimesViolating.RemoveAll(x => x.IsTrafficViolation);
    }
    public void Update()
    {
        Violations.CrimesViolating.RemoveAll(x => x.IsTrafficViolation);
        VehicleIsSuspicious = false;
        TreatAsCop = false;
        IsSpeeding = false;

        //IsRunningRedLight = false;
        if (Player.IsAliveAndFree && Player.ShouldCheckViolations && ShouldCheckTrafficViolations)
        {
            CheckTrafficViolations();
        }
        Player.DebugString = $"RedLight {IsRunningRedLight} Speed {IsSpeeding} Sus {VehicleIsSuspicious} Against {HasBeenDrivingAgainstTraffic} Pave {HasBeenDrivingOnPavement}";
    }
    private void CheckTrafficViolations()
    {
        bool isDrivingSuspiciously = false;
        UpdateTrafficStats();
        if (RecentlyHitPed && Player.VehicleSpeedMPH >= 20f)
        {
            isDrivingSuspiciously = true;
            Violations.AddViolating("HitPedWithCar");
            EntryPoint.WriteToConsole("Violations HitPedWithCar");
            GameFiber.Yield();
        }
        if (RecentlyHitVehicle && Player.VehicleSpeedMPH >= 20f)
        {
            isDrivingSuspiciously = true;

            if (!SentRecentCrash)
            {
                SentRecentCrash = true;
                Player.OnVehicleCrashed();
            }
            Violations.AddViolating("HitCarWithCar");
        }
        if (!RecentlyHitVehicle)
        {
            SentRecentCrash = false;
        }
        if (!TreatAsCop)
        {
            GameFiber.Yield();
            if ((HasBeenDrivingAgainstTraffic || (Game.LocalPlayer.IsDrivingAgainstTraffic && Player.VehicleSpeedMPH >= 20f)))
            {
                isDrivingSuspiciously = true;
                Violations.AddViolating("DrivingAgainstTraffic");
            }
            if ((HasBeenDrivingOnPavement || (Game.LocalPlayer.IsDrivingOnPavement && Player.VehicleSpeedMPH >= 20f)))
            {
                isDrivingSuspiciously = true;
                Violations.AddViolating("DrivingOnPavement");
            }
            if (VehicleIsSuspicious)
            {
                isDrivingSuspiciously = true;
                Violations.AddViolating("NonRoadworthyVehicle");
            }
            if (IsSpeeding)
            {
                isDrivingSuspiciously = true;
                Violations.AddViolating("FelonySpeeding");
            }
            if (IsRunningRedLight)
            {
                isDrivingSuspiciously = true;
                Violations.AddViolating("RunningARedLight");
            }
        }
        if (Player.Intoxication.IsIntoxicated && isDrivingSuspiciously)// DrivingAgainstTraffic.IsCurrentlyViolating || DrivingOnPavement.IsCurrentlyViolating || FelonySpeeding.IsCurrentlyViolating || RunningARedLight.IsCurrentlyViolating || HitPedWithCar.IsCurrentlyViolating || HitCarWithCar.IsCurrentlyViolating))
        {
            Violations.AddViolating("DrunkDriving");
        }
    }
    private void UpdateTrafficStats()
    {
        //GameFiber.Yield();//TR Yield RemovedTest 1
        VehicleIsSuspicious = false;
        TreatAsCop = false;
        IsSpeeding = false;
        if (Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists() && Player.IsDriver)
        {
            if (!IsRoadWorthy(Player.CurrentVehicle) || IsDamaged(Player.CurrentVehicle))
            {
                //GameFiber.Yield();//TR Yield RemovedTest 1
                VehicleIsSuspicious = true;
            }
            //if (DataMart.Instance.Settings.SettingsManager.TrafficViolations.ExemptCode3 && CurrentPlayer.CurrentVehicle.Vehicle != null && CurrentPlayer.CurrentVehicle.Vehicle.IsPoliceVehicle && CurrentPlayer.CurrentVehicle != null && !CurrentPlayer.CurrentVehicle.WasReportedStolen)
            //{
            //    if (CurrentPlayer.CurrentVehicle.Vehicle.IsSirenOn && !World.AnyPoliceCanRecognizePlayer) //see thru ur disguise if ur too close
            //    {
            //        TreatAsCop = true;//Cops dont have to do traffic laws stuff if ur running code3?
            //    }
            //}
            //IsRunningRedLight = false;

            if (Player.CurrentVehicle?.Indicators.LeftBlinkerOn == true || Player.CurrentVehicle?.Indicators.RightBlinkerOn == true)
            {

            }
            else
            {
                foreach (PedExt Civilian in World.Pedestrians.Citizens.Where(x => x.Pedestrian.Exists() && x.IsWaitingAtTrafficLight).OrderBy(x => x.DistanceToPlayer))
                {
                    if (Civilian.DistanceToPlayer <= 100f && Civilian.IsInVehicle)
                    {
                        if (Civilian.Pedestrian.IsInAnyVehicle(false) && Civilian.Pedestrian.CurrentVehicle != null)
                        {
                            float HeadingDiff = Math.Abs(Extensions.GetHeadingDifference(Game.LocalPlayer.Character.Heading, Civilian.Pedestrian.Heading));
                            if ((HeadingDiff <= 35f) && Game.LocalPlayer.Character.IsThisPedInFrontOf(Civilian.Pedestrian) && Civilian.DistanceToPlayer <= 20f && Game.LocalPlayer.Character.Speed >= 3f)
                            {
                                GameTimeLastRanRed = Game.GameTime;
                                //IsRunningRedLight = true;
                            }
                            //EntryPoint.WriteToConsole($"{Civilian.DistanceToPlayer} HeadingDiff {HeadingDiff} IsRunningRedLight {IsRunningRedLight}");
                        }
                    }
                }
            }
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
            float SpeedLimit = 60f;
            if (Player.CurrentLocation.CurrentStreet != null)
            {
                SpeedLimit = Player.CurrentLocation.CurrentStreet.SpeedLimitMPH;
            }
            IsSpeeding = Player.VehicleSpeedMPH > SpeedLimit + 25f;


            
        }
    }
    private bool IsDamaged(VehicleExt myCar)
    {
        if (!myCar.Vehicle.Exists())
        {
            return false;
        }
        if (myCar.Vehicle.Health <= 300 || (myCar.Vehicle.EngineHealth <= 300 && myCar.Engine.IsRunning))//can only see smoke and shit if its running
        {
            return true;
        }
        if (!NativeFunction.CallByName<bool>("ARE_ALL_VEHICLE_WINDOWS_INTACT", myCar.Vehicle))
        {
            return true;
        }
        foreach (VehicleDoor myDoor in myCar.Vehicle.GetDoors())
        {
            if (myDoor.IsDamaged)
            {
                return true;
            }
        }
        if (Time.IsNight)
        {
            if (myCar.IsCar && NativeFunction.CallByName<bool>("GET_IS_RIGHT_VEHICLE_HEADLIGHT_DAMAGED", myCar.Vehicle) || NativeFunction.CallByName<bool>("GET_IS_LEFT_VEHICLE_HEADLIGHT_DAMAGED", myCar.Vehicle))
            {
                return true;
            }
        }
        if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar.Vehicle, 0, false))
        {
            return true;
        }
        if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar.Vehicle, 1, false))
        {
            return true;
        }
        if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar.Vehicle, 2, false))
        {
            return true;
        }
        if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar.Vehicle, 3, false))
        {
            return true;
        }
        if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar.Vehicle, 4, false))
        {
            return true;
        }
        if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar.Vehicle, 5, false))
        {
            return true;
        }
        return false;
    }
    private bool IsRoadWorthy(VehicleExt myCar)
    {
        bool LightsOn;
        bool HighbeamsOn;
        if (Time.IsNight && myCar.Engine.IsRunning)
        {
            unsafe
            {
                NativeFunction.CallByName<bool>("GET_VEHICLE_LIGHTS_STATE", myCar.Vehicle, &LightsOn, &HighbeamsOn);
            }
            if (!LightsOn)
            {
                return false;
            }
            if (HighbeamsOn)
            {
                return false;
            }
            if (myCar.IsCar && NativeFunction.CallByName<bool>("GET_IS_RIGHT_VEHICLE_HEADLIGHT_DAMAGED", myCar.Vehicle) || NativeFunction.CallByName<bool>("GET_IS_LEFT_VEHICLE_HEADLIGHT_DAMAGED", myCar.Vehicle))
            {
                return false;
            }
        }
        if (myCar.Vehicle.LicensePlate == "        ")
        {
            return false;
        }
        return true;
    }

}

