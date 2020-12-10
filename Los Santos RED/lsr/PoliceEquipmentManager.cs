using ExtensionsMethods;
using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using System.Collections.Generic;
using System.Linq;

public class PoliceEquipmentManager
{
    private List<EquipedCop> EquipedCops;
    public bool IsRunning { get; set; }
    public void Initialize()
    {
        IsRunning = true;
        EquipedCops = new List<EquipedCop>();
    }
    public void Dispose()
    {
        IsRunning = false;
    }
    public void Tick()
    {
        if (IsRunning)
        {
            AddCops();
            ArmCops();
        }
    }
    public void IssueWeapons(Cop CopToFind)
    {
        EquipedCop MyCop = EquipedCops.FirstOrDefault(x => x.CopToArm.Pedestrian.Handle == CopToFind.Pedestrian.Handle);
        if(MyCop != null)
        {
            MyCop.IssueWeapons();
        }
    }
    private void AddCops()
    {
        Mod.PedManager.Cops.RemoveAll(x => !x.Pedestrian.Exists());
        EquipedCops.RemoveAll(x => !x.CopToArm.Pedestrian.Exists());
        foreach (Cop Cop in Mod.PedManager.Cops.Where(x => x.Pedestrian.Exists()))
        {
            if (!EquipedCops.Any(x => x.CopToArm.Pedestrian.Handle == Cop.Pedestrian.Handle))
            {
                EquipedCops.Add(new EquipedCop(Cop));
            }
        }
    }
    private void ArmCops()
    {
        foreach (EquipedCop Cop in EquipedCops.Where(x => x.CopToArm.Pedestrian.Exists()))
        {
            Cop.IssueWeapons();
            Cop.ArmCopAppropriately();
        }
    }
    private class EquipedCop
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
        public Cop CopToArm { get; set; }
        public bool NeedsWeaponCheck
        {
            get
            {
                if (GameTimeLastWeaponCheck == 0)
                    return true;
                else if (Game.GameTime > GameTimeLastWeaponCheck + 500)
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
        public EquipedCop(Cop _GTAPedToArm)
        {
            CopToArm = _GTAPedToArm;
        }
        public void IssueWeapons()
        {
            if (CopToArm != null)
            {
                if (!HasPistol)
                {
                    IssuePistol();
                }
                if (SettingsManager.MySettings.Police.IssuePoliceHeavyWeapons && Mod.WantedLevelManager.IsDeadlyChase && !HasHeavyWeapon)
                {
                    CheckIssueHeavy();
                }
            }
        }
        public void ArmCopAppropriately()
        {
            if (CopToArm.ShouldAutoSetWeaponState)
            {
                if (Mod.WantedLevelManager.IsDeadlyChase)
                {
                    if (CopToArm.IsInVehicle && Mod.Player.WantedLevel < 4)
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
        public void CheckIssueHeavy()
        {
            if (SettingsManager.MySettings.Police.IssuePoliceHeavyWeapons && Mod.WantedLevelManager.IsDeadlyChase && !HasHeavyWeapon && CopToArm.IsInVehicle)
                IssueHeavyWeapon();
        }
        public void IssuePistol()
        {
            Agency AssignedAgency = CopToArm.AssignedAgency;
            WeaponInformation Pistol;
            IssuedWeapon PistolToPick = new IssuedWeapon("weapon_pistol", true, null);
            if (AssignedAgency != null)
            {
                PistolToPick = AssignedAgency.IssuedWeapons.Where(x => x.IsPistol).PickRandom();
            }
            Pistol = WeaponManager.GetWeapon(PistolToPick.ModelName);
            IssuedPistol = Pistol;
            if (IssuedPistol == null)
            {
                return;
            }
            CopToArm.Pedestrian.Inventory.GiveNewWeapon(Pistol.ModelName, Pistol.AmmoAmount, false);
            if (SettingsManager.MySettings.Police.AllowPoliceWeaponVariations)
            {
                WeaponVariation MyVariation = PistolToPick.MyVariation;
                PistolVariation = MyVariation;
                MyVariation.ApplyWeaponVariation(CopToArm.Pedestrian, (uint)Pistol.Hash);
            }

            Debugging.WriteToLog("PoliceEquipment", string.Format("Issued Pistol: {0}", IssuedPistol.ModelName));
        }
        public void IssueHeavyWeapon()
        {
            Agency AssignedAgency = CopToArm.AssignedAgency;
            WeaponInformation IssuedHeavy;
            IssuedWeapon HeavyToPick = new IssuedWeapon("weapon_shotgun", true, null);
            if (AssignedAgency != null)
            {
                HeavyToPick = AssignedAgency.IssuedWeapons.Where(x => !x.IsPistol).PickRandom();
            }
            IssuedHeavy = WeaponManager.GetWeapon(HeavyToPick.ModelName);
            IssuedHeavyWeapon = IssuedHeavy;
            if (IssuedHeavyWeapon == null)
            {
                return;
            }
            CopToArm.Pedestrian.Inventory.GiveNewWeapon(IssuedHeavy.ModelName, IssuedHeavy.AmmoAmount, true);
            if (SettingsManager.MySettings.Police.AllowPoliceWeaponVariations)
            {
                WeaponVariation MyVariation = HeavyToPick.MyVariation;
                HeavyVariation = MyVariation;
                MyVariation.ApplyWeaponVariation(CopToArm.Pedestrian, (uint)IssuedHeavy.Hash);
            }
            Debugging.WriteToLog("PoliceEquipment", string.Format("Issued Heavy: {0}", IssuedHeavyWeapon.ModelName));
        }
        public void SetUnarmed()
        {
            if (!CopToArm.Pedestrian.Exists() || CopToArm.Pedestrian.IsDead || (IsSetUnarmed && !NeedsWeaponCheck))
                return;
            if (SettingsManager.MySettings.Police.OverridePoliceAccuracy)
                CopToArm.Pedestrian.Accuracy = SettingsManager.MySettings.Police.PoliceGeneralAccuracy;

            NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", CopToArm.Pedestrian, 0);
            if (!(CopToArm.Pedestrian.Inventory.EquippedWeapon == null))
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", CopToArm.Pedestrian, 2725352035, true); //Unequip weapon so you don't get shot
                NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", CopToArm.Pedestrian, false);
            }
            NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", CopToArm.Pedestrian, 2, false);//cant do drivebys

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
        public void SetDeadly()
        {
            if (!CopToArm.Pedestrian.Exists() || CopToArm.Pedestrian.IsDead || (IsSetDeadly && !NeedsWeaponCheck))
                return;
            if (SettingsManager.MySettings.Police.OverridePoliceAccuracy)
                CopToArm.Pedestrian.Accuracy = SettingsManager.MySettings.Police.PoliceGeneralAccuracy;
            NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", CopToArm.Pedestrian, 30);
            if (!CopToArm.Pedestrian.Inventory.Weapons.Contains(IssuedPistol.ModelName))
                CopToArm.Pedestrian.Inventory.GiveNewWeapon(IssuedPistol.ModelName, -1, true);

            if ((CopToArm.Pedestrian.Inventory.EquippedWeapon == null || CopToArm.Pedestrian.Inventory.EquippedWeapon.Hash == WeaponHash.StunGun) && Game.LocalPlayer.WantedLevel >= 0)
                CopToArm.Pedestrian.Inventory.GiveNewWeapon(IssuedPistol.ModelName, -1, true);

            if (IssuedHeavyWeapon != null)
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", CopToArm.Pedestrian, NativeFunction.CallByName<bool>("GET_BEST_PED_WEAPON", CopToArm.Pedestrian, 0), true);
            }

            if (SettingsManager.MySettings.Police.AllowPoliceWeaponVariations)
            {
                PistolVariation.ApplyWeaponVariation(CopToArm.Pedestrian, (uint)IssuedPistol.Hash);
            }
            NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", CopToArm.Pedestrian, true);
            NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", CopToArm.Pedestrian, 2, true);//can do drivebys

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
        public void SetLessLethal()
        {
            if (!CopToArm.Pedestrian.Exists() || CopToArm.Pedestrian.IsDead || (IsSetLessLethal && !NeedsWeaponCheck))
                return;

            if (SettingsManager.MySettings.Police.OverridePoliceAccuracy)
                CopToArm.Pedestrian.Accuracy = SettingsManager.MySettings.Police.PoliceTazerAccuracy;
            NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", CopToArm.Pedestrian, 100);
            if (!CopToArm.Pedestrian.Inventory.Weapons.Contains(WeaponHash.StunGun))
            {
                CopToArm.Pedestrian.Inventory.GiveNewWeapon(WeaponHash.StunGun, 100, true);
            }
            else if (CopToArm.Pedestrian.Inventory.EquippedWeapon != WeaponHash.StunGun)
            {
                CopToArm.Pedestrian.Inventory.EquippedWeapon = WeaponHash.StunGun;
            }
            NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", CopToArm.Pedestrian, false);
            NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", CopToArm.Pedestrian, 2, false);//cant do drivebys

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
}

