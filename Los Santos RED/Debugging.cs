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
        WantedLevelScript.CurrentPoliceState = WantedLevelScript.PoliceState.Normal;
        Game.LocalPlayer.WantedLevel = 0;
        foreach (Cop Cop in PedList.K9Peds.Where(x => x.Pedestrian.Exists() && !x.Pedestrian.IsDead && !x.Pedestrian.IsInHelicopter))
        {
            Cop.Pedestrian.Delete();
        }
        foreach (Cop Cop in PedList.CopPeds.Where(x => x.Pedestrian.Exists() && !x.Pedestrian.IsDead && !x.Pedestrian.IsInAnyVehicle(false) && !x.Pedestrian.IsInHelicopter))
        {
            Cop.Pedestrian.Delete();
        }

        foreach (Cop Cop in PedList.CopPeds.Where(x => x.Pedestrian.Exists() && !x.Pedestrian.IsDead && x.Pedestrian.IsInAnyVehicle(false) && !x.Pedestrian.IsInHelicopter))
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
        PlayerState.ResetState(true);
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
        int Toassign = PlayerState.WantedLevel;
        if (Toassign == 5)
            return;
        Toassign++;
        WantedLevelScript.SetWantedLevel(Toassign, "Debug", true);

    }
    private static void DebugNumpad3()
    {
        PedList.ClearPoliceCompletely();
        WantedLevelScript.SetWantedLevel(0, "Debug", true);
    }

    private static void DebugNumpad4()
    {
        WriteToLog("MyVariation", "------------------------------");
        if(PlayerLocation.PlayerCurrentZone != null)
        {
            WriteToLog("MyVariation", string.Format("Main Agency: {0},{1},{2}", PlayerLocation.PlayerCurrentZone.InternalGameName,PlayerLocation.PlayerCurrentZone.DisplayName,Jurisdiction.MainAgencyAtZone(PlayerLocation.PlayerCurrentZone.InternalGameName).FullName));
            WriteToLog("MyVariation", string.Format("Random Agency: {0},{1},{2}", PlayerLocation.PlayerCurrentZone.InternalGameName, PlayerLocation.PlayerCurrentZone.DisplayName, Jurisdiction.RandomAgencyAtZone(PlayerLocation.PlayerCurrentZone.InternalGameName).FullName));

            foreach (Agency MyAgency in Jurisdiction.GetAgenciesAtZone(PlayerLocation.PlayerCurrentZone.InternalGameName))
            {
                WriteToLog("MyVariation", string.Format("   Agency List: {0}", MyAgency.FullName));
            }
        }


        //DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.SuspectSpotted, 1));

        //PedVariation MyVariation = LosSantosRED.GetPedVariation(Game.LocalPlayer.Character);
        //WriteToLog("MyVariation", "------------------------------");
        //foreach (PedComponent Comp in MyVariation.MyPedComponents)
        //{
        //    WriteToLog("MyVariation",string.Format("Component: {0},{1},{2},{3}",Comp.ComponentID,Comp.DrawableID,Comp.TextureID,Comp.PaletteID));
        //}
        //foreach (PropComponent prop in MyVariation.MyPedProps)
        //{
        //    WriteToLog("MyVariation", string.Format("Props: {0},{1},{2}", prop.PropID, prop.DrawableID, prop.TextureID));
        //}
        //WriteToLog("MyVariation", "------------------------------");
    }
    private static void DebugNumpad5()
    {
        //GET_NTH_CLOSEST_VEHICLE_NODE_FAVOUR_DIRECTION
        //GET_OFFSET_FROM_ENTITY_IN_WORLD_COORDS




        General.GetStreetPositionandHeading(Game.LocalPlayer.Character.Position, out Vector3 ClosestRoad, out float Heading, true);

        Vector3 OffsetPos = Game.LocalPlayer.Character.GetOffsetPositionFront(50f);

        Vector3 PlayerPos = Game.LocalPlayer.Character.Position;
        Vector3 OutPos = Vector3.Zero;
        float OutHeading = 0f;
        unsafe
        {
            NativeFunction.CallByName<bool>("GET_NTH_CLOSEST_VEHICLE_NODE_FAVOUR_DIRECTION", ClosestRoad.X, ClosestRoad.Y, ClosestRoad.Z, OffsetPos.X, OffsetPos.Y, OffsetPos.Z, 50, &OutPos, &OutHeading, 1, 0x40400000, 0);
        }
        Blip MyBlip = new Blip(OutPos, 10f)
        {
            Name = "Cool",
            Color = Color.Blue,
            Alpha = 0.5f
        };

        GameFiber.Sleep(5000);

        if (MyBlip.Exists())
            MyBlip.Delete();




        //Agency ToChoose = Agencies.AgenciesList.Where(x => x.Initials == "ARMY").FirstOrDefault();
        //WriteToLog("DebugNumpad5", ToChoose.FullName);
        //Ped MyCop = PoliceSpawning.SpawnCopPed(ToChoose, Game.LocalPlayer.Character.GetOffsetPositionFront(5f), false, new List<string>() { "s_m_y_marine_03" });
        //if (MyCop.Exists())
        //    WriteToLog("DebugNumpad5", "Spawned");

    }
    private static void DebugNumpad6()
    {
        ScriptController.OutputTable();


        WriteToLog("DebugNumpad6", string.Format("                      PlayerCoordinates: {0}f,{1}f,{2}f", Game.LocalPlayer.Character.Position.X, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z));
        WriteToLog("DebugNumpad6", string.Format("                      PlayerHeading: {0}", Game.LocalPlayer.Character.Heading));
    }
        
    private static void DebugNumpad7()
    {

       Tasking.OutputTasks();



   

        WriteToLog("CurrentCrimes", "--------------------------------");
        foreach (WantedLevelScript.Crime MyCrime in WantedLevelScript.CurrentCrimes.CrimeList)
        {
            if (MyCrime.IsCurrentlyViolating)
            {

                WriteToLog("IsCurrentlyViolating", MyCrime.Name);
            }
        }
        WriteToLog("CurrentCrimes", "--------------------------------");

        //PoliceSpawning.SpawnRoadblock(Game.LocalPlayer.Character.GetOffsetPositionFront(10F));


        //PoliceSpawning.DebugRoadblock(Game.LocalPlayer.Character.GetOffsetPositionFront(10F));











    }
    public static void DebugNumpad8()
    {

        try
        {
            CameraControl.DebugAbort();

            WriteToLog("DebugNumpad7", "--------------------------------");
            WriteToLog("DebugNumpad7", "--------Police Status-----------");

            foreach (Cop Cop in PedList.CopPeds.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsAlive && x.AssignedAgency != null).OrderBy(x => x.DistanceToPlayer))
            {
                if (Cop.Pedestrian.IsInAnyVehicle(false))
                {
                    WriteToLog("DebugNumpad7", string.Format("Cop {0,-20},  Model {1,-20}, Agency {2,-20}, Zone {3,-20}, TimeBehindPlayer {4,-20}, NA {5,-20}, RTask {6,-20}, Spawned {7,-20}, CanSee {8,-20}, Distance {9,-20}, Distant IP {10,-20},Vehicle {11,-20},ChaseStatus {12,-20},HurtByPlayer {13,-20}",
                            Cop.Pedestrian.Handle, Cop.Pedestrian.Model.Name, Cop.AssignedAgency.Initials, Cop.CurrentZone.DisplayName, Cop.TimeBehindPlayer, "NA", Cop.Pedestrian.Tasks.CurrentTaskStatus, Cop.WasModSpawned, Cop.CanSeePlayer, Cop.DistanceToPlayer, Cop.Pedestrian.DistanceTo2D(Investigation.InvestigationPosition),Cop.Pedestrian.CurrentVehicle.Model.Name,"NA",Cop.HurtByPlayer));// Cop.CanSeePlayer, Cop.DistanceToPlayer, Cop.HurtByPlayer, Cop.IssuedHeavyWeapon, Cop.TaskIsQueued, Cop.TaskType, Cop.WasRandomSpawn, Cop.TaskFiber, Cop.Pedestrian.Tasks.CurrentTaskStatus, Cop.AssignedAgency.Initials, Cop.Pedestrian.DistanceTo2D(InvestigationScript.InvestigationPosition)));

                }
                else
                {
                    WriteToLog("DebugNumpad7", string.Format("Cop {0,-20},  Model {1,-20}, Agency {2,-20}, Zone {3,-20}, TimeBehindPlayer {4,-20}, NA {5,-20}, RTask {6,-20}, Spawned {7,-20}, CanSee {8,-20}, Distance {9,-20}, Distant IP {10,-20},ChaseStatus {11,-20}",
                            Cop.Pedestrian.Handle, Cop.Pedestrian.Model.Name, Cop.AssignedAgency.Initials, Cop.CurrentZone.DisplayName, Cop.TimeBehindPlayer, "NA", Cop.Pedestrian.Tasks.CurrentTaskStatus, Cop.WasModSpawned, Cop.CanSeePlayer, Cop.DistanceToPlayer, Cop.Pedestrian.DistanceTo2D(Investigation.InvestigationPosition),"NA"));// Cop.CanSeePlayer, Cop.DistanceToPlayer, Cop.HurtByPlayer, Cop.IssuedHeavyWeapon, Cop.TaskIsQueued, Cop.TaskType, Cop.WasRandomSpawn, Cop.TaskFiber, Cop.Pedestrian.Tasks.CurrentTaskStatus, Cop.AssignedAgency.Initials, Cop.Pedestrian.DistanceTo2D(InvestigationScript.InvestigationPosition)));
                }
            }
            WriteToLog("DebugNumpad7", string.Format("CurrentPoliceTickRunning: {0}", Tasking.CurrentPoliceTickRunning));
            WriteToLog("DebugNumpad7", string.Format("PoliceInInvestigationMode: {0}", Investigation.InInvestigationMode));
            WriteToLog("DebugNumpad7", string.Format("InvestigationPosition: {0}", Investigation.InvestigationPosition));
            WriteToLog("DebugNumpad7", string.Format("InvestigationDistance: {0}", Investigation.InvestigationDistance));
            WriteToLog("DebugNumpad7", string.Format("ActiveDistance: {0}", Police.ActiveDistance));
            WriteToLog("DebugNumpad7", string.Format("AnyNear Investigation Position: {0}", PedList.CopPeds.Any(x => x.Pedestrian.DistanceTo2D(Investigation.InvestigationPosition) <= Investigation.InvestigationDistance)));

            WriteToLog("DebugNumpad7", "--------------------------------");
            WriteToLog("DebugNumpad7", "");
            WriteToLog("DebugNumpad7", "--------Player Status-----------");
            WriteToLog("DebugNumpad7", string.Format("PlayerIsPersonOfInterest: {0}", PersonOfInterest.PlayerIsPersonOfInterest));
            WriteToLog("DebugNumpad7", string.Format("JustTakenOver: {0}", PedSwap.JustTakenOver(5000)));
            WriteToLog("DebugNumpad7", string.Format("Current Zone: {0}", Zones.GetZoneStringAtLocation(Game.LocalPlayer.Character.Position)));
            if (PlayerLocation.PlayerCurrentStreet != null)
                WriteToLog("DebugNumpad6", string.Format("Street: {0}", PlayerLocation.PlayerCurrentStreet.Name));
            if (PlayerLocation.PlayerCurrentCrossStreet != null)
                WriteToLog("DebugNumpad6", string.Format("Cross Street: {0}", PlayerLocation.PlayerCurrentCrossStreet.Name));
            WriteToLog("DebugNumpad6", string.Format("PlayerCoordinates: {0}f,{1}f,{2}f", Game.LocalPlayer.Character.Position.X, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z));
            WriteToLog("DebugNumpad6", string.Format("PlayerHeading: {0}", Game.LocalPlayer.Character.Heading));
            WriteToLog("DebugNumpad6", string.Format("PlayerWantedCenter: {0}", NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer)));
            //foreach (GTAPed DeadPerson in PedWoundSystem.PlayerKilledCivilians)
            //{
            //    WriteToLog("DebugNumpad7", string.Format("Player Killed: Handle: {0}, Distance: {1}", DeadPerson.Pedestrian.Handle, Game.LocalPlayer.Character.DistanceTo2D(DeadPerson.Pedestrian)));
            //}

            WriteToLog("DebugNumpad7", string.Format("Near Any CivMurderVictim: {0}", PedWounds.NearCivilianMurderVictim(15f)));
            WriteToLog("DebugNumpad7", string.Format("Near Any CopMurderVictim: {0}", PedWounds.NearCopMurderVictim(15f)));
            WriteToLog("DebugNumpad7", "--------------------------------");
            WriteToLog("DebugNumpad7", "-------Criminal History---------");
            foreach (WantedLevelScript.RapSheet MyRapSheet in PersonOfInterest.CriminalHistory)
            {
                WriteToLog("DebugNumpad7", MyRapSheet.DebugPrintCrimes());
            }
            WriteToLog("DebugNumpad7", "--------------------------------");
            WriteToLog("DebugNumpad7", "-------Current Crimes-----------");
            WriteToLog("DebugNumpad7", WantedLevelScript.CurrentCrimes.DebugPrintCrimes());
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
            foreach (Vehicle MyCar in PedList.PoliceVehicles.Where(x => x.Exists()))
            {
                WriteToLog("DebugNumpad7", string.Format("Vehicle: {0,-20} {1,-20}", MyCar.Model.Name, MyCar.Health));
            }

            WriteToLog("DebugNumpad7", "--------Unassigned Cops-----------");
            foreach (Cop Cop in PedList.CopPeds.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsAlive && x.AssignedAgency == null))
            {
                WriteToLog("DebugNumpad7", string.Format("Cop {0} {1}", Cop.Pedestrian.Handle,Cop.Pedestrian.Model.Name));
            }

            WriteToLog("DebugNumpad7", string.Format("Scale {0} {1} {2} {3}", General.MySettings.UI.PlayerStatusScale, General.MySettings.UI.VehicleStatusScale, General.MySettings.UI.StreetScale, General.MySettings.UI.ZoneScale));
            WriteToLog("DebugNumpad7", string.Format("PlayerStatusPosition {0} {1}", General.MySettings.UI.PlayerStatusPositionX, General.MySettings.UI.PlayerStatusPositionY));
            WriteToLog("DebugNumpad7", string.Format("VehicleStatusPosition {0} {1}", General.MySettings.UI.VehicleStatusPositionX, General.MySettings.UI.VehicleStatusPositionY));
            
            WriteToLog("DebugNumpad7", string.Format("StreetPosition {0} {1}", General.MySettings.UI.StreetPositionX, General.MySettings.UI.StreetPositionY));
            WriteToLog("DebugNumpad7", string.Format("ZonePosition {0} {1}", General.MySettings.UI.ZonePositionX, General.MySettings.UI.ZonePositionY));


        }
        catch(Exception e)
        {
            WriteToLog("Debugging error", e.Message + e.StackTrace);
        }
    }
    private static void DebugNumpad9()
    {
        ScriptController.IsRunning = false;
        GameFiber.Sleep(500);
        DebugCopReset();
        Game.DisplayNotification("Instant Action Deactivated");
        ScriptController.Dispose();
    }
    public static void WriteToLog(String ProcedureString, String TextToLog)
    {
        if (ProcedureString == "Error")
        {
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~has crashed and needs to be restarted");
        }

        if (General.MySettings != null && General.MySettings.General.Logging)
            Game.Console.Print(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ": " + ProcedureString + ": " + TextToLog);
    }

}

