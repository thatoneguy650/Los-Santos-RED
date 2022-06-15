using LosSantosRED.lsr;
using Rage;
using Rage.Attributes;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

[assembly: Rage.Attributes.Plugin("Los Santos RED", Description = "Total Conversion", Author = "Greskrendtregk")]
public static class EntryPoint
{
    private static int LogLevel = 0;
    private static bool MenyooRunning = false;
    private static System.Reflection.Assembly assembly;
    private static System.Diagnostics.FileVersionInfo fvi;
    public static int PersistentPedsCreated { get; set; } = 0;
    public static int PersistentPedsNonPersistent { get; set; } = 0;
    public static int PersistentPedsDeleted { get; set; } = 0;
    public static int PersistentVehiclesCreated { get; set; } = 0;
    public static int PersistentVehiclesNonPersistent { get; set; } = 0;
    public static int PersistentVehiclesDeleted { get; set; } = 0;
    public static float CellSize { get; private set; } = 50f;
    public static int FocusCellX { get; set; } = 0;
    public static int FocusCellY { get; set; } = 0;
    public static Zone FocusZone { get; set; } = null;
    public static ModController ModController { get; set; }
    public static List<Entity> SpawnedEntities = new List<Entity>();
    public static Color LSRedColor { get; set; } = Color.FromArgb(181, 48, 48);
    public static uint NotificationID { get; set; }
    public static string OfficerFriendlyContactName => "Officer Friendly";//these have gotta go, but where?
    public static string UndergroundGunsContactName => "Underground Guns";//these have gotta go, but where?
    public static string EmergencyServicesContactName => "911 - Emergency Services";//these have gotta go, but where?
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
            NotificationID = Game.DisplayNotification($"~s~Los Santos ~r~RED ~s~v{fvi.FileVersion} ~n~By ~g~Greskrendtregk ~n~~s~Press Shift+F10 to Start~n~~n~~r~Menyoo~s~ can cause issues with duplicated items/peds/vehicles, remove it if you encounter issues");
        }
        else
        {
            NotificationID = Game.DisplayNotification($"~s~Los Santos ~r~RED ~s~v{fvi.FileVersion} ~n~By ~g~Greskrendtregk ~n~~s~Press Shift+F10 to Start");
        }
        while (true)
        {
            if ((ModController == null || !ModController.IsRunning) && Game.IsKeyDown(Keys.F10) && Game.IsShiftKeyDownRightNow)//maybe add cheat string instead of keys?
            {
                if (NotificationID != 0)
                {
                    Game.RemoveNotification(NotificationID);
                }
                if (File.Exists("menyoo.asi"))
                {
                    MenyooRunning = true;
                    NotificationID = Game.DisplayNotification($"~s~Los Santos ~r~RED ~s~v{fvi.FileVersion} ~n~By ~g~Greskrendtregk~s~~n~~n~~r~Menyoo~s~ can cause issues with duplicated items/peds/vehicles, remove it if you encounter issues");
                }
                ModController = new ModController();
                ModController.Setup();
            }
            GameFiber.Yield();
        }
    }
    public static void WriteToConsole(string Message)
    {
        if (5 <= LogLevel)
        {
            Game.Console.Print($"m{(MenyooRunning ? 7556 : 0)} v{fvi.FileVersion} - {Message}");
        }
    }
    public static void WriteToConsole(string Message, int level)
    {
        if (level <= LogLevel)
        {
            Game.Console.Print($"m{(MenyooRunning ? 7556 : 0)} v{fvi.FileVersion} - {Message}");
        }
    }
    [ConsoleCommand]
    public static void Command_UnloadLSR()
    {
        ModController?.Dispose();
    }
}