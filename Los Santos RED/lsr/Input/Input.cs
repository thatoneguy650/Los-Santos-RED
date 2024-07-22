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

        private uint GameTimeLastPressedEngineToggle;
        private uint GameTimeLastPressedDoorClose;
        private uint GameTimeLastPressedCrouch;
        private uint GameTimeLastPressedSimplePhone;
        private uint GameTimeLastPressedSurrender;
        private uint GameTimeLastPressedGesture;
        private int TicksPressedVehicleEnter;
        private int TicksNotPressedVehicleEnter;
        private bool heldVehicleEnter;
        private bool HasShownControllerHelpPrompt;
        private uint GameTimeLastPressedAltMenu;
        private bool CanToggleAltMenu;
        private bool IsPressingActionWheelMenu;
        private uint GameTimeLastPressedStartTransaction;
        private uint GameTimeLastPressedYell;
        private uint GameTimeLastPressedGroupModeToggle;

        private bool IsPressingSurrender => IsKeyDownSafe(Settings.SettingsManager.KeySettings.SurrenderKey, false) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.SurrenderKeyModifier, true);
        private bool IsPressingSprint => IsKeyDownSafe(Settings.SettingsManager.KeySettings.SprintKey, false) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.SprintKeyModifier, true);
        private bool IsPressingRightIndicator => IsKeyDownSafe(Settings.SettingsManager.KeySettings.RightIndicatorKey, false) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.RightIndicatorKeyModifer, true);
        public bool IsPressingEngineToggle => IsKeyDownSafe(Settings.SettingsManager.KeySettings.EngineToggle, false) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.EngineToggleModifier, true);
        private bool IsPressingDoorClose => IsKeyDownSafe(Settings.SettingsManager.KeySettings.ManualDriverDoorClose, false) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.ManualDriverDoorCloseModifier, true);
        private bool IsPressingLeftIndicator => IsKeyDownSafe(Settings.SettingsManager.KeySettings.LeftIndicatorKey, false) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.LeftIndicatorKeyModifer, true);
        private bool IsPressingHazards => IsKeyDownSafe(Settings.SettingsManager.KeySettings.HazardKey, false) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.HazardKeyModifer, true);
        private bool IsPressingGesture => IsKeyDownSafe(Settings.SettingsManager.KeySettings.GestureKey, false) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.GestureKeyModifier, true);
        private bool IsPressingStopActivity => IsKeyDownSafe(Settings.SettingsManager.KeySettings.ActivityKey, false) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.ActivityKeyModifier, true);
        private bool IsPressingSelectorToggle => IsKeyDownSafe(Settings.SettingsManager.KeySettings.SelectorKey, false) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.SelectorKeyModifier, true);
        private bool IsPressingCrouchToggle => IsKeyDownSafe(Settings.SettingsManager.KeySettings.CrouchKey, false) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.CrouchKeyModifier, true);
        private bool IsPressingSimpleCellphone => IsKeyDownSafe(Settings.SettingsManager.KeySettings.SimplePhoneKey,false) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.SimplePhoneKeyModifer, true);


        private bool IsPressingYell => IsKeyDownSafe(Settings.SettingsManager.KeySettings.YellKey, false) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.YellKeyModifier, true);

        private bool IsPressingGroupModeToggle => IsKeyDownSafe(Settings.SettingsManager.KeySettings.GroupModeToggleKey, false) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.GroupModeToggleKeyModifier, true);

        private bool ReleasedFireWeapon => NativeFunction.Natives.xFB6C4072E9A32E92<bool>(2, (int)GameControl.Attack) || NativeFunction.Natives.xFB6C4072E9A32E92<bool>(2, (int)GameControl.Attack2) || NativeFunction.Natives.xFB6C4072E9A32E92<bool>(2, (int)GameControl.VehicleAttack) || NativeFunction.Natives.xFB6C4072E9A32E92<bool>(2, (int)GameControl.VehicleAttack2) || NativeFunction.Natives.xFB6C4072E9A32E92<bool>(2, (int)GameControl.VehiclePassengerAttack) || NativeFunction.Natives.xFB6C4072E9A32E92<bool>(2, (int)GameControl.VehiclePassengerAttack);
        private bool IsPressingFireWeapon => Game.IsControlPressed(0, GameControl.Attack) || Game.IsControlPressed(0, GameControl.Attack2) || Game.IsControlPressed(0, GameControl.VehicleAttack) || Game.IsControlPressed(0, GameControl.VehicleAttack2) || Game.IsControlPressed(0, GameControl.VehiclePassengerAttack) || Game.IsControlPressed(0, GameControl.VehiclePassengerAttack);
        private bool IsMoveControlPressed => Game.IsControlPressed(2, GameControl.MoveUpOnly) || Game.IsControlPressed(2, GameControl.MoveRight) || Game.IsControlPressed(2, GameControl.MoveDownOnly) || Game.IsControlPressed(2, GameControl.MoveLeft);
        private bool IsNotHoldingEnter => !heldVehicleEnter;
        private bool IsPressingVehicleAccelerate => Game.IsControlPressed(0, GameControl.VehicleAccelerate);
        private bool RecentlyPressedCrouch => Game.GameTime - GameTimeLastPressedCrouch <= 500;
        private bool RecentlyPressedGesture => Game.GameTime - GameTimeLastPressedGesture <= 500;
        private bool RecentlyPressedDoorClose => Game.GameTime - GameTimeLastPressedDoorClose <= 500;
        private bool RecentlyPressedIndicators => Game.GameTime - GameTimeLastPressedIndicators <= 500;
        private bool RecentlyPressedEngineToggle => Game.GameTime - GameTimeLastPressedEngineToggle <= 500;
        private bool RecentlyPressedAltMenu => Game.GameTime - GameTimeLastPressedAltMenu <= 200;
        private bool RecentlyPressedSimplePhone => Game.GameTime - GameTimeLastPressedSimplePhone <= 500;
        private bool RecentlyPressedYell => Game.GameTime - GameTimeLastPressedYell <= 500;
        private bool RecentlyPressedGroupModeToggle => Game.GameTime - GameTimeLastPressedGroupModeToggle <= 750;
        public Input(IInputable player, ISettingsProvideable settings, IMenuProvideable menuProvider, IPedSwap pedswap)
        {
            Player = player;
            Settings = settings;
            MenuProvider = menuProvider;
            PedSwap = pedswap;
        }
        private bool RecentlyPressedSurrender => Game.GameTime - GameTimeLastPressedSurrender <= 1000;
        public bool IsUsingController { get; private set; }
        public bool IsPressingMenuKey => Game.IsKeyDown(Settings.SettingsManager.KeySettings.MenuKey);
        public bool IsPressingDebugMenuKey => Game.IsKeyDown(Settings.SettingsManager.KeySettings.DebugMenuKey);
        public void Tick()
        {        
            DisableVanillaControls();
            ProcessButtonPrompts();
            //ProcessActivityControls();
            ProcessBurnerControls();
            ProcessInteractControls();
            ProcessOtherControls();
            ProcessButtonPromptAutoControls();
            ProcessHotkeyControls();
            ProcessVehicleControls();
            ProcessMenuControls();
            ProcessWheelMenuInput();
        }
        private void ProcessWheelMenuInput()
        {
            if(Player.CuffManager.IsHandcuffed)
            {
                MenuProvider.IsPressingActionWheelButton = false;
                return;
            }
            if (IsKeyDownSafe(Settings.SettingsManager.KeySettings.ActionPopUpDisplayKey, false) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.ActionPopUpDisplayKeyModifier, true))
            {
                MenuProvider.IsPressingActionWheelButton = true;
            }
            else if (IsKeyDownSafe(Settings.SettingsManager.KeySettings.AltActionPopUpDisplayKey, false) && IsKeyDownSafe(Settings.SettingsManager.KeySettings.AltActionPopUpDisplayKeyModifier, true))
            {
                MenuProvider.IsPressingActionWheelButton = true;
            }
            else if (IsUsingController && 
                (
                Settings.SettingsManager.KeySettings.ControllerActionModifier == -1 || 
                    (
                    Game.IsControlPressed(0, (GameControl)Settings.SettingsManager.KeySettings.ControllerActionModifier) || NativeFunction.Natives.x91AEF906BCA88877<bool>(0, Settings.SettingsManager.KeySettings.ControllerActionModifier)
                    )
                ) && 
                (Game.IsControlPressed(0, (GameControl)Settings.SettingsManager.KeySettings.ControllerAction) || NativeFunction.Natives.x91AEF906BCA88877<bool>(0, Settings.SettingsManager.KeySettings.ControllerAction)))
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
            Player.IsUsingController = IsUsingController;
            Game.DisableControlAction(0, GameControl.CharacterWheel, true);
            Game.DisableControlAction(0, GameControl.SelectCharacterFranklin, true);
            Game.DisableControlAction(0, GameControl.SelectCharacterMichael, true);
            Game.DisableControlAction(0, GameControl.SelectCharacterMultiplayer, true);
            Game.DisableControlAction(0, GameControl.SelectCharacterTrevor, true);
            if(IsUsingController && Settings.SettingsManager.KeySettings.GameControlToDisable >= 0)
            {
                Game.DisableControlAction(0, (GameControl)Settings.SettingsManager.KeySettings.GameControlToDisable, true);
            }       
            if (Settings.SettingsManager.ActivitySettings.AllowPlayerCrouching)
            {
                Game.DisableControlAction(0, GameControl.Duck, true);
            }
            if(Player.IsBusted || Player.IsSetDisabledControls)
            {
                NativeHelper.DisablePlayerControl();
            }

            if(Player.CuffManager.IsHandcuffed)
            {
                DisableHandcuffedControls();
            }

        }
        private void DisableHandcuffedControls()
        {
            Game.DisableControlAction(0, GameControl.VehicleExit, true);
            Game.DisableControlAction(0, GameControl.Enter, true);


            Game.DisableControlAction(0, GameControl.Attack, true);
            Game.DisableControlAction(0, GameControl.Attack2, true);

            Game.DisableControlAction(0, GameControl.MeleeAttack1, true);
            Game.DisableControlAction(0, GameControl.MeleeAttack2, true);
        }
        private void ProcessOtherControls()
        {
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
        }
        private void ProcessButtonPromptAutoControls()
        {
            foreach (ButtonPrompt bp in Player.ButtonPrompts.Prompts.ToList())
            {
                if (bp.Action != null && (bp.IsPressedNow || bp.IsHeldNow || bp.IsFakePressed))
                {
                    bp.Action();
                }
            }
        }
        private void ProcessHotkeyControls()
        {
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
            if (Settings.SettingsManager.ActivitySettings.AllowPlayerCrouching && !Player.IsInVehicle && !RecentlyPressedCrouch && IsPressingCrouchToggle)
            {
                Player.Stance.Crouch();
                GameTimeLastPressedCrouch = Game.GameTime;
            }
            if (!RecentlyPressedSurrender && (IsPressingSurrender || Player.ButtonPrompts.IsPressed("ShowSurrender") || Player.ButtonPrompts.IsPressed("ShowStopSurrender")))
            {
                Player.Surrendering.ToggleSurrender();
                GameTimeLastPressedSurrender = Game.GameTime;
            }
            if (IsPressingGesture && !RecentlyPressedGesture)
            {
                Player.ActivityManager.Gesture();
                GameTimeLastPressedGesture = Game.GameTime;
            }
            if(!RecentlyPressedYell && (IsPressingYell || Player.ButtonPrompts.IsPressed("YellGetDown")))
            {
                EntryPoint.WriteToConsole("INPUT YELL RAN");
                Player.IntimidationManager.YellGetDown();
                GameTimeLastPressedYell = Game.GameTime;
            }


            if (!RecentlyPressedGroupModeToggle && Player.GroupManager.MemberCount > 0 && (IsPressingGroupModeToggle || Player.ButtonPrompts.IsPressed("ToggleGroupMode")))
            {
                EntryPoint.WriteToConsole("INPUT TOGGLE GROUP MODE RAN");
                Player.GroupManager.ToggleMode();
                GameTimeLastPressedGroupModeToggle = Game.GameTime;
            }

        }
        private void ProcessBurnerControls()
        {
            if (IsPressingSimpleCellphone && !RecentlyPressedSimplePhone && !MenuProvider.IsDisplayingMenu && !Player.IsDisplayingCustomMenus && !Player.CuffManager.IsHandcuffed)
            {
                Player.CellPhone.OpenBurner();
                GameTimeLastPressedSimplePhone = Game.GameTime;
            }
        }
        private void ProcessInteractControls()
        {
            if (Player.ButtonPrompts.IsGroupPressed("StartConversation"))//Player.ButtonPromptList.Any(x => x.Group == "StartConversation" && x.IsPressedNow))//string for now...
            {
                Player.ActivityManager.StartConversation();
            }
            else if (Player.ButtonPrompts.IsGroupPressed("StartTransaction"))//Player.ButtonPromptList.Any(x => x.Group == "StartTransaction" && x.IsPressedNow))//string for now...
            {
                Player.ActivityManager.StartConversation();
            }
            else if (Player.ButtonPrompts.IsGroupPressed("InteractableLocation"))//Player.ButtonPromptList.Any(x => x.Group == "InteractableLocation" && x.IsPressedNow))//string for now...
            {
                Player.ActivityManager.StartLocationInteraction();
            }
            else if (Player.ButtonPrompts.IsGroupPressed("Search"))//Player.ButtonPromptList.Any(x => x.Group == "Search" && x.IsPressedNow))//string for now...
            {
                Player.ActivityManager.InspectPed();
            }
            else if (Player.ButtonPrompts.IsGroupPressed("Drag"))//Player.ButtonPromptList.Any(x => x.Group == "Drag" && x.IsPressedNow))//string for now...
            {
                Player.ActivityManager.DragPed();
            }
            else if (Player.ButtonPrompts.IsGroupPressed("Grab"))//Player.ButtonPromptList.Any(x => x.Group == "Grab" && x.IsPressedNow))//string for now...
            {
                Player.ActivityManager.GrabPed();
            }
            else if (Player.ButtonPrompts.IsGroupPressed("HoldUp"))//Player.ButtonPromptList.Any(x => x.Group == "Grab" && x.IsPressedNow))//string for now...
            {
                Player.ActivityManager.StartHoldUp();
            }
        }
        private void ProcessVehicleControls()
        {
            if(Player.CuffManager.IsHandcuffed)
            {
                return;
            }
            if (Player.CurrentVehicle != null)
            {
                if (!RecentlyPressedEngineToggle)
                {
                    if (IsPressingEngineToggle && Settings.SettingsManager.VehicleSettings.AllowSetEngineState)
                    {
                        Player.ActivityManager.ToggleVehicleEngine();
                        GameTimeLastPressedEngineToggle = Game.GameTime;
                    }
                    else if (IsPressingVehicleAccelerate && !Player.CurrentVehicle.Engine.IsRunning && Settings.SettingsManager.VehicleSettings.AllowSetEngineState)
                    {
                        Player.ActivityManager.SetVehicleEngine(true);
                        GameTimeLastPressedEngineToggle = Game.GameTime;
                    }

                }
                if (!RecentlyPressedIndicators)
                {
                    if (Settings.SettingsManager.VehicleSettings.AllowSetIndicatorState)
                    {
                        if (IsPressingHazards)
                        {
                            Player.ActivityManager.ToggleHazards();//Player.CurrentVehicle.Indicators.ToggleHazards();
                            GameTimeLastPressedIndicators = Game.GameTime;
                        }
                        if (IsPressingLeftIndicator)
                        {
                            Player.ActivityManager.ToggleLeftIndicator();
                            GameTimeLastPressedIndicators = Game.GameTime;
                        }
                        if (IsPressingRightIndicator)
                        {
                            Player.ActivityManager.ToggleRightIndicator();
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
                if (Settings.SettingsManager.VehicleSettings.DisableRolloverFlip && Player.IsInVehicle && (Player.CurrentVehicleIsRolledOver || Player.CurrentVehicleIsInAir) && Player.CurrentVehicle != null && !Player.CurrentVehicle.CanAlwaysRollOver)
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
                if(IsUsingController && MenuProvider.IsPressingActionWheelButton && CanToggleAltMenu)
                {
                    //EntryPoint.WriteToConsoleTestLong("TOGGLE ALT MENU RAN");
                    CanToggleAltMenu = false;
                    MenuProvider.ToggleAltMenu();
                   // GameTimeLastPressedAltMenu = Game.GameTime;
                }
                else if (IsPressingDebugMenuKey)
                {
                    MenuProvider.ToggleDebugMenu();
                }
            }
            if(!MenuProvider.IsPressingActionWheelButton && !CanToggleAltMenu)
            {
                CanToggleAltMenu = true;
            }
        }
        private void ProcessButtonPrompts()
        {
            bool hasPrompts = false;
           // List<ButtonPrompt> ActivatedPrompts = new List<ButtonPrompt>();
            foreach (ButtonPrompt bp in Player.ButtonPrompts.Prompts)//.OrderBy(x=> x.Order).ThenBy(x=>x.Identifier))
            {
                if (Player.ButtonPrompts.IsSuspended)
                {
                    bp.Reset();
                }
                else
                {
                    hasPrompts = true;
                    bp.UpdateState();
                }
            }
            if(Player.ButtonPrompts.IsSuspended)
            {
                hasPrompts = false;
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
            string helpText = $"You can also access prompts through the action wheel.~n~Press ";
            helpText += EntryPoint.ModController.FormatControls(Settings.SettingsManager.KeySettings.ControllerActionModifier, Settings.SettingsManager.KeySettings.ControllerAction);
            helpText += " to open";
            Game.DisplayHelp(helpText);
            HasShownControllerHelpPrompt = true;
        }
        private bool IsKeyDownSafe(Keys key, bool isModifier)
        {
            if(key == Keys.None && isModifier)
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
        private bool IsControllerButtonDownSafe(ControllerButtons buttons, bool isModifier)
        {
            if (buttons == ControllerButtons.None && isModifier)
            {
                return true;
            }
            else
            {
                return Game.IsControllerButtonDownRightNow(buttons);
            }
        }
    }
}