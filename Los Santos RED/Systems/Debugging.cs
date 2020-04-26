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
        WriteToLog("KeyDown", "You are NOT invicible");
    }
    private static void DebugInvincible()
    {
        Game.LocalPlayer.Character.IsInvincible = true;
        Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
        WriteToLog("KeyDown", "You are invicible");
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
        int Toassign = LosSantosRED.PlayerWantedLevel;
        if (Toassign == 5)
            return;
        Toassign++;
        Police.SetWantedLevel(Toassign, "Debug", true);

    }
    private static void DebugNumpad3()
    {
        PedScanning.ClearPoliceCompletely();
        Police.SetWantedLevel(0, "Debug", true);
    }

    private static void DebugNumpad4()
    {
        Agencies.ReadConfig();
    }
    private static void DebugNumpad5()
    {


    }
    private static void DebugNumpad6()
    {

        WriteToLog("DebugNumpad6", string.Format("                      PlayerCoordinates: {0}f,{1}f,{2}f", Game.LocalPlayer.Character.Position.X, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z));
        WriteToLog("DebugNumpad6", string.Format("                      PlayerHeading: {0}", Game.LocalPlayer.Character.Heading));
    }
        
    private static void DebugNumpad7()
    {
        WriteToLog("DebugNumpad7", "--------Agencies-----------");
        foreach (Agency MyAgency in Agencies.AgenciesList)
        {
            WriteToLog("DebugNumpad7", string.Format("Agency: {0} CanSpawn: {1} Min: {2} Max: {3}", MyAgency.Initials, MyAgency.CanSpawn, MyAgency.MinWantedLevelSpawn, MyAgency.MaxWantedLevelSpawn));
        }
    }
    private static void DebugNumpad8()
    {
        try
        {
            WriteToLog("DebugNumpad7", "--------------------------------");
            WriteToLog("DebugNumpad7", "--------Police Status-----------");

            foreach (GTACop Cop in PedScanning.CopPeds.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsAlive && x.AssignedAgency != null))
            {
                if (Cop.Pedestrian.IsInAnyVehicle(false))
                {
                    WriteToLog("DebugNumpad7", string.Format("Cop {0,-20},  Model {1,-20}, Agency {2,-20}, Tasked {3,-20}, TaskType {4,-20}, TaskQueued {5,-20}, RTask {6,-20}, Spawned {7,-20}, CanSee {8,-20}, Distance {9,-20}, Distant IP {10,-20},Vehicle {11,-20},ChaseStatus {12,-20}",
                            Cop.Pedestrian.Handle, Cop.Pedestrian.Model.Name, Cop.AssignedAgency.Initials, Cop.IsTasked, Cop.TaskType, Cop.TaskIsQueued, Cop.Pedestrian.Tasks.CurrentTaskStatus, Cop.WasModSpawned, Cop.CanSeePlayer, Cop.DistanceToPlayer, Cop.Pedestrian.DistanceTo2D(Police.InvestigationPosition),Cop.Pedestrian.CurrentVehicle.Model.Name,Cop.CurrentChaseStatus));// Cop.CanSeePlayer, Cop.DistanceToPlayer, Cop.HurtByPlayer, Cop.IssuedHeavyWeapon, Cop.TaskIsQueued, Cop.TaskType, Cop.WasRandomSpawn, Cop.TaskFiber, Cop.Pedestrian.Tasks.CurrentTaskStatus, Cop.AssignedAgency.Initials, Cop.Pedestrian.DistanceTo2D(Police.InvestigationPosition)));

                }
                else
                {
                    WriteToLog("DebugNumpad7", string.Format("Cop {0,-20},  Model {1,-20}, Agency {2,-20}, Tasked {3,-20}, TaskType {4,-20}, TaskQueued {5,-20}, RTask {6,-20}, Spawned {7,-20}, CanSee {8,-20}, Distance {9,-20}, Distant IP {10,-20},ChaseStatus {11,-20}",
                            Cop.Pedestrian.Handle, Cop.Pedestrian.Model.Name, Cop.AssignedAgency.Initials, Cop.IsTasked, Cop.TaskType, Cop.TaskIsQueued, Cop.Pedestrian.Tasks.CurrentTaskStatus, Cop.WasModSpawned, Cop.CanSeePlayer, Cop.DistanceToPlayer, Cop.Pedestrian.DistanceTo2D(Police.InvestigationPosition),Cop.CurrentChaseStatus));// Cop.CanSeePlayer, Cop.DistanceToPlayer, Cop.HurtByPlayer, Cop.IssuedHeavyWeapon, Cop.TaskIsQueued, Cop.TaskType, Cop.WasRandomSpawn, Cop.TaskFiber, Cop.Pedestrian.Tasks.CurrentTaskStatus, Cop.AssignedAgency.Initials, Cop.Pedestrian.DistanceTo2D(Police.InvestigationPosition)));
                }
            }
            WriteToLog("DebugNumpad7", string.Format("CurrentPoliceTickRunning: {0}", Tasking.CurrentPoliceTickRunning));
            WriteToLog("DebugNumpad7", string.Format("PoliceInInvestigationMode: {0}", Police.PoliceInInvestigationMode));
            WriteToLog("DebugNumpad7", string.Format("InvestigationPosition: {0}", Police.InvestigationPosition));
            WriteToLog("DebugNumpad7", string.Format("InvestigationDistance: {0}", Police.InvestigationDistance));
            WriteToLog("DebugNumpad7", string.Format("ActiveDistance: {0}", Police.ActiveDistance));
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
            WriteToLog("DebugNumpad6", string.Format("PlayerCoordinates: {0}f,{1}f,{2}f", Game.LocalPlayer.Character.Position.X, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z));
            WriteToLog("DebugNumpad6", string.Format("PlayerHeading: {0}", Game.LocalPlayer.Character.Heading));
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

            WriteToLog("DebugNumpad7", "--------Vehicles-----------");
            foreach (Vehicle MyCar in PedScanning.PoliceVehicles.Where(x => x.Exists()))
            {
                WriteToLog("DebugNumpad7", string.Format("Vehicle: {0,-20} {1,-20}", MyCar.Model.Name, MyCar.Health));
            }

            WriteToLog("DebugNumpad7", "--------Unassigned Cops-----------");
            foreach (GTACop Cop in PedScanning.CopPeds.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsAlive && x.AssignedAgency == null))
            {
                WriteToLog("DebugNumpad7", string.Format("Cop {0} {1}", Cop.Pedestrian.Handle,Cop.Pedestrian.Model.Name));
            }

        }
        catch(Exception e)
        {
            WriteToLog("Debugging error", e.Message + e.StackTrace);
        }
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

        if (LosSantosRED.MySettings != null && LosSantosRED.MySettings.General.Logging)
            Game.Console.Print(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ": " + ProcedureString + ": " + TextToLog);
    }

}

