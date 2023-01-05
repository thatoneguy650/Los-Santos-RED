using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

[Serializable]
public class IssuableWeapon
{
    private uint ModelHash;
    private bool isTaser = false;
    public IssuableWeapon()
    {

    }
    public IssuableWeapon(string name, WeaponVariation variation)
    {
        ModelName = name;
        Variation = variation;
    }
    public IssuableWeapon(string name, WeaponVariation variation, int spawnChance)
    {
        ModelName = name;
        Variation = variation;
        SpawnChance = spawnChance;
    }

    public string ModelName { get; set; }
    public WeaponVariation Variation { get; set; }
    public int SpawnChance { get; set; } = 100;

    [XmlIgnore]
    private List<WeaponComponent> PossibleComponents { get; set; } = new List<WeaponComponent>();
    public bool IsTaser => isTaser;
    public void ApplyVariation(Ped WeaponOwner)
    {
        //EntryPoint.WriteToConsole($"ISSUABLE WEAPON: ApplyVariation {ModelName}", 5);
        if (Variation == null)//getting weird null errors here, 
        {
            return;
        }
        NativeFunction.Natives.SET_PED_WEAPON_TINT_INDEX<bool>(WeaponOwner, ModelHash, Variation.Tint);
        if (PossibleComponents != null)
        {
            foreach (WeaponComponent ToRemove in PossibleComponents)
            {
                NativeFunction.Natives.REMOVE_WEAPON_COMPONENT_FROM_PED(WeaponOwner, ModelHash, ToRemove.Hash);
                //EntryPoint.WriteToConsole($"ISSUABLE WEAPON: REMOVE_WEAPON_COMPONENT_FROM_PED {ModelName} {ModelHash} {ToRemove.Hash} {ToRemove.Name}", 5);
            }
        }
        foreach (WeaponComponent ToAdd in Variation.Components)
        {
            WeaponComponent MyComponent = Variation.Components.Where(x => x.Name == ToAdd.Name).FirstOrDefault();
            if (MyComponent != null)
            {
                WeaponComponent lookup = PossibleComponents.FirstOrDefault(x => x.Name == MyComponent.Name);
                if (lookup != null)
                {
                    NativeFunction.Natives.GIVE_WEAPON_COMPONENT_TO_PED(WeaponOwner, ModelHash, lookup.Hash);
                }
                //EntryPoint.WriteToConsole($"ISSUABLE WEAPON: GIVE_WEAPON_COMPONENT_TO_PED {ModelName} {ModelHash} {lookup.Hash} {lookup.Name}", 5);
            }
        }
    }
    public void SetIssued(uint hash, List<WeaponComponent> possibleComponents, bool isWeaponTaser)
    {
        ModelHash = hash;
        PossibleComponents = possibleComponents;
        isTaser = isWeaponTaser;
    }
    public void SetIssued(uint hash, bool isWeaponTaser)
    {
        ModelHash = hash;
        isTaser = isWeaponTaser;
    }
    public uint GetHash()
    {
        return ModelHash;
    }
}

