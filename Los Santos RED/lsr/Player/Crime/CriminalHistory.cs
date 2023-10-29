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
        private float PlayerDistanceToLastSeen = 9999f;
        private Color blipColor => IsNearLastSeenLocation ? Color.Orange : Color.Yellow;
        public bool IsNearLastSeenLocation { get; set; }
        public bool IsWithinMarshalDistance => HasHistory && PlayerDistanceToLastSeen <= SearchRadius + Settings.SettingsManager.PoliceSettings.MarshalsAPBResponseExtraRadiusDistance;
        public CriminalHistory(IPoliceRespondable currentPlayer, ISettingsProvideable settings, ITimeReportable time)
        {
            Player = currentPlayer;
            Settings = settings;
            Time = time;
        }
        private int LastWantedMaxLevel => CurrentHistory == null ? 0 : CurrentHistory.WantedLevel;
        private float SearchRadius => LastWantedMaxLevel > 0 ? LastWantedMaxLevel * Settings.SettingsManager.CriminalHistorySettings.SearchRadiusIncrement : Settings.SettingsManager.CriminalHistorySettings.MinimumSearchRadius;// 400f;
        public bool HasHistory => CurrentHistory != null;
        public bool HasDeadlyHistory => CurrentHistory != null && CurrentHistory.Crimes.Any(x => x.AssociatedCrime.ResultsInLethalForce);
        public int MaxWantedLevel => LastWantedMaxLevel;
        public List<Crime> WantedCrimes => CurrentHistory?.Crimes.Select(x => x.AssociatedCrime).ToList();
        public void Dispose()
        {
            if (CriminalHistoryBlip.Exists())
            {
                CriminalHistoryBlip.Delete();
            }
        }
        public void OnSuspectEluded(List<CrimeEvent> CrimesAssociated,Vector3 PlaceLastSeen)
        {
            if (CrimesAssociated != null && PlaceLastSeen != Vector3.Zero)
            {
                CurrentHistory = new BOLO(PlaceLastSeen,  CrimesAssociated, Player.WantedLevel);
            }
        }
        public void OnLostWanted()
        {
            //clear criminal history?
        }
        public void Update()
        {
            UpdateData();
            //GameFiber.Yield();//TR 05
            UpdateBlip();
        }
        public void Reset()
        {
            Clear();
        }
        public void Clear()
        {
            CurrentHistory = null;
            //EntryPoint.WriteToConsole($" PLAYER EVENT: Criminal History Clear");
        }
        public void AddCrime(Crime crime)
        {
            Player.PoliceResponse.OnLostWanted();
            if (CurrentHistory == null)
            {
                CurrentHistory = new BOLO(Vector3.Zero,new List<CrimeEvent>() { new CrimeEvent(crime,null) }, crime.ResultingWantedLevel);
            }
            else
            {
                if (!CurrentHistory.Crimes.Any(x => x.AssociatedCrime != null && x.AssociatedCrime.Name == crime.Name))
                {
                    CurrentHistory.Crimes.Add(new CrimeEvent(crime, null));
                }
            }
        }
        public string PrintCriminalHistory()
        {
            if(CurrentHistory != null)
            {
                string CrimeString = "";
                foreach (CrimeEvent MyCrime in CurrentHistory.Crimes.Where(x=> x.AssociatedCrime != null).OrderBy(x => x.AssociatedCrime.Priority).Take(3))
                {
                    CrimeString += string.Format("~n~{0}~s~", MyCrime.AssociatedCrime.Name);
                }
                return CrimeString;
            }
            return "";
        }
        private void UpdateData()
        {
            if (!Player.IsAliveAndFree || !HasHistory)
            {
                PlayerDistanceToLastSeen = 9999f;
                IsNearLastSeenLocation = false;
                return;
            }
            IsNearLastSeenLocation = UpdateLastSeenDistance();
            if (Player.AnyPoliceCanRecognizePlayer)
            {
                if (Player.IsWanted)
                {
                    ApplyLastWantedStats();
                    GameFiber.Yield();//TR 05
                    //EntryPoint.WriteToConsole("CRIMINAL HISTORY EVENT: Became Wanted");
                }
                else if (IsNearLastSeenLocation && Player.PoliceResponse.HasBeenNotWantedFor >= 5000)//move the second one OUT
                {
                    ApplyLastWantedStats();
                    GameFiber.Yield();//TR 05
                    //EntryPoint.WriteToConsole("CRIMINAL HISTORY EVENT: Near Last Location");
                }
                else if (Player.IsInVehicle && Player.CurrentVehicle != null && Player.CurrentVehicle.IsWanted)//.CopsRecognizeAsStolen)
                {
                    ApplyLastWantedStats();
                    GameFiber.Yield();//TR 05
                    //EntryPoint.WriteToConsole("CRIMINAL HISTORY EVENT: Recognized Vehicle");
                }
            }
            if (Player.PoliceResponse.HasBeenNotWantedFor >= (Settings.SettingsManager.CriminalHistorySettings.RealTimeExpireWantedMultiplier * LastWantedMaxLevel))// 120000)
            {
                Clear();
                //EntryPoint.WriteToConsole("CRIMINAL HISTORY EVENT: History Expired (Real Time)");
            }
            if (DateTime.Compare(Player.PoliceResponse.DateTimeLastWantedEnded.AddHours(LastWantedMaxLevel * Settings.SettingsManager.CriminalHistorySettings.CalendarTimeExpireWantedMultiplier), Time.CurrentDateTime) < 0)
            {
                //EntryPoint.WriteToConsole($"POLICE RESPONSE: Lost Wanted ToExpire: {Player.PoliceResponse.DateTimeLastWantedEnded.AddHours(LastWantedMaxLevel * Settings.SettingsManager.CriminalHistorySettings.CalendarTimeExpireWantedMultiplier)} Current: {Time.CurrentDateTime}");
                Clear();
                //EntryPoint.WriteToConsole("CRIMINAL HISTORY EVENT: History Expired (Calendar Time)");
            }    

            if(Player.IsWanted && Player.PoliceResponse.WantedLevelHasBeenRadioedIn && HasHistory)
            {
                CurrentHistory = null;
            }
        }
        private bool UpdateLastSeenDistance()
        {
            if(CurrentHistory == null)
            {
                PlayerDistanceToLastSeen = 9999f;
                return false;
            }
            PlayerDistanceToLastSeen = Player.Position.DistanceTo2D(CurrentHistory.LastSeenLocation);
            if (PlayerDistanceToLastSeen <= SearchRadius)
            {
                return true;
            }
            return false;
        }
        private void ApplyLastWantedStats()
        {
            if(CurrentHistory == null)
            {
                return;
            }
            foreach(CrimeEvent crime in CurrentHistory.Crimes)
            {
                //EntryPoint.WriteToConsole($"PLAYER EVENT: APPLYING WANTED STATS: ADDING CRIME: {crime.Name}");
                Player.AddCrime(crime.AssociatedCrime, true, Player.Position, Player.CurrentSeenVehicle, Player.WeaponEquipment.CurrentSeenWeapon, true,false, true);
                CrimeEvent addedCrime = Player.PoliceResponse.CrimesObserved.Where(x => x.AssociatedCrime?.ID == crime.AssociatedCrime.ID).FirstOrDefault();
                if(addedCrime != null)
                {
                    addedCrime.Instances = crime.Instances;
                }
            }
            int highestWantedLevel = CurrentHistory.WantedLevel;
            //CurrentHistory = null;
            Player.OnAppliedWantedStats(highestWantedLevel);        
        }
        private void UpdateBlip()
        {
            if (HasHistory && Player.IsNotWanted && Settings.SettingsManager.CriminalHistorySettings.CreateBlip)
            {
                CreateBlip();
            }
            else
            {
                RemoveBlip();
            }
        }
        private void CreateBlip()
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

                EntryPoint.WriteToConsole($"CRIMINAL HISORY BLIP CREATED");
                //GameFiber.Yield();//TR Yield RemovedTest 1
            }
            else
            {
                CriminalHistoryBlip.Position = CurrentHistory.LastSeenLocation;
                CriminalHistoryBlip.Color = blipColor;
            }
        }
        private void RemoveBlip()
        {
            if (CriminalHistoryBlip.Exists())
            {
                CriminalHistoryBlip.Delete();
            }
        }
       

    }


}