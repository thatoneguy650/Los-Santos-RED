using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class IssuableWeapon
{
    public IssuableWeapon(string name, WeaponVariation variation)
    {
        ModelName = name;
        Variation = variation;
    }
    public uint ModelHash { get; private set; }
    public string ModelName { get; set; }
    public List<WeaponComponent> PossibleComponents { get; private set; }
    public WeaponVariation Variation { get; set; }
    public void ApplyVariation(Ped WeaponOwner)
    {
        if (Variation == null)
        {
            return;
        }
        NativeFunction.CallByName<bool>("SET_PED_WEAPON_TINT_INDEX", WeaponOwner, ModelHash, Variation.Tint);
        foreach (WeaponComponent ToRemove in PossibleComponents)
        {
            NativeFunction.CallByName<bool>("REMOVE_WEAPON_COMPONENT_FROM_PED", WeaponOwner, ModelHash, ToRemove.Hash);
        }
        foreach (WeaponComponent ToAdd in Variation.Components)
        {
            WeaponComponent MyComponent = PossibleComponents.Where(x => x.Name == ToAdd.Name).FirstOrDefault();
            if (MyComponent != null)
                NativeFunction.CallByName<bool>("GIVE_WEAPON_COMPONENT_TO_PED", WeaponOwner, ModelHash, MyComponent.Hash);
        }
    }
    public void SetIssued(uint hash, List<WeaponComponent> possibleComponents)
    {
        ModelHash = hash;
        PossibleComponents = possibleComponents;
    }
    public void SetIssued(uint hash)
    {
        ModelHash = hash;
    }
}

