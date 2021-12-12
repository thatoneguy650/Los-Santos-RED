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


public class FuelTank
{
    private VehicleExt VehicleExt;
    private uint GameTimeLastCheckedFuel;
    private float FuelLevel;
    private float prevFuelLevel;
    public bool IsLeaking { get; set; }
    public FuelTank(VehicleExt vehicleToMonitor)
    {
        VehicleExt = vehicleToMonitor;
    }
    public string UIText
    {
        get
        {
           return string.Format(" Fuel: {0}", (VehicleExt.Vehicle.FuelLevel / 100f).ToString("P2"));
        }
    }   
    public void Update()
    {
        FuelLevel = VehicleExt.Vehicle.FuelLevel;
        if(prevFuelLevel != FuelLevel)
        {
            if(FuelLevel > prevFuelLevel)
            {
                OnFuelAdded();
            }
            else if(FuelLevel < prevFuelLevel)
            {
                if (!IsLeaking)
                {
                    OnTankStartedLeaking();
                }
            }
            prevFuelLevel = FuelLevel;
        }
        else
        {
            IsLeaking = false;
        }
        if (Game.GameTime - GameTimeLastCheckedFuel >= 200)
        {
            ConsumeFuel();
            GameTimeLastCheckedFuel = Game.GameTime;
        }    
    }

    private void OnFuelAdded()
    {
        IsLeaking = false;
        //EntryPoint.WriteToConsole("FuelTank Fuel Added", 5);
    }
    private void OnTankStartedLeaking()
    {
        IsLeaking = true;    
    }
    private void ConsumeFuel()
    {
        if (VehicleExt.Vehicle.IsEngineOn)
        {
            float AmountToSubtract = 0.001f + VehicleExt.Vehicle.Speed * 0.0001f;
            FuelLevel -= AmountToSubtract;
            VehicleExt.Vehicle.FuelLevel = FuelLevel;
        }
    }
}

