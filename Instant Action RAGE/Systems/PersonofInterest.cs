using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class PersonOfInterest
{
    private static bool PrevPlayerIsWanted;
    private static List<WantedLevelStats> PreviousWantedStats;

    private static bool CheckPreviousWantedOnSight;
    public static bool PlayerIsPersonOfInterest { get; set; }
    public static bool IsRunning { get; set; } = true;

    public static void Initialize()
    {
        IsRunning = true;
        PrevPlayerIsWanted = false;

        PreviousWantedStats = new List<WantedLevelStats>();
        PlayerIsPersonOfInterest = false;
        CheckPreviousWantedOnSight = false;
        MainLoop();
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void MainLoop()
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                while (IsRunning)
                {
                    PersonOfInterestTick();
                    GameFiber.Sleep(500);
                }
            }
            catch (Exception e)
            {
                InstantAction.Dispose();
                Debugging.WriteToLog("Error", e.Message + " : " + e.StackTrace);
            }
        });
    }
    
    private static void PersonOfInterestTick()
    {
        if (PrevPlayerIsWanted != InstantAction.PlayerIsWanted)
            WantedLevelAddedOrRemoved();

        if (InstantAction.PlayerIsNotWanted)
        {
            if (PlayerIsPersonOfInterest && Police.PlayerHasBeenNotWantedFor >= 120000)
            {
                ResetPersonOfInterest();
            }

            if (PlayerIsPersonOfInterest && Police.PlayerHasBeenNotWantedFor >= 5000 && Police.PlayerHasBeenNotWantedFor <= 120000)
            {
                if (Police.AnyPoliceCanSeePlayer && Police.NearLastWanted())
                {
                    Police.SetWantedLevel(1, "Cops Reacquired after losing them in the same area, actual wanted will be applied");
                    DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspectSpotted, 3));
                }
                else if (Police.AnyPoliceCanRecognizePlayer && 1 == 0)
                {
                    Police.SetWantedLevel(1, "Cops Reacquired after losing them you were recognized");
                    DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspectSpotted, 3));
                }
            }
        }
        else
        {
            if (!PlayerIsPersonOfInterest && Police.AnyPoliceCanSeePlayer)
            {
                PlayerIsPersonOfInterest = true;
                if (PreviousWantedStats.Any())
                {
                    ApplyWantedStats(GetLastWantedStats());
                    DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspectSpotted, 1));
                }
            }
        }
    }
    private static void WantedLevelAddedOrRemoved()
    {
        if(InstantAction.PlayerIsWanted)
        {
            Police.AddUpdateLastWantedBlip(Vector3.Zero);
        }
        else
        {
            if (PlayerIsPersonOfInterest)
                Police.AddUpdateLastWantedBlip(Police.LastWantedCenterPosition);
            else
                Police.AddUpdateLastWantedBlip(Vector3.Zero);

            Police.AddUpdateCurrentWantedBlip(Vector3.Zero);
        }
        PrevPlayerIsWanted = InstantAction.PlayerIsWanted;
    }

    public static void ResetPersonOfInterest()
    {
        PlayerIsPersonOfInterest = false;
        PreviousWantedStats.Clear();
        Police.LastWantedCenterPosition = Vector3.Zero;
        Police.AddUpdateLastWantedBlip(Vector3.Zero);

        DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportPersonOfInterestExpire, 3));
    }
    public static void StoreWantedStats()
    {
        PreviousWantedStats.Add(new WantedLevelStats());
    }
    public static bool ApplyWantedStatsForPlate(string PlateNumber)
    {
        WantedLevelStats StatsForPlate = GetWantedLevelStatsForPlate(PlateNumber);
        if (StatsForPlate != null)
        {
            ApplyWantedStats(StatsForPlate);
            return true;
        }
        return false;
    }
    public static void ApplyWantedStats(WantedLevelStats WantedStatsToApply)
    {
        if (WantedStatsToApply == null)
            return;

        if (Game.LocalPlayer.WantedLevel < WantedStatsToApply.MaxWantedLevel)
            Police.SetWantedLevel(WantedStatsToApply.MaxWantedLevel, "Applying old Wanted stats");

        Police.CopsKilledByPlayer = WantedStatsToApply.CopsKilledByPlayer;
        Police.CiviliansKilledByPlayer = WantedStatsToApply.CiviliansKilledByPlayer;
        Police.PlayerHurtPolice = WantedStatsToApply.PlayerHurtPolice;
        Police.PlayerKilledPolice = WantedStatsToApply.PlayerKilledPolice;
        Police.PlayerKilledCivilians = WantedStatsToApply.PlayerKilledCivilians;
        Police.PlayerAimedAtPolice = WantedStatsToApply.PlayerAimedAtPolice;
        Police.PlayerFiredWeaponNearPolice = WantedStatsToApply.PlayerFiredWeaponNearPolice;
        Police.PlayerWentNearPrisonDuringChase = WantedStatsToApply.PlayerWentNearPrisonDuringChase;

        Police.PlayerCaughtBreakingIntoCar = WantedStatsToApply.PlayerCaughtBreakingIntoCar;
        Police.PlayerCaughtChangingPlates = WantedStatsToApply.PlayerCaughtChangingPlates;
        Police.PlayerCaughtWithGun = WantedStatsToApply.PlayerCaughtWithGun;
        Police.PlayerGotInAirVehicleDuringChase = WantedStatsToApply.PlayerGotInAirVehicleDuringChase;

        DispatchAudio.ReportedOfficerDown = WantedStatsToApply.DispatchReportedOfficerDown;
        DispatchAudio.ReportedLethalForceAuthorized = WantedStatsToApply.DispatchReportedLethalForceAuthorized;
        DispatchAudio.ReportedAssaultOnOfficer = WantedStatsToApply.DispatchReportedAssaultOnOfficer;
        DispatchAudio.ReportedShotsFired = WantedStatsToApply.DispatchReportedShotsFired;
        DispatchAudio.ReportedTrespassingOnGovernmentProperty = WantedStatsToApply.DispatchReportedTrespassingOnGovernmentProperty;
        DispatchAudio.ReportedCarryingWeapon = WantedStatsToApply.DispatchReportedCarryingWeapon;
        DispatchAudio.ReportedThreateningWithAFirearm = WantedStatsToApply.DispatchReportedThreateningWithAFirearm;
        DispatchAudio.ReportedGrandTheftAuto = WantedStatsToApply.DispatchReportedGrandTheftAuto;
        DispatchAudio.ReportedSuspiciousVehicle = WantedStatsToApply.DispatchReportedSuspiciousVehicle;

        DispatchAudio.ClearDispatchQueue();

        Debugging.WriteToLog("WantedLevelStats Replace", "Replaced Wanted Stats");
    }

    public static WantedLevelStats GetLastWantedStats()
    {
        if (PreviousWantedStats == null || !PreviousWantedStats.Any())
            return null;

        return PreviousWantedStats.Where(x => x.PlayerSeenDuringWanted).OrderByDescending(x => x.GameTimeWantedEnded).OrderByDescending(x => x.GameTimeWantedStarted).FirstOrDefault();
    }
    public static WantedLevelStats GetWantedLevelStatsForPlate(string PlateNumber)
    {
        if (PreviousWantedStats == null || !PreviousWantedStats.Any())
            return null;

        return PreviousWantedStats.Where(x => x.PlayerSeenDuringWanted && x.WantedPlates.Any(y => y.PlateNumber == PlateNumber)).OrderByDescending(x => x.GameTimeWantedEnded).OrderByDescending(x => x.GameTimeWantedStarted).FirstOrDefault();
    }

}
