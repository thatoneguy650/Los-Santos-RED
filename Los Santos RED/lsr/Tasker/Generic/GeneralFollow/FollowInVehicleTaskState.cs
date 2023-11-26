using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class FollowInVehicleTaskState : TaskState
{
    private PedExt PedGeneral;
    private IEntityProvideable World;
    private SeatAssigner SeatAssigner;
    private bool ForceGuard;
    private uint GameTimeBetweenScenarios;
    private uint GameTimeLastStartedScenario;
    private uint GameTimeLastStartedFootPatrol;
    private uint GameTimeBetweenFootPatrols;

    private bool IsFastDriving = false;
    private bool PrevIsFastDriving = false;
    private float MoveSpeed = 1.0f;
    private float PrevMoveSpeed = 1.0f;

    private bool isGuarding = false;
    private bool isPatrolling = false;
    private ISettingsProvideable Settings;
    private ITargetable Player;
    private int GroupMemberNumber;
    public FollowInVehicleTaskState(PedExt pedGeneral, IEntityProvideable world, SeatAssigner seatAssigner, ISettingsProvideable settings, ITargetable player)
    {
        PedGeneral = pedGeneral;
        World = world;
        SeatAssigner = seatAssigner;
        Settings = settings;
        Player = player;
    }

    public bool IsValid => PedGeneral != null && PedGeneral.Pedestrian.Exists() && PedGeneral.IsInVehicle && Player.IsInVehicle;
    public string DebugName { get; } = "FollowInVehicleTaskState";
    public void Dispose()
    {

    }
    public void Start()
    {
        SetEscortTask();
    }
    public void Stop()
    {

    }
    public void Update()
    {
        //bool isPlayerDrivingFast = false;
        //if(Player.IsInVehicle && Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists() && Player.CurrentVehicle.Vehicle.Speed >= Settings.SettingsManager.GangSettings.EscortSpeedNormal)
        //{
        //    isPlayerDrivingFast = true;
        //}
        //IsFastDriving = Player.IsWanted || isPlayerDrivingFast || PedGeneral.IsWanted || PedGeneral.Pedestrian.IsInCombat;
        //if(PrevIsFastDriving != IsFastDriving)
        //{
        //    SetEscortTask();
        //    PrevIsFastDriving = IsFastDriving;
        //}
        UpdateChaseTask();
    }
    private void UpdateChaseTask()
    {
        if(PedGeneral == null || !PedGeneral.Pedestrian.Exists())
        {
            return;
        }

        NativeFunction.Natives.SET_DRIVER_ABILITY(PedGeneral.Pedestrian, 1.0f);
        //NativeFunction.Natives.SET_DRIVER_AGGRESSIVENESS(PedGeneral.Pedestrian, 1.0f);
       // NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(PedGeneral.Pedestrian, 70f);
       // NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(PedGeneral.Pedestrian, (int)eCustomDrivingStyles.Code3);
    }
    private void SetEscortTask()
    {
        if (PedGeneral != null && PedGeneral.IsInVehicle && PedGeneral.Pedestrian.Exists() && PedGeneral.Pedestrian.CurrentVehicle.Exists())
        {
            NativeFunction.Natives.SET_DRIVER_ABILITY(PedGeneral.Pedestrian, 1.0f);
            
            
          //  NativeFunction.Natives.TASK_VEHICLE_ESCORT(PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, Player.Character, -1, 100f, (int)eCustomDrivingStyles.Code3, -1.0f, 20, 20.0f);
            //NativeFunction.Natives.SET_DRIVER_AGGRESSIVENESS(PedGeneral.Pedestrian, 1.0f);



            NativeFunction.Natives.TASK_VEHICLE_FOLLOW(PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, Player.Character, 100f, (int)eCustomDrivingStyles.Code3, 20);



            //NativeFunction.Natives.TASK_VEHICLE_CHASE(PedGeneral.Pedestrian, Player.Character);

            // NativeFunction.Natives.SET_DRIVE_TASK_MAX_CRUISE_SPEED(PedGeneral.Pedestrian, 100.0f);//reset?

            // NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE(PedGeneral.Pedestrian, 8f);
            //NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(PedGeneral.Pedestrian, (int)eChaseBehaviorFlag.FullContact, false);
            //NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(PedGeneral.Pedestrian, (int)eChaseBehaviorFlag.MediumContact, false);
            //NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(PedGeneral.Pedestrian, (int)eChaseBehaviorFlag.LowContact, false);
            //NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(PedGeneral.Pedestrian, (int)eChaseBehaviorFlag.PIT, false);
            // NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(PedGeneral.Pedestrian, (int)eChaseBehaviorFlag.NoContact, true);
            //NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(PedGeneral.Pedestrian, (int)eCombatAttributes.BF_DisableCruiseInFrontDuringBlockDuringVehicleChase, true);
            //NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(PedGeneral.Pedestrian, (int)eCombatAttributes.BF_DisableSpinOutDuringVehicleChase, true);
            //NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(PedGeneral.Pedestrian, (int)eCombatAttributes.BF_DisableBlockFromPursueDuringVehicleChase, true);
            //NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(PedGeneral.Pedestrian, 0f);
            //NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(PedGeneral.Pedestrian, (int)eCustomDrivingStyles.Code3);
            //unsafe
            //{
            //    int lol = 0;
            //    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            //    NativeFunction.CallByName<bool>("TASK_VEHICLE_ESCORT", 0, PedGeneral.Pedestrian.CurrentVehicle, Player.Character, -1, 100f, (int)eCustomDrivingStyles.Code3, -1.0f * Math.Abs(Settings.SettingsManager.GangSettings.EscortOffsetValue) * GroupMemberNumber, 20, 20.0f);
            //    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
            //    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            //    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", PedGeneral.Pedestrian, lol);
            //    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            //}
        }
    }
}