using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using System.Collections.Generic;
using System.IO;

public class Crimes : ICrimes
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Crimes.xml";
    private bool UseVanillaConfig = true;
    public Crimes()
    {
    }
    public List<Crime> CrimeList { get; private set; } = new List<Crime>();
    public void ReadConfig()
    {
        if (File.Exists(ConfigFileName))
        {
            CrimeList = Serialization.DeserializeParams<Crime>(ConfigFileName);
        }
        else
        {
            if (UseVanillaConfig)
            {
                DefaultConfig();
            }
            else
            {
                CustomConfig();
            }
            Serialization.SerializeParams(CrimeList, ConfigFileName);
        }
    }
    private void CustomConfig()
    {
        DefaultConfig();
    }
    private void DefaultConfig()
    {
        CrimeList = new List<Crime>()
        {
            new Crime("AimingWeaponAtPolice", "Aiming Weapons At Police", 3, false, 4, 1, false),
            new Crime("AttemptingSuicide", "Attempting Suicide", 2, false, 12, 3),
            new Crime("BrandishingCloseCombatWeapon", "Brandishing Close Combat Weapon", 1, false, 20, 4, true, true, true),
            new Crime("BrandishingHeavyWeapon", "Brandishing Heavy Weapon", 3, false, 6, 1, false, true, true),
            new Crime("BrandishingWeapon", "Brandishing Weapon", 2, false, 18, 3, true, true, true),
            new Crime("ChangingPlates", "Stealing License Plates", 1, false, 31, 4, true, true, false),
            new Crime("DrivingAgainstTraffic", "Driving Against Traffic", 1, false, 32, 4, false, false, false) { IsTrafficViolation = true },
            new Crime("DrivingOnPavement", "Driving On Pavement", 1, false, 33, 4, false, false, false) { IsTrafficViolation = true },
            new Crime("DrivingStolenVehicle", "Driving a Stolen Vehicle", 2, false, 38, 5, false),
            new Crime("DrunkDriving", "Drunk Driving", 2, false, 30, 4, false, false, false),
            new Crime("FelonySpeeding", "Speeding", 1, false, 37, 5, false, false, false) { IsTrafficViolation = true },
            new Crime("FiringWeapon", "Firing Weapon", 2, false, 9, 2, true, true, true) { CanReportBySound = true },
            new Crime("FiringWeaponNearPolice", "Shots Fired at Police", 3, true, 3, 1, false) { CanReportBySound = true },
            new Crime("GotInAirVehicleDuringChase", "Stealing an Air Vehicle", 3, false, 8, 2),
            new Crime("GrandTheftAuto", "Grand Theft Auto", 2, false, 16, 3, true, true, true),
            new Crime("Harassment", "Harassment", 1, false, 41, 5, false),
            new Crime("HitCarWithCar", "Hit and Run", 1, false, 30, 4, true, true, true) { IsTrafficViolation = true },
            new Crime("HitPedWithCar", "Pedestrian Hit and Run", 2, false, 15, 3, true, true, true) { IsTrafficViolation = true },
            new Crime("HurtingCivilians", "Assaulting Civilians", 2, false, 14, 3, true, true, true),
            new Crime("HurtingPolice", "Assaulting Police", 3, false, 5, 1),
            new Crime("InsultingOfficer", "Insulting a Police Officer", 1, false, 40, 5),
            new Crime("Kidnapping", "Kidnapping", 2, false, 10, 2, false, false, false),
            new Crime("KillingCivilians", "Civilian Fatality", 2, false, 11, 2, true, true, true),
            new Crime("KillingPolice", "Police Fatality", 3, true, 1, 1, false) { CanViolateWithoutPerception = true },
            new Crime("Mugging", "Mugging", 2, false, 11, 2, true, true, true),
            new Crime("NonRoadworthyVehicle", "NonRoadworthy Vehicle", 1, false, 34, 4, false, false, false) { IsTrafficViolation = true },
            new Crime("PublicIntoxication", "Public Intoxication", 1, false, 31, 4, true, false, false),
            new Crime("ResistingArrest", "Resisting Arrest", 2, false, 19, 4, false) { CanViolateWithoutPerception = true },
            new Crime("RunningARedLight", "Running a Red Light", 1, false, 36, 5, false, false, false) { IsTrafficViolation = true },
            new Crime("SuspiciousActivity", "Suspicious Activity", 1, false, 39, 5, false),
            new Crime("TerroristActivity", "Terrorist Activity", 4, true, 2, 1) { CanReportBySound = true },
            new Crime("TrespessingOnGovtProperty", "Trespassing on Government Property", 3, false, 7, 2, false) { CanViolateMultipleTimes = false }
        };
    }
}