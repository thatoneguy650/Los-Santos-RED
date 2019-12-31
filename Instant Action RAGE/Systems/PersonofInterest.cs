using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class PersonOfInterest
{
    private static WantedLevelStats LastWantedStats;
    private static bool PrevPlayerIsWanted;
    private static List<WantedLevelStats> PreviousWantedStats;
    public static bool PlayerIsPersonOfInterest { get; set; }
    public static bool IsRunning { get; set; } = true;

    public static void Initialize()
    {
        IsRunning = true;
        PrevPlayerIsWanted = false;

        PreviousWantedStats = new List<WantedLevelStats>();
        PlayerIsPersonOfInterest = false;
        MainLoop();
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void MainLoop()
    {
        //GameFiber.StartNew(delegate
        //{
        //    try
        //    {
        //        while (IsRunning)
        //        {
        //            PersonOfInterestTick();
        //            GameFiber.Sleep(500);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        InstantAction.Dispose();
        //        Debugging.WriteToLog("Error", e.Message + " : " + e.StackTrace);
        //    }
        //});
    }
    
    public static void PersonOfInterestTick()
    {
        LastWantedStats = GetLastWantedStats();

        if (PrevPlayerIsWanted != InstantAction.PlayerIsWanted)
            WantedLevelAddedOrRemoved();

        CheckCurrentVehicle();

        if (InstantAction.PlayerIsNotWanted)
        {       
            CheckSight();          

            if (PlayerIsPersonOfInterest && Police.PlayerHasBeenNotWantedFor >= 120000)
            {
                ResetPersonOfInterest(true);
            }
        }
        else
        {
            if (!PlayerIsPersonOfInterest && Police.AnyPoliceCanSeePlayer)
            {
                PlayerBecamePersonOfInterest();
            }
        }
    }
    private static void PlayerBecamePersonOfInterest()
    {
        PlayerIsPersonOfInterest = true;
        if (ApplyLastWantedStats())
        {
            Debugging.WriteToLog("PlayerBecamePersonOfInterest", "There was previous wanted stats that were applied");
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspectSpotted, 1));
        }
        Debugging.WriteToLog("PlayerBecamePersonOfInterest", "Happened");
    }
    public static void CheckCurrentVehicle()
    {
        if ((InstantAction.PlayerIsNotWanted || InstantAction.PlayerWantedLevel == 1) && Police.AnyPoliceCanRecognizePlayer && InstantAction.PlayerInVehicle && Game.LocalPlayer.Character.IsInAnyVehicle(false))//first check is cheaper, but second is required to verify
        {
            GTAVehicle VehicleToCheck = InstantAction.GetPlayersCurrentTrackedVehicle();

            if (VehicleToCheck == null)
                return;

            if (VehicleToCheck.WasReportedStolen && VehicleToCheck.IsStolen && VehicleToCheck.MatchesOriginalDescription)
            {
                if (!ApplyWantedStatsForPlate(VehicleToCheck.CarPlate.PlateNumber))
                    Police.SetWantedLevel(2, "Car was reported stolen and it matches the original description (formerly First)");
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSpottedStolenCar, 10)
                {
                    ResultsInStolenCarSpotted = true,
                    VehicleToReport = VehicleToCheck,
                    Speed = Game.LocalPlayer.Character.CurrentVehicle.Speed * 2.23694f
                });
            }
            else if (VehicleToCheck.CarPlate.IsWanted && !VehicleToCheck.IsStolen && VehicleToCheck.ColorMatchesDescription)
            {
                if (!ApplyWantedStatsForPlate(VehicleToCheck.CarPlate.PlateNumber))
                    Police.SetWantedLevel(2, "Car plate is wanted and color matches original (formerly Second)");
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspiciousVehicle, 10)
                {
                    ResultsInStolenCarSpotted = true,
                    VehicleToReport = VehicleToCheck
                });
            }
        }
    }
    public static void CheckSight()
    {
        if (PlayerIsPersonOfInterest && Police.PlayerHasBeenNotWantedFor >= 5000 && Police.PlayerHasBeenNotWantedFor <= 120000)
        {
            if (Police.AnyPoliceCanSeePlayer && Police.NearLastWanted())
            {
                if(!ApplyLastWantedStats())
                    Police.SetWantedLevel(2, "Cops Reacquired after losing them in the same area, actual wanted not found");
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspectSpotted, 1));
            }
            //else if (Police.AnyPoliceCanRecognizePlayer && 1 == 0)
            //{
            //    if (!ApplyLastWantedStats())
            //        Police.SetWantedLevel(2, "Cops Reacquired after losing them you were recognized, actual wanted not found");
            //    DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspectSpotted, 3));
            //}
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
        }
        PrevPlayerIsWanted = InstantAction.PlayerIsWanted;
    }

    public static void ResetPersonOfInterest(bool PlayAudio)
    {
        PlayerIsPersonOfInterest = false;
        PreviousWantedStats.ForEach(x => x.IsExpired = true);
        Debugging.WriteToLog("ResetPersonOfInterest", "All Previous wanted items are expired");
        Police.LastWantedCenterPosition = Vector3.Zero;
        Police.AddUpdateLastWantedBlip(Vector3.Zero);

        if(PlayAudio)
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
    public static bool ApplyLastWantedStats()
    {
        WantedLevelStats MyLastWantedStats = GetLastWantedStats();
        if (MyLastWantedStats == null)
            return false;
        else
            ApplyWantedStats(MyLastWantedStats);

        return true;
    }
    public static int LastWantedLevel()
    {
        if (LastWantedStats == null)
            return 0;
        else
            return LastWantedStats.MaxWantedLevel;
    }
    public static void ApplyWantedStats(WantedLevelStats WantedStatsToApply)
    {
        if (WantedStatsToApply == null)
            return;

        WantedStatsToApply.ApplyValues();
    }

    public static WantedLevelStats GetLastWantedStats()
    {
        if (PreviousWantedStats == null || !PreviousWantedStats.Where(x => x.PlayerSeenDuringWanted && !x.IsExpired).Any())
            return null;

        return PreviousWantedStats.Where(x => x.PlayerSeenDuringWanted && !x.IsExpired).OrderByDescending(x => x.GameTimeWantedEnded).OrderByDescending(x => x.GameTimeWantedStarted).FirstOrDefault();
    }
    public static WantedLevelStats GetWantedLevelStatsForPlate(string PlateNumber)
    {
        if (PreviousWantedStats == null || !PreviousWantedStats.Where(x => x.PlayerSeenDuringWanted).Any())
            return null;

        return PreviousWantedStats.Where(x => x.PlayerSeenDuringWanted && x.WantedPlates.Any(y => y.PlateNumber == PlateNumber)).OrderByDescending(x => x.GameTimeWantedEnded).OrderByDescending(x => x.GameTimeWantedStarted).FirstOrDefault();
    }

}
