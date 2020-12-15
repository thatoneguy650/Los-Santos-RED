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
    public bool ShouldAutoSetWeaponState { get; set; } = true;
    public Cop Cop { get; set; }
    public bool NeedsWeaponCheck
    {
        get
        {
            if (GameTimeLastWeaponCheck == 0)
                return true;
            else if (Game.GameTime > GameTimeLastWeaponCheck + 750)//500
                return true;
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
            if (Mod.DataMart.Settings.SettingsManager.Police.IssuePoliceHeavyWeapons && !HasHeavyWeapon && Mod.Player.CurrentPoliceResponse.IsDeadlyChase)
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
            if (Mod.Player.CurrentPoliceResponse.IsDeadlyChase)
            {
                if (Cop.IsInVehicle && Mod.Player.WantedLevel < 4)
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
                if (Mod.Player.IsNotWanted)
                {
                    SetUnarmed();
                }
                else
                {
                    SetLessLethal();
                }
            }
        }
    }
    private void CheckIssueHeavy()
    {
        if (Mod.DataMart.Settings.SettingsManager.Police.IssuePoliceHeavyWeapons && Mod.Player.CurrentPoliceResponse.IsDeadlyChase && !HasHeavyWeapon && Cop.IsInVehicle)
            IssueHeavyWeapon();
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
        Pistol = Mod.DataMart.Weapons.GetWeapon(PistolToPick.ModelName);
        IssuedPistol = Pistol;
        if (IssuedPistol == null)
        {
            return;
        }
        Cop.Pedestrian.Inventory.GiveNewWeapon(Pistol.ModelName, Pistol.AmmoAmount, false);
        if (Mod.DataMart.Settings.SettingsManager.Police.AllowPoliceWeaponVariations)
        {
            WeaponVariation MyVariation = PistolToPick.MyVariation;
            PistolVariation = MyVariation;
            MyVariation.ApplyWeaponVariation(Cop.Pedestrian, (uint)Pistol.Hash);
        }

        Mod.Debug.WriteToLog("PoliceEquipment", string.Format("Issued Pistol: {0}", IssuedPistol.ModelName));
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
        IssuedHeavy = Mod.DataMart.Weapons.GetWeapon(HeavyToPick.ModelName);
        IssuedHeavyWeapon = IssuedHeavy;
        if (IssuedHeavyWeapon == null)
        {
            return;
        }
        Cop.Pedestrian.Inventory.GiveNewWeapon(IssuedHeavy.ModelName, IssuedHeavy.AmmoAmount, true);
        if (Mod.DataMart.Settings.SettingsManager.Police.AllowPoliceWeaponVariations)
        {
            WeaponVariation MyVariation = HeavyToPick.MyVariation;
            HeavyVariation = MyVariation;
            MyVariation.ApplyWeaponVariation(Cop.Pedestrian, (uint)IssuedHeavy.Hash);
        }
        Mod.Debug.WriteToLog("PoliceEquipment", string.Format("Issued Heavy: {0}", IssuedHeavyWeapon.ModelName));
    }
    private void SetUnarmed()
    {
        if (!Cop.Pedestrian.Exists() || Cop.Pedestrian.IsDead || (IsSetUnarmed && !NeedsWeaponCheck))
            return;
        if (Mod.DataMart.Settings.SettingsManager.Police.OverridePoliceAccuracy)
            Cop.Pedestrian.Accuracy = Mod.DataMart.Settings.SettingsManager.Police.PoliceGeneralAccuracy;

        NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Cop.Pedestrian, 0);
        if (!(Cop.Pedestrian.Inventory.EquippedWeapon == null))
        {
            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Cop.Pedestrian, 2725352035, true); //Unequip weapon so you don't get shot
            NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Cop.Pedestrian, false);
        }
        NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Cop.Pedestrian, 2, false);//cant do drivebys

        //if (!IsSetUnarmed)
        //{
        //    General.RequestAnimationDictionay("weapons@holster_1h");
        //    NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Pedestrian, "weapons@holster_1h", "holster", 2.0f, -2.0f, -1, 52, 0, false, false, false);
        //}

        IsSetLessLethal = false;
        IsSetUnarmed = true;
        IsSetDeadly = false;
        GameTimeLastWeaponCheck = Game.GameTime;
    }
    private void SetDeadly()
    {
        if (!Cop.Pedestrian.Exists() || Cop.Pedestrian.IsDead || (IsSetDeadly && !NeedsWeaponCheck))
            return;
        if (Mod.DataMart.Settings.SettingsManager.Police.OverridePoliceAccuracy)
            Cop.Pedestrian.Accuracy = Mod.DataMart.Settings.SettingsManager.Police.PoliceGeneralAccuracy;
        NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Cop.Pedestrian, 30);
        if (!Cop.Pedestrian.Inventory.Weapons.Contains(IssuedPistol.ModelName))
            Cop.Pedestrian.Inventory.GiveNewWeapon(IssuedPistol.ModelName, -1, true);

        if ((Cop.Pedestrian.Inventory.EquippedWeapon == null || Cop.Pedestrian.Inventory.EquippedWeapon.Hash == WeaponHash.StunGun) && Game.LocalPlayer.WantedLevel >= 0)
            Cop.Pedestrian.Inventory.GiveNewWeapon(IssuedPistol.ModelName, -1, true);

        if (IssuedHeavyWeapon != null)
        {
            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Cop.Pedestrian, NativeFunction.CallByName<bool>("GET_BEST_PED_WEAPON", Cop.Pedestrian, 0), true);
        }

        if (Mod.DataMart.Settings.SettingsManager.Police.AllowPoliceWeaponVariations)
        {
            PistolVariation.ApplyWeaponVariation(Cop.Pedestrian, (uint)IssuedPistol.Hash);
        }
        NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Cop.Pedestrian, true);
        NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Cop.Pedestrian, 2, true);//can do drivebys

        //if (!IsSetDeadly)
        //{
        //    General.RequestAnimationDictionay("weapons@holster_1h");
        //    NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Pedestrian, "weapons@holster_1h", "unholster", 2.0f, -2.0f, -1, 52, 0, false, false, false);
        //}

        IsSetLessLethal = false;
        IsSetUnarmed = false;
        IsSetDeadly = true;
        GameTimeLastWeaponCheck = Game.GameTime;
    }
    private void SetLessLethal()
    {
        if (!Cop.Pedestrian.Exists() || Cop.Pedestrian.IsDead || (IsSetLessLethal && !NeedsWeaponCheck))
            return;

        if (Mod.DataMart.Settings.SettingsManager.Police.OverridePoliceAccuracy)
            Cop.Pedestrian.Accuracy = Mod.DataMart.Settings.SettingsManager.Police.PoliceTazerAccuracy;
        NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Cop.Pedestrian, 100);
        if (!Cop.Pedestrian.Inventory.Weapons.Contains(WeaponHash.StunGun))
        {
            Cop.Pedestrian.Inventory.GiveNewWeapon(WeaponHash.StunGun, 100, true);
        }
        else if (Cop.Pedestrian.Inventory.EquippedWeapon != WeaponHash.StunGun)
        {
            Cop.Pedestrian.Inventory.EquippedWeapon = WeaponHash.StunGun;
        }
        NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Cop.Pedestrian, false);
        NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Cop.Pedestrian, 2, false);//cant do drivebys

        //if (!IsSetLessLethal)
        //{
        //    General.RequestAnimationDictionay("weapons@holster_1h");
        //    NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Pedestrian, "weapons@holster_1h", "unholster", 2.0f, -2.0f, -1, 52, 0, false, false, false);
        //}
        IsSetLessLethal = true;
        IsSetUnarmed = false;
        IsSetDeadly = false;
        GameTimeLastWeaponCheck = Game.GameTime;
    }
}


