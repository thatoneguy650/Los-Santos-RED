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


class RideInVehicleTaskState : TaskState
{
    private PedExt PedGeneral;
    private IEntityProvideable World;
    private SeatAssigner SeatAssigner;
    private bool ForceGuard;
    private uint GameTimeBetweenScenarios;
    private uint GameTimeLastStartedScenario;
    private uint GameTimeLastStartedFootPatrol;
    private uint GameTimeBetweenFootPatrols;
    private IWeaponIssuable WeaponIssuable;
    private bool isGuarding = false;
    private bool isPatrolling = false;
    private ISettingsProvideable Settings;
    private ITargetable Player;
    private GroupManager GroupManager;
    private bool isSetCombat = false;
    private bool isSetFollow = false;
    private float followSpeed;
    private GeneralFollow GeneralFollow;
    private bool isSetForce = false;

    public RideInVehicleTaskState(PedExt pedGeneral, ITargetable player, IEntityProvideable world, SeatAssigner seatAssigner, ISettingsProvideable settings, GroupManager groupManager, IWeaponIssuable weaponissueable, GeneralFollow generalFollow)
    {
        PedGeneral = pedGeneral;
        Player = player;
        World = world;
        SeatAssigner = seatAssigner;
        Settings = settings;
        GroupManager = groupManager;
        WeaponIssuable = weaponissueable;
        GeneralFollow = generalFollow;
    }

    public bool IsValid => PedGeneral != null && PedGeneral.Pedestrian.Exists() && PedGeneral.IsInVehicle && !Player.IsOnFoot;
    public string DebugName { get; } = "RideInVehicleTaskState";
    public void Dispose()
    {

    }
    public void Start()
    {
        if (!PedGeneral.Pedestrian.Exists())
        {
            return;
        }
       // NativeFunction.Natives.CLEAR_PED_TASKS(PedGeneral.Pedestrian);
        Update();
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
        UpdateTasking();
    }
    private void UpdateTasking()
    {
        if (GeneralFollow.SetCombat)
        {
            if (!isSetCombat || isSetForce != Player.GroupManager.BlockPermanentEvents)
            {
               // PedGeneral.Pedestrian.BlockPermanentEvents = false;// Player.GroupManager.ForceTasking;
                NativeFunction.Natives.TASK_COMBAT_HATED_TARGETS_AROUND_PED(PedGeneral.Pedestrian, 100f, 0);//TR
                //NativeFunction.Natives.CLEAR_PED_TASKS(PedGeneral.Pedestrian);

                EntryPoint.WriteToConsole("GENERAL FOLLOW RideInVehicleTaskState COMBAT SET");

                isSetCombat = true;
                isSetFollow = false;
                isSetForce = Player.GroupManager.BlockPermanentEvents;
            }
        }
        else
        {
            if (!isSetFollow || isSetForce != Player.GroupManager.BlockPermanentEvents)
            {
                //PedGeneral.Pedestrian.BlockPermanentEvents = Player.GroupManager.SetForceFollow;
               
                               // NativeFunction.Natives.CLEAR_PED_TASKS(PedGeneral.Pedestrian);
                 NativeFunction.Natives.TASK_PAUSE(PedGeneral.Pedestrian, -1);


                EntryPoint.WriteToConsole("GENERAL FOLLOW RideInVehicleTaskState PAUSE SET");


                isSetForce = Player.GroupManager.BlockPermanentEvents;
                isSetFollow = true;
                isSetCombat = false;
            }
        }
    }

}

