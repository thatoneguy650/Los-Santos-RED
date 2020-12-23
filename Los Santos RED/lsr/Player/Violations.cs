using ExtensionsMethods;
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
        private readonly Crime KillingPolice = new Crime("KillingPolice", "Police Fatality", 3, true, 1, 1, false) { IsAlwaysFlagged = true };
        private readonly Crime FiringWeaponNearPolice = new Crime("FiringWeaponNearPolice", "Shots Fired at Police", 3, true, 3, 1, false) { CanReportBySound = true };
        private readonly Crime TerroristActivity = new Crime("TerroristActivity", "Terrorist Activity", 4, true, 2, 1) { CanReportBySound = true };
        private readonly Crime BrandishingHeavyWeapon = new Crime("BrandishingHeavyWeapon", "Brandishing Heavy Weapon", 3, false, 6, 1, false, true, true);
        private readonly Crime AimingWeaponAtPolice = new Crime("AimingWeaponAtPolice", "Aiming Weapons At Police", 3, false, 4, 1, false);
        private readonly Crime HurtingPolice = new Crime("HurtingPolice", "Assaulting Police", 3, false, 5, 1);
        private readonly Crime TrespessingOnGovtProperty = new Crime("TrespessingOnGovtProperty", "Trespassing on Government Property", 3, false, 7, 2, false);
        private readonly Crime GotInAirVehicleDuringChase = new Crime("GotInAirVehicleDuringChase", "Stealing an Air Vehicle", 3, false, 8, 2);
        private readonly Crime FiringWeapon = new Crime("FiringWeapon", "Firing Weapon", 2, false, 9, 2, true, true, true) { CanReportBySound = true };
        private readonly Crime KillingCivilians = new Crime("KillingCivilians", "Civilian Fatality", 2, false, 10, 2, true, true, true);
        private readonly Crime GrandTheftAuto = new Crime("GrandTheftAuto", "Grand Theft Auto", 2, false, 16, 3, true, true, true);
        private readonly Crime DrivingStolenVehicle = new Crime("DrivingStolenVehicle", "Driving a Stolen Vehicle", 2, false, 38, 5, false);
        private readonly Crime Mugging = new Crime("Mugging", "Mugging", 2, false, 11, 2, true, true, true);
        private readonly Crime AttemptingSuicide = new Crime("AttemptingSuicide", "Attempting Suicide", 2, false, 12, 3);
        private readonly Crime BrandishingWeapon = new Crime("BrandishingWeapon", "Brandishing Weapon", 2, false, 18, 3, true, true, true);
        private readonly Crime BrandishingCloseCombatWeapon = new Crime("BrandishingCloseCombatWeapon", "Brandishing Close Combat Weapon", 1, false, 20, 4, true, true, true);
        private readonly Crime HurtingCivilians = new Crime("HurtingCivilians", "Assaulting Civilians", 2, false, 14, 3, true, true, true);
        private readonly Crime ChangingPlates = new Crime("ChangingPlates", "Stealing License Plates", 1, false, 31, 4, true, true, false);
        private readonly Crime ResistingArrest = new Crime("ResistingArrest", "Resisting Arrest", 2, false, 19, 4, false);
        private readonly Crime SuspiciousActivity = new Crime("SuspiciousActivity", "Suspicious Activity", 1, false, 39, 5, false);
        private readonly Crime DrunkDriving = new Crime("DrunkDriving", "Drunk Driving", 2, false, 30, 4, false, false, false);
        private readonly Crime DrivingAgainstTraffic = new Crime("DrivingAgainstTraffic", "Driving Against Traffic", 1, false, 32, 4, false, false, false) { IsTrafficViolation = true };
        private readonly Crime DrivingOnPavement = new Crime("DrivingOnPavement", "Driving On Pavement", 1, false, 33, 4, false, false, false) { IsTrafficViolation = true };
        private readonly Crime NonRoadworthyVehicle = new Crime("NonRoadworthyVehicle", "NonRoadworthy Vehicle", 1, false, 34, 4, false, false, false) { IsTrafficViolation = true };
        private readonly Crime FelonySpeeding = new Crime("FelonySpeeding", "Speeding", 1, false, 37, 5, false, false, false) { IsTrafficViolation = true };
        private readonly Crime RunningARedLight = new Crime("RunningARedLight", "Running a Red Light", 1, false, 36, 5, false, false, false) { IsTrafficViolation = true };
        private readonly Crime HitPedWithCar = new Crime("HitPedWithCar", "Pedestrian Hit and Run", 2, false, 15, 3, true, true, true) { IsTrafficViolation = true };
        private readonly Crime HitCarWithCar = new Crime("HitCarWithCar", "Hit and Run", 1, false, 30, 4, true, true, true) { IsTrafficViolation = true };
        private uint GameTimeStartedDrivingOnPavement;
        private uint GameTimeStartedDrivingAgainstTraffic;
        private int TimeSincePlayerHitPed;
        private int TimeSincePlayerHitVehicle;
        private bool VehicleIsSuspicious;
        private bool TreatAsCop;
        private float CurrentSpeed;
        private uint GameTimeStartedBrandishing;
        public Violations()
        {
            CrimeList = new List<Crime>
                {
                    BrandishingCloseCombatWeapon,TerroristActivity,BrandishingHeavyWeapon, FiringWeapon, Mugging, AttemptingSuicide, ResistingArrest, KillingPolice, FiringWeaponNearPolice, AimingWeaponAtPolice, HurtingPolice, TrespessingOnGovtProperty, GotInAirVehicleDuringChase, KillingCivilians, BrandishingWeapon,
                    ChangingPlates, GrandTheftAuto, DrivingStolenVehicle, HurtingCivilians, SuspiciousActivity,DrivingAgainstTraffic,DrivingOnPavement,NonRoadworthyVehicle,FelonySpeeding,RunningARedLight,HitPedWithCar,HitCarWithCar,DrunkDriving
                };

        }
        private bool ShouldCheckTrafficViolations
        {
            get
            {
                if (Mod.Player.IsInVehicle && (Mod.Player.IsInAutomobile || Mod.Player.IsOnMotorcycle) && !Mod.Player.RecentlyStartedPlaying)
                {
                    return true;
                }
                else
                    return false;
            }
        }
        public bool IsViolatingAnyLaws
        {
            get
            {
                return CrimeList.Any(x => x.IsCurrentlyViolating);
            }
        }
        public string LawsViolatingDisplay
        {
            get
            {
                return string.Join(",", CrimeList.Where(x => x.IsCurrentlyViolating).Select(x => x.Name));
            }
        }
        public bool IsViolatingAnyTrafficLaws
        {
            get
            {
                if (HasBeenDrivingAgainstTraffic || HasBeenDrivingOnPavement || IsRunningRedLight || IsSpeeding || VehicleIsSuspicious)
                {
                    return true;
                }
                else
                    return false;
            }
        }
        public List<Crime> CivilianReportableCrimesViolating
        {
            get
            {
                return CrimeList.Where(x => x.IsCurrentlyViolating && x.CanBeReportedByCivilians).ToList();
            }
        }
        public bool IsViolatingAnyCivilianReportableCrime
        {
            get
            {
                return CrimeList.Any(x => x.IsCurrentlyViolating && x.CanBeReportedByCivilians);
            }
        }
        public bool IsViolatingAnyAudioBasedCivilianReportableCrime
        {
            get
            {
                return CrimeList.Any(x => x.IsCurrentlyViolating && x.CanBeReportedByCivilians && x.CanReportBySound);
            }
        }
        public bool RecentlyHitPed
        {
            get
            {
                if (TimeSincePlayerHitPed > -1 && TimeSincePlayerHitPed <= 1000)
                {
                    return true;
                }
                else
                    return false;
            }
        }
        public bool RecentlyHitVehicle
        {
            get
            {
                if (TimeSincePlayerHitVehicle > -1 && TimeSincePlayerHitVehicle <= 1000)
                {
                    return true;
                }
                else
                    return false;
            }
        }
        public bool HasBeenDrivingAgainstTraffic
        {
            get
            {
                if (GameTimeStartedDrivingAgainstTraffic == 0)
                {
                    return false;
                }
                else if (Game.GameTime - GameTimeStartedDrivingAgainstTraffic >= 1000)
                {
                    return true;
                }
                else
                    return false;
            }
        }
        public bool HasBeenDrivingOnPavement
        {
            get
            {
                if (GameTimeStartedDrivingOnPavement == 0)
                {
                    return false;
                }
                else if (Game.GameTime - GameTimeStartedDrivingOnPavement >= 1000)
                {
                    return true;
                }
                else
                    return false;
            }
        }
        public bool IsSpeeding { get; set; }
        public bool IsRunningRedLight { get; set; }
        public void Update()
        {
            if (Mod.Player.IsAliveAndFree)
            {
                CheckViolations();
                FlagViolations();
            }
            else
            {
                ResetViolations();
            }
        }
        private void ResetViolations()
        {
            CrimeList.ForEach(x => x.IsCurrentlyViolating = false);
        }
        public void TrafficUpdate()
        {
            if (Mod.Player.IsAliveAndFree && ShouldCheckTrafficViolations)
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
        private void CheckViolations()
        {
            CheckPedDamageCrimes();
            CheckWeaponCrimes();
            CheckTheftCrimes();
            CheckOtherCrimes();
        }
        private void CheckOtherCrimes()
        {
            if (Mod.Player.IsCommitingSuicide)
            {
                AttemptingSuicide.IsCurrentlyViolating = true;
            }
            else
            {
                AttemptingSuicide.IsCurrentlyViolating = false;
            }
            if (Mod.Player.IsWanted && Mod.Player.CurrentZone.IsRestrictedDuringWanted)
            {
                TrespessingOnGovtProperty.IsCurrentlyViolating = true;
            }
            else
            {
                TrespessingOnGovtProperty.IsCurrentlyViolating = false;
            }
            if (Mod.Player.Investigations.IsSuspicious)
            {
                SuspiciousActivity.IsCurrentlyViolating = true;
            }
            else
            {
                SuspiciousActivity.IsCurrentlyViolating = false;
            }
            if (Mod.Player.IsWanted && Mod.World.AnyPoliceSeenPlayerCurrentWanted && !Mod.Player.AreStarsGreyedOut && Mod.Player.Character.Speed >= 2.0f && !Mod.Player.HandsAreUp && Mod.Player.CurrentPoliceResponse.HasBeenWantedFor >= 20000)
            {
                ResistingArrest.IsCurrentlyViolating = true;
            }
            else
            {
                ResistingArrest.IsCurrentlyViolating = false;
            }

        }
        private void CheckTheftCrimes()
        {
            if (Mod.Player.IsWanted && Mod.Player.IsInVehicle && Mod.Player.IsInAirVehicle)
            {
                GotInAirVehicleDuringChase.IsCurrentlyViolating = true;
            }
            else
            {
                GotInAirVehicleDuringChase.IsCurrentlyViolating = false;
            }
            if (Mod.Player.CurrentVehicle != null && Mod.Player.CurrentVehicle.CopsRecognizeAsStolen)
            {
                DrivingStolenVehicle.IsCurrentlyViolating = true;
            }
            else
            {
                DrivingStolenVehicle.IsCurrentlyViolating = false;
            }
            if (Mod.Player.IsMugging)
            {
                Mugging.IsCurrentlyViolating = true;
            }
            else
            {
                Mugging.IsCurrentlyViolating = false;
            }
            if (Mod.Player.IsBreakingIntoCar)
            {
                GrandTheftAuto.IsCurrentlyViolating = true;
            }
            else
            {
                GrandTheftAuto.IsCurrentlyViolating = false;
            }

            if (Mod.Player.IsChangingLicensePlates)
            {
                ChangingPlates.IsCurrentlyViolating = true;
            }
            else
            {
                ChangingPlates.IsCurrentlyViolating = false;
            }
        }
        private void CheckWeaponCrimes()
        {
            if (Mod.Player.RecentlyShot(5000) || Game.LocalPlayer.Character.IsShooting)
            {
                if (!(Mod.Player.Character.IsCurrentWeaponSilenced || Mod.Player.CurrentWeaponCategory == WeaponCategory.Melee))
                {
                    FiringWeapon.IsCurrentlyViolating = true;
                    if (Mod.World.AnyPoliceRecentlySeenPlayer || Mod.World.AnyPoliceCanHearPlayer)
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
            if (CheckBrandishing() && Mod.Player.Character.Inventory.EquippedWeapon != null && !Mod.Player.IsInVehicle)
            {
                BrandishingWeapon.IsCurrentlyViolating = true;
                if (Mod.Player.CurrentWeapon != null && Mod.Player.CurrentWeapon.WeaponLevel >= 4)
                {
                    TerroristActivity.IsCurrentlyViolating = true;
                }
                else
                {
                    TerroristActivity.IsCurrentlyViolating = false;
                }
                if (Mod.Player.CurrentWeapon != null && Mod.Player.CurrentWeapon.WeaponLevel >= 3)
                {
                    BrandishingHeavyWeapon.IsCurrentlyViolating = true;
                }
                else
                {
                    BrandishingHeavyWeapon.IsCurrentlyViolating = false;
                }
                if (Mod.Player.CurrentWeapon != null && Mod.Player.CurrentWeapon.Category == WeaponCategory.Melee)
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
        private void CheckPedDamageCrimes()
        {
            if (Mod.Player.RecentlyKilledCop)
            {
                KillingPolice.IsCurrentlyViolating = true;
            }
            else
            {
                KillingPolice.IsCurrentlyViolating = false;
            }

            if (Mod.Player.RecentlyHurtCop)
            {
                HurtingPolice.IsCurrentlyViolating = true;
            }
            else
            {
                HurtingPolice.IsCurrentlyViolating = false;
            }

            if (Mod.Player.RecentlyKilledCivilian || Mod.Player.NearCivilianMurderVictim)
            {
                KillingCivilians.IsCurrentlyViolating = true;
            }
            else
            {
                KillingCivilians.IsCurrentlyViolating = false;
            }

            if (Mod.Player.RecentlyHurtCivilian)
            {
                HurtingCivilians.IsCurrentlyViolating = true;
            }
            else
            {
                HurtingCivilians.IsCurrentlyViolating = false;
            }
        }
        private bool CheckBrandishing()
        {
            if (Mod.Player.IsConsideredArmed)
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
                return false;
        }
        private void CheckTrafficViolations()
        {
            UpdateTrafficStats();
            if (Mod.DataMart.Settings.SettingsManager.TrafficViolations.HitPed && RecentlyHitPed && (Mod.Player.RecentlyHurtCivilian || Mod.Player.RecentlyHurtCop) && (Mod.World.Pedestrians.Civilians.Any(x => x.DistanceToPlayer <= 10f) || Mod.World.Pedestrians.Police.Any(x => x.DistanceToPlayer <= 10f)))//needed for non humans that are returned from this native
            {
                HitPedWithCar.IsCurrentlyViolating = true;
            }
            else
            {
                HitPedWithCar.IsCurrentlyViolating = false;
            }
            if (Mod.DataMart.Settings.SettingsManager.TrafficViolations.HitVehicle && RecentlyHitVehicle)
            {
                HitCarWithCar.IsCurrentlyViolating = true;
            }
            else
            {
                HitCarWithCar.IsCurrentlyViolating = false;
            }
            if (!TreatAsCop)
            {
                if (Mod.DataMart.Settings.SettingsManager.TrafficViolations.DrivingAgainstTraffic && (HasBeenDrivingAgainstTraffic || (Game.LocalPlayer.IsDrivingAgainstTraffic && Mod.Player.Character.CurrentVehicle.Speed >= 10f)))
                {
                    DrivingAgainstTraffic.IsCurrentlyViolating = true;
                }
                else
                {
                    DrivingAgainstTraffic.IsCurrentlyViolating = false;
                }
                if (Mod.DataMart.Settings.SettingsManager.TrafficViolations.DrivingOnPavement && (HasBeenDrivingOnPavement || (Game.LocalPlayer.IsDrivingOnPavement && Mod.Player.Character.CurrentVehicle.Speed >= 10f)))
                {
                    DrivingOnPavement.IsCurrentlyViolating = true;
                }
                else
                {
                    DrivingOnPavement.IsCurrentlyViolating = false;
                }

                if (Mod.DataMart.Settings.SettingsManager.TrafficViolations.NotRoadworthy && VehicleIsSuspicious)
                {
                    NonRoadworthyVehicle.IsCurrentlyViolating = true;
                }
                else
                {
                    NonRoadworthyVehicle.IsCurrentlyViolating = false;
                }

                if (Mod.DataMart.Settings.SettingsManager.TrafficViolations.Speeding && IsSpeeding)
                {
                    FelonySpeeding.IsCurrentlyViolating = true;
                }
                else
                {
                    FelonySpeeding.IsCurrentlyViolating = false;
                }
                if (Mod.DataMart.Settings.SettingsManager.TrafficViolations.RunningRedLight)//Off for now
                {
                    //RunningARedLight.IsCurrentlyViolating = true;//turned off for now until i fix it
                }
                else
                {
                    RunningARedLight.IsCurrentlyViolating = false;
                }
            }
            if (Mod.Player.IsDrunk && (DrivingAgainstTraffic.IsCurrentlyViolating || DrivingOnPavement.IsCurrentlyViolating || FelonySpeeding.IsCurrentlyViolating || RunningARedLight.IsCurrentlyViolating || HitPedWithCar.IsCurrentlyViolating || HitCarWithCar.IsCurrentlyViolating))
            {
                DrunkDriving.IsCurrentlyViolating = true;
            }
            else
            {
                DrunkDriving.IsCurrentlyViolating = false;
            }
        }
        private void UpdateTrafficStats()
        {
            VehicleIsSuspicious = false;
            TreatAsCop = false;
            IsSpeeding = false;
            if (Mod.Player.CurrentVehicle != null && Mod.Player.CurrentVehicle.Vehicle.Exists())
            {
                CurrentSpeed = Mod.Player.CurrentVehicle.Vehicle.Speed * 2.23694f;
                if (!IsRoadWorthy(Mod.Player.CurrentVehicle.Vehicle) || IsDamaged(Mod.Player.CurrentVehicle.Vehicle))
                {
                    VehicleIsSuspicious = true;
                }
                if (Mod.DataMart.Settings.SettingsManager.TrafficViolations.ExemptCode3 && Mod.Player.CurrentVehicle.Vehicle != null && Mod.Player.CurrentVehicle.Vehicle.IsPoliceVehicle && Mod.Player.CurrentVehicle != null && !Mod.Player.CurrentVehicle.WasReportedStolen)
                {
                    if (Mod.Player.CurrentVehicle.Vehicle.IsSirenOn && !Mod.World.AnyPoliceCanRecognizePlayer) //see thru ur disguise if ur too close
                    {
                        TreatAsCop = true;//Cops dont have to do traffic laws stuff if ur running code3?
                    }
                }
                IsRunningRedLight = false;
                //foreach (PedExt Civilian in Mod.World.Pedestrians.Civilians.Where(x => x.Pedestrian.Exists()).OrderBy(x => x.DistanceToPlayer))
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
                if (Mod.Player.CurrentStreet != null)
                {
                    SpeedLimit = Mod.Player.CurrentStreet.SpeedLimit;
                }
                IsSpeeding = CurrentSpeed > SpeedLimit + Mod.DataMart.Settings.SettingsManager.TrafficViolations.SpeedingOverLimitThreshold;
            }
        }
        private void FlagViolations()
        {
            foreach (Crime Violating in CrimeList.Where(x => x.IsCurrentlyViolating))
            {
                if (Mod.World.AnyPoliceCanSeePlayer || (Violating.CanReportBySound && Mod.World.AnyPoliceCanHearPlayer) || Violating.IsAlwaysFlagged)
                {
                    Mod.Player.CurrentPoliceResponse.CurrentCrimes.AddCrime(Violating, true, Mod.Player.CurrentPosition, Mod.Player.CurrentSeenVehicle, Mod.Player.CurrentSeenWeapon,true);
                }
            }
        }
        private bool IsRoadWorthy(Vehicle myCar)
        {
            bool LightsOn;
            bool HighbeamsOn;
            if (Mod.World.IsNight)
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
            if (Mod.World.IsNight)
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
    }
}
