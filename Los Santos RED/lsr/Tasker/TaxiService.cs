using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TaxiService : ComplexTask, ILocationReachable
{
    private TaxiDriver TaxiDriver;
    private PedExt PedGeneral;
    private IEntityProvideable World;
    private IPlacesOfInterest PlacesOfInterest;
    private SeatAssigner SeatAssigner;
    private TaskState CurrentTaskState;
    private ISettingsProvideable Settings;
    private bool BlockPermanentEvents = false;
    private uint GameTimeReachedLocation;
    private bool AllowEnteringVehicle => !Ped.IsAnimal && (!Ped.IsLocationSpawned || PedGeneral.HasExistedFor >= 10000);

    public bool HasReachedLocatePosition { get; protected set; } = false;

    public TaxiService(PedExt pedGeneral, IComplexTaskable ped, ITargetable player, IEntityProvideable world, List<VehicleExt> possibleVehicles, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, 
        bool blockPermanentEvents, TaxiDriver taxiDriver) : base(player, ped, 1500)//1500
    {
        PedGeneral = pedGeneral;
        Name = "TaxiService";
        SubTaskName = "";
        World = world;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        BlockPermanentEvents = blockPermanentEvents;
        TaxiDriver = taxiDriver;
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
        if (CurrentTaskState == null || !CurrentTaskState.IsValid)
        {
            Start();
        }
        else
        {
            SubTaskName = CurrentTaskState.DebugName;
            CurrentTaskState.Update();
        }
        CheckLocationItems();
    }

    private void CheckLocationItems()
    {
        if(GameTimeReachedLocation > 0 && Game.GameTime - GameTimeReachedLocation >= 10000)
        {
            TaxiDriver.ReleaseTasking();
            EntryPoint.WriteToConsole("TAXI DRIVER WAITED 10 SECONDS, RELEASING");
        }
    }

    private void GetNewTaskState()
    {
        if (AllowEnteringVehicle && !Ped.IsInVehicle && !SeatAssigner.IsAssignmentValid())
        {
            SeatAssigner.AssignFrontSeat(PedGeneral.HasExistedFor >= 10000);
        }
        if (Ped.IsInVehicle)
        {
            if (!HasReachedLocatePosition)
            {
                if (Ped.IsDriver)
                {
                    if (Ped.Pedestrian.Exists() && Ped.Pedestrian.IsInAnyVehicle(false) && SeatAssigner.HasPedsWaitingToEnter(World.Vehicles.GetVehicleExt(Ped.Pedestrian.CurrentVehicle), Ped.Pedestrian.SeatIndex))
                    {
                        CurrentTaskState = new WaitInVehicleTaskState(PedGeneral, Player, World, SeatAssigner, Settings, BlockPermanentEvents);
                    }
                    else
                    {
                        CurrentTaskState = new RegularGoToInVehicleTaskState(PedGeneral, Player, World, SeatAssigner, Settings, BlockPermanentEvents, TaxiDriver.PickupLocation, this);// CurrentTaskState = new WanderInVehicleTaskState(PedGeneral, World, SeatAssigner, PlacesOfInterest, Settings, BlockPermanentEvents, false);
                    }
                }
                else
                {
                    CurrentTaskState = new RegularGoToInVehicleTaskState(PedGeneral, Player, World, SeatAssigner, Settings, BlockPermanentEvents, TaxiDriver.PickupLocation, this);
                }
            }
            else
            {
                CurrentTaskState = new WaitInVehicleTaskState(PedGeneral, Player, World, SeatAssigner, Settings, BlockPermanentEvents);
            }
        }
        else
        {
            if (SeatAssigner.IsAssignmentValid())//Ped.ShouldGetInVehicle)
            {
                CurrentTaskState = new GetInVehicleTaskState(PedGeneral, Player, World, SeatAssigner, Settings, BlockPermanentEvents);
            }
            else
            {
                CurrentTaskState = new WanderOnFootTaskState(PedGeneral, World, SeatAssigner, Settings, BlockPermanentEvents, true);
            }
        }
    }

    public virtual void OnLocationReached()
    {
        GameTimeReachedLocation = Game.GameTime;
        HasReachedLocatePosition = true;
        EntryPoint.WriteToConsole("TAXI DRIVER ARRIVED, WAITING 10 SECONDS");
    }
}

