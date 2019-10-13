using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instant_Action_RAGE.Entities.Weapon_Variation
{
    public class ModdedWeapon
    {
        public ModdedWeapon(uint _Hash, int _Tint)
        {
            Hash = _Hash;
            Tint = _Tint;
            GetComponents();
        }
        public ModdedWeapon(uint _Hash)
        {
            Hash = _Hash;
            NativeFunction.CallByName<int>("GET_PED_WEAPON_TINT_INDEX", Game.LocalPlayer.Character, Hash);
            GetComponents();
        }

        private void GetComponents()
        {
           
        }

        public int Tint { get; set; }
        public uint Hash { get; set; }
        public List<WeaponComponent> Components = new List<WeaponComponent>();
}
}
