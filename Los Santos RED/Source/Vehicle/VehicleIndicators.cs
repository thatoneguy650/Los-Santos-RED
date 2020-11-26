using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public static class VehicleIndicators
{
    private static bool LeftBlinkerStartedTurn;
    private static bool RightBlinkerStartedTurn;
    private static int TimeWheelsTurnedRight;
    private static int TimeWheelsTurnedLeft;
    private static int TimeWheelsStraight;
    public static bool IsRunning { get; set; }
    public static bool LeftBlinkerOn { get; set; }
    public static bool RightBlinkerOn { get; set; }
    public static bool HazardsOn { get; set; }
    public static string VehicleIndicatorStatus
    {
        get
        {
            if (LeftBlinkerOn)
                return " (LI)";
            else if (RightBlinkerOn)
                return " (RI)";
            else if (HazardsOn)
                return " (HAZ)";
            else
                return "";
        }
    }
    public static void Initialize()
    {
        IsRunning = true;
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void Tick()
    {
        if (IsRunning)
        {
            bool PlayerInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
            if (PlayerInVehicle)
            {
                IndicatorsTick();
            }
        }
    }
    private static void IndicatorsTick()
    {
        Vehicle MyCar = Game.LocalPlayer.Character.CurrentVehicle;
        if (MyCar == null || !MyCar.Exists())
            return;

        if (Game.IsKeyDown(Keys.Space) && Game.IsShiftKeyDownRightNow)
        {
            if (HazardsOn)
            {
                MyCar.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Off;
                HazardsOn = false;
            }
            else
            {
                MyCar.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Both;
                HazardsOn = true;
                LeftBlinkerOn = false;
                RightBlinkerOn = false;
                return;
            }
        }
        RightBlinkerTick(MyCar);
        LeftBlinkerTick(MyCar);
        // UI.DebugLine = string.Format("LOn: {0},LTime: {1}, LStart {2},ROn: {3},RTime: {4},RStart: {5},STime: {6},Hazard: {7},Angle: {8}", LeftBlinkerOn, TimeWheelsTurnedRight, LeftBlinkerStartedTurn, RightBlinkerOn, TimeWheelsTurnedRight, RightBlinkerStartedTurn, TimeWheelsStraight,HazardsOn, MyCar.SteeringAngle);
    }
    private static void RightBlinkerTick(Vehicle MyCar)
    {
        if (Game.IsKeyDown(Keys.E) && Game.IsShiftKeyDownRightNow)
        {
            if (RightBlinkerOn)
            {
                MyCar.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Off;
                RightBlinkerOn = false;
            }
            else
            {
                MyCar.IndicatorLightsStatus = VehicleIndicatorLightsStatus.RightOnly;
                RightBlinkerOn = true;
                LeftBlinkerOn = false;
                HazardsOn = false;
            }
        }
        if (RightBlinkerOn)
        {
            if (MyCar.SteeringAngle <= -25f)
                TimeWheelsTurnedRight++;
            else
                TimeWheelsTurnedRight = 0;

            if (TimeWheelsTurnedRight >= 20)
            {
                RightBlinkerStartedTurn = true;
            }

        }
        if (RightBlinkerOn && RightBlinkerStartedTurn)
        {
            if (MyCar.SteeringAngle > -10f)
                TimeWheelsStraight++;
            else
                TimeWheelsStraight = 0;
        }
        if (RightBlinkerOn && TimeWheelsStraight >= 20)
        {
            TimeWheelsTurnedRight = 0;
            TimeWheelsStraight = 0;
            RightBlinkerStartedTurn = false;
            MyCar.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Off;
            RightBlinkerOn = false;
        }
    }
    private static void LeftBlinkerTick(Vehicle MyCar)
    {
        if (Game.IsKeyDown(Keys.Q) && Game.IsShiftKeyDownRightNow)
        {
            if (LeftBlinkerOn)
            {
                MyCar.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Off;
                LeftBlinkerOn = false;
            }
            else
            {
                MyCar.IndicatorLightsStatus = VehicleIndicatorLightsStatus.LeftOnly;
                LeftBlinkerOn = true;
                RightBlinkerOn = false;
                HazardsOn = false;
            }
        }
        if (LeftBlinkerOn)
        {
            if (MyCar.SteeringAngle >= 25f)
                TimeWheelsTurnedLeft++;
            else
                TimeWheelsTurnedLeft = 0;

            if (TimeWheelsTurnedLeft >= 20)
            {
                LeftBlinkerStartedTurn = true;
            }

        }
        if (LeftBlinkerOn && LeftBlinkerStartedTurn)
        {
            if (MyCar.SteeringAngle < 10f)
                TimeWheelsStraight++;
            else
                TimeWheelsStraight = 0;
        }
        if (LeftBlinkerOn && TimeWheelsStraight >= 20)
        {
            TimeWheelsTurnedLeft = 0;
            TimeWheelsStraight = 0;
            LeftBlinkerStartedTurn = false;
            MyCar.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Off;
            LeftBlinkerOn = false;
        }
    }
}

