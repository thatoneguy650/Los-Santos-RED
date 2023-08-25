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
    public interface IInteractionable //: ILocationInteractable
    {
        ButtonPrompts ButtonPrompts { get; }
        CellPhone CellPhone { get; }
        WeaponEquipment WeaponEquipment { get; }
        RelationshipManager RelationshipManager { get; }
        Inventory Inventory { get; }
        VehicleOwnership VehicleOwnership { get; }
        BankAccounts BankAccounts { get; }
        ActivityManager ActivityManager { get; }
        Ped Character { get; }
        bool CharacterModelIsFreeMode { get; }
        PedExt CurrentTargetedPed { get; }
        string FreeModeVoice { get; }
        bool IsAliveAndFree { get; }
        bool IsCarJacking { get; set; }
        bool IsDealingDrugs { get; set; }
        bool IsDealingIllegalGuns { get; set; }
        bool IsIncapacitated { get; }
        bool IsInVehicle { get; }
        bool IsMoveControlPressed { get; }
        bool IsTransacting { get; set; }
        Stance Stance { get; }
        bool IsShowingActionWheel { get; }
        bool IsEMT { get; }
        bool IsCop { get; }


        void PlaySpeech(string v1, bool v2);
        void SetAngeredCop();
    }
}
