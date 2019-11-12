
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

    public WeaponVariation Variation { get; set; }
    public int Ammo { get; set; }

    public DroppedWeapon(WeaponDescriptor _Weapon, Vector3 _CoordinatedDropped)
    {
        Weapon = _Weapon;
        CoordinatedDropped = _CoordinatedDropped;
    }
    public DroppedWeapon(WeaponDescriptor _Weapon, Vector3 _CoordinatedDropped, WeaponVariation _Variation)
    {
        Weapon = _Weapon;
        CoordinatedDropped = _CoordinatedDropped;
        Variation = _Variation;
    }
    public DroppedWeapon(WeaponDescriptor _Weapon, Vector3 _CoordinatedDropped, WeaponVariation _Variation,int _Ammo)
    {
        Weapon = _Weapon;
        CoordinatedDropped = _CoordinatedDropped;
        Variation = _Variation;
        Ammo = _Ammo;
    }
}

