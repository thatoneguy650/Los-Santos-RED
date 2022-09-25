using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static DispatchScannerFiles;

namespace LosSantosRED.lsr
{
    public class ScannerNew
    {
        private IPoliceRespondable Player;
        private IAudioPlayable AudioPlayer;
        private ISettingsProvideable Settings;
        private ITimeReportable Time;
        private IEntityProvideable World;
        private PoliceScannerNew PoliceScanner;

        public ScannerNew(IEntityProvideable world, IPoliceRespondable currentPlayer, IAudioPlayable audioPlayer, ISettingsProvideable settings, ITimeReportable time)
        {
            AudioPlayer = audioPlayer;
            Player = currentPlayer;
            World = world;
            Settings = settings;
            Time = time;
            PoliceScanner = new PoliceScannerNew(World, Player, AudioPlayer, Settings, Time);
        }
        public void Setup()
        {
            PoliceScanner.Setup();
        }
        public void Update()
        {
            PoliceScanner.Update();
        }
        public void Reset()
        {
            PoliceScanner.Reset();
        }
        public void Dispose()
        {
            PoliceScanner.Dispose();
        }
        public void Abort()
        {
            PoliceScanner.Abort();
        }
        public void AnnounceCrime(Crime crimeAssociated, CrimeSceneDescription reportInformation)
        {
            PoliceScanner.AnnounceCrime(crimeAssociated, reportInformation);
        }
        public void DebugPlayDispatch()
        {
            PoliceScanner.DebugPlayDispatch();
        }

