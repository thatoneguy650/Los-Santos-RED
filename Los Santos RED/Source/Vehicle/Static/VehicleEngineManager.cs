using ExtensionsMethods;
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


public static class VehicleEngineManager
{
    private static bool PrevEngineRunning;
    private static bool WasinVehicle;
    private static bool TogglingEngine;
    private static uint GameTimeStartedHotwiring;
    private static bool PrevIsHotwiring;
    private static bool IsPlayerInVehicle;
    public static Keys EngineToggleKey { get; private set; }
    public static bool IsEngineRunning { get; private set; }
    public static bool IsRunning { get; set; }
    public static bool IsHotwiring
    {
        get
        {
            if (GameTimeStartedHotwiring == 0)
                return false;
            else if (Game.GameTime - GameTimeStartedHotwiring <= 4000)
                return true;
            else
                return false;
        }
    }
    public static bool CanToggleEngine
    {
        get
        {
            if(!IsPlayerInVehicle)
            {
                return false;
            }
            else if (TogglingEngine)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    public static void Initialize()
    {
        IsRunning = true;
        IsEngineRunning = false;
        PrevEngineRunning = false;
        WasinVehicle = false;
        TogglingEngine = false;
        GameTimeStartedHotwiring = 0;
        PrevIsHotwiring = false; 
        EngineToggleKey = SettingsManager.MySettings.KeyBinding.VehicleKey;

        if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && !Game.LocalPlayer.Character.IsInHelicopter && !Game.LocalPlayer.Character.IsInPlane && !Game.LocalPlayer.Character.IsInBoat)
        {
            if(Game.LocalPlayer.Character.CurrentVehicle != null)
                IsEngineRunning = Game.LocalPlayer.Character.CurrentVehicle.IsEngineOn;
        }      
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void Tick()
    {
        if (IsRunning)
        {
            IsPlayerInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
            if (WasinVehicle != IsPlayerInVehicle)
            {
                IsPlayerInVehicleChanged();
            }

            if (PrevIsHotwiring != IsHotwiring)
            {
                IsHotWiringChanged();
            }

            if (IsPlayerInVehicle)
            {
                SetEngineToDesiredStatus();
            }
            else
            {
                GameTimeStartedHotwiring = 0;
            }

            if (PrevEngineRunning != IsEngineRunning)
            {
                IsEngineRunningChanged();
            }
        }
    }
    public static void ToggleEngine(bool PerformAnimation, bool DesiredEngineStatus)
    {
        if (TogglingEngine)
            return;

        Debugging.WriteToLog("ToggleEngine", string.Format("Start {0}", IsEngineRunning));

        TogglingEngine = true;

        if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && !Game.LocalPlayer.Character.IsInHelicopter && !Game.LocalPlayer.Character.IsInPlane && !Game.LocalPlayer.Character.IsInBoat)
        {
            if (Game.LocalPlayer.Character.CurrentVehicle.Speed > 4f)
            {
                TogglingEngine = false;
                return;
            }

            if (IsHotwiring)
            {
                TogglingEngine = false;
                return;
            }
            if (!Game.LocalPlayer.Character.IsOnBike && PerformAnimation)
            {
                StartEngineAnimation();
            }
            if (Game.LocalPlayer.Character.IsInAnyVehicle(false))
            {
                if (!DesiredEngineStatus)
                    IsEngineRunning = false;
                else
                    IsEngineRunning = true;
            }
        }
        TogglingEngine = false;
        Debugging.WriteToLog("ToggleEngine", string.Format("End {0}", IsEngineRunning));
    }
    public static void TurnOffEngine()
    {
        ToggleEngine(false, false);
    }
    private static void SetEngineToDesiredStatus()
    {
        if (!Game.LocalPlayer.Character.IsInHelicopter && !Game.LocalPlayer.Character.IsInPlane && !Game.LocalPlayer.Character.IsInBoat)
        {
            if (!IsEngineRunning)
            {
                Game.LocalPlayer.Character.CurrentVehicle.IsDriveable = false;
            }
            else
            {
                Game.LocalPlayer.Character.CurrentVehicle.IsDriveable = true;
                Game.LocalPlayer.Character.CurrentVehicle.IsEngineOn = true;
            }
        }
    }
    private static void IsHotWiringChanged()
    {
        if(IsHotwiring)
        {

        }
        else
        {
            if(Game.LocalPlayer.Character.IsInAnyVehicle(false))
                IsEngineRunning = true;
        }
        PrevIsHotwiring = IsHotwiring;
    }
    private static void IsEngineRunningChanged()
    {
        Debugging.WriteToLog("ToggleEngine", string.Format("EngineRunning: {0}",IsEngineRunning));
        PrevEngineRunning = IsEngineRunning;
    }
    private static void IsPlayerInVehicleChanged()
    {
        if(IsPlayerInVehicle)
        {
            if (Game.LocalPlayer.Character.CurrentVehicle.IsEngineOn)
            {
                IsEngineRunning = true;
            }
            else
            {
                IsEngineRunning = false;


                if(Game.LocalPlayer.Character.CurrentVehicle.MustBeHotwired)
                {
                    if(Game.LocalPlayer.Character.SeatIndex == -1)
                        GameTimeStartedHotwiring = Game.GameTime;
                    else
                        GameTimeStartedHotwiring = Game.GameTime + 2000;
                }
            }
        }
        else
        {
            if(Game.LocalPlayer.Character.LastVehicle.Exists())
                Game.LocalPlayer.Character.LastVehicle.IsEngineOn = IsEngineRunning;
        }
        WasinVehicle = IsPlayerInVehicle;
    }  
    private static void StartEngineAnimation()
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
                if (Game.IsControlJustPressed(0, GameControl.VehicleExit) || !Game.LocalPlayer.Character.IsInAnyVehicle(false))
                {
                    Cancel = true;
                    NativeFunction.CallByName<bool>("STOP_ANIM_TASK", Game.LocalPlayer.Character, sDict, "start_engine", 8.0f);
                    TogglingEngine = false;
                }
                GameFiber.Sleep(200);
            }
            if(Cancel)
            {
                TogglingEngine = false;
            }
        });
    }
}

