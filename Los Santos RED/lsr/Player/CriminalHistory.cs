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
        private BOLO CurrentHistory;
        private IPoliceRespondable Player;
        public CriminalHistory(IPoliceRespondable currentPlayer)
        {
            Player = currentPlayer;
        }
             private int LastWantedMaxLevel => CurrentHistory == null ? 0 : CurrentHistory.WantedLevel;
        private float SearchRadius => LastWantedMaxLevel > 0 ? LastWantedMaxLevel * 400f : 400f;
        public bool HasHistory => CurrentHistory != null;
        public void OnSuspectEluded(List<Crime> CrimesAssociated,Vector3 PlaceLastSeen)
        {
            CurrentHistory = new BOLO(PlaceLastSeen, CrimesAssociated, CrimesAssociated.Max(x=> x.ResultingWantedLevel));
        }
        public void OnLostWanted()
        {
            //clear criminal history?
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
                    else if (IsNearLastSeenLocation() && Player.PoliceResponse.HasBeenNotWantedFor >= 5000)//move the second one OUT
                    {
                        ApplyLastWantedStats();
                    }
                    else if (Player.IsInVehicle && Player.CurrentVehicle != null && Player.CurrentVehicle.CopsRecognizeAsStolen)
                    {
                        ApplyLastWantedStats();
                    }
                }
                if(HasHistory && Player.PoliceResponse.HasBeenNotWantedFor >= 120000)
                {
                    Clear();
                    EntryPoint.WriteToConsole("CRIMINAL HISTORY EVENT: History Expired", 3);
                }
            }
        }
        public void Clear()
        {
            CurrentHistory = null;
            EntryPoint.WriteToConsole($" PLAYER EVENT: Criminal History Clear", 3);
        }
        public void PrintCriminalHistory()
        {
            if(CurrentHistory != null)
            {
                EntryPoint.WriteToConsole("-------------------------------Criminal History Start", 3);
                EntryPoint.WriteToConsole($"Wanted Level: {CurrentHistory.WantedLevel}", 3);
                EntryPoint.WriteToConsole($"LastSeenLocation: {CurrentHistory.LastSeenLocation}", 3);
                foreach(Crime crime in CurrentHistory.Crimes)
                {
                    EntryPoint.WriteToConsole($" Crime: {crime.Name}, {crime.ResultingWantedLevel}", 3);
                }
                EntryPoint.WriteToConsole("-------------------------------Criminal History End", 3);
            }
        }
        private bool IsNearLastSeenLocation()
        {
            if(CurrentHistory != null && Player.Position.DistanceTo2D(CurrentHistory.LastSeenLocation) <= SearchRadius)
            {
                return true;
            }
            return false;
        }
        private void ApplyLastWantedStats()
        {
            if(CurrentHistory != null)
            {
                foreach(Crime crime in CurrentHistory.Crimes)
                {
                    EntryPoint.WriteToConsole($"PLAYER EVENT: APPLYING WANTED STATS: ADDING CRIME: {crime.Name}", 3);
                    Player.AddCrime(crime, true, Player.Position, Player.CurrentSeenVehicle, Player.CurrentSeenWeapon, true,false);
                }
                CurrentHistory = null;
                Player.OnAppliedWantedStats();
            }
        }
        private class BOLO
        {
            public Vector3 LastSeenLocation { get; set; }
            public List<Crime> Crimes = new List<Crime>();

            public BOLO(Vector3 lastSeenLocation, List<Crime> crimes, int wantedLevel)
            {
                LastSeenLocation = lastSeenLocation;
                Crimes = crimes;
                WantedLevel = wantedLevel;
            }

            public int WantedLevel { get; set; }
        }

    }


}