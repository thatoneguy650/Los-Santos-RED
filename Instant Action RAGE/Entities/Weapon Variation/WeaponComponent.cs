using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instant_Action_RAGE.Entities.Weapon_Variation
{
    public class WeaponComponent
    {
        public WeaponComponent(uint _ComponentID,bool _Enabled)
        {
            ComponentID = _ComponentID;
            Enabled = _Enabled;
        }
        public uint ComponentID { get; set; }
        public bool Enabled { get; set; }
    }
}
