using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


internal static class VehicleEngine
{
    private static bool EngineRunning;
    private static bool PrevEngineRunning;
    private static bool WasinVehicle;
    private static bool TogglingEngine;
    private static bool ChangingStation;
    private static uint GameTimeStartedHotwiring;
    private static bool PrevIsHotwiring;
    private static bool PrevWantedLevelTune = false;
    //private static List<string> strRadioStations = new List<string> { "RADIO_01_CLASS_ROCK", "RADIO_02_POP", "RADIO_03_HIPHOP_NEW", "RADIO_04_PUNK", "RADIO_05_TALK_01", "RADIO_06_COUNTRY", "RADIO_07_DANCE_01", "RADIO_08_MEXICAN", "RADIO_09_HIPHOP_OLD", "RADIO_12_REGGAE", "RADIO_13_JAZZ", "RADIO_14_DANCE_02", "RADIO_15_MOTOWN", "RADIO_20_THELAB", "RADIO_16_SILVERLAKE", "RADIO_17_FUNK", "RADIO_18_90S_ROCK", "RADIO_19_USER", "RADIO_11_TALK_02", "HIDDEN_RADIO_AMBIENT_TV_BRIGHT", "OFF" };
    public static bool AutoTune { get; private set; } = true;
    public static string AutoTuneStation { get; set; } = "RADIO_19_USER";
    public static Keys EngineToggleKey { get; private set; } = Keys.R;
    public static bool IsRunning { get; set; } = true;
    public static bool WantedLevelTune { get; set; } = false;
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
    public static void Initialize()
    {
        if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && !Game.LocalPlayer.Character.IsInHelicopter && !Game.LocalPlayer.Character.IsInPlane && !Game.LocalPlayer.Character.IsInBoat)
        {
            if(Game.LocalPlayer.Character.CurrentVehicle != null)
                EngineRunning = Game.LocalPlayer.Character.CurrentVehicle.IsEngineOn;
        }
        AutoTuneStation = "RADIO_19_USER";
        AutoTune = true;
        EngineToggleKey = Keys.R;
        MainLoop();         
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void MainLoop()
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                while (IsRunning)
                {

                    bool PlayerInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);

                    if (WasinVehicle != PlayerInVehicle)
                    {
                        EnterExitVehicleEvent(PlayerInVehicle);
                    }

                    if (PrevIsHotwiring != IsHotwiring)
                        IsHotWiringChanged();

