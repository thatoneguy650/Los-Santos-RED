using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class PersonOfInterestManager
{
    private static uint GameTimeLastAppliedWantedStats;
    public static bool PlayerIsPersonOfInterest { get; private set; }
    public static bool IsRunning { get; set; }
    public static List<CriminalHistory> CriminalHistory { get; set; }
    public static bool RecentlyAppliedWantedStats
    {
        get
        {
            if (GameTimeLastAppliedWantedStats == 0)
                return false;
            else
                return Game.GameTime - GameTimeLastAppliedWantedStats <= 5000;
        }
    }
    public static int MaxWantedLevel
    {
        get
        {
            CriminalHistory LastWanted = GetLastWantedStats();
            if (LastWanted != null)
                return LastWanted.MaxWantedLevel;
            else
                return 0;
        }
    }
    public static bool LethalForceAuthorized
    {
        get
        {
            CriminalHistory LastWanted = GetLastWantedStats();
            if (LastWanted != null)
                return LastWanted.LethalForceAuthorized;
            else
                return false;
        }
    }
    public static float SearchRadius
    {
        get
        {
            if (MaxWantedLevel > 0)
                return MaxWantedLevel * SettingsManager.MySettings.Police.LastWantedCenterSize;
            else
                return SettingsManager.MySettings.Police.LastWantedCenterSize;
        }
    }  
    public static void Initialize()
    {
        IsRunning = true;
        CriminalHistory = new List<CriminalHistory>();
        PlayerIsPersonOfInterest = false;
    }
    public static void Dispose()
    {
        IsRunning = false;
    }    
    public static void Tick()
    {
        if (IsRunning && !PlayerStateManager.IsDead && !PlayerStateManager.IsBusted)
        {
            CheckCurrentVehicle();
            CheckSight();

            if(PlayerStateManager.IsWanted)
            {
                if (!PlayerIsPersonOfInterest && PoliceManager.AnyCanSeePlayer)
                {
                    PlayerIsPersonOfInterest = true;
                }
            }
            else
            {
                if (PlayerIsPersonOfInterest && WantedLevelManager.HasBeenNotWantedFor >= 120000)
                {
                    Reset();
                }
            }
        }
    }
    public static void StoreCriminalHistory(CriminalHistory rapSheet)
    {
        CriminalHistory.Add(rapSheet);
        Debugging.WriteToLog("StoreCriminalHistory", "Stored this Rap Sheet");
    }
    public static void Reset()
    {
        PlayerIsPersonOfInterest = false;
        CriminalHistory.Clear();
        Debugging.WriteToLog("ResetPersonOfInterest", "All Previous wanted items are cleared");
    }
    private static void CheckCurrentVehicle()
    {
        if ((PlayerStateManager.IsNotWanted || PlayerStateManager.WantedLevel == 1) && PoliceManager.AnyCanRecognizePlayer && PlayerStateManager.IsInVehicle && Game.LocalPlayer.Character.IsInAnyVehicle(false))//first check is cheaper, but second is required to verify
        {
            VehicleExt VehicleToCheck = PlayerStateManager.CurrentVehicle;

            if (VehicleToCheck == null)
                return;

            if(VehicleToCheck.CopsRecognizeAsStolen)
            {
                ApplyWantedStatsForPlate(VehicleToCheck.CarPlate.PlateNumber);
            }
        }
    }
    private static void CheckSight()
    {
        if (PlayerIsPersonOfInterest && PoliceManager.AnyCanSeePlayer)
        {
            if (PlayerStateManager.IsWanted)
            {
                ApplyLastWantedStats();
            }
            else
            {
                if (WantedLevelManager.NearLastWanted(SearchRadius) && WantedLevelManager.HasBeenNotWantedFor >= 5000)
                {
                    ApplyLastWantedStats();
                }
            }
        }
    }
    private static void ApplyWantedStatsForPlate(string PlateNumber)
    {
        CriminalHistory StatsForPlate = GetWantedLevelStatsForPlate(PlateNumber);
        if (StatsForPlate != null)
            ApplyWantedStats(StatsForPlate);
    }
    private static void ApplyLastWantedStats()
    {
        CriminalHistory CriminalHistory = GetLastWantedStats();
        if (CriminalHistory != null)
            ApplyWantedStats(CriminalHistory);
    }
    private static void ApplyWantedStats(CriminalHistory CriminalHistory)
    {
        if (CriminalHistory == null)
            return;

        if (PlayerStateManager.WantedLevel < CriminalHistory.MaxWantedLevel)
            WantedLevelManager.SetWantedLevel(CriminalHistory.MaxWantedLevel, "Applying old Wanted stats",true);


        PersonOfInterestManager.CriminalHistory.Remove(CriminalHistory);
        WantedLevelManager.CurrentCrimes = CriminalHistory;

        GameTimeLastAppliedWantedStats = Game.GameTime;
        Debugging.WriteToLog("WantedLevelStats Replace", WantedLevelManager.CurrentCrimes.DebugPrintCrimes());
    }
    private static CriminalHistory GetLastWantedStats()
    {
        if (CriminalHistory == null || !CriminalHistory.Where(x => x.PlayerSeenDuringWanted).Any())
            return null;

        return CriminalHistory.Where(x => x.PlayerSeenDuringWanted).OrderByDescending(x => x.GameTimeWantedEnded).OrderByDescending(x => x.GameTimeWantedStarted).FirstOrDefault();
    }
    private static CriminalHistory GetWantedLevelStatsForPlate(string PlateNumber)
    {
        if (CriminalHistory == null || !CriminalHistory.Where(x => x.PlayerSeenDuringWanted).Any())
            return null;

        return CriminalHistory.Where(x => x.PlayerSeenDuringWanted && x.WantedPlates.Any(y => y.PlateNumber == PlateNumber)).OrderByDescending(x => x.GameTimeWantedEnded).OrderByDescending(x => x.GameTimeWantedStarted).FirstOrDefault();
    }
}