        //Events
        public void OnAppliedWantedStats(int wantedLevel)
        {
            PoliceScanner.OnAppliedWantedStats(wantedLevel);
            EntryPoint.WriteToConsole($"SCANNER EVENT: OnAppliedWantedStats", 3);
        }
        public void OnArmyDeployed()
        {
            EntryPoint.WriteToConsole($"SCANNER EVENT: OnArmyDeployed", 3);
            //MILITARY
        }
        public void OnBribedPolice()
        {
            PoliceScanner.OnBribedPolice();
            EntryPoint.WriteToConsole($"SCANNER EVENT: OnBribedPolice", 3);
        }
        public void OnExcessiveSpeed()
        {
            PoliceScanner.OnExcessiveSpeed();
            EntryPoint.WriteToConsole($"SCANNER EVENT: OnExcessiveSpeed", 5);
        }
        public void OnFIBHRTDeployed()
        {
            PoliceScanner.OnFIBHRTDeployed();
            EntryPoint.WriteToConsole($"SCANNER EVENT: OnFIBHRTDeployed", 3);
        }
        public void OnFirefightingServicesRequested()
        {

            EntryPoint.WriteToConsole($"SCANNER EVENT: FirefightingServicesRequired", 3);
        }
        public void OnGotInVehicle()
        {
            PoliceScanner.OnGotInVehicle();
            EntryPoint.WriteToConsole($"SCANNER EVENT: OnGotInVehicle", 3);
        }
        public void OnGotOffFreeway()
        {
            PoliceScanner.OnGotOffFreeway();
            EntryPoint.WriteToConsole($"SCANNER EVENT: OnGotOffFreeway", 5);
        }
        public void OnGotOnFreeway()
        {
            PoliceScanner.OnGotOnFreeway();
            EntryPoint.WriteToConsole($"SCANNER EVENT: OnGotOnFreeway", 5);
        }
        public void OnGotOutOfVehicle()
        {
            PoliceScanner.OnGotOutOfVehicle();
            EntryPoint.WriteToConsole($"SCANNER EVENT: OnGotOutOfVehicle", 3);
        }
        public void OnHelicoptersDeployed()
        {
            PoliceScanner.OnHelicoptersDeployed();
            EntryPoint.WriteToConsole($"SCANNER EVENT: OnHelicoptersDeployed", 3);
        }
        public void OnInvestigationExpire()
        {
            PoliceScanner.OnInvestigationExpire();
            EntryPoint.WriteToConsole($"SCANNER EVENT: OnInvestigationExpire", 3);
        }
        public void OnLethalForceAuthorized()
        {
            PoliceScanner.OnLethalForceAuthorized();
            EntryPoint.WriteToConsole($"SCANNER EVENT: OnLethalForceAuthorized", 3);
        }
        public void OnMedicalServicesRequested()
        {
            PoliceScanner.OnMedicalServicesRequested();
            EntryPoint.WriteToConsole($"SCANNER EVENT: MedicalServicesRequired", 3);
        }
        public void OnNooseDeployed()
        {
            PoliceScanner.OnNooseDeployed();
            EntryPoint.WriteToConsole($"SCANNER EVENT: OnNooseDeployed", 3);
        }
        public void OnPaidFine()
        {
            PoliceScanner.OnPaidFine();
            EntryPoint.WriteToConsole($"SCANNER EVENT: OnBribedPolice", 3);
        }
        public void OnTalkedOutOfTicket()
        {
            PoliceScanner.OnTalkedOutOfTicket();
            EntryPoint.WriteToConsole($"SCANNER EVENT: OnTalkedOutOfTicket", 3);
        }
        public void OnPlayerBusted()
        {
            PoliceScanner.OnPlayerBusted();
            EntryPoint.WriteToConsole($"SCANNER EVENT: OnSuspectBusted", 3);
        }
        public void OnPoliceNoticeVehicleChange()
        {
            PoliceScanner.OnPoliceNoticeVehicleChange();
            EntryPoint.WriteToConsole($"SCANNER EVENT: OnPoliceNoticeVehicleChange", 3);
        }
        public void OnRequestedBackUp()
        {
            PoliceScanner.OnRequestedBackUp();
            EntryPoint.WriteToConsole($"SCANNER EVENT: OnRequestedBackUp", 3);
            //MILITARY
        }
        public void OnRequestedBackUpSimple()
        {
            PoliceScanner.OnRequestedBackUpSimple();
            EntryPoint.WriteToConsole($"SCANNER EVENT: OnRequestedBackUpSimple", 3);
            //MILITARY
        }
        public void OnSuspectEluded()
        {
            PoliceScanner.OnSuspectEluded();
            EntryPoint.WriteToConsole($"SCANNER EVENT: OnSuspectEluded", 3);
        }
        public void OnSuspectWasted()
        {
            PoliceScanner.OnSuspectWasted();
            EntryPoint.WriteToConsole($"SCANNER EVENT: OnSuspectWasted", 3);
        }
        public void OnSuspectShooting()
        {
            PoliceScanner.OnSuspectShooting();
            EntryPoint.WriteToConsole($"SCANNER EVENT: OnSuspectShooting", 3);
        }
        public void OnVehicleCrashed()
        {
            PoliceScanner.OnVehicleCrashed();
            EntryPoint.WriteToConsole($"SCANNER EVENT: OnVehicleCrashed", 3);
        }
        public void OnVehicleStartedFire()
        {
            PoliceScanner.OnVehicleStartedFire();
            EntryPoint.WriteToConsole($"SCANNER EVENT: OnVehicleStartedFire", 3);
        }
        public void OnWantedActiveMode()
        {
            PoliceScanner.OnWantedActiveMode();
            EntryPoint.WriteToConsole($"SCANNER EVENT: OnStarsActive", 3);
            //MILITARY
        }
        public void OnWantedSearchMode()
        {
            PoliceScanner.OnWantedSearchMode();
            EntryPoint.WriteToConsole($"SCANNER EVENT: OnStarsGreyedOut", 3);
            //MILITARY
        }
        public void OnWeaponsFree()
        {
            PoliceScanner.OnWeaponsFree();
            EntryPoint.WriteToConsole($"SCANNER EVENT: OnWeaponsFree", 3);
            //MILITARY
        }

    }
}