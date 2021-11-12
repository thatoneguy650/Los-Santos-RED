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
    private Cop Cop;
    private uint GameTimeLastWeaponCheck;
    private bool IsSetDeadly;
    private bool IsSetLessLethal;
    private bool IsSetUnarmed;
    private bool IsSetDefault;
    private IssuableWeapon LongGun;
    private IssuableWeapon Sidearm;
    private bool HasHeavyWeaponOnPerson;
    private ISettingsProvideable Settings;
    private int DesiredAccuracy => IsSetLessLethal ? 30 : Settings.SettingsManager.PoliceSettings.GeneralAccuracy;
    public WeaponInventory(Cop cop, ISettingsProvideable settings)
    {
        Cop = cop;
        Settings = settings;
    }
    public bool NeedsWeaponCheck => GameTimeLastWeaponCheck == 0 || Game.GameTime > GameTimeLastWeaponCheck + 750;
    public bool ShouldAutoSetWeaponState { get; set; } = true;
    public string DebugWeaponState { get; set; }
    public bool HasPistol => Sidearm != null;
    public void IssueWeapons(IWeapons weapons)
    {
        Sidearm = Cop.AssignedAgency.GetRandomWeapon(true, weapons);
        LongGun = Cop.AssignedAgency.GetRandomWeapon(false, weapons);
    }
    public void UpdateLoadout(bool IsDeadlyChase, int WantedLevel)
    {
        if (ShouldAutoSetWeaponState && NeedsWeaponCheck)
        {
            if (Cop.CurrentTask?.Name == "AIApprehend")
            {     
                if (Cop.CurrentTask?.SubTaskName == "Fighting")
                {
                    SetDeadly(true);
                }
                else
                {
                    SetLessLethal();
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
                            if (WantedLevel < 4)
                            {
                                SetUnarmed();
                            }
                            else
                            {
                                SetDeadly(false);
                            }
                        }
                        else
                        {
                            SetDeadly(false);
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
                            SetLessLethal();
                        }
                    }
                }
            }
            if(Settings.SettingsManager.PoliceSettings.OverrideAccuracy)
            {
                Cop.Pedestrian.Accuracy = DesiredAccuracy;
            }
        }
    }
    public void SetDefault()
    {
        if ((!IsSetDefault || NeedsWeaponCheck) && Cop.Pedestrian.Exists() && Cop.Pedestrian.IsAlive)
        {
            EntryPoint.WriteToConsole($"COP EVENT {Cop.Pedestrian.Handle}: SETTING DEFAULT WEAPON LOADOUT", 3);
            if (!NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(Cop.Pedestrian, (uint)WeaponHash.StunGun, false))
            {
                NativeFunction.Natives.GIVE_WEAPON_TO_PED(Cop.Pedestrian, (uint)WeaponHash.StunGun, 100, false, false);
            }
            if (!NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(Cop.Pedestrian, (uint)Sidearm.GetHash(), false))
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
            if (Cop.IsInHelicopter || Cop.IsInAPC)
            {
                if (!IsSetDeadly)//only set this once?
                {
                    if (LongGun != null && !Cop.IsDriver)
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
                if (LongGun != null && (HasHeavyWeaponOnPerson || ForceLong) && !Cop.IsDriver)
                {
                    NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Cop.Pedestrian, LongGun.GetHash(), true);
                }
                else
                {
                    NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Cop.Pedestrian, Sidearm.GetHash(), true);
                }
                
            }
            NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Cop.Pedestrian, true);//was false, but might need them to switch in vehicles and if hanging outside vehicle
            //NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Pedestrian, 1, true);//can use vehicle in combat
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
            if (!NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(Cop.Pedestrian, (uint)WeaponHash.StunGun, false))//if (Cop.Pedestrian.Inventory != null && Cop.Pedestrian.Inventory.Weapons != null && !Cop.Pedestrian.Inventory.Weapons.Contains(WeaponHash.StunGun))
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
            //NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Pedestrian, 1, false);//cant use vehicle in combat
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
            if (currentWeapon != 2725352035)//if (Cop.Pedestrian.Inventory != null && Cop.Pedestrian.Inventory.Weapons != null && Cop.Pedestrian.Inventory.EquippedWeapon != null)
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Cop.Pedestrian, 2725352035, true);
                NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Cop.Pedestrian, false);
            }
            // NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Pedestrian, 1, false);//cant use vehicle in combat
            NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Cop.Pedestrian, 2, false);//cant do drivebys
            IsSetLessLethal = false;
            IsSetUnarmed = true;
            IsSetDeadly = false;
            IsSetDefault = false;
            DebugWeaponState = "Set Unarmed";
            GameTimeLastWeaponCheck = Game.GameTime;
        }
    }
}

