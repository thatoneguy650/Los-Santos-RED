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
    private WeaponInformation IssuedHeavyWeapon;
    private WeaponVariation PistolVariation;
    private WeaponVariation HeavyVariation;
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
            if (IssuedHeavyWeapon != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public Loadout(Cop cop)
    {
        Cop = cop;
    }
    public void IssueWeapons()
    {
        if (Cop != null)
        {
            if (!HasPistol)
            {
                IssuePistol();
            }
            if (DataMart.Instance.Settings.SettingsManager.Police.IssuePoliceHeavyWeapons && !HasHeavyWeapon && Mod.Player.Instance.CurrentPoliceResponse.IsDeadlyChase)
            {
                CheckIssueHeavy();
            }
        }
    }
    public void Update()
    {
        IssueWeapons();
        if (Cop.ShouldAutoSetWeaponState)
        {
            if (Mod.Player.Instance.CurrentPoliceResponse.IsDeadlyChase)
            {
                if (Cop.IsInVehicle && Mod.Player.Instance.WantedLevel < 4)
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
                if (Mod.Player.Instance.IsWanted)
                {
                    SetLessLethal();
                }
                else if (Cop.IsInVehicle)
                {
                    SetUnarmed();
                }
                else
                {
                    NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Cop.Pedestrian, true);//for idle, 
                }
            }
        }
    }
    private void CheckIssueHeavy()
    {
        if (DataMart.Instance.Settings.SettingsManager.Police.IssuePoliceHeavyWeapons && Mod.Player.Instance.CurrentPoliceResponse.IsDeadlyChase && !HasHeavyWeapon && Cop.IsInVehicle)
        {
            IssueHeavyWeapon();
        }
    }
    private void IssuePistol()
    {
        Agency AssignedAgency = Cop.AssignedAgency;
        WeaponInformation Pistol;
        IssuedWeapon PistolToPick = new IssuedWeapon("weapon_pistol", true, null);
        if (AssignedAgency != null)
        {
            PistolToPick = AssignedAgency.IssuedWeapons.Where(x => x.IsPistol).PickRandom();
        }
        Pistol = DataMart.Instance.Weapons.GetWeapon(PistolToPick.ModelName);
        IssuedPistol = Pistol;
        if (IssuedPistol == null)
        {
            return;
        }
        Cop.Pedestrian.Inventory.GiveNewWeapon(Pistol.ModelName, Pistol.AmmoAmount, false);
        if (DataMart.Instance.Settings.SettingsManager.Police.AllowPoliceWeaponVariations)
        {
            WeaponVariation MyVariation = PistolToPick.MyVariation;
            PistolVariation = MyVariation;
            MyVariation.ApplyWeaponVariation(Cop.Pedestrian, (uint)Pistol.Hash);
        }
        Debug.Instance.WriteToLog("PoliceEquipment", $"Issued Pistol: {IssuedPistol.ModelName} to {Cop.Pedestrian.Handle}");
    }
    private void IssueHeavyWeapon()
    {
        Agency AssignedAgency = Cop.AssignedAgency;
        WeaponInformation IssuedHeavy;
        IssuedWeapon HeavyToPick = new IssuedWeapon("weapon_shotgun", true, null);
        if (AssignedAgency != null)
        {
            HeavyToPick = AssignedAgency.IssuedWeapons.Where(x => !x.IsPistol).PickRandom();
        }
        IssuedHeavy = DataMart.Instance.Weapons.GetWeapon(HeavyToPick.ModelName);
        IssuedHeavyWeapon = IssuedHeavy;
        if (IssuedHeavyWeapon == null)
        {
            return;
        }
        Cop.Pedestrian.Inventory.GiveNewWeapon(IssuedHeavy.ModelName, IssuedHeavy.AmmoAmount, true);
        if (DataMart.Instance.Settings.SettingsManager.Police.AllowPoliceWeaponVariations)
        {
            WeaponVariation MyVariation = HeavyToPick.MyVariation;
            HeavyVariation = MyVariation;
            MyVariation.ApplyWeaponVariation(Cop.Pedestrian, (uint)IssuedHeavy.Hash);
        }
        Debug.Instance.WriteToLog("PoliceEquipment", $"Issued Heavy: {IssuedHeavy.ModelName} to {Cop.Pedestrian.Handle}");
    }
    private void SetUnarmed()
    {
        if (Cop.Pedestrian.Exists() && Cop.Pedestrian.IsAlive && (!IsSetUnarmed || NeedsWeaponCheck))
        {
            if (DataMart.Instance.Settings.SettingsManager.Police.OverridePoliceAccuracy)
            {
                Cop.Pedestrian.Accuracy = DataMart.Instance.Settings.SettingsManager.Police.PoliceGeneralAccuracy;
            }
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
            if (DataMart.Instance.Settings.SettingsManager.Police.OverridePoliceAccuracy)
            {
                Cop.Pedestrian.Accuracy = DataMart.Instance.Settings.SettingsManager.Police.PoliceGeneralAccuracy;
            }
            if (!Cop.Pedestrian.Inventory.Weapons.Contains(IssuedPistol.ModelName))
            {
                Cop.Pedestrian.Inventory.GiveNewWeapon(IssuedPistol.ModelName, -1, true);
                if (DataMart.Instance.Settings.SettingsManager.Police.AllowPoliceWeaponVariations)
                {
                    PistolVariation.ApplyWeaponVariation(Cop.Pedestrian, (uint)IssuedPistol.Hash);
                }
            }
            NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Cop.Pedestrian, 50);//30
            if(IssuedHeavyWeapon != null)
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Cop.Pedestrian, IssuedHeavyWeapon.Hash, true);
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
            if (DataMart.Instance.Settings.SettingsManager.Police.OverridePoliceAccuracy)
            {
                Cop.Pedestrian.Accuracy = DataMart.Instance.Settings.SettingsManager.Police.PoliceTazerAccuracy;
            }  
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
    private void SetNormal()
    {
        if (Cop.Pedestrian.Exists() && Cop.Pedestrian.IsAlive && (!IsSetDeadly || NeedsWeaponCheck))
        {
            if (DataMart.Instance.Settings.SettingsManager.Police.OverridePoliceAccuracy)
            {
                Cop.Pedestrian.Accuracy = DataMart.Instance.Settings.SettingsManager.Police.PoliceGeneralAccuracy;
            }
            if (!Cop.Pedestrian.Inventory.Weapons.Contains(IssuedPistol.ModelName))
            {
                Cop.Pedestrian.Inventory.GiveNewWeapon(IssuedPistol.ModelName, -1, true);
                if (DataMart.Instance.Settings.SettingsManager.Police.AllowPoliceWeaponVariations)
                {
                    PistolVariation.ApplyWeaponVariation(Cop.Pedestrian, (uint)IssuedPistol.Hash);
                }
            }
            NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Cop.Pedestrian, 30);
            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Cop.Pedestrian, NativeFunction.CallByName<bool>("GET_BEST_PED_WEAPON", Cop.Pedestrian, 0), true);
            NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Cop.Pedestrian, true);
            NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Cop.Pedestrian, 2, true);//can do drivebys
            IsSetLessLethal = false;
            IsSetUnarmed = false;
            IsSetDeadly = true;
            GameTimeLastWeaponCheck = Game.GameTime;
        }
    }
}


