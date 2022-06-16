﻿using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GangFlee : ComplexTask
{
    private ITargetable Target;
    public GangFlee(IComplexTaskable ped, ITargetable player) : base(player, ped, 5000)
    {
        Name = "GangFlee";
        SubTaskName = "";
        Target = player;
    }
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
            
            EntryPoint.WriteToConsole($"TASKER: Flee Start: {Ped.Pedestrian.Handle} OtherTarget {OtherTarget?.Handle}", 3);
            //Ped.Pedestrian.BlockPermanentEvents = true;
            //Ped.Pedestrian.KeepTasks = true;

            NativeFunction.Natives.SET_CURRENT_PED_WEAPON(Ped.Pedestrian, (uint)2725352035, true);//set unarmed


            if (NativeFunction.Natives.IS_PED_USING_ANY_SCENARIO<bool>(Ped.Pedestrian))
            {
                Vector3 PedPos = Ped.Pedestrian.Position;
                NativeFunction.Natives.SET_PED_SHOULD_PLAY_FLEE_SCENARIO_EXIT(Ped.Pedestrian, PedPos.X, PedPos.Y, PedPos.Z);
                NativeFunction.Natives.SET_PED_PANIC_EXIT_SCENARIO(Ped.Pedestrian, PedPos.X, PedPos.Y, PedPos.Z);
            }


            Vector3 CurrentPos = Ped.Pedestrian.Position;
            NativeFunction.CallByName<bool>("TASK_SMART_FLEE_COORD", Ped.Pedestrian, CurrentPos.X, CurrentPos.Y, CurrentPos.Z, 500f, -1, false, false);



            ////Ped.Pedestrian.Tasks.Flee(Target.Character, 100f, -1);
            //if (OtherTarget != null && OtherTarget.Pedestrian.Exists())
            //{
            //    NativeFunction.Natives.TASK_SMART_FLEE_PED(Ped.Pedestrian, OtherTarget.Pedestrian, 1000f, -1, false, false);
            //}
            //else
            //{
            //    NativeFunction.Natives.TASK_SMART_FLEE_PED(Ped.Pedestrian, Target.Character, 1000f, -1, false, false);
            //}
            GameTimeLastRan = Game.GameTime;
        }
    }
    public override void Update()
    {

    }
    public override void Stop()
    {

    }
    public override void ReTask()
    {

    }
}

