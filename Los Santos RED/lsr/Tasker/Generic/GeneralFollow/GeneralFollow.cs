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
    private bool AllowEnteringVehicle => true;//!Ped.IsAmbientSpawn || PedGeneral.HasExistedFor >= 10000;
    public bool ShouldGetInVehicle => Player.IsInVehicle;
    public bool ShouldGetOutOfVehicle => Player.IsOnFoot;
    public GeneralFollow(PedExt pedGeneral, IComplexTaskable ped, ITargetable player, IEntityProvideable world, List<VehicleExt> possibleVehicles, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, GroupManager groupManager) : base(player, ped, 1500)//1500
    {
        PedGeneral = pedGeneral;
        Name = "GeneralFollow";
        SubTaskName = "";
        World = world;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        SeatAssigner = new SeatAssigner(Ped, World, possibleVehicles);
        GroupManager = groupManager;
    }
    public override void ReTask()
    {
        Start();
    }
    public override void Start()
    {
        CurrentTaskState?.Stop();
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
    }

    private void GetNewTaskState()
    {
        if (ShouldGetInVehicle && AllowEnteringVehicle && !Ped.IsInVehicle && !SeatAssigner.IsAssignmentValid())
        {
            SeatAssigner.AssignFrontSeat(true);
        }
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
                    //Wait
                }
            }
        }
        else
        {
            if(ShouldGetInVehicle && SeatAssigner.IsAssignmentValid())
            {
                CurrentTaskState = new GetInVehicleTaskState(PedGeneral, Player, World, SeatAssigner, Settings,false) { IsGang = true };

            }
            else
            {
                CurrentTaskState = new FollowOnFootTaskState(PedGeneral, Player, World, SeatAssigner, Settings, GroupManager);
            }
        }
        //if (CurrentTaskState != null)
        //{
        //    EntryPoint.WriteToConsole($"{PedGeneral?.Handle} GetNewTaskState {CurrentTaskState.DebugName}");
        //}
        //else
        //{
        //    EntryPoint.WriteToConsole($"{PedGeneral?.Handle} GetNewTaskState NONE");
        //}
    }
}

