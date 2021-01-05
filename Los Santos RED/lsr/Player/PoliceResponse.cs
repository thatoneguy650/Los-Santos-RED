using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
namespace LosSantosRED.lsr
{
    public class PoliceResponse
    {
        public List<CrimeEvent> CrimesObserved = new List<CrimeEvent>();
        public List<CrimeEvent> CrimesReported = new List<CrimeEvent>();
        public uint GameTimeWantedEnded;
        public List<LicensePlate> WantedPlates = new List<LicensePlate>();
        private PoliceState CurrentPoliceState;
        private uint GameTimeLastRequestedBackup;
        private uint GameTimeLastSetWanted;
        private uint GameTimeLastWantedEnded;
        private uint GameTimePoliceStateStart;
        private uint GameTimeWantedLevelStarted;
        private IPoliceRespondable Player;
        private PoliceState PrevPoliceState;
        public PoliceResponse(IPoliceRespondable player)
        {
            Player = player;
            Player.BecameWanted += OnBecameWanted;
            Player.LostWanted += OnLostWanted;
            Player.WantedLevelIncreased += OnWantedLevelIncreased;
        }
        private enum PoliceState
        {
            Normal = 0,
            UnarmedChase = 1,
            CautiousChase = 2,
            DeadlyChase = 3,
            ArrestedWait = 4,
        }
        public string CrimesObservedJoined => string.Join(",", CrimesObserved.Select(x => x.AssociatedCrime.Name));
        public string CrimesReportedJoined => string.Join(",", CrimesReported.Select(x => x.AssociatedCrime.Name));
        public uint GameTimeWantedStarted { get; private set; }
        public uint HasBeenAtCurrentPoliceStateFor => Player.WantedLevel == 0 ? 0 : Game.GameTime - GameTimePoliceStateStart;
        public uint HasBeenAtCurrentWantedLevelFor => Player.WantedLevel == 0 ? 0 : Game.GameTime - GameTimeWantedLevelStarted;
        public uint HasBeenNotWantedFor => Player.WantedLevel != 0 || GameTimeLastWantedEnded == 0 ? 0 : Game.GameTime - GameTimeLastWantedEnded;
        public uint HasBeenWantedFor => Player.WantedLevel == 0 ? 0 : Game.GameTime - GameTimeWantedStarted;
        public bool HasObservedCrimes => CrimesObserved.Any();
        public bool HasReportedCrimes => CrimesReported.Any();
        public bool IsDeadlyChase => CurrentPoliceState == PoliceState.DeadlyChase;
        public bool IsWeaponsFree { get; set; }
        public Vector3 LastWantedCenterPosition { get; set; }
        public float LastWantedSearchRadius { get; set; }
        public bool LethalForceAuthorized => CrimesObserved.Any(x => x.AssociatedCrime.ResultsInLethalForce);
        public int ObservedMaxWantedLevel => CrimesObserved.Max(x => x.AssociatedCrime.ResultingWantedLevel);
        public Vector3 PlaceLastReportedCrime { get; private set; }
        public Vector3 PlaceWantedStarted { get; private set; }
        public bool PlayerSeenDuringCurrentWanted { get; set; }
        public bool PlayerSeenDuringWanted { get; set; } = false;
        public bool PoliceChasingRecklessly => CurrentPoliceState == PoliceState.DeadlyChase && (InstancesOfCrime("KillingPolice") >= 1 || InstancesOfCrime("KillingCivilians") >= 2 || Player.WantedLevel >= 4);
        public bool PoliceHaveDescription { get; private set; }
        public bool RecentlyLostWanted => GameTimeLastWantedEnded != 0 && Game.GameTime - GameTimeLastWantedEnded <= 5000;
        public List<CrimeEvent> RecentlyOccuredCrimes => CrimesObserved.Where(x => x.RecentlyOccurred(10000)).ToList();
        public List<CrimeEvent> RecentlyReportedCrimes => CrimesReported.Where(x => x.RecentlyOccurred(10000)).ToList();
        public bool RecentlyRequestedBackup => GameTimeLastRequestedBackup != 0 && Game.GameTime - GameTimeLastRequestedBackup <= 5000;
        public bool RecentlySetWanted => GameTimeLastSetWanted != 0 && Game.GameTime - GameTimeLastSetWanted <= 5000;
        public float ResponseDrivingSpeed => CurrentResponse == ResponsePriority.High || CurrentResponse == ResponsePriority.Medium ? 25f : 20f;
        public bool ShouldSirenBeOn => CurrentResponse == ResponsePriority.Full || CurrentResponse == ResponsePriority.High || CurrentResponse == ResponsePriority.Medium;
        private ResponsePriority CurrentResponse
        {
            get
            {
                if (Player.IsNotWanted)
                {
                    if (Player.Investigation.IsActive)
                    {
                        if (CrimesReported.Any(x => x.AssociatedCrime.Priority <= 8))
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
        public void AddCrime(Crime CrimeInstance, bool ByPolice, Vector3 Location, VehicleExt VehicleObserved, WeaponInformation WeaponObserved, bool HaveDescription)
        {
            if (Player.IsAliveAndFree)// && !CurrentPlayer.RecentlyBribedPolice)
            {
                if (HaveDescription)
                {
                    PoliceHaveDescription = HaveDescription;
                }
                PlaceLastReportedCrime = Location;
                CrimeEvent PreviousViolation;
                if (ByPolice)
                {
                    PreviousViolation = CrimesObserved.FirstOrDefault(x => x.AssociatedCrime.Name == CrimeInstance.Name);
                }
                else
                {
                    PreviousViolation = CrimesReported.FirstOrDefault(x => x.AssociatedCrime.Name == CrimeInstance.Name);
                }

                int CurrentInstances = 1;
                if (PreviousViolation != null)
                {
                    PreviousViolation.AddInstance();
                    CurrentInstances = PreviousViolation.Instances;
                }
                else
                {
                    if (ByPolice)
                    {
                        CrimesObserved.Add(new CrimeEvent(CrimeInstance, new PoliceScannerCallIn(!Player.IsInVehicle, ByPolice, Location, HaveDescription) { VehicleSeen = VehicleObserved, WeaponSeen = WeaponObserved, Speed = Game.LocalPlayer.Character.Speed, InstancesObserved = CurrentInstances }));
                    }
                    else
                    {
                        CrimesReported.Add(new CrimeEvent(CrimeInstance, new PoliceScannerCallIn(!Player.IsInVehicle, ByPolice, Location, HaveDescription) { VehicleSeen = VehicleObserved, WeaponSeen = WeaponObserved, Speed = Game.LocalPlayer.Character.Speed, InstancesObserved = CurrentInstances }));
                    }
                }
                if (ByPolice && Player.WantedLevel != CrimeInstance.ResultingWantedLevel)
                {
                    SetWantedLevel(CrimeInstance.ResultingWantedLevel, CrimeInstance.Name, true);
                }
            }
        }
        public void ApplyReportedCrimes()
        {
            if (CrimesReported.Any())
            {
                foreach (CrimeEvent MyCrimes in CrimesReported)
                {
                    CrimeEvent PreviousViolation = CrimesObserved.FirstOrDefault(x => x.AssociatedCrime == MyCrimes.AssociatedCrime);
                    if (PreviousViolation == null)
                    {
                        CrimesObserved.Add(new CrimeEvent(MyCrimes.AssociatedCrime, new PoliceScannerCallIn(!Player.IsInVehicle, true, Game.LocalPlayer.Character.Position, true)));
                    }
                    else if (PreviousViolation.CanAddInstance)
                    {
                        PreviousViolation.AddInstance();
                    }
                }
                CrimeEvent WorstObserved = CrimesObserved.OrderBy(x => x.AssociatedCrime.Priority).FirstOrDefault();
                if (WorstObserved != null)
                {
                    SetWantedLevel(WorstObserved.AssociatedCrime.ResultingWantedLevel, "you are a suspect!", true);
                }
            }
        }
        public int InstancesOfCrime(string CrimeID)
        {
            CrimeEvent MyStuff = CrimesObserved.Where(x => x.AssociatedCrime.ID == CrimeID).FirstOrDefault();
            if (MyStuff == null)
                return 0;
            else
                return MyStuff.Instances;
        }
        public int InstancesOfCrime(Crime ToCheck)
        {
            CrimeEvent MyStuff = CrimesObserved.Where(x => x.AssociatedCrime.ID == ToCheck.ID).FirstOrDefault();
            if (MyStuff == null)
                return 0;
            else
                return MyStuff.Instances;
        }
        public bool NearLastWanted(float DistanceTo)
        {
            return LastWantedCenterPosition != Vector3.Zero && Game.LocalPlayer.Character.DistanceTo2D(LastWantedCenterPosition) <= DistanceTo;
        }
        public string PrintCrimes()
        {
            string CrimeString = "";
            foreach (CrimeEvent MyCrime in CrimesObserved.Where(x => x.Instances > 0).OrderBy(x => x.AssociatedCrime.Priority).Take(3))
            {
                CrimeString += string.Format("~n~{0} ({1})~s~", MyCrime.AssociatedCrime.Name, MyCrime.Instances);
            }
            return CrimeString;
        }
        public void Reset()
        {
            SetWantedLevel(0, "Police Response Reset", true);
            IsWeaponsFree = false;
            PlayerSeenDuringWanted = false;
            PlaceLastReportedCrime = Vector3.Zero;
            PoliceHaveDescription = false;
            CurrentPoliceState = PoliceState.Normal;
            GameTimeWantedLevelStarted = 0;
            GameTimeWantedEnded = 0;
        }
        public void SetWantedLevel(int WantedLevel, string Reason, bool UpdateRecent)
        {
            if (UpdateRecent)
            {
                GameTimeLastSetWanted = Game.GameTime;
            }

            if (Player.WantedLevel < WantedLevel || WantedLevel == 0)
            {
                NativeFunction.CallByName<bool>("SET_MAX_WANTED_LEVEL", WantedLevel);
                Game.LocalPlayer.WantedLevel = WantedLevel;
            }
        }
        public void Update()
        {
            GetPoliceState();
            WantedLevelTick();
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
                    if (LethalForceAuthorized)
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
        private void OnBecameWanted(object sender, EventArgs e)
        {
            if (!RecentlySetWanted && Player.WantedLevel <= 2)//randomly set by the game
            {
                SetWantedLevel(0, "Resetting Unknown Wanted", false);
                return;
            }
            GameTimeWantedStarted = Game.GameTime;
            PlaceWantedStarted = Game.LocalPlayer.Character.Position;
        }
        private void OnLostWanted(object sender, EventArgs e)
        {
            if (!Player.IsDead)
            {
                GameTimeWantedEnded = Game.GameTime;
            }
            GameTimeLastWantedEnded = Game.GameTime;
        }
        private void OnWantedLevelIncreased(object sender, EventArgs e)
        {
            GameTimeWantedLevelStarted = Game.GameTime;
        }
        private void PoliceStateChanged()
        {
            //Game.Console.Print(string.Format("PoliceState Changed to: {0} Was {1}", CurrentPoliceState, PrevPoliceState));
            GameTimePoliceStateStart = Game.GameTime;
            PrevPoliceState = CurrentPoliceState;
        }
        private void WantedLevelTick()
        {
            if (PrevPoliceState != CurrentPoliceState)
            {
                PoliceStateChanged();
            }
            if (Player.IsWanted)
            {
                if (!Player.IsDead && !Player.IsBusted)
                {
                    Vector3 CurrentWantedCenter = Player.PlacePoliceLastSeenPlayer;
                    if (CurrentWantedCenter != Vector3.Zero)
                    {
                        LastWantedCenterPosition = CurrentWantedCenter;
                    }
                    if (Player.AnyPoliceCanSeePlayer)
                    {
                        PlayerSeenDuringCurrentWanted = true;
                        PlayerSeenDuringWanted = true;
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
                    int PoliceKilled = InstancesOfCrime("KillingPolice");
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
    }
}