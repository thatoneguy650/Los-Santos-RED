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
    private bool IsStandardFollow = true;
    private bool IsCombat = false;
    private float followSpeed;
    private float RunSpeed;

    public FollowOnFootTaskState(PedExt pedGeneral, ITargetable player, IEntityProvideable world, SeatAssigner seatAssigner, ISettingsProvideable settings, GroupManager groupManager, IWeaponIssuable weaponissueable)
    {
        PedGeneral = pedGeneral;
        Player = player;
        World = world;
        SeatAssigner = seatAssigner;
        Settings = settings;
        GroupManager = groupManager;
        WeaponIssuable = weaponissueable;
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
        //Update();
    }
    public void Stop()
    {

    }
    public void Update()
    {
        //if (!PedGeneral.Pedestrian.Exists())
        //{
        //    return;
        //}
        //UpdateParameters();
        //UpdateWeapons();
        //UpdateTasking();
    }
    private void UpdateTasking()
    {
        if (IsCombat)
        {
            if (!isSetCombat)
            {
                NativeFunction.Natives.TASK_COMBAT_HATED_TARGETS_AROUND_PED(PedGeneral.Pedestrian, 100f, 0);//TR
                isSetCombat = true;
            }
        }
        else
        {
            if (!isSetFollow || followSpeed != RunSpeed)
            {
                if (PedGeneral.DistanceToPlayer <= 10f)
                {
                    NativeFunction.Natives.CLEAR_PED_TASKS(PedGeneral.Pedestrian);
                }
                else
                {
                    unsafe
                    {
                        float offsetX = RandomItems.GetRandomNumber(-3.0f, 3.0f);
                        float offsetY = RandomItems.GetRandomNumber(-3.0f, 3.0f);
                        followSpeed = RunSpeed;
                        int lol = 0;
                        NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                        NativeFunction.CallByName<bool>("TASK_FOLLOW_TO_OFFSET_OF_ENTITY", 0, Player.Character, offsetX, offsetY, 0f, RunSpeed, -1, 10.0f, true);//NativeFunction.Natives.TASK_FOLLOW_TO_OFFSET_OF_ENTITY(0, Player.Character, 3.0f, 3.0f, 0f, 0.75f, 20000, 5f, true);
                        NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, Player.Character, 2000);

                        NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, Player.Character, -1, 0, 2);
                        NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                        NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                        NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", PedGeneral.Pedestrian, lol);
                        NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                    }
                }
                EntryPoint.WriteToConsole($"Assign Follow speed:{RunSpeed}");
                isSetFollow = true;
            }
        }
    }
    private void UpdateParameters()
    {
        if (PedGeneral.AlwaysFollow)
        {
            IsCombat = false;
            IsStandardFollow = true;
        }
        else if (PedGeneral.AlwaysInCombat)
        {
            IsCombat = true;
            IsStandardFollow = false;
        }
        else if (Player.Character.IsInCombat || PedGeneral.IsWanted || Player.IsWanted || PedGeneral.Pedestrian.IsInCombat || PedGeneral.AlwaysInCombat)
        {
            IsCombat = true;
            IsStandardFollow = false;
        }
        else
        {
            IsCombat = false;
            IsStandardFollow = true;
        }
        RunSpeed = 1.0f;
        if (IsCombat)
        {
            RunSpeed = 7.0f;
        }
        else if (PedGeneral.DistanceToPlayer >= 20f)
        {
            RunSpeed = 3.0f;
        }
        else
        {
            float playerSpeed = Player.Character.Speed;
            if (playerSpeed <= 2.2f)
            {
                RunSpeed = 1.0f;
            }
            else if (playerSpeed <= 3.2)
            {
                RunSpeed = 2.0f;
            }
            else if (playerSpeed <= 4.2)
            {
                RunSpeed = 3.0f;
            }
            else
            {
                RunSpeed = 4.0f;
            }
        }
    }
    private void UpdateWeapons()
    {
        if (WeaponIssuable != null)
        {
            if (!IsCombat && !Player.IsVisiblyArmed)
            {
                WeaponIssuable.WeaponInventory.SetUnarmed();
            }
            else if (Player.IsVisiblyArmed)
            {

                if (WeaponIssuable.Pedestrian.Exists())
                {
                    uint bestWeapon = NativeFunction.Natives.GET_BEST_PED_WEAPON<uint>(WeaponIssuable.Pedestrian);
                    uint currentWeapon;
                    NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(WeaponIssuable.Pedestrian, out currentWeapon, true);
                    if (currentWeapon != bestWeapon)
                    {
                        NativeFunction.Natives.SET_CURRENT_PED_WEAPON(WeaponIssuable.Pedestrian, bestWeapon, true);
                    }
                }
            }
        }
    }
}

