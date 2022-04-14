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
            EntryPoint.WriteToConsole($"TASKER: GangFight Start: {Ped.Pedestrian.Handle} OtherTarget {OtherTarget?.Handle}", 3);
            if (OtherTarget != null && OtherTarget.Pedestrian.Exists())
            {
                NativeFunction.Natives.TASK_COMBAT_PED(Ped.Pedestrian, OtherTarget.Pedestrian, 0, 16);
            }
            else
            {
                NativeFunction.Natives.TASK_COMBAT_HATED_TARGETS_AROUND_PED(Ped.Pedestrian, 75f, 0);//TR
            }
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

