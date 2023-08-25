using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
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
    public bool IsDriverSide => DoorID == 0 || DoorID == 2;

    public Vector3 GetDoorOffset(Vehicle toCheck, ISettingsProvideable settings)
    {
        if(!toCheck.Exists())
        {
            return Vector3.Zero;
        }
        if(DoorID == 5)//Trunk
        {
            float length = toCheck.Model.Dimensions.Y;
            Vector3 DoorTogglePosition = toCheck.Position;
            return NativeHelper.GetOffsetPosition(DoorTogglePosition, toCheck.Heading + settings.SettingsManager.DoorToggleSettings.TrunkHeading, (-1 * length / 2) + settings.SettingsManager.DoorToggleSettings.TrunkOffset);
        }
        if (DoorID == 4)//Hood
        {
            float length = toCheck.Model.Dimensions.Y;
            Vector3 DoorTogglePosition = toCheck.Position;
            return NativeHelper.GetOffsetPosition(DoorTogglePosition, toCheck.Heading + settings.SettingsManager.DoorToggleSettings.HoodHeading, (length / 2) + settings.SettingsManager.DoorToggleSettings.HoodOffset);
        }
        return NativeFunction.Natives.GET_ENTRY_POINT_POSITION<Vector3>(toCheck, DoorID);    
    }
    public float GetDoorHeading(Vehicle toCheck, ISettingsProvideable settings)
    {
        if (!toCheck.Exists())
        {
            return 0f;
        }
        float DoorToggleHeading;
        if (DoorID == 5)//is trunk
        {
            DoorToggleHeading = toCheck.Heading;
        }
        else if (DoorID == 4)//is hood
        {
            DoorToggleHeading = toCheck.Heading - 180f;
        }
        else
        {
            if (IsDriverSide)
            {
                //EntryPoint.WriteToConsoleTestLong("DRIVER SIDE");
                DoorToggleHeading = toCheck.Heading - 90f;
            }
            else
            {
                //EntryPoint.WriteToConsoleTestLong("PASSENGER SIDE");
                DoorToggleHeading = toCheck.Heading + 90f;
            }
        }
        if (DoorToggleHeading >= 360f)
        {
            //EntryPoint.WriteToConsoleTestLong("TOO LARGE< LOWERING");
            DoorToggleHeading = DoorToggleHeading - 360f;
        }

        if(DoorToggleHeading < 0f)
        {
            DoorToggleHeading = 360f + DoorToggleHeading;
        }

        return DoorToggleHeading;
    }
}

