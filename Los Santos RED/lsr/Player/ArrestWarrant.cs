using LosSantosRED.lsr;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr
{
    public class ArrestWarrant
    {
        private Player CurrentPlayer;
        private uint GameTimeLastAppliedWantedStats;
        public ArrestWarrant(Player currentPlayer)
        {
            CurrentPlayer = currentPlayer;
        }
        public bool IsPersonOfInterest { get; private set; }
        public List<CriminalHistory> CriminalHistoryList { get; set; } = new List<CriminalHistory>();
        public bool RecentlyAppliedWantedStats
        {
            get
            {
                if (GameTimeLastAppliedWantedStats == 0)
                {
                    return false;
                }
                else
                {
                    return Game.GameTime - GameTimeLastAppliedWantedStats <= 5000;
                }
            }
        }
        public int MaxWantedLevel
        {
            get
            {
                CriminalHistory LastWanted = GetLastWantedStats();
                if (LastWanted != null)
                {
                    return LastWanted.MaxWantedLevel;
                }
                else
                {
                    return 0;
                }
            }
        }
        public bool LethalForceAuthorized
        {
            get
            {
                CriminalHistory LastWanted = GetLastWantedStats();
                if (LastWanted != null)
                {
                    return LastWanted.LethalForceAuthorized;
                }
                else
                {
                    return false;
                }
            }
        }
        public float SearchRadius
        {
            get
            {
                if (MaxWantedLevel > 0)
                {
                    return MaxWantedLevel * Mod.DataMart.Settings.SettingsManager.Police.LastWantedCenterSize;
                }
                else
                {
                    return Mod.DataMart.Settings.SettingsManager.Police.LastWantedCenterSize;
                }
            }
        }
        public void Update()
        {
            if (!CurrentPlayer.IsDead && !CurrentPlayer.IsBusted)
            {
                CheckCurrentVehicle();
                CheckSight();

                if (CurrentPlayer.IsWanted)
                {
                    if (!IsPersonOfInterest && Mod.World.AnyPoliceCanSeePlayer)
                    {
                        IsPersonOfInterest = true;
                    }
                }
                else
                {
                    if (IsPersonOfInterest && CurrentPlayer.CurrentPoliceResponse.HasBeenNotWantedFor >= 120000)
                    {
                        Reset();
                    }
                }
            }
        }
        public void StoreCriminalHistory(CriminalHistory rapSheet)
        {
            CriminalHistoryList.Add(rapSheet);
            Mod.Debug.WriteToLog("StoreCriminalHistory", "Stored this Rap Sheet");
        }
        public void Reset()
        {
            IsPersonOfInterest = false;
            CriminalHistoryList.Clear();
            Mod.Debug.WriteToLog("ResetPersonOfInterest", "All Previous wanted items are cleared");
        }
        private void CheckCurrentVehicle()
        {
            if ((CurrentPlayer.IsNotWanted || CurrentPlayer.WantedLevel == 1) && Mod.World.AnyPoliceCanRecognizePlayer && CurrentPlayer.IsInVehicle && Game.LocalPlayer.Character.IsInAnyVehicle(false))//first check is cheaper, but second is required to verify
            {
                VehicleExt VehicleToCheck = CurrentPlayer.CurrentVehicle;

                if (VehicleToCheck == null)
                {
                    return;
                }

                if (VehicleToCheck.CopsRecognizeAsStolen)
                {
                    ApplyWantedStatsForPlate(VehicleToCheck.CarPlate.PlateNumber);
                }
            }
        }
        private void CheckSight()
        {
            if (IsPersonOfInterest && Mod.World.AnyPoliceCanSeePlayer)
            {
                if (CurrentPlayer.IsWanted)
                {
                    ApplyLastWantedStats();
                }
                else
                {
                    if (CurrentPlayer.CurrentPoliceResponse.NearLastWanted(SearchRadius) && CurrentPlayer.CurrentPoliceResponse.HasBeenNotWantedFor >= 5000)
                    {
                        ApplyLastWantedStats();
                    }
                }
            }
        }
        private void ApplyWantedStatsForPlate(string PlateNumber)
        {
            CriminalHistory StatsForPlate = GetWantedLevelStatsForPlate(PlateNumber);
            if (StatsForPlate != null)
            {
                ApplyWantedStats(StatsForPlate);
            }
        }
        private void ApplyLastWantedStats()
        {
            CriminalHistory CriminalHistory = GetLastWantedStats();
            if (CriminalHistory != null)
            {
                ApplyWantedStats(CriminalHistory);
            }
        }
        private void ApplyWantedStats(CriminalHistory CriminalHistory)
        {
            if (CriminalHistory == null)
            {
                return;
            }
            if (CurrentPlayer.WantedLevel < CriminalHistory.MaxWantedLevel)
            {
                CurrentPlayer.CurrentPoliceResponse.SetWantedLevel(CriminalHistory.MaxWantedLevel, "Applying old Wanted stats", true);
            }
            CriminalHistoryList.Remove(CriminalHistory);
            CurrentPlayer.CurrentPoliceResponse.CurrentCrimes = CriminalHistory;

            GameTimeLastAppliedWantedStats = Game.GameTime;
            Mod.Debug.WriteToLog("WantedLevelStats Replace", CurrentPlayer.CurrentPoliceResponse.CurrentCrimes.DebugPrintCrimes());
        }
        private CriminalHistory GetLastWantedStats()
        {
            if (CriminalHistoryList == null || !CriminalHistoryList.Where(x => x.PlayerSeenDuringWanted).Any())
            {
                return null;
            }

            return CriminalHistoryList.Where(x => x.PlayerSeenDuringWanted).OrderByDescending(x => x.GameTimeWantedEnded).OrderByDescending(x => x.GameTimeWantedStarted).FirstOrDefault();
        }
        private CriminalHistory GetWantedLevelStatsForPlate(string PlateNumber)
        {
            if (CriminalHistoryList == null || !CriminalHistoryList.Where(x => x.PlayerSeenDuringWanted).Any())
            {
                return null;
            }

            return CriminalHistoryList.Where(x => x.PlayerSeenDuringWanted && x.WantedPlates.Any(y => y.PlateNumber == PlateNumber)).OrderByDescending(x => x.GameTimeWantedEnded).OrderByDescending(x => x.GameTimeWantedStarted).FirstOrDefault();
        }
    }
}