using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WeaponVariation
{
    public int Tint;
    public List<WeaponComponent> Components = new List<WeaponComponent>();
    public WeaponVariation()
    {

    }
    public WeaponVariation(int _Tint, List<WeaponComponent> _Components)
    {
        Tint = _Tint;
        Components = _Components;
    }
    public WeaponVariation(int _Tint)
    {
        Tint = _Tint;
    }
    public WeaponVariation(List<WeaponComponent> _Components)
    {
        Components = _Components;
    }
}