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

    public bool AutoTune { get; set; } = false;
    public string AutoTuneStation { get; set; } = "RADIO_19_USER";
    public bool CanChangeStation
    {
        get
        {
            if(Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.IsEngineOn)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public void Update()
    {
        if (AutoTune)
        {
            unsafe
            {
                IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_PLAYER_RADIO_STATION_NAME");
                CurrentRadioStationName = Marshal.PtrToStringAnsi(ptr);
            }
            //int GET_PLAYER_RADIO_STATION_INDEX = NativeFunction.CallByName<int>("GET_PLAYER_RADIO_STATION_INDEX");//why are there two? mobile and regular?
            if (CurrentRadioStationName != AutoTuneStation)
            {
                SetRadioStation(AutoTuneStation);
            }
        }
    }
    public void SetNextTrack()
    {
        if(CanChangeStation)
        {
            NativeFunction.CallByName<bool>("SKIP_RADIO_FORWARD");
        }
        
    }
    public void ChangeStation(string StationName)
    {
        if (CanChangeStation)
        {
            SetRadioStation(StationName);
        }
    }
    private void SetRadioStation(string StationName)
    {
        if (VehicleToMonitor != null && VehicleToMonitor.Vehicle.IsEngineOn && VehicleToMonitor.Vehicle.Exists())
        {
            Debug.Instance.WriteToLog("RadioTuning", string.Format("Tuned: {0} Desired: {1}", CurrentRadioStationName, StationName));
            if(Audio.Instance.IsMobileRadioEnabled)
            {
                NativeFunction.CallByName<bool>("SET_RADIO_TO_STATION_INDEX", 1);//just do this to wake it up for now, eventually get the index from the station?
                NativeFunction.CallByName<bool>("SET_RADIO_TO_STATION_NAME", StationName);
            }
            else
            {
                NativeFunction.CallByName<bool>("SET_VEH_RADIO_STATION", VehicleToMonitor.Vehicle, StationName);
            }
            
        }
    }
}
