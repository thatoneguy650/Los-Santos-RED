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
    private FightClubFight FightClubFight;
    private Vehicle TargetVehicle;
    private uint GameTimeLastCheckedTarget;
    private ISettingsProvideable Settings;

    public GeneralFight(PedExt pedGeneral, IComplexTaskable ped, ITargetable player, FightClubFight fightClubFight, ISettingsProvideable settings) :
        base(player, ped, 1000)//1500
    {
        PedGeneral = pedGeneral;
        Name = "GeneralFight";
        SubTaskName = "";
        FightClubFight = fightClubFight;
        Settings = settings;
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
    private void GetNearestTarget()
    {
        float closestDistance = 999f;
        float distanceToPlayer = 999f;
        float distanceToClosestAI = 998f;
        AIFightClubFighter closestAI = FightClubFight.AIFighters.Where(x => x.PedExt != null && !x.HasLost).OrderBy(x => x.PedExt.Position.DistanceTo2D(Ped.Pedestrian)).FirstOrDefault();
        Vehicle NewTargetVehicle = null;
        if(FightClubFight.IsPlayerFight && Player.Character.CurrentVehicle.Exists())
        {
            distanceToPlayer = Ped.DistanceToPlayer;
        }
        if(closestAI != null)
        {
            distanceToClosestAI = closestAI.PedExt.Position.DistanceTo2D(Ped.Pedestrian);
        }

        if(FightClubFight.IsPlayerFight && distanceToPlayer < distanceToClosestAI && Player.Character.CurrentVehicle.Exists())
        {
            NewTargetVehicle = Player.Character.CurrentVehicle;
        }
        else if (closestAI.PedExt.Pedestrian.Exists() && closestAI.PedExt.Pedestrian.CurrentVehicle.Exists())
        {
            NewTargetVehicle = closestAI.PedExt.Pedestrian.CurrentVehicle;
        }
        TargetVehicle = NewTargetVehicle;
    }
    private void AssignCombat()
    {
        if(FightClubFight.IsVehicleFight)
        {
            GetNearestTarget();
        }

        if (Ped.Pedestrian.CurrentVehicle.Exists() && TargetVehicle.Exists())
        {

            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_VEHICLE_MISSION", 0, Ped.Pedestrian.CurrentVehicle, TargetVehicle, 
                    Settings.SettingsManager.DebugSettings.FightClubVehicleMission, 50f, (int)eCustomDrivingStyles.Code3, 0f, 2f, true);//8f
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }

            //NativeFunction.Natives.TASK_VEHICLE_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle,
            //TargetVehicle, 
            //Settings.SettingsManager.DebugSettings.FightClubVehicleMission,
            //190f,
            //262144,//(int)eCustomDrivingStyles.RacingNew3, //(int)eCustomDrivingStyles.RacingNew3,//(int)eCustomDrivingStyles.CopRacingNew2Based,//(int)eCustomDrivingStyles.Code3,

            ////1.0f, 20f,
            //-1.0f, -1.0f,
            //true);
            EntryPoint.WriteToConsole("GENERAL FIGHT ASSIGNED VEHICLE MISSION!");
        }
        else
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

        if(FightClubFight.IsVehicleFight)
        {
            UpdateVehicle();
        }
    }

    private void UpdateVehicle()
    {
        if(GameTimeLastCheckedTarget == 0 || Game.GameTime - GameTimeLastCheckedTarget > 2000)
        {
            GetNearestTarget();
            AssignCombat();
            GameTimeLastCheckedTarget = Game.GameTime;
        }
    }

    private void CheckTasks()
    {
        if (FightClubFight != null && !FightClubFight.IsFightActive)
        {
            return;
        }
        Rage.TaskStatus taskStatus = PedGeneral.Pedestrian.Tasks.CurrentTaskStatus;
        if ((taskStatus == Rage.TaskStatus.NoTask || taskStatus == Rage.TaskStatus.Preparing) && Game.GameTime - GametimeLastRetasked >= 2000)
        {
            AssignCombat();
            GametimeLastRetasked = Game.GameTime;
            EntryPoint.WriteToConsole($"GENERAL FIGHT: PED {PedGeneral?.Handle} RETASKED");
        }
    }

}

