using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSR.Vehicles
{
    public class Door
    {
        private VehicleExt VehicleToMonitor;
        public Door(VehicleExt vehicleToMonitor, int iD)
        {
            VehicleToMonitor = vehicleToMonitor;
            ID = iD;
        }
        public int ID { get; set; }
        public bool IsClosed { get; set; } = true;

        public void Toggle(IActivityManageable player)
        {
            SetState(!IsClosed, player);
        }
        public void SetState(bool Close, IActivityManageable player)
        {
            if (VehicleToMonitor == null || !VehicleToMonitor.Vehicle.Exists())
            {
                return;
            }
            bool isValid = NativeFunction.Natives.x645F4B6E8499F632<bool>(VehicleToMonitor.Vehicle, ID);
            if (!isValid)
            {
                return;
            }
            if (Close)
            {
                if (!IsClosed)
                {
                    NativeFunction.Natives.SET_VEHICLE_DOOR_SHUT(VehicleToMonitor.Vehicle, ID, false, false);
                    IsClosed = true;
                    EntryPoint.WriteToConsole($"SET_VEHICLE_DOOR_SHUT 1 {ID} IsClosed{IsClosed}");
                    player.OnManuallyClosedDoor();
                }
            }
            else
            {
                NativeFunction.Natives.SET_VEHICLE_DOOR_OPEN(VehicleToMonitor.Vehicle, ID, false, false);
                IsClosed = false;
                EntryPoint.WriteToConsole($"SET_VEHICLE_DOOR_OPEN {ID} IsClosed{IsClosed}");
                GameFiber.Sleep(750);
                if (VehicleToMonitor.Vehicle.Exists())
                {
                    NativeFunction.Natives.SET_VEHICLE_DOOR_OPEN(VehicleToMonitor.Vehicle, ID, true, false);
                }
                player.OnManuallyOpenedDoor();
            }
        }
        public float GetDoorState()
        {
            bool isValid = NativeFunction.Natives.x645F4B6E8499F632<bool>(VehicleToMonitor.Vehicle, ID);
            if (!isValid)
            {
                return -1.0f;
            }
            float DoorAngle = NativeFunction.Natives.GET_VEHICLE_DOOR_ANGLE_RATIO<float>(VehicleToMonitor.Vehicle, ID);
            IsClosed = DoorAngle <= 0.0f;
            return DoorAngle;
        }

    }
}
