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
    public class ScannerPlus
    {
        private IPoliceRespondable Player;
        private IEntityProvideable World;
        private IPlacesOfInterest PlacesOfInterest;
        private ISettingsProvideable Settings;
        private ITimeReportable Time;
        private IAudioPlayable AudioPlayer;
        private IAudioPlayable SecondaryAudioPlayer;


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


        private List<string> RadioStart;

        private DispatchEvent CurrentlyPlaying;
        private CrimeSceneDescription CurrentlyPlayingCallIn;
        private Dispatch CurrentlyPlayingDispatch;


        private List<Dispatch> DispatchQueue = new List<Dispatch>();
        private List<Dispatch> HoldingDispatchQueue = new List<Dispatch>();
        private List<Dispatch> AmbientDispatchQueue = new List<Dispatch>();

        private IDispatcherVoiceable DispatcherVoice;


        private bool ShouldAddAmbientDispatch => Game.GameTime - GameTimeLastAddedAmbientDispatch >= GameTimeBetweenAmbientDispatches;
        private float DesiredVolume => Settings.SettingsManager.ScannerSettings.AudioVolume + (ScannerBoostLevel * Settings.SettingsManager.ScannerSettings.AudioVolumeBoostAmount);

        public ScannerPlus(IEntityProvideable world, IPoliceRespondable currentPlayer, IAudioPlayable audioPlayer, IAudioPlayable secondaryAudioPlayer, ISettingsProvideable settings, ITimeReportable time, IPlacesOfInterest placesOfInterest)
        {
            AudioPlayer = audioPlayer;
            SecondaryAudioPlayer = secondaryAudioPlayer;
            Player = currentPlayer;
            World = world;
            Settings = settings;
            Time = time;
            DispatcherVoice = new DispatcherVoice_LS();
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
            DispatcherVoice.Setup();
            //DefaultConfig();
            GameTimeBetweenAmbientDispatches = RandomItems.GetRandomNumber(Settings.SettingsManager.ScannerSettings.AmbientDispatchesMinTimeBetween, Settings.SettingsManager.ScannerSettings.AmbientDispatchesMaxTimeBetween);
        }
        public void Update()
        {
            if (Settings.SettingsManager.ScannerSettings.IsEnabled && Player.ActivityManager.CanHearScanner)
            {
                UpdateDispatch();
            }
            if (Player.ActivityManager.CanHearScanner != canHearScanner)
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
            foreach (Dispatch ToReset in DispatcherVoice.DispatchList)
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
                            else if (!RecentlyAnnouncedDispatch)
                            {
                                AddToQueue(DispatcherVoice.SDI.CivilianReportUpdate, reportInformation);
                            }
                        }
                    }
                }
            }
        }
  
        private void UpdateDispatch()
        {
            if (Player.RecentlyStartedPlaying)
            {
                return;//don't care right when you become a new person
            }
            AddStatusDispatchesToQueue();
            AnnounceQueue();
        }
        private void AddStatusDispatchesToQueue()
        {
            if (Player.IsWanted && Player.IsAliveAndFree && Settings.SettingsManager.ScannerSettings.AllowStatusAnnouncements)
            {
                if (Player.PoliceResponse.HasBeenWantedFor > 25000 && Player.WantedLevel <= 4)
                {
                    if (!DispatcherVoice.SDI.SuspectSpotted.HasRecentlyBeenPlayed && !VeryRecentlyAnnouncedDispatch && Player.AnyPoliceCanSeePlayer)
                    {
                        //EntryPoint.WriteToConsole($"SCANNER EVENT: ADDED SuspectSpotted", 3);
                        AddToQueue(DispatcherVoice.SDI.SuspectSpotted, new CrimeSceneDescription(!Player.IsInVehicle, true, Game.LocalPlayer.Character.Position));
                    }
                    else if (!Player.AnyPoliceRecentlySeenPlayer && !DispatcherVoice.SDI.AttemptToReacquireSuspect.HasRecentlyBeenPlayed && !DispatcherVoice.SDI.SuspectEvaded.HasRecentlyBeenPlayed)
                    {
                        //EntryPoint.WriteToConsole($"SCANNER EVENT: ADDED AttemptToReacquireSuspect", 3);
                        AddToQueue(DispatcherVoice.SDI.AttemptToReacquireSuspect, new CrimeSceneDescription(false, true, Player.PlacePoliceLastSeenPlayer));
                    }
                }
            }
            else
            {
                foreach (VehicleExt StolenCar in Player.ReportedStolenVehicles)
                {
                    StolenCar.AddedToReportedStolenQueue = true;
                    AddToQueue(DispatcherVoice.SDI.AnnounceStolenVehicle, new CrimeSceneDescription(!Player.IsInVehicle, false, StolenCar.PlaceOriginallyEntered) { VehicleSeen = StolenCar });
                }
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
                        if (Player.IsWanted)
                        {
                            if (!Player.PoliceResponse.WantedLevelHasBeenRadioedIn)
                            {
                                HoldingDispatchQueue.AddRange(DispatchQueue.Where(x => x.LatestInformation.SeenByOfficers || x.IsPoliceStatus));
                                DispatchQueue.RemoveAll(x => x.LatestInformation.SeenByOfficers || x.IsPoliceStatus);
                                EntryPoint.WriteToConsole("Player is Wanted Without Radio In Holding Officer Reports");
                            }
                            else if (HoldingDispatchQueue.Any())
                            {
                                DispatchQueue.AddRange(HoldingDispatchQueue);
                                HoldingDispatchQueue.Clear();
                                EntryPoint.WriteToConsole("Player is Wanted and has radioed in. Restoring Officer Reports");
                            }
                        }

                        if (DispatchQueue.Any())
                        {
                            EntryPoint.WriteToConsole($"DISPATCHES {string.Join(",", DispatchQueue.Select(x => x.Name))}");
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
            if (!DispatcherVoice.SDI.WantedSuspectSpotted.HasRecentlyBeenPlayed)
            {
                if (wantedLevel == 1)
                {
                    AddToQueue(DispatcherVoice.SDI.SuspectSpotted, new CrimeSceneDescription(!Player.IsInVehicle, true, Player.PlacePoliceLastSeenPlayer) { VehicleSeen = Player.CurrentVehicle });
                }
                else
                {
                    AddToQueue(DispatcherVoice.SDI.WantedSuspectSpotted, new CrimeSceneDescription(!Player.IsInVehicle, true, Player.PlacePoliceLastSeenPlayer) { VehicleSeen = Player.CurrentVehicle });
                }
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: OnAppliedWantedStats", 3);
        }
        public void OnArmyDeployed()
        {
            if (Player.IsWanted && !DispatcherVoice.SDI.RequestMilitaryUnits.HasBeenPlayedThisWanted && World.Pedestrians.AnyArmyUnitsSpawned)
            {
                AddToQueue(DispatcherVoice.SDI.RequestMilitaryUnits);
            }
        }
        public void OnBribedPolice()
        {
            if (!DispatcherVoice.SDI.ResumePatrol.HasRecentlyBeenPlayed)
            {
                AddToQueue(DispatcherVoice.SDI.ResumePatrol);
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: OnBribedPolice", 3);
        }
        public void OnExcessiveSpeed()
        {
            if (!DispatcherVoice.SDI.ExcessiveSpeed.HasRecentlyBeenPlayed && Player.IsInVehicle && Player.AnyPoliceCanSeePlayer && Player.WantedLevel <= 4)
            {
                DispatcherVoice.SDI.ExcessiveSpeed.LatestInformation.SeenByOfficers = true;
                DispatcherVoice.SDI.ExcessiveSpeed.LatestInformation.Speed = Game.LocalPlayer.Character.Speed;
                AddToQueue(DispatcherVoice.SDI.ExcessiveSpeed);
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: OnExcessiveSpeed", 5);
        }
        public void OnFIBHETDeployed()
        {
            if (Player.IsWanted && Player.WantedLevel >= 5 && !DispatcherVoice.SDI.RequestFIBUnits.HasBeenPlayedThisWanted)
            {
                AddToQueue(DispatcherVoice.SDI.RequestFIBUnits);
            }
        }
        public void OnFirefightingServicesRequested()
        {
            if (!DispatcherVoice.SDI.FirefightingServicesRequired.HasRecentlyBeenPlayed)
            {
                AddToQueue(DispatcherVoice.SDI.FirefightingServicesRequired);
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: FirefightingServicesRequired", 3);
        }
        public void OnGotInVehicle()
        {
            if (!DispatcherVoice.SDI.ChangedVehicles.HasRecentlyBeenPlayed && Player.CurrentVehicle != null && Player.CurrentVehicle.HasBeenDescribedByDispatch && Player.WantedLevel <= 4)
            {
                AddToQueue(DispatcherVoice.SDI.ChangedVehicles);
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: InVehicle", 3);
        }
        public void OnGotOffFreeway()
        {
            if (!DispatcherVoice.SDI.GotOffFreeway.HasRecentlyBeenPlayed && Player.IsInVehicle && Player.AnyPoliceCanSeePlayer && Player.WantedLevel <= 4)
            {
                DispatcherVoice.SDI.GotOffFreeway.LatestInformation.SeenByOfficers = true;
                AddToQueue(DispatcherVoice.SDI.GotOffFreeway);
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: OnGotOffFreeway", 5);
        }
        public void OnGotOnFreeway()
        {
            if (!DispatcherVoice.SDI.GotOnFreeway.HasRecentlyBeenPlayed && Player.IsInVehicle && Player.AnyPoliceCanSeePlayer && Player.WantedLevel <= 4)
            {
                DispatcherVoice.SDI.GotOnFreeway.LatestInformation.SeenByOfficers = true;
                AddToQueue(DispatcherVoice.SDI.GotOnFreeway);
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: OnGotOnFreeway", 5);
        }
        public void OnWentInTunnel()
        {
            if (!DispatcherVoice.SDI.WentInTunnel.HasRecentlyBeenPlayed && Player.AnyPoliceCanSeePlayer && Player.WantedLevel <= 4)
            {
                DispatcherVoice.SDI.WentInTunnel.LatestInformation.SeenByOfficers = true;
                AddToQueue(DispatcherVoice.SDI.WentInTunnel);
            }
            EntryPoint.WriteToConsole($"SCANNER EVENT: WENT IN TUNNEL", 3);
        }
        public void OnGotOutOfVehicle()
        {
            if (!DispatcherVoice.SDI.OnFoot.HasRecentlyBeenPlayed && Player.WantedLevel <= 4)
            {
                AddToQueue(DispatcherVoice.SDI.OnFoot);
            }
            // EntryPoint.WriteToConsole($"SCANNER EVENT: OnFoot", 3);
        }
        public void OnStoppingTrains()
        {
            if (!DispatcherVoice.SDI.StoppingTrains.HasRecentlyBeenPlayed)
            {
                AddToQueue(DispatcherVoice.SDI.StoppingTrains);
            }

        }
        public void OnHelicoptersDeployed()
        {
            if (Player.IsWanted && !ReportedRequestAirSupport && !DispatcherVoice.SDI.RequestAirSupport.HasBeenPlayedThisWanted && !DispatcherVoice.SDI.RequestSwatAirSupport.HasBeenPlayedThisWanted && World.Pedestrians.AnyHelicopterUnitsSpawned)
            {
                if (World.Pedestrians.AnyNooseUnitsSpawned && Player.WantedLevel >= 4)
                {
                    AddToQueue(DispatcherVoice.SDI.RequestSwatAirSupport);
                }
                else
                {
                    AddToQueue(DispatcherVoice.SDI.RequestAirSupport);
                }
            }
        }
        public void OnInvestigationExpire()
        {
            if (!DispatcherVoice.SDI.NoFurtherUnitsNeeded.HasRecentlyBeenPlayed)
            {
                Reset();
                AddToQueue(DispatcherVoice.SDI.NoFurtherUnitsNeeded);
            }
            else
            {
                Reset();
            }

            // EntryPoint.WriteToConsole($"SCANNER EVENT: OnInvestigationExpire", 3);
        }
        public void OnLethalForceAuthorized()
        {
            if (!ReportedLethalForceAuthorized && !DispatcherVoice.SDI.LethalForceAuthorized.HasBeenPlayedThisWanted && Player.PoliceResponse.IsDeadlyChase)
            {
                AddToQueue(DispatcherVoice.SDI.LethalForceAuthorized);
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: OnLethalForceAuthorized", 3);
        }
        public void OnMedicalServicesRequested()
        {
            if (!DispatcherVoice.SDI.MedicalServicesRequired.HasRecentlyBeenPlayed && (DispatcherVoice.SDI.MedicalServicesRequired.TimesPlayed <= 2 || DispatcherVoice.SDI.MedicalServicesRequired.HasntBeenPlayedForAWhile))
            {
                AddToQueue(DispatcherVoice.SDI.MedicalServicesRequired);
                //EntryPoint.WriteToConsole($"SCANNER EVENT: MedicalServicesRequired", 3);
            }

        }
        public void OnOfficerMIA()
        {
            if (!DispatcherVoice.SDI.OfficerMIA.HasRecentlyBeenPlayed && (DispatcherVoice.SDI.OfficerMIA.TimesPlayed <= 2 || DispatcherVoice.SDI.OfficerMIA.HasntBeenPlayedForAWhile))
            {
                AddToQueue(DispatcherVoice.SDI.OfficerMIA);
            }
        }
        public void OnNooseDeployed()
        {
            if (Player.IsWanted && Player.WantedLevel >= 4 && !DispatcherVoice.SDI.RequestNooseUnitsAlt.HasBeenPlayedThisWanted && !DispatcherVoice.SDI.RequestNooseUnitsAlt2.HasBeenPlayedThisWanted && World.Pedestrians.AnyNooseUnitsSpawned)
            {
                if (RandomItems.RandomPercent(50))
                {
                    AddToQueue(DispatcherVoice.SDI.RequestNooseUnitsAlt);
                }
                else
                {
                    AddToQueue(DispatcherVoice.SDI.RequestNooseUnitsAlt2);
                }
            }
        }
        public void OnPaidFine()
        {
            if (!DispatcherVoice.SDI.ResumePatrol.HasRecentlyBeenPlayed)
            {
                AddToQueue(DispatcherVoice.SDI.ResumePatrol);
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: OnBribedPolice", 3);
        }
        public void OnTalkedOutOfTicket()
        {
            OnPaidFine();
        }
        public void OnPlayerBusted()
        {
            if (!DispatcherVoice.SDI.SuspectArrested.HasRecentlyBeenPlayed && Player.AnyPoliceCanSeePlayer && Player.WantedLevel > 1)
            {
                AddToQueue(DispatcherVoice.SDI.SuspectArrested);
            }
            // EntryPoint.WriteToConsole($"SCANNER EVENT: OnSuspectBusted", 3);
        }
        public void OnPoliceNoticeVehicleChange()
        {
            if (!DispatcherVoice.SDI.ChangedVehicles.HasVeryRecentlyBeenPlayed && Player.CurrentVehicle != null && Player.WantedLevel <= 4)// && !CurrentPlayer.CurrentVehicle.HasBeenDescribedByDispatch)
            {
                AddToQueue(DispatcherVoice.SDI.ChangedVehicles, new CrimeSceneDescription(!Player.IsInVehicle, true, Player.PlacePoliceLastSeenPlayer) { VehicleSeen = Player.CurrentVehicle });
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: OnPoliceNoticeVehicleChange", 3);
        }
        public void OnRequestedBackUp()
        {
            if (!DispatcherVoice.SDI.RequestBackup.HasRecentlyBeenPlayed && Player.WantedLevel <= 5)
            {
                AddToQueue(DispatcherVoice.SDI.RequestBackup, new CrimeSceneDescription(!Player.IsInVehicle, true, Player.PlacePoliceLastSeenPlayer));
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: OnRequestedBackUp", 3);

            //MILITARY
        }
        public void OnRequestedBackUpSimple()
        {
            if (!DispatcherVoice.SDI.RequestBackupSimple.HasRecentlyBeenPlayed && !DispatchQueue.Any(x => x.Name == DispatcherVoice.SDI.RequestBackupSimple.Name) && Player.WantedLevel <= 5)
            {
                AddToQueue(DispatcherVoice.SDI.RequestBackupSimple, new CrimeSceneDescription(true, true, World.PoliceBackupPoint));
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
                    if (!DispatcherVoice.SDI.RemainInArea.HasRecentlyBeenPlayed)
                    {
                        AddToQueue(DispatcherVoice.SDI.RemainInArea, new CrimeSceneDescription(!Player.IsInVehicle, true, Player.PlacePoliceLastSeenPlayer));
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
            if (!DispatcherVoice.SDI.SuspectWasted.HasRecentlyBeenPlayed && Player.AnyPoliceRecentlySeenPlayer && Player.WantedLevel > 1)// && Player.MaxWantedLastLife > 1)
            {
                AddToQueue(DispatcherVoice.SDI.SuspectWasted);
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: OnSuspectWasted", 3);
        }
        public void OnSuspectShooting()
        {
            if (!DispatcherVoice.SDI.ShotsFiredStatus.HasRecentlyBeenPlayed && !VeryRecentlyAnnouncedDispatch && Player.WantedLevel <= 4)
            {
                //EntryPoint.WriteToConsole($"SCANNER EVENT: ADDED Shooting", 3);
                AddToQueue(DispatcherVoice.SDI.ShotsFiredStatus, new CrimeSceneDescription(!Player.IsInVehicle, true, Game.LocalPlayer.Character.Position));
            }
        }
        public void OnVehicleCrashed()
        {
            if (!DispatcherVoice.SDI.VehicleCrashed.HasRecentlyBeenPlayed && Player.AnyPoliceCanSeePlayer && DispatcherVoice.SDI.VehicleCrashed.HasntBeenPlayedForAWhile && Player.WantedLevel <= 4)
            {
                DispatcherVoice.SDI.VehicleCrashed.LatestInformation.SeenByOfficers = true;
                AddToQueue(DispatcherVoice.SDI.VehicleCrashed);
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: OnVehicleCrashed", 3);
        }
        public void OnVehicleStartedFire()
        {
            if (!DispatcherVoice.SDI.VehicleStartedFire.HasRecentlyBeenPlayed && Player.AnyPoliceCanSeePlayer && DispatcherVoice.SDI.VehicleStartedFire.HasntBeenPlayedForAWhile && Player.WantedLevel <= 4)
            {
                DispatcherVoice.SDI.VehicleStartedFire.LatestInformation.SeenByOfficers = true;
                AddToQueue(DispatcherVoice.SDI.VehicleStartedFire);
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: OnVehicleStartedFire", 3);
        }
        public void OnWantedActiveMode()
        {
            if (Player.WantedLevel <= 4)
            {
                if (!DispatcherVoice.SDI.SuspectSpotted.HasVeryRecentlyBeenPlayed && !DispatchQueue.Any() && Player.PoliceResponse.HasBeenWantedFor > 25000)
                {
                    AddToQueue(DispatcherVoice.SDI.SuspectSpotted, new CrimeSceneDescription(!Player.IsInVehicle, true, Game.LocalPlayer.Character.Position));
                }
            }
            else
            {
                if (!DispatcherVoice.SDI.SuspectSpottedSimple.HasVeryRecentlyBeenPlayed && !DispatchQueue.Any() && Player.PoliceResponse.HasBeenWantedFor > 25000)
                {
                    AddToQueue(DispatcherVoice.SDI.SuspectSpottedSimple, new CrimeSceneDescription(!Player.IsInVehicle, true, Game.LocalPlayer.Character.Position));
                }
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: OnStarsActive", 3);
            //MILITARY
        }
        public void OnWantedSearchMode()
        {
            if (Player.WantedLevel <= 4)
            {
                if (!DispatcherVoice.SDI.SuspectEvaded.HasRecentlyBeenPlayed && !DispatchQueue.Any() && !World.Pedestrians.AnyCopsNearPosition(Player.Position, 100f) && Player.WantedLevel <= 4)
                {
                    AddToQueue(DispatcherVoice.SDI.SuspectEvaded, new CrimeSceneDescription(!Player.IsInVehicle, true, Player.PlacePoliceLastSeenPlayer));
                }
            }
            else
            {
                if (!DispatcherVoice.SDI.SuspectEvadedSimple.HasRecentlyBeenPlayed && !DispatchQueue.Any() && !World.Pedestrians.AnyCopsNearPosition(Player.Position, 100f))
                {
                    AddToQueue(DispatcherVoice.SDI.SuspectEvadedSimple, new CrimeSceneDescription(!Player.IsInVehicle, true, Player.PlacePoliceLastSeenPlayer));
                }
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: OnStarsGreyedOut", 3);
            //MILITARY
        }
        public void OnWeaponsFree()
        {
            if (!ReportedWeaponsFree & !DispatcherVoice.SDI.WeaponsFree.HasBeenPlayedThisWanted)
            {
                AddToQueue(DispatcherVoice.SDI.WeaponsFree);
            }
            //EntryPoint.WriteToConsole($"SCANNER EVENT: OnWeaponsFree", 3);
            //MILITARY
        }

        //Builder
        private void AddAudioSet(DispatchEvent dispatchEvent, AudioSet audioSet)
        {
            if (audioSet != null)
            {
                dispatchEvent.SoundsToPlay.AddRange(audioSet.Sounds);
                dispatchEvent.Subtitles += " " + audioSet.Subtitles;
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
                //EventToPlay.SoundsToPlay.Add(RadioEnd.PickRandom());
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
                //AddAudioSet(EventToPlay, AttentionAllUnits.PickRandom());
            }
            else if (!DispatchToPlay.LatestInformation.SeenByOfficers && !DispatchToPlay.IsPoliceStatus)
            {
               // AddAttentionUnits(EventToPlay);
            }

            if (DispatchToPlay.IncludeReportedBy)
            {
                if (DispatchToPlay.LatestInformation.SeenByOfficers)
                {
                    //AddAudioSet(EventToPlay, OfficersReport.PickRandom());
                }
                else
                {
                    //AddAudioSet(EventToPlay, CiviliansReport.PickRandom());
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
                //AddVehicleDescription(EventToPlay, DispatchToPlay.LatestInformation.VehicleSeen, !DispatchToPlay.LatestInformation.SeenByOfficers && DispatchToPlay.IncludeLicensePlate, DispatchToPlay);
                GameFiber.Yield();
            }
            if (DispatchToPlay.IncludeRapSheet)
            {
                //AddRapSheet(EventToPlay);
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
                //AddWeaponDescription(EventToPlay, DispatchToPlay.LatestInformation.WeaponSeen);
                GameFiber.Yield();
            }
            if (DispatchToPlay.ResultsInLethalForce && !DispatcherVoice.SDI.LethalForceAuthorized.HasBeenPlayedThisWanted && DispatchToPlay.Name != DispatcherVoice.SDI.LethalForceAuthorized.Name)
            {
                //AddLethalForce(EventToPlay);
            }
            if (DispatchToPlay.CanAddExtras && Player.IsWanted && !Player.IsDead && Player.PoliceResponse.IsWeaponsFree && !DispatcherVoice.SDI.WeaponsFree.HasBeenPlayedThisWanted && DispatchToPlay.Name != DispatcherVoice.SDI.WeaponsFree.Name)
            {
                //AddWeaponsFree(EventToPlay);
            }
            if (DispatchToPlay.CanAddExtras && Player.IsWanted && !Player.IsDead && World.Pedestrians.AnyHelicopterUnitsSpawned && !DispatcherVoice.SDI.RequestAirSupport.HasBeenPlayedThisWanted && DispatchToPlay.Name != DispatcherVoice.SDI.RequestAirSupport.Name)
            {
               // AddRequestAirSupport(EventToPlay);
            }
            if (DispatchToPlay.CanAddExtras && DispatchToPlay.IncludeDrivingSpeed && Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
            {
                if (DispatchToPlay.LatestInformation.Speed <= Player.Character.Speed)
                {
                    //AddSpeed(EventToPlay, Player.Character.Speed);
                }
                else
                {
                    //AddSpeed(EventToPlay, DispatchToPlay.LatestInformation.Speed);
                }
                //AddSpeed(EventToPlay, DispatchToPlay.LatestInformation.Speed);// CurrentPlayer.CurrentVehicle.Vehicle.Speed);
                GameFiber.Yield();
            }
            if (DispatchToPlay.LocationDescription != LocationSpecificity.Nothing)
            {
                //AddLocationDescription(EventToPlay, DispatchToPlay.LocationDescription);
                GameFiber.Yield();
            }
            if (DispatchToPlay.CanAddExtras && Player.PoliceResponse.PoliceHaveDescription && !DispatchToPlay.LatestInformation.SeenByOfficers && !DispatchToPlay.IsPoliceStatus)
            {
                //AddHaveDescription(EventToPlay);
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
                    //AddAudioSet(EventToPlay, RespondCode2Set.PickRandom());
                }
                else if (Player.Investigation.InvestigationWantedLevel > 1)
                {
                    EventToPlay.NotificationText += "~n~~r~Responding Code-3~s~";
                    //AddAudioSet(EventToPlay, RespondCode3Set.PickRandom());
                }
            }

           // EventToPlay.SoundsToPlay.Add(RadioEnd.PickRandom());

            if (EventToPlay.HasUnitAudio)
            {
                //foreach (AudioSet audioSet in UnitEnRouteSet.OrderBy(x => Guid.NewGuid()).Take(EventToPlay.UnitAudioAmount).ToList())
                //{
                //    EventToPlay.SoundsToPlay.Add(RadioStart.PickRandom());
                //    AddAudioSet(EventToPlay, audioSet);
                //    EventToPlay.SoundsToPlay.Add(RadioEnd.PickRandom());
                //}
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
                //EventToPlay.SoundsToPlay.Add(RadioEnd.PickRandom());
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
            if (DispatchToPlay.AnyDispatchInterrupts)
            {
                EventToPlay.AnyDispatchInterrupts = true;
            }
            PlayDispatch(EventToPlay, DispatchToPlay.LatestInformation, DispatchToPlay);

        }
        private Dispatch DetermineDispatchFromCrime(Crime crimeAssociated)
        {
            CrimeDispatch ToLookup = DispatcherVoice.GetCrimeDispatch(crimeAssociated); //DispatcherVoice.DispatchLookup.FirstOrDefault(x => x.CrimeID == crimeAssociated.ID);
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
            if (MyAudioEvent.CanInterrupt && CurrentlyPlaying != null && CurrentlyPlayingCallIn != null && (CurrentlyPlayingDispatch.Name == DispatcherVoice.SDI.SuspectEvaded.Name || CurrentlyPlayingDispatch.Name == DispatcherVoice.SDI.AttemptToReacquireSuspect.Name) && Player.AnyPoliceCanSeePlayer)
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
                            //AudioPlayer.Play(RadioEnd.PickRandom(), DesiredVolume, false, Settings.SettingsManager.ScannerSettings.ApplyFilter);
                        }
                        else
                        {
                           // AudioPlayer.Play(RadioEnd.PickRandom(), false, Settings.SettingsManager.ScannerSettings.ApplyFilter);
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
     
        private void RemoveAllNotifications()
        {
            foreach (uint handles in NotificationHandles)
            {
                Game.RemoveNotification(handles);
            }
            NotificationHandles.Clear();
        }
    }
}