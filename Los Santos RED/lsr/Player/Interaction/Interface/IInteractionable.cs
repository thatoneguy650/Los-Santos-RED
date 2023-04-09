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
    public interface IInteractionable
    {
        ButtonPrompts ButtonPrompts { get; }
        CellPhone CellPhone { get; }
        WeaponEquipment WeaponEquipment { get; }
        RelationshipManager RelationshipManager { get; }
        Inventory Inventory { get; }
        VehicleOwnership VehicleOwnership { get; }
        BankAccounts BankAccounts { get; }
        ActivityManager ActivityManager { get; }
      //  bool CanConverse { get; }
        //bool CanPerformActivities { get; }
        Ped Character { get; }
        bool CharacterModelIsFreeMode { get; }
        PedExt CurrentTargetedPed { get; }
        string FreeModeVoice { get; }
        bool IsAliveAndFree { get; }
        bool IsCarJacking { get; set; }
      //  bool IsConversing { get; set; }
        bool IsDealingDrugs { get; set; }
        bool IsDealingIllegalGuns { get; set; }
      //  bool IsDraggingBody { get; set; }
      //  bool IsHoldingHostage { get; set; }
      //  bool IsHoldingUp { get; set; }
        bool IsIncapacitated { get; }
        bool IsInVehicle { get; }

        // bool IsLootingBody { get; set; }
        bool IsMoveControlPressed { get; }
       // bool IsPerformingActivity { get; }
     //   bool IsSitting { get; }
        bool IsTransacting { get; set; }
        Stance Stance { get; }
        bool IsShowingActionWheel { get; }

        void SetAngeredCop();
    }
}
