using LosSantosRED.lsr;
using Rage;
using Rage.Attributes;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

[assembly: Rage.Attributes.Plugin("Los Santos RED", Description = "Total Conversion", Author = "Greskrendtregk", PrefersSingleInstance = true, ShouldTickInPauseMenu = true)]
public static class EntryPoint
{
    public static int LogLevel { get; set; } = 0;
    private static System.Reflection.Assembly LSRAssembly;
    private static System.Diagnostics.FileVersionInfo LSRInstalledVersionInfo;
    private static string PreStartMessage;
    private static DependencyChecker RageNativeUIChecker;
    private static DependencyChecker NaudioChecker;
    public static int PersistentPedsCreated { get; set; } = 0;
    public static int PersistentPedsNonPersistent { get; set; } = 0;
    public static int PersistentPedsDeleted { get; set; } = 0;
    public static int PersistentVehiclesCreated { get; set; } = 0;
    public static int PersistentVehiclesNonPersistent { get; set; } = 0;
    public static int PersistentVehiclesDeleted { get; set; } = 0;
    public static float CellSize { get; private set; } = 50f;
    public static int FocusCellX { get; set; } = 0;
    public static int FocusCellY { get; set; } = 0;
    public static Vector3 FocusPosition { get; set; }
    public static Zone FocusZone { get; set; } = null;
    public static ModController ModController { get; set; }
    public static List<Entity> SpawnedEntities { get; set; } = new List<Entity>();
    public static Color LSRedColor { get; set; } = Color.FromArgb(181, 48, 48);
    public static uint NotificationID { get; set; }
    public static void Main()
    {
        #if DEBUG
                LogLevel = 5;
        #endif

        while (Game.IsLoading)
        {
            GameFiber.Yield();
        }

        Startup();
        Loop();
    }
    private static void Startup()
    {
        GetVersionInfo();
        CheckDependencies();
        NotificationID = Game.DisplayNotification($"{PreStartMessage}");
    }
    private static void Loop()
    {
        while (true)
        {
            if ((ModController == null || !ModController.IsRunning) && Game.IsKeyDown(Keys.F10) && Game.IsShiftKeyDownRightNow)//maybe add cheat string instead of keys?
            {
                if (NotificationID != 0)
                {
                    Game.RemoveNotification(NotificationID);
                }

                Game.FadeScreenOut(500, true);

                ModController = new ModController();
                ModController.Setup();
            }
            GameFiber.Yield();
        }
    }
    private static void GetVersionInfo()
    {
        LSRAssembly = System.Reflection.Assembly.GetExecutingAssembly();
        LSRInstalledVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(LSRAssembly.Location);
        PreStartMessage = $"~s~Los Santos ~r~RED ~s~v{LSRInstalledVersionInfo.FileVersion} ~n~By ~g~Greskrendtregk ~n~~s~Press Shift+F10 to Start";
        WriteToConsole($"Has Started Successfully",0);
    }
    private static void CheckDependencies()
    {
        RageNativeUIChecker = new DependencyChecker("RAGENativeUI.dll", "1.9.2.0");
        RageNativeUIChecker.Verify();
        if (!RageNativeUIChecker.IsValid)
        {
            PreStartMessage = $"{PreStartMessage} ~n~~n~{RageNativeUIChecker.GameMessage}~s~";
        }
        WriteToConsole($"{RageNativeUIChecker.LogMessage}",0);



        NaudioChecker = new DependencyChecker("NAudio.dll", "1.9.0.0");
        NaudioChecker.Verify();
        if (!NaudioChecker.IsValid)
        {
            PreStartMessage = $"{PreStartMessage} ~n~~n~{NaudioChecker.GameMessage}~s~";
        }
        WriteToConsole($"{NaudioChecker.LogMessage}", 0);

    }
    public static void WriteToConsole(string Message) => WriteToConsole(Message, 5);
    public static void WriteToConsole(string Message, int level)
    {
        if (level <= LogLevel)
        {
            Game.Console.Print($"Los Santos RED v{LSRInstalledVersionInfo.FileVersion} - {Message}");
        }
    }
    [ConsoleCommand]
    public static void Command_UnloadLSR()
    {
        ModController?.Dispose();
    }


#if DEBUG
    [ConsoleCommand]
    public static void Command_CreateConfig()
    {
        ModController = new ModController();
        ModController.SetupFileOnly();
    }
#endif
}