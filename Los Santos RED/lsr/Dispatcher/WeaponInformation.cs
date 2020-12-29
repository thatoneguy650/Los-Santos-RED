using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable()]
public class WeaponInformation
{
    public string ModelName { get; set; } = "Unknown";
    public short AmmoAmount { get; set; }
    public WeaponCategory Category { get; set; }
    public int WeaponLevel { get; set; }
    public uint Hash { get; set; }
    public bool CanPistolSuicide { get; set; } = false;
    public bool IsTwoHanded { get; set; } = false;
    public bool IsOneHanded { get; set; } = false;
    public bool IsLegal { get; set; } = false;
    public bool IsRegular { get; set; } = true;
    public List<WeaponComponent> PossibleComponents { get; set; } = new List<WeaponComponent>();
    public WeaponInformation()
    {

    }
    public WeaponInformation(string _Name, short _AmmoAmount, WeaponCategory _Category, int _WeaponLevel, uint _Hash, bool _IsOneHanded, bool _IsTwoHanded, bool _IsLegal)
    {
        ModelName = _Name;
        AmmoAmount = _AmmoAmount;
        Category = _Category;
        WeaponLevel = _WeaponLevel;
        Hash = _Hash;
        IsOneHanded = _IsOneHanded;
        IsTwoHanded = _IsTwoHanded;
        IsLegal = _IsLegal;
    }
    public bool IsLowEnd
    {
        get
        {
            if(Category == WeaponCategory.Pistol || Category == WeaponCategory.Melee || Category == WeaponCategory.Shotgun)
            {
                return true;
                
            }
            else
            {
                return false;
            }
        }
    }
    public void ApplyWeaponVariation(Ped WeaponOwner, uint WeaponHash, WeaponVariation weaponVariation)
    {
        if (weaponVariation == null)
        {
            return;
        }    
        NativeFunction.CallByName<bool>("SET_PED_WEAPON_TINT_INDEX", WeaponOwner, WeaponHash, weaponVariation.Tint);
        foreach (WeaponComponent ToRemove in PossibleComponents)
        {
            NativeFunction.CallByName<bool>("REMOVE_WEAPON_COMPONENT_FROM_PED", WeaponOwner, WeaponHash, ToRemove.GetHash());
        }
        foreach (WeaponComponent ToAdd in weaponVariation.Components)
        {
            WeaponComponent MyComponent = PossibleComponents.Where(x => x.Name == ToAdd.Name).FirstOrDefault();
            if (MyComponent != null)
                NativeFunction.CallByName<bool>("GIVE_WEAPON_COMPONENT_TO_PED", WeaponOwner, WeaponHash, MyComponent.GetHash());
        }
    }
}