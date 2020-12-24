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
    public void ApplyWeaponVariation(Ped WeaponOwner, uint WeaponHash)
    {
        NativeFunction.CallByName<bool>("SET_PED_WEAPON_TINT_INDEX", WeaponOwner, WeaponHash, Tint);
        WeaponInformation LookupGun = DataMart.Instance.Weapons.GetWeapon(WeaponHash);//Weapons.Where(x => x.Hash == WeaponHash).FirstOrDefault();
        if (LookupGun == null)
            return;
        foreach (WeaponComponent ToRemove in LookupGun.PossibleComponents)
        {
            NativeFunction.CallByName<bool>("REMOVE_WEAPON_COMPONENT_FROM_PED", WeaponOwner, WeaponHash, ToRemove.Hash);
        }
        foreach (string ToAdd in Components)
        {
            WeaponComponent MyComponent = LookupGun.PossibleComponents.Where(x => x.Name == ToAdd).FirstOrDefault();
            if (MyComponent != null)
                NativeFunction.CallByName<bool>("GIVE_WEAPON_COMPONENT_TO_PED", WeaponOwner, WeaponHash, MyComponent.Hash);
        }
    }
}