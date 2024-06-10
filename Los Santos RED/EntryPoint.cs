using ExtensionsMethods;
using LosSantosRED.lsr;
using LSR.Vehicles;
using Rage;
using Rage.Attributes;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows.Forms;

[assembly: Rage.Attributes.Plugin("Los Santos RED", Description = "Total Conversion", Author = "Greskrendtregk", PrefersSingleInstance = true, ShouldTickInPauseMenu = true)]
public static class EntryPoint
{
    private static bool HasDependencies = true;
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
        CheckForUpdates();
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
    private static void CheckForUpdates()
    {
        WebClientWithTimeout webClient = new WebClientWithTimeout();
        string receivedData = string.Empty;
        try
        {
            receivedData = webClient.DownloadString("https://www.lcpdfr.com/applications/downloadsng/interface/api.php?do=checkForUpdates&fileId=36665&textOnly=1").Trim();
            string WebLatestVersionFixed = FixWebVersionString(receivedData);
            if (WebLatestVersionFixed != LSRInstalledVersionInfo.FileVersion)
            {
                Version webLatest = new Version(WebLatestVersionFixed);
                Version installed = new Version(LSRInstalledVersionInfo.FileVersion);
                string description = webLatest > installed ? "~r~Update Available:~s~" : "~o~Released:~s~";
                PreStartMessage = $"{PreStartMessage} ~n~{description} v{WebLatestVersionFixed}~s~~n~Current Version: v{LSRInstalledVersionInfo.FileVersion}";
                WriteToConsole($"{description}: New: {WebLatestVersionFixed} Installed: {LSRInstalledVersionInfo.FileVersion}", 0);
            }
        }
        catch(Exception ex)
        {
            PreStartMessage = $"{PreStartMessage} ~n~~n~~r~UPDATE CHECK FAILED~s~";
            WriteToConsole($"Failed to check for updates", 0);
        }
    }
    private static string FixWebVersionString(string webVersionString)
    {
        string fixedString = "";
        int Character = 1;
        foreach (char c in webVersionString)
        {
            if(Character <= 2)
            {
                fixedString += c;
            }
            else if (webVersionString.Length == Character)
            {
                fixedString += c;
            }
            else if (c != '.' && Character <= 4)
            { 
                fixedString += c + ".";
            }
            else
            {
                fixedString += c;
            }
            Character++;
        }
        return fixedString;
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
    //public static void OnUnload(bool isTerminating)
    //{
    //    ModController?.Dispose();
    //}
    public class WebClientWithTimeout : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest wr = base.GetWebRequest(address);
            wr.Timeout = 3000; // timeout in milliseconds (ms)
            return wr;
        }
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