using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WeaponComponent
{
    public uint Hash { get; set; }
    public string Name { get; set; }
    public ComponentSlot ComponentSlot { get; set; }
    public WeaponComponent()
    {

    }
    public WeaponComponent(string _Name, uint _Hash, ComponentSlot _ComponentSlot)
    {
        Name = _Name;
        Hash = _Hash;
        ComponentSlot = _ComponentSlot;
    }
    public WeaponComponent(string _Name)
    {
        Name = _Name;
    }
}