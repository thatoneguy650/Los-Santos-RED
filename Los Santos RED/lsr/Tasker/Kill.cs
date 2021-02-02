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
        Game.Console.Print($"TASKER: Kill Start: {Cop.Pedestrian.Handle}");
        ClearTasks();
        NativeFunction.Natives.SET_PED_SHOOT_RATE(Cop.Pedestrian, 100);//30
        NativeFunction.Natives.SET_PED_ALERTNESS(Cop.Pedestrian, 3);//very altert
        NativeFunction.Natives.SET_PED_COMBAT_ABILITY(Cop.Pedestrian, 2);//professional
        NativeFunction.Natives.SET_PED_COMBAT_RANGE(Cop.Pedestrian, 2);//far
        NativeFunction.Natives.SET_PED_COMBAT_MOVEMENT(Cop.Pedestrian, 2);//offensinve
        Cop.Pedestrian.Tasks.FightAgainst(Game.LocalPlayer.Character, -1);
        Game.Console.Print(string.Format("TASKER Set to KILLLLLLL!!!!!!!!!: {0}", Cop.Pedestrian.Handle));
    }
    public override void Update()
    {

    }
    public void ClearTasks()//temp public
    {
        if (Cop.Pedestrian.Exists())
        {
            int seatIndex = 0;
            Vehicle CurrentVehicle = null;
            bool WasInVehicle = false;
            if (Cop.Pedestrian.IsInAnyVehicle(false))
            {
                WasInVehicle = true;
                CurrentVehicle = Cop.Pedestrian.CurrentVehicle;
                seatIndex = Cop.Pedestrian.SeatIndex;
            }
            Cop.Pedestrian.Tasks.Clear();
            Cop.Pedestrian.BlockPermanentEvents = false;
            Cop.Pedestrian.KeepTasks = false;
            Cop.Pedestrian.RelationshipGroup.SetRelationshipWith(RelationshipGroup.Player, Relationship.Neutral);
            if (WasInVehicle && !Cop.Pedestrian.IsInAnyVehicle(false) && CurrentVehicle != null)
            {
                Cop.Pedestrian.WarpIntoVehicle(CurrentVehicle, seatIndex);
            }            
            Game.Console.Print(string.Format("     ClearedTasks: {0}", Cop.Pedestrian.Handle));
        }
    }
    public override void Stop()
    {

    }
}

