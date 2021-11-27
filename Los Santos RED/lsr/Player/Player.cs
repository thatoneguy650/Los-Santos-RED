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
    public class Player : IDispatchable, IActivityPerformable, IIntoxicatable, ITargetable, IPoliceRespondable, IInputable, IPedSwappable, IMuggable, IRespawnable, IViolateable, IWeaponDroppable, IDisplayable, ICarStealable, IPlateChangeable, IActionable, IInteractionable, IInventoryable, IRespawning, ISaveable, IPerceptable
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
        private GameLocation ClosestSimpleTransaction;
        
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
        private VehicleExt VehicleTryingToEnter;
        private int SeatTryingToEnter;
        private Vehicle VehicleTaskedToEnter;
        private int SeatTaskedToEnter;
        private IPlacesOfInterest PlacesOfInterest;
        private bool isSetPoliceIgnored = false;
        private uint GameTimeLastFedUpCop;
        private bool isJacking = false;
        private IModItems ModItems;
        public Player(string modelName, bool isMale, string suspectsName, IEntityProvideable provider, ITimeControllable timeControllable, IStreets streets, IZones zones, ISettingsProvideable settings, IWeapons weapons, IRadioStations radioStations, IScenarios scenarios, ICrimes crimes, IAudioPlayable audio, IPlacesOfInterest placesOfInterest, IInteriors interiors, IModItems modItems)
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
            PlacesOfInterest = placesOfInterest;
            ModItems = modItems;
            Scanner = new Scanner(provider, this, audio, Settings, TimeControllable);
            HealthState = new HealthState(new PedExt(Game.LocalPlayer.Character, Settings, Crimes, Weapons,PlayerName), Settings);
            CurrentLocation = new LocationData(Game.LocalPlayer.Character, streets, zones, interiors);
            WeaponDropping = new WeaponDropping(this, Weapons, Settings);
            Surrendering = new SurrenderActivity(this, EntityProvider);
            Violations = new Violations(this, TimeControllable, Crimes, Settings);
            Violations.Setup();
            Investigation = new Investigation(this, Settings, provider);
            CriminalHistory = new CriminalHistory(this, Settings);
            PoliceResponse = new PoliceResponse(this, Settings);
            SearchMode = new SearchMode(this, Settings);
            Inventory = new Inventory(this);    
            Respawning = new Respawning(TimeControllable, EntityProvider, this, Weapons, PlacesOfInterest, Settings);
        }
        public float ActiveDistance => Investigation.IsActive ? Investigation.Distance : 500f + (WantedLevel * 200f);
        public bool AnyHumansNear => EntityProvider.PoliceList.Any(x => x.DistanceToPlayer <= 10f) || EntityProvider.CivilianList.Any(x => x.DistanceToPlayer <= 10f); //move or delete?
        public bool AnyPoliceCanHearPlayer { get; set; } //all this perception stuff gets moved out?
        public bool AnyPoliceCanRecognizePlayer { get; set; }
        public bool AnyPoliceCanSeePlayer { get; set; }
        public bool AnyPoliceRecentlySeenPlayer { get; set; }
        public bool BeingArrested { get; private set; }
        public bool IsCop { get; set; } = false;
        public List<ButtonPrompt> ButtonPrompts { get; private set; } = new List<ButtonPrompt>();
        public bool CanConverse => !IsGettingIntoAVehicle && !IsBreakingIntoCar && !IsIncapacitated && !IsVisiblyArmed && IsAliveAndFree && !IsMovingDynamically;
        public bool CanConverseWithLookedAtPed => CurrentLookedAtPed != null && CurrentTargetedPed == null && CurrentLookedAtPed.CanConverse && CanConverse; // && (Relationship)NativeFunction.Natives.GET_RELATIONSHIP_BETWEEN_PEDS<int>(CurrentLookedAtPed.Pedestrian, Character) != Relationship.Hate;//off for performance checking
        public bool CanDropWeapon => CanPerformActivities && WeaponDropping.CanDropWeapon;
        public bool CanHoldUpTargettedPed => CurrentTargetedPed != null && !IsCop && CurrentTargetedPed.CanBeMugged && !IsGettingIntoAVehicle && !IsBreakingIntoCar && !IsStunned && !IsRagdoll && IsVisiblyArmed && IsAliveAndFree && CurrentTargetedPed.DistanceToPlayer <= 7f;
        public bool CanPerformActivities => !IsMovingFast && !IsIncapacitated && !IsDead && !IsBusted && !IsGettingIntoAVehicle && !IsMovingDynamically;// && !IsInVehicle;//THIS IS TURNED OFF TO SEE HOW THE ANIMATIONS LOOK, PROBABLYT WONT WORK!
        public bool CanSurrender => Surrendering.CanSurrender;
        public bool CanUndie => Respawning.CanUndie;
        public Ped Character => Game.LocalPlayer.Character;
        public int GroupID { get; set; }
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
        public ComplexTask CurrentTask { get; set; }
        public Inventory Inventory { get; set; }
        public List<InventoryItem> InventoryItems => Inventory.Items;
        public List<InventoryItem> ConsumableItems => Inventory.Items.Where(x=> x.ModItem?.Type != eConsumableType.None).ToList();
        public List<Crime> CivilianReportableCrimesViolating => Violations.CivilianReportableCrimesViolating;
        public string DebugLine1 => $"Player: {ModelName},{Game.LocalPlayer.Character.Handle} RcntStrPly: {RecentlyStartedPlaying} IsMovingDynam: {IsMovingDynamically} IsIntoxicated: {IsIntoxicated}";
        public string DebugLine2 => $"Vio: {Violations.LawsViolatingDisplay}";
        public string DebugLine3 => $"Rep: {PoliceResponse.ReportedCrimesDisplay}";
        public string DebugLine4 => $"Obs: {PoliceResponse.ObservedCrimesDisplay}";
        public string DebugLine5 => CurrentVehicleDebugString;
        public string DebugLine6 => $" IsJacking {Game.LocalPlayer.Character.IsJacking} isJacking {isJacking} BreakingIntoCar {IsBreakingIntoCar} IsCarJacking {IsCarJacking} IsLockPicking {IsLockPicking} IsHotWiring {IsHotWiring}";//SearchMode.SearchModeDebug;//$" Street {CurrentLocation?.CurrentStreet?.Name} - {CurrentLocation?.CurrentCrossStreet?.Name} IsJacking {Game.LocalPlayer.Character.IsJacking} isJacking {isJacking} BreakingIntoCar {IsBreakingIntoCar}";//SearchMode.SearchModeDebug;
        public string DebugLine7 => $"AnyPolice: CanSee: {AnyPoliceCanSeePlayer}, RecentlySeen: {AnyPoliceRecentlySeenPlayer}, CanHear: {AnyPoliceCanHearPlayer}, CanRecognize {AnyPoliceCanRecognizePlayer}";
        public string DebugLine8 => $"AliasedCop : {AliasedCop != null} AliasedCopCanBeAmbientTasked: {AliasedCop?.CanBeAmbientTasked} LastSeenPlayer {PlacePoliceLastSeenPlayer} HaveDesc: {PoliceResponse.PoliceHaveDescription} LastRptCrime {PoliceResponse.PlaceLastReportedCrime} IsSuspicious: {Investigation.IsSuspicious}";
        public string DebugLine9 => CurrentVehicle != null ? $"IsEngineRunning: {CurrentVehicle.Engine.IsRunning} {CurrentVehicle.Vehicle.Handle}" : $"NO VEHICLE" + $" IsGettingIntoAVehicle: {IsGettingIntoAVehicle}, IsInVehicle: {IsInVehicle} OwnedVehicleHandle {OwnedVehicleHandle}";
        public string DebugLine10 => $"Cop#: {EntityProvider.PoliceList.Count()} CopCar#: {EntityProvider.PoliceVehicleCount} Civ#: {EntityProvider.CivilianList.Count()} CivCar:#: {EntityProvider.CivilianVehicleCount} Tracked#: {TrackedVehicles.Count}";
        public string DebugLine11 { get; set; }
        public string LawsViolating => Violations.LawsViolatingDisplay;
        public Cop AliasedCop { get; set; }
        public Scanner DebugScanner => Scanner;
        //move or delete?
        //should be private but needed?
        public bool DiedInVehicle { get; private set; }
        public bool HandsAreUp { get; set; }
        public uint HasBeenWantedFor => PoliceResponse.HasBeenWantedFor;
        public void AddToInventory(ModItem toadd, int v) => Inventory.Add(toadd, v);
        public bool HasCriminalHistory => CriminalHistory.HasHistory;
        public bool HasDeadlyCriminalHistory => CriminalHistory.HasDeadlyHistory;
        public int CriminalHistoryMaxWantedLevel => CriminalHistory.MaxWantedLevel;
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
        public bool IsBreakingIntoCar => IsCarJacking || IsLockPicking || IsHotWiring || isJacking;//(Game.LocalPlayer.Character.IsJacking && (!Game.LocalPlayer.Character.VehicleTryingToEnter.Exists() || Game.LocalPlayer.Character.VehicleTryingToEnter.Handle != OwnedVehicleHandle));
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
        public bool RecentlyFedUpCop => GameTimeLastFedUpCop != 0 && Game.GameTime - GameTimeLastFedUpCop <= 5000;
        public List<VehicleExt> ReportedStolenVehicles => TrackedVehicles.Where(x => x.NeedsToBeReportedStolen && !x.HasBeenDescribedByDispatch && !x.AddedToReportedStolenQueue).ToList();
        public Vector3 RootPosition { get; set; }
        public bool ShouldCheckViolations => !Settings.SettingsManager.PlayerSettings.Violations_TreatAsCop && !IsCop;
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
                    if(NativeFunction.Natives.GET_PED_CONFIG_FLAG<bool>(Character,359,true))//isduckinginvehicle?
                    {
                        Time += 5000;
                    }
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
        public void AddCrime(Crime crimeObserved, bool isObservedByPolice, Vector3 Location, VehicleExt VehicleObserved, WeaponInformation WeaponObserved, bool HaveDescription, bool AnnounceCrime, bool isForPlayer)
        {
            CrimeSceneDescription description = new CrimeSceneDescription(!IsInVehicle, isObservedByPolice, Location, HaveDescription) { VehicleSeen = VehicleObserved, WeaponSeen = WeaponObserved, Speed = Game.LocalPlayer.Character.Speed };
            PoliceResponse.AddCrime(crimeObserved, description, isForPlayer);
            if (AnnounceCrime)
            {
                Scanner.AnnounceCrime(crimeObserved, description);
            }
            if (!isObservedByPolice && IsNotWanted)
            {
                Investigation.Start(Location, PoliceResponse.PoliceHaveDescription);       
            }
        }
        public void AnnounceCrime(Crime crimeObserved, bool isObservedByPolice, Vector3 Location, VehicleExt VehicleObserved, WeaponInformation WeaponObserved)
        {
            CrimeSceneDescription description = new CrimeSceneDescription(false, isObservedByPolice, Location, false) { VehicleSeen = VehicleObserved, WeaponSeen = WeaponObserved };
            Scanner.AnnounceCrime(crimeObserved, description);
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
            Crime ToCallIn = Crimes.CrimeList.FirstOrDefault(x => x.ID == "OfficersNeeded");
            PedExt violatingCiv = EntityProvider.CivilianList.Where(x => x.DistanceToPlayer <= 200f).OrderByDescending(x => x.CurrentlyViolatingWantedLevel).FirstOrDefault();
            CrimeSceneDescription description;
            if (violatingCiv != null && violatingCiv.Pedestrian.Exists() && violatingCiv.CrimesCurrentlyViolating.Any())
            {
                description = new CrimeSceneDescription(!violatingCiv.IsInVehicle, IsCop, violatingCiv.Pedestrian.Position, false) { VehicleSeen = null, WeaponSeen = null };
                ToCallIn = violatingCiv.CrimesCurrentlyViolating.OrderBy(x => x.Priority).FirstOrDefault();
            }
            else
            {
                description = new CrimeSceneDescription(false, IsCop, Position);
            }

            if (IsCop)
            {
                Scanner.Reset();
                Scanner.AnnounceCrime(ToCallIn, description);
                Investigation.Start(Position, false);
            }
            else
            {
                AddCrime(ToCallIn, false, description.PlaceSeen, description.VehicleSeen, description.WeaponSeen, false, true, false);
            }

            //if (IsCop)
            //{
            //    CrimeSceneDescription description = new CrimeSceneDescription(!IsInVehicle, isObservedByPolice, Location, HaveDescription) { VehicleSeen = VehicleObserved, WeaponSeen = WeaponObserved, Speed = Game.LocalPlayer.Character.Speed };
            //    Scanner.AnnounceCrime(crimeObserved, description);
               
            //        Investigation.Start(Position, false);
                
            //}
            //else
            //{
            //    PedExt violatingCiv = EntityProvider.CivilianList.Where(x => x.DistanceToPlayer <= 200f).OrderByDescending(x => x.CurrentlyViolatingWantedLevel).FirstOrDefault();
            //    if (violatingCiv != null && violatingCiv.Pedestrian.Exists() && violatingCiv.CrimesCurrentlyViolating.Any())
            //    {
            //        Crime ToCallIn = violatingCiv.CrimesCurrentlyViolating.OrderBy(x => x.Priority).FirstOrDefault();
            //        if (ToCallIn != null)
            //        {
            //            AddCrime(ToCallIn, IsCop, Position, null, null, false, true, false);
            //        }
            //        else
            //        {
            //            AddCrime(Crimes.CrimeList.FirstOrDefault(x => x.ID == "OfficersNeeded"), IsCop, Position, null, null, false, true, false);
            //        }
            //    }
            //    else
            //    {
            //        AddCrime(Crimes.CrimeList.FirstOrDefault(x => x.ID == "OfficersNeeded"), IsCop, Position, null, null, false, true, false);
            //    }
            //}
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
                DynamicActivity = new PlateTheft(this, SpareLicensePlates[Index], Settings, EntityProvider);
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
                DynamicActivity = new PlateTheft(this, toChange, Settings, EntityProvider);
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
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~b~Personal Info", $"~y~{PlayerName}", NotifcationText);
            DisplayPlayerVehicleNotification();
        }
        public void DisplayPlayerVehicleNotification()
        {
            string NotifcationText = "";
            VehicleExt VehicleToDescribe = null;
            bool usingOwned = true;
            VehicleExt OwnedVehicle = TrackedVehicles.FirstOrDefault(x => x.Vehicle.Exists() && x.Vehicle.Handle == OwnedVehicleHandle);
            if (IsInVehicle)
            {
                if(OwnedVehicle != null && CurrentVehicle != null && OwnedVehicle.Handle == CurrentVehicle.Handle)
                {
                    VehicleToDescribe = OwnedVehicle;
                }
                else
                {
                    VehicleToDescribe = CurrentVehicle;
                    usingOwned = false;
                }
            }
            else
            {
                if (OwnedVehicle != null && OwnedVehicle.Vehicle.Exists())
                {
                    VehicleToDescribe = OwnedVehicle;
                }
            }

                
            if (VehicleToDescribe != null)
            {
                string Make = VehicleToDescribe.MakeName();
                string Model = VehicleToDescribe.ModelName();
                string VehicleName = "";
                if (Make != "")
                {
                    VehicleName = Make;
                }
                if (Model != "")
                {
                    VehicleName += " " + Model;
                }

                string VehicleNameColor = "~p~";
                string VehicleString = "";
                if(usingOwned)
                {
                    NotifcationText += $"Vehicle: ~p~{VehicleName}~n~~s~Status: ~p~Owned~s~";
                }
                else if (!VehicleToDescribe.IsStolen)
                {
                    NotifcationText += $"Vehicle: ~p~{VehicleName}~n~~s~Status: ~p~Unknown~s~";
                }
                else
                {
                    NotifcationText += $"Vehicle: ~r~{VehicleName}~n~~s~Status: ~r~Stolen~s~";
                }
                if (VehicleToDescribe.CarPlate != null && VehicleToDescribe.CarPlate.IsWanted)
                {
                    NotifcationText += $"~n~Plate: ~r~{VehicleToDescribe.CarPlate.PlateNumber} ~r~(Wanted)~s~";
                }
                else
                {
                    NotifcationText += $"~n~Plate: ~p~{VehicleToDescribe.CarPlate.PlateNumber} ~s~";
                }
            }
            
            if (NotifcationText != "")
            {
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~g~Vehicle Info", $"~y~{PlayerName}", NotifcationText);
            }
            else
            {
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~g~Vehicle Info", $"~y~{PlayerName}", "~s~Vehicle: None");
            }
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
            NativeFunction.Natives.SET_PED_AS_COP(Game.LocalPlayer.Character, false);
            if (Settings.SettingsManager.PlayerSettings.SetSlowMoOnDeath)
            {
                Game.TimeScale = 1f;
            }
        }
        public void RemoveFromInventory(ModItem modItem, int amount) => Inventory.Remove(modItem, amount);
        public void StartConsumingActivity(ModItem modItem)
        {
            if (!IsPerformingActivity && CanPerformActivities && modItem.Type != eConsumableType.None)
            {
                if (DynamicActivity != null)
                {
                    DynamicActivity.Cancel();
                }
                IsPerformingActivity = true;
                if(modItem.Type == eConsumableType.Drink)
                {
                    DynamicActivity = new DrinkingActivity(this, Settings, modItem);
                }
                else if (modItem.Type == eConsumableType.Eat)
                {
                    DynamicActivity = new EatingActivity(this, Settings, modItem);
                }
                else if (modItem.Type == eConsumableType.Smoke)
                {
                    DynamicActivity = new SmokingActivity(this, Settings, modItem);
                }
                DynamicActivity?.Start();
            }
        }
        public void StartServiceActivity(ModItem modItem, GameLocation location)
        {
            if(location.Type == LocationType.Hotel)
            {
                IsPerformingActivity = true;
                TimeControllable.FastForward(new DateTime(TimeControllable.CurrentYear,TimeControllable.CurrentMonth,TimeControllable.CurrentDay + 1,11,0,0));
                GameFiber FastForwardWatcher = GameFiber.StartNew(delegate
                {
                    while (TimeControllable.IsFastForwarding)
                    {
                        if(Game.LocalPlayer.Character.Health < Game.LocalPlayer.Character.MaxHealth -1)
                        {
                            Game.LocalPlayer.Character.Health++;
                        }
                        GameFiber.Yield();
                    }
                    IsPerformingActivity = false;
                }, "FastForwardWatcher");
                EntryPoint.WriteToConsole($"PLAYER EVENT: StartServiceActivity HOTEL", 3);
            }
        }
        public void StartDrinkingActivity()
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
            EntryPoint.WriteToConsole($"PLAYER EVENT: OnInvestigationExpire", 3);
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
                DynamicActivity = new PlateTheft(this, Settings, EntityProvider);
                DynamicActivity.Start();
            }
        }
        public void Reset(bool resetWanted, bool resetTimesDied, bool clearWeapons, bool clearCriminalHistory, bool clearInventory)
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
            if(clearInventory)
            {
                Inventory.Clear();
            }
        }
        public void ResetScanner() => Scanner.Reset();
        public void ResistArrest() => Respawning.ResistArrest();
        public void RespawnAtCurrentLocation(bool withInvicibility, bool resetWanted, bool clearCriminalHistory, bool clearInventory) => Respawning.RespawnAtCurrentLocation(withInvicibility, resetWanted, clearCriminalHistory, clearInventory);
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
        public void SetAngeredCop()
        {
            GameTimeLastFedUpCop = Game.GameTime;
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
                Game.LocalPlayer.Character.CurrentVehicle.IsStolen = false;
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
        }
        public void SetWantedLevel(int desiredWantedLevel, string Reason, bool UpdateRecent)
        {
            if (desiredWantedLevel <= Settings.SettingsManager.PoliceSettings.MaxWantedLevel)
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
                Interaction = new Conversation(this, CurrentLookedAtPed, Settings, Crimes);
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
        public void StartTransaction()
        {
            if (!IsInteracting && CanConverseWithLookedAtPed)
            {
                if (Interaction != null)
                {
                    Interaction.Dispose();
                }
                IsConversing = true;
                Merchant myPed = (Merchant)CurrentLookedAtPed;
                Interaction = new Transaction(this, myPed, myPed.Store, Settings, ModItems);
                Interaction.Start();
            }
        }
        public void StartSimpleTransaction()
        {
            if (!IsInteracting)
            {
                if (Interaction != null)
                {
                    Interaction.Dispose();
                }
                IsConversing = true;
                Interaction = new SimpleTransaction(this, ClosestSimpleTransaction, Settings, ModItems, TimeControllable);
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
            VehicleExt toTakeOwnershipOf = null;
            if (CurrentVehicle != null && CurrentVehicle.Vehicle.Exists())
            {
                toTakeOwnershipOf = CurrentVehicle;
            }
            else
            {
                toTakeOwnershipOf = EntityProvider.GetClosestVehicleExt(Character.Position, false, 10f);
            }
            if(toTakeOwnershipOf != null && toTakeOwnershipOf.Vehicle.Exists())
            {
                if (!TrackedVehicles.Any(x => x.Vehicle.Handle == toTakeOwnershipOf.Vehicle.Handle))
                {
                    TrackedVehicles.Add(toTakeOwnershipOf);
                }
                toTakeOwnershipOf.SetNotWanted();
                toTakeOwnershipOf.Vehicle.IsStolen = false;
                OwnedVehicleHandle = toTakeOwnershipOf.Vehicle.Handle;
                DisplayPlayerNotification();
            }
            else
            {
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~b~Personal Info", string.Format("~y~{0}", PlayerName), "No Vehicle Found");
            }
        }
        public void TrafficViolationsUpdate() => Violations.UpdateTraffic();
        public void UnSetArrestedAnimation() => Surrendering.UnSetArrestedAnimation();
        public void SetArrestedAnimation(bool stayStanding) => Surrendering.SetArrestedAnimation(stayStanding);
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
                EntityProvider.AddEntity(createdVehicleExt, ResponseType.None);
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
            existingVehicleExt.Update();
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
                //Game.LocalPlayer.Character.Tasks.Clear();
                NativeFunction.Natives.CLEAR_PED_TASKS(Game.LocalPlayer.Character);
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
                HealthState.MyPed = new PedExt(Game.LocalPlayer.Character, Settings, Crimes, Weapons, PlayerName);
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



































            //COULD BE VERY PROBLEMATIC!!!!!!!!!!



  
            if (PoliceResponse.IsDeadlyChase && !IsBusted)
            {
                if (isSetPoliceIgnored)
                {
                    NativeFunction.Natives.SET_POLICE_IGNORE_PLAYER(Game.LocalPlayer, false);
                    isSetPoliceIgnored = false;
                }

            }
            else
            {
                if(EntityProvider.PoliceList.Any(x=> x.CurrentTask?.OtherTarget != null))
                {
                    if (!isSetPoliceIgnored)
                    {
                        NativeFunction.Natives.SET_POLICE_IGNORE_PLAYER(Game.LocalPlayer, true);
                        isSetPoliceIgnored = true;
                    }
                }
                else
                {
                    if (isSetPoliceIgnored)
                    {
                        NativeFunction.Natives.SET_POLICE_IGNORE_PLAYER(Game.LocalPlayer, false);
                        isSetPoliceIgnored = false;
                    }
                }

            }







































            ClosestSimpleTransaction = null;
            if (!IsMovingFast && IsAliveAndFree && !IsConversing)
            {
                foreach (GameLocation gl in PlacesOfInterest.GetAllStores())
                {
                    if (!gl.HasVendor && gl.CanPurchase && Character.DistanceTo2D(gl.EntrancePosition) <= 3f)
                    {
                        ClosestSimpleTransaction = gl;
                        break;
                    }
                }
            }








            if (Settings.SettingsManager.PlayerSettings.AllowStartRandomScenario && IsNotWanted && !IsInVehicle)//works fine, just turned off by default, needs some work
            {
                IsNearScenario = NativeFunction.Natives.DOES_SCENARIO_EXIST_IN_AREA<bool>(Position.X, Position.Y, Position.Z, 2f, true) && !NativeFunction.Natives.IS_SCENARIO_OCCUPIED<bool>(Position.X, Position.Y, Position.Z, 2f, true);
                ClosestScenario = new Scenario("", "Unknown");
                if (IsNearScenario)
                {
                    foreach (Scenario scenario in Scenarios.ScenarioList)
                    {
                        if (NativeFunction.Natives.DOES_SCENARIO_OF_TYPE_EXIST_IN_AREA<bool>(Position.X, Position.Y, Position.Z, scenario.InternalName, 2f, true))
                        {
                            ClosestScenario = scenario;
                            break;
                        }
                    }
                }
            }
            else
            {
                IsNearScenario = false;
            }
        }
        public void UpdateVehicleData()
        {
            IsInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
            IsGettingIntoAVehicle = Game.LocalPlayer.Character.IsGettingIntoVehicle;
            if (IsInVehicle)
            {

                if (Character.CurrentVehicle.Exists() && Character.CurrentVehicle.Handle == OwnedVehicleHandle)
                {
                    isJacking = false;
                }
                else
                {
                    isJacking = Character.IsJacking;
                }

                IsDriver = Game.LocalPlayer.Character.SeatIndex == -1;
                IsInAirVehicle = Game.LocalPlayer.Character.IsInAirVehicle;
                IsInAutomobile = !(IsInAirVehicle || Game.LocalPlayer.Character.IsInSeaVehicle || Game.LocalPlayer.Character.IsOnBike || Game.LocalPlayer.Character.IsInHelicopter);
                IsOnMotorcycle = Game.LocalPlayer.Character.IsOnBike;
                UpdateCurrentVehicle();
                IsHotWiring = CurrentVehicle != null && CurrentVehicle.Vehicle.Exists() && CurrentVehicle.IsStolen && CurrentVehicle.Vehicle.MustBeHotwired;


                VehicleSpeed = Game.LocalPlayer.Character.CurrentVehicle.Speed;
                if (isHotwiring != IsHotWiring)
                {
                    if (IsHotWiring)
                    {
                        GameTimeStartedHotwiring = Game.GameTime;
                    }
                    else
                    {
                        //EntryPoint.WriteToConsole($"PLAYER EVENT: HotWiring Took {Game.GameTime - GameTimeStartedHotwiring}", 3);
                        GameTimeStartedHotwiring = 0;
                    }
                    //EntryPoint.WriteToConsole($"PLAYER EVENT: IsHotWiring Changed to {IsHotWiring}", 3);
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


                //if(IsGettingIntoAVehicle && Character.VehicleTryingToEnter.Exists() && Character.VehicleTryingToEnter.Handle == OwnedVehicleHandle)
                //{
                //    isJacking = false;
                //}
                //else
                //{
                //    isJacking = Character.IsJacking;
                //}

                isJacking = Character.IsJacking;

            }
            TrackedVehicles.RemoveAll(x => !x.Vehicle.Exists());

            //if(IsCop && AliasedCop != null)
            //{
            //    AliasedCop.Update(this, this, PlacePoliceLastSeenPlayer, EntityProvider);
            //}
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
            //EntryPoint.WriteToConsole($"PLAYER EVENT: IsAiming Changed to: {IsAiming}", 5);
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
            //EntryPoint.WriteToConsole($"PLAYER EVENT: IsAimingInVehicle Changed to: {IsAimingInVehicle}", 5);
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
                        if (!CurrentVehicle.HasBeenEnteredByPlayer && !IsCop )
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
                        else if (VehicleTryingToEnter.LockStatus == (VehicleLockStatus)7 && CurrentVehicle.IsCar)
                        {
                            EntryPoint.WriteToConsole($"PLAYER EVENT: Car Break-In Start LockStatus {VehicleTryingToEnter.LockStatus}", 3);
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
                Surrendering.SetArrestedAnimation(WantedLevel <= 2);//needs to move
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

            if(Settings.SettingsManager.PlayerSettings.SetSlowMoOnDeath)
            {
                Game.TimeScale = 0.4f;
            }

            
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
            //EntryPoint.WriteToConsole($"PLAYER EVENT: CurrentTargetedPed to {CurrentTargetedPed?.Pedestrian?.Handle} CanHoldUpTargettedPed {CanHoldUpTargettedPed} CurrentTargetedPed?.CanBeMugged {CurrentTargetedPed?.CanBeMugged}", 5);
        }
        private void OnWantedLevelChanged()//runs after OnSuspectEluded (If Applicable)
        {
            if (IsNotWanted && PreviousWantedLevel != 0)//Lost Wanted
            {
                CriminalHistory.OnLostWanted();
                PoliceResponse.OnLostWanted();
                EntityProvider.CivilianList.ForEach(x => x.PlayerCrimesWitnessed.Clear());
                EntryPoint.WriteToConsole($"PLAYER EVENT: LOST WANTED", 3);
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
                }
                else
                {
                    Investigation.Reset();
                    PoliceResponse.OnBecameWanted();

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
                ButtonPrompts.RemoveAll(x => x.Group == "StartSimpleTransaction");

                if (!ButtonPrompts.Any(x => x.Identifier == $"Talk {CurrentLookedAtPed.Pedestrian.Handle}"))
                {
                    ButtonPrompts.RemoveAll(x => x.Group == "StartConversation");
                    ButtonPrompts.Add(new ButtonPrompt($"Talk to {CurrentLookedAtPed.FormattedName}", "StartConversation", $"Talk {CurrentLookedAtPed.Pedestrian.Handle}", Settings.SettingsManager.KeySettings.InteractStart, 1));
                }
                if (CurrentLookedAtPed.GetType() == typeof(Merchant) && CurrentLookedAtPed.IsNearSpawnPosition && !ButtonPrompts.Any(x => x.Identifier == $"Purchase {CurrentLookedAtPed.Pedestrian.Handle}"))
                {
                    ButtonPrompts.RemoveAll(x => x.Group == "StartTransaction");
                    ButtonPrompts.Add(new ButtonPrompt($"Purchase from {CurrentLookedAtPed.FormattedName}", "StartTransaction", $"Purchase {CurrentLookedAtPed.Pedestrian.Handle}", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 1));
                }
            }
            else
            {
                ButtonPrompts.RemoveAll(x => x.Group == "StartConversation");
                ButtonPrompts.RemoveAll(x => x.Group == "StartTransaction");


                if(ClosestSimpleTransaction != null)
                {
                    if (!ButtonPrompts.Any(x => x.Identifier == $"Purchase {ClosestSimpleTransaction.Name}"))
                    {
                        ButtonPrompts.RemoveAll(x => x.Group == "StartSimpleTransaction");
                        ButtonPrompts.Add(new ButtonPrompt($"Purchase from {ClosestSimpleTransaction.Name}", "StartSimpleTransaction", $"Purchase {ClosestSimpleTransaction.Name}", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 1));
                    }
                }
                else
                {
                    ButtonPrompts.RemoveAll(x => x.Group == "StartSimpleTransaction");
                }
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

            if(IsCop && AliasedCop != null && AliasedCop.Pedestrian.Exists())
            {
                if(AliasedCop.CanBeTasked)
                {
                    if (!ButtonPrompts.Any(x => x.Identifier == $"TakeoverAIControl"))
                    {
                        ButtonPrompts.RemoveAll(x => x.Group == "AIControl");
                        ButtonPrompts.Add(new ButtonPrompt($"Takeover Control", "AIControl", $"TakeoverAIControl", Settings.SettingsManager.KeySettings.ScenarioStart, 2));
                    }
                }
                else
                {
                    if (!ButtonPrompts.Any(x => x.Identifier == $"RelinquishAIControl"))
                    {
                        ButtonPrompts.RemoveAll(x => x.Group == "AIControl");
                        ButtonPrompts.Add(new ButtonPrompt($"Relinquish Control", "AIControl", $"RelinquishAIControl", Settings.SettingsManager.KeySettings.ScenarioStart, 2));
                    }
                }

            }
            else
            {
                ButtonPrompts.RemoveAll(x => x.Group == "AIControl");
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
        public void SetInPoliceCar()
        {
            ////this is no good

            //GameFiber SetPlayerInPoliceCarGF = GameFiber.StartNew(delegate
            //{
            //    while (Character.Exists() && (Character.IsRagdoll || Character.IsStunned))
            //    {
            //        GameFiber.Yield();
            //    }
            //    if (!Character.Exists())
            //    {
            //        return;
            //    }

            //    UnSetArrestedAnimation();
            //    GameFiber.Sleep(2000);

            //    NativeFunction.CallByName<bool>("NETWORK_REQUEST_CONTROL_OF_ENTITY", Game.LocalPlayer.Character);
            //    NativeFunction.CallByName<uint>("RESET_PLAYER_ARREST_STATE", Game.LocalPlayer);
            //    NativeFunction.Natives.xC0AA53F866B3134D();//FORCE_GAME_STATE_PLAYING
            //    if (Settings.SettingsManager.PlayerSettings.SetSlowMoOnDeath)
            //    {
            //        Game.TimeScale = 1f;
            //    }
            //    NativeFunction.Natives.xB4EDDC19532BFB85(); //_STOP_ALL_SCREEN_EFFECTS;
            //    NativeFunction.Natives.x80C8B1846639BB19(0);//_SET_CAM_EFFECT (0 = cancelled)
            //    NativeFunction.Natives.SET_PED_ALERTNESS(Character, 0);
            //    //new for drunk stuff
            //    NativeFunction.CallByName<int>("CLEAR_TIMECYCLE_MODIFIER");
            //    NativeFunction.CallByName<int>("STOP_GAMEPLAY_CAM_SHAKING", true);
            //    NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, false);
            //    NativeFunction.CallByName<bool>("RESET_PED_MOVEMENT_CLIPSET", Game.LocalPlayer.Character,0f);
            //    NativeFunction.CallByName<bool>("SET_PED_IS_DRUNK", Game.LocalPlayer.Character, false);

            //    NativeFunction.CallByName<bool>("RESET_HUD_COMPONENT_VALUES", 0);
            //    NativeFunction.Natives.xB9EFD5C25018725A("DISPLAY_HUD", true);
            //    NativeFunction.Natives.xC0AA53F866B3134D();//_RESET_LOCALPLAYER_STATE
            //    NativeFunction.CallByName<bool>("SET_PLAYER_HEALTH_RECHARGE_MULTIPLIER", Game.LocalPlayer, 0f);

            //    GameLocation PoliceStation = PlacesOfInterest.GetClosestLocation(Game.LocalPlayer.Character.Position, LocationType.Police);
            //   // shouldCheckViolations = false;
            //    Character.CanBePulledOutOfVehicles = false;
            //    NativeFunction.Natives.CLEAR_PED_TASKS(Character);
            //    NativeFunction.CallByName<bool>("RESET_PED_MOVEMENT_CLIPSET", Character);
            //    AnimationDictionary.RequestAnimationDictionay("mp_arresting");
            //    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Character, "mp_arresting", "idle", 1.0f, -1.0f, -1, 49, 0, 0, 1, 0);
            //    GameFiber.Sleep(1500);
            //    ButtonPrompts.Add(new ButtonPrompt("Skip Ride", "Surrender", "PlayerSkipRide", Keys.O, 1));
            //    while (!ButtonPrompts.Any(x => x.Identifier == "PlayerSkipRide" && x.IsPressedNow))
            //    {
            //        if (!IsInVehicle)
            //        {
            //            if (VehicleTaskedToEnter == null || !VehicleTaskedToEnter.Exists())
            //            {
            //                GetClosesetPoliceVehicle();
            //                EntryPoint.WriteToConsole($"PlayerArrested: Get in Car, Got New Car, was Blank", 3);
            //                GetInCarTask();
            //            }
            //            else if (VehicleTryingToEnter != null && VehicleTaskedToEnter.Exists() && !VehicleTaskedToEnter.IsSeatFree(SeatTaskedToEnter) && VehicleTaskedToEnter.GetPedOnSeat(SeatTaskedToEnter).Exists() && VehicleTaskedToEnter.GetPedOnSeat(SeatTaskedToEnter).Handle != Character.Handle)// && (VehicleTryingToEnter.Vehicle.Handle != VehicleTaskedToEnter.Handle || SeatTaskedToEnter != SeatTryingToEnter) && Ped.Pedestrian.Exists() && !Ped.Pedestrian.IsInAnyVehicle(true))
            //            {
            //                GetClosesetPoliceVehicle();
            //                EntryPoint.WriteToConsole($"PlayerArrested: Get in Car Got New Car, was occupied?", 3);
            //                GetInCarTask();
            //            }
            //            else if (VehicleTryingToEnter != null && VehicleTaskedToEnter.Exists() && VehicleTaskedToEnter.Speed > 1.0f)// && (VehicleTryingToEnter.Vehicle.Handle != VehicleTaskedToEnter.Handle || SeatTaskedToEnter != SeatTryingToEnter) && Ped.Pedestrian.Exists() && !Ped.Pedestrian.IsInAnyVehicle(true))
            //            {
            //                GetClosesetPoliceVehicle();
            //                EntryPoint.WriteToConsole($"PlayerArrested: Get in Car Got New Car, was driving away?", 3);
            //                GetInCarTask();
            //            }
            //        }
            //        else
            //        {
            //            if(WantedLevel > 0)
            //            {
            //                Reset(true, true, true, true, false);
            //            }


            //            if(PoliceStation != null && Character.DistanceTo2D(PoliceStation.EntrancePosition) <= 30f && Character.CurrentVehicle.Exists() && Character.CurrentVehicle.Speed <= 0.5f)
            //            {
            //                EntryPoint.WriteToConsole($"PlayerArrested: Arrived", 3);
            //                break;
            //            }
            //        }
            //        GameFiber.Yield();
            //    }
            //    Character.CanBePulledOutOfVehicles = true;
            //   // shouldCheckViolations = true;
            //    ButtonPrompts.RemoveAll(x => x.Group == "Surrender");
            //    if (!IsBusted)
            //    {
            //        NativeFunction.Natives.CLEAR_PED_TASKS(Character);
            //    }
            //    else
            //    {
            //        SurrenderToPolice(null);
            //    }    
            //}, "SetPlayerInPoliceCarGF");
        }
        //private void GetInCarTask()
        //{
        //    if (Character.Exists() && VehicleTryingToEnter != null && VehicleTryingToEnter.Vehicle.Exists())
        //    {
        //        EntryPoint.WriteToConsole($"GetArrested: Get in Car TASK START", 3);
        //        Character.BlockPermanentEvents = true;
        //        Character.KeepTasks = true;
        //        VehicleTaskedToEnter = VehicleTryingToEnter.Vehicle;
        //        SeatTaskedToEnter = SeatTryingToEnter;
        //        unsafe
        //        {
        //            int lol = 0;
        //            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
        //            NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", 0, VehicleTryingToEnter.Vehicle, -1, SeatTryingToEnter, 1f, 9);
        //            NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(4000, 8000));
        //            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
        //            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
        //            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Character, lol);
        //            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        //        }
        //    }
        //}
        //private void GetClosesetPoliceVehicle()
        //{
        //    VehicleExt ClosestAvailablePoliceVehicle = null;
        //    int OpenSeatInClosestAvailablePoliceVehicle = 9;
        //    float ClosestAvailablePoliceVehicleDistance = 999f;
        //    foreach (VehicleExt copCar in EntityProvider.PoliceVehicleList)
        //    {
        //        if (copCar.Vehicle.Exists() && copCar.Vehicle.Model.NumberOfSeats >= 4 && copCar.Vehicle.Speed < 0.5f)//stopped 4 door car with at least one seat free in back
        //        {
        //            float DistanceTo = copCar.Vehicle.DistanceTo2D(Character);
        //            if (DistanceTo <= 50f)
        //            {
        //                if (copCar.Vehicle.IsSeatFree(1))
        //                {
        //                    if (DistanceTo < ClosestAvailablePoliceVehicleDistance)
        //                    {
        //                        OpenSeatInClosestAvailablePoliceVehicle = 1;
        //                        ClosestAvailablePoliceVehicle = copCar;
        //                        ClosestAvailablePoliceVehicleDistance = DistanceTo;
        //                    }

        //                }
        //                else if (copCar.Vehicle.IsSeatFree(2))
        //                {
        //                    if (DistanceTo < ClosestAvailablePoliceVehicleDistance)
        //                    {
        //                        OpenSeatInClosestAvailablePoliceVehicle = 2;
        //                        ClosestAvailablePoliceVehicle = copCar;
        //                        ClosestAvailablePoliceVehicleDistance = DistanceTo;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    VehicleTryingToEnter = ClosestAvailablePoliceVehicle;
        //    SeatTryingToEnter = OpenSeatInClosestAvailablePoliceVehicle;
        //    if (ClosestAvailablePoliceVehicle != null && ClosestAvailablePoliceVehicle.Vehicle.Exists())
        //    {
        //        EntryPoint.WriteToConsole($"PlayerArrested: Seat Assigned Vehicle {VehicleTryingToEnter.Vehicle.Handle} Seat {SeatTryingToEnter}", 3);
        //    }
        //    else
        //    {
        //        EntryPoint.WriteToConsole($"PlayerArrested: Seat NOT Assigned", 3);
        //    }
        //}
    }
}