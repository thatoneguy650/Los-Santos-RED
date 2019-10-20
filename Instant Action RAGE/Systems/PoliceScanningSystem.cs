using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExtensionsMethods;
using System.Drawing;
using System.IO;
using System.Diagnostics;

namespace Instant_Action_RAGE.Systems
{
    internal static class PoliceScanningSystem
    {
        private static uint GameTimeInterval;
        //private static uint VehicleReplaceInterval;
        private static uint LOSInterval;
        private static Random rnd;
        private static List<Vehicle> ReplacedVehicles = new List<Vehicle>();
        public static List<PoliceTask> CopsToTask = new List<PoliceTask>();
        private static uint K9Interval;

        public static List<Entity> NewsTeam = new List<Entity>();

        static PoliceScanningSystem()
        {
            rnd = new Random();
        }
        enum PoliceState
        {
            Normal = 0,
            UnarmedChase = 1,
            CautiousChase = 2,
            DeadlyChase = 3,
            ArrestedWait = 4,
        }
        public enum PoliceAgencies
        {
            LSPD = 0,
            LSSD = 1,
            DOA = 2,
            FIB = 3,
            IAA = 4,
            SAPR = 5,
        }
        public static List<GTACop> CopPeds { get; private set; }
        public static List<GTACop> K9Peds { get; private set; }

        public static List<GTANewsReporter> Reporters { get; private set; }
        public static int ScanningInterval { get; private set; }
        public static float ScanningRange { get; private set; }
        public static float InnocentScanningRange { get; private set; }
        public static bool InnocentsNear { get; private set; }
        public static bool SpawnRandomCops { get; set; } = true;
        public static bool Enabled { get; set; } = true;
        public static bool PlayerHurtPolice { get; set; } = false;
        public static bool PlayerKilledPolice { get; set; } = false;
        public static Vector3 PlacePlayerLastSeen
        {
            get
            {
                if (!CopPeds.Any(x => x.GameTimeLastSeenPlayer > 0))
                    return new Vector3(0f, 0f, 0f);
                else
                    return CopPeds.Where(x => x.GameTimeLastSeenPlayer > 0).OrderByDescending(x => x.GameTimeLastSeenPlayer).FirstOrDefault().PositionLastSeenPlayer;
            }
        }
        public static bool IsRunning { get; set; } = true;
        public static GTACop PrimaryPursuer { get; set; }
        public static int CopsKilledByPlayer { get; set; } = 0;
        private static Model K9Model = new Model("a_c_shepherd");
        private static Model LSPDMale = new Model("s_m_y_cop_01");
        private static Model LSPDFemale = new Model("s_f_y_cop_01");
        private static Model LSSDMale = new Model("s_m_y_sheriff_01");
        private static Model LSSDFemale = new Model("s_f_y_sheriff_01");

        private static Vehicle NewsChopper;
        private static List<long> FrameTimes = new List<long>();
        private static uint RandomCopInterval;

        public static void Initialize()
        {
            ScanningInterval = 5000;
            ScanningRange = 200f;
            InnocentScanningRange = 10f;
            CopPeds = new List<GTACop>();
            K9Peds = new List<GTACop>();
            Reporters = new List<GTANewsReporter>();
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


            TaskQueue();
            MainLoop();
        }
        public static void MainLoop()
        {
            var stopwatch = new Stopwatch();
            GameFiber.StartNew(delegate
            {
                while (IsRunning)
                {
                     CheckKilled();
                    stopwatch.Start();
                    bool PlayerInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
                    int losInterval = 500;

                    if (Game.GameTime > GameTimeInterval + ScanningInterval)
                    {
                        ScanForPolice();                        
                        GameTimeInterval = Game.GameTime;
                    }
                    if (Game.GameTime > LOSInterval + losInterval) // was 2000
                    {
                        CheckLOS(PlayerInVehicle);
                        SetPrimaryPursuer();
                        LOSInterval = Game.GameTime;
                    }
                    if (Game.GameTime > K9Interval + 5000) // was 2000
                    {
                        //if (Game.LocalPlayer.WantedLevel > 0 && !PlayerInVehicle && K9Peds.Count < 3)
                        //    CreateK9();

                        //MoveK9s();
                        
                        K9Interval = Game.GameTime;
                    }


                    if(SpawnRandomCops && Game.GameTime > RandomCopInterval + 5000)
                    {

                        //Zones.Zone MyZone = Zones.GetZoneName(Game.LocalPlayer.Character.Position);
                        //WriteToLog("Zones", string.Format("GameName {0},Cops {1}, TextName {2}", MyZone.GameName,MyZone.CopsTypeToDispatch,MyZone.TextName));

                        if(Game.LocalPlayer.WantedLevel == 0 && CopPeds.Where(x => x.WasRandomSpawn).Count() <= 8)
                            SpawnRandomCop(true);
                        RemoveFarAwayRandomlySpawnedCops();
                        RandomCopInterval = Game.GameTime;


                        

                    }





                    //foreach (GTACop Cop in CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead))
                    //{
                    //    if (Cop.InChasingLoop)
                    //        Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Green);
                    //    else
                    //        Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Red);
                    //}




                    //foreach (GTACop Cop in CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead))
                    //{
                    //    if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.InProgress)
                    //        Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Green);
                    //    else if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.Interrupted)
                    //        Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Purple);
                    //    else if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.None)
                    //        Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.White);
                    //    else if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask)
                    //        Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Orange);
                    //    else if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.Preparing)
                    //        Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Red);
                    //    else if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.Unknown)
                    //        Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Black);
                    //    else if (Cop == PrimaryPursuer)
                    //        Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Brown);
                    //    else
                    //        Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Yellow);
                    //}




                    if (Settings.Debug)
                       DebugLoop();


                    stopwatch.Stop();
                    //FrameTimes.Add(stopwatch.ElapsedMilliseconds);

                    //if (FrameTimes.Count >= 100)
                    //{
                    //    WriteToLog("PoliceScanningTick", string.Format("             Avg of Last 100 Ticks {0} ms,Max {1},TotalCops {2}", FrameTimes.Average(), FrameTimes.Max(), CopPeds.Count()));
                    //    FrameTimes.Clear();
                    //}
                    if (stopwatch.ElapsedMilliseconds >= 20)
                    {
                        WriteToLog("PoliceScanningTick", string.Format("Tick took {0} ms", stopwatch.ElapsedMilliseconds));
                    }
                    stopwatch.Reset();
                    GameFiber.Yield();
                }
            });
        }

        private static void MoveK9s()
        {
            foreach(GTACop K9 in K9Peds)
            {
                if(K9.CopPed.IsInAnyVehicle(false))
                {
                    GTACop ClosestDriver = CopPeds.Where(x => x.CopPed.IsInAnyVehicle(false) && !x.isInHelicopter && x.CopPed.CurrentVehicle.Driver == x.CopPed && x.CopPed.CurrentVehicle.IsSeatFree(1)).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
                    if(ClosestDriver != null)
                    {
                        PutK9InCar(K9, ClosestDriver);
                    }
                }
            }
            
        }

