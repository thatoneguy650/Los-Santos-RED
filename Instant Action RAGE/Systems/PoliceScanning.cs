using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


public static class PoliceScanning
{
    //private static uint GameTimeInterval;
    //private static uint LOSInterval;
    //private static uint GameTimeCheckedKilled;
    //private static uint K9Interval;
    private static Model K9Model = new Model("a_c_shepherd");
    private static Model LSPDMale = new Model("s_m_y_cop_01");
    private static Model LSPDFemale = new Model("s_f_y_cop_01");
    private static Model LSSDMale = new Model("s_m_y_sheriff_01");
    private static Model LSSDFemale = new Model("s_f_y_sheriff_01");
    //private static uint RandomCopInterval;

    public static List<GTACop> CopPeds { get; private set; } = new List<GTACop>();
    public static List<GTACop> K9Peds { get; private set; } = new List<GTACop>();
    public static List<Ped> Civilians { get; private set; } = new List<Ped>();
    //public static List<GTANewsReporter> Reporters { get; private set; } = new List<GTANewsReporter>();
    public static bool IsRunning { get; set; } = true;
    public static string AgenciesChasingPlayer
    {
        get
        {
            return string.Join(" ", CopPeds.Where(x => x.SeenPlayerSince(60000)).Select(x => x.AssignedAgency.ColorPrefix + x.AssignedAgency.Initials).Distinct().ToArray());
        }
    }
    public static void Initialize()
    {
        //ScanningInterval = 5000;
        //ScanningRange = 200f;
        //LOSInterval = 500;
        //InnocentScanningRange = 10f;

        K9Model.LoadAndWait();
        K9Model.LoadCollisionAndWait();
        LSPDMale.LoadAndWait();
        LSPDMale.LoadCollisionAndWait();
        LSPDFemale.LoadAndWait();
        LSPDFemale.LoadCollisionAndWait();
        LSSDMale.LoadAndWait();
        LSSDMale.LoadCollisionAndWait();
        LSSDFemale.LoadAndWait();
        LSSDFemale.LoadCollisionAndWait();

        //MainLoop();
    }
    public static void Dispose()
    {
        IsRunning = false;
        PoliceSpawning.DeleteNewsTeam();
        foreach(GTACop Cop in CopPeds)
        {
            if(Cop.CopPed.Exists())
            {
                if (Cop.CopPed.IsInAnyVehicle(false))
                    Cop.CopPed.CurrentVehicle.Delete();
                Cop.CopPed.Delete();
            }
        }
        CopPeds.Clear();
    }
    //private static void MainLoop()
    //{
    //    var stopwatch = new Stopwatch();
    //    GameFiber.StartNew(delegate
    //    {
    //        try
    //        {
    //            while (IsRunning)
    //            {
    //                //stopwatch.Start();
    //                //bool PlayerInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
    //                ////Check if we have killed any cops
    //                //int CheckKilledInterval = 200;
    //                //if (Game.GameTime > GameTimeCheckedKilled + CheckKilledInterval) // was 2000
    //                //{
    //                //    Police.CheckKilled();
    //                //    GameTimeCheckedKilled = Game.GameTime;
    //                //}
    //                //if (Game.GameTime > GameTimeInterval + ScanningInterval)
    //                //{
    //                //    ScanForPolice();
    //                //    GameTimeInterval = Game.GameTime;
    //                //}
    //                //if (Game.GameTime > LOSInterval + LOSInterval) // was 2000
    //                //{
    //                //    Police.CheckLOS((PlayerInVehicle) ? (Entity)Game.LocalPlayer.Character.CurrentVehicle : (Entity)Game.LocalPlayer.Character);
    //                //    Police.SetPrimaryPursuer();
    //                //    Police.UpdatePlacePlayerLastSeen();
    //                //}
    //                //if (Settings.SpawnPoliceK9 && 1 == 0 && Game.GameTime > K9Interval + 5555) // was 2000
    //                //{
    //                //    if (Game.LocalPlayer.WantedLevel > 0 && !PlayerInVehicle && K9Peds.Count < 3)
    //                //        PoliceSpawning.CreateK9();
    //                //    PoliceSpawning.MoveK9s();
    //                //    K9Interval = Game.GameTime;
    //                //}
    //                //if (Settings.SpawnRandomPolice && Game.GameTime > RandomCopInterval + 2000)
    //                //{
    //                //    if (Game.LocalPlayer.WantedLevel == 0 && CopPeds.Where(x => x.WasRandomSpawn).Count() < Settings.SpawnRandomPoliceLimit)
    //                //        PoliceSpawning.SpawnRandomCop();
    //                //    PoliceSpawning.RemoveFarAwayRandomlySpawnedCops();
    //                //    RandomCopInterval = Game.GameTime;
    //                //}
    //                //stopwatch.Stop();
    //                //if (stopwatch.ElapsedMilliseconds >= 20)
    //                //    Debugging.WriteToLog("PoliceScanningTick", string.Format("Tick took {0} ms", stopwatch.ElapsedMilliseconds));
    //                //stopwatch.Reset();
    //                GameFiber.Yield();
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            InstantAction.Dispose();
    //            Debugging.WriteToLog("Error", e.Message + " : " + e.StackTrace);
    //        }
    //    });
    //}
    public static void ScanForPolice()
    {
        Ped[] Pedestrians = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 450f, GetEntitiesFlags.ConsiderHumanPeds | GetEntitiesFlags.ExcludePlayerPed).Where(x => x is Ped).ToArray(), (x => (Ped)x));//250
        foreach (Ped Pedestrian in Pedestrians.Where(s => s.Exists() && !s.IsDead && s.IsVisible))
        {
            if(Pedestrian.isPoliceArmy())
            {
                if (!CopPeds.Any(x => x.CopPed == Pedestrian))
                {
                    bool canSee = false;
                    if (Pedestrian.PlayerIsInFront() && Pedestrian.IsInRangeOf(Game.LocalPlayer.Character.Position, 55f) && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Pedestrian, Game.LocalPlayer.Character))
                        canSee = true;


                    GTACop myCop = new GTACop(Pedestrian, canSee, canSee ? Game.GameTime : 0, canSee ? Game.LocalPlayer.Character.Position : new Vector3(0f, 0f, 0f), Pedestrian.Health,Agencies.GetAgencyFromPed(Pedestrian));
                    Pedestrian.IsPersistent = false;
                    if (Settings.OverridePoliceAccuracy)
                        Pedestrian.Accuracy = Settings.PoliceGeneralAccuracy;
                    Pedestrian.Inventory.Weapons.Clear();
                    Police.IssueCopPistol(myCop);
                    NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Pedestrian, 7, false);//No commandeering//https://gtaforums.com/topic/833391-researchguide-combat-behaviour-flags/
                    if (Police.GhostCop != null && Police.GhostCop.Handle == Pedestrian.Handle)
                        continue;

                    CopPeds.Add(myCop);

                    if (Settings.IssuePoliceHeavyWeapons && Police.CurrentPoliceState == Police.PoliceState.DeadlyChase)
                        Police.IssueCopHeavyWeapon(myCop);
                }
            }
            else
            {
                //if (!Civilians.Any(x => x == Pedestrian))
                //{
                //    Civilians.Add(Pedestrian);
                //}
            }
        }
        CopPeds.RemoveAll(x => !x.CopPed.Exists() || x.CopPed.IsDead);
        K9Peds.RemoveAll(x => !x.CopPed.Exists() || x.CopPed.IsDead);
        Civilians.RemoveAll(x => !x.Exists() || x.IsDead);
    }
}
