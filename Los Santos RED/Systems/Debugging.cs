using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public static class Debugging
{

    public static bool ShowCopTaskStatus;
    public static List<GameFiber> GameFibers;

    public static bool IsRunning { get; set; }
    public static void Initialize()
    {
        ShowCopTaskStatus = false;
        GameFibers = new List<GameFiber>();
        IsRunning = true;
        MainLoop();
    }
    public static void MainLoop()
    {
        var stopwatch = new Stopwatch();
        GameFiber.StartNew(delegate
        {
            try
            {
                while (IsRunning)
                {
                    stopwatch.Start();
                    DebugLoop();
                    stopwatch.Stop();
                    if (stopwatch.ElapsedMilliseconds >= 16)
                        WriteToLog("DebuggingTick", string.Format("Tick took {0} ms", stopwatch.ElapsedMilliseconds));
                    stopwatch.Reset();
                    GameFiber.Yield();
                }
            }
            catch (Exception e)
            {
                Dispose();
                WriteToLog("Error", e.Message + " : " + e.StackTrace);
            }
        });
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    private static void DebugLoop()
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
    private static void DebugNonInvincible()
    {
        Game.LocalPlayer.Character.IsInvincible = false;
        Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
        Debugging.WriteToLog("KeyDown", "You are NOT invicible");
    }
    private static void DebugInvincible()
    {
        Game.LocalPlayer.Character.IsInvincible = true;
        Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
        Debugging.WriteToLog("KeyDown", "You are invicible");
    }
    private static void DebugCopReset()
    {
        Police.CurrentPoliceState = Police.PoliceState.Normal;
        Game.LocalPlayer.WantedLevel = 0;
        Tasking.UntaskAll(true);


        foreach (GTACop Cop in PedScanning.K9Peds.Where(x => x.Pedestrian.Exists() && !x.Pedestrian.IsDead && !x.Pedestrian.IsInHelicopter))
        {
            Cop.Pedestrian.Delete();
        }
        foreach (GTACop Cop in PedScanning.CopPeds.Where(x => x.Pedestrian.Exists() && !x.Pedestrian.IsDead && !x.Pedestrian.IsInAnyVehicle(false) && !x.Pedestrian.IsInHelicopter))
        {
            Cop.Pedestrian.Delete();
        }

        foreach (GTACop Cop in PedScanning.CopPeds.Where(x => x.Pedestrian.Exists() && !x.Pedestrian.IsDead && x.Pedestrian.IsInAnyVehicle(false) && !x.Pedestrian.IsInHelicopter))
        {
            Cop.Pedestrian.CurrentVehicle.Delete();

            Cop.Pedestrian.Delete();
        }


        Ped[] closestPed = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 400f, GetEntitiesFlags.ExcludePlayerPed | GetEntitiesFlags.ConsiderAnimalPeds).Where(x => x is Ped).ToArray(), (x => (Ped)x));
        foreach (Ped dog in closestPed)
        {
            dog.Delete();
        }

        Game.TimeScale = 1f;
        LosSantosRED.IsBusted = false;
        LosSantosRED.BeingArrested = false;
        NativeFunction.Natives.xB4EDDC19532BFB85();


        //PoliceSpawning.Dispose();
    }
    private static void DebugNumpad0()
    {
        DebugNonInvincible();
    }
    private static void DebugNumpad1()
    {
        DebugInvincible();
    }
    private static void DebugNumpad2()
    {
        Police.SetWantedLevel(2, "Debug", true);

    }
    private static void DebugNumpad3()
    {
        Police.SetWantedLevel(0, "Debug", true);
    }

    private static void DebugNumpad4()
    {
        PlayerHealth.IsBleeding = true;
    }
    private static void DebugNumpad5()
    {
        PlayerHealth.IsBleeding = false;
    }
    private static void DebugNumpad6()
    {
        foreach(GTACop MyCop in PedScanning.CopPeds.Where(x => x.RecentlySeenPlayer()))
        {
            MyCop.HasItemsToRadioIn = true;
        }
    }
        
    private static void DebugNumpad7()
    {
        Game.LocalPlayer.Character.PlayAmbientSpeech("REQUEST_BACKUP");
        LosSantosRED.RequestAnimationDictionay("random@arrests");

        List<string> ToChoose = new List<string>() { "generic_radio_enter", "generic_radio_exit", "generic_radio_chatter", "radio_chatter", "radio_enter", "radio_exit" };
        string ToAnimate = ToChoose.PickRandom();
        NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "random@arrests", ToAnimate, 2.0f, -2.0f, -1, 52, 0, false, false, false);

        Debugging.WriteToLog("DebugNumpad7", ToAnimate);
    }
    private static void DebugNumpad8()
    {
        WriteToLog("DebugNumpad7", "--------------------------------");
        WriteToLog("DebugNumpad7", "--------Police Status-----------");
        foreach (GTACop Cop in PedScanning.CopPeds.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsAlive))
        {
            WriteToLog("DebugNumpad7", string.Format("Cop: {0},Model.Name:{1},isTasked: {2},canSeePlayer: {3},DistanceToPlayer: {4},HurtByPlayer: {5},IssuedHeavyWeapon {6},TaskIsQueued: {7},TaskType: {8},WasRandomSpawn: {9},TaskFiber: {10},CurrentTaskStatus: {11},Agency: {12}, DistancetoInvestigation: {13}",
                    Cop.Pedestrian.Handle, Cop.Pedestrian.Model.Name, Cop.IsTasked, Cop.CanSeePlayer, Cop.DistanceToPlayer, Cop.HurtByPlayer, Cop.IssuedHeavyWeapon, Cop.TaskIsQueued, Cop.TaskType, Cop.WasRandomSpawn, Cop.TaskFiber, Cop.Pedestrian.Tasks.CurrentTaskStatus, Cop.AssignedAgency.Initials, Cop.Pedestrian.DistanceTo2D(Police.InvestigationPosition)));
        }

        WriteToLog("DebugNumpad7", string.Format("PoliceInInvestigationMode: {0}", Police.PoliceInInvestigationMode));
        WriteToLog("DebugNumpad7", string.Format("InvestigationPosition: {0}", Police.InvestigationPosition));
        WriteToLog("DebugNumpad7", string.Format("InvestigationDistance: {0}", Police.InvestigationDistance));
        WriteToLog("DebugNumpad7", string.Format("AnyNear Investigation Position: {0}", PedScanning.CopPeds.Any(x => x.Pedestrian.DistanceTo2D(Police.InvestigationPosition) <= Police.InvestigationDistance)));

        WriteToLog("DebugNumpad7", "--------------------------------");
        WriteToLog("DebugNumpad7", "");
        WriteToLog("DebugNumpad7", "--------Player Status-----------");
        WriteToLog("DebugNumpad7", string.Format("PlayerIsPersonOfInterest: {0}", PersonOfInterest.PlayerIsPersonOfInterest));
        WriteToLog("DebugNumpad7", string.Format("JustTakenOver: {0}", PedSwapping.JustTakenOver(5000)));
        WriteToLog("DebugNumpad7", string.Format("Current Zone: {0}", Zones.GetZoneStringAtLocation(Game.LocalPlayer.Character.Position)));
        if (PlayerLocation.PlayerCurrentStreet != null)
            WriteToLog("DebugNumpad6", string.Format("Street: {0}", PlayerLocation.PlayerCurrentStreet.Name));
        if (PlayerLocation.PlayerCurrentCrossStreet != null)
            WriteToLog("DebugNumpad6", string.Format("Cross Street: {0}", PlayerLocation.PlayerCurrentCrossStreet.Name));
        WriteToLog("DebugNumpad6", string.Format("PlayerCoordinates: {0},{1},{2}", Game.LocalPlayer.Character.Position.X, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z));
        foreach (GTAPed DeadPerson in PedScanning.PlayerKilledCivilians)
        {
            WriteToLog("DebugNumpad7", string.Format("Player Killed: Handle: {0}, Distance: {1}", DeadPerson.Pedestrian.Handle, Game.LocalPlayer.Character.DistanceTo2D(DeadPerson.Pedestrian)));
        }

        WriteToLog("DebugNumpad7", string.Format("Near Any MurderVictim: {0}", Civilians.NearMurderVictim(15f)));
        WriteToLog("DebugNumpad7", "--------------------------------");
        WriteToLog("DebugNumpad7", "-------Criminal History---------");
        foreach (RapSheet MyRapSheet in PersonOfInterest.CriminalHistory)
        {
            WriteToLog("DebugNumpad7", MyRapSheet.DebugPrintCrimes());
        }
        WriteToLog("DebugNumpad7", "--------------------------------");
        WriteToLog("DebugNumpad7", "-------Current Crimes-----------");
        WriteToLog("DebugNumpad7", Police.CurrentCrimes.DebugPrintCrimes());
        WriteToLog("DebugNumpad7", "--------------------------------");
        WriteToLog("DebugNumpad7", "-------Game Fibers-----------");
        WriteToLog("DebugNumpad7", string.Join(";", GameFibers.Where(x => x.IsAlive).GroupBy(g => g.Name).Select(group => group.Key + ":" + group.Count())));
        WriteToLog("DebugNumpad7", "--------------------------------");
        WriteToLog("DebugNumpad7", "-------Player State-------------");
        WriteToLog("DebugNumpad8", string.Format("CAN_PLAYER_START_MISSION: {0}", NativeFunction.CallByName<bool>("CAN_PLAYER_START_MISSION", Game.LocalPlayer)));
        WriteToLog("DebugNumpad8", string.Format("IS_PLAYER_CONTROL_ON: {0}", NativeFunction.CallByName<bool>("IS_PLAYER_CONTROL_ON", Game.LocalPlayer)));
        WriteToLog("DebugNumpad8", string.Format("_IS_PLAYER_CAM_CONTROL_DISABLED: {0}", NativeFunction.CallByHash<bool>(0x7C814D2FB49F40C0, Game.LocalPlayer)));
        WriteToLog("DebugNumpad8", string.Format("IS_PLAYER_SCRIPT_CONTROL_ON: {0}", NativeFunction.CallByName<bool>("IS_PLAYER_SCRIPT_CONTROL_ON", Game.LocalPlayer)));
        WriteToLog("DebugNumpad8", string.Format("IsConsideredArmed: {0}", Game.LocalPlayer.Character.IsConsideredArmed()));

    }
    private static void DebugNumpad9()
    {
        DebugCopReset();
        GameFiber.Sleep(1000);
        DebugCopReset();
        Game.DisplayNotification("Instant Action Deactivated");
        ScriptController.Dispose();
    }
    public static void WriteToLog(String ProcedureString, String TextToLog)
    {
        if (ProcedureString == "Error")
        {
            Game.DisplayNotification("Instant Action has Crashed and needs to be restarted");
        }

        if (Settings.Logging)
            Game.Console.Print(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ": " + ProcedureString + ": " + TextToLog);
    }

}

