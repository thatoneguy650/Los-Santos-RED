using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
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
        Player.DEBUGSETBUSTED();
       // Scanner.Abort();
    }
    private void DebugNumpad3()
    {
       
       // Scanner.AnnounceCrime(new Crime("KillingPolice", "Police Fatality", 3, true, 1, 1, false), new PoliceScannerCallIn(false,true,Game.LocalPlayer.Character.Position));
    }
    private void DebugNumpad4()
    {
        SpawnInteractiveChaser();
    }
    private void DebugNumpad5()
    {
        foreach (Cop cop in World.PoliceList)
        {
            Player.AddCrime(new Crime("PublicIntoxication", "Public Intoxication", 1, false, 31, 4, true, false, false), false, Game.LocalPlayer.Character.Position, null, null, false);
            cop.CurrentTask = new Investigate(cop, Player);
            cop.CurrentTask.Start();
        }
    }
    private void DebugNumpad6()
    {
        //foreach (Cop cop in World.PoliceList)
        //{
        //    Player.PlacePoliceLastSeenPlayer = Game.LocalPlayer.Character.Position;
        //    if(cop.CurrentTask == null || cop.CurrentTask.Name != "Locate")
        //    {
        //        cop.CurrentTask = new Locate(cop, Player);
        //        cop.CurrentTask.Start();
        //    }

        //}
        Player.Character.RelationshipGroup.SetRelationshipWith(RelationshipGroup.AmbientGangFamily, (Relationship)255);
        RelationshipGroup.AmbientGangFamily.SetRelationshipWith(RelationshipGroup.Player, (Relationship)255);
        //foreach (ButtonPrompt bp in Player.ButtonPrompts)
        //{
        //    Game.Console.Print($"{bp.Text}, {bp.Key}, {bp.IsPressedNow}, {bp.Text}");
        //}
        NativeFunction.CallByName<bool>("STOP_GAMEPLAY_HINT", true);
    }
    private void DebugNumpad7()
    {
        //foreach (Cop cop in World.PoliceList)
        //{
        //    cop.CurrentTask = new Chase(cop, Player);
        //    cop.CurrentTask.Start();
        //}
        Game.Console.Print("===================================");
        foreach (PedExt ped in World.CivilianList.OrderBy(x => x.DistanceToPlayer))
        {
            Relationship PlayerToPed = (Relationship)NativeFunction.Natives.GET_RELATIONSHIP_BETWEEN_PEDS<int>(Player.Character, ped.Pedestrian);
            Relationship PedToPlayer = (Relationship)NativeFunction.Natives.GET_RELATIONSHIP_BETWEEN_PEDS<int>(ped.Pedestrian, Player.Character);
            Game.Console.Print(ped.DebugString + $" PlayerToPed {PlayerToPed} PedToPlayer {PedToPlayer}");
        }
        Game.Console.Print("===================================");

    }
    public void DebugNumpad8()
    {
        Game.Console.Print("===================================");
        foreach (Cop cop in World.PoliceList.OrderBy(x=>x.DistanceToPlayer))
        {
            Game.Console.Print(cop.DebugString);
        }
        Game.Console.Print("===================================");
    }
    private void DebugNumpad9()
    {
        MakeSober();
        TerminateMod();
    }
    private void TerminateMod()
    {
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
    private void SpawnInteractiveChaser()
    {
        Ped newped = new Ped("s_m_y_cop_01", Game.LocalPlayer.Character.GetOffsetPositionFront(2f), Game.LocalPlayer.Character.Heading);
        Vehicle car = new Vehicle("police", Game.LocalPlayer.Character.GetOffsetPositionFront(-8f), Game.LocalPlayer.Character.Heading);
        if (newped.Exists() && car.Exists())
        {
            newped.WarpIntoVehicle(car, -1);
            GameFiber.StartNew(delegate
            {
                try
                {
                    while (newped.Exists() && newped.IsAlive && !Game.IsKeyDownRightNow(Keys.E))
                    {
                        Game.DisplayHelp("Press E to delete chaser ~n~Press R to Repair");
                        if (Game.IsKeyDownRightNow(Keys.R) && car.Exists())
                        {
                            car.Repair();
                            newped.Health = newped.MaxHealth;
                        }
                        GameFiber.Yield();
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
                    //Game.Console.Print("Error" + e.Message + " : " + e.StackTrace);
                }
            }, "DebugLoop2");

        }
    }
    private void MakeNonInvincible()
    {
        Game.LocalPlayer.Character.IsInvincible = false;
        Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
        Game.Console.Print("KeyDown: You are NOT invicible");
        Game.DisplaySubtitle("Invincibility Off");
    }
    private void MakeInvincible()
    {
        Game.LocalPlayer.Character.IsInvincible = true;
        Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
        Game.Console.Print("KeyDown: You are invicible");
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
        //Game.Console.Print("Player Made Drunk");
    }
    private void MakeSober()
    {
        NativeFunction.Natives.SET_PED_IS_DRUNK<bool>(Game.LocalPlayer.Character, false);
        NativeFunction.Natives.RESET_PED_MOVEMENT_CLIPSET<bool>(Game.LocalPlayer.Character);
        NativeFunction.Natives.SET_PED_CONFIG_FLAG<bool>(Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, false);
        NativeFunction.Natives.CLEAR_TIMECYCLE_MODIFIER<int>();
        NativeFunction.Natives.x80C8B1846639BB19(0);
        NativeFunction.Natives.STOP_GAMEPLAY_CAM_SHAKING<int>(true);
        //Game.Console.Print("Player Made Sober");
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

