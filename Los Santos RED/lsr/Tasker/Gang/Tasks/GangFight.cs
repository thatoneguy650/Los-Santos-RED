using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GangFight : ComplexTask
{
    WeaponInformation ToIssue;
    ITargetable Player;
    public bool ForceCombatPlayer = false;
    public GangFight(IComplexTaskable ped, ITargetable player, WeaponInformation toIssue) : base(player, ped, 5000)
    {
        Name = "GangFight";
        SubTaskName = "";
        ToIssue = toIssue;
        Player = player;
    }
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
            NativeFunction.Natives.SET_PED_SHOULD_PLAY_IMMEDIATE_SCENARIO_EXIT(Ped.Pedestrian);
            //EntryPoint.WriteToConsole($"TASKER: GangFight Start: {Ped.Pedestrian.Handle} OtherTarget {OtherTarget?.Handle}", 3);
            if (OtherTarget != null && OtherTarget.Pedestrian.Exists())
            {
                NativeFunction.Natives.TASK_COMBAT_PED(Ped.Pedestrian, OtherTarget.Pedestrian, Ped.DefaultCombatFlag, 16);
            }
            else if(ForceCombatPlayer)
            {
                NativeFunction.Natives.TASK_COMBAT_PED(Ped.Pedestrian, Player.Character, Ped.DefaultCombatFlag, 16);
            }
            else
            {
                NativeFunction.Natives.TASK_COMBAT_HATED_TARGETS_AROUND_PED(Ped.Pedestrian, 500f, 0);//75f//TR
            }
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

