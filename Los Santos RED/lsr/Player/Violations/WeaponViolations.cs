using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class WeaponViolations
{
    private uint GameTimeStartedBrandishing;
    private IViolateable Player;
    private Violations Violations;
    private ISettingsProvideable Settings;
    private ITimeReportable Time;
    private uint GameTimeLastViolatedShooting;
    private uint GameTimeLastViolatedShootingSuppressed;

    //  private uint GameTimeLastViolatedShootingAtCops;

    public WeaponViolations(IViolateable player, Violations violations, ISettingsProvideable settings, ITimeReportable time)
    {
        Player = player;
        Violations = violations;
        Settings = settings;
        Time = time;
    }
    public bool ShotSomewhatRecently => GameTimeLastViolatedShooting > 0 && Game.GameTime - GameTimeLastViolatedShooting <= 20000;
    public bool RecentlyShot => GameTimeLastViolatedShooting > 0 && Game.GameTime - GameTimeLastViolatedShooting <= 5000;
    public bool RecentlyShotSuppressed => GameTimeLastViolatedShootingSuppressed > 0 && Game.GameTime - GameTimeLastViolatedShootingSuppressed <= 5000;
    //public bool RecentlyShotNearCops => GameTimeLastViolatedShootingAtCops > 0 && Game.GameTime - GameTimeLastViolatedShootingAtCops <= 5000;
    public void Setup()
    {

    }
    public void Dispose()
    {

    }
    public void Reset()
    {
        GameTimeStartedBrandishing = 0;
    }
    public void Update()
    {
        if (Violations.CanCarryAndFireWeapons)
        {
            return;
        }
        if (Player.RecentlyShot)
        {
            if (Player.WeaponEquipment.CurrentWeapon != null && !(Player.WeaponEquipment.CurrentWeaponCategory == WeaponCategory.Melee || Player.WeaponEquipment.CurrentWeaponCategory == WeaponCategory.Throwable)) //if (!(Player.Character.IsCurrentWeaponSilenced || Player.WeaponEquipment.CurrentWeaponCategory == WeaponCategory.Melee || Player.WeaponEquipment.CurrentWeaponCategory == WeaponCategory.Throwable))
            {
                if (Player.Character.IsCurrentWeaponSilenced)
                {
                    GameTimeLastViolatedShootingSuppressed = Game.GameTime;
                    Violations.AddViolating(StaticStrings.FiringSilencedWeaponCrimeID);
                }
                else
                {
                    GameTimeLastViolatedShooting = Game.GameTime;
                    Violations.AddViolating(StaticStrings.FiringWeaponCrimeID);
                }
                
                if (Player.AnyPoliceCanSeePlayer)
                {
                    Violations.AddViolating(StaticStrings.FiringWeaponNearPoliceCrimeID);
                }
            }
        }
        bool isBrandishing = CheckBrandishing();
        if (isBrandishing && Player.Character.Inventory.EquippedWeapon != null && !Player.IsInVehicle)
        {
            Violations.AddViolating(StaticStrings.BrandishingWeaponCrimeID);// "BrandishingWeapon");//.IsCurrentlyViolating = true;
            if (Player.WeaponEquipment.CurrentWeapon != null && Player.WeaponEquipment?.CurrentWeapon?.WeaponLevel >= 4)
            {
                Violations.AddViolating(StaticStrings.TerroristActivityCrimeID);//.IsCurrentlyViolating = true;
            }
            if (Player.WeaponEquipment.CurrentWeapon != null && Player.WeaponEquipment?.CurrentWeapon?.WeaponLevel >= 3)
            {
                Violations.AddViolating(StaticStrings.BrandishingHeavyWeaponCrimeID);// "BrandishingHeavyWeapon");//.IsCurrentlyViolating = true;
            }
            if (Player.WeaponEquipment.CurrentWeapon != null && Player.WeaponEquipment.CurrentWeapon?.Category == WeaponCategory.Melee)
            {
                Violations.AddViolating(StaticStrings.BrandishingCloseCombatWeaponCrimeID);// "BrandishingCloseCombatWeapon");//.IsCurrentlyViolating = true;
            }
            //if(Player.CurrentLocation?.CurrentInterior?.IsWeaponRestricted == true)
            //{
            //    Violations.AddViolating(StaticStrings.ArmedRobberyCrimeID);             
            //}
            if(Player.CurrentLocation != null && Player.CurrentLocation.CurrentInterior != null)
            {
                Player.CurrentLocation?.CurrentInterior?.OnCarryingWeaponInside(Player);
            }
            
        }
        if (isBrandishing && Player.CurrentTargetedPed != null && Player.WeaponEquipment?.CurrentWeapon?.Category != WeaponCategory.Melee)
        {
            if (Player.CurrentTargetedPed.IsCop)
            {
                Violations.AddViolating(StaticStrings.AimingWeaponAtPoliceCrimeID);// "AimingWeaponAtPolice");
            }
            else
            {
                Violations.AddViolating(StaticStrings.AssaultingWithDeadlyWeaponCrimeID);// "AssaultingWithDeadlyWeapon");
            }
        }
    }
    private bool CheckBrandishing()
    {
        if (Player.IsVisiblyArmed)
        {
            if (GameTimeStartedBrandishing == 0)
            {
                GameTimeStartedBrandishing = Game.GameTime;
            }
        }
        else
        {
            GameTimeStartedBrandishing = 0;
        }

        if (GameTimeStartedBrandishing > 0 && Game.GameTime - GameTimeStartedBrandishing >= 1500)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool AddFoundWeapon(WeaponInformation weaponInformation, bool hasCCW)
    {
        if(weaponInformation == null)
        {
            //EntryPoint.WriteToConsoleTestLong("AddFoundWeapon WEAPONIFNO IS NULL");
            return false;
        }
        if(weaponInformation.IsLegalWithoutCCW)
        {
            //EntryPoint.WriteToConsoleTestLong("AddFoundWeapon Legal without CCW");
            return false;
        }
        else if (weaponInformation.IsLegal && hasCCW)
        {
            //EntryPoint.WriteToConsoleTestLong("AddFoundWeapon Legal and have CCW");
            return false;
        }
        if (weaponInformation.WeaponLevel >= 4)
        {
            Violations.AddViolatingAndObserved(StaticStrings.TerroristActivityCrimeID);
            //EntryPoint.WriteToConsoleTestLong("AddFoundWeapon TerroristActivity");
            return true;
        }
        else if (weaponInformation.WeaponLevel >= 3)
        {
            Violations.AddViolatingAndObserved(StaticStrings.BrandishingHeavyWeaponCrimeID);// "BrandishingHeavyWeapon");
            //EntryPoint.WriteToConsoleTestLong("AddFoundWeapon BrandishingHeavyWeapon");
            return true;
        }
        else 
        {
            Violations.AddViolatingAndObserved(StaticStrings.BrandishingWeaponCrimeID);// "BrandishingWeapon");
            //EntryPoint.WriteToConsoleTestLong("AddFoundWeapon !IsLegal");
            return true;
        }
    }
}

