using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

public class WeaponInventory
{
    private readonly uint UnarmedHash = 2725352035;
    private ISettingsProvideable Settings;
    private uint CurrentVehicleWeapon;
    private bool HasVehicleWeapon;
    private IPoliceRespondable Player;
    private IWeaponIssuable WeaponOwner;
    private uint GameTimeLastWeaponCheck;
    private bool IsHolsterFull;
    private PedComponent EmptyHolster;
    private PedComponent FullHolster;
    private uint CurrentWeapon;
    private bool HasSearchlight;

    public IssuableWeapon LongGun { get; private set; }
    public IssuableWeapon Sidearm { get; private set; }
    public IssuableWeapon Melee { get; private set; }
    public bool IsSetDeadly { get; private set; }
    public bool IsSetLessLethal { get; private set; }
    public bool IsSetUnarmed { get; private set; }
    public bool IsSetDefault { get; private set; }
    public bool HasHeavyWeaponOnPerson { get; private set; }
    public bool NeedsWeaponCheck => GameTimeLastWeaponCheck == 0 || Game.GameTime > GameTimeLastWeaponCheck + 750;
    public bool ShouldAutoSetWeaponState { get; set; } = true;
    public string DebugWeaponState { get; set; }
    public bool HasPistol => Sidearm != null;
    public bool HasLongGun => LongGun != null;

