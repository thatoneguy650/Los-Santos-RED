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
        bool IsHoldingUp { get; set; }
        WeaponCategory CurrentWeaponCategory { get; }
        bool IsConversing { get; set; }
        Ped Character { get; }
        PedExt CurrentTargetedPed { get; }
        bool IsAliveAndFree { get; }
    }
}
