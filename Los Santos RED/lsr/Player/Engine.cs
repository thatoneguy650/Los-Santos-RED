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
    private bool IsToggling;
    public bool IsRunning { get; private set; }
    public bool CanToggleEngine
    {
        get
        {
            if (IsToggling)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    public Engine()
    {

    }
    public void Update(VehicleExt VehicleToMonitor)
    {
        if (!IsRunning)
        {
            VehicleToMonitor.Vehicle.IsDriveable = false;
        }
        else
        {
            VehicleToMonitor.Vehicle.IsDriveable = true;
            VehicleToMonitor.Vehicle.IsEngineOn = true;
        }
    }
    public void ToggleEngine(VehicleExt VehicleToMonitor,Ped Driver, bool PerformAnimation, bool DesiredEngineStatus)
    {
        if (IsToggling)
        {
            return;
        }

        Mod.Debug.WriteToLog("ToggleEngine", string.Format("Start {0}", IsRunning));
        IsToggling = true;
        if (VehicleToMonitor.Vehicle.Speed > 4f)
        {
            IsToggling = false;
            return;
        }

        if (Mod.Player.IsHotwiring)
        {
            IsToggling = false;
            return;
        }

        if (PerformAnimation)
        {
            StartEngineAnimation(Driver);
        }
        else
        {
            IsToggling = false;
        }

        if (!DesiredEngineStatus)
        {
            IsRunning = false;
        }
        else
        {
            IsRunning = true;
        }  
        
        Mod.Debug.WriteToLog("ToggleEngine", string.Format("End {0}", IsRunning));
    }
    private void StartEngineAnimation(Ped Driver)
    {
        GameFiber.StartNew(delegate
        {
            var sDict = "veh@van@ds@base";
            NativeFunction.CallByName<bool>("REQUEST_ANIM_DICT", sDict);
            while (!NativeFunction.CallByName<bool>("HAS_ANIM_DICT_LOADED", sDict))
                GameFiber.Yield();
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Driver, sDict, "start_engine", 2.0f, -2.0f, -1, 48, 0, true, false, true);

            bool Cancel = false;
            uint GameTimeStartedAnimation = Game.GameTime;
            while (Game.GameTime - GameTimeStartedAnimation <= 1000)
            {
                if (Game.IsControlJustPressed(0, GameControl.VehicleExit) || !Mod.Player.IsInVehicle)
                {
                    Cancel = true;
                    NativeFunction.CallByName<bool>("STOP_ANIM_TASK", Driver, sDict, "start_engine", 8.0f);
                    IsToggling = false;
                }
                GameFiber.Sleep(200);
            }
            IsToggling = false;
        });
    }
}

