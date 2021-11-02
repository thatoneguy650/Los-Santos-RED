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
using System.Windows.Forms;

namespace Mod
{
    public class Player : IDispatchable, IActivityPerformable, IIntoxicatable, ITargetable, IPoliceRespondable, IInputable, IPedSwappable, IMuggable, IRespawnable, IViolateable, IWeaponDroppable, IDisplayable, ICarStealable, IPlateChangeable, IActionable, IInteractionable, IInventoryable, IRespawning, ISaveable
    {
        public int UpdateState = 0;
        private ICrimes Crimes;
        private CriminalHistory CriminalHistory;
        private string CurrentVehicleDebugString;
        private DynamicActivity DynamicActivity;
        private IEntityProvideable EntityProvider;
        private uint GameTimeLastBusted;
        private uint GameTimeLastDied;
        private uint GameTimeLastMoved;
        private uint GameTimeLastMovedFast;
        private uint GameTimeLastSetWanted;
        private uint GameTimeLastShot;
        private uint GameTimeLastUpdatedLookedAtPed;
        private uint GameTimeStartedHotwiring;
        private uint GameTimeStartedPlaying;
        private uint GameTimeWantedLevelStarted;
        private HealthState HealthState;
        private Inventory Inventory;
        private bool isActive = true;
        private bool isAiming;
        private bool isAimingInVehicle;
        private bool isCurrentVehicleEngineOn;
        private bool isGettingIntoVehicle;
        private bool isHotwiring;
        private bool isInVehicle;
        private int PreviousWantedLevel;
        private IRadioStations RadioStations;
        private Respawning Respawning;
        private Scanner Scanner;
        private IScenarios Scenarios;
        private SearchMode SearchMode;
        private ISettingsProvideable Settings;
        private SurrenderActivity Surrendering;
        private uint targettingHandle;
        private ITimeControllable TimeControllable;
        private WeaponDropping WeaponDropping;
        private IWeapons Weapons;
        private uint GameTimeLastCheckedAmbientCrimes;

