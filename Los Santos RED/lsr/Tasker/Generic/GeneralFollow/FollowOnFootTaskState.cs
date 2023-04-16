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


class FollowOnFootTaskState : TaskState
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
    private GroupManager GroupManager;
    public FollowOnFootTaskState(PedExt pedGeneral, ITargetable player, IEntityProvideable world, SeatAssigner seatAssigner, ISettingsProvideable settings, GroupManager groupManager)
    {
        PedGeneral = pedGeneral;
        Player = player;
        World = world;
        SeatAssigner = seatAssigner;
        Settings = settings;
        GroupManager = groupManager;
    }

    public bool IsValid => PedGeneral != null && PedGeneral.Pedestrian.Exists() && !PedGeneral.IsInVehicle && Player.IsOnFoot;
    public string DebugName { get; } = "FollowOnFootTaskState";
    public void Dispose()
    {

    }
    public void Start()
    {
        if (PedGeneral.Pedestrian.Exists())
        {
            NativeFunction.Natives.CLEAR_PED_TASKS(PedGeneral.Pedestrian);
        }
        //GroupManager.Add(PedGeneral);
    }
    public void Stop()
    {

    }
    public void Update()
    {

    }
}

