using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WeaponVariation
{
    public string Name;
    public int Tint;
    public List<string> Components = new List<string>();
    public WeaponVariation()
    {

    }
    public WeaponVariation(string _name, int _Tint, List<string> _Components)
    {
        Name = _name;
        Tint = _Tint;
        Components = _Components;
    }
    public WeaponVariation(string _name, int _Tint)
    {
        Name = _name;
        Tint = _Tint;
    }
    public WeaponVariation(int _Tint, List<string> _Components)
    {
        Tint = _Tint;
        Components = _Components;
    }
}