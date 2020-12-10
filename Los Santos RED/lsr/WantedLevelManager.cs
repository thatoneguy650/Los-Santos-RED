using ExtensionsMethods;
using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WantedLevelManager

{
    private PoliceState PrevPoliceState;
    private uint GameTimePoliceStateStart;
    private uint GameTimeLastSetWanted;
    private uint GameTimeWantedStarted;
    private uint GameTimeWantedLevelStarted;
    private uint GameTimeLastWantedEnded;
    private uint GameTimeLastRequestedBackup;
    private Blip CurrentWantedCenterBlip;
    private Blip LastWantedCenterBlip;
    private int PreviousWantedLevel;
    private PoliceState CurrentPoliceState;
    private enum PoliceState
    {
        Normal = 0,
        UnarmedChase = 1,
        CautiousChase = 2,
        DeadlyChase = 3,
        ArrestedWait = 4,
    }
    public int WantedLevelLastset { get; set; }
    public float LastWantedSearchRadius { get; set; }
    public bool PlayerSeenDuringCurrentWanted { get; set; }
    public bool IsWeaponsFree { get; set; }
    public bool IsRunning { get; set; }
    public CriminalHistory CurrentCrimes { get; set; } 
    public Vector3 LastWantedCenterPosition { get; set; }
    public Vector3 PlaceWantedStarted { get; private set; }
    public uint HasBeenNotWantedFor
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
    public uint HasBeenWantedFor
    {
        get
        {
            if (Game.LocalPlayer.WantedLevel == 0)
                return 0;
            else
                return Game.GameTime - GameTimeWantedStarted;
        }
    }
    public uint HasBeenAtCurrentWantedLevelFor
    {
        get
        {
            if (Game.LocalPlayer.WantedLevel == 0)
                return 0;
            else
                return Game.GameTime - GameTimeWantedLevelStarted;
        }
    }
    public uint HasBeenAtCurrentPoliceStateFor
    {
        get
        {
             return Game.GameTime - GameTimePoliceStateStart;
        }
    }
    public bool IsDeadlyChase
    {
        get
        {
            if (CurrentPoliceState == PoliceState.DeadlyChase)
                return true;
            else
                return false;
        }
    }
    public string CurrentPoliceStateString
    {
        get
        {
            return CurrentPoliceState.ToString();
        }
    }
    public bool RecentlySetWanted
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
    public bool RecentlyRequestedBackup
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
    public bool RecentlyLostWanted
    {
        get
        {
            if (GameTimeLastWantedEnded == 0)
                return false;
            else if (Game.GameTime - GameTimeLastWantedEnded <= 5000)
                return true;
            else
                return false;
        }
    }
    public bool ShouldSirenBeOn
    {
        get
        {
            if (CurrentResponse == ResponsePriority.Low || CurrentResponse == ResponsePriority.None)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    public float ResponseDrivingSpeed
    {
        get
        {
            if (CurrentResponse == ResponsePriority.High)
            {
                return 25f; //55 mph
            }
            else if (CurrentResponse == ResponsePriority.Medium)
            {
                return 25f; //55 mph
            }
            else
            {
                return 20f; //40 mph
            }
        }
    }
    public bool PoliceChasingRecklessly
    {
        get
        {
            if (CurrentPoliceState == PoliceState.DeadlyChase && (CurrentCrimes.InstancesOfCrime(Mod.CrimeManager.KillingPolice) >= 1 || CurrentCrimes.InstancesOfCrime(Mod.CrimeManager.KillingCivilians) >= 2 || Mod.Player.WantedLevel >= 4))
                return true;
            else
                return false;
        }
    }
    public ResponsePriority CurrentResponse
    {
        get
        {
            if (Mod.Player.IsNotWanted)
            {
                if (Mod.InvestigationManager.InInvestigationMode)
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
                if (Mod.Player.WantedLevel > 4)
                {
                    return ResponsePriority.Full;
                }
                else if (Mod.Player.WantedLevel >= 2)
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
    public void Initialize()
    {
        CurrentCrimes = new CriminalHistory();
        LastWantedCenterPosition = default;
        LastWantedSearchRadius = SettingsManager.MySettings.Police.LastWantedCenterSize;
        LastWantedCenterBlip = default;
        PlaceWantedStarted = default;
        CurrentWantedCenterBlip = default;
        GameTimePoliceStateStart = 0;
        GameTimeLastSetWanted = 0;
        PrevPoliceState = PoliceState.Normal;
        CurrentPoliceState = PoliceState.Normal;
        PreviousWantedLevel = 0;
        IsRunning = true;
        SetWantedLevel(0, "Initial", true);
    }
    public void Dispose()
    {
        IsRunning = false;
    }
    public void Tick()
    {
        if (IsRunning)
        {
            GetPoliceState();
            WantedLevelTick();
        }
    }
    public void Reset()
    {
        foreach (Cop Cop in Mod.PedManager.Cops)
        {
            Cop.HurtByPlayer = false;
        }
        CurrentCrimes = new CriminalHistory();
        IsWeaponsFree = false;
        CurrentPoliceState = PoliceState.Normal;
        GameTimeWantedLevelStarted = 0;
        Mod.PolicePerception.Reset();
        Mod.InvestigationManager.Reset();
        Mod.ScannerManager.Reset();
    }
    public void SetWantedLevel(int WantedLevel, string Reason, bool UpdateRecent)
    {
        if (UpdateRecent)
            GameTimeLastSetWanted = Game.GameTime;

        if (Game.LocalPlayer.WantedLevel < WantedLevel || WantedLevel == 0)
        {
            NativeFunction.CallByName<bool>("SET_MAX_WANTED_LEVEL", WantedLevel);
            Debugging.WriteToLog("SetWantedLevel", string.Format("Current Wanted: {0}, Desired Wanted: {1}, {2}", Game.LocalPlayer.WantedLevel, WantedLevel, Reason));
            Game.LocalPlayer.WantedLevel = WantedLevel;
            WantedLevelLastset = WantedLevel;
        }
    }
    public void ApplyReportedCrimes()
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
                Mod.ScannerManager.AnnounceCrime(WorstObserved.AssociatedCrime, new PoliceScannerCallIn(!Mod.Player.IsInVehicle, true, Game.LocalPlayer.Character.Position));
            }
        }
    }
    public void RefreshPoliceState()
    {
        CurrentPoliceState = PoliceState.Normal;
        GetPoliceState();
    }
    public bool NearLastWanted(float DistanceTo)
    {
        return LastWantedCenterPosition != Vector3.Zero && Game.LocalPlayer.Character.DistanceTo2D(LastWantedCenterPosition) <= DistanceTo;
    }
    private void GetPoliceState()
    {
        if (Mod.Player.WantedLevel == 0)
            CurrentPoliceState = PoliceState.Normal;//Default state

        if (Mod.Player.IsBusted)
            CurrentPoliceState = PoliceState.ArrestedWait;

        if (CurrentPoliceState == PoliceState.ArrestedWait || CurrentPoliceState == PoliceState.DeadlyChase)
            return;

        if (Mod.Player.WantedLevel >= 1 && Mod.Player.WantedLevel <= 3 && Mod.PolicePerception.AnyCanSeePlayer)
        {
            if (Mod.PolicePerception.AnyCanSeePlayer)
            {   
                if (CurrentCrimes.LethalForceAuthorized)
                {
                    CurrentPoliceState = PoliceState.DeadlyChase;
                }
                else if (Mod.Player.IsConsideredArmed)
                {
                    CurrentPoliceState = PoliceState.CautiousChase;
                }
                else
                {
                    CurrentPoliceState = PoliceState.UnarmedChase;
                }
            }
            else
            {
                CurrentPoliceState = PoliceState.UnarmedChase;
            }
        }
        else if (Mod.Player.WantedLevel >= 4)
            CurrentPoliceState = PoliceState.DeadlyChase;

    }
    private void PoliceStateChanged()
    {
        Debugging.WriteToLog("ValueChecker", string.Format("PoliceState Changed to: {0} Was {1}", CurrentPoliceState, PrevPoliceState));
        GameTimePoliceStateStart = Game.GameTime;
        PrevPoliceState = CurrentPoliceState;
    }
    private void WantedLevelTick()
    {
        //Game.LocalPlayer.WantedLevel = WantedLevelLastset;//freeze it 
        //if (Game.LocalPlayer.WantedLevel != WantedLevelLastset)
        //{
        //    SetWantedLevel(WantedLevelLastset, "Freezing Wanted Level", false);
        //}

        if (PreviousWantedLevel != Game.LocalPlayer.WantedLevel)
            WantedLevelChanged();

        if (PrevPoliceState != CurrentPoliceState)
            PoliceStateChanged();

        if (!Mod.PersonOfInterestManager.PlayerIsPersonOfInterest)
            RemoveLastBlip();

        if (Mod.Player.IsWanted)
        {
            if (!Mod.Player.IsDead && !Mod.Player.IsBusted)
            {
                Vector3 CurrentWantedCenter = Mod.PolicePerception.PlaceLastSeenPlayer; //NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
                if (CurrentWantedCenter != Vector3.Zero)
                {
                    LastWantedCenterPosition = CurrentWantedCenter;
                    UpdateBlip(CurrentWantedCenter,SearchModeManager.BlipSize);
                }

                if(Mod.PolicePerception.AnyCanSeePlayer)
                {
                    PlayerSeenDuringCurrentWanted = true;
                    CurrentCrimes.PlayerSeenDuringWanted = true;
                }

                if (SettingsManager.MySettings.Police.WantedLevelIncreasesOverTime && HasBeenAtCurrentWantedLevelFor > SettingsManager.MySettings.Police.WantedLevelIncreaseTime && Mod.PolicePerception.AnyCanSeePlayer && Mod.Player.WantedLevel <= SettingsManager.MySettings.Police.WantedLevelInceaseOverTimeLimit)
                {
                    GameTimeLastRequestedBackup = Game.GameTime;
                    SetWantedLevel(Mod.Player.WantedLevel + 1, "WantedLevelIncreasesOverTime", true);
                }
                if (CurrentPoliceState == PoliceState.DeadlyChase && Mod.Player.WantedLevel < 3)
                {
                    SetWantedLevel(3, "Deadly chase requires 3+ wanted level", true);
                }
                CrimeEvent KillingPolice = CurrentCrimes.CrimesObserved.FirstOrDefault(x => x.AssociatedCrime == Mod.CrimeManager.KillingPolice);
                if (KillingPolice != null)
                {
                    if (KillingPolice.Instances >= 2 * SettingsManager.MySettings.Police.PoliceKilledSurrenderLimit && Mod.Player.WantedLevel < 5)
                    {
                        SetWantedLevel(5, "You killed too many cops 5 Stars", true);
                        IsWeaponsFree = true;
                    }
                    else if (KillingPolice.Instances >= SettingsManager.MySettings.Police.PoliceKilledSurrenderLimit && Mod.Player.WantedLevel < 4)
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
    private void WantedLevelChanged()
    {
        if (Game.LocalPlayer.WantedLevel == 0)
        {
            WantedLevelRemoved();
        }
        else if (PreviousWantedLevel == 0 && Game.LocalPlayer.WantedLevel > 0)
        {
            WantedLevelAdded();
        }

        CurrentCrimes.MaxWantedLevel = Mod.Player.WantedLevel;
        GameTimeWantedLevelStarted = Game.GameTime;
        Debugging.WriteToLog("WantedLevel", string.Format("Changed to: {0}, Recently Set: {1}", Game.LocalPlayer.WantedLevel, RecentlySetWanted));
        PreviousWantedLevel = Game.LocalPlayer.WantedLevel;
    }
    private void WantedLevelAdded()
    {
        if (!RecentlySetWanted)//randomly set by the game
        {
            if (Mod.Player.WantedLevel <= 2)//let some level 3 and 4 wanted override and be set
            {
                SetWantedLevel(0, "Resetting Unknown Wanted", false);
                return;
            }
        }
        Mod.InvestigationManager.Reset();
        CurrentCrimes.GameTimeWantedStarted = Game.GameTime;
        CurrentCrimes.MaxWantedLevel = Mod.Player.WantedLevel;
        PlaceWantedStarted = Game.LocalPlayer.Character.Position;
        GameTimeWantedStarted = Game.GameTime;
        RemoveLastBlip();
    }
    private void WantedLevelRemoved()
    {
        if (!Mod.Player.IsDead)//they might choose the respawn as the same character, so do not replace it yet?
        {
            CurrentCrimes.GameTimeWantedEnded = Game.GameTime;
            CurrentCrimes.MaxWantedLevel = Mod.Player.MaxWantedLastLife;
            if (CurrentCrimes.PlayerSeenDuringWanted && PreviousWantedLevel != 0)// && !RecentlySetWanted)//i didnt make it go to zero, the chase was lost organically
            {
                Mod.PersonOfInterestManager.StoreCriminalHistory(CurrentCrimes);
            }
            Reset();
        }
        GameTimeLastWantedEnded = Game.GameTime;
        PlayerSeenDuringCurrentWanted = false;
        RemoveBlip();


        if (Mod.PersonOfInterestManager.PlayerIsPersonOfInterest)
        {
            UpdateLastBlip(LastWantedCenterPosition);
        }
        else
        {
            RemoveLastBlip();
        }
    }
    private void RemoveBlip()
    {
        if (CurrentWantedCenterBlip.Exists())
            CurrentWantedCenterBlip.Delete();
    }
    private void UpdateBlip(Vector3 Position,float Size)
    {
        if (Position == Vector3.Zero)
        {
            if (CurrentWantedCenterBlip.Exists())
                CurrentWantedCenterBlip.Delete();
            return;
        }
        if (!CurrentWantedCenterBlip.Exists())
        {
            CurrentWantedCenterBlip = new Blip(Position, Size)
            {
                Name = "Current Wanted Center Position",
                Color = Color.Red,
                Alpha = 0.5f
            };
            
            NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)CurrentWantedCenterBlip.Handle, true);
            Mod.Map.AddBlip(CurrentWantedCenterBlip);
        }
        if (CurrentWantedCenterBlip.Exists())
        {
            CurrentWantedCenterBlip.Position = Position;
        }
        CurrentWantedCenterBlip.Color = SearchModeManager.BlipColor;
        CurrentWantedCenterBlip.Scale = SearchModeManager.BlipSize; ;
    }
    private void UpdateLastBlip(Vector3 Position)
    {
        if (Position == Vector3.Zero)
        {
            if (LastWantedCenterBlip.Exists())
                LastWantedCenterBlip.Delete();
            return;
        }
        if (!LastWantedCenterBlip.Exists())
        {
            int MaxWanted = Mod.PersonOfInterestManager.MaxWantedLevel;
            if (MaxWanted != 0)
                LastWantedSearchRadius = MaxWanted * SettingsManager.MySettings.Police.LastWantedCenterSize;
            else
                LastWantedSearchRadius = SettingsManager.MySettings.Police.LastWantedCenterSize;

            LastWantedCenterBlip = new Blip(LastWantedCenterPosition, LastWantedSearchRadius)
            {
                Name = "Last Wanted Center Position",
                Color = Color.Yellow,
                Alpha = 0.25f
            };

            NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)LastWantedCenterBlip.Handle, true);
            Mod.Map.AddBlip(LastWantedCenterBlip);
        }
        if (LastWantedCenterBlip.Exists())
            LastWantedCenterBlip.Position = Position;
    }
    private void RemoveLastBlip()
    {
        if (LastWantedCenterBlip.Exists())
            LastWantedCenterBlip.Delete();
    }
}

