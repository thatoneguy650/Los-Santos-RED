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
    private uint ModelHash;
    public IssuableWeapon()
    {

    }
    public IssuableWeapon(string name, WeaponVariation variation)
    {
        ModelName = name;
        Variation = variation;
    }

    public string ModelName { get; set; }
    private List<WeaponComponent> PossibleComponents  = new List<WeaponComponent>();
    public WeaponVariation Variation { get; set; }
    public void ApplyVariation(Ped WeaponOwner)
    {
        if (Variation == null)//getting weird null errors here, 
        {
            return;
        }
        NativeFunction.Natives.SET_PED_WEAPON_TINT_INDEX<bool>(WeaponOwner, ModelHash, Variation.Tint);
        if (PossibleComponents != null)
        {
            foreach (WeaponComponent ToRemove in PossibleComponents)
            {
                NativeFunction.Natives.REMOVE_WEAPON_COMPONENT_FROM_PED<bool>(WeaponOwner, ModelHash, ToRemove.GetHash());
            }
        }
        foreach (WeaponComponent ToAdd in Variation.Components)
        {
            WeaponComponent MyComponent = Variation.Components.Where(x => x.Name == ToAdd.Name).FirstOrDefault();
            if (MyComponent != null)
            {
                NativeFunction.Natives.GIVE_WEAPON_COMPONENT_TO_PED<bool>(WeaponOwner, ModelHash, MyComponent.GetHash());
            }
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
    public uint GetHash()
    {
        return ModelHash;
    }
}

