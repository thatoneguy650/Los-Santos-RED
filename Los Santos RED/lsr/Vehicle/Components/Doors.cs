using LosSantosRED.lsr.Interface;
using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSR.Vehicles
{
    public class Doors
    {
        private VehicleExt VehicleToMonitor;
        private ISettingsProvideable Settings;
        public List<Door> DoorList { get; set; } = new List<Door>();
        public bool IsLocked { get; set; } = false;
        public Doors(VehicleExt vehicleToMonitor, ISettingsProvideable settings)
        {
            VehicleToMonitor = vehicleToMonitor;
            Settings = settings;
        }
        public void Toggle(int doorID, IActivityManageable player)
        {
            Door toSet = GetOrCreate(doorID);
            if (toSet == null)
            {
                EntryPoint.WriteToConsole($"WINDOW IS NULL TOGGLE {doorID}");
                return;
            }
            toSet.Toggle(player);
        }
        public void SetState(int doorID, bool IsClosed, IActivityManageable player)
        {
            Door toSet = GetOrCreate(doorID);
            if (toSet == null)
            {
                EntryPoint.WriteToConsole($"DOOR IS NULL SET STATE {doorID} IsClosed {IsClosed}");
                return;
            }
            toSet.SetState(IsClosed, player);
        }
        private Door GetOrCreate(int doorID)
        {
            Door toSet = DoorList.FirstOrDefault(x => x.ID == doorID);
            if (toSet == null)
            {
                toSet = new Door(VehicleToMonitor, doorID);
                DoorList.Add(toSet);
            }
            return toSet;
        }

        public float GetDoorAngle(int doorID)
        {
            Door toSet = GetOrCreate(doorID);
            if (toSet == null)
            {
                EntryPoint.WriteToConsole($"DOOR IS NULL SET STATE {doorID}");
                return -1.0f;
            }
            return toSet.GetDoorState();
        }
        public void ToggleDoorLocks()
        {
            SetDoorLocks(!IsLocked);
        }
        public void SetDoorLocks(bool isLocked)
        {
            if (VehicleToMonitor == null || !VehicleToMonitor.Vehicle.Exists())
            {
                return;
            }
            int lockState = isLocked ? 7 : 1;
            NativeFunction.Natives.SET_VEHICLE_DOORS_LOCKED(VehicleToMonitor.Vehicle, lockState);
            IsLocked = isLocked;
            Game.DisplaySubtitle($"Doors {(IsLocked ? "Locked" : "Unlocked")}");
        }
    }
}