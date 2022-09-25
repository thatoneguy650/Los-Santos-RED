using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DispatchScannerFiles;

public class PoliceScannerNew
{
    private IPoliceRespondable Player;
    private IAudioPlayable AudioPlayer;
    private ISettingsProvideable Settings;
    private ITimeReportable Time;
    private IEntityProvideable World;
    private uint GameTimeLastAnnouncedDispatch;
    private uint GameTimeLastDisplayedSubtitle;
    private uint GameTimeLastMentionedStreet;
    private uint GameTimeLastMentionedUnits;
    private uint GameTimeLastMentionedZone;
    private uint GameTimeLastMentionedLocation;
    private bool AbortedAudio;
    private DispatchEvent CurrentlyPlaying;
    private CrimeSceneDescription CurrentlyPlayingCallIn;
    private Dispatch CurrentlyPlayingDispatch;
    private List<Dispatch> DispatchList = new List<Dispatch>();
    private List<Dispatch> DispatchQueue = new List<Dispatch>();
    private bool ExecutingQueue;
    private List<uint> NotificationHandles = new List<uint>();
    private PoliceScannerDispatchesNew PoliceScannerDispatches;

    public PoliceScannerNew(IEntityProvideable world, IPoliceRespondable currentPlayer, IAudioPlayable audioPlayer, ISettingsProvideable settings, ITimeReportable time)
    {
        AudioPlayer = audioPlayer;
        Player = currentPlayer;
        World = world;
        Settings = settings;
        Time = time;
        PoliceScannerDispatches = new PoliceScannerDispatchesNew(World, Player, AudioPlayer, Settings, Time, this);
    }
    public bool RecentlyAnnouncedDispatch => GameTimeLastAnnouncedDispatch != 0 && Game.GameTime - GameTimeLastAnnouncedDispatch <= 25000;
    public bool RecentlyMentionedStreet => GameTimeLastMentionedStreet != 0 && Game.GameTime - GameTimeLastMentionedStreet <= 10000;
    public bool RecentlyMentionedLocation => GameTimeLastMentionedLocation != 0 && Game.GameTime - GameTimeLastMentionedLocation <= 15000;
    public bool RecentlyMentionedUnits => GameTimeLastMentionedUnits != 0 && Game.GameTime - GameTimeLastMentionedUnits <= 10000;
    public bool RecentlyMentionedZone => GameTimeLastMentionedZone != 0 && Game.GameTime - GameTimeLastMentionedZone <= 10000;
    public bool VeryRecentlyAnnouncedDispatch => GameTimeLastAnnouncedDispatch != 0 && Game.GameTime - GameTimeLastAnnouncedDispatch <= 10000;
    public int HighestCivilianReportedPriority = 99;
    public int HighestOfficerReportedPriority = 99;
    public bool ReportedLethalForceAuthorized;
    public bool ReportedRequestAirSupport;
    public bool ReportedWeaponsFree;

