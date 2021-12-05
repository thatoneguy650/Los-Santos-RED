using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

[assembly: Rage.Attributes.Plugin("Los Santos RED", Description = "Total Conversion", Author = "Greskrendtregk")]
public static class EntryPoint
{
    private static bool MenyooRunning = false;
    private static System.Reflection.Assembly assembly;
    private static System.Diagnostics.FileVersionInfo fvi;
    public static int PersistentPedsCreated { get; set; } = 0;
    public static int PersistentPedsNonPersistent { get; set; } = 0;
    public static int PersistentPedsDeleted { get; set; } = 0;
    public static int PersistentVehiclesCreated { get; set; } = 0;
    public static int PersistentVehiclesNonPersistent { get; set; } = 0;
    public static int PersistentVehiclesDeleted { get; set; } = 0;
    private static int LogLevel = 0;
    public static float CellSize { get; private set; } = 50f;
    public static int FocusCellX { get; set; } = 0;
    public static int FocusCellY { get; set; } = 0;
    public static ModController ModController { get; set; }
    public static List<Entity> SpawnedEntities = new List<Entity>();
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
        assembly = System.Reflection.Assembly.GetExecutingAssembly();
        fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
        if (File.Exists("menyoo.asi"))
        {
            Game.DisplayNotification($"~s~Los Santos ~r~RED ~s~v{fvi.FileVersion} ~n~By ~g~Greskrendtregk ~n~~s~Press Shift+F10 to Start~n~~r~Menyoo is not compatible with LSR, please remove it and restart the game");
        }
        else
        {
            Game.DisplayNotification($"~s~Los Santos ~r~RED ~s~v{fvi.FileVersion} ~n~By ~g~Greskrendtregk ~n~~s~Press Shift+F10 to Start");
        }
        while (true)
        {
            if ((ModController == null || !ModController.IsRunning) && Game.IsKeyDown(Keys.F10) && Game.IsShiftKeyDownRightNow)//maybe add cheat string instead of keys?
            {
                if(File.Exists("menyoo.asi"))
                {
                    MenyooRunning = true;
                    Game.DisplayNotification($"~s~Los Santos ~r~RED ~s~v{fvi.FileVersion} ~n~By ~g~Greskrendtregk ~n~~r~Menyoo is not compatible with LSR, please remove it and restart the game");
                }
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
            Game.Console.Print($"m{(MenyooRunning ? 4556 : 0)} v{fvi.FileVersion} - {Message}");
        }
    }
}