    public bool IsArmed => HasPistol || HasLongGun;
    public bool CanRadioIn => IsSetLessLethal || (IsSetDeadly && !HasHeavyWeaponOnPerson) || (IsSetDefault && !HasHeavyWeaponOnPerson);
    public WeaponInventory(IWeaponIssuable weaponOwner, ISettingsProvideable settings)
    {
        WeaponOwner = weaponOwner;
        Settings = settings;
    }
    public WeaponInventory(IWeaponIssuable weaponOwner, ISettingsProvideable settings, PedComponent emptyHolster, PedComponent fullHolster)
    {
        WeaponOwner = weaponOwner;
        Settings = settings;
        EmptyHolster = emptyHolster;
        FullHolster = fullHolster;
    }
    public void IssueWeapons(IWeapons weapons, bool issueMelee, bool issueSidearm, bool issueLongGun, DispatchablePerson dispatchablePerson)// PedComponent emptyHolster, PedComponent fullHolster,IssuableWeapon meleeOverride,IssuableWeapon sidearmOverride,IssuableWeapon longGunOverride)
    {
        //EntryPoint.WriteToConsole($" IssueWeapons issueMelee{issueMelee} issueSidearm {issueSidearm} issueLongGun {issueLongGun}");
        bool hasOVerride = false;
        if (issueMelee)
        {
            if(dispatchablePerson?.OverrideAgencyLessLethalWeapons == true)
            {
                Melee = dispatchablePerson?.OverrideLessLethalWeapons?.PickRandom();
               // EntryPoint.WriteToConsole($"IssueWeapons Melee Override {Melee?.ModelName}");
            }
            else
            {
                Melee = WeaponOwner.GetRandomMeleeWeapon(weapons);
            }     
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
            if (dispatchablePerson?.OverrideAgencySideArms == true)
            {
                Sidearm = dispatchablePerson?.OverrideSideArms?.PickRandom();
                if (Sidearm != null)
                {
                    WeaponInformation WeaponLookup = weapons.GetWeapon(Sidearm?.ModelName);
                    if (WeaponLookup != null)
                    {
                        Sidearm.SetIssued(Game.GetHashKey(Sidearm.ModelName), WeaponLookup.PossibleComponents, WeaponLookup.IsTaser);
                    }
                   // EntryPoint.WriteToConsole($"IssueWeapons Sidearm Override {Sidearm?.ModelName}");
                }
            }
            else
            {
                //EntryPoint.WriteToConsole($"IssueWeapons issueSidearm RAN");
                Sidearm = WeaponOwner.GetRandomWeapon(true, weapons);
            }    
            if (Sidearm != null && !NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(WeaponOwner.Pedestrian, (uint)Sidearm.GetHash(), false))
            {
                NativeFunction.Natives.GIVE_WEAPON_TO_PED(WeaponOwner.Pedestrian, (uint)Sidearm.GetHash(), 200, false, false);
                Sidearm.ApplyVariation(WeaponOwner.Pedestrian);
            }
        }
        if (issueLongGun)
        {
            if (dispatchablePerson?.OverrideAgencyLongGuns == true)
            {
                hasOVerride = true;
                LongGun = dispatchablePerson?.OverrideLongGuns?.PickRandom();
                if (LongGun != null)
                {
                    WeaponInformation WeaponLookup = weapons.GetWeapon(LongGun?.ModelName);
                    if (WeaponLookup != null)
                    {
                        LongGun.SetIssued(Game.GetHashKey(LongGun.ModelName), WeaponLookup.PossibleComponents, WeaponLookup.IsTaser);
                    }
                }
                //EntryPoint.WriteToConsole($"IssueWeapons LongGun Override {LongGun?.ModelName}");
            }
            else
            {
                //EntryPoint.WriteToConsole($"IssueWeapons issueLongGun RAN");
                LongGun = WeaponOwner.GetRandomWeapon(false, weapons);
            }     
            if (LongGun != null && !NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(WeaponOwner.Pedestrian, (uint)LongGun.GetHash(), false))
            {
                NativeFunction.Natives.GIVE_WEAPON_TO_PED(WeaponOwner.Pedestrian, (uint)LongGun.GetHash(), 200, false, false);
                LongGun.ApplyVariation(WeaponOwner.Pedestrian);
                //EntryPoint.WriteToConsole($"IssueWeapons LongGun GIVING WEAPON TO PED hasOVerride?{hasOVerride} {LongGun.ModelName} HASH{(uint)LongGun.GetHash()}");
            }
        }
        if (dispatchablePerson != null)
        {
            EmptyHolster = dispatchablePerson.EmptyHolster;
            FullHolster = dispatchablePerson.FullHolster;
        }
        NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", WeaponOwner.Pedestrian, true);//was false, but might need them to switch in vehicles and if hanging outside vehicle
        NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", WeaponOwner.Pedestrian, 2, true);//can do drivebys    
        NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", WeaponOwner.Pedestrian, 3, true);//can leave vehicle

    }
    public void UpdateLoadout(IPoliceRespondable policeRespondablePlayer, IEntityProvideable world, bool isNightTime, bool overrideAccuracy)
    {
        Player = policeRespondablePlayer;
        if(!WeaponOwner.Pedestrian.Exists() || Player == null)
        {
            return;
        }
        //EntryPoint.WriteToConsole($"{WeaponOwner.Handle} UPDATING LOADOUT");
        GetVehicleWeapon();


        if (HasVehicleWeapon && CurrentVehicleWeapon == 3450622333 || (HasSearchlight && WeaponOwner.IsDriver))//searchlight
        {
            //EntryPoint.WriteToConsole($"{WeaponOwner.Handle} HANDLE SEARCHLIGHT IS RUNNING");
            HandleSearchlight(isNightTime);
            return;
        }
        if (ShouldAutoSetWeaponState && NeedsWeaponCheck)
        {
            if (WeaponOwner.CurrentTask?.Name == "AIApprehend")
            {
                AutoSetWeapons_AI(world);
            }
            else
            {
                if(WeaponOwner.PedAlerts.IsAlerted)
                {
                    if (Player.IsNotWanted || Player.PoliceResponse.IsDeadlyChase)
                    {
                        AutoSetWeapons_Deadly();
                    }
                    else
                    {
                        AutoSetWeapons_Other();
                    }
                }
                else if (Player.IsNotWanted)
                {
                    AutoSetWeapons_Default();
                }
                else
                {
                    if (Player.PoliceResponse.IsDeadlyChase)
                    {
                        AutoSetWeapons_Deadly();
                    }
                    else if (Player.IsBusted)
                    {
                        AutoSetWeapons_Busted();
                    }
                    else
                    {
                        AutoSetWeapons_Other();
                    }
                }
            }
            if (overrideAccuracy)
            {
                UpdateSettings();
            }
        }   
    }
    private void AutoSetWeapons_AI(IEntityProvideable world)
    {
        HasHeavyWeaponOnPerson = true;
        if (WeaponOwner.CurrentTask.OtherTarget != null && WeaponOwner.CurrentTask.OtherTarget.IsDeadlyChase)
        {
            SetDeadly(true);
        }
        else
        {
            if(Player.WantedLevel >= 2 && Player.PoliceResponse.IsDeadlyChase)
            {
                SetDeadly(false);
            }
            else if (WeaponOwner.IsInVehicle)
            {
                SetUnarmed();
            }
            else
            {
                SetLessLethal();
            }
        }
    }
    private void AutoSetWeapons_Default()
    {
        if (!IsSetDefault && !IsSetUnarmed)
        {
            SetDefault();
            if (!WeaponOwner.AlwaysHasLongGun)
            {
                HasHeavyWeaponOnPerson = false;
            }
        }
    }
    private void AutoSetWeapons_Deadly()
    {
        if (WeaponOwner.IsInVehicle)
        {
            HasHeavyWeaponOnPerson = true;
            if (Player.PoliceResponse.IsWeaponsFree)
            {
                SetDeadly(false);
            }
            else
            {
                if (Player.IsAttemptingToSurrender)
                {
                    SetUnarmed();
                }
                else if (!Player.PoliceResponse.HasShotAtPolice && Player.WantedLevel <= 4)
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
    private void AutoSetWeapons_Busted()
    {
        if (WeaponOwner.IsInVehicle)
        {
            SetUnarmed();
        }
        else
        {
            if (Player.PoliceResponse.LethalForceAuthorized || Player.WasDangerouslyArmedWhenBusted)
            {
                SetDeadly(false);
            }
            else
            {
                if (Player.WantedLevel >= 2)
                {
                    if (Player.IsDangerouslyArmed)
                    {
                        SetDeadly(false);
                    }
                    else
                    {
                        SetLessLethal();
                    }
                }
                else
                {
                    GameTimeLastWeaponCheck = Game.GameTime;
                    SetUnarmed();
                }
            }
        }
        
    }
    private void AutoSetWeapons_Other()
    {
        if (WeaponOwner.IsInVehicle || Player.WantedLevel == 1)
        {
            SetUnarmed();
        }
        else
        {
            if (Player.IsDangerouslyArmed)
            {
                SetDeadly(false);
            }
            else
            {
                SetLessLethal();
            }
        }
        if (WeaponOwner.IsInVehicle && Player.IsDangerouslyArmed)
        {
            HasHeavyWeaponOnPerson = true;
        }
    }
    private void GetVehicleWeapon()
    {
        HasVehicleWeapon = NativeFunction.Natives.GET_CURRENT_PED_VEHICLE_WEAPON<bool>(WeaponOwner.Pedestrian, out CurrentVehicleWeapon);//3450622333 searchlight
        WeaponOwner.IsUsingMountedWeapon = HasVehicleWeapon;
        HasSearchlight = WeaponOwner.Pedestrian.CurrentVehicle.Exists() && NativeFunction.Natives.DOES_VEHICLE_HAVE_SEARCHLIGHT<bool>(WeaponOwner.Pedestrian.CurrentVehicle);
    }
    public void UpdateSettings()
    {
        if (WeaponOwner.Pedestrian.Exists())
        {
            if (WeaponOwner.IsInVehicle)
            {
                if(HasVehicleWeapon)
                {
                    WeaponOwner.Pedestrian.Accuracy = WeaponOwner.TurretAccuracy;
                    NativeFunction.Natives.SET_PED_SHOOT_RATE(WeaponOwner.Pedestrian, WeaponOwner.TurretShootRate);
                }
                else
                {
                    WeaponOwner.Pedestrian.Accuracy = WeaponOwner.VehicleAccuracy;
                    NativeFunction.Natives.SET_PED_SHOOT_RATE(WeaponOwner.Pedestrian, WeaponOwner.VehicleShootRate);
                }
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
        if(IsSetDefault || !WeaponOwner.Pedestrian.Exists() || !WeaponOwner.Pedestrian.IsAlive)
        {
            return;
        }
        if (Melee != null && !NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(WeaponOwner.Pedestrian, (uint)Melee.GetHash(), false))
        {
            NativeFunction.Natives.GIVE_WEAPON_TO_PED(WeaponOwner.Pedestrian, (uint)Melee.GetHash(), 100, false, false);
        }
        if (Sidearm != null && !NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(WeaponOwner.Pedestrian, (uint)Sidearm.GetHash(), false))
        {
            NativeFunction.Natives.GIVE_WEAPON_TO_PED(WeaponOwner.Pedestrian, (uint)Sidearm.GetHash(), 200, false, false);
            Sidearm.ApplyVariation(WeaponOwner.Pedestrian);
        }
        if (HasHeavyWeaponOnPerson && LongGun != null && !NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(WeaponOwner.Pedestrian, (uint)LongGun.GetHash(), false))
        {
            NativeFunction.Natives.GIVE_WEAPON_TO_PED(WeaponOwner.Pedestrian, (uint)LongGun.GetHash(), 200, false, false);
            LongGun.ApplyVariation(WeaponOwner.Pedestrian);
        }
        GetCurrentWeapon();



        if (LongGun != null && WeaponOwner.LocationTaskRequirements.TaskRequirements.HasFlag(TaskRequirements.EquipLongGunWhenIdle))
        {
            NativeFunction.Natives.SET_CURRENT_PED_WEAPON(WeaponOwner.Pedestrian, LongGun.GetHash(), true);
        }
        else if (Sidearm != null && WeaponOwner.LocationTaskRequirements.TaskRequirements.HasFlag(TaskRequirements.EquipSidearmWhenIdle))
        {
            NativeFunction.Natives.SET_CURRENT_PED_WEAPON(WeaponOwner.Pedestrian, Sidearm.GetHash(), true);
        }
        else if (Melee != null && WeaponOwner.LocationTaskRequirements.TaskRequirements.HasFlag(TaskRequirements.EquipMeleeWhenIdle))
        {
            NativeFunction.Natives.SET_CURRENT_PED_WEAPON(WeaponOwner.Pedestrian, Melee.GetHash(), true);
        }
        else if (CurrentWeapon != 2725352035 && CurrentWeapon != 966099553)//unarmed and notepad
        {
            NativeFunction.Natives.SET_CURRENT_PED_WEAPON(WeaponOwner.Pedestrian, 2725352035, true);
        }

        NativeFunction.Natives.SET_PED_CAN_SWITCH_WEAPON(WeaponOwner.Pedestrian, true);
        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(WeaponOwner.Pedestrian, (int)eCombat_Attribute.CA_DO_DRIVEBYS, true);
        IsSetLessLethal = false;
        IsSetUnarmed = false;
        IsSetDeadly = false;
        IsSetDefault = true;
        if(!IsHolsterFull)
        {
            HolsterPistol();
        }
        //EntryPoint.WriteToConsole($"{WeaponOwner.Handle} SET DEFAULT");
        DebugWeaponState = "Set Default";
        GameTimeLastWeaponCheck = Game.GameTime;      
    }
    public void SetDefaultArmed()
    {
        SetDefault();
        if (LongGun != null && HasHeavyWeaponOnPerson)
        {
            NativeFunction.Natives.SET_CURRENT_PED_WEAPON(WeaponOwner.Pedestrian, LongGun.GetHash(), true);
        }
        else if (Sidearm != null)
        {
            NativeFunction.Natives.SET_CURRENT_PED_WEAPON(WeaponOwner.Pedestrian, Sidearm.GetHash(), true);
        }
        else if (Melee != null)
        {
            NativeFunction.Natives.SET_CURRENT_PED_WEAPON(WeaponOwner.Pedestrian, Melee.GetHash(), true);
        }
    }
    public void SetDeadly(bool ForceLong)
    {
        if(IsSetDeadly || !WeaponOwner.Pedestrian.Exists() || !WeaponOwner.Pedestrian.IsAlive)
        {
            return;
        }
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
        GetVehicleWeapon();
        if (HasVehicleWeapon)
        {
            //EntryPoint.WriteToConsole($"I AM {WeaponOwner.Handle} AND I HAVE A VEHICLE WEAPON {CurrentVehicleWeapon} HELI: {WeaponOwner.IsInHelicopter} DRVIER: {WeaponOwner.IsDriver}");
            //NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", WeaponOwner.Pedestrian, 3, false);//can leave vehicle
        }
        else
        {
            //EntryPoint.WriteToConsole($"I AM {WeaponOwner.Handle} AND I DO NOT HAVE A VEHICLE WEAPON {CurrentVehicleWeapon} HELI: {WeaponOwner.IsInHelicopter} DRVIER: {WeaponOwner.IsDriver}");
            //NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", WeaponOwner.Pedestrian, 3, true);//can leave vehicle
            GetCurrentWeapon();
            if (WeaponOwner.IsInHelicopter)//unarmed with no mounted weapon in a heli
            {
                if (!WeaponOwner.IsDriver)
                {
                    if (LongGun != null)
                    {
                        NativeFunction.Natives.SET_CURRENT_PED_WEAPON(WeaponOwner.Pedestrian, LongGun.GetHash(), true);
                    }
                    else if (Sidearm != null)
                    {
                        NativeFunction.Natives.SET_CURRENT_PED_WEAPON(WeaponOwner.Pedestrian, Sidearm.GetHash(), true);
                    }
                }
            }
            else
            {
                if (!WeaponOwner.IsDriver)
                {
                    if (LongGun != null && (HasHeavyWeaponOnPerson || ForceLong))
                    {
                        NativeFunction.Natives.SET_CURRENT_PED_WEAPON(WeaponOwner.Pedestrian, LongGun.GetHash(), true);
                    }
                    else if (Sidearm != null)
                    {
                        NativeFunction.Natives.SET_CURRENT_PED_WEAPON(WeaponOwner.Pedestrian, Sidearm.GetHash(), true);
                    }
                }
            }
        }
        NativeFunction.Natives.SET_PED_CAN_SWITCH_WEAPON(WeaponOwner.Pedestrian, true);
        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(WeaponOwner.Pedestrian, (int)eCombat_Attribute.CA_DO_DRIVEBYS, true);
        if (HasHeavyWeaponOnPerson && !IsHolsterFull)
        {
            HolsterPistol();
        }
        else if (!HasHeavyWeaponOnPerson && IsHolsterFull)
        {
            UnHolsterPistol();
        }
        IsSetLessLethal = false;
        IsSetUnarmed = false;
        IsSetDeadly = true;
        IsSetDefault = false;
        //EntryPoint.WriteToConsole($"{WeaponOwner.Handle} SET DEADLY");
        DebugWeaponState = "Set Deadly";
        GameTimeLastWeaponCheck = Game.GameTime;    
    }
    private void GetCurrentWeapon()
    {
        NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(WeaponOwner.Pedestrian, out CurrentWeapon, true);
    }
    public void SetLessLethal()
    {
        if(!(!IsSetLessLethal || NeedsWeaponCheck) || !WeaponOwner.Pedestrian.Exists() || !WeaponOwner.Pedestrian.IsAlive)
        {
            return;
        }
        if (!IsSetLessLethal)
        {
            NativeFunction.Natives.REMOVE_ALL_PED_WEAPONS(WeaponOwner.Pedestrian, false);
        }
        GetCurrentWeapon();
        if (Melee != null)
        {
            if (!NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(WeaponOwner.Pedestrian, (uint)Melee.GetHash(), false))
            {
                NativeFunction.Natives.GIVE_WEAPON_TO_PED(WeaponOwner.Pedestrian, (uint)Melee.GetHash(), 100, false, true);
            }       
            NativeFunction.Natives.SET_CURRENT_PED_WEAPON(WeaponOwner.Pedestrian, (uint)Melee.GetHash(), true);
        }
        else
        {
            if (CurrentWeapon != UnarmedHash)//unarmed
            {
                NativeFunction.Natives.SET_CURRENT_PED_WEAPON(WeaponOwner.Pedestrian, UnarmedHash, true);
            }
        }
        NativeFunction.Natives.SET_PED_CAN_SWITCH_WEAPON(WeaponOwner.Pedestrian, false);
        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(WeaponOwner.Pedestrian, (int)eCombat_Attribute.CA_DO_DRIVEBYS, false);
        if (!IsHolsterFull)
        {
            HolsterPistol();
        }
        IsSetLessLethal = true;
        IsSetUnarmed = false;
        IsSetDeadly = false;
        IsSetDefault = false;
        //EntryPoint.WriteToConsole($"{WeaponOwner.Handle} SET LESS LETHAL");
        DebugWeaponState = "Set Less Lethal";
        GameTimeLastWeaponCheck = Game.GameTime;     
    }
    public void SetUnarmed()
    {
        if(!(!IsSetUnarmed || NeedsWeaponCheck) || !WeaponOwner.Pedestrian.Exists() || !WeaponOwner.Pedestrian.IsAlive)
        {
            return;
        }
        GetCurrentWeapon();
        if (CurrentWeapon != UnarmedHash)
        {
            NativeFunction.Natives.SET_CURRENT_PED_WEAPON(WeaponOwner.Pedestrian, UnarmedHash, true);
            NativeFunction.Natives.SET_PED_CAN_SWITCH_WEAPON(WeaponOwner.Pedestrian, false);
        }
        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(WeaponOwner.Pedestrian, (int)eCombat_Attribute.CA_DO_DRIVEBYS, false);
        if (!IsHolsterFull)
        {
            HolsterPistol();
        }
        IsSetLessLethal = false;
        IsSetUnarmed = true;
        IsSetDeadly = false;
        IsSetDefault = false;
        //EntryPoint.WriteToConsole($"{WeaponOwner.Handle} SET UNARMED");
        DebugWeaponState = "Set Unarmed";
        GameTimeLastWeaponCheck = Game.GameTime;    
    }
    public void SetSimpleUnarmed()
    {
        if(!WeaponOwner.Pedestrian.Exists() || !WeaponOwner.Pedestrian.IsAlive)
        {
            return;
        }
        GetCurrentWeapon();
        if (CurrentWeapon != UnarmedHash)
        {
            NativeFunction.Natives.SET_CURRENT_PED_WEAPON(WeaponOwner.Pedestrian, UnarmedHash, true);
            NativeFunction.Natives.SET_PED_CAN_SWITCH_WEAPON(WeaponOwner.Pedestrian, false);
        }
        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(WeaponOwner.Pedestrian, (int)eCombat_Attribute.CA_DO_DRIVEBYS, true);
    }
    public void SetSimpleArmed()
    {
        if (!WeaponOwner.Pedestrian.Exists() || !WeaponOwner.Pedestrian.IsAlive)
        {
            return;
        }
        //EntryPoint.WriteToConsole("SetSimpleArmed");
        uint bestWeapon = NativeFunction.Natives.GET_BEST_PED_WEAPON<uint>(WeaponOwner.Pedestrian);
        uint currentWeapon;
        NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(WeaponOwner.Pedestrian, out currentWeapon, true);
        if (currentWeapon != bestWeapon)
        {
            NativeFunction.Natives.SET_CURRENT_PED_WEAPON(WeaponOwner.Pedestrian, bestWeapon, true);
        }
        NativeFunction.Natives.SET_PED_CAN_SWITCH_WEAPON(WeaponOwner.Pedestrian, true);
        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(WeaponOwner.Pedestrian, (int)eCombat_Attribute.CA_DO_DRIVEBYS, true);
    }
    public void SetCompletelyUnarmed()
    {
        if(!WeaponOwner.Pedestrian.Exists() || !WeaponOwner.Pedestrian.IsAlive)
        { 
            return; 
        }
        ShouldAutoSetWeaponState = false;
        NativeFunction.Natives.REMOVE_ALL_PED_WEAPONS(WeaponOwner.Pedestrian, false);
        GetCurrentWeapon();
        if (CurrentWeapon != UnarmedHash)//unarmed
        {
            NativeFunction.Natives.SET_CURRENT_PED_WEAPON(WeaponOwner.Pedestrian, UnarmedHash, true);
            NativeFunction.Natives.SET_PED_CAN_SWITCH_WEAPON(WeaponOwner.Pedestrian, false);
        }
        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(WeaponOwner.Pedestrian, (int)eCombat_Attribute.CA_DO_DRIVEBYS, false);
        if (!IsHolsterFull)
        {
            HolsterPistol();
        }
        IsSetLessLethal = false;
        IsSetUnarmed = true;
        IsSetDeadly = false;
        IsSetDefault = false;
        //EntryPoint.WriteToConsole($"{WeaponOwner.Handle} SET FULLY UNARMED");
        DebugWeaponState = "Set Unarmed";
        GameTimeLastWeaponCheck = Game.GameTime;   
    }
    public void Reset()
    {
        IsSetDeadly = false;
        IsSetLessLethal = false;
        IsSetUnarmed = false;
        IsSetDefault = false;
        ShouldAutoSetWeaponState = true;
        GameTimeLastWeaponCheck = 0;
        DebugWeaponState = "";
    }
    public void RemoveHeavyWeapon()
    {
        HasHeavyWeaponOnPerson = false;
    }
    public void GiveHeavyWeapon()
    {
        HasHeavyWeaponOnPerson = true;
    }
    private void HolsterPistol()
    {
        if(IsHolsterFull)
        {
            return;
        }
        if (FullHolster != null)
        {
            if (WeaponOwner.Pedestrian.Exists())
            {
                NativeFunction.Natives.SET_PED_COMPONENT_VARIATION(WeaponOwner.Pedestrian, FullHolster.ComponentID, FullHolster.DrawableID, FullHolster.TextureID, FullHolster.PaletteID);
                GameFiber.Yield();
            }
        }
        IsHolsterFull = true;     
    }
    private void UnHolsterPistol()
    {
        if(!IsHolsterFull)
        {
            return;
        }
        if(EmptyHolster != null)
        {
            if (WeaponOwner.Pedestrian.Exists())
            {
                NativeFunction.Natives.SET_PED_COMPONENT_VARIATION(WeaponOwner.Pedestrian, EmptyHolster.ComponentID, EmptyHolster.DrawableID, EmptyHolster.TextureID, EmptyHolster.PaletteID);
                GameFiber.Yield();
            }
        }
        IsHolsterFull = false;       
    }
    private void HandleSearchlight(bool isNightTime)
    {
        if (WeaponOwner.Pedestrian.Exists() && WeaponOwner.Pedestrian.CurrentVehicle.Exists())
        {
            bool isChasingOtherPed = WeaponOwner.CurrentTask?.Name == "AIApprehend";
            bool isChasingPlayer = !isChasingOtherPed && Player.IsWanted;
            bool isChasingAnyone = isChasingOtherPed || Player.IsWanted;
            if (isNightTime)
            {
                if(isChasingAnyone)
                {
                    if (!NativeFunction.Natives.IS_VEHICLE_SEARCHLIGHT_ON<bool>(WeaponOwner.Pedestrian.CurrentVehicle))
                    {
                        NativeFunction.Natives.SET_VEHICLE_SEARCHLIGHT(WeaponOwner.Pedestrian.CurrentVehicle, true, true);
                        EntryPoint.WriteToConsole("TURNING SPOTLIGHT ON");
                    }
                    if (isChasingOtherPed)
                    {
                        if (WeaponOwner.CurrentTask.OtherTarget != null && WeaponOwner.CurrentTask.OtherTarget.Pedestrian.Exists() && WeaponOwner.CurrentTask.OtherTarget.WantedLevel > 0)
                        {
                            NativeFunction.Natives.SET_MOUNTED_WEAPON_TARGET(WeaponOwner.Pedestrian, WeaponOwner.CurrentTask.OtherTarget.Pedestrian, 0, 0f, 0f, 0f, 2, false);//2 == AIM
                        }
                    }
                    else if (isChasingPlayer && WeaponOwner.RecentlySeenPlayer)
                    {
                        NativeFunction.Natives.SET_MOUNTED_WEAPON_TARGET(WeaponOwner.Pedestrian, Player.Character, 0, 0f, 0f, 0f, 2, false);//2 == AIM
                    }
                }
                else
                {
                    if (NativeFunction.Natives.IS_VEHICLE_SEARCHLIGHT_ON<bool>(WeaponOwner.Pedestrian.CurrentVehicle))
                    {
                        NativeFunction.Natives.SET_MOUNTED_WEAPON_TARGET(WeaponOwner.Pedestrian, 0, 0, 0f, 0f, 0f, 2, false);//2 == AIM
                        //NativeFunction.Natives.SET_VEHICLE_SEARCHLIGHT(WeaponOwner.Pedestrian.CurrentVehicle, false, true);
                        //EntryPoint.WriteToConsole("TURNING SPOTLIGHT OFF");
                    }
                }
            }
            else if (NativeFunction.Natives.IS_VEHICLE_SEARCHLIGHT_ON<bool>(WeaponOwner.Pedestrian.CurrentVehicle))
            {
                NativeFunction.Natives.SET_VEHICLE_SEARCHLIGHT(WeaponOwner.Pedestrian.CurrentVehicle, false, true);
                //EntryPoint.WriteToConsole("TURNING SPOTLIGHT OFF");
            }
        }
    }
}

