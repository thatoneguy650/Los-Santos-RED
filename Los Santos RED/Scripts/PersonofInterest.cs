using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class PersonOfInterest
{
    private static bool PrevPlayerIsWanted;
    private static Blip LastWantedCenterBlip;
    public static float LastWantedSearchRadius { get; set; }
    public static bool PlayerIsPersonOfInterest { get; set; }
    public static bool IsRunning { get; set; } = true;
    public static List<RapSheet> CriminalHistory { get; set; }

    public static void Initialize()
    {
        IsRunning = true;
        PrevPlayerIsWanted = false;
        CriminalHistory = new List<RapSheet>();
        LastWantedSearchRadius = General.MySettings.Police.LastWantedCenterSize;
        LastWantedCenterBlip = default;
        PlayerIsPersonOfInterest = false;
    }
    public static void Dispose()
    {
        IsRunning = false;
    }    
    public static void Tick()
    {
        if (IsRunning)
        {
            if (PrevPlayerIsWanted != General.PlayerIsWanted)
                WantedLevelAddedOrRemoved();

            if (General.IsDead || General.IsBusted)
                return;

            CheckCurrentVehicle();
            CheckSight();
            if (General.PlayerIsNotWanted)
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
    }
    public static void PlayerBecamePersonOfInterest()
    {
        PlayerIsPersonOfInterest = true;
        Debugging.WriteToLog("PlayerBecamePersonOfInterest", "Happened");
    }
    public static void CheckCurrentVehicle()
    {
        if ((General.PlayerIsNotWanted || General.PlayerWantedLevel == 1) && Police.AnyPoliceCanRecognizePlayer && General.PlayerInVehicle && Game.LocalPlayer.Character.IsInAnyVehicle(false))//first check is cheaper, but second is required to verify
        {
            GTAVehicle VehicleToCheck = General.GetPlayersCurrentTrackedVehicle();

            if (VehicleToCheck == null)
                return;

            if (VehicleToCheck.WasReportedStolen && VehicleToCheck.IsStolen && VehicleToCheck.MatchesOriginalDescription)
            {
                if (!ApplyWantedStatsForPlate(VehicleToCheck.CarPlate.PlateNumber))
                    Police.SetWantedLevel(2, "Car was reported stolen and it matches the original description (formerly First)",true);
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.SpottedStolenCar, 10)
                {
                    ResultsInStolenCarSpotted = true,
                    VehicleToReport = VehicleToCheck,
                    Speed = Game.LocalPlayer.Character.CurrentVehicle.Speed * 2.23694f
                });
            }
            else if (VehicleToCheck.CarPlate.IsWanted && !VehicleToCheck.IsStolen && VehicleToCheck.ColorMatchesDescription)
            {
                if (!ApplyWantedStatsForPlate(VehicleToCheck.CarPlate.PlateNumber))
                    Police.SetWantedLevel(2, "Car plate is wanted and color matches original (formerly Second)",true);
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.SuspiciousVehicle, 10)
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
            if (Police.PlayerHasBeenNotWantedFor >= 5000 && Police.NearLastWanted(LastWantedSearchRadius))
            {
                if(!ApplyLastWantedStats())
                    Police.SetWantedLevel(2, "Cops Reacquired after losing them in the same area, actual wanted not found",true);
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.SuspectReacquired, 1));
            }
            else if(General.PlayerIsWanted)
            {
                if (ApplyLastWantedStats())
                {
                    Debugging.WriteToLog("PlayerBecamePersonOfInterest", "There was previous wanted stats that were applied");
                    DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.SuspectReacquired, 1));
                }
            }
            else if (Police.PoliceInInvestigationMode && General.PlayerIsNotWanted && Police.NearInvestigationPosition)
            {
                ApplyReportedCrimes();
                Police.SetWantedLevel(2, "you are a suspect!",true);
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.SuspectReacquired, 1));
            }
        }
    }
    private static void ApplyReportedCrimes()
    {
        foreach(Crime MyCrimes in Police.CurrentCrimes.CrimeList)
        {
            if(MyCrimes.RecentlyCalledInByCivilians(180000) && !MyCrimes.HasBeenWitnessedByPolice)
            {
                MyCrimes.CrimeObserved();
            }
        }
    }
    private static void WantedLevelAddedOrRemoved()
    {
        if(General.PlayerIsWanted)
        {
            AddUpdateLastWantedBlip(Vector3.Zero);
        }
        else
        {
            if (PlayerIsPersonOfInterest)
                AddUpdateLastWantedBlip(Police.LastWantedCenterPosition);
            else
                AddUpdateLastWantedBlip(Vector3.Zero);
        }
        PrevPlayerIsWanted = General.PlayerIsWanted;
    }

    public static void ResetPersonOfInterest(bool PlayAudio)
    {
        PlayerIsPersonOfInterest = false;
        CriminalHistory.Clear();
        Debugging.WriteToLog("ResetPersonOfInterest", "All Previous wanted items are cleared");
        Police.LastWantedCenterPosition = Vector3.Zero;
        AddUpdateLastWantedBlip(Vector3.Zero);
        RemoveLastWantedBlips();
        if (PlayAudio && !Police.PoliceInInvestigationMode)
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.ResumePatrol, 3));
    }
    private static void AddUpdateLastWantedBlip(Vector3 Position)
    {
        if (Position == Vector3.Zero)
        {
            if (LastWantedCenterBlip.Exists())
                LastWantedCenterBlip.Delete();
            return;
        }
        if (!LastWantedCenterBlip.Exists())
        {
            int MaxWanted = LastWantedLevel();
            if (MaxWanted != 0)
                LastWantedSearchRadius = MaxWanted * General.MySettings.Police.LastWantedCenterSize;
            else
                LastWantedSearchRadius = General.MySettings.Police.LastWantedCenterSize;

            LastWantedCenterBlip = new Blip(Police.LastWantedCenterPosition, LastWantedSearchRadius)
            {
                Name = "Last Wanted Center Position",
                Color = Color.Yellow,
                Alpha = 0.25f
            };

            NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)LastWantedCenterBlip.Handle, true);
            Police.CreatedBlips.Add(LastWantedCenterBlip);
        }
        if (LastWantedCenterBlip.Exists())
            LastWantedCenterBlip.Position = Position;
    }
    private static void RemoveLastWantedBlips()
    {
        if (LastWantedCenterBlip.Exists())
            LastWantedCenterBlip.Delete();
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
            Police.SetWantedLevel(CriminalHistory.MaxWantedLevel, "Applying old Wanted stats",true);


        PersonOfInterest.CriminalHistory.Remove(CriminalHistory);
        Police.CurrentCrimes = CriminalHistory;

        DispatchAudio.ClearDispatchQueue();
        Debugging.WriteToLog("WantedLevelStats Replace", Police.CurrentCrimes.DebugPrintCrimes());
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
    public static void StoreCriminalHistory(RapSheet rapSheet)
    {
        //rapSheet.IsExpired = true;
        CriminalHistory.Add(rapSheet);
        Debugging.WriteToLog("StoreCriminalHistory", "Stored this Rap Sheet");
    }
}
