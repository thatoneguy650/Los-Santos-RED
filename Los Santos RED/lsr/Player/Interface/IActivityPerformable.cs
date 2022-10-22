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
        ButtonPrompts ButtonPrompts { get; }
        CellPhone CellPhone { get; }
        WeaponEquipment WeaponEquipment { get; }
        RelationshipManager RelationshipManager { get; }
        Inventory Inventory { get; }
        Licenses Licenses { get; }
        PlayerTasks PlayerTasks { get; }
        Properties Properties { get; }
        Violations Violations { get; }
        VehicleOwnership VehicleOwnership { get; }
        BankAccounts BankAccounts { get; }
      //  bool CanConverse { get; }
       // bool CanPerformActivities { get; }
        Ped Character { get; }
        bool CharacterModelIsFreeMode { get; }
        VehicleExt CurrentVehicle { get; }
        string FreeModeVoice { get; }
        bool IsAliveAndFree { get; }
      //  bool IsConversing { get; set; }
        bool IsDealingDrugs { get; set; }
        bool IsDealingIllegalGuns { get; set; }
        bool IsDoingSuspiciousActivity { get; set; }
        bool IsDriver { get; }
     //   bool IsInteractingWithLocation { get; set; }
        bool IsInVehicle { get; }
        bool IsMale { get; }
        bool IsMoveControlPressed { get; }
        bool IsNotWanted { get; }
       // bool IsPerformingActivity { get; set; }
        bool IsResting { get; set; }
        bool IsSleeping { get; set; }
        bool IsTransacting { get; set; }
        bool IsWanted { get; }
        Vehicle LastFriendlyVehicle { get; set; }
        string ModelName { get; }
        string PlayerName { get; }
        Vector3 Position { get; }
        bool RecentlyCrashedVehicle { get; }
        float VehicleSpeedMPH { get; }

        ActivityManager ActivityManager { get; }

        void ChangeName(string newName);
       // void EnterVehicleAsPassenger(bool withBlocking);
        void SetWantedLevel(int v1, string v2, bool v3);
      //  void StartConsumingActivity(ModItem modItem, bool v);
      //  void StopDynamicActivity();

    }
}
