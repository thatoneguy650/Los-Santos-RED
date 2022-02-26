using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI;
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
        private ISettingsProvideable Settings;
        private ITimeReportable Time;
        private Blip CriminalHistoryBlip;
        private Color blipColor => IsNearLastSeenLocation ? Color.Orange : Color.Yellow;
        public bool IsNearLastSeenLocation { get; set; }
        public CriminalHistory(IPoliceRespondable currentPlayer, ISettingsProvideable settings, ITimeReportable time)
        {
            Player = currentPlayer;
            Settings = settings;
            Time = time;
        }
        private int LastWantedMaxLevel => CurrentHistory == null ? 0 : CurrentHistory.WantedLevel;
        private float SearchRadius => LastWantedMaxLevel > 0 ? LastWantedMaxLevel * Settings.SettingsManager.CriminalHistorySettings.SearchRadiusIncrement : Settings.SettingsManager.CriminalHistorySettings.MinimumSearchRadius;// 400f;
        public bool HasHistory => CurrentHistory != null;
        public bool HasDeadlyHistory => CurrentHistory != null && CurrentHistory.Crimes.Any(x => x.ResultsInLethalForce);
        public int MaxWantedLevel => LastWantedMaxLevel;
        public List<Crime> WantedCrimes => CurrentHistory?.Crimes;
        public void Dispose()
        {
            if (CriminalHistoryBlip.Exists())
            {
                CriminalHistoryBlip.Delete();
            }
        }
        public void OnSuspectEluded(List<Crime> CrimesAssociated,Vector3 PlaceLastSeen)
        {
            bool isDeadly = CrimesAssociated.Any(x => x.ResultsInLethalForce);
            CurrentHistory = new BOLO(PlaceLastSeen, CrimesAssociated, CrimesAssociated == null ? 1 : CrimesAssociated.Max(x=> x.ResultingWantedLevel));
            if(isDeadly)
            {
               // Player.BigMessage.ShowColoredShard("APB Issued", "", HudColor.Gold, HudColor.InGameBackground, 2500);
            }
            else
            {
               // Player.BigMessage.ShowColoredShard("BOLO Issued", "", HudColor.Red, HudColor.InGameBackground, 2500);
            }
        }
        public void OnLostWanted()
        {
            //clear criminal history?
        }
        public void Update()
        {
            if (Player.IsAliveAndFree && HasHistory)
            {
                IsNearLastSeenLocation = UpdateLastSeenDistance();
                if (Player.AnyPoliceCanRecognizePlayer)
                {
                    if (Player.IsWanted)
                    {
                        ApplyLastWantedStats();
                        EntryPoint.WriteToConsole("CRIMINAL HISTORY EVENT: Became Wanted", 3);
                    }
                    else if (IsNearLastSeenLocation && Player.PoliceResponse.HasBeenNotWantedFor >= 5000)//move the second one OUT
                    {
                        ApplyLastWantedStats();
                        EntryPoint.WriteToConsole("CRIMINAL HISTORY EVENT: Near Last Location", 3);
                    }
                    else if (Player.IsInVehicle && Player.CurrentVehicle != null && Player.CurrentVehicle.IsWanted)//.CopsRecognizeAsStolen)
                    {
                        ApplyLastWantedStats();
                        EntryPoint.WriteToConsole("CRIMINAL HISTORY EVENT: Recognized Vehicle", 3);
                    }
                }
                if(Player.PoliceResponse.HasBeenNotWantedFor >= (Settings.SettingsManager.CriminalHistorySettings.RealTimeExpireWantedMultiplier * LastWantedMaxLevel))// 120000)
                {
                    Clear();
                    EntryPoint.WriteToConsole("CRIMINAL HISTORY EVENT: History Expired (Real Time)", 3);
                }
                if(DateTime.Compare(Player.PoliceResponse.DateTimeLastWantedEnded.AddHours(LastWantedMaxLevel * Settings.SettingsManager.CriminalHistorySettings.CalendarTimeExpireWantedMultiplier), Time.CurrentDateTime) < 0)
                {
                    EntryPoint.WriteToConsole($"POLICE RESPONSE: Lost Wanted ToExpire: {Player.PoliceResponse.DateTimeLastWantedEnded.AddHours(LastWantedMaxLevel * Settings.SettingsManager.CriminalHistorySettings.CalendarTimeExpireWantedMultiplier)} Current: {Time.CurrentDateTime}", 5);
                    Clear();
                    EntryPoint.WriteToConsole("CRIMINAL HISTORY EVENT: History Expired (Calendar Time)", 3);
                }
            }
            UpdateBlip();
        }
        public void Clear()
        {
            CurrentHistory = null;
            EntryPoint.WriteToConsole($" PLAYER EVENT: Criminal History Clear", 3);
        }
        public void AddCrime(Crime crime)
        {
            Player.PoliceResponse.OnLostWanted();
            if (CurrentHistory == null)
            {
                CurrentHistory = new BOLO(Vector3.Zero,new List<Crime>() { crime }, crime.ResultingWantedLevel);
            }
            else
            {
                if (!CurrentHistory.Crimes.Any(x => x.Name == crime.Name))
                {
                    CurrentHistory.Crimes.Add(crime);
                }
            }
        }
        public string PrintCriminalHistory()
        {
            if(CurrentHistory != null)
            {

                string CrimeString = "";
                foreach (Crime MyCrime in CurrentHistory.Crimes.OrderBy(x => x.Priority).Take(3))
                {
                    CrimeString += string.Format("~n~{0}~s~", MyCrime.Name);
                }
                return CrimeString;




                //EntryPoint.WriteToConsole("-------------------------------Criminal History Start", 3);
                //EntryPoint.WriteToConsole($"Wanted Level: {CurrentHistory.WantedLevel}", 3);
                //EntryPoint.WriteToConsole($"LastSeenLocation: {CurrentHistory.LastSeenLocation}", 3);
                //foreach(Crime crime in CurrentHistory.Crimes)
                //{
                //    EntryPoint.WriteToConsole($" Crime: {crime.Name}, {crime.ResultingWantedLevel}", 3);
                //}
                //EntryPoint.WriteToConsole("-------------------------------Criminal History End", 3);
            }
            return "";
        }
        private bool UpdateLastSeenDistance()
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
                    Player.AddCrime(crime, true, Player.Position, Player.CurrentSeenVehicle, Player.CurrentSeenWeapon, true,false, true);
                }
                int highestWantedLevel = CurrentHistory.WantedLevel;
                CurrentHistory = null;
                Player.OnAppliedWantedStats(highestWantedLevel);
            }
        }
        private void UpdateBlip()
        {
            if (HasHistory && Player.IsNotWanted && Settings.SettingsManager.CriminalHistorySettings.CreateBlip)
            {
                if (!CriminalHistoryBlip.Exists())
                {
                    CriminalHistoryBlip = new Blip(CurrentHistory.LastSeenLocation, SearchRadius)
                    {
                        Name = "APB Center",
                        Color = blipColor,//Color.Yellow,
                        Alpha = 0.25f
                    };
                    NativeFunction.Natives.BEGIN_TEXT_COMMAND_SET_BLIP_NAME("STRING");
                    NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME("APB Center");
                    NativeFunction.Natives.END_TEXT_COMMAND_SET_BLIP_NAME(CriminalHistoryBlip);
                    NativeFunction.Natives.SET_BLIP_AS_SHORT_RANGE((uint)CriminalHistoryBlip.Handle, true);
                    //GameFiber.Yield();//TR Yield RemovedTest 1
                }
                else
                {
                    CriminalHistoryBlip.Position = CurrentHistory.LastSeenLocation;
                    CriminalHistoryBlip.Color = blipColor;
                }
            }
            else
            {
                if (CriminalHistoryBlip.Exists())
                {
                    CriminalHistoryBlip.Delete();
                }
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