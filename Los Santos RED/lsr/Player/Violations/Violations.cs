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
        private IViolateable Player;
        private ITimeReportable TimeReporter;
        private ICrimes Crimes;
        private ISettingsProvideable Settings;
        private IZones Zones;
        private IGangTerritories GangTerritories;
        private IEntityProvideable World;
        public Violations(IViolateable currentPlayer, ITimeReportable timeReporter, ICrimes crimes, ISettingsProvideable settings, IZones zones, IGangTerritories gangTerritories, IEntityProvideable world)
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
            OtherViolations = new OtherViolations(Player, this, Settings, TimeReporter);
        }
        public readonly List<Crime> CrimesViolating = new List<Crime>();
        public TrafficViolations TrafficViolations { get; private set; }
        public DamageViolations DamageViolations { get; private set; }
        public WeaponViolations WeaponViolations { get; private set; }
        public TheftViolations TheftViolations { get; private set; }
        public OtherViolations OtherViolations { get; private set; }
        public List<Crime> CivilianReportableCrimesViolating => CrimesViolating.Where(x => x.CanBeReactedToByCivilians).ToList();
        public string LawsViolatingDisplay => string.Join(", ", CrimesViolating.OrderBy(x=>x.Priority).Select(x => x.Name));
        public void Setup()
        {
            TrafficViolations.Setup();
            DamageViolations.Setup();
            WeaponViolations.Setup();
            TheftViolations.Setup();
            OtherViolations.Setup();
        }
        public void Update()
        {
            CrimesViolating.RemoveAll(x => !x.IsTrafficViolation);
            if (Player.IsAliveAndFree && Player.ShouldCheckViolations)
            {
                DamageViolations.Update();
                //GameFiber.Yield();
                WeaponViolations.Update();
                GameFiber.Yield();
                TheftViolations.Update();
                //GameFiber.Yield();
                OtherViolations.Update();
                //GameFiber.Yield();
                AddObservedAndReported();
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
        }
        public void Dispose()
        {
            TrafficViolations.Dispose();
            DamageViolations.Dispose();
            WeaponViolations.Dispose();
            TheftViolations.Dispose();
            OtherViolations.Dispose();
        }
        public void AddViolating(string crimeID)
        {
            Crime crime = Crimes.GetCrime(crimeID);
            if (crime != null && crime.Enabled && !Player.PoliceResponse.IsWithinGracePeriod(crime))
            {
                if (Settings.SettingsManager.ViolationSettings.ShowCrimeWarnings && Player.IsAliveAndFree && Player.IsNotWanted)
                {
                    crime.DisplayWarning();
                }
                CrimesViolating.Add(crime);
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
                if (Player.AnyPoliceCanSeePlayer || (Violating.CanReportBySound && Player.AnyPoliceCanHearPlayer && Player.ClosestPoliceDistanceToPlayer <= Violating.MaxHearingDistance) || Violating.CanViolateWithoutPerception)
                {
                    Player.AddCrime(Violating, true, Player.Position, Player.CurrentSeenVehicle, Player.WeaponEquipment.CurrentSeenWeapon, true, true, true);
                    //EntryPoint.WriteToConsole($"AddObservedAndReported {Violating.ID} AnyPoliceCanSeePlayer:{Player.AnyPoliceCanSeePlayer} AnyPoliceCanHearPlayer:{Player.AnyPoliceCanHearPlayer} CanReportBySound:{Violating.CanReportBySound} CanViolateWithoutPerception:{Violating.CanViolateWithoutPerception}");
                }
            }
        }
    }
}
