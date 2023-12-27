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
    public bool AllowLastVehicle { get; set; } = false;

    public VehicleExt VehicleAssigned { get; set; }
    public int SeatAssigned { get; set; }
    public void AssignDriverSeat(bool includeNonAssigned)
    {
        VehicleAssigned = null;
        SeatAssigned = -99;
        if (IsSeatAvailable(Ped.AssignedVehicle, -1))
        {
            VehicleAssigned = Ped.AssignedVehicle;
            SeatAssigned = -1;
        }
        else
        {
            VehicleExt LastVehicle = null;//
            if (Ped.Pedestrian.Exists())
            {
                if (AllowLastVehicle && Ped.Pedestrian.LastVehicle.Exists())
                {
                    if (!Ped.BlackListedVehicles.Any(x => x == Ped.Pedestrian.LastVehicle.Handle))
                    {
                        LastVehicle = World.Vehicles.GetVehicleExt(Ped.Pedestrian.LastVehicle);
                    }
                }
                if (LastVehicle != null && IsSeatAvailable(LastVehicle, -1))
                {
                    VehicleAssigned = LastVehicle;
                    SeatAssigned = -1;
                }
               
                else if (includeNonAssigned)
                {
                    foreach (VehicleExt possibleVehicle in VehiclesToTest.Where(x => x != null && x.Vehicle.Exists() && x.Vehicle.Speed < 0.5f).OrderBy(x => x.Vehicle.DistanceTo2D(Ped.Pedestrian)))
                    {
                        float DistanceTo = possibleVehicle.Vehicle.DistanceTo2D(Ped.Pedestrian);
                        if (DistanceTo <= 125f)
                        {
                            if (IsSeatAvailable(possibleVehicle, -1))
                            {
                                VehicleAssigned = possibleVehicle;
                                SeatAssigned = -1;
                                break;
                            }
                        }
                    }
                }
            }
        }
        if (VehicleAssigned != null && SeatAssigned != -1)
        {
            if (!World.Pedestrians.IsSeatAssignedToAnyone(VehicleAssigned, -1) && IsSeatAvailable(VehicleAssigned, -1))
            {
                VehicleAssigned = VehicleAssigned;
                SeatAssigned = -1;
            }
        }
        if (VehicleAssigned != null && SeatAssigned != -99)
        {
            World.Pedestrians.RemoveSeatAssignment(Ped);
            World.Pedestrians.AddSeatAssignment(Ped, VehicleAssigned, SeatAssigned);
            if (VehicleAssigned.Vehicle.Exists())
            {
                VehicleAssigned.Vehicle.LockStatus = Rage.VehicleLockStatus.Unlocked;
                VehicleAssigned.Vehicle.MustBeHotwired = false;
            }
        }
    }
    public void AssignFrontSeat(bool includeNonAssigned)
    {
        if(Ped.IsAnimal)
        {
            AssignPrisonerSeat();
            return;
        }
        VehicleAssigned = null;
        SeatAssigned = -99;

        if (Ped.AssignedSeat == -1 && IsSeatAvailable(Ped.AssignedVehicle, Ped.AssignedSeat))
        {
            VehicleAssigned = Ped.AssignedVehicle;
            SeatAssigned = Ped.AssignedSeat;
        }
        else if (IsSeatAvailable(Ped.AssignedVehicle, Ped.AssignedSeat))
        {
            VehicleAssigned = Ped.AssignedVehicle;
            SeatAssigned = Ped.AssignedSeat;
        }
        else if (IsSeatAvailable(Ped.AssignedVehicle, -1))
        {
            VehicleAssigned = Ped.AssignedVehicle;
            SeatAssigned = -1;
        }
        else if (IsSeatAvailable(Ped.AssignedVehicle, 0))
        {
            VehicleAssigned = Ped.AssignedVehicle;
            SeatAssigned = 0;
        }
        else if (!IsSeatAvailable(Ped.AssignedVehicle, -1) && IsSeatAvailable(Ped.AssignedVehicle, 1))
        {
            VehicleAssigned = Ped.AssignedVehicle;
            SeatAssigned = 1;
        }
        else 
        {
            VehicleExt LastVehicle = null;//
            if (Ped.Pedestrian.Exists())
            {
                if (AllowLastVehicle && Ped.Pedestrian.LastVehicle.Exists())
                {
                    if (!Ped.BlackListedVehicles.Any(x => x == Ped.Pedestrian.LastVehicle.Handle))
                    {
                        LastVehicle = World.Vehicles.GetVehicleExt(Ped.Pedestrian.LastVehicle);
                    }
                }
                if (LastVehicle != null && IsSeatAvailable(LastVehicle, Ped.LastSeatIndex))
                {
                    VehicleAssigned = LastVehicle;
                    SeatAssigned = Ped.LastSeatIndex;
                }
                else if (LastVehicle != null && IsSeatAvailable(LastVehicle, -1))
                {
                    VehicleAssigned = LastVehicle;
                    SeatAssigned = -1;
                }
                else if (LastVehicle != null && IsSeatAvailable(LastVehicle, 0))
                {
                    VehicleAssigned = LastVehicle;
                    SeatAssigned = 0;
                }
                else if (includeNonAssigned)
                {
                    foreach (VehicleExt possibleVehicle in VehiclesToTest.Where(x => x != null && x.Vehicle.Exists() && x.Vehicle.Speed < 0.5f).OrderBy(x => x.Vehicle.DistanceTo2D(Ped.Pedestrian)))
                    {
                        float DistanceTo = possibleVehicle.Vehicle.DistanceTo2D(Ped.Pedestrian);
                        if (DistanceTo <= 75f)//125f)
                        {
                            if (IsSeatAvailable(possibleVehicle, -1))
                            {
                                VehicleAssigned = possibleVehicle;
                                SeatAssigned = -1;
                                break;
                            }
                            else if (IsSeatAvailable(possibleVehicle, 0))
                            {
                                VehicleAssigned = possibleVehicle;
                                SeatAssigned = 0;
                                break;
                            }
                        }
                    }
                }
            }
        }
        if(VehicleAssigned != null && SeatAssigned != -1)
        {
            if(!World.Pedestrians.IsSeatAssignedToAnyone(VehicleAssigned, -1) && IsSeatAvailable(VehicleAssigned, -1))
            {
                VehicleAssigned = VehicleAssigned;
                SeatAssigned = -1;
            }
        }
        if(VehicleAssigned != null && SeatAssigned != -99)
        {
            World.Pedestrians.RemoveSeatAssignment(Ped);
            World.Pedestrians.AddSeatAssignment(Ped, VehicleAssigned, SeatAssigned);
            if (VehicleAssigned.Vehicle.Exists())
            {
                VehicleAssigned.Vehicle.LockStatus = Rage.VehicleLockStatus.Unlocked;
                VehicleAssigned.Vehicle.MustBeHotwired = false;
            }
        }
    }
    public void AssignPrisonerSeat()
    {
        VehicleAssigned = null;
        SeatAssigned = -99;
        foreach (VehicleExt possibleVehicle in VehiclesToTest.Where(x=> x != null && x.Vehicle.Exists() && x.Vehicle.Model.NumberOfSeats >= 4 && x.Vehicle.Speed < 0.5f).OrderBy(x=> x.Vehicle.DistanceTo2D(Ped.Pedestrian)))
        {
            float DistanceTo = possibleVehicle.Vehicle.DistanceTo2D(Ped.Pedestrian);
            if (DistanceTo <= 75f)
            {
                if (IsSeatAvailable(possibleVehicle, 1))
                {
                    VehicleAssigned = possibleVehicle;
                    SeatAssigned = 1;
                    
                    //EntryPoint.WriteToConsoleTestLong($"Prisoner Seat Assigned 1 Distance: {DistanceTo} Seat: {SeatAssigned}");
                    break;
                }
                else if (IsSeatAvailable(possibleVehicle, 2))
                {
                    VehicleAssigned = possibleVehicle;
                    SeatAssigned = 2;
                    //EntryPoint.WriteToConsoleTestLong($"Prisoner Seat Assigned 2 Distance: {DistanceTo} Seat: {SeatAssigned}");
                    break;
                }
            }
        }
        if (VehicleAssigned != null && SeatAssigned != -99)
        {
            World.Pedestrians.RemoveSeatAssignment(Ped);
            World.Pedestrians.AddSeatAssignment(Ped, VehicleAssigned, SeatAssigned);
        }
    }
    public bool IsAssignedPlayersVehicle(VehicleExt playersVehicle)
    {
        if(VehicleAssigned == null)
        {
            return false;
        }
        if(playersVehicle == null)
        {
            return false;
        }
        if(playersVehicle.Handle == VehicleAssigned.Handle)
        {
            return true;
        }
        return false;
    }

    private bool IsSeatAvailable(VehicleExt vehicleToCheck, int seatToCheck)
    {
        //if (vehicleToCheck != null && 
        //    vehicleToCheck.Vehicle.Exists() && 
        //    vehicleToCheck.Vehicle.IsSeatFree(seatToCheck) && 
        //    !World.Pedestrians.IsSeatAssigned(Ped, vehicleToCheck, seatToCheck) && 
        //    NativeFunction.Natives.x639431E895B9AA57<bool>(Ped.Pedestrian, vehicleToCheck.Vehicle, seatToCheck, false, true))
        //{
        //    return true;
        //}
        if (vehicleToCheck != null && Ped != null)
        {
            if (vehicleToCheck.Vehicle.Exists())
            {
                if (vehicleToCheck.Vehicle.IsSeatFree(seatToCheck))
                {
                    if (!World.Pedestrians.IsSeatAssigned(Ped, vehicleToCheck, seatToCheck))
                    {
                        if (Ped.Pedestrian.Exists())
                        {
                            if (NativeFunction.Natives.x639431E895B9AA57<bool>(Ped.Pedestrian, vehicleToCheck.Vehicle, seatToCheck, false, false))//if (NativeFunction.Natives.x639431E895B9AA57<bool>(Ped.Pedestrian, vehicleToCheck.Vehicle, seatToCheck, false, true))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        //IS_ENTRY_POINT_FOR_SEAT_CLEAR
        //GET_ENTRY_POINT_POSITION
        return false;
    }
    public bool HasPedsWaitingToEnter(VehicleExt vehicleToCheck, int seatToExclude)
    {
        if (vehicleToCheck != null && vehicleToCheck.Vehicle.Exists())
        {
            foreach(AssignedSeat assignedSeat in World.Pedestrians.GetPedsAssignedToVehicle(vehicleToCheck))
            {
                if(assignedSeat.Seat != seatToExclude)
                {
                    if(!assignedSeat.Ped.IsInVehicle)
                    {
                        return true;
                    }
                }
            }
        }
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
    }

    public bool IsAssignmentValid(bool checkDriver)
    {
        if(VehicleAssigned != null && SeatAssigned != -99 && VehicleAssigned.Vehicle.Exists() && Ped.Pedestrian.Exists())
        {
            if(VehicleAssigned.Vehicle.Speed >= 1.0f)
            {
                return false;
            }
            Ped onSeat = VehicleAssigned.Vehicle.GetPedOnSeat(SeatAssigned);
            if (onSeat.Exists() && onSeat.Handle != Ped.Pedestrian.Handle)
            {
                return false;
            }
            if(!checkDriver)
            {
                return true;
            }
            if(SeatAssigned != -1 && !World.Pedestrians.IsSeatAssignedToAnyone(VehicleAssigned,-1))//no driver and isnt assigned driver seat
            {
                return false;
            }
            return true;
        }
        return false;
    }

    public void AssignPlayerPassenger(VehicleExt playerVehicle)
    {
        VehicleAssigned = null;
        SeatAssigned = -99;
        if (playerVehicle == null || !playerVehicle.Vehicle.Exists())
        {
            EntryPoint.WriteToConsole("AssignPlayerPassenger player vehicle does not exists, returning");
            return;
        }
        int passengerCapacity = playerVehicle.Vehicle.PassengerCapacity;



        for (int testSeatIndex = 0;testSeatIndex < passengerCapacity; testSeatIndex++)
        {
            if(IsSeatAvailable(playerVehicle, testSeatIndex))
            {
                VehicleAssigned = playerVehicle;
                SeatAssigned = testSeatIndex;
                break;
            }
            EntryPoint.WriteToConsole($"AssignPlayerPassenger passengerCapacity{passengerCapacity} testSeatIndex{testSeatIndex}");
        }
        if (VehicleAssigned != null && SeatAssigned != -99)
        {
            World.Pedestrians.RemoveSeatAssignment(Ped);
            World.Pedestrians.AddSeatAssignment(Ped, VehicleAssigned, SeatAssigned);
            EntryPoint.WriteToConsole($"AssignPlayerPassenger veh{VehicleAssigned.Handle} SeatAssigned{SeatAssigned}");
        }
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

