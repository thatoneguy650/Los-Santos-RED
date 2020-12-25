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
        // Vector3 PlacePoliceLastSeenPlayer { get; }
        bool AnyHelicopterUnitsSpawned { get; }
        bool AnyNooseUnitsSpawned { get; }
        List<PedExt> CivilianList { get; }
        //bool AnyPoliceCanSeePlayer { get; }
        // bool AnyPoliceCanRecognizePlayer { get; }
        //bool ShouldBustPlayer { get; }
        //bool AnyPoliceRecentlySeenPlayer { get; }
        //bool AnyPoliceSeenPlayerCurrentWanted { get; }
        //bool AnyPoliceCanHearPlayer { get; }
        // bool AnyHumansNearPlayer { get; }
        bool IsNight { get; }
       // Vector3 PlaceLastSeenPlayer { get; }
        List<Cop> PoliceList { get; }
        int TotalSpawnedCops { get; }
        bool AnyHumansNearPlayer { get; }

        // void ResetPolice();
        void AddBlip(Blip currentWantedCenterBlip);
        void ClearPolice();
        PedExt GetCivilian(uint handle);
        //void ResetWitnessedCrimes();
        VehicleExt GetVehicle(Vehicle vehicleTryingToEnter);
        void PauseTime();
        void UnPauseTime();
        int CountNearbyCops(Ped pedestrian);

        //void Delete(Cop outOfRangeCop);
        //void SpawnCop(Agency agencyToSpawn, Vector3 finalSpawnPosition, float heading, VehicleInformation agencyVehicle);
    }
}
