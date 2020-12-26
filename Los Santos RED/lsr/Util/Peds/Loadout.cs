using ExtensionsMethods;
using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Loadout
{
    private bool IsSetLessLethal;
    private bool IsSetUnarmed;
    private bool IsSetDeadly;
    private uint GameTimeLastWeaponCheck;
    private WeaponInformation IssuedPistol;
    private WeaponInformation IssuedHeavy;
    private WeaponVariation IssuedPistolVariation;
    private WeaponVariation IssuedHeavyVariation;
    public Cop Cop { get; set; }
    public bool NeedsWeaponCheck
    {
        get
        {
            if (GameTimeLastWeaponCheck == 0)
            {
                return true;
            }
            else if (Game.GameTime > GameTimeLastWeaponCheck + 750)//500
            {
                return true;
            }
            else
                return false;
        }
    }
    public bool HasPistol
    {
        get
        {
            if (IssuedPistol != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public bool HasHeavyWeapon
    {
        get
        {
            if (IssuedHeavy != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public Loadout(Cop cop, WeaponInformation pistolToIssue, WeaponVariation pistolVariation, WeaponInformation heavyToIssue, WeaponVariation heavyVaritaion)
    {
        Cop = cop;
        IssuedPistol = pistolToIssue;
        IssuedPistolVariation = pistolVariation;
        IssuedHeavy = heavyToIssue;
        IssuedHeavyVariation = heavyVaritaion;
    }
    public void Update(bool IsDeadlyChase, int WantedLevel)
    {
        if (Cop.ShouldAutoSetWeaponState)
        {
            if (IsDeadlyChase)
            {
                if (Cop.IsInVehicle && WantedLevel < 4)
                {
                    SetUnarmed();
                }
                else
                {
                    SetDeadly();
                }
            }
            else
            {
                if (WantedLevel > 0)
                {
                    SetLessLethal();
                }
                else if (Cop.IsInVehicle)
                {
                    SetUnarmed();
                }
                else if(WantedLevel == 0)
                {
                    NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Cop.Pedestrian, true);//for idle, 
                }
                else
                {
                    SetUnarmed();
                }
            }
        }
    }
    private void SetUnarmed()
    {
        if (Cop.Pedestrian.Exists() && Cop.Pedestrian.IsAlive && (!IsSetUnarmed || NeedsWeaponCheck))
        {
            if (Cop.Pedestrian.Inventory.EquippedWeapon != null)
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Cop.Pedestrian, 2725352035, true); //Unequip weapon so you don't get shot
                NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Cop.Pedestrian, false);
            }
            NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Cop.Pedestrian, 0);
            NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Cop.Pedestrian, 2, false);//cant do drivebys
            IsSetLessLethal = false;
            IsSetUnarmed = true;
            IsSetDeadly = false;
            GameTimeLastWeaponCheck = Game.GameTime;
        }
    }
    private void SetDeadly()
    {
        if (Cop.Pedestrian.Exists() && Cop.Pedestrian.IsAlive && (!IsSetDeadly || NeedsWeaponCheck))
        {
            Cop.Pedestrian.Accuracy = 10;
            if (!Cop.Pedestrian.Inventory.Weapons.Contains(IssuedPistol.ModelName))
            {
                Cop.Pedestrian.Inventory.GiveNewWeapon(IssuedPistol.ModelName, -1, true);
                IssuedPistol.ApplyWeaponVariation(Cop.Pedestrian, (uint)IssuedPistol.Hash, IssuedPistolVariation);
            }
            NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Cop.Pedestrian, 50);//30
            if (IssuedHeavy != null)
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Cop.Pedestrian, IssuedHeavy.Hash, true);
            }
            else
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Cop.Pedestrian, IssuedPistol.Hash, true);
            }
            NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Cop.Pedestrian, 2, true);//can do drivebys
            IsSetLessLethal = false;
            IsSetUnarmed = false;
            IsSetDeadly = true;
            GameTimeLastWeaponCheck = Game.GameTime;
        }
    }
    private void SetLessLethal()
    {
        if (Cop.Pedestrian.Exists() && Cop.Pedestrian.IsAlive && (!IsSetLessLethal || NeedsWeaponCheck))
        {
            Cop.Pedestrian.Accuracy = 30;
            if (!Cop.Pedestrian.Inventory.Weapons.Contains(WeaponHash.StunGun))
            {
                Cop.Pedestrian.Inventory.GiveNewWeapon(WeaponHash.StunGun, 100, true);
            }
            else if (Cop.Pedestrian.Inventory.EquippedWeapon != WeaponHash.StunGun)
            {
                Cop.Pedestrian.Inventory.EquippedWeapon = WeaponHash.StunGun;
            }
            NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Cop.Pedestrian, 100);
            NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Cop.Pedestrian, false);
            NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Cop.Pedestrian, 2, false);//cant do drivebys
            IsSetLessLethal = true;
            IsSetUnarmed = false;
            IsSetDeadly = false;
            GameTimeLastWeaponCheck = Game.GameTime;
        }
    }
}


