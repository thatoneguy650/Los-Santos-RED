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
    public FollowInVehicleTaskState(PedExt pedGeneral, IEntityProvideable world, SeatAssigner seatAssigner, ISettingsProvideable settings, ITargetable player, int index)
    {
        PedGeneral = pedGeneral;
        World = world;
        SeatAssigner = seatAssigner;
        Settings = settings;
        Player = player;
        GroupMemberNumber = index;
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
        bool isPlayerDrivingFast = false;
        if(Player.IsInVehicle && Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists() && Player.CurrentVehicle.Vehicle.Speed >= Settings.SettingsManager.GangSettings.EscortSpeedNormal)
        {
            isPlayerDrivingFast = true;
        }
        IsFastDriving = Player.IsWanted || isPlayerDrivingFast || PedGeneral.IsWanted || PedGeneral.Pedestrian.IsInCombat;
        if(PrevIsFastDriving != IsFastDriving)
        {
            SetEscortTask();
            PrevIsFastDriving = IsFastDriving;
        }
    }
    private void SetEscortTask()
    {
        if (PedGeneral != null && PedGeneral.IsInVehicle && PedGeneral.Pedestrian.Exists() && PedGeneral.Pedestrian.CurrentVehicle.Exists())
        {
            int drivingStyle = (int)eCustomDrivingStyles.Code3;
            float drivingSpeed = Settings.SettingsManager.GangSettings.EscortSpeedFast;
            if (IsFastDriving)
            {
                drivingStyle = (int)eCustomDrivingStyles.Code3;
                drivingSpeed = Settings.SettingsManager.GangSettings.EscortSpeedFast;
            }
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_VEHICLE_ESCORT", 0, PedGeneral.Pedestrian.CurrentVehicle, Player.Character, -1, drivingSpeed, drivingStyle, -1.0f * Math.Abs(Settings.SettingsManager.GangSettings.EscortOffsetValue) * GroupMemberNumber, 20, 20.0f);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", PedGeneral.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
    }
}