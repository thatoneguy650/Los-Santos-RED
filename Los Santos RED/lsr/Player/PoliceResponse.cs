using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LosSantosRED.lsr
{
    public class PoliceResponse
    {
        private IPoliceRespondable Player;
        private ArrestWarrant ArrestWarrant;
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

        public PoliceResponse(IPoliceRespondable player, ArrestWarrant arrestWarrant)
        {
            Player = player;
            CurrentCrimes = new CriminalHistory(player);
            ArrestWarrant = arrestWarrant;
        }

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
        public CriminalHistory CurrentCrimes { get; set; }
        public Vector3 LastWantedCenterPosition { get; set; }
        public Vector3 PlaceWantedStarted { get; private set; }
        public uint HasBeenNotWantedFor
        {
            get
            {
                if (Game.LocalPlayer.WantedLevel != 0)
                {
                    return 0;
                }
                if (GameTimeLastWantedEnded == 0)
                {
                    return 0;
                }
                else
                    return Game.GameTime - GameTimeLastWantedEnded;
            }
        }
        public uint HasBeenWantedFor
        {
            get
            {
                if (Game.LocalPlayer.WantedLevel == 0)
                {
                    return 0;
                }
                else
                    return Game.GameTime - GameTimeWantedStarted;
            }
        }
        public uint HasBeenAtCurrentWantedLevelFor
        {
            get
            {
                if (Game.LocalPlayer.WantedLevel == 0)
                {
                    return 0;
                }
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
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public string CurrentPoliceStateString
        {
            get
            {
                return CurrentPoliceState.ToString();
            }
        }
        public string CrimesObservedJoined
        {
            get
            {
                return string.Join(",", CurrentCrimes.CrimesObserved.Select(x => x.AssociatedCrime.Name));
            }
        }
        public string CrimesReportedJoined
        {
            get
            {
                return string.Join(",", CurrentCrimes.CrimesReported.Select(x => x.AssociatedCrime.Name));
            }
        }
        public bool HasReportedCrimes
        {
            get
            {
                return CurrentCrimes.CrimesReported.Any();
            }
        }
        public bool RecentlySetWanted
        {
            get
            {
                if (GameTimeLastSetWanted == 0)
                {
                    return false;
                }
                else if (Game.GameTime - GameTimeLastSetWanted <= 5000)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool RecentlyRequestedBackup
        {
            get
            {
                if (GameTimeLastRequestedBackup == 0)
                {
                    return false;
                }
                else if (Game.GameTime - GameTimeLastRequestedBackup <= 5000)
                {
                    return true;
                }
                else
                    return false;
            }
        }
        public bool RecentlyLostWanted
        {
            get
            {
                if (GameTimeLastWantedEnded == 0)
                {
                    return false;
                }
                else if (Game.GameTime - GameTimeLastWantedEnded <= 5000)
                {
                    return true;
                }
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
                if (CurrentPoliceState == PoliceState.DeadlyChase && (CurrentCrimes.InstancesOfCrime("KillingPolice") >= 1 || CurrentCrimes.InstancesOfCrime("KillingCivilians") >= 2 || Player.WantedLevel >= 4))
                {
                    return true;
                }
                else
                    return false;
            }
        }
        public ResponsePriority CurrentResponse
        {
            get
            {
                if (Player.IsNotWanted)
                {
                    if (Player.Investigations.IsActive)
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
                    if (Player.WantedLevel > 4)
                    {
                        return ResponsePriority.Full;
                    }
                    else if (Player.WantedLevel >= 2)
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
        public void Update()
        {
            GetPoliceState();
            WantedLevelTick();
        }
        public void Reset()
        {
            SetWantedLevel(0, "Police Response Reset", true);
            CurrentCrimes = new CriminalHistory(Player);
            IsWeaponsFree = false;
            CurrentPoliceState = PoliceState.Normal;
            GameTimeWantedLevelStarted = 0;


            //World.ResetPolice();
            //World.ResetScanner();
        }
        public void SetWantedLevel(int WantedLevel, string Reason, bool UpdateRecent)
        {
            if (UpdateRecent)
            {
                GameTimeLastSetWanted = Game.GameTime;
            }

            if (Game.LocalPlayer.WantedLevel < WantedLevel || WantedLevel == 0)
            {
                NativeFunction.CallByName<bool>("SET_MAX_WANTED_LEVEL", WantedLevel);
                //Game.Console.Print(string.Format("SetWantedLevel! Current Wanted: {0}, Desired Wanted: {1}, {2}", Game.LocalPlayer.WantedLevel, WantedLevel, Reason));
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
                        CurrentCrimes.CrimesObserved.Add(new CrimeEvent(MyCrimes.AssociatedCrime, new PoliceScannerCallIn(!Player.IsInVehicle, true, Game.LocalPlayer.Character.Position, true)));
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
            if (Player.WantedLevel == 0)
            {
                CurrentPoliceState = PoliceState.Normal;//Default state
            }

            if (Player.IsBusted)
            {
                CurrentPoliceState = PoliceState.ArrestedWait;
            }

            if (CurrentPoliceState == PoliceState.ArrestedWait || CurrentPoliceState == PoliceState.DeadlyChase)
            {
                return;
            }

            if (Player.WantedLevel >= 1 && Player.WantedLevel <= 3 && Player.AnyPoliceCanSeePlayer)
            {
                if (Player.AnyPoliceCanSeePlayer)
                {
                    if (CurrentCrimes.LethalForceAuthorized)
                    {
                        CurrentPoliceState = PoliceState.DeadlyChase;
                    }
                    else if (Player.IsConsideredArmed)
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
            else if (Player.WantedLevel >= 4)
            {
                CurrentPoliceState = PoliceState.DeadlyChase;
            }

        }
        private void PoliceStateChanged()
        {
            //Game.Console.Print(string.Format("PoliceState Changed to: {0} Was {1}", CurrentPoliceState, PrevPoliceState));
            GameTimePoliceStateStart = Game.GameTime;
            PrevPoliceState = CurrentPoliceState;
        }
        private void WantedLevelTick()
        {
            if (PreviousWantedLevel != Game.LocalPlayer.WantedLevel)
            {
                WantedLevelChanged();
            }
            if (PrevPoliceState != CurrentPoliceState)
            {
                PoliceStateChanged();
            }
            if (Player.IsWanted)
            {
                if (!Player.IsDead && !Player.IsBusted)
                {
                    Vector3 CurrentWantedCenter = Player.PlacePoliceLastSeenPlayer; //NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
                    if (CurrentWantedCenter != Vector3.Zero)
                    {
                        LastWantedCenterPosition = CurrentWantedCenter;
                    }

                    if (Player.AnyPoliceCanSeePlayer)
                    {
                        PlayerSeenDuringCurrentWanted = true;
                        CurrentCrimes.PlayerSeenDuringWanted = true;
                    }

                    if (HasBeenAtCurrentWantedLevelFor > 240000 && Player.AnyPoliceCanSeePlayer && Player.WantedLevel <= 4)
                    {
                        GameTimeLastRequestedBackup = Game.GameTime;
                        SetWantedLevel(Player.WantedLevel + 1, "WantedLevelIncreasesOverTime", true);
                    }
                    if (CurrentPoliceState == PoliceState.DeadlyChase && Player.WantedLevel < 3)
                    {
                        SetWantedLevel(3, "Deadly chase requires 3+ wanted level", true);
                    }
                    int PoliceKilled = CurrentCrimes.InstancesOfCrime("KillingPolice");
                    if (PoliceKilled > 0)
                    {
                        if (PoliceKilled >= 4 && Player.WantedLevel < 5)
                        {
                            SetWantedLevel(5, "You killed too many cops 5 Stars", true);
                            IsWeaponsFree = true;
                        }
                        else if (PoliceKilled >= 2 && Player.WantedLevel < 4)
                        {
                            SetWantedLevel(4, "You killed too many cops 4 Stars", true);
                            IsWeaponsFree = true;
                        }
                    }
                }
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
            //CurrentCrimes.MaxWantedLevel = CurrentPlayer.WantedLevel;
            GameTimeWantedLevelStarted = Game.GameTime;
            //Game.Console.Print(string.Format("WantedLevel! Changed to: {0}, Recently Set: {1}", Game.LocalPlayer.WantedLevel, RecentlySetWanted));
            PreviousWantedLevel = Game.LocalPlayer.WantedLevel;
        }
        private void WantedLevelAdded()
        {
            if (!RecentlySetWanted)//randomly set by the game
            {
                if (Player.WantedLevel <= 2)//let some level 3 and 4 wanted override and be set
                {
                    SetWantedLevel(0, "Resetting Unknown Wanted", false);
                    return;
                }
            }
            Player.Investigations.Reset();
            CurrentCrimes.GameTimeWantedStarted = Game.GameTime;
            //CurrentCrimes.MaxWantedLevel = CurrentPlayer.WantedLevel;
            PlaceWantedStarted = Game.LocalPlayer.Character.Position;
            GameTimeWantedStarted = Game.GameTime;
        }
        private void WantedLevelRemoved()
        {
            if (!Player.IsDead)// && !CurrentPlayer.RecentlyRespawned)//they might choose the respawn as the same character, so do not replace it yet?
            {
                CurrentCrimes.GameTimeWantedEnded = Game.GameTime;
                //CurrentCrimes.MaxWantedLevel = CurrentPlayer.MaxWantedLastLife;
                if (CurrentCrimes.PlayerSeenDuringWanted && PreviousWantedLevel != 0)// && !RecentlySetWanted)//i didnt make it go to zero, the chase was lost organically
                {
                    ArrestWarrant.StoreCriminalHistory(CurrentCrimes);
                }
                Reset();
            }
            GameTimeLastWantedEnded = Game.GameTime;
            PlayerSeenDuringCurrentWanted = false;
        }
    }
}