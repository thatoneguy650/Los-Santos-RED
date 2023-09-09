using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class EMTFlee : ComplexTask
{
    private ITargetable Target;
    public EMTFlee(IComplexTaskable ped, ITargetable player) : base(player, ped, 5000)
    {
        Name = "EMTFlee";
        SubTaskName = "";
        Target = player;
    }
    public override void Start()
    {
        if (!Ped.Pedestrian.Exists())
        {
            return;
        }
        NativeFunction.Natives.SET_PED_SHOULD_PLAY_IMMEDIATE_SCENARIO_EXIT(Ped.Pedestrian);
        Vector3 CurrentPos = Ped.Pedestrian.Position;
        NativeFunction.CallByName<bool>("TASK_SMART_FLEE_COORD", Ped.Pedestrian, CurrentPos.X, CurrentPos.Y, CurrentPos.Z, 500f, -1, false, false);
        GameTimeLastRan = Game.GameTime;
    }
    public override void Update()
    {
        if (Ped.Pedestrian.Exists() && Ped.IsDriver)
        {
            NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.Code3);
        }
        GameTimeLastRan = Game.GameTime;
    }
    public override void Stop()
    {

    }
    public override void ReTask()
    {

    }
}

