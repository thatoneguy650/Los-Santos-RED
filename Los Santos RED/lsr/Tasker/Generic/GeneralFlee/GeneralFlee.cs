//using LosSantosRED.lsr.Interface;
//using LSR.Vehicles;
//using Rage;
//using Rage.Native;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


//public class GeneralFlee : ComplexTask
//{
//    private PedExt PedGeneral;
//    private IEntityProvideable World;
//    private IPlacesOfInterest PlacesOfInterest;
//    private SeatAssigner SeatAssigner;
//    private TaskState CurrentTaskState;
//    private ISettingsProvideable Settings;
//    private bool BlockPermanentEvents = false;
//    private bool CheckPassengers = false;
//    private bool CheckSiren = false;
//    private bool ForceStandardScenarios = false;
//    private bool AllowEnteringVehicle => !Ped.IsAnimal && (!Ped.IsLocationSpawned || PedGeneral.HasExistedFor >= 10000);
//    public GeneralFlee(PedExt pedGeneral, IComplexTaskable ped, ITargetable player, IEntityProvideable world, List<VehicleExt> possibleVehicles, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, bool blockPermanentEvents, bool checkPassengers, bool checkSiren, bool forceStandardScenarios) : base(player, ped, 1500)//1500
//    {
//        PedGeneral = pedGeneral;
//        Name = "Flee";
//        SubTaskName = "";
//        World = world;
//        PlacesOfInterest = placesOfInterest;
//        Settings = settings;
//        BlockPermanentEvents = blockPermanentEvents;
//        CheckPassengers = checkPassengers;
//        CheckSiren = checkSiren;
//        ForceStandardScenarios = forceStandardScenarios;
//        SeatAssigner = new SeatAssigner(Ped, World, possibleVehicles);
//    }
//    public override void ReTask()
//    {
//        Start();
//    }
//    public override void Start()
//    {
//        CurrentTaskState?.Stop();
//        GetNewTaskState();
//        CurrentTaskState?.Start();
//    }
//    public override void Stop()
//    {
//        CurrentTaskState?.Stop();
//    }
//    public override void Update()
//    {
//        if (CurrentTaskState == null || !CurrentTaskState.IsValid)
//        {
//            Start();
//        }
//        else
//        {
//            SubTaskName = CurrentTaskState.DebugName;
//            CurrentTaskState.Update();
//        }
//        if (CheckSiren)
//        {
//            SetSiren();
//        }
//    }
//    private void GetNewTaskState()
//    {
//        if (AllowEnteringVehicle && !Ped.IsInVehicle && !SeatAssigner.IsAssignmentValid(true))
//        {
//            SeatAssigner.AssignFrontSeat(PedGeneral.HasExistedFor >= 10000);
//        }
//        if (Ped.IsInVehicle)
//        {
//            if (Ped.IsDriver)
//            {
//                if (Ped.Pedestrian.Exists() && Ped.Pedestrian.IsInAnyVehicle(false) && SeatAssigner.HasPedsWaitingToEnter(World.Vehicles.GetVehicleExt(Ped.Pedestrian.CurrentVehicle), Ped.Pedestrian.SeatIndex))
//                {
//                    CurrentTaskState = new WaitInVehicleTaskState(PedGeneral, Player, World, SeatAssigner, Settings, BlockPermanentEvents);
//                }
//                else if (CheckPassengers && HasArrestedPassengers())
//                {
//                    CurrentTaskState = new ReturnToStationVehicleTaskState(PedGeneral, World, PlacesOfInterest, Settings, BlockPermanentEvents);
//                }
//                else
//                {
//                    CurrentTaskState = new WanderInVehicleTaskState(PedGeneral, World, SeatAssigner, PlacesOfInterest, Settings, BlockPermanentEvents, false);
//                }
//            }
//            else
//            {
//                CurrentTaskState = new WanderInVehicleTaskState(PedGeneral, World, SeatAssigner, PlacesOfInterest, Settings, BlockPermanentEvents, false);//Maybe Get Out
//            }
//        }
//        else
//        {
//            if (SeatAssigner.IsAssignmentValid(true))//Ped.ShouldGetInVehicle)
//            {
//                CurrentTaskState = new GetInVehicleTaskState(PedGeneral, Player, World, SeatAssigner, Settings, BlockPermanentEvents) { DefaultEnterSpeed = 2.0f };
//            }
//            else
//            {
//                CurrentTaskState = new WanderOnFootTaskState(PedGeneral, World, SeatAssigner, Settings, BlockPermanentEvents, ForceStandardScenarios);
//            }
//        }

//        //if(CurrentTaskState != null)
//        //{
//        //    EntryPoint.WriteToConsole($"{PedGeneral?.Handle} GetNewTaskState {CurrentTaskState.DebugName}");  
//        //}
//        //else
//        //{
//        //    EntryPoint.WriteToConsole($"{PedGeneral?.Handle} GetNewTaskState NONE");
//        //}
//    }
//    private void SetSiren()
//    {
//        if (Ped.Pedestrian.Exists() && Ped.Pedestrian.CurrentVehicle.Exists() && Ped.IsDriver && Ped.Pedestrian.CurrentVehicle.HasSiren && Ped.Pedestrian.CurrentVehicle.IsSirenOn)
//        {
//            Ped.Pedestrian.CurrentVehicle.IsSirenOn = false;
//            Ped.Pedestrian.CurrentVehicle.IsSirenSilent = false;
//        }
//    }
//    public bool HasArrestedPassengers()
//    {
//        if (PedGeneral.IsDriver && PedGeneral.Pedestrian.IsInAnyVehicle(false) && PedGeneral.Pedestrian.CurrentVehicle.Exists())
//        {
//            foreach (Ped ped in PedGeneral.Pedestrian.CurrentVehicle.Passengers)
//            {
//                PedExt pedExt = World.Pedestrians.GetPedExt(ped.Handle);
//                if (pedExt != null && pedExt.IsArrested)
//                {
//                    return true;
//                }
//                if (ped.Handle == Game.LocalPlayer.Character.Handle)
//                {
//                    return true;
//                }
//            }
//        }
//        return false;
//    }
//}

