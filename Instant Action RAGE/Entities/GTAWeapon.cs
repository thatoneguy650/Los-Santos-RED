using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class GTAWeapon
    {
    public GTAWeapon(WeaponHash _Type, short _AmmoAmount, string _Category, int _WeaponLevel)
    {
        Type = _Type;
        AmmoAmount = _AmmoAmount;
        Category = _Category;
        WeaponLevel = _WeaponLevel;
    }
    public WeaponHash Type;
    public short AmmoAmount;
    public string Category;
    public int WeaponLevel;
}

