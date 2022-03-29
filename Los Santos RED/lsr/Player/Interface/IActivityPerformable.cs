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
    public interface IActivityPerformable
    {
        bool IsPerformingActivity { get; set; }
        Ped Character { get; }
        string ModelName { get; }
        bool IsMale { get; }
        bool CanPerformActivities { get; }
        GangRelationships GangRelationships { get; }
        int Money { get; }

        void GiveMoney(int moneyToReceive);
        CellPhone CellPhone { get; }
        bool IsMoveControlPressed { get; }
        string FreeModeVoice { get; }
        bool CharacterModelIsFreeMode { get; }
        bool CanConverse { get; }
        bool IsInteractingWithLocation { get; set; }
        List<ButtonPrompt> ButtonPromptList { get; }
        Inventory Inventory { get; }
        PlayerTasks PlayerTasks { get; }
        bool IsDoingSuspiciousActivity { get; set; }
        GunDealerRelationship GunDealerRelationship { get; }
        string PlayerName { get; }
        Licenses Licenses { get; }
        Properties Properties { get; }
        bool IsConversing { get; set; }
        bool IsInVehicle { get; }
        bool IsDealingDrugs { get; set; }
        bool IsDealingIllegalGuns { get; set; }
        bool IsTransacting { get; set; }
        VehicleExt CurrentVehicle { get; }
        Vehicle LastFriendlyVehicle { get; set; }
        Violations Violations { get; }
        bool IsDriver { get; }
        List<VehicleExt> OwnedVehicles { get; }
        Vector3 Position { get; }
        float VehicleSpeedMPH { get; }
        bool RecentlyCrashedVehicle { get; }

        void SetUnarmed();
        void StopDynamicActivity();
        void TakeOwnershipOfVehicle(VehicleExt myNewCar, bool showNotification);
        void ConsumeItem(ModItem modItem);
        void ChangeName(string newName);
        void EnterVehicleAsPassenger();
        void RemoveOwnershipOfVehicle(VehicleExt myNewCar);
        ButtonPrompts ButtonPrompts { get; }
        bool IsWanted { get; }
        bool IsNotWanted { get; }

        void SetWantedLevel(int v1, string v2, bool v3);
    }
}
