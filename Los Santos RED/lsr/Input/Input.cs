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


        private bool IsMoveControlPressed => Game.IsControlPressed(2, GameControl.MoveUpOnly) || Game.IsControlPressed(2, GameControl.MoveRight) || Game.IsControlPressed(2, GameControl.MoveDownOnly) || Game.IsControlPressed(2, GameControl.MoveLeft);
        private bool IsNotHoldingEnter => !Game.IsControlPressed(2, GameControl.Enter);

        private bool RecentlyPressedDoorClose => Game.GameTime - GameTimeLastPressedDoorClose <= 1500;
        private bool RecentlyPressedIndicators => Game.GameTime - GameTimeLastPressedIndicators <= 1500;
        private bool RecentlyPressedEngineToggle => Game.GameTime - GameTimeLastPressedEngineToggle <= 1500;

        public void Update()
        {
            SurrenderCheck();
            WeaponDropCheck();
            VehicleCheck();
            ButtonPromptCheck();
            ConversationCheck();
            ScenarioCheck();
            ControlCheck();
            Player.IsNotHoldingEnter = IsNotHoldingEnter;
            Player.IsMoveControlPressed = IsMoveControlPressed;
            //GameFiber.Yield();
            MenuCheck();
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
                    Player.StartSprinting();
                }
                else
                {
                    Player.StopSprinting();
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
            if (IsPressingDropWeapon && Player.CanDropWeapon && Settings.SettingsManager.PlayerSettings.AllowWeaponDropping)
            {
                Player.DropWeapon();
            }
        }
        private void VehicleCheck()
        {
            if (Player.CurrentVehicle != null)
            {
                if(!RecentlyPressedEngineToggle)
                {
                    if(IsPressingEngineToggle && Settings.SettingsManager.PlayerSettings.AllowSetEngineState)
                    {
                        Player.CurrentVehicle.Engine.Toggle();
                        GameTimeLastPressedEngineToggle = Game.GameTime;
                    }
                }
                if (!RecentlyPressedIndicators)
                {
                    if (Settings.SettingsManager.PlayerSettings.AllowSetIndicatorState)
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
            return false;
        }

    }
}