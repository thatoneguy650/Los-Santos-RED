using ExtensionsMethods;
using LosSantosRED.lsr.Data;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr
{

    public class Violations
    {
        private List<string> ContinuouslyViolatingCrimes= new List<string>();
        private IViolateable Player;
        private ITimeReportable TimeReporter;
        private ICrimes Crimes;
        private ISettingsProvideable Settings;
        private IZones Zones;
        private IGangTerritories GangTerritories;
        private IEntityProvideable World;
        public Violations(IViolateable currentPlayer, ITimeReportable timeReporter, ICrimes crimes, ISettingsProvideable settings, IZones zones, IGangTerritories gangTerritories, IEntityProvideable world, IInteractionable interactionable)
        {
            TimeReporter = timeReporter;
            Player = currentPlayer;
            Crimes = crimes;
            Settings = settings;
            Zones = zones;
            GangTerritories = gangTerritories;
            World = world;
            TrafficViolations = new TrafficViolations(Player, this, Settings, TimeReporter, World);
            DamageViolations = new DamageViolations(Player, this, Settings, TimeReporter, Crimes, Zones, GangTerritories);
            WeaponViolations = new WeaponViolations(Player, this, Settings, TimeReporter);
            TheftViolations = new TheftViolations(Player, this, Settings, TimeReporter, Zones, GangTerritories);
            OtherViolations = new OtherViolations(Player, this, Settings, TimeReporter, World, interactionable);
            MinorViolations = new MinorViolations(Player, this, Settings, TimeReporter, World, interactionable);
        }
        public readonly List<Crime> CrimesViolating = new List<Crime>();
        public TrafficViolations TrafficViolations { get; private set; }
        public DamageViolations DamageViolations { get; private set; }
        public WeaponViolations WeaponViolations { get; private set; }
        public TheftViolations TheftViolations { get; private set; }
        public OtherViolations OtherViolations { get; private set; }
        public MinorViolations MinorViolations { get; private set; }
        public List<Crime> CivilianReportableCrimesViolating => CrimesViolating.Where(x => x.CanBeReactedToByCivilians).ToList();
        public string LawsViolatingDisplay => string.Join(", ", CrimesViolating.OrderBy(x=>x.Priority).Select(x => x.Name));
        public bool IsViolatingSeriousCrime => CrimesViolating.Any(x => x.ResultingWantedLevel >= 2);
        public bool CanCarryAndFireWeapons => Player.IsCop || Player.IsSecurityGuard || Settings.SettingsManager.ViolationSettings.TreatAsCop;
        public bool CanDamageWantedCivilians => Player.IsCop || Player.IsSecurityGuard || Settings.SettingsManager.ViolationSettings.TreatAsCop;
        public bool CanBodyInteract => Player.IsEMT;
        public bool CanDriveRecklesslyWithSiren => Player.IsCop || Player.IsEMT || Player.IsFireFighter || Settings.SettingsManager.ViolationSettings.TreatAsCop;
        public bool CanIgnoreAllTrafficLaws => Player.IsCop || Settings.SettingsManager.ViolationSettings.TreatAsCop;
        public bool CanEnterRestrictedAreas => Player.IsCop || Settings.SettingsManager.ViolationSettings.TreatAsCop;
        public void Setup()
        {
            TrafficViolations.Setup();
            DamageViolations.Setup();
            WeaponViolations.Setup();
            TheftViolations.Setup();
            OtherViolations.Setup();
            MinorViolations.Setup();
        }
        public void Update()
        {
            CrimesViolating.RemoveAll(x => !x.IsTrafficViolation);
            if (Player.IsAliveAndFree && Player.ShouldCheckViolations)
            {
                WeaponViolations.Update();
                GameFiber.Yield();
                TheftViolations.Update();
                OtherViolations.Update();
                DamageViolations.Update();
                MinorViolations.Update();
                HandleContinuoslyViolating();
                AddObservedAndReported();
            }
        }

        private void HandleContinuoslyViolating()
        {
            foreach(string crimeID in ContinuouslyViolatingCrimes)
            {
                AddViolating(crimeID);
            }
        }

        public void Reset()
        {
            DamageViolations.Reset();
            WeaponViolations.Reset();
            TheftViolations.Reset();
            OtherViolations.Reset();
            CrimesViolating.RemoveAll(x => !x.IsTrafficViolation);
            TrafficViolations.Reset();
            MinorViolations.Reset();
            ContinuouslyViolatingCrimes.Clear();
        }
        public void Dispose()
        {
            TrafficViolations.Dispose();
            DamageViolations.Dispose();
            WeaponViolations.Dispose();
            TheftViolations.Dispose();
            OtherViolations.Dispose();
            MinorViolations.Dispose();
            ContinuouslyViolatingCrimes.Clear();
        }
        public void AddViolating(string crimeID)
        {
            Crime crime = Crimes.GetCrime(crimeID);
            if(crime == null || !crime.Enabled || Player.PoliceResponse.IsWithinGracePeriod(crime))
            {
                return;
            }
            if (Settings.SettingsManager.ViolationSettings.ShowCrimeWarnings && Player.IsAliveAndFree && Player.IsNotWanted)
            {
                crime.DisplayWarning();
            }
            CrimesViolating.Add(crime);        
        }
        public void SetContinuouslyViolating(string crimeID)
        {
            if(!ContinuouslyViolatingCrimes.Contains(crimeID))
            {
                ContinuouslyViolatingCrimes.Add(crimeID);
            }
        }
        public void StopContinuouslyViolating(string crimeID)
        {
            if (ContinuouslyViolatingCrimes.Contains(crimeID))
            {
                ContinuouslyViolatingCrimes.Remove(crimeID);
            }
        }
        public void AddViolatingAndObserved(string crimeID) //for when the cops find a gun on you
        {
            Crime crime = Crimes.GetCrime(crimeID);
            if(crime == null)
            {
                return;
            }
            Player.AddCrime(crime, true, Player.Position, Player.CurrentSeenVehicle, Player.WeaponEquipment.CurrentSeenWeapon, true, true, true);
        }
        private void AddObservedAndReported()
        {
            foreach (Crime Violating in CrimesViolating.ToList())//for when they see you doing stuffo
            {
                if (Player.AnyPoliceCanSeePlayer)
                {
                    Player.AddCrime(Violating, true, Player.Position, Player.CurrentSeenVehicle, Player.WeaponEquipment.CurrentSeenWeapon, true, true, true);
                }
            }
        }
    }
}
