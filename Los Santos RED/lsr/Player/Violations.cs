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
        private ISettingsProvideable Settings;
        public Violations(IViolateable currentPlayer, ITimeReportable timeReporter, ICrimes crimes, ISettingsProvideable settings)
        {
            TimeReporter = timeReporter;
            Player = currentPlayer;
            Crimes = crimes;
            CrimeList = Crimes.CrimeList;
            Settings = settings;
        }
        public List<Crime> CivilianReportableCrimesViolating => CrimesViolating.Where(x => x.CanBeReportedByCivilians).ToList();//CrimeList.Where(x => x.IsCurrentlyViolating && x.CanBeReportedByCivilians).ToList();
        public bool IsSpeeding { get; set; }
        public bool IsViolatingAnyTrafficLaws => HasBeenDrivingAgainstTraffic || HasBeenDrivingOnPavement || IsRunningRedLight || IsSpeeding || VehicleIsSuspicious;
        public string LawsViolatingDisplay => string.Join(", ", CrimesViolating.OrderBy(x=>x.Priority).Select(x => x.Name));
        public bool NearCivilianMurderVictim => PlayerKilledCivilians.Any(x => x.Pedestrian.Exists() && x.Pedestrian.DistanceTo2D(Player.Character) <= Settings.SettingsManager.PlayerSettings.Violations_MurderDistance);
        public bool RecentlyHurtCivilian => GameTimeLastHurtCivilian != 0 && Game.GameTime - GameTimeLastHurtCivilian <= Settings.SettingsManager.PlayerSettings.Violations_RecentlyHurtCivilianTime;
        public bool RecentlyHurtCop => GameTimeLastHurtCop != 0 && Game.GameTime - GameTimeLastHurtCop <= Settings.SettingsManager.PlayerSettings.Violations_RecentlyHurtPoliceTime;
        public bool RecentlyKilledCivilian => GameTimeLastKilledCivilian != 0 && Game.GameTime - GameTimeLastKilledCivilian <= Settings.SettingsManager.PlayerSettings.Violations_RecentlyKilledCivilianTime;
        public bool RecentlyKilledCop => GameTimeLastKilledCop != 0 && Game.GameTime - GameTimeLastKilledCop <= Settings.SettingsManager.PlayerSettings.Violations_RecentlyKilledPoliceTime;
        private bool HasBeenDrivingAgainstTraffic => GameTimeStartedDrivingAgainstTraffic != 0 && Game.GameTime - GameTimeStartedDrivingAgainstTraffic >= Settings.SettingsManager.PlayerSettings.Violations_RecentlyDrivingAgainstTraffiTime;
        private bool HasBeenDrivingOnPavement => GameTimeStartedDrivingOnPavement != 0 && Game.GameTime - GameTimeStartedDrivingOnPavement >= Settings.SettingsManager.PlayerSettings.Violations_RecentlyDrivingOnPavementTime;
        private bool RecentlyHitPed => TimeSincePlayerHitPed > 0 && TimeSincePlayerHitPed <= Settings.SettingsManager.PlayerSettings.Violations_RecentlyHitPedTime;
        private bool RecentlyHitVehicle => TimeSincePlayerHitVehicle > 0 && TimeSincePlayerHitVehicle <= Settings.SettingsManager.PlayerSettings.Violations_RecentlyHitVehicleTime;
        private bool ShouldCheckTrafficViolations => Player.IsInVehicle && (Player.IsInAutomobile || Player.IsOnMotorcycle) && !Player.RecentlyStartedPlaying;
        public void AddInjured(PedExt myPed)
        {
            if (myPed.IsCop)
            {
                GameTimeLastHurtCop = Game.GameTime;
                //AddViolating(CrimeList.FirstOrDefault(x => x.ID == "HurtingPolice"));
                Player.AddCrime(CrimeList.FirstOrDefault(x => x.ID == "HurtingPolice"), true, Player.Position, Player.CurrentSeenVehicle, Player.CurrentSeenWeapon, true, true, true);
                EntryPoint.WriteToConsole($"VIOLATIONS: Hurting Police Added", 5);
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
                Player.AddCrime(CrimeList.FirstOrDefault(x => x.ID == "KillingPolice"), true, Player.Position, Player.CurrentSeenVehicle, Player.CurrentSeenWeapon, true, true, true);
                //AddViolating(CrimeList.FirstOrDefault(x => x.ID == "KillingPolice"));
                EntryPoint.WriteToConsole($"VIOLATIONS: Killing Police Added", 5);
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
        public void UpdateTraffic()
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
                AddObservedAndReported();
            }
        }
        private void ResetViolations()
        {
            CrimesViolating.RemoveAll(x => !x.IsTrafficViolation);
        }
        private void ResetTrafficViolations()
        {
            CrimesViolating.RemoveAll(x => x.IsTrafficViolation);
        }
        private void CheckViolations()
        {
            CheckPedDamageCrimes();
            CheckWeaponCrimes();
            CheckTheftCrimes();
            CheckOtherCrimes();
        }
        private void AddViolating(Crime crime)
        {
            if(crime != null && crime.Enabled)
            {
                CrimesViolating.Add(crime);
            }
        }
        private void CheckPedDamageCrimes()
        {
            //if (RecentlyKilledCop)
            //{
            //    AddViolating(CrimeList.FirstOrDefault(x => x.ID == "KillingPolice"));//.IsCurrentlyViolating = true;
            //}

            //if (RecentlyHurtCop)
            //{
            //    AddViolating(CrimeList.FirstOrDefault(x => x.ID == "HurtingPolice"));//.IsCurrentlyViolating = true;
            //}

            if (RecentlyKilledCivilian || NearCivilianMurderVictim)
            {
                AddViolating(CrimeList.FirstOrDefault(x => x.ID == "KillingCivilians"));//.IsCurrentlyViolating = true;
            }

            if (RecentlyHurtCivilian)
            {
                AddViolating(CrimeList.FirstOrDefault(x => x.ID == "HurtingCivilians"));//.IsCurrentlyViolating = true;
            }
        }
        private void CheckWeaponCrimes()
        {
            if (Player.RecentlyShot)
            {
                if (!(Player.Character.IsCurrentWeaponSilenced || Player.CurrentWeaponCategory == WeaponCategory.Melee))
                {
                    AddViolating(CrimeList.FirstOrDefault(x => x.ID == "FiringWeapon"));//.IsCurrentlyViolating = true;
                    if (Player.AnyPoliceRecentlySeenPlayer || Player.AnyPoliceCanHearPlayer)
                    {
                        AddViolating(CrimeList.FirstOrDefault(x => x.ID == "FiringWeaponNearPolice"));//.IsCurrentlyViolating = true;
                    }
                }
            }
            if (CheckBrandishing() && Player.Character.Inventory.EquippedWeapon != null && !Player.IsInVehicle)
            {
                AddViolating(CrimeList.FirstOrDefault(x => x.ID == "BrandishingWeapon"));//.IsCurrentlyViolating = true;
                if (Player.CurrentWeapon != null && Player.CurrentWeapon.WeaponLevel >= 4)
                {
                    AddViolating(CrimeList.FirstOrDefault(x => x.ID == "TerroristActivity"));//.IsCurrentlyViolating = true;
                }
                if (Player.CurrentWeapon != null && Player.CurrentWeapon.WeaponLevel >= 3)
                {
                    AddViolating(CrimeList.FirstOrDefault(x => x.ID == "BrandishingHeavyWeapon"));//.IsCurrentlyViolating = true;
                }
                if (Player.CurrentWeapon != null && Player.CurrentWeapon.Category == WeaponCategory.Melee)
                {
                    AddViolating(CrimeList.FirstOrDefault(x => x.ID == "BrandishingCloseCombatWeapon"));//.IsCurrentlyViolating = true;
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
                AddViolating(CrimeList.FirstOrDefault(x => x.ID == "GotInAirVehicleDuringChase"));//.IsCurrentlyViolating = true;
            }
            if (Player.CurrentVehicle != null && Player.CurrentVehicle.CopsRecognizeAsStolen && Player.IsInVehicle)
            {
                AddViolating(CrimeList.FirstOrDefault(x => x.ID == "DrivingStolenVehicle"));//.IsCurrentlyViolating = true;
            }
            if (Player.IsHoldingUp)
            {
                AddViolating(CrimeList.FirstOrDefault(x => x.ID == "Mugging"));//.IsCurrentlyViolating = true;
            }
            if (Player.IsBreakingIntoCar)
            {
                AddViolating(CrimeList.FirstOrDefault(x => x.ID == "GrandTheftAuto"));//.IsCurrentlyViolating = true;
            }
            if (Player.IsChangingLicensePlates)
            {
                AddViolating(CrimeList.FirstOrDefault(x => x.ID == "ChangingPlates"));//.IsCurrentlyViolating = true;
            }
        }
        private void CheckOtherCrimes()
        {
            if (Player.IsCommitingSuicide)
            {
                AddViolating(CrimeList.FirstOrDefault(x=> x.ID == "AttemptingSuicide"));//.IsCurrentlyViolating = true;
            }
            if (Player.IsWanted && Player.CurrentLocation != null && Player.CurrentLocation.CurrentZone != null && Player.CurrentLocation.CurrentZone.IsRestrictedDuringWanted && Player.CurrentLocation.GameTimeInZone >= 15000)
            {
                AddViolating(CrimeList.FirstOrDefault(x => x.ID == "TrespessingOnGovtProperty"));//.IsCurrentlyViolating = true;
            }
            if (Player.Investigation.IsSuspicious)
            {
                AddViolating(CrimeList.FirstOrDefault(x => x.ID == "SuspiciousActivity"));//.IsCurrentlyViolating = true;
            }
            if (Player.IsWanted && Player.AnyPoliceRecentlySeenPlayer && Player.Character.Speed >= 2.0f && !Player.HandsAreUp && Player.PoliceResponse.HasBeenWantedFor >= 20000)
            {
                AddViolating(CrimeList.FirstOrDefault(x => x.ID == "ResistingArrest"));//.IsCurrentlyViolating = true;
            }

            if (Player.IsInVehicle && Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists() && Player.CurrentVehicle.Vehicle.HasPassengers && Player.CurrentVehicle.Vehicle.Passengers.Any(x => NativeFunction.Natives.IS_PED_GROUP_MEMBER<bool>(x,Game.LocalPlayer.Character.Group)))
            {
                AddViolating(CrimeList.FirstOrDefault(x => x.ID == "Kidnapping"));//.IsCurrentlyViolating = true;
            }
            if (Player.IsIntoxicated && Player.IntoxicatedIntensity >= 2.0f && !Player.IsInVehicle)
            {
                AddViolating(CrimeList.FirstOrDefault(x => x.ID == "PublicIntoxication"));//.IsCurrentlyViolating = true;
            }
        }
        private void CheckTrafficViolations()
        {
            bool isDrivingSuspiciously = false;
            UpdateTrafficStats();
            if (RecentlyHitPed && (RecentlyHurtCivilian || RecentlyHurtCop) && Player.AnyHumansNear && Player.VehicleSpeedMPH >= 20f)//needed for non humans that are returned from this native
            {
                isDrivingSuspiciously = true;
                AddViolating(CrimeList.FirstOrDefault(x => x.ID == "HitPedWithCar"));//.IsCurrentlyViolating = true;
            }
            if (RecentlyHitVehicle && Player.VehicleSpeedMPH >= 20f)
            {
                isDrivingSuspiciously = true;
                AddViolating(CrimeList.FirstOrDefault(x => x.ID == "HitCarWithCar"));//.IsCurrentlyViolating = true;
            }
            if (!TreatAsCop)
            {
                if ((HasBeenDrivingAgainstTraffic || (Game.LocalPlayer.IsDrivingAgainstTraffic && Player.VehicleSpeedMPH >= 20f)))
                {
                    isDrivingSuspiciously = true;
                    AddViolating(CrimeList.FirstOrDefault(x => x.ID == "DrivingAgainstTraffic"));//.IsCurrentlyViolating = true;
                }
                if ((HasBeenDrivingOnPavement || (Game.LocalPlayer.IsDrivingOnPavement && Player.VehicleSpeedMPH >= 20f)))
                {
                    isDrivingSuspiciously = true;
                    AddViolating(CrimeList.FirstOrDefault(x => x.ID == "DrivingOnPavement"));//.IsCurrentlyViolating = true;
                }
                if (VehicleIsSuspicious)
                {
                    isDrivingSuspiciously = true;
                    AddViolating(CrimeList.FirstOrDefault(x => x.ID == "NonRoadworthyVehicle"));//.IsCurrentlyViolating = true;
                }
                if (IsSpeeding)
                {
                    isDrivingSuspiciously = true;
                    AddViolating(CrimeList.FirstOrDefault(x => x.ID == "FelonySpeeding"));//.IsCurrentlyViolating = true;
                }
            }
            if (Player.IsIntoxicated && isDrivingSuspiciously)// DrivingAgainstTraffic.IsCurrentlyViolating || DrivingOnPavement.IsCurrentlyViolating || FelonySpeeding.IsCurrentlyViolating || RunningARedLight.IsCurrentlyViolating || HitPedWithCar.IsCurrentlyViolating || HitCarWithCar.IsCurrentlyViolating))
            {
                AddViolating(CrimeList.FirstOrDefault(x => x.ID == "DrunkDriving"));//.IsCurrentlyViolating = true;
            }
        }
        private void UpdateTrafficStats()
        {
            VehicleIsSuspicious = false;
            TreatAsCop = false;
            IsSpeeding = false;
            if (Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
            {
                if (!IsRoadWorthy(Player.CurrentVehicle) || IsDamaged(Player.CurrentVehicle))
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
                    SpeedLimit = Player.CurrentLocation.CurrentStreet.SpeedLimitMPH;
                }
                IsSpeeding = Player.VehicleSpeedMPH > SpeedLimit + 25f;
            }
        }
        private void AddObservedAndReported()
        {
            foreach (Crime Violating in CrimesViolating)
            {
                if (Player.AnyPoliceCanSeePlayer || (Violating.CanReportBySound && Player.AnyPoliceCanHearPlayer) || Violating.CanViolateWithoutPerception)
                {
                    bool shouldAdd = true;
                    if(Player.RecentlyBribedPolice)
                    {
                        if(Violating.ResultingWantedLevel <= 2)
                        {
                            shouldAdd = false;
                        }
                    }
                    else if(Player.RecentlyPaidFine)
                    {
                        if (Violating.ResultingWantedLevel <= 1)
                        {
                            shouldAdd = false;
                        }
                    }
                    if (shouldAdd)
                    {
                        EntryPoint.WriteToConsole($"VIOLATIONS: ADDED {Violating.Name}", 5);
                        Player.AddCrime(Violating, true, Player.Position, Player.CurrentSeenVehicle, Player.CurrentSeenWeapon, true, true, true);
                    }
                }
            }
        }
        private bool IsDamaged(VehicleExt myCar)
        {
            if (!myCar.Vehicle.Exists())
            {
                return false;
            }
            if (myCar.Vehicle.Health <= 300 || (myCar.Vehicle.EngineHealth <= 300 && myCar.Engine.IsRunning))//can only see smoke and shit if its running
            {
                return true;
            }
            if (!NativeFunction.CallByName<bool>("ARE_ALL_VEHICLE_WINDOWS_INTACT", myCar.Vehicle))
            {
                return true;
            }
            foreach (VehicleDoor myDoor in myCar.Vehicle.GetDoors())
            {
                if (myDoor.IsDamaged)
                {
                    return true;
                }
            }
            if (TimeReporter.IsNight)
            {
                if (myCar.IsCar && NativeFunction.CallByName<bool>("GET_IS_RIGHT_VEHICLE_HEADLIGHT_DAMAGED", myCar.Vehicle) || NativeFunction.CallByName<bool>("GET_IS_LEFT_VEHICLE_HEADLIGHT_DAMAGED", myCar.Vehicle))
                {
                    return true;
                }
            }
            if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar.Vehicle, 0, false))
            {
                return true;
            }
            if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar.Vehicle, 1, false))
            {
                return true;
            }
            if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar.Vehicle, 2, false))
            {
                return true;
            }
            if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar.Vehicle, 3, false))
            {
                return true;
            }
            if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar.Vehicle, 4, false))
            {
                return true;
            }
            if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar.Vehicle, 5, false))
            {
                return true;
            }
            return false;
        }
        private bool IsRoadWorthy(VehicleExt myCar)
        {
            bool LightsOn;
            bool HighbeamsOn;
            if (TimeReporter.IsNight && myCar.Engine.IsRunning)
            {
                unsafe
                {
                    NativeFunction.CallByName<bool>("GET_VEHICLE_LIGHTS_STATE", myCar.Vehicle, &LightsOn, &HighbeamsOn);
                }
                if (!LightsOn)
                {
                    return false;
                }
                if (HighbeamsOn)
                {
                    return false;
                }
                if (myCar.IsCar && NativeFunction.CallByName<bool>("GET_IS_RIGHT_VEHICLE_HEADLIGHT_DAMAGED", myCar.Vehicle) || NativeFunction.CallByName<bool>("GET_IS_LEFT_VEHICLE_HEADLIGHT_DAMAGED", myCar.Vehicle))
                {
                    return false;
                }
            }
            if (myCar.Vehicle.LicensePlate == "        ")
            {
                return false;
            }
            return true;
        }
    }
}
