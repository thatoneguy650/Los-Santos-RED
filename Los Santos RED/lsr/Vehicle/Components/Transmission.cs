using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


public class Transmission
{

    private VehicleExt VehicleToMonitor;
    private ISettingsProvideable Settings;
    private float CurrentSpeedMPH;
    private float CurrentRPMRatio;
    private bool IsRunningFiber = false;
    private IDriveable Driver;
    public bool CanToggle => VehicleToMonitor != null && VehicleToMonitor.Vehicle.Exists() && VehicleToMonitor.Engine.IsRunning && CurrentSpeedMPH < 5f && !VehicleToMonitor.Vehicle.MustBeHotwired;
    public eTransmissionState TransmissionState { get; private set; }
    public string TransmissionSymbol
    {
        get
        {
            if(TransmissionState == eTransmissionState.Park)
            {
                return "P";
            }
            else if (TransmissionState == eTransmissionState.Reverse)
            {
                return "R";
            }
            if (TransmissionState == eTransmissionState.Drive)
            {
                return "D";
            }
            if (TransmissionState == eTransmissionState.Neutral)
            {
                return "N";
            }
            return "";
        }
    }
    public Transmission(VehicleExt vehicleToMonitor, ISettingsProvideable settings)
    {
        VehicleToMonitor = vehicleToMonitor;
        Settings = settings;
        TransmissionState = eTransmissionState.Drive;
    }
    public void Update(IDriveable driver)
    {
        Driver = driver;
        if (Driver?.IsDriver == true)
        {
            UpdateDriverState();
        }
    }
    public void Set(eTransmissionState statToSet)
    {
        if (!CanToggle)
        {
            return;
        }
        TransmissionState = statToSet;
    }
    public void SetParked()
    {
        TransmissionState = eTransmissionState.Park;
    }
    public void ShiftTransmissionUp()
    {
        if (!CanToggle)
        {
            return;
        }

        if (TransmissionState == eTransmissionState.Drive)
        {
            TransmissionState = eTransmissionState.Reverse;
        }
        else if (TransmissionState == eTransmissionState.Neutral)
        {
            TransmissionState = eTransmissionState.Reverse;
        }
        else if (TransmissionState == eTransmissionState.Reverse)
        {
            TransmissionState = eTransmissionState.Park;
        }
        else if (TransmissionState == eTransmissionState.Park)
        {
            TransmissionState = eTransmissionState.Drive;
        }
    }
    public void ShiftTransmissionDown()
    {

        if(!CanToggle)
        {
            return;
        }



        if (TransmissionState == eTransmissionState.Park)
        {
            TransmissionState = eTransmissionState.Reverse;
        }
        else if (TransmissionState == eTransmissionState.Reverse)
        {
            TransmissionState = eTransmissionState.Drive;
        }
        else if (TransmissionState == eTransmissionState.Neutral)
        {
            TransmissionState = eTransmissionState.Drive;
        }
        else if (TransmissionState == eTransmissionState.Drive)
        {
            TransmissionState = eTransmissionState.Park;
        }
    }
    private void UpdateDriverState()
    {
        if(VehicleToMonitor == null || !VehicleToMonitor.Vehicle.Exists())
        {
            return;
        }
        if (!IsRunningFiber)
        {
            IsRunningFiber = true;
            //EntryPoint.WriteToConsole("TRANSMISSION FIBER STARTED FOR PLAYERS CAR");
            GameFiber.StartNew(delegate
            {
                while (Driver?.IsDriver == true && EntryPoint.ModController.IsRunning)
                {
                    UpdateCurrentGear();
                    GameFiber.Yield();
                }
                //EntryPoint.WriteToConsole("TRANSMISSION FIBER ENDED FOR PLAYERS CAR");
                IsRunningFiber = false;
            }, "Run Debug Logic");
        }
    }

    private void UpdateCurrentGear()
    {
        if(VehicleToMonitor == null || !VehicleToMonitor.Vehicle.Exists())
        {
            return;
        }

        CurrentSpeedMPH = VehicleToMonitor.Vehicle.Speed * 2.23694f;
        CurrentRPMRatio = VehicleToMonitor.Vehicle.EngineRevolutionsRatio;



        if (TransmissionState == eTransmissionState.Park)
        {
            UpdateParked();
        }
        else if (TransmissionState == eTransmissionState.Drive)
        {
            UpdateDrive();
        }
        else if (TransmissionState == eTransmissionState.Neutral)
        {
            UpdateNeutral();
        }
        else if (TransmissionState == eTransmissionState.Reverse)
        {
            //if (Settings.SettingsManager.VehicleSettings.TransmissionInvertControlForReverse)
            //{
            //    UpdateReverseInvert();
                
            //}
            //else
            //{
                UpdateReverse();
            //}
        }
    }

