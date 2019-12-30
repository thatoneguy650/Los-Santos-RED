using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class WantedLevelStats
{
    public int MaxWantedLevel = 0;
    public int CopsKilledByPlayer = 0;
    public int CiviliansKilledByPlayer = 0;
    public bool PlayerHurtPolice = false;
    public bool PlayerKilledPolice = false;
    public bool PlayerKilledCivilians = false;
    public bool PlayerAimedAtPolice = false;
    public bool PlayerFiredWeaponNearPolice = false;
    public bool PlayerWentNearPrisonDuringChase = false;
    public bool PlayerCaughtWithGun = false;
    public bool PlayerGotInAirVehicleDuringChase = false;
    public bool PlayerCaughtChangingPlates = false;
    public bool PlayerCaughtBreakingIntoCar = false;
    public bool PlayerKilledCiviliansInFrontOfPolice = false;

    public bool DispatchReportedOfficerDown = false;
    public bool DispatchReportedLethalForceAuthorized = false;
    public bool DispatchReportedAssaultOnOfficer = false;
    public bool DispatchReportedShotsFired = false;
    public bool DispatchReportedTrespassingOnGovernmentProperty = false;
    public bool DispatchReportedCarryingWeapon = false;
    public bool DispatchReportedThreateningWithAFirearm = false;
    public bool DispatchReportedGrandTheftAuto = false;
    public bool DispatchReportedSuspiciousVehicle = false;
    public bool DispatchReportedCivilianShot = false;
    public bool DispatchReportedWeaponsFree = false;
    public bool DispatchReportedStolenAirVehicle = false;

    public List<GTALicensePlate> WantedPlates = new List<GTALicensePlate>();
    public uint GameTimeWantedStarted;
    public uint GameTimeWantedEnded;
    public bool PlayerSeenDuringWanted = false;
    public bool IsExpired = false;

    public WantedLevelStats()
    {
        StoreValues();
    }
    public void StoreValues()
    {

        PlayerSeenDuringWanted = Police.AnyPoliceSeenPlayerThisWanted;
        GameTimeWantedStarted = Police.GameTimePoliceStateStart;
        GameTimeWantedEnded = Game.GameTime;

        MaxWantedLevel = InstantAction.MaxWantedLastLife;
        CopsKilledByPlayer = Police.CopsKilledByPlayer;
        CiviliansKilledByPlayer = Police.CiviliansKilledByPlayer;
        PlayerHurtPolice = Police.PlayerHurtPolice;
        PlayerKilledPolice = Police.PlayerKilledPolice;
        PlayerKilledCivilians = Police.PlayerKilledCivilians;
        PlayerAimedAtPolice = Police.PlayerAimedAtPolice;
        PlayerFiredWeaponNearPolice = Police.PlayerFiredWeaponNearPolice;
        PlayerWentNearPrisonDuringChase = Police.PlayerWentNearPrisonDuringChase;
        PlayerCaughtWithGun = Police.PlayerCaughtWithGun;
        PlayerGotInAirVehicleDuringChase = Police.PlayerGotInAirVehicleDuringChase;
        PlayerCaughtChangingPlates = Police.PlayerCaughtChangingPlates;
        PlayerCaughtBreakingIntoCar = Police.PlayerCaughtBreakingIntoCar;
        PlayerKilledCiviliansInFrontOfPolice = Police.PlayerKilledCiviliansInFrontOfPolice;

        DispatchReportedOfficerDown = DispatchAudio.ReportedOfficerDown;
        DispatchReportedLethalForceAuthorized = DispatchAudio.ReportedLethalForceAuthorized;
        DispatchReportedAssaultOnOfficer = DispatchAudio.ReportedAssaultOnOfficer;
        DispatchReportedShotsFired = DispatchAudio.ReportedShotsFired;
        DispatchReportedTrespassingOnGovernmentProperty = DispatchAudio.ReportedTrespassingOnGovernmentProperty;
        DispatchReportedCarryingWeapon = DispatchAudio.ReportedCarryingWeapon;
        DispatchReportedThreateningWithAFirearm = DispatchAudio.ReportedThreateningWithAFirearm;
        DispatchReportedGrandTheftAuto = DispatchAudio.ReportedGrandTheftAuto;
        DispatchReportedSuspiciousVehicle = DispatchAudio.ReportedSuspiciousVehicle;
        DispatchReportedCivilianShot = DispatchAudio.ReportedCivilianKilled;
        DispatchReportedWeaponsFree = DispatchAudio.ReportedWeaponsFree;
        DispatchReportedStolenAirVehicle = DispatchAudio.ReportedStolenAirVehicle;

        foreach (GTALicensePlate Plate in LicensePlateChanging.SpareLicensePlates.Where(x => x.IsWanted))
        {
            WantedPlates.Add(Plate);
        }

    }
    public void ApplyValues()
    {
        if (Game.LocalPlayer.WantedLevel < MaxWantedLevel)
            Police.SetWantedLevel(MaxWantedLevel, "Applying old Wanted stats");

        Police.CopsKilledByPlayer = CopsKilledByPlayer;
        Police.CiviliansKilledByPlayer = CiviliansKilledByPlayer;
        Police.PlayerHurtPolice = PlayerHurtPolice;
        Police.PlayerKilledPolice = PlayerKilledPolice;
        Police.PlayerKilledCivilians = PlayerKilledCivilians;
        Police.PlayerAimedAtPolice = PlayerAimedAtPolice;
        Police.PlayerFiredWeaponNearPolice = PlayerFiredWeaponNearPolice;
        Police.PlayerWentNearPrisonDuringChase = PlayerWentNearPrisonDuringChase;

        Police.PlayerCaughtBreakingIntoCar = PlayerCaughtBreakingIntoCar;
        Police.PlayerCaughtChangingPlates = PlayerCaughtChangingPlates;
        Police.PlayerCaughtWithGun = PlayerCaughtWithGun;
        Police.PlayerGotInAirVehicleDuringChase = PlayerGotInAirVehicleDuringChase;
        Police.PlayerKilledCiviliansInFrontOfPolice = PlayerKilledCiviliansInFrontOfPolice;

        DispatchAudio.ReportedOfficerDown = DispatchReportedOfficerDown;
        DispatchAudio.ReportedLethalForceAuthorized = DispatchReportedLethalForceAuthorized;
        DispatchAudio.ReportedAssaultOnOfficer = DispatchReportedAssaultOnOfficer;
        DispatchAudio.ReportedShotsFired = DispatchReportedShotsFired;
        DispatchAudio.ReportedTrespassingOnGovernmentProperty = DispatchReportedTrespassingOnGovernmentProperty;
        DispatchAudio.ReportedCarryingWeapon = DispatchReportedCarryingWeapon;
        DispatchAudio.ReportedThreateningWithAFirearm = DispatchReportedThreateningWithAFirearm;
        DispatchAudio.ReportedGrandTheftAuto = DispatchReportedGrandTheftAuto;
        DispatchAudio.ReportedSuspiciousVehicle = DispatchReportedSuspiciousVehicle;
        DispatchAudio.ReportedStolenAirVehicle = DispatchReportedStolenAirVehicle;
        DispatchAudio.ReportedCivilianKilled = DispatchReportedCivilianShot;
        DispatchAudio.ReportedWeaponsFree = DispatchReportedWeaponsFree;


        DispatchAudio.ClearDispatchQueue();

        Debugging.WriteToLog("WantedLevelStats Replace", "Replaced Wanted Stats");
    }
}

