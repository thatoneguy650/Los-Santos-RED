//using ExtensionsMethods;
//using LosSantosRED.lsr.Helper;
//using LosSantosRED.lsr.Interface;
//using LSR.Vehicles;
//using Rage;
//using Rage.Native;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


//public class EMTIdle : ComplexTask
//{
//    private bool IsReturningToStation = false;
//    private bool NeedsUpdates;
//    private Task CurrentTask = Task.Nothing;
//    private uint GameTimeClearedIdle;
//    private int SeatTryingToEnter;
//    private VehicleExt VehicleTryingToEnter;
//    private IEntityProvideable World;
//    private Vehicle VehicleTaskedToEnter;
//    private int SeatTaskedToEnter;
//    private IPlacesOfInterest PlacesOfInterest;
//    private Vector3 taskedPosition;
//    private EMT EMT;
//    private SeatAssigner SeatAssigner;
//    private uint GameTimeLastStartedScenario;
//    private uint GameTimeBetweenScenarios = 60000;

//    private enum Task
//    {
//        GetInCar,
//        Wander,
//        Nothing,
//        OtherTarget,
//    }
//    private Task CurrentTaskDynamic
//    {
//        get
//        {
//            if (EMT.IsLocationSpawned)
//            {
//                return Task.Wander;
//            }
//            else if (!Ped.Pedestrian.IsInAnyVehicle(false))
//            {
//                if (Ped.DistanceToPlayer <= 75f && VehicleTryingToEnter != null && VehicleTryingToEnter.Vehicle.Exists() && VehicleTryingToEnter.Vehicle.IsDriveable && VehicleTryingToEnter.Vehicle.FreeSeatsCount > 0 && VehicleTryingToEnter.Vehicle.Speed < 1.0f) //if (Ped.DistanceToPlayer <= 75f && Ped.Pedestrian.LastVehicle.Exists() && Ped.Pedestrian.LastVehicle.IsDriveable && Ped.Pedestrian.LastVehicle.FreeSeatsCount > 0)
//                {
//                    return Task.GetInCar;
//                }
//                else if (CurrentTask == Task.GetInCar)
//                {
//                    return Task.GetInCar;
//                }
//                else
//                {
//                    return Task.Wander;
//                }
//            }
//            else
//            {
//                return Task.Wander;
//            }
//        }
//    }
//    public EMTIdle(IComplexTaskable ped, ITargetable player, IEntityProvideable world, IPlacesOfInterest placesOfInterest, EMT emt) : base(player, ped, 1500)//1500
//    {
//        Name = "EMTIdle";
//        SubTaskName = "";
//        World = world;
//        PlacesOfInterest = placesOfInterest;
//        EMT = emt;
//        SeatAssigner = new SeatAssigner(Ped, World, new List<VehicleExt>());
//    }
//    public override void Start()
//    {
//        if (Ped.Pedestrian.Exists())
//        {
//            //EntryPoint.WriteToConsole($"TASKER: Idle Start: {Ped.Pedestrian.Handle}", 5);
//            // NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 10f);
//            ClearTasks(true);
//            GetClosesetAmbulanceVehicle();
//            Update();
//        }
//    }
//    public override void Update()
//    {
//        if (Ped.Pedestrian.Exists() && ShouldUpdate)
//        {
//            if (CurrentTask != CurrentTaskDynamic)
//            {
//                CurrentTask = CurrentTaskDynamic;
//                //EntryPoint.WriteToConsole($"      Idle SubTask Changed: {Ped.Pedestrian.Handle} to {CurrentTask} {CurrentDynamic}");
//                ExecuteCurrentSubTask(true);
//            }
//            else if (NeedsUpdates)
//            {
//                ExecuteCurrentSubTask(false);
//            }
//            //Ped.Pedestrian.RelationshipGroup.SetRelationshipWith(RelationshipGroup.Player, Relationship.Neutral);
//            SetSiren();
//        }
//    }
//    public override void ReTask()
//    {

//    }
//    public override void Stop()
//    {

