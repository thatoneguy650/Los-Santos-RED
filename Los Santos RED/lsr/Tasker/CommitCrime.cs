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
    private WeaponInformation ToIssue;
    private ITargetable Player;
    private Ped Target;
    private bool IsPlayerTarget => Target.Exists() && Player.Character.Exists() && Target.Handle == Player.Character.Handle;
    private IEntityProvideable World;
    public CommitCrime(IComplexTaskable ped, ITargetable player, WeaponInformation toIssue, Ped target, IEntityProvideable world) : base(player, ped, 2000)
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
        if (Ped != null && Ped.Pedestrian.Exists() && Target.Exists())
        {
            PreviousTargetHandle = Target.Handle;
            if (ToIssue != null)
            {
                Ped.Pedestrian.Inventory.GiveNewWeapon(ToIssue.Hash, ToIssue.AmmoAmount, true);
            }
            AttackTarget();
            //EntryPoint.WriteToConsole($"TASKER: Fight Start: {Ped.Pedestrian.Handle}", 3);
        }
    }
    public override void Update()
    {
        if (NewTargets < 3 && !Ped.IsInVehicle)
        {
            if (Target == null || !Target.Exists())
            {
                GetNewVictim();
                //EntryPoint.WriteToConsole($"TASKER: Commit Crime: {Ped.Pedestrian.Handle} Should Get New Victim DOES NOT EXIST", 3);
            }
            else if (Target.Exists() && (Target.IsDead || Target.DistanceTo2D(Ped.Pedestrian) >= 65f))
            {
                GetNewVictim();
                //EntryPoint.WriteToConsole($"TASKER: Commit Crime: {Ped.Pedestrian.Handle} Should Get New Victim DEAD OR FAR AWAY", 3);
            }
        }
    }
    private void AttackTarget()
    {
        if (Ped.Pedestrian.Exists())
        {
            NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
            Ped.Pedestrian.BlockPermanentEvents = true;
            Ped.Pedestrian.KeepTasks = true;

            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_AlwaysFight, true);
            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_CanFightArmedPedsWhenNotArmed, true);
            NativeFunction.Natives.SET_PED_FLEE_ATTRIBUTES(Ped.Pedestrian, 0, false);
            if (Target.Exists())
            {
                if (!IsPlayerTarget)
                {
                    NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Target, (int)eCombatAttributes.BF_AlwaysFight, true);
                    NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Target, (int)eCombatAttributes.BF_CanFightArmedPedsWhenNotArmed, true);
                    NativeFunction.Natives.SET_PED_FLEE_ATTRIBUTES(Target, 0, false);
                }
                if (Target.IsInAnyVehicle(false) && Target.CurrentVehicle.Exists())
                {
                    Vehicle TargetVehicle = Target.CurrentVehicle;
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
                        NativeFunction.CallByName<bool>("TASK_COMBAT_PED", 0, Target, 0, 16);
                        NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                        NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                        NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                        NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                    }
                }
            }
            GameTimeLastRan = Game.GameTime;
        }
    }
    private void GetNewVictim()
    {
        if (Ped.Pedestrian.Exists())
        {
            float closestDistance = 999f;
            PedExt closestTarget = null;
            Target = null;
            foreach(PedExt possibletarget in World.CivilianList)
            {
                if(possibletarget.Pedestrian.Exists() && possibletarget.Pedestrian.Handle != Ped.Pedestrian.Handle && PreviousTargetHandle != possibletarget.Pedestrian.Handle && possibletarget.DistanceToPlayer <= 85 && possibletarget.CanBeAmbientTasked && possibletarget.Pedestrian.Speed <= 2.0f && !possibletarget.IsGangMember && possibletarget.Pedestrian.IsAlive)
                {
                    float distanceToPossibleTarget = possibletarget.Pedestrian.DistanceTo2D(Ped.Pedestrian);
                    if(distanceToPossibleTarget <= closestDistance)
                    {
                        closestDistance = distanceToPossibleTarget;
                        closestTarget = possibletarget;
                    }
                }
            }
            if(closestTarget != null && closestTarget.Pedestrian.Exists())
            {
                if(closestDistance <= Ped.DistanceToPlayer)
                {
                    Target = closestTarget.Pedestrian;
                }
                else
                {
                    Target = Player.Character;
                }
            }
            if(Target.Exists())
            {
                PreviousTargetHandle = Target.Handle;
                NewTargets++;
                AttackTarget();
            }
        }
    }
    public override void Stop()
    {

    }
}

