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
    private bool CanToggle => VehicleToMonitor.Vehicle.Speed < 4f;
    public Engine(VehicleExt vehicleToMonitor)
    {
        VehicleToMonitor = vehicleToMonitor;
        if (vehicleToMonitor.Vehicle.Exists())
        {
            IsRunning = vehicleToMonitor.Vehicle.IsEngineOn;
        }
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
        EntryPoint.WriteToConsole(string.Format("ToggleEngine Start {0}", IsRunning),3);
        if (CanToggle)
        {
            IsRunning = DesiredStatus;
            Update();
        }
        EntryPoint.WriteToConsole(string.Format("ToggleEngine End {0}", IsRunning), 3);
    }
}
