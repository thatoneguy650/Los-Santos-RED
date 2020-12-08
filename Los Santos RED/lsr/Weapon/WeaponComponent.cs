using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WeaponComponent
{
    public string Name { get; set; }
    public ulong Hash { get; set; }
    public string BaseWeapon { get; set; }
    public WeaponComponent()
    {

    }
    public WeaponComponent(string _Name, ulong _Hash, string _BaseWeapon)
    {
        Name = _Name;
        Hash = _Hash;
        BaseWeapon = _BaseWeapon;
    }
}