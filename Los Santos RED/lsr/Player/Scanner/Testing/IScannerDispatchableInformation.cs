using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IScannerDispatchableInformation
{
     Dispatch AimingWeaponAtPolice { get; }
     Dispatch AnnounceStolenVehicle { get; }
     Dispatch ArmedRobbery { get; }
     Dispatch BankRobbery { get; }
     Dispatch AssaultingCivilians { get; }
     Dispatch AssaultingCiviliansWithDeadlyWeapon { get; }
     Dispatch AssaultingOfficer { get; }
     Dispatch AttemptingSuicide { get; }
     Dispatch AttemptToReacquireSuspect { get; }
     Dispatch CarryingWeapon { get; }
     Dispatch ChangedVehicles { get; }
     Dispatch CivilianDown { get; }
     Dispatch CivilianInjury { get; }
     Dispatch CivilianShot { get; }
     Dispatch CriminalActivity { get; }
     Dispatch CurrentlyPlayingDispatch { get; }
     Dispatch DealingDrugs { get; }
     Dispatch DealingGuns { get; }
     Dispatch DrivingAtStolenVehicle { get; }
     Dispatch DrunkDriving { get; }
     Dispatch ExcessiveSpeed { get; }
     Dispatch FelonySpeeding { get; }
     Dispatch Speeding { get; }
     Dispatch FirefightingServicesRequired { get; }
     Dispatch GotOffFreeway { get; }
     Dispatch GotOnFreeway { get; }
     Dispatch WentInTunnel { get; }
     Dispatch GrandTheftAuto { get; }
     Dispatch Harassment { get; }
     Dispatch Kidnapping { get; }
     Dispatch LethalForceAuthorized { get; }
     Dispatch MedicalServicesRequired { get; }
     Dispatch Mugging { get; }
     Dispatch NoFurtherUnitsNeeded { get; }
     Dispatch OfficerDown { get; }
     Dispatch OfficerMIA { get; }
     Dispatch OfficerNeedsAssistance { get; }
     Dispatch OfficersNeeded { get; }
     Dispatch OnFoot { get; }
     Dispatch PedHitAndRun { get; }
     Dispatch Intoxication { get; }
     Dispatch Nuisance { get; }
     Dispatch StandingOnVehicle { get; }
     Dispatch RecklessDriving { get; }
     Dispatch RemainInArea { get; }
     Dispatch RequestAirSupport { get; }
     Dispatch RequestBackup { get; }
     Dispatch RequestBackupSimple { get; }
     Dispatch RequestFIBUnits { get; }
     Dispatch RequestMilitaryUnits { get; }
     Dispatch RequestNOOSEUnits { get; }
     Dispatch RequestNooseUnitsAlt { get; }
     Dispatch RequestNooseUnitsAlt2 { get; }
     Dispatch RequestSwatAirSupport { get; }
     Dispatch ShotsFiredStatus { get; }
     Dispatch ResistingArrest { get; }
     Dispatch ResumePatrol { get; }
     Dispatch RunningARedLight { get; }
     Dispatch ShotsFired { get; }
     Dispatch ShotsFiredAtAnOfficer { get; }
     Dispatch StealingAirVehicle { get; }
     Dispatch SuspectArrested { get; }
     Dispatch SuspectEvaded { get; }
     Dispatch SuspectEvadedSimple { get; }
     Dispatch SuspectSpotted { get; }
     Dispatch UnlawfulBodyDisposal { get; }
     Dispatch CivilianReportUpdate { get; }
     Dispatch SuspectWasted { get; }
     Dispatch SuspiciousActivity { get; }
     Dispatch SuspiciousVehicle { get; }
     Dispatch TamperingWithVehicle { get; }
     Dispatch TerroristActivity { get; }
     Dispatch ThreateningOfficerWithFirearm { get; }
     Dispatch TrespassingOnGovernmentProperty { get; }
     Dispatch TrespassingOnMilitaryBase { get; }
     Dispatch Trespassing { get; }
     Dispatch VehicleCrashed { get; }
     Dispatch VehicleHitAndRun { get; }
     Dispatch VehicleStartedFire { get; }
     Dispatch Vagrancy { get; }
     Dispatch IndecentExposure { get; }
     Dispatch MaliciousVehicleDamage { get; }
     Dispatch WantedSuspectSpotted { get; }
     Dispatch WeaponsFree { get; }
     Dispatch DrugPossession { get; }
     Dispatch SuspectSpottedSimple { get; }
     Dispatch StoppingTrains { get; }
     Dispatch TheftDispatch { get; }
     Dispatch Shoplifting { get; }
}

