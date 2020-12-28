using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IWorld
    {
        bool AnyArmyUnitsSpawned { get; }
        bool AnyCopsNearPlayer { get; }
        bool AnyHelicopterUnitsSpawned { get; }
        bool AnyNooseUnitsSpawned { get; }
        List<PedExt> CivilianList { get; }
        bool IsNight { get; }
        List<Cop> PoliceList { get; }
        int TotalSpawnedCops { get; }
        int PoliceHelicoptersCount { get; }
        int PoliceBoatsCount { get; }

        void AddEntity(Blip currentWantedCenterBlip);
        void ClearPolice();
        PedExt GetCivilian(uint handle);
        VehicleExt GetVehicle(Vehicle vehicleTryingToEnter);
        void PauseTime();
        void UnPauseTime();
        int CountNearbyCops(Ped pedestrian);
        bool AnyCopsNearPosition(Vector3 position, float closestSpawnToOtherPoliceAllowed);
    }
}
