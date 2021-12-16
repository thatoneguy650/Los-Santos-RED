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
        List<Merchant> MerchantList { get; }
        List<Cop> PoliceList { get; }
        bool AnyArmyUnitsSpawned { get; }
        bool AnyNooseUnitsSpawned { get; }
        bool AnyHelicopterUnitsSpawned { get; }
        PedExt GetPedExt(uint handle);
        VehicleExt GetVehicleExt(Vehicle vehicleTryingToEnter);
        VehicleExt GetVehicleExt(uint Handle);
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
        bool IsMPMapLoaded { get; }
        bool AnyWantedCiviliansNearPlayer { get; }
        List<VehicleExt> PoliceVehicleList { get; }
        int SpawnedPoliceVehicleCount { get; }
        string DebugString { get; }
        List<GameLocation> ActiveLocations { get; }

        void AddEntity(Blip blip);
        void AddEntity(PedExt pedExt);
        void AddEntity(VehicleExt x, ResponseType responseType);
        bool AnyCopsNearPosition(Vector3 initialPosition, float closestSpawnToOtherPoliceAllowed);
        void LoadMPMap();
        void LoadSPMap();
        VehicleExt GetClosestVehicleExt(Vector3 position, bool includePolice, float maxDistance);
        void ActivateLocation(GameLocation toSet);
    }
}
