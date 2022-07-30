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
    public interface ILocationInteractable //: IActivityPerformable,IActionable
    {
        ButtonPrompts ButtonPrompts { get; }
        BankAccounts BankAccounts { get; }
        Inventory Inventory { get; }
        Properties Properties { get; }
        bool IsInteractingWithLocation { get; set; }
        Ped Character { get; }
        bool CharacterModelIsFreeMode { get; }
        bool CanConverse { get; }
        string FreeModeVoice { get; }
        Vector3 Position { get; }
        bool IsTransacting { get; set; }
        bool IsMoveControlPressed { get; }
        bool IsDoingSuspiciousActivity { get; set; }
        bool CanPerformActivities { get; }
        WeaponEquipment WeaponEquipment { get; }
        VehicleOwnership VehicleOwnership { get; }
        GunDealerRelationship GunDealerRelationship { get; }
        bool IsPerformingActivity { get; }
        Licenses Licenses { get; }
        string PlayerName { get; }
        bool IsConversing { get; set; }
        bool IsAliveAndFree { get; }
        bool IsInVehicle { get; }
        float VehicleSpeedMPH { get; }
        bool RecentlyCrashedVehicle { get; }
        Vehicle LastFriendlyVehicle { get; set; }
        VehicleExt CurrentVehicle { get; }
        bool IsResting { get; set; }
        bool IsSleeping { get; set; }
        GangRelationships GangRelationships { get; }
        bool IsDealingDrugs { get; set; }
        bool IsDealingIllegalGuns { get; set; }
        bool IsNotWanted { get; }
        bool IsWanted { get; }
        PlayerTasks PlayerTasks { get; }

        void StopDynamicActivity();
        void StartConsumingActivity(ModItem modItem, bool v);
        void ChangeName(string newName);
        void SetWantedLevel(int v1, string v2, bool v3);
    }
}
