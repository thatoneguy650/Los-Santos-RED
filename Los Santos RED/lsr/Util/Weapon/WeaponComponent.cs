using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WeaponComponent
{
    private uint Hash;
    public string Name { get; set; }
    
    public WeaponComponent()
    {

    }
    public WeaponComponent(string _Name, uint _Hash)
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
    public uint GetHash()
    {
        return Hash;
    }
}