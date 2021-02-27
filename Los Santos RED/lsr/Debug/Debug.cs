using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class Debug
{
    private int PlateIndex;
    private Vector3 StoredPosition;
    private PlateTypes PlateTypes;
    private Mod.World World;
    private Mod.Player Player;
    private Scanner Scanner;
    private Ped DebugPed;
    private int VehicleMissionFlag = 1;
    public Debug(PlateTypes plateTypes, Mod.World world, Mod.Player targetable, Scanner scanner)
    {
        PlateTypes = plateTypes;
        World = world;
        Player = targetable;
        Scanner = scanner;
    }
    public void Update()
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

        foreach(Cop cop in World.PoliceList.Where(x=> x.Pedestrian.Exists()))
        {
            DrawColoredArrowTaskStatus(cop);
            DrawColoredArrowAlertness(cop);
        }
    }
    private void DebugNumpad0()
    {
        MakeNonInvincible();
    }
    private void DebugNumpad1()
    {
        MakeInvincible();
    }
    private void DebugNumpad2()
    {
        Player.PoliceResponse.SetWantedLevel(0, "RESETTING DEBUG!", true);
    }
    private void DebugNumpad3()
    {
        Player.PoliceResponse.SetWantedLevel(2, "SETTING DEBUG!", true);
    }
    private void DebugNumpad4()
    {
        if (DebugPed.Exists())
        {
            DebugPed.Delete();
        }
        //Vector3 Pos = Game.LocalPlayer.Character.GetOffsetPositionFront(3f);
        ////Cop = new Ped("s_m_y_cop_01", Game.LocalPlayer.Character.GetOffsetPositionFront(3f), Game.LocalPlayer.Character.Heading);
        //Cop = NativeFunction.Natives.CREATE_PED<Ped>(6, Game.GetHashKey("s_m_y_cop_01"), Pos.X, Pos.Y, Pos.Z, Game.LocalPlayer.Character.Heading, false, false);
        //Cop.RelationshipGroup = RelationshipGroup.Cop;
    }
    private void DebugNumpad5()
    {
        VehicleMissionFlag++;
        EntryPoint.WriteToConsole($"VehicleMissionFlag {VehicleMissionFlag}",4);
        //DebugPed = new Ped(Game.LocalPlayer.Character.GetOffsetPositionFront(3f), Game.LocalPlayer.Character.Heading);
        //DebugPed.Inventory.GiveNewWeapon(new WeaponAsset("weapon_pistol"), 60, true);
        //DebugPed.Tasks.FightAgainst(Game.LocalPlayer.Character);


        //EntryPoint.WriteToConsole("HandleRespawn RUN");
        //Game.HandleRespawn();
    }
    private void DebugNumpad6()
    {
        if(VehicleMissionFlag > 0)
        {
            VehicleMissionFlag--;
        }
        EntryPoint.WriteToConsole($"VehicleMissionFlag {VehicleMissionFlag}", 4);
    }
    private void DebugNumpad7()
    {
        SpawnInteractiveChaser(1f) ;
    }
    public void DebugNumpad8()
    {
        //SpawnInteractiveChaser(5f);


        EntryPoint.WriteToConsole("===================================", 4);
        foreach (Cop cop in World.PoliceList.OrderBy(x => x.DistanceToPlayer))
        {
            //int rel1 = NativeFunction.Natives.GET_RELATIONSHIP_BETWEEN_PEDS<int>(Game.LocalPlayer.Character, cop.Pedestrian);
            //int rel2 = NativeFunction.Natives.GET_RELATIONSHIP_BETWEEN_PEDS<int>(cop.Pedestrian, Game.LocalPlayer.Character);

            if(cop.Pedestrian.CurrentVehicle.Exists())
            {
                int MissionType = NativeFunction.Natives.GET_ACTIVE_VEHICLE_MISSION_TYPE<int>(cop.Pedestrian.CurrentVehicle);
                EntryPoint.WriteToConsole(cop.DebugString + $"MissionType {MissionType}", 4);
            }

            
        }
        EntryPoint.WriteToConsole("===================================", 4);
    }
    private void DebugNumpad9()
    {
        MakeSober();
        TerminateMod();
    }
    private void TerminateMod()
    {
        if (DebugPed.Exists())
        {
            DebugPed.Delete();
        }
        EntryPoint.ModController.Dispose();
        Game.LocalPlayer.WantedLevel = 0;
        Game.TimeScale = 1f;
        NativeFunction.Natives.xB4EDDC19532BFB85();
        Game.DisplayNotification("Instant Action Deactivated");
    }
    private void DrawDebugArrowsOnPeds()
    {
        Vector3 Position = NativeFunction.Natives.GET_WORLD_POSITION_OF_ENTITY_BONE<Vector3>(Game.LocalPlayer.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, 31086));
        Rage.Debug.DrawArrowDebug(Position, Vector3.Zero, Rotator.Zero, 1f, Color.White);
        Vector3 Position2 = NativeFunction.Natives.GET_WORLD_POSITION_OF_ENTITY_BONE<Vector3>(Game.LocalPlayer.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, 57005));
        Rage.Debug.DrawArrowDebug(Position2, Vector3.Zero, Rotator.Zero, 1f, Color.Red);
    }
    private void DrawColoredArrowTaskStatus(PedExt PedToDraw)
    {
        Color Color = Color.White;
        TaskStatus taskStatus = PedToDraw.Pedestrian.Tasks.CurrentTaskStatus;
        if(taskStatus == TaskStatus.InProgress)
        {
            Color = Color.Green;
        }
        else if (taskStatus == TaskStatus.Interrupted)
        {
            Color = Color.Red;
        }
        else if (taskStatus == TaskStatus.None)
        {
            Color = Color.White;
        }
        else if (taskStatus == TaskStatus.NoTask)
        {
            Color = Color.Blue;
        }
        else if (taskStatus == TaskStatus.Preparing)
        {
            Color = Color.Purple;
        }
        else if (taskStatus == TaskStatus.Unknown)
        {
            Color = Color.Yellow;
        }

        Rage.Debug.DrawArrowDebug(PedToDraw.Pedestrian.Position, Vector3.Zero, Rotator.Zero, 1f, Color);
    }
    private void DrawColoredArrowAlertness(PedExt PedToDraw)
    {
        Color Color = Color.White;
        int Alertness = NativeFunction.Natives.GET_PED_ALERTNESS<int>(PedToDraw.Pedestrian);
        if (Alertness == 0)
        {
            Color = Color.Green;
        }
        else if (Alertness == 1)
        {
            Color = Color.Red;
        }
        else if (Alertness == 2)
        {
            Color = Color.White;
        }
        else if (Alertness == 3)
        {
            Color = Color.Blue;
        }
        else 
        {
            Color = Color.Yellow;
        }
        Rage.Debug.DrawArrowDebug(new Vector3(PedToDraw.Pedestrian.Position.X, PedToDraw.Pedestrian.Position.Y, PedToDraw.Pedestrian.Position.Z + 1f), Vector3.Zero, Rotator.Zero, 1f, Color);
    }
    private void SpawnInteractiveChaser(float Distance)
    {
        Ped newped = new Ped("a_f_m_business_02", Game.LocalPlayer.Character.GetOffsetPositionFront(2f), Game.LocalPlayer.Character.Heading);
        Vehicle car = new Vehicle("police", Game.LocalPlayer.Character.GetOffsetPositionFront(-8f), Game.LocalPlayer.Character.Heading);
        if (newped.Exists() && car.Exists())
        {

           car.IsCollisionProof = true;
            //  car.IsCollisionEnabled = false;

          //  NativeFunction.Natives.SET_ENTITY_COLLISION(car, false, false);
         
            newped.WarpIntoVehicle(car, -1);
            GameFiber.StartNew(delegate
            {
                try
                {
                    
                    PedExt Coolio = new PedExt(newped, false, false, false, "Test1", null);
                    Coolio.Update(Player,Player.Position);
                    Coolio.CurrentTask = new Chase(Coolio, Player, Distance, VehicleMissionFlag);
                    Coolio.CurrentTask.Start();

                        //NativeFunction.CallByName<bool>("SET_DRIVER_ABILITY", newped, 100f);
                        //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", newped, 8f);
                    car.AttachBlip();

                    uint GameTimeColliisonCheck = Game.GameTime;
                    while (newped.Exists() && newped.IsAlive && !Game.IsKeyDownRightNow(Keys.E))
                    {
                        Game.DisplayHelp("Press E to delete chaser");



                        if(Coolio.DistanceToPlayer <= 20f)
                        {
                            car.IsCollisionProof = false;
                        }
                        else
                        {
                            car.IsCollisionProof = true;
                        }


                        Game.DisplayNotification($"{Coolio.CurrentTask?.Name} AND {Coolio.CurrentTask?.SubTaskName}");

                        //if (Game.GameTime - GameTimeColliisonCheck >= 1000)
                        //{


                        //    VehicleExt ClosestCar = World.CivilianVehicleList.Where(x => x.Vehicle.Handle != car.Handle).OrderBy(x => car.DistanceTo2D(x.Vehicle)).FirstOrDefault();
                        //    if (ClosestCar != null && ClosestCar.Vehicle.Exists())
                        //    {
                        //        car.CollisionIgnoredEntity = ClosestCar.Vehicle;
                        //    }
                        //}
                       // Rage.World.GetClosestEntity(car.Position, 10f, GetEntitiesFlags.ConsiderGroundVehicles | GetEntitiesFlags.ExcludePoliceCars | GetEntitiesFlags.ExcludePlayerVehicle);

                        //NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(newped, 4, true);
                        //NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(newped, 8, true);
                        //NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(newped, 16, true);
                        //NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(newped, 32, true);
                        //NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(newped, 4194304, true);

                        //262144 - Take shortest path (Removes most pathing limits, the driver even goes on dirtroads)
                        //4194304 - Ignore roads (Uses local pathing, only works within 200~ meters around the player)

                        //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", newped, 4, true);
                        //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", newped, 8, true);
                        //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", newped, 16, true);
                        //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", newped, 32, true);
                        //  NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", newped, 512, true);
                        //  NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", newped, 262144, true);

                        //  NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", newped, 4194304, true);



                        Coolio.CurrentTask.Update();
                        //GameFiber.Sleep(250);
                        GameFiber.Sleep(25);
                        //GameFiber.Yield();
                    }
                    if (newped.Exists())
                    {
                        newped.Delete();
                    }
                    if (car.Exists())
                    {
                        car.Delete();
                    }
                }
                catch (Exception e)
                {
                    if (newped.Exists())
                    {
                        newped.Delete();
                    }
                    if (car.Exists())
                    {
                        car.Delete();
                    }
                    //EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace);
                }
            }, "DebugLoop2");

        }
    }
    private void MakeNonInvincible()
    {
        Game.LocalPlayer.Character.IsInvincible = false;
        Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
        EntryPoint.WriteToConsole("KeyDown: You are NOT invicible", 4);
        Game.DisplaySubtitle("Invincibility Off");
    }
    private void MakeInvincible()
    {
        Game.LocalPlayer.Character.IsInvincible = true;
        Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
        EntryPoint.WriteToConsole("KeyDown: You are invicible", 4);
        Game.DisplaySubtitle("Invincibility On");
    }
    private void MakeDrunk()
    {
        NativeFunction.Natives.SET_PED_IS_DRUNK<bool>( Game.LocalPlayer.Character, true);
        if (!NativeFunction.Natives.HAS_ANIM_SET_LOADED<bool>("move_m@drunk@verydrunk"))
        {
            NativeFunction.Natives.REQUEST_ANIM_SET<bool>("move_m@drunk@verydrunk");
        }
        NativeFunction.Natives.SET_PED_MOVEMENT_CLIPSET<bool>(Game.LocalPlayer.Character, "move_m@drunk@verydrunk", 0x3E800000);
        NativeFunction.Natives.SET_PED_CONFIG_FLAG<bool>(Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, true);
        NativeFunction.Natives.SET_TIMECYCLE_MODIFIER<int>("Drunk");
        NativeFunction.Natives.SET_TIMECYCLE_MODIFIER_STRENGTH<int>(1.1f);
        NativeFunction.Natives.x80C8B1846639BB19(1);
        NativeFunction.Natives.SHAKE_GAMEPLAY_CAM<int>("DRUNK_SHAKE", 5.0f);
        //EntryPoint.WriteToConsole("Player Made Drunk");
    }
    private void MakeSober()
    {
        NativeFunction.Natives.SET_PED_IS_DRUNK<bool>(Game.LocalPlayer.Character, false);
        NativeFunction.Natives.RESET_PED_MOVEMENT_CLIPSET<bool>(Game.LocalPlayer.Character);
        NativeFunction.Natives.SET_PED_CONFIG_FLAG<bool>(Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, false);
        NativeFunction.Natives.CLEAR_TIMECYCLE_MODIFIER<int>();
        NativeFunction.Natives.x80C8B1846639BB19(0);
        NativeFunction.Natives.STOP_GAMEPLAY_CAM_SHAKING<int>(true);
        //EntryPoint.WriteToConsole("Player Made Sober");
    }
    public void LoadNorthYankton()
    {
        NativeFunction.CallByName<bool>("REQUEST_IPL", "plg_01");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01c");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01c_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01d");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01d_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01e");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01e_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01f");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01f_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01g");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01h");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01h_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01i");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01i_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01j");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01j_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01k");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01k_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01z");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue01z_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "plg_02");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue02");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue02_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "plg_03");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue03");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue03_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue03b");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue03b_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue03_grv_dug");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue03_grv_dug_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue_grv_torch");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "plg_04");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue04");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue04_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue04b");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue04b_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue04_cover");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "des_protree_end");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "des_protree_start");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "des_protree_start_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "plg_05");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue05");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue05_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue05b");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue05b_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "plg_06");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue06");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue06_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue06b");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue06b_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue06_int");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue06_int_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue06_pannel");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue06_pannel_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue_m2_door");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue_m2_door_lod");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "plg_occl_00");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologue_occl");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "plg_rd");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologuerd");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologuerdb");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "prologuerd_lod");
    }
    public void UnLoadNorthYankton()
    {
        NativeFunction.CallByName<bool>("REMOVE_IPL", "plg_01");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01c");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01c_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01d");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01d_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01e");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01e_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01f");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01f_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01g");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01h");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01h_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01i");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01i_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01j");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01j_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01k");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01k_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01z");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue01z_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "plg_02");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue02");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue02_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "plg_03");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue03");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue03_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue03b");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue03b_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue03_grv_dug");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue03_grv_dug_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue_grv_torch");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "plg_04");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue04");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue04_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue04b");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue04b_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue04_cover");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "des_protree_end");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "des_protree_start");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "des_protree_start_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "plg_05");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue05");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue05_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue05b");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue05b_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "plg_06");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue06");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue06_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue06b");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue06b_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue06_int");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue06_int_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue06_pannel");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue06_pannel_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue_m2_door");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue_m2_door_lod");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "plg_occl_00");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologue_occl");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "plg_rd");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologuerd");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologuerdb");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "prologuerd_lod");
    }
    private void SetIndex()
    {
        if (PlateIndex < 0)
        {
            return;
        }
        if (Game.LocalPlayer.Character.IsInAnyVehicle(false))
        {
            Vehicle Car = Game.LocalPlayer.Character.CurrentVehicle;
            PlateType NewType = PlateTypes.GetPlateType(PlateIndex);
            if (NewType != null)
            {
                string NewPlateNumber = NewType.GenerateNewLicensePlateNumber();
                if (NewPlateNumber != "")
                {
                    Car.LicensePlate = NewPlateNumber;
                }
                NativeFunction.CallByName<int>("SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", Car, NewType.Index);
                Game.DisplaySubtitle($" PlateIndex: {PlateIndex}, Index: {NewType.Index}, State: {NewType.State}, Description: {NewType.Description}");
            }
            else
            {
                Game.DisplaySubtitle($" PlateIndex: {PlateIndex} None Found");
            }
        }
    }

}

