using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IComplexTaskable: ISeatAssignable
    {
        bool IsBusted { get; set; }
        bool IsInVehicle { get; }
        bool IsInHelicopter { get; }
        bool IsInBoat { get; }
        float DistanceToPlayer { get; }
        bool IsDriver { get; }
        bool IsStill { get; }
        //Ped Pedestrian { get; }
        bool IsSuspicious { get; set; }
        //int LastSeatIndex { get; }
        List<WitnessedCrime> PlayerCrimesWitnessed { get; }
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
        bool IsArrested { get; set; }
        //uint Handle { get; }
        bool IsDrunk { get; set; }
        bool IsSpeeding { get; set; }
        bool IsDrivingRecklessly { get; set; }
        bool IsSuicidal { get; set; }
        VehicleExt AssignedVehicle { get; }
        //int AssignedSeat { get; }
        bool IsGangMember { get; }
        bool IsDealingDrugs { get; set; }
        bool HasMenu { get; }
        string VoiceName { get; }
      //  bool HasSeenDistressedPed { get; set; }
       // Vector3 PositionLastSeenDistressedPed { get; }
        bool IsOnBike { get; }
        //List<uint> BlackListedVehicles { get; }
        bool RecentlySeenPlayer { get; }
        bool IsLocationSpawned { get; }
      //  TaskRequirements TaskRequirements { get; }



        LocationTaskRequirements LocationTaskRequirements { get; }

        PlayerPerception PlayerPerception { get; }
        uint GameTimeReachedInvestigationPosition { get; set; }
        bool IsAnimal { get; }
        int DefaultCombatFlag { get; }
        int DefaultEnterExitFlag { get; }
        bool CanFlee { get; }
        //List<PedExt> BodiesSeen { get; }
        //Vector3 AlertedPoint { get; }
        //bool IsAlerted { get; }


        PedAlerts PedAlerts { get; }
        bool WillCower { get; }
        float CowerDistance { get; }
        bool HasCellPhone { get; }
        bool WillCallPolice { get; }
        bool WillCallPoliceIntense { get; }
        PedReactions PedReactions { get; }
        bool IsCowering { get; set; }
        bool CanSurrender { get; }
        PedViolations PedViolations { get; }
        bool ShouldSurrender { get; set; }
        bool IsInPlane { get; }
      //  bool IsAssignedToHover { get; set; }

        void ControlLandingGear();
        void PlaySpeech(string name, bool v);
        void PlaySpeech(List<string> list, bool isInVehicle, bool v);
        void ReportCrime(ITargetable player);
        void SetWantedLevel(int v);
    }
}
