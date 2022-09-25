using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SeatAssigner
{
    private ISeatAssignable Ped;
    private IEntityProvideable World;
    private List<VehicleExt> VehiclesToTest = new List<VehicleExt>();

    public SeatAssigner(ISeatAssignable ped, IEntityProvideable world, List<VehicleExt> vehiclesToTest)
    {
        Ped = ped;
        World = world;
        VehiclesToTest = vehiclesToTest;
    }

    public VehicleExt VehicleTryingToEnter { get; set; }
    public int SeatTryingToEnter { get; set; }
    public void AssignFrontSeat()
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
            VehicleExt LastVehicle = null;//
            if (Ped.Pedestrian.LastVehicle.Exists())
            {
                if (!Ped.BlackListedVehicles.Any(x => x == Ped.Pedestrian.LastVehicle.Handle))
                {
                    LastVehicle = World.Vehicles.GetVehicleExt(Ped.Pedestrian.LastVehicle);
                }
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
                foreach (VehicleExt possibleVehicle in VehiclesToTest.Where(x => x.Vehicle.Exists() && x.Vehicle.Speed < 0.5f).OrderBy(x => x.Vehicle.DistanceTo2D(Ped.Pedestrian)))
                {
                    float DistanceTo = possibleVehicle.Vehicle.DistanceTo2D(Ped.Pedestrian);
                    if (DistanceTo <= 125f)
                    {
                        if(IsSeatAvailable(possibleVehicle, -1))
                        {
                            VehicleTryingToEnter = possibleVehicle;
                            SeatTryingToEnter = -1;
                            break;
                        }
                        else if (IsSeatAvailable(possibleVehicle, 0))
                        {
                            VehicleTryingToEnter = possibleVehicle;
                            SeatTryingToEnter = 0;
                            break;
                        }
                    }
                }
            }
        }
        if(VehicleTryingToEnter != null && SeatTryingToEnter != -1)
        {
            if(!World.Pedestrians.IsSeatAssignedToAnyone(VehicleTryingToEnter, -1) && IsSeatAvailable(VehicleTryingToEnter, -1))
            {
                VehicleTryingToEnter = VehicleTryingToEnter;
                SeatTryingToEnter = -1;
            }
        }
        if(VehicleTryingToEnter != null && SeatTryingToEnter != -99)
        {
            World.Pedestrians.RemoveSeatAssignment(Ped);
            World.Pedestrians.AddSeatAssignment(Ped, VehicleTryingToEnter, SeatTryingToEnter);
            if (VehicleTryingToEnter.Vehicle.Exists())
            {
                VehicleTryingToEnter.Vehicle.LockStatus = Rage.VehicleLockStatus.Unlocked;
                VehicleTryingToEnter.Vehicle.MustBeHotwired = false;
            }
        }
    }


    public void AssignPrisonerSeat()
    {
        VehicleTryingToEnter = null;
        SeatTryingToEnter = -99;
        foreach (VehicleExt possibleVehicle in VehiclesToTest.Where(x=> x.Vehicle.Exists() && x.Vehicle.Model.NumberOfSeats >= 4 && x.Vehicle.Speed < 0.5f).OrderBy(x=> x.Vehicle.DistanceTo2D(Ped.Pedestrian)))
        {
            float DistanceTo = possibleVehicle.Vehicle.DistanceTo2D(Ped.Pedestrian);
            if (DistanceTo <= 75f)
            {
                if (IsSeatAvailable(possibleVehicle, 1))
                {
                    VehicleTryingToEnter = possibleVehicle;
                    SeatTryingToEnter = 1;
                    
                    EntryPoint.WriteToConsole($"Prisoner Seat Assigned 1 Distance: {DistanceTo} Seat: {SeatTryingToEnter}");
                    break;
                }
                else if (IsSeatAvailable(possibleVehicle, 2))
                {
                    VehicleTryingToEnter = possibleVehicle;
                    SeatTryingToEnter = 2;
                    EntryPoint.WriteToConsole($"Prisoner Seat Assigned 2 Distance: {DistanceTo} Seat: {SeatTryingToEnter}");
                    break;
                }
            }
        }
        if (VehicleTryingToEnter != null && SeatTryingToEnter != -99)
        {
            World.Pedestrians.RemoveSeatAssignment(Ped);
            World.Pedestrians.AddSeatAssignment(Ped, VehicleTryingToEnter, SeatTryingToEnter);
        }
    }
    private bool IsSeatAvailable(VehicleExt vehicleToCheck, int seatToCheck)
    {
        if (vehicleToCheck != null && vehicleToCheck.Vehicle.Exists() && vehicleToCheck.Vehicle.IsSeatFree(seatToCheck) && !World.Pedestrians.IsSeatAssigned(Ped, vehicleToCheck, seatToCheck) && NativeFunction.Natives.x639431E895B9AA57<bool>(Ped.Pedestrian, vehicleToCheck.Vehicle, seatToCheck, false, true))
        {
            return true;
        }
        //IS_ENTRY_POINT_FOR_SEAT_CLEAR
        //GET_ENTRY_POINT_POSITION
        return false;
    }
    public Vector3 GetEntryPosition(VehicleExt vehicleToCheck, int seatToCheck)
    {
        Vector3 EntryPoint = Vector3.Zero;
        if (vehicleToCheck != null && vehicleToCheck.Vehicle.Exists())
        {
            int doorID = GetDoorFromSeat(seatToCheck);
            if (doorID != -1)
            {
                EntryPoint = NativeFunction.Natives.GET_ENTRY_POINT_POSITION<Vector3>(vehicleToCheck.Vehicle, doorID);
            }
        }
        return EntryPoint;
    }
    public int GetDoorFromSeat(int seatToCheck)
    {
        return seatToCheck + 1;


        //if(seatToCheck == -1)//driver
        //{
        //    return 0;
        //}
        //else if (seatToCheck == 0)//passenger
        //{
        //    return 2;
        //}
        //else if (seatToCheck == 1)//left rear
        //{
        //    return 1;
        //}
        //else if (seatToCheck == 2)//right rear
        //{
        //    return 3;
        //}
        //else if (seatToCheck == 3)//outside left
        //{
        //    return 4;
        //}
        //else if (seatToCheck == 4)//outside right
        //{
        //    return 5;
        //}
        //return -1;
    }


    //enum eDoorId//THIS IS BULLSHIT
    //{
    //    VEH_EXT_DOOR_INVALID_ID = -1,
    //    VEH_EXT_DOOR_DSIDE_F,//driver
    //    VEH_EXT_DOOR_DSIDE_R,//passenger front
    //    VEH_EXT_DOOR_PSIDE_F,
    //    VEH_EXT_DOOR_PSIDE_R,
    //    VEH_EXT_BONNET,
    //    VEH_EXT_BOOT
    //};

    /*Moreinfo of Seat Index
DriverSeat = -1
Passenger = 0
Left Rear = 1
RightRear = 2*/
}

