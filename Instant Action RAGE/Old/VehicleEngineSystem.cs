using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Instant_Action_RAGE.Systems
{
    internal static class VehicleEngineSystem
    {
        private static bool EngineRunning;

        private static bool WasGettingInVehicle;
        private static bool WasinVehicle;
        private static bool needsHotwiring;
        private static bool needsToUnlock;

        public static bool AutoTune { get; private set; } = true;
        public static bool SetLoud { get; private set; } = true;
        public static RadioStation AutoTuneStation { get; private set; } = RadioStation.SelfRadio;
        public static Keys EngineToggleKey { get; private set; } = Keys.R;
        public static bool Enabled { get; set; } = true;
        public static bool IsRunning { get; set; } = true;
        public static void Initialize()
        {
            if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && !Game.LocalPlayer.Character.IsInHelicopter && !Game.LocalPlayer.Character.IsInPlane && !Game.LocalPlayer.Character.IsInBoat)
            {
                if(Game.LocalPlayer.Character.CurrentVehicle != null)
                    EngineRunning = Game.LocalPlayer.Character.CurrentVehicle.IsEngineOn;
            }
            AutoTuneStation = RadioStation.SelfRadio;
            AutoTune = true;
            EngineToggleKey = Keys.R;
            MainLoop();
        }
        public static void MainLoop()
        {
            GameFiber.StartNew(delegate
            {
                try
                {
                    while (IsRunning)
                    {
                        if (Game.IsKeyDown(EngineToggleKey))
                            ToggleEngine(false);



                        //if (Game.IsKeyDown(Keys.NumPad4))
                        //    WriteToLog("Button Press", string.Format("IsInAnyVehicle Currently : {0}", Game.LocalPlayer.Character.IsInAnyVehicle(true)));

                        //if (Game.IsKeyDown(Keys.NumPad5))
                        //    WriteToLog("Button Press", "What?");

                        if (Game.LocalPlayer.Character.IsGettingIntoVehicle)
                        {
                            if (Game.LocalPlayer.Character.VehicleTryingToEnter == null)
                                return;
                            if (Game.LocalPlayer.Character.VehicleTryingToEnter.IsEngineOn)
                                EngineRunning = true;
                            if (Game.LocalPlayer.Character.VehicleTryingToEnter.MustBeHotwired)
                                needsHotwiring = true;
                            if (Game.LocalPlayer.Character.VehicleTryingToEnter.LockStatus == VehicleLockStatus.LockedButCanBeBrokenInto)
                                needsToUnlock = true;
                        }

                        if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && !Game.LocalPlayer.Character.IsInHelicopter && !Game.LocalPlayer.Character.IsInPlane && !Game.LocalPlayer.Character.IsInBoat)
                        {
                            //if (Game.LocalPlayer.Character.CurrentVehicle == null)
                            //    return;

                            if (Game.LocalPlayer.Character.CurrentVehicle.IsEngineOn != EngineRunning)
                                Game.LocalPlayer.Character.CurrentVehicle.IsEngineOn = EngineRunning;
                            if (SetLoud)
                                NativeFunction.CallByName<bool>("SET_VEHICLE_RADIO_LOUD", Game.LocalPlayer.Character.CurrentVehicle, !EngineRunning);
                            if (!EngineRunning)
                            {
                                Game.LocalPlayer.Character.CurrentVehicle.RadioStation = RadioStation.Off;
                                Game.LocalPlayer.Character.CurrentVehicle.IsDriveable = false;
                            }
                            else
                            {
                                if (AutoTune)
                                    Game.LocalPlayer.Character.CurrentVehicle.RadioStation = AutoTuneStation;

                                Game.LocalPlayer.Character.CurrentVehicle.IsDriveable = true;
                            }
                        }

                        if (WasinVehicle != Game.LocalPlayer.Character.IsInAnyVehicle(false) && !Game.LocalPlayer.Character.IsInHelicopter && !Game.LocalPlayer.Character.IsInPlane && !Game.LocalPlayer.Character.IsInBoat)
                            VehicleExitEnterEvent();

                        //if (WasGettingInVehicle != Game.LocalPlayer.Character.IsGettingIntoVehicle && Game.LocalPlayer.Character.IsGettingIntoVehicle)
                        //    VehiclePreEnterEvent();

                        WasGettingInVehicle = Game.LocalPlayer.Character.IsGettingIntoVehicle;



                        GameFiber.Yield();
                    }
                }
                catch(Exception e)
                {
                    WriteToLog("ToggleEngine", e.Message);
                }
            });
        }
        private static void ToggleEngine(bool _animation)
        {
            WriteToLog("ToggleEngine", "Toggle Start");
            if (!Enabled)
                return;
            
            
            if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && !Game.LocalPlayer.Character.IsInHelicopter && !Game.LocalPlayer.Character.IsInPlane && !Game.LocalPlayer.Character.IsInBoat)
            {
                if (Game.LocalPlayer.Character.CurrentVehicle.Speed > 4f)
                    return;
                EngineRunning = !Game.LocalPlayer.Character.CurrentVehicle.IsEngineOn;
                //if (!Game.LocalPlayer.Character.IsOnBike && _animation)
                //    StartEngineAnimation();
            }
            WriteToLog("ToggleEngine", "toggled");
        }
        public static void TurnOffEngine()
        {
            if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && !Game.LocalPlayer.Character.IsInHelicopter && !Game.LocalPlayer.Character.IsInPlane && !Game.LocalPlayer.Character.IsInBoat)
            {
                if (Game.LocalPlayer.Character.CurrentVehicle.Speed > 4f)
                    return;
                //if (EngineRunning && !Game.LocalPlayer.Character.IsOnBike)
                //    StartEngineAnimation();
                EngineRunning = false;
                WriteToLog("ToggleEngine", "Turned Off");
            }
        }
        private static void StartEngineAnimation()
        {
            var sDict = "veh@van@ds@base";
            NativeFunction.CallByName<bool>("REQUEST_ANIM_DICT", sDict);
            while (!NativeFunction.CallByName<bool>("HAS_ANIM_DICT_LOADED", sDict))
                GameFiber.Yield();
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, sDict, "start_engine", 2.0f, -2.0f, -1, 48, 0, true, false, true);
            GameFiber.Sleep(1250);// is there a way to wait for the animation to finish?
        }
        private static void VehicleExitEnterEvent()
        {
            if (WasinVehicle) //Just got out
            {
                EngineRunning = false; // set to false/unknown state
                WriteToLog("VehicleExitEnterEvent", "Got Out");
            }
            else
            {
                if (needsHotwiring)
                {
                    //GameFiber.Sleep(3500); // Let the hotwire animation play
                    EngineRunning = true;
                    needsHotwiring = false;
                }
                WriteToLog("VehicleExitEnterEvent", "Got In");
            }

            WriteToLog("VehicleExitEnterEvent", string.Format("Currently : {0}", Game.LocalPlayer.Character.IsInAnyVehicle(false)));
            WriteToLog("VehicleExitEnterEvent", string.Format("Previously : {0}",WasinVehicle));



            WasinVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
        }
        public static void AfterPedTakeover()
        {
            EngineRunning = true;
        }
        private static void VehiclePreEnterEvent()
        {
            if (needsToUnlock && Game.LocalPlayer.WantedLevel == 0)
            {
                WriteToLog("VehiclePreEnterEvent", "Needs to Unlock");
                Vehicle AttemptingToEnter = Game.LocalPlayer.Character.VehicleTryingToEnter;
                AttemptingToEnter.LockStatus = VehicleLockStatus.Unlocked;
                GameFiber.Sleep(100);

                Game.LocalPlayer.Character.Tasks.EnterVehicle(AttemptingToEnter, 0);
                GameFiber.Sleep(500);
                Game.LocalPlayer.Character.Tasks.ClearImmediately();


                var sDict = "veh@break_in@0h@p_m_one@";
                NativeFunction.CallByName<bool>("REQUEST_ANIM_DICT", sDict);
                while (!NativeFunction.CallByName<bool>("HAS_ANIM_DICT_LOADED", sDict))
                    GameFiber.Yield();

                NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, sDict, "std_force_entry_ps", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                GameFiber.Sleep(1500);

                Game.LocalPlayer.Character.Tasks.EnterVehicle(AttemptingToEnter, 0);
                WriteToLog("VehiclePreEnterEvent", "Done");
                needsToUnlock = false;
            }
        }

        private static void WriteToLog(String ProcedureString, String TextToLog)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ": " + ProcedureString + ": " + TextToLog + System.Environment.NewLine);
            File.AppendAllText("Plugins\\InstantAction\\" + "log.txt", sb.ToString());
            sb.Clear();
        }
    }
}
