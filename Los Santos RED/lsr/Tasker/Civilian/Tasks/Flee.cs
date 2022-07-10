using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Flee : ComplexTask
{
    private ITargetable Target;
    public Flee(IComplexTaskable ped, ITargetable player) : base(player, ped, 5000)
    {
        Name = "Flee";
        SubTaskName = "";
        Target = player;
    }
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
            if(NativeFunction.Natives.IS_PED_USING_ANY_SCENARIO<bool>(Ped.Pedestrian))
            {
                Vector3 PedPos = Ped.Pedestrian.Position;
                NativeFunction.Natives.SET_PED_SHOULD_PLAY_FLEE_SCENARIO_EXIT(Ped.Pedestrian, PedPos.X, PedPos.Y, PedPos.Z);
                NativeFunction.Natives.SET_PED_PANIC_EXIT_SCENARIO(Ped.Pedestrian, PedPos.X, PedPos.Y, PedPos.Z);
            }
            Vector3 CurrentPos = Ped.Pedestrian.Position;
            NativeFunction.CallByName<bool>("TASK_SMART_FLEE_COORD", Ped.Pedestrian, CurrentPos.X, CurrentPos.Y, CurrentPos.Z, 1500f, -1, false, false);         
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

