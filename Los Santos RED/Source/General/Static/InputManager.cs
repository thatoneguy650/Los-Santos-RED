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

public static class InputManager
{
    public static bool IsRunning { get; set; }
    private static bool IsPressingSurrender
    {
        get
        {
            if(Game.IsKeyDownRightNow(SettingsManager.MySettings.KeyBinding.SurrenderKey) && !Game.IsShiftKeyDownRightNow && !Game.IsControlKeyDownRightNow)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    private static bool IsPressingRefuel
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
    private static bool IsPressingDropWeapon
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
    private static bool IsPressingEngineToggle
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
    private static bool IsPressingRightIndicator
    {
        get
        {
            if(Game.IsKeyDown(Keys.E) && Game.IsShiftKeyDownRightNow)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    private static bool IsPressingLeftIndicator
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
    private static bool IsPressingHazards
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
    public static void Initialize()
    {
        IsRunning = true;
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void Tick()
    {
        if (IsRunning)
        {
            if (IsPressingSurrender && SurrenderManager.CanSurrender)
            {
                if (!PlayerStateManager.HandsAreUp && !PlayerStateManager.IsBusted)
                {
                    SurrenderManager.RaiseHands();
                }
            }
            else
            {
                if (PlayerStateManager.HandsAreUp && !PlayerStateManager.IsBusted)
                {
                    SurrenderManager.LowerHands();
                }
            }

            if (IsPressingDropWeapon && WeaponDroppingManager.CanDropWeapon)
            {
                WeaponDroppingManager.DropWeapon();
            }

            if(IsPressingEngineToggle && VehicleEngineManager.CanToggleEngine)
            {
                VehicleEngineManager.ToggleEngine(true, !VehicleEngineManager.IsEngineRunning);
            }


            if (IsPressingRefuel && VehicleFuelManager.CanPumpFuel)
            {
                VehicleFuelManager.PumpFuel();
            }


            if (IsPressingHazards && Game.LocalPlayer.Character.IsInAnyVehicle(false))
            {
                VehicleIndicatorManager.ToggleHazards();
            }
            if (IsPressingLeftIndicator && Game.LocalPlayer.Character.IsInAnyVehicle(false))
            {
                VehicleIndicatorManager.ToggleLeftIndicator();
            }
            if (IsPressingRightIndicator && Game.LocalPlayer.Character.IsInAnyVehicle(false))
            {
                VehicleIndicatorManager.ToggleRightIndicator();
            }





        }
    }
}