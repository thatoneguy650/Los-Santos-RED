using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IComplexTaskable
    {
        bool IsBusted { get; set; }
        bool IsInVehicle { get; }
        bool IsInHelicopter { get; }
        bool IsInBoat { get; }
        float DistanceToPlayer { get; }
        bool IsDriver { get; }
        bool IsStill { get; }
        Ped Pedestrian { get; }
        int LastSeatIndex { get; }
        List<Crime> PlayerCrimesWitnessed { get; }
        List<WitnessedCrime> OtherCrimesWitnessed { get; }
        VehicleExt VehicleLastSeenPlayerIn { get; }
        WeaponInformation WeaponLastSeenPlayerWith { get; }
        bool EverSeenPlayer { get; }
        float ClosestDistanceToPlayer { get; }
        Vector3 PositionLastSeenCrime { get; }
        bool IsCop { get; }
        ComplexTask CurrentTask { get; }
        bool IsRunningOwnFiber { get; set; }
        int WantedLevel { get; }
        bool IsMovingFast { get; }
        bool RecentlyGotOutOfVehicle { get; }
        bool RecentlyGotInVehicle { get; }
        bool IsArrested { get; }
        uint Handle { get; }

        void SetWantedLevel(int v);
    }
}