                    if (PlayerInVehicle)
                    {
                        if (Game.LocalPlayer.Character.IsInAnyPoliceVehicle)
                        {                        
                            NativeFunction.CallByName<bool>("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", true);
                        }

                        if (!TogglingEngine && Game.IsKeyDown(EngineToggleKey))
                        {
                            Debugging.WriteToLog("ToggleEngine", string.Format("Start {0}", EngineRunning));
                            TogglingEngine = true;
                            ToggleEngine(true, !EngineRunning);
                            Debugging.WriteToLog("ToggleEngine", string.Format("End {0}", EngineRunning));
                        }

                        if (Game.LocalPlayer.Character.IsInAnyVehicle(false))
                        {
                            if (!EngineRunning)
                            {
                                Game.LocalPlayer.Character.CurrentVehicle.IsDriveable = false;
                            }
                            else
                            {
                                Game.LocalPlayer.Character.CurrentVehicle.IsDriveable = true;
                                Game.LocalPlayer.Character.CurrentVehicle.IsEngineOn = true;
                                //if (WantedLevelTune)
                                //    NativeFunction.CallByName<bool>("SET_VEH_RADIO_STATION", Game.LocalPlayer.Character.CurrentVehicle, "RADIO_19_USER");
                            }
                        }
                    }
                    else
                    {
                        GameTimeStartedHotwiring = 0;
                        NativeFunction.CallByName<bool>("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", false);
                    }                     
                    if (PrevEngineRunning != EngineRunning)
                    {
                        EngineRunningEvent();
                    }
                    if(PrevWantedLevelTune != WantedLevelTune)
                    {
                        WantedLevelTuneEvent();
                    }
                    GameFiber.Yield();
                }
            }
            catch (Exception e)
            {
                InstantAction.Dispose();
                Debugging.WriteToLog("Error", e.Message + " : " + e.StackTrace);
            }
        });
    }

    private static void IsHotWiringChanged()
    {
        if(IsHotwiring)
        {

        }
        else
        {
            EngineRunning = true;//ToggleEngine(true, !EngineRunning);
        }
        PrevIsHotwiring = IsHotwiring;
    }

    private static void WantedLevelTuneEvent()
    {
        ChangeStation(VehicleEngine.AutoTuneStation);
        PrevWantedLevelTune = WantedLevelTune;
    }

    private static void EngineRunningEvent()
    {
        Debugging.WriteToLog("ToggleEngine", string.Format("EngineRunning: {0}",EngineRunning));
        PrevEngineRunning = EngineRunning;
    }
    public static void EnterExitVehicleEvent(bool PlayerInVehicle)
    {
        if(PlayerInVehicle)
        {
            Debugging.WriteToLog("EnterExitVehicleEvent", "You got into a vehicle");
            if (Game.LocalPlayer.Character.CurrentVehicle.IsEngineOn)
            {
                EngineRunning = true;
                Debugging.WriteToLog("EnterExitVehicleEvent", "The Engine was already on");
            }
            else
            {
                EngineRunning = false;


                if(Game.LocalPlayer.Character.CurrentVehicle.MustBeHotwired)
                {
                    GameTimeStartedHotwiring = Game.GameTime;
                    Debugging.WriteToLog("EnterExitVehicleEvent", "The Engine was off and Needed Hotwire");
                }

                Debugging.WriteToLog("EnterExitVehicleEvent", "The Engine was off");
            }
        }
        else
        {
            Debugging.WriteToLog("EnterExitVehicleEvent", "You got out of a vehicle");
            if(Game.LocalPlayer.Character.LastVehicle.Exists())
                Game.LocalPlayer.Character.LastVehicle.IsEngineOn = EngineRunning;
        }
        WasinVehicle = PlayerInVehicle;
    }  
    private static void ToggleEngine(bool _animation,bool DesiredEngineStatus)
    {                 
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

            if (!Game.LocalPlayer.Character.IsOnBike && _animation)
            {

                if (!StartEngineAnimation())
                {
                    TogglingEngine = false;
                    return;
                }
            }
            if (Game.LocalPlayer.Character.IsInAnyVehicle(false))
            {
                if (!DesiredEngineStatus)
                    EngineRunning = false;
                else
                    EngineRunning = true;
            }
        }
        Debugging.WriteToLog("ToggleEngine", "toggled");
        TogglingEngine = false;
    }
    private static bool StartEngineAnimation()
    {
        var sDict = "veh@van@ds@base";
        NativeFunction.CallByName<bool>("REQUEST_ANIM_DICT", sDict);
        while (!NativeFunction.CallByName<bool>("HAS_ANIM_DICT_LOADED", sDict))
            GameFiber.Yield();
        NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, sDict, "start_engine", 2.0f, -2.0f, -1, 48, 0, true, false, true);

        uint GameTimeStartedAnimation = Game.GameTime;
        while(Game.GameTime - GameTimeStartedAnimation <= 1000)
        {
            if(Game.IsControlJustPressed(0,GameControl.VehicleExit))
            {
                NativeFunction.CallByName<bool>("STOP_ANIM_TASK", Game.LocalPlayer.Character, sDict, "start_engine", 8.0f);
                TogglingEngine = false;
                return false;
            }
            GameFiber.Sleep(200);
        }
        return true;
        //GameFiber.Sleep(1000);// is there a way to wait for the animation to finish?
    }
    public static void ChangeStation(string StationName)
    {
        if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && !Game.LocalPlayer.Character.IsInHelicopter && !Game.LocalPlayer.Character.IsInPlane && !Game.LocalPlayer.Character.IsInBoat)
        {
            if (!Game.LocalPlayer.Character.CurrentVehicle.IsEngineOn)
                return;

            if (IsHotwiring)
                return;

            if (!Game.LocalPlayer.Character.IsOnBike)
            {
                ChangingStation = true;
                if (!ChangeStationAnimation())
                    return;
            }
            if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.IsEngineOn)
            {
                NativeFunction.CallByName<bool>("SET_VEH_RADIO_STATION", Game.LocalPlayer.Character.CurrentVehicle, StationName);
            }
        }
        Debugging.WriteToLog("ToggleEngine", "toggled");
        ChangingStation = false;
    }
    private static bool ChangeStationAnimation()
    {
        var sDict = "veh@van@ds@base";
        NativeFunction.CallByName<bool>("REQUEST_ANIM_DICT", sDict);
        while (!NativeFunction.CallByName<bool>("HAS_ANIM_DICT_LOADED", sDict))
            GameFiber.Yield();
        NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, sDict, "start_engine", 2.0f, -2.0f, -1, 48, 0, true, false, true);

        uint GameTimeStartedAnimation = Game.GameTime;
        while (Game.GameTime - GameTimeStartedAnimation <= 1000)
        {
            if (Game.IsControlJustPressed(0, GameControl.VehicleExit))
            {
                NativeFunction.CallByName<bool>("STOP_ANIM_TASK", Game.LocalPlayer.Character, sDict, "start_engine", 8.0f);
                ChangingStation = false;
                return false;
            }
            GameFiber.Sleep(200);
        }
        return true;
    }
}

