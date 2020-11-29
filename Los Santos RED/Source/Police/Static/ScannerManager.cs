using ExtensionsMethods;
using LSR.Vehicles;
using NAudio.Wave;
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static DispatchScannerFiles;

public static class ScannerManager
{
    private static WaveOutEvent outputDevice;
    private static AudioFileReader audioFile;
    private static List<uint> NotificationHandles = new List<uint>();
    private static DispatchEvent CurrentlyPlaying;
    private static int HighestCivilianReportedPriority = 99;
    private static int HighestOfficerReportedPriority = 99;
    private static uint GameTimeLastDisplayedSubtitle;
    private static uint GameTimeLastAnnouncedDispatch;
    private static bool ReportedLethalForceAuthorized = false;

    private static Dispatch OfficerDown;
    private static Dispatch ShotsFiredAtAnOfficer;
    private static Dispatch AssaultingOfficer;
    private static Dispatch ThreateningOfficerWithFirearm;
    private static Dispatch TrespassingOnGovernmentProperty;
    private static Dispatch StealingAirVehicle;
    private static Dispatch ShotsFired;
    private static Dispatch CarryingWeapon;
    private static Dispatch CivilianDown;
    private static Dispatch CivilianShot;
    private static Dispatch CivilianInjury;
    private static Dispatch GrandTheftAuto;
    private static Dispatch SuspiciousActivity;
    private static Dispatch CriminalActivity;
    private static Dispatch Mugging;
    private static Dispatch TerroristActivity;
    private static Dispatch SuspiciousVehicle;
    private static Dispatch DrivingAtStolenVehicle;
    private static Dispatch ResistingArrest;
    private static Dispatch AttemptingSuicide;
    private static Dispatch FelonySpeeding;
    private static Dispatch PedHitAndRun;
    private static Dispatch VehicleHitAndRun;
    private static Dispatch RecklessDriving;
    private static Dispatch AnnounceStolenVehicle;
    private static Dispatch RequestAirSupport;
    private static Dispatch RequestMilitaryUnits;
    private static Dispatch RequestNOOSEUnits;
    private static Dispatch SuspectSpotted;
    private static Dispatch WantedSuspectSpotted;
    private static Dispatch SuspectEvaded;
    private static Dispatch LostVisual;
    private static Dispatch ResumePatrol;
    private static Dispatch SuspectLost;
    private static Dispatch NoFurtherUnitsNeeded;
    private static Dispatch SuspectArrested;
    private static Dispatch SuspectWasted;
    private static Dispatch ChangedVehicles;
    private static Dispatch RequestBackup;
    private static Dispatch WeaponsFree;
    private static Dispatch LethalForceAuthorized;
    private static Dispatch RunningARedLight;

