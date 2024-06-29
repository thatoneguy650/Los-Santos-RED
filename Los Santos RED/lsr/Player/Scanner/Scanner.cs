using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static DispatchScannerFiles;

//Needs some refactoring
namespace LosSantosRED.lsr
{
    public class Scanner
    {
        private IPoliceRespondable Player;
        private IEntityProvideable World;
        private IPlacesOfInterest PlacesOfInterest;
        private ISettingsProvideable Settings;    
        private ITimeReportable Time;
        private IAudioPlayable AudioPlayer;
        private IAudioPlayable SecondaryAudioPlayer;   
        private ScannerDispatchInformation SDI;
        private VehicleScannerAudio VehicleScannerAudio;
        private ZoneScannerAudio ZoneScannerAudio;
        private StreetScannerAudio StreetScannerAudio;
        private CallsignScannerAudio CallsignScannerAudio;

        private bool AbortedAudio;
        private bool ExecutingQueue;
        private bool Flipper = false;
        private bool ReportedLethalForceAuthorized;
        private bool ReportedRequestAirSupport;
        private bool ReportedWeaponsFree;
        private bool canHearScanner;
        private bool ExecutingAmbientQueue;

        private uint GameTimeLastAnnouncedDispatch;
        private uint GameTimeLastDisplayedSubtitle;
        private uint GameTimeLastMentionedStreet;
        private uint GameTimeLastMentionedUnits;
        private uint GameTimeLastMentionedZone;
        private uint GameTimeLastMentionedLocation;
        private uint GameTimeLastAddedAmbientDispatch;
        private uint GameTimeBetweenAmbientDispatches;
        private int HighestCivilianReportedPriority = 99;
        private int HighestOfficerReportedPriority = 99;
        private int CurrentUnitEnRouteID;

        private List<uint> NotificationHandles = new List<uint>();

        private List<AudioSet> LethalForce;
        private List<AudioSet> LicensePlateSet;
        private List<AudioSet> AttentionAllUnits;
        private List<AudioSet> OfficersReport;
        private List<AudioSet> CiviliansReport;
        private List<AudioSet> RespondCode2Set;
        private List<AudioSet> RespondCode3Set;
        private List<AudioSet> UnitEnRouteSet;
        private List<string> RadioEnd;
        private List<string> RadioStart;

        private DispatchEvent CurrentlyPlaying;
        private CrimeSceneDescription CurrentlyPlayingCallIn;
        private Dispatch CurrentlyPlayingDispatch;

        private List<CrimeDispatch> DispatchLookup;
        private List<Dispatch> DispatchList = new List<Dispatch>();    
        private List<Dispatch> DispatchQueue = new List<Dispatch>();
        private List<Dispatch> HoldingDispatchQueue = new List<Dispatch>();
        private List<Dispatch> AmbientDispatchQueue = new List<Dispatch>();

        private bool ShouldAddAmbientDispatch => Game.GameTime - GameTimeLastAddedAmbientDispatch >= GameTimeBetweenAmbientDispatches;
        private float DesiredVolume => Settings.SettingsManager.ScannerSettings.AudioVolume + (ScannerBoostLevel * Settings.SettingsManager.ScannerSettings.AudioVolumeBoostAmount);

        public Scanner(IEntityProvideable world, IPoliceRespondable currentPlayer, IAudioPlayable audioPlayer, IAudioPlayable secondaryAudioPlayer,  ISettingsProvideable settings, ITimeReportable time, IPlacesOfInterest placesOfInterest)
        {
            AudioPlayer = audioPlayer;
            SecondaryAudioPlayer = secondaryAudioPlayer;
            Player = currentPlayer;
            World = world;
            Settings = settings;
            Time = time;
            VehicleScannerAudio = new VehicleScannerAudio();
            StreetScannerAudio = new StreetScannerAudio();
            ZoneScannerAudio = new ZoneScannerAudio();
            CallsignScannerAudio = new CallsignScannerAudio();
            PlacesOfInterest = placesOfInterest;
        }
        public bool RecentlyAnnouncedDispatch => GameTimeLastAnnouncedDispatch != 0 && Game.GameTime - GameTimeLastAnnouncedDispatch <= 25000;
        public bool RecentlyMentionedStreet => GameTimeLastMentionedStreet != 0 && Game.GameTime - GameTimeLastMentionedStreet <= 10000;
        public bool RecentlyMentionedLocation => GameTimeLastMentionedLocation != 0 && Game.GameTime - GameTimeLastMentionedLocation <= 15000;
        public bool RecentlyMentionedUnits => GameTimeLastMentionedUnits != 0 && Game.GameTime - GameTimeLastMentionedUnits <= 10000;
        public bool RecentlyMentionedZone => GameTimeLastMentionedZone != 0 && Game.GameTime - GameTimeLastMentionedZone <= 10000;
        public bool VeryRecentlyAnnouncedDispatch => GameTimeLastAnnouncedDispatch != 0 && Game.GameTime - GameTimeLastAnnouncedDispatch <= 10000;
        public int ScannerBoostLevel { get; set; } = 0;
        public bool CanHearAmbient { get; set; } = false;
        public void Setup()
        {
            VehicleScannerAudio.ReadConfig();
            StreetScannerAudio.ReadConfig();
            ZoneScannerAudio.ReadConfig();
            CallsignScannerAudio.ReadConfig();
            DefaultConfig();
            GameTimeBetweenAmbientDispatches = RandomItems.GetRandomNumber(Settings.SettingsManager.ScannerSettings.AmbientDispatchesMinTimeBetween, Settings.SettingsManager.ScannerSettings.AmbientDispatchesMaxTimeBetween);
        }
        public void Update()
        {
            if (Settings.SettingsManager.ScannerSettings.IsEnabled && Player.ActivityManager.CanHearScanner)
            {
                UpdateDispatch();
            }
            if(Player.ActivityManager.CanHearScanner != canHearScanner)
            {
                OnCanHearScannerChanged();
            }
        }
        public void Reset()
        {
            DispatchQueue.Clear();
            ReportedLethalForceAuthorized = false;
            ReportedWeaponsFree = false;
            ReportedRequestAirSupport = false;
            HighestCivilianReportedPriority = 99;
            HighestOfficerReportedPriority = 99;
            foreach (Dispatch ToReset in DispatchList)
            {
                ToReset.HasBeenPlayedThisWanted = false;
                ToReset.LatestInformation = new CrimeSceneDescription();
                ToReset.TimesPlayed = 0;
            }
            //newish
            GameTimeLastAnnouncedDispatch = 0;
            GameTimeLastDisplayedSubtitle = 0;
            GameTimeLastMentionedStreet = 0;
            GameTimeLastMentionedZone = 0;
            GameTimeLastMentionedUnits = 0;
            GameTimeLastMentionedLocation = 0;
            //end newish
            DispatchQueue.Clear();
            HoldingDispatchQueue.Clear();
        }
        public void Dispose()
        {
            Abort();
        }
        public void Abort()
        {
            AudioPlayer.Abort();
            RemoveAllNotifications();
        }
        public void AnnounceCrime(Crime crimeAssociated, CrimeSceneDescription reportInformation)
        {
            Dispatch ToAnnounce = DetermineDispatchFromCrime(crimeAssociated);
            if (ToAnnounce != null)
            {
                if (!ToAnnounce.HasVeryRecentlyBeenPlayed && ((ToAnnounce.CanBeReportedMultipleTimes && ToAnnounce.TimesPlayed <= 2) || ToAnnounce.TimesPlayed == 0))
                {
                    if (reportInformation.SeenByOfficers)
                    {
                        if (ToAnnounce.Priority <= HighestOfficerReportedPriority)
                        {
                            AddToQueue(ToAnnounce, reportInformation);
                        }
                    }
                    else
                    {
                        if (Player.IsNotWanted)
                        {
                            if (ToAnnounce.Priority < HighestCivilianReportedPriority || (ToAnnounce.Priority == HighestCivilianReportedPriority && !ToAnnounce.HasRecentlyBeenPlayed))
                            {
                                AddToQueue(ToAnnounce, reportInformation);
                            }
                            else if(!RecentlyAnnouncedDispatch)
                            {
                                AddToQueue(SDI.CivilianReportUpdate, reportInformation);
                            }
                        }
                    }
                }
            }
        }
        public void ForceRandomDispatch()
        {
            Reset();
            AddToQueue(DispatchList.PickRandom());
        }
        private void UpdateDispatch()
        {
            if (Player.RecentlyStartedPlaying)
            {
                return;//don't care right when you become a new person
            }
            AddStatusDispatchesToQueue();
            AddAmbientDispatchesToQueue();
            AnnounceQueue();
        }
        private void AddStatusDispatchesToQueue()
        {
            if (Player.IsWanted && Player.IsAliveAndFree && Settings.SettingsManager.ScannerSettings.AllowStatusAnnouncements)
            {
                if (Player.PoliceResponse.HasBeenWantedFor > 25000 && Player.WantedLevel <= 4)
                {
                    if (!SDI.SuspectSpotted.HasRecentlyBeenPlayed && !VeryRecentlyAnnouncedDispatch && Player.AnyPoliceCanSeePlayer)
                    {
                        //EntryPoint.WriteToConsole($"SCANNER EVENT: ADDED SuspectSpotted", 3);
                        AddToQueue(SDI.SuspectSpotted, new CrimeSceneDescription(!Player.IsInVehicle, true, Game.LocalPlayer.Character.Position));
                    }
                    else if (!Player.AnyPoliceRecentlySeenPlayer && !SDI.AttemptToReacquireSuspect.HasRecentlyBeenPlayed && !SDI.SuspectEvaded.HasRecentlyBeenPlayed)
                    {
                        //EntryPoint.WriteToConsole($"SCANNER EVENT: ADDED AttemptToReacquireSuspect", 3);
                        AddToQueue(SDI.AttemptToReacquireSuspect, new CrimeSceneDescription(false, true, Player.PlacePoliceLastSeenPlayer));
                    }
                }
            }
            else
            {
                foreach (VehicleExt StolenCar in Player.ReportedStolenVehicles)
                {
                    StolenCar.AddedToReportedStolenQueue = true;
                    AddToQueue(SDI.AnnounceStolenVehicle, new CrimeSceneDescription(!Player.IsInVehicle, false, StolenCar.PlaceOriginallyEntered) { VehicleSeen = StolenCar });
                }
            }
        }
        private void AddAmbientDispatchesToQueue()
        {
            if(1==0 && ShouldAddAmbientDispatch)//turned completely off for now, needs major rethink
            {
                if (Player.IsAliveAndFree && Player.IsNotWanted && !Player.Investigation.IsActive && Settings.SettingsManager.ScannerSettings.AllowAmbientDispatches)
                {
                   // Reset();
                    Dispatch toPlay = DispatchList.Where(x => x.IsAmbientAllowed).PickRandom();
                    if (toPlay != null)
                    {
                        GameLocation basicLocation = PlacesOfInterest.AllLocations().PickRandom();
                        if(basicLocation != null)
                        {
                            CrimeSceneDescription csd = new CrimeSceneDescription(RandomItems.RandomPercent(45), RandomItems.RandomPercent(45), basicLocation.EntrancePosition, false);
                            AddToAmbientQueue(toPlay, csd);
                        } 
                    }
                }
                GameTimeLastAddedAmbientDispatch = Game.GameTime;
                GameTimeBetweenAmbientDispatches = RandomItems.GetRandomNumber(Settings.SettingsManager.ScannerSettings.AmbientDispatchesMinTimeBetween, Settings.SettingsManager.ScannerSettings.AmbientDispatchesMaxTimeBetween);
            }
        }
        private void AnnounceQueue()
        {
            if ((DispatchQueue.Count > 0 || HoldingDispatchQueue.Any()) && !ExecutingQueue)
            {
                ExecutingQueue = true;
                GameFiber.Yield();
                GameFiber PlayDispatchQueue = GameFiber.StartNew(delegate
                {
                    try
                    {
                        if(Player.IsWanted)
                        {
                            if (!Player.PoliceResponse.WantedLevelHasBeenRadioedIn)
                            {
                                HoldingDispatchQueue.AddRange(DispatchQueue.Where(x => x.LatestInformation.SeenByOfficers || x.IsPoliceStatus));
                                DispatchQueue.RemoveAll(x => x.LatestInformation.SeenByOfficers || x.IsPoliceStatus);
                                EntryPoint.WriteToConsole("Player is Wanted Without Radio In Holding Officer Reports");
                            }
                            else if(HoldingDispatchQueue.Any())
                            {
                                DispatchQueue.AddRange(HoldingDispatchQueue);
                                HoldingDispatchQueue.Clear();
                                EntryPoint.WriteToConsole("Player is Wanted and has radioed in. Restoring Officer Reports");
                            }
                        }

                        if (DispatchQueue.Any())
                        {
                            EntryPoint.WriteToConsole($"DISPATCHES {string.Join(",",DispatchQueue.Select(x=> x.Name))}");
                            //if (Player.IsWanted && !Player.PoliceResponse.WantedLevelHasBeenRadioedIn)// Settings.SettingsManager.PoliceSettings.AllowLosingWantedByKillingBeforeRadio && Player.PoliceResponse.HasBeenWantedFor <= Settings.SettingsManager.PoliceSettings.RadioInTime)
                            //{
                            //    EntryPoint.WriteToConsole("DOING RADIO IN SLEEP SINCE YOU JUST STARTED BEING WANTED");
                            //    while (!Player.PoliceResponse.WantedLevelHasBeenRadioedIn && Player.IsWanted)
                            //    {
                            //        GameFiber.Sleep(1000);
                            //    }
                            //    GameFiber.Sleep(1000);
                            //    //GameFiber.Sleep(Settings.SettingsManager.PoliceSettings.RadioInTime + 2500);
                            //    EntryPoint.WriteToConsole("RADIO SLEEP OVER");
                            //}
                            //else
                            //{
                            GameFiber.Sleep(RandomItems.MyRand.Next(Settings.SettingsManager.ScannerSettings.DelayMinTime, Settings.SettingsManager.ScannerSettings.DelayMaxTime));//GameFiber.Sleep(RandomItems.MyRand.Next(2500, 4500));//Next(1500, 2500)
                            // }
                            CleanQueue();
                            PlayQueue();
                        }
                        ExecutingQueue = false;
                    }
                    catch (Exception ex)
                    {
                        EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                        EntryPoint.ModController.CrashUnload();
                    }
                }, "PlayDispatchQueue");
            }

            if(1==0 && AmbientDispatchQueue.Count > 0 && !ExecutingAmbientQueue)//turned off completly for now
            {
                ExecutingAmbientQueue = true;
                GameFiber.Yield();
                GameFiber PlayDispatchQueue = GameFiber.StartNew(delegate
                {
                    try
                    {
                        PlayAmbientQueue();
                        ExecutingAmbientQueue = false;
                    }
                    catch (Exception ex)
                    {
                        EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                        EntryPoint.ModController.CrashUnload();
                    }
                }, "PlayDispatchQueue");
            }

        }
        private void CleanQueue()
        {

            if (Player.IsNotWanted || !Player.PoliceResponse.WantedLevelHasBeenRadioedIn)
            {
                DispatchQueue.RemoveAll(x => x.LatestInformation.SeenByOfficers || x.IsPoliceStatus);
            }


            if (DispatchQueue.Any(x => x.LatestInformation.SeenByOfficers))
            {
                DispatchQueue.RemoveAll(x => !x.LatestInformation.SeenByOfficers);
            }
            if (DispatchQueue.Count() > 1)
            {
                Dispatch HighestItem = DispatchQueue.OrderBy(x => x.Priority).FirstOrDefault();
                DispatchQueue.Clear();
                if (HighestItem != null)
                {
                    DispatchQueue.Add(HighestItem);
                }
            }
        }
        private void PlayQueue()
        {
            while (DispatchQueue.Count > 0)
            {
                Dispatch Item = DispatchQueue.OrderBy(x => x.Priority).ToList()[0];
                bool AddToPlayed = true;
                if (Player.IsNotWanted && Item.LatestInformation.SeenByOfficers)
                {
                    AddToPlayed = false;
                }
                BuildDispatch(Item, AddToPlayed, false);
                if (DispatchQueue.Contains(Item))
                {
                    DispatchQueue.Remove(Item);
                }
                GameFiber.Yield();
            }
        }
        private void PlayAmbientQueue()
        {
            while (AmbientDispatchQueue.Count > 0)
            {
                Dispatch Item = AmbientDispatchQueue.OrderBy(x => x.Priority).ToList()[0];
                BuildDispatch(Item, false, true);
                if (AmbientDispatchQueue.Contains(Item))
                {
                    AmbientDispatchQueue.Remove(Item);
                }
                GameFiber.Yield();
            }
        }

