using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class PersonOfInterest
{
    //private static WantedLevelStats LastWantedStats;
    private static bool PrevPlayerIsWanted;
    //private static List<WantedLevelStats> PreviousWantedStats;
   // public static List<RapSheet> CriminalHistory;

    public static bool PlayerIsPersonOfInterest { get; set; }
    public static bool IsRunning { get; set; } = true;
    public static List<RapSheet> CriminalHistory { get; set; }

    public static void Initialize()
    {
        IsRunning = true;
        PrevPlayerIsWanted = false;

        //PreviousWantedStats = new List<WantedLevelStats>();
        CriminalHistory = new List<RapSheet>();
        PlayerIsPersonOfInterest = false;
        MainLoop();
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void MainLoop()
    {

    }
    
    public static void PersonOfInterestTick()
    {
        if (PrevPlayerIsWanted != LosSantosRED.PlayerIsWanted)
            WantedLevelAddedOrRemoved();


        if (LosSantosRED.IsDead || LosSantosRED.IsBusted)
            return;

        CheckCurrentVehicle();
        CheckSight();
        if (LosSantosRED.PlayerIsNotWanted)
        {       
            if (PlayerIsPersonOfInterest && Police.PlayerHasBeenNotWantedFor >= 120000)
            {
                ResetPersonOfInterest(true);
            }
        }
        else
        {
            if (!PlayerIsPersonOfInterest && Police.AnyPoliceCanSeePlayer)
            {
                Police.CurrentCrimes.PlayerSeenDuringWanted = true;
                PlayerBecamePersonOfInterest();
            }
        }
    }
    private static void PlayerBecamePersonOfInterest()
    {
        PlayerIsPersonOfInterest = true;
        //if (ApplyLastWantedStats())
        //{
        //    Debugging.WriteToLog("PlayerBecamePersonOfInterest", "There was previous wanted stats that were applied");
        //    DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspectSpotted, 1));
        //}
        Debugging.WriteToLog("PlayerBecamePersonOfInterest", "Happened");
    }
    public static void CheckCurrentVehicle()
    {
        if ((LosSantosRED.PlayerIsNotWanted || LosSantosRED.PlayerWantedLevel == 1) && Police.AnyPoliceCanRecognizePlayer && LosSantosRED.PlayerInVehicle && Game.LocalPlayer.Character.IsInAnyVehicle(false))//first check is cheaper, but second is required to verify
        {
            GTAVehicle VehicleToCheck = LosSantosRED.GetPlayersCurrentTrackedVehicle();

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
        if (PlayerIsPersonOfInterest && Police.AnyPoliceCanSeePlayer)//(Police.PlayerHasBeenNotWantedFor >= 5000 || InstantAction.PlayerIsWanted))//Police.PlayerHasBeenNotWantedFor >= 5000 && Police.PlayerHasBeenNotWantedFor <= 120000)
        {
            if (Police.PlayerHasBeenNotWantedFor >= 5000 && Police.NearLastWanted())
            {
                if(!ApplyLastWantedStats())
                    Police.SetWantedLevel(2, "Cops Reacquired after losing them in the same area, actual wanted not found");
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspectSpotted, 1));
            }
            else if(LosSantosRED.PlayerIsWanted)
            {
                if (ApplyLastWantedStats())
                {
                    Debugging.WriteToLog("PlayerBecamePersonOfInterest", "There was previous wanted stats that were applied");
                    DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspectSpotted, 1));
                }
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
        if(LosSantosRED.PlayerIsWanted)
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
        PrevPlayerIsWanted = LosSantosRED.PlayerIsWanted;
    }

    public static void ResetPersonOfInterest(bool PlayAudio)
    {
        PlayerIsPersonOfInterest = false;
        CriminalHistory.Clear();
        Debugging.WriteToLog("ResetPersonOfInterest", "All Previous wanted items are cleared");
        Police.LastWantedCenterPosition = Vector3.Zero;
        Police.AddUpdateLastWantedBlip(Vector3.Zero);

        if(PlayAudio)
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportPersonOfInterestExpire, 3));

    }
    public static bool ApplyWantedStatsForPlate(string PlateNumber)
    {
        RapSheet StatsForPlate = GetWantedLevelStatsForPlate(PlateNumber);
        if (StatsForPlate != null)
        {
            ApplyWantedStats(StatsForPlate);
            return true;
        }
        return false;
    }
    public static bool ApplyLastWantedStats()
    {
        RapSheet CriminalHistory = GetLastWantedStats();
        if (CriminalHistory == null)
            return false;
        else
            ApplyWantedStats(CriminalHistory);

        return true;
    }
    public static int LastWantedLevel()
    {
        RapSheet MyRapSheet = GetLastWantedStats();
        if (MyRapSheet == null)
            return 0;
        else
            return MyRapSheet.MaxWantedLevel;
    }
    public static void ApplyWantedStats(RapSheet CriminalHistory)
    {
        if (CriminalHistory == null)
            return;

        if (Game.LocalPlayer.WantedLevel < CriminalHistory.MaxWantedLevel)
            Police.SetWantedLevel(CriminalHistory.MaxWantedLevel, "Applying old Wanted stats");


        PersonOfInterest.CriminalHistory.Remove(CriminalHistory);
        Police.CurrentCrimes = CriminalHistory;

        DispatchAudio.ClearDispatchQueue();
        Debugging.WriteToLog("WantedLevelStats Replace", Police.CurrentCrimes.PrintCrimes());
    }

    public static RapSheet GetLastWantedStats()
    {
        if (CriminalHistory == null || !CriminalHistory.Where(x => x.PlayerSeenDuringWanted).Any())
            return null;

        return CriminalHistory.Where(x => x.PlayerSeenDuringWanted).OrderByDescending(x => x.GameTimeWantedEnded).OrderByDescending(x => x.GameTimeWantedStarted).FirstOrDefault();
    }
    public static RapSheet GetWantedLevelStatsForPlate(string PlateNumber)
    {
        if (CriminalHistory == null || !CriminalHistory.Where(x => x.PlayerSeenDuringWanted).Any())
            return null;

        return CriminalHistory.Where(x => x.PlayerSeenDuringWanted && x.WantedPlates.Any(y => y.PlateNumber == PlateNumber)).OrderByDescending(x => x.GameTimeWantedEnded).OrderByDescending(x => x.GameTimeWantedStarted).FirstOrDefault();
    }

    internal static void StoreCriminalHistory(RapSheet rapSheet)
    {
        //rapSheet.IsExpired = true;
        CriminalHistory.Add(rapSheet);
        Debugging.WriteToLog("StoreCriminalHistory", "Stored this Rap Sheet");
    }
}
