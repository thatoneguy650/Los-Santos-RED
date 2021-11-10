using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CommitCrime : ComplexTask
{
    private int NewTargets = 0;
    private uint PreviousTargetHandle;
    WeaponInformation ToIssue;
    ITargetable Player;
    IComplexTaskable Target;
    IEntityProvideable World;
    public CommitCrime(IComplexTaskable ped, ITargetable player, WeaponInformation toIssue, IComplexTaskable target, IEntityProvideable world) : base(player, ped, 2000)
    {
        Name = "CommitCrime";
        SubTaskName = "";
        ToIssue = toIssue;
        Player = player;
        Target = target;
        World = world;
    }
    public override void Start()
    {
        if (Ped != null && Ped.Pedestrian.Exists() && Target != null && Target.Pedestrian.Exists())
        {
            PreviousTargetHandle = Target.Pedestrian.Handle;
            if (ToIssue != null)
            {
                Ped.Pedestrian.Inventory.GiveNewWeapon(ToIssue.Hash, ToIssue.AmmoAmount, true);
            }
            AttackTarget();
            EntryPoint.WriteToConsole($"TASKER: Fight Start: {Ped.Pedestrian.Handle}", 3);
        }
    }
    public override void Update()
    {
        if (NewTargets < 3 && !Ped.IsInVehicle)
        {
            if (Target == null || !Target.Pedestrian.Exists())
            {
                GetNewVictim();
                EntryPoint.WriteToConsole($"TASKER: Commit Crime: {Ped.Pedestrian.Handle} Should Get New Victim DOES NOT EXIST", 3);
            }
            else if (Target.Pedestrian.Exists() && (Target.Pedestrian.IsDead || Target.Pedestrian.DistanceTo2D(Ped.Pedestrian) >= 65f))
            {
                GetNewVictim();
                EntryPoint.WriteToConsole($"TASKER: Commit Crime: {Ped.Pedestrian.Handle} Should Get New Victim DEAD OR FAR AWAY", 3);
            }
        }
    }
    private void AttackTarget()
    {
        NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
        Ped.Pedestrian.BlockPermanentEvents = true;
        Ped.Pedestrian.KeepTasks = true;
        
        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_AlwaysFight, true);
        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_CanFightArmedPedsWhenNotArmed, true);
        NativeFunction.Natives.SET_PED_FLEE_ATTRIBUTES(Ped.Pedestrian, 0, false);

        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Target.Pedestrian, (int)eCombatAttributes.BF_AlwaysFight, true);
        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Target.Pedestrian, (int)eCombatAttributes.BF_CanFightArmedPedsWhenNotArmed, true);
        NativeFunction.Natives.SET_PED_FLEE_ATTRIBUTES(Target.Pedestrian, 0, false);

        if (Target.IsInVehicle && Target.Pedestrian.CurrentVehicle.Exists() && Target.IsDriver)
        {
            Vehicle TargetVehicle = Target.Pedestrian.CurrentVehicle;
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", 0, TargetVehicle, -1, -1, 15.0f, 9);
                NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", 0, TargetVehicle, 25f, (int)VehicleDrivingFlags.Emergency, 25f);
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
                NativeFunction.CallByName<bool>("TASK_COMBAT_PED", 0, Target.Pedestrian, 0, 16);
                //NativeFunction.CallByName<bool>("TASK_SMART_FLEE_COORD", 0, Ped.Pedestrian.Position.X, Ped.Pedestrian.Position.Y, Ped.Pedestrian.Position.Z, 500f, -1, false, false);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }      
        GameTimeLastRan = Game.GameTime;
    }
    private void GetNewVictim()
    {
        if (Ped.Pedestrian.Exists())
        {
            Target = World.CivilianList.Where(x => x.Pedestrian.Exists() && x.Handle != Ped.Pedestrian.Handle && x.Pedestrian.Handle != PreviousTargetHandle && x.DistanceToPlayer <= 85f && x.CanBeAmbientTasked && x.Pedestrian.Speed <= 2.0f && !x.IsGangMember && x.Pedestrian.IsAlive).OrderBy(x => x.Pedestrian.DistanceTo2D(Ped.Pedestrian)).FirstOrDefault();//150f
            if (Target != null && Target.Pedestrian.Exists())
            {
                PreviousTargetHandle = Target.Pedestrian.Handle;
                NewTargets++;
                AttackTarget();
                EntryPoint.WriteToConsole($"TASKER: Commit Crime: {Ped.Pedestrian.Handle} Got New Victim", 3);
            }
        }
    }
    public override void Stop()
    {

    }
}

