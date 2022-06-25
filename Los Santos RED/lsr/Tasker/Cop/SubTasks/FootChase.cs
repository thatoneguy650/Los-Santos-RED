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
    private float prevRunSpeed;

    private enum SubTask
    {
        Shoot,
        Aim,
        Goto,
        None,
        Look,
        WriteTicket,
        SimpleLook,
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


        if(!Cop.HasTaser)
        {
            MoveRate = 1.1f;
        }
        RunSpeed = 500f;
        Cop.WeaponInventory.ShouldAutoSetWeaponState = true;
        Cop.WeaponInventory.SetLessLethal();
    }
    public void Update()
    {
        SetRunSpeed();
        Ped.Pedestrian.BlockPermanentEvents = true;
        Ped.Pedestrian.KeepTasks = false;
        LocalDistance = Ped.Pedestrian.DistanceTo2D(Game.LocalPlayer.Character);

        GoToDistance = 4f;//4f;
        if (Player.IsHoldingHostage || Player.IsCommitingSuicide)
        {
            GoToDistance = 10f;
        }
        else if(Player.IsInVehicle)
        {
            GoToDistance = 3f;
        }
        if (CurrentSubTask != SubTask.Shoot && LocalDistance < 10f && ShouldShoot && ShouldAim)//7f
        {
            Cop.WeaponInventory.ShouldAutoSetWeaponState = true;
            TaskShoot();
        }
        else if (CurrentSubTask != SubTask.Aim && LocalDistance < 10f && !ShouldShoot && ShouldAim && Cop.HasTaser)//7f
        {
            Cop.WeaponInventory.ShouldAutoSetWeaponState = true;
            TaskAim();
        }
        else if (LocalDistance < 10f && !ShouldShoot && !ShouldAim)
        {
            if(!Cop.HasTaser)
            {
                if (CurrentSubTask != SubTask.SimpleLook)
                {
                    Cop.WeaponInventory.ShouldAutoSetWeaponState = true;
                    TaskLookAt();
                }
            }
            else if (Player.ClosestCopToPlayer != null && Player.ClosestCopToPlayer.Handle == Ped.Handle)
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
    private void SetRunSpeed()
    {
        if (Player.WantedLevel == 1)
        {
            if (LocalDistance >= 15f)
            {
                RunSpeed = 3.0f;// 1.4f;
            }
            else
            {
                RunSpeed = 1.4f;// 1.4f;
            }
        }
        else
        {
            RunSpeed = 500f;
            NativeFunction.Natives.SET_PED_MOVE_RATE_OVERRIDE<uint>(Ped.Pedestrian, MoveRate);
        }
        if (prevRunSpeed != RunSpeed)
        {
            CurrentSubTask = SubTask.None;
            prevRunSpeed = RunSpeed;
        }
    }
    private void TaskShoot()
    {
        CurrentSubTask = SubTask.Shoot;
        if (Cop.HasTaser)
        {
            if (LocalDistance > 5f)
            {
                unsafe
                {
                    int lol = 0;
                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                    NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY_WHILE_AIMING_AT_ENTITY", 0, Player.Character, Player.Character, 200f, true, GoToDistance, 200f, false, false, (uint)FiringPattern.DelayFireByOneSecond);
                    NativeFunction.CallByName<bool>("TASK_SHOOT_AT_ENTITY", 0, Player.Character, 2000, (uint)FiringPattern.DelayFireByOneSecond);
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
                    NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY_WHILE_AIMING_AT_ENTITY", 0, Player.Character, Player.Character, 200f, true, GoToDistance, 200f, false, false, (uint)FiringPattern.DelayFireByOneSecond);
                    NativeFunction.CallByName<bool>("TASK_SHOOT_AT_ENTITY", 0, Player.Character, 2000, (uint)FiringPattern.DelayFireByOneSecond);
                    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                }
            }
        }
        else
        {
            Cop.WeaponInventory.SetLessLethal();
            if (LocalDistance > 5f)
            {
                unsafe
                {
                    int lol = 0;
                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                    //NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Player.Character, -1, 5f, RunSpeed, 1073741824, 1); //Original and works ok//7f
                   // NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Player.Character, -1, GoToDistance, RunSpeed, 7f, 0); //Original and works ok//7f
                    
                    NativeFunction.CallByName<bool>("TASK_PUT_PED_DIRECTLY_INTO_MELEE", 0, Player.Character, 0.0f, -1.0f, 0.0f, 0);
                    NativeFunction.CallByName<bool>("TASK_COMBAT_PED", 0, Player.Character, 0, 16);
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
                    //NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Player.Character, -1, 5f, RunSpeed, 1073741824, 1); //Original and works ok//7f
                    //NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Player.Character, -1, GoToDistance, RunSpeed, 7f, 0); //Original and works ok//7f
                    NativeFunction.CallByName<bool>("TASK_PUT_PED_DIRECTLY_INTO_MELEE", 0, Player.Character, 0.0f, -1.0f, 0.0f, 0);
                    NativeFunction.CallByName<bool>("TASK_COMBAT_PED", 0, Player.Character, 0, 16);
                    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                }
            }
        }
    }
    private void TaskAim()
    {
        CurrentSubTask = SubTask.Aim;
        AnimationDictionary.RequestAnimationDictionay("random@arrests");
        if (LocalDistance > 5f)
        {
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", 0, Player.Character, GoToDistance, 20f);

                if(RandomItems.RandomPercent(50) && (Cop.WeaponInventory.IsSetLessLethal || (Cop.WeaponInventory.IsSetDeadly && !Cop.WeaponInventory.HasHeavyWeaponOnPerson) || (Cop.WeaponInventory.IsSetDefault && !Cop.WeaponInventory.HasHeavyWeaponOnPerson)))
                {
                    NativeFunction.CallByName<bool>("TASK_AIM_GUN_AT_ENTITY", 0, Player.Character, RandomItems.GetRandomNumberInt(20000, 45000), false);
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "random@arrests", "radio_enter", 2.0f, -2.0f, 1000, 0, 0, false, false, false);
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "random@arrests", "radio_chatter", 2.0f, -2.0f, 2000, 0, 0, false, false, false);
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "random@arrests", "radio_exit", 2.0f, -2.0f, 1000, 0, 0, false, false, false);
                }
                else
                {
                    NativeFunction.CallByName<bool>("TASK_AIM_GUN_AT_ENTITY", 0, Player.Character, -1, false);
                }

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

                if (RandomItems.RandomPercent(50) && (Cop.WeaponInventory.IsSetLessLethal || (Cop.WeaponInventory.IsSetDeadly && !Cop.WeaponInventory.HasHeavyWeaponOnPerson) || (Cop.WeaponInventory.IsSetDefault && !Cop.WeaponInventory.HasHeavyWeaponOnPerson)))
                {
                    NativeFunction.CallByName<bool>("TASK_AIM_GUN_AT_ENTITY", 0, Player.Character, RandomItems.GetRandomNumberInt(20000, 45000), false);
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "random@arrests", "radio_enter", 2.0f, -2.0f, 1000, 0, 0, false, false, false);
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "random@arrests", "radio_chatter", 2.0f, -2.0f, 2000, 0, 0, false, false, false);
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "random@arrests", "radio_exit", 2.0f, -2.0f, 1000, 0, 0, false, false, false);
                }
                else
                {
                    NativeFunction.CallByName<bool>("TASK_AIM_GUN_AT_ENTITY", 0, Player.Character, -1, false);
                }

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
        AnimationDictionary.RequestAnimationDictionay("random@arrests");
        NativeFunction.Natives.SET_PED_SHOULD_PLAY_IMMEDIATE_SCENARIO_EXIT(Ped.Pedestrian);
        if (LocalDistance > 5f)
        {
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Player.Character, -1, GoToDistance, RunSpeed, 2f, 0);
                NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, Player.Character, 1000);
               // NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, Player.Character, -1, 0, 2);
                if(RandomItems.RandomPercent(50))
                {
                    //NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, Player.Character, -1, 2048, 3);//, 0, 2);
                    //NativeFunction.CallByName<bool>("TASK_STAND_STILL", 0, RandomItems.GetRandomNumberInt(10000, 25000));
                    //NativeFunction.CallByName<bool>("TASK_START_SCENARIO_IN_PLACE", 0, "WORLD_HUMAN_COP_IDLES", 0, true);
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "random@arrests", "generic_radio_enter", 2.0f, -2.0f, 1000, 0, 0, false, false, false);
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "random@arrests", "generic_radio_chatter", 2.0f, -2.0f, 2000, 0, 0, false, false, false);
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "random@arrests", "generic_radio_exit", 2.0f, -2.0f, 1000, 0, 0, false, false, false);
                    NativeFunction.CallByName<bool>("TASK_START_SCENARIO_IN_PLACE", 0, "WORLD_HUMAN_COP_IDLES", 0, true);
                }
                else
                {
                    NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, Player.Character, -1, 0, 2);
                    NativeFunction.CallByName<bool>("TASK_START_SCENARIO_IN_PLACE", 0, "WORLD_HUMAN_COP_IDLES", 0, true);
                }
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
                //NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, Player.Character, -1, 0, 2);
                if (RandomItems.RandomPercent(50))
                {
                    //NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, Player.Character, -1, 2048, 3);// 0, 2);
                    //NativeFunction.CallByName<bool>("TASK_STAND_STILL", 0, RandomItems.GetRandomNumberInt(10000, 25000));
                    //NativeFunction.CallByName<bool>("TASK_START_SCENARIO_IN_PLACE", 0, "WORLD_HUMAN_COP_IDLES", 0, true);
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "random@arrests", "generic_radio_enter", 2.0f, -2.0f, 1000, 0, 0, false, false, false);
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "random@arrests", "generic_radio_chatter", 2.0f, -2.0f, 2000, 0, 0, false, false, false);
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "random@arrests", "generic_radio_exit", 2.0f, -2.0f, 1000, 0, 0, false, false, false);
                    NativeFunction.CallByName<bool>("TASK_START_SCENARIO_IN_PLACE", 0, "WORLD_HUMAN_COP_IDLES", 0, true);
                }
                else
                {
                    NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, Player.Character, -1, 0, 2);
                    NativeFunction.CallByName<bool>("TASK_START_SCENARIO_IN_PLACE", 0, "WORLD_HUMAN_COP_IDLES", 0, true);
                }

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
    private void TaskLookAtSimple()
    {
        CurrentSubTask = SubTask.SimpleLook;
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
                NativeFunction.CallByName<bool>("TASK_START_SCENARIO_IN_PLACE", 0, "WORLD_HUMAN_COP_IDLES", 0, true);
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
                NativeFunction.CallByName<bool>("TASK_START_SCENARIO_IN_PLACE", 0, "WORLD_HUMAN_COP_IDLES", 0, true);
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

