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
        public readonly List<Crime> CrimeList = new List<Crime>();
        public readonly Crime AimingWeaponAtPolice = new Crime("AimingWeaponAtPolice", "Aiming Weapons At Police", 3, false, 4, 1, false);
        public readonly Crime AttemptingSuicide = new Crime("AttemptingSuicide", "Attempting Suicide", 2, false, 12, 3);
        public readonly Crime BrandishingCloseCombatWeapon = new Crime("BrandishingCloseCombatWeapon", "Brandishing Close Combat Weapon", 1, false, 20, 4, true, true, true);
        public readonly Crime BrandishingHeavyWeapon = new Crime("BrandishingHeavyWeapon", "Brandishing Heavy Weapon", 3, false, 6, 1, false, true, true);
        public readonly Crime BrandishingWeapon = new Crime("BrandishingWeapon", "Brandishing Weapon", 2, false, 18, 3, true, true, true);
        public readonly Crime ChangingPlates = new Crime("ChangingPlates", "Stealing License Plates", 1, false, 31, 4, true, true, false);       
        public readonly Crime DrivingAgainstTraffic = new Crime("DrivingAgainstTraffic", "Driving Against Traffic", 1, false, 32, 4, false, false, false) { IsTrafficViolation = true };
        public readonly Crime DrivingOnPavement = new Crime("DrivingOnPavement", "Driving On Pavement", 1, false, 33, 4, false, false, false) { IsTrafficViolation = true };
        public readonly Crime DrivingStolenVehicle = new Crime("DrivingStolenVehicle", "Driving a Stolen Vehicle", 2, false, 38, 5, false);
        public readonly Crime DrunkDriving = new Crime("DrunkDriving", "Drunk Driving", 2, false, 30, 4, false, false, false);
        public readonly Crime FelonySpeeding = new Crime("FelonySpeeding", "Speeding", 1, false, 37, 5, false, false, false) { IsTrafficViolation = true };
        public readonly Crime FiringWeapon = new Crime("FiringWeapon", "Firing Weapon", 2, false, 9, 2, true, true, true) { CanReportBySound = true };
        public readonly Crime FiringWeaponNearPolice = new Crime("FiringWeaponNearPolice", "Shots Fired at Police", 3, true, 3, 1, false) { CanReportBySound = true };
        public readonly Crime GotInAirVehicleDuringChase = new Crime("GotInAirVehicleDuringChase", "Stealing an Air Vehicle", 3, false, 8, 2);
        public readonly Crime GrandTheftAuto = new Crime("GrandTheftAuto", "Grand Theft Auto", 2, false, 16, 3, true, true, true);
        public readonly Crime Harassment = new Crime("Harassment", "Harassment", 1, false, 41, 5, false);
        public readonly Crime HitCarWithCar = new Crime("HitCarWithCar", "Hit and Run", 1, false, 30, 4, true, true, true) { IsTrafficViolation = true };
        public readonly Crime HitPedWithCar = new Crime("HitPedWithCar", "Pedestrian Hit and Run", 2, false, 15, 3, true, true, true) { IsTrafficViolation = true };
        public readonly Crime HurtingCivilians = new Crime("HurtingCivilians", "Assaulting Civilians", 2, false, 14, 3, true, true, true);
        public readonly Crime HurtingPolice = new Crime("HurtingPolice", "Assaulting Police", 3, false, 5, 1);
        public readonly Crime InsultingOfficer = new Crime("InsultingOfficer", "Insulting a Police Officer", 1, false, 40, 5);
        public readonly Crime Kidnapping = new Crime("Kidnapping", "Kidnapping", 2, false, 10, 2, false, false, false);
        public readonly Crime KillingCivilians = new Crime("KillingCivilians", "Civilian Fatality", 2, false, 11, 2, true, true, true);
        public readonly Crime KillingPolice = new Crime("KillingPolice", "Police Fatality", 3, true, 1, 1, false) { CanViolateWithoutPerception = true };
        public readonly Crime Mugging = new Crime("Mugging", "Mugging", 2, false, 11, 2, true, true, true);
        public readonly Crime NonRoadworthyVehicle = new Crime("NonRoadworthyVehicle", "NonRoadworthy Vehicle", 1, false, 34, 4, false, false, false) { IsTrafficViolation = true };
        public readonly Crime PublicIntoxication = new Crime("PublicIntoxication", "Public Intoxication", 1, false, 31, 4, true, false, false);
        public readonly Crime ResistingArrest = new Crime("ResistingArrest", "Resisting Arrest", 2, false, 19, 4, false) { CanViolateWithoutPerception = true };
        public readonly Crime RunningARedLight = new Crime("RunningARedLight", "Running a Red Light", 1, false, 36, 5, false, false, false) { IsTrafficViolation = true };
        public readonly Crime SuspiciousActivity = new Crime("SuspiciousActivity", "Suspicious Activity", 1, false, 39, 5, false);
        public readonly Crime TerroristActivity = new Crime("TerroristActivity", "Terrorist Activity", 4, true, 2, 1) { CanReportBySound = true };
        public readonly Crime TrespessingOnGovtProperty = new Crime("TrespessingOnGovtProperty", "Trespassing on Government Property", 3, false, 7, 2, false);
        private IViolateable CurrentPlayer;
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
        private int TimeSincePlayerHitPed;
        private int TimeSincePlayerHitVehicle;
        private bool TreatAsCop;
        private bool VehicleIsSuspicious;
        public Violations(IViolateable currentPlayer, ITimeReportable timeReporter)
        {
            TimeReporter = timeReporter;
            CurrentPlayer = currentPlayer;
            CrimeList = new List<Crime>
                {
                    BrandishingCloseCombatWeapon,TerroristActivity,BrandishingHeavyWeapon, FiringWeapon, Mugging, AttemptingSuicide, ResistingArrest, KillingPolice, FiringWeaponNearPolice, AimingWeaponAtPolice, HurtingPolice,
                TrespessingOnGovtProperty, GotInAirVehicleDuringChase, KillingCivilians, BrandishingWeapon,ChangingPlates, GrandTheftAuto, DrivingStolenVehicle, HurtingCivilians, SuspiciousActivity,DrivingAgainstTraffic,
                DrivingOnPavement,NonRoadworthyVehicle,FelonySpeeding,RunningARedLight,HitPedWithCar,HitCarWithCar,DrunkDriving,Kidnapping,PublicIntoxication,InsultingOfficer
                };

        }
        public List<Crime> CivilianReportableCrimesViolating => CrimeList.Where(x => x.IsCurrentlyViolating && x.CanBeReportedByCivilians).ToList();
        public bool IsSpeeding { get; set; }
        public bool IsViolatingAnyTrafficLaws => HasBeenDrivingAgainstTraffic || HasBeenDrivingOnPavement || IsRunningRedLight || IsSpeeding || VehicleIsSuspicious;
        public string LawsViolatingDisplay => string.Join(",", CrimeList.Where(x => x.IsCurrentlyViolating).Select(x => x.Name));
        public bool NearCivilianMurderVictim => PlayerKilledCivilians.Any(x => x.Pedestrian.Exists() && x.Pedestrian.DistanceTo2D(Game.LocalPlayer.Character) <= 9f);
        public bool RecentlyHurtCivilian => GameTimeLastHurtCivilian != 0 && Game.GameTime - GameTimeLastHurtCivilian <= 5000;
        public bool RecentlyHurtCop => GameTimeLastHurtCop != 0 && Game.GameTime - GameTimeLastHurtCop <= 5000;
        public bool RecentlyKilledCivilian => GameTimeLastKilledCivilian != 0 && Game.GameTime - GameTimeLastKilledCivilian <= 5000;
        public bool RecentlyKilledCop => GameTimeLastKilledCop != 0 && Game.GameTime - GameTimeLastKilledCop <= 5000;
        private bool HasBeenDrivingAgainstTraffic => GameTimeStartedDrivingAgainstTraffic != 0 && Game.GameTime - GameTimeStartedDrivingAgainstTraffic >= 1000;
        private bool HasBeenDrivingOnPavement => GameTimeStartedDrivingOnPavement != 0 && Game.GameTime - GameTimeStartedDrivingOnPavement >= 1000;
        private bool RecentlyHitPed => TimeSincePlayerHitPed > -1 && TimeSincePlayerHitPed <= 1000;
        private bool RecentlyHitVehicle => TimeSincePlayerHitVehicle > -1 && TimeSincePlayerHitVehicle <= 1000;
        private bool ShouldCheckTrafficViolations => CurrentPlayer.IsInVehicle && (CurrentPlayer.IsInAutomobile || CurrentPlayer.IsOnMotorcycle) && !CurrentPlayer.RecentlyStartedPlaying;
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
        }
        public void TrafficUpdate()
        {
            if (CurrentPlayer.IsAliveAndFree && ShouldCheckTrafficViolations)
            {
                CheckTrafficViolations();
            }
            else
            {
                foreach (Crime Traffic in CrimeList.Where(x => x.IsTrafficViolation))
                {
                    Traffic.IsCurrentlyViolating = false;
                }
                VehicleIsSuspicious = false;
                TreatAsCop = false;
                IsSpeeding = false;
                IsRunningRedLight = false;
            }
        }
        public void Update()
        {
            if (CurrentPlayer.IsAliveAndFree)
            {
                CheckViolations();
                FlagViolations();
            }
            else
            {
                ResetViolations();
            }
        }
        private bool CheckBrandishing()
        {
            if (CurrentPlayer.IsVisiblyArmed)
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
        private void CheckOtherCrimes()
        {
            if (CurrentPlayer.IsCommitingSuicide)
            {
                AttemptingSuicide.IsCurrentlyViolating = true;
            }
            else
            {
                AttemptingSuicide.IsCurrentlyViolating = false;
            }
            if (CurrentPlayer.IsWanted && CurrentPlayer.CurrentLocation.CurrentZone.IsRestrictedDuringWanted)
            {
                TrespessingOnGovtProperty.IsCurrentlyViolating = true;
            }
            else
            {
                TrespessingOnGovtProperty.IsCurrentlyViolating = false;
            }
            if (CurrentPlayer.Investigation.IsSuspicious)
            {
                SuspiciousActivity.IsCurrentlyViolating = true;
            }
            else
            {
                SuspiciousActivity.IsCurrentlyViolating = false;
            }
            if (CurrentPlayer.IsWanted && CurrentPlayer.AnyPoliceSeenPlayerCurrentWanted && CurrentPlayer.Character.Speed >= 2.0f && !CurrentPlayer.HandsAreUp && CurrentPlayer.PoliceResponse.HasBeenWantedFor >= 20000)
            {
                ResistingArrest.IsCurrentlyViolating = true;
            }
            else
            {
                ResistingArrest.IsCurrentlyViolating = false;
            }

            if (CurrentPlayer.IsInVehicle && CurrentPlayer.CurrentVehicle != null && CurrentPlayer.CurrentVehicle.Vehicle.Exists() && CurrentPlayer.CurrentVehicle.Vehicle.HasPassengers && CurrentPlayer.CurrentVehicle.Vehicle.Passengers.Any(x => x.Group != Game.LocalPlayer.Character.Group))
            {
                Kidnapping.IsCurrentlyViolating = true;
            }
            else
            {
                Kidnapping.IsCurrentlyViolating = false;
            }
            if (CurrentPlayer.IsIntoxicated && CurrentPlayer.IntoxicatedIntensity >= 2.0f && !CurrentPlayer.IsInVehicle)
            {
                PublicIntoxication.IsCurrentlyViolating = true;
            }
            else
            {
                PublicIntoxication.IsCurrentlyViolating = false;
            }
        }
        private void CheckPedDamageCrimes()
        {
            if (RecentlyKilledCop)
            {
                KillingPolice.IsCurrentlyViolating = true;
            }
            else
            {
                KillingPolice.IsCurrentlyViolating = false;
            }

            if (RecentlyHurtCop)
            {
                HurtingPolice.IsCurrentlyViolating = true;
            }
            else
            {
                HurtingPolice.IsCurrentlyViolating = false;
            }

            if (RecentlyKilledCivilian || NearCivilianMurderVictim)
            {
                KillingCivilians.IsCurrentlyViolating = true;
            }
            else
            {
                KillingCivilians.IsCurrentlyViolating = false;
            }

            if (RecentlyHurtCivilian)
            {
                HurtingCivilians.IsCurrentlyViolating = true;
            }
            else
            {
                HurtingCivilians.IsCurrentlyViolating = false;
            }
        }
        private void CheckTheftCrimes()
        {
            if (CurrentPlayer.IsWanted && CurrentPlayer.IsInVehicle && CurrentPlayer.IsInAirVehicle)
            {
                GotInAirVehicleDuringChase.IsCurrentlyViolating = true;
            }
            else
            {
                GotInAirVehicleDuringChase.IsCurrentlyViolating = false;
            }
            if (CurrentPlayer.CurrentVehicle != null && CurrentPlayer.CurrentVehicle.CopsRecognizeAsStolen)
            {
                DrivingStolenVehicle.IsCurrentlyViolating = true;
            }
            else
            {
                DrivingStolenVehicle.IsCurrentlyViolating = false;
            }
            if (CurrentPlayer.IsHoldingUp)
            {
                Mugging.IsCurrentlyViolating = true;
            }
            else
            {
                Mugging.IsCurrentlyViolating = false;
            }
            if (CurrentPlayer.IsBreakingIntoCar)
            {
                GrandTheftAuto.IsCurrentlyViolating = true;
            }
            else
            {
                GrandTheftAuto.IsCurrentlyViolating = false;
            }

            if (CurrentPlayer.IsChangingLicensePlates)
            {
                ChangingPlates.IsCurrentlyViolating = true;
            }
            else
            {
                ChangingPlates.IsCurrentlyViolating = false;
            }
        }
        private void CheckTrafficViolations()
        {
            UpdateTrafficStats();
            if (RecentlyHitPed && (RecentlyHurtCivilian || RecentlyHurtCop) && CurrentPlayer.AnyHumansNear)//needed for non humans that are returned from this native
            {
                HitPedWithCar.IsCurrentlyViolating = true;
            }
            else
            {
                HitPedWithCar.IsCurrentlyViolating = false;
            }
            if (RecentlyHitVehicle)
            {
                HitCarWithCar.IsCurrentlyViolating = true;
            }
            else
            {
                HitCarWithCar.IsCurrentlyViolating = false;
            }
            if (!TreatAsCop)
            {
                if ((HasBeenDrivingAgainstTraffic || (Game.LocalPlayer.IsDrivingAgainstTraffic && CurrentPlayer.Character.CurrentVehicle.Speed >= 10f)))
                {
                    DrivingAgainstTraffic.IsCurrentlyViolating = true;
                }
                else
                {
                    DrivingAgainstTraffic.IsCurrentlyViolating = false;
                }
                if ((HasBeenDrivingOnPavement || (Game.LocalPlayer.IsDrivingOnPavement && CurrentPlayer.Character.CurrentVehicle.Speed >= 10f)))
                {
                    DrivingOnPavement.IsCurrentlyViolating = true;
                }
                else
                {
                    DrivingOnPavement.IsCurrentlyViolating = false;
                }

                if (VehicleIsSuspicious)
                {
                    NonRoadworthyVehicle.IsCurrentlyViolating = true;
                }
                else
                {
                    NonRoadworthyVehicle.IsCurrentlyViolating = false;
                }

                if (IsSpeeding)
                {
                    FelonySpeeding.IsCurrentlyViolating = true;
                }
                else
                {
                    FelonySpeeding.IsCurrentlyViolating = false;
                }
                //if (DataMart.Instance.Settings.SettingsManager.TrafficViolations.RunningRedLight)//Off for now
                //{
                //    //RunningARedLight.IsCurrentlyViolating = true;//turned off for now until i fix it
                //}
                //else
                //{
                RunningARedLight.IsCurrentlyViolating = false;
                //}
            }
            if (CurrentPlayer.IsIntoxicated && CurrentPlayer.IntoxicatedIntensity >= 4.0f && (DrivingAgainstTraffic.IsCurrentlyViolating || DrivingOnPavement.IsCurrentlyViolating || FelonySpeeding.IsCurrentlyViolating || RunningARedLight.IsCurrentlyViolating || HitPedWithCar.IsCurrentlyViolating || HitCarWithCar.IsCurrentlyViolating))
            {
                DrunkDriving.IsCurrentlyViolating = true;
            }
            else
            {
                DrunkDriving.IsCurrentlyViolating = false;
            }
        }
        private void CheckViolations()
        {
            CheckPedDamageCrimes();
            CheckWeaponCrimes();
            CheckTheftCrimes();
            CheckOtherCrimes();
        }
        private void CheckWeaponCrimes()
        {
            if (CurrentPlayer.RecentlyShot)
            {
                if (!(CurrentPlayer.Character.IsCurrentWeaponSilenced || CurrentPlayer.CurrentWeaponCategory == WeaponCategory.Melee))
                {
                    FiringWeapon.IsCurrentlyViolating = true;
                    if (CurrentPlayer.AnyPoliceRecentlySeenPlayer || CurrentPlayer.AnyPoliceCanHearPlayer)
                    {
                        FiringWeaponNearPolice.IsCurrentlyViolating = true;
                    }
                }
            }
            else
            {
                FiringWeapon.IsCurrentlyViolating = false;
                FiringWeaponNearPolice.IsCurrentlyViolating = false;
            }
            if (CheckBrandishing() && CurrentPlayer.Character.Inventory.EquippedWeapon != null && !CurrentPlayer.IsInVehicle)
            {
                BrandishingWeapon.IsCurrentlyViolating = true;
                if (CurrentPlayer.CurrentWeapon != null && CurrentPlayer.CurrentWeapon.WeaponLevel >= 4)
                {
                    TerroristActivity.IsCurrentlyViolating = true;
                }
                else
                {
                    TerroristActivity.IsCurrentlyViolating = false;
                }
                if (CurrentPlayer.CurrentWeapon != null && CurrentPlayer.CurrentWeapon.WeaponLevel >= 3)
                {
                    BrandishingHeavyWeapon.IsCurrentlyViolating = true;
                }
                else
                {
                    BrandishingHeavyWeapon.IsCurrentlyViolating = false;
                }
                if (CurrentPlayer.CurrentWeapon != null && CurrentPlayer.CurrentWeapon.Category == WeaponCategory.Melee)
                {
                    BrandishingCloseCombatWeapon.IsCurrentlyViolating = true;
                }
                else
                {
                    BrandishingCloseCombatWeapon.IsCurrentlyViolating = false;
                }
            }
            else
            {
                BrandishingCloseCombatWeapon.IsCurrentlyViolating = false;
                BrandishingWeapon.IsCurrentlyViolating = false;
                TerroristActivity.IsCurrentlyViolating = false;
                BrandishingHeavyWeapon.IsCurrentlyViolating = false;
            }
        }
        private void FlagViolations()
        {
            foreach (Crime Violating in CrimeList.Where(x => x.IsCurrentlyViolating))
            {
                if (CurrentPlayer.AnyPoliceCanSeePlayer || (Violating.CanReportBySound && CurrentPlayer.AnyPoliceCanHearPlayer) || Violating.CanViolateWithoutPerception)
                {
                    CurrentPlayer.AddCrime(Violating, true, CurrentPlayer.Position, CurrentPlayer.CurrentSeenVehicle, CurrentPlayer.CurrentSeenWeapon, true);
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
        private void ResetViolations()
        {
            CrimeList.ForEach(x => x.IsCurrentlyViolating = false);
        }
        private void UpdateTrafficStats()
        {
            VehicleIsSuspicious = false;
            TreatAsCop = false;
            IsSpeeding = false;
            if (CurrentPlayer.CurrentVehicle != null && CurrentPlayer.CurrentVehicle.Vehicle.Exists())
            {
                CurrentSpeed = CurrentPlayer.CurrentVehicle.Vehicle.Speed * 2.23694f;
                if (!IsRoadWorthy(CurrentPlayer.CurrentVehicle.Vehicle) || IsDamaged(CurrentPlayer.CurrentVehicle.Vehicle))
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
                if (CurrentPlayer.CurrentLocation.CurrentStreet != null)
                {
                    SpeedLimit = CurrentPlayer.CurrentLocation.CurrentStreet.SpeedLimit;
                }
                IsSpeeding = CurrentSpeed > SpeedLimit + 25f;
            }
        }
    }
}
