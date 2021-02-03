using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Kill : ComplexTask
{
    public Kill(IComplexTaskable cop, ITargetable player) : base(player, cop, 0)
    {
        Name = "Kill";
        SubTaskName = "";
    }
    public override void Start()
    {
        Game.Console.Print($"TASKER: Kill Start: {Ped.Pedestrian.Handle}");
        ClearTasks();
        NativeFunction.Natives.SET_PED_SHOOT_RATE(Ped.Pedestrian, 100);//30
        NativeFunction.Natives.SET_PED_ALERTNESS(Ped.Pedestrian, 3);//very altert
        NativeFunction.Natives.SET_PED_COMBAT_ABILITY(Ped.Pedestrian, 2);//professional
        NativeFunction.Natives.SET_PED_COMBAT_RANGE(Ped.Pedestrian, 2);//far
        NativeFunction.Natives.SET_PED_COMBAT_MOVEMENT(Ped.Pedestrian, 2);//offensinve
        NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.FullContact, true);
        Ped.Pedestrian.Tasks.FightAgainst(Game.LocalPlayer.Character, -1);
        Game.Console.Print(string.Format("TASKER Set to KILLLLLLL!!!!!!!!!: {0}", Ped.Pedestrian.Handle));
        
    }
    public override void Update()
    {

    }
    public void ClearTasks()//temp public
    {
        if (Ped.Pedestrian.Exists())
        {
            int seatIndex = 0;
            Vehicle CurrentVehicle = null;
            bool WasInVehicle = false;
            if (Ped.Pedestrian.IsInAnyVehicle(false))
            {
                WasInVehicle = true;
                CurrentVehicle = Ped.Pedestrian.CurrentVehicle;
                seatIndex = Ped.Pedestrian.SeatIndex;
            }
            Ped.Pedestrian.Tasks.Clear();
            Ped.Pedestrian.BlockPermanentEvents = false;
            Ped.Pedestrian.KeepTasks = false;
            Ped.Pedestrian.RelationshipGroup.SetRelationshipWith(RelationshipGroup.Player, Relationship.Neutral);
            if (WasInVehicle && !Ped.Pedestrian.IsInAnyVehicle(false) && CurrentVehicle != null)
            {
                Ped.Pedestrian.WarpIntoVehicle(CurrentVehicle, seatIndex);
            }            
            Game.Console.Print(string.Format("     ClearedTasks: {0}", Ped.Pedestrian.Handle));
        }
    }
    public override void Stop()
    {

    }
}

