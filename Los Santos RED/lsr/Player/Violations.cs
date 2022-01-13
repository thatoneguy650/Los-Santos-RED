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
        private bool SentRecentCrash = false;
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
        public void AddInjured(PedExt myPed, bool WasShot, bool WasMeleeAttacked, bool WasHitByVehicle)
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
                if (myPed.GetType() == typeof(GangMember))
                {
                    GangMember gm = (GangMember)myPed;
                    Player.ChangeReputation(gm.Gang, -500);
                }
                GameTimeLastHurtCivilian = Game.GameTime;
            }
        }
        public void AddKilled(PedExt myPed, bool WasShot, bool WasMeleeAttacked, bool WasHitByVehicle)
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
                if(myPed.IsGangMember)
                {
                    if(myPed.GetType() == typeof(GangMember))
                    {
                        GangMember gm = (GangMember)myPed;
                        Player.ChangeReputation(gm.Gang, -1000);
                    }
                }
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
        public void Setup()
        {
            AnimationDictionary.RequestAnimationDictionay("switch@franklin@002110_04_magd_3_weed_exchange");
        }
        public void UpdateTraffic()
        {
            ResetTrafficViolations();
            VehicleIsSuspicious = false;
            TreatAsCop = false;
            IsSpeeding = false;
            IsRunningRedLight = false;
            if (Player.IsAliveAndFree && Player.ShouldCheckViolations && ShouldCheckTrafficViolations)
            {
                CheckTrafficViolations();
            }
        }
        public void Update()
        {
            ResetViolations();
            if (Player.IsAliveAndFree && Player.ShouldCheckViolations)
            {
                //ResetViolations();
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
            GameFiber.Yield();//TR Yield RemovedTest 1, added 12
            CheckWeaponCrimes();
           GameFiber.Yield();//TR Yield RemovedTest 1, added 12
            CheckTheftCrimes();
            GameFiber.Yield();//TR Yield RemovedTest 1, added 12
            CheckOtherCrimes();
            GameFiber.Yield();//TR Yield RemovedTest 1, added 12
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
                    if (Player.AnyPoliceRecentlySeenPlayer || (Player.CurrentTargetedPed != null && Player.CurrentTargetedPed.IsCop) || (Player.AnyPoliceCanHearPlayer && Player.ClosestPoliceDistanceToPlayer <= 50f))
                    {
                        AddViolating(CrimeList.FirstOrDefault(x => x.ID == "FiringWeaponNearPolice"));//.IsCurrentlyViolating = true;
                    }
                }
            }
            bool isBrandishing = CheckBrandishing();
            if (isBrandishing && Player.Character.Inventory.EquippedWeapon != null && !Player.IsInVehicle)
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
            if (isBrandishing && Player.CurrentTargetedPed != null && Player.CurrentWeapon.Category != WeaponCategory.Melee)
            {
                if(Player.CurrentTargetedPed.IsCop)
                {
                    AddViolating(CrimeList.FirstOrDefault(x => x.ID == "AimingWeaponAtPolice"));
                }
                else
                {
                    AddViolating(CrimeList.FirstOrDefault(x => x.ID == "AssaultingWithDeadlyWeapon"));
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
            if (Player.CurrentVehicle != null && Player.CurrentVehicle.CopsRecognizeAsStolen && Player.IsInVehicle && Player.IsDriver)
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
            if (Player.IsWanted && !Player.HandsAreUp) //&& Player.AnyPoliceRecentlySeenPlayer)// && ((!Player.IsInVehicle && Player.Character.Speed >= 1.5f) || (Player.IsInVehicle && Player.VehicleSpeedMPH > 40f)) && !Player.HandsAreUp && Player.PoliceResponse.HasBeenWantedFor >= 20000)
            {
                if(Player.IsInVehicle)
                {
                    if(Player.VehicleSpeedMPH >= 65f)
                    {
                        if (Player.PoliceResponse.HasBeenWantedFor >= Settings.SettingsManager.PlayerSettings.Violations_ResistingArrestFastTriggerTime)//kept going or took off
                        {
                            AddViolating(CrimeList.FirstOrDefault(x => x.ID == "ResistingArrest"));
                            //EntryPoint.WriteToConsole($"VIOLATIONS: ADDED RESISTING ARREST FAST IN VEHICLE", 5);
                        }
                    }
                    else if (Player.VehicleSpeedMPH >= 35f)
                    {
                        if (Player.PoliceResponse.HasBeenWantedFor >= Settings.SettingsManager.PlayerSettings.Violations_ResistingArrestMediumTriggerTime)
                        {
                            AddViolating(CrimeList.FirstOrDefault(x => x.ID == "ResistingArrest"));
                            //EntryPoint.WriteToConsole($"VIOLATIONS: ADDED RESISTING ARREST MEDIUM IN VEHICLE", 5);
                        }
                    }
                    else
                    {
                        if(Player.VehicleSpeedMPH >= 20f && Player.PoliceResponse.HasBeenWantedFor >= Settings.SettingsManager.PlayerSettings.Violations_ResistingArrestSlowTriggerTime)
                        {
                            AddViolating(CrimeList.FirstOrDefault(x => x.ID == "ResistingArrest"));
                           // EntryPoint.WriteToConsole($"VIOLATIONS: ADDED RESISTING ARREST SLOW IN VEHICLE", 5);
                        }
                    }
                }
                else
                {
                    if (Player.Character.Exists() && Player.Character.Speed >= 1.2f)
                    {
                        if (Player.PoliceResponse.HasBeenWantedFor >= Settings.SettingsManager.PlayerSettings.Violations_ResistingArrestFastTriggerTime)//kept going or took off
                        {
                            AddViolating(CrimeList.FirstOrDefault(x => x.ID == "ResistingArrest"));
                           // EntryPoint.WriteToConsole($"VIOLATIONS: ADDED RESISTING ARREST FAST", 5);
                        }
                    }
                    else
                    {
                        if (Player.Character.Exists() && Player.Character.Speed >= 0.5f)
                        {
                            if (Player.PoliceResponse.HasBeenWantedFor >= Settings.SettingsManager.PlayerSettings.Violations_ResistingArrestSlowTriggerTime)
                            {
                                AddViolating(CrimeList.FirstOrDefault(x => x.ID == "ResistingArrest"));
                                //EntryPoint.WriteToConsole($"VIOLATIONS: ADDED RESISTING ARREST SLOW", 5);
                            }
                        }
                    }
                }
            }
            //GameFiber.Yield();//TR Yield RemovedTest 1



            //if (Player.IsInVehicle && Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists() && Player.CurrentVehicle.Vehicle.HasPassengers)// && Player.CurrentVehicle.Vehicle.Passengers.Any(x => x.Exists() && !x.IsPersistent && !NativeFunction.Natives.IS_PED_GROUP_MEMBER<bool>(x,Player.GroupID) && x.Handle != Player.Character.Handle))
            //{
            //    foreach(Ped passenger in Player.CurrentVehicle.Vehicle.Passengers)
            //    {
            //        //EntryPoint.WriteToConsole($"VIOLATIONS: Kidnapping {passenger.Handle} IsPersistent {passenger.IsPersistent} IS_PED_GROUP_MEMBER {NativeFunction.Natives.IS_PED_GROUP_MEMBER<bool>(passenger, Player.GroupID)}", 5);
            //        if (passenger.Exists() && passenger.IsAlive && !passenger.IsPersistent && !NativeFunction.Natives.IS_PED_GROUP_MEMBER<bool>(passenger, Player.GroupID) && passenger.Handle != Player.Character.Handle)
            //        {
            //            AddViolating(CrimeList.FirstOrDefault(x => x.ID == "Kidnapping"));//.IsCurrentlyViolating = true;
            //            break;
            //        }

            //    }

            //}
            if (Player.IsIntoxicated && Player.IntoxicatedIntensity >= 2.0f && !Player.IsInVehicle)
            {
                AddViolating(CrimeList.FirstOrDefault(x => x.ID == "PublicIntoxication"));//.IsCurrentlyViolating = true;
            }

            if(Player.RecentlyFedUpCop)
            {
                AddViolating(CrimeList.FirstOrDefault(x => x.ID == "InsultingOfficer"));
                
            }
            if(Player.IsDealingDrugs)
            {
                AddViolating(CrimeList.FirstOrDefault(x => x.ID == "DealingDrugs"));
            }
            if (Player.IsDealingIllegalGuns)
            {
                AddViolating(CrimeList.FirstOrDefault(x => x.ID == "DealingGuns"));
            }

        }
        private void CheckTrafficViolations()
        {
            bool isDrivingSuspiciously = false;
            UpdateTrafficStats();
            //GameFiber.Yield();//TR Yield RemovedTest 1
            if (RecentlyHitPed && (RecentlyHurtCivilian || RecentlyHurtCop) && Player.VehicleSpeedMPH >= 20f)//needed for non humans that are returned from this native
            {
                if (Player.AnyHumansNear)//TR ADDED 20 checking this might heavy?
                {
                    isDrivingSuspiciously = true;
                    AddViolating(CrimeList.FirstOrDefault(x => x.ID == "HitPedWithCar"));//.IsCurrentlyViolating = true; 
                }
                GameFiber.Yield();
            }
            if (RecentlyHitVehicle && Player.VehicleSpeedMPH >= 20f)
            {
                isDrivingSuspiciously = true;

                if (!SentRecentCrash)
                {
                    SentRecentCrash = true;
                    Player.OnVehicleCrashed();
                }
                AddViolating(CrimeList.FirstOrDefault(x => x.ID == "HitCarWithCar"));//.IsCurrentlyViolating = true;
            }
            if (!RecentlyHitVehicle)
            {
                SentRecentCrash = false;
            }
            if (!TreatAsCop)
            {
                GameFiber.Yield();
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
            //GameFiber.Yield();//TR Yield RemovedTest 1
            VehicleIsSuspicious = false;
            TreatAsCop = false;
            IsSpeeding = false;
            if (Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists() && Player.IsDriver)
            {
                if (!IsRoadWorthy(Player.CurrentVehicle) || IsDamaged(Player.CurrentVehicle))
                {
                    //GameFiber.Yield();//TR Yield RemovedTest 1
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
                        //EntryPoint.WriteToConsole($"AddObservedAndReported: ADDED {Violating.Name}", 5);
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
