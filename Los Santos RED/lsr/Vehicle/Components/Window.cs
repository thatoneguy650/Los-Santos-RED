using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSR.Vehicles
{
    public class Window
    {
        private VehicleExt VehicleToMonitor;
        public Window(VehicleExt vehicleToMonitor, int iD)
        {
            VehicleToMonitor = vehicleToMonitor;
            ID = iD;
        }

        public int ID { get; set; }
        public bool IsRolledUp { get; set; } = true;
        public void Toggle()
        {
            SetState(!IsRolledUp);
        }
        public void SetState(bool RollUp)
        {
            if (VehicleToMonitor == null || !VehicleToMonitor.Vehicle.Exists())
            {
                return;
            }


            if (RollUp)
            {
                NativeFunction.CallByName<bool>("ROLL_UP_WINDOW", VehicleToMonitor.Vehicle, ID);
                IsRolledUp = true;
                EntryPoint.WriteToConsole($"ROLL_UP_WINDOW 1 {ID} IsRolledUp{IsRolledUp}");
            }
            else
            {
                NativeFunction.CallByName<bool>("ROLL_DOWN_WINDOW", VehicleToMonitor.Vehicle, ID);
                IsRolledUp = false;
                EntryPoint.WriteToConsole($"ROLL_DOWN_WINDOW {ID} IsRolledUp{IsRolledUp}");
            }



            //if (NativeFunction.CallByName<bool>("IS_VEHICLE_WINDOW_INTACT", VehicleToMonitor.Vehicle, ID))
            //{

            //}
            //else if (!RollUp)
            //{
            //    NativeFunction.CallByName<bool>("ROLL_DOWN_WINDOW", VehicleToMonitor.Vehicle, ID);
            //    IsRolledUp = false;
            //    EntryPoint.WriteToConsole($"ROLL_DOWN_WINDOW 2 {ID} IsRolledUp{IsRolledUp}");
            //}
        }
    }
}