        public Player(string modelName, bool isMale, string suspectsName, IEntityProvideable provider, ITimeControllable timeControllable, IStreets streets, IZones zones, ISettingsProvideable settings, IWeapons weapons, IRadioStations radioStations, IScenarios scenarios, ICrimes crimes, IAudioPlayable audio, IPlacesOfInterest placesOfInterest)
        {
            ModelName = modelName;
            IsMale = isMale;
            PlayerName = suspectsName;
            Crimes = crimes;
            EntityProvider = provider;
            TimeControllable = timeControllable;
            Settings = settings;
            Weapons = weapons;
            RadioStations = radioStations;
            Scenarios = scenarios;
            GameTimeStartedPlaying = Game.GameTime;
            Scanner = new Scanner(provider, this, audio, Settings);
            HealthState = new HealthState(new PedExt(Game.LocalPlayer.Character, Settings, Crimes), Settings);
            CurrentLocation = new LocationData(Game.LocalPlayer.Character, streets, zones);
            WeaponDropping = new WeaponDropping(this, Weapons, Settings);
            Surrendering = new SurrenderActivity(this);
            Violations = new Violations(this, TimeControllable, Crimes, Settings);
            Investigation = new Investigation(this, Settings, provider);
            CriminalHistory = new CriminalHistory(this, Settings);
            PoliceResponse = new PoliceResponse(this, Settings);
            SearchMode = new SearchMode(this, Settings);
            Inventory = new Inventory(this);    
            Respawning = new Respawning(TimeControllable, EntityProvider, this, Weapons, placesOfInterest, Settings);
        }
        public float ActiveDistance => Investigation.IsActive ? Investigation.Distance : 500f + (WantedLevel * 200f);
        public bool AnyHumansNear => EntityProvider.PoliceList.Any(x => x.DistanceToPlayer <= 10f) || EntityProvider.CivilianList.Any(x => x.DistanceToPlayer <= 10f); //move or delete?
        public bool AnyPoliceCanHearPlayer { get; set; } //all this perception stuff gets moved out?
        public bool AnyPoliceCanRecognizePlayer { get; set; }
        public bool AnyPoliceCanSeePlayer { get; set; }
        public bool AnyPoliceRecentlySeenPlayer { get; set; }
        public string AutoTuneStation { get; set; } = "NONE";
        public bool BeingArrested { get; private set; }
        public List<ButtonPrompt> ButtonPrompts { get; private set; } = new List<ButtonPrompt>();
        public bool CanConverse => !IsGettingIntoAVehicle && !IsBreakingIntoCar && !IsIncapacitated && !IsVisiblyArmed && IsAliveAndFree && !IsMovingDynamically;
        public bool CanConverseWithLookedAtPed => CurrentLookedAtPed != null && CurrentTargetedPed == null && CurrentLookedAtPed.CanConverse && CanConverse; // && (Relationship)NativeFunction.Natives.GET_RELATIONSHIP_BETWEEN_PEDS<int>(CurrentLookedAtPed.Pedestrian, Character) != Relationship.Hate;//off for performance checking
        public bool CanDropWeapon => CanPerformActivities && WeaponDropping.CanDropWeapon;
        public bool CanHoldUpTargettedPed => CurrentTargetedPed != null && CurrentTargetedPed.CanBeMugged && !IsGettingIntoAVehicle && !IsBreakingIntoCar && !IsStunned && !IsRagdoll && IsVisiblyArmed && IsAliveAndFree && CurrentTargetedPed.DistanceToPlayer <= 7f;
        public bool CanPerformActivities => !IsMovingFast && !IsIncapacitated && !IsDead && !IsBusted && !IsInVehicle && !IsGettingIntoAVehicle && !IsMovingDynamically;
        public bool CanSurrender => Surrendering.CanSurrender;
        public bool CanUndie => Respawning.CanUndie;
        public Ped Character => Game.LocalPlayer.Character;
        public Scenario ClosestScenario { get; private set; }
        public LocationData CurrentLocation { get; set; }
        public PedExt CurrentLookedAtPed { get; private set; }
        public string CurrentModelName { get; set; }//should be private but needed?
        public PedVariation CurrentModelVariation { get; set; }
        public VehicleExt CurrentSeenVehicle => CurrentVehicle ?? VehicleGettingInto;
        public WeaponInformation CurrentSeenWeapon => !IsInVehicle ? CurrentWeapon : null;
        public PedExt CurrentTargetedPed { get; private set; }
        public VehicleExt CurrentVehicle { get; private set; }
        public WeaponInformation CurrentWeapon { get; private set; }
        public WeaponCategory CurrentWeaponCategory => CurrentWeapon != null ? CurrentWeapon.Category : WeaponCategory.Unknown;
        public WeaponHash CurrentWeaponHash { get; set; }
        public bool CurrentWeaponIsOneHanded { get; private set; }
        public string DebugLine1 => $"Player: {ModelName},{Game.LocalPlayer.Character.Handle} RcntStrPly: {RecentlyStartedPlaying} IsMovingDynam: {IsMovingDynamically} IsIntoxicated: {IsIntoxicated}";
        public string DebugLine2 => $"Vio: {Violations.LawsViolatingDisplay}";
        public string DebugLine3 => $"Rep: {PoliceResponse.ReportedCrimesDisplay}";
        public string DebugLine4 => $"Obs: {PoliceResponse.ObservedCrimesDisplay}";
        public string DebugLine5 => CurrentVehicleDebugString;
        public string DebugLine6 => SearchMode.SearchModeDebug;
        public string DebugLine7 => $"AnyPolice: CanSee: {AnyPoliceCanSeePlayer}, RecentlySeen: {AnyPoliceRecentlySeenPlayer}, CanHear: {AnyPoliceCanHearPlayer}, CanRecognize {AnyPoliceCanRecognizePlayer}";
        public string DebugLine8 => $"PlacePoliceLastSeenPlayer {PlacePoliceLastSeenPlayer}";
        public string DebugLine9 => CurrentVehicle != null ? $"IsEngineRunning: {CurrentVehicle.Engine.IsRunning}" : $"NO VEHICLE" + $" IsGettingIntoAVehicle: {IsGettingIntoAVehicle}, IsInVehicle: {IsInVehicle}";
        public string DebugLine10 => $"Cop#: {EntityProvider.PoliceList.Count()} CopCar#: {EntityProvider.PoliceVehicleCount} Civ#: {EntityProvider.CivilianList.Count()} CivCar:#: {EntityProvider.CivilianVehicleCount} Tracked#: {TrackedVehicles.Count}";
        public string DebugLine11 { get; set; }
        public Scanner DebugScanner => Scanner;
        //move or delete?
        //should be private but needed?
        public bool DiedInVehicle { get; private set; }
        public bool HandsAreUp { get; set; }
        public uint HasBeenWantedFor => PoliceResponse.HasBeenWantedFor;
        public bool HasCriminalHistory => CriminalHistory.HasHistory;
        public Interaction Interaction { get; private set; }
        public float IntoxicatedIntensity { get; set; }
        public Investigation Investigation { get; private set; }
        public bool IsAiming
        {
            get => isAiming;
            private set
            {
                if (isAiming != value)
                {
                    isAiming = value;
                    OnAimingChanged();
                }
            }
        }
        public bool IsAimingInVehicle
        {
            get => isAimingInVehicle;
            private set
            {
                if (isAimingInVehicle != value)
                {
                    isAimingInVehicle = value;
                    OnAimingInVehicleChanged();
                }
            }
        }
        public bool IsAliveAndFree => !IsBusted && !IsDead;
        public bool IsAttemptingToSurrender => HandsAreUp && !PoliceResponse.IsWeaponsFree;
        public bool IsBreakingIntoCar => IsCarJacking || IsLockPicking || IsHotWiring || Game.LocalPlayer.Character.IsJacking;
        public bool IsBustable => IsAliveAndFree && PoliceResponse.HasBeenWantedFor >= 3000 && !Surrendering.IsCommitingSuicide && !RecentlyBusted && !RecentlyResistedArrest && !PoliceResponse.IsWeaponsFree && (IsIncapacitated || (!IsMoving && !IsMovingDynamically));//took out vehicle in here, might need at one star vehicle is ok
        public bool IsBusted { get; private set; }
        public bool IsCarJacking { get; set; }
        public bool IsChangingLicensePlates { get; set; }
        public bool IsCommitingSuicide { get; set; }
        public bool IsConversing { get; set; }
        public bool IsDead { get; private set; }
        public bool IsDriver { get; private set; }
        public bool IsGettingIntoAVehicle
        {
            get => isGettingIntoVehicle;
            private set
            {
                if (isGettingIntoVehicle != value)
                {
                    isGettingIntoVehicle = value;
                    OnGettingIntoAVehicleChanged();
                }
            }
        }
        public bool IsHoldingUp { get; set; }
        public bool IsHotWiring { get; private set; }
        public bool IsInAirVehicle { get; private set; }
        public bool IsInAutomobile { get; private set; }
        public bool IsIncapacitated => IsStunned || IsRagdoll;
        public bool IsInSearchMode { get; set; }
        public bool IsInteracting => IsConversing || IsHoldingUp;
        public bool IsIntoxicated { get; set; }
        public bool IsInVehicle
        {
            get => isInVehicle;
            private set
            {
                if (isInVehicle != value)
                {
                    isInVehicle = value;
                    OnIsInVehicleChanged();
                }
            }
        }
        public bool IsLockPicking { get; set; }
        public bool IsMale { get; set; }
        public bool IsMobileRadioEnabled { get; private set; }
        public bool IsMoveControlPressed { get; set; }
        public bool IsMoving => GameTimeLastMoved != 0 && Game.GameTime - GameTimeLastMoved <= 2000;
        public bool IsMovingDynamically { get; private set; }
        public bool IsMovingFast => GameTimeLastMovedFast != 0 && Game.GameTime - GameTimeLastMovedFast <= 2000;
        public bool IsNearScenario { get; private set; }
        public bool IsNotHoldingEnter { get; set; }
        public bool IsNotWanted => Game.LocalPlayer.WantedLevel == 0;
        public bool IsOnMotorcycle { get; private set; }
        public bool IsPerformingActivity { get; set; }
        public bool IsRagdoll { get; private set; }
        public bool IsSpeeding => Violations.IsSpeeding;
        public bool IsStill { get; private set; }
        public bool IsStunned { get; private set; }
        public bool IsViolatingAnyTrafficLaws => Violations.IsViolatingAnyTrafficLaws;
        public bool IsVisiblyArmed { get; private set; }
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
                    NativeFunction.CallByName<int>("STAT_GET_INT", NativeHelper.CashHash(Settings.SettingsManager.PedSwapSettings.MainCharacterToAlias), &CurrentCash, -1);
                }
                return CurrentCash;
            }
        }
        public PoolHandle OwnedVehicleHandle { get; set; }
        public Vector3 PlacePoliceLastSeenPlayer { get; set; }
        public string PlayerName { get; private set; }
        public PoliceResponse PoliceResponse { get; private set; }
        public Vector3 Position => Game.LocalPlayer.Character.Position;
        public bool RecentlyBribedPolice => Respawning.RecentlyBribedPolice;
        public bool RecentlyBusted => GameTimeLastBusted != 0 && Game.GameTime - GameTimeLastBusted <= 5000;
        public bool RecentlyPaidFine => Respawning.RecentlyPaidFine;
        public bool RecentlyResistedArrest => Respawning.RecentlyResistedArrest;
        public bool RecentlyRespawned => Respawning.RecentlyRespawned;
        public bool RecentlySetWanted => GameTimeLastSetWanted != 0 && Game.GameTime - GameTimeLastSetWanted <= 5000;
        public bool RecentlyShot => GameTimeLastShot != 0 && !RecentlyStartedPlaying && Game.GameTime - GameTimeLastShot <= 3000;
        public bool RecentlyStartedPlaying => GameTimeStartedPlaying != 0 && Game.GameTime - GameTimeStartedPlaying <= 3000;
        public List<VehicleExt> ReportedStolenVehicles => TrackedVehicles.Where(x => x.NeedsToBeReportedStolen && !x.HasBeenDescribedByDispatch && !x.AddedToReportedStolenQueue).ToList();
        public Vector3 RootPosition { get; set; }
        public float SearchModePercentage => SearchMode.SearchModePercentage;
        public List<LicensePlate> SpareLicensePlates { get; private set; } = new List<LicensePlate>();
        public uint TargettingHandle
        {
            get => targettingHandle;
            private set
            {
                if (targettingHandle != value)
                {
                    targettingHandle = value;
                    OnTargettingHandleChanged();
                }
            }
        }
        public uint TimeInSearchMode => SearchMode.TimeInSearchMode;
        public int TimesDied => Respawning.TimesDied;
        public uint TimeToRecognize
        {
            get
            {
                uint Time = Settings.SettingsManager.PlayerSettings.Recognize_BaseTime;
                if (TimeControllable.IsNight)
                {
                    Time += Settings.SettingsManager.PlayerSettings.Recognize_NightPenalty;
                }
                else if (IsInVehicle)
                {
                    Time += Settings.SettingsManager.PlayerSettings.Recognize_VehiclePenalty;
                }
                return Time;
            }
        }
        public List<VehicleExt> TrackedVehicles { get; private set; } = new List<VehicleExt>();
        public VehicleExt VehicleGettingInto { get; private set; }
        public float VehicleSpeed { get; private set; }
        public float VehicleSpeedKMH => VehicleSpeed * 3.6f;
        public float VehicleSpeedMPH => VehicleSpeed * 2.23694f;
        public Violations Violations { get; private set; }
        public int WantedLevel => Game.LocalPlayer.WantedLevel;
        public void AddCrime(Crime crimeObserved, bool isObservedByPolice, Vector3 Location, VehicleExt VehicleObserved, WeaponInformation WeaponObserved, bool HaveDescription, bool AnnounceCrime)
        {
            CrimeSceneDescription description = new CrimeSceneDescription(!IsInVehicle, isObservedByPolice, Location, HaveDescription) { VehicleSeen = VehicleObserved, WeaponSeen = WeaponObserved, Speed = Game.LocalPlayer.Character.Speed };
            PoliceResponse.AddCrime(crimeObserved, description);
            if(AnnounceCrime)
            {
                Scanner.AnnounceCrime(crimeObserved, description);
            }
            if (!isObservedByPolice && IsNotWanted)
            {
                Investigation.Start();       
            }
        }
        public void AddCrimeToHistory(Crime crime) => CriminalHistory.AddCrime(crime);
        public void AddInjured(PedExt MyPed) => Violations.AddInjured(MyPed);
        public void AddKilled(PedExt MyPed) => Violations.AddKilled(MyPed);
        public void Arrest()
        {
            BeingArrested = true;
            if (!IsBusted)
            {
                OnPlayerBusted();
            }
        }
        public void ArrestWarrantUpdate() => CriminalHistory.Update();
        public bool BribePolice(int bribeAmount)
        {
            bool toReturn = Respawning.BribePolice(bribeAmount);
            if (toReturn)
            {
                Scanner.OnBribedPolice();
            }
            return toReturn;
        }
        public void CallPolice()
        {
            PedExt violatingCiv = EntityProvider.CivilianList.Where(x => x.DistanceToPlayer <= 90f).OrderByDescending(x => x.CurrentlyViolatingWantedLevel).FirstOrDefault();
            if(violatingCiv != null && violatingCiv.Pedestrian.Exists() && violatingCiv.CrimesCurrentlyViolating.Any())
            {
                Crime ToCallIn = violatingCiv.CrimesCurrentlyViolating.OrderBy(x => x.Priority).FirstOrDefault();
                if(ToCallIn != null)
                {
                    AddCrime(ToCallIn, false, Position, null, null, false, true);
                }
                else
                {
                    AddCrime(Crimes.CrimeList.FirstOrDefault(x => x.ID == "OfficersNeeded"), false, Position, null, null, false, true);
                }
            }
            else
            {
                AddCrime(Crimes.CrimeList.FirstOrDefault(x => x.ID == "OfficersNeeded"), false, Position, null, null, false, true);
            }
        }
        public void CallInAmbientCrimes()
        {
            //dont like this at all
            PedExt violatingCiv = EntityProvider.CivilianList.Where(x => x.RecentlySeenPlayer).OrderByDescending(x => x.CurrentlyViolatingWantedLevel).FirstOrDefault();
            bool HasReporters = EntityProvider.CivilianList.Any(x => x.DistanceToPlayer <= 90f && x.WillCallPolice && x.Pedestrian.Exists() && x.Pedestrian.IsAlive);
            if (HasReporters)
            {
                if (violatingCiv != null && violatingCiv.Pedestrian.Exists() && violatingCiv.CrimesCurrentlyViolating.Any())
                {
                    Crime ToCallIn = violatingCiv.CrimesCurrentlyViolating.OrderBy(x => x.Priority).FirstOrDefault();
                    if (ToCallIn != null)
                    {
                        AddCrime(ToCallIn, false, Position, null, null, false, true);
                    }
                }
            }
        }
        public void ChangePlate(int Index)
        {
            if (!IsPerformingActivity && CanPerformActivities)
            {
                if (DynamicActivity != null)
                {
                    DynamicActivity.Cancel();
                }
                IsPerformingActivity = true;
                DynamicActivity = new PlateTheft(this, SpareLicensePlates[Index], Settings);
                DynamicActivity.Start();
            }
        }
        public void ChangePlate(LicensePlate toChange)
        {
            if (!IsPerformingActivity && CanPerformActivities)
            {
                if (DynamicActivity != null)
                {
                    DynamicActivity.Cancel();
                }
                IsPerformingActivity = true;
                DynamicActivity = new PlateTheft(this, toChange, Settings);
                DynamicActivity.Start();
            }
        }
        public void CommitSuicide()
        {
            if (!IsPerformingActivity && CanPerformActivities)
            {
                if (DynamicActivity != null)
                {
                    DynamicActivity.Cancel();
                }
                IsPerformingActivity = true;
                DynamicActivity = new SuicideActivity(this, Settings);
                DynamicActivity.Start();
            }
        }
        public void DeleteTrackedVehicles()
        {
            TrackedVehicles.Clear();
        }
        public void DisplayPlayerNotification()
        {
            string NotifcationText = "Warrants: ~g~None~s~";
            if (PoliceResponse.HasObservedCrimes)
            {
                NotifcationText = "Wanted For:" + PoliceResponse.PrintCrimes();
            }
            else if (HasCriminalHistory)
            {
                NotifcationText = "Wanted For:" + PrintCriminalHistory();
            }
            VehicleExt OwnedVehicle = TrackedVehicles.FirstOrDefault(x => x.Vehicle.Exists() && x.Vehicle.Handle == OwnedVehicleHandle);
            if (OwnedVehicle != null)
            {
                string Make = OwnedVehicle.MakeName();
                string Model = OwnedVehicle.ModelName();
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
                NotifcationText += string.Format("~n~Plate: ~p~{0}~s~", OwnedVehicle.CarPlate.PlateNumber);
            }
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~b~Personal Info", string.Format("~y~{0}", PlayerName), NotifcationText);
        }
        public void Dispose()
        {
            Investigation.Dispose(); //remove blip
            PoliceResponse.Dispose(); //same ^
            Interaction?.Dispose();
            SearchMode.Dispose();
            isActive = false;
            NativeFunction.Natives.SET_PED_CONFIG_FLAG<bool>(Game.LocalPlayer.Character, (int)PedConfigFlags._PED_FLAG_DISABLE_STARTING_VEH_ENGINE, false);
            // NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags._PED_FLAG_DISABLE_STARTING_VEH_ENGINE, false);
            MakeSober();
            Game.LocalPlayer.WantedLevel = 0;
            Game.TimeScale = 1f;
        }
        public void DrinkBeer()
        {
            if (!IsPerformingActivity && CanPerformActivities)
            {
                if (DynamicActivity != null)
                {
                    DynamicActivity.Cancel();
                }
                IsPerformingActivity = true;
                DynamicActivity = new DrinkingActivity(this, Settings);
                DynamicActivity.Start();
            }
        }
        public void DropWeapon() => WeaponDropping.DropWeapon();
        public void GiveMoney(int Amount)
        {
            int CurrentCash;
            uint PlayerCashHash = NativeHelper.CashHash(Settings.SettingsManager.PedSwapSettings.MainCharacterToAlias);
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
        public void LocationUpdate() => CurrentLocation.Update(Character);
        public void LowerHands() => Surrendering.LowerHands();
        public void OnAppliedWantedStats() => Scanner.OnAppliedWantedStats();
        public void OnInvestigationExpire()
        {
            PoliceResponse.Reset();
            Scanner.OnInvestigationExpire();
        }
        public void OnLawEnforcementSpawn(Agency agency, DispatchableVehicle vehicleType, DispatchablePerson officerType)
        {
            if (IsWanted)
            {
                if (agency.ID == "ARMY")
                {
                    Scanner.OnArmyDeployed();
                }
                else if (agency.ID == "NOOSE")
                {
                    Scanner.OnNooseDeployed();
                }
                else if (vehicleType.IsHelicopter)
                {
                    Scanner.OnHelicoptersDeployed();
                }
            }
        }
        public void OnLethalForceAuthorized() => Scanner.OnLethalForceAuthorized();
        public void OnPoliceNoticeVehicleChange() => Scanner.OnPoliceNoticeVehicleChange();
        public void OnRequestedBackUp() => Scanner.OnRequestedBackUp();
        public void OnSuspectEluded()//runs before OnWantedLevelChanged
        {
            CriminalHistory.OnSuspectEluded(PoliceResponse.CrimesObserved.Select(x => x.AssociatedCrime).ToList(), PlacePoliceLastSeenPlayer);
            Scanner.OnSuspectEluded();
        }
        public void OnWantedActiveMode() => Scanner.OnWantedActiveMode();
        public void OnWantedSearchMode() => Scanner.OnWantedSearchMode();
        public void OnWeaponsFree() => Scanner.OnWeaponsFree();
        public bool PayFine()
        {
            bool toReturn = Respawning.PayFine();
            if (toReturn)
            {
                Scanner.OnPaidFine();
            }
            return toReturn;
        }
        public string PrintCriminalHistory() => CriminalHistory.PrintCriminalHistory();
        public void RaiseHands() => Surrendering.RaiseHands();
        public void RemovePlate()
        {
            if (!IsPerformingActivity && CanPerformActivities)
            {
                if (DynamicActivity != null)
                {
                    DynamicActivity.Cancel();
                }
                IsPerformingActivity = true;
                DynamicActivity = new PlateTheft(this, Settings);
                DynamicActivity.Start();
            }
        }
        public void Reset(bool resetWanted, bool resetTimesDied, bool clearWeapons, bool clearCriminalHistory)
        {
            IsDead = false;
            IsBusted = false;
            Game.LocalPlayer.HasControl = true;
            BeingArrested = false;
            //HealthState = new HealthState(new PedExt(Game.LocalPlayer.Character, Settings), Settings);
            HealthState.Reset();
            IsPerformingActivity = false;
            if (resetWanted)
            {
                PoliceResponse.Reset();
                Investigation.Reset();
                Violations.Reset();
                MaxWantedLastLife = 0;
                GameTimeStartedPlaying = Game.GameTime;
                Scanner.Reset();
                //OwnedVehicleHandle = 0;
                Update();

                //GameFiber.StartNew(delegate
                //{
                //    uint GameTimeLastResetWanted = Game.GameTime;
                //    while (Game.GameTime - GameTimeLastResetWanted <= 5000)
                //    {
                //        if (Game.LocalPlayer.WantedLevel != 0)
                //        {
                //            SetWantedLevel(0, "Player Reset with resetWanted: resetting afterwards", true);
                //        }
                //        GameFiber.Yield();
                //    }

                //}, "Wanted Level Stopper");


            }
            if (resetTimesDied)
            {
                Respawning.Reset();
            }
            if (clearWeapons)
            {
                Game.LocalPlayer.Character.Inventory.Weapons.Clear();
            }
            if (clearCriminalHistory)
            {
                CriminalHistory.Clear();
            }       
        }
        public void ResetScanner() => Scanner.Reset();
        public void ResistArrest() => Respawning.ResistArrest();
        public void RespawnAtCurrentLocation(bool withInvicibility, bool resetWanted, bool clearCriminalHistory) => Respawning.RespawnAtCurrentLocation(withInvicibility, resetWanted, clearCriminalHistory);
        public void RespawnAtGrave() => Respawning.RespawnAtGrave();
        public void RespawnAtHospital(GameLocation currentSelectedHospitalLocation) => Respawning.RespawnAtHospital(currentSelectedHospitalLocation);
        public void ScannerUpdate() => Scanner.Tick();
        public void SearchModeUpdate() => SearchMode.UpdateWanted();
        public void SetDemographics(string modelName, bool isMale, string playerName, int money)
        {
            ModelName = modelName;
            PlayerName = playerName;
            IsMale = isMale;
            SetMoney(money);
            EntryPoint.WriteToConsole($"PLAYER EVENT: SetDemographics MoneyToSet {money} Current: {Money} {NativeHelper.CashHash(Settings.SettingsManager.PedSwapSettings.MainCharacterToAlias)}", 3);
        }
        public void SetMoney(int Amount)
        {
            NativeFunction.CallByName<int>("STAT_SET_INT", NativeHelper.CashHash(Settings.SettingsManager.PedSwapSettings.MainCharacterToAlias), Amount, 1);
        }
        public void SetPlayerToLastWeapon()
        {
            if (Game.LocalPlayer.Character.Inventory.EquippedWeapon != null && LastWeaponHash != 0)
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)LastWeaponHash, true);
                //EntryPoint.WriteToConsole("SetPlayerToLastWeapon" + LastWeaponHash.ToString());
            }
        }
        public void SetUnarmed()
        {
            if (!(Game.LocalPlayer.Character.Inventory.EquippedWeapon == null))
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)2725352035, true); //Unequip weapon so you don't get shot
            }
        }
        public void Setup()
        {
            SetWantedLevel(0, "Initial", true);
            SetUnarmed();
            SpareLicensePlates.Add(new LicensePlate(RandomItems.RandomString(8), 3, false));//random cali
            CurrentModelName = Game.LocalPlayer.Character.Model.Name;
            CurrentModelVariation = NativeHelper.GetPedVariation(Game.LocalPlayer.Character);
            if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Exists())
            {
                OwnedVehicleHandle = Game.LocalPlayer.Character.CurrentVehicle.Handle;
            }


            if (Settings.SettingsManager.PlayerSettings.DisableAutoEngineStart)
            {
                NativeFunction.Natives.SET_PED_CONFIG_FLAG<bool>(Game.LocalPlayer.Character, (int)PedConfigFlags._PED_FLAG_DISABLE_STARTING_VEH_ENGINE, true);
            }
            GameFiber.StartNew(delegate
            {
                while (isActive)
                {
                    if (Game.LocalPlayer.Character.IsShooting)
                    {
                        GameTimeLastShot = Game.GameTime;
                    }
                    GameFiber.Yield();
                }

            }, "IsShootingChecker");
            if (Settings.SettingsManager.PlayerSettings.KeepRadioStationAutoTuned)
            {
                AutoTuneStation = Settings.SettingsManager.PlayerSettings.AutoTuneRadioStation;// "RADIO_19_USER";
            }
            #if DEBUG
                        AutoTuneStation = "RADIO_19_USER";
            #endif
        }
        public void SetWantedLevel(int desiredWantedLevel, string Reason, bool UpdateRecent)
        {
            if (UpdateRecent)
            {
                GameTimeLastSetWanted = Game.GameTime;
            }
            if (WantedLevel < desiredWantedLevel || (desiredWantedLevel == 0 && WantedLevel != 0))
            {
                NativeFunction.CallByName<bool>("SET_MAX_WANTED_LEVEL", desiredWantedLevel);
                Game.LocalPlayer.WantedLevel = desiredWantedLevel;
                if (desiredWantedLevel > 0)
                {
                    GameTimeWantedLevelStarted = Game.GameTime;
                }
                OnWantedLevelChanged();
                EntryPoint.WriteToConsole($"Set Wanted: From {WantedLevel} to {desiredWantedLevel} Reason: {Reason}", 3);
            }
        }
        public void ShootAt(Vector3 TargetCoordinate)
        {
            NativeFunction.CallByName<bool>("SET_PED_SHOOTS_AT_COORD", Game.LocalPlayer.Character, TargetCoordinate.X, TargetCoordinate.Y, TargetCoordinate.Z, true);
            GameTimeLastShot = Game.GameTime;
        }
        public void StartConversation()
        {
            if (!IsInteracting && CanConverseWithLookedAtPed)
            {
                if (Interaction != null)
                {
                    Interaction.Dispose();
                }
                IsConversing = true;
                Interaction = new Conversation(this, CurrentLookedAtPed, Settings);
                Interaction.Start();
            }
        }
        public void StartHoldUp()
        {
            if (!IsInteracting && CanHoldUpTargettedPed)
            {
                if (Interaction != null)
                {
                    Interaction.Dispose();
                }
                IsHoldingUp = true;
                Interaction = new HoldUp(this, CurrentTargetedPed, Settings);
                Interaction.Start();
            }
        }
        public void StartScenario()
        {
            if (!IsPerformingActivity && CanPerformActivities)
            {
                if (DynamicActivity != null)
                {
                    DynamicActivity.Cancel();
                }
                IsPerformingActivity = true;
                DynamicActivity = new ScenarioActivity(this);
                DynamicActivity.Start();
            }
        }
        public void StartSmoking()
        {
            if (!IsPerformingActivity && CanPerformActivities)
            {
                if (DynamicActivity != null)
                {
                    DynamicActivity.Cancel();
                }
                IsPerformingActivity = true;
                DynamicActivity = new SmokingActivity(this, false, Settings);
                DynamicActivity.Start();
            }
            else if (IsPerformingActivity && CanPerformActivities)
            {
                DynamicActivity.Continue();
            }
        }
        public void StartSmokingPot()
        {
            if (!IsPerformingActivity && CanPerformActivities)
            {
                if (DynamicActivity != null)
                {
                    DynamicActivity.Cancel();
                }
                IsPerformingActivity = true;
                DynamicActivity = new SmokingActivity(this, true, Settings);
                DynamicActivity.Start();
            }
            else if (IsPerformingActivity && CanPerformActivities)
            {
                DynamicActivity.Continue();
            }
        }
        public void StopDynamicActivity()
        {
            if (IsPerformingActivity)
            {
                DynamicActivity?.Cancel();
                IsPerformingActivity = false;
            }
        }
        public void StopVanillaSearchMode() => SearchMode.StopVanilla();
        public void SurrenderToPolice(GameLocation currentSelectedSurrenderLocation) => Respawning.SurrenderToPolice(currentSelectedSurrenderLocation);
        public void TakeOwnershipOfNearestCar()
        {
            Vehicle vehicleToCheck = (Vehicle)Rage.World.GetClosestEntity(Character.Position, 10f, GetEntitiesFlags.ConsiderCars | GetEntitiesFlags.ExcludeOccupiedVehicles);
            VehicleExt FoundVehicle;
            if (vehicleToCheck != null)
            {
                FoundVehicle = TrackedVehicles.Where(x => x.Vehicle.Handle == vehicleToCheck.Handle).FirstOrDefault();
                if (FoundVehicle == null)
                {
                    FoundVehicle = new VehicleExt(vehicleToCheck, Settings);
                    TrackedVehicles.Add(FoundVehicle);
                }
                FoundVehicle.SetNotWanted();
                OwnedVehicleHandle = FoundVehicle.Vehicle.Handle;
                DisplayPlayerNotification();
            }
            else
            {
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~b~Personal Info", string.Format("~y~{0}", PlayerName), "No Vehicle Found");
            }
        }
        public void TrafficViolationsUpdate() => Violations.UpdateTraffic();
        public void UnSetArrestedAnimation(Ped character) => Surrendering.UnSetArrestedAnimation(character);
        public void Update()
        {
           UpdateData();
           UpdateButtonPrompts();
        }
        public void UpdateCurrentVehicle() //should this be public?
        {
            bool IsGettingIntoVehicle = Game.LocalPlayer.Character.IsGettingIntoVehicle;
            bool IsInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
            if (!IsInVehicle && !IsGettingIntoVehicle)
            {
                CurrentVehicle = null;
                return;
            }
            Vehicle vehicle;
            if (IsGettingIntoVehicle)
            {
                vehicle = Game.LocalPlayer.Character.VehicleTryingToEnter;
            }
            else
            {
                vehicle = Game.LocalPlayer.Character.CurrentVehicle;
            }
            if (!vehicle.Exists())
            {
                CurrentVehicle = null;
                return;
            }
            VehicleExt existingVehicleExt = EntityProvider.GetVehicleExt(vehicle);
            //GameFiber.Yield();
            if (existingVehicleExt == null)
            {
                VehicleExt createdVehicleExt = new VehicleExt(vehicle, Settings);
                TrackedVehicles.Add(createdVehicleExt);
                existingVehicleExt = createdVehicleExt;
            }
            if (!TrackedVehicles.Any(x => x.Vehicle.Handle == vehicle.Handle))
            {
                TrackedVehicles.Add(existingVehicleExt);
            }
            if (IsInVehicle && !existingVehicleExt.HasBeenEnteredByPlayer)
            {
                existingVehicleExt.SetAsEntered();
            }
            existingVehicleExt.Update(AutoTuneStation, Settings.SettingsManager.PlayerSettings.UseCustomFuelSystem, Settings.SettingsManager.PlayerSettings.ScaleEngineDamage);
            if (!existingVehicleExt.IsStolen)
            {
                if (IsDriver && existingVehicleExt.Vehicle.Handle != OwnedVehicleHandle)
                {
                    existingVehicleExt.IsStolen = true;
                }
            }
            CurrentVehicle = existingVehicleExt;
        }
        public void UpdateStateData()
        {
            if (Game.LocalPlayer.Character.IsDead && !IsDead)
            {
                OnPlayerDied();
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
                OnPlayerBusted();
            }
            if (IsAliveAndFree && !Game.LocalPlayer.Character.IsDead)
            {
                MaxWantedLastLife = WantedLevel;
            }
            if (PreviousWantedLevel != Game.LocalPlayer.WantedLevel)
            {
                OnWantedLevelChanged();
            }
            if (CurrentLocation.CharacterToLocate.Exists() && CurrentLocation.CharacterToLocate.Handle != Game.LocalPlayer.Character.Handle)
            {
                CurrentLocation.CharacterToLocate = Game.LocalPlayer.Character;
            }
            if (HealthState.MyPed.Pedestrian.Exists() && HealthState.MyPed.Pedestrian.Handle != Game.LocalPlayer.Character.Handle)
            {
                HealthState.MyPed = new PedExt(Game.LocalPlayer.Character, Settings, Crimes);
            }
            HealthState.Update();
            IsStunned = Game.LocalPlayer.Character.IsStunned;
            IsRagdoll = Game.LocalPlayer.Character.IsRagdoll;
            IsMovingDynamically = Game.LocalPlayer.Character.IsInCover || Game.LocalPlayer.Character.IsInCombat || Game.LocalPlayer.Character.IsJumping || Game.LocalPlayer.Character.IsRunning;
            if (NativeFunction.CallByName<bool>("GET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, 1) || NativeFunction.CallByName<int>("GET_TIMECYCLE_MODIFIER_INDEX") == 722)
            {
                IsIntoxicated = true;
            }
            else
            {
                IsIntoxicated = false;
            }

            RootPosition = NativeFunction.Natives.GET_WORLD_POSITION_OF_ENTITY_BONE<Vector3>(Game.LocalPlayer.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, 57005));// if you are in a car, your position is the mioddle of the car, hopefully this fixes that

            if(Game.GameTime - GameTimeLastCheckedAmbientCrimes >= 10000)
            {
                //CallInAmbientCrimes();
                GameTimeLastCheckedAmbientCrimes = Game.GameTime;
            }

            //works fine, just turned off for now
            //if (IsNotWanted && !IsInVehicle)//meh only on not wanted for now, well see
            //{
            //    IsNearScenario = NativeFunction.Natives.DOES_SCENARIO_EXIST_IN_AREA<bool>(Position.X, Position.Y, Position.Z, 2f, true) && !NativeFunction.Natives.IS_SCENARIO_OCCUPIED<bool>(Position.X, Position.Y, Position.Z, 2f, true);
            //    ClosestScenario = new Scenario("", "Unknown");
            //    if (IsNearScenario)
            //    {
            //        foreach (Scenario scenario in Scenarios.ScenarioList)
            //        {
            //            if (NativeFunction.Natives.DOES_SCENARIO_OF_TYPE_EXIST_IN_AREA<bool>(Position.X, Position.Y, Position.Z, scenario.InternalName, 2f, true))
            //            {
            //                ClosestScenario = scenario;
            //                break;
            //            }
            //        }

            //    }
            //}
            //else
            //{
            //    IsNearScenario = false;
            //}

        }
        public void UpdateVehicleData()
        {
            IsInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
            IsGettingIntoAVehicle = Game.LocalPlayer.Character.IsGettingIntoVehicle;
            if (IsInVehicle)
            {
                IsDriver = Game.LocalPlayer.Character.SeatIndex == -1;
                IsInAirVehicle = Game.LocalPlayer.Character.IsInAirVehicle;
                IsInAutomobile = !(IsInAirVehicle || Game.LocalPlayer.Character.IsInSeaVehicle || Game.LocalPlayer.Character.IsOnBike || Game.LocalPlayer.Character.IsInHelicopter);
                IsOnMotorcycle = Game.LocalPlayer.Character.IsOnBike;
                UpdateCurrentVehicle();
                IsHotWiring = CurrentVehicle != null && CurrentVehicle.Vehicle.Exists() && CurrentVehicle.Vehicle.MustBeHotwired;
                VehicleSpeed = Game.LocalPlayer.Character.CurrentVehicle.Speed;
                if (isHotwiring != IsHotWiring)
                {
                    if (IsHotWiring)
                    {
                        GameTimeStartedHotwiring = Game.GameTime;
                    }
                    else
                    {
                        EntryPoint.WriteToConsole($"PLAYER EVENT: HotWiring Took {Game.GameTime - GameTimeStartedHotwiring}", 3);
                        GameTimeStartedHotwiring = 0;
                    }
                    EntryPoint.WriteToConsole($"PLAYER EVENT: IsHotWiring Changed to {IsHotWiring}", 3);
                    isHotwiring = IsHotWiring;
                }

                if (CurrentVehicle != null && CurrentVehicle.Vehicle.IsEngineOn && CurrentVehicle.Vehicle.IsPoliceVehicle)
                {
                    if (!IsMobileRadioEnabled)
                    {
                        IsMobileRadioEnabled = true;
                        NativeFunction.CallByName<bool>("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", true);
                    }
                }
                else
                {
                    if (IsMobileRadioEnabled)
                    {
                        IsMobileRadioEnabled = false;
                        NativeFunction.CallByName<bool>("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", false);
                    }
                }
                if (VehicleSpeed >= 0.1f)
                {
                    GameTimeLastMoved = Game.GameTime;
                }
                else
                {
                    GameTimeLastMoved = 0;
                }
                if (VehicleSpeed >= 2.0f)
                {
                    GameTimeLastMovedFast = Game.GameTime;
                }
                else
                {
                    GameTimeLastMovedFast = 0;
                }
                IsStill = VehicleSpeed <= 0.1f;


                if (CurrentVehicle != null && CurrentVehicle.Vehicle.Exists())
                {
                    CurrentVehicleDebugString = $"Health {CurrentVehicle.Vehicle.Health} EngineHealth {CurrentVehicle.Vehicle.EngineHealth} IsStolen {CurrentVehicle.IsStolen} CopsRecogn {CurrentVehicle.CopsRecognizeAsStolen}";
                }
                // CurrentVehicleDebugString = $"LSREngineOn: {CurrentVehicle.Engine.IsRunning} GTAEngineOn: {CurrentVehicle.Vehicle.IsEngineOn}";
            }
            else
            {
                IsDriver = false;
                CurrentVehicleDebugString = "";
                IsOnMotorcycle = false;
                IsInAutomobile = false;
                CurrentVehicle = null;
                // CurrentSpeedDisplay = "";
                float PlayerSpeed = Game.LocalPlayer.Character.Speed;

                foreach (VehicleExt ownedCar in TrackedVehicles.Where(x => x.Vehicle.Exists() && x.Vehicle.Handle == OwnedVehicleHandle))
                {
                    if (ownedCar.Vehicle.DistanceTo2D(Position) >= 1000f && ownedCar.Vehicle.IsPersistent)
                    {
                        ownedCar.Vehicle.IsPersistent = false;
                    }
                    else
                    {
                        ownedCar.Vehicle.IsPersistent = true;
                    }
                }
                if (PlayerSpeed >= 0.1f)
                {
                    GameTimeLastMoved = Game.GameTime;
                }
                else
                {
                    GameTimeLastMoved = 0;
                }
                if (PlayerSpeed >= 7.0f)
                {
                    GameTimeLastMovedFast = Game.GameTime;
                }
                else
                {
                    GameTimeLastMovedFast = 0;
                }
                IsStill = Game.LocalPlayer.Character.IsStill;
                NativeFunction.CallByName<bool>("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", false);
            }
            TrackedVehicles.RemoveAll(x => !x.Vehicle.Exists());
        }
        public void UpdateWeaponData()
        {
            if (Game.LocalPlayer.Character.IsShooting)
            {
                GameTimeLastShot = Game.GameTime;
            }
            IsAiming = Game.LocalPlayer.IsFreeAiming;
            IsAimingInVehicle = IsInVehicle && IsAiming;
            UpdateVisiblyArmed();
            WeaponDescriptor PlayerCurrentWeapon = Game.LocalPlayer.Character.Inventory.EquippedWeapon;
            if (PlayerCurrentWeapon != null)
            {

                CurrentWeaponHash = PlayerCurrentWeapon.Hash;
                CurrentWeapon = Weapons.GetCurrentWeapon(Game.LocalPlayer.Character);
                GameFiber.Yield();
            }
            else
            {

                CurrentWeaponHash = 0;
                CurrentWeapon = null;
            }
            if (Game.LocalPlayer.Character.Inventory.EquippedWeaponObject != null)
            {
                CurrentWeaponIsOneHanded = Game.LocalPlayer.Character.Inventory.EquippedWeaponObject.Model.Dimensions.X <= 0.4f;
            }
            else
            {
                CurrentWeaponIsOneHanded = false;
            }
            if (CurrentWeaponHash != 0 && PlayerCurrentWeapon.Hash != LastWeaponHash)
            {
                LastWeaponHash = PlayerCurrentWeapon.Hash;
            }






            WeaponDropping.Update();
            UpdateTargetedPed();
            GameFiber.Yield();
            UpdateLookedAtPed();
            GameFiber.Yield();
        }
        public void ViolationsUpdate() => Violations.Update();
        private void MakeSober()
        {
            NativeFunction.Natives.SET_PED_IS_DRUNK<bool>(Game.LocalPlayer.Character, false);
            NativeFunction.Natives.RESET_PED_MOVEMENT_CLIPSET<bool>(Game.LocalPlayer.Character);
            NativeFunction.Natives.SET_PED_CONFIG_FLAG<bool>(Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, false);
            if (Settings.SettingsManager.UISettings.AllowScreenEffectReset)//this should be moved methinks
            {
                NativeFunction.Natives.CLEAR_TIMECYCLE_MODIFIER<int>();
                NativeFunction.Natives.x80C8B1846639BB19(0);
                NativeFunction.Natives.STOP_GAMEPLAY_CAM_SHAKING<int>(true);
            }
            //EntryPoint.WriteToConsole("Player Made Sober");
        }
        private void OnAimingChanged()
        {
            if (IsAiming)
            {
            }
            else
            {
            }
            EntryPoint.WriteToConsole($"PLAYER EVENT: IsAiming Changed to: {IsAiming}", 5);
        }
        private void OnAimingInVehicleChanged()
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
            EntryPoint.WriteToConsole($"PLAYER EVENT: IsAimingInVehicle Changed to: {IsAimingInVehicle}", 5);
        }
        private void OnGettingIntoAVehicleChanged()
        {
            if (IsGettingIntoAVehicle)
            {
                Vehicle VehicleTryingToEnter = Game.LocalPlayer.Character.VehicleTryingToEnter;
                int SeatTryingToEnter = Game.LocalPlayer.Character.SeatIndexTryingToEnter;
                if (VehicleTryingToEnter == null)
                {
                    return;
                }
                UpdateCurrentVehicle();
                //GameFiber.Yield();
                if (CurrentVehicle != null)
                {
                    VehicleGettingInto = CurrentVehicle;
                    if (CurrentVehicle.Handle == OwnedVehicleHandle && CurrentVehicle.Vehicle.Exists())
                    {
                        CurrentVehicle.Vehicle.LockStatus = (VehicleLockStatus)1;
                        CurrentVehicle.Vehicle.MustBeHotwired = false;
                    }
                    else
                    {
                        if (!CurrentVehicle.HasBeenEnteredByPlayer)
                        {
                            CurrentVehicle.AttemptToLock();
                            GameFiber.Yield();
                        }
                        if (IsNotHoldingEnter && VehicleTryingToEnter.Driver == null && VehicleTryingToEnter.LockStatus == (VehicleLockStatus)7 && !VehicleTryingToEnter.IsEngineOn)//no driver && Unlocked
                        {
                            EntryPoint.WriteToConsole($"PLAYER EVENT: LockPick Start", 3);
                            CarLockPick MyLockPick = new CarLockPick(this, VehicleTryingToEnter, SeatTryingToEnter);
                            MyLockPick.PickLock();
                        }
                        else if (IsNotHoldingEnter && SeatTryingToEnter == -1 && VehicleTryingToEnter.Driver != null && VehicleTryingToEnter.Driver.IsAlive) //Driver
                        {
                            EntryPoint.WriteToConsole($"PLAYER EVENT: CarJack Start", 3);
                            CarJack MyJack = new CarJack(this, CurrentVehicle, EntityProvider.CivilianList.FirstOrDefault(x => x.Pedestrian.Handle == VehicleTryingToEnter.Driver.Handle), SeatTryingToEnter, CurrentWeapon);
                            MyJack.Start();
                        }
                        else if (VehicleTryingToEnter.LockStatus == (VehicleLockStatus)7)
                        {
                            EntryPoint.WriteToConsole($"PLAYER EVENT: Car Break-In Start", 3);
                            CarBreakIn MyBreakIn = new CarBreakIn(this, VehicleTryingToEnter);
                            MyBreakIn.BreakIn();
                        }
                    }
                }
                else
                {
                    EntryPoint.WriteToConsole($"PLAYER EVENT: IsGettingIntoVehicle ERROR VEHICLE NOT FOUND (ARE YOU SCANNING ENOUGH?)", 3);
                }
            }
            else
            {
            }
            isGettingIntoVehicle = IsGettingIntoAVehicle;
            EntryPoint.WriteToConsole($"PLAYER EVENT: IsGettingIntoVehicleChanged to {IsGettingIntoAVehicle}, HoldingEnter {IsNotHoldingEnter}", 3);
        }
        private void OnIsInVehicleChanged()
        {
            if (IsInVehicle)
            {
                //if (CurrentVehicle != null)
                //{
                //    EntryPoint.WriteToConsole("-------------------------------", 3);
                //    EntryPoint.WriteToConsole($" CurrentVehicle.Vehicle.Handle: {CurrentVehicle.Vehicle.Handle}", 3);
                //    EntryPoint.WriteToConsole($" CurrentVehicle.IsStolen: {CurrentVehicle.IsStolen}", 3);
                //    EntryPoint.WriteToConsole($" CurrentVehicle.WasReportedStolen: {CurrentVehicle.WasReportedStolen}", 3);
                //    EntryPoint.WriteToConsole($" CurrentVehicle.CopsRecognizeAsStolen: {CurrentVehicle.CopsRecognizeAsStolen}", 3);
                //    EntryPoint.WriteToConsole($" CurrentVehicle.NeedsToBeReportedStolen: {CurrentVehicle.NeedsToBeReportedStolen}", 3);
                //    EntryPoint.WriteToConsole($" CurrentVehicle.GameTimeToReportStolen: {CurrentVehicle.GameTimeToReportStolen}", 3);
                //    EntryPoint.WriteToConsole($" CurrentVehicle.CarPlate.IsWanted: {CurrentVehicle.CarPlate.IsWanted}", 3);
                //    EntryPoint.WriteToConsole($" CurrentVehicle.CarPlate.PlateNumber: {CurrentVehicle.CarPlate.PlateNumber}", 3);
                //    EntryPoint.WriteToConsole($" CurrentVehicle.OriginalLicensePlate.IsWanted: {CurrentVehicle.OriginalLicensePlate.IsWanted}", 3);
                //    EntryPoint.WriteToConsole($" CurrentVehicle.OriginalLicensePlate.PlateNumber: {CurrentVehicle.OriginalLicensePlate.PlateNumber}", 3);
                //    EntryPoint.WriteToConsole($" CurrentVehicle.HasOriginalPlate: {CurrentVehicle.HasOriginalPlate}", 3);
                //    EntryPoint.WriteToConsole($" CurrentVehicle.HasBeenDescribedByDispatch: {CurrentVehicle.HasBeenDescribedByDispatch}", 3);
                //    EntryPoint.WriteToConsole("-------------------------------", 3);
                //}
            }
            else
            {
                if (IsWanted && AnyPoliceCanSeePlayer)
                {
                    Scanner.OnGotOutOfVehicle();
                }
            }
            EntryPoint.WriteToConsole($"PLAYER EVENT: IsInVehicle to {IsInVehicle}", 3);
        }
        private void OnPlayerBusted()
        {
            DiedInVehicle = IsInVehicle;
            IsBusted = true;
            BeingArrested = true;
            GameTimeLastBusted = Game.GameTime;
            HandsAreUp = false;
            if (WantedLevel > 1)
            {
                Surrendering.SetArrestedAnimation(Game.LocalPlayer.Character, false, WantedLevel <= 2);//needs to move
            }
            Game.LocalPlayer.HasControl = false;

            Scanner.OnPlayerBusted();
            EntryPoint.WriteToConsole($"PLAYER EVENT: IsBusted Changed to: {IsBusted}", 3);
        }
        private void OnPlayerDied()
        {
            TimeControllable.PauseTime();
            DiedInVehicle = IsInVehicle;
            IsDead = true;
            GameTimeLastDied = Game.GameTime;
            Game.LocalPlayer.Character.Kill();
            Game.LocalPlayer.Character.Health = 0;
            Game.LocalPlayer.Character.IsInvincible = true;
            Game.TimeScale = 0.4f;
            Scanner.OnSuspectWasted();
            EntryPoint.WriteToConsole($"PLAYER EVENT: IsDead Changed to: {IsDead}", 3);
        }
        private void OnTargettingHandleChanged()
        {
            if (TargettingHandle != 0)
            {
                CurrentTargetedPed = EntityProvider.GetPedExt(TargettingHandle);
                if (!IsInteracting && CanHoldUpTargettedPed && CurrentTargetedPed != null && CurrentTargetedPed.CanBeMugged)
                {
                    StartHoldUp();
                }
            }
            else
            {
                CurrentTargetedPed = null;
            }
            EntryPoint.WriteToConsole($"PLAYER EVENT: CurrentTargetedPed to {CurrentTargetedPed?.Pedestrian?.Handle}", 5);
        }
        private void OnWantedLevelChanged()//runs after OnSuspectEluded (If Applicable)
        {
            if (IsNotWanted && PreviousWantedLevel != 0)//Lost Wanted
            {
                CriminalHistory.OnLostWanted();
                PoliceResponse.OnLostWanted();
                EntityProvider.CivilianList.ForEach(x => x.CrimesWitnessed.Clear());
                EntryPoint.WriteToConsole($"PLAYER EVENT: LOST WANTED", 3);

                //NativeFunction.Natives.SET_POLICE_IGNORE_PLAYER(Game.LocalPlayer, true);
                //NativeFunction.Natives.SET_IGNORE_LOW_PRIORITY_SHOCKING_EVENTS(Game.LocalPlayer, true);



            }
            else if (IsWanted && PreviousWantedLevel == 0)//Added Wanted Level
            {
                if (!RecentlySetWanted)//only allow my process to set the wanted level
                {
                    if (Settings.SettingsManager.PoliceSettings.AllowExclusiveControlOverWantedLevel)
                    {
                        EntryPoint.WriteToConsole($"PLAYER EVENT: GAME AUTO SET WANTED TO {WantedLevel}, RESETTING", 3);
                        SetWantedLevel(0, "GAME AUTO SET WANTED", true);
                    }
                    //Game.LocalPlayer.WantedLevel = 0;
                }
                else
                {
                    Investigation.Reset();
                    PoliceResponse.OnBecameWanted();
                    //NativeFunction.Natives.SET_POLICE_IGNORE_PLAYER(Game.LocalPlayer, false);
                    //NativeFunction.Natives.SET_IGNORE_LOW_PRIORITY_SHOCKING_EVENTS(Game.LocalPlayer, false);



                    EntryPoint.WriteToConsole($"PLAYER EVENT: BECAME WANTED", 3);
                }
            }
            else if (IsWanted && PreviousWantedLevel < WantedLevel)//Increased Wanted Level (can't decrease only remove for now.......)
            {
                PoliceResponse.OnWantedLevelIncreased();
                EntryPoint.WriteToConsole($"PLAYER EVENT: WANTED LEVEL INCREASED", 3);
            }
            else if (IsWanted && PreviousWantedLevel > WantedLevel)
            {
                //PoliceResponse.OnWantedLevelDecreased();
                EntryPoint.WriteToConsole($"PLAYER EVENT: WANTED LEVEL DECREASED", 3);
            }
            EntryPoint.WriteToConsole($"Wanted Changed: {WantedLevel} Previous: {PreviousWantedLevel}", 3);
            PreviousWantedLevel = Game.LocalPlayer.WantedLevel;
        }
        private void UpdateButtonPrompts()
        {
            if (!IsInteracting && CanConverseWithLookedAtPed)
            {
                if (!ButtonPrompts.Any(x => x.Identifier == $"Talk {CurrentLookedAtPed.Pedestrian.Handle}"))
                {
                    ButtonPrompts.RemoveAll(x => x.Group == "StartConversation");
                    ButtonPrompts.Add(new ButtonPrompt($"Talk to {CurrentLookedAtPed.FormattedName}", "StartConversation", $"Talk {CurrentLookedAtPed.Pedestrian.Handle}", Settings.SettingsManager.KeySettings.InteractStart, 1));
                }
            }
            else
            {
                ButtonPrompts.RemoveAll(x => x.Group == "StartConversation");
            }
            if (CanPerformActivities && IsNearScenario)//currently isnearscenario is turned off
            {
                if (!ButtonPrompts.Any(x => x.Identifier == $"StartScenario"))
                {
                    ButtonPrompts.RemoveAll(x => x.Group == "StartScenario");
                    ButtonPrompts.Add(new ButtonPrompt($"{ClosestScenario?.Name}", "StartScenario", $"StartScenario", Settings.SettingsManager.KeySettings.ScenarioStart, 2));
                }
            }
            else
            {
                ButtonPrompts.RemoveAll(x => x.Group == "StartScenario");
            }



        }
        private void UpdateData()
        {
            UpdateVehicleData();
            GameFiber.Yield();
            UpdateWeaponData();
            GameFiber.Yield();
            UpdateStateData();
            GameFiber.Yield();
        }
        private void UpdateLookedAtPed()
        {
            if (Game.GameTime - GameTimeLastUpdatedLookedAtPed >= 750)//750
            {
                GameFiber.Yield();

                //Works fine just going simpler
                Vector3 RayStart = Game.LocalPlayer.Character.GetBonePosition(PedBoneId.Head);
                // Vector3 RayStart = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
                Vector3 RayEnd = RayStart + NativeHelper.GetGameplayCameraDirection() * 6.0f;
                //Vector3 RayStart = Game.LocalPlayer.Character.GetBonePosition(PedBoneId.Head);
                //Vector3 RayEnd = RayStart + Game.LocalPlayer.Character.Direction * 5.0f;
                HitResult result = Rage.World.TraceCapsule(RayStart, RayEnd, 1f, TraceFlags.IntersectVehicles | TraceFlags.IntersectPedsSimpleCollision, Game.LocalPlayer.Character);//2 meter wide cylinder out 10 meters that ignores the player charater going from the head in the players direction
                                                                                                                                                                                     //  Rage.Debug.DrawArrowDebug(RayStart, Game.LocalPlayer.Character.Direction, Rotator.Zero, 1f, Color.White);
                                                                                                                                                                                     //  Rage.Debug.DrawArrowDebug(RayEnd, Game.LocalPlayer.Character.Direction, Rotator.Zero, 1f, Color.Red);
                if (result.Hit && result.HitEntity is Ped)
                {
                    // Rage.Debug.DrawArrowDebug(result.HitPosition, Game.LocalPlayer.Character.Direction, Rotator.Zero, 1f, Color.Green);
                    CurrentLookedAtPed = EntityProvider.GetPedExt(result.HitEntity.Handle);
                }
                else
                {
                    CurrentLookedAtPed = null;
                }

                //CurrentLookedAtPed = EntityProvider.CivilianList.Where(x => x.DistanceToPlayer <= 4f && !x.IsBehindPlayer).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
                GameTimeLastUpdatedLookedAtPed = Game.GameTime;


                GameFiber.Yield();
            }
        }
        private void UpdateTargetedPed()
        {
            if (IsAiming)
            {
                Entity AimingAt = Game.LocalPlayer.GetFreeAimingTarget();
                if (AimingAt.Exists())
                {
                    TargettingHandle = AimingAt.Handle;
                }
                else
                {
                    TargettingHandle = 0;
                }
            }
            else
            {
                TargettingHandle = NativeHelper.GetTargettingHandle();
            }
        }
        private void UpdateVisiblyArmed()
        {
            if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && !Game.LocalPlayer.IsFreeAiming)
            {
                IsVisiblyArmed = false;
            }
            else if (Game.LocalPlayer.Character.Inventory.EquippedWeapon == null)
            {
                IsVisiblyArmed = false;
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
                IsVisiblyArmed = false;
            }
            else if (!NativeFunction.Natives.IS_PLAYER_CONTROL_ON<bool>(Game.LocalPlayer))
            {
                IsVisiblyArmed = false;
            }
            else
            {
                IsVisiblyArmed = true;
            }

        }
    }
}