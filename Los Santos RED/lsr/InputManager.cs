using ExtensionsMethods;
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
    public class InputManager
    {
        private bool IsPressingSurrender
        {
            get
            {
                if (Game.IsKeyDownRightNow(SettingsManager.MySettings.KeyBinding.SurrenderKey) && !Game.IsShiftKeyDownRightNow && !Game.IsControlKeyDownRightNow)
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
                if (Game.IsKeyDownRightNow(SettingsManager.MySettings.KeyBinding.SurrenderKey) && !Game.IsShiftKeyDownRightNow && !Game.IsControlKeyDownRightNow && Game.LocalPlayer.Character.IsInAnyVehicle(false))
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
                if (Game.IsKeyDownRightNow(SettingsManager.MySettings.KeyBinding.DropWeaponKey) && !Game.IsControlKeyDownRightNow)
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
                if (Game.IsKeyDownRightNow(SettingsManager.MySettings.KeyBinding.VehicleKey) && !Game.IsControlKeyDownRightNow)
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
        public InputManager()
        {
        }
        public void Tick()
        {
            SurrenderCheck();
            WeaponDropCheck();
            VehicleCheck();
        }
        private void SurrenderCheck()
        {
            if (IsPressingSurrender && SurrenderManager.CanSurrender)
            {
                if (!Mod.Player.HandsAreUp && !Mod.Player.IsBusted)
                {
                    SurrenderManager.RaiseHands();
                }
            }
            else
            {
                if (Mod.Player.HandsAreUp && !Mod.Player.IsBusted)
                {
                    SurrenderManager.LowerHands();
                }
            }
        }
        private void WeaponDropCheck()
        {
            if (IsPressingDropWeapon && WeaponDroppingManager.CanDropWeapon)
            {
                WeaponDroppingManager.DropWeapon();
            }
        }
        private void VehicleCheck()
        {
            if (IsPressingEngineToggle && Mod.VehicleEngineManager.CanToggleEngine)
            {
                Mod.VehicleEngineManager.ToggleEngine(true, !Mod.VehicleEngineManager.IsEngineRunning);
            }
            if (IsPressingRefuel && VehicleFuelManager.CanPumpFuel)
            {
                VehicleFuelManager.PumpFuel();
            }
            if (IsPressingHazards)
            {
                VehicleIndicatorManager.ToggleHazards();
            }
            if (IsPressingLeftIndicator)
            {
                VehicleIndicatorManager.ToggleLeftIndicator();
            }
            if (IsPressingRightIndicator)
            {
                VehicleIndicatorManager.ToggleRightIndicator();
            }
        }
    }
}