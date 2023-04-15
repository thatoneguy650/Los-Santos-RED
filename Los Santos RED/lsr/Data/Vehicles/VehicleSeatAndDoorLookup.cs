using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class VehicleSeatAndDoorLookup : IVehicleSeatAndDoorLookup
{

    public List<VehicleDoorSeatData> VehicleDoorSeatDataList { get; set; }
    public VehicleSeatAndDoorLookup()
    {

    }
    public void ReadConfig()
    {
        VehicleDoorSeatDataList = new List<VehicleDoorSeatData>()
        {
            new VehicleDoorSeatData("Trunk", "boot", 5, -2),
            new VehicleDoorSeatData("Front Driver", "seat_dside_f", 0, -1),
            new VehicleDoorSeatData("Front Passenger", "seat_pside_f", 1, 0),
            new VehicleDoorSeatData("Rear Driver", "seat_dside_r", 2, 1),
            new VehicleDoorSeatData("Rear Passenger", "seat_pside_r", 3, 2),
        };
    }
    public int GetDoorID(string seatBone)
    {
        VehicleDoorSeatData vdsd = VehicleDoorSeatDataList.FirstOrDefault(x => x.SeatBone == seatBone);
        if(vdsd == null)
        {
            return -99;
        }
        return vdsd.DoorID;
    }
    public int GetSeatID(string seatBone)
    {
        VehicleDoorSeatData vdsd = VehicleDoorSeatDataList.FirstOrDefault(x => x.SeatBone == seatBone);
        if (vdsd == null)
        {
            return -99;
        }
        return vdsd.SeatID;
    }
    public string GetSeatName(string seatBone)
    {
        VehicleDoorSeatData vdsd = VehicleDoorSeatDataList.FirstOrDefault(x => x.SeatBone == seatBone);
        if (vdsd == null)
        {
            return "Unknown";
        }
        return vdsd.SeatName;
    }
}

