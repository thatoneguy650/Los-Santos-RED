using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

public class VehicleRadio
{
    private bool MobileEnabled;
    private string CurrentRadioStationName;
    public bool AutoTune { get; set; } = true;
    public string AutoTuneStation { get; set; } = "RADIO_19_USER";
    public bool CanChangeStation
    {
        get
        {
            if(Game.LocalPlayer.Character.IsInAnyVehicle(false) && !Game.LocalPlayer.Character.IsInHelicopter && !Game.LocalPlayer.Character.IsInPlane && !Game.LocalPlayer.Character.IsInBoat && Game.LocalPlayer.Character.CurrentVehicle.IsEngineOn && !Mod.Player.VehicleEngine.IsHotwiring)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public void Tick()
    {
        EnablePoliceCarMusic();
        CheckAutoTuning();  
    }
    public void ChangeStation(string StationName)
    {
        if (CanChangeStation)
        {
            if (!Game.LocalPlayer.Character.IsOnBike)
            {
                ChangeStationAnimation(StationName);
            }
            else
            {
                SetRadioStation(StationName);
            }
        }
    }
    private void EnablePoliceCarMusic()
    {
        if (Mod.Player.IsInVehicle && Mod.Player.VehicleEngine.IsEngineRunning && Game.LocalPlayer.Character.IsInAnyPoliceVehicle)
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
        if (Mod.Player.IsInVehicle)
        {
            if (AutoTuneStation.ToUpper() != "NONE")
            {
                unsafe
                {
                    IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_PLAYER_RADIO_STATION_NAME");
                    CurrentRadioStationName = Marshal.PtrToStringAnsi(ptr);
                }
                if (CurrentRadioStationName != AutoTuneStation && Game.LocalPlayer.Character.CurrentVehicle != null)
                {
                    SetRadioStation(AutoTuneStation);
                }
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
        if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle != null && Game.LocalPlayer.Character.CurrentVehicle.IsEngineOn)
        {
            Mod.Debug.WriteToLog("RadioTuning", string.Format("Tuned: {0} Desired: {1}", CurrentRadioStationName, StationName));

            if(MobileEnabled)
            {
                NativeFunction.CallByName<bool>("SET_RADIO_TO_STATION_NAME", StationName);
            }
            else
            {
                NativeFunction.CallByName<bool>("SET_VEH_RADIO_STATION", Game.LocalPlayer.Character.CurrentVehicle, StationName);
            }
            
        }
    }
}