        //Events
        private void OnCanHearScannerChanged()
        {
            if (!Player.ActivityManager.CanHearScanner)
            {
                DispatchQueue.Clear();
            }
            canHearScanner = Player.ActivityManager.CanHearScanner;
        }
        public void OnAppliedWantedStats(int wantedLevel)
        {
            if (!SDI.WantedSuspectSpotted.HasRecentlyBeenPlayed)
            {
                if (wantedLevel == 1)
                {
                    AddToQueue(SDI.SuspectSpotted, new CrimeSceneDescription(!Player.IsInVehicle, true, Player.PlacePoliceLastSeenPlayer) { VehicleSeen = Player.CurrentVehicle });
                }
                else
                {
                    AddToQueue(SDI.WantedSuspectSpotted, new CrimeSceneDescription(!Player.IsInVehicle, true, Player.PlacePoliceLastSeenPlayer) { VehicleSeen = Player.CurrentVehicle });
                }
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: OnAppliedWantedStats", 3);
        }
        public void OnArmyDeployed()
        {
            if (Player.IsWanted && !SDI.RequestMilitaryUnits.HasBeenPlayedThisWanted && World.Pedestrians.AnyArmyUnitsSpawned)
            {
                AddToQueue(SDI.RequestMilitaryUnits);
            }
        }
        public void OnBribedPolice()
        {
            if (!SDI.ResumePatrol.HasRecentlyBeenPlayed)
            {
                AddToQueue(SDI.ResumePatrol);
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: OnBribedPolice", 3);
        }
        public void OnExcessiveSpeed()
        {
            if (!SDI.ExcessiveSpeed.HasRecentlyBeenPlayed && Player.IsInVehicle && Player.AnyPoliceCanSeePlayer && Player.WantedLevel <= 4)
            {
                SDI.ExcessiveSpeed.LatestInformation.SeenByOfficers = true;
                SDI.ExcessiveSpeed.LatestInformation.Speed = Game.LocalPlayer.Character.Speed;
                AddToQueue(SDI.ExcessiveSpeed);
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: OnExcessiveSpeed", 5);
        }
        public void OnFIBHETDeployed()
        {
            if (Player.IsWanted && Player.WantedLevel >= 5 && !SDI.RequestFIBUnits.HasBeenPlayedThisWanted)
            {
                AddToQueue(SDI.RequestFIBUnits);
            }
        }
        public void OnFirefightingServicesRequested()
        {
            if (!SDI.FirefightingServicesRequired.HasRecentlyBeenPlayed)
            {
                AddToQueue(SDI.FirefightingServicesRequired);
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: FirefightingServicesRequired", 3);
        }
        public void OnGotInVehicle()
        {
            if (!SDI.ChangedVehicles.HasRecentlyBeenPlayed && Player.CurrentVehicle != null && Player.CurrentVehicle.HasBeenDescribedByDispatch && Player.WantedLevel <= 4)
            {
                AddToQueue(SDI.ChangedVehicles);
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: InVehicle", 3);
        }
        public void OnGotOffFreeway()
        {
            if (!SDI.GotOffFreeway.HasRecentlyBeenPlayed && Player.IsInVehicle && Player.AnyPoliceCanSeePlayer && Player.WantedLevel <= 4)
            {
                SDI.GotOffFreeway.LatestInformation.SeenByOfficers = true;
                AddToQueue(SDI.GotOffFreeway);
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: OnGotOffFreeway", 5);
        }
        public void OnGotOnFreeway()
        {
            if (!SDI.GotOnFreeway.HasRecentlyBeenPlayed && Player.IsInVehicle && Player.AnyPoliceCanSeePlayer && Player.WantedLevel <= 4)
            {
                SDI.GotOnFreeway.LatestInformation.SeenByOfficers = true;
                AddToQueue(SDI.GotOnFreeway);
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: OnGotOnFreeway", 5);
        }
        public void OnWentInTunnel()
        {
            if (!SDI.WentInTunnel.HasRecentlyBeenPlayed && Player.AnyPoliceCanSeePlayer && Player.WantedLevel <= 4)
            {
                SDI.WentInTunnel.LatestInformation.SeenByOfficers = true;
                AddToQueue(SDI.WentInTunnel);
            }
             EntryPoint.WriteToConsole($"SCANNER EVENT: WENT IN TUNNEL", 3);
        }
        public void OnGotOutOfVehicle()
        {
            if (!SDI.OnFoot.HasRecentlyBeenPlayed && Player.WantedLevel <= 4)
            {
                AddToQueue(SDI.OnFoot);
            }
           // EntryPoint.WriteToConsole($"SCANNER EVENT: OnFoot", 3);
        }
        public void OnStoppingTrains()
        {
            if (!SDI.StoppingTrains.HasRecentlyBeenPlayed)
            {
                AddToQueue(SDI.StoppingTrains);
            }

        }
        public void OnHelicoptersDeployed()
        {
            if (Player.IsWanted && !ReportedRequestAirSupport && !SDI.RequestAirSupport.HasBeenPlayedThisWanted && !SDI.RequestSwatAirSupport.HasBeenPlayedThisWanted && World.Pedestrians.AnyHelicopterUnitsSpawned)
            {
                if (World.Pedestrians.AnyNooseUnitsSpawned && Player.WantedLevel >= 4)
                {
                    AddToQueue(SDI.RequestSwatAirSupport);
                }
                else
                {
                    AddToQueue(SDI.RequestAirSupport);
                }
            }
        }
        public void OnInvestigationExpire()
        {
            if (!SDI.NoFurtherUnitsNeeded.HasRecentlyBeenPlayed)
            {
                Reset();
                AddToQueue(SDI.NoFurtherUnitsNeeded);
            }
            else
            {
                Reset();
            }

           // EntryPoint.WriteToConsole($"SCANNER EVENT: OnInvestigationExpire", 3);
        }
        public void OnLethalForceAuthorized()
        {
            if (!ReportedLethalForceAuthorized && !SDI.LethalForceAuthorized.HasBeenPlayedThisWanted && Player.PoliceResponse.IsDeadlyChase)
            {
                AddToQueue(SDI.LethalForceAuthorized);
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: OnLethalForceAuthorized", 3);
        }
        public void OnMedicalServicesRequested()
        {
            if (!SDI.MedicalServicesRequired.HasRecentlyBeenPlayed && (SDI.MedicalServicesRequired.TimesPlayed <= 2 || SDI.MedicalServicesRequired.HasntBeenPlayedForAWhile))
            {
                AddToQueue(SDI.MedicalServicesRequired);
                //EntryPoint.WriteToConsole($"SCANNER EVENT: MedicalServicesRequired", 3);
            }
            
        }
        public void OnOfficerMIA()
        {
            if (!SDI.OfficerMIA.HasRecentlyBeenPlayed && (SDI.OfficerMIA.TimesPlayed <= 2 || SDI.OfficerMIA.HasntBeenPlayedForAWhile))
            {
                AddToQueue(SDI.OfficerMIA);
            }
        }
        public void OnNooseDeployed()
        {
            if (Player.IsWanted && Player.WantedLevel >= 4 && !SDI.RequestNooseUnitsAlt.HasBeenPlayedThisWanted && !SDI.RequestNooseUnitsAlt2.HasBeenPlayedThisWanted && World.Pedestrians.AnyNooseUnitsSpawned)
            {
                if (RandomItems.RandomPercent(50))
                {
                    AddToQueue(SDI.RequestNooseUnitsAlt);
                }
                else
                {
                    AddToQueue(SDI.RequestNooseUnitsAlt2);
                }
            }
        }
        public void OnPaidFine()
        {
            if (!SDI.ResumePatrol.HasRecentlyBeenPlayed)
            {
                AddToQueue(SDI.ResumePatrol);
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: OnBribedPolice", 3);
        }
        public void OnTalkedOutOfTicket()
        {
            OnPaidFine();
        }
        public void OnPlayerBusted()
        {
            if (!SDI.SuspectArrested.HasRecentlyBeenPlayed && Player.AnyPoliceCanSeePlayer && Player.WantedLevel > 1)
            {
                AddToQueue(SDI.SuspectArrested);
            }
           // EntryPoint.WriteToConsole($"SCANNER EVENT: OnSuspectBusted", 3);
        }
        public void OnPoliceNoticeVehicleChange()
        {
            if (!SDI.ChangedVehicles.HasVeryRecentlyBeenPlayed && Player.CurrentVehicle != null && Player.WantedLevel <= 4)// && !CurrentPlayer.CurrentVehicle.HasBeenDescribedByDispatch)
            {
                AddToQueue(SDI.ChangedVehicles, new CrimeSceneDescription(!Player.IsInVehicle, true, Player.PlacePoliceLastSeenPlayer) { VehicleSeen = Player.CurrentVehicle });
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: OnPoliceNoticeVehicleChange", 3);
        }
        public void OnRequestedBackUp()
        {
            if (!SDI.RequestBackup.HasRecentlyBeenPlayed && Player.WantedLevel <= 5)
            {
                AddToQueue(SDI.RequestBackup, new CrimeSceneDescription(!Player.IsInVehicle, true, Player.PlacePoliceLastSeenPlayer));
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: OnRequestedBackUp", 3);

            //MILITARY
        }
        public void OnRequestedBackUpSimple()
        {
            if (!SDI.RequestBackupSimple.HasRecentlyBeenPlayed && !DispatchQueue.Any(x=> x.Name == SDI.RequestBackupSimple.Name) && Player.WantedLevel <= 5)
            {
                AddToQueue(SDI.RequestBackupSimple, new CrimeSceneDescription(true, true, World.PoliceBackupPoint));
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: OnRequestedBackUpSimple", 3);
            //MILITARY
        }
        public void OnSuspectEluded()
        {
            //this is becuase when the search mode times out, it just sets the wanted to zero, which clears all the scanner dispatch queue stuff, so this doesnt get played, temp waiting 5 seconds so it will go in after this
            //long teerm need to change how the wanted level is set maybe with the chase result flag
            GameFiber TempWait = GameFiber.StartNew(delegate
            {
                try
                {
                    GameFiber.Sleep(1000);
                    if (!SDI.RemainInArea.HasRecentlyBeenPlayed)
                    {
                        AddToQueue(SDI.RemainInArea, new CrimeSceneDescription(!Player.IsInVehicle, true, Player.PlacePoliceLastSeenPlayer));
                    }
                    //EntryPoint.WriteToConsole($"SCANNER EVENT: OnSuspectEluded", 3);
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "PlayDispatchQueue");
        }
        public void OnSuspectWasted()
        {
            if (!SDI.SuspectWasted.HasRecentlyBeenPlayed && Player.AnyPoliceRecentlySeenPlayer && Player.WantedLevel > 1)// && Player.MaxWantedLastLife > 1)
            {
                AddToQueue(SDI.SuspectWasted);
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: OnSuspectWasted", 3);
        }
        public void OnSuspectShooting()
        {
            if (!SDI.ShotsFiredStatus.HasRecentlyBeenPlayed && !VeryRecentlyAnnouncedDispatch && Player.WantedLevel <= 4)
            {
                //EntryPoint.WriteToConsole($"SCANNER EVENT: ADDED Shooting", 3);
                AddToQueue(SDI.ShotsFiredStatus, new CrimeSceneDescription(!Player.IsInVehicle, true, Game.LocalPlayer.Character.Position));
            }
        }
        public void OnVehicleCrashed()
        {
            if (!SDI.VehicleCrashed.HasRecentlyBeenPlayed && Player.AnyPoliceCanSeePlayer && SDI.VehicleCrashed.HasntBeenPlayedForAWhile && Player.WantedLevel <= 4)
            {
                SDI.VehicleCrashed.LatestInformation.SeenByOfficers = true;
                AddToQueue(SDI.VehicleCrashed);
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: OnVehicleCrashed", 3);
        }
        public void OnVehicleStartedFire()
        {
            if (!SDI.VehicleStartedFire.HasRecentlyBeenPlayed && Player.AnyPoliceCanSeePlayer && SDI.VehicleStartedFire.HasntBeenPlayedForAWhile && Player.WantedLevel <= 4)
            {
                SDI.VehicleStartedFire.LatestInformation.SeenByOfficers = true;
                AddToQueue(SDI.VehicleStartedFire);
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: OnVehicleStartedFire", 3);
        }
        public void OnWantedActiveMode()
        {
            if (Player.WantedLevel <= 4)
            {
                if (!SDI.SuspectSpotted.HasVeryRecentlyBeenPlayed && !DispatchQueue.Any() && Player.PoliceResponse.HasBeenWantedFor > 25000)
                {
                    AddToQueue(SDI.SuspectSpotted, new CrimeSceneDescription(!Player.IsInVehicle, true, Game.LocalPlayer.Character.Position));
                }
            }
            else
            {
                if (!SDI.SuspectSpottedSimple.HasVeryRecentlyBeenPlayed && !DispatchQueue.Any() && Player.PoliceResponse.HasBeenWantedFor > 25000)
                {
                    AddToQueue(SDI.SuspectSpottedSimple, new CrimeSceneDescription(!Player.IsInVehicle, true, Game.LocalPlayer.Character.Position));
                }
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: OnStarsActive", 3);
            //MILITARY
        }
        public void OnWantedSearchMode()
        {
            if (Player.WantedLevel <= 4)
            {
                if (!SDI.SuspectEvaded.HasRecentlyBeenPlayed && !DispatchQueue.Any() && !World.Pedestrians.AnyCopsNearPosition(Player.Position, 100f) && Player.WantedLevel <= 4)
                {
                    AddToQueue(SDI.SuspectEvaded, new CrimeSceneDescription(!Player.IsInVehicle, true, Player.PlacePoliceLastSeenPlayer));
                }
            }
            else
            {
                if (!SDI.SuspectEvadedSimple.HasRecentlyBeenPlayed && !DispatchQueue.Any() && !World.Pedestrians.AnyCopsNearPosition(Player.Position, 100f))
                {
                    AddToQueue(SDI.SuspectEvadedSimple, new CrimeSceneDescription(!Player.IsInVehicle, true, Player.PlacePoliceLastSeenPlayer));
                }
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: OnStarsGreyedOut", 3);
            //MILITARY
        }
        public void OnWeaponsFree()
        {
            if (!ReportedWeaponsFree & !SDI.WeaponsFree.HasBeenPlayedThisWanted)
            {
                AddToQueue(SDI.WeaponsFree);
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: OnWeaponsFree", 3);
            //MILITARY
        }

        //Builder
        private void AddAttentionRandomUnit(DispatchEvent dispatchEvent)
        {
            dispatchEvent.SoundsToPlay.Add(RadioStart.PickRandom());
            dispatchEvent.SoundsToPlay.Add(new List<string>() { attention_unit_specific.Attentionunit.FileName, attention_unit_specific.Dispatchcallingunit.FileName, attention_unit_specific.Dispatchcallingunitumm.FileName,
                                                                    attention_specific.Attentioncaruhh.FileName, attention_specific.Attentioncaruhh1.FileName, attention_specific.Attentioncaruhhorof.FileName, attention_specific.Dispatchtocarumm.FileName, attention_specific.Dispatchtocarumm1.FileName, }.PickRandom());
            dispatchEvent.SoundsToPlay.Add(new List<string>() { car_code_composite.SevenEdwardSeven.FileName,
                                                                    car_code_composite._1Adam13.FileName,
                                                                    car_code_composite._1Adam5.FileName,
                                                                    car_code_composite._1David4.FileName,
                                                                    car_code_composite._2Edward5.FileName,
                                                                    car_code_composite._2Edward6.FileName,
                                                                    car_code_composite._2Lincoln8.FileName,
                                                                    car_code_composite._3Lincoln12.FileName,
                                                                    car_code_composite._3Lincoln2.FileName,
                                                                    car_code_composite._4Mary5.FileName,
                                                                    car_code_composite._6David6.FileName,
                                                                    car_code_composite._7Edward14.FileName,
                                                                    car_code_composite._8Lincoln5.FileName,
                                                                    car_code_composite._8Mary7.FileName,
                                                                    car_code_composite._9David1.FileName,
                                                                    car_code_composite._9Lincoln15.FileName}.PickRandom());
            dispatchEvent.SoundsToPlay.Add(RadioEnd.PickRandom());
        }
        private void AddAttentionUnits(DispatchEvent dispatchEvent)
        {
            if (RecentlyMentionedUnits)
            {
                return;
            }
            bool AddedZoneUnits = false;
            bool AddedSingleUnit = false;
            int totalAdded = 0;
            int totalToAdd = Settings.SettingsManager.ScannerSettings.NumberOfUnitsToAnnounce;
            List<string> CallSigns = new List<string>();
            foreach (Cop UnitToCall in World.Pedestrians.AllPoliceList.Where(x => x.IsRespondingToInvestigation || x.IsRespondingToWanted).OrderBy(x => x.DistanceToPlayer))
            {
                if (UnitToCall != null && UnitToCall.Division != -1)
                {
                    string CallSign = $"{UnitToCall.Division}-{UnitToCall.UnitType}-{UnitToCall.BeatNumber}";
                    if (!CallSigns.Contains(CallSign))
                    {
                        CallSigns.Add(CallSign);
                        //EntryPoint.WriteToConsoleTestLong($"Scanner Calling Specific Unit {CallSign}");
                        List<string> CallsignAudio = CallsignScannerAudio.GetAudio(UnitToCall.Division, UnitToCall.UnitType, UnitToCall.BeatNumber);
                        if (CallsignAudio != null)
                        {
                            if (!AddedSingleUnit)
                            {
                                dispatchEvent.SoundsToPlay.Add(new List<string>() { attention_unit_specific.Attentionunit.FileName, attention_unit_specific.Dispatchcallingunit.FileName, attention_unit_specific.Dispatchcallingunitumm.FileName,
                                                                   // attention_specific.Attentioncaruhh.FileName, attention_specific.Attentioncaruhh1.FileName, attention_specific.Attentioncaruhhorof.FileName, attention_specific.Dispatchtocarumm.FileName, attention_specific.Dispatchtocarumm1.FileName,
                                }.PickRandom());
                                AddedSingleUnit = true;
                                dispatchEvent.NotificationText += $"~n~~s~Responding:";
                            }
                            else
                            {
                                dispatchEvent.SoundsToPlay.Add(new List<string>() { officer.Unituhh.FileName, officer.Unitumm.FileName }.PickRandom());
                            }

                            dispatchEvent.NotificationText += $" ~p~{CallSign}~s~";

                            dispatchEvent.SoundsToPlay.AddRange(CallsignAudio);
                            totalAdded++;
                            AddedZoneUnits = true;
                        }
                    }
                }
                if (totalAdded >= totalToAdd)
                {
                    break;
                }
            }
            if (!AddedZoneUnits)
            {
                Zone MyZone = Player.CurrentLocation.CurrentZone;
                if (MyZone != null)
                {
                    ZoneLookup zoneAudio = ZoneScannerAudio.GetLookup(MyZone.InternalGameName);
                    if (zoneAudio != null)
                    {
                        string ScannerAudio = zoneAudio.ScannerUnitValues.PickRandom();
                        if (ScannerAudio != "" && ScannerAudio != null && ScannerAudio.Length > 2)
                        {
                            dispatchEvent.SoundsToPlay.Add(ScannerAudio);
                            AddedZoneUnits = true;
                        }
                    }
                }
            }
            //if(!AddedZoneUnits)
            //{
            //    dispatchEvent.SoundsToPlay.Add(new List<string>() { attention_unit_specific.Attentionunit.FileName, attention_unit_specific.Dispatchcallingunit.FileName, attention_unit_specific.Dispatchcallingunitumm.FileName,
            //                                                        attention_specific.Attentioncaruhh.FileName, attention_specific.Attentioncaruhh1.FileName, attention_specific.Attentioncaruhhorof.FileName, attention_specific.Dispatchtocarumm.FileName, attention_specific.Dispatchtocarumm1.FileName, }.PickRandom());

            //    dispatchEvent.SoundsToPlay.Add(new List<string>() { car_code_composite.SevenEdwardSeven.FileName,
            //                                                        car_code_composite._1Adam13.FileName,
            //                                                        car_code_composite._1Adam5.FileName,
            //                                                        car_code_composite._1David4.FileName,
            //                                                        car_code_composite._2Edward5.FileName,
            //                                                        car_code_composite._2Edward6.FileName,
            //                                                        car_code_composite._2Lincoln8.FileName,
            //                                                        car_code_composite._3Lincoln12.FileName,
            //                                                        car_code_composite._3Lincoln2.FileName,
            //                                                        car_code_composite._4Mary5.FileName,
            //                                                        car_code_composite._6David6.FileName,
            //                                                        car_code_composite._7Edward14.FileName,
            //                                                        car_code_composite._8Lincoln5.FileName,
            //                                                        car_code_composite._8Mary7.FileName,
            //                                                        car_code_composite._9David1.FileName,
            //                                                        car_code_composite._9Lincoln15.FileName}.PickRandom());
            //    AddedZoneUnits = true;
            //}
            if (AddedZoneUnits)
            {
                dispatchEvent.HasUnitAudio = true;
                dispatchEvent.UnitAudioAmount = totalAdded;
            }
        }
        private void AddAudioSet(DispatchEvent dispatchEvent, AudioSet audioSet)
        {
            if (audioSet != null)
            {
                dispatchEvent.SoundsToPlay.AddRange(audioSet.Sounds);
                dispatchEvent.Subtitles += " " + audioSet.Subtitles;
            }
        }
        private void AddHaveDescription(DispatchEvent dispatchEvent)
        {
            dispatchEvent.NotificationText += "~n~~r~Have Description~s~";
        }
        private void AddHeading(DispatchEvent dispatchEvent)
        {
            dispatchEvent.SoundsToPlay.Add(new List<string>() { suspect_heading.TargetLastSeenHeading.FileName, suspect_heading.TargetReportedHeading.FileName, suspect_heading.TargetSeenHeading.FileName, suspect_heading.TargetSpottedHeading.FileName }.PickRandom());
            dispatchEvent.Subtitles += " ~s~suspect heading~s~";
            string heading = NativeHelper.GetSimpleCompassHeading(Game.LocalPlayer.Character.Heading);
            if (heading == "N")
            {
                dispatchEvent.SoundsToPlay.Add(direction_heading.North.FileName);
                dispatchEvent.Subtitles += " ~g~North~s~";
            }
            else if (heading == "S")
            {
                dispatchEvent.SoundsToPlay.Add(direction_heading.South.FileName);
                dispatchEvent.Subtitles += " ~g~South~s~";
            }
            else if (heading == "E")
            {
                dispatchEvent.SoundsToPlay.Add(direction_heading.East.FileName);
                dispatchEvent.Subtitles += " ~g~East~s~";
            }
            else if (heading == "W")
            {
                dispatchEvent.SoundsToPlay.Add(direction_heading.West.FileName);
                dispatchEvent.Subtitles += " ~g~West~s~";
            }
        }
        private void AddLethalForce(DispatchEvent dispatchEvent)
        {
            if (!ReportedLethalForceAuthorized)
            {
                AddAudioSet(dispatchEvent, LethalForce.PickRandom());
                ReportedLethalForceAuthorized = true;
            }
        }
        private void AddLocationDescription(DispatchEvent dispatchEvent, LocationSpecificity locationSpecificity)
        {
            GameLocation NearbyLocation = World.Places.ActiveLocations.Where(x => !string.IsNullOrEmpty(x.ScannerFilePath) && x.DistanceToPlayer <= 100f).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();

            if (NearbyLocation != null && !RecentlyMentionedLocation)
            {
                AddLocation(dispatchEvent, NearbyLocation);
            }
            else
            {
                if (locationSpecificity == LocationSpecificity.HeadingAndStreet)
                {
                    AddHeading(dispatchEvent);
                }
                if (locationSpecificity == LocationSpecificity.Street || locationSpecificity == LocationSpecificity.HeadingAndStreet || locationSpecificity == LocationSpecificity.StreetAndZone)
                {
                    AddStreet(dispatchEvent);
                }
                if (locationSpecificity == LocationSpecificity.Zone || locationSpecificity == LocationSpecificity.StreetAndZone)
                {
                    AddZone(dispatchEvent);
                }
            }
        }
        private void AddRapSheet(DispatchEvent dispatchEvent)
        {
            dispatchEvent.NotificationText = "Wanted For:" + Player.PoliceResponse.PrintCrimes(true);
        }
        private void AddRequestAirSupport(DispatchEvent dispatchEvent)
        {
            if (!ReportedRequestAirSupport)
            {
                AddAudioSet(dispatchEvent, new List<AudioSet>()
                {
                    new AudioSet(new List<string>() { officer_requests_air_support.Officersrequestinghelicoptersupport.FileName },"officers requesting helicopter support"),
                    new AudioSet(new List<string>() { officer_requests_air_support.Code99unitsrequestimmediateairsupport.FileName },"code-99 units request immediate air support"),
                    new AudioSet(new List<string>() { officer_requests_air_support.Officersrequireaerialsupport.FileName },"officers require aerial support"),
                    new AudioSet(new List<string>() { officer_requests_air_support.Officersrequireaerialsupport1.FileName },"officers require aerial support"),
                    new AudioSet(new List<string>() { officer_requests_air_support.Officersrequireairsupport.FileName },"officers require air support"),
                    new AudioSet(new List<string>() { officer_requests_air_support.Unitsrequestaerialsupport.FileName },"units request aerial support"),
                    new AudioSet(new List<string>() { officer_requests_air_support.Unitsrequestingairsupport.FileName },"units requesting air support"),
                    new AudioSet(new List<string>() { officer_requests_air_support.Unitsrequestinghelicoptersupport.FileName },"units requesting helicopter support"),
                }.PickRandom());
                ReportedRequestAirSupport = true;
                dispatchEvent.NotificationText += "~n~~r~Air Support Requested~s~";
            }
        }
        private void AddSpeed(DispatchEvent dispatchEvent, float Speed)
        {
            Speed = Speed * 2.23694f;//convert to mph
            dispatchEvent.SoundsToPlay.Add(suspect_last_seen.TargetLastReported.FileName);
            dispatchEvent.Subtitles += " ~s~target last reported~s~";
            if (Speed >= 40f)
            {
                if (Speed >= 40f && Speed < 50f)
                {
                    dispatchEvent.SoundsToPlay.Add(doing_speed.Doing40mph.FileName);
                    dispatchEvent.Subtitles += " ~s~doing ~o~40 mph~s~";
                    dispatchEvent.NotificationText += "~n~Speed Exceeding: ~o~40 mph~s~";
                }
                else if (Speed >= 50f && Speed < 60f)
                {
                    dispatchEvent.SoundsToPlay.Add(doing_speed.Doing50mph.FileName);
                    dispatchEvent.Subtitles += " ~s~doing ~o~50 mph~s~";
                    dispatchEvent.NotificationText += "~n~Speed Exceeding: ~o~50 mph~s~";
                }
                else if (Speed >= 60f && Speed < 70f)
                {
                    dispatchEvent.SoundsToPlay.Add(doing_speed.Doing60mph.FileName);
                    dispatchEvent.Subtitles += " ~s~doing ~o~60 mph~s~";
                    dispatchEvent.NotificationText += "~n~Speed Exceeding: ~o~60 mph~s~";
                }
                else if (Speed >= 70f && Speed < 80f)
                {
                    dispatchEvent.SoundsToPlay.Add(doing_speed.Doing70mph.FileName);
                    dispatchEvent.Subtitles += " ~s~doing ~o~70 mph~s~";
                    dispatchEvent.NotificationText += "~n~Speed Exceeding: ~o~70 mph~s~";
                }
                else if (Speed >= 80f && Speed < 90f)
                {
                    dispatchEvent.SoundsToPlay.Add(doing_speed.Doing80mph.FileName);
                    dispatchEvent.Subtitles += " ~s~doing ~o~80 mph~s~";
                    dispatchEvent.NotificationText += "~n~Speed Exceeding: ~o~80 mph~s~";
                }
                else if (Speed >= 90f && Speed < 100f)
                {
                    dispatchEvent.SoundsToPlay.Add(doing_speed.Doing90mph.FileName);
                    dispatchEvent.Subtitles += " ~s~doing ~o~90 mph~s~";
                    dispatchEvent.NotificationText += "~n~Speed Exceeding: ~o~90 mph~s~";
                }
                else if (Speed >= 100f && Speed < 104f)
                {
                    dispatchEvent.SoundsToPlay.Add(doing_speed.Doing100mph.FileName);
                    dispatchEvent.Subtitles += " ~s~doing ~o~100 mph~s~";
                    dispatchEvent.NotificationText += "~n~Speed Exceeding: ~o~100 mph~s~";
                }
                else if (Speed >= 105f)
                {
                    dispatchEvent.SoundsToPlay.Add(doing_speed.Doingover100mph.FileName);
                    dispatchEvent.Subtitles += " ~s~doing ~o~over 100 mph~s~";
                    dispatchEvent.NotificationText += "~n~Speed Exceeding: ~o~105 mph~s~";
                }
            }
            else
            {
                dispatchEvent.SoundsToPlay.Add(crime_speeding.Speeding.FileName);
                dispatchEvent.Subtitles += " ~s~speeding~s~";
                dispatchEvent.NotificationText += "~n~Speeding~s~";
            }
        }
        private void AddStreet(DispatchEvent dispatchEvent)
        {
            if (RecentlyMentionedStreet)
            {
                return;
            }
            Street MyStreet = Player.CurrentLocation.CurrentStreet;
            if (MyStreet != null)
            {
                string StreetAudio = StreetScannerAudio.GetAudio(MyStreet.Name);
                if (StreetAudio != "")
                {
                    dispatchEvent.SoundsToPlay.Add(new List<string>() { conjunctives.On.FileName, conjunctives.On1.FileName, conjunctives.On2.FileName, conjunctives.On3.FileName, conjunctives.On4.FileName }.PickRandom());
                    dispatchEvent.SoundsToPlay.Add(StreetAudio);
                    dispatchEvent.Subtitles += " ~s~on ~HUD_COLOUR_YELLOWLIGHT~" + MyStreet.ProperStreetName + "~s~";
                    dispatchEvent.NotificationText += "~n~~HUD_COLOUR_YELLOWLIGHT~" + MyStreet.ProperStreetName + "~s~";
                    dispatchEvent.HasStreetAudio = true;

                    if (Player.CurrentLocation.CurrentCrossStreet != null)
                    {
                        Street MyCrossStreet = Player.CurrentLocation.CurrentCrossStreet;
                        if (MyCrossStreet != null)
                        {
                            string CrossStreetAudio = StreetScannerAudio.GetAudio(MyCrossStreet.Name);
                            if (CrossStreetAudio != "")
                            {
                                dispatchEvent.SoundsToPlay.Add(new List<string>() { conjunctives.AT01.FileName, conjunctives.AT02.FileName }.PickRandom());
                                dispatchEvent.SoundsToPlay.Add(CrossStreetAudio);
                                dispatchEvent.NotificationText += " ~s~at ~HUD_COLOUR_YELLOWLIGHT~" + MyCrossStreet.ProperStreetName + "~s~";
                                dispatchEvent.Subtitles += " ~s~at ~HUD_COLOUR_YELLOWLIGHT~" + MyCrossStreet.ProperStreetName + "~s~";
                            }
                        }
                    }
                }
            }
        }
        private void AddToQueue(Dispatch ToAdd, CrimeSceneDescription ToCallIn)
        {
            if (Settings.SettingsManager.ScannerSettings.IsEnabled && Player.ActivityManager.CanHearScanner)
            {
                GameFiber.Yield();//TR Added 7
                Dispatch Existing = DispatchQueue.FirstOrDefault(x => x.Name == ToAdd.Name);
                if (Existing != null)
                {
                    Existing.LatestInformation = ToCallIn;
                }
                else
                {
                    ToAdd.LatestInformation = ToCallIn;
                    DispatchQueue.Add(ToAdd);
                }
            }
        }
        private void AddToQueue(Dispatch ToAdd)
        {
            if (Settings.SettingsManager.ScannerSettings.IsEnabled && Player.ActivityManager.CanHearScanner)
            {
                GameFiber.Yield();//TR Added 7
                Dispatch Existing = DispatchQueue.FirstOrDefault(x => x.Name == ToAdd.Name);
                if (Existing == null)
                {
                    DispatchQueue.Add(ToAdd);
                }
            }
        }
        private void AddToAmbientQueue(Dispatch ToAdd, CrimeSceneDescription ToCallIn)
        {
            if (Settings.SettingsManager.ScannerSettings.IsEnabled && Player.ActivityManager.CanHearScanner)
            {
                GameFiber.Yield();//TR Added 7
                Dispatch Existing = AmbientDispatchQueue.FirstOrDefault(x => x.Name == ToAdd.Name);
                if (Existing == null)
                {
                    AmbientDispatchQueue.Add(ToAdd);
                }
                else
                {
                    ToAdd.LatestInformation = ToCallIn;
                    DispatchQueue.Add(ToAdd);
                }
            }
        }
        private void AddVehicleDescription(DispatchEvent dispatchEvent, VehicleExt VehicleToDescribe, bool IncludeLicensePlate, Dispatch DispatchToPlay)
        {
            if (VehicleToDescribe == null)
                return;
            if (VehicleToDescribe.HasBeenDescribedByDispatch)
                return;
            //else
            //    VehicleToDescribe.HasBeenDescribedByDispatch = true;

            if (VehicleToDescribe != null && VehicleToDescribe.Vehicle.Exists())
            {
                dispatchEvent.NotificationText += "~n~Vehicle:~s~";
                //dispatchEvent.SoundsToPlay.Add(suspect_is.SuspectIs.FileName);
                //dispatchEvent.SoundsToPlay.Add(conjunctives.Drivinga.FileName);
                //dispatchEvent.Subtitles += " suspect is driving a ~s~";

                if (VehicleToDescribe.IsPolice)
                {
                    dispatchEvent.SoundsToPlay.Add(suspect_is.SuspectIs.FileName);
                    if (VehicleToDescribe.Vehicle.IsBike)
                    {
                        dispatchEvent.SoundsToPlay.Add(conjunctives.Onuh.FileName);
                    }
                    else
                    {
                        dispatchEvent.SoundsToPlay.Add(conjunctives.DrivingAUmmm.FileName);
                    }

                    if (RandomItems.RandomPercent(50))
                    {
                        dispatchEvent.SoundsToPlay.Add(crime_stolen_cop_car.Astolenpolicevehicle1.FileName);
                    }
                    else
                    {
                        dispatchEvent.SoundsToPlay.Add(crime_stolen_cop_car.Astolenpolicevehicle.FileName);
                    }
                    dispatchEvent.NotificationText += " ~s~Stolen Police~s~";
                    dispatchEvent.Subtitles += " suspect is driving a stolen police vehicle ~s~";
                }
                else
                {
                    Color CarColor = VehicleToDescribe.VehicleColor(); //Vehicles.VehicleManager.VehicleColor(VehicleToDescribe);

                    int primaryColor = -1;
                    int secondaryColor = -1;
                    unsafe
                    {
                        NativeFunction.CallByName<bool>("GET_VEHICLE_COLOURS", VehicleToDescribe.Vehicle, &primaryColor, &secondaryColor);
                    }


                    string MakeName = VehicleToDescribe.MakeName();// Vehicles.VehicleManager.MakeName(VehicleToDescribe);
                    int ClassInt = VehicleToDescribe.ClassInt();// Vehicles.VehicleManager.ClassInt(VehicleToDescribe);
                    string ClassName = VehicleScannerAudio.ClassName(ClassInt);
                    string ModelName = VehicleToDescribe.ModelName();// Vehicles.VehicleManager.ModelName(VehicleToDescribe);

                    string ColorAudio = "";
                    
                    if(primaryColor == -1)
                    {
                        VehicleScannerAudio.GetColorAudio(CarColor);
                    }
                    else
                    {
                        VehicleScannerAudio.GetColorAudioByID(primaryColor);
                    }
                    
                    
                    

                    string MakeAudio = VehicleScannerAudio.GetMakeAudio(MakeName);
                    string ClassAudio = VehicleScannerAudio.GetClassAudio(ClassInt);
                    string ModelAudio = VehicleScannerAudio.GetModelAudio(VehicleToDescribe.Vehicle.Model.Hash);

                    //if(VehicleToDescribe.IsStolen)
                    //{
                    //    dispatchEvent.SoundsToPlay.Add(suspect_is.SuspectIs.FileName);
                    //    dispatchEvent.SoundsToPlay.Add(conjunctives.In.FileName);
                    //    dispatchEvent.Subtitles += " suspect is in a stolen vehicle, a ~s~";
                    //    dispatchEvent.SoundsToPlay.Add(crime_10_851.Astolenvehicle.FileName);
                    //    dispatchEvent.SoundsToPlay.Add(conjunctives.A01.FileName);
                    //}
                    //else
                    //{
                    dispatchEvent.SoundsToPlay.Add(suspect_is.SuspectIs.FileName);
                    dispatchEvent.SoundsToPlay.Add(conjunctives.Drivinga.FileName);
                    dispatchEvent.Subtitles += " suspect is driving a ~s~";

                    //}

                    if (ColorAudio != "")
                    {
                        dispatchEvent.SoundsToPlay.Add(ColorAudio);
                        dispatchEvent.Subtitles += " ~s~" + CarColor.Name + "~s~";
                        dispatchEvent.NotificationText += " ~s~" + CarColor.Name + "~s~";
                    }
                    if (MakeAudio != "")
                    {
                        dispatchEvent.SoundsToPlay.Add(MakeAudio);
                        dispatchEvent.Subtitles += " ~s~" + MakeName + "~s~";
                        dispatchEvent.NotificationText += " ~s~" + MakeName + "~s~";
                    }

                    if (ModelAudio != "")
                    {
                        dispatchEvent.SoundsToPlay.Add(ModelAudio);
                        dispatchEvent.Subtitles += " ~s~" + ModelName + "~s~";
                        dispatchEvent.NotificationText += " ~s~" + ModelName + "~s~";
                    }
                    else if (ClassAudio != "")
                    {
                        dispatchEvent.SoundsToPlay.Add(ClassAudio);
                        dispatchEvent.Subtitles += " ~s~" + ClassName + "~s~";
                        dispatchEvent.NotificationText += " ~s~" + ClassName + "~s~";
                    }
                }
                if (IncludeLicensePlate)
                {
                    AddAudioSet(dispatchEvent, LicensePlateSet.PickRandom());
                    string LicensePlateText = VehicleToDescribe.OriginalLicensePlate.PlateNumber;
                    dispatchEvent.SoundsToPlay.AddRange(VehicleScannerAudio.GetPlateAudio(LicensePlateText));
                    dispatchEvent.Subtitles += " ~s~" + LicensePlateText + "~s~";
                    dispatchEvent.NotificationText += " ~s~Plate: " + LicensePlateText + "~s~";
                }
                if (DispatchToPlay.Name == "Suspicious Vehicle")
                {
                    dispatchEvent.NotificationText += "~n~~s~For: " + VehicleToDescribe.IsSuspicious(Time.IsNight) + "~s~";
                }
                //EntryPoint.WriteToConsole(string.Format("ScannerScript Color {0}, Make {1}, Class {2}, Model {3}, RawModel {4}", CarColor.Name, MakeName, ClassName, ModelName, VehicleToDescribe.Vehicle.Model.Name));
            }
        }
        private void AddWeaponDescription(DispatchEvent dispatchEvent, WeaponInformation WeaponToDescribe)
        {
            dispatchEvent.NotificationText += "~n~Weapon:~s~";
            dispatchEvent.SoundsToPlay.Add(suspect_is.SuspectIs.FileName);
            if (WeaponToDescribe == null)
            {
                dispatchEvent.SoundsToPlay.Add(carrying_weapon.Carryingaweapon.FileName);
                dispatchEvent.Subtitles += " suspect is carrying a ~r~weapon~s~";
                dispatchEvent.NotificationText += " Unknown";
            }
            else if (WeaponToDescribe.ModelName == "weapon_rpg")
            {
                dispatchEvent.SoundsToPlay.Add(carrying_weapon.ArmedwithanRPG.FileName);
                dispatchEvent.Subtitles += " suspect is armed with an ~r~RPG~s~";
                dispatchEvent.NotificationText += " RPG";
            }
            else if (WeaponToDescribe.ModelName == "weapon_bat")
            {
                dispatchEvent.SoundsToPlay.Add(carrying_weapon.Armedwithabat.FileName);
                dispatchEvent.Subtitles += " suspect is armed with a ~r~bat~s~";
                dispatchEvent.NotificationText += " Bat";
            }
            else if (WeaponToDescribe.ModelName == "weapon_grenadelauncher" || WeaponToDescribe.ModelName == "weapon_grenadelauncher_smoke" || WeaponToDescribe.ModelName == "weapon_compactlauncher")
            {
                dispatchEvent.SoundsToPlay.Add(carrying_weapon.Armedwithagrenadelauncher.FileName);
                dispatchEvent.Subtitles += " suspect is armed with a ~r~grenade launcher~s~";
                dispatchEvent.NotificationText += " Grenade Launcher";
            }
            else if (WeaponToDescribe.Category == WeaponCategory.Throwable || WeaponToDescribe.ModelName == "weapon_grenadelauncher_smoke" || WeaponToDescribe.ModelName == "weapon_compactlauncher")
            {
                dispatchEvent.SoundsToPlay.Add(carrying_weapon.Armedwithexplosives.FileName);
                dispatchEvent.Subtitles += " suspect is armed with ~r~explosives~s~";
                dispatchEvent.NotificationText += " Explosives";
            }
            else if (WeaponToDescribe.ModelName == "weapon_dagger" || WeaponToDescribe.ModelName == "weapon_knife" || WeaponToDescribe.ModelName == "weapon_switchblade")
            {
                dispatchEvent.SoundsToPlay.Add(carrying_weapon.Armedwithaknife.FileName);
                dispatchEvent.Subtitles += " suspect is armed with a ~r~knife~s~";
                dispatchEvent.NotificationText += " Knife";
            }
            else if (WeaponToDescribe.ModelName == "weapon_minigun")
            {
                dispatchEvent.SoundsToPlay.Add(carrying_weapon.Armedwithaminigun.FileName);
                dispatchEvent.Subtitles += " suspect is armed with a ~r~minigun~s~";
                dispatchEvent.NotificationText += " Minigun";
            }
            else if (WeaponToDescribe.ModelName == "weapon_sawnoffshotgun")
            {
                dispatchEvent.SoundsToPlay.Add(carrying_weapon.Armedwithasawedoffshotgun.FileName);
                dispatchEvent.Subtitles += " suspect is armed with a ~r~sawed off shotgun~s~";
                dispatchEvent.NotificationText += " Sawed Off Shotgun";
            }
            else if (WeaponToDescribe.Category == WeaponCategory.LMG)
            {
                dispatchEvent.SoundsToPlay.Add(carrying_weapon.Armedwithamachinegun.FileName);
                dispatchEvent.Subtitles += " suspect is armed with a ~r~machine gun~s~";
                dispatchEvent.NotificationText += " Machine Gun";
            }
            else if (WeaponToDescribe.Category == WeaponCategory.Pistol)
            {
                dispatchEvent.SoundsToPlay.Add(carrying_weapon.Armedwithafirearm.FileName);
                dispatchEvent.Subtitles += " suspect is armed with a ~r~pistol~s~";
                dispatchEvent.NotificationText += " Pistol";
            }
            else if (WeaponToDescribe.Category == WeaponCategory.Shotgun)
            {
                dispatchEvent.SoundsToPlay.Add(carrying_weapon.Armedwithashotgun.FileName);
                dispatchEvent.Subtitles += " suspect is armed with a ~r~shotgun~s~";
                dispatchEvent.NotificationText += " Shotgun";
            }
            else if (WeaponToDescribe.Category == WeaponCategory.SMG)
            {
                dispatchEvent.SoundsToPlay.Add(carrying_weapon.Armedwithasubmachinegun.FileName);
                dispatchEvent.Subtitles += " suspect is armed with a ~r~submachine gun~s~";
                dispatchEvent.NotificationText += " Submachine Gun";
            }
            else if (WeaponToDescribe.Category == WeaponCategory.AR)
            {
                dispatchEvent.SoundsToPlay.Add(carrying_weapon.Carryinganassaultrifle.FileName);
                dispatchEvent.Subtitles += " suspect is carrying an ~r~assault rifle~s~";
                dispatchEvent.NotificationText += " Assault Rifle";
            }
            else if (WeaponToDescribe.Category == WeaponCategory.Sniper)
            {
                dispatchEvent.SoundsToPlay.Add(carrying_weapon.Armedwithasniperrifle.FileName);
                dispatchEvent.Subtitles += " suspect is armed with a ~r~sniper rifle~s~";
                dispatchEvent.NotificationText += " Sniper Rifle";
            }
            else if (WeaponToDescribe.Category == WeaponCategory.Heavy)
            {
                dispatchEvent.SoundsToPlay.Add(status_message.HeavilyArmed.FileName);
                dispatchEvent.Subtitles += " suspect is ~r~heaviy armed~s~";
                dispatchEvent.NotificationText += " Heavy Weapon";
            }
            else if (WeaponToDescribe.Category == WeaponCategory.Melee)
            {
                dispatchEvent.SoundsToPlay.Add(carrying_weapon.Carryingaweapon.FileName);
                dispatchEvent.Subtitles += " suspect is carrying a ~r~weapon~s~";
                dispatchEvent.NotificationText += " Close Combat Weapon";
            }
            else
            {
                int Num = RandomItems.MyRand.Next(1, 5);
                if (Num == 1)
                {
                    dispatchEvent.SoundsToPlay.Add(carrying_weapon.Armedwithafirearm.FileName);
                    dispatchEvent.Subtitles += " suspect is armed with a ~r~firearm~s~";
                }
                else if (Num == 2)
                {
                    dispatchEvent.SoundsToPlay.Add(carrying_weapon.Armedwithagat.FileName);
                    dispatchEvent.Subtitles += " suspect is armed with a ~r~gat~s~";
                }
                else if (Num == 3)
                {
                    dispatchEvent.SoundsToPlay.Add(carrying_weapon.Carryingafirearm.FileName);
                    dispatchEvent.Subtitles += " suspect is carrying a ~r~firearm~s~";
                }
                else
                {
                    dispatchEvent.SoundsToPlay.Add(carrying_weapon.Carryingagat.FileName);
                    dispatchEvent.Subtitles += " suspect is carrying a ~r~gat~s~";
                }
                dispatchEvent.NotificationText += " Gat";
            }
        }
        private void AddWeaponsFree(DispatchEvent dispatchEvent)
        {
            if (!ReportedWeaponsFree)
            {
                AddAudioSet(dispatchEvent, new AudioSet(new List<string>() { custom_wanted_level_line.Suspectisarmedanddangerousweaponsfree.FileName }, "suspect is armed and dangerous, weapons free"));
                dispatchEvent.NotificationText += "~n~~r~Weapons Free~s~";
                ReportedWeaponsFree = true;
            }
        }
        private void AddLocation(DispatchEvent dispatchEvent, GameLocation location)
        {
            if (RecentlyMentionedLocation)
            {
                return;
            }
            if (location != null)
            {
                string ScannerAudio = location.ScannerFilePath;
                if (ScannerAudio != "")
                {
                    //dispatchEvent.HasZoneAudio = true;
                    //if (MyZone.IsSpecificLocation || Settings.SettingsManager.ScannerSettings.UseNearForLocations)
                    //{

                    dispatchEvent.SoundsToPlay.Add(new List<string> { suspect_last_seen.TargetLastSeen.FileName, suspect_last_seen.TargetLastReported.FileName, suspect_last_seen.SuspectSpotted.FileName, suspect_last_seen.TargetIs.FileName, suspect_last_seen.TargetSpotted.FileName }.PickRandom());
                    dispatchEvent.Subtitles += " ~s~suspect seen~s~";

                    dispatchEvent.SoundsToPlay.Add(new List<string> { conjunctives.Nearumm.FileName, conjunctives.Closetoum.FileName, conjunctives.Closetouhh.FileName }.PickRandom());
                        dispatchEvent.Subtitles += " ~s~near ~p~" + location.Name + "~s~";
                    //}
                    //else
                    //{
                    //    dispatchEvent.SoundsToPlay.Add(new List<string> { conjunctives.In.FileName }.PickRandom());
                    //    dispatchEvent.Subtitles += " ~s~in ~p~" + location.Name + "~s~";
                    //}
                    dispatchEvent.SoundsToPlay.Add(ScannerAudio);
                    dispatchEvent.NotificationText += "~n~~p~" + location.Name + "~s~";
                    dispatchEvent.HasLocationAudio = true;
                    location.GameTimeLastMentioned = Game.GameTime;
                }
            }
        }
        private void AddZone(DispatchEvent dispatchEvent)
        {
            if (RecentlyMentionedZone)
            {
                return;
            }
            Zone MyZone = Player.CurrentLocation.CurrentZone;
            if (MyZone != null)
            {
                string ScannerAudio = ZoneScannerAudio.GetAudio(MyZone.InternalGameName);
                if (ScannerAudio != "")
                {
                    dispatchEvent.HasZoneAudio = true;
                    if (MyZone.IsSpecificLocation || Settings.SettingsManager.ScannerSettings.UseNearForLocations)
                    {
                        dispatchEvent.SoundsToPlay.Add(new List<string> { conjunctives.Nearumm.FileName, conjunctives.Closetoum.FileName, conjunctives.Closetouhh.FileName }.PickRandom());
                        dispatchEvent.Subtitles += " ~s~near ~p~" + MyZone.DisplayName + "~s~";
                    }
                    else
                    {
                        dispatchEvent.SoundsToPlay.Add(new List<string> { conjunctives.In.FileName }.PickRandom());
                        dispatchEvent.Subtitles += " ~s~in ~p~" + MyZone.DisplayName + "~s~";
                    }
                    dispatchEvent.SoundsToPlay.Add(ScannerAudio);
                    dispatchEvent.NotificationText += "~n~~p~" + MyZone.DisplayName + "~s~";
                }
            }
        }
        private void BuildDispatch(Dispatch DispatchToPlay, bool addtoPlayed, bool isAmbient)
        {
            //EntryPoint.WriteToConsole($"SCANNER EVENT: Building {DispatchToPlay.Name}, MarkVehicleAsStolen: {DispatchToPlay.MarkVehicleAsStolen} Vehicle: {DispatchToPlay.LatestInformation?.VehicleSeen?.Vehicle.Handle} Instances: {DispatchToPlay.LatestInformation?.InstancesObserved}", 3);
            DispatchEvent EventToPlay = new DispatchEvent();
            if (DispatchToPlay.HasPreamble)
            {
                EventToPlay.SoundsToPlay.Add(RadioStart.PickRandom());

                if (Player.AnyPoliceInHeliCanSeePlayer && DispatchToPlay.LatestInformation.SeenByOfficers && DispatchToPlay.PreambleHelicopterAudioSet != null && DispatchToPlay.PreambleHelicopterAudioSet.Any())
                {
                    AddAudioSet(EventToPlay, DispatchToPlay.PreambleHelicopterAudioSet.PickRandom());
                }
                else
                {
                    AddAudioSet(EventToPlay, DispatchToPlay.PreambleAudioSet.PickRandom());
                }
                EventToPlay.SoundsToPlay.Add(RadioEnd.PickRandom());
            }






            EventToPlay.SoundsToPlay.Add(RadioStart.PickRandom());
            EventToPlay.NotificationTitle = DispatchToPlay.NotificationTitle;

            if (DispatchToPlay.NotificationSubtitle != "")
            {
                EventToPlay.NotificationSubtitle = DispatchToPlay.NotificationSubtitle + "~s~";
            }
            else if (DispatchToPlay.IsPoliceStatus)
            {
                EventToPlay.NotificationSubtitle = "~g~Status";
            }
            else if (DispatchToPlay.LatestInformation.SeenByOfficers)
            {
                EventToPlay.NotificationSubtitle = "~r~Crime Observed";
            }
            else
            {
                EventToPlay.NotificationSubtitle = "~o~Crime Reported";
            }
            EventToPlay.NotificationText = DispatchToPlay.NotificationText;


            if (DispatchToPlay.IncludeAttentionAllUnits)
            {
                AddAudioSet(EventToPlay, AttentionAllUnits.PickRandom());
            }
            else if (!DispatchToPlay.LatestInformation.SeenByOfficers && !DispatchToPlay.IsPoliceStatus)
            {
                AddAttentionUnits(EventToPlay);
            }

            if (DispatchToPlay.IncludeReportedBy)
            {
                if (DispatchToPlay.LatestInformation.SeenByOfficers)
                {
                    AddAudioSet(EventToPlay, OfficersReport.PickRandom());
                }
                else
                {
                    AddAudioSet(EventToPlay, CiviliansReport.PickRandom());
                }
            }
            if (DispatchToPlay.LatestInformation.InstancesObserved > 1 && DispatchToPlay.MainMultiAudioSet.Any())
            {
                AddAudioSet(EventToPlay, DispatchToPlay.MainMultiAudioSet.PickRandom());
            }
            else
            {
                AddAudioSet(EventToPlay, DispatchToPlay.MainAudioSet.PickRandom());
            }

            if (DispatchToPlay.SecondaryAudioSet.Any())
            {
                AddAudioSet(EventToPlay, DispatchToPlay.SecondaryAudioSet.PickRandom());
            }
            if (DispatchToPlay.IncludeDrivingVehicle)
            {
                AddVehicleDescription(EventToPlay, DispatchToPlay.LatestInformation.VehicleSeen, !DispatchToPlay.LatestInformation.SeenByOfficers && DispatchToPlay.IncludeLicensePlate, DispatchToPlay);
                GameFiber.Yield();
            }
            if (DispatchToPlay.IncludeRapSheet)
            {
                AddRapSheet(EventToPlay);
            }
            if (DispatchToPlay.MarkVehicleAsStolen && DispatchToPlay.LatestInformation != null && DispatchToPlay.LatestInformation.VehicleSeen != null && Player.CurrentVehicle != null)//temp current vehicle BS
            {
                //THIS NEED TO NOT BE CURRENT VEHICLE, BUT OTHERWISE THE LINK GETS MESSED UP?

                Player.CurrentVehicle.SetReportedStolen();

                //Player.CurrentVehicle.WasReportedStolen = true;
                //Player.CurrentVehicle.OriginalLicensePlate.IsWanted = true;
                //if (Player.CurrentVehicle.OriginalLicensePlate.PlateNumber == Player.CurrentVehicle.CarPlate.PlateNumber)
                //{
                //    Player.CurrentVehicle.CarPlate.IsWanted = true;
                //}
            }
            if (DispatchToPlay.IncludeCarryingWeapon && (DispatchToPlay.LatestInformation.WeaponSeen != null || DispatchToPlay.Name == "Carrying Weapon"))
            {
                AddWeaponDescription(EventToPlay, DispatchToPlay.LatestInformation.WeaponSeen);
                GameFiber.Yield();
            }
            if (DispatchToPlay.ResultsInLethalForce && !SDI.LethalForceAuthorized.HasBeenPlayedThisWanted && DispatchToPlay.Name != SDI.LethalForceAuthorized.Name)
            {
                AddLethalForce(EventToPlay);
            }
            if (DispatchToPlay.CanAddExtras && Player.IsWanted && !Player.IsDead && Player.PoliceResponse.IsWeaponsFree && !SDI.WeaponsFree.HasBeenPlayedThisWanted && DispatchToPlay.Name != SDI.WeaponsFree.Name)
            {
                AddWeaponsFree(EventToPlay);
            }
            if (DispatchToPlay.CanAddExtras && Player.IsWanted && !Player.IsDead && World.Pedestrians.AnyHelicopterUnitsSpawned && !SDI.RequestAirSupport.HasBeenPlayedThisWanted && DispatchToPlay.Name != SDI.RequestAirSupport.Name)
            {
                AddRequestAirSupport(EventToPlay);
            }
            if (DispatchToPlay.CanAddExtras && DispatchToPlay.IncludeDrivingSpeed && Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
            {
                if (DispatchToPlay.LatestInformation.Speed <= Player.Character.Speed)
                {
                    AddSpeed(EventToPlay, Player.Character.Speed);
                }
                else
                {
                    AddSpeed(EventToPlay, DispatchToPlay.LatestInformation.Speed);
                }
                //AddSpeed(EventToPlay, DispatchToPlay.LatestInformation.Speed);// CurrentPlayer.CurrentVehicle.Vehicle.Speed);
                GameFiber.Yield();
            }
            if (DispatchToPlay.LocationDescription != LocationSpecificity.Nothing)
            {
                AddLocationDescription(EventToPlay, DispatchToPlay.LocationDescription);
                GameFiber.Yield();
            }
            if (DispatchToPlay.CanAddExtras && Player.PoliceResponse.PoliceHaveDescription && !DispatchToPlay.LatestInformation.SeenByOfficers && !DispatchToPlay.IsPoliceStatus)
            {
                AddHaveDescription(EventToPlay);
            }
            if (EventToPlay.SoundsToPlay.Count() == 1)//only has radio beep
            {
                return;
            }

            if (EventToPlay.HasUnitAudio)
            {
                if (Player.Investigation.InvestigationWantedLevel == 1)
                {
                    EventToPlay.NotificationText += "~n~~o~Responding Code-2~s~";
                    AddAudioSet(EventToPlay, RespondCode2Set.PickRandom());
                }
                else if (Player.Investigation.InvestigationWantedLevel > 1)
                {
                    EventToPlay.NotificationText += "~n~~r~Responding Code-3~s~";
                    AddAudioSet(EventToPlay, RespondCode3Set.PickRandom());
                }
            }

            EventToPlay.SoundsToPlay.Add(RadioEnd.PickRandom());

            if (EventToPlay.HasUnitAudio)
            {
                foreach (AudioSet audioSet in UnitEnRouteSet.OrderBy(x => Guid.NewGuid()).Take(EventToPlay.UnitAudioAmount).ToList())
                {
                    EventToPlay.SoundsToPlay.Add(RadioStart.PickRandom());
                    AddAudioSet(EventToPlay, audioSet);
                    EventToPlay.SoundsToPlay.Add(RadioEnd.PickRandom());
                }
            }



            if (DispatchToPlay.HasEpilogue)
            {
                EventToPlay.SoundsToPlay.Add(RadioStart.PickRandom());

                if (Player.AnyPoliceInHeliCanSeePlayer && DispatchToPlay.LatestInformation.SeenByOfficers && DispatchToPlay.EpilogueHelicopterAudioSet != null && DispatchToPlay.EpilogueHelicopterAudioSet.Any())
                {
                    AddAudioSet(EventToPlay, DispatchToPlay.EpilogueHelicopterAudioSet.PickRandom());
                }
                else
                {
                    AddAudioSet(EventToPlay, DispatchToPlay.EpilogueAudioSet.PickRandom());
                }
                EventToPlay.SoundsToPlay.Add(RadioEnd.PickRandom());
            }





            if (EventToPlay.Subtitles != "")
            {
                EventToPlay.Subtitles = NativeHelper.FirstCharToUpper(EventToPlay.Subtitles);
            }
            EventToPlay.Priority = DispatchToPlay.Priority;

            if (addtoPlayed)
            {
                DispatchToPlay.SetPlayed();
                if (DispatchToPlay.LatestInformation.SeenByOfficers && DispatchToPlay.Priority < HighestOfficerReportedPriority)
                {
                    HighestOfficerReportedPriority = DispatchToPlay.Priority;
                }
                else if (!DispatchToPlay.LatestInformation.SeenByOfficers && !DispatchToPlay.IsPoliceStatus && DispatchToPlay.Priority < HighestCivilianReportedPriority)
                {
                    HighestCivilianReportedPriority = DispatchToPlay.Priority;
                }
            }
            GameFiber.Yield();

            if (DispatchToPlay.CanAlwaysBeInterrupted)
            {
                EventToPlay.CanBeInterrupted = true;
            }
            if (DispatchToPlay.CanAlwaysInterrupt)
            {
                EventToPlay.CanInterrupt = true;
            }
            if(DispatchToPlay.AnyDispatchInterrupts)
            {
                EventToPlay.AnyDispatchInterrupts = true;
            }
            if(isAmbient)
            {
                PlayAmbientDispatch(EventToPlay);
            }
            else
            {
                PlayDispatch(EventToPlay, DispatchToPlay.LatestInformation, DispatchToPlay);
            }
            
        }
        private void DefaultConfig()
        {
            SetupDispatches();
            DispatchLookup = new List<CrimeDispatch>
        {
            new CrimeDispatch("AttemptingSuicide",SDI.AttemptingSuicide),
            new CrimeDispatch("BrandishingWeapon",SDI.CarryingWeapon),
            new CrimeDispatch("ChangingPlates",SDI.TamperingWithVehicle),
            new CrimeDispatch("DrivingAgainstTraffic",SDI.RecklessDriving),
            new CrimeDispatch("DrivingOnPavement",SDI.RecklessDriving),
            new CrimeDispatch("FelonySpeeding",SDI.FelonySpeeding),
            new CrimeDispatch(StaticStrings.FiringWeaponCrimeID,SDI.ShotsFired),
            new CrimeDispatch(StaticStrings.FiringSilencedWeaponCrimeID,SDI.ShotsFired),
            new CrimeDispatch("FiringWeaponNearPolice",SDI.ShotsFiredAtAnOfficer),
            new CrimeDispatch("GotInAirVehicleDuringChase",SDI.StealingAirVehicle),
            new CrimeDispatch("GrandTheftAuto",SDI.GrandTheftAuto),
            new CrimeDispatch("HitCarWithCar",SDI.VehicleHitAndRun),
            new CrimeDispatch("HitPedWithCar",SDI.PedHitAndRun),
            new CrimeDispatch("RunningARedLight",SDI.RunningARedLight),
            new CrimeDispatch("HurtingCivilians",SDI.CivilianInjury),
            new CrimeDispatch("HurtingPolice",SDI.AssaultingOfficer),
            new CrimeDispatch("KillingCivilians",SDI.CivilianDown),
            new CrimeDispatch("KillingPolice",SDI.OfficerDown),
            new CrimeDispatch("Mugging",SDI.Mugging),
            new CrimeDispatch("NonRoadworthyVehicle",SDI.SuspiciousVehicle),
            new CrimeDispatch("ResistingArrest",SDI.ResistingArrest),
            new CrimeDispatch(StaticStrings.TrespessingOnGovtPropertyCrimeID,SDI.TrespassingOnGovernmentProperty),


            new CrimeDispatch(StaticStrings.TrespassingOnMilitaryBaseCrimeID,SDI.TrespassingOnMilitaryBase),

            new CrimeDispatch(StaticStrings.TrespessingCrimeID,SDI.Trespassing),
            new CrimeDispatch(StaticStrings.CivilianTrespessingCrimeID,SDI.Trespassing),
            new CrimeDispatch(StaticStrings.VehicleInvasionCrimeID,SDI.SuspiciousActivity),

            new CrimeDispatch(StaticStrings.SuspiciousVehicleCrimeID,SDI.SuspiciousActivity),

            new CrimeDispatch("DrivingStolenVehicle",SDI.DrivingAtStolenVehicle),
            new CrimeDispatch("TerroristActivity",SDI.TerroristActivity),
            new CrimeDispatch("BrandishingCloseCombatWeapon",SDI.CarryingWeapon),
            new CrimeDispatch("SuspiciousActivity",SDI.SuspiciousActivity),
            new CrimeDispatch("DrunkDriving",SDI.DrunkDriving),
            new CrimeDispatch("Kidnapping",SDI.Kidnapping),
            new CrimeDispatch("PublicIntoxication",SDI.PublicIntoxication),
            new CrimeDispatch("InsultingOfficer",SDI.OfficerNeedsAssistance),//these are bad
            new CrimeDispatch("OfficersNeeded",SDI.OfficersNeeded),
            new CrimeDispatch("Harassment",SDI.Harassment),
            new CrimeDispatch("AssaultingCivilians",SDI.AssaultingCivilians),
            new CrimeDispatch("AssaultingWithDeadlyWeapon",SDI.AssaultingCiviliansWithDeadlyWeapon),
            new CrimeDispatch("DealingDrugs",SDI.DealingDrugs),
            new CrimeDispatch("DealingGuns",SDI.DealingGuns),
            new CrimeDispatch("AimingWeaponAtPolice",SDI.AimingWeaponAtPolice),
            new CrimeDispatch(StaticStrings.ArmedRobberyCrimeID,SDI.ArmedRobbery),
            new CrimeDispatch(StaticStrings.BankRobberyCrimeID,SDI.BankRobbery),
            new CrimeDispatch("PublicNuisance",SDI.PublicNuisance),
            new CrimeDispatch("Speeding",SDI.Speeding),
            new CrimeDispatch("PublicVagrancy",SDI.PublicVagrancy),
            new CrimeDispatch(StaticStrings.IndecentExposureCrimeID,SDI.IndecentExposure),
            new CrimeDispatch(StaticStrings.MaliciousVehicleDamageCrimeID,SDI.MaliciousVehicleDamage),
            new CrimeDispatch(StaticStrings.DrugPossessionCrimeID,SDI.DrugPossession),
            new CrimeDispatch(StaticStrings.StandingOnVehicleCrimeID,SDI.StandingOnVehicle),
            new CrimeDispatch(StaticStrings.BuryingABody,SDI.UnlawfulBodyDisposal),

            new CrimeDispatch(StaticStrings.TheftCrimeID,SDI.TheftDispatch),
            new CrimeDispatch(StaticStrings.ShopliftingCrimeID,SDI.Shoplifting),
        };
            DispatchList = new List<Dispatch>
        {
            SDI.OfficerDown
            ,SDI.ShotsFiredAtAnOfficer
            ,SDI.AssaultingOfficer
            ,SDI.ThreateningOfficerWithFirearm
            ,SDI.TrespassingOnGovernmentProperty
            ,SDI.TrespassingOnMilitaryBase
            ,SDI.Trespassing
            ,SDI.StealingAirVehicle
            ,SDI.ShotsFired
            ,SDI.CarryingWeapon
            ,SDI.CivilianDown
            ,SDI.CivilianShot
            ,SDI.CivilianInjury
            ,SDI.GrandTheftAuto
            ,SDI.SuspiciousActivity
            ,SDI.CriminalActivity
            ,SDI.Mugging
            ,SDI.TerroristActivity
            ,SDI.SuspiciousVehicle
            ,SDI.DrivingAtStolenVehicle
            ,SDI.ResistingArrest
            ,SDI.AttemptingSuicide
            ,SDI.FelonySpeeding
            ,SDI.Speeding
            ,SDI.PedHitAndRun
            ,SDI.VehicleHitAndRun
            ,SDI.RecklessDriving
            ,SDI.AnnounceStolenVehicle
            ,SDI.RequestAirSupport
            ,SDI.RequestMilitaryUnits
            ,SDI.RequestNOOSEUnits
            ,SDI.SuspectSpotted
            ,SDI.SuspectSpottedSimple
            ,SDI.SuspectEvaded
            ,SDI.SuspectEvadedSimple
            ,SDI.RemainInArea
            ,SDI.ResumePatrol
            ,SDI.AttemptToReacquireSuspect
            ,SDI.NoFurtherUnitsNeeded
            ,SDI.SuspectArrested
            ,SDI.SuspectWasted
            ,SDI.ChangedVehicles
            ,SDI.RequestBackup
            ,SDI.RequestBackupSimple
            ,SDI.WeaponsFree
            ,SDI.LethalForceAuthorized
            ,SDI.RunningARedLight
            ,SDI.DrunkDriving
            ,SDI.Kidnapping
            ,SDI.PublicIntoxication
            ,SDI.Harassment
            ,SDI.OfficerNeedsAssistance
            ,SDI.OfficersNeeded
            ,SDI.AssaultingCivilians
            ,SDI.AssaultingCiviliansWithDeadlyWeapon
            ,SDI.DealingDrugs
            ,SDI.DealingGuns
            ,SDI.WantedSuspectSpotted
            ,SDI.RequestNooseUnitsAlt
            ,SDI.RequestNooseUnitsAlt2
            ,SDI.RequestFIBUnits
            ,SDI.RequestSwatAirSupport
            ,SDI.AimingWeaponAtPolice
            ,SDI.OnFoot
            ,SDI.ExcessiveSpeed
            ,SDI.GotOnFreeway
            ,SDI.GotOffFreeway
            ,SDI.WentInTunnel
            ,SDI.TamperingWithVehicle
            ,SDI.VehicleCrashed
            ,SDI.VehicleStartedFire
            ,SDI.ArmedRobbery
            ,SDI.BankRobbery
            ,SDI.MedicalServicesRequired
            ,SDI.FirefightingServicesRequired
            ,SDI.PublicNuisance
            ,SDI.CivilianReportUpdate
            ,SDI.ShotsFiredStatus
            ,SDI.PublicVagrancy
            ,SDI.IndecentExposure
            ,SDI.MaliciousVehicleDamage
            ,SDI.DrugPossession
            ,SDI.StandingOnVehicle
            ,SDI.UnlawfulBodyDisposal
            ,SDI.StoppingTrains
            ,SDI.TheftDispatch
            ,SDI.Shoplifting
        };
        }
        private Dispatch DetermineDispatchFromCrime(Crime crimeAssociated)
        {
            CrimeDispatch ToLookup = DispatchLookup.FirstOrDefault(x => x.CrimeID == crimeAssociated.ID);
            if (ToLookup != null && ToLookup.Dispatch != null)
            {
                ToLookup.Dispatch.Priority = crimeAssociated.Priority;
                return ToLookup.Dispatch;
            }
            return null;
        }
        private void PlayDispatch(DispatchEvent MyAudioEvent, CrimeSceneDescription dispatchDescription, Dispatch dispatchToPlay)
        {
            List<string> soundsToPlayer = MyAudioEvent.SoundsToPlay.ToList();

            EntryPoint.WriteToConsole($"Scanner Start. Playing: {string.Join(",", MyAudioEvent.SoundsToPlay)}", 5);
            if (MyAudioEvent.CanInterrupt && CurrentlyPlaying != null && CurrentlyPlaying.CanBeInterrupted && MyAudioEvent.Priority < CurrentlyPlaying.Priority)
            {
                EntryPoint.WriteToConsole(string.Format("ScannerScript ABORT! Incoming: {0}, Playing: {1}", MyAudioEvent.NotificationText, CurrentlyPlaying.NotificationText), 4);
                AbortedAudio = true;
                Abort();
            }
            if (CurrentlyPlaying != null && CurrentlyPlayingCallIn != null && !CurrentlyPlayingCallIn.SeenByOfficers && dispatchDescription.SeenByOfficers)
            {
                EntryPoint.WriteToConsole(string.Format("ScannerScript ABORT! OFFICER REPORTED STOPPING CIV REPORTING Incoming: {0}, Playing: {1}", MyAudioEvent.NotificationText, CurrentlyPlaying.NotificationText), 4);
                AbortedAudio = true;
                Abort();
            }
            if (MyAudioEvent.CanInterrupt && CurrentlyPlaying != null && CurrentlyPlayingCallIn != null && (CurrentlyPlayingDispatch.Name == SDI.SuspectEvaded.Name ||CurrentlyPlayingDispatch.Name == SDI.AttemptToReacquireSuspect.Name) && Player.AnyPoliceCanSeePlayer)
            {
                EntryPoint.WriteToConsole(string.Format("ScannerScript ABORT! Special Case, Lost Visual Being Cancelled Incoming: {0}, Playing: {1}", MyAudioEvent.NotificationText, CurrentlyPlaying.NotificationText), 4);
                AbortedAudio = true;
                Abort();
            }
            if (AudioPlayer.IsAudioPlaying && AudioPlayer.IsPlayingLowPriority)
            {
                EntryPoint.WriteToConsole("ScannerScript ABORT! LOW PRIORITY PLAYING", 4);
                AbortedAudio = true;
                Abort();
            }

            if (CurrentlyPlaying != null && CurrentlyPlaying.AnyDispatchInterrupts)
            {
                EntryPoint.WriteToConsole(string.Format("ScannerScript ABORT! Incoming: {0}, Playing: {1}", MyAudioEvent.NotificationText, CurrentlyPlaying.NotificationText), 4);
                AbortedAudio = true;
                Abort();
            }


            GameFiber.Yield();
            GameFiber PlayAudioList = GameFiber.StartNew(delegate
            {
                try
                {
                    GameFiber.Yield();
                    if (AbortedAudio)
                    {
                        EntryPoint.WriteToConsole($"Scanner Aborted. Incoming: {string.Join(",", MyAudioEvent.SoundsToPlay)}", 5);
                        if (Settings.SettingsManager.ScannerSettings.SetVolume)
                        {
                            AudioPlayer.Play(RadioEnd.PickRandom(), DesiredVolume, false, Settings.SettingsManager.ScannerSettings.ApplyFilter);
                        }
                        else
                        {
                            AudioPlayer.Play(RadioEnd.PickRandom(), false, Settings.SettingsManager.ScannerSettings.ApplyFilter);
                        }
                        AbortedAudio = false;
                        GameFiber.Sleep(1000);
                    }
                    EntryPoint.WriteToConsole($"PLAY AUDIO LIST TEST 1", 5);
                    uint GameTimeStartedWaitingForAudio = Game.GameTime;
                    while (AudioPlayer.IsAudioPlaying && Game.GameTime - GameTimeStartedWaitingForAudio <= 15000)
                    {
                        GameFiber.Yield();
                    }
                    EntryPoint.WriteToConsole($"PLAY AUDIO LIST TEST 2", 5);
                    if (MyAudioEvent.NotificationTitle != "" && Settings.SettingsManager.ScannerSettings.EnableNotifications)
                    {
                        RemoveAllNotifications();
                        NotificationHandles.Add(Game.DisplayNotification("CHAR_CALL911", "CHAR_CALL911", MyAudioEvent.NotificationTitle, MyAudioEvent.NotificationSubtitle, MyAudioEvent.NotificationText));
                    }
                    EntryPoint.WriteToConsole($"PLAY AUDIO LIST TEST 3", 5);
                    CurrentlyPlaying = MyAudioEvent;
                    CurrentlyPlayingCallIn = dispatchDescription;
                    CurrentlyPlayingDispatch = dispatchToPlay;
                    EntryPoint.WriteToConsole($"PLAY AUDIO LIST TEST 4", 5);
                    if (Settings.SettingsManager.ScannerSettings.EnableAudio)
                    {
                        foreach (string audioname in soundsToPlayer)
                        {
                            EntryPoint.WriteToConsole($"Scanner Playing. ToAudioPlayer: {audioname} isblank {audioname == ""}", 5);
                            if (audioname != "" && audioname != null && audioname.Length > 2 && EntryPoint.ModController.IsRunning)
                            {
                                if (Settings.SettingsManager.ScannerSettings.SetVolume)
                                {
                                    AudioPlayer.Play(audioname, DesiredVolume, false, Settings.SettingsManager.ScannerSettings.ApplyFilter);
                                }
                                else
                                {
                                    AudioPlayer.Play(audioname, false, Settings.SettingsManager.ScannerSettings.ApplyFilter);
                                }
                                while (AudioPlayer.IsAudioPlaying && EntryPoint.ModController.IsRunning)
                                {
                                    if (MyAudioEvent.Subtitles != "" && Settings.SettingsManager.ScannerSettings.EnableSubtitles && Game.GameTime - GameTimeLastDisplayedSubtitle >= 1500)
                                    {
                                        Game.DisplaySubtitle(MyAudioEvent.Subtitles, 2000);
                                        GameTimeLastDisplayedSubtitle = Game.GameTime;
                                    }
                                    GameTimeLastAnnouncedDispatch = Game.GameTime;
                                    if (MyAudioEvent.HasStreetAudio)
                                    {
                                        GameTimeLastMentionedStreet = Game.GameTime;
                                    }
                                    if (MyAudioEvent.HasZoneAudio)
                                    {
                                        GameTimeLastMentionedZone = Game.GameTime;
                                    }
                                    if (MyAudioEvent.HasUnitAudio)
                                    {
                                        GameTimeLastMentionedUnits = Game.GameTime;
                                    }
                                    if (MyAudioEvent.HasLocationAudio)
                                    {
                                        GameTimeLastMentionedLocation = Game.GameTime;
                                    }
                                    GameFiber.Yield();
                                    if (AbortedAudio)
                                    {
                                        //EntryPoint.WriteToConsole($"AbortedAudio1", 5);
                                        break;
                                    }
                                }
                                if (AbortedAudio)
                                {
                                    //EntryPoint.WriteToConsole($"AbortedAudio2", 5);
                                    break;
                                }
                            }
                        }
                    }
                    if (AbortedAudio)
                    {
                        AbortedAudio = false;
                    }
                    CurrentlyPlaying = null;
                    if (dispatchDescription.VehicleSeen != null)
                    {
                        dispatchDescription.VehicleSeen.HasBeenDescribedByDispatch = true;
                    }
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "PlayAudioList");
        }
        private void PlayAmbientDispatch(DispatchEvent MyAudioEvent)
        {
            return;
            List<string> soundsToPlayer = MyAudioEvent.SoundsToPlay.ToList();
            GameFiber PlayAudioList = GameFiber.StartNew(delegate
            {
                try
                {
                    uint GameTimeStartedWaitingForAudio = Game.GameTime;
                    while (SecondaryAudioPlayer.IsAudioPlaying && Game.GameTime - GameTimeStartedWaitingForAudio <= 15000)
                    {
                        GameFiber.Yield();
                    }
                    if (Settings.SettingsManager.ScannerSettings.EnableAudio)
                    {
                        foreach (string audioname in soundsToPlayer)
                        {
                            //EntryPoint.WriteToConsole($"Scanner Playing. ToAudioPlayer: {audioname} isblank {audioname == ""}", 5);
                            if (audioname != "" && audioname != null && audioname.Length > 2 && EntryPoint.ModController.IsRunning && !AudioPlayer.IsAudioPlaying)
                            {
                                if (Settings.SettingsManager.ScannerSettings.SetVolume)
                                {
                                    SecondaryAudioPlayer.Play(audioname, DesiredVolume, false, Settings.SettingsManager.ScannerSettings.ApplyFilter);
                                }
                                else
                                {
                                    SecondaryAudioPlayer.Play(audioname, false, Settings.SettingsManager.ScannerSettings.ApplyFilter);
                                }
                                while (SecondaryAudioPlayer.IsAudioPlaying && EntryPoint.ModController.IsRunning)
                                {
                                    GameFiber.Yield();
                                    if (AbortedAudio)
                                    {
                                        //EntryPoint.WriteToConsole($"AbortedAudio1", 5);
                                        break;
                                    }
                                    if (AudioPlayer.IsAudioPlaying)
                                    {
                                       //EntryPoint.WriteToConsole($"AbortedAudio333", 5);
                                        break;
                                    }
                                }
                                if (AbortedAudio)
                                {
                                    //EntryPoint.WriteToConsole($"AbortedAudio2", 5);
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "PlayAudioList");
        }
        private void RemoveAllNotifications()
        {
            foreach (uint handles in NotificationHandles)
            {
                Game.RemoveNotification(handles);
            }
            NotificationHandles.Clear();
        }
        private void SetupDispatches()
        {
            RadioStart = new List<string>() { AudioBeeps.Radio_Start_1.FileName };
            RadioEnd = new List<string>() { AudioBeeps.Radio_End_1.FileName };
            AttentionAllUnits = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { attention_all_units_gen.Attentionallunits.FileName},"attention all units"),
                new AudioSet(new List<string>() { attention_all_units_gen.Attentionallunits1.FileName },"attention all units"),
                new AudioSet(new List<string>() { attention_all_units_gen.Attentionallunits3.FileName },"attention all units"),
            };
            OfficersReport = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { we_have.OfficersReport_1.FileName},"officers report"),
                new AudioSet(new List<string>() { we_have.OfficersReport_2.FileName },"officers report"),
                new AudioSet(new List<string>() { we_have.UnitsReport_1.FileName },"units report"),
            };
            CiviliansReport = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { we_have.CitizensReport_1.FileName },"citizens report"),
                new AudioSet(new List<string>() { we_have.CitizensReport_2.FileName },"citizens report"),
                new AudioSet(new List<string>() { we_have.CitizensReport_3.FileName },"citizens report"),
                new AudioSet(new List<string>() { we_have.CitizensReport_4.FileName },"citizens report"),
            };
            LethalForce = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { lethal_force.Useofdeadlyforceauthorized.FileName},"use of deadly force authorized"),
                new AudioSet(new List<string>() { lethal_force.Useofdeadlyforceisauthorized.FileName },"use of deadly force is authorized"),
                new AudioSet(new List<string>() { lethal_force.Useofdeadlyforceisauthorized1.FileName },"use of deadly force is authorized"),
                new AudioSet(new List<string>() { lethal_force.Useoflethalforceisauthorized.FileName },"use of lethal force is authorized"),
                new AudioSet(new List<string>() { lethal_force.Useofdeadlyforcepermitted1.FileName },"use of deadly force permitted"),
            };
            LicensePlateSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { suspect_license_plate.SuspectLicensePlate.FileName},"suspect license plate"),
                new AudioSet(new List<string>() { suspect_license_plate.SuspectsLicensePlate01.FileName },"suspects license plate"),
                new AudioSet(new List<string>() { suspect_license_plate.SuspectsLicensePlate02.FileName },"suspects license plate"),
                new AudioSet(new List<string>() { suspect_license_plate.TargetLicensePlate.FileName },"target license plate"),
                new AudioSet(new List<string>() { suspect_license_plate.TargetsLicensePlate.FileName },"targets license plate"),
                new AudioSet(new List<string>() { suspect_license_plate.TargetVehicleLicensePlate.FileName },"target vehicle license plate"),
            };
            RespondCode3Set = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { dispatch_respond_code.UnitsrespondCode3.FileName },"units respond code-3"),
                new AudioSet(new List<string>() { dispatch_respond_code.RespondCode3.FileName },"units respond code-3"),
                new AudioSet(new List<string>() { dispatch_respond_code.UnitrespondCode3.FileName },"units respond code-3"),
            };
            RespondCode2Set = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { dispatch_respond_code.RespondCode2.FileName },"units respond code-2"),
                new AudioSet(new List<string>() { dispatch_respond_code.UnitrespondCode2.FileName },"units respond code-2"),
                new AudioSet(new List<string>() { dispatch_respond_code.UnitsrespondCode2.FileName },"units respond code-2"),
            };

            UnitEnRouteSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { s_m_y_cop_black_full_02.RogerEnRoute1.FileName},"Copy Dispatch."),
                new AudioSet(new List<string>() { s_m_y_cop_black_mini_01.RogerEnRoute1.FileName},"Copy that we are on our way."),
               // new AudioSet(new List<string>() { s_m_y_cop_black_mini_02.RogerEnRoute1.FileName},"we are en route"),//victor 13
                new AudioSet(new List<string>() { s_m_y_cop_black_mini_03.RogerEnRoute1.FileName},"Acknowledged, on our way."),
                //new AudioSet(new List<string>() { s_m_y_cop_black_mini_04.RogerEnRoute1.FileName},"we are en route"),//ocean-1
                new AudioSet(new List<string>() { s_m_y_cop_white_full_01.RogerEnRoute1.FileName},"Copy that we are on our way."),
                new AudioSet(new List<string>() { s_m_y_cop_white_full_02.RogerEnRoute1.FileName},"Copy that we are on our way."),
                new AudioSet(new List<string>() { s_m_y_cop_white_mini_02.RogerEnRoute1.FileName},"Copy that we are on our way."),
                //new AudioSet(new List<string>() { s_m_y_cop_white_mini_03.RogerEnRoute1.FileName},"we are en route"),//Specific unit
                //new AudioSet(new List<string>() { s_m_y_cop_white_mini_04.RogerEnRoute1.FileName},"we are en route"),//Specific unit
                new AudioSet(new List<string>() { s_m_y_hwaycop_black_full_01.RogerEnRoute1.FileName},"Copy that we are on our way."),
                //new AudioSet(new List<string>() { s_m_y_hwaycop_black_full_02.RogerEnRoute1.FileName},"we are en route"),//Specific unit
               // new AudioSet(new List<string>() { s_m_y_hwaycop_white_full_01.RogerEnRoute1.FileName},"we are en route"),//Specific unit
               // new AudioSet(new List<string>() { s_m_y_hwaycop_white_full_02.RogerEnRoute1.FileName},"we are en route"),//Specific unit
            };
            SDI = new ScannerDispatchInformation();
            SDI.Setup();
        }
    }
}