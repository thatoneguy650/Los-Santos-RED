using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class CrimeManager
{
    private static uint GameTimeStartedBrandishing;
    public static bool IsRunning { get; set; } = true;
    public static List<Crime> CrimeList = new List<Crime>();
    public static Crime KillingPolice { get; set; }
    public static Crime FiringWeaponNearPolice { get; set; }
    public static Crime TerroristActivity { get; set; }
    public static Crime BrandishingHeavyWeapon { get; set; }
    public static Crime AimingWeaponAtPolice { get; set; }
    public static Crime HurtingPolice { get; set; }
    public static Crime TrespessingOnGovtProperty { get; set; }
    public static Crime GotInAirVehicleDuringChase { get; set; }
    public static Crime FiringWeapon { get; set; }
    public static Crime KillingCivilians { get; set; }
    public static Crime GrandTheftAuto { get; set; }
    public static Crime DrivingStolenVehicle { get; set; }
    public static Crime Mugging { get; set; }
    public static Crime AttemptingSuicide { get; set; }
    public static Crime BrandishingWeapon { get; set; }
    public static Crime BrandishingCloseCombatWeapon { get; set; }   
    public static Crime HitPedWithCar { get; set; }
    public static Crime HurtingCivilians { get; set; }
    public static Crime HitCarWithCar { get; set; }
    public static Crime ChangingPlates { get; set; }
    public static Crime ResistingArrest { get; set; }
    public static Crime DrivingAgainstTraffic { get; set; }
    public static Crime DrivingOnPavement { get; set; }
    public static Crime NonRoadworthyVehicle { get; set; }
    public static Crime FelonySpeeding { get; set; }
    public static Crime RunningARedLight { get; set; }
    public static Crime DrunkDriving { get; set; }
    public static Crime SuspiciousActivity { get; set; }
    public static bool IsViolatingAnyCrimes
    {
        get
        {
            return CrimeList.Any(x => x.IsCurrentlyViolating);
        }
    }
    public static List<Crime> CurrentlyViolatingCanBeReportedByCivilians
    {
        get
        {
            return CrimeList.Where(x => x.IsCurrentlyViolating && x.CanBeReportedByCivilians).ToList();
        }
    }
    public static bool PlayerViolatingInFrontOfCivilians//whatever fuck name it something
    {
        get
        {
            return CrimeList.Any(x => x.IsCurrentlyViolating && x.CanBeReportedByCivilians);
        }
    }
    public static bool PlayerViolatingAudioCivilians//whatever idk name it something fuck
    {
        get
        {
            return CrimeList.Any(x => x.IsCurrentlyViolating && x.CanBeReportedByCivilians && x.CanReportBySound);
        }
    }
    public static void Initialize()
    {
        IsRunning = true;
        DefaultConfig();
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    private static void DefaultConfig()
    {

        KillingPolice = new Crime("Police Fatality", 3, true, 1,1) { CanBeReportedByCivilians = false, IsAlwaysFlagged = true };
        TerroristActivity = new Crime("Terrorist Activity", 4, true, 2, 1);
        FiringWeaponNearPolice = new Crime("Shots Fired at Police",  3, true,3, 1) { CanBeReportedByCivilians = false };
        AimingWeaponAtPolice = new Crime("Aiming Weapons At Police",  3, false,4, 1) { CanBeReportedByCivilians = false };
        HurtingPolice = new Crime("Assaulting Police",  3, false,5, 1);
        BrandishingHeavyWeapon = new Crime("Brandishing Heavy Weapon", 3, false, 6, 1) { WillAngerCivilians = true };
        TrespessingOnGovtProperty = new Crime("Trespassing on Government Property",  3, false,7,2) { CanBeReportedByCivilians = false };
        GotInAirVehicleDuringChase = new Crime("Stealing an Air Vehicle",  3, false, 8,2);
        FiringWeapon = new Crime("Firing Weapon",  2, false,9, 2) { WillAngerCivilians = true };
        KillingCivilians = new Crime("Civilian Fatality", 2, false,10, 2) { WillAngerCivilians = true };
        Mugging = new Crime("Mugging", 2, false,11,2) { WillAngerCivilians = true };
        AttemptingSuicide = new Crime("Attempting Suicide", 2, false,12,3);
        HurtingCivilians = new Crime("Assaulting Civilians", 2, false, 14,3) { WillAngerCivilians = true };
        HitPedWithCar = new Crime("Pedestrian Hit and Run", 2, false,15,3) { WillAngerCivilians = true };
        GrandTheftAuto = new Crime("Grand Theft Auto", 2, false, 16,3) { WillAngerCivilians = true };
        //DrivingStolenVehicle = new Crime("Driving a Stolen Vehicle", 2, false, 17) { CanBeReportedByCivilians = false };
        BrandishingWeapon = new Crime("Brandishing Weapon", 2, false, 18,3) { WillAngerCivilians = true };
        ResistingArrest = new Crime("Resisting Arrest", 2, false, 19,4) { CanBeReportedByCivilians = false };
        BrandishingCloseCombatWeapon = new Crime("Brandishing Close Combat Weapon", 1, false, 20,4) { WillAngerCivilians = true };
        // DrunkDriving = new Crime("Drunk Driving", 1, false, 21) { WillAngerCivilians = true, WillScareCivilians = false };
        HitCarWithCar = new Crime("Hit and Run", 1, false,30,4) { WillAngerCivilians = true, WillScareCivilians = false };
        ChangingPlates = new Crime("Stealing License Plates", 1, false,31,4) { WillAngerCivilians = true, WillScareCivilians = false };  
        DrivingAgainstTraffic = new Crime("Driving Against Traffic", 1, false,32,4) { CanBeReportedByCivilians = false, WillAngerCivilians = false, WillScareCivilians = false };
        DrivingOnPavement = new Crime("Driving On Pavement", 1, false,33,4) { CanBeReportedByCivilians = false, WillAngerCivilians = false, WillScareCivilians = false };
        NonRoadworthyVehicle = new Crime("Non-Roadworthy Vehicle", 1, false,34,4) { CanBeReportedByCivilians = false, WillAngerCivilians = false, WillScareCivilians = false };
         
        RunningARedLight = new Crime("Running a Red Light", 1, false,36,5) { CanBeReportedByCivilians = false, WillAngerCivilians = false, WillScareCivilians = false };
        FelonySpeeding = new Crime("Speeding", 1, false, 37,5) { CanBeReportedByCivilians = false, WillAngerCivilians = false, WillScareCivilians = false };
        DrivingStolenVehicle = new Crime("Driving a Stolen Vehicle", 2, false, 38,5) { CanBeReportedByCivilians = false };
        SuspiciousActivity = new Crime("Suspicious Activity", 1, false, 39, 5) { CanBeReportedByCivilians = false };
        CrimeList = new List<Crime>
        {
            BrandishingCloseCombatWeapon,TerroristActivity,BrandishingHeavyWeapon, FiringWeapon, Mugging, AttemptingSuicide, ResistingArrest, KillingPolice, FiringWeaponNearPolice, AimingWeaponAtPolice, HurtingPolice, TrespessingOnGovtProperty, GotInAirVehicleDuringChase, KillingCivilians, BrandishingWeapon,
            ChangingPlates, GrandTheftAuto, DrivingStolenVehicle, HurtingCivilians, DrivingAgainstTraffic, DrivingOnPavement, HitPedWithCar, HitCarWithCar, NonRoadworthyVehicle, FelonySpeeding, RunningARedLight, SuspiciousActivity
        };

    }
    public static void Tick()
    {
        if(!PlayerStateManager.IsBusted && !PlayerStateManager.IsDead)
        {
            CheckCrimes();
            FlagCrimes();
        }
    }
    private static void CheckCrimes()
    {
        if (PlayerStateManager.IsBusted || PlayerStateManager.IsDead)
            return;

        CheckPedDamageCrimes();
        CheckWeaponCrimes();
        CheckTheftCrimes();
        CheckOtherCrimes();
    }

    private static void CheckOtherCrimes()
    {
        if (SurrenderManager.IsCommitingSuicide)
        {
            AttemptingSuicide.IsCurrentlyViolating = true;
        }
        else
        {
            AttemptingSuicide.IsCurrentlyViolating = false;
        }
        if (PlayerStateManager.IsWanted && PlayerLocationManager.PlayerCurrentZone.IsRestrictedDuringWanted)
        {
            TrespessingOnGovtProperty.IsCurrentlyViolating = true;
        }
        else
        {
            TrespessingOnGovtProperty.IsCurrentlyViolating = false;
        }
        if (InvestigationManager.IsSuspicious)
        {
            SuspiciousActivity.IsCurrentlyViolating = true;
        }
        else
        {
            SuspiciousActivity.IsCurrentlyViolating = false;
        }
        if (PlayerStateManager.IsWanted && PolicePedManager.AnySeenPlayerCurrentWanted && !PlayerStateManager.AreStarsGreyedOut && Game.LocalPlayer.Character.Speed >= 2.0f && !PlayerStateManager.HandsAreUp && WantedLevelManager.HasBeenWantedFor >= 10000)
        {
            ResistingArrest.IsCurrentlyViolating = true;
        }
        else
        {
            ResistingArrest.IsCurrentlyViolating = false;
        }

    }
    private static void CheckTheftCrimes()
    {
        if (PlayerStateManager.IsWanted && PlayerStateManager.IsInVehicle && Game.LocalPlayer.Character.IsInAirVehicle)
        {
            GotInAirVehicleDuringChase.IsCurrentlyViolating = true;
        }
        else
        {
            GotInAirVehicleDuringChase.IsCurrentlyViolating = false;
        }
        if (PlayerStateManager.CurrentVehicle != null && PlayerStateManager.CurrentVehicle.CopsRecognizeAsStolen)
        {
            DrivingStolenVehicle.IsCurrentlyViolating = true;
        }
        else
        {
            DrivingStolenVehicle.IsCurrentlyViolating = false;
        }
        if (MuggingManager.IsMugging)
        {
            Mugging.IsCurrentlyViolating = true;
        }
        else
        {
            Mugging.IsCurrentlyViolating = false;
        }
        if (PlayerStateManager.IsBreakingIntoCar)
        {
            GrandTheftAuto.IsCurrentlyViolating = true;
        }
        else
        {
            GrandTheftAuto.IsCurrentlyViolating = false;
        }

        if (LicensePlateTheftManager.PlayerChangingPlate)
        {
            ChangingPlates.IsCurrentlyViolating = true;
        }
        else
        {
            ChangingPlates.IsCurrentlyViolating = false;
        }
    }
    private static void CheckWeaponCrimes()
    {
        if (PlayerStateManager.RecentlyShot(5000) || Game.LocalPlayer.Character.IsShooting)
        {
            if (!(Game.LocalPlayer.Character.IsCurrentWeaponSilenced || PlayerStateManager.CurrentWeaponCategory == WeaponCategory.Melee))
            {
                FiringWeapon.IsCurrentlyViolating = true;
                if (PolicePedManager.AnyRecentlySeenPlayer || PolicePedManager.AnyCanHearPlayerShooting)
                    FiringWeaponNearPolice.IsCurrentlyViolating = true;
            }
        }
        else
        {
            FiringWeapon.IsCurrentlyViolating = false;
            FiringWeaponNearPolice.IsCurrentlyViolating = false;
        }
        if (CheckBrandishing() && Game.LocalPlayer.Character.Inventory.EquippedWeapon != null && !PlayerStateManager.IsInVehicle)
        {
            BrandishingWeapon.IsCurrentlyViolating = true;
            if (PlayerStateManager.CurrentWeapon != null && PlayerStateManager.CurrentWeapon.WeaponLevel >= 4)
            {
                TerroristActivity.IsCurrentlyViolating = true;
            }
            else
            {
                TerroristActivity.IsCurrentlyViolating = false;
            }
            if (PlayerStateManager.CurrentWeapon != null && PlayerStateManager.CurrentWeapon.WeaponLevel >= 3)
            {
                BrandishingHeavyWeapon.IsCurrentlyViolating = true;
            }
            else
            {
                BrandishingHeavyWeapon.IsCurrentlyViolating = false;
            }
            if (PlayerStateManager.CurrentWeapon != null && PlayerStateManager.CurrentWeapon.Category == WeaponCategory.Melee)
            {
                BrandishingCloseCombatWeapon.IsCurrentlyViolating = true;
            }
            else
            {
                BrandishingCloseCombatWeapon.IsCurrentlyViolating = false;
            }
        }
        else
        {
            BrandishingCloseCombatWeapon.IsCurrentlyViolating = false;
            BrandishingWeapon.IsCurrentlyViolating = false;
            TerroristActivity.IsCurrentlyViolating = false;
            BrandishingHeavyWeapon.IsCurrentlyViolating = false;
        }
    }
    private static void CheckPedDamageCrimes()
    {
        if (PedDamageManager.RecentlyKilledCop)
        {
            KillingPolice.IsCurrentlyViolating = true;
        }
        else
        {
            KillingPolice.IsCurrentlyViolating = false;
        }

        if (PedDamageManager.RecentlyHurtCop)
        {
            HurtingPolice.IsCurrentlyViolating = true;
        }
        else
        {
            HurtingPolice.IsCurrentlyViolating = false;
        }

        if (PedDamageManager.RecentlyKilledCivilian || PedDamageManager.NearCivilianMurderVictim)
        {
            KillingCivilians.IsCurrentlyViolating = true;
        }
        else
        {
            KillingCivilians.IsCurrentlyViolating = false;
        }

        if (PedDamageManager.RecentlyHurtCivilian)
        {
            HurtingCivilians.IsCurrentlyViolating = true;
        }
        else
        {
            HurtingCivilians.IsCurrentlyViolating = false;
        }
    }
    private static void FlagCrimes()
    {
        foreach (Crime Violating in CrimeList.Where(x => x.IsCurrentlyViolating))
        {
            if (PolicePedManager.AnyCanSeePlayer || (Violating.CanReportBySound && PolicePedManager.AnyCanHearPlayerShooting) || Violating.IsAlwaysFlagged)
            {
                WeaponInformation ToSee = null;
                if (!PlayerStateManager.IsInVehicle)
                    ToSee = PlayerStateManager.CurrentWeapon;
                WantedLevelManager.CurrentCrimes.AddCrime(Violating, true,PlayerStateManager.CurrentPosition,PlayerStateManager.CurrentVehicle, ToSee);
            }
        }
    }
    private static bool CheckBrandishing()
    {
        if (PlayerStateManager.IsConsideredArmed)
        {
            if (GameTimeStartedBrandishing == 0)
                GameTimeStartedBrandishing = Game.GameTime;
        }
        else
        {
            GameTimeStartedBrandishing = 0;
        }

        if (GameTimeStartedBrandishing > 0 && Game.GameTime - GameTimeStartedBrandishing >= 1500)
            return true;
        else
            return false;
    }

}
