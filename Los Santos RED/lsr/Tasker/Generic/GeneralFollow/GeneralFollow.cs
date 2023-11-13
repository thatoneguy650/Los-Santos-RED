using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GeneralFollow : ComplexTask
{
    private PedExt PedGeneral;
    private IEntityProvideable World;
    private IPlacesOfInterest PlacesOfInterest;
    private SeatAssigner SeatAssigner;
    private TaskState CurrentTaskState;
    private ISettingsProvideable Settings;
    private GroupManager GroupManager;
    private IWeaponIssuable WeaponIssuable;
    private bool IsSetPlayerVehicle;

    private bool AllowEnteringVehicle => true;//!Ped.IsAmbientSpawn || PedGeneral.HasExistedFor >= 10000;
    public bool ShouldGetInVehicle => Player.IsInVehicle;
    public bool ShouldGetOutOfVehicle => Player.IsOnFoot && PedGeneral.DistanceToPlayer <= 20f;
    public bool SetFollow { get; private set; } = true;
    public bool SetCombat { get; private set; } = false;
    public float RunSpeed { get; private set; } = 1.0f;
    public GeneralFollow(PedExt pedGeneral, IComplexTaskable ped, ITargetable player, IEntityProvideable world, List<VehicleExt> possibleVehicles, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, GroupManager groupManager, IWeaponIssuable weaponIssuable) : 
        base(player, ped, 1000)//1500
    {
        PedGeneral = pedGeneral;
        Name = "GeneralFollow";
        SubTaskName = "";
        World = world;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        SeatAssigner = new SeatAssigner(Ped, World, possibleVehicles);
        GroupManager = groupManager;
        WeaponIssuable = weaponIssuable;
    }
    public override void ReTask()
    {
        Start();
    }
    public override void Start()
    {
        CurrentTaskState?.Stop();
        IsSetPlayerVehicle = Player.GroupManager.RideInPlayerVehicleIfPossible;// PedGeneral.RideInPlayerVehicle;
        GetNewTaskState();
        CurrentTaskState?.Start();
    }
    public override void Stop()
    {
        CurrentTaskState?.Stop();
    }
    public override void Update()
    {
        if (CurrentTaskState == null || !CurrentTaskState.IsValid)
        {
            Start();
        }
        else
        {
            SubTaskName = CurrentTaskState.DebugName;
            CurrentTaskState.Update();
        }
        if (PedGeneral != null && PedGeneral.Pedestrian.Exists())
        {
            NativeFunction.Natives.SET_DRIVER_ABILITY(PedGeneral.Pedestrian, 1.0f);
            NativeFunction.Natives.SET_DRIVER_AGGRESSIVENESS(PedGeneral.Pedestrian, 1.0f);
        }
        UpdateParameters();
        UpdateWeapons();
    }

    private void GetNewTaskState()
    {
        if (ShouldGetInVehicle && AllowEnteringVehicle && !Ped.IsInVehicle && !SeatAssigner.IsAssignmentValid(false))
        {
            if (!IsSetPlayerVehicle)
            {
                SeatAssigner.AssignFrontSeat(true);
            }
            else
            {
                SeatAssigner.AssignPlayerPassenger(Player.CurrentVehicle);
                if(!SeatAssigner.IsAssignmentValid(false))
                {
                    SeatAssigner.AssignFrontSeat(true);
                }
            }
        }


        EntryPoint.WriteToConsole($"SeatAssigner.IsAssignmentValid(){SeatAssigner.IsAssignmentValid(false)}");
        if (Ped.IsInVehicle)
        {
            if (ShouldGetOutOfVehicle)
            {
                CurrentTaskState = new GetOutOfVehicleTaskState(PedGeneral, World, SeatAssigner, Settings, Player);
            }
            else
            {
                if (Ped.IsDriver)
                {
                    if (Ped.Pedestrian.Exists() && Ped.Pedestrian.IsInAnyVehicle(false) && SeatAssigner.HasPedsWaitingToEnter(World.Vehicles.GetVehicleExt(Ped.Pedestrian.CurrentVehicle), Ped.Pedestrian.SeatIndex))
                    {
                        CurrentTaskState = new WaitInVehicleTaskState(PedGeneral, Player, World, SeatAssigner, Settings, false);
                    }
                    else
                    {
                        int index = 0;
                        GroupMember gm = GroupManager.GetMember(PedGeneral);
                        if(gm != null)
                        {
                            index = gm.Index;
                        }
                        CurrentTaskState = new FollowInVehicleTaskState(PedGeneral, World, SeatAssigner, Settings, Player, index);
                    }
                }
                else
                {
                    CurrentTaskState = new RideInVehicleTaskState(PedGeneral, Player, World, SeatAssigner, Settings, GroupManager, WeaponIssuable, this);//can i get away with this?
                    //Wait
                }
            }
        }
        else
        {
            bool isAssignmentValid = SeatAssigner.IsAssignmentValid(false);
            //if (PedGeneral.IsInVehicle && isAssignmentValid)
            //{

            //}
            //else 
            if(ShouldGetInVehicle && isAssignmentValid)
            {
                CurrentTaskState = new GetInVehicleTaskState(PedGeneral, Player, World, SeatAssigner, Settings,Player.GroupManager.SetForceTasking) { IsGang = true };

            }
            else
            {
                CurrentTaskState = new FollowOnFootTaskState(PedGeneral, Player, World, SeatAssigner, Settings, GroupManager, WeaponIssuable, this);
            }
        }
    }


    private void UpdateParameters()
    {
        if (Player.GroupManager.SetFollowIfPossible)//PedGeneral.AlwaysFollow)
        {
            SetCombat = false;
            SetFollow = true;
        }
        else if (Player.GroupManager.SetCombatIfPossible)//PedGeneral.AlwaysInCombat)
        {
            SetCombat = true;
            SetFollow = false;
        }
        else if (Player.Character.IsInCombat || PedGeneral.IsWanted || Player.IsWanted || PedGeneral.Pedestrian.IsInCombat)
        {
            SetCombat = true;
            SetFollow = false;
        }
        else
        {
            SetCombat = false;
            SetFollow = true;
        }
        RunSpeed = 1.0f;
        if (SetCombat)
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

            //if (WeaponIssuable.Pedestrian.Exists())
            //{
            //    NativeFunction.Natives.SET_PED_CAN_SWITCH_WEAPON(WeaponIssuable.Pedestrian, true);
            //}
            //return;


            if (!SetCombat && Player.IsOnFoot && !Player.IsVisiblyArmed)
            {
                EntryPoint.WriteToConsole("GENERAL FOLLOW SET UNARMED");
                WeaponIssuable.WeaponInventory.SetUnarmed();
            }
            else if (Player.IsOnFoot && Player.IsVisiblyArmed && !WeaponIssuable.IsInVehicle)
            {
                if (WeaponIssuable.Pedestrian.Exists())
                {
                    EntryPoint.WriteToConsole("GENERAL FOLLOW SET BEST WEAPON");
                    uint bestWeapon = NativeFunction.Natives.GET_BEST_PED_WEAPON<uint>(WeaponIssuable.Pedestrian);
                    uint currentWeapon;
                    NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(WeaponIssuable.Pedestrian, out currentWeapon, true);
                    if (currentWeapon != bestWeapon)
                    {
                        EntryPoint.WriteToConsole("GENERAL FOLLOW SET BEST WEAPON RESETTING WEAPON CUZ ITS NOT OUT");
                        NativeFunction.Natives.SET_CURRENT_PED_WEAPON(WeaponIssuable.Pedestrian, bestWeapon, true);
                    }
                    NativeFunction.Natives.SET_PED_CAN_SWITCH_WEAPON(WeaponIssuable.Pedestrian, true);
                }
            }
            else
            {
                if (WeaponIssuable.Pedestrian.Exists())
                {
                    NativeFunction.Natives.SET_PED_CAN_SWITCH_WEAPON(WeaponIssuable.Pedestrian, true);
                }
            }
        }
    }


}

