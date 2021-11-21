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
        private uint GameTimeLastPressedIndicators;
        private IInputable Player;
        private ISettingsProvideable Settings;
        private IMenuProvideable MenuProvider;
        public Input(IInputable player, ISettingsProvideable settings, IMenuProvideable menuProvider)
        {
            Player = player;
            Settings = settings;
            MenuProvider = menuProvider;
        }
        private uint GameTimeLastPressedEngineToggle;
        private bool IsMoveControlPressed => Game.IsControlPressed(2, GameControl.MoveUpOnly) || Game.IsControlPressed(2, GameControl.MoveRight) || Game.IsControlPressed(2, GameControl.MoveDownOnly) || Game.IsControlPressed(2, GameControl.MoveLeft);
        private bool IsNotHoldingEnter => !Game.IsControlPressed(2, GameControl.Enter);
        public bool IsPressingMenuKey => Game.IsKeyDown(Settings.SettingsManager.KeySettings.MenuKey);
        public bool IsPressingDebugMenuKey => Game.IsKeyDown(Settings.SettingsManager.KeySettings.DebugMenuKey);
        private bool IsPressingSurrender => Game.IsKeyDownRightNow(Settings.SettingsManager.KeySettings.SurrenderKey) && Game.IsShiftKeyDownRightNow;
        private bool IsPressingDropWeapon => Game.IsKeyDownRightNow(Settings.SettingsManager.KeySettings.DropWeaponKey) && !Game.IsControlKeyDownRightNow;
        private bool IsPressingRightIndicator => Game.IsKeyDown(Settings.SettingsManager.KeySettings.RightIndicatorKey) && Game.IsShiftKeyDownRightNow;
        private bool IsPressingLeftIndicator => Game.IsKeyDown(Settings.SettingsManager.KeySettings.LeftIndicatorKey) && Game.IsShiftKeyDownRightNow;
        private bool IsPressingHazards => Game.IsKeyDown(Settings.SettingsManager.KeySettings.HazardKey) && Game.IsShiftKeyDownRightNow;
        private bool RecentlyPressedIndicators => Game.GameTime - GameTimeLastPressedIndicators <= 1500;
        private bool RecentlyPressedEngineToggle => Game.GameTime - GameTimeLastPressedEngineToggle <= 1500;
        public bool IsPressingEngineToggle => Game.IsKeyDown(Settings.SettingsManager.KeySettings.EngineToggle) && Game.IsShiftKeyDownRightNow;
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
                EntryPoint.WriteToConsole($"Excessive Button Prompts {Player.ButtonPrompts.Count}", 1);
            }
            foreach (ButtonPrompt bp in Player.ButtonPrompts)
            {
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
            if (IsPressingDropWeapon && Player.CanDropWeapon)
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
                    if(IsPressingEngineToggle)
                    {
                        Player.CurrentVehicle.Engine.Toggle();
                        GameTimeLastPressedEngineToggle = Game.GameTime;
                    }
                }
                if (!RecentlyPressedIndicators)
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
        }
        private void MenuCheck()
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
}