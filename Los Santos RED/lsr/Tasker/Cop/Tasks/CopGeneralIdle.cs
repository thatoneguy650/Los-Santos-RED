using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CopGeneralIdle : GeneralIdle
{


    public CopGeneralIdle(PedExt pedGeneral, IComplexTaskable ped, ITargetable player, IEntityProvideable world, List<VehicleExt> possibleVehicles, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, bool blockPermanentEvents, bool checkPassengers, bool checkSiren, bool forceStandardScenarios) : base(pedGeneral, ped, player, world, possibleVehicles, placesOfInterest, settings, blockPermanentEvents, checkPassengers, checkSiren, forceStandardScenarios)
    {

    }

    protected override void GetNewTaskState()
    {
        if (AllowEnteringVehicle && !Ped.IsInVehicle && !SeatAssigner.IsAssignmentValid(true))
        {
            SeatAssigner.AssignFrontSeat(PedGeneral.HasExistedFor >= 10000);
        }
        if (Ped.IsInVehicle)
        {
            if (Ped.IsDriver)
            {
                if (Ped.Pedestrian.Exists() && Ped.Pedestrian.IsInAnyVehicle(false) && SeatAssigner.HasPedsWaitingToEnter(World.Vehicles.GetVehicleExt(Ped.Pedestrian.CurrentVehicle), Ped.Pedestrian.SeatIndex))
                {
                    CurrentTaskState = new WaitInVehicleTaskState(PedGeneral, Player, World, SeatAssigner, Settings, BlockPermanentEvents);
                }
                else if (CheckPassengers && HasArrestedPassengers())
                {
                    CurrentTaskState = new ReturnToStationVehicleTaskState(PedGeneral, World, PlacesOfInterest, Settings, BlockPermanentEvents);
                }
                else
                {
                    CurrentTaskState = new WanderInVehicleTaskState(PedGeneral, World, SeatAssigner, PlacesOfInterest, Settings, BlockPermanentEvents, false);
                }
            }
            else
            {
                CurrentTaskState = new WanderInVehicleTaskState(PedGeneral, World, SeatAssigner, PlacesOfInterest, Settings, BlockPermanentEvents, false);//Maybe Get Out
            }
        }
        else
        {
            if (SeatAssigner.IsAssignmentValid(true))//Ped.ShouldGetInVehicle)
            {
                CurrentTaskState = new GetInVehicleTaskState(PedGeneral, Player, World, SeatAssigner, Settings, BlockPermanentEvents);
            }
            else
            {
                CurrentTaskState = new WanderOnFootTaskState(PedGeneral, World, SeatAssigner, Settings, BlockPermanentEvents, ForceStandardScenarios);
            }
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

