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

namespace Instant_Action_RAGE.Systems
{
    internal static class PoliceScanningSystem
    {
        private static uint GameTimeInterval;
        private static uint VehicleReplaceInterval;
        private static uint LOSInterval;
        private static int CopDogRelationShip;
        private static Random rnd;
        private static List<Vehicle> ReplacedVehicles = new List<Vehicle>();
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
        public static List<GTACop> CopPeds { get; private set; }
        public static int ScanningInterval { get; private set; }
        public static float ScanningRange { get; private set; }
        public static float InnocentScanningRange { get; private set; }
        public static int CopsKilled { get; set; } = 0;
        public static bool InnocentsNear { get; private set; }
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
        public static void Initialize()
        {
            ScanningInterval = 5000;
            ScanningRange = 200f;
            InnocentScanningRange = 10f;
            CopPeds = new List<GTACop>();
            MainLoop();
        }
        public static void MainLoop()
        {
            GameFiber.StartNew(delegate
            {
                while (IsRunning)
                {
                    CopPeds.RemoveAll(x => !x.CopPed.Exists() ||  x.CopPed.IsDead);

                    if (Game.GameTime > GameTimeInterval + ScanningInterval)
                    {

                        ScanForPolice();
                        //CheckLOS();
                        //CheckInnocentsNear();
                        
                        GameTimeInterval = Game.GameTime;
                        Game.LogTrivial(String.Format("Scanned for police {0}!",Game.GameTime));
                    }
                    if (Game.GameTime > VehicleReplaceInterval + 5000)
                    {
                        //CheckVehiclesToReplace();

                        VehicleReplaceInterval = Game.GameTime;
                    }
                    if(Game.GameTime > LOSInterval + 2000)
                    {
                        CheckLOS();
                        SetPrimaryPursuer();
                        LOSInterval = Game.GameTime;
                    }


                    if (!PlayerHurtPolice || !PlayerKilledPolice)
                        CheckDamage();

                    if (Game.IsKeyDown(Keys.NumPad2)) // Our menu on/off switch.
                    {

                        GTACop CopToTask = CopPeds.OrderBy(x => x.CopPed.Position.DistanceTo2D(Game.LocalPlayer.Character.Position)).FirstOrDefault();
                        if (CopToTask == null)
                            return;
                        TaskChasing(CopToTask);
                    }
                    if (Game.IsKeyDown(Keys.NumPad3)) // Our menu on/off switch.
                    {

                        CopPeds.ForEach(x => x.CopPed.Tasks.Clear());
                    }

                    if (Game.IsKeyDown(Keys.NumPad7)) // Our menu on/off switch.
                    {
                        Game.LocalPlayer.WantedLevel = 0;
                        InstantAction.CurrentPoliceState = InstantAction.PoliceState.Normal;
                    }
                    if (Game.IsKeyDown(Keys.NumPad8)) // Our menu on/off switch.
                    {
                        Game.LocalPlayer.WantedLevel--;
                    }
                    if (Game.IsKeyDown(Keys.NumPad9)) // Our menu on/off switch.
                    {
                        Game.LocalPlayer.WantedLevel++;
                    }
                    foreach (GTACop Cop in CopPeds)
                    {
                        if (Cop.TaskFiber != null && Cop.TaskFiber.Name == "Chase")
                            Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Green);
                        else if(Cop.TaskFiber != null && Cop.TaskFiber.Name == "Arrest")
                            Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Black);
                        else
                            Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Yellow);


                        if (!Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 500f))
                        {

                            Cop.CopPed.Delete();
                            WriteToLog("Deleted Cop", "Cop has been deleted as they are too far away");
                        }
                    }
                    Debug.DrawArrowDebug(new Vector3(PlacePlayerLastSeen.X, PlacePlayerLastSeen.Y, PlacePlayerLastSeen.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Yellow);
                    
                    GameFiber.Yield();
                }
            });
        }
        private static void ScanForPolice()
        {
            Ped[] Pedestrians = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 250f, GetEntitiesFlags.ConsiderHumanPeds | GetEntitiesFlags.ExcludePlayerPed).Where(x => x is Ped).ToArray(), (x => (Ped)x));
            //Ped[] Pedestrians = World.GetAllPeds();
            foreach (Ped Cop in Pedestrians.Where(s => s.Exists() && !s.IsDead && s.isPoliceArmy() && s.IsVisible))
            {
                if (!CopPeds.Any(x => x.CopPed == Cop))
                {
                    bool canSee = false;
                    if (Cop.IsInRangeOf(Game.LocalPlayer.Character.Position, 100f) && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Cop, Game.LocalPlayer.Character))
                        canSee = true;

                    GTACop myCop = new GTACop(Cop, "", canSee, canSee ? Game.GameTime : 0, canSee ? Game.LocalPlayer.Character.Position : new Vector3(0f, 0f, 0f));
                    // if (rnd.Next(0, 100) <= 85 && myCop.CopPed.IsInVehicle() && myCop.CopPed.CurrentVehicle.Model == Function.Call<int>(Hash.GET_HASH_KEY, "polscoutk9"))
                    //    MakeK9Handler(myCop);
                    Cop.IsPersistent = false;
                    CopPeds.Add(myCop);
                }
            }
            //CopPeds.Where(x => (x.CopPed.IsDead || !x.CopPed.Exists()) && x.K9 != null && !x.K9.IsDead).ToList().ForEach(x => TaskK9Flee(x));
            CopPeds.RemoveAll(x => !x.CopPed.Exists() || x.CopPed.IsDead);
        }
        private static void CheckDamage()
        {
            foreach (GTACop Cop in CopPeds)
            {
                if (NativeFunction.CallByName<bool>("HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY", Cop.CopPed, Game.LocalPlayer.Character, true))
                {
                    PlayerHurtPolice = true;
                    Cop.HurtByPlayer = true;
                }
                if(Game.LocalPlayer.Character.IsInAnyVehicle(false) && NativeFunction.CallByName<bool>("HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY", Cop.CopPed, Game.LocalPlayer.Character.CurrentVehicle, true))
                {
                    PlayerHurtPolice = true;
                    Cop.HurtByPlayer = true;
                }

                if (Cop.CopPed.IsDead && Cop.HurtByPlayer)
                {
                    // PlayerKilledPolice = true;
                    Ped killer = NativeFunction.Natives.GetPedSourceOfDeath<Entity>(Cop.CopPed);//Cop.CopPed.GetKiller();//GET_PED_SOURCE_OF_DEATH
                    if (NativeFunction.CallByName<bool>("IS_ENTITY_A_PED", killer))
                    {
                        if (Game.LocalPlayer.Character == (Ped)killer || Cop.CopPed == (Ped)killer)
                        {
                            PlayerKilledPolice = true;
                        }
                    }
                    else if (NativeFunction.CallByName<bool>("IS_ENTITY_A_VEHICLE", killer))
                    {
                        if (Game.LocalPlayer.Character.CurrentVehicle == killer)
                        {
                            PlayerKilledPolice = true;
                        }
                    }
                }
            }
        }
        private static void CheckLOS()
        {
            foreach (GTACop Cop in CopPeds)
            {
                if (Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 100f) && !Cop.CopPed.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Cop.CopPed, Game.LocalPlayer.Character))
                {
                    Cop.canSeePlayer = true;
                    Cop.GameTimeLastSeenPlayer = Game.GameTime;
                    Cop.PositionLastSeenPlayer = Game.LocalPlayer.Character.Position;
                }
                else
                {
                    Cop.canSeePlayer = false;
                }
            }
        }
        private static void SetPrimaryPursuer()
        {
            if (CopPeds.Count == 0)
                return;
            foreach (GTACop Cop in CopPeds.Where(x => !x.CopPed.IsInHelicopter))
            {
                Cop.isPursuitPrimary = false;
            }
            CopPeds.OrderBy(x => x.CopPed.Position.DistanceTo2D(Game.LocalPlayer.Character.Position)).FirstOrDefault().isPursuitPrimary = true; ;
        }

        //public static void TaskChasing(GTACop Cop)
        //{
        //    Cop.TaskFiber =
        //    GameFiber.StartNew(delegate
        //    {
        //        if (!Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 100f))
        //        {
        //            Cop.CopPed.Tasks.Clear();
        //            Cop.CopPed.BlockPermanentEvents = false;
        //            return;
        //        }

        //        Cop.CopPed.BlockPermanentEvents = true;
        //        NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", Cop.CopPed, Game.LocalPlayer.Character, -1, 5.0f, 500f, 1073741824, 1);

        //        while (Cop.CopPed.Exists() && !Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 10f) && !Cop.CopPed.IsDead)
        //            GameFiber.Yield();

        //        if (!Cop.CopPed.Exists() || Cop.CopPed.IsDead)
        //            return;

        //        NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", Cop.CopPed, Game.LocalPlayer.Character, 4f, 10f);

        //        while(Cop.CopPed.Exists() && !Cop.CopPed.IsDead)
        //        {
        //            GameFiber.Yield();
        //        }

        //        //while (Cop.CopPed.Exists() && Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 15f) && !Cop.CopPed.IsDead)
        //        //    GameFiber.Yield();

        //        //if (!Cop.CopPed.Exists() || Cop.CopPed.IsDead)
        //        //    return;

        //        Cop.TaskFiber = null;
        //        //TaskChasing(Cop);
        //    });
        //}
        public static void TaskChasing(GTACop Cop)
        {
            if (!Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 100f) || Cop.TaskFiber != null)
            {
               // WriteToLog("TaskChasing", "FUCK OFFFFFFF");
                Cop.CopPed.Tasks.Clear();
                Cop.CopPed.BlockPermanentEvents = false;
                Cop.TaskFiber = null;
                WriteToLog("Task Chasing", string.Format("Initial Return: {0}", Cop.CopPed.Handle));
                return;
            }

            Cop.TaskFiber =
            GameFiber.StartNew(delegate
            {
                uint TaskTime = Game.GameTime;
                string LocalTaskName = "GoTo";
                //Otherwise task the chase
                WriteToLog("Task Chasing", string.Format("Started Chase: {0}", Cop.CopPed.Handle));
                NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_USE_CLIMBOVERS", Cop.CopPed, true);
                NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_USE_LADDERS", Cop.CopPed, true);
                NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_DROP_FROM_HEIGHT", Cop.CopPed, true);


                Cop.CopPed.BlockPermanentEvents = true;
                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", Cop.CopPed, Game.LocalPlayer.Character, -1, 5.0f, 500f, 1073741824, 1);

                while (Cop.CopPed.Exists() && !Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 10f) && !Cop.CopPed.IsDead)
                    GameFiber.Yield();

                if (!Cop.CopPed.Exists() || Cop.CopPed.IsDead)
                    return;

                //Initial GOTO workerd

                while (Cop.CopPed.Exists() && !Cop.CopPed.IsDead && Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 80f))
                {
                    Cop.CopPed.BlockPermanentEvents = true;
                    //if (Game.GameTime - TaskTime < 1000)
                    //    return; //Dont change tasking more than every second

                    if (Game.LocalPlayer.Character.IsInAnyVehicle(false))
                    {
                        if (Game.GameTime - TaskTime >= 1000 && Cop.isPursuitPrimary && Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 25f) && LocalTaskName != "CarJack")
                        {
                            Cop.CopPed.CanRagdoll = false;
                            unsafe
                            {
                                int lol = 0;
                                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                                NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", 0, Game.LocalPlayer.Character.CurrentVehicle, -1, -1, 2f, 9);
                                NativeFunction.CallByName<bool>("TASK_SHOOT_AT_ENTITY", 0, Game.LocalPlayer.Character, 2000, (uint)FiringPattern.DelayFireByOneSecond);
                                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.CopPed, lol);
                                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                            }
                            TaskTime = Game.GameTime;
                            LocalTaskName = "CarJack";
                            WriteToLog("TaskChasing", "Primary Cop Tasked with CarJack");
                        }
                        else if (Game.GameTime - TaskTime >= 1000 && !Cop.isPursuitPrimary && Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 25f) && LocalTaskName != "Arrest")
                        {
                            unsafe
                            {
                                int lol = 0;
                                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                                NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", 0, Game.LocalPlayer.Character, 4f, 20f);
                                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.CopPed, lol);
                                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                            }
                            TaskTime = Game.GameTime;
                            LocalTaskName = "Arrest";
                            WriteToLog("TaskChasing", "Cop Tasked with Arresting");
                        }
                        if (LocalTaskName == "CarJack" && Cop.CopPed.IsInAnyVehicle(true))
                        {
                            Cop.CopPed.Tasks.ClearImmediately(); // Get Out
                            LocalTaskName = "";
                        }
                    }
                    else
                    {
                        if(Game.GameTime - TaskTime >= 1000 && (InstantAction.CurrentPoliceState == InstantAction.PoliceState.ArrestedWait || InstantAction.CurrentPoliceState == InstantAction.PoliceState.CautiousChase) && LocalTaskName != "Arrest" )
                        {
                            unsafe
                            {
                                int lol = 0;
                                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                                NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", 0, Game.LocalPlayer.Character, 4f, 20f);
                                NativeFunction.CallByName<bool>("TASK_AIM_GUN_AT_ENTITY", 0, Game.LocalPlayer.Character, -1, false);
                                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.CopPed, lol);
                                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                            }
                            //NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", Cop.CopPed, Game.LocalPlayer.Character, 4f, 20f);
                            //Cop.CopPed.KeepTasks = true;

                            TaskTime = Game.GameTime;
                            LocalTaskName = "Arrest";
                            WriteToLog("TaskChasing", "Cop Tasked with Arresting");
                        }
                        else if (Game.GameTime - TaskTime >= 1000 && Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 5f) && InstantAction.CurrentPoliceState == InstantAction.PoliceState.UnarmedChase && LocalTaskName != "GotoShooting") //was 10f
                        {
                            Cop.CopPed.CanRagdoll = true;
                            NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY_WHILE_AIMING_AT_ENTITY", Cop.CopPed, Game.LocalPlayer.Character, Game.LocalPlayer.Character, 200f, true, 1.0f, 200f, false, false, (uint)FiringPattern.DelayFireByOneSecond);
                            TaskTime = Game.GameTime;
                            LocalTaskName = "GotoShooting";
                            WriteToLog("TaskChasing", "Cop Tasked with Shooting");
                        }
                        else if (Game.GameTime - TaskTime >= 1000 && !Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 10f) && InstantAction.CurrentPoliceState == InstantAction.PoliceState.UnarmedChase && LocalTaskName != "Goto") //was 15f
                        {
                            Cop.CopPed.CanRagdoll = true;
                            //NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_OFFSET", Cop.CopPed, Game.LocalPlayer.Character, 20000, 0f, 2f, 0f, -1);
                            NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", Cop.CopPed, Game.LocalPlayer.Character, -1, 5.0f, 500f, 1073741824, 1); //Original and works ok
                            TaskTime = Game.GameTime;
                            LocalTaskName = "Goto";
                            WriteToLog("TaskChasing", "Cop Tasked with GoTo");
                        }
                    }
 
                    GameFiber.Yield();
                    if (InstantAction.CurrentPoliceState != InstantAction.PoliceState.ArrestedWait && InstantAction.CurrentPoliceState != InstantAction.PoliceState.UnarmedChase && InstantAction.CurrentPoliceState != InstantAction.PoliceState.CautiousChase)
                        break;
                }
                WriteToLog("Task Chasing", string.Format("Loop End: {0}", Cop.CopPed.Handle));
                Cop.TaskFiber = null;
                if(Cop.CopPed.Exists() && !Cop.CopPed.IsDead)
                    Cop.CopPed.CanRagdoll = true;

            },"Chase");
        }
        public static void TaskArresting(GTACop Cop)
        {
            if(Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 100f) && Cop.TaskFiber != null && Cop.TaskFiber.Name == "Arrest")
            {
                return;
            }
            if (!Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 100f) && Cop.TaskFiber != null)
            {
                Cop.CopPed.Tasks.Clear();
                Cop.CopPed.BlockPermanentEvents = false;
                Cop.TaskFiber.Abort();
                Cop.TaskFiber = null;
                WriteToLog("TaskArresting", "Cop Cleared");
            }
                
            Cop.TaskFiber =
            GameFiber.StartNew(delegate
            {
                uint TaskTime = Game.GameTime;
                string LocalTaskName = "GoTo";
                //Otherwise task the chase
                WriteToLog("TaskArresting", "Cop Tasked with Arresting");
                NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_USE_CLIMBOVERS", Cop.CopPed, true);
                NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_USE_LADDERS", Cop.CopPed, true);
                NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_DROP_FROM_HEIGHT", Cop.CopPed, true);



                Cop.CopPed.BlockPermanentEvents = true;
                //NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", Cop.CopPed, Game.LocalPlayer.Character, -1, 5.0f, 500f, 1073741824, 1);

                //while (Cop.CopPed.Exists() && !Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 10f) && !Cop.CopPed.IsDead)
                //    GameFiber.Yield();

                //if (!Cop.CopPed.Exists() || Cop.CopPed.IsDead)
                //    return;

                //Initial GOTO workerd

                while (Cop.CopPed.Exists() && !Cop.CopPed.IsDead)
                {
                    Cop.CopPed.BlockPermanentEvents = true;
                    if (Game.GameTime - TaskTime >= 1000)
                    {
                        if(Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 5f) && LocalTaskName != "AimAt")
                        {
                            Cop.CopPed.Tasks.AimWeaponAt(Game.LocalPlayer.Character, -1);
                            LocalTaskName = "AimAt";
                        }
                        else if (!Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 5f))
                        {
                            Cop.CopPed.Tasks.GoToWhileAiming(Game.LocalPlayer.Character, 4f,20f);
                            LocalTaskName = "GoToAiming";
                        }
                        Cop.CopPed.KeepTasks = true;


                        //unsafe
                        //{
                        //    int lol = 0;
                        //    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                        //    NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", 0, Game.LocalPlayer.Character, 4f, 20f);
                        //    NativeFunction.CallByName<bool>("TASK_AIM_GUN_AT_ENTITY", 0, Game.LocalPlayer.Character, -1, false);
                        //    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                        //    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                        //    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.CopPed, lol);
                        //    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                        //}
                        //NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", Cop.CopPed, Game.LocalPlayer.Character, 4f, 20f);
                        //Cop.CopPed.KeepTasks = true;

                        TaskTime = Game.GameTime;
                        //LocalTaskName = "Arrest";
                        //WriteToLog("TaskArresting", "Cop Subtasked with Arresting");
                    }
                    GameFiber.Yield();
                }
            WriteToLog("TaskArresting", "Cop Arresting Loop End");
            Cop.TaskFiber = null;
            },"Arrest");
        }
        public static void UntaskAll()
        {
            foreach (GTACop Cop in CopPeds)
            {
                if (Cop.TaskFiber != null && Cop.TaskFiber.IsAlive)
                {
                    Cop.CopPed.Tasks.Clear();
                    Cop.CopPed.BlockPermanentEvents = false;
                    Cop.TaskFiber.Abort();
                    Cop.TaskFiber = null;
                    WriteToLog("UntaskAll", string.Format("Untasked: {0}",Cop.CopPed.Handle));
                }
            }
            
        }
        public static void Untask(GTACop Cop)
        {
            if (Cop.TaskFiber != null && Cop.TaskFiber.IsAlive)
            {
                Cop.CopPed.Tasks.Clear();
                Cop.CopPed.BlockPermanentEvents = false;
                Cop.TaskFiber.Abort();
                Cop.TaskFiber = null;
                Cop.CopPed.IsPersistent = false;
                WriteToLog("UntaskAll", string.Format("Untasked: {0}", Cop.CopPed.Handle));
            } 
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
