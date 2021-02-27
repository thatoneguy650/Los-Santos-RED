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
    public class CriminalHistory
    {
        private List<PoliceResponse> RapSheetList = new List<PoliceResponse>();
        private uint GameTimeLastAppliedWantedStats;
        private IPoliceRespondable Player;
        public CriminalHistory(IPoliceRespondable currentPlayer)
        {
            Player = currentPlayer;
        }
        public int LastWantedMaxLevel => LastResponse == null ? 0 : LastResponse.ObservedMaxWantedLevel;
        public bool RecentlyAppliedWantedStats => GameTimeLastAppliedWantedStats != 0 && Game.GameTime - GameTimeLastAppliedWantedStats <= 5000;
        public float SearchRadius => LastWantedMaxLevel > 0 ? LastWantedMaxLevel * 400f : 400f;
        private bool HasHistory => RapSheetList.Any();
        private PoliceResponse LastResponse => RapSheetList.Where(x => x.PlayerSeenDuringWanted).OrderByDescending(x => x.GameTimeWantedEnded).OrderByDescending(x => x.GameTimeWantedStarted).FirstOrDefault();
        public void StoreCriminalHistory(PoliceResponse rapSheet)
        {
            RapSheetList.Add(rapSheet);
        }
        public void Update()
        {
            if (Player.IsAliveAndFree)
            {
                if (HasHistory && Player.AnyPoliceCanRecognizePlayer)
                {
                    if (Player.IsWanted)
                    {
                        ApplyLastWantedStats();
                    }
                    else if (Player.PoliceResponse.NearLastWanted(SearchRadius) && Player.PoliceResponse.HasBeenNotWantedFor >= 5000)
                    {
                        ApplyLastWantedStats();
                    }
                    else if (Player.IsInVehicle && Player.CurrentVehicle != null && Player.CurrentVehicle.CopsRecognizeAsStolen)
                    {
                        ApplyWantedStatsForPlate(Player.CurrentVehicle.CarPlate.PlateNumber);
                    }
                }
                RapSheetList.RemoveAll(x => x.HasBeenNotWantedFor >= 120000);
            }
        }
        public void Clear()
        {
            RapSheetList.Clear();
        }
        private void ApplyLastWantedStats()
        {
            ApplyWantedStats(LastResponse);
        }
        private void ApplyWantedStats(PoliceResponse CriminalHistory)
        {
            if (CriminalHistory != null)
            {
                //if (Player.WantedLevel < CriminalHistory.ObservedMaxWantedLevel)
                //{
                //    Player.PoliceResponse.SetWantedLevel(CriminalHistory.ObservedMaxWantedLevel, "Applying old Wanted stats", true);
                //}
                RapSheetList.Remove(CriminalHistory);
                foreach(CrimeEvent crime in CriminalHistory.CrimesObserved)
                {
                    Player.AddCrime(crime.AssociatedCrime, true, Player.Position, Player.CurrentSeenVehicle, Player.CurrentSeenWeapon, true);
                }
                //Player.PoliceResponse = CriminalHistory;
                GameTimeLastAppliedWantedStats = Game.GameTime;
                EntryPoint.WriteToConsole($"PLAYER EVENT: APPLYING WANTED STATS", 3);
            }
        }
        private void ApplyWantedStatsForPlate(string PlateNumber)
        {
            ApplyWantedStats(RapSheetList.Where(x => x.PlayerSeenDuringWanted && x.WantedPlates.Any(y => y.PlateNumber == PlateNumber)).OrderByDescending(x => x.GameTimeWantedEnded).OrderByDescending(x => x.GameTimeWantedStarted).FirstOrDefault());
        }
    }
}