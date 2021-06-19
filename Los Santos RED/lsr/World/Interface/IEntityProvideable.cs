using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IEntityProvideable
    {
        List<PedExt> CivilianList { get; }
        List<Cop> PoliceList { get; }
        bool AnyArmyUnitsSpawned { get; }
        bool AnyNooseUnitsSpawned { get; }
        bool AnyHelicopterUnitsSpawned { get; }
        PedExt GetPedExt(uint handle);
        VehicleExt GetVehicleExt(Vehicle vehicleTryingToEnter);
        void ClearSpawned();
        int PoliceHelicoptersCount { get; }
        int PoliceBoatsCount { get; }
        int TotalSpawnedFirefighters { get; }
        int TotalSpawnedEMTs { get; }
        int TotalSpawnedPolice { get; }
        List<Firefighter> FirefighterList { get; }
        List<EMT> EMTList { get; }
        int PoliceVehicleCount { get; }
        int CivilianVehicleCount { get; }
        void AddEntity(PedExt pedExt);
        void AddEntity(VehicleExt x);
        bool AnyCopsNearPosition(Vector3 initialPosition, float closestSpawnToOtherPoliceAllowed);
        int CountNearbyPolice(Ped pedestrian);
    }
}
