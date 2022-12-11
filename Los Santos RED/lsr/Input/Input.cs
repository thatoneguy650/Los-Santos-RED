using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
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
        private uint GameTimeLastPressedCrouch;
        private uint GameTimeLastPressedSimplePhone;
        private int TicksPressedVehicleEnter;
        private int TicksNotPressedVehicleEnter;
        private bool heldVehicleEnter;

        public bool IsPressingMenuKey => Game.IsKeyDown(Settings.SettingsManager.KeySettings.MenuKey);
        public bool IsPressingDebugMenuKey => Game.IsKeyDown(Settings.SettingsManager.KeySettings.DebugMenuKey);
        private bool IsPressingSurrender => IsKeyDownSafe(Settings.SettingsManager.KeySettings.SurrenderKey) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.SurrenderKeyModifier);
        private bool IsPressingSprint => IsKeyDownSafe(Settings.SettingsManager.KeySettings.SprintKey) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.SprintKeyModifier);
        private bool IsPressingRightIndicator => IsKeyDownSafe(Settings.SettingsManager.KeySettings.RightIndicatorKey) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.RightIndicatorKeyModifer);
        public bool IsPressingEngineToggle => IsKeyDownSafe(Settings.SettingsManager.KeySettings.EngineToggle) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.EngineToggleModifier);
        private bool IsPressingDoorClose => IsKeyDownSafe(Settings.SettingsManager.KeySettings.ManualDriverDoorClose) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.ManualDriverDoorCloseModifier);
        private bool IsPressingLeftIndicator => IsKeyDownSafe(Settings.SettingsManager.KeySettings.LeftIndicatorKey) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.LeftIndicatorKeyModifer);
        private bool IsPressingHazards => IsKeyDownSafe(Settings.SettingsManager.KeySettings.HazardKey) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.HazardKeyModifer);
        private bool IsPressingGesture => IsKeyDownSafe(Settings.SettingsManager.KeySettings.GestureKey) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.GestureKeyModifier);
        private bool IsPressingStopActivity => IsKeyDownSafe(Settings.SettingsManager.KeySettings.ActivityKey) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.ActivityKeyModifier);
        private bool IsPressingSelectorToggle => IsKeyDownSafe(Settings.SettingsManager.KeySettings.SelectorKey) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.SelectorKeyModifier);
        private bool IsPressingCrouchToggle => IsKeyDownSafe(Settings.SettingsManager.KeySettings.CrouchKey) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.CrouchKeyModifier);
        private bool IsPressingSimpleCellphone => IsKeyDownSafe(Settings.SettingsManager.KeySettings.SimplePhoneKey) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.SimplePhoneKeyModifer);
        private bool ReleasedFireWeapon => NativeFunction.Natives.xFB6C4072E9A32E92<bool>(2, (int)GameControl.Attack) || NativeFunction.Natives.xFB6C4072E9A32E92<bool>(2, (int)GameControl.Attack2) || NativeFunction.Natives.xFB6C4072E9A32E92<bool>(2, (int)GameControl.VehicleAttack) || NativeFunction.Natives.xFB6C4072E9A32E92<bool>(2, (int)GameControl.VehicleAttack2) || NativeFunction.Natives.xFB6C4072E9A32E92<bool>(2, (int)GameControl.VehiclePassengerAttack) || NativeFunction.Natives.xFB6C4072E9A32E92<bool>(2, (int)GameControl.VehiclePassengerAttack);
        private bool IsPressingFireWeapon => Game.IsControlPressed(0, GameControl.Attack) || Game.IsControlPressed(0, GameControl.Attack2) || Game.IsControlPressed(0, GameControl.VehicleAttack) || Game.IsControlPressed(0, GameControl.VehicleAttack2) || Game.IsControlPressed(0, GameControl.VehiclePassengerAttack) || Game.IsControlPressed(0, GameControl.VehiclePassengerAttack);
        private bool IsMoveControlPressed => Game.IsControlPressed(2, GameControl.MoveUpOnly) || Game.IsControlPressed(2, GameControl.MoveRight) || Game.IsControlPressed(2, GameControl.MoveDownOnly) || Game.IsControlPressed(2, GameControl.MoveLeft);
        private bool IsNotHoldingEnter => !heldVehicleEnter;//!Game.IsControlPressed(2, GameControl.Enter);
        private bool IsPressingVehicleAccelerate => Game.IsControlPressed(0, GameControl.VehicleAccelerate);
        private bool RecentlyPressedCrouch => Game.GameTime - GameTimeLastPressedCrouch <= 250;
        private bool RecentlyPressedDoorClose => Game.GameTime - GameTimeLastPressedDoorClose <= 500;
        private bool RecentlyPressedIndicators => Game.GameTime - GameTimeLastPressedIndicators <= 500;
        private bool RecentlyPressedEngineToggle => Game.GameTime - GameTimeLastPressedEngineToggle <= 500;
        private bool RecentlyPressedSimplePhone => Game.GameTime - GameTimeLastPressedSimplePhone <= 500;
        private bool IsPressingActionWheelMenu;// => (IsKeyDownSafe(Settings.SettingsManager.KeySettings.ActionPopUpDisplayKey) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.ActionPopUpDisplayKeyModifier)) || (IsKeyDownSafe(Settings.SettingsManager.KeySettings.AltActionPopUpDisplayKey) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.AltActionPopUpDisplayKeyModifier)) || (IsKeyDownSafe(Settings.SettingsManager.KeySettings.AltActionPopUpDisplayKey) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.AltActionPopUpDisplayKeyModifier));
        private bool HasShownControllerHelpPrompt;

        public bool IsUsingController { get; private set; }
        
        public void Tick()
        {        
            DisableVanillaControls();
            ProcessButtonPrompts();
            ProcessGeneralControls();
            ProcessVehicleControls();
            ProcessMenuControls();
            ProcessWheelMenuInput();
        }
        private void ProcessWheelMenuInput()
        {
            //bool IsUsingKeyboardAndMouse = NativeFunction.Natives.IS_USING_KEYBOARD_AND_MOUSE<bool>(2);
            //bool IsUsingController = !IsUsingKeyboardAndMouse;
            if (IsKeyDownSafe(Settings.SettingsManager.KeySettings.ActionPopUpDisplayKey) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.ActionPopUpDisplayKeyModifier))
            {
                MenuProvider.IsPressingActionWheelButton = true;
            }
            else if (IsKeyDownSafe(Settings.SettingsManager.KeySettings.AltActionPopUpDisplayKey) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.AltActionPopUpDisplayKeyModifier))
            {
                MenuProvider.IsPressingActionWheelButton = true;
            }
            else if (IsUsingController && (Game.IsControlPressed(0, (GameControl)Settings.SettingsManager.KeySettings.GameControlActionPopUpDisplayKey) || NativeFunction.Natives.x91AEF906BCA88877<bool>(0, Settings.SettingsManager.KeySettings.GameControlActionPopUpDisplayKey)))
            {
                MenuProvider.IsPressingActionWheelButton = true;
            }
            else
            {
                MenuProvider.IsPressingActionWheelButton = false;
            }
        }
        private void DisableVanillaControls()
        {
            IsUsingController = NativeHelper.IsUsingController;


            Game.DisableControlAction(0, GameControl.CharacterWheel, true);
            Game.DisableControlAction(0, GameControl.SelectCharacterFranklin, true);
            Game.DisableControlAction(0, GameControl.SelectCharacterMichael, true);
            Game.DisableControlAction(0, GameControl.SelectCharacterMultiplayer, true);
            Game.DisableControlAction(0, GameControl.SelectCharacterTrevor, true);
            Game.DisableControlAction(0, GameControl.Talk, true);//dont mess up my other talking!

            if(IsUsingController && Settings.SettingsManager.KeySettings.GameControlToDisable >= 0)
            {
                Game.DisableControlAction(0, (GameControl)Settings.SettingsManager.KeySettings.GameControlToDisable, true);
            }
            
            if (Settings.SettingsManager.ActivitySettings.AllowPlayerCrouching)
            {
                Game.DisableControlAction(0, GameControl.Duck, true);
            }
        }
        private void ProcessGeneralControls()
        {
            if (Player.ActivityManager.IsPerformingActivity)
            {
                if (Player.ButtonPrompts.IsPressed("ActivityControlCancel"))
                {
                    Player.ActivityManager.CancelCurrentActivity();
                }
                else if (Player.ButtonPrompts.IsPressed("ActivityControlPause"))
                {
                    Player.ActivityManager.PauseCurrentActivity();
                }
            }
            else
            {
                if (Player.ButtonPrompts.IsPressed("ActivityControlContinue"))
                {
                    Player.ActivityManager.ContinueCurrentActivity();
                }
                if (Player.ButtonPrompts.IsPressed("ActivityControlCancel"))
                {
                    Player.ActivityManager.CancelCurrentActivity();
                }
            }

            if (IsPressingSimpleCellphone && !RecentlyPressedSimplePhone && !MenuProvider.IsDisplayingMenu && !Player.IsDisplayingCustomMenus)
            {
                Player.CellPhone.OpenBurner();
                GameTimeLastPressedSimplePhone = Game.GameTime;
            }

            if (Player.ButtonPrompts.IsGroupPressed("StartConversation"))//Player.ButtonPromptList.Any(x => x.Group == "StartConversation" && x.IsPressedNow))//string for now...
            {
                Player.ActivityManager.StartConversation();
            }
            else if (Player.ButtonPrompts.IsGroupPressed("StartTransaction"))//Player.ButtonPromptList.Any(x => x.Group == "StartTransaction" && x.IsPressedNow))//string for now...
            {
                Player.ActivityManager.StartTransaction();
            }
            else if (Player.ButtonPrompts.IsGroupPressed("InteractableLocation"))//Player.ButtonPromptList.Any(x => x.Group == "InteractableLocation" && x.IsPressedNow))//string for now...
            {
                Player.ActivityManager.StartLocationInteraction();
            }
            else if (Player.ButtonPrompts.IsGroupPressed("Search"))//Player.ButtonPromptList.Any(x => x.Group == "Search" && x.IsPressedNow))//string for now...
            {
                Player.ActivityManager.LootPed();
            }
            else if (Player.ButtonPrompts.IsGroupPressed("Drag"))//Player.ButtonPromptList.Any(x => x.Group == "Drag" && x.IsPressedNow))//string for now...
            {
                Player.ActivityManager.DragPed();
            }
            else if (Player.ButtonPrompts.IsGroupPressed("Grab"))//Player.ButtonPromptList.Any(x => x.Group == "Grab" && x.IsPressedNow))//string for now...
            {
                Player.ActivityManager.GrabPed();
            }

            if (Player.ButtonPrompts.IsGroupPressed("StartScenario"))//Player.ButtonPromptList.Any(x => x.Group == "StartScenario" && x.IsPressedNow))//string for now...
            {
                Player.ActivityManager.StartScenario();
            }


            if (Player.ButtonPrompts.IsGroupPressed("Recruit"))
            {
                Player.GroupManager.TryRecruitLookedAtPed();
            }
            if (Player.ButtonPrompts.IsGroupPressed("Sit"))
            {
                Player.ActivityManager.StartSittingDown(true, true);
            }

            if (!Player.IsInVehicle)
            {
                if (IsPressingSprint)
                {
                    Player.Sprinting.Start();
                }
                else
                {
                    Player.Sprinting.Stop();
                }
            }
            if (IsPressingSelectorToggle)
            {
                Player.WeaponEquipment.ToggleSelector();
            }
            if (Settings.SettingsManager.ActivitySettings.AllowPlayerCrouching && !Player.IsInVehicle)
            {
                if (!RecentlyPressedCrouch)
                {
                    if (IsPressingCrouchToggle)
                    {
                        Player.Stance.Crouch();
                        GameTimeLastPressedCrouch = Game.GameTime;
                    }
                }
            }

            if (IsPressingSurrender || Player.ButtonPrompts.IsPressed("ShowSurrender"))
            {
                Player.Surrendering.ToggleSurrender();
            }

            if (IsPressingGesture)
            {
                EntryPoint.WriteToConsole("Gesture Start Hotkey");
                Player.ActivityManager.Gesture();
            }
        } 
        private void ProcessVehicleControls()
        {
            if (Player.CurrentVehicle != null)
            {
                if (!RecentlyPressedEngineToggle)
                {
                    if (IsPressingEngineToggle && Settings.SettingsManager.VehicleSettings.AllowSetEngineState)
                    {
                        Player.CurrentVehicle.Engine.Toggle();
                        GameTimeLastPressedEngineToggle = Game.GameTime;
                    }
                    else if (IsPressingVehicleAccelerate && !Player.CurrentVehicle.Engine.IsRunning && Settings.SettingsManager.VehicleSettings.AllowSetEngineState)
                    {
                        Player.CurrentVehicle.Engine.Toggle(true);
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
                if (!RecentlyPressedDoorClose)
                {
                    if (IsPressingDoorClose)
                    {
                        Player.ActivityManager.CloseDriverDoor();
                        GameTimeLastPressedDoorClose = Game.GameTime;
                    }
                }
                if (Player.IsInVehicle && (Player.CurrentVehicleIsRolledOver || Player.CurrentVehicleIsInAir) && Settings.SettingsManager.VehicleSettings.DisableRolloverFlip)
                {
                    Game.DisableControlAction(0, GameControl.VehicleMoveLeftRight, true);
                    Game.DisableControlAction(0, GameControl.VehicleMoveUpDown, true);
                }
            }

            if(Game.IsControlPressed(2, GameControl.Enter))
            {
                TicksNotPressedVehicleEnter = 0;
                TicksPressedVehicleEnter++;
            }
            else
            {
                TicksNotPressedVehicleEnter++;
                TicksPressedVehicleEnter = 0;
            }


            if(TicksPressedVehicleEnter >= 20)
            {
                heldVehicleEnter = true;
            }
            else if(TicksNotPressedVehicleEnter >= 60)
            {
                heldVehicleEnter = false;
            }
        }
        private void ProcessMenuControls()
        {
            if (!Player.IsDisplayingCustomMenus)
            {
                if (IsPressingMenuKey || Player.ButtonPrompts.IsPressed("MenuShowBusted") || Player.ButtonPrompts.IsPressed("MenuShowDead"))
                {
                    MenuProvider.ToggleMenu();
                }
                else if (IsPressingDebugMenuKey)
                {
                    MenuProvider.ToggleDebugMenu();
                }
            }
        }
        private void ProcessButtonPrompts()
        {
            bool hasPrompts = false;
            foreach (ButtonPrompt bp in Player.ButtonPrompts.Prompts)
            {
                if (Player.ButtonPrompts.IsSuspended)
                {
                    bp.IsHeldNow = false;
                    bp.IsPressedNow = false;
                }
                else
                {
                    hasPrompts = true;
                    if (Game.IsKeyDownRightNow(bp.Key) && (bp.Modifier == Keys.None || Game.IsKeyDownRightNow(bp.Modifier)) && !bp.IsHeldNow)
                    {
                        //EntryPoint.WriteToConsole($"INPUT! Control :{bp.Text}: Down");
                        bp.IsHeldNow = true;
                    }
                    else if (bp.IsAlternativePressed)
                    {
                        bp.IsHeldNow = true;
                    }
                    else
                    {
                        bp.IsHeldNow = false;
                    }



                    if (Game.IsKeyDown(bp.Key) && (bp.Modifier == Keys.None || Game.IsKeyDown(bp.Modifier)) && !bp.IsPressedNow)
                    {
                        EntryPoint.WriteToConsole($"INPUT! Control :{bp.Text}: Down 1");
                        bp.IsPressedNow = true;
                    }
                    else if (bp.HasGameControl && Game.IsControlJustPressed(2, bp.GameControl) && !bp.IsPressedNow)
                    {
                        EntryPoint.WriteToConsole($"INPUT! Control :{bp.Text}: Down 2");
                        bp.IsPressedNow = true;
                    }
                    else if (bp.HasGameControl && NativeFunction.Natives.IS_DISABLED_CONTROL_JUST_PRESSED<bool>(2, (int)bp.GameControl) && !bp.IsPressedNow)
                    {
                        EntryPoint.WriteToConsole($"INPUT! Control :{bp.Text}: Down 3");
                        bp.IsPressedNow = true;
                    }
                    else if (bp.IsAlternativePressed)
                    {
                        EntryPoint.WriteToConsole($"INPUT! Control :{bp.Text}: Down 4");
                        bp.IsPressedNow = true;
                        //if(Game.GameTime - bp.GameTimeAlternativePressed >= 20)
                        //{
                        //    bp.IsPressedNow = false;
                        //    bp.IsAlternativePressed = false;
                        //}
                        //else
                        //{
                        //    EntryPoint.WriteToConsole($"INPUT! Control :{bp.Text}: Down 4");
                        //    bp.IsPressedNow = true;
                        //}
                    }
                    else
                    {
                        bp.IsPressedNow = false;
                    }



                    if (Game.GameTime - bp.GameTimeAlternativePressed >= 20)
                    {
                        bp.IsAlternativePressed = false;
                    }
                }
            }
            Player.IsNotHoldingEnter = IsNotHoldingEnter;
            Player.IsMoveControlPressed = IsMoveControlPressed;
            Player.IsPressingFireWeapon = IsPressingFireWeapon;
            Player.ReleasedFireWeapon = ReleasedFireWeapon;


            if(hasPrompts && IsUsingController && !HasShownControllerHelpPrompt)
            {
                ShowControllerHelpPrompt();
            }

        }
        private void ShowControllerHelpPrompt()
        {
            EntryPoint.WriteToConsole($"Controller Prompt ID: {InstructionalButton.GetButtonId((GameControl)Settings.SettingsManager.KeySettings.GameControlActionPopUpDisplayKey)}");
            Game.DisplayHelp($"You can also access prompts through the action wheel.~n~Press ~{InstructionalButton.GetButtonId((GameControl)Settings.SettingsManager.KeySettings.GameControlActionPopUpDisplayKey)}~ to open");
            HasShownControllerHelpPrompt = true;
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