using ExtensionsMethods;
using LosSantosRED.lsr.Data;
using LosSantosRED.lsr.Interface;
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
        private readonly List<Crime> CrimeList = new List<Crime>();
        private IViolateable Player;
        private float CurrentSpeed;
        private uint GameTimeLastHurtCivilian;
        private uint GameTimeLastHurtCop;
        private uint GameTimeLastKilledCivilian;
        private uint GameTimeLastKilledCop;
        private uint GameTimeStartedBrandishing;
        private uint GameTimeStartedDrivingAgainstTraffic;
        private uint GameTimeStartedDrivingOnPavement;
        private bool IsRunningRedLight;
        private List<PedExt> PlayerKilledCivilians = new List<PedExt>();
        private List<PedExt> PlayerKilledCops = new List<PedExt>();
        private ITimeReportable TimeReporter;
        private ICrimes Crimes;
        private int TimeSincePlayerHitPed;
        private int TimeSincePlayerHitVehicle;
        private bool TreatAsCop;
        private bool VehicleIsSuspicious;
        private readonly List<Crime> CrimesViolating = new List<Crime>();
        public Violations(IViolateable currentPlayer, ITimeReportable timeReporter, ICrimes crimes)
        {
            TimeReporter = timeReporter;
            Player = currentPlayer;
            Crimes = crimes;
            CrimeList = Crimes.CrimeList;
        }
        public List<Crime> CivilianReportableCrimesViolating => CrimesViolating.Where(x => x.CanBeReportedByCivilians).ToList();//CrimeList.Where(x => x.IsCurrentlyViolating && x.CanBeReportedByCivilians).ToList();
        public bool IsSpeeding { get; set; }
        public bool IsViolatingAnyTrafficLaws => HasBeenDrivingAgainstTraffic || HasBeenDrivingOnPavement || IsRunningRedLight || IsSpeeding || VehicleIsSuspicious;
        public string LawsViolatingDisplay => string.Join(",", CrimesViolating.Select(x => x.Name));
        public bool NearCivilianMurderVictim => PlayerKilledCivilians.Any(x => x.Pedestrian.Exists() && x.Pedestrian.DistanceTo2D(Player.Character) <= 9f);
        public bool RecentlyHurtCivilian => GameTimeLastHurtCivilian != 0 && Game.GameTime - GameTimeLastHurtCivilian <= 5000;
        public bool RecentlyHurtCop => GameTimeLastHurtCop != 0 && Game.GameTime - GameTimeLastHurtCop <= 5000;
        public bool RecentlyKilledCivilian => GameTimeLastKilledCivilian != 0 && Game.GameTime - GameTimeLastKilledCivilian <= 5000;
        public bool RecentlyKilledCop => GameTimeLastKilledCop != 0 && Game.GameTime - GameTimeLastKilledCop <= 5000;
        private bool HasBeenDrivingAgainstTraffic => GameTimeStartedDrivingAgainstTraffic != 0 && Game.GameTime - GameTimeStartedDrivingAgainstTraffic >= 1000;
        private bool HasBeenDrivingOnPavement => GameTimeStartedDrivingOnPavement != 0 && Game.GameTime - GameTimeStartedDrivingOnPavement >= 1000;
        private bool RecentlyHitPed => TimeSincePlayerHitPed > -1 && TimeSincePlayerHitPed <= 1000;
        private bool RecentlyHitVehicle => TimeSincePlayerHitVehicle > -1 && TimeSincePlayerHitVehicle <= 1000;
        private bool ShouldCheckTrafficViolations => Player.IsInVehicle && (Player.IsInAutomobile || Player.IsOnMotorcycle) && !Player.RecentlyStartedPlaying;
        public void AddInjured(PedExt myPed)
        {
            if (myPed.IsCop)
            {
                GameTimeLastHurtCop = Game.GameTime;
            }
            else
            {
                GameTimeLastHurtCivilian = Game.GameTime;
            }
        }
        public void AddKilled(PedExt myPed)
        {
            if (myPed.IsCop)
            {
                PlayerKilledCops.Add(myPed);
                GameTimeLastKilledCop = Game.GameTime;
                GameTimeLastHurtCop = Game.GameTime;
            }
            else
            {
                PlayerKilledCivilians.Add(myPed);
                GameTimeLastKilledCivilian = Game.GameTime;
                GameTimeLastHurtCivilian = Game.GameTime;
            }
        }
        public void Reset()
        {
            GameTimeLastHurtCivilian = 0;
            GameTimeLastKilledCivilian = 0;
            GameTimeLastHurtCop = 0;
            GameTimeLastKilledCop = 0;
            IsSpeeding = false;
            PlayerKilledCops.Clear();
            PlayerKilledCivilians.Clear();
            VehicleIsSuspicious = false;
            IsRunningRedLight = false;
            TimeSincePlayerHitPed = 0;
            TimeSincePlayerHitVehicle = 0;
            GameTimeStartedBrandishing = 0;
            ResetViolations();
        }
        public void TrafficUpdate()
        {
            ResetTrafficViolations();
            VehicleIsSuspicious = false;
            TreatAsCop = false;
            IsSpeeding = false;
            IsRunningRedLight = false;
            if (Player.IsAliveAndFree && ShouldCheckTrafficViolations)
            {
                CheckTrafficViolations();
            }
        }
        public void Update()
        {
            ResetViolations();
            if (Player.IsAliveAndFree)
            {
                ResetViolations();
                CheckViolations();
                AddCrimes();
            }
        }
        private void ResetViolations()
        {
            CrimesViolating.RemoveAll(x => !x.IsTrafficViolation);
            //CrimeList.ForEach(x => x.IsCurrentlyViolating = false);
        }
        private void ResetTrafficViolations()
        {
            CrimesViolating.RemoveAll(x => x.IsTrafficViolation);
            //foreach (Crime Traffic in CrimeList.Where(x => x.IsTrafficViolation))
            //{
            //    Traffic.IsCurrentlyViolating = false;
            //}
        }
        private void CheckViolations()
        {
            CheckPedDamageCrimes();
            CheckWeaponCrimes();
            CheckTheftCrimes();
            CheckOtherCrimes();
        }
        private void CheckPedDamageCrimes()
        {
            if (RecentlyKilledCop)
            {
                CrimesViolating.Add(CrimeList.FirstOrDefault(x => x.ID == "KillingPolice"));//.IsCurrentlyViolating = true;
            }

            if (RecentlyHurtCop)
            {
                CrimesViolating.Add(CrimeList.FirstOrDefault(x => x.ID == "HurtingPolice"));//.IsCurrentlyViolating = true;
            }

            if (RecentlyKilledCivilian || NearCivilianMurderVictim)
            {
                CrimesViolating.Add(CrimeList.FirstOrDefault(x => x.ID == "KillingCivilians"));//.IsCurrentlyViolating = true;
            }

            if (RecentlyHurtCivilian)
            {
                CrimesViolating.Add(CrimeList.FirstOrDefault(x => x.ID == "HurtingCivilians"));//.IsCurrentlyViolating = true;
            }
        }
        private void CheckWeaponCrimes()
        {
            if (Player.RecentlyShot)
            {
                if (!(Player.Character.IsCurrentWeaponSilenced || Player.CurrentWeaponCategory == WeaponCategory.Melee))
                {
                    CrimesViolating.Add(CrimeList.FirstOrDefault(x => x.ID == "FiringWeapon"));//.IsCurrentlyViolating = true;
                    if (Player.AnyPoliceRecentlySeenPlayer || Player.AnyPoliceCanHearPlayer)
                    {
                        CrimesViolating.Add(CrimeList.FirstOrDefault(x => x.ID == "FiringWeaponNearPolice"));//.IsCurrentlyViolating = true;
                    }
                }
            }
            if (CheckBrandishing() && Player.Character.Inventory.EquippedWeapon != null && !Player.IsInVehicle)
            {
                CrimesViolating.Add(CrimeList.FirstOrDefault(x => x.ID == "BrandishingWeapon"));//.IsCurrentlyViolating = true;
                if (Player.CurrentWeapon != null && Player.CurrentWeapon.WeaponLevel >= 4)
                {
                    CrimesViolating.Add(CrimeList.FirstOrDefault(x => x.ID == "TerroristActivity"));//.IsCurrentlyViolating = true;
                }
                if (Player.CurrentWeapon != null && Player.CurrentWeapon.WeaponLevel >= 3)
                {
                    CrimesViolating.Add(CrimeList.FirstOrDefault(x => x.ID == "BrandishingHeavyWeapon"));//.IsCurrentlyViolating = true;
                }
                if (Player.CurrentWeapon != null && Player.CurrentWeapon.Category == WeaponCategory.Melee)
                {
                    CrimesViolating.Add(CrimeList.FirstOrDefault(x => x.ID == "BrandishingCloseCombatWeapon"));//.IsCurrentlyViolating = true;
                }
            }
        }
        private bool CheckBrandishing()
        {
            if (Player.IsVisiblyArmed)
            {
                if (GameTimeStartedBrandishing == 0)
                {
                    GameTimeStartedBrandishing = Game.GameTime;
                }
            }
            else
            {
                GameTimeStartedBrandishing = 0;
            }

            if (GameTimeStartedBrandishing > 0 && Game.GameTime - GameTimeStartedBrandishing >= 1500)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void CheckTheftCrimes()
        {
            if (Player.IsWanted && Player.IsInVehicle && Player.IsInAirVehicle)
            {
                CrimesViolating.Add(CrimeList.FirstOrDefault(x => x.ID == "GotInAirVehicleDuringChase"));//.IsCurrentlyViolating = true;
            }
            if (Player.CurrentVehicle != null && Player.CurrentVehicle.CopsRecognizeAsStolen && Player.IsInVehicle)
            {
                CrimesViolating.Add(CrimeList.FirstOrDefault(x => x.ID == "DrivingStolenVehicle"));//.IsCurrentlyViolating = true;
            }
            if (Player.IsHoldingUp)
            {
                CrimesViolating.Add(CrimeList.FirstOrDefault(x => x.ID == "Mugging"));//.IsCurrentlyViolating = true;
            }
            if (Player.IsBreakingIntoCar)
            {
                CrimesViolating.Add(CrimeList.FirstOrDefault(x => x.ID == "GrandTheftAuto"));//.IsCurrentlyViolating = true;
            }
            if (Player.IsChangingLicensePlates)
            {
                CrimesViolating.Add(CrimeList.FirstOrDefault(x => x.ID == "ChangingPlates"));//.IsCurrentlyViolating = true;
            }
        }
        private void CheckOtherCrimes()
        {
            if (Player.IsCommitingSuicide)
            {
                CrimesViolating.Add(CrimeList.FirstOrDefault(x=> x.ID == "AttemptingSuicide"));//.IsCurrentlyViolating = true;
            }
            if (Player.IsWanted && Player.CurrentLocation.CurrentZone.IsRestrictedDuringWanted)
            {
                CrimesViolating.Add(CrimeList.FirstOrDefault(x => x.ID == "TrespessingOnGovtProperty"));//.IsCurrentlyViolating = true;
            }
            if (Player.Investigation.IsSuspicious)
            {
                CrimesViolating.Add(CrimeList.FirstOrDefault(x => x.ID == "SuspiciousActivity"));//.IsCurrentlyViolating = true;
            }
            if (Player.IsWanted && Player.AnyPoliceRecentlySeenPlayer && Player.Character.Speed >= 2.0f && !Player.HandsAreUp && Player.PoliceResponse.HasBeenWantedFor >= 20000)
            {
                CrimesViolating.Add(CrimeList.FirstOrDefault(x => x.ID == "ResistingArrest"));//.IsCurrentlyViolating = true;
            }

            if (Player.IsInVehicle && Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists() && Player.CurrentVehicle.Vehicle.HasPassengers && Player.CurrentVehicle.Vehicle.Passengers.Any(x => x.Group != Game.LocalPlayer.Character.Group))
            {
                CrimesViolating.Add(CrimeList.FirstOrDefault(x => x.ID == "Kidnapping"));//.IsCurrentlyViolating = true;
            }
            if (Player.IsIntoxicated && Player.IntoxicatedIntensity >= 2.0f && !Player.IsInVehicle)
            {
                CrimesViolating.Add(CrimeList.FirstOrDefault(x => x.ID == "PublicIntoxication"));//.IsCurrentlyViolating = true;
            }
        }
        private void CheckTrafficViolations()
        {
            bool isDrivingSuspiciously = false;
            UpdateTrafficStats();
            if (RecentlyHitPed && (RecentlyHurtCivilian || RecentlyHurtCop) && Player.AnyHumansNear)//needed for non humans that are returned from this native
            {
                isDrivingSuspiciously = true;
                CrimesViolating.Add(CrimeList.FirstOrDefault(x => x.ID == "HitPedWithCar"));//.IsCurrentlyViolating = true;
            }
            if (RecentlyHitVehicle)
            {
                isDrivingSuspiciously = true;
                CrimesViolating.Add(CrimeList.FirstOrDefault(x => x.ID == "HitCarWithCar"));//.IsCurrentlyViolating = true;
            }
            if (!TreatAsCop)
            {
                if ((HasBeenDrivingAgainstTraffic || (Game.LocalPlayer.IsDrivingAgainstTraffic && Player.Character.CurrentVehicle.Speed >= 10f)))
                {
                    isDrivingSuspiciously = true;
                    CrimesViolating.Add(CrimeList.FirstOrDefault(x => x.ID == "DrivingAgainstTraffic"));//.IsCurrentlyViolating = true;
                }
                if ((HasBeenDrivingOnPavement || (Game.LocalPlayer.IsDrivingOnPavement && Player.Character.CurrentVehicle.Speed >= 10f)))
                {
                    isDrivingSuspiciously = true;
                    CrimesViolating.Add(CrimeList.FirstOrDefault(x => x.ID == "DrivingOnPavement"));//.IsCurrentlyViolating = true;
                }
                if (VehicleIsSuspicious)
                {
                    isDrivingSuspiciously = true;
                    CrimesViolating.Add(CrimeList.FirstOrDefault(x => x.ID == "NonRoadworthyVehicle"));//.IsCurrentlyViolating = true;
                }
                if (IsSpeeding)
                {
                    isDrivingSuspiciously = true;
                    CrimesViolating.Add(CrimeList.FirstOrDefault(x => x.ID == "FelonySpeeding"));//.IsCurrentlyViolating = true;
                }
            }
            if (Player.IsIntoxicated && isDrivingSuspiciously)// DrivingAgainstTraffic.IsCurrentlyViolating || DrivingOnPavement.IsCurrentlyViolating || FelonySpeeding.IsCurrentlyViolating || RunningARedLight.IsCurrentlyViolating || HitPedWithCar.IsCurrentlyViolating || HitCarWithCar.IsCurrentlyViolating))
            {
                CrimesViolating.Add(CrimeList.FirstOrDefault(x => x.ID == "DrunkDriving"));//.IsCurrentlyViolating = true;
            }
        }
        private void UpdateTrafficStats()
        {
            VehicleIsSuspicious = false;
            TreatAsCop = false;
            IsSpeeding = false;
            if (Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
            {
                CurrentSpeed = Player.CurrentVehicle.Vehicle.Speed * 2.23694f;
                if (!IsRoadWorthy(Player.CurrentVehicle.Vehicle) || IsDamaged(Player.CurrentVehicle.Vehicle))
                {
                    VehicleIsSuspicious = true;
                }
                //if (DataMart.Instance.Settings.SettingsManager.TrafficViolations.ExemptCode3 && CurrentPlayer.CurrentVehicle.Vehicle != null && CurrentPlayer.CurrentVehicle.Vehicle.IsPoliceVehicle && CurrentPlayer.CurrentVehicle != null && !CurrentPlayer.CurrentVehicle.WasReportedStolen)
                //{
                //    if (CurrentPlayer.CurrentVehicle.Vehicle.IsSirenOn && !World.AnyPoliceCanRecognizePlayer) //see thru ur disguise if ur too close
                //    {
                //        TreatAsCop = true;//Cops dont have to do traffic laws stuff if ur running code3?
                //    }
                //}
                IsRunningRedLight = false;
                //foreach (PedExt Civilian in World.Pedestrians.Civilians.Where(x => x.Pedestrian.Exists()).OrderBy(x => x.DistanceToPlayer))
                //{
                //    Civilian.IsWaitingAtTrafficLight = false;
                //    Civilian.IsFirstWaitingAtTrafficLight = false;
                //    Civilian.PlaceCheckingInfront = Vector3.Zero;
                //    if (Civilian.DistanceToPlayer <= 250f && Civilian.IsInVehicle)
                //    {
                //        if (Civilian.Pedestrian.IsInAnyVehicle(false) && Civilian.Pedestrian.CurrentVehicle != null)
                //        {
                //            Vehicle PedCar = Civilian.Pedestrian.CurrentVehicle;
                //            if (NativeFunction.CallByName<bool>("IS_VEHICLE_STOPPED_AT_TRAFFIC_LIGHTS", PedCar))
                //            {
                //                Civilian.IsWaitingAtTrafficLight = true;

                //                if (Extensions.FacingSameOrOppositeDirection(Civilian.Pedestrian, Game.LocalPlayer.Character) && Game.LocalPlayer.Character.InFront(Civilian.Pedestrian) && Civilian.DistanceToPlayer <= 10f && Game.LocalPlayer.Character.Speed >= 3f)
                //                {
                //                    GameTimeLastRanRed = Game.GameTime;
                //                    PlayerIsRunningRedLight = true;
                //                }
                //            }
                //        }
                //    }
                //}
                if (Game.LocalPlayer.IsDrivingOnPavement && GameTimeStartedDrivingOnPavement == 0)
                {
                    GameTimeStartedDrivingOnPavement = Game.GameTime;
                }
                else
                {
                    GameTimeStartedDrivingOnPavement = 0;
                }
                if (Game.LocalPlayer.IsDrivingAgainstTraffic && GameTimeStartedDrivingAgainstTraffic == 0)
                {
                    GameTimeStartedDrivingAgainstTraffic = Game.GameTime;
                }
                else
                {
                    GameTimeStartedDrivingAgainstTraffic = 0;
                }
                TimeSincePlayerHitPed = Game.LocalPlayer.TimeSincePlayerLastHitAnyPed;
                TimeSincePlayerHitVehicle = Game.LocalPlayer.TimeSincePlayerLastHitAnyVehicle;
                float SpeedLimit = 60f;
                if (Player.CurrentLocation.CurrentStreet != null)
                {
                    SpeedLimit = Player.CurrentLocation.CurrentStreet.SpeedLimit;
                }
                IsSpeeding = CurrentSpeed > SpeedLimit + 25f;
            }
        }
        private void AddCrimes()
        {
            foreach (Crime Violating in CrimesViolating)
            {
                if (Player.AnyPoliceCanSeePlayer || (Violating.CanReportBySound && Player.AnyPoliceCanHearPlayer) || Violating.CanViolateWithoutPerception)
                {
                    Player.AddCrime(Violating, true, Player.Position, Player.CurrentSeenVehicle, Player.CurrentSeenWeapon, true, true);
                }
            }
        }
        private bool IsDamaged(Vehicle myCar)
        {
            if (!myCar.Exists())
            {
                return false;
            }
            if (myCar.Health <= 700 || myCar.EngineHealth <= 700)
            {
                return true;
            }
            if (!NativeFunction.CallByName<bool>("ARE_ALL_VEHICLE_WINDOWS_INTACT", myCar))
            {
                return true;
            }
            foreach (VehicleDoor myDoor in myCar.GetDoors())
            {
                if (myDoor.IsDamaged)
                {
                    return true;
                }
            }
            if (TimeReporter.IsNight)
            {
                if (myCar.IsCar && NativeFunction.CallByName<bool>("GET_IS_RIGHT_VEHICLE_HEADLIGHT_DAMAGED", myCar) || NativeFunction.CallByName<bool>("GET_IS_LEFT_VEHICLE_HEADLIGHT_DAMAGED", myCar))
                {
                    return true;
                }
            }
            if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar, 0, false))
            {
                return true;
            }
            if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar, 1, false))
            {
                return true;
            }
            if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar, 2, false))
            {
                return true;
            }
            if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar, 3, false))
            {
                return true;
            }
            if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar, 4, false))
            {
                return true;
            }
            if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar, 5, false))
            {
                return true;
            }
            return false;
        }
        private bool IsRoadWorthy(Vehicle myCar)
        {
            bool LightsOn;
            bool HighbeamsOn;
            if (TimeReporter.IsNight)
            {
                unsafe
                {
                    NativeFunction.CallByName<bool>("GET_VEHICLE_LIGHTS_STATE", myCar, &LightsOn, &HighbeamsOn);
                }
                if (!LightsOn)
                {
                    return false;
                }
                if (HighbeamsOn)
                {
                    return false;
                }
                if (myCar.IsCar && NativeFunction.CallByName<bool>("GET_IS_RIGHT_VEHICLE_HEADLIGHT_DAMAGED", myCar) || NativeFunction.CallByName<bool>("GET_IS_LEFT_VEHICLE_HEADLIGHT_DAMAGED", myCar))
                {
                    return false;
                }
            }
            if (myCar.LicensePlate == "        ")
            {
                return false;
            }
            return true;
        }
    }
}
