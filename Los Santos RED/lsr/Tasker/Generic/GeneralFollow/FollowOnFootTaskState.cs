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
    private bool isSetCombat = false;
    private bool isSetFollow = false;
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
        if (!PedGeneral.Pedestrian.Exists())
        {
            return;

        }
        NativeFunction.Natives.CLEAR_PED_TASKS(PedGeneral.Pedestrian);
        //let the group stuff handle this!
        //GroupManager.Add(PedGeneral);
    }
    public void Stop()
    {

    }
    public void Update()
    {
        if (!PedGeneral.Pedestrian.Exists())
        {
            return;
        }
        //if(Player.Character.IsInCombat || PedGeneral.IsWanted)
        //{
        //    if (!isSetCombat)
        //    {
        //        NativeFunction.Natives.TASK_COMBAT_HATED_TARGETS_AROUND_PED(PedGeneral.Pedestrian, 100f, 0);//TR
        //        isSetCombat = true;
        //    }
        //}
        //else
        //{
        //    if(!isSetFollow)
        //    {

        //        NativeFunction.Natives.TASK_FOLLOW_TO_OFFSET_OF_ENTITY(PedGeneral.Pedestrian, Player.Character, 5f, 5f, 0f, 3.0f, -1, 10f, true);
        //        isSetFollow = true;
        //    }
        //}
    }
}

