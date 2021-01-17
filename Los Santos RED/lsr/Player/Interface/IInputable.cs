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
        bool IsMoveControlPressed { get; }
        bool CanConverse { get; }
        bool CanHoldUp { get; }
        bool IsConversing { get; }
        List<ButtonPrompt> ButtonPrompts { get; }

        void RaiseHands();
        void LowerHands();
        void DropWeapon();
        void SetUnarmed();
    }
}
