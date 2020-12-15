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
    private bool MobileEnabled;
    private string CurrentRadioStationName;

    public Radio(VehicleExt vehicleToMonitor)
    {
        VehicleToMonitor = vehicleToMonitor;
    }

    public bool AutoTune { get; set; } = true;
    public string AutoTuneStation { get; set; } = "RADIO_19_USER";
    public bool CanChangeStation
    {
        get
        {
            if(Game.LocalPlayer.Character.IsInAnyVehicle(false) && !Game.LocalPlayer.Character.IsInHelicopter && !Game.LocalPlayer.Character.IsInPlane && !Game.LocalPlayer.Character.IsInBoat && Game.LocalPlayer.Character.CurrentVehicle.IsEngineOn && !Mod.Player.IsHotwiring)
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
        EnablePoliceCarMusic();
        CheckAutoTuning();  
    }
    public void ChangeStation(string StationName)
    {
        if (CanChangeStation)
        {
            SetRadioStation(StationName);
        }
    }
    private void EnablePoliceCarMusic()
    {
        if (VehicleToMonitor != null && VehicleToMonitor.Engine.IsRunning && VehicleToMonitor.Vehicle.Exists() && VehicleToMonitor.Vehicle.IsPoliceVehicle)
        {
            MobileEnabled = true;
            NativeFunction.CallByName<bool>("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", true);
        }
        else
        {
            MobileEnabled = false;
            NativeFunction.CallByName<bool>("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", false);
        }
    }
    private void CheckAutoTuning()
    {
        if (AutoTuneStation.ToUpper() != "NONE")
        {
            unsafe
            {
                IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_PLAYER_RADIO_STATION_NAME");
                CurrentRadioStationName = Marshal.PtrToStringAnsi(ptr);
            }


            string GET_PLAYER_RADIO_STATION_NAME = "";
            unsafe
            {
                IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_PLAYER_RADIO_STATION_NAME");
                GET_PLAYER_RADIO_STATION_NAME = Marshal.PtrToStringAnsi(ptr);
            }


            int GET_PLAYER_RADIO_STATION_INDEX = NativeFunction.CallByName<int>("GET_PLAYER_RADIO_STATION_INDEX");







            if (CurrentRadioStationName != AutoTuneStation)
            {
                SetRadioStation(AutoTuneStation);
            }
        }
    }
    private void ChangeStationAnimation(string StationName)
    {
        GameFiber.StartNew(delegate
        {
            var sDict = "veh@van@ds@base";
            NativeFunction.CallByName<bool>("REQUEST_ANIM_DICT", sDict);
            while (!NativeFunction.CallByName<bool>("HAS_ANIM_DICT_LOADED", sDict))
                GameFiber.Yield();
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, sDict, "start_engine", 2.0f, -2.0f, -1, 48, 0, true, false, true);

            bool Cancel = false;
            uint GameTimeStartedAnimation = Game.GameTime;
            while (Game.GameTime - GameTimeStartedAnimation <= 1000)
            {
                if (Game.IsControlJustPressed(0, GameControl.VehicleExit))
                {
                    NativeFunction.CallByName<bool>("STOP_ANIM_TASK", Game.LocalPlayer.Character, sDict, "start_engine", 8.0f);
                    Cancel = true;
                }
                GameFiber.Sleep(200);
            }
            if (!Cancel)
                SetRadioStation(StationName);

        });
    }
    private void SetRadioStation(string StationName)
    {
        if (VehicleToMonitor != null && VehicleToMonitor.Engine.IsRunning && VehicleToMonitor.Vehicle.Exists())
        {
            Mod.Debug.WriteToLog("RadioTuning", string.Format("Tuned: {0} Desired: {1}", CurrentRadioStationName, StationName));
            if(MobileEnabled)
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
