using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WeaponComponent
{
    public string Name;
    public string HashKey;
    public ulong Hash;
    public string BaseWeapon;
    public bool Enabled;
    public WeaponComponent()
    {

    }
    public WeaponComponent(string _Name, string _HashKey, ulong _Hash, bool _Enabled)
    {
        Name = _Name;
        HashKey = _HashKey;
        Hash = _Hash;
        Enabled = _Enabled;
    }
    public WeaponComponent(string _Name, string _HashKey, ulong _Hash, bool _Enabled, string _BaseWeapon)
    {
        Name = _Name;
        HashKey = _HashKey;
        Hash = _Hash;
        Enabled = _Enabled;
        BaseWeapon = _BaseWeapon;
    }
}