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
    public bool IsTaser { get; set; } = false;
    public bool IsTwoHanded { get; set; } = false;
    public bool IsOneHanded { get; set; } = false;
    public bool IsLegal { get; set; } = false;
    public bool IsRegular { get; set; } = true;

    public float MinVerticalRecoil { get; set; } = 0.0f;
    public float MaxVerticalRecoil { get; set; } = 0.0f;
    public float MinHorizontalRecoil { get; set; } = 0.0f;
    public float MaxHorizontalRecoil { get; set; } = 0.0f;

    public float MinHorizontalSway { get; set; } = 0.0f;
    public float MaxHorizontalSway { get; set; } = 0.0f;
    public float MinVerticaSway { get; set; } = 0.0f;
    public float MaxVerticaSway { get; set; } = 0.0f;
    public List<WeaponComponent> PossibleComponents { get; set; } = new List<WeaponComponent>();




    public SelectorOptions SelectorOptions { get; set; } = SelectorOptions.Safe | SelectorOptions.SemiAuto;

    public bool IsLegalWithoutCCW => Category == WeaponCategory.Melee && IsLegal ? true : false;
    
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
    public WeaponInformation(string _Name, short _AmmoAmount, WeaponCategory _Category, int _WeaponLevel, uint _Hash, bool _IsOneHanded, bool _IsTwoHanded, bool _IsLegal, float _MinVerticalRecoil, float _MaxVerticalRecoil, float _MinHorizontalRecoil, float _MaxHorizontalRecoil, float _MinHorizontalSway, float _MaxHorizontalSway, float _MinVerticaSway, float _MaxVerticaSway, SelectorOptions selectorOptions)
    {
        ModelName = _Name;
        AmmoAmount = _AmmoAmount;
        Category = _Category;
        WeaponLevel = _WeaponLevel;
        Hash = _Hash;
        IsOneHanded = _IsOneHanded;
        IsTwoHanded = _IsTwoHanded;
        IsLegal = _IsLegal;
        MinVerticalRecoil = _MinVerticalRecoil;
        MaxVerticalRecoil = _MaxVerticalRecoil;
        MinHorizontalRecoil = _MinHorizontalRecoil;
        MaxHorizontalRecoil = _MaxHorizontalRecoil;


        MinHorizontalSway = _MinHorizontalSway;
        MaxHorizontalSway = _MaxHorizontalSway;
        MinVerticaSway = _MinVerticaSway;
        MaxVerticaSway = _MaxVerticaSway;
        SelectorOptions = selectorOptions;
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

    public bool DoesNotTriggerBrandishing { get; set; } = false;

    public void ApplyWeaponVariation(Ped WeaponOwner, WeaponVariation weaponVariation)
    {
        if (weaponVariation == null)
        {
            return;
        }    
        NativeFunction.Natives.SET_PED_WEAPON_TINT_INDEX<bool>(WeaponOwner, Hash, weaponVariation.Tint);
        foreach (WeaponComponent ToRemove in PossibleComponents)
        {
            NativeFunction.Natives.REMOVE_WEAPON_COMPONENT_FROM_PED<bool>(WeaponOwner, Hash, ToRemove.Hash);
        }
        foreach (WeaponComponent ToAdd in weaponVariation.Components)
        {
            WeaponComponent MyComponent = PossibleComponents.Where(x => x.Name == ToAdd.Name).FirstOrDefault();
            if (MyComponent != null)
            {
                NativeFunction.Natives.GIVE_WEAPON_COMPONENT_TO_PED<bool>(WeaponOwner, Hash, MyComponent.Hash);
            }
        }
    }
    public bool AddComponent(Ped WeaponOwner, WeaponComponent weaponComponent)
    {
        if (weaponComponent != null && NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(WeaponOwner, Hash, false) && !NativeFunction.Natives.HAS_PED_GOT_WEAPON_COMPONENT<bool>(WeaponOwner, Hash, weaponComponent.Hash))
        {
            ComponentSlot componentSlot = weaponComponent.ComponentSlot;
            foreach (WeaponComponent ToRemove in PossibleComponents.Where(x => x.ComponentSlot == componentSlot))
            {
                NativeFunction.Natives.REMOVE_WEAPON_COMPONENT_FROM_PED<bool>(WeaponOwner, Hash, ToRemove.Hash);
            }
            NativeFunction.Natives.GIVE_WEAPON_COMPONENT_TO_PED<bool>(WeaponOwner, Hash, weaponComponent.Hash);
            return true;
        }
        return false;
    }
    public bool RemoveComponent(Ped WeaponOwner, WeaponComponent weaponComponent)
    {
        if (weaponComponent != null && NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(WeaponOwner, Hash, false))
        {
            if (NativeFunction.Natives.HAS_PED_GOT_WEAPON_COMPONENT<bool>(WeaponOwner, Hash, weaponComponent.Hash))
            {
                NativeFunction.Natives.REMOVE_WEAPON_COMPONENT_FROM_PED(WeaponOwner, Hash, weaponComponent.Hash);
                return true;
            }
        }
        return false;
    }
    public bool HasComponent(Ped WeaponOwner, WeaponComponent weaponComponent)
    {
        if (weaponComponent != null && NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(WeaponOwner, Hash, false))
        {
            return NativeFunction.Natives.HAS_PED_GOT_WEAPON_COMPONENT<bool>(WeaponOwner, Hash, weaponComponent.Hash);
        }
        return false;
    }
    public bool HasWeapon(Ped WeaponOwner)
    {
        if (NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(WeaponOwner, Hash, false))
        {
            return true;
        }
        return false;
    }
    public bool SetSlotDefault(Ped WeaponOwner, ComponentSlot componentSlot)
    {
        if (NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(WeaponOwner, Hash, false))
        {
            foreach (WeaponComponent ToRemove in PossibleComponents.Where(x => x.ComponentSlot == componentSlot))
            {
                NativeFunction.Natives.REMOVE_WEAPON_COMPONENT_FROM_PED<bool>(WeaponOwner, Hash, ToRemove.Hash);
            }
            return true;
        }
        return false;
    }
    private void SetDefaultRecoilForCategory()
    {
        if (Category == WeaponCategory.AR)
        {
            MinVerticalRecoil = 0.25f;
            MaxVerticalRecoil = 0.35f;
            MinHorizontalRecoil = 0.1f;
            MaxHorizontalRecoil = 0.2f;
        }
        else if (Category == WeaponCategory.Heavy)
        {
            MinVerticalRecoil = 0.4f;
            MaxVerticalRecoil = 0.5f;
            MinHorizontalRecoil = 0.1f;
            MaxHorizontalRecoil = 0.2f;
        }
        else if (Category == WeaponCategory.LMG)
        {
            MinVerticalRecoil = 0.3f;
            MaxVerticalRecoil = 0.4f;
            MinHorizontalRecoil = 0.1f;
            MaxHorizontalRecoil = 0.2f;
        }
        else if (Category == WeaponCategory.Pistol)
        {
            MinVerticalRecoil = 1.0f;
            MaxVerticalRecoil = 1.2f;
            MinHorizontalRecoil = 0.7f;
            MaxHorizontalRecoil = 0.9f;
        }
        else if (Category == WeaponCategory.Shotgun)
        {
            MinVerticalRecoil = 0.6f;
            MaxVerticalRecoil = 0.7f;
            MinHorizontalRecoil = 0.3f;
            MaxHorizontalRecoil = 0.4f;
        }
        else if (Category == WeaponCategory.SMG)
        {
            if (IsOneHanded)
            {
                MinVerticalRecoil = 0.9f;
                MaxVerticalRecoil = 1.2f;
                MinHorizontalRecoil = 0.5f;
                MaxHorizontalRecoil = 0.7f;
            }
            else
            {
                MinVerticalRecoil = 0.7f;
                MaxVerticalRecoil = 0.9f;
                MinHorizontalRecoil = 0.4f;
                MaxHorizontalRecoil = 0.5f;
            }
        }
        else if (Category == WeaponCategory.Sniper)
        {
            MinVerticalRecoil = 0.5f;
            MaxVerticalRecoil = 0.75f;
            MinHorizontalRecoil = 0.1f;
            MaxHorizontalRecoil = 0.2f;
        }
    }
}