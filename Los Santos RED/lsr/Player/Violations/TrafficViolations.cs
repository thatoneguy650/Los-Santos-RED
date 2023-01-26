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

    private uint GameTimeStartedSpeeding;
    private uint GameTimeStartedFelonySpeeding;



    private int TimeSincePlayerHitPed;
    private int TimeSincePlayerHitVehicle;
   // private bool TreatAsCop;
    private bool VehicleIsSuspicious;
 

    private bool SentRecentCrash = false;
    private bool isRegularSpeeding;
    private bool isFelonySpeeding;

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
    private bool ShouldCheckTrafficViolations => Player.IsInVehicle && (Player.IsInAutomobile || Player.IsOnMotorcycle) && !Player.RecentlyStartedPlaying;
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
        //IsRunningRedLight = false;
        TimeSincePlayerHitPed = 0;
        TimeSincePlayerHitVehicle = 0;
        Violations.CrimesViolating.RemoveAll(x => x.IsTrafficViolation);
    }
    public void Update()
    {
        Violations.CrimesViolating.RemoveAll(x => x.IsTrafficViolation);
        VehicleIsSuspicious = false;
        //TreatAsCop = false;
        IsFelonySpeeding = false;
        IsRegularSpeeding = false;
        //IsRunningRedLight = false;
        if (Player.IsAliveAndFree && Player.ShouldCheckViolations && ShouldCheckTrafficViolations)
        {
            CheckTrafficViolations();

        }
        //Player.DebugString = $"RedLight {IsRunningRedLight} Speed {IsFelonySpeeding} Sus {VehicleIsSuspicious} Against {HasBeenDrivingAgainstTraffic} Pave {HasBeenDrivingOnPavement}";
    }
    private void CheckTrafficViolations()
    {
        bool isDrivingSuspiciously = false;
        UpdateTrafficStats();
        if (RecentlyHitPed && IsFastEnoughToCheckViolations)
        {
            isDrivingSuspiciously = true;
            Violations.AddViolating(StaticStrings.HitPedWithCarCrimeID);
            EntryPoint.WriteToConsole("Violations HitPedWithCar");
            //GameFiber.Yield();
        }
        if (RecentlyHitVehicle && IsFastEnoughToCheckViolations)
        {
            isDrivingSuspiciously = true;

            if (!SentRecentCrash)
            {
                SentRecentCrash = true;
                Player.OnVehicleCrashed();
            }
            Violations.AddViolating(StaticStrings.HitCarWithCarCrimeID);
        }
        if (!RecentlyHitVehicle)
        {
            SentRecentCrash = false;
        }
        if (!Player.IsInPoliceVehicle)
        {
           // GameFiber.Yield();
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
        }
        if (Player.Intoxication.IsIntoxicated && isDrivingSuspiciously)// DrivingAgainstTraffic.IsCurrentlyViolating || DrivingOnPavement.IsCurrentlyViolating || FelonySpeeding.IsCurrentlyViolating || RunningARedLight.IsCurrentlyViolating || HitPedWithCar.IsCurrentlyViolating || HitCarWithCar.IsCurrentlyViolating))
        {
            Violations.AddViolating(StaticStrings.DrunkDrivingCrimeID);
        }
    }
    private void UpdateTrafficStats()
    {

        VehicleIsSuspicious = false;
        IsFelonySpeeding = false;
        if (Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists() && Player.IsDriver)
        {
            if (!IsRoadWorthy(Player.CurrentVehicle) || IsDamaged(Player.CurrentVehicle))
            {
                VehicleIsSuspicious = true;
            }
            if (Player.CurrentVehicle?.Indicators.LeftBlinkerOn == true || Player.CurrentVehicle?.Indicators.RightBlinkerOn == true)
            {

            }
            else
            {

                if (NativeFunction.Natives.GET_IS_PLAYER_DRIVING_WRECKLESS<bool>(Game.LocalPlayer, 1))
                {
                    GameTimeLastRanRed = Game.GameTime;
                }

                //foreach (PedExt Civilian in World.Pedestrians.Citizens.Where(x => x.Pedestrian.Exists() && x.IsWaitingAtTrafficLight).OrderBy(x => x.DistanceToPlayer))
                //{
                //    if (Civilian.DistanceToPlayer <= 100f && Civilian.IsInVehicle)
                //    {
                //        if (Civilian.Pedestrian.IsInAnyVehicle(false) && Civilian.Pedestrian.CurrentVehicle != null)
                //        {
                //            float HeadingDiff = Math.Abs(Extensions.GetHeadingDifference(Game.LocalPlayer.Character.Heading, Civilian.Pedestrian.Heading));
                //            if ((HeadingDiff <= 35f) && Game.LocalPlayer.Character.IsThisPedInFrontOf(Civilian.Pedestrian) && Civilian.DistanceToPlayer <= 20f && Game.LocalPlayer.Character.Speed >= 3f)
                //            {
                //                GameTimeLastRanRed = Game.GameTime;
                //                //IsRunningRedLight = true;
                //            }
                //            //EntryPoint.WriteToConsole($"{Civilian.DistanceToPlayer} HeadingDiff {HeadingDiff} IsRunningRedLight {IsRunningRedLight}");
                //        }
                //    }
                //}
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

            IsFelonySpeeding = Player.VehicleSpeedMPH > SpeedLimit + Settings.SettingsManager.ViolationSettings.OverLimitFelonySpeedingAmount;
            IsRegularSpeeding = Player.VehicleSpeedMPH > SpeedLimit + Settings.SettingsManager.ViolationSettings.OverLimitSpeedingAmount;

            if(isRegularSpeeding != IsRegularSpeeding)
            {

                if(IsRegularSpeeding)
                {
                    GameTimeStartedSpeeding = Game.GameTime;
                    EntryPoint.WriteToConsole("STARTED REGULAR SPEEDING");
                }
                else
                {
                    GameTimeStartedSpeeding = 0;
                    EntryPoint.WriteToConsole("ENDED REGULAR SPEEDING");
                }
                isRegularSpeeding = IsRegularSpeeding;
            }

            if (isFelonySpeeding != IsFelonySpeeding)
            {
                if (IsFelonySpeeding)
                {
                    GameTimeStartedFelonySpeeding = Game.GameTime;
                    EntryPoint.WriteToConsole("STARTED FELONY SPEEDING");
                }
                else
                {
                    GameTimeStartedFelonySpeeding = 0;
                    EntryPoint.WriteToConsole("ENDED FELONY SPEEDING");
                }
                isFelonySpeeding = IsFelonySpeeding;
            }
        }
    }
    private bool IsDamaged(VehicleExt myCar)
    {
        if (!myCar.Vehicle.Exists())
        {
            return false;
        }

        if (myCar.Vehicle.Health <= Settings.SettingsManager.VehicleSettings.NonRoadworthyVehicleHealthLimit || (myCar.Vehicle.EngineHealth <= Settings.SettingsManager.VehicleSettings.NonRoadworthyEngineHealthLimit && myCar.Engine.IsRunning))//can only see smoke and shit if its running
        {
            return true;
        }
        if (Settings.SettingsManager.VehicleSettings.NonRoadworthyVehicleCheckDamagedWindows && !NativeFunction.CallByName<bool>("ARE_ALL_VEHICLE_WINDOWS_INTACT", myCar.Vehicle))
        {
            return true;
        }
        if (Settings.SettingsManager.VehicleSettings.NonRoadworthyVehicleCheckDamagedDoors)
        {
            foreach (VehicleDoor myDoor in myCar.Vehicle.GetDoors())
            {
                if (myDoor.IsDamaged)
                {
                    return true;
                }
            }
        }
        if (Time.IsNight && Settings.SettingsManager.VehicleSettings.NonRoadworthyVehicleCheckDamagedHeadlights)
        {
            if (myCar.IsCar && NativeFunction.CallByName<bool>("GET_IS_RIGHT_VEHICLE_HEADLIGHT_DAMAGED", myCar.Vehicle) || NativeFunction.CallByName<bool>("GET_IS_LEFT_VEHICLE_HEADLIGHT_DAMAGED", myCar.Vehicle))
            {
                return true;
            }
        }
        if (Settings.SettingsManager.VehicleSettings.NonRoadworthyVehicleCheckDamagedTires)
        {
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
            if (Settings.SettingsManager.VehicleSettings.NonRoadworthyVehicleCheckNoHeadlights && !LightsOn)
            {
                return false;
            }
            //if (HighbeamsOn)
            //{
            //    return false;
            //}
            if (myCar.IsCar && Settings.SettingsManager.VehicleSettings.NonRoadworthyVehicleCheckDamagedHeadlights && NativeFunction.CallByName<bool>("GET_IS_RIGHT_VEHICLE_HEADLIGHT_DAMAGED", myCar.Vehicle) || NativeFunction.CallByName<bool>("GET_IS_LEFT_VEHICLE_HEADLIGHT_DAMAGED", myCar.Vehicle))
            {
                return false;
            }
        }
        if (Settings.SettingsManager.VehicleSettings.NonRoadworthyVehicleCheckNoPlate && myCar.Vehicle.LicensePlate == "        ")
        {
            return false;
        }
        return true;
    }

}