    private static List<Dispatch> DispatchList = new List<Dispatch>();
    private static List<Dispatch> DispatchQueue = new List<Dispatch>();
    private static List<string> RadioStart;
    private static List<string> RadioEnd;
    private static List<Dispatch.AudioSet> AttentionAllUnits;
    private static List<Dispatch.AudioSet> OfficersReport;
    private static List<Dispatch.AudioSet> CiviliansReport;
    private static List<Dispatch.AudioSet> LethalForce;
    private static List<Dispatch.AudioSet> LicensePlateSet;
    private static List<CrimeDispatch> DispatchLookup;
    private static bool ExecutingQueue;
    public static bool CancelAudio { get; set; }
    public static bool IsRunning { get; set; } = true;
    public static bool IsAudioPlaying
    {
        get
        {
            return outputDevice != null;
        }
    }
    public static bool RecentlyAnnouncedDispatch
    {
        get
        {
            if (GameTimeLastAnnouncedDispatch == 0)
                return false;
            if (Game.GameTime - GameTimeLastAnnouncedDispatch <= 25000)
                return true;
            else
                return false;
        }
    }
    private enum LocationSpecificity
    {
        Nothing = 0,
        Zone = 1,
        HeadingAndStreet = 3,
        StreetAndZone = 5,
        Street = 6,
    }
    public static void Initialize()
    {
        SetupLists();
        VehicleScanner.Initialize();
        ZoneScanner.Intitialize();
        StreetScanner.Intitialize();
        IsRunning = true;
    }
    public static void Dispose()
    {
        VehicleScanner.Dispose();
        IsRunning = false;
    }
    public static void Tick()
    {
        if (IsRunning && SettingsManager.MySettings.Police.DispatchAudio)
        {
            CheckDispatch();
            if (DispatchQueue.Count > 0 && !ExecutingQueue)
            {
                ExecutingQueue = true;
                GameFiber PlayDispatchQueue = GameFiber.StartNew(delegate
                {
                    GameFiber.Sleep(General.MyRand.Next(2500, 4500));//Next(1500, 2500)
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
                        BuildDispatch(Item);
                        if (DispatchQueue.Contains(Item))
                            DispatchQueue.Remove(Item);
                    }
                    ExecutingQueue = false;
                }, "PlayDispatchQueue");
                Debugging.GameFibers.Add(PlayDispatchQueue);
            }
        }
    }
    public static void AnnounceCrime(Crime crimeAssociated, PoliceScannerCallIn reportInformation)
    {
        Dispatch ToAnnounce = DetermineDispatchFromCrime(crimeAssociated);
        if(ToAnnounce != null)
        {
            if (!ToAnnounce.HasRecentlyBeenPlayed && (ToAnnounce.CanBeReportedMultipleTimes || ToAnnounce.TimesPlayed == 0))
            {
                if (reportInformation.SeenByOfficers)
                {
                    if (ToAnnounce.Priority <= HighestOfficerReportedPriority)
                        AddToQueue(ToAnnounce, reportInformation);
                }
                else
                {
                   if (ToAnnounce.Priority <= HighestCivilianReportedPriority)
                        AddToQueue(ToAnnounce, reportInformation);
                }
            }
        }
    }
    private static void CheckDispatch()
    {
        if (IsRunning)
        {
            if (PlayerStateManager.IsWanted && PolicePedManager.AnySeenPlayerCurrentWanted)
            {
                if (!RequestBackup.HasRecentlyBeenPlayed && WantedLevelManager.RecentlyRequestedBackup)
                {
                    AddToQueue(RequestBackup, new PoliceScannerCallIn(!PlayerStateManager.IsInVehicle, true, PolicePedManager.PlaceLastSeenPlayer));
                }
                if (!RequestMilitaryUnits.HasBeenPlayedThisWanted && PedManager.AnyArmyUnitsSpawned)
                {
                    AddToQueue(RequestMilitaryUnits);
                }
                if (!RequestNOOSEUnits.HasBeenPlayedThisWanted && PedManager.AnyNooseUnitsSpawned)
                {
                    AddToQueue(RequestNOOSEUnits);
                }
                if (!WeaponsFree.HasBeenPlayedThisWanted && WantedLevelManager.IsWeaponsFree)
                {
                    AddToQueue(WeaponsFree);
                }
                if (!RequestAirSupport.HasBeenPlayedThisWanted && PedManager.AnyHelicopterUnitsSpawned)
                {
                    AddToQueue(RequestAirSupport);
                }
                if (!ReportedLethalForceAuthorized && WantedLevelManager.IsDeadlyChase)
                {
                    AddToQueue(LethalForceAuthorized);
                }
                if (!SuspectArrested.HasRecentlyBeenPlayed && PlayerStateManager.RecentlyBusted && PolicePedManager.AnyCanSeePlayer)
                {
                    AddToQueue(SuspectArrested);
                }
                if (!ChangedVehicles.HasRecentlyBeenPlayed && PlayerStateManager.PoliceRecentlyNoticedVehicleChange && PlayerStateManager.CurrentVehicle != null && !PlayerStateManager.CurrentVehicle.HasBeenDescribedByDispatch)
                {
                    AddToQueue(ChangedVehicles, new PoliceScannerCallIn(!PlayerStateManager.IsInVehicle, true, PolicePedManager.PlaceLastSeenPlayer) { VehicleSeen = PlayerStateManager.CurrentVehicle });
                }
                if (!WantedSuspectSpotted.HasRecentlyBeenPlayed && PersonOfInterestManager.RecentlyAppliedWantedStats)
                {
                    AddToQueue(WantedSuspectSpotted, new PoliceScannerCallIn(!PlayerStateManager.IsInVehicle, true, PolicePedManager.PlaceLastSeenPlayer) { VehicleSeen = PlayerStateManager.CurrentVehicle });
                }

                if (!PlayerStateManager.IsBusted && !PlayerStateManager.IsDead)
                {
                    if (!LostVisual.HasRecentlyBeenPlayed && PlayerStateManager.StarsRecentlyGreyedOut && WantedLevelManager.HasBeenWantedFor > 45000 && !PedManager.AnyCopsNearPlayer)
                    {
                        AddToQueue(LostVisual, new PoliceScannerCallIn(!PlayerStateManager.IsInVehicle, true, PolicePedManager.PlaceLastSeenPlayer));
                    }
                    else if (!SuspectSpotted.HasRecentlyBeenPlayed && PlayerStateManager.StarsRecentlyActive && WantedLevelManager.HasBeenWantedFor > 25000 && PolicePedManager.AnyRecentlySeenPlayer)
                    {
                        AddToQueue(SuspectSpotted, new PoliceScannerCallIn(!PlayerStateManager.IsInVehicle, true, Game.LocalPlayer.Character.Position));
                    }
                    else if (!RecentlyAnnouncedDispatch && PolicePedManager.AnyCanSeePlayer && WantedLevelManager.HasBeenWantedFor > 25000)
                    {
                        AddToQueue(SuspectSpotted, new PoliceScannerCallIn(!PlayerStateManager.IsInVehicle, true, Game.LocalPlayer.Character.Position));
                    }
                }

            }
            else
            {
                if (!ResumePatrol.HasRecentlyBeenPlayed && RespawnManager.RecentlyBribedPolice)
                {
                    AddToQueue(ResumePatrol);
                }
                if (!SuspectLost.HasRecentlyBeenPlayed && WantedLevelManager.RecentlyLostWanted && !RespawnManager.RecentlyRespawned && !RespawnManager.RecentlyBribedPolice && !RespawnManager.RecentlySurrenderedToPolice)
                {
                    AddToQueue(SuspectLost, new PoliceScannerCallIn(!PlayerStateManager.IsInVehicle, true, PolicePedManager.PlaceLastSeenPlayer));
                }
                if (!NoFurtherUnitsNeeded.HasRecentlyBeenPlayed && InvestigationManager.LastInvestigationRecentlyExpired && InvestigationManager.DistanceToInvestigationPosition <= 1000f)
                {
                    AddToQueue(NoFurtherUnitsNeeded);
                }
                foreach (VehicleExt StolenCar in PlayerStateManager.ReportedStolenVehicles)
                {
                    AddToQueue(AnnounceStolenVehicle, new PoliceScannerCallIn(!PlayerStateManager.IsInVehicle, true, PolicePedManager.PlaceLastSeenPlayer) { VehicleSeen = StolenCar });
                }
            }

            if (!SuspectWasted.HasRecentlyBeenPlayed && PlayerStateManager.RecentlyDied && PolicePedManager.AnyRecentlySeenPlayer && PlayerStateManager.MaxWantedLastLife > 0)
            {
                AddToQueue(SuspectWasted);
            }

        }
    }
    private static void AddToQueue(Dispatch ToAdd,PoliceScannerCallIn ToCallIn)
    {
        Dispatch Existing = DispatchQueue.FirstOrDefault(x => x.Name == ToAdd.Name);
        if (Existing != null)
        {
            Existing.LatestInformation = ToCallIn;
        }
        else
        {
            ToAdd.LatestInformation = ToCallIn;
            Debugging.WriteToLog("ScannerScript", ToAdd.Name);
            DispatchQueue.Add(ToAdd);
        }
    }
    private static void AddToQueue(Dispatch ToAdd)
    {
        Dispatch Existing = DispatchQueue.FirstOrDefault(x => x.Name == ToAdd.Name);
        if (Existing == null)
        {
            DispatchQueue.Add(ToAdd);
            Debugging.WriteToLog("ScannerScript", ToAdd.Name);
        }
    }
    private static Dispatch DetermineDispatchFromCrime(Crime crimeAssociated)
    {
        CrimeDispatch ToLookup = DispatchLookup.FirstOrDefault(x => x.CrimeIdentified == crimeAssociated);
        if(ToLookup != null && ToLookup.DispatchToPlay != null)
        {
            ToLookup.DispatchToPlay.Priority = crimeAssociated.Priority;
            ToLookup.DispatchToPlay.PriorityGroup = crimeAssociated.PriorityGroup;
            return ToLookup.DispatchToPlay;
        }
        return null;
    }
    public static void ResetReportedItems()
    {
        ReportedLethalForceAuthorized = false;
        HighestCivilianReportedPriority = 99;
        HighestOfficerReportedPriority = 99;
        foreach (Dispatch ToReset in DispatchList)
        {
            ToReset.HasBeenPlayedThisWanted = false;
            ToReset.LatestInformation = new PoliceScannerCallIn();
            ToReset.TimesPlayed = 0;
        }
    }   
    private static void BuildDispatch(Dispatch DispatchToPlay)
    {


        DispatchEvent EventToPlay = new DispatchEvent();
        if (DispatchToPlay.HasPreamble)
        {
            EventToPlay.SoundsToPlay.Add(RadioStart.PickRandom());
            AddAudioSet(EventToPlay, DispatchToPlay.PreambleAudioSet.PickRandom());
            EventToPlay.SoundsToPlay.Add(RadioEnd.PickRandom());
        }

        EventToPlay.SoundsToPlay.Add(RadioStart.PickRandom());
        EventToPlay.NotificationTitle = DispatchToPlay.NotificationTitle;

        if(DispatchToPlay.IsStatus)
            EventToPlay.NotificationSubtitle = "~g~Status";
        else if(DispatchToPlay.LatestInformation.SeenByOfficers)
            EventToPlay.NotificationSubtitle = "~r~Crime Observed";
        else
            EventToPlay.NotificationSubtitle = "~o~Crime Reported";

        EventToPlay.NotificationText = DispatchToPlay.NotificationText;

        if (DispatchToPlay.IncludeAttentionAllUnits)
            AddAudioSet(EventToPlay, AttentionAllUnits.PickRandom());

        if (DispatchToPlay.IncludeReportedBy)
        {
            if (DispatchToPlay.LatestInformation.SeenByOfficers)
                AddAudioSet(EventToPlay, OfficersReport.PickRandom());
            else
                AddAudioSet(EventToPlay, CiviliansReport.PickRandom());
        }

        if(DispatchToPlay.LatestInformation.InstancesObserved > 1 && DispatchToPlay.MainMultiAudioSet.Any())
        {
            AddAudioSet(EventToPlay, DispatchToPlay.MainMultiAudioSet.PickRandom());
        }
        else
        {
            AddAudioSet(EventToPlay, DispatchToPlay.MainAudioSet.PickRandom());
        }
        
        AddAudioSet(EventToPlay, DispatchToPlay.SecondaryAudioSet.PickRandom());

        if (DispatchToPlay.IncludeDrivingVehicle)
            AddVehicleDescription(EventToPlay, DispatchToPlay.LatestInformation.VehicleSeen, !DispatchToPlay.LatestInformation.SeenByOfficers && DispatchToPlay.IncludeLicensePlate);

        if (DispatchToPlay.IncludeRapSheet)
            AddRapSheet(EventToPlay);

        if(DispatchToPlay.MarkVehicleAsStolen && DispatchToPlay.LatestInformation != null && DispatchToPlay.LatestInformation.VehicleSeen != null)
        {
            DispatchToPlay.LatestInformation.VehicleSeen.WasReportedStolen = true;
            DispatchToPlay.LatestInformation.VehicleSeen.OriginalLicensePlate.IsWanted = true;
            if(DispatchToPlay.LatestInformation.VehicleSeen.OriginalLicensePlate.PlateNumber == DispatchToPlay.LatestInformation.VehicleSeen.CarPlate.PlateNumber)
            {
                DispatchToPlay.LatestInformation.VehicleSeen.CarPlate.IsWanted = true;
                Debugging.WriteToLog("ScannerScript", "MarkedAsStolen Current Plate");
            }
            else
            {
                Debugging.WriteToLog("ScannerScript", "MarkedAsStolen");
            }
           
        }

        if (DispatchToPlay.IncludeCarryingWeapon && (DispatchToPlay.LatestInformation.WeaponSeen != null || DispatchToPlay.Name  == "Carrying Weapon"))
            AddWeaponDescription(EventToPlay,DispatchToPlay.LatestInformation.WeaponSeen);

        if (DispatchToPlay.ResultsInLethalForce)
            AddLethalForce(EventToPlay);

        if (DispatchToPlay.IncludeDrivingSpeed)
        {
            if (PlayerStateManager.CurrentVehicle != null && PlayerStateManager.CurrentVehicle.VehicleEnt.Exists())
            {
                AddSpeed(EventToPlay,PlayerStateManager.CurrentVehicle.VehicleEnt.Speed);
            }
        }

        AddLocationDescription(EventToPlay, DispatchToPlay.LocationDescription);

        if (InvestigationManager.HavePlayerDescription && !DispatchToPlay.LatestInformation.SeenByOfficers && !DispatchToPlay.IsStatus)
        {
            AddHaveDescription(EventToPlay);
        }

        EventToPlay.SoundsToPlay.Add(RadioEnd.PickRandom());

        EventToPlay.Subtitles = FirstCharToUpper(EventToPlay.Subtitles);
        EventToPlay.Priority = DispatchToPlay.Priority;

        DispatchToPlay.SetPlayed();


        if (DispatchToPlay.LatestInformation.SeenByOfficers && DispatchToPlay.Priority < HighestOfficerReportedPriority)
            HighestOfficerReportedPriority = DispatchToPlay.Priority;
        else if (!DispatchToPlay.LatestInformation.SeenByOfficers && !DispatchToPlay.IsStatus && DispatchToPlay.Priority < HighestCivilianReportedPriority)
            HighestCivilianReportedPriority = DispatchToPlay.Priority;

        PlayDispatch(EventToPlay,DispatchToPlay.LatestInformation);
    }
    private static void PlayDispatch(DispatchEvent MyAudioEvent,PoliceScannerCallIn MyDispatch)
    {
        /////////Maybe?
        bool AbortedAudio = false;
        if (MyAudioEvent.CanInterrupt && CurrentlyPlaying != null && CurrentlyPlaying.CanBeInterrupted && MyAudioEvent.Priority < CurrentlyPlaying.Priority)
        {
            Debugging.WriteToLog("ScannerScript", string.Format("Incoming: {0}, Playing: {1}",MyAudioEvent.NotificationText,CurrentlyPlaying.NotificationText));
            AbortAllAudio();
            AbortedAudio = true;
        }
        GameFiber PlayAudioList = GameFiber.StartNew(delegate
        {
            if (AbortedAudio)
            {
                PlayAudioFile(RadioEnd.PickRandom());
                GameFiber.Sleep(1000);
            }

            while (IsAudioPlaying)
            {
                GameFiber.Yield();
            }

            if (MyAudioEvent.NotificationTitle != "" && SettingsManager.MySettings.Police.DispatchNotifications)
            {
                RemoveAllNotifications();
                NotificationHandles.Add(Game.DisplayNotification("CHAR_CALL911", "CHAR_CALL911", MyAudioEvent.NotificationTitle, MyAudioEvent.NotificationSubtitle, MyAudioEvent.NotificationText));
            }

            Debugging.WriteToLog("ScannerScript", string.Format("Name: {0}, MyAudioEvent.Priority: {1}", MyAudioEvent.NotificationText, MyAudioEvent.Priority));
            CurrentlyPlaying = MyAudioEvent;
            if (MyDispatch.VehicleSeen != null)
                MyDispatch.VehicleSeen.HasBeenDescribedByDispatch = true;

            foreach (string audioname in MyAudioEvent.SoundsToPlay)
            {
                PlayAudioFile(audioname);

                while (IsAudioPlaying)
                {
                    if (MyAudioEvent.Subtitles != "" && SettingsManager.MySettings.Police.DispatchSubtitles && Game.GameTime - GameTimeLastDisplayedSubtitle >= 1500)
                    {
                        Game.DisplaySubtitle(MyAudioEvent.Subtitles, 2000);
                        GameTimeLastDisplayedSubtitle = Game.GameTime;
                    }
                    GameTimeLastAnnouncedDispatch = Game.GameTime;
                    GameFiber.Yield();
                }
                if (CancelAudio)
                {
                    CancelAudio = false;
                    Debugging.WriteToLog("ScannerScript", "CancelAudio Set to False");
                    break;
                }
            }
            CurrentlyPlaying = null;
        }, "PlayAudioList");
        Debugging.GameFibers.Add(PlayAudioList);
    }
    private static void PlayAudioFile(string _Audio)
    {
        try
        {
            if (_Audio == "")
                return;
            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent();
                outputDevice.PlaybackStopped += OnPlaybackStopped;
            }
            if (audioFile == null)
            {
                audioFile = new AudioFileReader(string.Format("Plugins\\LosSantosRED\\audio\\{0}", _Audio))
                {
                    Volume = SettingsManager.MySettings.Police.DispatchAudioVolume
                };
                outputDevice.Init(audioFile);
            }
            else
            {
                outputDevice.Init(audioFile);
            }
            outputDevice.Play();
        }
        catch (Exception e)
        {
            Debugging.WriteToLog("ScannerScript", e.Message);
        }
    }
    private static void AddAudioSet(DispatchEvent dispatchEvent, Dispatch.AudioSet audioSet)
    {
        if (audioSet != null)
        {
            Debugging.WriteToLog("ScannerScript", string.Format("{0}", string.Join(",", audioSet.Sounds)));
            dispatchEvent.SoundsToPlay.AddRange(audioSet.Sounds);
            dispatchEvent.Subtitles += " " + audioSet.Subtitles;
        }
    }
    private static void OnPlaybackStopped(object sender, StoppedEventArgs args)
    {
        outputDevice.Dispose();
        outputDevice = null;
        if (audioFile != null)
        {
            audioFile.Dispose();
        }
        audioFile = null;
    }
    private static void AddLocationDescription(DispatchEvent dispatchEvent, LocationSpecificity locationSpecificity)
    {
        if (locationSpecificity == LocationSpecificity.HeadingAndStreet)
            AddHeading(dispatchEvent);

        if (locationSpecificity == LocationSpecificity.Street || locationSpecificity == LocationSpecificity.HeadingAndStreet || locationSpecificity == LocationSpecificity.StreetAndZone)
            AddStreet(dispatchEvent);

        if (locationSpecificity == LocationSpecificity.Zone || locationSpecificity == LocationSpecificity.StreetAndZone)
            AddZone(dispatchEvent);
    }
    private static void AddHeading(DispatchEvent dispatchEvent)
    {
            dispatchEvent.SoundsToPlay.Add((new List<string>() { suspect_heading.TargetLastSeenHeading.FileName, suspect_heading.TargetReportedHeading.FileName, suspect_heading.TargetSeenHeading.FileName, suspect_heading.TargetSpottedHeading.FileName }).PickRandom());
            dispatchEvent.Subtitles += " ~s~suspect heading~s~";
            string heading = General.GetSimpleCompassHeading(Game.LocalPlayer.Character.Heading);
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
    private static void AddStreet(DispatchEvent dispatchEvent)
    {
        Street MyStreet = PlayerLocationManager.PlayerCurrentStreet;
        if (MyStreet != null)
        {
            string StreetAudio = StreetScanner.AudioAtStreet(MyStreet.Name);
            if (StreetAudio != "")
            {
                dispatchEvent.SoundsToPlay.Add((new List<string>() { conjunctives.On.FileName, conjunctives.On1.FileName, conjunctives.On2.FileName, conjunctives.On3.FileName, conjunctives.On4.FileName }).PickRandom());
                dispatchEvent.SoundsToPlay.Add(StreetAudio);
                dispatchEvent.Subtitles += " ~s~on ~HUD_COLOUR_YELLOWLIGHT~" + MyStreet.Name + "~s~";
                dispatchEvent.NotificationText += "~n~~HUD_COLOUR_YELLOWLIGHT~" + MyStreet.Name + "~s~";

                if (PlayerLocationManager.PlayerCurrentCrossStreet != null)
                {
                    Street MyCrossStreet = PlayerLocationManager.PlayerCurrentCrossStreet;
                    if (MyCrossStreet != null)
                    {
                        string CrossStreetAudio = StreetScanner.AudioAtStreet(MyCrossStreet.Name);
                        if (CrossStreetAudio != "")
                        {
                            dispatchEvent.SoundsToPlay.Add((new List<string>() { conjunctives.AT01.FileName, conjunctives.AT02.FileName }).PickRandom());
                            dispatchEvent.SoundsToPlay.Add(CrossStreetAudio);
                            dispatchEvent.NotificationText += " ~s~at ~HUD_COLOUR_YELLOWLIGHT~" + MyCrossStreet.Name + "~s~";
                            dispatchEvent.Subtitles += " ~s~at ~HUD_COLOUR_YELLOWLIGHT~" + MyCrossStreet.Name + "~s~";
                        }
                    }
                }
            }
        }
    }
    private static void AddZone(DispatchEvent dispatchEvent)
    {
        Zone MyZone = ZoneManager.GetZone(PlayerStateManager.CurrentPosition);
        if (MyZone != null)
        {
            string ScannerAudio = ZoneScanner.AudioAtZone(MyZone.InternalGameName);
            if (ScannerAudio != "")
            {
                dispatchEvent.SoundsToPlay.Add(new List<string> { conjunctives.Nearumm.FileName, conjunctives.Closetoum.FileName, conjunctives.Closetouhh.FileName }.PickRandom());
                dispatchEvent.SoundsToPlay.Add(ScannerAudio);
                dispatchEvent.Subtitles += " ~s~near ~p~" + MyZone.DisplayName + "~s~";
                dispatchEvent.NotificationText += "~n~~p~" + MyZone.DisplayName + "~s~";
            }
        }
    }
    private static void AddVehicleDescription(DispatchEvent dispatchEvent, VehicleExt VehicleToDescribe, bool IncludeLicensePlate)
    {
        if (VehicleToDescribe == null)
            return;
        if (VehicleToDescribe.HasBeenDescribedByDispatch)
            return;
        //else
        //    VehicleToDescribe.HasBeenDescribedByDispatch = true;

        if (VehicleToDescribe != null && VehicleToDescribe.VehicleEnt.Exists())
        {
            dispatchEvent.NotificationText += "~n~Vehicle:~s~";
            dispatchEvent.SoundsToPlay.Add(suspect_is.SuspectIs.FileName);
            dispatchEvent.SoundsToPlay.Add(conjunctives.Drivinga.FileName);
            dispatchEvent.Subtitles += " suspect is driving a ~s~";


            Color CarColor = VehicleToDescribe.VehicleColor(); //Vehicles.VehicleManager.VehicleColor(VehicleToDescribe);
            string MakeName = VehicleToDescribe.MakeName();// Vehicles.VehicleManager.MakeName(VehicleToDescribe);
            int ClassInt = VehicleToDescribe.ClassInt();// Vehicles.VehicleManager.ClassInt(VehicleToDescribe);
            string ClassName = VehicleScanner.ClassName(ClassInt);
            string ModelName = VehicleToDescribe.ModelName();// Vehicles.VehicleManager.ModelName(VehicleToDescribe);

            string ColorAudio = VehicleScanner.ColorAudio(CarColor);
            string MakeAudio = VehicleScanner.MakeAudio(MakeName);
            string ClassAudio = VehicleScanner.ClassAudio(ClassInt);
            string ModelAudio = VehicleScanner.ModelAudio(VehicleToDescribe.VehicleEnt.Model.Hash);

            if(ColorAudio != "")
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

            if(IncludeLicensePlate)
            {
                AddAudioSet(dispatchEvent, LicensePlateSet.PickRandom());
                string LicensePlateText = VehicleToDescribe.OriginalLicensePlate.PlateNumber;
                dispatchEvent.SoundsToPlay.AddRange(VehicleScanner.LicensePlateAudio(LicensePlateText));
                dispatchEvent.Subtitles += " ~s~" + LicensePlateText + "~s~";
                dispatchEvent.NotificationText += " ~s~Plate: " + LicensePlateText + "~s~";
            }
            
            Debugging.WriteToLog("ScannerScript", string.Format("ScannerScript Color {0}, Make {1}, Class {2}, Model {3}, RawModel {4}", CarColor.Name,MakeName,ClassName,ModelName, VehicleToDescribe.VehicleEnt.Model.Name));
        }

    }
    private static void AddWeaponDescription(DispatchEvent dispatchEvent, WeaponInformation WeaponToDescribe)
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
        else if (WeaponToDescribe.Category ==  WeaponCategory.Throwable || WeaponToDescribe.ModelName == "weapon_grenadelauncher_smoke" || WeaponToDescribe.ModelName == "weapon_compactlauncher")
        {
            dispatchEvent.SoundsToPlay.Add(carrying_weapon.Armedwithexplosives.FileName);
            dispatchEvent.Subtitles += " suspect is armed with a ~r~explosives~s~";
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
            dispatchEvent.NotificationText += " melee weapon";
        }
        else
        {
            int Num = General.MyRand.Next(1, 5);
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
    private static void AddRapSheet(DispatchEvent dispatchEvent)
    {
        dispatchEvent.NotificationText = "Wanted For:" + WantedLevelManager.CurrentCrimes.PrintCrimes();
    }
    private static void AddSpeed(DispatchEvent dispatchEvent,float Speed)
    {
        Speed = Speed * 2.23694f;//convert to mph
        if (Speed >= 40f)
        {
            dispatchEvent.SoundsToPlay.Add(suspect_last_seen.TargetLastReported.FileName);
            dispatchEvent.Subtitles += " ~s~target last reported~s~";
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
    }
    private static void AddHaveDescription(DispatchEvent dispatchEvent)
    {
                dispatchEvent.NotificationText += "~n~~r~Have Description~s~";
          
    }
    private static void AddLethalForce(DispatchEvent dispatchEvent)
    {
        if(!ReportedLethalForceAuthorized)
        {
            AddAudioSet(dispatchEvent, LethalForce.PickRandom());
            ReportedLethalForceAuthorized = true;
        }
    }
    private static void RemoveAllNotifications()
    {
        foreach (uint handles in NotificationHandles)
        {
            Game.RemoveNotification(handles);
        }
        NotificationHandles.Clear();
    }
    public static void PlayTestAudio()
    {
        SetupLists();
        BuildDispatch(OfficerDown);
    }
    private static string FirstCharToUpper(this string input)
    {
        switch (input)
        {
            case null: throw new ArgumentNullException(nameof(input));
            case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
            default: return input.First().ToString().ToUpper() + input.Substring(1);
        }
    }
    private static void SetupLists()
    {
        SetupDispatches();
        DispatchLookup = new List<CrimeDispatch>
        {
            new CrimeDispatch(CrimeManager.AttemptingSuicide,AttemptingSuicide),
            new CrimeDispatch(CrimeManager.BrandishingWeapon,CarryingWeapon),
            new CrimeDispatch(CrimeManager.ChangingPlates,SuspiciousActivity),
            new CrimeDispatch(CrimeManager.DrivingAgainstTraffic,RecklessDriving),
            new CrimeDispatch(CrimeManager.DrivingOnPavement,RecklessDriving),
            new CrimeDispatch(CrimeManager.FelonySpeeding,FelonySpeeding),
            new CrimeDispatch(CrimeManager.FiringWeapon,ShotsFired),
            new CrimeDispatch(CrimeManager.FiringWeaponNearPolice,ShotsFiredAtAnOfficer),
            new CrimeDispatch(CrimeManager.GotInAirVehicleDuringChase,StealingAirVehicle),
            new CrimeDispatch(CrimeManager.GrandTheftAuto,GrandTheftAuto),
            new CrimeDispatch(CrimeManager.HitCarWithCar,VehicleHitAndRun),
            new CrimeDispatch(CrimeManager.HitPedWithCar,PedHitAndRun),
            new CrimeDispatch(CrimeManager.RunningARedLight,RunningARedLight),
            new CrimeDispatch(CrimeManager.HurtingCivilians,CivilianInjury),
            new CrimeDispatch(CrimeManager.HurtingPolice,AssaultingOfficer),
            new CrimeDispatch(CrimeManager.KillingCivilians,CivilianDown),
            new CrimeDispatch(CrimeManager.KillingPolice,OfficerDown),
            new CrimeDispatch(CrimeManager.Mugging,Mugging),
            new CrimeDispatch(CrimeManager.NonRoadworthyVehicle,SuspiciousVehicle),
            new CrimeDispatch(CrimeManager.ResistingArrest,ResistingArrest),
            new CrimeDispatch(CrimeManager.TrespessingOnGovtProperty,TrespassingOnGovernmentProperty),
            new CrimeDispatch(CrimeManager.DrivingStolenVehicle,DrivingAtStolenVehicle),
            new CrimeDispatch(CrimeManager.TerroristActivity,TerroristActivity),
            new CrimeDispatch(CrimeManager.BrandishingCloseCombatWeapon,CarryingWeapon),
            new CrimeDispatch(CrimeManager.SuspiciousActivity,SuspiciousActivity),

        };
        DispatchList = new List<Dispatch>
        {
            OfficerDown
            ,ShotsFiredAtAnOfficer
            ,AssaultingOfficer
            ,ThreateningOfficerWithFirearm
            ,TrespassingOnGovernmentProperty
            ,StealingAirVehicle
            ,ShotsFired
            ,CarryingWeapon
            ,CivilianDown
            ,CivilianShot
            ,CivilianInjury
            ,GrandTheftAuto
            ,SuspiciousActivity
            ,CriminalActivity
            ,Mugging
            ,TerroristActivity
            ,SuspiciousVehicle
            ,DrivingAtStolenVehicle
            ,ResistingArrest
            ,AttemptingSuicide
            ,FelonySpeeding
            ,PedHitAndRun
            ,VehicleHitAndRun
            ,RecklessDriving
            ,AnnounceStolenVehicle
            ,RequestAirSupport
            ,RequestMilitaryUnits
            ,RequestNOOSEUnits
            ,SuspectSpotted
            ,SuspectEvaded
            ,LostVisual
            ,ResumePatrol
            ,SuspectLost
            ,NoFurtherUnitsNeeded
            ,SuspectArrested
            ,SuspectWasted
            ,ChangedVehicles
            ,RequestBackup
            ,WeaponsFree
            ,LethalForceAuthorized
            ,RunningARedLight
        };

    }    
    private static void SetupDispatches()
    {

        RadioStart = new List<string>() { AudioBeeps.Radio_Start_1.FileName };
        RadioEnd = new List<string>() { AudioBeeps.Radio_End_1.FileName };
        AttentionAllUnits = new List<Dispatch.AudioSet>()
        {
            new Dispatch.AudioSet(new List<string>() { attention_all_units_gen.Attentionallunits.FileName},"attention all units"),
            new Dispatch.AudioSet(new List<string>() { attention_all_units_gen.Attentionallunits1.FileName },"attention all units"),
            new Dispatch.AudioSet(new List<string>() { attention_all_units_gen.Attentionallunits3.FileName },"attention all units"),
        };
        OfficersReport = new List<Dispatch.AudioSet>()
        {
            new Dispatch.AudioSet(new List<string>() { we_have.OfficersReport_1.FileName},"officers report"),
            new Dispatch.AudioSet(new List<string>() { we_have.OfficersReport_2.FileName },"officers report"),
            new Dispatch.AudioSet(new List<string>() { we_have.UnitsReport_1.FileName },"units report"),
        };
        CiviliansReport = new List<Dispatch.AudioSet>()
        {
            new Dispatch.AudioSet(new List<string>() { we_have.CitizensReport_1.FileName },"citizens report"),
            new Dispatch.AudioSet(new List<string>() { we_have.CitizensReport_2.FileName },"citizens report"),
            new Dispatch.AudioSet(new List<string>() { we_have.CitizensReport_3.FileName },"citizens report"),
            new Dispatch.AudioSet(new List<string>() { we_have.CitizensReport_4.FileName },"citizens report"),
        };
        LethalForce = new List<Dispatch.AudioSet>()
        {
            new Dispatch.AudioSet(new List<string>() { lethal_force.Useofdeadlyforceauthorized.FileName},"use of deadly force authorized"),
            new Dispatch.AudioSet(new List<string>() { lethal_force.Useofdeadlyforceisauthorized.FileName },"use of deadly force is authorized"),
            new Dispatch.AudioSet(new List<string>() { lethal_force.Useofdeadlyforceisauthorized1.FileName },"use of deadly force is authorized"),
            new Dispatch.AudioSet(new List<string>() { lethal_force.Useoflethalforceisauthorized.FileName },"use of lethal force is authorized"),
            new Dispatch.AudioSet(new List<string>() { lethal_force.Useofdeadlyforcepermitted1.FileName },"use of deadly force permitted"),
        };

        LicensePlateSet = new List<Dispatch.AudioSet>()
        {
            new Dispatch.AudioSet(new List<string>() { suspect_license_plate.SuspectLicensePlate.FileName},"suspect license plate"),
            new Dispatch.AudioSet(new List<string>() { suspect_license_plate.SuspectsLicensePlate01.FileName },"suspects license plate"),
            new Dispatch.AudioSet(new List<string>() { suspect_license_plate.SuspectsLicensePlate02.FileName },"suspects license plate"),
            new Dispatch.AudioSet(new List<string>() { suspect_license_plate.TargetLicensePlate.FileName },"target license plate"),
            new Dispatch.AudioSet(new List<string>() { suspect_license_plate.TargetsLicensePlate.FileName },"targets license plate"),
            new Dispatch.AudioSet(new List<string>() { suspect_license_plate.TargetVehicleLicensePlate.FileName },"target vehicle license plate"),
        };


        OfficerDown = new Dispatch()
        {
            Name = "Officer Down",
            IncludeAttentionAllUnits = true,
            ResultsInLethalForce = true,
            LocationDescription = LocationSpecificity.StreetAndZone,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { we_have.We_Have_1.FileName, crime_officer_down.AcriticalsituationOfficerdown.FileName },"we have a critical situation, officer down"),
                new Dispatch.AudioSet(new List<string>() { we_have.We_Have_1.FileName, crime_officer_down.AnofferdownpossiblyKIA.FileName },"we have an officer down, possibly KIA"),
                new Dispatch.AudioSet(new List<string>() { we_have.We_Have_1.FileName, crime_officer_down.Anofficerdown.FileName },"we have an officer down"),
                new Dispatch.AudioSet(new List<string>() { we_have.We_Have_2.FileName, crime_officer_down.Anofficerdownconditionunknown.FileName },"we have an officer down, condition unknown"),
            },
            SecondaryAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { dispatch_respond_code.AllunitsrespondCode99.FileName },"all units repond code-99"),
                new Dispatch.AudioSet(new List<string>() { dispatch_respond_code.AllunitsrespondCode99emergency.FileName },"all units repond code-99 emergency"),
                new Dispatch.AudioSet(new List<string>() { dispatch_respond_code.Code99allunitsrespond.FileName },"code-99 all units repond"),
                new Dispatch.AudioSet(new List<string>() { custom_wanted_level_line.Code99allavailableunitsconvergeonsuspect.FileName },"code-99 all available units converge on suspect"),
                new Dispatch.AudioSet(new List<string>() { custom_wanted_level_line.Wehavea1099allavailableunitsrespond.FileName },"we have a 10-99  all available units repond"),
                new Dispatch.AudioSet(new List<string>() { dispatch_respond_code.Code99allunitsrespond.FileName },"code-99 all units respond"),
                new Dispatch.AudioSet(new List<string>() { dispatch_respond_code.EmergencyallunitsrespondCode99.FileName },"emergency all units respond code-99"),
                new Dispatch.AudioSet(new List<string>() { escort_boss.Immediateassistancerequired.FileName },"immediate assistance required"),
            },
            MainMultiAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { we_have.We_Have_1.FileName, crime_officers_down.Multipleofficersdown.FileName },"we have multiple officers down"),
                new Dispatch.AudioSet(new List<string>() { we_have.We_Have_2.FileName, crime_officers_down.Severalofficersdown.FileName },"we have several officers down"),
            },
        };
        ShotsFiredAtAnOfficer = new Dispatch()
        {
            Name = "Shots Fired at an Officer",
            IncludeAttentionAllUnits = true,
            ResultsInLethalForce = true,
            LocationDescription = LocationSpecificity.StreetAndZone,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { crime_shots_fired_at_an_officer.Shotsfiredatanofficer.FileName },"shots fired at an officer"),
                new Dispatch.AudioSet(new List<string>() { crime_shots_fired_at_officer.Afirearmattackonanofficer.FileName },"a firearm attack on an officer"),
                new Dispatch.AudioSet(new List<string>() { crime_shots_fired_at_officer.Anofficershot.FileName },"a officer shot"),
                new Dispatch.AudioSet(new List<string>() { crime_shots_fired_at_officer.Anofficerunderfire.FileName },"a officer under fire"),
                new Dispatch.AudioSet(new List<string>() { crime_shots_fired_at_officer.Shotsfiredatanofficer.FileName },"a shots fired at an officer"),
            },
            SecondaryAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { dispatch_respond_code.AllunitsrespondCode99.FileName },"all units repond code-99"),
                new Dispatch.AudioSet(new List<string>() { dispatch_respond_code.AllunitsrespondCode99emergency.FileName },"all units repond code-99 emergency"),
                new Dispatch.AudioSet(new List<string>() { dispatch_respond_code.Code99allunitsrespond.FileName },"code-99 all units repond"),
                new Dispatch.AudioSet(new List<string>() { custom_wanted_level_line.Code99allavailableunitsconvergeonsuspect.FileName },"code-99 all available units converge on suspect"),
                new Dispatch.AudioSet(new List<string>() { custom_wanted_level_line.Wehavea1099allavailableunitsrespond.FileName },"we have a 10-99  all available units repond"),
                new Dispatch.AudioSet(new List<string>() { dispatch_respond_code.Code99allunitsrespond.FileName },"code-99 all units respond"),
                new Dispatch.AudioSet(new List<string>() { dispatch_respond_code.EmergencyallunitsrespondCode99.FileName },"emergency all units respond code-99"),
                new Dispatch.AudioSet(new List<string>() { escort_boss.Immediateassistancerequired.FileName },"immediate assistance required"),
            }
        };
        AssaultingOfficer = new Dispatch()
        {
            Name = "Assault on an Officer",
            LocationDescription = LocationSpecificity.Street,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { crime_assault_on_an_officer.Anassaultonanofficer.FileName },"an assault on an officer"),
                new Dispatch.AudioSet(new List<string>() { crime_assault_on_an_officer.Anofficerassault.FileName },"an officer assault"),
            },
        };
        ThreateningOfficerWithFirearm = new Dispatch()
        {
            Name = "Threatening an Officer with a Firearm",
            LocationDescription = LocationSpecificity.StreetAndZone,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { crime_suspect_threatening_an_officer_with_a_firearm.Asuspectthreateninganofficerwithafirearm.FileName },"a suspect threatening an officer with a firearm"),
            },
        };
        TrespassingOnGovernmentProperty = new Dispatch()
        {
            Name = "Trespassing on Government Property",
            ResultsInLethalForce = true,
            LocationDescription = LocationSpecificity.Zone,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { crime_trespassing_on_government_property.Trespassingongovernmentproperty.FileName },"trespassing on government property"),
            },
        };
        StealingAirVehicle = new Dispatch()
        {
            Name = "Stolen Air Vehicle",
            ResultsInLethalForce = true,
            IncludeDrivingVehicle = true,
            MarkVehicleAsStolen = true,
            LocationDescription = LocationSpecificity.Zone,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { crime_stolen_aircraft.Astolenaircraft.FileName},"a stolen aircraft"),
                new Dispatch.AudioSet(new List<string>() { crime_hijacked_aircraft.Ahijackedaircraft.FileName },"a hijacked aircraft"),
                new Dispatch.AudioSet(new List<string>() { crime_theft_of_an_aircraft.Theftofanaircraft.FileName },"theft of an aircraft"),
            },
        };
        ShotsFired = new Dispatch()
        {
            Name = "Shots Fired",
            LocationDescription = LocationSpecificity.StreetAndZone,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { crime_shooting.Afirearmssituationseveralshotsfired.FileName },"a firearms situation, several shots fired"),
                new Dispatch.AudioSet(new List<string>() { crime_shooting.Aweaponsincidentshotsfired.FileName },"a weapons incdient, shots fired"),
                new Dispatch.AudioSet(new List<string>() { crime_shoot_out.Ashootout.FileName },"a shoot-out"),
                new Dispatch.AudioSet(new List<string>() { crime_firearms_incident.AfirearmsincidentShotsfired.FileName },"a firearms incident, shots fired"),
                new Dispatch.AudioSet(new List<string>() { crime_firearms_incident.Anincidentinvolvingshotsfired.FileName },"an incident involving shots fired"),
                new Dispatch.AudioSet(new List<string>() { crime_firearms_incident.AweaponsincidentShotsfired.FileName },"a weapons incident, shots fired"),
            },
        };
        CarryingWeapon = new Dispatch()
        {
            Name = "Carrying Weapon",
            LocationDescription = LocationSpecificity.StreetAndZone,
            IncludeCarryingWeapon = true,
        };
        TerroristActivity = new Dispatch()
        {
            Name = "Terrorist Activity",
            LocationDescription = LocationSpecificity.StreetAndZone,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() {  crime_terrorist_activity.Possibleterroristactivity.FileName },"possible terrorist activity"),
                new Dispatch.AudioSet(new List<string>() {  crime_terrorist_activity.Possibleterroristactivity1.FileName},"possible terrorist activity"),
                new Dispatch.AudioSet(new List<string>() {  crime_terrorist_activity.Possibleterroristactivity2.FileName },"possible terrorist activity"),
                new Dispatch.AudioSet(new List<string>() {  crime_terrorist_activity.Terroristactivity.FileName },"terrorist activity"),
            },
        };
        CivilianDown = new Dispatch()
        {
            Name = "Civilian Down",
            LocationDescription = LocationSpecificity.StreetAndZone,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { crime_civilian_fatality.Acivilianfatality.FileName },"civilian fatality"),
                new Dispatch.AudioSet(new List<string>() { crime_civilian_down.Aciviliandown.FileName },"civilian down"),

                new Dispatch.AudioSet(new List<string>() { crime_1_87.A187.FileName },"a 1-87"),
                new Dispatch.AudioSet(new List<string>() { crime_1_87.Ahomicide.FileName },"a homicide"),
            },
        };
        CivilianShot = new Dispatch()
        {
            Name = "Civilian Shot",
            LocationDescription = LocationSpecificity.Street,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { crime_civillian_gsw.AcivilianGSW.FileName },"a civilian GSW"),
                new Dispatch.AudioSet(new List<string>() { crime_civillian_gsw.Acivilianshot.FileName },"a civilian shot"),
                new Dispatch.AudioSet(new List<string>() { crime_civillian_gsw.Agunshotwound.FileName },"a gunshot wound"),
            },
        };
        CivilianInjury = new Dispatch()
        {
            Name = "Civilian Injury",
            LocationDescription = LocationSpecificity.StreetAndZone,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { crime_injured_civilian.Aninjuredcivilian.FileName },"an injured civilian"),
                new Dispatch.AudioSet(new List<string>() { crime_civilian_needing_assistance.Acivilianinneedofassistance.FileName },"a civilian in need of assistance"),
                new Dispatch.AudioSet(new List<string>() { crime_civilian_needing_assistance.Acivilianrequiringassistance.FileName },"a civilian requiring assistance"),
                new Dispatch.AudioSet(new List<string>() { crime_assault_on_a_civilian.Anassaultonacivilian.FileName },"an assault on a civilian"),
            },
        };

        GrandTheftAuto = new Dispatch()
        {
            Name = "Grand Theft Auto",
            IncludeDrivingVehicle = true,
            MarkVehicleAsStolen = true,
            IncludeLicensePlate = true,
            IncludeCarryingWeapon = true,
            LocationDescription = LocationSpecificity.HeadingAndStreet,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { crime_grand_theft_auto.Agrandtheftauto.FileName },"a grand theft auto"),
                new Dispatch.AudioSet(new List<string>() { crime_grand_theft_auto.Agrandtheftautoinprogress.FileName },"a grand theft auto in progress"),
                new Dispatch.AudioSet(new List<string>() { crime_grand_theft_auto.AGTAinprogress.FileName },"a GTA in progress"),
                new Dispatch.AudioSet(new List<string>() { crime_grand_theft_auto.AGTAinprogress1.FileName },"a GTA in progress"),
            },
        };
        SuspiciousActivity = new Dispatch()
        {
            Name = "Suspicious Activity",
            LocationDescription = LocationSpecificity.StreetAndZone,
            IncludeCarryingWeapon = true,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { crime_suspicious_activity.Suspiciousactivity.FileName },"suspicious activity"),
                new Dispatch.AudioSet(new List<string>() { crime_theft.Apossibletheft.FileName },"a possible theft"),
            },
        };
        CriminalActivity = new Dispatch()
        {
            Name = "Criminal Activity",
            LocationDescription = LocationSpecificity.Street,
            IncludeCarryingWeapon = true,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { crime_criminal_activity.Criminalactivity.FileName },"criminal activity"),
                new Dispatch.AudioSet(new List<string>() { crime_criminal_activity.Illegalactivity.FileName },"illegal activity"),
                new Dispatch.AudioSet(new List<string>() { crime_criminal_activity.Prohibitedactivity.FileName },"prohibited activity"),
            },
        };
        Mugging = new Dispatch()
        {
            Name = "Mugging",
            LocationDescription = LocationSpecificity.Street,
            IncludeCarryingWeapon = true,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { crime_mugging.Apossiblemugging.FileName },"a possible mugging"),
            },
        };
        TerroristActivity = new Dispatch()
        {
            Name = "Terrorist Activity",
            LocationDescription = LocationSpecificity.Street,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { crime_terrorist_activity.Possibleterroristactivity.FileName },"possible terrorist activity in progress"),
                new Dispatch.AudioSet(new List<string>() { crime_terrorist_activity.Possibleterroristactivity1.FileName },"possible terrorist activity in progress"),
                new Dispatch.AudioSet(new List<string>() { crime_terrorist_activity.Possibleterroristactivity2.FileName },"possible terrorist activity in progress"),
                new Dispatch.AudioSet(new List<string>() { crime_terrorist_activity.Terroristactivity.FileName },"terrorist activity"),
            },
        };
        SuspiciousVehicle = new Dispatch()
        {
            Name = "Suspicious Vehicle",
            IncludeDrivingVehicle = true,
            LocationDescription = LocationSpecificity.StreetAndZone,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { crime_suspicious_vehicle.Asuspiciousvehicle.FileName },"a suspicious vehicle"),
            },
        };

        DrivingAtStolenVehicle = new Dispatch()
        {
            Name = "Driving a Stolen Vehicle",
            IncludeDrivingVehicle = true,
            LocationDescription = LocationSpecificity.HeadingAndStreet,
            IncludeDrivingSpeed = true,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { crime_person_in_a_stolen_car.Apersoninastolencar.FileName},"a person in a stolen car"),
                new Dispatch.AudioSet(new List<string>() { crime_person_in_a_stolen_vehicle.Apersoninastolenvehicle.FileName },"a person in a stolen vehicle"),
            },
        };
        ResistingArrest = new Dispatch()
        {
            Name = "Resisting Arrest",
            LocationDescription = LocationSpecificity.Zone,
            IncludeCarryingWeapon = true,
            CanBeReportedMultipleTimes = false,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { crime_person_resisting_arrest.Apersonresistingarrest.FileName },"a person resisting arrest"),
                new Dispatch.AudioSet(new List<string>() { crime_suspect_resisting_arrest.Asuspectresistingarrest.FileName },"a suspect resisiting arrest"),

                new Dispatch.AudioSet(new List<string>() { crime_1_48_resist_arrest.Acriminalresistingarrest.FileName },"a criminal resisiting arrest"),
                new Dispatch.AudioSet(new List<string>() { crime_1_48_resist_arrest.Acriminalresistingarrest1.FileName },"a criminal resisiting arrest"),
                new Dispatch.AudioSet(new List<string>() { crime_1_48_resist_arrest.Asuspectfleeingacrimescene.FileName },"a suspect fleeing a crime scene"),
                new Dispatch.AudioSet(new List<string>() { crime_1_48_resist_arrest.Asuspectontherun.FileName },"a suspect on the run"),
            }
        };
        AttemptingSuicide = new Dispatch()
        {
            Name = "Suicide Attempt",
            LocationDescription = LocationSpecificity.Street,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { crime_9_14a_attempted_suicide.Apossibleattemptedsuicide.FileName },"a possible attempted suicide"),
                new Dispatch.AudioSet(new List<string>() { crime_9_14a_attempted_suicide.Anattemptedsuicide.FileName },"an attempted suicide")
            }
        };
        FelonySpeeding = new Dispatch()
        {
            Name = "Felony Speeding",
            IncludeDrivingVehicle = true,
            VehicleIncludesIn = true,
            IncludeDrivingSpeed = true,
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { crime_speeding_felony.Aspeedingfelony.FileName },"a speeding felony"),
                new Dispatch.AudioSet(new List<string>() { crime_5_10.A510.FileName,crime_5_10.Speedingvehicle.FileName },"a 5-10, speeding vehicle"),
            },
        };
        PedHitAndRun = new Dispatch()
        {
            Name = "Pedestrian Hit-and-Run",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { crime_ped_struck_by_veh.Apedestrianstruck.FileName},"a pedestrian struck"),
                new Dispatch.AudioSet(new List<string>() { crime_ped_struck_by_veh.Apedestrianstruck1.FileName },"a pedestrian struck"),
                new Dispatch.AudioSet(new List<string>() { crime_ped_struck_by_veh.Apedestrianstruckbyavehicle.FileName },"a pedestrian struck by a vehicle"),
                new Dispatch.AudioSet(new List<string>() { crime_ped_struck_by_veh.Apedestrianstruckbyavehicle1.FileName },"a pedestrian struck by a vehicle"),
            },
        };
        VehicleHitAndRun = new Dispatch()
        {
            Name = "Motor Vehicle Accident",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { crime_motor_vehicle_accident.Amotorvehicleaccident.FileName},"a motor vehicle accident"),
                new Dispatch.AudioSet(new List<string>() { crime_motor_vehicle_accident.AnAEincident.FileName },"an A&E incident"),
                new Dispatch.AudioSet(new List<string>() { crime_motor_vehicle_accident.AseriousMVA.FileName },"a serious MVA"),
            },
        };
        RunningARedLight = new Dispatch()
        {
            Name = "Running a Red Light",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { crime_person_running_a_red_light.Apersonrunningaredlight.FileName},"a person running a red light"),
            },
        };
        RecklessDriving = new Dispatch()
        {
            Name = "Reckless Driving",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { crime_reckless_driver.Arecklessdriver.FileName},"a reckless driver"),
                new Dispatch.AudioSet(new List<string>() { crime_5_05.A505.FileName,crime_5_05.Adriveroutofcontrol.FileName },"a 505, a driver out of control"),
            },
        };

        AnnounceStolenVehicle = new Dispatch()
        {
            Name = "Stolen Vehicle Reported",
            IsStatus = true,
            IncludeDrivingVehicle = true,
            CanAlwaysBeInterrupted = true,
            MarkVehicleAsStolen = true,
            IncludeLicensePlate = true,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() {crime_stolen_vehicle.Apossiblestolenvehicle.FileName},"a possible stolen vehicle"),
            },
        };
        RequestAirSupport = new Dispatch()
        {
            Name = "Air Support Requested",
            IsStatus = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.Zone,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { officer_requests_air_support.Officersrequestinghelicoptersupport.FileName },"officers requesting helicopter support"),
                new Dispatch.AudioSet(new List<string>() { officer_requests_air_support.Code99unitsrequestimmediateairsupport.FileName },"code-99 units request immediate air support"),
                new Dispatch.AudioSet(new List<string>() { officer_requests_air_support.Officersrequireaerialsupport.FileName },"officers require aerial support"),
                new Dispatch.AudioSet(new List<string>() { officer_requests_air_support.Officersrequireaerialsupport1.FileName },"officers require aerial support"),
                new Dispatch.AudioSet(new List<string>() { officer_requests_air_support.Officersrequireairsupport.FileName },"officers require air support"),
                new Dispatch.AudioSet(new List<string>() { officer_requests_air_support.Unitsrequestaerialsupport.FileName },"units request aerial support"),
                new Dispatch.AudioSet(new List<string>() { officer_requests_air_support.Unitsrequestingairsupport.FileName },"units requesting air support"),
                new Dispatch.AudioSet(new List<string>() { officer_requests_air_support.Unitsrequestinghelicoptersupport.FileName },"units requesting helicopter support"),
            },
        };
        RequestMilitaryUnits = new Dispatch()
        {
            IncludeAttentionAllUnits = true,
            Name = "Military Units Requested",
            IsStatus = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.Zone,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { custom_wanted_level_line.Code13militaryunitsrequested.FileName },"code-13 military units requested"),
            },
        };



        RequestNOOSEUnits = new Dispatch()
        {
            IncludeAttentionAllUnits = true,
            Name = "NOOSE Units Requested",
            IsStatus = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.Zone,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { dispatch_units_full.DispatchingSWATunitsfrompoliceheadquarters.FileName },"dispatching swat units from police headquarters"),
                new Dispatch.AudioSet(new List<string>() { dispatch_units_full.DispatchingSWATunitsfrompoliceheadquarters1.FileName },"dispatching swat units from police headquarters"),
            },
        };

        SuspectSpotted = new Dispatch()
        {
            Name = "Suspect Spotted",
            IsStatus = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.HeadingAndStreet,
            IncludeDrivingVehicle = true,
        };
        WantedSuspectSpotted = new Dispatch()
        {
            Name = "Wanted Suspect Spotted",
            IsStatus = true,
            IncludeReportedBy = true,
            IncludeRapSheet = true,
            Priority = 10,
            LocationDescription = LocationSpecificity.HeadingAndStreet,
            IncludeDrivingVehicle = true,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { crime_wanted_felon_on_the_loose.Awantedfelonontheloose.FileName },"a wanted felon on the loose"),
            },
        };
        SuspectEvaded = new Dispatch()
        {
            Name = "Suspect Evaded",
            IsStatus = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.Zone,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { suspect_eluded_pt_1.SuspectEvadedPursuingOfficiers.FileName },"suspect evaded pursuing officers"),
                new Dispatch.AudioSet(new List<string>() { suspect_eluded_pt_1.OfficiersHaveLostVisualOnSuspect.FileName },"officers have lost visual on suspect"),
            },
        };
        LostVisual = new Dispatch()
        {
            Name = "Lost Visual",
            IsStatus = true,
            IncludeReportedBy = false,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { suspect_eluded_pt_2.AllUnitsStayInTheArea.FileName },"all units stay in the area"),
                new Dispatch.AudioSet(new List<string>() { suspect_eluded_pt_2.AllUnitsRemainOnAlert.FileName },"all units remain on alert"),

                new Dispatch.AudioSet(new List<string>() { suspect_eluded_pt_2.AllUnitsStandby.FileName },"all units standby"),
                new Dispatch.AudioSet(new List<string>() { suspect_eluded_pt_2.AllUnitsStayInTheArea.FileName },"all units stay in the area"),
                new Dispatch.AudioSet(new List<string>() { suspect_eluded_pt_2.AllUnitsRemainOnAlert.FileName },"all un its remain on alert"),
            },
        };
        ResumePatrol = new Dispatch()
        {
            Name = "Resume Patrol",
            IsStatus = true,
            IncludeReportedBy = false,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { officer_begin_patrol.Beginpatrol.FileName },"begin patrol"),
                new Dispatch.AudioSet(new List<string>() { officer_begin_patrol.Beginbeat.FileName },"begin beat"),

                new Dispatch.AudioSet(new List<string>() { officer_begin_patrol.Assigntopatrol.FileName },"assign to patrol"),
                new Dispatch.AudioSet(new List<string>() { officer_begin_patrol.Proceedtopatrolarea.FileName },"proceed to patrol area"),
                new Dispatch.AudioSet(new List<string>() { officer_begin_patrol.Proceedwithpatrol.FileName },"proceed with patrol"),
            },
        };
        SuspectLost = new Dispatch()
        {
            Name = "Suspect Lost",
            IsStatus = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.Zone,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { attempt_to_find.AllunitsATonsuspects20.FileName },"all units ATL on suspects 20"),
                new Dispatch.AudioSet(new List<string>() { attempt_to_find.Allunitsattempttoreacquire.FileName },"all units attempt to reacquire"),
                new Dispatch.AudioSet(new List<string>() { attempt_to_find.Allunitsattempttoreacquirevisual.FileName },"all units attempt to reacquire visual"),
                new Dispatch.AudioSet(new List<string>() { attempt_to_find.RemainintheareaATL20onsuspect.FileName },"remain in the area, ATL-20 on suspect"),
                new Dispatch.AudioSet(new List<string>() { attempt_to_find.RemainintheareaATL20onsuspect1.FileName },"remain in the area, ATL-20 on suspect"),
            },
        };
        NoFurtherUnitsNeeded = new Dispatch()
        {
            Name = "Officers On-Site, Code 4-ADAM",
            IsStatus = true,
            IncludeReportedBy = false,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { officers_on_scene.Officersareatthescene.FileName },"officers are at the scene"),
                new Dispatch.AudioSet(new List<string>() { officers_on_scene.Officersarrivedonscene.FileName },"offices have arrived on scene"),
                new Dispatch.AudioSet(new List<string>() { officers_on_scene.Officershavearrived.FileName },"officers have arrived"),
                new Dispatch.AudioSet(new List<string>() { officers_on_scene.Officersonscene.FileName },"officers on scene"),
                new Dispatch.AudioSet(new List<string>() { officers_on_scene.Officersonsite.FileName },"officers on site"),
            },
            SecondaryAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { no_further_units.Noadditionalofficersneeded.FileName },"no additional officers needed"),
                new Dispatch.AudioSet(new List<string>() { no_further_units.Noadditionalofficersneeded1.FileName },"no additional officers needed"),
                new Dispatch.AudioSet(new List<string>() { no_further_units.Nofurtherunitsrequired.FileName },"no further units required"),
                new Dispatch.AudioSet(new List<string>() { no_further_units.WereCode4Adam.FileName },"we're code-4 adam"),
                new Dispatch.AudioSet(new List<string>() { no_further_units.Code4Adamnoadditionalsupportneeded.FileName },"code-4 adam no additional support needed"),
            },
        };
        SuspectArrested = new Dispatch()
        {
            Name = "Suspect Arrested",
            IsStatus = true,
            IncludeReportedBy = false,
            CanAlwaysInterrupt = true,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { crook_arrested.Officershaveapprehendedsuspect.FileName },"officers have apprehended suspect"),
                new Dispatch.AudioSet(new List<string>() { crook_arrested.Officershaveapprehendedsuspect1.FileName },"officers have apprehended suspect"),
            },
        };
        SuspectWasted = new Dispatch()
        {
            Name = "Suspect Wasted",
            IsStatus = true,
            IncludeReportedBy = false,
            CanAlwaysInterrupt = true,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { crook_killed.Criminaldown.FileName },"criminal down"),
                new Dispatch.AudioSet(new List<string>() { crook_killed.Suspectdown.FileName },"suspect down"),
                new Dispatch.AudioSet(new List<string>() { crook_killed.Suspectneutralized.FileName },"suspect neutralized"),
                new Dispatch.AudioSet(new List<string>() { crook_killed.Suspectdownmedicalexaminerenroute.FileName },"suspect down, medical examiner in route"),
                new Dispatch.AudioSet(new List<string>() { crook_killed.Suspectdowncoronerenroute.FileName },"suspect down, coroner in route"),
                new Dispatch.AudioSet(new List<string>() { crook_killed.Officershavepacifiedsuspect.FileName },"officers have pacified suspect"),
             },
        };
        ChangedVehicles = new Dispatch()
        {
            Name = "Suspect Changed Vehicle",
            IsStatus = true,
            IncludeDrivingVehicle = true,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { "" },""),
             },
        };
        RequestBackup = new Dispatch()
        {
            IncludeAttentionAllUnits = true,
            Name = "Backup Required",
            IsStatus = true,
            IncludeReportedBy = false,
            CanAlwaysInterrupt = true,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { assistance_required.Assistanceneeded.FileName },"assistance needed"),
                new Dispatch.AudioSet(new List<string>() { assistance_required.Assistancerequired.FileName },"Assistance required"),
                new Dispatch.AudioSet(new List<string>() { assistance_required.Backupneeded.FileName },"backup needed"),
                new Dispatch.AudioSet(new List<string>() { assistance_required.Backuprequired.FileName },"backup required"),
                new Dispatch.AudioSet(new List<string>() { assistance_required.Officersneeded.FileName },"officers needed"),
                new Dispatch.AudioSet(new List<string>() { assistance_required.Officersrequired.FileName },"officers required"),
             },
            SecondaryAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { dispatch_respond_code.UnitsrespondCode3.FileName },"units respond code-3"),
             },
            PreambleAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { s_m_y_cop_white_full_01.RequestingBackup.FileName },"requesting backup"),
                new Dispatch.AudioSet(new List<string>() { s_m_y_cop_white_full_01.RequestingBackupWeNeedBackup.FileName },"requesting back, we need backup"),
                new Dispatch.AudioSet(new List<string>() { s_m_y_cop_white_full_01.WeNeedBackupNow.FileName },"we need backup now"),
                new Dispatch.AudioSet(new List<string>() { s_m_y_cop_white_full_02.MikeOscarSamInHotNeedOfBackup.FileName },"MOS in hot need of backup"),
                new Dispatch.AudioSet(new List<string>() { s_m_y_cop_white_full_02.MikeOScarSamRequestingBackup.FileName },"MOS requesting backup"),
                new Dispatch.AudioSet(new List<string>() { s_m_y_cop_white_mini_02.INeedSomeSeriousBackupHere.FileName },"i need some serious backup here"),
                new Dispatch.AudioSet(new List<string>() { s_m_y_cop_white_mini_03.OfficerInNeedofSomeBackupHere.FileName },"officer in need of some backup here"),
             },
        };
        WeaponsFree = new Dispatch()
        {
            IncludeAttentionAllUnits = true,
            Name = "Weapons Free",
            IsStatus = true,
            IncludeReportedBy = false,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { custom_wanted_level_line.Suspectisarmedanddangerousweaponsfree.FileName },"suspect is armed and dangerous, weapons free"),
             },
        };
        LethalForceAuthorized = new Dispatch()
        {
            IncludeAttentionAllUnits = true,
            Name = "Lethal Force Authorized",
            IsStatus = true,
            IncludeReportedBy = false,
            ResultsInLethalForce = true,
        };

    }
    public static void AbortAllAudio()
    {
        DispatchQueue.Clear();
        if (IsAudioPlaying)
        {
            CancelAudio = true;
            outputDevice.Stop();
        }
        DispatchQueue.Clear();
        if (IsAudioPlaying)
        {
            CancelAudio = true;
            outputDevice.Stop();
        }
        DispatchQueue.Clear();

        RemoveAllNotifications();
    }
    private class Dispatch
    {
        private uint GameTimeLastPlayed;
        public string Name { get; set; } = "Unknown";
        public bool IncludeAttentionAllUnits { get; set; } = false;
        public bool IsStatus { get; set; } = false;
        public bool IncludeReportedBy { get; set; } = true;
        public string NotificationSubtitle
        {
            get
            {
                return Name;
            }
        }
        public string NotificationTitle { get; set; } = "Police Scanner";
        public string NotificationText
        {
            get
            {
                return Name;
            }
        }
        public List<AudioSet> MainAudioSet { get; set; } = new List<AudioSet>();
        public List<AudioSet> SecondaryAudioSet { get; set; } = new List<AudioSet>();
        public List<AudioSet> MainMultiAudioSet { get; set; } = new List<AudioSet>();
        public List<AudioSet> PreambleAudioSet { get; set; } = new List<AudioSet>();   
        public bool MarkVehicleAsStolen { get; set; } = false;
        public bool IncludeDrivingVehicle { get; set; } = false;
        public bool VehicleIncludesIn { get; set; } = false;
        public bool IncludeCarryingWeapon { get; set; } = false;
        public bool IncludeDrivingSpeed { get; set; } = false;
        public bool IncludeLicensePlate { get; set; } = false;
        public bool IncludeRapSheet { get; set; } = false;
        public bool ReportCharctersPosition { get; set; } = true;
        public bool CanBeReportedMultipleTimes { get; set; } = true;
        public int TimesPlayed { get; set; } = 0;
        public int Priority { get; set; } = 99;
        public int PriorityGroup { get; set; } = 99;
        public bool ResultsInLethalForce { get; set; } = false;
        public bool ResultsInStolenCarSpotted { get; set; } = false;
        public bool IsTrafficViolation { get; set; } = false;
        public bool IsAmbient { get; set; } = false;
        public int ResultingWantedLevel { get; set; }
        public bool CanAlwaysBeInterrupted { get; set; } = false;
        public bool CanAlwaysInterrupt { get; set; } = false;

        public PoliceScannerCallIn LatestInformation { get; set; } = new PoliceScannerCallIn();
        public bool HasBeenPlayedThisWanted { get; set; } = false;
        public bool HasRecentlyBeenPlayed
        {
            get
            {
                uint TimeBetween = 25000;
                if (TimesPlayed > 0)
                    TimeBetween = 60000;

                if (Game.GameTime - GameTimeLastPlayed <= TimeBetween)
                    return true;
                else
                    return false;
            }
        }
        public bool HasPreamble
        {
            get
            {
                if (PreambleAudioSet.Any())
                    return true;
                else
                    return false;
            }
        }
        public LocationSpecificity LocationDescription { get; set; } = LocationSpecificity.Nothing;
        public Dispatch()
        {

        }
        public class AudioSet
        {
            public AudioSet(List<string> sounds, string subtitles)
            {
                Sounds = sounds;
                Subtitles = subtitles;
            }
            public List<string> Sounds { get; set; }
            public string Subtitles { get; set; }

        }
        public void SetPlayed()
        {
            GameTimeLastPlayed = Game.GameTime;
            HasBeenPlayedThisWanted = true;
            TimesPlayed++;
        }
    }
    private class DispatchEvent
    {
        public DispatchEvent()
        {

        }
        public List<string> SoundsToPlay { get; set; } = new List<string>();
        public string Subtitles { get; set; }
        public bool CanBeInterrupted { get; set; } = true;
        public bool CanInterrupt { get; set; } = true;
        public Vector3 PositionToReport { get; set; }
        public string NotificationTitle { get; set; } = "Police Scanner";
        public string NotificationSubtitle { get; set; } = "Status";
        public string NotificationText { get; set; } = "~b~Scanner Audio";
        public int Priority { get; set; } = 99;
    }
    private class CrimeDispatch
    {
        public CrimeDispatch()
        {

        }
        public CrimeDispatch(Crime crimeIdentified, Dispatch dispatchToPlay)
        {
            CrimeIdentified = crimeIdentified;
            DispatchToPlay = dispatchToPlay;
        }
        public Crime CrimeIdentified { get; set; }
        public Dispatch DispatchToPlay { get; set; }
    }
    private static class VehicleScanner
    {
        private static List<LetterLookup> LettersAndNumbersLookup = new List<LetterLookup>();
        private static List<ColorLookup> ColorLookups = new List<ColorLookup>();
        private static List<VehicleModelLookup> VehicleModelLookups = new List<VehicleModelLookup>();
        private static List<VehicleClassLookup> VehicleClassLookups = new List<VehicleClassLookup>();
        private static List<VehicleMakeLookup> VehicleMakeLookups = new List<VehicleMakeLookup>();
        public static void Initialize()
        {
            SetupLists();
        }
        public static void Dispose()
        {

        }
        private static void SetupLists()
        {
            LettersAndNumbersLookup = new List<LetterLookup>()
        {
            new LetterLookup('A', lp_letters_high.Adam.FileName),
            new LetterLookup('B', lp_letters_high.Boy.FileName),
            new LetterLookup('C', lp_letters_high.Charles.FileName),
            new LetterLookup('D', lp_letters_high.David.FileName),
            new LetterLookup('E', lp_letters_high.Edward.FileName),
            new LetterLookup('F', lp_letters_high.Frank.FileName),
            new LetterLookup('G', lp_letters_high.George.FileName),
            new LetterLookup('H', lp_letters_high.Henry.FileName),
            new LetterLookup('I', lp_letters_high.Ita.FileName),
            new LetterLookup('J', lp_letters_high.John.FileName),
            new LetterLookup('K', lp_letters_high.King.FileName),
            new LetterLookup('L', lp_letters_high.Lincoln.FileName),
            new LetterLookup('M', lp_letters_high.Mary.FileName),
            new LetterLookup('N', lp_letters_high.Nora.FileName),
            new LetterLookup('O', lp_letters_high.Ocean.FileName),
            new LetterLookup('P', lp_letters_high.Paul.FileName),
            new LetterLookup('Q', lp_letters_high.Queen.FileName),
            new LetterLookup('R', lp_letters_high.Robert.FileName),
            new LetterLookup('S', lp_letters_high.Sam.FileName),
            new LetterLookup('T', lp_letters_high.Tom.FileName),
            new LetterLookup('U', lp_letters_high.Union.FileName),
            new LetterLookup('V', lp_letters_high.Victor.FileName),
            new LetterLookup('W', lp_letters_high.William.FileName),
            new LetterLookup('X', lp_letters_high.XRay.FileName),
            new LetterLookup('Y', lp_letters_high.Young.FileName),
            new LetterLookup('Z', lp_letters_high.Zebra.FileName),
            new LetterLookup('A', lp_letters_high.Adam1.FileName),
            new LetterLookup('B', lp_letters_high.Boy1.FileName),
            new LetterLookup('C', lp_letters_high.Charles1.FileName),
            new LetterLookup('E', lp_letters_high.Edward1.FileName),
            new LetterLookup('F', lp_letters_high.Frank1.FileName),
            new LetterLookup('G', lp_letters_high.George1.FileName),
            new LetterLookup('H', lp_letters_high.Henry1.FileName),
            new LetterLookup('I', lp_letters_high.Ita1.FileName),
            new LetterLookup('J', lp_letters_high.John1.FileName),
            new LetterLookup('K', lp_letters_high.King1.FileName),
            new LetterLookup('L', lp_letters_high.Lincoln1.FileName),
            new LetterLookup('M', lp_letters_high.Mary1.FileName),
            new LetterLookup('N', lp_letters_high.Nora1.FileName),
            new LetterLookup('O', lp_letters_high.Ocean1.FileName),
            new LetterLookup('P', lp_letters_high.Paul1.FileName),
            new LetterLookup('Q', lp_letters_high.Queen1.FileName),
            new LetterLookup('R', lp_letters_high.Robert1.FileName),
            new LetterLookup('S', lp_letters_high.Sam1.FileName),
            new LetterLookup('T', lp_letters_high.Tom1.FileName),
            new LetterLookup('U', lp_letters_high.Union1.FileName),
            new LetterLookup('V', lp_letters_high.Victor1.FileName),
            new LetterLookup('W', lp_letters_high.William1.FileName),
            new LetterLookup('X', lp_letters_high.XRay1.FileName),
            new LetterLookup('Y', lp_letters_high.Young1.FileName),
            new LetterLookup('Z', lp_letters_high.Zebra1.FileName),
            new LetterLookup('1', lp_numbers.One.FileName),
            new LetterLookup('2', lp_numbers.Two.FileName),
            new LetterLookup('3', lp_numbers.Three.FileName),
            new LetterLookup('4', lp_numbers.Four.FileName),
            new LetterLookup('5', lp_numbers.Five.FileName),
            new LetterLookup('6', lp_numbers.Six.FileName),
            new LetterLookup('7', lp_numbers.Seven.FileName),
            new LetterLookup('8', lp_numbers.Eight.FileName),
            new LetterLookup('9', lp_numbers.Nine.FileName),
            new LetterLookup('0', lp_numbers.Zero.FileName),
            new LetterLookup('1', lp_numbers.One1.FileName),
            new LetterLookup('2', lp_numbers.Two1.FileName),
            new LetterLookup('3', lp_numbers.Three1.FileName),
            new LetterLookup('4', lp_numbers.Four1.FileName),
            new LetterLookup('5', lp_numbers.Five1.FileName),
            new LetterLookup('6', lp_numbers.Six1.FileName),
            new LetterLookup('7', lp_numbers.Seven1.FileName),
            new LetterLookup('8', lp_numbers.Eight1.FileName),
            new LetterLookup('9', lp_numbers.Niner.FileName),
            new LetterLookup('0', lp_numbers.Zero1.FileName),
            new LetterLookup('1', lp_numbers.One2.FileName),
            new LetterLookup('2', lp_numbers.Two2.FileName),
            new LetterLookup('3', lp_numbers.Three2.FileName),
            new LetterLookup('4', lp_numbers.Four2.FileName),
            new LetterLookup('5', lp_numbers.Five2.FileName),
            new LetterLookup('6', lp_numbers.Six2.FileName),
            new LetterLookup('7', lp_numbers.Seven2.FileName),
            new LetterLookup('8', lp_numbers.Eight2.FileName),
            new LetterLookup('9', lp_numbers.Niner2.FileName),
            new LetterLookup('0', lp_numbers.Zero2.FileName),
        };
            ColorLookups = new List<ColorLookup>()
        {
            new ColorLookup(colour.COLORRED01.FileName, Color.Red),
            new ColorLookup(colour.COLORAQUA01.FileName, Color.Aqua),
            new ColorLookup(colour.COLORBEIGE01.FileName, Color.Beige),
            new ColorLookup(colour.COLORBLACK01.FileName, Color.Black),
            new ColorLookup(colour.COLORBLUE01.FileName, Color.Blue),
            new ColorLookup(colour.COLORBROWN01.FileName, Color.Brown),
            new ColorLookup(colour.COLORDARKBLUE01.FileName, Color.DarkBlue),
            new ColorLookup(colour.COLORDARKGREEN01.FileName, Color.DarkGreen),
            new ColorLookup(colour.COLORDARKGREY01.FileName, Color.DarkGray),
            new ColorLookup(colour.COLORDARKORANGE01.FileName, Color.DarkOrange),
            new ColorLookup(colour.COLORDARKRED01.FileName, Color.DarkRed),
            new ColorLookup(colour.COLORGOLD01.FileName, Color.Gold),
            new ColorLookup(colour.COLORGREEN01.FileName, Color.Green),
            new ColorLookup(colour.COLORGREY01.FileName, Color.Gray),
            new ColorLookup(colour.COLORGREY02.FileName, Color.Gray),
            new ColorLookup(colour.COLORLIGHTBLUE01.FileName, Color.LightBlue),
            new ColorLookup(colour.COLORMAROON01.FileName, Color.Maroon),
            new ColorLookup(colour.COLORORANGE01.FileName, Color.Orange),
            new ColorLookup(colour.COLORPINK01.FileName, Color.Pink),
            new ColorLookup(colour.COLORPURPLE01.FileName, Color.Purple),
            new ColorLookup(colour.COLORRED01.FileName, Color.Red),
            new ColorLookup(colour.COLORSILVER01.FileName, Color.Silver),
            new ColorLookup(colour.COLORWHITE01.FileName, Color.White),
            new ColorLookup(colour.COLORYELLOW01.FileName, Color.Yellow),
         };
            VehicleModelLookups = new List<VehicleModelLookup>
        {
            new VehicleModelLookup("airtug", 0x5D0AAC8F, model.AIRTUG01.FileName),
            new VehicleModelLookup("akuma", 0x63ABADE7, model.AKUMA01.FileName),
            new VehicleModelLookup("asea", 0x94204D89, model.ASEA01.FileName),
            new VehicleModelLookup("asea2", 0x9441D8D5, model.ASEA01.FileName),
            new VehicleModelLookup("asterope", 0x8E9254FB, model.ASTEROPE01.FileName),
            new VehicleModelLookup("bagger", 0x806B9CC3, model.BAGGER01.FileName),
            new VehicleModelLookup("baller", 0xCFCA3668, model.BALLER01.FileName),
            new VehicleModelLookup("baller2", 0x8852855, model.BALLER01.FileName),
            new VehicleModelLookup("baller3", 0x6FF0F727, model.BALLER01.FileName),
            new VehicleModelLookup("baller4", 0x25CBE2E2, model.BALLER01.FileName),
            new VehicleModelLookup("baller5", 0x1C09CF5E, model.BALLER01.FileName),
            new VehicleModelLookup("baller6", 0x27B4E6B0, model.BALLER01.FileName),
            new VehicleModelLookup("banshee", 0xC1E908D2, model.BANSHEE01.FileName),
            new VehicleModelLookup("banshee2", 0x25C5AF13, model.BANSHEE01.FileName),
            new VehicleModelLookup("barracks", 0xCEEA3F4B, model.BARRACKS01.FileName),
            new VehicleModelLookup("barracks2", 0x4008EABB, model.BARRACKS01.FileName),
            new VehicleModelLookup("barracks3", 0x2592B5CF, model.BARRACKS01.FileName),
            new VehicleModelLookup("bati", 0xF9300CC5, model.BATI01.FileName),
            new VehicleModelLookup("bati2", 0xCADD5D2D, model.BATI01.FileName),
            new VehicleModelLookup("benson", 0x7A61B330, model.BENSON01.FileName),
            new VehicleModelLookup("bfinjection", 0x432AA566, model.BFINJECTION01.FileName),
            new VehicleModelLookup("biff", 0x32B91AE8, model.BIFF01.FileName),
            new VehicleModelLookup("bison", 0xFEFD644F, model.BISON01.FileName),
            new VehicleModelLookup("bison2", 0x7B8297C5, model.BISON01.FileName),
            new VehicleModelLookup("bison3", 0x67B3F020, model.BISON01.FileName),
            new VehicleModelLookup("bjxl", 0x32B29A4B, model.BJXL01.FileName),
            new VehicleModelLookup("blazer", 0x8125BCF9, model.BLAZER01.FileName),
            new VehicleModelLookup("blazer2", 0xFD231729, model.BLAZER01.FileName),
            new VehicleModelLookup("blazer3", 0xB44F0582, model.BLAZER01.FileName),
            new VehicleModelLookup("blazer4", 0xE5BA6858, model.BLAZER01.FileName),
            new VehicleModelLookup("blazer5", 0xA1355F67, model.BLAZER01.FileName),
            new VehicleModelLookup("blista", 0xEB70965F, model.BLISTA01.FileName),
            new VehicleModelLookup("blista2", 0x3DEE5EDA, model.BLISTA01.FileName),
            new VehicleModelLookup("blista3", 0xDCBC1C3B, model.BLISTA01.FileName),
            new VehicleModelLookup("bobcatxl", 0x3FC5D440, model.BOBCATXL01.FileName),
            new VehicleModelLookup("bodhi2", 0xAA699BB6, model.BODHI01.FileName),
            new VehicleModelLookup("boxville", 0x898ECCEA, model.BOXVILLE01.FileName),
            new VehicleModelLookup("boxville2", 0xF21B33BE, model.BOXVILLE01.FileName),
            new VehicleModelLookup("boxville3", 0x07405E08, model.BOXVILLE01.FileName),
            new VehicleModelLookup("boxville4", 0x1A79847A, model.BOXVILLE01.FileName),
            new VehicleModelLookup("boxville5", 0x28AD20E1, model.BOXVILLE01.FileName),
            new VehicleModelLookup("buccaneer", 0xD756460C, model.BUCCANEER01.FileName),
            new VehicleModelLookup("buccaneer2", 0xC397F748, model.BUCCANEER01.FileName),
            new VehicleModelLookup("buffalo", 0xEDD516C6, model.BUFFALO01.FileName),
            new VehicleModelLookup("buffalo2", 0x2BEC3CBE, model.BUFFALO01.FileName),
            new VehicleModelLookup("buffalo3", 0xE2C013E, model.BUFFALO01.FileName),
            new VehicleModelLookup("bullet", 0x9AE6DDA1, model.BULLET01.FileName),
            new VehicleModelLookup("burrito", 0xAFBB2CA4, model.BURRITO01.FileName),
            new VehicleModelLookup("burrito2", 0xC9E8FF76, model.BURRITO01.FileName),
            new VehicleModelLookup("burrito3", 0x98171BD3, model.BURRITO01.FileName),
            new VehicleModelLookup("burrito4", 0x353B561D, model.BURRITO01.FileName),
            new VehicleModelLookup("burrito5", 0x437CF2A0, model.BURRITO01.FileName),
            new VehicleModelLookup("bus", 0xD577C962, model.BUS01.FileName),
            new VehicleModelLookup("caddy", 0x44623884, model.CADDY01.FileName),
            new VehicleModelLookup("caddy2", 0xDFF0594C, model.CADDY01.FileName),
            new VehicleModelLookup("caddy3", 0xD227BDBB, model.CADDY01.FileName),
            new VehicleModelLookup("camper", 0x6FD95F68, model.CAMPER01.FileName),
            new VehicleModelLookup("carbonizzare", 0x7B8AB45F, model.CARBONIZZARE01.FileName),
            new VehicleModelLookup("carbonrs", 0xABB0C0, model.CARBONRS01.FileName),
            new VehicleModelLookup("cavalcade", 0x779F23AA, model.CAVALCADE01.FileName),
            new VehicleModelLookup("cavalcade2", 0xD0EB2BE5, model.CAVALCADE01.FileName),
            new VehicleModelLookup("cheetah", 0xB1D95DA0, model.CHEETAH01.FileName),
            new VehicleModelLookup("cheetah2", 0xD4E5F4D, model.CHEETAH01.FileName),
            new VehicleModelLookup("coach", 0x84718D34, model.COACH01.FileName),
            new VehicleModelLookup("cog55", 0x360A438E, model.COG5501.FileName),
            new VehicleModelLookup("cog552", 0x29FCD3E4, model.COG5501.FileName),
            new VehicleModelLookup("cogcabrio", 0x13B57D8A, model.COGCABRIO01.FileName),
            new VehicleModelLookup("cognoscenti", 0x86FE0B60, model.COGNOSCENTI01.FileName),
            new VehicleModelLookup("cognoscenti2", 0xDBF2D57A, model.COGNOSCENTI01.FileName),
            new VehicleModelLookup("comet2", 0xC1AE4D16, model.COMET01.FileName),
            new VehicleModelLookup("comet3", 0x877358AD, model.COMET01.FileName),
            new VehicleModelLookup("comet4", 0x5D1903F9, model.COMET01.FileName),
            new VehicleModelLookup("coquette", 0x67BC037, model.COQUETTE01.FileName),
            new VehicleModelLookup("coquette2", 0x3C4E2113, model.COQUETTE01.FileName),
            new VehicleModelLookup("coquette3", 0x2EC385FE, model.COQUETTE01.FileName),
            new VehicleModelLookup("daemon", 0x77934CEE, model.DAEMON01.FileName),
            new VehicleModelLookup("daemon2", 0xAC4E93C9, model.DAEMON01.FileName),
            new VehicleModelLookup("dilettante", 0xBC993509, model.DILETTANTE01.FileName),
            new VehicleModelLookup("dilettante2", 0x64430650, model.DILETTANTE01.FileName),
            new VehicleModelLookup("dinghy", 0x3D961290, model.DINGHY01.FileName),
            new VehicleModelLookup("dinghy2", 0x107F392C, model.DINGHY01.FileName),
            new VehicleModelLookup("dinghy3", 0x1E5E54EA, model.DINGHY01.FileName),
            new VehicleModelLookup("dinghy4", 0x33B47F96, model.DINGHY01.FileName),
            new VehicleModelLookup("dominator", 0x4CE68AC, model.DOMINATOR01.FileName),
            new VehicleModelLookup("dominator2", 0xC96B73D9, model.COQUETTE01.FileName),
            new VehicleModelLookup("dominator3", 0xC52C6B93, model.DOMINATOR01.FileName),
            new VehicleModelLookup("dominator4", 0xD6FB0F30, model.DOMINATOR01.FileName),
            new VehicleModelLookup("dominator5", 0xAE0A3D4F, model.DOMINATOR01.FileName),
            new VehicleModelLookup("dominator6", 0xB2E046FB, model.DOMINATOR01.FileName),
            new VehicleModelLookup("double", 0x9C669788, model.DOUBLE01.FileName),
            new VehicleModelLookup("dubsta", 0x462FE277, model.DUBSTA01.FileName),
            new VehicleModelLookup("dubsta2", 0xE882E5F6, model.DUBSTA01.FileName),
            new VehicleModelLookup("dubsta3", 0xB6410173, model.DUBSTA01.FileName),
            new VehicleModelLookup("dukes", 0x2B26F456, model.DUKES01.FileName),
            new VehicleModelLookup("dukes2", 0xEC8F7094, model.DUKES01.FileName),
            new VehicleModelLookup("elegy", 0xBBA2261, model.ELEGY01.FileName),
            new VehicleModelLookup("elegy2", 0xDE3D9D22, model.ELEGY01.FileName),
            new VehicleModelLookup("emperor", 0xD7278283, model.EMPEROR01.FileName),
            new VehicleModelLookup("emperor2", 0x8FC3AADC, model.EMPEROR01.FileName),
            new VehicleModelLookup("emperor3", 0xB5FCF74E, model.EMPEROR01.FileName),
            new VehicleModelLookup("entityxf", 0xB2FE5CF9, model.ENTITYXF01.FileName),
            new VehicleModelLookup("exemplar", 0xFFB15B5E, model.EXEMPLAR01.FileName),
            new VehicleModelLookup("f620", 0xDCBCBE48, model.F62001.FileName),
            new VehicleModelLookup("faction", 0x81A9CDDF, model.FACTION01.FileName),
            new VehicleModelLookup("faction2", 0x95466BDB, model.FACTION01.FileName),
            new VehicleModelLookup("faction3", 0x866BCE26, model.FACTION01.FileName),
            new VehicleModelLookup("faggio", 0x9229E4EB, model.FAGGIO01.FileName),
            new VehicleModelLookup("faggio2", 0x350D1AB, model.FAGGIO01.FileName),
            new VehicleModelLookup("faggio3", 0xB328B188, model.FAGGIO01.FileName),
            //new VehicleModelLookup("fbi", 0x432EA949, model.POLICECAR01.FileName),
            //new VehicleModelLookup("fbi2", 0x9DC66994, model.POLICECAR01.FileName),
            new VehicleModelLookup("fcr", 0x25676EAF, model.DOMINATOR01.FileName),
            new VehicleModelLookup("felon", 0xE8A8BDA8, model.FELON01.FileName),
            new VehicleModelLookup("felon2", 0xFAAD85EE, model.FELON01.FileName),
            new VehicleModelLookup("feltzer2", 0x8911B9F5, model.FELTZER01.FileName),
            new VehicleModelLookup("feltzer3", 0xA29D6D10, model.FELTZER01.FileName),
            new VehicleModelLookup("firetruk", 0x73920F8E, model.FIRETRUCK01.FileName),
            new VehicleModelLookup("fq2", 0xBC32A33B, model.FQ201.FileName),
            new VehicleModelLookup("fugitive", 0x71CB2FFB, model.FUGITIVE01.FileName),
            new VehicleModelLookup("fusilade", 0x1DC0BA53, model.FUSILADE01.FileName),
            new VehicleModelLookup("futo", 0x7836CE2F, model.FUTO01.FileName),
            new VehicleModelLookup("gauntlet", 0x94B395C5, model.GAUNTLET01.FileName),
            new VehicleModelLookup("gauntlet2", 0x14D22159, model.GAUNTLET01.FileName),
            new VehicleModelLookup("gauntlet3", 0x2B0C4DCD, model.GAUNTLET01.FileName),
            new VehicleModelLookup("gauntlet4", 0x734C5E50, model.GAUNTLET01.FileName),
            new VehicleModelLookup("gburrito", 0x97FA4F36, model.BURRITO01.FileName),
            new VehicleModelLookup("gburrito2", 0x11AA0E14, model.BURRITO01.FileName),
            new VehicleModelLookup("granger", 0x9628879C, model.GRANGER01.FileName),
            new VehicleModelLookup("gresley", 0xA3FC0F4D, model.GRESLEY01.FileName),
            new VehicleModelLookup("habanero", 0x34B7390F, model.HABANERO01.FileName),
            new VehicleModelLookup("hotknife", 0x239E390, model.HOTKNIFE01.FileName),
            new VehicleModelLookup("infernus", 0x18F25AC7, model.INFERNUS01.FileName),
            new VehicleModelLookup("infernus2", 0xAC33179C, model.INFERNUS01.FileName),
            new VehicleModelLookup("ingot", 0xB3206692, model.INGOT01.FileName),
            new VehicleModelLookup("intruder", 0x34DD8AA1, model.INTRUDER01.FileName),
            new VehicleModelLookup("issi2", 0xB9CB3B69, model.ISSI01.FileName),
            new VehicleModelLookup("issi3", 0x378236E1, model.ISSI01.FileName),
            new VehicleModelLookup("issi4", 0x256E92BA, model.ISSI01.FileName),
            new VehicleModelLookup("issi5", 0x5BA0FF1E, model.ISSI01.FileName),
            new VehicleModelLookup("issi6", 0x49E25BA1, model.ISSI01.FileName),
            new VehicleModelLookup("issi7", 0x6E8DA4F7, model.ISSI01.FileName),
            new VehicleModelLookup("jackal", 0xDAC67112, model.JACKAL01.FileName),
            new VehicleModelLookup("jb700", 0x3EAB5555, model.JB70001.FileName),
            new VehicleModelLookup("journey", 0xF8D48E7A, model.JOURNEY01.FileName),
            new VehicleModelLookup("khamelion", 0x206D1B68, model.KHAMELION01.FileName),
            new VehicleModelLookup("landstalker", 0x4BA4E8DC, model.LANDSTALKER01.FileName),
            new VehicleModelLookup("manana", 0x81634188, model.MANANA01.FileName),
            new VehicleModelLookup("mesa", 0x36848602, model.MESA01.FileName),
            new VehicleModelLookup("mesa2", 0xD36A4B44, model.MESA01.FileName),
            new VehicleModelLookup("mesa3", 0x84F42E51, model.MESA01.FileName),
            new VehicleModelLookup("minivan", 0xED7EADA4, model.MINIVAN01.FileName),
            new VehicleModelLookup("minivan2", 0xBCDE91F0, model.MINIVAN01.FileName),
            new VehicleModelLookup("mixer", 0xD138A6BB, model.MIXER01.FileName),
            new VehicleModelLookup("mixer2", 0x1C534995, model.MIXER01.FileName),
            new VehicleModelLookup("monroe", 0xE62B361B, model.MONROE01.FileName),
            new VehicleModelLookup("mule", 0x35ED670B, model.MULE01.FileName),
            new VehicleModelLookup("mule2", 0xC1632BEB, model.MULE01.FileName),
            new VehicleModelLookup("mule3", 0x85A5B471, model.MULE01.FileName),
            new VehicleModelLookup("mule4", 0x73F4110E, model.MULE01.FileName),
            new VehicleModelLookup("nemesis", 0xDA288376, model.NEMESIS01.FileName),
            new VehicleModelLookup("ninef", 0x3D8FA25C, model.NINEF01.FileName),
            new VehicleModelLookup("ninef2", 0xA8E38B01, model.NINEF01.FileName),
            new VehicleModelLookup("oracle", 0x506434F6, model.ORACLE01.FileName),
            new VehicleModelLookup("oracle2", 0xE18195B2, model.ORACLE01.FileName),
            new VehicleModelLookup("packer", 0x21EEE87D, model.PACKER01.FileName),
            new VehicleModelLookup("patriot", 0xCFCFEB3B, model.PATRIOT01.FileName),
            new VehicleModelLookup("patriot2", 0xE6E967F8, model.PATRIOT01.FileName),
            new VehicleModelLookup("pcj", 0xC9CEAF06, model.PCJ60001.FileName),
            new VehicleModelLookup("penumbra", 0xE9805550, model.PENUMBRA01.FileName),
            new VehicleModelLookup("peyote", 0x6D19CCBC, model.PEYOTE01.FileName),
            new VehicleModelLookup("peyote2", 0x9472CD24, model.PEYOTE01.FileName),
            new VehicleModelLookup("phantom", 0x809AA4CB, model.PHANTOM01.FileName),
            new VehicleModelLookup("phantom2", 0x9DAE1398, model.PHANTOM01.FileName),
            new VehicleModelLookup("phantom3", 0xA90ED5C, model.PHANTOM01.FileName),
            new VehicleModelLookup("phoenix", 0x831A21D5, model.PHOENIX01.FileName),
            new VehicleModelLookup("picador", 0x59E0FBF3, model.PICADOR01.FileName),
            //new VehicleModelLookup("police", 0x79FBB0C5, model.POLICECAR01.FileName),
            //new VehicleModelLookup("police2", 0x9F05F101, model.POLICECAR01.FileName),
            //new VehicleModelLookup("police3", 0x71FA16EA, model.POLICECAR01.FileName),
            //new VehicleModelLookup("police4", 0x8A63C7B9, model.POLICECAR01.FileName),
            //new VehicleModelLookup("policeb", 0xFDEFAEC3, model.POLICECAR01.FileName),
            //new VehicleModelLookup("policeold1", 0xA46462F7, model.POLICECAR01.FileName),
            //new VehicleModelLookup("policeold2", 0x95F4C618, model.POLICECAR01.FileName),
            new VehicleModelLookup("policet", 0x1B38E955, model.POLICETRANSPORT01.FileName),
            new VehicleModelLookup("polmav", 0x1517D4D9, model.POLICEMAVERICK01.FileName),
            new VehicleModelLookup("pony", 0xF8DE29A8, model.PONY01.FileName),
            new VehicleModelLookup("pony2", 0x38408341, model.PONY01.FileName),
            new VehicleModelLookup("pounder", 0x7DE35E7D, model.POUNDER01.FileName),
            new VehicleModelLookup("pounder2", 0x6290F15B, model.POUNDER01.FileName),
            new VehicleModelLookup("prairie", 0xA988D3A2, model.PRAIRIE01.FileName),
            new VehicleModelLookup("pranger", 0x2C33B46E, model.GRANGER01.FileName),
            new VehicleModelLookup("predator", 0xE2E7D4AB, model.PREDATOR01.FileName),
            new VehicleModelLookup("premier", 0x8FB66F9B, model.PREMIER01.FileName),
            new VehicleModelLookup("primo", 0xBB6B404F, model.PRIMO01.FileName),
            new VehicleModelLookup("primo2", 0x86618EDA, model.PRIMO01.FileName),
            new VehicleModelLookup("radi", 0x9D96B45B, model.RADI01.FileName),
            new VehicleModelLookup("rancherxl", 0x6210CBB0, model.RANCHERXL01.FileName),
            new VehicleModelLookup("rancherxl2", 0x7341576B, model.RANCHERXL01.FileName),
            new VehicleModelLookup("rapidgt", 0x8CB29A14, model.RAPIDGT01.FileName),
            new VehicleModelLookup("rapidgt2", 0x679450AF, model.RAPIDGT01.FileName),
            new VehicleModelLookup("rapidgt3", 0x7A2EF5E4, model.RAPIDGT01.FileName),
            new VehicleModelLookup("ratloader", 0xD83C13CE, model.RATLOADER01.FileName),
            new VehicleModelLookup("ratloader2", 0xDCE1D9F7, model.RATLOADER01.FileName),
            new VehicleModelLookup("rebel", 0xB802DD46, model.REBEL01.FileName),
            new VehicleModelLookup("rebel2", 0x8612B64B, model.REBEL01.FileName),
            new VehicleModelLookup("regina", 0xFF22D208, model.REGINA01.FileName),
            new VehicleModelLookup("rhino", 0x2EA68690, model.RHINO01.FileName),
            new VehicleModelLookup("riot", 0xB822A1AA, model.RIOT01.FileName),
            new VehicleModelLookup("riot2", 0x9B16A3B4, model.RIOT01.FileName),
            new VehicleModelLookup("rocoto", 0x7F5C91F1, model.ROCOTO01.FileName),
            new VehicleModelLookup("romero", 0x2560B2FC, model.HEARSE01.FileName),
            new VehicleModelLookup("ruiner", 0xF26CEFF9, model.RUINER01.FileName),
            new VehicleModelLookup("ruiner2", 0x381E10BD, model.RUINER01.FileName),
            new VehicleModelLookup("ruiner3", 0x2E5AFD37, model.RUINER01.FileName),
            new VehicleModelLookup("adder", 0xB779A091, model.ADDER01.FileName),
        };
            VehicleClassLookups = new List<VehicleClassLookup>()
        {
            new VehicleClassLookup("Compact",0,vehicle_category.TwoDoor01.FileName),
            new VehicleClassLookup("Sedan",1,vehicle_category.Sedan.FileName),
            new VehicleClassLookup("SUV",2,vehicle_category.SUV01.FileName),
            new VehicleClassLookup("Coupe",3,vehicle_category.Coupe01.FileName),
            new VehicleClassLookup("Muscle",4,vehicle_category.MuscleCar01.FileName),
            new VehicleClassLookup("Sports Classic",5,vehicle_category.SportsCar01.FileName),
            new VehicleClassLookup("Sports Car",6,vehicle_category.SportsCar01.FileName),
            new VehicleClassLookup("Super",7,vehicle_category.PerformanceCar01.FileName),
            new VehicleClassLookup("Motorcycle",8,vehicle_category.Motorcycle01.FileName),
            new VehicleClassLookup("Off Road",9,vehicle_category.OffRoad01.FileName),
            new VehicleClassLookup("Industrial",10,vehicle_category.IndustrialVehicle01.FileName),
            new VehicleClassLookup("Utility",11,vehicle_category.UtilityVehicle01.FileName),
            new VehicleClassLookup("Van",12,vehicle_category.Van01.FileName),
            new VehicleClassLookup("Bicycle",13,vehicle_category.Bicycle01.FileName),
            new VehicleClassLookup("Boat",14,vehicle_category.Boat01.FileName),
            new VehicleClassLookup("Helicopter",15,vehicle_category.Helicopter01.FileName),
            new VehicleClassLookup("Plane",16,vehicle_category.Sedan.FileName),
            new VehicleClassLookup("Service",17,vehicle_category.Service01.FileName),
            new VehicleClassLookup("Emergency",18,vehicle_category.PoliceCar.FileName),
            new VehicleClassLookup("Military",19,vehicle_category.TroopTransport.FileName),
            new VehicleClassLookup("Commercial",20,vehicle_category.UtilityVehicle01.FileName),
            new VehicleClassLookup("Train",21,vehicle_category.Train01.FileName),
        };
            VehicleMakeLookups = new List<VehicleMakeLookup>()
        {
            new VehicleMakeLookup("Albany",manufacturer.ALBANY01.FileName),
            new VehicleMakeLookup("Annis",manufacturer.ANNIS01.FileName),
            new VehicleMakeLookup("Benefactor",manufacturer.BENEFACTOR01.FileName),
            new VehicleMakeLookup("Bollokan",manufacturer.BOLLOKAN01.FileName),
            new VehicleMakeLookup("Bravado",manufacturer.BRAVADO01.FileName),
            new VehicleMakeLookup("Brute",manufacturer.BRUTE01.FileName),
            new VehicleMakeLookup("Buckingham",""),
            new VehicleMakeLookup("Burgerfahrzeug",manufacturer.BF01.FileName),
            new VehicleMakeLookup("Canis",manufacturer.CANIS01.FileName),
            new VehicleMakeLookup("Chariot",manufacturer.CHARIOT01.FileName),
            new VehicleMakeLookup("Cheval",manufacturer.CHEVAL01.FileName),
            new VehicleMakeLookup("Classique",manufacturer.CLASSIQUE01.FileName),
            new VehicleMakeLookup("Coil",manufacturer.COIL01.FileName),
            new VehicleMakeLookup("Declasse",manufacturer.DECLASSE01.FileName),
            new VehicleMakeLookup("Dewbauchee",manufacturer.DEWBAUCHEE01.FileName),
            new VehicleMakeLookup("Dinka",manufacturer.DINKA01.FileName),
            new VehicleMakeLookup("DUDE",""),
            new VehicleMakeLookup("Dundreary",manufacturer.DUNDREARY01.FileName),
            new VehicleMakeLookup("Emperor",manufacturer.EMPEROR01.FileName),
            new VehicleMakeLookup("Enus",manufacturer.ENUS01.FileName),
            new VehicleMakeLookup("Fathom",manufacturer.FATHOM01.FileName),
            new VehicleMakeLookup("Gallivanter",manufacturer.GALLIVANTER01.FileName),
            new VehicleMakeLookup("Grotti",manufacturer.GROTTI01.FileName),
            new VehicleMakeLookup("Hijak",manufacturer.HIJAK01.FileName),
            new VehicleMakeLookup("HVY",manufacturer.HVY01.FileName),
            new VehicleMakeLookup("Imponte",manufacturer.IMPONTE01.FileName),
            new VehicleMakeLookup("Invetero",manufacturer.INVETERO01.FileName),
            new VehicleMakeLookup("JackSheepe",manufacturer.JACKSHEEPE01.FileName),
            new VehicleMakeLookup("Jobuilt",manufacturer.JOEBUILT01.FileName),
            new VehicleMakeLookup("Karin",manufacturer.KARIN01.FileName),
            new VehicleMakeLookup("Kraken Submersibles",""),
            new VehicleMakeLookup("Lampadati",manufacturer.LAMPADATI01.FileName),
            new VehicleMakeLookup("Liberty Chop Shop",""),
            new VehicleMakeLookup("Liberty City Cycles",""),
            new VehicleMakeLookup("Maibatsu Corporation",manufacturer.MAIBATSU01.FileName),
            new VehicleMakeLookup("Mammoth",manufacturer.MAMMOTH01.FileName),
            new VehicleMakeLookup("MTL",manufacturer.MTL01.FileName),
            new VehicleMakeLookup("Nagasaki",manufacturer.NAGASAKI01.FileName),
            new VehicleMakeLookup("Obey",manufacturer.OBEY01.FileName),
            new VehicleMakeLookup("Ocelot",manufacturer.OCELOT01.FileName),
            new VehicleMakeLookup("Overflod",manufacturer.OVERFLOD01.FileName),
            new VehicleMakeLookup("Pegassi",manufacturer.PEGASI01.FileName),
            new VehicleMakeLookup("Pfister",""),
            new VehicleMakeLookup("Principe",manufacturer.PRINCIPE01.FileName),
            new VehicleMakeLookup("Progen",manufacturer.PROGEN01.FileName),
            new VehicleMakeLookup("ProLaps",""),
            new VehicleMakeLookup("RUNE",""),
            new VehicleMakeLookup("Schyster",manufacturer.SCHYSTER01.FileName),
            new VehicleMakeLookup("Shitzu",manufacturer.SHITZU01.FileName),
            new VehicleMakeLookup("Speedophile",manufacturer.SPEEDOPHILE01.FileName),
            new VehicleMakeLookup("Stanley",manufacturer.STANLEY01.FileName),
            new VehicleMakeLookup("SteelHorse",manufacturer.STEELHORSE01.FileName),
            new VehicleMakeLookup("Truffade",manufacturer.TRUFFADE01.FileName),
            new VehicleMakeLookup("Ubermacht",manufacturer.UBERMACHT01.FileName),
            new VehicleMakeLookup("Vapid",manufacturer.VAPID01.FileName),
            new VehicleMakeLookup("Vulcar",manufacturer.VULCAR01.FileName),
            new VehicleMakeLookup("Vysser",""),
            new VehicleMakeLookup("Weeny",manufacturer.WEENY01.FileName),
            new VehicleMakeLookup("Western Company",manufacturer.WESTERNCOMPANY01.FileName),
            new VehicleMakeLookup("Western Motorcycle Company",manufacturer.WESTERNMOTORCYCLECOMPANY01.FileName),
            new VehicleMakeLookup("Willard",""),
            new VehicleMakeLookup("Zirconium",manufacturer.ZIRCONIUM01.FileName),
        };
        }
        public static string ColorAudio(Color ToLookup)
        {
            ColorLookup VehicleColor = ColorLookups.FirstOrDefault(x => x.BaseColor == ToLookup);
            if (VehicleColor == null)
                return "";
            else
                return VehicleColor.ScannerFile;
        }
        public static string MakeAudio(string MakeName)
        {
            VehicleMakeLookup VehicleMake = VehicleMakeLookups.FirstOrDefault(x => x.MakeName == MakeName);
            if (VehicleMake == null)
                return "";
            else
                return VehicleMake.ScannerFile;
        }
        public static string ModelAudio(uint VehicleHash)
        {
            VehicleModelLookup VehicleModel = VehicleModelLookups.FirstOrDefault(x => x.Hash == VehicleHash);
            if (VehicleModel == null)
                return "";
            else
                return VehicleModel.ScannerFile;
        }
        public static string ClassAudio(int GameClass)
        {
            VehicleClassLookup VehicleClass = VehicleClassLookups.FirstOrDefault(x => x.GameClass == GameClass);
            if (VehicleClass == null)
                return "";
            else
                return VehicleClass.ScannerFile;
        }
        public static string ClassName(int GameClass)
        {
            VehicleClassLookup VehicleClass = VehicleClassLookups.FirstOrDefault(x => x.GameClass == GameClass);
            if (VehicleClass == null)
                return "";
            else
                return VehicleClass.Name;
        }
        public static List<string> LicensePlateAudio(string LicensePlate)
        {
            List<string> AudioFiles = new List<string>();
            foreach (char c in LicensePlate)
            {
                string DispatchFileName = LettersAndNumbersLookup.Where(x => x.AlphaNumeric == c).PickRandom().ScannerFile;
                AudioFiles.Add(DispatchFileName);
            }
            return AudioFiles;
        }
        private class ColorLookup
        {
            public Color BaseColor { get; set; }
            public string ScannerFile { get; set; }

            public ColorLookup(string _ScannerFile, Color _BaseColor)
            {
                BaseColor = _BaseColor;
                ScannerFile = _ScannerFile;
            }

        }
        private class LetterLookup
        {
            public char AlphaNumeric { get; set; }
            public string ScannerFile { get; set; }

            public LetterLookup(char _AlphaNumeric, string _ScannerFile)
            {
                AlphaNumeric = _AlphaNumeric;
                ScannerFile = _ScannerFile;
            }

        }
        private class VehicleModelLookup
        {
            public string Name { get; set; }
            public uint Hash { get; set; }
            public string ScannerFile { get; set; } = "";
            public VehicleModelLookup()
            {

            }
            public VehicleModelLookup(string _Name, uint _Hash, string _ScannerFile)
            {
                Name = _Name;
                Hash = _Hash;
                ScannerFile = _ScannerFile;
            }
        }
        private class VehicleMakeLookup
        {
            public string MakeName { get; set; }
            public string ScannerFile { get; set; } = "";
            public VehicleMakeLookup()
            {

            }
            public VehicleMakeLookup(string makeName, string scannerFile)
            {
                MakeName = makeName;
                ScannerFile = scannerFile;
            }
        }
        private class VehicleClassLookup
        {
            public string Name { get; set; }
            public int GameClass { get; set; }
            public string ScannerFile { get; set; } = "";
            public VehicleClassLookup()
            {

            }
            public VehicleClassLookup(string name, int gameClass, string scannerFile)
            {
                Name = name;
                GameClass = gameClass;
                ScannerFile = scannerFile;
            }
        }

    }
    private static class ZoneScanner
    {
        private static List<ZoneLookup> ZoneList = new List<ZoneLookup>();
        public class ZoneLookup
        {
            public ZoneLookup()
            {

            }
            public ZoneLookup(string _GameName, string _ScannerValue)
            {
                InternalGameName = _GameName;
                ScannerValue = _ScannerValue;
            }
            public string InternalGameName { get; set; }
            public string ScannerValue { get; set; }

        }
        public static void Intitialize()
        {
            SetupLists();
        }
        public static string AudioAtZone(string ZoneName)
        {
            ZoneLookup Returned = ZoneList.Where(x => x.InternalGameName == ZoneName).FirstOrDefault();
            if (Returned == null)
                return "";
            return Returned.ScannerValue;
        }
        private static void SetupLists()
        {

            ZoneList = new List<ZoneLookup>
            {
            //One Off
            new ZoneLookup("OCEANA", areas.TheOcean.FileName),

            //North Blaine
            new ZoneLookup("PROCOB", areas.ProcopioBeach.FileName),
            new ZoneLookup("MTCHIL", areas.MountChiliad.FileName),
            new ZoneLookup("MTGORDO", areas.MountGordo.FileName),
            new ZoneLookup("PALETO", areas.PaletoBay.FileName),
            new ZoneLookup("PALCOV", areas.PaletoBay.FileName),
            new ZoneLookup("PALFOR", areas.PaletoForest.FileName),
            new ZoneLookup("CMSW", areas.ChilliadMountainStWilderness.FileName),
            new ZoneLookup("CALAFB", ""),
            new ZoneLookup("GALFISH", ""),
            new ZoneLookup("ELGORL", areas.MountGordo.FileName),
            new ZoneLookup("GRAPES", areas.Grapeseed.FileName),
            new ZoneLookup("BRADP", areas.BraddockPass.FileName),
            new ZoneLookup("BRADT", areas.TheBraddockTunnel.FileName),
            new ZoneLookup("CCREAK", ""),

            //Blaine
            new ZoneLookup("ALAMO", areas.TheAlamaSea.FileName),
            new ZoneLookup("ARMYB", areas.FtZancudo.FileName),
            new ZoneLookup("CANNY", areas.RatonCanyon.FileName),
            new ZoneLookup("DESRT", areas.GrandeSonoranDesert.FileName),
            new ZoneLookup("HUMLAB", ""),
            new ZoneLookup("JAIL", areas.BoilingBrookPenitentiary.FileName),
            new ZoneLookup("LAGO", areas.LagoZancudo.FileName),
            new ZoneLookup("MTJOSE", areas.MtJosiah.FileName),
            new ZoneLookup("NCHU", areas.NorthChumash.FileName),
            new ZoneLookup("SANCHIA", ""),
            new ZoneLookup("SANDY", areas.SandyShores.FileName),
            new ZoneLookup("SLAB", areas.SlabCity.FileName),
            new ZoneLookup("ZANCUDO", areas.ZancudoRiver.FileName),
            new ZoneLookup("ZQ_UAR", areas.DavisCourts.FileName),

            //Vespucci
            new ZoneLookup("BEACH", areas.VespucciBeach.FileName),
            new ZoneLookup("DELBE", areas.DelPierroBeach.FileName),
            new ZoneLookup("DELPE", areas.DelPierro.FileName),
            new ZoneLookup("VCANA", areas.VespucciCanal.FileName),
            new ZoneLookup("VESP", areas.Vespucci.FileName),
            new ZoneLookup("LOSPUER", areas.LaPuertes.FileName),
            new ZoneLookup("PBLUFF", areas.PacificBluffs.FileName),
            new ZoneLookup("DELSOL", areas.PuertoDelSoul.FileName),

            //Central
            new ZoneLookup("BANNING", areas.Banning.FileName),
            new ZoneLookup("CHAMH", areas.ChamberlainHills.FileName),
            new ZoneLookup("DAVIS", areas.Davis.FileName),
            new ZoneLookup("DOWNT", areas.Downtown.FileName),
            new ZoneLookup("PBOX", areas.PillboxHill.FileName),
            new ZoneLookup("RANCHO", areas.Rancho.FileName),
            new ZoneLookup("SKID", areas.MissionRow.FileName),
            new ZoneLookup("STAD", areas.MazeBankArena.FileName),
            new ZoneLookup("STRAW", areas.Strawberry.FileName),
            new ZoneLookup("TEXTI", areas.TextileCity.FileName),
            new ZoneLookup("LEGSQU", ""),

            //East LS
            new ZoneLookup("CYPRE", areas.CypressFlats.FileName),
            new ZoneLookup("LMESA", areas.LaMesa.FileName),
            new ZoneLookup("MIRR", areas.MirrorPark.FileName),
            new ZoneLookup("MURRI", areas.MuriettaHeights.FileName),
            new ZoneLookup("EBURO", areas.ElBerroHights.FileName),

            //Vinewood
            new ZoneLookup("ALTA", areas.Alta.FileName),
            new ZoneLookup("DTVINE", areas.DowntownVinewood.FileName),
            new ZoneLookup("EAST_V", areas.EastVinewood.FileName),
            new ZoneLookup("HAWICK", ""),
            new ZoneLookup("HORS", areas.TheRaceCourse.FileName),
            new ZoneLookup("VINE", areas.Vinewood.FileName),
            new ZoneLookup("WVINE", areas.WestVinewood.FileName),

            //PortOfLosSantos
            new ZoneLookup("ELYSIAN", areas.ElysianIsland.FileName),
            new ZoneLookup("ZP_ORT", areas.PortOfSouthLosSantos.FileName),
            new ZoneLookup("TERMINA", areas.Terminal.FileName),
            new ZoneLookup("ZP_ORT", areas.PortOfSouthLosSantos.FileName),
            new ZoneLookup("AIRP", areas.LosSantosInternationalAirport.FileName),

            //Rockford Hills
            new ZoneLookup("BURTON", areas.Burton.FileName),
            new ZoneLookup("GOLF", areas.TheGWCGolfingSociety.FileName),
            new ZoneLookup("KOREAT", areas.LittleSeoul.FileName),
            new ZoneLookup("MORN", areas.MorningWood.FileName),
            new ZoneLookup("MOVIE", areas.RichardsMajesticStudio.FileName),
            new ZoneLookup("RICHM", areas.Richman.FileName),
            new ZoneLookup("ROCKF", areas.RockfordHills.FileName),     

            //Vinewood Hills
            new ZoneLookup("CHIL", areas.VinewoodHills.FileName),
            new ZoneLookup("GREATC", areas.GreatChapparalle.FileName),
            new ZoneLookup("BAYTRE", areas.BayTreeCanyon.FileName),
            new ZoneLookup("RGLEN", areas.RichmanGlenn.FileName),
            new ZoneLookup("TONGVAV", areas.TongvaValley.FileName),
            new ZoneLookup("HARMO", areas.Harmony.FileName),
            new ZoneLookup("RTRAK", areas.TheRedwoodLightsTrack.FileName),
           
            //Chumash
            new ZoneLookup("BANHAMC", ""),
            new ZoneLookup("BHAMCA", ""),
            new ZoneLookup("CHU", areas.Chumash.FileName),
            new ZoneLookup("TONGVAH", areas.TongaHills.FileName),
           
            //Tataviam 
            new ZoneLookup("LACT", ""),
            new ZoneLookup("LDAM", ""),
            new ZoneLookup("NOOSE", ""),
            new ZoneLookup("PALHIGH", areas.PalominoHighlands.FileName),
            new ZoneLookup("PALMPOW", areas.PalmerTaylorPowerStation.FileName),
            new ZoneLookup("SANAND", areas.SanAndreas.FileName),
            new ZoneLookup("TATAMO", areas.TatathiaMountains.FileName),
            new ZoneLookup("WINDF", areas.RonAlternatesWindFarm.FileName),
    };

        }
    }
    private static class StreetScanner
    {
        private static List<StreetLookup> StreetsList = new List<StreetLookup>();
        public static void Intitialize()
        {
            SetupLists();
        }
        public static string AudioAtStreet(string StreetName)
        {
            StreetLookup Returned = StreetsList.Where(x => x.Name == StreetName).FirstOrDefault();
            if (Returned == null)
                return "";
            return Returned.DispatchFile;
        }
        public static void SetupLists()
        {
            StreetsList = new List<StreetLookup>
        {
            new StreetLookup("Joshua Rd", streets.JoshuaRoad.FileName),
            new StreetLookup("East Joshua Road", streets.EastJoshuaRoad.FileName),
            new StreetLookup("Marina Dr", streets.MarinaDrive.FileName),
            new StreetLookup("Alhambra Dr", streets.ElHamberDrive.FileName),
            new StreetLookup("Niland Ave", streets.NeelanAve.FileName),
            new StreetLookup("Zancudo Ave", streets.ZancudoAve.FileName),
            new StreetLookup("Armadillo Ave", streets.ArmadilloAve.FileName),
            new StreetLookup("Algonquin Blvd", streets.AlgonquinBlvd.FileName),
            new StreetLookup("Mountain View Dr", streets.MountainViewDrive.FileName),
            new StreetLookup("Cholla Springs Ave", streets.ChollaSpringsAve.FileName),
            new StreetLookup("Panorama Dr", streets.PanoramaDrive.FileName),
            new StreetLookup("Lesbos Ln", streets.LesbosLane.FileName),
            new StreetLookup("Calafia Rd", streets.CalapiaRoad.FileName),
            new StreetLookup("North Calafia Way", streets.NorthKalafiaWay.FileName),
            new StreetLookup("Cassidy Trail", streets.CassidyTrail.FileName),
            new StreetLookup("Seaview Rd", streets.SeaviewRd.FileName),
            new StreetLookup("Grapeseed Main St", streets.GrapseedMainStreet.FileName),
            new StreetLookup("Grapeseed Ave", streets.GrapeseedAve.FileName),
            new StreetLookup("Joad Ln", streets.JilledLane.FileName),
            new StreetLookup("Union Rd", streets.UnionRoad.FileName),
            new StreetLookup("O'Neil Way", streets.OneilWay.FileName),
            new StreetLookup("Senora Fwy", streets.SonoraFreeway.FileName),
            new StreetLookup("Catfish View", streets.CatfishView.FileName),
            new StreetLookup("Great Ocean Hwy", streets.GreatOceanHighway.FileName),
            new StreetLookup("Paleto Blvd", streets.PaletoBlvd.FileName),
            new StreetLookup("Duluoz Ave", streets.DelouasAve.FileName),
            new StreetLookup("Procopio Dr", streets.ProcopioDrive.FileName),
            new StreetLookup("Cascabel Ave"),
            new StreetLookup("Peaceful St", streets.PeacefulStreet.FileName),
            new StreetLookup("Procopio Promenade", streets.ProcopioPromenade.FileName),
            new StreetLookup("Pyrite Ave", streets.PyriteAve.FileName),
            new StreetLookup("Fort Zancudo Approach Rd", streets.FortZancudoApproachRoad.FileName),
            new StreetLookup("Barbareno Rd", streets.BarbarinoRoad.FileName),
            new StreetLookup("Ineseno Road", streets.EnecinoRoad.FileName),
            new StreetLookup("West Eclipse Blvd", streets.WestEclipseBlvd.FileName),
            new StreetLookup("Playa Vista", streets.PlayaVista.FileName),
            new StreetLookup("Bay City Ave", streets.BaseCityAve.FileName),
            new StreetLookup("Del Perro Fwy", streets.DelPierroFreeway.FileName),
            new StreetLookup("Equality Way", streets.EqualityWay.FileName),
            new StreetLookup("Red Desert Ave", streets.RedDesertAve.FileName),
            new StreetLookup("Magellan Ave", streets.MagellanAve.FileName),
            new StreetLookup("Sandcastle Way", streets.SandcastleWay.FileName),
            new StreetLookup("Vespucci Blvd", streets.VespucciBlvd.FileName),
            new StreetLookup("Prosperity St", streets.ProsperityStreet.FileName),
            new StreetLookup("San Andreas Ave", streets.SanAndreasAve.FileName),
            new StreetLookup("North Rockford Dr", streets.NorthRockfordDrive.FileName),
            new StreetLookup("South Rockford Dr", streets.SouthRockfordDrive.FileName),
            new StreetLookup("Marathon Ave", streets.MarathonAve.FileName),
            new StreetLookup("Boulevard Del Perro", streets.BlvdDelPierro.FileName),
            new StreetLookup("Cougar Ave", streets.CougarAve.FileName),
            new StreetLookup("Liberty St", streets.LibertyStreet.FileName),
            new StreetLookup("Bay City Incline", streets.BaseCityIncline.FileName),
            new StreetLookup("Conquistador St", streets.ConquistadorStreet.FileName),
            new StreetLookup("Cortes St", streets.CortezStreet.FileName),
            new StreetLookup("Vitus St", streets.VitasStreet.FileName),
            new StreetLookup("Aguja St", streets.ElGouhaStreet.FileName),/////maytbe????!?!?!
            new StreetLookup("Goma St", streets.GomezStreet.FileName),
            new StreetLookup("Melanoma St", streets.MelanomaStreet.FileName),
            new StreetLookup("Palomino Ave", streets.PalaminoAve.FileName),
            new StreetLookup("Invention Ct", streets.InventionCourt.FileName),
            new StreetLookup("Imagination Ct", streets.ImaginationCourt.FileName),
            new StreetLookup("Rub St", streets.RubStreet.FileName),
            new StreetLookup("Tug St", streets.TugStreet.FileName),
            new StreetLookup("Ginger St", streets.GingerStreet.FileName),
            new StreetLookup("Lindsay Circus", streets.LindsayCircus.FileName),
            new StreetLookup("Calais Ave", streets.CaliasAve.FileName),
            new StreetLookup("Adam's Apple Blvd", streets.AdamsAppleBlvd.FileName),
            new StreetLookup("Alta St", streets.AlterStreet.FileName),
            new StreetLookup("Integrity Way", streets.IntergrityWy.FileName),
            new StreetLookup("Swiss St", streets.SwissStreet.FileName),
            new StreetLookup("Strawberry Ave", streets.StrawberryAve.FileName),
            new StreetLookup("Capital Blvd", streets.CapitalBlvd.FileName),
            new StreetLookup("Crusade Rd", streets.CrusadeRoad.FileName),
            new StreetLookup("Innocence Blvd", streets.InnocenceBlvd.FileName),
            new StreetLookup("Davis Ave", streets.DavisAve.FileName),
            new StreetLookup("Little Bighorn Ave", streets.LittleBighornAve.FileName),
            new StreetLookup("Roy Lowenstein Blvd", streets.RoyLowensteinBlvd.FileName),
            new StreetLookup("Jamestown St", streets.JamestownStreet.FileName),
            new StreetLookup("Carson Ave", streets.CarsonAve.FileName),
            new StreetLookup("Grove St", streets.GroveStreet.FileName),
            new StreetLookup("Brouge Ave"),
            new StreetLookup("Covenant Ave", streets.CovenantAve.FileName),
            new StreetLookup("Dutch London St", streets.DutchLondonStreet.FileName),
            new StreetLookup("Signal St", streets.SignalStreet.FileName),
            new StreetLookup("Elysian Fields Fwy", streets.ElysianFieldsFreeway.FileName),
            new StreetLookup("Plaice Pl"),
            new StreetLookup("Chum St", streets.ChumStreet.FileName),
            new StreetLookup("Chupacabra St"),
            new StreetLookup("Miriam Turner Overpass", streets.MiriamTurnerOverpass.FileName),
            new StreetLookup("Autopia Pkwy", streets.AltopiaParkway.FileName),
            new StreetLookup("Exceptionalists Way", streets.ExceptionalistWay.FileName),
            new StreetLookup("La Puerta Fwy", ""),
            new StreetLookup("New Empire Way", streets.NewEmpireWay.FileName),
            new StreetLookup("Runway1", streets.RunwayOne.FileName),
            new StreetLookup("Greenwich Pkwy", streets.GrenwichParkway.FileName),
            new StreetLookup("Kortz Dr", streets.KortzDrive.FileName),
            new StreetLookup("Banham Canyon Dr", streets.BanhamCanyonDrive.FileName),
            new StreetLookup("Buen Vino Rd"),
            new StreetLookup("Route 68", streets.Route68.FileName),
            new StreetLookup("Zancudo Grande Valley", streets.ZancudoGrandeValley.FileName),
            new StreetLookup("Zancudo Barranca", streets.ZancudoBaranca.FileName),
            new StreetLookup("Galileo Rd", streets.GallileoRoad.FileName),
            new StreetLookup("Mt Vinewood Dr", streets.MountVinewoodDrive.FileName),
            new StreetLookup("Marlowe Dr"),
            new StreetLookup("Milton Rd", streets.MiltonRoad.FileName),
            new StreetLookup("Kimble Hill Dr", streets.KimbalHillDrive.FileName),
            new StreetLookup("Normandy Dr", streets.NormandyDrive.FileName),
            new StreetLookup("Hillcrest Ave", streets.HillcrestAve.FileName),
            new StreetLookup("Hillcrest Ridge Access Rd", streets.HillcrestRidgeAccessRoad.FileName),
            new StreetLookup("North Sheldon Ave", streets.NorthSheldonAve.FileName),
            new StreetLookup("Lake Vinewood Dr", streets.LakeVineWoodDrive.FileName),
            new StreetLookup("Lake Vinewood Est", streets.LakeVinewoodEstate.FileName),
            new StreetLookup("Baytree Canyon Rd", streets.BaytreeCanyonRoad.FileName),
            new StreetLookup("North Conker Ave", streets.NorthConkerAve.FileName),
            new StreetLookup("Wild Oats Dr", streets.WildOatsDrive.FileName),
            new StreetLookup("Whispymound Dr", streets.WispyMoundDrive.FileName),
            new StreetLookup("Didion Dr", streets.DiedianDrive.FileName),
            new StreetLookup("Cox Way", streets.CoxWay.FileName),
            new StreetLookup("Picture Perfect Drive", streets.PicturePerfectDrive.FileName),
            new StreetLookup("South Mo Milton Dr", streets.SouthMoMiltonDrive.FileName),
            new StreetLookup("Cockingend Dr", streets.CockandGinDrive.FileName),
            new StreetLookup("Mad Wayne Thunder Dr", streets.MagwavevendorDrive.FileName),
            new StreetLookup("Hangman Ave", streets.HangmanAve.FileName),
            new StreetLookup("Dunstable Ln", streets.DunstableLane.FileName),
            new StreetLookup("Dunstable Dr", streets.DunstableDrive.FileName),
            new StreetLookup("Greenwich Way", streets.GrenwichWay.FileName),
            new StreetLookup("Greenwich Pl", streets.GrunnichPlace.FileName),
            new StreetLookup("Hardy Way"),
            new StreetLookup("Richman St", streets.RichmondStreet.FileName),
            new StreetLookup("Ace Jones Dr", streets.AceJonesDrive.FileName),
            new StreetLookup("Los Santos Freeway", ""),
            new StreetLookup("Senora Rd", streets.SonoraRoad.FileName),
            new StreetLookup("Nowhere Rd", streets.NowhereRoad.FileName),
            new StreetLookup("Smoke Tree Rd", streets.SmokeTreeRoad.FileName),
            new StreetLookup("Cholla Rd", streets.ChollaRoad.FileName),
            new StreetLookup("Cat-Claw Ave", streets.CatClawAve.FileName),
            new StreetLookup("Senora Way", streets.SonoraWay.FileName),
            new StreetLookup("Palomino Fwy", streets.PaliminoFreeway.FileName),
            new StreetLookup("Shank St", streets.ShankStreet.FileName),
            new StreetLookup("Macdonald St", streets.McDonaldStreet.FileName),
            new StreetLookup("Route 68 Approach", streets.Route68.FileName),
            new StreetLookup("Vinewood Park Dr", streets.VinewoodParkDrive.FileName),
            new StreetLookup("Vinewood Blvd", streets.VinewoodBlvd.FileName),
            new StreetLookup("Mirror Park Blvd", streets.MirrorParkBlvd.FileName),
            new StreetLookup("Glory Way", streets.GloryWay.FileName),
            new StreetLookup("Bridge St", streets.BridgeStreet.FileName),
            new StreetLookup("West Mirror Drive", streets.WestMirrorDrive.FileName),
            new StreetLookup("Nikola Ave", streets.NicolaAve.FileName),
            new StreetLookup("East Mirror Dr", streets.EastMirrorDrive.FileName),
            new StreetLookup("Nikola Pl", streets.NikolaPlace.FileName),
            new StreetLookup("Mirror Pl", streets.MirrorPlace.FileName),
            new StreetLookup("El Rancho Blvd", streets.ElRanchoBlvd.FileName),
            new StreetLookup("Olympic Fwy", streets.OlympicFreeway.FileName),
            new StreetLookup("Fudge Ln", streets.FudgeLane.FileName),
            new StreetLookup("Amarillo Vista", streets.AmarilloVista.FileName),
            new StreetLookup("Labor Pl", streets.ForceLaborPlace.FileName),
            new StreetLookup("El Burro Blvd", streets.ElBurroBlvd.FileName),
            new StreetLookup("Sustancia Rd", streets.SustanciaRoad.FileName),
            new StreetLookup("South Shambles St", streets.SouthShambleStreet.FileName),
            new StreetLookup("Hanger Way", streets.HangarWay.FileName),
            new StreetLookup("Orchardville Ave", streets.OrchidvilleAve.FileName),
            new StreetLookup("Popular St", streets.PopularStreet.FileName),
            new StreetLookup("Buccaneer Way", streets.BuccanierWay.FileName),
            new StreetLookup("Abattoir Ave", streets.AvatorAve.FileName),
            new StreetLookup("Voodoo Place"),
            new StreetLookup("Mutiny Rd", streets.MutineeRoad.FileName),
            new StreetLookup("South Arsenal St", streets.SouthArsenalStreet.FileName),
            new StreetLookup("Forum Dr", streets.ForumDrive.FileName),
            new StreetLookup("Morningwood Blvd", streets.MorningwoodBlvd.FileName),
            new StreetLookup("Dorset Dr", streets.DorsetDrive.FileName),
            new StreetLookup("Caesars Place", streets.CaesarPlace.FileName),
            new StreetLookup("Spanish Ave", streets.SpanishAve.FileName),
            new StreetLookup("Portola Dr", streets.PortolaDrive.FileName),
            new StreetLookup("Edwood Way", streets.EdwardWay.FileName),
            new StreetLookup("San Vitus Blvd", streets.SanVitusBlvd.FileName),
            new StreetLookup("Eclipse Blvd", streets.EclipseBlvd.FileName),
            new StreetLookup("Gentry Lane"),
            new StreetLookup("Las Lagunas Blvd", streets.LasLegunasBlvd.FileName),
            new StreetLookup("Power St", streets.PowerStreet.FileName),
            new StreetLookup("Mt Haan Rd", streets.MtHaanRoad.FileName),
            new StreetLookup("Elgin Ave", streets.ElginAve.FileName),
            new StreetLookup("Hawick Ave", streets.HawickAve.FileName),
            new StreetLookup("Meteor St", streets.MeteorStreet.FileName),
            new StreetLookup("Alta Pl", streets.AltaPlace.FileName),
            new StreetLookup("Occupation Ave", streets.OccupationAve.FileName),
            new StreetLookup("Carcer Way", streets.CarcerWay.FileName),
            new StreetLookup("Eastbourne Way", streets.EastbourneWay.FileName),
            new StreetLookup("Rockford Dr", streets.RockfordDrive.FileName),
            new StreetLookup("Abe Milton Pkwy", streets.EightMiltonParkway.FileName),
            new StreetLookup("Laguna Pl", streets.LagunaPlace.FileName),
            new StreetLookup("Sinners Passage", streets.SinnersPassage.FileName),
            new StreetLookup("Atlee St", streets.AtleyStreet.FileName),
            new StreetLookup("Sinner St", streets.SinnerStreet.FileName),
            new StreetLookup("Supply St", streets.SupplyStreet.FileName),
            new StreetLookup("Amarillo Way", streets.AmarilloWay.FileName),
            new StreetLookup("Tower Way", streets.TowerWay.FileName),
            new StreetLookup("Decker St", streets.DeckerStreet.FileName),
            new StreetLookup("Tackle St", streets.TackleStreet.FileName),
            new StreetLookup("Low Power St", streets.LowPowerStreet.FileName),
            new StreetLookup("Clinton Ave", streets.ClintonAve.FileName),
            new StreetLookup("Fenwell Pl", streets.FenwellPlace.FileName),
            new StreetLookup("Utopia Gardens", streets.UtopiaGardens.FileName),
            new StreetLookup("Cavalry Blvd"),
            new StreetLookup("South Boulevard Del Perro", streets.SouthBlvdDelPierro.FileName),
            new StreetLookup("Americano Way", streets.AmericanoWay.FileName),
            new StreetLookup("Sam Austin Dr", streets.SamAustinDrive.FileName),
            new StreetLookup("East Galileo Ave", streets.EastGalileoAve.FileName),
            new StreetLookup("Galileo Park"),
            new StreetLookup("West Galileo Ave", streets.WestGalileoAve.FileName),
            new StreetLookup("Tongva Dr", streets.TongvaDrive.FileName),
            new StreetLookup("Zancudo Rd", streets.ZancudoRoad.FileName),
            new StreetLookup("Movie Star Way", streets.MovieStarWay.FileName),
            new StreetLookup("Heritage Way", streets.HeritageWay.FileName),
            new StreetLookup("Perth St", streets.PerfStreet.FileName),
            new StreetLookup("Chianski Passage"),
            new StreetLookup("Lolita Ave", streets.LolitaAve.FileName),
            new StreetLookup("Meringue Ln", streets.MirangeLane.FileName),
            new StreetLookup("Strangeways Dr", streets.StrangeWaysDrive.FileName),

            new StreetLookup("Mt Haan Dr", streets.MtHaanDrive.FileName)
        };
        }
        public class StreetLookup
        {
            public string Name = "";
            public string DispatchFile = "";
            public StreetLookup()
            {

            }
            public StreetLookup(string _Name)
            {
                Name = _Name;
            }
            public StreetLookup(string _Name, string _DispatchFile)
            {
                Name = _Name;
                DispatchFile = _DispatchFile;
            }
        }
    }

}

