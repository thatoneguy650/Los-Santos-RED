
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class WeaponExt
{
    public WeaponDescriptor Weapon { get; set; }
    public Vector3 CoordinatedDropped { get; set; }
    public uint GameTimeDropped { get; set; }
    public int Tint { get; set; }

    public GTAWeapon.WeaponVariation Variation { get; set; }
    public int Ammo { get; set; }

    public WeaponExt(WeaponDescriptor _Weapon, Vector3 _CoordinatedDropped)
    {
        Weapon = _Weapon;
        CoordinatedDropped = _CoordinatedDropped;
    }
    public WeaponExt(WeaponDescriptor _Weapon, Vector3 _CoordinatedDropped, GTAWeapon.WeaponVariation _Variation)
    {
        Weapon = _Weapon;
        CoordinatedDropped = _CoordinatedDropped;
        Variation = _Variation;
    }
    public WeaponExt(WeaponDescriptor _Weapon, Vector3 _CoordinatedDropped, GTAWeapon.WeaponVariation _Variation,int _Ammo)
    {
        Weapon = _Weapon;
        CoordinatedDropped = _CoordinatedDropped;
        Variation = _Variation;
        Ammo = _Ammo;
    }
}