    private void UpdateParked()
    {
        if (CurrentSpeedMPH < 3.0f)
        {
            NativeFunction.Natives.SET_VEHICLE_HANDBRAKE(VehicleToMonitor.Vehicle, true);
            Game.DisableControlAction(0, GameControl.VehicleAccelerate, true);
            Game.DisableControlAction(0, GameControl.VehicleBrake, true);
        }
        else
        {
            Game.DisableControlAction(0, GameControl.VehicleAccelerate, true);
            NativeFunction.Natives.SET_CONTROL_VALUE_NEXT_FRAME(0, (int)GameControl.VehicleBrake, 1.0f);
        }
    }
    private void UpdateDrive()
    {
        NativeFunction.Natives.SET_VEHICLE_HANDBRAKE(VehicleToMonitor.Vehicle, false);
        if (!Game.IsControlPressed(0, GameControl.VehicleAccelerate) && CurrentSpeedMPH <= 15.0f)// && !NativeFunction.Natives.IS_DISABLED_CONTROL_PRESSED<bool>(0, 71))
        {
            if(CurrentSpeedMPH <= 15f)
            {
                NativeFunction.Natives.SET_CONTROL_VALUE_NEXT_FRAME(0, (int)GameControl.VehicleAccelerate, Settings.SettingsManager.VehicleSettings.TransmissionDriveCreepPercentage);
            }
            //else if (CurrentSpeedMPH <= 15f)
            //{
            //    NativeFunction.Natives.SET_CONTROL_VALUE_NEXT_FRAME(0, (int)GameControl.VehicleAccelerate, Math.Max(Settings.SettingsManager.VehicleSettings.TransmissionDriveCreepPercentage - 0.1f, 0f));
            //}
        }
        if(CurrentSpeedMPH < 1.0f || VehicleToMonitor.Vehicle.CurrentGear == 0)
        {
            Game.DisableControlAction(0, GameControl.VehicleBrake, true);
            if(Game.IsControlPressed(0, GameControl.VehicleBrake) || NativeFunction.Natives.IS_DISABLED_CONTROL_PRESSED<bool>(0, 72))
            {
                NativeFunction.Natives.TASK_VEHICLE_TEMP_ACTION(Game.LocalPlayer.Character, VehicleToMonitor.Vehicle, 27, 200);
                NativeFunction.Natives.SET_VEHICLE_BRAKE_LIGHTS(VehicleToMonitor.Vehicle, true);
            }
            else
            {
                NativeFunction.Natives.SET_VEHICLE_BRAKE_LIGHTS(VehicleToMonitor.Vehicle, false);
            }
        }
    }
    private void UpdateNeutral()
    {
        NativeFunction.Natives.SET_VEHICLE_HANDBRAKE(VehicleToMonitor.Vehicle, false);
        Game.DisableControlAction(0, GameControl.VehicleAccelerate, true);
        Game.DisableControlAction(0, GameControl.VehicleBrake, true);
        if (CurrentSpeedMPH < 1.0f && Game.IsControlPressed(0, GameControl.VehicleBrake))
        {
            NativeFunction.Natives.SET_VEHICLE_FORWARD_SPEED(VehicleToMonitor.Vehicle, 0.0f);
        }
    }
    private void UpdateReverse()
    {
        NativeFunction.Natives.SET_VEHICLE_HANDBRAKE(VehicleToMonitor.Vehicle, false);
        Game.DisableControlAction(0, GameControl.VehicleAccelerate, true);
        if (NativeFunction.Natives.IS_DISABLED_CONTROL_PRESSED<bool>(0, 71))
        {
            NativeFunction.Natives.SET_VEHICLE_BRAKE_LIGHTS(VehicleToMonitor.Vehicle, true);
            NativeFunction.Natives.TASK_VEHICLE_TEMP_ACTION(Game.LocalPlayer.Character, VehicleToMonitor.Vehicle, 27, 200);
        }
        else
        {
            NativeFunction.Natives.SET_VEHICLE_BRAKE(VehicleToMonitor.Vehicle, 0.0f);
        }
        if (CurrentSpeedMPH <= 15f)
        {
            NativeFunction.Natives.SET_CONTROL_VALUE_NEXT_FRAME(0, (int)GameControl.VehicleBrake, Settings.SettingsManager.VehicleSettings.TransmissionDriveCreepPercentage);
        }
        //else if (CurrentSpeedMPH <= 15f)
        //{
        //    NativeFunction.Natives.SET_CONTROL_VALUE_NEXT_FRAME(0, (int)GameControl.VehicleBrake, Math.Max(Settings.SettingsManager.VehicleSettings.TransmissionDriveCreepPercentage - 0.1f,0f));
        //}
    }
    private void UpdateReverseInvert()
    {
        NativeFunction.Natives.SET_INVERT_VEHICLE_CONTROLS(VehicleToMonitor.Vehicle, true);


        NativeFunction.Natives.SET_VEHICLE_HANDBRAKE(VehicleToMonitor.Vehicle, false);
        Game.DisableControlAction(0, GameControl.VehicleAccelerate, true);
        if (NativeFunction.Natives.IS_DISABLED_CONTROL_PRESSED<bool>(0, 71))
        {
            NativeFunction.Natives.SET_VEHICLE_BRAKE_LIGHTS(VehicleToMonitor.Vehicle, true);
            NativeFunction.Natives.TASK_VEHICLE_TEMP_ACTION(Game.LocalPlayer.Character, VehicleToMonitor.Vehicle, 27, 200);
        }
        else
        {
            NativeFunction.Natives.SET_VEHICLE_BRAKE(VehicleToMonitor.Vehicle, 0.0f);
        }
        if (CurrentSpeedMPH <= 15f)
        {
            NativeFunction.Natives.SET_CONTROL_VALUE_NEXT_FRAME(0, (int)GameControl.VehicleBrake, Settings.SettingsManager.VehicleSettings.TransmissionDriveCreepPercentage);
        }
    }
    private void UpdateReverseOLD()
    {










        NativeFunction.Natives.SET_VEHICLE_HANDBRAKE(VehicleToMonitor.Vehicle, false);


        Vector3 DrivingVector = NativeFunction.Natives.GET_ENTITY_SPEED_VECTOR<Vector3>(VehicleToMonitor.Vehicle,true);


        if(DrivingVector.Y > 1.0f && CurrentSpeedMPH > 5.0f)
        {
            Game.DisableControlAction(0, GameControl.VehicleAccelerate, true);
            NativeFunction.Natives.SET_INVERT_VEHICLE_CONTROLS(VehicleToMonitor.Vehicle, false);
        }
        else
        {
            if (!Game.IsControlPressed(0, GameControl.VehicleBrake))
            {
                NativeFunction.Natives.SET_CONTROL_VALUE_NEXT_FRAME(0, (int)GameControl.VehicleBrake, 0.3f);
            }   
            if(Game.IsControlPressed(0, GameControl.VehicleBrake) || Game.IsControlPressed(0, GameControl.VehicleAccelerate)) //|| Game.IsControlPressed(0, GameControl.VehicleBrake) || Game.IsControlPressed(0, GameControl.VehicleBrake))
            {
                NativeFunction.Natives.SET_INVERT_VEHICLE_CONTROLS(VehicleToMonitor.Vehicle, true);
                NativeFunction.Natives.SET_CONTROL_VALUE_NEXT_FRAME(0, (int)GameControl.VehicleBrake, 0.0f);
                NativeFunction.Natives.SET_CONTROL_VALUE_NEXT_FRAME(0, (int)GameControl.VehicleAccelerate, 0.3f);
            }
            else
            {
                NativeFunction.Natives.SET_INVERT_VEHICLE_CONTROLS(VehicleToMonitor.Vehicle, false);
            }


            if(CurrentSpeedMPH < 1.0f && Game.IsControlPressed(0, GameControl.VehicleBrake))
            {
                //rpm?
                NativeFunction.Natives.SET_VEHICLE_BRAKE(VehicleToMonitor.Vehicle, true);//
                NativeFunction.Natives.SET_VEHICLE_FORWARD_SPEED(VehicleToMonitor.Vehicle, 0.0f);//
                NativeFunction.Natives.SET_VEHICLE_BRAKE_LIGHTS(VehicleToMonitor.Vehicle, true);
            }
            else if (CurrentSpeedMPH < 1.0f)
            {
                NativeFunction.Natives.SET_VEHICLE_BRAKE_LIGHTS(VehicleToMonitor.Vehicle, false);
            }
        }
    }
}