//    }
//    private void ExecuteCurrentSubTask(bool IsFirstRun)
//    {
//        if (CurrentTask == Task.Wander)
//        {
//            RunInterval = 1500;
//            SubTaskName = "Wander";
//            Wander(IsFirstRun);
//        }
//        else if (CurrentTask == Task.GetInCar)
//        {
//            RunInterval = 500;
//            SubTaskName = "GetInCar";
//            GetInCar(IsFirstRun);
//        }
//        else if (CurrentTask == Task.Nothing)
//        {
//            RunInterval = 1500;
//            SubTaskName = "Nothing";
//            Nothing(IsFirstRun);
//        }
//        GameTimeLastRan = Game.GameTime;
//    }
//    private void Wander(bool IsFirstRun)
//    {
//        if (Ped.Pedestrian.Exists())
//        {
//            if (IsFirstRun)
//            {
//                //EntryPoint.WriteToConsole($"COP EVENT: Wander Idle Start: {Ped.Pedestrian.Handle}", 3);
//                NeedsUpdates = true;
//                ClearTasks(true);
//                if (Ped.Pedestrian.CurrentVehicle.Exists())
//                {
//                    foreach (Ped ped in Ped.Pedestrian.CurrentVehicle.Passengers)
//                    {
//                        PedExt pedExt = World.Pedestrians.GetPedExt(ped.Handle);
//                        if (pedExt != null && pedExt.IsArrested)
//                        {
//                            IsReturningToStation = true;
//                            break;
//                        }
//                        if (ped.Handle == Player.Character.Handle)
//                        {
//                            IsReturningToStation = true;
//                            break;
//                        }
//                    }
//                }
//                WanderTask();
//            }
//            if (IsReturningToStation && Ped.Pedestrian.DistanceTo2D(taskedPosition) < 30f && Ped.Pedestrian.CurrentVehicle.Exists() && Ped.Pedestrian.CurrentVehicle.Speed <= 1.0f)//arrived, wait then drive away
//            {
//                IsReturningToStation = false;
//                WanderTask();
//               // EntryPoint.WriteToConsole($"COP EVENT: Wander Idle Arrived at Station: {Ped.Pedestrian.Handle}", 3);
//            }

//        }
//    }
//    private void WanderTask()
//    {
//        if (Ped.Pedestrian.Exists())
//        {
//            Ped.Pedestrian.BlockPermanentEvents = true;
//            Ped.Pedestrian.KeepTasks = true;
//            if (Ped.Pedestrian.IsInAnyVehicle(false))
//            {
//                if ((Ped.IsDriver || Ped.Pedestrian.SeatIndex == -1) && Ped.Pedestrian.CurrentVehicle.Exists())
//                {
//                    //if(SeatAssigner.HasPedsWaitingToEnter(World.Vehicles.GetVehicleExt(Ped.Pedestrian.CurrentVehicle),Ped.Pedestrian.SeatIndex))
//                    //{

//                    //}
//                    //else 
                    
//                    if (IsReturningToStation)
//                    {
//                        BasicLocation closestHospital = PlacesOfInterest.PossibleLocations.Hospitals.OrderBy(x => Ped.Pedestrian.DistanceTo2D(x.EntrancePosition)).FirstOrDefault();
//                        if (closestHospital != null)
//                        {
//                            taskedPosition = NativeHelper.GetStreetPosition(closestHospital.EntrancePosition);
//                            NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, taskedPosition.X, taskedPosition.Y, taskedPosition.Z, 12f, (int)eCustomDrivingStyles.RegularDriving, 20f);//NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, taskedPosition.X, taskedPosition.Y, taskedPosition.Z, 12f, (int)(VehicleDrivingFlags.FollowTraffic | VehicleDrivingFlags.YieldToCrossingPedestrians | VehicleDrivingFlags.RespectIntersections | (VehicleDrivingFlags)8), 20f);
//                        }
//                        else
//                        {
//                            unsafe
//                            {
//                                int lol = 0;
//                                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
//                                NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(4000, 8000));
//                                NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", 0, Ped.Pedestrian.CurrentVehicle, 10f, (int)eCustomDrivingStyles.RegularDriving, 10f);//NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", 0, Ped.Pedestrian.CurrentVehicle, 10f, (int)(VehicleDrivingFlags.FollowTraffic | VehicleDrivingFlags.YieldToCrossingPedestrians | VehicleDrivingFlags.RespectIntersections | (VehicleDrivingFlags)8), 10f);
//                                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
//                                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
//                                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
//                                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
//                            }
//                        }
//                    }
//                    else if (Ped.IsInHelicopter)
//                    {
//                        NativeFunction.CallByName<bool>("TASK_HELI_MISSION", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, 0, 0f, 0f, 300f, 9, 50f, 150f, -1f, -1, 30, -1.0f, 0);
//                    }
//                    else
//                    {
//                        unsafe
//                        {
//                            int lol = 0;
//                            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
//                            NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(4000, 8000));
//                            NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", 0, Ped.Pedestrian.CurrentVehicle, 10f, (int)eCustomDrivingStyles.RegularDriving, 10f);//NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", 0, Ped.Pedestrian.CurrentVehicle, 10f, (int)(VehicleDrivingFlags.FollowTraffic | VehicleDrivingFlags.YieldToCrossingPedestrians | VehicleDrivingFlags.RespectIntersections | (VehicleDrivingFlags)8), 10f);
//                            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
//                            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
//                            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
//                            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
//                        }
//                    }

