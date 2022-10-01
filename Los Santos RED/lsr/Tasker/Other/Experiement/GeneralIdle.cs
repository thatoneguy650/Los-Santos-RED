using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GeneralIdle : ComplexTask
{
    private PedExt PedGeneral;
    private IEntityProvideable World;
    private IPlacesOfInterest PlacesOfInterest;
    private SeatAssigner SeatAssigner;
    private TaskState CurrentTaskState;
    private bool AllowEnteringVehicle => !Ped.IsAmbientSpawn || PedGeneral.HasExistedFor >= 10000;
    public GeneralIdle(PedExt pedGeneral, IComplexTaskable ped, ITargetable player, IEntityProvideable world,List<VehicleExt> possibleVehicles, IPlacesOfInterest placesOfInterest) : base(player, ped, 1500)//1500
    {
        PedGeneral = pedGeneral;
        Name = "Idle";
        SubTaskName = "";
        World = world;
        PlacesOfInterest = placesOfInterest;
        SeatAssigner = new SeatAssigner(Ped, World, possibleVehicles);
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
        if(CurrentTaskState == null || !CurrentTaskState.IsValid)
        {
            Start();
        }
        else
        {
            SubTaskName = CurrentTaskState.DebugName;
            CurrentTaskState.Update();
        }
        SetSiren();
    }

    private void GetNewTaskState()
    {
        if(AllowEnteringVehicle && !Ped.IsInVehicle && !SeatAssigner.IsAssignmentValid())
        {
            SeatAssigner.AssignFrontSeat(PedGeneral.HasExistedFor >= 10000);
        }
        if(Ped.IsInVehicle)
        {
            if(Ped.IsDriver)
            {
                if (Ped.Pedestrian.Exists() && Ped.Pedestrian.IsInAnyVehicle(false) && SeatAssigner.HasPedsWaitingToEnter(World.Vehicles.GetVehicleExt(Ped.Pedestrian.CurrentVehicle), Ped.Pedestrian.SeatIndex))
                {
                    CurrentTaskState = new WaitInVehicleTaskState(PedGeneral, World, SeatAssigner);
                }
                else if(HasArrestedPassengers())
                {
                    CurrentTaskState = new ReturnToStationVehicleTaskState(PedGeneral, World, PlacesOfInterest);
                }
                else
                {
                    CurrentTaskState = new WanderInVehicleTaskState(PedGeneral, World, SeatAssigner, PlacesOfInterest);
                }
            }
            else
            {
                CurrentTaskState = new WanderInVehicleTaskState(PedGeneral, World, SeatAssigner, PlacesOfInterest);//Maybe Get Out
            }
        }
        else
        {
            if (SeatAssigner.IsAssignmentValid())//Ped.ShouldGetInVehicle)
            {
                CurrentTaskState = new GetInVehicleTaskState(PedGeneral,World,SeatAssigner);
            }
            else
            {
                CurrentTaskState = new WanderOnFootTaskState(PedGeneral, World, SeatAssigner);
            }
        }



        if(CurrentTaskState != null)
        {
            EntryPoint.WriteToConsole($"{PedGeneral?.Handle} GetNewTaskState {CurrentTaskState.DebugName}");  
        }
        else
        {
            EntryPoint.WriteToConsole($"{PedGeneral?.Handle} GetNewTaskState NONE");
        }
    }
    private void SetSiren()
    {
        if (Ped.Pedestrian.Exists() && Ped.Pedestrian.CurrentVehicle.Exists() && Ped.Pedestrian.CurrentVehicle.HasSiren && Ped.Pedestrian.CurrentVehicle.IsSirenOn)
        {
            Ped.Pedestrian.CurrentVehicle.IsSirenOn = false;
            Ped.Pedestrian.CurrentVehicle.IsSirenSilent = false;
        }
    }
    public bool HasArrestedPassengers()
    {
        if (PedGeneral.IsDriver && PedGeneral.Pedestrian.IsInAnyVehicle(false) && PedGeneral.Pedestrian.CurrentVehicle.Exists())
        {
            foreach (Ped ped in PedGeneral.Pedestrian.CurrentVehicle.Passengers)
            {
                PedExt pedExt = World.Pedestrians.GetPedExt(ped.Handle);
                if (pedExt != null && pedExt.IsArrested)
                {
                    return true;
                }
                if (ped.Handle == Game.LocalPlayer.Character.Handle)
                {
                    return true;
                }
            }
        }
        return false;
    }
}

