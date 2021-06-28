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

    public FuelTank(VehicleExt vehicleToMonitor)
    {
        VehicleExt = vehicleToMonitor;
    }
    public string UIText
    {
        get
        {
           return string.Format(" Fuel {0}", (VehicleExt.Vehicle.FuelLevel / 100f).ToString("P2"));
        }
    }   
    public void Update()
    {
        if (Game.GameTime - GameTimeLastCheckedFuel >= 200)
        {
            if(VehicleExt.Vehicle.IsEngineOn)
            {
                float CurrentLevel = VehicleExt.Vehicle.FuelLevel;
                float AmountToSubtract = 0.001f + VehicleExt.Vehicle.Speed * 0.0001f;
                VehicleExt.Vehicle.FuelLevel = CurrentLevel - AmountToSubtract;
            }
            GameTimeLastCheckedFuel = Game.GameTime;
        }    
    }
}