//                }
//            }
//            else
//            {

//                Vector3 pedPos = Ped.Pedestrian.Position;
//                if (EMT.IsLocationSpawned || (Game.GameTime - GameTimeLastStartedScenario >= GameTimeBetweenScenarios && NativeFunction.Natives.DOES_SCENARIO_EXIST_IN_AREA<bool>(pedPos.X, pedPos.Y, pedPos.Z, 10f, true)))
//                {
//                    //NativeFunction.Natives.TASK_USE_NEAREST_SCENARIO_TO_COORD(Ped.Pedestrian, pedPos.X, pedPos.Y, pedPos.Z, 15f, 15000);
//                    List<string> PossibleScenarios = new List<string>() { "WORLD_HUMAN_INSPECT_STAND", "WORLD_HUMAN_AA_COFFEE", "WORLD_HUMAN_AA_SMOKE", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_STAND_MOBILE_UPRIGHT", "WORLD_HUMAN_SMOKING" };
//                    string ScenarioChosen = PossibleScenarios.PickRandom();
//                    NativeFunction.CallByName<bool>("TASK_START_SCENARIO_IN_PLACE", Ped.Pedestrian, ScenarioChosen, 0, true);
//                    GameTimeBetweenScenarios = RandomItems.GetRandomNumber(30000, 90000);
//                    GameTimeLastStartedScenario = Game.GameTime;
//                    //EntryPoint.WriteToConsole($"PED {Ped.Pedestrian.Handle} Started Scenario GameTimeBetweenScenarios {GameTimeBetweenScenarios} ScenarioChosen {ScenarioChosen}", 5);
//                }
//                else
//                {
//                    NativeFunction.Natives.TASK_WANDER_IN_AREA(Ped.Pedestrian, Ped.Pedestrian.Position.X, Ped.Pedestrian.Position.Y, Ped.Pedestrian.Position.Z, 100f, 0f, 0f);
//                    //NativeFunction.Natives.TASK_WANDER_STANDARD(Ped.Pedestrian, 0, 0);
//                   // EntryPoint.WriteToConsole($"PED {Ped.Pedestrian.Handle} Started Regular wander on foot", 5);
//                }


//                // //Ped.Pedestrian.Tasks.Wander();
//                // //NativeFunction.Natives.TASK_WANDER_STANDARD(Ped.Pedestrian, 0, 0);

//                // //Vector3 pedPos = Ped.Pedestrian.Position;
//                // //if (Game.GameTime - GameTimeLastStartedScenario >= 60000 && NativeFunction.Natives.DOES_SCENARIO_EXIST_IN_AREA<bool>(pedPos.X, pedPos.Y, pedPos.Z, 10f, true))
//                // //{
//                // //    NativeFunction.Natives.TASK_USE_NEAREST_SCENARIO_TO_COORD(Ped.Pedestrian, pedPos.X, pedPos.Y, pedPos.Z, 15f, 15000);
//                // //    GameTimeLastStartedScenario = Game.GameTime;
//                // //    EntryPoint.WriteToConsole($"PED {Ped.Pedestrian.Handle} Started Scenarion", 5);
//                // //}
//                // //else
//                //// {
//                //     NativeFunction.Natives.TASK_WANDER_STANDARD(Ped.Pedestrian, 0, 0);
//                //     EntryPoint.WriteToConsole($"PED {Ped.Pedestrian.Handle} Started Regular wander on foot", 5);
//                // //}