    public void Setup()
    {
        PoliceScannerDispatches.Setup();
    }
    public void DebugPlayDispatch()
    {
        Reset();
        AddToQueue(DispatchList.PickRandom());
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
                    }
                }
            }
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
    }
    public void Update()
    {
        if (Settings.SettingsManager.ScannerSettings.IsEnabled)
        {
            CheckDispatch();
            if (DispatchQueue.Count > 0 && !ExecutingQueue)
            {
                EntryPoint.WriteToConsole("Scanner Dispatch Queue Count > 0, starting execution", 5);
                ExecutingQueue = true;
                GameFiber.Yield();
                GameFiber PlayDispatchQueue = GameFiber.StartNew(delegate
                {
                    GameFiber.Sleep(RandomItems.MyRand.Next(Settings.SettingsManager.ScannerSettings.DelayMinTime, Settings.SettingsManager.ScannerSettings.DelayMaxTime));//GameFiber.Sleep(RandomItems.MyRand.Next(2500, 4500));//Next(1500, 2500)
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
                    while (DispatchQueue.Count > 0)
                    {
                        Dispatch Item = DispatchQueue.OrderBy(x => x.Priority).ToList()[0];
                        bool AddToPlayed = true;
                        if (Player.IsNotWanted && Item.LatestInformation.SeenByOfficers)
                        {
                            AddToPlayed = false;
                        }



                        PoliceScannerDispatches.BuildDispatch(Item, AddToPlayed);




                        if (DispatchQueue.Contains(Item))
                        {
                            DispatchQueue.Remove(Item);
                        }
                        GameFiber.Yield();
                    }
                    ExecutingQueue = false;
                    EntryPoint.WriteToConsole("Scanner Dispatch Queue Count > 0, finishing execution, DONE", 5);
                }, "PlayDispatchQueue");
            }
        }
    }
    private void AddToQueue(Dispatch ToAdd, CrimeSceneDescription ToCallIn)
    {
        Dispatch Existing = DispatchQueue.FirstOrDefault(x => x.Name == ToAdd.Name);
        if (Existing != null)
        {
            Existing.LatestInformation = ToCallIn;
        }
        else
        {
            ToAdd.LatestInformation = ToCallIn;
            //EntryPoint.WriteToConsole("ScannerScript " + ToAdd.Name);
            DispatchQueue.Add(ToAdd);
        }
    }
    private void AddToQueue(Dispatch ToAdd)
    {
        GameFiber.Yield();//TR Added 7
        Dispatch Existing = DispatchQueue.FirstOrDefault(x => x.Name == ToAdd.Name);
        if (Existing == null && Settings.SettingsManager.ScannerSettings.IsEnabled)
        {
            DispatchQueue.Add(ToAdd);
            //EntryPoint.WriteToConsole("ScannerScript " + ToAdd.Name);
        }
    }

    //Events
    public void OnAppliedWantedStats(int wantedLevel)
    {
        if (!PoliceScannerDispatches.WantedSuspectSpotted.HasRecentlyBeenPlayed)
        {
            if (wantedLevel == 1)
            {
                AddToQueue(PoliceScannerDispatches.SuspectSpotted, new CrimeSceneDescription(!Player.IsInVehicle, true, Player.PlacePoliceLastSeenPlayer) { VehicleSeen = Player.CurrentVehicle });
            }
            else
            {
                AddToQueue(PoliceScannerDispatches.WantedSuspectSpotted, new CrimeSceneDescription(!Player.IsInVehicle, true, Player.PlacePoliceLastSeenPlayer) { VehicleSeen = Player.CurrentVehicle });
            }
        }
        EntryPoint.WriteToConsole($"SCANNER EVENT: OnAppliedWantedStats", 3);
    }
    public void OnArmyDeployed()
    {
        if (Player.IsWanted && !PoliceScannerDispatches.RequestMilitaryUnits.HasBeenPlayedThisWanted && World.Pedestrians.AnyArmyUnitsSpawned)
        {
            AddToQueue(PoliceScannerDispatches.RequestMilitaryUnits);
        }
    }
    public void OnBribedPolice()
    {
        if (!PoliceScannerDispatches.ResumePatrol.HasRecentlyBeenPlayed)
        {
            AddToQueue(PoliceScannerDispatches.ResumePatrol);
        }
        EntryPoint.WriteToConsole($"SCANNER EVENT: OnBribedPolice", 3);
    }
    public void OnExcessiveSpeed()
    {
        if (!PoliceScannerDispatches.ExcessiveSpeed.HasRecentlyBeenPlayed && Player.IsInVehicle && Player.AnyPoliceCanSeePlayer)
        {
            PoliceScannerDispatches.ExcessiveSpeed.LatestInformation.SeenByOfficers = true;
            PoliceScannerDispatches.ExcessiveSpeed.LatestInformation.Speed = Game.LocalPlayer.Character.Speed;
            AddToQueue(PoliceScannerDispatches.ExcessiveSpeed);
        }
        EntryPoint.WriteToConsole($"SCANNER EVENT: OnExcessiveSpeed", 5);
    }
    public void OnFIBHRTDeployed()
    {
        if (Player.IsWanted && Player.WantedLevel >= 5 && !PoliceScannerDispatches.RequestFIBUnits.HasBeenPlayedThisWanted)
        {
            AddToQueue(PoliceScannerDispatches.RequestFIBUnits);
        }
    }
    public void OnFirefightingServicesRequested()
    {
        if (!PoliceScannerDispatches.FirefightingServicesRequired.HasRecentlyBeenPlayed)
        {
            AddToQueue(PoliceScannerDispatches.FirefightingServicesRequired);
        }
        EntryPoint.WriteToConsole($"SCANNER EVENT: FirefightingServicesRequired", 3);
    }
    public void OnGotInVehicle()
    {
        if (!PoliceScannerDispatches.ChangedVehicles.HasRecentlyBeenPlayed && Player.CurrentVehicle != null && Player.CurrentVehicle.HasBeenDescribedByDispatch)
        {
            AddToQueue(PoliceScannerDispatches.ChangedVehicles);
        }
        EntryPoint.WriteToConsole($"SCANNER EVENT: InVehicle", 3);
    }
    public void OnGotOffFreeway()
    {
        if (!PoliceScannerDispatches.GotOffFreeway.HasRecentlyBeenPlayed && Player.IsInVehicle && Player.AnyPoliceCanSeePlayer)
        {
            PoliceScannerDispatches.GotOffFreeway.LatestInformation.SeenByOfficers = true;
            AddToQueue(PoliceScannerDispatches.GotOffFreeway);
        }
        EntryPoint.WriteToConsole($"SCANNER EVENT: OnGotOffFreeway", 5);
    }
    public void OnGotOnFreeway()
    {
        if (!PoliceScannerDispatches.GotOnFreeway.HasRecentlyBeenPlayed && Player.IsInVehicle && Player.AnyPoliceCanSeePlayer)
        {
            PoliceScannerDispatches.GotOnFreeway.LatestInformation.SeenByOfficers = true;
            AddToQueue(PoliceScannerDispatches.GotOnFreeway);
        }
        EntryPoint.WriteToConsole($"SCANNER EVENT: OnGotOnFreeway", 5);
    }
    public void OnGotOutOfVehicle()
    {
        if (!PoliceScannerDispatches.OnFoot.HasRecentlyBeenPlayed)
        {
            AddToQueue(PoliceScannerDispatches.OnFoot);
        }
        EntryPoint.WriteToConsole($"SCANNER EVENT: OnFoot", 3);
    }
    public void OnHelicoptersDeployed()
    {
        if (Player.IsWanted && !ReportedRequestAirSupport && !PoliceScannerDispatches.RequestAirSupport.HasBeenPlayedThisWanted && !PoliceScannerDispatches.RequestSwatAirSupport.HasBeenPlayedThisWanted && World.Pedestrians.AnyHelicopterUnitsSpawned)
        {
            if (World.Pedestrians.AnyNooseUnitsSpawned && Player.WantedLevel >= 4)
            {
                AddToQueue(PoliceScannerDispatches.RequestSwatAirSupport);
            }
            else
            {
                AddToQueue(PoliceScannerDispatches.RequestAirSupport);
            }
        }
    }
    public void OnInvestigationExpire()
    {
        if (!PoliceScannerDispatches.NoFurtherUnitsNeeded.HasRecentlyBeenPlayed)
        {
            Reset();
            AddToQueue(PoliceScannerDispatches.NoFurtherUnitsNeeded);
        }
        else
        {
            Reset();
        }

        EntryPoint.WriteToConsole($"SCANNER EVENT: OnInvestigationExpire", 3);
    }
    public void OnLethalForceAuthorized()
    {
        if (!ReportedLethalForceAuthorized && !PoliceScannerDispatches.LethalForceAuthorized.HasBeenPlayedThisWanted && Player.PoliceResponse.IsDeadlyChase)
        {
            AddToQueue(PoliceScannerDispatches.LethalForceAuthorized);
        }
        EntryPoint.WriteToConsole($"SCANNER EVENT: OnLethalForceAuthorized", 3);
    }
    public void OnMedicalServicesRequested()
    {
        if (!PoliceScannerDispatches.MedicalServicesRequired.HasRecentlyBeenPlayed && (PoliceScannerDispatches.MedicalServicesRequired.TimesPlayed <= 2 || PoliceScannerDispatches.MedicalServicesRequired.HasntBeenPlayedForAWhile))
        {
            AddToQueue(PoliceScannerDispatches.MedicalServicesRequired);
            EntryPoint.WriteToConsole($"SCANNER EVENT: MedicalServicesRequired", 3);
        }

    }
    public void OnNooseDeployed()
    {
        if (Player.IsWanted && Player.WantedLevel >= 4 && !PoliceScannerDispatches.RequestNooseUnitsAlt.HasBeenPlayedThisWanted && !PoliceScannerDispatches.RequestNooseUnitsAlt2.HasBeenPlayedThisWanted && World.Pedestrians.AnyNooseUnitsSpawned)
        {
            if (RandomItems.RandomPercent(50))
            {
                AddToQueue(PoliceScannerDispatches.RequestNooseUnitsAlt);
            }
            else
            {
                AddToQueue(PoliceScannerDispatches.RequestNooseUnitsAlt2);
            }
        }
    }
    public void OnPaidFine()
    {
        if (!PoliceScannerDispatches.ResumePatrol.HasRecentlyBeenPlayed)
        {
            AddToQueue(PoliceScannerDispatches.ResumePatrol);
        }
        EntryPoint.WriteToConsole($"SCANNER EVENT: OnBribedPolice", 3);
    }
    public void OnTalkedOutOfTicket()
    {
        OnPaidFine();
    }
    public void OnPlayerBusted()
    {
        if (!PoliceScannerDispatches.SuspectArrested.HasRecentlyBeenPlayed && Player.AnyPoliceCanSeePlayer && Player.WantedLevel > 1)
        {
            AddToQueue(PoliceScannerDispatches.SuspectArrested);
        }
        EntryPoint.WriteToConsole($"SCANNER EVENT: OnSuspectBusted", 3);
    }
    public void OnPoliceNoticeVehicleChange()
    {
        if (!PoliceScannerDispatches.ChangedVehicles.HasVeryRecentlyBeenPlayed && Player.CurrentVehicle != null)// && !CurrentPlayer.CurrentVehicle.HasBeenDescribedByDispatch)
        {
            AddToQueue(PoliceScannerDispatches.ChangedVehicles, new CrimeSceneDescription(!Player.IsInVehicle, true, Player.PlacePoliceLastSeenPlayer) { VehicleSeen = Player.CurrentVehicle });
        }
        EntryPoint.WriteToConsole($"SCANNER EVENT: OnPoliceNoticeVehicleChange", 3);
    }
    public void OnRequestedBackUp()
    {
        if (!PoliceScannerDispatches.RequestBackup.HasRecentlyBeenPlayed)
        {
            AddToQueue(PoliceScannerDispatches.RequestBackup, new CrimeSceneDescription(!Player.IsInVehicle, true, Player.PlacePoliceLastSeenPlayer));
        }
        EntryPoint.WriteToConsole($"SCANNER EVENT: OnRequestedBackUp", 3);

        //MILITARY
    }
    public void OnRequestedBackUpSimple()
    {
        if (!PoliceScannerDispatches.RequestBackupSimple.HasRecentlyBeenPlayed && !DispatchQueue.Any(x => x.Name == PoliceScannerDispatches.RequestBackupSimple.Name))
        {
            AddToQueue(PoliceScannerDispatches.RequestBackupSimple, new CrimeSceneDescription(true, true, World.PoliceBackupPoint));
        }
        EntryPoint.WriteToConsole($"SCANNER EVENT: OnRequestedBackUpSimple", 3);
        //MILITARY
    }
    public void OnSuspectEluded()
    {
        //this is becuase when the search mode times out, it just sets the wanted to zero, which clears all the scanner dispatch queue stuff, so this doesnt get played, temp waiting 5 seconds so it will go in after this
        //long teerm need to change how the wanted level is set maybe with the chase result flag
        GameFiber TempWait = GameFiber.StartNew(delegate
        {
            GameFiber.Sleep(1000);
            if (!PoliceScannerDispatches.RemainInArea.HasRecentlyBeenPlayed)
            {
                AddToQueue(PoliceScannerDispatches.RemainInArea, new CrimeSceneDescription(!Player.IsInVehicle, true, Player.PlacePoliceLastSeenPlayer));
            }
            EntryPoint.WriteToConsole($"SCANNER EVENT: OnSuspectEluded", 3);
        }, "PlayDispatchQueue");
    }
    public void OnSuspectWasted()
    {
        if (!PoliceScannerDispatches.SuspectWasted.HasRecentlyBeenPlayed && Player.AnyPoliceRecentlySeenPlayer && Player.WantedLevel > 1)// && Player.MaxWantedLastLife > 1)
        {
            AddToQueue(PoliceScannerDispatches.SuspectWasted);
        }
        EntryPoint.WriteToConsole($"SCANNER EVENT: OnSuspectWasted", 3);
    }
    public void OnSuspectShooting()
    {
        if (!PoliceScannerDispatches.ShotsFiredStatus.HasRecentlyBeenPlayed && !VeryRecentlyAnnouncedDispatch)
        {
            EntryPoint.WriteToConsole($"SCANNER EVENT: ADDED Shooting", 3);
            AddToQueue(PoliceScannerDispatches.ShotsFiredStatus, new CrimeSceneDescription(!Player.IsInVehicle, true, Game.LocalPlayer.Character.Position));
        }
    }
    public void OnVehicleCrashed()
    {
        if (!PoliceScannerDispatches.VehicleCrashed.HasRecentlyBeenPlayed && Player.AnyPoliceCanSeePlayer && PoliceScannerDispatches.VehicleCrashed.HasntBeenPlayedForAWhile)
        {
            PoliceScannerDispatches.VehicleCrashed.LatestInformation.SeenByOfficers = true;
            AddToQueue(PoliceScannerDispatches.VehicleCrashed);
        }
        EntryPoint.WriteToConsole($"SCANNER EVENT: OnVehicleCrashed", 3);
    }
    public void OnVehicleStartedFire()
    {
        if (!PoliceScannerDispatches.VehicleStartedFire.HasRecentlyBeenPlayed && Player.AnyPoliceCanSeePlayer && PoliceScannerDispatches.VehicleStartedFire.HasntBeenPlayedForAWhile)
        {
            PoliceScannerDispatches.VehicleStartedFire.LatestInformation.SeenByOfficers = true;
            AddToQueue(PoliceScannerDispatches.VehicleStartedFire);
        }
        EntryPoint.WriteToConsole($"SCANNER EVENT: OnVehicleStartedFire", 3);
    }
    public void OnWantedActiveMode()
    {
        if (!PoliceScannerDispatches.SuspectSpotted.HasVeryRecentlyBeenPlayed && !DispatchQueue.Any() && Player.PoliceResponse.HasBeenWantedFor > 25000)
        {
            AddToQueue(PoliceScannerDispatches.SuspectSpotted, new CrimeSceneDescription(!Player.IsInVehicle, true, Game.LocalPlayer.Character.Position));
        }
        EntryPoint.WriteToConsole($"SCANNER EVENT: OnStarsActive", 3);
        //MILITARY
    }
    public void OnWantedSearchMode()
    {
        if (!PoliceScannerDispatches.SuspectEvaded.HasRecentlyBeenPlayed && !DispatchQueue.Any() && !World.Pedestrians.AnyCopsNearPosition(Player.Position, 100f))
        {
            AddToQueue(PoliceScannerDispatches.SuspectEvaded, new CrimeSceneDescription(!Player.IsInVehicle, true, Player.PlacePoliceLastSeenPlayer));
        }
        EntryPoint.WriteToConsole($"SCANNER EVENT: OnStarsGreyedOut", 3);
        //MILITARY
    }
    public void OnWeaponsFree()
    {
        if (!ReportedWeaponsFree & !PoliceScannerDispatches.WeaponsFree.HasBeenPlayedThisWanted)
        {
            AddToQueue(PoliceScannerDispatches.WeaponsFree);
        }
        EntryPoint.WriteToConsole($"SCANNER EVENT: OnWeaponsFree", 3);
        //MILITARY
    }

    //Other
    private void CheckDispatch()
    {
        if (Player.RecentlyStartedPlaying)
        {
            return;//don't care right when you become a new person
        }
        // CheckCrimesToAnnounce();
        CheckStatusToAnnounce();
    }
    private void CheckStatusToAnnounce()
    {
        if (Player.IsWanted && Player.IsAliveAndFree && Settings.SettingsManager.ScannerSettings.AllowStatusAnnouncements)
        {
            if (Player.PoliceResponse.HasBeenWantedFor > 25000 && Player.WantedLevel <= 4)
            {
                if (!PoliceScannerDispatches.SuspectSpotted.HasRecentlyBeenPlayed && !VeryRecentlyAnnouncedDispatch && Player.AnyPoliceCanSeePlayer)
                {
                    EntryPoint.WriteToConsole($"SCANNER EVENT: ADDED SuspectSpotted", 3);
                    AddToQueue(PoliceScannerDispatches.SuspectSpotted, new CrimeSceneDescription(!Player.IsInVehicle, true, Game.LocalPlayer.Character.Position));
                }
                else if (!Player.AnyPoliceRecentlySeenPlayer && !PoliceScannerDispatches.AttemptToReacquireSuspect.HasRecentlyBeenPlayed && !PoliceScannerDispatches.SuspectEvaded.HasRecentlyBeenPlayed)
                {
                    EntryPoint.WriteToConsole($"SCANNER EVENT: ADDED AttemptToReacquireSuspect", 3);
                    AddToQueue(PoliceScannerDispatches.AttemptToReacquireSuspect, new CrimeSceneDescription(false, true, Player.PlacePoliceLastSeenPlayer));
                }
            }
        }
        else
        {
            foreach (VehicleExt StolenCar in Player.ReportedStolenVehicles)
            {
                StolenCar.AddedToReportedStolenQueue = true;
                AddToQueue(PoliceScannerDispatches.AnnounceStolenVehicle, new CrimeSceneDescription(!Player.IsInVehicle, false, StolenCar.PlaceOriginallyEntered) { VehicleSeen = StolenCar });
            }
        }
    }
    private Dispatch DetermineDispatchFromCrime(Crime crimeAssociated)
    {
        CrimeDispatch ToLookup = PoliceScannerDispatches.DispatchLookup.FirstOrDefault(x => x.CrimeID == crimeAssociated.ID);
        if (ToLookup != null && ToLookup.Dispatch != null)
        {
            ToLookup.Dispatch.Priority = crimeAssociated.Priority;
            return ToLookup.Dispatch;
        }
        return null;
    }
    public void PlayDispatch(DispatchEvent MyAudioEvent, CrimeSceneDescription dispatchDescription, Dispatch dispatchToPlay)
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
        if (MyAudioEvent.CanInterrupt && CurrentlyPlaying != null && CurrentlyPlayingCallIn != null && (CurrentlyPlayingDispatch.Name == PoliceScannerDispatches.SuspectEvaded.Name || CurrentlyPlayingDispatch.Name == PoliceScannerDispatches.AttemptToReacquireSuspect.Name) && Player.AnyPoliceCanSeePlayer)
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
            GameFiber.Yield();
            if (AbortedAudio)
            {
                EntryPoint.WriteToConsole($"Scanner Aborted. Incoming: {string.Join(",", MyAudioEvent.SoundsToPlay)}", 5);
                if (Settings.SettingsManager.ScannerSettings.SetVolume)
                {
                    AudioPlayer.Play(PoliceScannerDispatches.RadioEnd.PickRandom(), Settings.SettingsManager.ScannerSettings.AudioVolume, false);
                }
                else
                {
                    AudioPlayer.Play(PoliceScannerDispatches.RadioEnd.PickRandom(), false);
                }
                AbortedAudio = false;
                GameFiber.Sleep(1000);
            }
            uint GameTimeStartedWaitingForAudio = Game.GameTime;
            while (AudioPlayer.IsAudioPlaying && Game.GameTime - GameTimeStartedWaitingForAudio <= 15000)
            {
                GameFiber.Yield();
            }
            if (MyAudioEvent.NotificationTitle != "" && Settings.SettingsManager.ScannerSettings.EnableNotifications)
            {
                RemoveAllNotifications();
                NotificationHandles.Add(Game.DisplayNotification("CHAR_CALL911", "CHAR_CALL911", MyAudioEvent.NotificationTitle, MyAudioEvent.NotificationSubtitle, MyAudioEvent.NotificationText));
            }
            CurrentlyPlaying = MyAudioEvent;
            CurrentlyPlayingCallIn = dispatchDescription;
            CurrentlyPlayingDispatch = dispatchToPlay;
            if (Settings.SettingsManager.ScannerSettings.EnableAudio)
            {
                foreach (string audioname in soundsToPlayer)
                {
                    EntryPoint.WriteToConsole($"Scanner Playing. ToAudioPlayer: {audioname} isblank {audioname == ""}", 5);
                    if (audioname != "" && audioname != null && audioname.Length > 2)
                    {
                        if (Settings.SettingsManager.ScannerSettings.SetVolume)
                        {
                            AudioPlayer.Play(audioname, Settings.SettingsManager.ScannerSettings.AudioVolume, false);
                        }
                        else
                        {
                            AudioPlayer.Play(audioname, false);
                        }
                        while (AudioPlayer.IsAudioPlaying)
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
                                EntryPoint.WriteToConsole($"AbortedAudio1", 5);
                                break;
                            }
                        }
                        if (AbortedAudio)
                        {
                            EntryPoint.WriteToConsole($"AbortedAudio2", 5);
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

