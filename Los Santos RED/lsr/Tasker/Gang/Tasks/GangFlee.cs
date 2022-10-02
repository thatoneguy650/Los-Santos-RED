using LosSantosRED.lsr.Interface;
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
    private ISettingsProvideable Settings;
    public GangFlee(IComplexTaskable ped, ITargetable player, ISettingsProvideable settings) : base(player, ped, 5000)
    {
        Name = "GangFlee";
        SubTaskName = "";
        Target = player;
        Settings = settings;
    }
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
            
            EntryPoint.WriteToConsole($"TASKER: Flee Start: {Ped.Pedestrian.Handle} OtherTarget {OtherTarget?.Handle}", 3);


            if (Settings.SettingsManager.GangSettings.ForceFlee)
            {

                Ped.Pedestrian.BlockPermanentEvents = true;
                Ped.Pedestrian.KeepTasks = true;
            }

            NativeFunction.Natives.SET_CURRENT_PED_WEAPON(Ped.Pedestrian, (uint)2725352035, true);//set unarmed
            NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);

            if (NativeFunction.Natives.IS_PED_USING_ANY_SCENARIO<bool>(Ped.Pedestrian))
            {
                Vector3 PedPos = Ped.Pedestrian.Position;
                NativeFunction.Natives.SET_PED_SHOULD_PLAY_FLEE_SCENARIO_EXIT(Ped.Pedestrian, PedPos.X, PedPos.Y, PedPos.Z);
                NativeFunction.Natives.SET_PED_PANIC_EXIT_SCENARIO(Ped.Pedestrian, PedPos.X, PedPos.Y, PedPos.Z);
            }


            Vector3 CurrentPos = Ped.Pedestrian.Position;
            NativeFunction.CallByName<bool>("TASK_SMART_FLEE_COORD", Ped.Pedestrian, CurrentPos.X, CurrentPos.Y, CurrentPos.Z, 5000f, -1, true, false);



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

