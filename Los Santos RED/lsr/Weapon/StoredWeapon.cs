
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class StoredWeapon
{
    public uint WeaponHash { get; set; }
    public Vector3 CoordinatedDropped { get; set; }
    public WeaponVariation Variation { get; set; }
    public int Ammo { get; set; }
    public StoredWeapon(uint _WeaponHash, Vector3 _CoordinatedDropped, WeaponVariation _Variation,int _Ammo)
    {
        WeaponHash = _WeaponHash;
        CoordinatedDropped = _CoordinatedDropped;
        Variation = _Variation;
        Ammo = _Ammo;
    }

    public StoredWeapon()
    {
    }
}

