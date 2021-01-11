using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Extensions = ExtensionsMethods.Extensions;
namespace LosSantosRED.lsr
{
    public class Input
    {
        private IInputable Player;
        private ISettingsProvideable Settings;
        public Input(IInputable player, ISettingsProvideable settings)
        {
            Player = player;
            Settings = settings;
        }
        private uint GameTimeStartedHoldingEnter;
        private bool IsHoldingEnter
        {
            get
            {
                if (GameTimeStartedHoldingEnter == 0)
                    return false;
                if (Game.GameTime - GameTimeStartedHoldingEnter >= 75)
                    return true;
                return false;
            }
        }
        private bool IsPressingSurrender
        {
            get
            {
                if (Game.IsKeyDownRightNow(Settings.SettingsManager.KeyBinding.SurrenderKey) && Game.IsShiftKeyDownRightNow && !Game.IsControlKeyDownRightNow)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        private bool IsPressingConversation
        {
            get
            {
                if (Game.IsKeyDownRightNow(Keys.H))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        private bool IsPressingRefuel
        {
            get
            {
                if (Game.IsKeyDownRightNow(Settings.SettingsManager.KeyBinding.SurrenderKey) && !Game.IsShiftKeyDownRightNow && !Game.IsControlKeyDownRightNow && Game.LocalPlayer.Character.IsInAnyVehicle(false))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        private bool IsPressingDropWeapon
        {
            get
            {
                if (Game.IsKeyDownRightNow(Settings.SettingsManager.KeyBinding.DropWeaponKey) && !Game.IsControlKeyDownRightNow)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        private bool IsPressingEngineToggle
        {
            get
            {
                if (Game.IsKeyDownRightNow(Settings.SettingsManager.KeyBinding.VehicleKey) && !Game.IsControlKeyDownRightNow)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool IsPressingRightIndicator
        {
            get
            {
                if (Game.IsKeyDown(Keys.E) && Game.IsShiftKeyDownRightNow)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        private bool IsPressingLeftIndicator
        {
            get
            {
                if (Game.IsKeyDown(Keys.Q) && Game.IsShiftKeyDownRightNow)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        private bool IsPressingHazards
        {
            get
            {
                if (Game.IsKeyDown(Keys.Space) && Game.IsShiftKeyDownRightNow)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        private bool IsPressingNextTrack
        {
            get
            {
                if (Game.IsKeyDown(Keys.O) && !Game.IsShiftKeyDownRightNow)
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
            SurrenderCheck();
            WeaponDropCheck();
            VehicleCheck();
            HoldingEnterCheck();
            ConversationCheck();

            Player.IsHoldingEnter = IsHoldingEnter;
        }
        private void HoldingEnterCheck()
        {
            if (Game.IsControlPressed(2, GameControl.Enter))
            {
                if (GameTimeStartedHoldingEnter == 0)
                    GameTimeStartedHoldingEnter = Game.GameTime;
            }
            else
            {
                GameTimeStartedHoldingEnter = 0;
            }
        }
        private void SurrenderCheck()
        {
            if (IsPressingSurrender && Player.CanSurrender)
            {
                if (!Player.HandsAreUp && !Player.IsBusted)
                {
                    Player.RaiseHands();
                }
            }
            else
            {
                if (Player.HandsAreUp && !Player.IsBusted)
                {
                    Player.LowerHands();
                }
            }
        }
        private void ConversationCheck()
        {
            if (Player.CanConverse)
            {
                Game.DisplayHelp("Press H to Talk");
                if (IsPressingConversation)
                {
                    Player.StartConversation();
                }
            }
        }
        private void WeaponDropCheck()
        {
            if (IsPressingDropWeapon && Player.CanDropWeapon)
            {
                Player.DropWeapon();
            }
        }
        private void VehicleCheck()
        {
            if (Player.CurrentVehicle != null)
            {
                //if (IsPressingEngineToggle)
                //{
                //    GameFiber.Sleep(200);
                //    Player.CurrentVehicle.Vehicle.IsEngineOn = !Player.CurrentVehicle.Vehicle.IsEngineOn;
                //}
                //if (IsPressingRefuel &&  Mod.Player.Instance.GetCash() >= 1)
                //{
                //    Mod.Player.Instance.GiveCash(-1);
                //    Mod.Player.Instance.CurrentVehicle.FuelTank.PumpFuel();
                //    GameFiber.Sleep(100);
                //}
                if (IsPressingNextTrack)
                {
                    Player.CurrentVehicle.Radio.SetNextTrack();
                    GameFiber.Sleep(100);
                }
                if (IsPressingHazards)
                {
                    Player.CurrentVehicle.Indicators.ToggleHazards();
                    GameFiber.Sleep(500);
                }
                if (IsPressingLeftIndicator)
                {
                    Player.CurrentVehicle.Indicators.ToggleLeft();
                    GameFiber.Sleep(500);
                }
                if (IsPressingRightIndicator)
                {
                    Player.CurrentVehicle.Indicators.ToggleRight();
                    GameFiber.Sleep(500);
                }
            }
        }
        private void ToggleEngineAnimation()
        {
            GameFiber.StartNew(delegate
            {
                var sDict = "veh@van@ds@base";
                NativeFunction.CallByName<bool>("REQUEST_ANIM_DICT", sDict);
                while (!NativeFunction.CallByName<bool>("HAS_ANIM_DICT_LOADED", sDict))
                    GameFiber.Yield();
                NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, sDict, "start_engine", 2.0f, -2.0f, -1, 48, 0, true, false, true);

                uint GameTimeStartedAnimation = Game.GameTime;
                while (Game.GameTime - GameTimeStartedAnimation <= 1000)
                {
                    if (Game.IsControlJustPressed(0, GameControl.VehicleExit) || !Player.IsInVehicle)
                    {
                        NativeFunction.CallByName<bool>("STOP_ANIM_TASK", Game.LocalPlayer.Character, sDict, "start_engine", 8.0f);
                    }
                    GameFiber.Sleep(200);
                }
            });
        }
        //private void ChangeStationAnimation(string StationName)
        //{
        //    GameFiber.StartNew(delegate
        //    {
        //        var sDict = "veh@van@ds@base";
        //        NativeFunction.CallByName<bool>("REQUEST_ANIM_DICT", sDict);
        //        while (!NativeFunction.CallByName<bool>("HAS_ANIM_DICT_LOADED", sDict))
        //            GameFiber.Yield();
        //        NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, sDict, "start_engine", 2.0f, -2.0f, -1, 48, 0, true, false, true);

        //        bool Cancel = false;
        //        uint GameTimeStartedAnimation = Game.GameTime;
        //        while (Game.GameTime - GameTimeStartedAnimation <= 1000)
        //        {
        //            if (Game.IsControlJustPressed(0, GameControl.VehicleExit))
        //            {
        //                NativeFunction.CallByName<bool>("STOP_ANIM_TASK", Game.LocalPlayer.Character, sDict, "start_engine", 8.0f);
        //                Cancel = true;
        //            }
        //            GameFiber.Sleep(200);
        //        }
        //        if (!Cancel)
        //            SetRadioStation(StationName);

        //    });
        //}
    }
}