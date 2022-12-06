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


class GetOutOfVehicleTaskState : TaskState
{
    private PedExt PedGeneral;
    private IEntityProvideable World;
    private SeatAssigner SeatAssigner;
    private bool ForceGuard;
    private uint GameTimeBetweenScenarios;
    private uint GameTimeLastStartedScenario;
    private uint GameTimeLastStartedFootPatrol;
    private uint GameTimeBetweenFootPatrols;

    private bool isGuarding = false;
    private bool isPatrolling = false;
    private ISettingsProvideable Settings;
    private ITargetable Player;
    public GetOutOfVehicleTaskState(PedExt pedGeneral, IEntityProvideable world, SeatAssigner seatAssigner, ISettingsProvideable settings, ITargetable player)
    {
        PedGeneral = pedGeneral;
        World = world;
        SeatAssigner = seatAssigner;
        Settings = settings;
        Player = player;
    }

    public bool IsValid => PedGeneral != null && PedGeneral.Pedestrian.Exists() && PedGeneral.IsInVehicle && Player.IsOnFoot;
    public string DebugName { get; } = "GetOutOfVehicleTaskState";
    public void Dispose()
    {

    }
    public void Start()
    {
        SetLeaveVehicleTask();
    }
    public void Stop()
    {

    }
    public void Update()
    {

    }
    private void SetLeaveVehicleTask()
    {
        if (PedGeneral != null && PedGeneral.IsInVehicle && PedGeneral.Pedestrian.Exists() && PedGeneral.Pedestrian.CurrentVehicle.Exists())
        {
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", 0, PedGeneral.Pedestrian.CurrentVehicle, 27, 1000);
                NativeFunction.CallByName<bool>("TASK_LEAVE_VEHICLE", 0, PedGeneral.Pedestrian.CurrentVehicle, 256);
                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Player.Character, -1, 7f, 500f, 1073741824, 1); //Original and works ok
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", PedGeneral.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
    }

}
