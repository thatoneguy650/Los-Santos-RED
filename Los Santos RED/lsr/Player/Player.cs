using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Locations;
using LosSantosRED.lsr.Player;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Mod
{
    public class Player : IConsumeable, IIntoxicatable, ITargetable, IPoliceRespondable, IInputable, IPedSwappable, IMuggable, IRespawnable, IViolateable, IWeaponDroppable, IDisplayable, ICarStealable, IPlateChangeable, IActionable, ITaskableTarget_Old
    {
        private CriminalHistory CriminalHistory;
        private ConsumeActivity CurrentConsumingActivity;
        private HealthState CurrentHealth;
        private LocationData CurrentLocation;
        private IEntityProvideable EntityProvider;
        private uint GameTimeLastBusted;
        private uint GameTimeLastDied;
        private uint GameTimeLastMoved;
        private uint GameTimeLastMovedFast;
        private uint GameTimeLastShot;
        private uint GameTimeStartedPlaying;
        private bool isAimingInVehicle;
        private bool isGettingIntoVehicle;
        private bool isInVehicle;
        private bool IsVanillaRespawnActive = true;
        private bool LeftEngineOn;
        private Mugging Mugging;
        private int PreviousWantedLevel;
        private SearchMode SearchMode;
        private ISettingsProvideable Settings;
        private IStreets Streets;
        private Surrendering Surrendering;
        private ITimeControllable TimeControllable;
        private Violations Violations;
        private WeaponDropping WeaponDropping;
        private IWeapons Weapons;
        private IZones Zones;
        public Player(IEntityProvideable provider, ITimeControllable timeControllable, IStreets streets, IZones zones, ISettingsProvideable settings, IWeapons weapons)
        {
            EntityProvider = provider;
            TimeControllable = timeControllable;
            Streets = streets;
            Zones = zones;
            Settings = settings;
            Weapons = weapons;
            CurrentHealth = new HealthState(new PedExt(Game.LocalPlayer.Character));
            CurrentLocation = new LocationData(Game.LocalPlayer.Character, Streets, Zones);
            WeaponDropping = new WeaponDropping(this, Weapons);
            Mugging = new Mugging(this, EntityProvider);
            Surrendering = new Surrendering(this);

            Violations = new Violations(this, TimeControllable);
            Investigation = new Investigation(this);
            CriminalHistory = new CriminalHistory(this);
            PoliceResponse = new PoliceResponse(this);
            SearchMode = new SearchMode(this);
            PoliceResponse.SetWantedLevel(0, "Initial", true);
            NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags._PED_FLAG_DISABLE_STARTING_VEH_ENGINE, true);
            GameTimeStartedPlaying = Game.GameTime;
            ModelName = Game.LocalPlayer.Character.Model.Name;
            IsMale = Game.LocalPlayer.Character.IsMale;
        }
        public float ActiveDistance => Investigation.IsActive ? Investigation.Distance : 400f + (WantedLevel * 200f);
        public bool AnyHumansNear => EntityProvider.PoliceList.Any(x => x.DistanceToPlayer <= 10f) || EntityProvider.CivilianList.Any(x => x.DistanceToPlayer <= 10f);
        public bool AnyPoliceCanHearPlayer { get; set; }
        public bool AnyPoliceCanRecognizePlayer { get; set; }
        public bool AnyPoliceCanSeePlayer { get; set; }
        public bool AnyPoliceRecentlySeenPlayer { get; set; }
        public bool AnyPoliceSeenPlayerCurrentWanted { get; set; }
        public bool AreStarsGreyedOut { get; set; }
        public bool BeingArrested { get; private set; }
        public bool CanDropWeapon => WeaponDropping.CanDropWeapon;
        public bool CanPerformActivities => !IsInVehicle && IsStill && !IsIncapacitated && !IsConsideredArmed;
        public bool CanSurrender => Surrendering.CanSurrender;
        public bool CanUndie => TimesDied < Settings.SettingsManager.General.UndieLimit;
        public Ped Character => Game.LocalPlayer.Character;
        public List<Crime> CivilianReportableCrimesViolating => Violations.CivilianReportableCrimesViolating;
        public Street CurrentCrossStreet => CurrentLocation.CurrentCrossStreet;
        public Vector3 CurrentPosition => Game.LocalPlayer.Character.Position;
        public VehicleExt CurrentSeenVehicle => CurrentVehicle == null ? VehicleGettingInto : CurrentVehicle;
        public WeaponInformation CurrentSeenWeapon => !IsInVehicle ? CurrentWeapon : null;
        public Street CurrentStreet => CurrentLocation.CurrentStreet;
        public VehicleExt CurrentVehicle { get; private set; }
        public WeaponInformation CurrentWeapon { get; private set; }
        public WeaponCategory CurrentWeaponCategory => CurrentWeapon != null ? CurrentWeapon.Category : WeaponCategory.Unknown;
        public WeaponHash CurrentWeaponHash { get; set; }
        public Zone CurrentZone => CurrentLocation.CurrentZone;
        public string DebugLine1 => $"IsStill: {IsStill} IsMoving: {IsMoving} IsMovingFast: {IsMovingFast}";
        public string DebugLine2 => $"WantedFor {PoliceResponse.HasBeenWantedFor} NotWantedFor {PoliceResponse.HasBeenNotWantedFor} CurrentWantedFor {PoliceResponse.HasBeenAtCurrentWantedLevelFor}";//CurrentConsumingActivity != null ? CurrentConsumingActivity.DebugString : "";
        public string DebugLine3 => SearchMode.SearchModeDebug;
        public string DebugLine4 => $"Current Model: {ModelName} IsMale: {IsMale}";
        public string DebugLine5 => $"Obs {PoliceResponse.ObservedCrimesDisplay}";
        public string DebugLine6 => $"Rep {PoliceResponse.ReportedCrimesDisplay}";
        public string DebugLine7 => $"Vio {Violations.LawsViolatingDisplay}";
        public bool DiedInVehicle { get; private set; }
        public bool HandsAreUp { get; set; }
        public float IntoxicatedIntensity { get; set; }
        public Investigation Investigation { get; private set; }
        public bool IsAimingInVehicle
        {
            get => isAimingInVehicle;
            private set
            {
                if (isAimingInVehicle != value)
                {
                    isAimingInVehicle = value;
                    IsAimingInVehicleChanged();
                }
            }
        }
        public bool IsAliveAndFree => !IsBusted && !IsDead;
        public bool IsAttemptingToSurrender => HandsAreUp && !PoliceResponse.IsWeaponsFree;
        public bool IsBreakingIntoCar => IsCarJacking || IsLockPicking || IsHotWiring || Game.LocalPlayer.Character.IsJacking;
        public bool IsBustable => IsAliveAndFree && PoliceResponse.HasBeenWantedFor >= 3000 && !Surrendering.IsCommitingSuicide && !RecentlyBusted && !IsInVehicle;
        public bool IsBusted { get; private set; }
        public bool IsCarJacking { get; set; }
        public bool IsChangingLicensePlates { get; set; }
        public bool IsCommitingSuicide => Surrendering.IsCommitingSuicide;
        public bool IsConsideredArmed { get; private set; }
        public bool IsConsuming { get; set; }
        public bool IsDead { get; private set; }
        public bool IsGettingIntoAVehicle
        {
            get => isGettingIntoVehicle;
            private set
            {
                if (isGettingIntoVehicle != value)
                {
                    isGettingIntoVehicle = value;
                    IsGettingIntoVehicleChanged();
                }
            }
        }
        public bool IsHoldingEnter { get; set; }
        public bool IsHotWiring { get; private set; }
        public bool IsInAirVehicle { get; private set; }
        public bool IsInAutomobile { get; private set; }
        public bool IsIncapacitated => IsStunned || IsRagdoll;
        public bool IsInSearchMode { get; set; }
        public bool IsIntoxicated { get; set; }
        public bool IsInVehicle
        {
            get => isInVehicle;
            private set
            {
                if (isInVehicle != value)
                {
                    isInVehicle = value;
                    IsInVehicleChanged();
                }
            }
        }
        public bool IsLockPicking { get; set; }
        public bool IsMale { get; set; }
        public bool IsMobileRadioEnabled { get; private set; }
        public bool IsMoveControlPressed { get; set; }
        public bool IsMoving => GameTimeLastMoved != 0 && Game.GameTime - GameTimeLastMoved <= 2000;
        public bool IsMovingFast => GameTimeLastMovedFast != 0 && Game.GameTime - GameTimeLastMovedFast <= 2000;
        public bool IsMugging => Mugging.IsMugging;
        public bool IsNotWanted => Game.LocalPlayer.WantedLevel == 0;
        public bool IsOffroad => CurrentLocation.IsOffroad;
        public bool IsOnMotorcycle { get; private set; }
        public bool IsRagdoll { get; private set; }
        public bool IsSpeeding => Violations.IsSpeeding;
        public bool IsStill { get; private set; }
        public bool IsStunned { get; private set; }
        public bool IsViolatingAnyAudioBasedCivilianReportableCrime => Violations.IsViolatingAnyAudioBasedCivilianReportableCrime;
        public bool IsViolatingAnyCivilianReportableCrime => Violations.IsViolatingAnyCivilianReportableCrime;
        public bool IsViolatingAnyTrafficLaws => Violations.IsViolatingAnyTrafficLaws;
        public bool IsWanted => Game.LocalPlayer.WantedLevel > 0;
        public WeaponHash LastWeaponHash { get; set; }
        public int MaxWantedLastLife { get; set; }
        public string ModelName { get; set; }
        public int Money
        {
            get
            {
                int CurrentCash;
                unsafe
                {
                    NativeFunction.CallByName<int>("STAT_GET_INT", Natives.CashHash(Settings.SettingsManager.General.MainCharacterToAlias), &CurrentCash, -1);
                }
                return CurrentCash;
            }
        }
        public Vector3 PlacePoliceLastSeenPlayer { get; set; }
        public bool PoliceRecentlyNoticedVehicleChange { get; set; }
        public PoliceResponse PoliceResponse { get; set; }
        public bool RecentlyAppliedWantedStats => CriminalHistory.RecentlyAppliedWantedStats;
        public bool RecentlyBusted => GameTimeLastBusted != 0 && Game.GameTime - GameTimeLastBusted <= 5000;
        public bool RecentlyDied => GameTimeLastDied != 0 && Game.GameTime - GameTimeLastDied <= 5000;
        public bool RecentlyStartedPlaying => Game.GameTime - GameTimeStartedPlaying <= 15000;
        public List<VehicleExt> ReportedStolenVehicles => TrackedVehicles.Where(x => x.NeedsToBeReportedStolen).ToList();
        public List<LicensePlate> SpareLicensePlates { get; private set; } = new List<LicensePlate>();
        public bool StarsRecentlyActive => SearchMode.StarsRecentlyActive;
        public bool StarsRecentlyGreyedOut => SearchMode.StarsRecentlyGreyedOut;
        public string SuspectsName { get; private set; }
        public uint TimeInSearchMode => SearchMode.TimeInSearchMode;
        public int TimesDied { get; set; }
        public uint TimeToRecognize
        {
            get
            {
                uint Time = 2000;
                if (TimeControllable.IsNight)
                {
                    Time += 3500;
                }
                else if (IsInVehicle)
                {
                    Time += 750;
                }
                return Time;
            }
        }
        public List<VehicleExt> TrackedVehicles { get; private set; } = new List<VehicleExt>();
        public VehicleExt VehicleGettingInto { get; private set; }
        public int WantedLevel => Game.LocalPlayer.WantedLevel;
        public void AddCrime(Crime CrimeInstance, bool ByPolice, Vector3 Location, VehicleExt VehicleObserved, WeaponInformation WeaponObserved, bool HaveDescription)
        {
            PoliceResponse.AddCrime(CrimeInstance, ByPolice, Location, VehicleObserved, WeaponObserved, HaveDescription);
            if (!ByPolice)
            {
                Investigation.Start();
            }
            
        }
        public void AddSpareLicensePlate()
        {
            SpareLicensePlates.Add(new LicensePlate(RandomItems.RandomString(8), 3, false));//random cali
        }
        public void Arrest()
        {
            BeingArrested = true;
            if (!IsBusted)
            {
                BustedEvent();
            }
        }
        public void ArrestWarrantUpdate()
        {
            CriminalHistory.Update();
        }
        public void ChangePlate()
        {
            PlateTheft plateTheft = new PlateTheft(this);
            plateTheft.ChangePlate(SpareLicensePlates[0]);//this isnt gonna work
        }
        public void CommitSuicide()
        {
            Surrendering.CommitSuicide(Game.LocalPlayer.Character);
        }
        public void DisplayPlayerNotification()
        {
            string NotifcationText = "Warrants: ~g~None~s~";
            if (PoliceResponse.HasObservedCrimes)
            {
                NotifcationText = "Wanted For:" + PoliceResponse.PrintCrimes();
            }

            if (CurrentVehicle != null && !CurrentVehicle.IsStolen)
            {
                string Make = CurrentVehicle.MakeName();
                string Model = CurrentVehicle.ModelName();
                string VehicleName = "";
                if (Make != "")
                {
                    VehicleName = Make;
                }
                if (Model != "")
                {
                    VehicleName += " " + Model;
                }

                NotifcationText += string.Format("~n~Vehicle: ~p~{0}~s~", VehicleName);
                NotifcationText += string.Format("~n~Plate: ~p~{0}~s~", CurrentVehicle.CarPlate.PlateNumber);
            }

            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~b~Personal Info", string.Format("~y~{0}", SuspectsName), NotifcationText);
        }
        public void Dispose()
        {
            ActivateVanillaRespawn();
            NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags._PED_FLAG_DISABLE_STARTING_VEH_ENGINE, false);
        }
        public void DrinkBeer()
        {
            if (!IsConsuming && CanPerformActivities)
            {
                IsConsuming = true;
                CurrentConsumingActivity = new DrinkingActivity(this);
                CurrentConsumingActivity.Start();
            }
        }
        public void DropWeapon()
        {
            WeaponDropping.DropWeapon();
        }
        public void GiveMoney(int Amount)
        {
            int CurrentCash;
            uint PlayerCashHash = Natives.CashHash(Settings.SettingsManager.General.MainCharacterToAlias);
            unsafe
            {
                NativeFunction.CallByName<int>("STAT_GET_INT", PlayerCashHash, &CurrentCash, -1);
            }
            if (CurrentCash + Amount < 0)
            {
                NativeFunction.CallByName<int>("STAT_SET_INT", PlayerCashHash, 0, 1);
            }
            else
            {
                NativeFunction.CallByName<int>("STAT_SET_INT", PlayerCashHash, CurrentCash + Amount, 1);
            }
        }
        public void GiveName()
        {
            string ModelName = Game.LocalPlayer.Character.Model.Name.ToLower();
            if (ModelName == "player_zero")
            {
                SuspectsName = "Michael De Santa";
            }
            else if (ModelName == "player_one")
            {
                SuspectsName = "Franklin Clinton";
            }
            else if (ModelName == "player_two")
            {
                SuspectsName = "Trevor Philips";
            }
            else
            {
                SuspectsName = "John Doe";
            }
        }
        public void GiveName(string modelBeforeSpoof, string defaultName)
        {
            if (modelBeforeSpoof.ToLower() == "player_zero")
            {
                SuspectsName = "Michael De Santa";
            }
            else if (modelBeforeSpoof.ToLower() == "player_one")
            {
                SuspectsName = "Franklin Clinton";
            }
            else if (modelBeforeSpoof.ToLower() == "player_two")
            {
                SuspectsName = "Trevor Philips";
            }
            else
            {
                SuspectsName = defaultName;
            }
        }
        public void Injured(PedExt MyPed)
        {
            Violations.AddInjured(MyPed);
        }
        public void LocationUpdate()
        {
            CurrentLocation.Update();
        }
        public void LowerHands()
        {
            Surrendering.LowerHands();
        }
        public void MuggingUpdate()
        {
            Mugging.Update();
        }
        public void Murdered(PedExt MyPed)
        {
            Violations.AddKilled(MyPed);
        }
        public void RaiseHands()
        {
            Surrendering.RaiseHands();
        }
        public bool RecentlyShot(int duration)
        {
            if (GameTimeLastShot == 0)
            {
                return false;
            }
            else if (RecentlyStartedPlaying)
            {
                return false;
            }
            else if (Game.GameTime - GameTimeLastShot <= duration) //15000
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void RemovePlate()
        {
            PlateTheft plateTheft = new PlateTheft(this);
            plateTheft.RemovePlate();
        }
        public void Reset(bool resetWanted, bool resetTimesDied, bool clearWeapons)
        {
            IsDead = false;
            IsBusted = false;
            Game.LocalPlayer.HasControl = true;
            BeingArrested = false;
            CurrentHealth = new HealthState(new PedExt(Game.LocalPlayer.Character));
            IsConsuming = false;
            if (resetWanted)
            {
                Game.LocalPlayer.WantedLevel = 0;
                PoliceResponse = new PoliceResponse(this);
                //PoliceResponse.Reset();
                Investigation.Reset();
                Violations = new Violations(this, TimeControllable);
                //Violations.Reset();
                NativeFunction.CallByName<bool>("RESET_PLAYER_ARREST_STATE", Game.LocalPlayer);
                MaxWantedLastLife = 0;
                GameTimeStartedPlaying = Game.GameTime;
                Update();
            }
            if (resetTimesDied)
            {
                TimesDied = 0;
            }
            if (clearWeapons)
            {
                Game.LocalPlayer.Character.Inventory.Weapons.Clear();
            }
        }
        public void SearchModeUpdate()
        {
            SearchMode.UpdateWanted();
        }
        public void SetMoney(int Amount)
        {
            NativeFunction.CallByName<int>("STAT_SET_INT", Natives.CashHash(Settings.SettingsManager.General.MainCharacterToAlias), Amount, 1);
        }
        public void SetPlayerToLastWeapon()
        {
            if (Game.LocalPlayer.Character.Inventory.EquippedWeapon != null && LastWeaponHash != 0)
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)LastWeaponHash, true);
                //Game.Console.Print("SetPlayerToLastWeapon" + LastWeaponHash.ToString());
            }
        }
        public void SetUnarmed()
        {
            if (!(Game.LocalPlayer.Character.Inventory.EquippedWeapon == null))
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)2725352035, true); //Unequip weapon so you don't get shot
            }
        }
        public void ShootAt(Vector3 TargetCoordinate)
        {
            NativeFunction.CallByName<bool>("SET_PED_SHOOTS_AT_COORD", Game.LocalPlayer.Character, TargetCoordinate.X, TargetCoordinate.Y, TargetCoordinate.Z, true);
            GameTimeLastShot = Game.GameTime;
        }
        public void StartSmoking()
        {
            if (!IsConsuming && CanPerformActivities)
            {
                IsConsuming = true;
                CurrentConsumingActivity = new SmokingActivity(this, false);
                CurrentConsumingActivity.Start();
            }
        }
        public void StartSmokingPot()
        {
            if (!IsConsuming && CanPerformActivities)
            {
                IsConsuming = true;
                CurrentConsumingActivity = new SmokingActivity(this, true);
                CurrentConsumingActivity.Start();
            }
        }
        public void StopVanillaSearchMode()
        {
            SearchMode.StopVanilla();
        }
        public void StoreCriminalHistory()
        {
            CriminalHistory.StoreCriminalHistory(PoliceResponse);
        }
        public void TrafficViolationsUpdate()
        {
            Violations.TrafficUpdate();
        }
        public void UnSetArrestedAnimation(Ped character)
        {
            Surrendering.UnSetArrestedAnimation(character);
        }
        public void Update()
        {
            if (IsVanillaRespawnActive)
            {
                TerminateVanillaRespawnController();
            }
            TerminateVanillaRespawnScripts();
            TerminateVanillaHealthRecharge();
            TerminateVanillaAudio();

            UpdateData();
            UpdateState();
            TrackedVehiclesTick();
            CurrentHealth.Update(null);
            WeaponDropping.Tick();

            UpdateWantedLevel();
        }
        public void ViolationsUpdate()
        {
            Violations.Update();
        }
        private void ActivateVanillaRespawn()
        {
            var MyPtr = Game.GetScriptGlobalVariableAddress(4); //the script id for respawn_controller
            Marshal.WriteInt32(MyPtr, 0); //setting it to 0 turns it on somehow?
            Game.StartNewScript("respawn_controller");
            Game.StartNewScript("selector");
            IsVanillaRespawnActive = true;
        }
        private void BustedEvent()
        {
            DiedInVehicle = IsInVehicle;
            IsBusted = true;
            BeingArrested = true;
            GameTimeLastBusted = Game.GameTime;
            HandsAreUp = false;
            Surrendering.SetArrestedAnimation(Game.LocalPlayer.Character, false, WantedLevel <= 2);
            Game.LocalPlayer.HasControl = false;
        }
        private void DeathEvent()
        {
            TimeControllable.PauseTime();
            DiedInVehicle = IsInVehicle;
            IsDead = true;
            GameTimeLastDied = Game.GameTime;
            Game.LocalPlayer.Character.Kill();
            Game.LocalPlayer.Character.Health = 0;
            Game.LocalPlayer.Character.IsInvincible = true;
            TransitionToSlowMo();
        }
        private void IsAimingInVehicleChanged()
        {
            if (IsAimingInVehicle)
            {
                if (CurrentVehicle != null)
                {
                    CurrentVehicle.SetDriverWindow(true);
                }
            }
            else
            {
                if (CurrentVehicle != null)
                {
                    CurrentVehicle.SetDriverWindow(false);
                }
            }
            //Game.Console.Print(string.Format("IsAimingInVehicle Changed to: {0}", IsAimingInVehicle));
        }
        private bool IsArmed()
        {
            if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && !Game.LocalPlayer.IsFreeAiming)
            {
                return false;
            }
            else if (Game.LocalPlayer.Character.Inventory.EquippedWeapon == null)
            {
                return false;
            }
            else if (Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)2725352035
                || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)966099553
                || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)0x787F0BB//weapon_snowball
                || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)0x060EC506//weapon_fireextinguisher
                || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)0x34A67B97//weapon_petrolcan
                || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)0xBA536372//weapon_hazardcan
                || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)0x8BB05FD7//weapon_flashlight
                || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)0x23C9F95C)//weapon_ball
            {
                return false;
            }
            else if (!NativeFunction.CallByName<bool>("IS_PLAYER_CONTROL_ON", Game.LocalPlayer))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private void IsGettingIntoVehicleChanged()
        {
            if (IsGettingIntoAVehicle)
            {
                Vehicle VehicleTryingToEnter = Game.LocalPlayer.Character.VehicleTryingToEnter;
                int SeatTryingToEnter = Game.LocalPlayer.Character.SeatIndexTryingToEnter;
                if (VehicleTryingToEnter == null)
                {
                    return;
                }
                VehicleExt MyCar = EntityProvider.GetVehicle(VehicleTryingToEnter);
                if (MyCar != null)
                {
                    VehicleGettingInto = MyCar;

                    //MyCar.
                    //Maybe Call MyCar.EnterEvent(Character)
                    //that will check if you are the owner and just unlock the doors
                    //alos how to check if the car is owned by ped by the game

                    //also need in tasking when you are idle set them in, use the same function iwtht he ped passed in
                    //will need to have a list of owners or occupants?
                    //maybe check relationship group of the ped driver and if they are friendly set it unlocked

                    //add auto door locks to the cars
                    //when you start going they auto lock and peds cannot carjack you as easily

                    MyCar.AttemptToLock();
                    if (IsHoldingEnter && VehicleTryingToEnter.Driver == null && VehicleTryingToEnter.LockStatus == (VehicleLockStatus)7 && !VehicleTryingToEnter.IsEngineOn)//no driver && Unlocked
                    {
                        CarLockPick MyLockPick = new CarLockPick(this, VehicleTryingToEnter, SeatTryingToEnter);
                        MyLockPick.PickLock();
                    }
                    else if (IsHoldingEnter && SeatTryingToEnter == -1 && VehicleTryingToEnter.Driver != null && VehicleTryingToEnter.Driver.IsAlive) //Driver
                    {
                        CarJack MyJack = new CarJack(this, VehicleTryingToEnter, VehicleTryingToEnter.Driver, EntityProvider.CivilianList.FirstOrDefault(x => x.Pedestrian.Handle == VehicleTryingToEnter.Driver.Handle), SeatTryingToEnter, CurrentWeapon);
                        MyJack.StartCarJack();
                    }
                    else if (VehicleTryingToEnter.LockStatus == (VehicleLockStatus)7)
                    {
                        CarBreakIn MyBreakIn = new CarBreakIn(this, VehicleTryingToEnter);
                        MyBreakIn.BreakIn();
                    }
                }
            }
            else
            {
            }
            isGettingIntoVehicle = IsGettingIntoAVehicle;
            //Game.Console.Print(string.Format("IsGettingIntoVehicleChanged to {0}", IsGettingIntoAVehicle));
        }
        private void IsInVehicleChanged()
        {
            if (IsInVehicle)
            {
            }
            else
            {
                if (CurrentVehicle != null && CurrentVehicle.Vehicle.Exists())
                {
                    CurrentVehicle.Vehicle.IsEngineOn = LeftEngineOn;
                    ////Game.Console.Print(string.Format("IsInVehicle Changed: Current Engine Status: {0}", CurrentVehicle.Vehicle.IsEngineOn));
                }
            }
            //Game.Console.Print(string.Format("IsInVehicle Changed: {0}", IsInVehicle));
        }
        private void TerminateVanillaAudio()
        {
            if (Settings.SettingsManager.Police.DisableAmbientScanner)
            {
                NativeFunction.Natives.xB9EFD5C25018725A("PoliceScannerDisabled", true);
            }
            if (Settings.SettingsManager.Police.WantedMusicDisable)
            {
                NativeFunction.Natives.xB9EFD5C25018725A("WantedMusicDisabled", true);
            }
        }
        private void TerminateVanillaHealthRecharge()
        {
            NativeFunction.CallByName<bool>("SET_PLAYER_HEALTH_RECHARGE_MULTIPLIER", Game.LocalPlayer, 0f);
        }
        private void TerminateVanillaRespawnController()
        {
            var MyPtr = Game.GetScriptGlobalVariableAddress(4); //the script id for respawn_controller
            Marshal.WriteInt32(MyPtr, 1); //setting it to 1 turns it off somehow?
            Game.TerminateAllScriptsWithName("respawn_controller");
            IsVanillaRespawnActive = false;
        }
        private void TerminateVanillaRespawnScripts()
        {
            Game.DisableAutomaticRespawn = true;
            Game.FadeScreenOutOnDeath = false;
            Game.TerminateAllScriptsWithName("selector");
            NativeFunction.Natives.x1E0B4DC0D990A4E7(false);
            NativeFunction.Natives.x21FFB63D8C615361(true);
        }
        private void TrackedVehiclesTick()
        {
            TrackedVehicles.RemoveAll(x => !x.Vehicle.Exists());
            if (IsInVehicle)
            {
                if (CurrentVehicle == null)
                {
                    return;
                }
            }
        }
        private void TransitionToSlowMo()
        {
            Game.TimeScale = 0.4f;//Stuff below works, could add it back, it just doesnt really do much
                                  //GameFiber Transition = GameFiber.StartNew(delegate
                                  //{
                                  //    int WaitTime = 100;
                                  //    while (Game.TimeScale > 0.4f)
                                  //    {
                                  //        Game.TimeScale -= 0.05f;
                                  //        GameFiber.Wait(WaitTime);
                                  //        if (WaitTime <= 200)
                                  //            WaitTime += 1;
                                  //    }

            //}, "TransitionIn");
            //Debugging.GameFibers.Add(Transition);
        }
        private VehicleExt UpdateCurrentVehicle()
        {
            bool IsGettingIntoVehicle = Game.LocalPlayer.Character.IsGettingIntoVehicle;
            bool IsInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
            if (!IsInVehicle && !IsGettingIntoVehicle)
            {
                return null;
            }
            Vehicle CurrVehicle = null;
            if (IsGettingIntoVehicle)
            {
                CurrVehicle = Game.LocalPlayer.Character.VehicleTryingToEnter;
            }
            else
            {
                CurrVehicle = Game.LocalPlayer.Character.CurrentVehicle;
            }
            if (!CurrVehicle.Exists())
            {
                return null;
            }
            VehicleExt ToReturn = TrackedVehicles.Where(x => x.Vehicle.Handle == CurrVehicle.Handle).FirstOrDefault();
            if (ToReturn == null)
            {
                VehicleExt MyNewCar = new VehicleExt(CurrVehicle, Game.GameTime);
                TrackedVehicles.Add(MyNewCar);
                return MyNewCar;
            }
            if (IsInVehicle)
            {
                ToReturn.SetAsEntered();
            }
            ToReturn.Update();
            LeftEngineOn = ToReturn.Vehicle.IsEngineOn;
            return ToReturn;
        }
        private void UpdateData()
        {
            UpdateVehicleData();
            UpdateWeaponData();
            UpdateMiscData();
        }
        private void UpdateMiscData()
        {
            if (CurrentLocation.CharacterToLocate.Handle != Game.LocalPlayer.Character.Handle)
            {
                CurrentLocation.CharacterToLocate = Game.LocalPlayer.Character;
            }
            if (CurrentHealth.MyPed.Pedestrian.Handle != Game.LocalPlayer.Character.Handle)
            {
                CurrentHealth.MyPed = new PedExt(Game.LocalPlayer.Character);
            }
            IsStunned = Game.LocalPlayer.Character.IsStunned;
            IsRagdoll = Game.LocalPlayer.Character.IsRagdoll;

            if (NativeFunction.CallByName<bool>("GET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, 1) || NativeFunction.CallByName<int>("GET_TIMECYCLE_MODIFIER_INDEX") == 722)
            {
                IsIntoxicated = true;
            }
            else
            {
                IsIntoxicated = false;
            }
        }
        private void UpdateState()
        {
            if (Game.LocalPlayer.Character.IsDead && !IsDead)
            {
                DeathEvent();
            }
            if (NativeFunction.CallByName<bool>("IS_PLAYER_BEING_ARRESTED", 0))
            {
                BeingArrested = true;
            }
            if (NativeFunction.CallByName<bool>("IS_PLAYER_BEING_ARRESTED", 1))
            {
                BeingArrested = true;
                Game.LocalPlayer.Character.Tasks.Clear();
            }
            if (BeingArrested && !IsBusted)
            {
                BustedEvent();
            }
            if (IsAliveAndFree && !Game.LocalPlayer.Character.IsDead)
            {
                MaxWantedLastLife = WantedLevel;
            }
        }
        private void UpdateVehicleData()
        {
            IsInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
            IsGettingIntoAVehicle = Game.LocalPlayer.Character.IsGettingIntoVehicle;
            if (IsInVehicle)
            {
                IsInAirVehicle = Game.LocalPlayer.Character.IsInAirVehicle;
                if (IsInAirVehicle || Game.LocalPlayer.Character.IsInSeaVehicle || Game.LocalPlayer.Character.IsOnBike || Game.LocalPlayer.Character.IsInHelicopter)
                {
                    IsInAutomobile = false;
                }
                else
                {
                    IsInAutomobile = true;
                }

                if (Game.LocalPlayer.Character.IsOnBike)
                {
                    IsOnMotorcycle = true;
                }
                else
                {
                    IsOnMotorcycle = false;
                }
                CurrentVehicle = UpdateCurrentVehicle();
                if (CurrentVehicle != null && CurrentVehicle.Vehicle.Exists() && CurrentVehicle.Vehicle.MustBeHotwired)
                {
                    IsHotWiring = true;
                }
                else
                {
                    IsHotWiring = false;
                }

                if (CurrentVehicle != null && CurrentVehicle.Vehicle.IsEngineOn && CurrentVehicle.Vehicle.IsPoliceVehicle)
                {
                    if (!IsMobileRadioEnabled)
                    {
                        IsMobileRadioEnabled = true;
                        NativeFunction.CallByName<bool>("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", true);
                        //Game.Console.Print("Audio! Mobile Radio Enabled");
                    }
                }
                else
                {
                    if (IsMobileRadioEnabled)
                    {
                        IsMobileRadioEnabled = false;
                        NativeFunction.CallByName<bool>("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", false);
                        //Game.Console.Print("Audio! Mobile Radio Disabled");
                    }
                }
                if (Game.LocalPlayer.Character.CurrentVehicle.Speed >= 0.1f)
                {
                    GameTimeLastMoved = Game.GameTime;
                }
                if (Game.LocalPlayer.Character.CurrentVehicle.Speed >= 2.0f)
                {
                    GameTimeLastMovedFast = Game.GameTime;
                }
                IsStill = Game.LocalPlayer.Character.CurrentVehicle.Speed <= 0.1f;
            }
            else
            {
                IsOnMotorcycle = false;
                IsInAutomobile = false;
                CurrentVehicle = null;
                if (Game.LocalPlayer.Character.Speed >= 0.1f)
                {
                    GameTimeLastMoved = Game.GameTime;
                }
                if (Game.LocalPlayer.Character.Speed >= 7.0f)
                {
                    GameTimeLastMovedFast = Game.GameTime;
                }
                IsStill = Game.LocalPlayer.Character.IsStill;
                NativeFunction.CallByName<bool>("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", false);
            }
        }
        private void UpdateWantedLevel()
        {
            if (PreviousWantedLevel != Game.LocalPlayer.WantedLevel)
            {
                WantedLevelChanged();
            }
        }
        private void UpdateWeaponData()
        {
            if (Game.LocalPlayer.Character.IsShooting)
            {
                GameTimeLastShot = Game.GameTime;
            }
            IsAimingInVehicle = IsInVehicle && Game.LocalPlayer.IsFreeAiming;
            IsConsideredArmed = IsArmed();
            WeaponDescriptor PlayerCurrentWeapon = Game.LocalPlayer.Character.Inventory.EquippedWeapon;
            CurrentWeapon = Weapons.GetCurrentWeapon(Game.LocalPlayer.Character);
            if (PlayerCurrentWeapon != null)
            {
                CurrentWeaponHash = PlayerCurrentWeapon.Hash;
            }
            else
            {
                CurrentWeaponHash = 0;
            }

            if (CurrentWeaponHash != 0 && PlayerCurrentWeapon.Hash != LastWeaponHash)
            {
                LastWeaponHash = PlayerCurrentWeapon.Hash;
            }
        }
        private void WantedLevelChanged()
        {
            if (IsNotWanted)//removed
            {
                if (!IsDead)
                {
                    if (PoliceResponse.PlayerSeenDuringWanted && PreviousWantedLevel != 0)
                    {
                        StoreCriminalHistory();
                    }
                    PoliceResponse = new PoliceResponse(this);
                }
                PoliceResponse.OnLostWanted();
            }
            else if (IsWanted && PreviousWantedLevel == 0)//added
            {
                if(!PoliceResponse.RecentlySetWanted)//only allow my process to set the wanted level
                {
                    Game.LocalPlayer.WantedLevel = 0;
                }
                else
                {
                    Investigation.Reset();
                    PoliceResponse.OnBecameWanted();
                }  
            }
            else
            {
                PoliceResponse.OnWantedLevelIncreased();
            }
            PreviousWantedLevel = Game.LocalPlayer.WantedLevel;
        }
    }
}