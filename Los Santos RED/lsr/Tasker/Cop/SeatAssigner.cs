using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SeatAssigner
{
    private IComplexTaskable Ped;
    private Cop Cop;
    private ITaskerReportable Tasker;
    private IEntityProvideable World;

    public SeatAssigner(IComplexTaskable ped, Cop cop, ITaskerReportable tasker, IEntityProvideable world)
    {
        Ped = ped;
        Cop = cop;
        Tasker = tasker;
        World = world;
    }

    public VehicleExt VehicleTryingToEnter { get; set; }
    public int SeatTryingToEnter { get; set; }

    public void AssignSeat()
    {
        VehicleTryingToEnter = null;
        SeatTryingToEnter = -99;
        if (IsSeatAvailable(Ped.AssignedVehicle, Ped.AssignedSeat))
        {
            VehicleTryingToEnter = Ped.AssignedVehicle;
            SeatTryingToEnter = Ped.AssignedSeat;
        }
        else if (IsSeatAvailable(Ped.AssignedVehicle, -1))
        {
            VehicleTryingToEnter = Ped.AssignedVehicle;
            SeatTryingToEnter = -1;
        }
        else if (IsSeatAvailable(Ped.AssignedVehicle, 0))
        {
            VehicleTryingToEnter = Ped.AssignedVehicle;
            SeatTryingToEnter = 0;
        }
        else 
        {
            VehicleExt LastVehicle = null;
            if (Ped.Pedestrian.LastVehicle.Exists())
            {
                LastVehicle = World.Vehicles.GetVehicleExt(Ped.Pedestrian.LastVehicle);
            }
            if(LastVehicle != null && IsSeatAvailable(LastVehicle, Ped.LastSeatIndex))
            {
                VehicleTryingToEnter = LastVehicle;
                SeatTryingToEnter = Ped.LastSeatIndex;
            }
            else if (LastVehicle != null && IsSeatAvailable(LastVehicle, -1))
            {
                VehicleTryingToEnter = LastVehicle;
                SeatTryingToEnter = -1;
            }
            else if (LastVehicle != null && IsSeatAvailable(LastVehicle, 0))
            {
                VehicleTryingToEnter = LastVehicle;
                SeatTryingToEnter = 0;
            }
            else
            {
                foreach (VehicleExt copCar in World.Vehicles.PoliceVehicleList)
                {
                    if (copCar.Vehicle.Exists() && copCar.Vehicle.Speed < 0.5f)//stopped 4 door car with at least one seat free in back
                    {
                        float DistanceTo = copCar.Vehicle.DistanceTo2D(Ped.Pedestrian);
                        if (DistanceTo <= 75f)
                        {
                            if(IsSeatAvailable(copCar, -1))
                            {
                                VehicleTryingToEnter = copCar;
                                SeatTryingToEnter = -1;
                            }
                            else if (IsSeatAvailable(copCar, 0))
                            {
                                VehicleTryingToEnter = copCar;
                                SeatTryingToEnter = 0;
                            }
                        }
                    }
                }
            }
        }
        if(VehicleTryingToEnter != null && SeatTryingToEnter != -99)
        {
            Tasker.RemoveSeatAssignment(Ped);
            Tasker.AddSeatAssignment(Ped, VehicleTryingToEnter, SeatTryingToEnter);
        }
    }

    private bool IsSeatAvailable(VehicleExt vehicleToCheck, int seatToCheck)
    {
        if (vehicleToCheck != null && vehicleToCheck.Vehicle.Exists() && vehicleToCheck.Vehicle.IsSeatFree(seatToCheck) && !Tasker.IsSeatAssigned(Ped, vehicleToCheck, seatToCheck) && NativeFunction.Natives.x639431E895B9AA57<bool>(Ped.Pedestrian, vehicleToCheck.Vehicle, seatToCheck, false, true))
        {
            return true;
        }
        return false;
    }
}