//            }
//        }
//    }
//    private void GetInCar(bool IsFirstRun)
//    {
//        if (Ped.Pedestrian.Exists())
//        {
//            if (IsFirstRun)
//            {
//                //EntryPoint.WriteToConsole($"Idle {Ped.Pedestrian.Handle}: Get in Car Start", 3);
//                NeedsUpdates = true;
//            }
//            if (VehicleTaskedToEnter == null || !VehicleTaskedToEnter.Exists())
//            {
//                GetClosesetAmbulanceVehicle();
//                //EntryPoint.WriteToConsole($"Idle {Ped.Pedestrian.Handle}: Get in Car, Got New Car, was Blank", 3);
//                GetInCarTask();
//            }
//            else if (VehicleTryingToEnter != null && VehicleTaskedToEnter.Exists() && !VehicleTaskedToEnter.IsSeatFree(SeatTaskedToEnter) && VehicleTaskedToEnter.GetPedOnSeat(SeatTaskedToEnter).Exists() && VehicleTaskedToEnter.GetPedOnSeat(SeatTaskedToEnter).Handle != Ped.Pedestrian.Handle)// && (VehicleTryingToEnter.Vehicle.Handle != VehicleTaskedToEnter.Handle || SeatTaskedToEnter != SeatTryingToEnter) && Ped.Pedestrian.Exists() && !Ped.Pedestrian.IsInAnyVehicle(true))
//            {
//                GetClosesetAmbulanceVehicle();
//                //EntryPoint.WriteToConsole($"Idle {Ped.Pedestrian.Handle}: Get in Car Got New Car, was occupied?", 3);
//                GetInCarTask();
//            }
//            else if (VehicleTryingToEnter != null && VehicleTaskedToEnter.Exists() && VehicleTaskedToEnter.Speed > 1.0f)// && (VehicleTryingToEnter.Vehicle.Handle != VehicleTaskedToEnter.Handle || SeatTaskedToEnter != SeatTryingToEnter) && Ped.Pedestrian.Exists() && !Ped.Pedestrian.IsInAnyVehicle(true))
//            {
//                GetClosesetAmbulanceVehicle();
//                //EntryPoint.WriteToConsole($"Idle {Ped.Pedestrian.Handle}: Get in Car Got New Car, was driving away?", 3);
//                GetInCarTask();
//            }
//        }
//    }
//    private void GetInCarTask()
//    {
//        if (Ped.Pedestrian.Exists() && VehicleTryingToEnter != null && VehicleTryingToEnter.Vehicle.Exists())
//        {
//            //EntryPoint.WriteToConsole($"Idle {Ped.Pedestrian.Handle}: Get in Car TASK START", 3);
//            Ped.Pedestrian.BlockPermanentEvents = true;
//            Ped.Pedestrian.KeepTasks = true;
//            VehicleTaskedToEnter = VehicleTryingToEnter.Vehicle;
//            SeatTaskedToEnter = SeatTryingToEnter;
//            unsafe
//            {
//                int lol = 0;
//                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
//                NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", 0, VehicleTryingToEnter.Vehicle, -1, SeatTryingToEnter, 1f, 9);
//                NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(8000, 16000));
//                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
//                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
//                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
//                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
//            }
//        }
//        else if (Ped.Pedestrian.Exists())
//        {
//            NativeFunction.Natives.TASK_WANDER_STANDARD(Ped.Pedestrian, 0, 0);
//        }
//    }
//    private void GetClosesetAmbulanceVehicle()
//    {
//        SeatAssigner.AssignFrontSeat(true);
//        VehicleTryingToEnter = SeatAssigner.VehicleAssigned;
//        SeatTryingToEnter = SeatAssigner.SeatAssigned;
//    }
//    private void ClearTasks(bool resetAlertness)//temp public
//    {
//        if (Ped.Pedestrian.Exists())
//        {
//            int seatIndex = 0;
//            Vehicle CurrentVehicle = null;
//            bool WasInVehicle = false;
//            if (Ped.Pedestrian.IsInAnyVehicle(false))
//            {
//                WasInVehicle = true;
//                CurrentVehicle = Ped.Pedestrian.CurrentVehicle;
//                seatIndex = Ped.Pedestrian.SeatIndex;
//            }
//            NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
//            Ped.Pedestrian.BlockPermanentEvents = false;
//            Ped.Pedestrian.KeepTasks = false;
//            NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
//            if (resetAlertness)
//            {
//                NativeFunction.Natives.SET_PED_ALERTNESS(Ped.Pedestrian, 0);
//            }
//            if (WasInVehicle && !Ped.Pedestrian.IsInAnyVehicle(false) && CurrentVehicle != null)
//            {
//                Ped.Pedestrian.WarpIntoVehicle(CurrentVehicle, seatIndex);
//            }
//        }
//    }
//    private void Nothing(bool IsFirstRun)
//    {
//        if (Ped.Pedestrian.Exists())
//        {
//            //EntryPoint.WriteToConsole($"COP EVENT: Nothing Idle Start: {Ped.Pedestrian.Handle}", 3);
//            if (IsFirstRun)
//            {
//                ClearTasks(false);
//                GameTimeClearedIdle = Game.GameTime;
//            }
//            else if (Game.GameTime - GameTimeClearedIdle >= 10000)
//            {
//                NativeFunction.Natives.SET_PED_ALERTNESS(Ped.Pedestrian, 0);
//            }
//        }
//    }
//    private void SetSiren()
//    {
//        if (Ped.Pedestrian.Exists() && Ped.Pedestrian.CurrentVehicle.Exists() && Ped.Pedestrian.CurrentVehicle.HasSiren && Ped.Pedestrian.CurrentVehicle.IsSirenOn)
//        {
//            Ped.Pedestrian.CurrentVehicle.IsSirenOn = false;
//            Ped.Pedestrian.CurrentVehicle.IsSirenSilent = false;
//        }
//    }

//}



