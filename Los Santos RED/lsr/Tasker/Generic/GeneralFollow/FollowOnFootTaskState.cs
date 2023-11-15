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
    private IWeaponIssuable WeaponIssuable;
    private bool isGuarding = false;
    private bool isPatrolling = false;
    private ISettingsProvideable Settings;
    private ITargetable Player;
    private GroupManager GroupManager;
    private bool isSetCombat = false;
    private bool isSetFollow = false;
    //private bool IsStandardFollow = true;
    //private bool IsCombat = false;
    private float followSpeed;
    //private float RunSpeed;
    private GeneralFollow GeneralFollow;
    private bool isSetForce = false;

    public FollowOnFootTaskState(PedExt pedGeneral, ITargetable player, IEntityProvideable world, SeatAssigner seatAssigner, ISettingsProvideable settings, GroupManager groupManager, IWeaponIssuable weaponissueable, GeneralFollow generalFollow)
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
        //NativeFunction.Natives.CLEAR_PED_TASKS(PedGeneral.Pedestrian);
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
            if (!isSetCombat || isSetForce != Player.GroupManager.SetForceTasking)
            {
                NativeFunction.Natives.TASK_COMBAT_HATED_TARGETS_AROUND_PED(PedGeneral.Pedestrian, 100f, 0);//TR
                isSetCombat = true;
                isSetForce = Player.GroupManager.SetForceTasking;
                isSetFollow = false;
            }
        }
        else
        {
            if (!isSetFollow || followSpeed != GeneralFollow.RunSpeed || isSetForce != Player.GroupManager.SetForceTasking)
            {
                //if (PedGeneral.DistanceToPlayer <= 10f)
                //{
                //    NativeFunction.Natives.CLEAR_PED_TASKS(PedGeneral.Pedestrian);
                //    PedGeneral.Pedestrian.BlockPermanentEvents = Player.GroupManager.ForceTasking;
                //}
                //else
                //{
                    unsafe
                    {
                        float offsetX = RandomItems.GetRandomNumber(-1.0f * Player.GroupManager.GroupFollowDistance, Player.GroupManager.GroupFollowDistance);
                        float offsetY = RandomItems.GetRandomNumber(-1.0f * Player.GroupManager.GroupFollowDistance, Player.GroupManager.GroupFollowDistance);
                        followSpeed = GeneralFollow.RunSpeed;
                        int lol = 0;
                        NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                        NativeFunction.CallByName<bool>("TASK_FOLLOW_TO_OFFSET_OF_ENTITY", 0, Player.Character, offsetX, offsetY, 0f, GeneralFollow.RunSpeed, -1, 10.0f, true);//NativeFunction.Natives.TASK_FOLLOW_TO_OFFSET_OF_ENTITY(0, Player.Character, 3.0f, 3.0f, 0f, 0.75f, 20000, 5f, true);
                        NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, Player.Character, 2000);
                        NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, Player.Character, -1, 0, 2);
                        NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                        NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                        NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", PedGeneral.Pedestrian, lol);
                        NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                    }
                // }
                isSetForce = Player.GroupManager.SetForceTasking;
                EntryPoint.WriteToConsole($"Assign Follow speed:{GeneralFollow.RunSpeed}");
                isSetFollow = true;
                isSetCombat = false;
            }
        }
    }
   
}

