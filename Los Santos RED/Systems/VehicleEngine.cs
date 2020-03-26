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


internal static class VehicleEngine
{
    private static bool EngineRunning;
    private static bool PrevEngineRunning;
    private static bool WasinVehicle;
    private static bool TogglingEngine;
    //private static bool ChangingStation;
    private static uint GameTimeStartedHotwiring;
    private static bool PrevIsHotwiring;

    private static bool LeftBlinkerStartedTurn;
    private static bool RightBlinkerStartedTurn;
    private static int TimeWheelsTurnedRight;
    private static int TimeWheelsTurnedLeft;
    private static int TimeWheelsStraight;

    //public static string AutoSetRadioStation = "NONE";
    // private static bool PrevWantedLevelTune = false;
    //private static List<string> strRadioStations = new List<string> { "RADIO_01_CLASS_ROCK", "RADIO_02_POP", "RADIO_03_HIPHOP_NEW", "RADIO_04_PUNK", "RADIO_05_TALK_01", "RADIO_06_COUNTRY", "RADIO_07_DANCE_01", "RADIO_08_MEXICAN", "RADIO_09_HIPHOP_OLD", "RADIO_12_REGGAE", "RADIO_13_JAZZ", "RADIO_14_DANCE_02", "RADIO_15_MOTOWN", "RADIO_20_THELAB", "RADIO_16_SILVERLAKE", "RADIO_17_FUNK", "RADIO_18_90S_ROCK", "RADIO_19_USER", "RADIO_11_TALK_02", "HIDDEN_RADIO_AMBIENT_TV_BRIGHT", "OFF" };
    public static bool AutoTune { get; private set; }
    public static string AutoTuneStation { get; set; }
    public static Keys EngineToggleKey { get; private set; }
    public static bool IsRunning { get; set; }
    public static bool WantedLevelTune { get; set; }

    public static bool LeftBlinkerOn { get; set; }
    public static bool RightBlinkerOn { get; set; }
    public static bool HazardsOn { get; set; }
    public static string VehicleIndicatorStatus
    {
        get
        {
            if (LeftBlinkerOn)
                return " (LI)";
            else if (RightBlinkerOn)
                return " (RI)";
            else if (HazardsOn)
                return " (HAZ)";
            else
                return "";
        }
    }
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
        IsRunning = true;
        EngineRunning = false;
        PrevEngineRunning = false;
        WasinVehicle = false;
        TogglingEngine = false;
        GameTimeStartedHotwiring = 0;
        PrevIsHotwiring = false; 
        AutoTune = true;
        AutoTuneStation = "RADIO_19_USER";
        EngineToggleKey = Keys.R;

