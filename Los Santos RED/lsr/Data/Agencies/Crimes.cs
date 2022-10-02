using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Crimes : ICrimes
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Crimes.xml";
    public Crimes()
    {
    }
    public List<Crime> CrimeList { get; private set; } = new List<Crime>();
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("Crimes*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Crimes config: {ConfigFile.FullName}",0);
            CrimeList = Serialization.DeserializeParams<Crime>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Crimes config  {ConfigFileName}",0);
            CrimeList = Serialization.DeserializeParams<Crime>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Crimes config found, creating default", 0);
            DefaultConfig();
        }
    }
    public Crime GetCrime(string crimeID)
    {
        return CrimeList.FirstOrDefault(x => x.ID == crimeID);
    }
    private void DefaultConfig()
    {
        CrimeList = new List<Crime>()
        {

            new Crime("KillingPolice", "Police Fatality", 3, true, 1, false, true, true) { CanViolateWithoutPerception = true },
            new Crime("TerroristActivity", "Terrorist Activity", 4, true, 2, false, false, true) { CanReportBySound = true },
            new Crime("FiringWeaponNearPolice", "Shots Fired at Police", 3, true, 3, false, false, true) { CanReportBySound = true },
            new Crime("AimingWeaponAtPolice", "Aiming Weapons At Police", 3, true, 4, false, false, true),
            new Crime("HurtingPolice", "Assaulting Police", 3, false, 5,false, false, true) { CanViolateWithoutPerception = true },
            new Crime("BrandishingHeavyWeapon", "Brandishing Heavy Weapon", 3, false, 6, false, true, true),
            new Crime("TrespessingOnGovtProperty", "Trespassing on Government Property", 3, false, 7, false, false, true),
            new Crime("GotInAirVehicleDuringChase", "Stealing an Air Vehicle", 3, false, 8, false, false, true),
            new Crime("FiringWeapon", "Firing Weapon", 2, false, 9, true, true, true) { CanReportBySound = true },
            new Crime("Kidnapping", "Kidnapping", 2, false, 10, true, false, true),
            new Crime("KillingCivilians", "Civilian Fatality", 2, false, 11, true, true, true),
            new Crime("ArmedRobbery", "Armed Robbery", 2, false, 12, true, true, true),
            new Crime("Mugging", "Mugging", 2, false, 12, true, true, true),
            new Crime("AttemptingSuicide", "Attempting Suicide", 2, false, 13, false, false, true),
            new Crime("HitPedWithCar", "Pedestrian Hit and Run", 2, false, 14, true, true, true),
            new Crime("HurtingCivilians", "Assaulting Civilians", 2, false, 15, true, true, true),     
            new Crime("GrandTheftAuto", "Grand Theft Auto", 2, false, 16, true, true, true),
            new Crime("BrandishingWeapon", "Brandishing Weapon", 2, false, 17, true, true, true),
            new Crime("ResistingArrest", "Resisting Arrest", 2, false, 18, false, false, true),
            new Crime("BrandishingCloseCombatWeapon", "Brandishing Close Combat Weapon", 1, false, 19, true, true, true),
            new Crime("DrunkDriving", "Drunk Driving", 2, false, 20, true, false, false),     
            new Crime("AssaultingWithDeadlyWeapon", "Assaulting With A Deadly Weapon", 2, false, 21, true, true, true),
            new Crime("AssaultingCivilians", "Assaulting", 2, false, 22, true, true, true),



            new Crime("DealingDrugs", "Dealing Drugs", 2, false, 23, true, false, false) { MaxReportingDistance = 20f },
            new Crime("DealingGuns", "Illegal Weapons Dealing", 2, false, 24, true, false, false) { MaxReportingDistance = 20f },







            new Crime("HitCarWithCar", "Hit and Run", 1, false, 30, true, false, false) { IsTrafficViolation = true },
            new Crime("PublicIntoxication", "Public Intoxication", 1, false, 31, true, false, false),
            new Crime("ChangingPlates", "Stealing License Plates", 1, false, 32, true, false, false) { MaxReportingDistance = 20f },
            new Crime("DrivingAgainstTraffic", "Driving Against Traffic", 1, false, 33, true, false, false) { IsTrafficViolation = true, RequiresCitation = true },
            new Crime("DrivingOnPavement", "Driving On Pavement", 1, false, 34, true, false, false) { IsTrafficViolation = true, RequiresCitation = true },
            new Crime("NonRoadworthyVehicle", "NonRoadworthy Vehicle", 1, false, 35, true, false, false) { IsTrafficViolation = true, RequiresCitation = true },
            new Crime("RunningARedLight", "Running a Red Light", 1, false, 36, true, false, false) { IsTrafficViolation = true, RequiresCitation = true },
            new Crime("FelonySpeeding", "Felony Speeding", 1, false, 37, true, false, false) { IsTrafficViolation = true, RequiresCitation = true },
            
            new Crime("DrivingStolenVehicle", "Driving a Stolen Vehicle", 2, false, 38, false,false,false),

            new Crime("SuspiciousActivity", "Suspicious Activity", 1, false, 39, false,false,false) { RequiresSearch = true },



            new Crime("InsultingOfficer", "Insulting a Police Officer", 2, false, 40, false, false, true),


            new Crime("Harassment", "Harassment", 1, false, 41, true, false, false) { MaxReportingDistance = 15f, RequiresCitation = true },

            new Crime("Speeding", "Speeding", 1, false, 44, false, false, false) { IsTrafficViolation = true, RequiresCitation = true },


            new Crime("PublicNuisance", "Public Nuisance", 1, false, 50, true, false, false) { MaxReportingDistance = 15f, RequiresCitation = true },
            new Crime("PublicVagrancy", "Public Vagrancy", 1, false, 51, true, false, false) { MaxReportingDistance = 15f, RequiresCitation = true },
            new Crime("OfficersNeeded", "Officers Needed", 1, false, 60, false, false,false),

        };
        Serialization.SerializeParams(CrimeList, ConfigFileName);
    }
}