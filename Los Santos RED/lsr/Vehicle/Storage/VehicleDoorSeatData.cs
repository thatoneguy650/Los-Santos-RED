using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class VehicleDoorSeatData
{
    public VehicleDoorSeatData(string seatName, string seatBone, int doorID, int seatID)
    {
        SeatName = seatName;
        SeatBone = seatBone;
        DoorID = doorID;
        SeatID = seatID;
    }

    public string SeatName { get; set; }
    public string SeatBone { get; set; }
    public int DoorID { get; set; }
    public int SeatID { get; set; }
}