        WantedLevelTune = false;
        if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && !Game.LocalPlayer.Character.IsInHelicopter && !Game.LocalPlayer.Character.IsInPlane && !Game.LocalPlayer.Character.IsInBoat)
        {
            if(Game.LocalPlayer.Character.CurrentVehicle != null)
                EngineRunning = Game.LocalPlayer.Character.CurrentVehicle.IsEngineOn;
        }      
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void VehicleEngineTick()
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
            if (Game.LocalPlayer.Character.IsInAnyPoliceVehicle && EngineRunning)
            {
                NativeFunction.CallByName<bool>("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", true);
            }
            else if (!EngineRunning)
            {
                NativeFunction.CallByName<bool>("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", false);
            }

            if (!TogglingEngine && Game.IsKeyDown(EngineToggleKey))
            {
                Debugging.WriteToLog("ToggleEngine", string.Format("Start {0}", EngineRunning));
                TogglingEngine = true;
                ToggleEngine(true, !EngineRunning);
                Debugging.WriteToLog("ToggleEngine", string.Format("End {0}", EngineRunning));
            }

            if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && !Game.LocalPlayer.Character.IsInHelicopter && !Game.LocalPlayer.Character.IsInPlane && !Game.LocalPlayer.Character.IsInBoat)
            {
                if (!EngineRunning)
                {
                    Game.LocalPlayer.Character.CurrentVehicle.IsDriveable = false;
                }
                else
                {
                    Game.LocalPlayer.Character.CurrentVehicle.IsDriveable = true;
                    Game.LocalPlayer.Character.CurrentVehicle.IsEngineOn = true;
                }
            }

            if (AutoTuneStation.ToUpper() != "NONE")
            {
                string RadioStationLastTuned = "OFF";
                unsafe
                {
                    IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_PLAYER_RADIO_STATION_NAME");
                    RadioStationLastTuned = Marshal.PtrToStringAnsi(ptr);
                }
                if (RadioStationLastTuned != AutoTuneStation)
                {
                    NativeFunction.CallByName<bool>("SET_VEH_RADIO_STATION", Game.LocalPlayer.Character.CurrentVehicle, AutoTuneStation);
                }
            }
            IndicatorsTick();
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
    }
    private static void IndicatorsTick()
    {
        Vehicle MyCar = Game.LocalPlayer.Character.CurrentVehicle;
        if (MyCar == null || !MyCar.Exists())
            return;

        if (Game.IsKeyDown(Keys.Space) && Game.IsShiftKeyDownRightNow)
        {
            if (HazardsOn)
            {
                MyCar.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Off;
                HazardsOn = false;
            }
            else
            {
                MyCar.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Both;
                HazardsOn = true;
                LeftBlinkerOn = false;
                RightBlinkerOn = false;
                return;
            }
        }

        
        RightBlinkerTick(MyCar);
        LeftBlinkerTick(MyCar);


       // UI.DebugLine = string.Format("LOn: {0},LTime: {1}, LStart {2},ROn: {3},RTime: {4},RStart: {5},STime: {6},Hazard: {7},Angle: {8}", LeftBlinkerOn, TimeWheelsTurnedRight, LeftBlinkerStartedTurn, RightBlinkerOn, TimeWheelsTurnedRight, RightBlinkerStartedTurn, TimeWheelsStraight,HazardsOn, MyCar.SteeringAngle);
    }

    private static void RightBlinkerTick(Vehicle MyCar)
    {
        if (Game.IsKeyDown(Keys.E) && Game.IsShiftKeyDownRightNow)
        {
            if (RightBlinkerOn)
            {
                MyCar.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Off;
                RightBlinkerOn = false;
            }
            else
            {
                MyCar.IndicatorLightsStatus = VehicleIndicatorLightsStatus.RightOnly;
                RightBlinkerOn = true;
                LeftBlinkerOn = false;
                HazardsOn = false;
            }
        }
        if (RightBlinkerOn)
        {
            if (MyCar.SteeringAngle <= -25f)
                TimeWheelsTurnedRight++;
            else
                TimeWheelsTurnedRight = 0;

            if (TimeWheelsTurnedRight >= 20)
            {
                RightBlinkerStartedTurn = true;
            }

        }
        if (RightBlinkerOn && RightBlinkerStartedTurn)
        {
            if (MyCar.SteeringAngle > -10f)
                TimeWheelsStraight++;
            else
                TimeWheelsStraight = 0;
        }
        if (RightBlinkerOn && TimeWheelsStraight >= 20)
        {
            TimeWheelsTurnedRight = 0;
            TimeWheelsStraight = 0;
            RightBlinkerStartedTurn = false;
            MyCar.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Off;
            RightBlinkerOn = false;
        }
    }
    private static void LeftBlinkerTick(Vehicle MyCar)
    {
        if (Game.IsKeyDown(Keys.Q) && Game.IsShiftKeyDownRightNow)
        {
            if (LeftBlinkerOn)
            {
                MyCar.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Off;
                LeftBlinkerOn = false;
            }
            else
            {
                MyCar.IndicatorLightsStatus = VehicleIndicatorLightsStatus.LeftOnly;
                LeftBlinkerOn = true;
                RightBlinkerOn = false;
                HazardsOn = false;
            }
        }
        if (LeftBlinkerOn)
        {
            if (MyCar.SteeringAngle >= 25f)
                TimeWheelsTurnedLeft++;
            else
                TimeWheelsTurnedLeft = 0;

            if (TimeWheelsTurnedLeft >= 20)
            {
                LeftBlinkerStartedTurn = true;
            }

        }
        if (LeftBlinkerOn && LeftBlinkerStartedTurn)
        {
            if (MyCar.SteeringAngle < 10f)
                TimeWheelsStraight++;
            else
                TimeWheelsStraight = 0;
        }
        if (LeftBlinkerOn && TimeWheelsStraight >= 20)
        {
            TimeWheelsTurnedLeft = 0;
            TimeWheelsStraight = 0;
            LeftBlinkerStartedTurn = false;
            MyCar.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Off;
            LeftBlinkerOn = false;
        }
    }
    public static void TurnOffEngine()
    {
        ToggleEngine(false, false);
    }
    private static void IsHotWiringChanged()
    {
        if(IsHotwiring)
        {

        }
        else
        {
            if(Game.LocalPlayer.Character.IsInAnyVehicle(false))
                EngineRunning = true;
        }
        PrevIsHotwiring = IsHotwiring;
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
            if (Game.LocalPlayer.Character.CurrentVehicle.IsEngineOn)
            {
                EngineRunning = true;
            }
            else
            {
                EngineRunning = false;


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
            HazardsOn = false;

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
                StartEngineAnimation();
            }
            if (Game.LocalPlayer.Character.IsInAnyVehicle(false))
            {
                if (!DesiredEngineStatus)
                    EngineRunning = false;
                else
                    EngineRunning = true;
            }
        }
        //LocalWriteToLog("ToggleEngine", "toggled");
        TogglingEngine = false;
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
                ChangeStationAnimation(StationName);
            }
            else
            {
                SetRadioStation(StationName);
            }
        }
    }
    private static void ChangeStationAnimation(string StationName)
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
    private static void SetRadioStation(string  StationName)
    {
        if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.IsEngineOn)
        {
            NativeFunction.CallByName<bool>("SET_VEH_RADIO_STATION", Game.LocalPlayer.Character.CurrentVehicle, StationName);
        }
    }
}

