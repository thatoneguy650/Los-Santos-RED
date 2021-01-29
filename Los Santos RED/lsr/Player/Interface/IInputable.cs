using LSR.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IInputable
    {
        bool IsHoldingEnter { get; set; }
        bool IsBusted { get; }
        bool CanSurrender { get; }
        bool HandsAreUp { get; set; }
        bool CanDropWeapon { get; }
        VehicleExt CurrentVehicle { get; }
        bool IsInVehicle { get; }
        bool IsMoveControlPressed { get; set; }
        bool CanConverseWithLookedAtPed { get; }
        bool CanHoldUpTargettedPed { get; }
        bool IsConversing { get; }
        List<ButtonPrompt> ButtonPrompts { get; }
        bool IsInteracting { get; }
        void RaiseHands();
        void LowerHands();
        void DropWeapon();
        void SetUnarmed();
        void StartConversation();
    }
}
