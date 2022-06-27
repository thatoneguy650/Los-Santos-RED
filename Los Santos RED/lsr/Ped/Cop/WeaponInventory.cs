using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class WeaponInventory
{
    private IWeaponIssuable WeaponOwner;
    private uint GameTimeLastWeaponCheck;

    public IssuableWeapon LongGun { get; private set; }
    public IssuableWeapon Sidearm { get; private set; }
    public IssuableWeapon Melee { get; private set; }

    public bool IsSetDeadly { get; private set; }
    public bool IsSetLessLethal { get; private set; }
    public bool IsSetUnarmed { get; private set; }
    public bool IsSetDefault { get; private set; }


    public bool HasHeavyWeaponOnPerson { get; private set; }
    private ISettingsProvideable Settings;
    public WeaponInventory(IWeaponIssuable weaponOwner, ISettingsProvideable settings)
    {
        WeaponOwner = weaponOwner;
        Settings = settings;
    }
    public bool NeedsWeaponCheck => GameTimeLastWeaponCheck == 0 || Game.GameTime > GameTimeLastWeaponCheck + 750;
    public bool ShouldAutoSetWeaponState { get; set; } = true;
    public string DebugWeaponState { get; set; }
    public bool HasPistol => Sidearm != null;
    public void IssueWeapons(IWeapons weapons, bool issueMelee, bool issueSidearm, bool issueLongGun)
    {
        if (issueMelee)
        {
            Melee = WeaponOwner.GetRandomMeleeWeapon(weapons);
            if (Melee != null && !NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(WeaponOwner.Pedestrian, (uint)Melee.GetHash(), false))//(uint)WeaponHash.StunGun
            {
                NativeFunction.Natives.GIVE_WEAPON_TO_PED(WeaponOwner.Pedestrian, (uint)Melee.GetHash(), 100, false, false);
            }
            if(Melee != null && Melee.IsTaser)
            {
                WeaponOwner.HasTaser = true;
            }
            else
            {
                WeaponOwner.HasTaser = false;
            }
        }
        if (issueSidearm)
        {
            Sidearm = WeaponOwner.GetRandomWeapon(true, weapons);
            if (Sidearm != null && !NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(WeaponOwner.Pedestrian, (uint)Sidearm.GetHash(), false))
            {
                NativeFunction.Natives.GIVE_WEAPON_TO_PED(WeaponOwner.Pedestrian, (uint)Sidearm.GetHash(), 200, false, false);
                Sidearm.ApplyVariation(WeaponOwner.Pedestrian);
            }
        }
        if (issueLongGun)
        {
            LongGun = WeaponOwner.GetRandomWeapon(false, weapons);
            if (LongGun != null && !NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(WeaponOwner.Pedestrian, (uint)LongGun.GetHash(), false))
            {
                NativeFunction.Natives.GIVE_WEAPON_TO_PED(WeaponOwner.Pedestrian, (uint)LongGun.GetHash(), 200, false, false);
                LongGun.ApplyVariation(WeaponOwner.Pedestrian);
            }
        }
        NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", WeaponOwner.Pedestrian, true);//was false, but might need them to switch in vehicles and if hanging outside vehicle
        NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", WeaponOwner.Pedestrian, 2, true);//can do drivebys    
    }
    public void UpdateLoadout(bool PlayerInVehicle, bool IsDeadlyChase, int WantedLevel, bool isAttemptingToSurrender, bool isBusted, bool isWeaponsFree, bool hasShotAtPolice, bool lethalForceAuthorized)
    {
        if (WeaponOwner.Pedestrian.Exists())
        {
            uint currentVehicleWeapon;
            bool hasVehicleWeapon = false;
            hasVehicleWeapon = NativeFunction.Natives.GET_CURRENT_PED_VEHICLE_WEAPON<bool>(WeaponOwner.Pedestrian, out currentVehicleWeapon);
            //3450622333 searchlight
            if (hasVehicleWeapon && currentVehicleWeapon == 3450622333)//searchlight
            {
                return;
            }
            if (ShouldAutoSetWeaponState && NeedsWeaponCheck)
            {
                if (WeaponOwner.CurrentTask?.Name == "AIApprehend")
                {
                    HasHeavyWeaponOnPerson = true;
                    if (WeaponOwner.CurrentTask.OtherTarget != null && WeaponOwner.CurrentTask.OtherTarget.IsDeadlyChase)
                    {
                        SetDeadly(true);
                    }
                    else
                    {
                        if (WeaponOwner.IsInVehicle)
                        {
                            SetUnarmed();
                        }
                        else
                        {
                            SetLessLethal();
                        }
                    }
                }
                else
                {
                    if (WantedLevel == 0)
                    {
                        if (!IsSetDefault && !IsSetUnarmed)
                        {
                            SetDefault();
                            HasHeavyWeaponOnPerson = false;
                        }
                    }
                    else
                    {
                        if (IsDeadlyChase)
                        {
                            if (WeaponOwner.IsInVehicle)
                            {
                                HasHeavyWeaponOnPerson = true;
                                if (isWeaponsFree)
                                {
                                    SetDeadly(false);
                                }
                                else
                                {
                                    if (isAttemptingToSurrender)
                                    {
                                        SetUnarmed();
                                    }
                                    else if (!hasShotAtPolice && WantedLevel <= 4)
                                    {
                                        SetUnarmed();
                                    }
                                    else
                                    {
                                        SetDeadly(false);
                                    }
                                }
                            }
                            else
                            {
                                SetDeadly(false);
                            }
                        }
                        else if (isBusted)
                        {
                            if (WeaponOwner.IsInVehicle)
                            {
                                SetUnarmed();
                            }
                            else
                            {
                                if (lethalForceAuthorized)
                                {
                                    SetDeadly(false);
                                }
                                else
                                {
                                    if(WantedLevel >= 2)
                                    {
                                        SetLessLethal();
                                    }
                                    else
                                    {
                                        GameTimeLastWeaponCheck = Game.GameTime;
                                        SetUnarmed();
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (WeaponOwner.IsInVehicle || WantedLevel == 1)
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
                if (Settings.SettingsManager.PoliceSettings.OverrideAccuracy)
                {
                    UpdateSettings();
                }
            }
        }
    }
    public void UpdateSettings()
    {
        if (WeaponOwner.Pedestrian.Exists())
        {
            //if (WeaponOwner.IsCop && WeaponOwner.CurrentTask?.Name == "AIApprehend")
            //{
            //    WeaponOwner.Pedestrian.Accuracy = 95;//they gonna get FUCKED UP, player WATCH OUT!
            //    NativeFunction.Natives.SET_PED_SHOOT_RATE(WeaponOwner.Pedestrian, 1000);
            //    NativeFunction.Natives.SET_PED_COMBAT_ABILITY(WeaponOwner.Pedestrian, 2);
            //}
            //else 
            
            
            if (WeaponOwner.IsInVehicle)
            {
                WeaponOwner.Pedestrian.Accuracy = WeaponOwner.VehicleAccuracy;
                NativeFunction.Natives.SET_PED_SHOOT_RATE(WeaponOwner.Pedestrian, WeaponOwner.VehicleShootRate);
                NativeFunction.Natives.SET_PED_COMBAT_ABILITY(WeaponOwner.Pedestrian, WeaponOwner.CombatAbility);
            }
            else if (WeaponOwner.IsCop && IsSetLessLethal)
            {
                WeaponOwner.Pedestrian.Accuracy = WeaponOwner.TaserAccuracy;
                NativeFunction.Natives.SET_PED_SHOOT_RATE(WeaponOwner.Pedestrian, WeaponOwner.TaserShootRate);
                NativeFunction.Natives.SET_PED_COMBAT_ABILITY(WeaponOwner.Pedestrian, WeaponOwner.CombatAbility);
            }
            else
            {
                WeaponOwner.Pedestrian.Accuracy = WeaponOwner.Accuracy;//Settings.SettingsManager.PoliceSettings.GeneralAccuracy;
                NativeFunction.Natives.SET_PED_SHOOT_RATE(WeaponOwner.Pedestrian, WeaponOwner.ShootRate);// Settings.SettingsManager.PoliceSettings.GeneralShootRate);
                NativeFunction.Natives.SET_PED_COMBAT_ABILITY(WeaponOwner.Pedestrian, WeaponOwner.CombatAbility);
            }
        }
    }
    public void SetDefault()
    {
        if (!IsSetDefault && WeaponOwner.Pedestrian.Exists() && WeaponOwner.Pedestrian.IsAlive)
        {
            if (Melee != null && !NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(WeaponOwner.Pedestrian, (uint)Melee.GetHash(), false))
            {
                NativeFunction.Natives.GIVE_WEAPON_TO_PED(WeaponOwner.Pedestrian, (uint)Melee.GetHash(), 100, false, false);
            }
            if (Sidearm != null && !NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(WeaponOwner.Pedestrian, (uint)Sidearm.GetHash(), false))
            {
                NativeFunction.Natives.GIVE_WEAPON_TO_PED(WeaponOwner.Pedestrian, (uint)Sidearm.GetHash(), 200, false, false);
                Sidearm.ApplyVariation(WeaponOwner.Pedestrian);
            }
            uint currentWeapon;
            NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(WeaponOwner.Pedestrian, out currentWeapon, true);
            if (currentWeapon != 2725352035 && currentWeapon != 966099553)//unarmed and notepad
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", WeaponOwner.Pedestrian, 2725352035, true);
            }
            NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", WeaponOwner.Pedestrian, true);//was false, but might need them to switch in vehicles and if hanging outside vehicle
            NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", WeaponOwner.Pedestrian, 2, true);//can do drivebys       
            IsSetLessLethal = false;
            IsSetUnarmed = false;
            IsSetDeadly = false;
            IsSetDefault = true;
            DebugWeaponState = "Set Default";
            GameTimeLastWeaponCheck = Game.GameTime;
        }
    }
    public void SetDeadly(bool ForceLong)
    {
        if (!IsSetDeadly && WeaponOwner.Pedestrian.Exists() && WeaponOwner.Pedestrian.IsAlive) //if ((!IsSetDeadly || NeedsWeaponCheck) && WeaponOwner.Pedestrian.Exists() && WeaponOwner.Pedestrian.IsAlive)
        {
            if (Melee != null && !NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(WeaponOwner.Pedestrian, (uint)Melee.GetHash(), false))
            {
                NativeFunction.Natives.GIVE_WEAPON_TO_PED(WeaponOwner.Pedestrian, (uint)Melee.GetHash(), 200, false, false);
                Melee.ApplyVariation(WeaponOwner.Pedestrian);
            }
            if (Sidearm != null && !NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(WeaponOwner.Pedestrian, (uint)Sidearm.GetHash(), false))
            {
                NativeFunction.Natives.GIVE_WEAPON_TO_PED(WeaponOwner.Pedestrian, (uint)Sidearm.GetHash(), 200, false, false);
                Sidearm.ApplyVariation(WeaponOwner.Pedestrian);
            }
            if (LongGun != null && !NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(WeaponOwner.Pedestrian, (uint)LongGun.GetHash(), false))
            {
                NativeFunction.Natives.GIVE_WEAPON_TO_PED(WeaponOwner.Pedestrian, (uint)LongGun.GetHash(), 200, false, false);
                LongGun.ApplyVariation(WeaponOwner.Pedestrian);
            }
            uint currentVehicleWeapon;
            bool hasVehicleWeapon = false;
            hasVehicleWeapon = NativeFunction.Natives.GET_CURRENT_PED_VEHICLE_WEAPON<bool>(WeaponOwner.Pedestrian, out currentVehicleWeapon);
            //3450622333 searchlight
            if(hasVehicleWeapon)
            {
                EntryPoint.WriteToConsole($"I AM {WeaponOwner.Handle} AND I HAVE A VEHICLE WEAPON {currentVehicleWeapon} HELI: {WeaponOwner.IsInHelicopter} DRVIER: {WeaponOwner.IsDriver}");
            }
            if (!hasVehicleWeapon)
            {
                uint currentWeapon;
                NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(WeaponOwner.Pedestrian, out currentWeapon, true);
                if (WeaponOwner.IsInHelicopter)//unarmed with no mounted weapon in a heli
                {
                    if (!WeaponOwner.IsDriver)
                    {
                        if (LongGun != null)
                        {
                            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", WeaponOwner.Pedestrian, LongGun.GetHash(), true);
                        }
                        else if(Sidearm != null)
                        {
                            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", WeaponOwner.Pedestrian, Sidearm.GetHash(), true);
                        }
                    }
                }
                else
                {
                    if (!WeaponOwner.IsDriver)
                    {
                        if (LongGun != null && (HasHeavyWeaponOnPerson || ForceLong))
                        {
                            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", WeaponOwner.Pedestrian, LongGun.GetHash(), true);
                        }
                        else if (Sidearm != null)
                        {
                            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", WeaponOwner.Pedestrian, Sidearm.GetHash(), true);
                        }
                    }
                }
            }
            NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", WeaponOwner.Pedestrian, true);//was false, but might need them to switch in vehicles and if hanging outside vehicle
            NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", WeaponOwner.Pedestrian, 2, true);//can do drivebys       
            IsSetLessLethal = false;
            IsSetUnarmed = false;
            IsSetDeadly = true;
            IsSetDefault = false;
            DebugWeaponState = "Set Deadly";
            GameTimeLastWeaponCheck = Game.GameTime;
        }
    }
    public void SetLessLethal()
    {
        if ((!IsSetLessLethal || NeedsWeaponCheck) && WeaponOwner.Pedestrian.Exists() && WeaponOwner.Pedestrian.IsAlive)
        {
            EntryPoint.WriteToConsole($"WeaponOwner {WeaponOwner.Handle} LESS LETHAL RAN IsSetLessLethal :{IsSetLessLethal}");
            if (!IsSetLessLethal)
            {
                NativeFunction.Natives.REMOVE_ALL_PED_WEAPONS(WeaponOwner.Pedestrian, false);
            }
            if (Melee != null)
            {
                if (!NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(WeaponOwner.Pedestrian, (uint)Melee.GetHash(), false))
                {
                    NativeFunction.Natives.GIVE_WEAPON_TO_PED(WeaponOwner.Pedestrian, (uint)Melee.GetHash(), 100, false, true);
                }
                uint currentWeapon;
                NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(WeaponOwner.Pedestrian, out currentWeapon, true);
                if (currentWeapon != (uint)Melee.GetHash())
                {
                    NativeFunction.Natives.SET_CURRENT_PED_WEAPON(WeaponOwner.Pedestrian, (uint)Melee.GetHash(), true);
                }
                NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", WeaponOwner.Pedestrian, false);
                NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", WeaponOwner.Pedestrian, 2, false);//cant do drivebys
            }
            else
            {
                uint currentWeapon;
                NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(WeaponOwner.Pedestrian, out currentWeapon, true);
                if (currentWeapon != 2725352035)
                {
                    NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", WeaponOwner.Pedestrian, 2725352035, true);
                    NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", WeaponOwner.Pedestrian, false);
                }
                NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", WeaponOwner.Pedestrian, 2, false);//cant do drivebys
            }    
            IsSetLessLethal = true;
            IsSetUnarmed = false;
            IsSetDeadly = false;
            IsSetDefault = false;
            DebugWeaponState = "Set Less Lethal";
            GameTimeLastWeaponCheck = Game.GameTime;
        }
    }
    public void SetUnarmed()
    {
        if ((!IsSetUnarmed || NeedsWeaponCheck) && WeaponOwner.Pedestrian.Exists() && WeaponOwner.Pedestrian.IsAlive) //if ((!IsSetUnarmed || NeedsWeaponCheck) && WeaponOwner.Pedestrian.Exists() && WeaponOwner.Pedestrian.IsAlive)
        {
            uint currentWeapon;
            NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(WeaponOwner.Pedestrian, out currentWeapon, true);
            if (currentWeapon != 2725352035)
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", WeaponOwner.Pedestrian, 2725352035, true);
                NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", WeaponOwner.Pedestrian, false);
            }
            NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", WeaponOwner.Pedestrian, 2, false);//cant do drivebys
            IsSetLessLethal = false;
            IsSetUnarmed = true;
            IsSetDeadly = false;
            IsSetDefault = false;
            DebugWeaponState = "Set Unarmed";
            GameTimeLastWeaponCheck = Game.GameTime;
        }
    }
    public void SetCompletelyUnarmed()
    {
        if (WeaponOwner.Pedestrian.Exists() && WeaponOwner.Pedestrian.IsAlive)
        {
            ShouldAutoSetWeaponState = false;
            NativeFunction.Natives.REMOVE_ALL_PED_WEAPONS(WeaponOwner.Pedestrian, false);
            uint currentWeapon;
            NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(WeaponOwner.Pedestrian, out currentWeapon, true);
            if (currentWeapon != 2725352035)
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", WeaponOwner.Pedestrian, 2725352035, true);
                NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", WeaponOwner.Pedestrian, false);
            }
            NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", WeaponOwner.Pedestrian, 2, false);//cant do drivebys
            IsSetLessLethal = false;
            IsSetUnarmed = true;
            IsSetDeadly = false;
            IsSetDefault = false;
            DebugWeaponState = "Set Unarmed";
            GameTimeLastWeaponCheck = Game.GameTime;
        }
    }
    public void Reset()
    {
        IsSetDeadly = false;
        IsSetLessLethal = false;
        IsSetUnarmed = false;
        IsSetDefault = false;
        ShouldAutoSetWeaponState = true;
    }
}

