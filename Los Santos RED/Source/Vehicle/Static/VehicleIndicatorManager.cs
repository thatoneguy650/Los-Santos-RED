using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public static class VehicleIndicatorManager
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
    public static void ToggleHazards()
    {
        Vehicle MyCar = Game.LocalPlayer.Character.CurrentVehicle;
        if (MyCar == null || !MyCar.Exists())
            return;

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
    public static void ToggleLeftIndicator()
    {
        Vehicle MyCar = Game.LocalPlayer.Character.CurrentVehicle;
        if (MyCar == null || !MyCar.Exists())
            return;

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
    public static void ToggleRightIndicator()
    {
        Vehicle MyCar = Game.LocalPlayer.Character.CurrentVehicle;
        if (MyCar == null || !MyCar.Exists())
            return;

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
    private static void IndicatorsTick()
    {
        Vehicle MyCar = Game.LocalPlayer.Character.CurrentVehicle;
        if (MyCar == null || !MyCar.Exists())
            return;

        RightBlinkerTick(MyCar);
        LeftBlinkerTick(MyCar);
    }
    private static void RightBlinkerTick(Vehicle MyCar)
    {
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

