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
  //  private uint GameTimeLastViolatedShootingAtCops;

    public WeaponViolations(IViolateable player, Violations violations, ISettingsProvideable settings, ITimeReportable time)
    {
        Player = player;
        Violations = violations;
        Settings = settings;
        Time = time;
    }
    public bool RecentlyShot => GameTimeLastViolatedShooting > 0 && Game.GameTime - GameTimeLastViolatedShooting <= 5000;
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
        if (!Violations.CheckWeaponViolations)
        {
            return;
        }

        if (Player.RecentlyShot)
        {
            if (Player.WeaponEquipment.CurrentWeapon != null && !(Player.WeaponEquipment.CurrentWeaponCategory == WeaponCategory.Melee || Player.WeaponEquipment.CurrentWeaponCategory == WeaponCategory.Throwable)) //if (!(Player.Character.IsCurrentWeaponSilenced || Player.WeaponEquipment.CurrentWeaponCategory == WeaponCategory.Melee || Player.WeaponEquipment.CurrentWeaponCategory == WeaponCategory.Throwable))
            {
                if (Player.Character.IsCurrentWeaponSilenced)
                {
                    Violations.AddViolating(StaticStrings.FiringSilencedWeaponCrimeID);
                }
                else
                {   
                    Violations.AddViolating(StaticStrings.FiringWeaponCrimeID);
                }
                GameTimeLastViolatedShooting = Game.GameTime;
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

