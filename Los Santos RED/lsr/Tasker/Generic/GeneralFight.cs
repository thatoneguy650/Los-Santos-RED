using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GeneralFight : ComplexTask
{
    private PedExt PedGeneral;
    private TaskState CurrentTaskState;
    private uint GametimeLastRetasked;

    public GeneralFight(PedExt pedGeneral, IComplexTaskable ped, ITargetable player) :
        base(player, ped, 1000)//1500
    {
        PedGeneral = pedGeneral;
        Name = "GeneralFight";
        SubTaskName = "";
    }
    public override void ReTask()
    {
        Start();
    }
    public override void Start()
    {
        //CurrentTaskState?.Stop();
        if (PedGeneral != null && PedGeneral.Pedestrian.Exists())
        {
            Ped.Pedestrian.BlockPermanentEvents = true;
            Ped.Pedestrian.KeepTasks = true;
            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_AlwaysFight, true);
            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_CanFightArmedPedsWhenNotArmed, true);
            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_Aggressive, true);
            //unsafe
            //{
            //    int lol = 0;
            //    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            //    NativeFunction.CallByName<bool>("TASK_COMBAT_PED", 0, Player.Character, Ped.DefaultCombatFlag, 16);
            //    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
            //    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            //    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
            //    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            //}
            //NativeFunction.Natives.TASK_COMBAT_HATED_TARGETS_AROUND_PED(Ped.Pedestrian, 500000, 0);
        }
        //GetNewTaskState();
        AssignCombat();
        //CurrentTaskState?.Start();
        EntryPoint.WriteToConsole("STARTED GENERAL FIGHT TASK");
    }

    private void AssignCombat()
    {

        unsafe
        {
            int lol = 0;
            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            NativeFunction.CallByName<bool>("TASK_COMBAT_HATED_TARGETS_AROUND_PED", 0, 500f, Ped.DefaultCombatFlag);
            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        }


        //NativeFunction.Natives.TASK_COMBAT_HATED_TARGETS_AROUND_PED(Ped.Pedestrian, 200f, Ped.DefaultCombatFlag);
    }

    public override void Stop()
    {
        //CurrentTaskState?.Stop();
    }
    public override void Update()
    {
        if (PedGeneral == null)
        {
            return;
        }
        //if (CurrentTaskState == null || !CurrentTaskState.IsValid)
        //{
        //    Start();
        //}
        //else
        //{
        //    SubTaskName = CurrentTaskState.DebugName;
        //    CurrentTaskState.Update();
        //}
        //AssignCombat();
        CheckTasks();
    }

    private void CheckTasks()
    {
        Rage.TaskStatus taskStatus = PedGeneral.Pedestrian.Tasks.CurrentTaskStatus;
        if ((taskStatus == Rage.TaskStatus.NoTask || taskStatus == Rage.TaskStatus.Preparing) && Game.GameTime - GametimeLastRetasked >= 2000)
        {
            AssignCombat();
            GametimeLastRetasked = Game.GameTime;
            EntryPoint.WriteToConsole($"GENERAL FIGHT: PED {PedGeneral?.Handle} RETASKED");
        }
    }

}

