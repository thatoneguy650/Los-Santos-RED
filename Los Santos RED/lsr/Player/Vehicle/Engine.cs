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


//public class Engine
//{
//    private VehicleExt VehicleToMonitor;
//    private bool CanToggle
//    {
//        get
//        {
//            if (VehicleToMonitor.Vehicle.Speed > 2f)
//            {
//                return false;
//            }
//            else
//            {
//                return true;
//            }
//        }
//    }
//    public Engine(VehicleExt vehicleToMonitor)
//    {
//        VehicleToMonitor = vehicleToMonitor;
//    }
//    public void Toggle()
//    {
//        Toggle(!VehicleToMonitor.Vehicle.IsEngineOn);
//    }
//    public void Toggle(bool DesiredStatus)
//    {
//        Mod.Debug.WriteToLog("ToggleEngine", string.Format("Start {0}", VehicleToMonitor.Vehicle.IsEngineOn));
//        if (CanToggle)
//        {
//            VehicleToMonitor.Vehicle.IsEngineOn = DesiredStatus;
//        }
//        Mod.Debug.WriteToLog("ToggleEngine", string.Format("End {0}", VehicleToMonitor.Vehicle.IsEngineOn));
//    }
//}

