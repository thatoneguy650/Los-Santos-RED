using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class WantedLevelScript
{
    private static PoliceState PrevPoliceState;
    private static uint GameTimePoliceStateStart;
    private static uint GameTimeLastSetWanted;
    private static uint GameTimeWantedStarted;
    private static uint GameTimeWantedLevelStarted;
    private static uint GameTimeLastWantedEnded;
    private static uint GameTimeLastRequestedBackup;
    private static Blip CurrentWantedCenterBlip;
    private static Blip LastWantedCenterBlip;
    private static int PreviousWantedLevel;

    public static float LastWantedSearchRadius { get; set; }
    public static bool PlayerSeenDuringCurrentWanted { get; set; } = false;
    public static bool IsMilitaryDeployed { get; set; } = false;
    public static bool IsWeaponsFree { get; set; } = false;
    public static bool IsRunning { get; set; } = true;
    public static CriminalHistory CurrentCrimes { get; set; } 
    public static Vector3 LastWantedCenterPosition { get; set; }
    public static PoliceState CurrentPoliceState { get; set; }
    public static PoliceState LastPoliceState { get; private set; }
    public static Vector3 PlaceWantedStarted { get; private set; }
    public static uint HasBeenNotWantedFor
    {
        get
        {
            if (Game.LocalPlayer.WantedLevel != 0)
                return 0;
            if (GameTimeLastWantedEnded == 0)
                return 0;
            else
                return Game.GameTime - GameTimeLastWantedEnded;
        }
    }
    public static uint HasBeenWantedFor
    {
        get
        {
            if (Game.LocalPlayer.WantedLevel == 0)
                return 0;
            else
                return Game.GameTime - GameTimeWantedStarted;
        }
    }
    public static uint HasBeenAtCurrentWantedLevelFor
    {
        get
        {
            if (Game.LocalPlayer.WantedLevel == 0)
                return 0;
            else
                return Game.GameTime - GameTimeWantedLevelStarted;
        }
    }
    public static uint HasBeenAtCurrentPoliceStateFor
    {
        get
        {
             return Game.GameTime - GameTimePoliceStateStart;
        }
    }
    public static bool IsDeadlyChase
    {
        get
        {
            if (CurrentPoliceState == PoliceState.DeadlyChase)
                return true;
            else
                return false;
        }
    }
    public static bool RecentlySetWanted
    {
        get
        {
            if (GameTimeLastSetWanted == 0)
                return false;
            else if (Game.GameTime - GameTimeLastSetWanted <= 5000)
                return true;
            else
                return false;
        }
    }
    public static bool RecentlyRequestedBackup
    {
        get
        {
            if (GameTimeLastRequestedBackup == 0)
                return false;
            else if (Game.GameTime - GameTimeLastRequestedBackup <= 5000)
                return true;
            else
                return false;
        }
    }
    public static bool NearLastWanted(float DistanceTo)
    {
        return LastWantedCenterPosition != Vector3.Zero && Game.LocalPlayer.Character.DistanceTo2D(LastWantedCenterPosition) <= DistanceTo;
    }
    public static ResponsePriority CurrentResponse
    {
        get
        {
            if (PlayerState.IsNotWanted)
            {
                if (Investigation.InInvestigationMode)
                {
                    if (CurrentCrimes.CrimesReported.Any(x => x.AssociatedCrime.Priority <= 8))
                    {
                        return ResponsePriority.Medium;
                    }
                    else
                    {
                        return ResponsePriority.Low;
                    }
                }
                else
                {
                    return ResponsePriority.None;
                }
            }
            else
            {
                if (PlayerState.WantedLevel > 4)
                {
                    return ResponsePriority.Full;
                }
                else if (PlayerState.WantedLevel >= 2)
                {
                    return ResponsePriority.High;
                }
                else
                {
                    return ResponsePriority.Medium;
                }
            }
        }
    }
    public enum PoliceState
    {
        Normal = 0,
        UnarmedChase = 1,
        CautiousChase = 2,
        DeadlyChase = 3,
        ArrestedWait = 4,
    }
    public enum ResponsePriority
    {
        None = 0,
        Low = 1,
        Medium = 2,
        High = 3,
        Full = 4,
    }
    public static void Initialize()
    {
        CurrentCrimes = new CriminalHistory();
        LastWantedCenterPosition = default;
        LastWantedSearchRadius = General.MySettings.Police.LastWantedCenterSize;
        LastWantedCenterBlip = default;
        PlaceWantedStarted = default;
        CurrentWantedCenterBlip = default;
        GameTimePoliceStateStart = 0;
        GameTimeLastSetWanted = 0;
        PrevPoliceState = PoliceState.Normal;
        CurrentPoliceState = PoliceState.Normal;
        LastPoliceState = PoliceState.Normal;
        PreviousWantedLevel = 0;
        IsRunning = true;
        SetWantedLevel(0, "Initial", true);
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void Tick()
    {
        if (IsRunning)
        {
            GetPoliceState();
            WantedLevelTick();
        }
    }
    private static void GetPoliceState()
    {
        if (PlayerState.WantedLevel == 0)
            CurrentPoliceState = PoliceState.Normal;//Default state

        if (CurrentPoliceState == PoliceState.ArrestedWait || CurrentPoliceState == PoliceState.DeadlyChase)
            return;

        if (PlayerState.WantedLevel >= 1 && PlayerState.WantedLevel <= 3 && Police.AnyCanSeePlayer)
        {
            bool IsDeadly = CurrentCrimes.LethalForceAuthorized;
            if (!IsDeadly && !PlayerState.IsConsideredArmed) // Unarmed and you havent killed anyone
                CurrentPoliceState = PoliceState.UnarmedChase;
            else if (!IsDeadly)
                CurrentPoliceState = PoliceState.CautiousChase;
            else
                CurrentPoliceState = PoliceState.DeadlyChase;
        }
        else if (PlayerState.WantedLevel >= 1 && PlayerState.WantedLevel <= 3)
        {
            CurrentPoliceState = PoliceState.UnarmedChase;
        }
        else if (PlayerState.WantedLevel >= 4)
            CurrentPoliceState = PoliceState.DeadlyChase;

    }
    private static void PoliceStateChanged()
    {
        Debugging.WriteToLog("ValueChecker", string.Format("PoliceState Changed to: {0} Was {1}", CurrentPoliceState, PrevPoliceState));
        LastPoliceState = PrevPoliceState;
        GameTimePoliceStateStart = Game.GameTime;
        PrevPoliceState = CurrentPoliceState;
    }
    private static void WantedLevelTick()
    {
        if (PreviousWantedLevel != Game.LocalPlayer.WantedLevel)
            WantedLevelChanged();

        if (PrevPoliceState != CurrentPoliceState)
            PoliceStateChanged();

        if (!PersonOfInterest.PlayerIsPersonOfInterest)
            RemoveLastBlip();

        if (PlayerState.IsWanted)
        {
            if (!PlayerState.IsDead && !PlayerState.IsBusted)
            {
                Vector3 CurrentWantedCenter = NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
                if (CurrentWantedCenter != Vector3.Zero)
                {
                    LastWantedCenterPosition = CurrentWantedCenter;
                    UpdateBlip(CurrentWantedCenter);
                }

                if(Police.AnyCanSeePlayer)
                {
                    PlayerSeenDuringCurrentWanted = true;
                    CurrentCrimes.PlayerSeenDuringWanted = true;
                }

                if (General.MySettings.Police.WantedLevelIncreasesOverTime && HasBeenAtCurrentWantedLevelFor > General.MySettings.Police.WantedLevelIncreaseTime && Police.AnyCanSeePlayer && PlayerState.WantedLevel <= General.MySettings.Police.WantedLevelInceaseOverTimeLimit)
                {
                    GameTimeLastRequestedBackup = Game.GameTime;
                    SetWantedLevel(PlayerState.WantedLevel + 1, "WantedLevelIncreasesOverTime", true);
                }
                if (CurrentPoliceState == PoliceState.DeadlyChase && PlayerState.WantedLevel < 3)
                {
                    SetWantedLevel(3, "Deadly chase requires 3+ wanted level", true);
                }
                CrimeEvent KillingPolice = CurrentCrimes.CrimesObserved.FirstOrDefault(x => x.AssociatedCrime == Crimes.KillingPolice);
                if (KillingPolice != null)
                {
                    if (KillingPolice.Instances >= 2 * General.MySettings.Police.PoliceKilledSurrenderLimit && PlayerState.WantedLevel < 5)
                    {
                        SetWantedLevel(5, "You killed too many cops 5 Stars", true);
                        IsMilitaryDeployed = true;
                        IsWeaponsFree = true;
                    }
                    else if (KillingPolice.Instances >= General.MySettings.Police.PoliceKilledSurrenderLimit && PlayerState.WantedLevel < 4)
                    {
                        SetWantedLevel(4, "You killed too many cops 4 Stars", true);
                        IsWeaponsFree = true;
                    }
                }
            }
        }
        else
        {
            RemoveBlip();
        }
    }
    private static void WantedLevelChanged()
    {
        if (Game.LocalPlayer.WantedLevel == 0)
        {
            WantedLevelRemoved();
        }
        else if (PreviousWantedLevel == 0 && Game.LocalPlayer.WantedLevel > 0)
        {
            WantedLevelAdded();
        }

        CurrentCrimes.MaxWantedLevel = PlayerState.WantedLevel;
        GameTimeWantedLevelStarted = Game.GameTime;
        Debugging.WriteToLog("ValueChecker", string.Format("WantedLevel Changed to: {0}, Recently Set: {1}", Game.LocalPlayer.WantedLevel, RecentlySetWanted));
        PreviousWantedLevel = Game.LocalPlayer.WantedLevel;
    }
    private static void WantedLevelAdded()
    {
        if (!RecentlySetWanted)//randomly set by the game
        {
            if (PlayerState.WantedLevel <= 2)//let some level 3 and 4 wanted override and be set
            {
                SetWantedLevel(0, "Resetting Unknown Wanted", false);
                return;
            }
        }
        Investigation.InInvestigationMode = false;
        CurrentCrimes.GameTimeWantedStarted = Game.GameTime;
        CurrentCrimes.MaxWantedLevel = PlayerState.WantedLevel;
        PlaceWantedStarted = Game.LocalPlayer.Character.Position;
        GameTimeWantedStarted = Game.GameTime;
        RemoveLastBlip();
    }
    private static void WantedLevelRemoved()
    {
        if (!PlayerState.IsDead)//they might choose the respawn as the same character, so do not replace it yet?
        {
            CurrentCrimes.GameTimeWantedEnded = Game.GameTime;
            CurrentCrimes.MaxWantedLevel = PlayerState.MaxWantedLastLife;
            if (CurrentCrimes.PlayerSeenDuringWanted && PreviousWantedLevel != 0)// && !RecentlySetWanted)//i didnt make it go to zero, the chase was lost organically
            {
                PersonOfInterest.StoreCriminalHistory(CurrentCrimes);
            }
            ResetStats();
        }
        GameTimeLastWantedEnded = Game.GameTime;
        PlayerSeenDuringCurrentWanted = false;
        RemoveBlip();


        if (PersonOfInterest.PlayerIsPersonOfInterest)
        {
            UpdateLastBlip(LastWantedCenterPosition);
        }
        else
        {
            RemoveLastBlip();
        }
    }
    private static void RemoveBlip()
    {
        LastWantedCenterPosition = Vector3.Zero;
        if (CurrentWantedCenterBlip.Exists())
            CurrentWantedCenterBlip.Delete();
    }
    private static void UpdateBlip(Vector3 Position)
    {
        if (Position == Vector3.Zero)
        {
            if (CurrentWantedCenterBlip.Exists())
                CurrentWantedCenterBlip.Delete();
            return;
        }
        if (!CurrentWantedCenterBlip.Exists())
        {
            CurrentWantedCenterBlip = new Blip(Position, 100f)
            {
                Name = "Current Wanted Center Position",
                Color = Color.Red,
                Alpha = 0.5f
            };

            NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)CurrentWantedCenterBlip.Handle, true);
            General.CreatedBlips.Add(CurrentWantedCenterBlip);
        }
        if (CurrentWantedCenterBlip.Exists())
            CurrentWantedCenterBlip.Position = Position;
    }
    private static void UpdateLastBlip(Vector3 Position)
    {
        if (Position == Vector3.Zero)
        {
            if (LastWantedCenterBlip.Exists())
                LastWantedCenterBlip.Delete();
            return;
        }
        if (!LastWantedCenterBlip.Exists())
        {
            int MaxWanted = 0;
            CriminalHistory Test = PersonOfInterest.GetLastWantedStats();
            if(Test != null)
                MaxWanted = Test.MaxWantedLevel;

            if (MaxWanted != 0)
                LastWantedSearchRadius = MaxWanted * General.MySettings.Police.LastWantedCenterSize;
            else
                LastWantedSearchRadius = General.MySettings.Police.LastWantedCenterSize;

            LastWantedCenterBlip = new Blip(LastWantedCenterPosition, LastWantedSearchRadius)
            {
                Name = "Last Wanted Center Position",
                Color = Color.Yellow,
                Alpha = 0.25f
            };

            NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)LastWantedCenterBlip.Handle, true);
            General.CreatedBlips.Add(LastWantedCenterBlip);
        }
        if (LastWantedCenterBlip.Exists())
            LastWantedCenterBlip.Position = Position;
    }
    private static void RemoveLastBlip()
    {
        if (LastWantedCenterBlip.Exists())
            LastWantedCenterBlip.Delete();
    }
    private static void ResetStats()
    {
        foreach (Cop Cop in PedList.CopPeds)
        {
            Cop.HurtByPlayer = false;
        }
        CurrentCrimes = new CriminalHistory();
        Debugging.WriteToLog("ResetPoliceStats", "Ran (Made New Rap Sheet)");
        IsMilitaryDeployed = false;
        IsWeaponsFree = false;
        CurrentPoliceState = PoliceState.Normal;
        Police.AnySeenPlayerCurrentWanted = false;
        GameTimeWantedLevelStarted = 0;
        Investigation.InInvestigationMode = false;
        ScannerScript.ResetReportedItems();
    }
    public static void SetWantedLevel(int WantedLevel, string Reason, bool UpdateRecent)
    {
        if (UpdateRecent)
            GameTimeLastSetWanted = Game.GameTime;

        if (Game.LocalPlayer.WantedLevel < WantedLevel || WantedLevel == 0)
        {
            NativeFunction.CallByName<bool>("SET_MAX_WANTED_LEVEL", WantedLevel);
            Debugging.WriteToLog("SetWantedLevel", string.Format("Current Wanted: {0}, Desired Wanted: {1}, {2}", Game.LocalPlayer.WantedLevel, WantedLevel, Reason));
            Game.LocalPlayer.WantedLevel = WantedLevel;
        }
    }
    public static void Reset()
    {
        ResetStats();
    }
    public static void ApplyReportedCrimes()
    {
        if (CurrentCrimes.CrimesReported.Any())
        {
            foreach (CrimeEvent MyCrimes in CurrentCrimes.CrimesReported)
            {
                CrimeEvent PreviousViolation = CurrentCrimes.CrimesObserved.FirstOrDefault(x => x.AssociatedCrime == MyCrimes.AssociatedCrime);
                if (PreviousViolation == null)
                {
                    CurrentCrimes.CrimesObserved.Add(new CrimeEvent(MyCrimes.AssociatedCrime));
                }
                else if (PreviousViolation.CanAddInstance)
                {
                    PreviousViolation.AddInstance();
                }

            }       
            CrimeEvent WorstObserved = CurrentCrimes.CrimesObserved.OrderBy(x => x.AssociatedCrime.Priority).FirstOrDefault();
            if (WorstObserved != null)
            {
                SetWantedLevel(WorstObserved.AssociatedCrime.ResultingWantedLevel, "you are a suspect!", true);
                ScannerScript.AnnounceCrime(WorstObserved.AssociatedCrime, new DispatchCallIn(!PlayerState.IsInVehicle, true, Game.LocalPlayer.Character.Position));
            }
        }
    }
}

