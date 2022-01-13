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
    private IWeaponIssuable Cop;
    private uint GameTimeLastWeaponCheck;
    private bool IsSetDeadly;
    private bool IsSetLessLethal;
    private bool IsSetUnarmed;
    private bool IsSetDefault;
    public IssuableWeapon LongGun { get; private set; }
    public IssuableWeapon Sidearm { get; private set; }
    private bool HasHeavyWeaponOnPerson;
    private ISettingsProvideable Settings;
    private int DesiredAccuracy => IsSetLessLethal ? 30 : Settings.SettingsManager.PoliceSettings.GeneralAccuracy;
    public WeaponInventory(IWeaponIssuable cop, ISettingsProvideable settings)
    {
        Cop = cop;
        Settings = settings;
    }
    public bool NeedsWeaponCheck => GameTimeLastWeaponCheck == 0 || Game.GameTime > GameTimeLastWeaponCheck + 750;
    public bool ShouldAutoSetWeaponState { get; set; } = true;
    public string DebugWeaponState { get; set; }
    public bool HasPistol => Sidearm != null;
    public void IssueWeapons(IWeapons weapons, uint meleeHash, bool issueSidearm, bool issueLongGun)
    {
        if (meleeHash != 0)
        {
            if (!NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(Cop.Pedestrian, meleeHash, false))//(uint)WeaponHash.StunGun
            {
                NativeFunction.Natives.GIVE_WEAPON_TO_PED(Cop.Pedestrian, meleeHash, 100, false, false);
            }
        }
        if (issueSidearm)
        {
            Sidearm = Cop.GetRandomWeapon(true, weapons);
            if (Sidearm != null && !NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(Cop.Pedestrian, (uint)Sidearm.GetHash(), false))
            {
                NativeFunction.Natives.GIVE_WEAPON_TO_PED(Cop.Pedestrian, (uint)Sidearm.GetHash(), 200, false, false);
                Sidearm.ApplyVariation(Cop.Pedestrian);
            }
        }
        if (issueLongGun)
        {
            LongGun = Cop.GetRandomWeapon(false, weapons);
            if (LongGun != null && !NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(Cop.Pedestrian, (uint)LongGun.GetHash(), false))
            {
                NativeFunction.Natives.GIVE_WEAPON_TO_PED(Cop.Pedestrian, (uint)LongGun.GetHash(), 200, false, false);
                LongGun.ApplyVariation(Cop.Pedestrian);
            }
        }
        NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Cop.Pedestrian, true);//was false, but might need them to switch in vehicles and if hanging outside vehicle
        NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Cop.Pedestrian, 2, true);//can do drivebys    
    }
    public void UpdateLoadout(bool PlayerInVehicle, bool IsDeadlyChase, int WantedLevel, bool isAttemptingToSurrender, bool isBusted, bool isWeaponsFree, bool hasShotAtPolice, bool lethalForceAuthorized)
    {
        if (Cop.Pedestrian.Exists())
        {
            uint currentVehicleWeapon;
            bool hasVehicleWeapon = false;
            hasVehicleWeapon = NativeFunction.Natives.GET_CURRENT_PED_VEHICLE_WEAPON<bool>(Cop.Pedestrian, out currentVehicleWeapon);
            //3450622333 searchlight
            if (hasVehicleWeapon && currentVehicleWeapon == 3450622333)//searchlight
            {
                return;
            }
            if (ShouldAutoSetWeaponState && NeedsWeaponCheck)
            {
                if (Cop.CurrentTask?.Name == "AIApprehend")
                {
                    HasHeavyWeaponOnPerson = true;
                    if (Cop.CurrentTask.OtherTarget != null && Cop.CurrentTask.OtherTarget.IsDeadlyChase)
                    {
                        SetDeadly(true);
                    }
                    else
                    {
                        if (Cop.IsInVehicle)
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
                        if (!IsSetDefault)
                        {
                            SetDefault();
                            HasHeavyWeaponOnPerson = false;
                        }
                    }
                    else
                    {
                        if (IsDeadlyChase)
                        {
                            if (Cop.IsInVehicle)
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
                            if (Cop.IsInVehicle)
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
                                    SetLessLethal();
                                }
                                //SetDefault();
                            }
                        }
                        else
                        {
                            if (Cop.IsInVehicle)
                            {
                                SetUnarmed();
                            }
                            else
                            {
                                //if(PlayerInVehicle)
                                //{
                                //    SetUnarmed();
                                //}
                                //else
                                //{
                                SetLessLethal();
                                //}
                            }
                        }
                    }
                }
                //if (Settings.SettingsManager.PoliceSettings.OverrideAccuracy)
                //{
                //    if(Cop.CurrentTask?.Name == "AIApprehend")
                //    {
                //        Cop.Pedestrian.Accuracy = 95;//they gonna get FUCKED UP, player WATCH OUT!
                //        NativeFunction.Natives.SET_PED_SHOOT_RATE(Cop.Pedestrian, 1000);
                //    }
                //    else if (Cop.IsInVehicle)
                //    {
                //        Cop.Pedestrian.Accuracy = Settings.SettingsManager.PoliceSettings.VehicleAccuracy;
                //        NativeFunction.Natives.SET_PED_SHOOT_RATE(Cop.Pedestrian, Settings.SettingsManager.PoliceSettings.VehicleShootRate);
                //    }
                //    else if (IsSetLessLethal)
                //    {
                //        Cop.Pedestrian.Accuracy = Settings.SettingsManager.PoliceSettings.TaserAccuracy;
                //        NativeFunction.Natives.SET_PED_SHOOT_RATE(Cop.Pedestrian, Settings.SettingsManager.PoliceSettings.TaserShootRate);
                //    }
                //    else
                //    {
                //        Cop.Pedestrian.Accuracy = Settings.SettingsManager.PoliceSettings.GeneralAccuracy;
                //        NativeFunction.Natives.SET_PED_SHOOT_RATE(Cop.Pedestrian, Settings.SettingsManager.PoliceSettings.GeneralShootRate);
                //    }
                //}
                if (Settings.SettingsManager.PoliceSettings.OverrideAccuracy)
                {
                    if (Cop.CurrentTask?.Name == "AIApprehend")
                    {
                        Cop.Pedestrian.Accuracy = 95;//they gonna get FUCKED UP, player WATCH OUT!
                        NativeFunction.Natives.SET_PED_SHOOT_RATE(Cop.Pedestrian, 1000);
                        NativeFunction.Natives.SET_PED_COMBAT_ABILITY(Cop.Pedestrian, 2);
                    }
                    else if (Cop.IsInVehicle)
                    {
                        Cop.Pedestrian.Accuracy = Settings.SettingsManager.PoliceSettings.VehicleAccuracy;
                        NativeFunction.Natives.SET_PED_SHOOT_RATE(Cop.Pedestrian, Settings.SettingsManager.PoliceSettings.VehicleShootRate);
                        NativeFunction.Natives.SET_PED_COMBAT_ABILITY(Cop.Pedestrian, Cop.CombatAbility);
                    }
                    else if (IsSetLessLethal)
                    {
                        Cop.Pedestrian.Accuracy = Settings.SettingsManager.PoliceSettings.TaserAccuracy;
                        NativeFunction.Natives.SET_PED_SHOOT_RATE(Cop.Pedestrian, Settings.SettingsManager.PoliceSettings.TaserShootRate);
                        NativeFunction.Natives.SET_PED_COMBAT_ABILITY(Cop.Pedestrian, Cop.CombatAbility);
                    }
                    else
                    {
                        Cop.Pedestrian.Accuracy = Cop.Accuracy;//Settings.SettingsManager.PoliceSettings.GeneralAccuracy;
                        NativeFunction.Natives.SET_PED_SHOOT_RATE(Cop.Pedestrian, Cop.ShootRate);// Settings.SettingsManager.PoliceSettings.GeneralShootRate);
                        NativeFunction.Natives.SET_PED_COMBAT_ABILITY(Cop.Pedestrian, Cop.CombatAbility);
                    }
                }
            }
        }
    }
    public void SetDefault()
    {
        if ((!IsSetDefault || NeedsWeaponCheck) && Cop.Pedestrian.Exists() && Cop.Pedestrian.IsAlive)
        {
            //EntryPoint.WriteToConsole($"COP EVENT {Cop.Pedestrian.Handle}: SETTING DEFAULT WEAPON LOADOUT", 3);
            if (!NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(Cop.Pedestrian, (uint)WeaponHash.StunGun, false))
            {
                NativeFunction.Natives.GIVE_WEAPON_TO_PED(Cop.Pedestrian, (uint)WeaponHash.StunGun, 100, false, false);
            }
            if (Sidearm != null && !NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(Cop.Pedestrian, (uint)Sidearm.GetHash(), false))
            {
                NativeFunction.Natives.GIVE_WEAPON_TO_PED(Cop.Pedestrian, (uint)Sidearm.GetHash(), 200, false, false);
                Sidearm.ApplyVariation(Cop.Pedestrian);
            }
            NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Cop.Pedestrian, true);//was false, but might need them to switch in vehicles and if hanging outside vehicle
            NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Cop.Pedestrian, 2, true);//can do drivebys       
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
        if ((!IsSetDeadly || NeedsWeaponCheck) && Cop.Pedestrian.Exists() && Cop.Pedestrian.IsAlive)
        {
            if (!NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(Cop.Pedestrian, (uint)Sidearm.GetHash(), false))
            {
                NativeFunction.Natives.GIVE_WEAPON_TO_PED(Cop.Pedestrian, (uint)Sidearm.GetHash(), 200, false, false);
                Sidearm.ApplyVariation(Cop.Pedestrian);
            }
            if (!NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(Cop.Pedestrian, (uint)LongGun.GetHash(), false))
            {
                NativeFunction.Natives.GIVE_WEAPON_TO_PED(Cop.Pedestrian, (uint)LongGun.GetHash(), 200, false, false);
                LongGun.ApplyVariation(Cop.Pedestrian);
            }
            uint currentVehicleWeapon;
            bool hasVehicleWeapon = false;
            hasVehicleWeapon = NativeFunction.Natives.GET_CURRENT_PED_VEHICLE_WEAPON<bool>(Cop.Pedestrian, out currentVehicleWeapon);
            //3450622333 searchlight
            if (!hasVehicleWeapon)
            {
                uint currentWeapon;
                NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(Cop.Pedestrian, out currentWeapon, true);
                if (Cop.IsInHelicopter)//unarmed with no mounted weapon in a heli
                {
                    if (!Cop.IsDriver)
                    {
                        if (LongGun != null)
                        {
                            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Cop.Pedestrian, LongGun.GetHash(), true);
                        }
                        else
                        {
                            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Cop.Pedestrian, Sidearm.GetHash(), true);
                        }
                    }
                }
                else
                {
                    if (!Cop.IsDriver)
                    {
                        if (LongGun != null && (HasHeavyWeaponOnPerson || ForceLong))
                        {
                            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Cop.Pedestrian, LongGun.GetHash(), true);
                        }
                        else
                        {
                            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Cop.Pedestrian, Sidearm.GetHash(), true);
                        }
                    }
                }
            }
            NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Cop.Pedestrian, true);//was false, but might need them to switch in vehicles and if hanging outside vehicle
            NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Cop.Pedestrian, 2, true);//can do drivebys       
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
        if ((!IsSetLessLethal || NeedsWeaponCheck) && Cop.Pedestrian.Exists() && Cop.Pedestrian.IsAlive)
        {
            if (!NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(Cop.Pedestrian, (uint)WeaponHash.StunGun, false))
            {
                NativeFunction.Natives.GIVE_WEAPON_TO_PED(Cop.Pedestrian, (uint)WeaponHash.StunGun, 100, false, true);
            }
            uint currentWeapon;
            NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(Cop.Pedestrian, out currentWeapon, true);
            if(currentWeapon != (uint)WeaponHash.StunGun)
            {
                NativeFunction.Natives.SET_CURRENT_PED_WEAPON(Cop.Pedestrian, (uint)WeaponHash.StunGun, true);
            }
            NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Cop.Pedestrian, false);
            NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Cop.Pedestrian, 2, false);//cant do drivebys
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
        if ((!IsSetUnarmed || NeedsWeaponCheck) && Cop.Pedestrian.Exists() && Cop.Pedestrian.IsAlive)
        {
            uint currentWeapon;
            NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(Cop.Pedestrian, out currentWeapon, true);
            if (currentWeapon != 2725352035)
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Cop.Pedestrian, 2725352035, true);
                NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Cop.Pedestrian, false);
            }
            NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Cop.Pedestrian, 2, false);//cant do drivebys
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
        if (Cop.Pedestrian.Exists() && Cop.Pedestrian.IsAlive)
        {
            ShouldAutoSetWeaponState = false;
            NativeFunction.Natives.REMOVE_ALL_PED_WEAPONS(Cop.Pedestrian, false);
            uint currentWeapon;
            NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(Cop.Pedestrian, out currentWeapon, true);
            if (currentWeapon != 2725352035)
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Cop.Pedestrian, 2725352035, true);
                NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Cop.Pedestrian, false);
            }
            NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Cop.Pedestrian, 2, false);//cant do drivebys
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

