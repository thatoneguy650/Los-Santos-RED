﻿using LosSantosRED.lsr.Interface;
using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSR.Vehicles
{
    public class Windows
    {
        private VehicleExt VehicleToMonitor;
        private ISettingsProvideable Settings;
        public List<Window> WindowList { get; set; } = new List<Window>();
        public Windows(VehicleExt vehicleToMonitor, ISettingsProvideable settings)
        {
            VehicleToMonitor = vehicleToMonitor;
            Settings = settings;
        }
        public void SetWindows(bool IsRolledUp)
        {
            CreateWindowList();
            foreach(Window window in WindowList)
            {
                window.SetState(IsRolledUp);
            }
        }
        public void ToggleWindow(int windowID)
        {
            Window toSet = GetOrCreate(windowID);
            if (toSet == null)
            {
                EntryPoint.WriteToConsole($"WINDOW IS NULL TOGGLE {windowID}");
                return;
            }
            toSet.Toggle();
        }
        public void SetState(int windowID, bool IsRolledUp)
        {
            Window toSet = GetOrCreate(windowID);
            if (toSet == null)
            {
                EntryPoint.WriteToConsole($"WINDOW IS NULL SET STATE {windowID} IsRolledUp {IsRolledUp}");
                return;
            }
            toSet.SetState(IsRolledUp);
        }
        private Window GetOrCreate(int windowID)
        {
            Window toSet = WindowList.FirstOrDefault(x => x.ID == windowID);
            if (toSet == null)
            {
                toSet = new Window(VehicleToMonitor, windowID);
                WindowList.Add(toSet);
                EntryPoint.WriteToConsole($"CREATE WINDOW windowID:{windowID}");
            }
            return toSet;
        }

        public void CreateWindowList()
        {
            if(VehicleToMonitor == null || !VehicleToMonitor.Vehicle.Exists())
            {
                return;
            }
            for (int i = 0; i < 8; i++)
            {
                if (NativeFunction.Natives.IS_VEHICLE_WINDOW_INTACT<bool>(VehicleToMonitor.Vehicle, i))
                {
                    GetOrCreate(i);
                }
            }
        }
    }
}