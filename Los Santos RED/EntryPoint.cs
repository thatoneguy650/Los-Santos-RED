using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

[assembly: Rage.Attributes.Plugin("Los Santos RED", Description = "Total Conversion", Author = "Greskrendtregk")]
public static class EntryPoint
{
    public static int PersistentPedsCreated { get; set; } = 0;
    public static int PersistentPedsNonPersistent { get; set; } = 0;
    public static int PersistentPedsDeleted { get; set; } = 0;
    public static int PersistentVehiclesCreated { get; set; } = 0;
    public static int PersistentVehiclesNonPersistent { get; set; } = 0;
    public static int PersistentVehiclesDeleted { get; set; } = 0;
    private static int LogLevel = 3;
    public static ModController ModController { get; set; }
    public static void Main()
    {
        #if DEBUG
                LogLevel = 5;
        #endif
        while (Game.IsLoading)
        {
            GameFiber.Yield();
        }
        Loop();
    }
    private static void Loop()
    {
        System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
        System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
        Game.DisplayNotification($"~s~Los Santos ~r~RED ~s~v{fvi.FileVersion} ~n~By ~g~Greskrendtregk ~n~~s~Press Shift+F10 to Start");

        while (true)
        {
            if ((ModController == null || !ModController.IsRunning) && Game.IsKeyDown(Keys.F10) && Game.IsShiftKeyDownRightNow)//maybe add cheat string instead of keys?
            {
                ModController = new ModController();
                ModController.Start(); 
            }
            GameFiber.Yield();
        }
    }
    public static void WriteToConsole(string Message, int level)
    {
        if (level <= LogLevel)
        {
            Game.Console.Print(Message);
        }
    }
}