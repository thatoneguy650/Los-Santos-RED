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
        private static bool PrevEngineRunning;

        private static bool WasGettingInVehicle;
        private static bool WasinVehicle;
        private static bool needsHotwiring;
        private static bool needsToUnlock;
        private static uint GameTimeStartedExit;
        private static bool TogglingEngine;
        private static bool PrevMustBeHotwired;
        private static uint GameTimeStartedHotwiring;
        private static bool PrevIsHotwiring;

        public static bool AutoTune { get; private set; } = true;
        public static bool SetLoud { get; private set; } = true;
        public static RadioStation AutoTuneStation { get; private set; } = RadioStation.SelfRadio;
        public static Keys EngineToggleKey { get; private set; } = Keys.R;
        public static bool Enabled { get; set; } = true;
        public static bool IsRunning { get; set; } = true;
        public static bool IsHotwiring
        {
            get
            {
                if (GameTimeStartedHotwiring == 0)
                    return false;
                else if (Game.GameTime - GameTimeStartedHotwiring <= 12000)
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

                        bool PlayerInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);

                        if (WasinVehicle != PlayerInVehicle)
                        {
                            EnterExitVehicleEvent(PlayerInVehicle);
                        }

                        if (PlayerInVehicle)
                        {

                            if (!TogglingEngine && Game.IsKeyDown(EngineToggleKey))
                            {
                                ToggleEngine(true,false);
                            }
                            if (!EngineRunning)
                            {
                                Game.LocalPlayer.Character.CurrentVehicle.IsDriveable = false;
                            }
                            else
                            {
                                Game.LocalPlayer.Character.CurrentVehicle.IsDriveable = true;
                                Game.LocalPlayer.Character.CurrentVehicle.IsEngineOn = true;
                            }


                            bool MustBeHotwired = Game.LocalPlayer.Character.CurrentVehicle.MustBeHotwired;
                            if (PrevMustBeHotwired != MustBeHotwired)
                            {
                                if (MustBeHotwired)
                                {
                                    GameTimeStartedHotwiring = Game.GameTime;
                                }
                                else
                                {
                                    EngineRunning = true;
                                    GameTimeStartedHotwiring = 0;
                                }
                                PrevMustBeHotwired = MustBeHotwired;
                                InstantAction.WriteToLog("ToggleEngine", string.Format("MustBeHotwired: {0}", MustBeHotwired));
                            }

;
                            if (PrevIsHotwiring != IsHotwiring)
                            {

                                PrevIsHotwiring = IsHotwiring;
                                InstantAction.WriteToLog("ToggleEngine", string.Format("IsHotwiring: {0}", IsHotwiring));
                            }


                        }
                        else
                        {
                            GameTimeStartedHotwiring = 0;
                        }                     
                        if (PrevEngineRunning != EngineRunning)
                        {
                            EngineRunningEvent();
                        }

                        GameFiber.Yield();
                    }
                }
                catch(Exception e)
                {
                    InstantAction.WriteToLog("ToggleEngine", string.Format("{0},{1}", e.Message,e.StackTrace));
                }
            });
        }

        private static void EngineRunningEvent()
        {
            InstantAction.WriteToLog("ToggleEngine", string.Format("EngineRunning: {0}",EngineRunning));
            PrevEngineRunning = EngineRunning;
        }

        public static void EnterExitVehicleEvent(bool PlayerInVehicle)
        {
            if(PlayerInVehicle)
            {
                InstantAction.WriteToLog("EnterExitVehicleEvent", "You got into a vehicle");
                if (Game.LocalPlayer.Character.CurrentVehicle.IsEngineOn)
                {
                    EngineRunning = true;
                    InstantAction.WriteToLog("EnterExitVehicleEvent", "The Engine was already on");
                }
                else
                {
                    EngineRunning = false;


                    if(Game.LocalPlayer.Character.CurrentVehicle.MustBeHotwired)
                    {
                        InstantAction.WriteToLog("EnterExitVehicleEvent", "The Engine was off and Needed Hotwire");
                    }

                    InstantAction.WriteToLog("EnterExitVehicleEvent", "The Engine was off");
                }
            }
            else
            {
                InstantAction.WriteToLog("EnterExitVehicleEvent", "You got out of a vehicle");
                Game.LocalPlayer.Character.LastVehicle.IsEngineOn = EngineRunning;
            }
            WasinVehicle = PlayerInVehicle;
        }


        public static void MainLoopOLD()
        {
            GameFiber.StartNew(delegate
            {
                try
                {
                    while (IsRunning)
                    {
                        if (Game.IsKeyDown(EngineToggleKey))
                            ToggleEngine(false,false);



                        //if (Game.IsKeyDown(Keys.NumPad4))
                        //    InstantAction.WriteToLog("Button Press", string.Format("IsInAnyVehicle Currently : {0}", Game.LocalPlayer.Character.IsInAnyVehicle(true)));

                        //if (Game.IsKeyDown(Keys.NumPad5))
                        //    InstantAction.WriteToLog("Button Press", "What?");

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
                                //Game.LocalPlayer.Character.CurrentVehicle.RadioStation = RadioStation.Off;
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
                catch (Exception e)
                {
                    InstantAction.WriteToLog("ToggleEngine", e.Message);
                }
            });
        }
        private static void ToggleEngine(bool _animation,bool OnlyOff)
        {                 
            if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && !Game.LocalPlayer.Character.IsInHelicopter && !Game.LocalPlayer.Character.IsInPlane && !Game.LocalPlayer.Character.IsInBoat)
            {
                if (Game.LocalPlayer.Character.CurrentVehicle.Speed > 4f)
                    return;

                if (IsHotwiring)
                    return;

                if (!Game.LocalPlayer.Character.IsOnBike && _animation)
                {
                    TogglingEngine = true;
                    StartEngineAnimation();
                }

                if (OnlyOff)
                    EngineRunning = false;
                else
                    EngineRunning = !Game.LocalPlayer.Character.CurrentVehicle.IsEngineOn;
            }
            InstantAction.WriteToLog("ToggleEngine", "toggled");
            TogglingEngine = false;
        }
       

        private static void StartEngineAnimation()
        {
            var sDict = "veh@van@ds@base";
            NativeFunction.CallByName<bool>("REQUEST_ANIM_DICT", sDict);
            while (!NativeFunction.CallByName<bool>("HAS_ANIM_DICT_LOADED", sDict))
                GameFiber.Yield();
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, sDict, "start_engine", 2.0f, -2.0f, -1, 48, 0, true, false, true);
            GameFiber.Sleep(1000);// is there a way to wait for the animation to finish?
        }
        private static void VehicleExitEnterEvent()
        {
            if (WasinVehicle) //Just got out
            {
                EngineRunning = false; // set to false/unknown state
                InstantAction.WriteToLog("VehicleExitEnterEvent", "Got Out");
            }
            else
            {
                if (needsHotwiring)
                {
                    //GameFiber.Sleep(3500); // Let the hotwire animation play
                    EngineRunning = true;
                    needsHotwiring = false;
                }
                InstantAction.WriteToLog("VehicleExitEnterEvent", "Got In");
            }

            InstantAction.WriteToLog("VehicleExitEnterEvent", string.Format("Currently : {0}", Game.LocalPlayer.Character.IsInAnyVehicle(false)));
            InstantAction.WriteToLog("VehicleExitEnterEvent", string.Format("Previously : {0}",WasinVehicle));



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
                InstantAction.WriteToLog("VehiclePreEnterEvent", "Needs to Unlock");
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
                InstantAction.WriteToLog("VehiclePreEnterEvent", "Done");
                needsToUnlock = false;
            }
        }

    }
}
