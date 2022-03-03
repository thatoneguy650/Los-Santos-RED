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
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Extensions = ExtensionsMethods.Extensions;
namespace LosSantosRED.lsr
{
    public class Input
    {
        private uint GameTimeLastPressedIndicators;
        private IInputable Player;
        private ISettingsProvideable Settings;
        private IMenuProvideable MenuProvider;
        private IPedSwap PedSwap;
        public Input(IInputable player, ISettingsProvideable settings, IMenuProvideable menuProvider, IPedSwap pedswap)
        {
            Player = player;
            Settings = settings;
            MenuProvider = menuProvider;
            PedSwap = pedswap;
        }
        private uint GameTimeLastPressedEngineToggle;
        private uint GameTimeLastPressedDoorClose;
        private uint GameTimeLastPressedSelectorToggle;
        private bool isCellPhoneControlDisabled;

        public bool IsPressingMenuKey => Game.IsKeyDown(Settings.SettingsManager.KeySettings.MenuKey);
        public bool IsPressingDebugMenuKey => Game.IsKeyDown(Settings.SettingsManager.KeySettings.DebugMenuKey);
        private bool IsPressingSurrender => IsKeyDownSafe(Settings.SettingsManager.KeySettings.SurrenderKey) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.SurrenderKeyModifier);
        private bool IsPressingDropWeapon => IsKeyDownSafe(Settings.SettingsManager.KeySettings.DropWeaponKey) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.DropWeaponKeyModifer);
        private bool IsPressingSprint => IsKeyDownSafe(Settings.SettingsManager.KeySettings.SprintKey) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.SprintKeyModifier);
        private bool IsPressingRightIndicator => IsKeyDownSafe(Settings.SettingsManager.KeySettings.RightIndicatorKey) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.RightIndicatorKeyModifer);
        public bool IsPressingEngineToggle => IsKeyDownSafe(Settings.SettingsManager.KeySettings.EngineToggle) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.EngineToggleModifier);// Game.IsKeyDown(Settings.SettingsManager.KeySettings.EngineToggle) && Game.IsShiftKeyDownRightNow;
        private bool IsPressingDoorClose => IsKeyDownSafe(Settings.SettingsManager.KeySettings.ManualDriverDoorClose) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.ManualDriverDoorCloseModifier);// Game.IsControlKeyDownRightNow;
        private bool IsPressingLeftIndicator => IsKeyDownSafe(Settings.SettingsManager.KeySettings.LeftIndicatorKey) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.LeftIndicatorKeyModifer);
        private bool IsPressingHazards => IsKeyDownSafe(Settings.SettingsManager.KeySettings.HazardKey) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.HazardKeyModifer);
        private bool IsPressingGesture => IsKeyDownSafe(Settings.SettingsManager.KeySettings.GestureKey) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.GestureKeyModifier);

        private bool IsPressingSelectorToggle => IsKeyDownSafe(Settings.SettingsManager.KeySettings.SelectorKey) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.SelectorKeyModifier);

        private bool ReleasedFireWeapon => NativeFunction.Natives.xFB6C4072E9A32E92<bool>(2, (int)GameControl.Attack) || NativeFunction.Natives.xFB6C4072E9A32E92<bool>(2, (int)GameControl.Attack2) || NativeFunction.Natives.xFB6C4072E9A32E92<bool>(2, (int)GameControl.VehicleAttack) || NativeFunction.Natives.xFB6C4072E9A32E92<bool>(2, (int)GameControl.VehicleAttack2) || NativeFunction.Natives.xFB6C4072E9A32E92<bool>(2, (int)GameControl.VehiclePassengerAttack) || NativeFunction.Natives.xFB6C4072E9A32E92<bool>(2, (int)GameControl.VehiclePassengerAttack);
        private bool IsPressingFireWeapon => Game.IsControlPressed(0, GameControl.Attack) || Game.IsControlPressed(0, GameControl.Attack2) || Game.IsControlPressed(0, GameControl.VehicleAttack) || Game.IsControlPressed(0, GameControl.VehicleAttack2) || Game.IsControlPressed(0, GameControl.VehiclePassengerAttack) || Game.IsControlPressed(0, GameControl.VehiclePassengerAttack);
        private bool IsMoveControlPressed => Game.IsControlPressed(2, GameControl.MoveUpOnly) || Game.IsControlPressed(2, GameControl.MoveRight) || Game.IsControlPressed(2, GameControl.MoveDownOnly) || Game.IsControlPressed(2, GameControl.MoveLeft);
        private bool IsNotHoldingEnter => !Game.IsControlPressed(2, GameControl.Enter);

        private bool RecentlyPressedDoorClose => Game.GameTime - GameTimeLastPressedDoorClose <= 1500;
        private bool RecentlyPressedIndicators => Game.GameTime - GameTimeLastPressedIndicators <= 1500;
        private bool RecentlyPressedEngineToggle => Game.GameTime - GameTimeLastPressedEngineToggle <= 1500;
        public bool DisableCellPhoneControl { get; set; }

        public void Update()
        {
            SurrenderCheck();
            WeaponDropCheck();
            VehicleCheck();
            ButtonPromptCheck();
            ConversationCheck();
            ScenarioCheck();
            ControlCheck();
            KeyBindCheck();
            Player.IsNotHoldingEnter = IsNotHoldingEnter;
            Player.IsMoveControlPressed = IsMoveControlPressed;
            Player.IsPressingFireWeapon = IsPressingFireWeapon;

            Player.ReleasedFireWeapon = ReleasedFireWeapon;

            //GameFiber.Yield();
            MenuCheck();

            CellPhoneCheck();

        }

        private void CellPhoneCheck()
        {
            if (DisableCellPhoneControl)
            {
                //NativeFunction.Natives.DISABLE_CONTROL_ACTION(0, 27, true);//up
                NativeFunction.Natives.DISABLE_CONTROL_ACTION(0, 172, true);//up
                NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 173, true);//down
                NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 174, true);//left
                NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 175, true);//right
                NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 176, true);//select
                NativeFunction.Natives.DISABLE_CONTROL_ACTION(2, 177, true);//cencel
                isCellPhoneControlDisabled = true;
            }
            else
            {
                if (isCellPhoneControlDisabled)
                 {
                    //NativeFunction.Natives.ENABLE_CONTROL_ACTION(0, 27, true);
                    NativeFunction.Natives.ENABLE_CONTROL_ACTION(2, 172, true);
                    NativeFunction.Natives.ENABLE_CONTROL_ACTION(2, 173, true);
                    NativeFunction.Natives.ENABLE_CONTROL_ACTION(2, 174, true);
                    NativeFunction.Natives.ENABLE_CONTROL_ACTION(2, 175, true);
                    NativeFunction.Natives.ENABLE_CONTROL_ACTION(2, 176, true);
                    NativeFunction.Natives.ENABLE_CONTROL_ACTION(2, 177, true);
                    isCellPhoneControlDisabled = false;
                }
            }




            //if(DisableCellPhoneControl)
            //{
            //    Game.DisableControlAction(0, GameControl.CellphoneCameraDOF, true);
            //    Game.DisableControlAction(0, GameControl.CellphoneCameraExpression, true);
            //    Game.DisableControlAction(0, GameControl.CellphoneCameraFocusLock, true);
            //    Game.DisableControlAction(0, GameControl.CellphoneCameraGrid, true);
            //    Game.DisableControlAction(0, GameControl.CellphoneCameraSelfie, true);
            //    Game.DisableControlAction(0, GameControl.CellphoneCancel, true);
            //    Game.DisableControlAction(0, GameControl.CellphoneUp, true);
            //    Game.DisableControlAction(0, GameControl.CellphoneExtraOption, true);
            //    Game.DisableControlAction(0, GameControl.CellphoneLeft, true);
            //    Game.DisableControlAction(0, GameControl.CellphoneOption, true);
            //    Game.DisableControlAction(0, GameControl.CellphoneRight, true);
            //    Game.DisableControlAction(0, GameControl.CellphoneScrollBackward, true);
            //    Game.DisableControlAction(0, GameControl.CellphoneScrollForward, true);
            //    Game.DisableControlAction(0, GameControl.CellphoneSelect, true);
            //    Game.DisableControlAction(0, GameControl.CellphoneDown, true);
            //    isCellPhoneControlDisabled = true;
            //}
            //else
            //{
            //    if(isCellPhoneControlDisabled)
            //    {
            //        Game.DisableControlAction(0, GameControl.CellphoneCameraDOF, false);
            //        Game.DisableControlAction(0, GameControl.CellphoneCameraExpression, false);
            //        Game.DisableControlAction(0, GameControl.CellphoneCameraFocusLock, false);
            //        Game.DisableControlAction(0, GameControl.CellphoneCameraGrid, false);
            //        Game.DisableControlAction(0, GameControl.CellphoneCameraSelfie, false);
            //        Game.DisableControlAction(0, GameControl.CellphoneCancel, false);
             //      Game.DisableControlAction(0, GameControl.CellphoneDown, false);
            //        Game.DisableControlAction(0, GameControl.CellphoneExtraOption, false);
            //        Game.DisableControlAction(0, GameControl.CellphoneLeft, false);
            //        Game.DisableControlAction(0, GameControl.CellphoneOption, false);
            //        Game.DisableControlAction(0, GameControl.CellphoneRight, false);
            //        Game.DisableControlAction(0, GameControl.CellphoneScrollBackward, false);
            //        Game.DisableControlAction(0, GameControl.CellphoneScrollForward, false);
            //        Game.DisableControlAction(0, GameControl.CellphoneSelect, false);
            //        Game.DisableControlAction(0, GameControl.CellphoneUp, false);
            //        isCellPhoneControlDisabled = false;
            //    }
            //}
        }

        private void KeyBindCheck()
        {
            if(IsPressingGesture)
            {
                EntryPoint.WriteToConsole("Gesture Start Hotkey");
                Player.Gesture();
            }
        }
        private void ConversationCheck()
        {
            if (Player.ButtonPrompts.Any(x => x.Group == "StartConversation" && x.IsPressedNow))//string for now...
            {
                Player.StartConversation();
            }
            else if (Player.ButtonPrompts.Any(x => x.Group == "StartTransaction" && x.IsPressedNow))//string for now...
            {
                Player.StartTransaction();
            }
            else if (Player.ButtonPrompts.Any(x => x.Group == "StartSimpleTransaction" && x.IsPressedNow))//string for now...
            {
                Player.StartSimpleTransaction();
            }
            else if (Player.ButtonPrompts.Any(x => x.Group == "InteractableLocation" && x.IsPressedNow))//string for now...
            {
                Player.StartLocationInteraction();
            }
        }
        private void ScenarioCheck()
        {
            if (Player.ButtonPrompts.Any(x => x.Group == "StartScenario" && x.IsPressedNow))//string for now...
            {
                Player.StartScenario();
            }
            else if (Player.ButtonPrompts.Any(x => x.Group == "EnterLocation" && x.IsPressedNow))//string for now...
            {
                Player.EnterLocation();
            }
            else if (Player.ButtonPrompts.Any(x => x.Group == "PurchaseLocation" && x.IsPressedNow))//string for now...
            {
                Player.PurchaseLocation();
            }
            else if (Player.ButtonPrompts.Any(x => x.Group == "ExitLocation" && x.IsPressedNow))//string for now...
            {
                Player.ExitLocation();
            }  
        }
        private void ControlCheck()
        {
            if (Player.ButtonPrompts.Any(x => x.Group == "AIControl" && x.IsPressedNow))//string for now...
            {
                if(Player.AliasedCop != null)
                {
                    Player.AliasedCop.CanBeTasked = !Player.AliasedCop.CanBeTasked;
                    if(!Player.AliasedCop.CanBeTasked)
                    {
                        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                    }
                }
            }
            if(!Player.IsInVehicle)
            {
                if(IsPressingSprint)
                {
                    Player.Sprinting.Start();
                }
                else
                {
                    Player.Sprinting.Stop();
                }
            }
        }
        private void SurrenderCheck()
        {
            if (IsPressingSurrender)
            {
                if (Player.CanSurrender)
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
        private void ButtonPromptCheck()
        {
            Game.DisableControlAction(0, GameControl.CharacterWheel, true);
            Game.DisableControlAction(0, GameControl.SelectCharacterFranklin, true);
            Game.DisableControlAction(0, GameControl.SelectCharacterMichael, true);
            Game.DisableControlAction(0, GameControl.SelectCharacterMultiplayer, true);
            Game.DisableControlAction(0, GameControl.SelectCharacterTrevor, true);


            Game.DisableControlAction(0, GameControl.Talk, true);//dont mess up my other talking!
           // Game.DisableControlAction(0, GameControl.Context, true);//dont mess up my other talking! needed for stores?
            if(Player.ButtonPrompts.Count > 10)
            {
                EntryPoint.WriteToConsole($"INPUT: Excessive Button Prompts {Player.ButtonPrompts.Count}", 1);
            }
            foreach (ButtonPrompt bp in Player.ButtonPrompts)
            {
                if (Game.IsKeyDownRightNow(bp.Key) && (bp.Modifier == Keys.None || Game.IsKeyDownRightNow(bp.Modifier)) && !bp.IsHeldNow)
                {
                    //EntryPoint.WriteToConsole($"INPUT! Control :{bp.Text}: Down");
                    bp.IsHeldNow = true;
                }
                else
                {
                    bp.IsHeldNow = false;
                }
                if (Game.IsKeyDown(bp.Key) && (bp.Modifier == Keys.None || Game.IsKeyDown(bp.Modifier)) && !bp.IsPressedNow)
                {
                    //EntryPoint.WriteToConsole($"INPUT! Control :{bp.Text}: Down");
                    bp.IsPressedNow = true;
                }
                else if (Game.IsControlJustPressed(2, bp.GameControl) && !bp.IsPressedNow)
                {
                    bp.IsPressedNow = true;
                }
                else
                {
                    bp.IsPressedNow = false;
                }
            }
        }
        private void WeaponDropCheck()
        {
            if (IsPressingDropWeapon && Player.CanDropWeapon && Settings.SettingsManager.PlayerOtherSettings.AllowWeaponDropping)
            {
                Player.DropWeapon();
            }
            if(IsPressingSelectorToggle)
            {
                if (Game.GameTime - GameTimeLastPressedSelectorToggle >= 200)
                {
                    Player.ToggleSelector();
                    GameTimeLastPressedSelectorToggle = Game.GameTime;
                }
            }
        }
        private void VehicleCheck()
        {
            if (Player.CurrentVehicle != null)
            {
                if(!RecentlyPressedEngineToggle)
                {
                    if(IsPressingEngineToggle && Settings.SettingsManager.VehicleSettings.AllowSetEngineState)
                    {
                        Player.CurrentVehicle.Engine.Toggle();
                        GameTimeLastPressedEngineToggle = Game.GameTime;
                    }
                }
                if (!RecentlyPressedIndicators)
                {
                    if (Settings.SettingsManager.VehicleSettings.AllowSetIndicatorState)
                    {
                        if (IsPressingHazards)
                        {
                            Player.CurrentVehicle.Indicators.ToggleHazards();
                            GameTimeLastPressedIndicators = Game.GameTime;
                        }
                        if (IsPressingLeftIndicator)
                        {
                            Player.CurrentVehicle.Indicators.ToggleLeft();
                            GameTimeLastPressedIndicators = Game.GameTime;
                        }
                        if (IsPressingRightIndicator)
                        {
                            Player.CurrentVehicle.Indicators.ToggleRight();
                            GameTimeLastPressedIndicators = Game.GameTime;
                        }
                    }
                }
                if(!RecentlyPressedDoorClose)
                {
                    if (IsPressingDoorClose)
                    {
                        Player.CloseDriverDoor();
                        GameTimeLastPressedDoorClose = Game.GameTime;
                    }
                }
            }
        }
        private void MenuCheck()
        {
            if (!Player.IsDisplayingCustomMenus)
            {
                if (IsPressingMenuKey)
                {
                    MenuProvider.ToggleMenu();
                }
                else if (IsPressingDebugMenuKey)
                {
                    MenuProvider.ToggleDebugMenu();
                }
            }
        }
        private bool IsKeyDownSafe(Keys key)
        {
            if(key == Keys.None)
            {
                return true;
            }
            else if(key == Keys.Control)
            {
                return Game.IsControlKeyDownRightNow;
            }
            else if (key == Keys.Shift)
            {
                return Game.IsShiftKeyDownRightNow;
            }
            else if (key == Keys.Alt)
            {
                return Game.IsAltKeyDownRightNow;
            }
            else
            {
                return Game.IsKeyDownRightNow(key);
            }
        }


    }
}