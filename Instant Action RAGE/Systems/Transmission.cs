using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

internal static class Transmission
{
    public static bool brakingForward = false;
    public static bool brakingReverse = false;
    public static bool IsRunning { get; set; } = true;
    public static Gears CurrentGear = Gears.Park;
    private static uint GameTimeControlCheck;

    public static void Initialize()
    {
        //MainLoop();
    }
    public enum Gears
    {
        Park = 0,
        Drive = 1,
        Reverse = -1,
    }
    public static void MainLoop()
    {
        GameFiber.StartNew(delegate
        {
            while (IsRunning)
            {
                ControlLoop();

                if (Game.GameTime - GameTimeControlCheck >= 100 && Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Speed <= 0.5f)
                {
                    if (Game.IsKeyDownRightNow(Keys.LShiftKey))
                    {
                        if (CurrentGear == Gears.Drive)
                            CurrentGear = Gears.Drive;
                        else if (CurrentGear == Gears.Park)
                            CurrentGear = Gears.Drive;
                        else if (CurrentGear == Gears.Reverse)
                            CurrentGear = Gears.Park;
                    }
                    else if (Game.IsKeyDownRightNow(Keys.LControlKey))
                    {
                        if (CurrentGear == Gears.Drive)
                            CurrentGear = Gears.Park;
                        else if (CurrentGear == Gears.Park)
                            CurrentGear = Gears.Reverse;
                        else if (CurrentGear == Gears.Reverse)
                            CurrentGear = Gears.Reverse;
                    }
                    GameTimeControlCheck = Game.GameTime;
                }

                GameFiber.Yield();
            }
        });
    }

    private static void ControlLoop()
    {
        if (!Game.LocalPlayer.Character.IsInAnyVehicle(false) || Game.LocalPlayer.Character.IsInAirVehicle || Game.LocalPlayer.Character.IsInBoat)
            return;

        if (Game.LocalPlayer.Character.CurrentVehicle.Speed >= 0.5f)
        {
            //Game.DisableControlAction(0, GameControl.VehicleBrake, false);
            //Game.DisableControlAction(0, GameControl.VehicleAccelerate, false);
            return;
        }
        Vector3 CurrentVehicleSpeedVector = NativeFunction.CallByName<Vector3>("GET_ENTITY_SPEED_VECTOR", Game.LocalPlayer.Character.CurrentVehicle, true);

        bool BrakePressed = Game.IsControlPressed(0, GameControl.VehicleBrake);
        bool AcceleratePressed = Game.IsControlPressed(0, GameControl.VehicleAccelerate);

        if(CurrentGear == Gears.Park)
        {
            NativeFunction.CallByName<bool>("FREEZE_ENTITY_POSITION", Game.LocalPlayer.Character.CurrentVehicle, true);
        }
        else
        {
            NativeFunction.CallByName<bool>("FREEZE_ENTITY_POSITION", Game.LocalPlayer.Character.CurrentVehicle, false);
        }



        if(CurrentGear == Gears.Drive && BrakePressed)
        {
            Game.DisableControlAction(0, GameControl.VehicleBrake, true);
            NativeFunction.CallByName<Vector3>("SET_VEHICLE_FORWARD_SPEED", Game.LocalPlayer.Character.CurrentVehicle, CurrentVehicleSpeedVector.Y * 0.98f);
            NativeFunction.CallByName<Vector3>("SET_VEHICLE_BRAKE_LIGHTS", Game.LocalPlayer.Character.CurrentVehicle, true);        
        }



        if (CurrentGear == Gears.Reverse && AcceleratePressed)
        {
            Game.DisableControlAction(0, GameControl.VehicleAccelerate, true);
            NativeFunction.CallByName<Vector3>("SET_VEHICLE_FORWARD_SPEED", Game.LocalPlayer.Character.CurrentVehicle, CurrentVehicleSpeedVector.Y * 1.5f);
        }
        else if(CurrentGear == Gears.Reverse && BrakePressed)
        {

            NativeFunction.CallByName<Vector3>("SET_VEHICLE_FORWARD_SPEED", Game.LocalPlayer.Character.CurrentVehicle, CurrentVehicleSpeedVector.Y * 0.98f);
            NativeFunction.CallByName<Vector3>("SET_VEHICLE_BRAKE_LIGHTS", Game.LocalPlayer.Character.CurrentVehicle, true);
        }



        //if (BrakePressed && CurrentGear == Gears.Drive)
        //{
        //    Game.DisableControlAction(0, GameControl.VehicleBrake, true);
        //    NativeFunction.CallByName<Vector3>("SET_VEHICLE_FORWARD_SPEED", Game.LocalPlayer.Character.CurrentVehicle, CurrentVehicleSpeedVector.Y * 0.98f);
        //}
        //if (AcceleratePressed && CurrentGear == Gears.Reverse)
        //{
        //    Game.DisableControlAction(0, GameControl.VehicleAccelerate, true);
        //    NativeFunction.CallByName<Vector3>("SET_VEHICLE_FORWARD_SPEED", Game.LocalPlayer.Character.CurrentVehicle, CurrentVehicleSpeedVector.Y * 0.98f);
        //}
        //if (!AcceleratePressed && !BrakePressed && CurrentGear == Gears.Drive)
        //{
        //    NativeFunction.CallByName<Vector3>("SET_VEHICLE_FORWARD_SPEED", Game.LocalPlayer.Character.CurrentVehicle, CurrentVehicleSpeedVector.Y * 1.02f);
        //}
        //if (!AcceleratePressed && !BrakePressed && CurrentGear == Gears.Reverse)
        //{
        //    NativeFunction.CallByName<Vector3>("SET_VEHICLE_FORWARD_SPEED", Game.LocalPlayer.Character.CurrentVehicle, CurrentVehicleSpeedVector.Y * 1.02f);
        //}



        //Game.DisableControlAction(0, GameControl.VehicleBrake,true);
        //    //Game.DisableControlThisFrame(0, Control.VehicleBrake);
        //    Function.Call(Hash._0xAB54A438726D25D5, (InputArgument)Game.LocalPlayer.Character.CurrentVehicle, (InputArgument)((double)y * 0.98));
        //    Game.LocalPlayer.Character.CurrentVehicle.BrakeLightsOn = true;
        //}
        //if (brakingReverse && !disableUseAcceleratorAsBrake)
        //{
        //    Game.DisableControlThisFrame(0, Control.VehicleAccelerate);
        //    Function.Call(Hash._0xAB54A438726D25D5, (InputArgument)Game.LocalPlayer.Character.CurrentVehicle, (InputArgument)((double)y * 0.98));
        //    Game.LocalPlayer.Character.CurrentVehicle.BrakeLightsOn = true;
        //}
        //if (brakingForward && (double)Game.GetDisabledControlNormal(0, Control.VehicleBrake) == 0.0)
        //    brakingForward = false;
        //if (!brakingReverse || (double)Game.GetDisabledControlNormal(0, Control.VehicleAccelerate) != 0.0)
        //    return;
        //brakingReverse = false;
    }
}

