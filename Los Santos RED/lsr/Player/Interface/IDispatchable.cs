using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IDispatchable
    {
        bool IsWanted { get; }
        int WantedLevel { get; }
        bool AnyPoliceRecentlySeenPlayer { get; }
        Investigation Investigation { get; }
        Vector3 Position { get; }
        bool IsInVehicle { get; }
        Ped Character { get; }
        Locations.LocationData CurrentLocation { get; }
        VehicleExt CurrentVehicle { get; }
        PoliceResponse PoliceResponse { get; }
        bool IsNotWanted { get; }
        Respawning Respawning { get; }
        bool IsCustomizingPed { get; }
        uint Handle { get; }

        void OnLawEnforcementSpawn(Agency agency, DispatchableVehicle vehicleType, DispatchablePerson officerType);
    }
}
