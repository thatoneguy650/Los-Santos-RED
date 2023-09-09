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
        if (!Ped.Pedestrian.Exists())
        {
            return;
            //EntryPoint.WriteToConsole($"TASKER: Flee Start: {Ped.Pedestrian.Handle} OtherTarget {OtherTarget?.Handle}", 3);
        }
        if (Settings.SettingsManager.GangSettings.BlockEventsDuringFlee)
        {

            Ped.Pedestrian.BlockPermanentEvents = true;
        }
        NativeFunction.Natives.SET_PED_SHOULD_PLAY_IMMEDIATE_SCENARIO_EXIT(Ped.Pedestrian);
        Ped.Pedestrian.KeepTasks = true;
        NativeFunction.Natives.SET_CURRENT_PED_WEAPON(Ped.Pedestrian, (uint)2725352035, true);//set unarmed
        NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
        Vector3 CurrentPos = Ped.Pedestrian.Position;
        NativeFunction.CallByName<bool>("TASK_SMART_FLEE_COORD", Ped.Pedestrian, CurrentPos.X, CurrentPos.Y, CurrentPos.Z, 5000f, -1, true, false);
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

