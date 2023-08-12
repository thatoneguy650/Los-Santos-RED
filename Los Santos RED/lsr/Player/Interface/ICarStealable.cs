using LosSantosRED.lsr.Player;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface ICarStealable
    {
        bool IsLockPicking { get; set; }
        bool IsVisiblyArmed { get; }
        bool IsBusted { get; }
        bool IsDead { get; }
        bool IsCarJacking { get; set; }
        bool IsMoveControlPressed { get; }
        WeaponEquipment WeaponEquipment { get; }
        void ShootAt(Vector3 targetCoordinate);
        Ped Character { get; }
        Inventory Inventory { get; }
        ActivityManager ActivityManager { get; }
    }
}
