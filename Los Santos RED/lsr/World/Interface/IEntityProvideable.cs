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
        PedExt GetCivilian(uint handle);
        VehicleExt GetVehicle(Vehicle vehicleTryingToEnter);
        void ClearPolice();
        int PoliceHelicoptersCount { get; }
        int PoliceBoatsCount { get; }
        int TotalSpawnedCops { get; }
        void AddEntity(Cop x);
        void AddEntity(VehicleExt x);
        bool AnyCopsNearPosition(Vector3 initialPosition, float closestSpawnToOtherPoliceAllowed);
        int CountNearbyCops(Ped pedestrian);
    }
}
