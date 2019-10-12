using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class GTAWeapon
    {
    public GTAWeapon(String _Name, short _AmmoAmount, string _Category, int _WeaponLevel, ulong _Hash)
    {
        Name = _Name;
        AmmoAmount = _AmmoAmount;
        Category = _Category;
        WeaponLevel = _WeaponLevel;
        Hash = _Hash;
    }
    public string Name;
    public short AmmoAmount;
    public string Category;
    public int WeaponLevel;
    public ulong Hash;
    public string ScannerFile;
}

