using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WeaponComponent
{
    public string Name { get; set; }
    public ulong Hash { get; private set; }
    public WeaponComponent()
    {

    }
    public WeaponComponent(string _Name, ulong _Hash)
    {
        Name = _Name;
        Hash = _Hash;
    }
    public WeaponComponent(string _Name)
    {
        Name = _Name;
    }
    public void UpdateHash(uint hash)
    {
        Hash = hash;
    }
}