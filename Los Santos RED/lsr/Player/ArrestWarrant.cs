using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
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
        private IPoliceRespondable Player;
        private uint GameTimeLastAppliedWantedStats;
        public ArrestWarrant(IPoliceRespondable currentPlayer)
        {
            Player = currentPlayer;
        }
        public bool IsActive { get; private set; }
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
        public int LastWantedMaxLevel
        {
            get
            {
                CriminalHistory LastWanted = GetLastWantedStats();
                if (LastWanted != null)
                {
                    return LastWanted.ObservedMaxWantedLevel;
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
                if (LastWantedMaxLevel > 0)
                {
                    return LastWantedMaxLevel * 400f;
                }
                else
                {
                    return 400f;
                }
            }
        }
        public void Update()
        {
            if (!Player.IsDead && !Player.IsBusted)
            {
                CheckCurrentVehicle();
                CheckSight();

                if (Player.IsWanted)
                {
                    if (!IsActive && Player.AnyPoliceCanSeePlayer)
                    {
                        IsActive = true;
                    }
                }
                else
                {
                    if (IsActive && Player.CurrentPoliceResponse.HasBeenNotWantedFor >= 120000)
                    {
                        Reset();
                    }
                }
            }
        }
        public void StoreCriminalHistory(CriminalHistory rapSheet)
        {
            CriminalHistoryList.Add(rapSheet);
            //Game.Console.Print("Arrest Warrant! Stored Criminal History");
        }
        public void Reset()
        {
            IsActive = false;
            CriminalHistoryList.Clear();
            //Game.Console.Print("Arrest Warrant! History Cleared");
        }
        private void CheckCurrentVehicle()
        {
            if ((Player.IsNotWanted || Player.WantedLevel == 1) && Player.AnyPoliceCanRecognizePlayer && Player.IsInVehicle && Game.LocalPlayer.Character.IsInAnyVehicle(false))//first check is cheaper, but second is required to verify
            {
                VehicleExt VehicleToCheck = Player.CurrentVehicle;

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
            if (IsActive && Player.AnyPoliceCanSeePlayer)
            {
                if (Player.IsWanted)
                {
                    ApplyLastWantedStats();
                }
                else
                {
                    if (Player.CurrentPoliceResponse.NearLastWanted(SearchRadius) && Player.CurrentPoliceResponse.HasBeenNotWantedFor >= 5000)
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
            if (Player.WantedLevel < CriminalHistory.ObservedMaxWantedLevel)
            {
                Player.CurrentPoliceResponse.SetWantedLevel(CriminalHistory.ObservedMaxWantedLevel, "Applying old Wanted stats", true);
            }
            CriminalHistoryList.Remove(CriminalHistory);
            Player.CurrentPoliceResponse.CurrentCrimes = CriminalHistory;

            GameTimeLastAppliedWantedStats = Game.GameTime;
            //Game.Console.Print("WantedLevelStats Replace" + CurrentPlayer.CurrentPoliceResponse.CurrentCrimes.DebugPrintCrimes());
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