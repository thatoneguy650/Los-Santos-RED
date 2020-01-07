using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class RapSheet
{
    public int TimeAimedAtPolice;
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
    //public bool IsExpired = false;

    private static bool PrevPlayerKilledPolice = false;
    private static int PrevCopsKilledByPlayer = 0;
    private bool PrevPlayerHurtPolice = false;

    public uint GameTimeLastKilledCivilian;
    public uint GameTimeLastKilledCop;
    public uint GameTimeLastHurtPolice;
    public uint GameTimeLastHurtCivilian;

    public bool RecentlyHurtCivilian(uint TimeSince)
    {
        if (GameTimeLastHurtCivilian == 0)
            return false;
        else if (Game.GameTime - GameTimeLastHurtCivilian <= TimeSince)
            return true;
        else
            return false;
    }
    public bool RecentlyHurtPolice(uint TimeSince)
    {
        if (GameTimeLastHurtPolice == 0)
            return false;
        else if (Game.GameTime - GameTimeLastHurtPolice <= TimeSince)
            return true;
        else
            return false;
    }
    public bool RecentlyKilledCivilian(uint TimeSince)
    {
        if (GameTimeLastKilledCivilian == 0)
            return false;
        else if (Game.GameTime - GameTimeLastKilledCivilian <= TimeSince)
            return true;
        else
            return false;
    }
    public bool RecentlyKilledPolice(uint TimeSince)
    {
        if (GameTimeLastKilledCop == 0)
            return false;
        else if (Game.GameTime - GameTimeLastKilledCop <= TimeSince)
            return true;
        else
            return false;
    }
    public RapSheet()
    {

    }
    public string PrintCrimes()
    {
        return string.Format("MaxWantedLevel: {0},PlayerHurtPolice: {1}, PlayerKilledPolice: {2},PlayerKilledCivilians: {3},PlayerAimedAtPolice: {4},PlayerFiredWeaponNearPolice {5},PlayerWentNearPrisonDuringChase: {6},PlayerCaughtWithGun: {7},PlayerGotInAirVehicleDuringChase: {8},PlayerCaughtChangingPlates: {9},PlayerCaughtBreakingIntoCar: {10},PlayerKilledCiviliansInFrontOfPolice: {11}",
                                                                MaxWantedLevel, PlayerHurtPolice, PlayerKilledPolice, PlayerKilledCivilians, PlayerAimedAtPolice, PlayerFiredWeaponNearPolice, PlayerWentNearPrisonDuringChase, PlayerCaughtWithGun, PlayerGotInAirVehicleDuringChase, PlayerCaughtChangingPlates, PlayerCaughtBreakingIntoCar, PlayerKilledCiviliansInFrontOfPolice);
    }
    public void CheckCrimes()
    {
        if (Police.CurrentPoliceState == Police.PoliceState.ArrestedWait || Police.CurrentPoliceState == Police.PoliceState.DeadlyChase)
            return;

        if (!PlayerKilledCiviliansInFrontOfPolice && RecentlyKilledCivilian(5000) && Police.AnyPoliceCanSeePlayer)
        {
            PlayerKilledCiviliansInFrontOfPolice = true;
            PlayerKilledCivilianInFrontOfPoliceChanged();
        }

        if (!PlayerFiredWeaponNearPolice && (Game.LocalPlayer.Character.IsShooting || Police.PlayerArtificiallyShooting) && (PoliceScanning.CopPeds.Any(x => x.canSeePlayer || (x.DistanceToPlayer <= 20f && !Game.LocalPlayer.Character.IsCurrentWeaponSilenced)))) //if (!firedWeapon && Game.LocalPlayer.Character.IsShooting && (PoliceScanning.CopPeds.Any(x => x.canSeePlayer || x.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 100f))))
        {
            PlayerFiredWeaponNearPolice = true;
            FiredWeaponChanged();
        }

        if (!PlayerWentNearPrisonDuringChase && InstantAction.PlayerIsWanted && PlayerLocation.PlayerCurrentZone == Zones.JAIL && Police.AnyPoliceCanSeePlayer)
        {
            PlayerWentNearPrisonDuringChase = true;
            PlayerWentNearPrisonDuringChaseChanged();
        }

        if (!PlayerAimedAtPolice && InstantAction.PlayerIsConsideredArmed && Game.LocalPlayer.IsFreeAiming && Police.AnyPoliceCanSeePlayer && PoliceScanning.CopPeds.Any(x => Game.LocalPlayer.IsFreeAimingAtEntity(x.Pedestrian)))
            TimeAimedAtPolice++;
        else
            TimeAimedAtPolice = 0;

        if (!PlayerAimedAtPolice && TimeAimedAtPolice >= 100)
        {
            PlayerAimedAtPolice = true;
            AimedAtPoliceChanged();
        }

        if (!PlayerCaughtWithGun && Police.AnyPoliceCanSeePlayer && InstantAction.PlayerIsConsideredArmed && Game.LocalPlayer.Character.Inventory.EquippedWeapon != null && !InstantAction.PlayerInVehicle)
        {
            PlayerCaughtWithGun = true;
            PlayerCaughtWithGunChanged();
        }

        if (!PlayerCaughtChangingPlates && LicensePlateChanging.PlayerChangingPlate && Police.AnyPoliceCanSeePlayer)
        {
            PlayerCaughtChangingPlates = true;
            PlayerCaughtChangingPlatesChanged();
        }

        if (!PlayerCaughtBreakingIntoCar && CarStealing.PlayerBreakingIntoCar && Police.AnyPoliceCanSeePlayer)
        {
            PlayerCaughtBreakingIntoCar = true;
            PlayerCaughtBreakingIntoCarChanged();
        }

        if (!PlayerGotInAirVehicleDuringChase && InstantAction.PlayerIsWanted && InstantAction.PlayerInVehicle && Game.LocalPlayer.Character.IsInAirVehicle)
        {
            PlayerGotInAirVehicleDuringChase = true;
            PlayerGotInAirVehicleDuringChaseChanged();
        }

        if (PrevPlayerKilledPolice != PlayerKilledPolice)
            PlayerKilledPoliceChanged();

        if (PrevCopsKilledByPlayer != CopsKilledByPlayer)
            CopsKilledChanged();

    }
    private void PlayerGotInAirVehicleDuringChaseChanged()
    {
        if (PlayerGotInAirVehicleDuringChase)
        {
            Police.SetWantedLevel(4, "You tried to get away in an air vehicle during a chase");
            if (InstantAction.PlayerWantedLevel <= 4)
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportStolenAirVehicle, 1)
                {
                    ResultsInLethalForce = true,
                    VehicleToReport = InstantAction.GetPlayersCurrentTrackedVehicle()
                });
        }
    }
    private void PlayerCaughtBreakingIntoCarChanged()
    {
        if (PlayerCaughtBreakingIntoCar)
        {
            Police.SetWantedLevel(2, "Police saw you breaking into a car");
            if (InstantAction.PlayerWantedLevel <= 2)
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportGrandTheftAuto, 3));
        }
    }
    private void CopsKilledChanged()
    {
        PrevCopsKilledByPlayer = CopsKilledByPlayer;
    }
    private void PlayerHurtPoliceChanged()
    {
        if (PlayerHurtPolice)
        {
            Police.SetWantedLevel(3, "You hurt a police officer");
            if (InstantAction.PlayerWantedLevel <= 3 && !PlayerKilledPolice)
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportAssualtOnOfficer, 3));
        }
        PrevPlayerHurtPolice = PlayerHurtPolice;
    }
    private void PlayerKilledPoliceChanged()
    {
        if (PlayerKilledPolice)
        {
            Police.SetWantedLevel(3, "You killed a police officer");
            if (InstantAction.PlayerWantedLevel <= 3)
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportOfficerDown, 1) { ResultsInLethalForce = true });
        }
        PrevPlayerKilledPolice = PlayerKilledPolice;
    }
    private void FiredWeaponChanged()
    {
        if (PlayerFiredWeaponNearPolice)
        {
            Police.SetWantedLevel(3, "You fired a weapon at the police");
            if (InstantAction.PlayerWantedLevel <= 3)
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportShotsFired, 2) { ResultsInLethalForce = true });
        }
    }
    private void AimedAtPoliceChanged()
    {
        if (PlayerAimedAtPolice)
        {
            Police.SetWantedLevel(3, "You aimed at the police too long");
            if (InstantAction.PlayerWantedLevel <= 3)
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportThreateningWithFirearm, 2) { ResultsInLethalForce = true });
        }
    }
    private void PlayerCaughtChangingPlatesChanged()
    {
        if (PlayerCaughtChangingPlates)
        {
            Police.SetWantedLevel(2, "Police saw you changing plates");
            if (InstantAction.PlayerWantedLevel <= 2)
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspiciousActivity, 3));
        }
    }
    private void PlayerCaughtWithGunChanged()
    {
        if (PlayerCaughtWithGun)
        {
            if (Game.LocalPlayer.Character.Inventory.EquippedWeapon == null)
                return;

            GTAWeapon MatchedWeapon = GTAWeapons.GetWeaponFromHash((ulong)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash);

            int DesiredWantedLevel = 2;
            if (MatchedWeapon != null && MatchedWeapon.WeaponLevel >= 2)
                DesiredWantedLevel = MatchedWeapon.WeaponLevel;

            Police.SetWantedLevel(DesiredWantedLevel, "Cops saw you with a gun");
            if (InstantAction.PlayerWantedLevel <= DesiredWantedLevel)
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportCarryingWeapon, 3) { WeaponToReport = MatchedWeapon });
        }
    }
    private void PlayerWentNearPrisonDuringChaseChanged()
    {
        if (PlayerWentNearPrisonDuringChase)
        {
            Police.SetWantedLevel(3, "You went too close to the prison with a wanted level");
            if (InstantAction.PlayerWantedLevel <= 3)
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportTrespassingOnGovernmentProperty, 3) { ResultsInLethalForce = true });
        }
    }
    private void PlayerKilledCivilianInFrontOfPoliceChanged()
    {
        if (PlayerKilledCiviliansInFrontOfPolice)
        {
            Police.SetWantedLevel(3, "You killed someone in front of the police");
            if (InstantAction.PlayerWantedLevel <= 3)
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportCivilianKilled, 2));
        }
    }



}