        private static void DebugLoop()
        {
            if (Game.IsKeyDown(Keys.NumPad4) & Game.IsAltKeyDownRightNow) // Our menu on/off switch.
            {
                //WriteToLog("KeyDown", "==========");
                //foreach (GTACop Cop in CopPeds.Where(x => x.CopPed.Exists()))
                //{
                //    WriteToLog("KeyDown", string.Format("Handle: {0}, Can See Player: {1}, IsTasked: {2}, DistanceToPlayer: {3},HurtbyPlayer: {4}, Health: {5}, CanSpeak: {6}", Cop.CopPed.Handle,Cop.canSeePlayer,Cop.isTasked,Cop.DistanceToPlayer,Cop.HurtByPlayer,Cop.CopPed.Health,Cop.CanSpeak));
                //}
                //WriteToLog("KeyDown", "==========");
                //WriteToLog("KeyDown", string.Format("Total Cops: {0}", PoliceScanningSystem.CopPeds.Where(x => x.CopPed.Exists()).Count()));
            }
            if (Game.IsKeyDown(Keys.NumPad6)) // Our menu on/off switch.
            {

                //Ped Doggo = new Ped("a_c_rottweiler", Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0f, 4f, 0f)), 180);
                //Doggo.BlockPermanentEvents = true;
                //Doggo.IsPersistent = false;
                ////Doggo.RelationshipGroup = "COPDOGS";
                ////Game.SetRelationshipBetweenRelationshipGroups("COPDOGS", "COP", Relationship.Like);
                ////Game.SetRelationshipBetweenRelationshipGroups("COP", "COPDOGS", Relationship.Like);
                ////Doggo.Health = 50;

                ////Game.SetRelationshipBetweenRelationshipGroups("COPDOGS", "PLAYER", Relationship.Hate);
                ////Game.SetRelationshipBetweenRelationshipGroups("PLAYER", "COPDOGS", Relationship.Hate);

                //TempK9 = Doggo;
                ////NativeFunction.CallByName<bool>("TASK_COMBAT_HATED_TARGETS_AROUND_PED", Doggo, 75f, 0);





                //Doggo.Tasks.FightAgainst(Game.LocalPlayer.Character);
                ////Doggo.KeepTasks = true;

                //////PrimaryPursuer.CopPed.PlayAmbientSpeech("s_m_y_cop_01_white_full_01","DRAW_GUN",1,SpeechModifier.Force);
                ////CreateK9();
                ////GTACop K9 = K9Peds.FirstOrDefault();
                ////K9.CopPed.Position = Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0f, 4f, 0f));


                ////K9.CopPed.BlockPermanentEvents = true;






                //unsafe
                //{
                //    int lol = 0;
                //    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                //   // NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Game.LocalPlayer.Character, -1, 20f, 500f, 1073741824, 1); //Original and works ok
                //    NativeFunction.CallByName<bool>("TASK_COMBAT_PED", 0, Game.LocalPlayer.Character, 1, 16);
                //    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                //    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                //    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", K9.CopPed, lol);
                //    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                //}





                //K9.CopPed.Tasks.FightAgainst(Game.LocalPlayer.Character,90000);

                //NativeFunction.CallByName<bool>("TASK_COMBAT_PED", K9.CopPed, Game.LocalPlayer.Character, 0, 16);

            }
            if (Game.IsKeyDown(Keys.NumPad7)) // Our menu on/off switch.
            {
                //TempK9.Delete();
                //K9Peds.ForEach(x => x.CopPed.Delete());
                //InstantAction.CurrentPoliceState = InstantAction.PoliceState.Normal;
                //InstantAction.ResetPlayer(true, true);
            }
            if (Game.IsKeyDown(Keys.NumPad8)) // Our menu on/off switch.
            {
                //Game.LocalPlayer.WantedLevel--;
               // SpawnNewsChopper();
                //SpawnNewsVan();
            }
            if (Game.IsKeyDown(Keys.NumPad9)) // Our menu on/off switch.
            {
               // DeleteNewsTeam();
                //Game.LocalPlayer.WantedLevel++;
            }
            if (Settings.Debug)
            {
                foreach (GTACop Cop in CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead))
                {
                    if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.InProgress)
                        Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Green);
                    else if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.Interrupted)
                        Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Purple);
                    else if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.None)
                        Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.White);
                    else if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask)
                        Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Orange);
                    else if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.Preparing)
                        Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Red);
                    else if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.Unknown)
                        Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Black);
                    else if (Cop == PrimaryPursuer)
                        Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Brown);
                    else
                        Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Yellow);
                }


                foreach (GTACop Cop in K9Peds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead))
                {
                    if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.InProgress)
                        Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Green);
                    else if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.Interrupted)
                        Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Purple);
                    else if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.None)
                        Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.White);
                    else if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask)
                        Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Orange);
                    else if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.Preparing)
                        Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Red);
                    else if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.Unknown)
                        Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Black);
                    else if (Cop == PrimaryPursuer)
                        Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Brown);
                    else
                        Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Yellow);



                    if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask)
                    {
                        NativeFunction.CallByName<bool>("TASK_COMBAT_HATED_TARGETS_AROUND_PED", Cop.CopPed, 75f, 0);
                        WriteToLog("CreateK9", "Retasked");
                    }
                   // Cop.CopPed.Tasks.FightAgainst(Game.LocalPlayer.Character, 90000);

                }

                Rage.Debug.DrawArrowDebug(new Vector3(PlacePlayerLastSeen.X, PlacePlayerLastSeen.Y, PlacePlayerLastSeen.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Yellow);
            }
        }
        private static void ScanForPolice()
        {
            Ped[] Pedestrians = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 250f, GetEntitiesFlags.ConsiderHumanPeds | GetEntitiesFlags.ExcludePlayerPed).Where(x => x is Ped).ToArray(), (x => (Ped)x));
            foreach (Ped Cop in Pedestrians.Where(s => s.Exists() && !s.IsDead && s.isPoliceArmy() && s.IsVisible))
            {
                if (!CopPeds.Any(x => x.CopPed == Cop))
                {
                    bool canSee = false;
                    if (Cop.PlayerIsInFront() && Cop.IsInRangeOf(Game.LocalPlayer.Character.Position, 55f) && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Cop, Game.LocalPlayer.Character))
                        canSee = true;

                    GTACop myCop = new GTACop(Cop, canSee, canSee ? Game.GameTime : 0, canSee ? Game.LocalPlayer.Character.Position : new Vector3(0f, 0f, 0f),Cop.Health);
                    Cop.IsPersistent = false;
                    Cop.Accuracy = 10;
                    Cop.Inventory.Weapons.Clear();
                    IssueCopPistol(myCop);
                    NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Cop, 7, false);//No commandeering//https://gtaforums.com/topic/833391-researchguide-combat-behaviour-flags/
                    if (InstantAction.GhostCop != null && InstantAction.GhostCop.Handle == Cop.Handle)
                        continue;
                    CopPeds.Add(myCop);

                    if (InstantAction.CurrentPoliceState == InstantAction.PoliceState.DeadlyChase)
                        IssueCopHeavyWeapon(myCop);
                }
            }
            CopPeds.RemoveAll(x => !x.CopPed.Exists() || x.CopPed.IsDead);
            K9Peds.RemoveAll(x => !x.CopPed.Exists() || x.CopPed.IsDead);
        }
        public static void UpdatePolice()
        {
            CopPeds.RemoveAll(x => !x.CopPed.Exists());
            K9Peds.RemoveAll(x => !x.CopPed.Exists() || x.CopPed.IsDead);
            foreach (GTACop Cop in CopPeds)
            {
                if(Cop.CopPed.IsDead)
                {
                    if(PlayerHurtPed(Cop))
                    {
                        Cop.HurtByPlayer = true;
                        PlayerHurtPolice = true;
                    }
                    if(PlayerKilledPed(Cop))
                    {
                        CopsKilledByPlayer++;
                        PlayerKilledPolice = true;
                    }
                    continue;
                }
                int NewHealth = Cop.CopPed.Health;
                if(NewHealth != Cop.Health)
                {
                    if(PlayerHurtPed(Cop))
                    {
                        PlayerHurtPolice = true;
                        Cop.HurtByPlayer = true;
                        WriteToLog("UpdatePolice", String.Format("Cop {0}, Was hurt by player", Cop.CopPed.Handle));
                    }
                    WriteToLog("UpdatePolice", String.Format("Cop {0}, Health Changed from {1} to {2}", Cop.CopPed.Handle,Cop.Health,NewHealth));
                    Cop.Health = NewHealth;
                }
                Cop.isInVehicle = Cop.CopPed.IsInAnyVehicle(false);
                if (Cop.isInVehicle)
                {
                    Cop.isInHelicopter = Cop.CopPed.IsInHelicopter;
                    if(!Cop.isInHelicopter)
                        Cop.isOnBike = Cop.CopPed.IsOnBike;
                }
                else
                {
                    Cop.isInHelicopter = false;
                    Cop.isOnBike = false;
                }
                
                Cop.DistanceToPlayer = Cop.CopPed.RangeTo(Game.LocalPlayer.Character.Position);
            }
            CopPeds.RemoveAll(x => x.CopPed.IsDead);
        }
        public static void IssueCopPistol(GTACop Cop)
        {
            GTAWeapon Pistol;
            Pistol = InstantAction.Weapons.Where(x => x.isPoliceIssue && x.Category == GTAWeapon.WeaponCategory.Pistol).PickRandom();
            Cop.IssuedPistol = Pistol;
            Cop.CopPed.Inventory.GiveNewWeapon(Pistol.Name, Pistol.AmmoAmount, false);
            //WriteToLog("ScanForPolice", string.Format("Cop Issued Pistol: {0}", Pistol.Name));
        }
        public static void IssueCopHeavyWeapon(GTACop Cop)
        {
            GTAWeapon IssuedHeavy;
            int Num = rnd.Next(1, 5);
            if (Num == 1)
                IssuedHeavy = InstantAction.Weapons.Where(x => x.isPoliceIssue && x.Category == GTAWeapon.WeaponCategory.AR).PickRandom();
            else if (Num == 2)
                IssuedHeavy = InstantAction.Weapons.Where(x => x.isPoliceIssue && x.Category == GTAWeapon.WeaponCategory.Shotgun).PickRandom();
            else if (Num == 3)
                IssuedHeavy = InstantAction.Weapons.Where(x => x.isPoliceIssue && x.Category == GTAWeapon.WeaponCategory.SMG).PickRandom();
            else if (Num == 4)
                IssuedHeavy = InstantAction.Weapons.Where(x => x.isPoliceIssue && x.Category == GTAWeapon.WeaponCategory.AR).PickRandom();
            else
                IssuedHeavy = InstantAction.Weapons.Where(x => x.isPoliceIssue && x.Category == GTAWeapon.WeaponCategory.AR).PickRandom();

            Cop.IssuedHeavyWeapon = IssuedHeavy;
            Cop.CopPed.Inventory.GiveNewWeapon(IssuedHeavy.Name, IssuedHeavy.AmmoAmount, true);
            Cop.CopPed.Accuracy = 10;
            //WriteToLog("ScanForPolice", string.Format("Cop Issued Heavy Weapon: {0}", IssuedHeavy.Name));
        }
        private static void CreateK9()
        {
            GTACop ClosestDriver = CopPeds.Where(x => x.CopPed.IsInAnyVehicle(false) && !x.isInHelicopter && x.CopPed.CurrentVehicle.Driver == x.CopPed && x.CopPed.CurrentVehicle.IsSeatFree(1)).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
            if (ClosestDriver != null)
            {
                Ped Doggo = new Ped("a_c_shepherd", ClosestDriver.CopPed.GetOffsetPosition(new Vector3(0f, -10f, 0f)), 180);
                Doggo.BlockPermanentEvents = true;
                Doggo.IsPersistent = false;
                Doggo.RelationshipGroup = "COPDOGS";
                Game.SetRelationshipBetweenRelationshipGroups("COPDOGS", "COP", Relationship.Like);
                Game.SetRelationshipBetweenRelationshipGroups("COP", "COPDOGS", Relationship.Like);
                //Doggo.Health = 50;

                Game.SetRelationshipBetweenRelationshipGroups("COPDOGS", "PLAYER", Relationship.Hate);
                Game.SetRelationshipBetweenRelationshipGroups("PLAYER", "COPDOGS", Relationship.Hate);

                GTACop DoggoCop = new GTACop(Doggo, false, Doggo.Health);
                //PutK9InCar(DoggoCop, ClosestDriver);
                K9Peds.Add(DoggoCop);
                //TaskK9(DoggoCop);
                WriteToLog("CreateK9", String.Format("Created K9 ", Doggo.Handle));
            }

        }
        private static void PutK9InCar(GTACop DoggoCop, GTACop Cop)
        {
            if (!Cop.CopPed.IsInAnyVehicle(false) || Cop.CopPed.IsOnBike || Cop.CopPed.IsInBoat || Cop.CopPed.IsInHelicopter)
                return;
            if (Cop.CopPed.CurrentVehicle.IsSeatFree(1))
                DoggoCop.CopPed.WarpIntoVehicle(Cop.CopPed.CurrentVehicle, 1);
            else
                DoggoCop.CopPed.WarpIntoVehicle(Cop.CopPed.CurrentVehicle, 2);
            WriteToLog("PutK9InCar", String.Format("K9 {0}, put in Car", DoggoCop.CopPed.Handle));
        }
        private static void CheckKilled()
        {
            foreach (GTACop Cop in CopPeds.Where(x => x.CopPed.Exists() && x.CopPed.IsDead))
            {
                if (PlayerKilledPed(Cop))
                {
                    CopsKilledByPlayer++;
                    PlayerKilledPolice = true;
                }
            }
            CopPeds.RemoveAll(x => !x.CopPed.Exists() || x.CopPed.IsDead);
        }
        private static bool PlayerHurtPed(GTACop Cop)
        {
            if (NativeFunction.CallByName<bool>("HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY", Cop.CopPed, Game.LocalPlayer.Character, true))
            {
                WriteToLog("PlayerHurtPed", string.Format("Hurt: {0}", Cop.CopPed.Handle));
                return true;
                
            }
            else if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && NativeFunction.CallByName<bool>("HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY", Cop.CopPed, Game.LocalPlayer.Character.CurrentVehicle, true))
            {
                WriteToLog("PlayerHurtPed", string.Format("Hurt with Car: {0}", Cop.CopPed.Handle));
                return true;
            }
            return false;
        }
        private static bool PlayerKilledPed(GTACop Cop)
        {
            try
            {
                if (Cop.CopPed.IsDead)
                {
                    Entity killer = NativeFunction.Natives.GetPedSourceOfDeath<Entity>(Cop.CopPed);
                    if(killer.Handle == Game.LocalPlayer.Character.Handle || (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Handle == killer.Handle))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                    return false;
            }
            catch (Exception e)
            {
               // Game.LogTrivial(e.Message);
                WriteToLog("PlayerKilledPed", string.Format("Cop got killed by unknow, attributing it to you must be GSW2?{0}, DId you hurt them?: {1}", Cop.CopPed.Handle, Cop.HurtByPlayer));
                if (Cop.HurtByPlayer)
                    return true;
                else
                    return false;
            }
        }
        private static void CheckLOS(bool PlayerInVehicle)
        {
            int i = 0;
            Entity EntityToCheck;
            float RangeToCheck = 55f;
            if (PlayerInVehicle)
            {
                EntityToCheck = Game.LocalPlayer.Character.CurrentVehicle;
            }
            else
            {
                EntityToCheck = Game.LocalPlayer.Character;
            }

            foreach (GTACop Cop in CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.CopPed.IsInHelicopter))
            {
                if (Cop.CopPed.PlayerIsInFront() && Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, RangeToCheck) && !Cop.CopPed.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Cop.CopPed, EntityToCheck)) //was 55f
                {
                    if (Cop.GameTimeContinuoslySeenPlayerSince == 0)
                    {
                        Cop.GameTimeContinuoslySeenPlayerSince = Game.GameTime;
                    }
                    Cop.canSeePlayer = true;
                    Cop.GameTimeLastSeenPlayer = Game.GameTime;
                    Cop.PositionLastSeenPlayer = Game.LocalPlayer.Character.Position;
                    i++;
                    if (PlayerInVehicle)
                        break;
                }
                else
                {
                    Cop.GameTimeContinuoslySeenPlayerSince = 0;
                    Cop.canSeePlayer = false;
                }
            }
            foreach (GTACop Cop in CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && x.CopPed.IsInHelicopter))
            {
                if (Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 250f) && !Cop.CopPed.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY", Cop.CopPed, EntityToCheck, 17)) //was 55f
                {
                    if (Cop.GameTimeContinuoslySeenPlayerSince == 0)
                    {
                        Cop.GameTimeContinuoslySeenPlayerSince = Game.GameTime;
                    }
                    Cop.canSeePlayer = true;
                    Cop.GameTimeLastSeenPlayer = Game.GameTime;
                    Cop.PositionLastSeenPlayer = Game.LocalPlayer.Character.Position;
                    i++;
                }
                else
                {
                    Cop.GameTimeContinuoslySeenPlayerSince = 0;
                    Cop.canSeePlayer = false;
                }
            }
            foreach (GTANewsReporter Reporter in Reporters.Where(x => x.ReporterPed.Exists() && !x.ReporterPed.IsDead && x.ReporterPed.IsInHelicopter))
            {
                if (Reporter.ReporterPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 250f) && !Reporter.ReporterPed.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY", Reporter.ReporterPed, EntityToCheck, 17)) //was 55f
                {
                    if (Reporter.GameTimeContinuoslySeenPlayerSince == 0)
                    {
                        Reporter.GameTimeContinuoslySeenPlayerSince = Game.GameTime;
                    }
                    Reporter.canSeePlayer = true;
                    Reporter.GameTimeLastSeenPlayer = Game.GameTime;
                    Reporter.PositionLastSeenPlayer = Game.LocalPlayer.Character.Position;
                    i++;
                }
                else
                {
                    Reporter.GameTimeContinuoslySeenPlayerSince = 0;
                    Reporter.canSeePlayer = false;
                }
            }

        }
        private static void SetPrimaryPursuer()
        {
            if (CopPeds.Count == 0)
                return;
            foreach (GTACop Cop in CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.CopPed.IsInHelicopter))
            {
                Cop.isPursuitPrimary = false;
            }
            GTACop PursuitPrimary = CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.CopPed.IsInAnyVehicle(false)).OrderBy(x => x.CopPed.Position.DistanceTo2D(Game.LocalPlayer.Character.Position)).FirstOrDefault();
            if (PursuitPrimary == null)
            {
                PrimaryPursuer = null;
                return;
            }
            else
            {
                PrimaryPursuer = PursuitPrimary;
                PursuitPrimary.isPursuitPrimary = true;
            }
        }

        public static void SpawnNewsChopper()
        {

            Ped NewsPilot = new Ped("s_m_m_pilot_01", Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0.0f, 0.0f, 400f)), 0f);
            Ped CameraMan = new Ped("ig_beverly", Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0.0f, 0.0f, 410f)), 0f);
            Ped Assistant = new Ped("s_m_y_grip_01", Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0.0f, 0.0f, 420f)), 0f);     
            NewsChopper = new Vehicle("maverick", Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0.0f,0.0f,500f)), NewsPilot.Heading);
            NewsPilot.WarpIntoVehicle(NewsChopper, -1);
            CameraMan.WarpIntoVehicle(NewsChopper, 1);
            Assistant.WarpIntoVehicle(NewsChopper, 2);
            NewsPilot.BlockPermanentEvents = true;
            CameraMan.BlockPermanentEvents = true;
            Assistant.BlockPermanentEvents = true;
            NativeFunction.CallByName<bool>("TASK_HELI_CHASE", NewsPilot, Game.LocalPlayer.Character, 25f,25f, 40f);
            Reporters.Add(new GTANewsReporter(NewsPilot, false, NewsPilot.Health));
            Reporters.Add(new GTANewsReporter(CameraMan, false, CameraMan.Health));
            Reporters.Add(new GTANewsReporter(Assistant, false, Assistant.Health));
            NewsPilot.KeepTasks = true;
            WriteToLog("SpawnNewsChopper", "News Chopper Spawned");
        }

        public static void SpawnNewsVan()
        {

            Ped CameraMan = new Ped("ig_beverly", Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0.0f, 5f, 0f)), 0f);
            Ped Assistant = new Ped("s_m_y_grip_01", Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0.0f, 5f, 0f)), 0f);





            Rage.Object camera = new Rage.Object("prop_ing_camera_01", CameraMan.GetOffsetPosition(Vector3.RelativeTop * 30));
            CameraMan.Tasks.PlayAnimation("anim@mp_player_intupperphotography", "idle_a_fp", 8.0F, AnimationFlags.Loop);

            camera.AttachTo(CameraMan, 28252, Vector3.Zero, Rotator.Zero);

            //camera.Heading = CameraMan.Heading - 180;
            //camera.Position = CameraMan.GetOffsetPosition(Vector3.RelativeTop * 0.0f + Vector3.RelativeFront * 0.33f);
            //camera.IsPositionFrozen = true;


            //Vector3 SpawnLocation = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around2D(50f));
            //Vehicle NewsVan = new Vehicle("rumpo", SpawnLocation, Assistant.Heading);
            //NativeFunction.CallByName<bool>("SET_VEHICLE_LIVERY", NewsVan, 0);
            //NewsVan.PrimaryColor = Color.Gray;
            // CameraMan.WarpIntoVehicle(NewsVan, 0);
            //Assistant.WarpIntoVehicle(NewsVan, -1);
            CameraMan.BlockPermanentEvents = true;
            Assistant.BlockPermanentEvents = true;



            // NativeFunction.CallByName<bool>("TASK_VEHICLE_ESCORT",
            //NativeFunction.Natives.xFC545A9F0626E3B6(Assistant, NewsVan,Game.LocalPlayer.Character,40.0f, 262144, 10.0f);

            //Assistant.Tasks.ChaseWithGroundVehicle(Game.LocalPlayer.Character);

            //NativeFunction.CallByName<bool>("SET_DRIVER_ABILITY", Assistant, 100f);
            //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", Assistant, 8f);
            //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Assistant, 32, true);

            //Assistant.Tasks.FollowToOffsetFromEntity(Game.LocalPlayer.Character, new Vector3(0f, -20f, 0f));
            Assistant.KeepTasks = true;

            //NewsTeam.Add(NewsVan);
            NewsTeam.Add(CameraMan);
            NewsTeam.Add(Assistant);
        }
        public static void DeleteNewsTeam()
        {
            foreach(GTANewsReporter Reporter in Reporters)
            {
                if(Reporter.ReporterPed.Exists())
                    Reporter.ReporterPed.Delete();
            }
            Reporters.Clear();
            if(NewsChopper.Exists())
                NewsChopper.Delete();
            WriteToLog("DeleteNewsTeam", "News Team Deleted");
        }
        public static void SpawnRandomCop(bool InVehicle)
        {
            Vector3 SpawnLocation = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around2D(500f));
            Zones.Zone ZoneName = Zones.GetZoneName(SpawnLocation);
            if (ZoneName == null)
                return;
            if (ZoneName.CopsTypeToDispatch == Zones.PoliceDispatchType.LSPD)
            {
                int Value = rnd.Next(1, 11);
                if (Value <= 7)
                    SpawnCop(PoliceAgencies.LSPD, SpawnLocation);
                else if (Value == 8)
                    SpawnCop(PoliceAgencies.DOA, SpawnLocation);
                else if (Value == 9)
                    SpawnCop(PoliceAgencies.FIB, SpawnLocation);
                else
                    SpawnCop(PoliceAgencies.IAA, SpawnLocation);
            }
            else if (ZoneName.CopsTypeToDispatch == Zones.PoliceDispatchType.Sheriff)
            {
                int Value = rnd.Next(1, 11);
                if (Value <= 9)
                    SpawnCop(PoliceAgencies.LSSD, SpawnLocation);
                else
                    SpawnCop(PoliceAgencies.DOA, SpawnLocation);
            }
            else if (ZoneName.CopsTypeToDispatch == Zones.PoliceDispatchType.SAPR)
            {
                SpawnCop(PoliceAgencies.SAPR, SpawnLocation);
            }
            else
            {
                SpawnCop(PoliceAgencies.LSPD, SpawnLocation);
            }
        }
        public static void RemoveFarAwayRandomlySpawnedCops()
        {
            foreach (GTACop Cop in CopPeds.Where(x => x.WasRandomSpawn))
            {
                if (Cop.DistanceToPlayer >= 650f)
                {
                    if (Cop.CopPed.IsInAnyVehicle(false))
                        Cop.CopPed.CurrentVehicle.IsPersistent = false;
                    Cop.CopPed.IsPersistent = false;
                    break;
                    //WriteToLog("PoliceScanningTick", "Removed Random Spawn Cop (Distance)");
                }
            }
        }
        public static void RemoveAllRandomlySpawnedCops()
        {
            foreach (GTACop Cop in CopPeds.Where(x => x.WasRandomSpawn))
            {
                if (Cop.DistanceToPlayer >= 150f)
                {
                    if (Cop.CopPed.IsInAnyVehicle(false))
                        Cop.CopPed.CurrentVehicle.IsPersistent = false;
                    Cop.CopPed.IsPersistent = false;
                    break;
                    // WriteToLog("PoliceScanningTick", "Removed Random Spawn Cop");
                }
            }
        }
        public static void SpawnCop(PoliceAgencies _Agency, Vector3 SpawnLocation)
        {
            Ped Cop = SpawnCopPed(_Agency);
            Vehicle CopCar = SpawnCopCruiser(_Agency, SpawnLocation);
            Cop.WarpIntoVehicle(CopCar, -1);
            Cop.IsPersistent = true;
            CopCar.IsPersistent = true;
            Cop.Tasks.CruiseWithVehicle(Cop.CurrentVehicle, 15f, VehicleDrivingFlags.Normal);
            GTACop MyNewCop = new GTACop(Cop, false, Cop.Health);
            IssueCopPistol(MyNewCop);
            MyNewCop.WasRandomSpawn = true;


            bool AddPartner = rnd.Next(1, 11) <= 5;
            if(AddPartner)
            {
                Ped PartnerCop = SpawnCopPed(_Agency);
                PartnerCop.WarpIntoVehicle(CopCar, 0);
                PartnerCop.IsPersistent = true;
                GTACop MyNewPartnerCop = new GTACop(PartnerCop, false, PartnerCop.Health);
                IssueCopPistol(MyNewPartnerCop);
                MyNewPartnerCop.WasRandomSpawn = true;
                CopPeds.Add(MyNewPartnerCop);
            }

            //Blip myBlip = Cop.AttachBlip();
            //Color BlipColor = Color.Black;
            //if(_Agency == PoliceAgencies.LSPD)
            //    BlipColor = Color.Black;
            //else if (_Agency == PoliceAgencies.DOA)
            //    BlipColor = Color.Green;
            //else if (_Agency == PoliceAgencies.FIB)
            //    BlipColor = Color.White;
            //else if (_Agency == PoliceAgencies.IAA)
            //    BlipColor = Color.Purple;
            //else if (_Agency == PoliceAgencies.LSSD)
            //    BlipColor = Color.Brown;
            //else if (_Agency == PoliceAgencies.SAPR)
            //    BlipColor = Color.Blue;
            //else
            //    BlipColor = Color.Black;


            //myBlip.Color = BlipColor;

            CopPeds.Add(MyNewCop);
        }
        public static Ped SpawnCopPed(PoliceAgencies _Agency)
        {
            bool isMale = rnd.Next(1, 11) <= 7; //70% chance Male
            string PedModel;
            if (_Agency == PoliceAgencies.LSPD)
            {
                if (isMale)
                    PedModel = "s_m_y_cop_01";
                else
                    PedModel = "s_f_y_cop_01";
            }
            else if (_Agency == PoliceAgencies.LSSD)
            {
                if (isMale)
                    PedModel = "s_m_y_sheriff_01";
                else
                    PedModel = "s_f_y_sheriff_01";
            }
            else if (_Agency == PoliceAgencies.SAPR)
            {
                if (isMale)
                    PedModel = "s_m_y_ranger_01";
                else
                    PedModel = "s_f_y_ranger_01";
            }
            else if (_Agency == PoliceAgencies.DOA)
            {
                 PedModel = "u_m_m_doa_01";
            }
            else if (_Agency == PoliceAgencies.FIB)
            {
                PedModel = "s_m_m_fibsec_01";
            }
            else if (_Agency == PoliceAgencies.IAA)
            {
                PedModel = "s_m_m_ciasec_01";
            }
            else
            {
                if (isMale)
                    PedModel = "s_m_y_cop_01";
                else
                    PedModel = "s_f_y_cop_01";
            }
            
            Ped Cop = new Ped(PedModel, Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0f, 10f, 50f)), 0f);
            NativeFunction.CallByName<bool>("SET_PED_AS_COP", Cop, true);
            Cop.RandomizeVariation();
            if (_Agency == PoliceAgencies.LSSD || _Agency == PoliceAgencies.LSPD)
            {
                if (isMale && rnd.Next(1, 11) <= 4) //40% Chance of Vest
                    NativeFunction.CallByName<uint>("SET_PED_COMPONENT_VARIATION", Cop, 9, 2, 0, 2);//Vest male only
                if (!InstantAction.IsNightTime)
                    NativeFunction.CallByName<uint>("SET_PED_PROP_INDEX", Cop, 1, 0, 0, 2);//Sunglasses
            }

            return Cop;
        }
        public static Vehicle SpawnCopCruiser(PoliceAgencies _Agency,Vector3 SpawnLocation)
        {
            string CarModel;
            int RandomValue = rnd.Next(1, 11);
            if (_Agency == PoliceAgencies.LSPD)
            {
                if (RandomValue <= 4)
                    CarModel = "police3";
                else if (RandomValue <= 8)
                    CarModel = "police2";
                else
                    CarModel = "police";
            }
            else if (_Agency == PoliceAgencies.LSSD)
            {
                if (RandomValue <= 8)
                    CarModel = "sheriff2";
                else
                    CarModel = "sheriff";
            }
            else if (_Agency == PoliceAgencies.DOA)
            {
                CarModel = "police4";
            }
            else if (_Agency == PoliceAgencies.SAPR)
            {
                CarModel = "pranger";
            }
            else if (_Agency == PoliceAgencies.FIB)
            {
                if (RandomValue <= 8)
                    CarModel = "fbi";
                else
                    CarModel = "fbi2";
            }
            else if (_Agency == PoliceAgencies.IAA)
            {
                CarModel = "police4";
            }
            else
            {
                if (RandomValue <= 8)
                    CarModel = "police3";
                else
                    CarModel = "police2";
            }

            Vehicle CopCar = new Vehicle(CarModel, SpawnLocation, 0f);
            return CopCar;
        }

        //Tasking
        public static void AddItemToQueue(PoliceTask _MyTask)
        {
            if (!CopsToTask.Contains(_MyTask))
                CopsToTask.Add(_MyTask);
        }
        public static void TaskQueue()
        {
            GameFiber.StartNew(delegate
            {
                while (true)
                {
                    int _ToTask = CopsToTask.Count;

                    if (_ToTask > 0)
                    {
                        WriteToLog("TaskQueue", string.Format("Cops To Task: {0}", _ToTask));
                        PoliceTask _policeTask = CopsToTask[0];
                        _policeTask.CopToAssign.isTasked = true;
                        if (_policeTask.TaskToAssign == PoliceTask.Task.Arrest)
                            TaskChasing(_policeTask.CopToAssign);
                        else if (_policeTask.TaskToAssign == PoliceTask.Task.Chase)
                            TaskChasing(_policeTask.CopToAssign);
                        else if (_policeTask.TaskToAssign == PoliceTask.Task.Untask)
                            Untask(_policeTask.CopToAssign);
                        else if (_policeTask.TaskToAssign == PoliceTask.Task.SimpleArrest)
                            TaskSimpleArrest(_policeTask.CopToAssign);
                        else if (_policeTask.TaskToAssign == PoliceTask.Task.SimpleChase)
                            TaskSimpleChase(_policeTask.CopToAssign);
                        else if (_policeTask.TaskToAssign == PoliceTask.Task.VehicleChase)
                            TaskVehicleChase(_policeTask.CopToAssign);

                        _policeTask.CopToAssign.TaskIsQueued = false;
                        CopsToTask.RemoveAt(0);
                    }
                   //GameFiber.Sleep(100);
                    GameFiber.Sleep(250);
                }
            });
        }
        public static void TaskChasing(GTACop Cop)
        {
            if (Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 100f) && Cop.TaskFiber != null && Cop.TaskFiber.Name == "Chase" && !Cop.RecentlySeenPlayer())
            {
                return;
            }
            if (!Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 100f) && Cop.TaskFiber != null)
            {
                Cop.CopPed.Tasks.Clear();
                Cop.CopPed.BlockPermanentEvents = false;
                Cop.TaskFiber.Abort();
                Cop.TaskFiber = null;
                WriteToLog("Task Chasing", string.Format("Initial Return: {0}", Cop.CopPed.Handle));
                return;
            }
            Cop.TaskType = PoliceTask.Task.Chase;
            Cop.TaskFiber =
            GameFiber.StartNew(delegate
            {
                WriteToLog("Task Chasing", string.Format("Started Chase: {0}", Cop.CopPed.Handle));
                uint TaskTime = 0;// = Game.GameTime;
                string LocalTaskName = "GoTo";
                Cop.SimpleTaskName = "Chase";
                NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_USE_CLIMBOVERS", Cop.CopPed, true);
                NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_USE_LADDERS", Cop.CopPed, true);
                NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_DROP_FROM_HEIGHT", Cop.CopPed, true);
                Cop.CopPed.BlockPermanentEvents = true;

                //Main Loop
                while (Cop.CopPed.Exists() && !Cop.CopPed.IsDead)
                {
                    Cop.CopPed.BlockPermanentEvents = true;

                    NativeFunction.CallByName<uint>("SET_PED_MOVE_RATE_OVERRIDE", Cop.CopPed, 1.1f);
                    if (TaskTime == 0 || Game.GameTime - TaskTime >= 250)//250
                    {
                        ArmCopAppropriately(Cop);
                        if (Cop.DistanceToPlayer > 100f)
                            break;

                        Cop.InChasingLoop = true;


                        if (InstantAction.PlayerInVehicle && Game.LocalPlayer.Character.CurrentVehicle.Speed <= 2.5f)
                        {
                            if (Cop.isPursuitPrimary && Cop.DistanceToPlayer <= 25f && LocalTaskName != "CarJack")
                            {
                                Cop.CopPed.CanRagdoll = false;
                                //NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", Cop.CopPed, Game.LocalPlayer.Character.CurrentVehicle, -1, -1, 2f, 9);

                                NativeFunction.CallByName<bool>("TASK_OPEN_VEHICLE_DOOR", Cop.CopPed, Game.LocalPlayer.Character.CurrentVehicle, -1, -1, 10f);


                                Cop.CopPed.KeepTasks = true;
                                TaskTime = Game.GameTime;
                                Cop.SubTaskName = "CarJack";
                                LocalTaskName = "CarJack";
                                WriteToLog("TaskChasing", "Primary Cop SubTasked with CarJack 2");
                            }
                            else if (!Cop.isPursuitPrimary && Cop.DistanceToPlayer <= 25f && LocalTaskName != "Arrest")
                            {
                                NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", Cop.CopPed, Game.LocalPlayer.Character, 4f, 20f);
                                Cop.CopPed.KeepTasks = true;
                                TaskTime = Game.GameTime;
                                Cop.SubTaskName = "Arrest";
                                LocalTaskName = "Arrest";
                                WriteToLog("TaskChasing", string.Format("Cop SubTasked with Car Arrest, {0}", Cop.CopPed.Handle));
                            }
                            //if (Cop.RecentlySeenPlayer() && Cop.DistanceToPlayer <= 25f && Game.GameTime - TaskTime > 3500 && (LocalTaskName != "CarJack" || Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask)) //if (Cop.isPursuitPrimary && Cop.DistanceToPlayer <= 25f && LocalTaskName != "CarJack")
                            //{
                            //    Cop.CopPed.CanRagdoll = false;
                            //    NativeFunction.CallByName<bool>("TASK_OPEN_VEHICLE_DOOR", Cop.CopPed, Game.LocalPlayer.Character.CurrentVehicle, -1, -1, 500f);
                            //    Cop.CopPed.KeepTasks = true;
                            //    TaskTime = Game.GameTime;
                            //    Cop.SubTaskName = "CarJack";
                            //    LocalTaskName = "CarJack";
                            //    WriteToLog("TaskChasing", string.Format("Cop SubTasked with CarJack, {0}", Cop.CopPed.Handle));
                            //}
                            //else if (Cop.RecentlySeenPlayer() && Game.GameTime - TaskTime > 3500 && (LocalTaskName != "Arrest" || Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask))
                            //{
                            //    NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", Cop.CopPed, Game.LocalPlayer.Character, 3f, 20f);
                            //    Cop.CopPed.KeepTasks = true;
                            //    TaskTime = Game.GameTime;
                            //    Cop.SubTaskName = "Arrest";
                            //    LocalTaskName = "Arrest";
                            //    WriteToLog("TaskChasing", string.Format("Cop SubTasked with Car Arrest, {0}", Cop.CopPed.Handle));
                            //}

                        }
                        else
                        {
                            if (LocalTaskName != "Arrest" && (InstantAction.CurrentPoliceState == InstantAction.PoliceState.ArrestedWait || (InstantAction.CurrentPoliceState == InstantAction.PoliceState.CautiousChase && Cop.DistanceToPlayer <= 15f)))
                            {
                                unsafe
                                {
                                    int lol = 0;
                                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                                    NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Game.LocalPlayer.Character, -1, 20f, 500f, 1073741824, 1); //Original and works ok
                                    NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", 0, Game.LocalPlayer.Character, 4f, 20f);
                                    NativeFunction.CallByName<bool>("TASK_AIM_GUN_AT_ENTITY", 0, Game.LocalPlayer.Character, 10000, false);
                                    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                                    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                                    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.CopPed, lol);
                                    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                                }

                                TaskTime = Game.GameTime;

                                Cop.CopPed.KeepTasks = true;
                                Cop.SubTaskName = "Arrest";
                                LocalTaskName = "Arrest";
                                //WriteToLog("TaskChasing", "Cop SubTasked with Arresting");
                            }
                            else if (LocalTaskName != "GotoShooting" && InstantAction.CurrentPoliceState == InstantAction.PoliceState.UnarmedChase && Cop.DistanceToPlayer <= 7f)
                            {
                                Cop.CopPed.CanRagdoll = true;
                                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY_WHILE_AIMING_AT_ENTITY", Cop.CopPed, Game.LocalPlayer.Character, Game.LocalPlayer.Character, 200f, true, 4.0f, 200f, false, false, (uint)FiringPattern.DelayFireByOneSecond);
                                Cop.CopPed.KeepTasks = true;
                                TaskTime = Game.GameTime;
                                Cop.SubTaskName = "GotoShooting";
                                LocalTaskName = "GotoShooting";
                            }
                            else if (LocalTaskName != "Goto" && (InstantAction.CurrentPoliceState == InstantAction.PoliceState.UnarmedChase || InstantAction.CurrentPoliceState == InstantAction.PoliceState.CautiousChase) && Cop.DistanceToPlayer >= 15) //was 15f
                            {
                                Cop.CopPed.CanRagdoll = true;
                                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", Cop.CopPed, Game.LocalPlayer.Character, -1, 5.0f, 500f, 1073741824, 1); //Original and works ok
                                Cop.CopPed.KeepTasks = true;
                                TaskTime = Game.GameTime;
                                LocalTaskName = "Goto";
                                Cop.SubTaskName = "Goto";
                            }

                        }

                        if ((InstantAction.areHandsUp || Game.LocalPlayer.Character.IsStunned || Game.LocalPlayer.Character.IsRagdoll) && !InstantAction.isBusted && Cop.DistanceToPlayer <= 4f)
                            InstantAction.SurrenderBust = true;

                        if(Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Speed <= 4f && !InstantAction.isBusted && Cop.DistanceToPlayer <= 4f)
                            InstantAction.SurrenderBust = true;

                        if (InstantAction.PlayerInVehicle && (Cop.DistanceToPlayer >= 45f || Game.LocalPlayer.Character.CurrentVehicle.Speed >= 10f))
                        {
                            GameFiber.Sleep(rnd.Next(500, 2000));//GameFiber.Sleep(rnd.Next(900, 1500));//reaction time?
                            break;
                        }
                        Cop.CopPed.KeepTasks = true;
                        TaskTime = Game.GameTime;
                    }

                    GameFiber.Yield();
                    if (InstantAction.CurrentPoliceState == InstantAction.PoliceState.Normal || InstantAction.CurrentPoliceState == InstantAction.PoliceState.DeadlyChase)
                    {
                        GameFiber.Sleep(rnd.Next(500, 2000));//GameFiber.Sleep(rnd.Next(900, 1500));//reaction time?
                        break;
                    }
                }
                if (Cop.CopPed.Exists() && !Cop.CopPed.IsDead)
                {
                    Cop.CopPed.BlockPermanentEvents = false;
                    Cop.CopPed.Tasks.Clear();
                }
                WriteToLog("Task Chasing", string.Format("Loop End: {0}", Cop.CopPed.Handle));
                Cop.TaskFiber = null;
                Cop.isTasked = false;
                Cop.SimpleTaskName = "";
                if (Cop.CopPed.Exists() && !Cop.CopPed.IsDead)
                    Cop.CopPed.CanRagdoll = true;


                Cop.InChasingLoop = false;

            }, "Chase");
        }
        public static void ArmCopAppropriately(GTACop Cop)
        {
            if (InstantAction.CurrentPoliceState == InstantAction.PoliceState.UnarmedChase)
            {
                InstantAction.SetCopTazer(Cop);
            }
            else if (InstantAction.CurrentPoliceState == InstantAction.PoliceState.CautiousChase)
            {
                InstantAction.SetCopDeadly(Cop);
            }
            else if (InstantAction.CurrentPoliceState == InstantAction.PoliceState.ArrestedWait && InstantAction.LastPoliceState == InstantAction.PoliceState.UnarmedChase)
            {
                InstantAction.SetCopTazer(Cop);
            }
            else if (InstantAction.CurrentPoliceState == InstantAction.PoliceState.ArrestedWait && InstantAction.LastPoliceState != InstantAction.PoliceState.UnarmedChase)
            {
                InstantAction.SetCopDeadly(Cop);
            }
        }

        public static void TaskChasing_Old(GTACop Cop)
        {
            if (Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 100f) && Cop.TaskFiber != null && Cop.TaskFiber.Name == "Chase" && !Cop.RecentlySeenPlayer())
            {
                return;
            }
            if (!Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 100f) && Cop.TaskFiber != null)
            {
                Cop.CopPed.Tasks.Clear();
                Cop.CopPed.BlockPermanentEvents = false;
                Cop.TaskFiber.Abort();
                Cop.TaskFiber = null;
                WriteToLog("Task Chasing", string.Format("Initial Return: {0}", Cop.CopPed.Handle));
                return;
            }
            Cop.TaskType = PoliceTask.Task.Chase;
            Cop.TaskFiber =
            GameFiber.StartNew(delegate
            {
                WriteToLog("Task Chasing", string.Format("Started Chase: {0}", Cop.CopPed.Handle));
                uint TaskTime = Game.GameTime;
                string LocalTaskName = "GoTo";
                NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_USE_CLIMBOVERS", Cop.CopPed, true);
                NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_USE_LADDERS", Cop.CopPed, true);
                NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_DROP_FROM_HEIGHT", Cop.CopPed, true);
                Cop.CopPed.BlockPermanentEvents = true;

                //Main Loop
                while (Cop.CopPed.Exists() && !Cop.CopPed.IsDead)
                {
                    Cop.CopPed.BlockPermanentEvents = true;

                    //if(InstantAction.LastPoliceState == InstantAction.PoliceState.UnarmedChase)
                    if (InstantAction.CurrentPoliceState == InstantAction.PoliceState.UnarmedChase)
                    {
                        InstantAction.SetCopTazer(Cop);
                    }
                    else if (InstantAction.CurrentPoliceState == InstantAction.PoliceState.CautiousChase)
                    {
                        InstantAction.SetCopDeadly(Cop);
                    }
                    else if (InstantAction.CurrentPoliceState == InstantAction.PoliceState.ArrestedWait && InstantAction.LastPoliceState == InstantAction.PoliceState.UnarmedChase)
                    {
                        InstantAction.SetCopTazer(Cop);
                    }
                    else if (InstantAction.CurrentPoliceState == InstantAction.PoliceState.ArrestedWait && InstantAction.LastPoliceState != InstantAction.PoliceState.UnarmedChase)
                    {
                        InstantAction.SetCopDeadly(Cop);
                    }
                    NativeFunction.CallByName<uint>("SET_PED_MOVE_RATE_OVERRIDE", Cop.CopPed, 1.1f);

                    if (Game.GameTime - TaskTime >= 500)//250
                    {
                        bool PlayerInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
                        uint PlayerVehicleHandle = 0;
                        if (PlayerInVehicle)
                            PlayerVehicleHandle = Game.LocalPlayer.Character.CurrentVehicle.Handle;

                        float DistanceToPlayer = Cop.CopPed.RangeTo(Game.LocalPlayer.Character.Position);
                        Rage.TaskStatus _locTaskStatus = Cop.CopPed.Tasks.CurrentTaskStatus;
                        if (DistanceToPlayer > 100f)
                            break;

                        if (PlayerInVehicle && Game.LocalPlayer.Character.CurrentVehicle.Speed <= 2.5f)
                        {
                            if (Cop.isPursuitPrimary && DistanceToPlayer <= 25f && LocalTaskName != "CarJack")
                            {
                                Cop.CopPed.CanRagdoll = false;
                                //NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", Cop.CopPed, Game.LocalPlayer.Character.CurrentVehicle, -1, -1, 2f, 9);

                                NativeFunction.CallByName<bool>("TASK_OPEN_VEHICLE_DOOR", Cop.CopPed, Game.LocalPlayer.Character.CurrentVehicle, -1, -1, 10f);


                                Cop.CopPed.KeepTasks = true;
                                TaskTime = Game.GameTime;
                                Cop.SubTaskName = "CarJack";
                                LocalTaskName = "CarJack";
                                WriteToLog("TaskChasing", "Primary Cop SubTasked with CarJack 2");
                            }
                            else if (!Cop.isPursuitPrimary && DistanceToPlayer <= 25f && LocalTaskName != "Arrest")
                            {
                                NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", Cop.CopPed, Game.LocalPlayer.Character, 4f, 20f);
                                Cop.CopPed.KeepTasks = true;
                                TaskTime = Game.GameTime;
                                Cop.SubTaskName = "Arrest";
                                LocalTaskName = "Arrest";
                            }

                        }
                        else
                        {
                            if (LocalTaskName != "Arrest" && (InstantAction.CurrentPoliceState == InstantAction.PoliceState.ArrestedWait || (InstantAction.CurrentPoliceState == InstantAction.PoliceState.CautiousChase && DistanceToPlayer <= 15f)))
                            {
                                unsafe
                                {
                                    int lol = 0;
                                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                                    NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Game.LocalPlayer.Character, -1, 20f, 500f, 1073741824, 1); //Original and works ok
                                    NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", 0, Game.LocalPlayer.Character, 4f, 20f);
                                    NativeFunction.CallByName<bool>("TASK_AIM_GUN_AT_ENTITY", 0, Game.LocalPlayer.Character, 10000, false);
                                    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                                    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                                    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.CopPed, lol);
                                    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                                }

                                TaskTime = Game.GameTime;

                                Cop.CopPed.KeepTasks = true;
                                Cop.SubTaskName = "Arrest";
                                LocalTaskName = "Arrest";
                                //WriteToLog("TaskChasing", "Cop SubTasked with Arresting");
                            }
                            else if (LocalTaskName != "GotoShooting" && InstantAction.CurrentPoliceState == InstantAction.PoliceState.UnarmedChase && DistanceToPlayer <= 7f)
                            {
                                Cop.CopPed.CanRagdoll = true;
                                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY_WHILE_AIMING_AT_ENTITY", Cop.CopPed, Game.LocalPlayer.Character, Game.LocalPlayer.Character, 200f, true, 4.0f, 200f, false, false, (uint)FiringPattern.DelayFireByOneSecond);
                                Cop.CopPed.KeepTasks = true;
                                TaskTime = Game.GameTime;
                                Cop.SubTaskName = "GotoShooting";
                                LocalTaskName = "GotoShooting";
                            }
                            else if (LocalTaskName != "Goto" && (InstantAction.CurrentPoliceState == InstantAction.PoliceState.UnarmedChase || InstantAction.CurrentPoliceState == InstantAction.PoliceState.CautiousChase) && DistanceToPlayer >= 15) //was 15f
                            {
                                Cop.CopPed.CanRagdoll = true;
                                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", Cop.CopPed, Game.LocalPlayer.Character, -1, 5.0f, 500f, 1073741824, 1); //Original and works ok
                                Cop.CopPed.KeepTasks = true;
                                TaskTime = Game.GameTime;
                                LocalTaskName = "Goto";
                                Cop.SubTaskName = "Goto";
                            }

                        }

                        if ((InstantAction.areHandsUp || Game.LocalPlayer.Character.IsStunned || Game.LocalPlayer.Character.IsRagdoll) && !InstantAction.isBusted && DistanceToPlayer <= 4f)
                            InstantAction.SurrenderBust = true;

                        //if (Cop.CanSpeak && rnd.Next(0, 100) <= 10)
                        //{
                        //    Cop.GameTimeLastSpoke = Game.GameTime;
                        //    if (LocalTaskName == "Arrest")
                        //        Cop.CopPed.PlayAmbientSpeech("DRAW_GUN");
                        //    else
                        //        Cop.CopPed.PlayAmbientSpeech("CHALLENGE_THREATEN");
                        //    WriteToLog("TaskChasing", "Cop Spoke!");
                        //}

                        if (PlayerInVehicle && (DistanceToPlayer >= 45f || Game.LocalPlayer.Character.CurrentVehicle.Speed >= 10f))
                        {
                            GameFiber.Sleep(rnd.Next(500, 2000));//GameFiber.Sleep(rnd.Next(900, 1500));//reaction time?
                            break;
                        }


                        //if(Cop.CopPed.IsInAnyVehicle(true) && Cop.CopPed.CurrentVehicle.Handle == PlayerVehicleHandle)
                        //{
                        //    Cop.CopPed.Tasks.ClearImmediately();
                        //    Cop.isTasked = false;
                        //    Cop.SimpleTaskName = "";
                        //    WriteToLog("Cop Carjacking", "He should Get Out of the car");
                        //}
                        Cop.CopPed.KeepTasks = true;
                        TaskTime = Game.GameTime;
                    }

                    GameFiber.Yield();
                    if (InstantAction.CurrentPoliceState == InstantAction.PoliceState.Normal || InstantAction.CurrentPoliceState == InstantAction.PoliceState.DeadlyChase)
                    {
                        GameFiber.Sleep(rnd.Next(500, 2000));//GameFiber.Sleep(rnd.Next(900, 1500));//reaction time?
                        break;
                    }
                }
                if (Cop.CopPed.Exists() && !Cop.CopPed.IsDead)
                {
                    Cop.CopPed.BlockPermanentEvents = false;
                    Cop.CopPed.Tasks.Clear();
                }
                WriteToLog("Task Chasing", string.Format("Loop End: {0}", Cop.CopPed.Handle));
                Cop.TaskFiber = null;
                Cop.isTasked = false;
                if (Cop.CopPed.Exists() && !Cop.CopPed.IsDead)
                    Cop.CopPed.CanRagdoll = true;

            }, "Chase");
        }
        public static void TaskSimpleChase(GTACop Cop)
        {
            Cop.TaskType = PoliceTask.Task.SimpleChase;
            Cop.CopPed.BlockPermanentEvents = true;
            Cop.SimpleTaskName = "SimpleChase";
            //Cop.CopPed.Tasks.AimWeaponAt(Game.LocalPlayer.Character.Position,-1);
            Cop.CopPed.Tasks.GoToWhileAiming(Game.LocalPlayer.Character, 10f, 40f);
            Cop.CopPed.KeepTasks = true;
            WriteToLog("TaskSimpleChase", "How many times i this getting called?");
        }
        public static void TaskSimpleArrest(GTACop Cop)
        {
            Cop.TaskType = PoliceTask.Task.SimpleArrest;
            Cop.CopPed.BlockPermanentEvents = true;
            Cop.SimpleTaskName = "SimpleArrest";
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Game.LocalPlayer.Character, -1, 20f, 500f, 1073741824, 1); //Original and works ok
                NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", 0, Game.LocalPlayer.Character, 4f, 20f);
                NativeFunction.CallByName<bool>("TASK_AIM_GUN_AT_ENTITY", 0, Game.LocalPlayer.Character, -1, false);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.CopPed, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
            Cop.CopPed.KeepTasks = true;
            WriteToLog("TaskSimpleArrest", string.Format("Started SimpleArrest: {0}", Cop.CopPed.Handle));
        }
        public static void TaskVehicleChase(GTACop Cop)
        {
            if (!CopPeds.Any(x => x.TaskFiber != null && x.TaskFiber.Name == "Chase"))
            {
                WriteToLog("Task Vehicle Chasing", string.Format("Didn't Start Vehicle Chase: {0}", Cop.CopPed.Handle));
                return; //Only task this is we already have officers on foot
            }
            Cop.TaskType = PoliceTask.Task.VehicleChase;
            Cop.TaskFiber =
           GameFiber.StartNew(delegate
           {
               WriteToLog("Task Vehicle Chasing", string.Format("Started Vehicle Chase: {0}", Cop.CopPed.Handle));
               uint TaskTime = Game.GameTime;
               //string LocalTaskName = "VehicleChase";
               Cop.CopPed.BlockPermanentEvents = true;
               Cop.SimpleTaskName = "VehicleChase";

              // Cop.CopPed.Tasks.ChaseWithGroundVehicle(Game.LocalPlayer.Character);
               NativeFunction.CallByName<bool>("SET_DRIVER_ABILITY", Cop.CopPed, 100f);
              // NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", Cop.CopPed, 8f);
              // NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 32, true);
               Cop.CopPed.KeepTasks = true;

               //Main Loop
               while (Cop.CopPed.Exists() && !Cop.CopPed.IsDead)
               {

                   if (Game.GameTime - TaskTime >= 250)
                   {
                       //if (!CopPeds.Any(x => x.TaskFiber != null && x.TaskFiber.Name == "Chase"))
                       //{
                       //    GameFiber.Sleep(rnd.Next(500, 2000));//GameFiber.Sleep(rnd.Next(900, 1500));//reaction time?
                       //    break;
                       //}

                       if (!Cop.CopPed.IsInAnyVehicle(false))
                       {
                           WriteToLog("Task Vehicle Chase", string.Format("I got out of my car like a dummy: {0}", Cop.CopPed.Handle));
                           break;
                       }

                       if (InstantAction.PlayerInVehicle)
                       {
                           WriteToLog("Task Vehicle Chase", string.Format("Player got in a vehicle, letting ai takeover: {0}", Cop.CopPed.Handle));
                           break;
                       }


                       if (!Cop.RecentlySeenPlayer())
                       {
                           WriteToLog("Task Vehicle Chase", string.Format("Lost the player, let AI takeover: {0}", Cop.CopPed.Handle));
                           break;
                       }


                       Vector3 PlayerPos = Game.LocalPlayer.Character.Position;

                       NativeFunction.CallByName<bool>("SET_DRIVE_TASK_DRIVING_STYLE", Cop.CopPed, 6);

                       //NativeFunction.CallByName<bool>("TASK_VEHICLE_GOTO_NAVMESH", Cop.CopPed, Cop.CopPed.CurrentVehicle, PlayerPos.X, PlayerPos.Y, PlayerPos.Z,10.0f, 156, 3f);
                       // NativeFunction.CallByName<bool>("TASK_VEHICLE_GOTO_NAVMESH", Cop.CopPed, Cop.CopPed.CurrentVehicle, PlayerPos.X, PlayerPos.Y, PlayerPos.Z, 15.0f, 110, 5f); //Best one so far, but they get out
                       //NativeFunction.CallByName<bool>("TASK_VEHICLE_GOTO_NAVMESH", Cop.CopPed, Cop.CopPed.CurrentVehicle, PlayerPos.X, PlayerPos.Y, PlayerPos.Z,10.0f, 171, 3f);

                       //escort?

                       NativeFunction.CallByName<bool>("TASK_VEHICLE_GOTO_NAVMESH", Cop.CopPed, Cop.CopPed.CurrentVehicle, PlayerPos.X, PlayerPos.Y + 10f, PlayerPos.Z, 25f, 110, 10f);


                       Cop.CopPed.KeepTasks = true;
                       TaskTime = Game.GameTime;
                   }
                   GameFiber.Yield();
                   if (InstantAction.CurrentPoliceState == InstantAction.PoliceState.Normal || InstantAction.CurrentPoliceState == InstantAction.PoliceState.DeadlyChase || InstantAction.CurrentPoliceState == InstantAction.PoliceState.ArrestedWait || InstantAction.isBusted || InstantAction.isDead)
                   {
                       GameFiber.Sleep(rnd.Next(500, 2000));//GameFiber.Sleep(rnd.Next(900, 1500));//reaction time?
                       break;
                   }
               }
               if (Cop.CopPed.Exists() && !Cop.CopPed.IsDead)
               {
                   Cop.CopPed.BlockPermanentEvents = false;
                   Cop.CopPed.Tasks.Clear();
               }
               WriteToLog("Task Vehicle Chase", string.Format("Loop End: {0}", Cop.CopPed.Handle));
               Cop.TaskFiber = null;
               Cop.isTasked = false;
               Cop.SimpleTaskName = "";

           },"VehicleChase");

        }
        public static void TaskK9(GTACop Cop)
        {

            Cop.TaskFiber =
            GameFiber.StartNew(delegate
            {
                //WriteToLog("Task K9 Chasing", string.Format("Started Chase: {0}", Cop.CopPed.Handle));
                uint TaskTime = Game.GameTime;
                string LocalTaskName = "GoTo";

                Cop.CopPed.BlockPermanentEvents = true;
                while (Cop.CopPed.Exists() && !Cop.CopPed.IsDead && Cop.CopPed.IsInAnyVehicle(false) && !Cop.CopPed.CurrentVehicle.IsSeatFree(-1))
                    GameFiber.Sleep(2000);


                WriteToLog("Task K9 Chasing", string.Format("Near Player Chase: {0}", Cop.CopPed.Handle));

                while (Cop.CopPed.Exists() && !Cop.CopPed.IsDead)
                {
                    NativeFunction.CallByName<uint>("SET_PED_MOVE_RATE_OVERRIDE", Cop.CopPed, 1.5f);
                    Cop.CopPed.KeepTasks = true;
                    Cop.CopPed.BlockPermanentEvents = true;

                    if (Game.GameTime - TaskTime >= 500)
                    {

                        float _locrangeTo = Cop.CopPed.RangeTo(Game.LocalPlayer.Character.Position);
                        if (LocalTaskName != "Exit" && Cop.CopPed.IsInAnyVehicle(false) && Cop.CopPed.CurrentVehicle.Speed <= 5 && !Cop.CopPed.CurrentVehicle.HasDriver && _locrangeTo <= 75f)
                        {
                            NativeFunction.CallByName<bool>("TASK_LEAVE_VEHICLE", Cop.CopPed, Cop.CopPed.CurrentVehicle, 16);
                            //Cop.CopPed.Heading = Game.LocalPlayer.Character.Heading;
                            TaskTime = Game.GameTime;
                            LocalTaskName = "Exit";
                            WriteToLog("TaskK9Chasing", "Cop SubTasked with Exit");
                        }
                        else if (InstantAction.CurrentPoliceState == InstantAction.PoliceState.ArrestedWait && LocalTaskName != "Arrest")
                        {
                            NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", Cop.CopPed, Game.LocalPlayer.Character, -1, 5.0f, 500f, 1073741824, 1); //Original and works ok
                            TaskTime = Game.GameTime;
                            LocalTaskName = "Arrest";
                            WriteToLog("TaskK9Chasing", "Cop SubTasked with Arresting");
                        }
                        else if ((InstantAction.CurrentPoliceState == InstantAction.PoliceState.UnarmedChase || InstantAction.CurrentPoliceState == InstantAction.PoliceState.CautiousChase || InstantAction.CurrentPoliceState == InstantAction.PoliceState.DeadlyChase) && LocalTaskName != "GotoFighting" && _locrangeTo <= 5f) //was 10f
                        {
                            NativeFunction.CallByName<bool>("TASK_COMBAT_PED", Cop.CopPed, Game.LocalPlayer.Character, 0, 16);
                            Cop.CopPed.KeepTasks = true;
                            //Cop.CopPed.BlockPermanentEvents = false;
                            TaskTime = Game.GameTime;
                            LocalTaskName = "GotoFighting";
                            //GameFiber.Sleep(25000);
                            WriteToLog("TaskK9Chasing", "Cop SubTasked with Fighting");
                        }
                        else if ((InstantAction.CurrentPoliceState == InstantAction.PoliceState.UnarmedChase || InstantAction.CurrentPoliceState == InstantAction.PoliceState.CautiousChase || InstantAction.CurrentPoliceState == InstantAction.PoliceState.DeadlyChase) && LocalTaskName != "Goto" && _locrangeTo >= 45f) //was 15f
                        {
                            NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", Cop.CopPed, Game.LocalPlayer.Character, -1, 5.0f, 500f, 1073741824, 1); //Original and works ok
                            Cop.CopPed.KeepTasks = true;
                            TaskTime = Game.GameTime;
                            LocalTaskName = "Goto";
                            WriteToLog("TaskK9Chasing", "Cop SubTasked with GoTo");
                        }

                        if (InstantAction.CurrentPoliceState == InstantAction.PoliceState.Normal || InstantAction.CurrentPoliceState == InstantAction.PoliceState.DeadlyChase)
                        {
                            GameFiber.Sleep(rnd.Next(500, 2000));//GameFiber.Sleep(rnd.Next(900, 1500));//reaction time?
                            break;
                        }
                    }
                    GameFiber.Yield();
                }
                WriteToLog("Task K9 Chasing", string.Format("Loop End: {0}", Cop.CopPed.Handle));
                Cop.TaskFiber = null;
                
                if (Cop.CopPed.Exists() && !Cop.CopPed.IsDead)
                {
                    Cop.CopPed.IsPersistent = false;
                    Cop.CopPed.BlockPermanentEvents = false;
                    if (!Cop.CopPed.IsInAnyVehicle(false))
                        Cop.CopPed.Tasks.ReactAndFlee(Game.LocalPlayer.Character);
                }

            }, "K9");
        }
        public static void UntaskAll()
        {
            foreach (GTACop Cop in CopPeds)
            {
                if (Cop.isTasked && !Cop.TaskIsQueued)
                {
                    Cop.TaskIsQueued = true;
                    AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Untask));
                }
            }
            foreach (GTACop Cop in K9Peds)
            {
                if (Cop.isTasked && !Cop.TaskIsQueued)
                {
                    Cop.TaskIsQueued = true;
                    AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Untask));
                }
            }
            WriteToLog("UntaskAll", "");
        }
        public static void Untask(GTACop Cop)
        {
            if (Cop.CopPed.Exists())
            {
                if (Cop.TaskFiber != null)
                {
                    Cop.TaskFiber.Abort();
                    Cop.TaskFiber = null;
                }
                if(Cop.isTasked || Cop.SimpleTaskName != "")
                    Cop.CopPed.Tasks.Clear();

                Cop.CopPed.BlockPermanentEvents = false;
                Cop.CopPed.IsPersistent = false;

                WriteToLog("Untask", string.Format("Untasked: {0}", Cop.CopPed.Handle));
            }

            Cop.TaskType = PoliceTask.Task.NoTask;
            Cop.SimpleTaskName = "";
            Cop.isTasked = false;
        }

        private static void WriteToLog(String ProcedureString, String TextToLog)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ": " + ProcedureString + ": " + TextToLog + System.Environment.NewLine);
            File.AppendAllText("Plugins\\InstantAction\\" + "log.txt", sb.ToString());
            sb.Clear();
        }
    }
}
