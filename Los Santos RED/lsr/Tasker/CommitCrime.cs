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
            NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
            EntryPoint.WriteToConsole($"TASKER: Fight Start: {Ped.Pedestrian.Handle}", 3);
            Ped.Pedestrian.BlockPermanentEvents = true;
            Ped.Pedestrian.KeepTasks = true;



            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_AlwaysFight, true);
            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_CanFightArmedPedsWhenNotArmed, true);


            //int hashArg;
            //NativeFunction.Natives.ADD_RELATIONSHIP_GROUP("Criminals", out hashArg);




            //Ped.Pedestrian.RelationshipGroup = new RelationshipGroup("Criminals");
            //Ped.Pedestrian.RelationshipGroup.SetRelationshipWith(RelationshipGroup.c, Relationship.Neutral);


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
                    NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", 0, TargetVehicle, 25f, (int)VehicleDrivingFlags.Emergency, 25f);
                    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                } 
            }
            else
            {
                //NativeFunction.Natives.TASK_COMBAT_PED(Ped.Pedestrian, Target.Pedestrian, 0, 16);
                //unsafe
                //{
                //    int lol = 0;
                //    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                //    NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Target.Pedestrian, -1, 5f, 2.0f, 1073741824, 1); //Original and works ok//7f
                //    NativeFunction.CallByName<bool>("TASK_COMBAT_PED", 0, Target.Pedestrian, 0, 16);
                //    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                //    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                //    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                //    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                //}


                unsafe
                {
                    int lol = 0;
                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                    NativeFunction.CallByName<bool>("TASK_COMBAT_PED", 0, Target.Pedestrian, 0, 16);
                    NativeFunction.CallByName<bool>("TASK_SMART_FLEE_COORD", 0, Ped.Pedestrian.Position.X, Ped.Pedestrian.Position.Y, Ped.Pedestrian.Position.Z, 500f, -1, false, false);
                    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                }
            }
            //Ped.Pedestrian?.Tasks?.FightAgainst(Player.Character, -1);         
            GameTimeLastRan = Game.GameTime;
        }
    }
    public override void Update()
    {
        //if (Ped.Pedestrian.Exists() && Player.IsWanted)
        //{
        //    NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
        //}
    }
    public override void Stop()
    {

    }
}

