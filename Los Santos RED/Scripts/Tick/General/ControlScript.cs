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

public static class ControlScript
{
    public static bool IsRunning { get; set; }
    private static bool IsPressingSurrender
    {
        get
        {
            if(Game.IsKeyDownRightNow(General.MySettings.KeyBinding.SurrenderKey) && !Game.IsShiftKeyDownRightNow && !Game.IsControlKeyDownRightNow)
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
            if (Game.IsKeyDownRightNow(General.MySettings.KeyBinding.DropWeaponKey) && !Game.IsControlKeyDownRightNow)
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
            if (Game.IsKeyDownRightNow(General.MySettings.KeyBinding.VehicleKey) && !Game.IsControlKeyDownRightNow)
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
            if (IsPressingSurrender && Surrender.CanSurrender)
            {
                if (!PlayerState.HandsAreUp && !PlayerState.IsBusted)
                {
                    Surrender.RaiseHands();
                }
            }
            else
            {
                if (PlayerState.HandsAreUp && !PlayerState.IsBusted)
                {
                    Surrender.LowerHands();
                }
            }

            if (IsPressingDropWeapon && WeaponDropping.CanDropWeapon)
            {
                WeaponDropping.DropWeapon();
            }

            if(IsPressingEngineToggle && VehicleEngine.CanToggleEngine)
            {
                VehicleEngine.ToggleEngine(true, !VehicleEngine.IsEngineRunning);
            }
        }
    }
}