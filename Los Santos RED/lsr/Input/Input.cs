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
        private bool IsHoldingEnter => GameTimeStartedHoldingEnter != 0 && Game.GameTime - GameTimeStartedHoldingEnter >= 75;
        private bool IsPressingSurrender => Game.IsKeyDownRightNow(Settings.SettingsManager.KeyBinding.SurrenderKey) && Game.IsShiftKeyDownRightNow && !Game.IsControlKeyDownRightNow;
        private bool IsPressingDropWeapon => Game.IsKeyDownRightNow(Settings.SettingsManager.KeyBinding.DropWeaponKey) && !Game.IsControlKeyDownRightNow;
        private bool IsPressingRightIndicator => Game.IsKeyDown(Keys.E) && Game.IsShiftKeyDownRightNow;
        private bool IsPressingLeftIndicator => Game.IsKeyDown(Keys.Q) && Game.IsShiftKeyDownRightNow;
        private bool IsPressingHazards => Game.IsKeyDown(Keys.Space) && Game.IsShiftKeyDownRightNow;
        public void Update()
        {
            SurrenderCheck();
            WeaponDropCheck();
            VehicleCheck();
            HoldingEnterCheck();
            ButtonPromptCheck();
            Player.IsHoldingEnter = IsHoldingEnter;
        }
        private void HoldingEnterCheck()
        {
            if (Game.IsControlPressed(2, GameControl.Enter))
            {
                if (GameTimeStartedHoldingEnter == 0)
                {
                    GameTimeStartedHoldingEnter = Game.GameTime;
                }
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
        private void ButtonPromptCheck()
        {
            Game.DisableControlAction(0, GameControl.Talk, true);//dont mess up my other talking!
            Game.DisableControlAction(0, GameControl.Context, true);//dont mess up my other talking!
            foreach (ButtonPrompt bp in Player.ButtonPrompts)
            {
                if (Game.IsKeyDownRightNow(bp.Key) && !bp.IsPressedNow)
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
    }
}