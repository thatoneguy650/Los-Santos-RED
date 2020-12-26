using ExtensionsMethods;
using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class Debug
{
    public Debug()
    {
    }
    public void Update()
    {
        if (Game.IsKeyDown(Keys.NumPad0))
        {
            DebugNumpad0();
        }
        if (Game.IsKeyDown(Keys.NumPad1))
        {
            DebugNumpad1();
        }
        if (Game.IsKeyDown(Keys.NumPad2))
        {
            DebugNumpad2();
        }
        if (Game.IsKeyDown(Keys.NumPad3))
        {
            DebugNumpad3();
        }
        if (Game.IsKeyDown(Keys.NumPad4))
        {
            DebugNumpad4();
        }
        if (Game.IsKeyDown(Keys.NumPad5))
        {
            DebugNumpad5();
        }
        if (Game.IsKeyDown(Keys.NumPad6))
        {
            DebugNumpad6();
        }
        if (Game.IsKeyDown(Keys.NumPad7))
        {
            DebugNumpad7();
        }
        if (Game.IsKeyDown(Keys.NumPad8))
        {
            DebugNumpad8();
        }
        if (Game.IsKeyDown(Keys.NumPad9))
        {
            DebugNumpad9();
        }
    }
    private void DebugNumpad0()
    {
        DebugNonInvincible();
    }
    private void DebugNonInvincible()
    {
        Game.LocalPlayer.Character.IsInvincible = false;
        Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
        Game.Console.Print("KeyDown: You are NOT invicible");
    }
    private void DebugNumpad1()
    {
        DebugInvincible();
    }
    private void DebugInvincible()
    {
        Game.LocalPlayer.Character.IsInvincible = true;
        Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
        Game.Console.Print("KeyDown: You are invicible");
    }
    private void DebugNumpad2()
   {

    }
    private void DebugNumpad3()
    {

    }
    private void DebugNumpad4()
    {

    }
    private void DebugNumpad5()
    {

    }
    private void DebugNumpad6()
    {

    }
    private void DebugNumpad7()
    {

    }
    public void DebugNumpad8()
    {

    }
    private void DebugNumpad9()
    {
        Game.Console.Print("Debugging: Pressed Num9");
        EntryPoint.ModController.Stop();
        Game.LocalPlayer.WantedLevel = 0;
        Game.TimeScale = 1f;
        NativeFunction.Natives.xB4EDDC19532BFB85();
        Game.DisplayNotification("Instant Action Deactivated");
    }
}

