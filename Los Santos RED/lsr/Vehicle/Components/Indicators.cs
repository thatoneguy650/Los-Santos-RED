using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class Indicators
{
    private VehicleExt VehicleExt;
    private bool LeftBlinkerStartedTurn;
    private bool RightBlinkerStartedTurn;
    private uint GameTimeStartedTurnWheelRight;
    private uint GameTimeStartedTurnWheelLeft;
    private uint GameTimeStartedTurnWheelStraight;
    private uint GameTimeLastPressIndicator;

    public Indicators(VehicleExt vehicleToMonitor)
    {
        VehicleExt = vehicleToMonitor;
    }
    public bool LeftBlinkerOn { get; private set; }
    public bool RightBlinkerOn { get; private set; }
    public bool HazardsOn { get; private set; }
    public string DebugStatus
    {
        get
        {
            return string.Format("R {0},STR {1},RBST {2},L {3},STL {4},LBST {5},GTS {6}"
                            , RightBlinkerOn, GameTimeStartedTurnWheelRight, RightBlinkerStartedTurn, LeftBlinkerOn, GameTimeStartedTurnWheelLeft, LeftBlinkerStartedTurn, GameTimeStartedTurnWheelStraight);
        }
    }
    public string Status
    {
        get
        {
            if (LeftBlinkerOn)
                return "(LI)";
            else if (RightBlinkerOn)
                return "(RI)";
            else if (HazardsOn)
                return "(HAZ)";
            else
                return "";
        }
    }
    public void Update()
    {
        IndicatorsTick();
    }
    public void ToggleHazards()
    {
        if (Game.GameTime - GameTimeLastPressIndicator >= 1500)
        {
            if (VehicleExt == null || !VehicleExt.Vehicle.Exists() || (!VehicleExt.IsCar && !VehicleExt.IsMotorcycle))
            {
                return;
            }

            if (HazardsOn)
            {
                VehicleExt.Vehicle.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Off;
                HazardsOn = false;
            }
            else
            {
                VehicleExt.Vehicle.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Both;
                HazardsOn = true;
                LeftBlinkerOn = false;
                RightBlinkerOn = false;
                return;
            }
            GameTimeLastPressIndicator = Game.GameTime;
        }
    }
    public void ToggleLeft()
    {
        if (Game.GameTime - GameTimeLastPressIndicator >= 1500)
        {
            if (VehicleExt == null || !VehicleExt.Vehicle.Exists() || (!VehicleExt.IsCar && !VehicleExt.IsMotorcycle))
                return;

            if (LeftBlinkerOn)
            {
                VehicleExt.Vehicle.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Off;
                LeftBlinkerOn = false;
            }
            else
            {
                VehicleExt.Vehicle.IndicatorLightsStatus = VehicleIndicatorLightsStatus.LeftOnly;
                LeftBlinkerOn = true;
                RightBlinkerOn = false;
                HazardsOn = false;
            }
            GameTimeLastPressIndicator = Game.GameTime;


           
        }
    }
    public void ToggleRight()
    {
        if (Game.GameTime - GameTimeLastPressIndicator >= 1500)
        {
            if (VehicleExt == null || !VehicleExt.Vehicle.Exists() || (!VehicleExt.IsCar && !VehicleExt.IsMotorcycle))
                return;

            if (RightBlinkerOn)
            {
                VehicleExt.Vehicle.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Off;
                RightBlinkerOn = false;
            }
            else
            {
                VehicleExt.Vehicle.IndicatorLightsStatus = VehicleIndicatorLightsStatus.RightOnly;
                RightBlinkerOn = true;
                LeftBlinkerOn = false;
                HazardsOn = false;
            }
            GameTimeLastPressIndicator = Game.GameTime;
        }
        
    }
    private void IndicatorsTick()
    {
        if (VehicleExt == null || !VehicleExt.Vehicle.Exists())
            return;

        RightBlinkerTick();
        LeftBlinkerTick();
    }
    private void RightBlinkerTick()
    {
        if (RightBlinkerOn)
        {
            if (VehicleExt.Vehicle.SteeringAngle <= -25f)
            {
                if (GameTimeStartedTurnWheelRight == 0)
                {
                    GameTimeStartedTurnWheelRight = Game.GameTime;
                }
            }
            else
            {
                GameTimeStartedTurnWheelRight = 0;
            }

            if (GameTimeStartedTurnWheelRight != 0 && Game.GameTime - GameTimeStartedTurnWheelRight >= 750)
            {
                RightBlinkerStartedTurn = true;
            }

        }
        if (RightBlinkerOn && RightBlinkerStartedTurn)
        {
            if (VehicleExt.Vehicle.SteeringAngle > -10f)
            {
                if (GameTimeStartedTurnWheelStraight == 0)
                {
                    GameTimeStartedTurnWheelStraight = Game.GameTime;
                }
            }
            else
            {
                GameTimeStartedTurnWheelStraight = 0;
            }
        }
        if (RightBlinkerOn && GameTimeStartedTurnWheelStraight != 0 && Game.GameTime - GameTimeStartedTurnWheelStraight >= 750)
        {
            GameTimeStartedTurnWheelRight = 0;
            GameTimeStartedTurnWheelStraight = 0;
            RightBlinkerStartedTurn = false;
            VehicleExt.Vehicle.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Off;
            RightBlinkerOn = false;
        }
    }
    private void LeftBlinkerTick()
    {
        if (LeftBlinkerOn)
        {
            if (VehicleExt.Vehicle.SteeringAngle >= 25f)
            {
                if(GameTimeStartedTurnWheelLeft == 0)
                {
                    GameTimeStartedTurnWheelLeft = Game.GameTime;
                }
            }
            else
            {
                GameTimeStartedTurnWheelLeft = 0;
            }

            if (GameTimeStartedTurnWheelLeft != 0 && Game.GameTime - GameTimeStartedTurnWheelLeft >= 750)
            {
                LeftBlinkerStartedTurn = true;
            }

        }
        if (LeftBlinkerOn && LeftBlinkerStartedTurn)
        {
            if (VehicleExt.Vehicle.SteeringAngle < 10f)
            {
                if (GameTimeStartedTurnWheelStraight == 0)
                {
                    GameTimeStartedTurnWheelStraight = Game.GameTime;
                }
            }
            else
            {
                GameTimeStartedTurnWheelStraight = 0;
            }
        }
        if (LeftBlinkerOn && GameTimeStartedTurnWheelStraight != 0 && Game.GameTime - GameTimeStartedTurnWheelStraight >= 750)
        {
            GameTimeStartedTurnWheelLeft = 0;
            GameTimeStartedTurnWheelStraight = 0;
            LeftBlinkerStartedTurn = false;
            VehicleExt.Vehicle.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Off;
            LeftBlinkerOn = false;
        }
    }
}

