using LosSantosRED.lsr;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

public class Radio
{
    private VehicleExt VehicleToMonitor;
    private string CurrentRadioStationName;

    public Radio(VehicleExt vehicleToMonitor)
    {
        VehicleToMonitor = vehicleToMonitor;
    }
    public void Update(string DesiredStation)
    {
        if (DesiredStation != "NONE" && VehicleToMonitor != null && VehicleToMonitor.Vehicle.Exists() && VehicleToMonitor.Vehicle.IsEngineOn)
        {
            unsafe
            {
                IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_PLAYER_RADIO_STATION_NAME");
                CurrentRadioStationName = Marshal.PtrToStringAnsi(ptr);
            }
            if (CurrentRadioStationName != DesiredStation)
            {
                SetRadioStation(DesiredStation);
            }
            
        }
    }
    public void SetNextTrack()
    {
        if (VehicleToMonitor != null && VehicleToMonitor.Vehicle.Exists() && VehicleToMonitor.Vehicle.IsEngineOn)
        {
            NativeFunction.CallByName<bool>("SKIP_RADIO_FORWARD");
        }
    }
    public void SetRadioStation(string StationName)
    {
        if (VehicleToMonitor != null && VehicleToMonitor.Vehicle.Exists() && VehicleToMonitor.Vehicle.IsEngineOn)
        {
            NativeFunction.CallByName<bool>("SET_VEH_RADIO_STATION", VehicleToMonitor.Vehicle, StationName);

            if (!VehicleToMonitor.HasAutoSetRadio)
            {
                VehicleToMonitor.HasAutoSetRadio = true;
                //EntryPoint.WriteToConsole($"Player Event: Set Radio For the First Time {StationName}", 5);
            }
        }
    }
}
