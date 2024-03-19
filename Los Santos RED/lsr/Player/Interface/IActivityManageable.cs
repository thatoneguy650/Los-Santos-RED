using LosSantosRED.lsr.Player;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IActivityManageable
    {
        Ped Character { get; }
        bool IsInVehicle { get; } 
        bool IsDriver { get; }
        VehicleExt CurrentVehicle { get; }
        bool IsMovingDynamically { get; }
        bool IsMovingFast { get; }
        bool IsVisiblyArmed { get; }
        bool IsIncapacitated { get; }
        bool IsAliveAndFree { get; }
        PedExt CurrentLookedAtPed { get; }
        RelationshipManager RelationshipManager { get; }
        float VehicleSpeedMPH { get; }
        PedExt CurrentTargetedPed { get; }
        bool IsCop { get; }
        bool IsNotWanted { get; }
        GangMember CurrentLookedAtGangMember { get; }
        Investigation Investigation { get; }
        bool IsRagdoll { get; }
        bool IsStunned { get; }
        bool IsBreakingIntoCar { get; }
        bool IsGettingIntoAVehicle { get; }
        bool RecentlyGotOutOfVehicle { get; }
        WeaponEquipment WeaponEquipment { get; }
        HumanState HumanState { get; }
        HealthManager HealthManager { get; }
        Vehicle LastFriendlyVehicle { get; set; }
        Inventory Inventory { get; }
        bool CharacterModelIsFreeMode { get; }
        string FreeModeVoice { get; }
        GroupManager GroupManager { get; }
        GameLocation ClosestInteractableLocation { get; }
        string ModelName { get; }
        bool IsMale { get; }
       // List<LicensePlate> SpareLicensePlates { get; }
        bool IsOnFoot { get; }
        ButtonPrompts ButtonPrompts { get; }
        VehicleExt CurrentLookedAtVehicle { get; }
        bool IsMoveControlPressed { get; }
        bool IsEMT { get; }
        bool CanBustPeds { get; }
        int CurrentSeat { get; }
        string Gender { get; }
        CuffManager CuffManager { get; }

        void OnManuallyClosedDoor();
        void OnManuallyOpenedDoor();
        void PlaySpeech(string v1, bool v2);
    }
}
