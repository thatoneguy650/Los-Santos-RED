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
    private float Health = 1000f;
    private VehicleExt VehicleToMonitor;
    public bool IsRunning { get; private set; }
    private bool CanToggle => VehicleToMonitor.Vehicle.Speed < 4f;
    public Engine(VehicleExt vehicleToMonitor)
    {
        VehicleToMonitor = vehicleToMonitor;  
        if (vehicleToMonitor.Vehicle.Exists())
        {
            Health = vehicleToMonitor.Vehicle.EngineHealth;
            IsRunning = vehicleToMonitor.Vehicle.IsEngineOn;
        }
    }
    public void Update(bool ScaleEngineDamage)
    {
        if(ScaleEngineDamage)
        {
            if(Health > VehicleToMonitor.Vehicle.EngineHealth)
            {
                float Difference = Health - VehicleToMonitor.Vehicle.EngineHealth;
                float ScaledDamage = Health - 3.0f * Difference;

                if(ScaledDamage <= -4000f)
                {
                    ScaledDamage = -4000f;
                }
                EntryPoint.WriteToConsole($"ScaleEngineDamage PrevHeath = {Health}, Current = {VehicleToMonitor.Vehicle.EngineHealth}, Difference = {Difference}, ScaledDamage={ScaledDamage}", 3);
                VehicleToMonitor.Vehicle.EngineHealth = ScaledDamage;
                Health = ScaledDamage;
            }

        }
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
        if (CanToggle)
        {
            EntryPoint.WriteToConsole(string.Format("ToggleEngine Start {0}", IsRunning), 3);
            IsRunning = DesiredStatus;
            Update(false);
            EntryPoint.WriteToConsole(string.Format("ToggleEngine End {0}", IsRunning), 3);
        }
        
    }
}
