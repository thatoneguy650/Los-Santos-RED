using ExtensionsMethods;
using LosSantosRED.lsr;
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


public class Engine
{
    private VehicleExt VehicleToMonitor;
    public bool IsRunning { get; private set; }
    private bool CanToggle
    {
        get
        {
            if (VehicleToMonitor.Vehicle.Speed > 4f || Mod.Player.IsHotwiring)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    public Engine(VehicleExt vehicleToMonitor)
    {
        VehicleToMonitor = vehicleToMonitor;
    }
    public void Update()
    {

        if (VehicleToMonitor.Vehicle.IsEngineStarting)
        {
            IsRunning = true;
        }

        if (!IsRunning)
        {
            VehicleToMonitor.Vehicle.IsDriveable = false;
            VehicleToMonitor.Vehicle.IsEngineOn = false;
        }
        else
        {
            VehicleToMonitor.Vehicle.IsDriveable = true;
            VehicleToMonitor.Vehicle.IsEngineOn = true;
        }
    }
    public void Toggle()
    {
        Toggle(!IsRunning);
    }
    public void Toggle(bool DesiredStatus)
    {
        Mod.Debug.WriteToLog("ToggleEngine", string.Format("Start {0}", IsRunning));
        if (CanToggle)
        {
            IsRunning = DesiredStatus;
        }
        Mod.Debug.WriteToLog("ToggleEngine", string.Format("End {0}", IsRunning));
    }
}

