using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class FootChase
{
    private Cop Cop;
    private float RunSpeed;
    private IEntityProvideable World;
    private IComplexTaskable Ped;
    private ITargetable Player;
    private SubTask CurrentSubTask;
    private object MoveRate;
    private float LocalDistance;
    private float GoToDistance;

    private enum SubTask
    {
        Shoot,
        Aim,
        Goto,
        None,
        Look,
        WriteTicket,
    }
    private bool ShouldShoot => !Player.IsBusted && !Player.IsAttemptingToSurrender && Player.WantedLevel > 1 && !Player.IsHoldingHostage && !Player.IsCommitingSuicide;
    private bool ShouldAim => Player.WantedLevel > 1;
    public FootChase(IComplexTaskable ped, ITargetable player, IEntityProvideable world, Cop cop)
    {
        World = world;
        Ped = ped;
        Player = player;
        Cop = cop;
    }
    public void Setup()
    {
        CurrentSubTask = SubTask.None;
        MoveRate = (float)(RandomItems.MyRand.NextDouble() * (1.175 - 1.1) + 1.1);
        RunSpeed = 500f;
    }
    public void Update()
    {
        if (Player.WantedLevel == 1)
        {
            RunSpeed = 3.0f;// 1.4f;
        }
        else
        {
            RunSpeed = 500f;
            NativeFunction.Natives.SET_PED_MOVE_RATE_OVERRIDE<uint>(Ped.Pedestrian, MoveRate);
        }
        Ped.Pedestrian.BlockPermanentEvents = true;
        Ped.Pedestrian.KeepTasks = false;
        LocalDistance = Ped.Pedestrian.DistanceTo2D(Game.LocalPlayer.Character);
        GoToDistance = 5f;//4f;
        if (Player.IsHoldingHostage || Player.IsCommitingSuicide)
        {
            GoToDistance = 10f;
        }
        if (CurrentSubTask != SubTask.Shoot && ShouldShoot && LocalDistance < 10f && ShouldAim)//7f
        {
            Cop.WeaponInventory.ShouldAutoSetWeaponState = true;
            TaskShoot();
        }
        else if (CurrentSubTask != SubTask.Aim && !ShouldShoot && LocalDistance < 10f && ShouldAim)//7f
        {
            Cop.WeaponInventory.ShouldAutoSetWeaponState = true;
            TaskAim();
        }
        else if (!ShouldShoot && LocalDistance < 10f && !ShouldAim)
        {
            if (Player.ClosestCopToPlayer != null && Player.ClosestCopToPlayer.Handle == Ped.Handle)
            {
                if (CurrentSubTask != SubTask.WriteTicket)
                {
                    Cop.WeaponInventory.ShouldAutoSetWeaponState = false;
                    Cop.WeaponInventory.SetUnarmed();
                    TaskWriteTicket();
                }
            }
            else
            {
                if (CurrentSubTask != SubTask.Look)
                {
                    Cop.WeaponInventory.ShouldAutoSetWeaponState = true;
                    TaskLookAt();
                }
            }
        }     
        else if (CurrentSubTask != SubTask.Goto && LocalDistance >= 10f)
        {
            Cop.WeaponInventory.ShouldAutoSetWeaponState = true;
            TaskGoTo();
        }
    }
    public void Dispose()
    {

    }
    private void TaskShoot()
    {
        CurrentSubTask = SubTask.Shoot;
        if (LocalDistance > 5f)
        {
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY_WHILE_AIMING_AT_ENTITY", 0, Player.Character, Player.Character, 200f, true, GoToDistance, 200f, false, false, (uint)FiringPattern.DelayFireByOneSecond);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
        else
        {
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_SHOOT_AT_ENTITY", 0, Player.Character, -1, (uint)FiringPattern.DelayFireByOneSecond);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
    }
    private void TaskAim()
    {
        CurrentSubTask = SubTask.Aim;
        if (LocalDistance > 5f)
        {
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", 0, Player.Character, GoToDistance, 20f);
                NativeFunction.CallByName<bool>("TASK_AIM_GUN_AT_ENTITY", 0, Player.Character, -1, false);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
        else
        {
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_AIM_GUN_AT_ENTITY", 0, Player.Character, -1, false);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
    }
    private void TaskLookAt()
    {
        CurrentSubTask = SubTask.Look;
        if (LocalDistance > 5f)
        {
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Player.Character, -1, GoToDistance, RunSpeed, 2f, 0);
                NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, Player.Character, 1000);
                NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, Player.Character, -1, 0, 2);
               // NativeFunction.CallByName<bool>("TASK_START_SCENARIO_IN_PLACE", 0, "CODE_HUMAN_MEDIC_TIME_OF_DEATH", 0, true);
                //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "amb@medic@standing@timeofdeath@enter", "enter", 8.0f, -8.0f, -1, 0, 0, false, false, false);
                //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "amb@medic@standing@timeofdeath@idle_a", "idle_a", 8.0f, -8.0f, -1, 0, 0, false, false, false);
                //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "amb@medic@standing@timeofdeath@idle_a", "idle_b", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "amb@medic@standing@timeofdeath@idle_a", "idle_c", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
        else
        {
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, Player.Character, 1000);
                NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, Player.Character, -1, 0, 2);
               // NativeFunction.CallByName<bool>("TASK_START_SCENARIO_IN_PLACE", 0, "CODE_HUMAN_MEDIC_TIME_OF_DEATH", 0, true);
                //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "amb@medic@standing@timeofdeath@enter", "enter", 8.0f, -8.0f, -1, 0, 0, false, false, false);
                //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "amb@medic@standing@timeofdeath@idle_a", "idle_a", 8.0f, -8.0f, -1, 0, 0, false, false, false);
                //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "amb@medic@standing@timeofdeath@idle_a", "idle_b", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
    }
    private void TaskWriteTicket()
    {
        EntryPoint.WriteToConsole($"Foot Chase Write Ticket Started {Ped.Pedestrian.Handle} PrevSubTask = {CurrentSubTask}");
        CurrentSubTask = SubTask.WriteTicket;



        NativeFunction.Natives.SET_PED_SHOULD_PLAY_IMMEDIATE_SCENARIO_EXIT(Ped.Pedestrian);

        if (LocalDistance > 5f)
        {
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Player.Character, -1, GoToDistance, RunSpeed, 2f, 0);
                NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, Player.Character, 1000);
                NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, Player.Character, -1, 0, 2);
                NativeFunction.CallByName<bool>("TASK_START_SCENARIO_IN_PLACE", 0, "CODE_HUMAN_MEDIC_TIME_OF_DEATH", 0, true);
                //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "amb@medic@standing@timeofdeath@enter", "enter", 8.0f, -8.0f, -1, 0, 0, false, false, false);
                //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "amb@medic@standing@timeofdeath@idle_a", "idle_a", 8.0f, -8.0f, -1, 0, 0, false, false, false);
                //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "amb@medic@standing@timeofdeath@idle_a", "idle_b", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "amb@medic@standing@timeofdeath@idle_a", "idle_c", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
        else
        {
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, Player.Character, 1000);
                NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, Player.Character, -1, 0, 2);
                NativeFunction.CallByName<bool>("TASK_START_SCENARIO_IN_PLACE", 0, "CODE_HUMAN_MEDIC_TIME_OF_DEATH", 0, true);
                //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "amb@medic@standing@timeofdeath@enter", "enter", 8.0f, -8.0f, -1, 0, 0, false, false, false);
                //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "amb@medic@standing@timeofdeath@idle_a", "idle_a", 8.0f, -8.0f, -1, 0, 0, false, false, false);
                //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "amb@medic@standing@timeofdeath@idle_a", "idle_b", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
    }
    private void TaskGoTo()
    {
        CurrentSubTask = SubTask.Goto;
        unsafe
        {
            int lol = 0;
            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            //NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Player.Character, -1, 5f, RunSpeed, 1073741824, 1); //Original and works ok//7f
            NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Player.Character, -1, GoToDistance, RunSpeed, 2f, 0); //Original and works ok//7f
            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        }
    }


}

