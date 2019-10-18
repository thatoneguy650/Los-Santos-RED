using Instant_Action_RAGE.Entities.Weapon_Variation;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DroppedWeapon
{
    public WeaponDescriptor Weapon { get; set; }
    public Vector3 CoordinatedDropped { get; set; }
    public uint GameTimeDropped { get; set; }
    public int Tint { get; set; }
    public uint Hash { get; set; }

    public List<WeaponComponent> Components = new List<WeaponComponent>();
    public int Ammo { get; set; }

    public DroppedWeapon(WeaponDescriptor _Weapon, Vector3 _CoordinatedDropped)
    {
        Weapon = _Weapon;
        CoordinatedDropped = _CoordinatedDropped;
    }
}

