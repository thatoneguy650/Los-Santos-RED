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
    WeaponInformation ToIssue;
    ITargetable Player;
    IComplexTaskable Target;
    public CommitCrime(IComplexTaskable ped, ITargetable player, WeaponInformation toIssue, IComplexTaskable target) : base(player, ped, 0)
    {
        Name = "CommitCrime";
        SubTaskName = "";
        ToIssue = toIssue;
        Player = player;
        Target = target;
    }
    public override void Start()
    {
        if (Ped != null && Ped.Pedestrian.Exists() && Target != null && Target.Pedestrian.Exists())
        {
            EntryPoint.WriteToConsole($"TASKER: Fight Start: {Ped.Pedestrian.Handle}", 3);
            Ped.Pedestrian.BlockPermanentEvents = true;
            Ped.Pedestrian.KeepTasks = true;
            if (ToIssue != null)
            {
                Ped.Pedestrian.Inventory.GiveNewWeapon(ToIssue.Hash, ToIssue.AmmoAmount, true);
            }
            if(Target.IsInVehicle && Target.Pedestrian.CurrentVehicle.Exists() && Target.IsDriver)
            {
                Vehicle TargetVehicle = Target.Pedestrian.CurrentVehicle;
                unsafe
                {
                    int lol = 0;
                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                    NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", 0, TargetVehicle, -1, -1, 15.0f, 9);
                    NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", 0, TargetVehicle, 15f, (int)VehicleDrivingFlags.Emergency, 25f);
                    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                }

                
            }
            else
            {
                NativeFunction.Natives.TASK_COMBAT_PED(Ped.Pedestrian, Target.Pedestrian, 0, 16);
            }
            //Ped.Pedestrian?.Tasks?.FightAgainst(Player.Character, -1);         
            GameTimeLastRan = Game.GameTime;
        }
    }
    public override void Update()
    {
        if (Ped.Pedestrian.Exists() && Player.IsWanted)
        {
            //Ped.Pedestrian.Tasks.Clear();
            NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
        }
    }
    public override void Stop()
    {

    }
}

