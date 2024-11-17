using Blackjack;
using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Locations;
using LosSantosRED.lsr.Player;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mod
{
    public class Player : IDispatchable, IActivityPerformable, IIntoxicatable, ITargetable, IPoliceRespondable, IInputable, IPedSwappable, IMuggable, IRespawnable, IViolateable, IWeaponDroppable, IDisplayable,
                          ICarStealable, IPlateChangeable, IActionable, IInteractionable, IInventoryable, IRespawning, ISaveable, IPerceptable, ILocateable, IDriveable, ISprintable, IWeatherAnnounceable,
                          IBusRideable, IGangRelateable, IWeaponSwayable, IWeaponRecoilable, IWeaponSelectable, ICellPhoneable, ITaskAssignable, IContactInteractable, IContactRelateable, ILicenseable, IPropertyOwnable,
                          ILocationInteractable, IButtonPromptable, IHumanStateable, IStanceable, IItemEquipable, IDestinateable, IVehicleOwnable, IBankAccountHoldable, IActivityManageable, IHealthManageable, IGroupManageable,
                          IMeleeManageable, ISeatAssignable, ICameraControllable, IPlayerVoiceable, IClipsetManageable, IOutfitManageable, IArmorManageable, IRestrictedAreaManagable, ITaxiRideable, IGangBackupable, IInteriorManageable, 
                            ICuffable, IIntimidationManageable, ICasinoGamePlayable, IVehicleManageable
    {
        public int UpdateState = 0;
        private float CurrentVehicleRoll;
        private uint GameTimeGotInVehicle;
        private uint GameTimeGotOutOfVehicle;
        private uint GameTimeLastBusted;
        private uint GameTimeLastCrashedVehicle;
        private uint GameTimeLastDied;
        private uint GameTimeLastFedUpCop;
        private uint GameTimeLastMoved;
        private uint GameTimeLastMovedFast;
        private uint GameTimeLastSetWanted;
        private uint GameTimeLastShot;
        private uint GameTimeLastUpdatedLookedAtPed;
        private uint GameTimeStartedHotwiring;
        private uint GameTimeStartedMoving;
        private uint GameTimeStartedMovingFast;
        private uint GameTimeStartedPlaying;
        private uint GameTimeWantedLevelStarted;
        private bool HasThrownGotOffFreeway;
        private bool HasThrownGotOnFreeway;
        private bool isActive = true;
        private bool isAiming;
        private bool isAimingInVehicle;
        private bool isCheckingExcessSpeed;
        private bool isExcessiveSpeed;
        private bool isGettingIntoVehicle;
        private bool isHotwiring;
        private bool isInVehicle;
        private bool isJacking = false;
        private bool isShooting;
        private Vector3 position;
        private uint prevCurrentLookedAtObjectHandle;
        private int PreviousWantedLevel;
        private int storedViewMode = -1;
        private uint targettingHandle;
        private int wantedLevel = 0;
        private uint prevCurrentVehicleHandle;

        private IIntoxicants Intoxicants;
        private IModItems ModItems;
        private INameProvideable Names;
        private IPlacesOfInterest PlacesOfInterest;
        private IRadioStations RadioStations;
        private IScenarios Scenarios;
        private ISettingsProvideable Settings;
        private ISpeeches Speeches;
        private IEntityProvideable World;
        private IZones Zones;
        private IDances Dances;
        private IWeapons Weapons;
        private ITimeControllable TimeControllable;
        private ICrimes Crimes;
        private IContacts Contacts;
        private IGangTerritories GangTerritories;
        private IGameSaves GameSaves;
        private ISeats Seats;
        private IAgencies Agencies;
        private IVehicleSeatAndDoorLookup VehicleSeatDoorData;
        private Vehicle VehicleTryingToEnter;
        private int SeatTryingToEnter;
        private bool currentlyHasScrewdriver;
        private MenuPool MenuPool;
        private UIMenu VehicleInteractMenu;
        private bool disableAutoEngineStart;
        private bool IsSirenOn;



        private uint GameTimeLastReportedCamera;
        private bool ManuallyClosedDoor;
        private bool IsRunningDoorCloseFlag;
        private bool prevAliasPedAsMainCharacter = true;//if change default setting
        private bool prevIsSleeping;
        private uint KillerHandle;
        private bool HasThrownInTunnel;


        public Player(string modelName, bool isMale, string suspectsName, IEntityProvideable provider, ITimeControllable timeControllable, IStreets streets, IZones zones, ISettingsProvideable settings, IWeapons weapons, IRadioStations radioStations, IScenarios scenarios, ICrimes crimes
            , IAudioPlayable audio, IAudioPlayable secondaryAudio, IPlacesOfInterest placesOfInterest, IInteriors interiors, IModItems modItems, IIntoxicants intoxicants, IGangs gangs, IJurisdictions jurisdictions, IGangTerritories gangTerritories, IGameSaves gameSaves, INameProvideable names, IShopMenus shopMenus
            , IPedGroups pedGroups, IDances dances, ISpeeches speeches, ISeats seats, IAgencies agencies, ISavedOutfits savedOutfits, IVehicleSeatAndDoorLookup vehicleSeatDoorData, ICellphones cellphones, IContacts contacts)
        {
            ModelName = modelName;
            IsMale = isMale;
            PlayerName = suspectsName;
            Crimes = crimes;
            World = provider;
            TimeControllable = timeControllable;
            Settings = settings;
            Weapons = weapons;
            RadioStations = radioStations;
            Scenarios = scenarios;
            GameTimeStartedPlaying = Game.GameTime;
            PlacesOfInterest = placesOfInterest;
            ModItems = modItems;
            Intoxicants = intoxicants;
            GangTerritories = gangTerritories;
            Zones = zones;
            GameSaves = gameSaves;
            Names = names;
            Seats = seats;
            Agencies = agencies;
            VehicleSeatDoorData = vehicleSeatDoorData;
            Contacts = contacts;
            Scanner = new Scanner(provider, this, audio, secondaryAudio, Settings, TimeControllable, PlacesOfInterest);
            HealthState = new HealthState(new PedExt(Game.LocalPlayer.Character, Settings, Crimes, Weapons, PlayerName, "Person", World), Settings, true);
            if (CharacterModelIsFreeMode)
            {
                HealthState.MyPed.VoiceName = FreeModeVoice;
            }
            CurrentLocation = new LocationData(Game.LocalPlayer.Character, streets, zones, interiors, Settings);
            Surrendering = new SurrenderActivity(this, World, Settings);
            Violations = new Violations(this, TimeControllable, Crimes, Settings, Zones, GangTerritories, World, this);
            Investigation = new Investigation(this, Settings, provider);
            CriminalHistory = new CriminalHistory(this, Settings, TimeControllable);
            PoliceResponse = new PoliceResponse(this, Settings, TimeControllable, World);
            SecurityResponse = new SecurityResponse(this, Settings, TimeControllable, World);
            SearchMode = new SearchMode(this, Settings);
            Inventory = new Inventory(this, Settings, ModItems);
            Sprinting = new Sprinting(this, Settings);
            Intoxication = new Intoxication(this);
            Respawning = new Respawning(TimeControllable, World, this, Weapons, PlacesOfInterest, Settings, this, this, ModItems);
            RelationshipManager = new RelationshipManager(gangs, Settings, PlacesOfInterest, TimeControllable, this, this);
            CellPhone = new CellPhone(this, this, jurisdictions, Settings, TimeControllable, gangs, PlacesOfInterest, Zones, streets, GangTerritories, Crimes, World, ModItems, Weapons, Names, shopMenus, cellphones, Contacts, Agencies);
            PlayerTasks = new PlayerTasks(this, TimeControllable, gangs, PlacesOfInterest, Settings, World, Crimes, names, Weapons, shopMenus, ModItems, pedGroups, Agencies, gangTerritories, zones);
            Licenses = new Licenses(this);
            Properties = new Properties(this, PlacesOfInterest, TimeControllable);
            ButtonPrompts = new ButtonPrompts(this, Settings, World);
            Injuries = new Injuries(this, Settings);
            Dances = dances;
            HumanState = new HumanState(this, TimeControllable, Settings);
            Speeches = speeches;
            Stance = new Stance(this, Settings);
            WeaponEquipment = new WeaponEquipment(this, this, Weapons, Settings, this, this, this);
            GPSManager = new GPSManager(this, World, Settings, TimeControllable);
            VehicleOwnership = new VehicleOwnership(this, World, Settings);
            BankAccounts = new BankAccounts(this, Settings, PlacesOfInterest);
            ActivityManager = new ActivityManager(this, settings, this, this, this, this, this, TimeControllable, RadioStations, Crimes, ModItems, Dances, World, Intoxicants, this, Speeches, Seats, Weapons, PlacesOfInterest, Zones, shopMenus, gangs, 
                gangTerritories, VehicleSeatDoorData, cellphones);
            HealthManager = new HealthManager(this, Settings);
            ArmorManager = new ArmorManager(this, settings);
            GroupManager = new GroupManager(this, this, Settings, World, gangs, Weapons);
            MeleeManager = new MeleeManager(this, Settings);
            PlayerVoice = new PlayerVoice(this, Settings, Speeches);
            ClipsetManager = new ClipsetManager(this, Settings);
            OutfitManager = new OutfitManager(this, savedOutfits);
            OfficerMIAWatcher = new OfficerMIAWatcher(World, this, this, Settings, TimeControllable);
            RestrictedAreaManager = new RestrictedAreaManager(this, this, World, Settings);
            TaxiManager = new TaxiManager(this, World,PlacesOfInterest, Settings);
            GangBackupManager = new GangBackupManager(World, this);
            InteriorManager = new InteriorManager(World, PlacesOfInterest, Settings, this, this, this);
            CuffManager = new CuffManager(this, Settings);
            RadarDetector = new RadarDetector(this, World, Settings);
            IntimidationManager = new IntimidationManager(this, World, Settings);
            GamblingManager = new GamblingManager(this, Settings, TimeControllable);
            VehicleManager = new VehicleManager(this, World, Settings);
        }
        public IntimidationManager IntimidationManager { get; private set; }
        public CuffManager CuffManager { get; private set; }
        public RelationshipManager RelationshipManager { get; private set; }
        public GPSManager GPSManager { get; private set; }
        public CriminalHistory CriminalHistory { get; private set; }
        public PlayerTasks PlayerTasks { get; private set; }
        public VehicleManager VehicleManager { get; private set; }
        public PoliceResponse PoliceResponse { get; private set; }
        public SecurityResponse SecurityResponse { get; private set; }
        public ButtonPrompts ButtonPrompts { get; private set; }
        public CellPhone CellPhone { get; private set; }
        public LocationData CurrentLocation { get; set; }
        public WeaponEquipment WeaponEquipment { get; private set; }
        public HumanState HumanState { get; set; }
        public HealthState HealthState { get; private set; }
        public Injuries Injuries { get; private set; }
        public Intoxication Intoxication { get; private set; }
        public Inventory Inventory { get; set; }
        public Investigation Investigation { get; private set; }
        public Licenses Licenses { get; private set; }
        public Properties Properties { get; private set; }
        public Sprinting Sprinting { get; private set; }
        public Stance Stance { get; private set; }
        public Violations Violations { get; private set; }
        public Respawning Respawning { get; private set; }
        public Scanner Scanner { get; private set; }
        public SearchMode SearchMode { get; private set; }
        public SurrenderActivity Surrendering { get; private set; }
        public VehicleOwnership VehicleOwnership { get; private set; }
        public BankAccounts BankAccounts { get; private set; }
        public ActivityManager ActivityManager { get; private set; }
        public HealthManager HealthManager { get; private set; }
        public ArmorManager ArmorManager { get; private set; }
        public GroupManager GroupManager { get; private set; }
        public MeleeManager MeleeManager { get; private set; }
        public PlayerVoice PlayerVoice { get; private set; }
        public ClipsetManager ClipsetManager { get; private set; }
        public OutfitManager OutfitManager { get; private set; }
        public OfficerMIAWatcher OfficerMIAWatcher { get; private set; }
        public RestrictedAreaManager RestrictedAreaManager { get; private set; }
        public TaxiManager TaxiManager { get; private set; }
        public RadarDetector RadarDetector { get; private set; }
        public GamblingManager GamblingManager { get; private set; }

        public GangBackupManager GangBackupManager { get; private set; }
        public InteriorManager InteriorManager { get; private set; }
        public float ActiveDistance => Investigation.IsActive ? Investigation.Distance : WantedLevel >= 6 ? 5000f : 500f + (WantedLevel * 200f);
        public bool AnyGangMemberCanHearPlayer { get; set; }
        public bool AnyGangMemberCanSeePlayer { get; set; }
        public bool AnyGangMemberRecentlySeenPlayer { get; set; }
        public bool AnyHumansNear => World.Pedestrians.LivingPeople.Any(x => x.DistanceToPlayer <= 10f);
        public bool AnyPoliceCanHearPlayer { get; set; }
        public bool AnyPoliceCanRecognizePlayer { get; set; }
        public bool AnyPoliceCanSeePlayer { get; set; }
        public bool AnyPoliceInHeliCanSeePlayer { get; set; }
        public bool AnyPoliceKnowInteriorLocation { get; set; }
        public bool AnyPoliceRecentlySeenPlayer { get; set; }
        public bool AnyPoliceSawPlayerViolating { get; set; }
        public int AssignedSeat => -1;
        public VehicleExt AssignedVehicle => null;
        public List<Rage.Object> AttachedProp { get; set; } = new List<Rage.Object>();
        public bool BeingArrested { get; private set; }
        public List<uint> BlackListedVehicles => new List<uint>();
        public bool CanSitOnCurrentLookedAtObject { get; private set; }
        public int CellX { get; private set; }
        public int CellY { get; private set; }
        public Ped Character => Game.LocalPlayer.Character;
        public bool CharacterModelIsFreeMode => ModelName.ToLower() == "mp_f_freemode_01" || ModelName.ToLower() == "mp_m_freemode_01";
        public bool CharacterModelIsPrimaryCharacter => ModelName.ToLower() == "player_zero" || ModelName.ToLower() == "player_one" || ModelName.ToLower() == "player_two";
        public Cop ClosestCopToPlayer { get; set; }
        public Cop ClosestCopDriverToPlayer { get; set; }
        public Agency AssignedAgency { get; set; }
        public GameLocation ClosestInteractableLocation { get; private set; }
        public float ClosestPoliceDistanceToPlayer { get; set; }
        public Scenario ClosestScenario { get; private set; }
        public GangMember CurrentLookedAtGangMember { get; private set; }
        public Rage.Object CurrentLookedAtObject { get; private set; }
        public PedExt CurrentLookedAtPed { get; private set; }
        public VehicleExt CurrentLookedAtVehicle { get; private set; }
        public PedVariation CurrentModelVariation { get; set; }
        public VehicleExt CurrentSeenVehicle => CurrentVehicle ?? VehicleGettingInto;
        public PedExt CurrentTargetedPed { get; private set; }
        public VehicleExt CurrentVehicle { get; set; }
        public PedExt PedLastKilledPlayer { get; private set; }

        public PedSwap PedSwap { get; set; }
        public bool CurrentVehicleIsInAir { get; set; }
        public bool CurrentVehicleIsRolledOver { get; set; }
        public string DebugString { get; set; }
        public bool DiedInVehicle { get; private set; }
        public float FootSpeed { get; set; }
        public string FreeModeVoice { get; set; }//IsMale ? Settings.SettingsManager.PlayerOtherSettings.MaleFreeModeVoice : Settings.SettingsManager.PlayerOtherSettings.FemaleFreeModeVoice;
        public string Gender => IsMale ? "M" : "F";
        public int GroupID { get; set; }
        public uint Handle => Game.LocalPlayer.Character.Handle;
        public bool HasBeenMoving => GameTimeStartedMoving != 0 && Game.GameTime - GameTimeStartedMoving >= 5000;
        public bool HasBeenMovingFast => GameTimeStartedMovingFast != 0 && Game.GameTime - GameTimeStartedMovingFast >= 2000;
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
        public bool IsAlive => !IsDead;
        public bool IsAliveAndFree => !IsBusted && !IsDead;
        public bool IsArrested { get; set; }
        public bool IsAttemptingToSurrender => Surrendering.HandsAreUp && !PoliceResponse.IsWeaponsFree;
        public bool IsBeingANuisance { get; set; }
        public bool IsBeingBooked { get; set; }
        public bool IsBreakingIntoCar => IsCarJacking || IsLockPicking || IsHotWiring || isJacking;
        public bool IsBustable => !Settings.SettingsManager.ViolationSettings.IsUnBustable && IsAliveAndFree && PoliceResponse.HasBeenWantedFor >= 3000 && !ActivityManager.IsCommitingSuicide && !ActivityManager.IsHoldingHostage && !RecentlyBusted && !IsAiming && !RecentlyShot && !RecentlyResistedArrest && !PoliceResponse.IsWeaponsFree && (IsIncapacitated || (!IsMoving && !IsMovingDynamically)) && (!IsInVehicle || WantedLevel == 1 || IsIncapacitated);
        public bool IsDetainable => !Settings.SettingsManager.ViolationSettings.IsUnBustable && IsAliveAndFree && !ActivityManager.IsCommitingSuicide && !ActivityManager.IsHoldingHostage && !RecentlyBusted && !RecentlyResistedArrest && !IsAiming && !RecentlyShot && !PoliceResponse.IsWeaponsFree && (IsIncapacitated || (!IsMoving && !IsMovingDynamically)) && (!IsInVehicle || IsIncapacitated);
        public bool IsAnimal => false;
        public bool IsBusted { get; private set; }
        public bool IsCarJacking { get; set; }
        public bool IsChangingLicensePlates { get; set; }
        public bool IsSetAutoCallBackup { get; set; } = false;
        public bool IsCop { get; set; } = false;
        public bool IsEMT { get; set; } = false;
        public bool IsFireFighter { get; set; } = false;
        public bool IsSecurityGuard { get; set; } = false;
        public bool IsRidingOnTrain { get; set; }
        public bool HasBustPowers => IsCop || IsSecurityGuard;

        public bool CanBustPeds => (IsCop || IsSecurityGuard) && !IsIncapacitated;
        public bool IsServicePed => IsCop || IsEMT || IsFireFighter;
        public bool AutoDispatch { get; set; } = false;
        public bool IsCustomizingPed { get; set; }
        public bool IsDangerouslyArmed => WeaponEquipment.IsDangerouslyArmed;
        public bool IsDead { get; private set; }
        public bool IsDealingDrugs { get; set; } = false;
        public bool IsDealingIllegalGuns { get; set; } = false;
        public bool IsDisplayingCustomMenus => IsTransacting || IsCustomizingPed || ActivityManager.IsConversing;
        public bool IsDoingSuspiciousActivity { get; set; } = false;
        public int CurrentSeat { get; private set; }
        public bool IsDriver { get; private set; }
        public bool IsDuckingInVehicle { get; set; } = false;
        public bool IsSetDisabledControls { get; set; } = false;
        public bool IsGangMember => RelationshipManager.GangRelationships.CurrentGang != null;
        public Gang CurrentGang => RelationshipManager.GangRelationships.CurrentGang;
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
        public bool IsHotWiring { get; private set; }
        public bool IsInAirVehicle { get; private set; }
        public bool IsInAutomobile { get; private set; }
        public bool IsOnBicycle { get; private set; }
        public bool IsIncapacitated => IsStunned || IsRagdoll;
        public bool IsInCover { get; private set; }
        public ICasinoGamePlayable CasinoGamePlayer => this;
        public bool IsInCombat { get; private set; }

        public bool IsInFirstPerson { get; private set; }
        public bool IsInSearchMode { get; set; }
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
        public bool IsMakingInsultingGesture { get; set; }
        public bool IsMale { get; set; }
        public bool IsMobileRadioEnabled { get; private set; }
        public bool IsMoveControlPressed { get; set; }
        public bool IsMoving => GameTimeLastMoved != 0 && Game.GameTime - GameTimeLastMoved <= 2000;
        public bool IsMovingDynamically { get; private set; }
        public bool IsSwimming { get; private set; }
        public bool IsShowingFrontEndMenus => !IsNotShowingFrontEndMenus;
        public bool IsNotShowingFrontEndMenus { get; set; }
        public bool IsMovingFast => GameTimeLastMovedFast != 0 && Game.GameTime - GameTimeLastMovedFast <= 2000;
        public bool IsNearScenario { get; private set; }
        public bool IsNotHoldingEnter { get; set; }
        public bool IsNotWanted => wantedLevel == 0;
        public bool IsOnFoot => !IsInVehicle;
        public bool IsOnMotorcycle { get; private set; }
        public bool IsOnMuscleRelaxants { get; set; }
        public bool IsPressingFireWeapon { get; set; }
        public bool IsRagdoll { get; private set; }
        public bool IsResting { get; set; } = false;
        public bool IsStandingOnNonTrainVehicle { get; set; }
        public bool IsRidingBus { get; set; }
        public bool IsShooting
        {
            get => isShooting;
            private set
            {
                if (isShooting != value)
                {
                    isShooting = value;
                    OnIsShootingChanged();
                }
            }
        }
        public bool IsSleeping { get; set; } = false;
        public bool IsSleepingOutside { get; set; } = false;
        public bool IsStill { get; private set; }
        public bool IsStunned { get; private set; }
        public bool IsTransacting { get; set; }
        public bool IsVisiblyArmed { get; set; }
        public VehicleExt InterestedVehicle => IsInVehicle ? CurrentVehicle : CurrentLookedAtVehicle;
        public bool IsWanted => wantedLevel > 0;
        public Vehicle LastFriendlyVehicle { get; set; }
        public int LastSeatIndex => -1;
        public string ModelName { get; set; }
        public Ped Pedestrian => Game.LocalPlayer.Character;
        public bool PoliceLastSeenOnFoot { get; set; }
        public Vector3 PlacePolicePhysicallyLastSeenPlayer { get; set; }
        public Vector3 PlacePoliceLastSeenPlayer { get; set; }
        public bool IsNearbyPlacePoliceShouldSearchForPlayer { get; set; }
        public Vector3 PlacePoliceShouldSearchForPlayer { get; set; }
        public Vector3 StreetPlacePoliceShouldSearchForPlayer { get; set; }
        public Vector3 StreetPlacePoliceLastSeenPlayer { get; set; }
        public bool IsGeneralTrafficLawImmune => (IsCop || IsEMT || IsFireFighter) && IsSirenOn;
        public string PlayerName { get; set; }
        public Vector3 Position => position;
        public VehicleExt PreviousVehicle { get; private set; }
        public bool RecentlyBribedPolice => Respawning.RecentlyBribedPolice;
        public bool RecentlyBusted => GameTimeLastBusted != 0 && Game.GameTime - GameTimeLastBusted <= 5000;
        public bool RecentlyCrashedVehicle => GameTimeLastCrashedVehicle != 0 && Game.GameTime - GameTimeLastCrashedVehicle <= 5000;
        public bool RecentlyFedUpCop => GameTimeLastFedUpCop != 0 && Game.GameTime - GameTimeLastFedUpCop <= 5000;
        public bool RecentlyGotOutOfVehicle => GameTimeGotOutOfVehicle != 0 && Game.GameTime - GameTimeGotOutOfVehicle <= 1000;
        public bool RecentlyPaidFine => Respawning.RecentlyPaidFine;
        public bool RecentlyResistedArrest => Respawning.RecentlyResistedArrest;
        public bool RecentlyRespawned => Respawning.RecentlyRespawned;
        public bool RecentlySetWanted => GameTimeLastSetWanted != 0 && Game.GameTime - GameTimeLastSetWanted <= 5000;
        public bool RecentlyShot => GameTimeLastShot != 0 && !RecentlyStartedPlaying && Game.GameTime - GameTimeLastShot <= 3000;
        public bool SemiRecentlyShot => GameTimeLastShot != 0 && !RecentlyStartedPlaying && Game.GameTime - GameTimeLastShot <= 17000;
        public bool RecentlyStartedPlaying => GameTimeStartedPlaying != 0 && Game.GameTime - GameTimeStartedPlaying <= 3000;
        public bool ReleasedFireWeapon { get; set; }
        public List<VehicleExt> ReportedStolenVehicles => TrackedVehicles.Where(x => x.NeedsToBeReportedStolen && !x.HasBeenDescribedByDispatch && !x.AddedToReportedStolenQueue).ToList();
        public float SearchModePercentage => SearchMode.SearchModePercentage;
        public bool ShouldCheckViolations => !Settings.SettingsManager.ViolationSettings.TreatAsCop && !IsCop && !RecentlyStartedPlaying;
        public int SpeechSkill { get; set; }
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
        public uint TimeInCurrentVehicle => GameTimeGotInVehicle == 0 || !IsInVehicle ? 0 : Game.GameTime - GameTimeGotInVehicle;
        public uint TimeInSearchMode => SearchMode.TimeInSearchMode;
        public uint TimeToRecognize
        {
            get
            {
                uint Time = Settings.SettingsManager.PlayerOtherSettings.Recognize_BaseTime;
                if (TimeControllable.IsNight)
                {
                    Time += Settings.SettingsManager.PlayerOtherSettings.Recognize_NightPenalty;
                }
                else if (IsInVehicle)
                {
                    Time += Settings.SettingsManager.PlayerOtherSettings.Recognize_VehiclePenalty;
                    if (NativeFunction.Natives.GET_PED_CONFIG_FLAG<bool>(Character, 359, true))//isduckinginvehicle?
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
        public bool VeryRecentlyShot => GameTimeLastShot != 0 && Game.GameTime - GameTimeLastShot <= 500;
        public int WantedLevel => wantedLevel;
        public bool WasDangerouslyArmedWhenBusted { get; private set; }
        public bool IsUsingController { get; set; }
        public bool IsShowingActionWheel { get; set; }
        public bool IsInPoliceVehicle { get; private set; }
        public Dispatcher Dispatcher { get; set; }
        public bool IsBlockingTraffic { get; set; }




        //Required
        public void Setup()
        {
            GameTimeStartedPlaying = Game.GameTime;
            Violations.Setup();
            Respawning.Setup();
            Scanner.Setup();
            RelationshipManager.Setup();
            CellPhone.Setup();
            PlayerTasks.Setup();
            Properties.Setup();
            ButtonPrompts.Setup();
            HumanState.Setup();
            GPSManager.Setup();
            SetWantedLevel(0, "Initial", true);

            WeaponEquipment.SetUnarmed();
            
            VehicleOwnership.Setup();
            BankAccounts.Setup();
            HealthManager.Setup();
            ArmorManager.Setup();
            GroupManager.Setup();
            MeleeManager.Setup();
            PlayerVoice.Setup();
            ActivityManager.Setup();
            HealthState.Setup();
            OfficerMIAWatcher.Setup();
            RestrictedAreaManager.Setup();
            InteriorManager.Setup();
            CuffManager.Setup();
            RadarDetector.Setup();
            GamblingManager.Setup();
            VehicleManager.Setup();
            ModelName = Game.LocalPlayer.Character.Model.Name;
            CurrentModelVariation = NativeHelper.GetPedVariation(Game.LocalPlayer.Character);
            FreeModeVoice = Game.LocalPlayer.Character.IsMale ? Settings.SettingsManager.PlayerOtherSettings.MaleFreeModeVoice : Settings.SettingsManager.PlayerOtherSettings.FemaleFreeModeVoice;
            if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Exists())
            {
                UpdateCurrentVehicle();
                VehicleOwnership.TakeOwnershipOfVehicle(CurrentVehicle, false);
            }
            disableAutoEngineStart = Settings.SettingsManager.VehicleSettings.DisableAutoEngineStart;
            if (Settings.SettingsManager.VehicleSettings.DisableAutoEngineStart)
            {
                NativeFunction.Natives.SET_PED_CONFIG_FLAG<bool>(Game.LocalPlayer.Character, (int)PedConfigFlags._PED_FLAG_DISABLE_STARTING_VEH_ENGINE, true);
            }
            if (Settings.SettingsManager.VehicleSettings.DisableAutoHelmet)
            {
                NativeFunction.Natives.SET_PED_CONFIG_FLAG<bool>(Game.LocalPlayer.Character, (int)PedConfigFlags._PED_FLAG_PUT_ON_MOTORCYCLE_HELMET, false);
            }
            if (Settings.SettingsManager.PlayerOtherSettings.DisableVanillaGangHassling)
            {
                NativeFunction.Natives.SET_PLAYER_CAN_BE_HASSLED_BY_GANGS(Game.LocalPlayer, false);
            }
            if (Settings.SettingsManager.PlayerOtherSettings.AllowAttackingFriendlyPeds)
            {
                NativeFunction.Natives.SET_CAN_ATTACK_FRIENDLY(Character, true, false);
            }
            if (Settings.SettingsManager.PlayerOtherSettings.AllowRunningInInteriors)
            {
                NativeFunction.Natives.SET_PED_CONFIG_FLAG<bool>(Game.LocalPlayer.Character, 427, true);
            }
            WeaponEquipment.Setup();
            CellPhone.Start();
            GangBackupManager.Setup();
            IntimidationManager.Setup();
            SpeechSkill = RandomItems.GetRandomNumberInt(Settings.SettingsManager.PlayerOtherSettings.PlayerSpeechSkill_Min, Settings.SettingsManager.PlayerOtherSettings.PlayerSpeechSkill_Max);
            Update();
            foreach(GameLocation bl in PlacesOfInterest.PossibleLocations.InteractableLocations())
            {
                bl.SetupPlayer(this);
            }
        }

 

        public void Update()
        {
            UpdateVehicleData();
            if (!Settings.SettingsManager.PerformanceSettings.EnableIncreasedUpdateMode)
            {
                GameFiber.Yield();
            }
            UpdateWeaponData();
            if (!Settings.SettingsManager.PerformanceSettings.EnableIncreasedUpdateMode)
            {
                GameFiber.Yield();
            }
            UpdateStateData();
            GameFiber.Yield();
            bool IntoxicationIsPrimary = false;
            if (Intoxication.CurrentIntensity > Injuries.CurrentIntensity)
            {
                IntoxicationIsPrimary = true;
            }
            Intoxication.Update(IntoxicationIsPrimary);
            if (!Settings.SettingsManager.PerformanceSettings.EnableIncreasedUpdateMode)
            {
                GameFiber.Yield();
            }
            Injuries.Update(!IntoxicationIsPrimary);
            if (!Settings.SettingsManager.PerformanceSettings.EnableIncreasedUpdateMode)
            {
                GameFiber.Yield();
            }
            HumanState.Update();
            BankAccounts.Update();
            HealthManager.Update();
            GroupManager.Update();
            if (!Settings.SettingsManager.PerformanceSettings.EnableIncreasedUpdateMode)
            {
                GameFiber.Yield();
            }
            ButtonPrompts.Update();
            MeleeManager.Update();
            if (!Settings.SettingsManager.PerformanceSettings.EnableIncreasedUpdateMode)
            {
                GameFiber.Yield();
            }
            PlayerVoice.Update();
            ActivityManager.Update();
            OfficerMIAWatcher.Update();
            //GameFiber.Yield();//TR Yield RemovedTest 1
            RestrictedAreaManager.Update();//yields in here
            TaxiManager.Update();

            GangBackupManager.Update();
            InteriorManager.Update();
            CuffManager.Update();
            if (!Settings.SettingsManager.PerformanceSettings.EnableIncreasedUpdateMode)
            {
                GameFiber.Yield();
            }
            RadarDetector.Update();
            IntimidationManager.Update();
            VehicleManager.Update();
        }
        public void SetNotBusted()
        {
            BeingArrested = false;
            IsBusted = false;
        }
        public void Reset(bool resetWanted, bool resetTimesDied, bool resetWeapons, bool resetCriminalHistory, bool resetInventory, bool resetIntoxication, bool resetRelationships, bool resetOwnedVehicles, 
            bool resetCellphone, bool resetActiveTasks, bool resetProperties, bool resetHealth, bool resetNeeds, bool resetGroup, bool resetLicenses, bool resetActivites, bool resetGracePeriod, 
            bool resetBankAccounts, bool resetSavedGame, bool resetMessages, bool resetInteriors, bool resetGambling, bool resetPersistVehicle)
        {
            IsDead = false;
            IsBusted = false;
            IsArrested = false;
            IsBeingBooked = false;
            Game.LocalPlayer.HasControl = true;
            BeingArrested = false;
            HealthState.Reset();
            if (resetActivites)
            {
                ActivityManager.Reset();
            }
            CurrentVehicle = null;
            GPSManager.Reset();
            NativeFunction.Natives.SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY(false);
            IsMobileRadioEnabled = false;
            if(resetGracePeriod)
            {
                PoliceResponse.ResetGracePeriod();
            }
            else
            {
                PoliceResponse.AddToGracePeriod();
            }

            TaxiManager.Reset();
            GangBackupManager.Reset();

            KillerHandle = 0;
            PedLastKilledPlayer = null;

            if (resetWanted)
            {
                GameTimeStartedPlaying = Game.GameTime;
                PoliceResponse.Reset();
                Investigation.Reset();
                Violations.Reset();            
                Scanner.Reset();
                SecurityResponse.Reset();
                //Surrendering.Reset();

                Update();
            }
            if (resetTimesDied)
            {
                Respawning.Reset();
            }
            if (resetWeapons)
            {
                WeaponEquipment.Reset();           
            }
            if (resetCriminalHistory)
            {
                CriminalHistory.Reset();
            }
            if (resetInventory)
            {
                Inventory.Reset();
                ArmorManager.Reset();
            }
            if (resetIntoxication)
            {
                Intoxication.Reset();
            }

            if (resetActiveTasks)
            {
                PlayerTasks.Reset();
            }

            if (resetRelationships)
            {
                RelationshipManager.Reset(false);
            }
            if (resetOwnedVehicles)
            {
                VehicleOwnership.Reset();
            }
            if (resetCellphone)
            {
                CellPhone.Reset();
            }

            if (resetProperties)
            {
                Properties.Reset();
                World.Places.Reset();
            }


            if (resetHealth)
            {
                Injuries.Reset();
            }
            if (resetNeeds)
            {
                HumanState.Reset();
            }
            if(resetGroup)
            {
                GroupManager.Reset();
            }
            if(resetLicenses)
            {
                Licenses.Reset();
            }
            if(resetBankAccounts)
            {
                BankAccounts.Reset();
            }
            if(resetSavedGame)
            {
                GameSaves.OnChangedPlayer();
            }
            if(resetMessages)
            {
                CellPhone.ClearPendingTexts();
            }
            if(resetInteriors)
            {
                 InteriorManager.Reset();
            }
            if(resetGambling)
            {
                GamblingManager.Reset();
            }
            if(resetPersistVehicle)
            {
                VehicleManager.Reset();
            }

            if (Settings.SettingsManager.VehicleSettings.DisableAutoEngineStart)
            {
                NativeFunction.Natives.SET_PED_CONFIG_FLAG<bool>(Game.LocalPlayer.Character, (int)PedConfigFlags._PED_FLAG_DISABLE_STARTING_VEH_ENGINE, true);
            }
            if (Settings.SettingsManager.VehicleSettings.DisableAutoHelmet)
            {
                NativeFunction.Natives.SET_PED_CONFIG_FLAG<bool>(Game.LocalPlayer.Character, (int)PedConfigFlags._PED_FLAG_PUT_ON_MOTORCYCLE_HELMET, false);
            }
            if (Settings.SettingsManager.PlayerOtherSettings.AllowRunningInInteriors)
            {
                NativeFunction.Natives.SET_PED_CONFIG_FLAG<bool>(Game.LocalPlayer.Character, 427, true);
            }
            if (Settings.SettingsManager.PlayerOtherSettings.AllowAttackingFriendlyPeds)
            {
                NativeFunction.Natives.SET_CAN_ATTACK_FRIENDLY(Game.LocalPlayer.Character, true, false);
            }
            CuffManager.Reset();
            IntimidationManager.Reset();
           // NativeFunction.Natives.SET_PED_CONFIG_FLAG<bool>(Character, 313, false);
        }
        public void Dispose()
        {
            isActive = false;
            Scanner.Dispose();
            Investigation.Dispose(); //remove blip
            CriminalHistory.Dispose(); //remove blip
            PoliceResponse.Dispose(); //same ^
            SearchMode.Dispose();
            RelationshipManager.Dispose();
            CellPhone.Dispose();
            PlayerTasks.Dispose();
            Properties.Dispose();
            ButtonPrompts.Dispose();
            Intoxication.Dispose();
            Injuries.Dispose();
            HumanState.Dispose();
            WeaponEquipment.Dispose();
            GPSManager.Dispose();
            VehicleOwnership.Dispose();
            BankAccounts.Dispose();
            HealthManager.Dispose();
            ArmorManager.Dispose();
            GroupManager.Dispose();
            MeleeManager.Dispose();
            Violations.Dispose();
            PlayerVoice.Dispose();
            ActivityManager.Dispose();
            ClipsetManager.Dispose();
            OutfitManager.Dispose();
            OfficerMIAWatcher.Dispose();
            RestrictedAreaManager.Dispose();
            TaxiManager.Dispose();
            GangBackupManager.Dispose();
            InteriorManager.Dispose();
            CuffManager.Dispose();
            RadarDetector.Dispose();
            IntimidationManager.Dispose();
            GamblingManager.Dipsose();
            VehicleManager.Dispose();
            NativeFunction.Natives.SET_PED_RESET_FLAG(Game.LocalPlayer.Character, 186, true);
            NativeFunction.Natives.SET_PED_CONFIG_FLAG<bool>(Game.LocalPlayer.Character, (int)PedConfigFlags._PED_FLAG_PUT_ON_MOTORCYCLE_HELMET, true);
            NativeFunction.Natives.SET_PED_CONFIG_FLAG<bool>(Game.LocalPlayer.Character, (int)PedConfigFlags._PED_FLAG_DISABLE_STARTING_VEH_ENGINE, false);
            NativeFunction.Natives.SET_PED_IS_DRUNK<bool>(Game.LocalPlayer.Character, false);
            NativeFunction.Natives.RESET_PED_MOVEMENT_CLIPSET<bool>(Game.LocalPlayer.Character);
            NativeFunction.Natives.SET_PED_CONFIG_FLAG<bool>(Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, false);
            //NativeFunction.Natives.SET_PED_CONFIG_FLAG<bool>(Game.LocalPlayer.Character, 313, false);
            if (Settings.SettingsManager.UIGeneralSettings.AllowScreenEffectReset)//this should be moved methinks
            {
                NativeFunction.Natives.CLEAR_TIMECYCLE_MODIFIER<int>();
                NativeFunction.Natives.x80C8B1846639BB19(0);
                NativeFunction.Natives.STOP_GAMEPLAY_CAM_SHAKING<int>(true);
            }
            Game.LocalPlayer.WantedLevel = 0;
            NativeFunction.Natives.SET_FAKE_WANTED_LEVEL(0);
            NativeFunction.Natives.SET_MAX_WANTED_LEVEL(6);
            NativeFunction.Natives.SET_PED_AS_COP(Game.LocalPlayer.Character, false);      
            if (Settings.SettingsManager.PlayerOtherSettings.SetSlowMoOnDeath)
            {
                Game.TimeScale = 1f;
            }
            NativeFunction.Natives.ENABLE_ALL_CONTROL_ACTIONS(0);//enable all controls in case we left some disabled
            NativeFunction.Natives.SET_CAN_ATTACK_FRIENDLY(Character, false, false);
            NativeFunction.Natives.SET_PLAYER_CAN_BE_HASSLED_BY_GANGS(Game.LocalPlayer, true);
            NativeFunction.Natives.DESTROY_ALL_CAMS(0);
            NativeFunction.Natives.CLEAR_FOCUS();
            Game.LocalPlayer.HasControl = true;
            if(Game.IsScreenFadedOut || Game.IsScreenFadingOut)
            {
                Game.FadeScreenIn(0, false);
            }
        }
        //Needed
        public void ChangeName(string newName)
        {
            PlayerName = newName;
            //EntryPoint.WriteToConsole($"PLAYER EVENT: ChangeName {newName}");
        }
        public void DisplayPlayerNotification()
        {
            string NotifcationText = "Warrants: ~g~None~s~";
            if (PoliceResponse.HasObservedCrimes)
            {
                NotifcationText = "Wanted For:" + PoliceResponse.PrintCrimes(true);
            }
            else if (CriminalHistory.HasHistory)
            {
                NotifcationText = "Wanted For:" + CriminalHistory.PrintCriminalHistory();
            }
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~b~Personal Info", $"~y~{PlayerName}", NotifcationText);
            // DisplayPlayerVehicleNotification();
        }
        public void SetDemographics(string modelName, bool isMale, string playerName, int money, int speechSkill, string voiceName)
        {
            ModelName = modelName;
            PlayerName = playerName;
            IsMale = isMale;
            BankAccounts.SetCash(money);
            SpeechSkill = speechSkill;// 
            if (voiceName == "")
            {
                FreeModeVoice = IsMale ? Settings.SettingsManager.PlayerOtherSettings.MaleFreeModeVoice : Settings.SettingsManager.PlayerOtherSettings.FemaleFreeModeVoice;
            }
            else
            {
                FreeModeVoice = voiceName;
            }

            //EntryPoint.WriteToConsole($"PLAYER EVENT: SetDemographics MoneyToSet {money} Current: {BankAccounts.Money} {NativeHelper.CashHash(Settings.SettingsManager.PedSwapSettings.MainCharacterToAlias)}");
        }
        public void SetVoice(string voiceName)
        {
            if (voiceName == "")
            {
                FreeModeVoice = IsMale ? Settings.SettingsManager.PlayerOtherSettings.MaleFreeModeVoice : Settings.SettingsManager.PlayerOtherSettings.FemaleFreeModeVoice;
            }
            else
            {
                FreeModeVoice = voiceName;
            }
        }
        public void LocationUpdate()
        {
            CurrentLocation.Update(Character, IsInVehicle);
            if (CurrentLocation.HasBeenOnHighway && !HasThrownGotOnFreeway)
            {
                OnGotOnFreeway();
                HasThrownGotOnFreeway = true;
                HasThrownGotOffFreeway = false;
            }
            else if (CurrentLocation.HasBeenOffHighway && !HasThrownGotOffFreeway && HasThrownGotOnFreeway)
            {
                OnGotOffFreeway();
                HasThrownGotOnFreeway = false;
                HasThrownGotOffFreeway = true;
            }


            if (CurrentLocation.HasBeenInTunnel && !HasThrownInTunnel)
            {
                OnWentInTunnel();
                HasThrownInTunnel = true;
            }
            if(!CurrentLocation.TreatAsInTunnel && HasThrownInTunnel)
            {
                HasThrownInTunnel = false;
            }
        }
        public void PlaySpeech(string speechName, bool useMegaphone)
        {
            EntryPoint.WriteToConsole($"PLAYER PlaySpeech CharacterModelIsFreeMode:{CharacterModelIsFreeMode} FreeModeVoice{FreeModeVoice}");
            if (CharacterModelIsFreeMode && FreeModeVoice != "")
            {
                if (useMegaphone)
                {
                    Character.PlayAmbientSpeech(FreeModeVoice, speechName, 0, SpeechModifier.ForceMegaphone);
                    EntryPoint.WriteToConsole($"PLAYER PlaySpeech SPEAK 1");
                }
                else
                {
                    Character.PlayAmbientSpeech(FreeModeVoice, speechName, 0, SpeechModifier.Force);
                    EntryPoint.WriteToConsole($"PLAYER PlaySpeech SPEAK 2");
                }
                //EntryPoint.WriteToConsoleTestLong($"FREEMODE COP SPEAK {Character.Handle} freeModeVoice {FreeModeVoice} speechName {speechName}");
            }
            else
            {
                Character.PlayAmbientSpeech(speechName, useMegaphone);
                EntryPoint.WriteToConsole($"PLAYER PlaySpeech SPEAK 3");
            }
        }
        //Maybe?
        public int FineAmount()
        {
            int InitialAmount = Settings.SettingsManager.PoliceSettings.GeneralFineAmount;
            if (PoliceResponse.PlayerSeenInVehicleDuringWanted)
            {
                if (!Licenses.HasValidDriversLicense(TimeControllable))
                {
                    InitialAmount += Settings.SettingsManager.PoliceSettings.DrivingWithoutLicenseFineAmount;
                }
            }
            if(Respawning.TimesTalked > 0)
            {
                InitialAmount += Settings.SettingsManager.PoliceSettings.TalkFailFineAmount;
            }
            return InitialAmount;
        }
        public void SetDenStatus(Gang gang, bool isEnabled)
        {
           // EntryPoint.WriteToConsole($"SET DEN {gang.ShortName} {isEnabled}");
            World.Places.StaticPlaces.SetGangLocationActive(gang.ID, isEnabled);
        }
        public void SetAngeredCop()
        {
            GameTimeLastFedUpCop = Game.GameTime;
        }
        public void ShootAt(Vector3 TargetCoordinate)
        {
            NativeFunction.CallByName<bool>("SET_PED_SHOOTS_AT_COORD", Game.LocalPlayer.Character, TargetCoordinate.X, TargetCoordinate.Y, TargetCoordinate.Z, true);
            GameTimeLastShot = Game.GameTime;
        }
        public void SetShot()
        {
            GameTimeLastShot = Game.GameTime;
        }
        public void SetAgencyStatus(Agency toassign)
        {
            if(toassign == null)
            {
                return;
            }
            if(toassign.ResponseType == ResponseType.LawEnforcement)
            {
                Cop meAsCop = new Cop(Character, Settings, Character.MaxHealth, toassign, true, Crimes, Weapons, PlayerName, ModelName, World);
                meAsCop.CanBeTasked = false;
                meAsCop.CanBeAmbientTasked = false;
                meAsCop.AutoCallsInUnconsciousPeds = false;
                meAsCop.IsPlayerControlled = true;
                World.Pedestrians.AddEntity(meAsCop);
                AssignedAgency = toassign;
                IsCop = true;
                IsEMT = false;
                IsFireFighter = false;
                IsSecurityGuard = false;
                EntryPoint.WriteToConsole($"Assigned Player As Cop {toassign.ShortName}");
            }
            else if (toassign.ResponseType == ResponseType.EMS)
            {
                IsCop = false;
                IsEMT = true;
                IsFireFighter = false;
                IsSecurityGuard = false;
                AssignedAgency = toassign;
                EntryPoint.WriteToConsole($"Assigned Player As EMT {toassign.ShortName}");
            }
            else if (toassign.ResponseType == ResponseType.Fire)
            {
                IsCop = false;
                IsEMT = false;
                IsFireFighter = true;
                IsSecurityGuard = false;
                AssignedAgency = toassign;
                EntryPoint.WriteToConsole($"Assigned Player As Firefighter {toassign.ShortName}");
            }
            else if (toassign.ResponseType == ResponseType.Security)
            {
                SecurityGuard meAsSecurity = new SecurityGuard(Character, Settings, Character.MaxHealth, toassign, true, Crimes, Weapons, PlayerName, ModelName, World);
                meAsSecurity.CanBeTasked = false;
                meAsSecurity.CanBeAmbientTasked = false;
                meAsSecurity.AutoCallsInUnconsciousPeds = false;
                meAsSecurity.IsPlayerControlled = true;
                World.Pedestrians.AddEntity(meAsSecurity);
                IsCop = false;
                IsEMT = false;
                IsFireFighter = false;
                IsSecurityGuard = true;
                AssignedAgency = toassign;
                EntryPoint.WriteToConsole($"Assigned Player As Security Guard {toassign.ShortName}");
            }
        }
        public void RemoveAgencyStatus()
        {
            AssignedAgency = null;
            World.Pedestrians.PoliceList.RemoveAll(x => x.Handle == Handle);

            IsCop = false;
            IsEMT = false;
            IsFireFighter = false;
            EntryPoint.WriteToConsole($"Removed Player as Agency");
        }
        public void ToggleCopTaskable()
        {
            Cop meCop = World.Pedestrians.Police.FirstOrDefault(x => x.Handle == Handle);
            if(meCop != null)
            {
                meCop.CanBeAmbientTasked = !meCop.CanBeAmbientTasked;
                meCop.CanBeTasked = !meCop.CanBeTasked;
                Game.DisplaySubtitle($"Player Cop Taskable: {meCop.CanBeTasked}");
               //EntryPoint.WriteToConsoleTestLong($"Player Cop Taskable: {meCop.CanBeTasked}");
            }
        }
        public void ShowVehicleInteractMenu(bool showDefault)
        {
            if(InterestedVehicle == null || !InterestedVehicle.Vehicle.Exists() || (!InterestedVehicle.HasBeenEnteredByPlayer && !InterestedVehicle.IsOwnedByPlayer))
            {
                return;
            }
            VehicleDoorSeatData vdsd = null;
            if (!IsInVehicle)
            {
                vdsd = InterestedVehicle.GetClosestPedStorageBone(this, 7f, VehicleSeatDoorData);
            }
            if(InterestedVehicle.VehicleInteractionMenu.IsShowingMenu)
            {
                //EntryPoint.WriteToConsole("InterestedVehicle.VehicleInteractionMenu.IsShowingMenu");
                return;
            }
            InterestedVehicle.VehicleInteractionMenu.ShowInteractionMenu(this, Weapons, ModItems, vdsd, VehicleSeatDoorData, World, Settings, showDefault, PlacesOfInterest, TimeControllable);
        }
        public void ToggleAutoBackup()
        {
            AutoDispatch = !AutoDispatch;
            Game.DisplayHelp($"AutoDispatch {(AutoDispatch ? "Enabled" : "Disabled")}");
        }
        //Events
        public void OnAppliedWantedStats(int wantedLevel) => Scanner.OnAppliedWantedStats(wantedLevel);
        public void OnGotOffFreeway()
        {
            GameFiber.Yield();
            if (IsWanted && AnyPoliceCanSeePlayer && TimeInCurrentVehicle >= 10000 && IsAliveAndFree)
            {
                Scanner.OnGotOffFreeway();
            }
            //EntryPoint.WriteToConsole($"PLAYER EVENT: OnGotOffFreeway (5 Second Delay)");
        }
        public void OnGotOnFreeway()
        {
            GameFiber.Yield();
            if (IsWanted && AnyPoliceCanSeePlayer && TimeInCurrentVehicle >= 10000 && IsAliveAndFree)
            {
                Scanner.OnGotOnFreeway();
            }
            //EntryPoint.WriteToConsole($"PLAYER EVENT: OnGotOnFreeway (5 Second Delay)");
        }
        public void OnWentInTunnel()
        {
            GameFiber.Yield();
            if (IsWanted && AnyPoliceCanSeePlayer && IsAliveAndFree)
            {
                Scanner.OnWentInTunnel();
            }
            EntryPoint.WriteToConsole($"PLAYER EVENT: Went In Tunnel (5 Second Delay)");
        }
        public void OnInvestigationExpire()
        {
            GameFiber.Yield();
            PoliceResponse.Reset();
            Scanner.OnInvestigationExpire();
            //EntryPoint.WriteToConsole($"PLAYER EVENT: OnInvestigationExpire");
        }
        public void OnKilledCop()
        {
            PlayerVoice.OnKilledCop();
        }
        public void OnKilledCivilian()
        {
            PlayerVoice.OnKilledCivilian();
        }
        public void OnLawEnforcementSpawn(Agency agency, DispatchableVehicle vehicleType, DispatchablePerson officerType)
        {
            GameFiber.Yield();
            if (IsWanted)
            {
                if (agency?.ID == "ARMY")
                {
                    Scanner.OnArmyDeployed();
                }
                else if (agency?.ID == "NOOSE")
                {
                    Scanner.OnNooseDeployed();
                }
                else if (agency?.ID == "FIB" && WantedLevel >= 4)
                {
                    Scanner.OnFIBHETDeployed();
                }
                if (vehicleType?.IsHelicopter == true)
                {
                    Scanner.OnHelicoptersDeployed();
                    Dispatcher.LEDispatcher.OnHelicopterSpawnedOrRecalled();
                }
                if(officerType?.IsAnimal == true)
                {
                    Dispatcher.LEDispatcher.OnK9SpawnedOrRecalled();
                }
            }
        }
        public void OnLethalForceAuthorized() => Scanner.OnLethalForceAuthorized();
        public void OnPoliceNoticeVehicleChange() => Scanner.OnPoliceNoticeVehicleChange();
        public void OnRequestedBackUp() => Scanner.OnRequestedBackUp();
        public void OnSuspectEluded()//runs before OnWantedLevelChanged
        {
            GameFiber.Yield();
            if (WantedLevel > 1)
            {
                CriminalHistory.OnSuspectEluded(PoliceResponse.CrimesObserved.ToList(), PlacePoliceLastSeenPlayer);
                Scanner.OnSuspectEluded();
            }
            else if(WantedLevel == 1)
            {
                Scanner.Reset();
            }
            PlayerVoice.OnSuspectEluded();
        }
        public void OnVehicleCrashed()
        {
            GameFiber.Yield();
            if (IsWanted && AnyPoliceRecentlySeenPlayer && IsInVehicle && TimeInCurrentVehicle >= 5000 && IsAliveAndFree)
            {
                GameTimeLastCrashedVehicle = Game.GameTime;
                Scanner.OnVehicleCrashed();
            }

            PlayerVoice.OnCrashedCar();

            EntryPoint.WriteToConsole($"PLAYER EVENT: OnVehicleCrashed");
        }
        public void OnVehicleEngineHealthDecreased(float amount, bool isCollision)
        {
            GameFiber.Yield();
            if (IsInVehicle && amount >= 25f && TimeInCurrentVehicle >= 5000)
            {
                if (IsWanted && AnyPoliceRecentlySeenPlayer) //if (isCollision && IsWanted && AnyPoliceRecentlySeenPlayer)
                {
                    GameFiber.Yield();
                    EntryPoint.WriteToConsole($"PLAYER EVENT: OnVehicleEngineHealthDecreased SCANNER CALLED");
                    Scanner.OnVehicleCrashed();
                }
                CurrentVehicle?.VehicleBodyManager.OnVehicleCrashed();
                GameTimeLastCrashedVehicle = Game.GameTime;
            }
            EntryPoint.WriteToConsole($"PLAYER EVENT: OnVehicleEngineHealthDecreased {amount} {isCollision}");
        }
        public void OnVehicleHealthDecreased(int amount, bool isCollision)
        {
            GameFiber.Yield();
            if (IsInVehicle && amount >= 25 && TimeInCurrentVehicle >= 5000)
            {
                if (IsWanted && AnyPoliceRecentlySeenPlayer) //if (isCollision && IsWanted && AnyPoliceRecentlySeenPlayer)
                {
                    GameFiber.Yield();
                    EntryPoint.WriteToConsole($"PLAYER EVENT: OnVehicleHealthDecreased SCANNER CALLED");
                    Scanner.OnVehicleCrashed();
                }
                CurrentVehicle?.VehicleBodyManager.OnVehicleCrashed();
                GameTimeLastCrashedVehicle = Game.GameTime;
            }
            if (Settings.SettingsManager.VehicleSettings.InjureOnVehicleCrash && (amount >= Settings.SettingsManager.VehicleSettings.VehicleCrashInjureMinVehicleDamageTrigger) && IsInVehicle)
            {
                float HealthToRemove = amount * Settings.SettingsManager.VehicleSettings.VehicleCrashInjureScalar;
                int healthToRemove = (int)Math.Ceiling(HealthToRemove);
                HealthManager.ChangeHealth(-1 * healthToRemove);
                HealthState.SimpleRefresh(this);
                EntryPoint.WriteToConsole($"PLAYER EVENT: REMOVING HEALTH IN CRASH DamageAmount:{amount} healthToRemove:{healthToRemove}");
            }
            EntryPoint.WriteToConsole($"PLAYER EVENT: OnVehicleHealthDecreased {amount} {isCollision}");
        }
        public void OnVehicleStartedFire()
        {
            GameFiber.Yield();
            if (IsWanted && AnyPoliceRecentlySeenPlayer && IsInVehicle && TimeInCurrentVehicle >= 5000 && IsAliveAndFree)
            {
                Scanner.OnVehicleStartedFire();
            }
            //EntryPoint.WriteToConsole($"PLAYER EVENT: OnVehicleStartedFire", 5);
        }
        public void OnWantedActiveMode()
        {
            Scanner.OnWantedActiveMode();
            PlayerVoice.OnWantedActiveMode();
        }
        public void OnWantedSearchMode()
        {
            Scanner.OnWantedSearchMode();
            PlayerVoice.OnWantedSearchMode();
        }
        public void OnWeaponsFree() => Scanner.OnWeaponsFree();
        public void OnHitSquadDispatched(Gang enemyGang)
        {

            if (enemyGang == null)
            {
                return;
            }
            if (Settings.SettingsManager.GangSettings.SendHitSquadText)
            {
                PlayerTasks.GangTasks.SendHitSquadMessage(enemyGang.Contact);
                EntryPoint.WriteToConsole($"OnHitSquadDispatched SendHitSquadMessage {enemyGang.ShortName}");
            }
        }



        public void OnMarshalsDispatched(Agency deployedAgency)
        {
            if(!Settings.SettingsManager.PoliceSettings.SendMarshalsAPBResponseText)
            {
                return;
            }
            PhoneContact tosend = CellPhone.GetCorruptCopContact();
            if(tosend == null)
            {
                return;
            }
            List<string> Replies = new List<string>() {
                                $"Letting you know they got some feds looking for you.",
                                $"Feds on the prowl, watch your back.",
                                $"I got words the feds are out to get you, be careful.",
                                $"Guess you made someone really mad, the feds are looking for you.",
                                $"Fed task force on the way to your position.",
                                $"The feds are en route, get outta there.",
                                $"Got wind of a task force being deployed to find you and its the feds.",

                                };
            CellPhone.AddScheduledText(tosend, Replies.PickRandom(), 0, true);
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
                if (WeaponEquipment.CurrentWeapon == null)
                {
                    IsMakingInsultingGesture = true;
                }
                else
                {
                    //EntryPoint.WriteToConsole($"CurrentWeapon {WeaponEquipment.CurrentWeapon.ModelName}");
                }
            }
            else
            {
                IsMakingInsultingGesture = false;
            }
            //EntryPoint.WriteToConsole($"PLAYER EVENT: IsAimingInVehicle Changed to: {IsAimingInVehicle}", 5);
        }
        private void OnExcessiveSpeed()
        {
            GameFiber.Yield();
            if (IsWanted && VehicleSpeedMPH >= 75f && AnyPoliceCanSeePlayer && TimeInCurrentVehicle >= 10000 && !isCheckingExcessSpeed && IsAliveAndFree)
            {
                GameFiber SpeedWatcher = GameFiber.StartNew(delegate
                {
                    try
                    {
                        isCheckingExcessSpeed = true;
                        GameFiber.Sleep(5000);
                        if (isExcessiveSpeed)
                        {
                            Scanner.OnExcessiveSpeed();
                        }
                        isCheckingExcessSpeed = false;
                    }
                    catch (Exception ex)
                    {
                        EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                        //EntryPoint.ModController.CrashUnload();
                    }
                }, "FastForwardWatcher");
            }
            //EntryPoint.WriteToConsole($"PLAYER EVENT: OnExcessiveSpeed");
        }
        private void OnGettingIntoAVehicleChanged()
        {
            if (IsGettingIntoAVehicle)
            {
                EntryPoint.WriteToConsole("IsGettingIntoAVehicle CHANGED TO TRUE");
                VehicleTryingToEnter = Game.LocalPlayer.Character.VehicleTryingToEnter;
                SeatTryingToEnter = Game.LocalPlayer.Character.SeatIndexTryingToEnter;
                if (VehicleTryingToEnter != null)
                {
                    UpdateCurrentVehicle();
                    HandleVehicleEntry();
                }
            }
            isGettingIntoVehicle = IsGettingIntoAVehicle;
            //EntryPoint.WriteToConsole($"PLAYER EVENT: IsGettingIntoVehicleChanged to {IsGettingIntoAVehicle}, HoldingEnter {IsNotHoldingEnter}");
        }
        private void HandleVehicleEntry()
        {
            if(CurrentVehicle == null)
            {
                EntryPoint.WriteToConsole($"PLAYER EVENT: IsGettingIntoVehicle ERROR VEHICLE NOT FOUND (ARE YOU SCANNING ENOUGH?)", 3);
                return;
            }
            EntryPoint.WriteToConsole("HandleVehicleEntry STARTED");
            VehicleGettingInto = CurrentVehicle;
            bool isFreeToEnter = IsFreeToEnter();
            if (isFreeToEnter)//IsFreeToEnter())
            {
                EntryPoint.WriteToConsole($"PLAYER EVENT: IsGettingIntoVehicle Vehicle is Free to Enter, Ending", 3);
                //return;
            }
            if (!(isFreeToEnter || (CurrentVehicle.IsService && IsServicePed)))
            {
                EntryPoint.WriteToConsole("HandleVehicleEntry ATTEMPTING TO LOCK");
                CurrentVehicle.AttemptToLock();
            }
            HandleScrewdriver();
            if ((IsNotHoldingEnter || ActivityManager.HasScrewdriverInHand) && VehicleTryingToEnter.Driver == null && VehicleTryingToEnter.LockStatus == (VehicleLockStatus)7 && (!Settings.SettingsManager.VehicleSettings.RequireScrewdriverForLockPickEntry || currentlyHasScrewdriver))//no driver && Unlocked
            {
                EntryPoint.WriteToConsole($"PLAYER EVENT: LockPick Start", 3);
                CarLockPick MyLockPick = new CarLockPick(this, VehicleTryingToEnter, SeatTryingToEnter, ActivityManager.CurrentScrewdriver);
                MyLockPick.PickLock();
            }
            else if (IsNotHoldingEnter && SeatTryingToEnter == -1 && VehicleTryingToEnter.Driver != null && VehicleTryingToEnter.Driver.IsAlive) //Driver
            {
                EntryPoint.WriteToConsole($"PLAYER EVENT: CarJack Start", 3);
                PedExt jackedPed = World.Pedestrians.GetPedExt(VehicleTryingToEnter.Driver.Handle);
                Violations.TheftViolations.AddCarJacked(jackedPed);
                CarJack MyJack = new CarJack(this, CurrentVehicle, jackedPed, SeatTryingToEnter, WeaponEquipment.CurrentWeapon);
                MyJack.Start();
            }
            else if (VehicleTryingToEnter.LockStatus == (VehicleLockStatus)7 && CurrentVehicle.IsCar)
            {
                EntryPoint.WriteToConsole($"PLAYER EVENT: Car Break-In Start LockStatus {VehicleTryingToEnter.LockStatus}", 3);
                CarBreakIn MyBreakIn = new CarBreakIn(this, VehicleTryingToEnter, Settings, SeatTryingToEnter);
                MyBreakIn.BreakIn();
            }
        }
        private bool IsFreeToEnter()
        {
            if(CurrentVehicle.HasBeenEnteredByPlayer || CurrentVehicle.IsAlwaysOpenForPlayer)
            {
                CurrentVehicle.Vehicle.LockStatus = (VehicleLockStatus)1;
                CurrentVehicle.Vehicle.MustBeHotwired = false;
                return true;
            }
            if (CurrentVehicle.Vehicle.Exists() && NativeFunction.Natives.IS_TURRET_SEAT<bool>(VehicleTryingToEnter, SeatTryingToEnter))
            {
                EntryPoint.WriteToConsole($"YOU ARE GETTING INTO A TURRENT, NOT DOING LOCKPICK SeatTryingToEnter {SeatTryingToEnter}");
                return true;
            }
            else if (!Settings.SettingsManager.VehicleSettings.AllowLockVehicles)
            {
                CurrentVehicle.Vehicle.LockStatus = (VehicleLockStatus)1;
                CurrentVehicle.Vehicle.MustBeHotwired = false;
                return true;
            }

            else if (VehicleOwnership.OwnedVehicles.Any(x => CurrentVehicle.Vehicle.Exists() && x.Handle == CurrentVehicle.Handle))
            {
                CurrentVehicle.Vehicle.LockStatus = (VehicleLockStatus)1;
                CurrentVehicle.Vehicle.MustBeHotwired = false;
                return true;
            }
            else if (CurrentVehicle.Vehicle.Exists() && LastFriendlyVehicle.Exists() && CurrentVehicle.Vehicle.Handle == LastFriendlyVehicle.Handle)
            {
                CurrentVehicle.Vehicle.LockStatus = (VehicleLockStatus)1;
                CurrentVehicle.Vehicle.MustBeHotwired = false;
                return true;
            }
            else if (CurrentVehicle.IsGang && CurrentVehicle.AssociatedGang != null && RelationshipManager.GangRelationships.CurrentGang != null && CurrentVehicle.AssociatedGang.ID == RelationshipManager.GangRelationships.CurrentGang.ID)
            {
                CurrentVehicle.Vehicle.LockStatus = (VehicleLockStatus)1;
                CurrentVehicle.Vehicle.MustBeHotwired = false;
                CurrentVehicle.IsStolen = false;
                CurrentVehicle.CanBeConsideredStolen = false;
                return true;
            }
            else if (CurrentVehicle.IsPolice && IsCop)
            {
                CurrentVehicle.Vehicle.LockStatus = (VehicleLockStatus)1;
                CurrentVehicle.Vehicle.MustBeHotwired = false;
                CurrentVehicle.IsStolen = false;
                CurrentVehicle.CanBeConsideredStolen = false;
                return true;
            }
            else if (CurrentVehicle.IsFreeEntry)
            {
                CurrentVehicle.Vehicle.LockStatus = (VehicleLockStatus)1;
                CurrentVehicle.Vehicle.MustBeHotwired = false;
                return true;
            }
            else if (CurrentVehicle.WasModSpawned && (CurrentVehicle.IsService || CurrentVehicle.IsGang) && CurrentVehicle.Vehicle.Exists())//maybe unlock friendly gang vehicles?maybe not
            {
                return false;
            }
            else if (!CurrentVehicle.WasModSpawned && !Settings.SettingsManager.VehicleSettings.AllowLockMissionVehicles && CurrentVehicle.Vehicle.Exists() && CurrentVehicle.Vehicle.IsPersistent)
            {
                CurrentVehicle.Vehicle.LockStatus = (VehicleLockStatus)1;
                CurrentVehicle.Vehicle.MustBeHotwired = false;
                return true;
            }
            else if (CurrentVehicle.Vehicle.Exists() && !CurrentVehicle.IsRandomlyLocked)
            {
                CurrentVehicle.Vehicle.LockStatus = (VehicleLockStatus)1;
                CurrentVehicle.Vehicle.MustBeHotwired = false;
                return true;
            }
            return false;
        }
        private void HandleScrewdriver()
        {
            EntryPoint.WriteToConsole("HandleScrewdriver STARTED");
            currentlyHasScrewdriver = ActivityManager.HasScrewdriverInHand || Inventory.Has(typeof(ScrewdriverItem)); //Inventory.HasTool(ToolTypes.Screwdriver);
            if (Settings.SettingsManager.VehicleSettings.RequireScrewdriverForHotwire)
            {
                if (CurrentVehicle.Vehicle.MustBeHotwired)
                {
                    CurrentVehicle.IsHotWireLocked = true;
                    CurrentVehicle.Vehicle.MustBeHotwired = false;
                    EntryPoint.WriteToConsole("HandleScrewdriver SETTING MUST BE HOTWIRED 1");
                }
                if (Settings.SettingsManager.VehicleSettings.AutoHotwire && CurrentVehicle.IsHotWireLocked && currentlyHasScrewdriver)
                {
                    CurrentVehicle.IsHotWireLocked = false;
                    CurrentVehicle.Vehicle.MustBeHotwired = true;
                    EntryPoint.WriteToConsole("HandleScrewdriver CLEARING AS YOU HAVE A SCREWDRIVER AND AUTO HOTWIRE IS ON MUST BE HOTWIRED 1");
                }
            }
            else if(!Settings.SettingsManager.VehicleSettings.AutoHotwire)
            {
                if (CurrentVehicle.Vehicle.MustBeHotwired)
                {
                    CurrentVehicle.IsHotWireLocked = true;
                    CurrentVehicle.Vehicle.MustBeHotwired = false;
                    EntryPoint.WriteToConsole("HandleScrewdriver SETTING MUST BE HOTWIRED WITH AUTO HOTWIRE ONLY!");
                }
            }
            if (Settings.SettingsManager.VehicleSettings.RequireScrewdriverForLockPickEntry && !currentlyHasScrewdriver && IsNotHoldingEnter && VehicleTryingToEnter.Driver == null && VehicleTryingToEnter.LockStatus == (VehicleLockStatus)7 && !VehicleTryingToEnter.IsEngineOn)
            {
                Game.DisplayHelp("Screwdriver required to lockpick");
            }
        }
        private void OnIsInVehicleChanged()
        {
            GameFiber.Yield();
            if (IsInVehicle)
            {
                GameTimeGotInVehicle = Game.GameTime;
                GameTimeGotOutOfVehicle = 0;
                if (IsWanted && AnyPoliceCanSeePlayer && IsAliveAndFree)
                {
                    Scanner.OnGotInVehicle();
                }
                //RemoveOwnedVehicleBlip();
                if (CurrentVehicle != null)
                {
                    CurrentVehicle.HasAutoSetRadio = false;
                }
                EntryPoint.WriteToConsole("OnIsInVehicleChanged CHANGED TO TRUE");
            }
            else
            {
                GameTimeGotOutOfVehicle = Game.GameTime;
                GameTimeGotInVehicle = 0;
                if (IsWanted && AnyPoliceCanSeePlayer && !IsRagdoll && IsAliveAndFree)
                {
                    Scanner.OnGotOutOfVehicle();
                }
                //CreateOwnedVehicleBlip();

                if (!Settings.SettingsManager.PlayerOtherSettings.AllowMobileRadioOnFoot && IsMobileRadioEnabled && !ActivityManager.IsDancing)
                {
                    IsMobileRadioEnabled = false;
                    NativeFunction.CallByName<bool>("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", false);
                }
                TaxiManager.OnGotOutOfVehicle();
                Violations.MinorViolations.OnGotOutOfVehicle();
                EntryPoint.WriteToConsole("OnIsInVehicleChanged CHANGED TO FALSE");
                // NativeFunction.Natives.SET_PED_CONFIG_FLAG<bool>(Character, 313, false);
            }
            //UpdateOwnedBlips();
            //EntryPoint.WriteToConsole($"PLAYER EVENT: IsInVehicle to {IsInVehicle}");
        }
        private void OnPlayerBusted()
        {
            GameFiber.Yield();
            DiedInVehicle = IsInVehicle;
            IsBusted = true;
            BeingArrested = true;
            GameTimeLastBusted = Game.GameTime;
            WasDangerouslyArmedWhenBusted = IsDangerouslyArmed;
            Surrendering.OnPlayerBusted();
            Respawning.OnPlayerBusted();
            ActivityManager.OnPlayerBusted();
            if (Settings.SettingsManager.PlayerOtherSettings.SetSlowMoOnBusted)
            {
                Game.TimeScale = Settings.SettingsManager.PlayerOtherSettings.SlowMoOnBustedSpeed;// 0.4f;
            }
            //NativeHelper.DisablePlayerControl();
            //Game.LocalPlayer.HasControl = false;
            Scanner.OnPlayerBusted();
            //EntryPoint.WriteToConsole($"PLAYER EVENT: IsBusted Changed to: {IsBusted}");
        }
        private void OnPlayerDied()
        {
            GameFiber.Yield();
            TimeControllable.PauseTime();
            DiedInVehicle = IsInVehicle;
            IsDead = true;
            GameTimeLastDied = Game.GameTime;
            Game.LocalPlayer.Character.Kill();
            Game.LocalPlayer.Character.Health = 0;
            Game.LocalPlayer.Character.IsInvincible = true;

            if (Settings.SettingsManager.PlayerOtherSettings.SetCutToBlackDeath)
            {
                Game.FadeScreenOut(0, false);
            }


            if (Settings.SettingsManager.PlayerOtherSettings.SetSlowMoOnDeath)
            {
                Game.TimeScale = Settings.SettingsManager.PlayerOtherSettings.SlowMoOnDeathSpeed;// 0.4f;
            }
            Scanner.OnSuspectWasted();
            ActivityManager.OnPlayerDied();
            //GetKillingPed();
            //EntryPoint.WriteToConsole($"PLAYER EVENT: IsDead Changed to: {IsDead}");
        }

        public void GetKillingPed()
        {
            try
            {
                KillerHandle = NativeFunction.Natives.GetPedSourceOfDeath<uint>(Game.LocalPlayer.Character);
                EntryPoint.WriteToConsole($"PLAYER killed by {KillerHandle}");
                if(KillerHandle == 0)
                {
                    PedLastKilledPlayer = null;
                    EntryPoint.WriteToConsole($"COULD NOT FIND PLAYER KILLER HANDLE");
                    return;
                }
                PedLastKilledPlayer = World.Pedestrians.GetPedExt(KillerHandle);
                if(PedLastKilledPlayer == null)
                {
                    EntryPoint.WriteToConsole($"COULD NOT FIND PLAYER KILLER PEDEXT");
                    return;
                }
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole($"PLAYER LOG KILLER ERROR {ex.Message} {ex.StackTrace}", 5);
            }
        }

        private void OnIsShootingChanged()
        {
            if (IsShooting)
            {
                if (IsWanted && WantedLevel <= 4 && AnyPoliceRecentlySeenPlayer)
                {
                    Scanner.OnSuspectShooting();
                }
                PlayerVoice.OnShotGun();
                //EntryPoint.WriteToConsoleTestLong("PLAYER EVENT: Starting Shooting");
            }
            else
            {
                //EntryPoint.WriteToConsoleTestLong("PLAYER EVENT: Stopped Shooting");
            }
        }
        private void OnStartedDuckingInVehicle()
        {
            if (Settings.SettingsManager.VehicleSettings.ForceFirstPersonOnVehicleDuck)
            {
                int viewMode = NativeFunction.Natives.GET_FOLLOW_VEHICLE_CAM_VIEW_MODE<int>();
                if (viewMode != 4)
                {
                    storedViewMode = viewMode;
                    NativeFunction.Natives.SET_FOLLOW_VEHICLE_CAM_VIEW_MODE(4);
                }
               // EntryPoint.WriteToConsole($"OnStartedDuckingInVehicle viewMode {viewMode} storedViewMode {storedViewMode}", 5);
            }
        }
        private void OnStoppedDuckingInVehicle()
        {
            if (Settings.SettingsManager.VehicleSettings.ForceFirstPersonOnVehicleDuck)
            {
                int viewMode = NativeFunction.Natives.GET_FOLLOW_VEHICLE_CAM_VIEW_MODE<int>();
                if (viewMode != storedViewMode)
                {
                    NativeFunction.Natives.SET_FOLLOW_VEHICLE_CAM_VIEW_MODE(storedViewMode);
                    storedViewMode = -1;
                }
               // EntryPoint.WriteToConsole($"OnStoppedDuckingInVehicle storedViewMode {storedViewMode}", 5);
            }
        }
        private void OnTargettingHandleChanged()
        {
            if (TargettingHandle != 0)
            {
                CurrentTargetedPed = World.Pedestrians.GetPedExt(TargettingHandle);
                GameFiber.Yield();
               // ActivityManager.OnTargetHandleChanged();
            }
            else
            {
                CurrentTargetedPed = null;
            }
            ActivityManager.OnTargetHandleChanged();
            //EntryPoint.WriteToConsole($"PLAYER EVENT: CurrentTargetedPed to {CurrentTargetedPed?.Pedestrian?.Handle} CanHoldUpTargettedPed {CanHoldUpTargettedPed} CurrentTargetedPed?.CanBeMugged {CurrentTargetedPed?.CanBeMugged}", 5);
        }
        private void OnWantedLevelChanged()//runs after OnSuspectEluded (If Applicable)
        {
            GameFiber.Yield();
            if (IsNotWanted && PreviousWantedLevel != 0)//Lost Wanted
            {
                if (!RecentlySetWanted)//only allow my process to set the wanted level
                {
                    //if (Settings.SettingsManager.PoliceSettings.TakeExclusiveControlOverWantedLevel)
                    //{
                    //    EntryPoint.WriteToConsole($"PLAYER EVENT: GAME AUTO SET WANTED TO {WantedLevel}, RESETTING TO {PreviousWantedLevel}", 3);
                    //    SetWantedLevel(PreviousWantedLevel, "GAME AUTO SET WANTED", true);
                    //}
                }
                else
                {
                    CriminalHistory.OnLostWanted();
                    GameFiber.Yield();
                    PoliceResponse.OnLostWanted();
                    GameFiber.Yield();
                    PlayerVoice.OnLostWanted();
                    GameFiber.Yield();//TR 05
                    RelationshipManager.GangRelationships.OnLostWanted();
                    GameFiber.Yield();//TR 05
                    World.Pedestrians.CivilianList.ForEach(x => x.PlayerCrimesWitnessed.Clear());
                   // EntryPoint.WriteToConsole($"PLAYER EVENT: LOST WANTED", 3);
                }
            }
            else if (IsWanted && PreviousWantedLevel == 0)//Added Wanted Level
            {
                if (!RecentlySetWanted)//only allow my process to set the wanted level
                {
                    if (Settings.SettingsManager.PoliceSettings.TakeExclusiveControlOverWantedLevel)
                    {
                        EntryPoint.WriteToConsole($"PLAYER EVENT: GAME AUTO SET WANTED TO {WantedLevel}, RESETTING", 3);
                        SetWantedLevel(0, "GAME AUTO SET WANTED", true);
                    }
                }
                else
                {
                    Investigation.Reset();
                    GameFiber.Yield();
                    PoliceResponse.OnBecameWanted();
                    GameFiber.Yield();
                    PlayerVoice.OnBecameWanted();
                    GameFiber.Yield();//TR 05
                    RelationshipManager.GangRelationships.OnBecameWanted();
                   // EntryPoint.WriteToConsole($"PLAYER EVENT: BECAME WANTED", 3);
                }
            }
            else if (IsWanted && PreviousWantedLevel < WantedLevel)//Increased Wanted Level (can't decrease only remove for now.......)
            {
                PoliceResponse.OnWantedLevelIncreased();
                EntryPoint.WriteToConsole($"PLAYER EVENT: WANTED LEVEL INCREASED", 3);
            }
            else if (IsWanted && PreviousWantedLevel > WantedLevel)
            {
                EntryPoint.WriteToConsole($"PLAYER EVENT: WANTED LEVEL DECREASED", 3);
            }
             EntryPoint.WriteToConsole($"Wanted Changed: {WantedLevel} Previous: {PreviousWantedLevel}", 3);
            PreviousWantedLevel = wantedLevel;// NativeFunction.Natives.GET_FAKE_WANTED_LEVEL<int>();//PreviousWantedLevel = Game.LocalPlayer.WantedLevel;
        }
        private void OnCurrentVehicleChanged()
        {
            if (CurrentVehicle != null)
            {
                TaxiManager.OnGotInVehicle();
            }
            else
            {

            }
        }

        //Crimes
        public void AddCrime(Crime crimeObserved, bool isObservedByPolice, Vector3 Location, VehicleExt VehicleObserved, WeaponInformation WeaponObserved, bool HaveDescription, bool AnnounceCrime, bool isForPlayer)
        {
            if(crimeObserved == null)
            {
                return;
            }
            if (RecentlyBribedPolice && crimeObserved.ResultingWantedLevel <= 2)
            {
                return;
            }
            else if (RecentlyPaidFine && crimeObserved.ResultingWantedLevel <= 1)
            {
                return;
            }
            else if (RecentlyStartedPlaying)
            {
                return;
            }
            GameFiber.Yield();//TR 6 this is new, seems helpful so far with no downsides
            CrimeSceneDescription description = new CrimeSceneDescription(!IsInVehicle, isObservedByPolice, Location, HaveDescription) { InteriorSeen = isForPlayer ? CurrentLocation.CurrentInterior : null, VehicleSeen = VehicleObserved, WeaponSeen = WeaponObserved, Speed = Game.LocalPlayer.Character.Speed };
            PoliceResponse.AddCrime(crimeObserved, description, isForPlayer);
            if (!isObservedByPolice && IsNotWanted)
            {
                Investigation.Start(Location, PoliceResponse.PoliceHaveDescription, true, false, false, isForPlayer ? CurrentLocation.CurrentInterior : null);
            }
            if (AnnounceCrime)
            {
                Scanner.AnnounceCrime(crimeObserved, description);
            }
        }
        public void AddMedicalEvent(Vector3 position)
        {
            if (Settings.SettingsManager.EMSSettings.ManageDispatching && Settings.SettingsManager.EMSSettings.ManageTasking && World.TotalWantedLevel <= 1 && World.Pedestrians.PedExts.Any(x => (x.IsUnconscious || x.IsInWrithe) && !x.IsDead && !x.HasStartedEMTTreatment))
            {
                //Scanner.Reset();
                Investigation.Start(position, false, false, true, false);
                Scanner.OnMedicalServicesRequested();
            }
        }
        public void AddOfficerMIACall(Vector3 position)
        {
            Crime crimeObserved = Crimes.GetCrime(StaticStrings.KillingPoliceCrimeID);
            CrimeSceneDescription description = new CrimeSceneDescription(!IsInVehicle, false, position, false);
            PoliceResponse.AddCrime(crimeObserved, description, false);
            Investigation.Start(position, false, true, false, false);
            Scanner.OnOfficerMIA();
        }
        public void OnSeenInRestrictedAreaOnCamera(bool isSevere)
        {
            if(Violations.CanEnterRestrictedAreas)
            {
                return;
            }
            Crime crimeObserved = Crimes.GetCrime(StaticStrings.TrespessingCrimeID);
            if(isSevere)
            {
                crimeObserved = Crimes.GetCrime(StaticStrings.CivilianTrespessingCrimeID);
            }
            CrimeSceneDescription description = new CrimeSceneDescription(!IsInVehicle, false, Position, true);
            PoliceResponse.AddCrime(crimeObserved, description, false);
            Investigation.Start(Position, true, true, false, false, CurrentLocation.CurrentInterior);
            Scanner.AnnounceCrime(crimeObserved, description);
            EntryPoint.WriteToConsole("OnSeenInRestrictedAreaOnCamera");
        }
        public void Arrest()
        {
            BeingArrested = true;
            if (!IsBusted)
            {
                OnPlayerBusted();
            }
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
                    //if (Settings.SettingsManager.PoliceSettings.UseFakeWantedLevelSystem)
                    //{
                    //    NativeFunction.CallByName<bool>("SET_MAX_WANTED_LEVEL", 0);
                    //    if (Settings.SettingsManager.UIGeneralSettings.ShowFakeWantedLevelStars && desiredWantedLevel <= 6)
                    //    {
                    //        NativeFunction.Natives.SET_FAKE_WANTED_LEVEL(desiredWantedLevel);
                    //    }
                    //}
                    //else
                    //{
                    //    NativeFunction.CallByName<bool>("SET_MAX_WANTED_LEVEL", desiredWantedLevel);
                    //    Game.LocalPlayer.WantedLevel = desiredWantedLevel;
                    //}
                    wantedLevel = desiredWantedLevel;
                    if (desiredWantedLevel > 0)
                    {
                        GameTimeWantedLevelStarted = Game.GameTime;
                    }
                    OnWantedLevelChanged();
                    EntryPoint.WriteToConsole($"Set Wanted: From {WantedLevel} to {desiredWantedLevel} Reason: {Reason}", 3);
                }
            }
        }
        //Updates
        public void UpdateStateData()
        {
            UpdateGeneralStatus();
            UpdateWantedLevel();
            CurrentLocation.UpdatePlayer();

            //gotta go
            if (HealthState.MyPed.Pedestrian.Exists() && HealthState.MyPed.Pedestrian.Handle != Game.LocalPlayer.Character.Handle)
            {
                HealthState.MyPed = new PedExt(Game.LocalPlayer.Character, Settings, Crimes, Weapons, PlayerName, "Person", World);
                if (CharacterModelIsFreeMode)
                {
                    HealthState.MyPed.VoiceName = FreeModeVoice;
                }

            }
            HealthState.UpdatePlayer(this);
            IsStunned = Game.LocalPlayer.Character.IsStunned;
            IsRagdoll = Game.LocalPlayer.Character.IsRagdoll;
            IsInCover = Game.LocalPlayer.Character.IsInCover;
            IsInCombat = Game.LocalPlayer.Character.IsInCombat;
            IsMovingDynamically = IsInCover || IsInCombat || Game.LocalPlayer.Character.IsJumping || Game.LocalPlayer.Character.IsRunning;
            IsSwimming = Game.LocalPlayer.Character.IsSwimming;
            position = Game.LocalPlayer.Character.Position;
            CellX = (int)(position.X / EntryPoint.CellSize);
            CellY = (int)(position.Y / EntryPoint.CellSize);
            EntryPoint.FocusCellX = CellX;
            EntryPoint.FocusCellY = CellY;
            EntryPoint.FocusZone = CurrentLocation?.CurrentZone;
            EntryPoint.FocusPosition = position;
            UpdateClosestLocation();
            GameFiber.Yield();
            Stance.Update();
            Sprinting.Update();
            UpdateClosestScenario();      
            if (IsMakingInsultingGesture)
            {
                CurrentLookedAtPed?.OnInsultedByPlayer(this);
            }
            GPSManager.Update();
            Surrendering.Update();
            GetCurrentViewMode();
            PlayerTasks.Update();
            UpdateClosestLookedAtObject();
            UpdateSleeping();
            GameFiber.Yield();
            Violations.MinorViolations.UpdateData();
        }
        private void UpdateGeneralStatus()
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
        }
        public void UpdateVehicleData()
        {
            IsInVehicle = Character.IsInAnyVehicle(false);
            IsGettingIntoAVehicle = Character.IsGettingIntoVehicle;
            if (IsInVehicle)
            {
                UpdateInVehicleData();
            }
            else
            {
                UpdateOutOfVehicleData();
            }
            GameFiber.Yield();
            TrackedVehicles.RemoveAll(x => !x.Vehicle.Exists());
            UpdateDuckingInVehicle();
            VehicleOwnership.Update();
            CheckAutoEngineDisableToggled();
        }
        private void UpdateInVehicleData()
        {
            if (Character.CurrentVehicle.Exists() && VehicleOwnership.OwnedVehicles.Any(x => x.Vehicle.Exists() && x.Vehicle.Handle == Character.CurrentVehicle.Handle))//OwnedVehicle != null && OwnedVehicle.Vehicle.Exists() && Character.CurrentVehicle.Handle == OwnedVehicle.Vehicle.Handle)
            {
                isJacking = false;
            }
            else if (Character.CurrentVehicle.Exists() && LastFriendlyVehicle.Exists() && LastFriendlyVehicle.Handle == Character.CurrentVehicle.Handle)
            {
                isJacking = false;
            }
            else if (CurrentVehicle != null && CurrentVehicle.IsAlwaysOpenForPlayer)
            {
                isJacking = false;
            }
            else
            {
                isJacking = Character.IsJacking;
            }

            CurrentSeat = Character.SeatIndex;

            IsDriver = CurrentSeat == -1;
            IsInAirVehicle = Character.IsInAirVehicle;

            bool isModelBike = false;
            bool isModelBicycle = false;
            //IsRidingOnTrain = false;
            //if (Character.CurrentVehicle.Exists())
            //{
            //    isModelBike = NativeFunction.Natives.IS_THIS_MODEL_A_BIKE<bool>((uint)Character.CurrentVehicle.Model.Hash);
            //    isModelBicycle = NativeFunction.Natives.IS_THIS_MODEL_A_BICYCLE<bool>((uint)Character.CurrentVehicle.Model.Hash);
            //}
            UpdateCurrentVehicle();
            GameFiber.Yield();
            if (CurrentVehicle != null && CurrentVehicle.Vehicle.Exists())
            {
                IsOnBicycle = CurrentVehicle.IsBicycle;
                IsOnMotorcycle = CurrentVehicle.IsMotorcycle;
                IsInAutomobile = !(IsInAirVehicle || Game.LocalPlayer.Character.IsInSeaVehicle || IsOnBicycle || IsOnMotorcycle || Game.LocalPlayer.Character.IsInHelicopter);

                VehicleSpeed = CurrentVehicle.Vehicle.Speed;
                IsHotWiring = CurrentVehicle != null && CurrentVehicle.Vehicle.Exists() && CurrentVehicle.IsStolen && CurrentVehicle.Vehicle.MustBeHotwired;
                CurrentVehicleRoll = NativeFunction.Natives.GET_ENTITY_ROLL<float>(CurrentVehicle.Vehicle); ;
                if (CurrentVehicleRoll >= 80f || CurrentVehicleRoll <= -80f)
                {
                    CurrentVehicleIsRolledOver = true;
                }
                else
                {
                    CurrentVehicleIsRolledOver = false;
                }
                CurrentVehicleIsInAir = NativeFunction.Natives.IS_ENTITY_IN_AIR<bool>(CurrentVehicle.Vehicle);
                if (!CurrentVehicle.HasBeenEnteredByPlayer)
                {
                    CurrentVehicle.SetAsEntered();
                }


                if(prevCurrentVehicleHandle != CurrentVehicle.Handle)
                {
                    EntryPoint.WriteToConsole($"PLAYER EVENT OnCurrentVehicleChanged to {CurrentVehicle.Handle}");
                    OnCurrentVehicleChanged();
                    prevCurrentVehicleHandle = CurrentVehicle.Handle;
                }
            }
            else
            {
                IsOnBicycle = false;
                IsOnMotorcycle = false;
                IsInAutomobile = false;
                CurrentVehicleIsRolledOver = false;
                VehicleSpeed = 0f;
                prevCurrentVehicleHandle = 0;
            }
            if (VehicleSpeedMPH >= 80f)
            {
                if (!isExcessiveSpeed)
                {
                    OnExcessiveSpeed();
                    isExcessiveSpeed = true;
                }
            }
            else
            {
                if (isExcessiveSpeed)
                {
                    isExcessiveSpeed = false;
                }
            }
            if (isHotwiring != IsHotWiring)
            {
                if (IsHotWiring)
                {
                    GameTimeStartedHotwiring = Game.GameTime;
                }
                else
                {
                    GameTimeStartedHotwiring = 0;
                }
                isHotwiring = IsHotWiring;
            }


            if (CurrentVehicle != null && CurrentVehicle.Vehicle.Exists() && CurrentVehicle.IsPolice)// && CurrentVehicle.Vehicle.IsPoliceVehicle)
            {
                IsInPoliceVehicle = true;
            }
            else
            {
                IsInPoliceVehicle = false;
            }

            if (Settings.SettingsManager.VehicleSettings.AllowRadioInPoliceVehicles && CurrentVehicle != null && CurrentVehicle.Vehicle.Exists() && CurrentVehicle.Vehicle.IsEngineOn && IsInPoliceVehicle)
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

            if (CurrentVehicle != null && IsDriver && CurrentVehicle.Vehicle.Exists() && CurrentVehicle.Vehicle.HasSiren)
            {
                IsSirenOn = CurrentVehicle.Vehicle.IsSirenOn;
                if (Settings.SettingsManager.WorldSettings.AllowSettingSirenState && CurrentVehicle.Vehicle.IsSirenSilent)
                {
                    CurrentVehicle.Vehicle.IsSirenSilent = false;
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

            if (VehicleSpeedMPH >= 25f)
            {
                if (GameTimeStartedMovingFast == 0)
                {
                    GameTimeStartedMovingFast = Game.GameTime;
                }
            }
            else
            {
                GameTimeStartedMovingFast = 0;
            }

            if (VehicleSpeedMPH >= 5f)
            {
                if (GameTimeStartedMoving == 0)
                {
                    GameTimeStartedMoving = Game.GameTime;
                }
            }
            else
            {
                GameTimeStartedMoving = 0;
            }


            //if (CurrentVehicle != null && CurrentVehicle.Vehicle.Exists() && Character.CurrentVehicle.Exists() && Character.SeatIndex != -1 && !IsRidingBus && CurrentVehicle.Vehicle.Model.Name.ToLower().Contains("bus"))
            //{
            //    IsRidingBus = true;
            //    BusRide MyBusRide = new BusRide(this, CurrentVehicle.Vehicle, World, PlacesOfInterest, Settings);
            //    MyBusRide.Start();
            //}
        }



        private void UpdateOutOfVehicleData()
        {
            CurrentVehicleIsRolledOver = false;
            CurrentSeat = -99;
            IsDriver = false;
            IsOnMotorcycle = false;
            IsInAutomobile = false;
            IsOnBicycle = false;
            IsInPoliceVehicle = false;
            IsHotWiring = false;
            PreviousVehicle = CurrentVehicle;
            CurrentVehicle = null;
            VehicleSpeed = 0f;
            IsSirenOn = false;
            //IsRidingOnTrain = NativeFunction.Natives.IS_PLAYER_RIDING_TRAIN<bool>(Game.LocalPlayer);
            float PlayerSpeed = Character.Speed;


            FootSpeed = PlayerSpeed;
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
            IsStill = Character.IsStill;

            if (PlayerSpeed >= 3.0f)
            {
                if (GameTimeStartedMovingFast == 0)
                {
                    GameTimeStartedMovingFast = Game.GameTime;
                }
            }
            else
            {
                GameTimeStartedMovingFast = 0;
            }
            if (PlayerSpeed >= 0.5f)
            {
                if (GameTimeStartedMoving == 0)
                {
                    GameTimeStartedMoving = Game.GameTime;
                }
            }
            else
            {
                GameTimeStartedMoving = 0;
            }


            if (Settings.SettingsManager.PlayerOtherSettings.AllowMobileRadioOnFoot && CurrentVehicle == null && !ActivityManager.IsDancing)
            {
                if (!IsMobileRadioEnabled)
                {
                    IsMobileRadioEnabled = true;
                    NativeFunction.CallByName<bool>("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", true);
                }
            }
            else if(!ActivityManager.IsDancing)
            {
                if (IsMobileRadioEnabled)
                {
                    IsMobileRadioEnabled = false;
                    NativeFunction.CallByName<bool>("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", false);
                }
            }



            if (!Settings.SettingsManager.PlayerOtherSettings.AllowMobileRadioOnFoot && !ActivityManager.IsDancing)
            {
                NativeFunction.CallByName<bool>("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", false);
            }
            isJacking = Character.IsJacking;
        }
        private void UpdateDuckingInVehicle()
        {
            bool isDuckingInVehicle = NativeFunction.Natives.GET_PED_CONFIG_FLAG<bool>(Character, 359, 1);
            if (IsDuckingInVehicle != isDuckingInVehicle)
            {
                if (isDuckingInVehicle)
                {
                    OnStartedDuckingInVehicle();
                }
                else
                {
                    OnStoppedDuckingInVehicle();
                }
                IsDuckingInVehicle = isDuckingInVehicle;
            }
        }
        private void CheckAutoEngineDisableToggled()
        {
            if (Settings.SettingsManager.VehicleSettings.DisableAutoEngineStart != disableAutoEngineStart)
            {
                NativeFunction.Natives.SET_PED_CONFIG_FLAG<bool>(Game.LocalPlayer.Character, (int)PedConfigFlags._PED_FLAG_DISABLE_STARTING_VEH_ENGINE, Settings.SettingsManager.VehicleSettings.DisableAutoEngineStart);
                disableAutoEngineStart = Settings.SettingsManager.VehicleSettings.DisableAutoEngineStart;
            }
        }
        public void UpdateWeaponData()
        {           
            if (Game.LocalPlayer.Character.IsShooting)
            {
                GameTimeLastShot = Game.GameTime;
            }
            IsAiming = Game.LocalPlayer.IsFreeAiming;
            IsAimingInVehicle = IsInVehicle && IsAiming;

            WeaponEquipment.Update();
            UpdateTargetedPed();
            GameFiber.Yield();
            UpdateLookedAtPed();
            GameFiber.Yield();
            ClipsetManager.Update();
            IsShooting = RecentlyShot;
        }
        private void UpdateCurrentVehicle() //should this be public?
        {
            bool IsGettingIntoVehicle = Game.LocalPlayer.Character.IsGettingIntoVehicle;
            bool IsInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
            if (!IsInVehicle && !IsGettingIntoVehicle)
            {
                PreviousVehicle = CurrentVehicle;
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
                PreviousVehicle = CurrentVehicle;
                CurrentVehicle = null;
                return;
            }
            uint newVehicleHandle = vehicle.Handle;
            if (CurrentVehicle == null)
            {
                if (PreviousVehicle != null && PreviousVehicle.Handle == newVehicleHandle)
                {
                    CurrentVehicle = PreviousVehicle;
                }
                else
                {
                    VehicleExt existingVehicleExt = World.Vehicles.GetVehicleExt(vehicle);
                    GameFiber.Yield();
                    if (vehicle.Exists())
                    {
                        if (existingVehicleExt == null)
                        {
                            VehicleExt createdVehicleExt = new VehicleExt(vehicle, Settings);
                            createdVehicleExt.Setup();
                            //createdVehicleExt.CanHaveRandomItems = false;
                            World.Vehicles.AddCivilian(createdVehicleExt);
                            TrackedVehicles.Add(createdVehicleExt);
                            existingVehicleExt = createdVehicleExt;
                            //EntryPoint.WriteToConsoleTestLong("New Vehicle Created in UpdateCurrentVehicle");
                        }
                        if (!TrackedVehicles.Any(x => x.Vehicle.Handle == vehicle.Handle))
                        {
                            TrackedVehicles.Add(existingVehicleExt);
                        }
                        if (IsInVehicle && !existingVehicleExt.HasBeenEnteredByPlayer)
                        {
                            existingVehicleExt.SetAsEntered();
                        }
                        if(IsInVehicle && existingVehicleExt.IsImpounded && existingVehicleExt.ImpoundedLocation != "")
                        {
                            existingVehicleExt.ImpoundedLocation = "";
                            EntryPoint.WriteToConsole("REMOVING IMPOUNDED FROM VEHICLE");
                        }
                        if(existingVehicleExt.IsAircraft)
                        {
                            if(Licenses.HasPilotsLicense && Licenses.PilotsLicense.CanFlyType(existingVehicleExt.VehicleClass))
                            {
                                existingVehicleExt.IsDisabled = false;
                            }
                            else
                            {
                                existingVehicleExt.IsDisabled = true;
                            }
                        }
                        existingVehicleExt.Engine.Synchronize();
                        existingVehicleExt.Update(this);
                        GameFiber.Yield();//TR removed 4
                        if (vehicle.Exists())
                        {
                            if (existingVehicleExt.CanBeConsideredStolen && !existingVehicleExt.IsStolen)
                            {
                                if (IsDriver && !VehicleOwnership.OwnedVehicles.Any(x => x.Handle == existingVehicleExt.Handle))// == null || existingVehicleExt.Handle != OwnedVehicle.Handle))
                                {
                                    existingVehicleExt.IsStolen = true;
                                }
                            }
                            CurrentVehicle = existingVehicleExt;
                            //EntryPoint.WriteToConsole("PLAYER VEHICLE UPDATE Needed to re look up vehicle", 5);
                        }
                    }
                }
            }
            else
            {
                CurrentVehicle.Update(this);
            }
        }
        private void UpdateLookedAtPed()
        {
            if (Game.GameTime - GameTimeLastUpdatedLookedAtPed >= 200)// 500)//750)//750)//750
            {
                GameFiber.Yield();
                Vector3 RayStart = Game.LocalPlayer.Character.GetBonePosition(PedBoneId.Head);
                Vector3 RayEnd = RayStart + NativeHelper.GetGameplayCameraDirection() * 6.0f;
                HitResult result = Rage.World.TraceCapsule(RayStart, RayEnd, 0.25f, TraceFlags.IntersectVehicles | TraceFlags.IntersectPeds, Game.LocalPlayer.Character);
                if (result.Hit && result.HitEntity is Ped)
                {
                    CurrentLookedAtObject = null;
                    CurrentLookedAtPed = World.Pedestrians.GetPedExt(result.HitEntity.Handle);
                    if (CurrentLookedAtPed?.IsGangMember == true)
                    {
                        CurrentLookedAtGangMember = World.Pedestrians.GetGangMember(result.HitEntity.Handle);
                    }
                    else
                    {
                        CurrentLookedAtGangMember = null;
                    }
                    CurrentLookedAtVehicle = null;
                    //EntryPoint.WriteToConsole("HIT PED");
                }
                else if (result.Hit && result.HitEntity is Vehicle)
                {
                    CurrentLookedAtObject = null;
                    Vehicle myCar = (Vehicle)result.HitEntity;
                    if (myCar.Exists())
                    {
                        CurrentLookedAtVehicle = World.Vehicles.GetVehicleExt(myCar);
                    }
   
                    if (myCar.Exists() && Character.CurrentVehicle.Exists() && myCar.Handle == Character.CurrentVehicle.Handle)
                    {
                        CurrentLookedAtObject = null;
                        CurrentLookedAtVehicle = null;
                        CurrentLookedAtPed = null;
                        CurrentLookedAtGangMember = null;
                    }
                    else
                    {
                        if (myCar.Exists() && myCar.Driver.Exists())
                        {
                            Ped closestPed = null;
                            float ClosestDistance = 999f;
                            foreach (Ped occupant in myCar.Occupants)
                            {
                                if (occupant.Exists())
                                {
                                    float distanceTo = occupant.GetBonePosition(0).DistanceTo2D(Character);
                                    if (distanceTo <= ClosestDistance)
                                    {
                                        closestPed = occupant;
                                        ClosestDistance = distanceTo;
                                    }
                                }
                            }
                            if (closestPed.Exists())
                            {
                                CurrentLookedAtPed = World.Pedestrians.GetPedExt(closestPed.Handle);
                                if (CurrentLookedAtPed?.IsGangMember == true)
                                {
                                    CurrentLookedAtGangMember = World.Pedestrians.GetGangMember(closestPed.Handle);
                                }
                                else
                                {
                                    CurrentLookedAtGangMember = null;
                                }
                            }
                        }
                        else
                        {
                            //CurrentLookedAtVehicle = null;
                            CurrentLookedAtPed = null;
                            CurrentLookedAtGangMember = null;
                            CurrentLookedAtObject = null;
                        }
                    }
                }
                else
                {
                    GameFiber.Yield();
                    result = Rage.World.TraceCapsule(RayStart, RayEnd, 0.25f, TraceFlags.IntersectObjects, Game.LocalPlayer.Character);
                    if (result.Hit && result.HitEntity is Rage.Object)
                    {
                        Rage.Object objectHit = (Rage.Object)result.HitEntity;
                        CurrentLookedAtObject = objectHit;
                        CurrentLookedAtVehicle = null;
                        CurrentLookedAtPed = null;
                        CurrentLookedAtGangMember = null;
                        //EntryPoint.WriteToConsole("HIT OBJECT");
                    }
                    else
                    {
                        CurrentLookedAtVehicle = null;
                        CurrentLookedAtPed = null;
                        CurrentLookedAtGangMember = null;
                        CurrentLookedAtObject = null;
                    }
                }
                if(CurrentLookedAtPed != null && CurrentLookedAtPed.IsDead && CurrentLookedAtPed.Pedestrian.Exists())
                {
                    CurrentLookedAtPed.UpdateVehicleState();
                }


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

        private void UpdateClosestScenario()
        {
            if (Settings.SettingsManager.ActivitySettings.AllowStartingScenarios && IsNotWanted && !IsInVehicle)//works fine, just turned off by default, needs some work
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
        private void UpdateClosestLocation()
        {
            ClosestInteractableLocation = null;
            if (!IsMovingFast && IsAliveAndFree && !ActivityManager.IsConversing)
            {
                float ClosestDistance = 999f;
                ClosestInteractableLocation = null;
                ClosestDistance = 999f;
                foreach (GameLocation gl in World.Places.ActiveLocations)// PlacesOfInterest.GetAllStores())
                {
                    if (gl.DistanceToPlayer <= 15.0f && gl.ShowInteractPrompt && !ActivityManager.IsInteractingWithLocation && !InteriorManager.IsInsideTeleportInterior && gl.CanCurrentlyInteract(this))
                    {
                        float liveDistance = gl.EntrancePosition.DistanceTo2D(Position);
                        if (liveDistance <= 4.0f && gl.DistanceToPlayer < ClosestDistance)
                        {
                            ClosestInteractableLocation = gl;
                            ClosestDistance = gl.DistanceToPlayer;
                        }
                    }
                }
                //EntryPoint.WriteToConsole($"CLOSEST LOCATION UPDATE RAN! HasLocation:{ClosestInteractableLocation?.Name} {ClosestDistance}");
            }
        }
        private void UpdateClosestLookedAtObject()
        {
            if (CurrentLookedAtObject != null && CurrentLookedAtObject.Exists())
            {
                if (CurrentLookedAtObject.Handle != prevCurrentLookedAtObjectHandle)
                {
                    if (Seats.CanSit(CurrentLookedAtObject))
                    {
                        CanSitOnCurrentLookedAtObject = true;
                    }
                    else
                    {
                        CanSitOnCurrentLookedAtObject = false;
                    }


                    World.Places.DynamicPlaces.OnLookedAtObject(CurrentLookedAtObject);


                    prevCurrentLookedAtObjectHandle = CurrentLookedAtObject.Handle;
                }
            }
            else
            {
                CanSitOnCurrentLookedAtObject = false;
                prevCurrentLookedAtObjectHandle = 0;
            }
        }
        private void GetCurrentViewMode()
        {
            if (IsInVehicle)
            {
                int VehicleViewMode = NativeFunction.Natives.GET_FOLLOW_VEHICLE_CAM_VIEW_MODE<int>();
                if (VehicleViewMode == 4)
                {
                    IsInFirstPerson = true;
                }
                else
                {
                    IsInFirstPerson = false;
                }
            }
            else
            {
                int ViewMode = NativeFunction.Natives.GET_FOLLOW_PED_CAM_VIEW_MODE<int>();
                if (ViewMode == 4)
                {
                    IsInFirstPerson = true;
                }
                else
                {
                    IsInFirstPerson = false;
                }
            }
        }
        private void UpdateWantedLevel()
        {
            //this was below that, see if this helps with the flashing.....
            int realWantedLevel = Game.LocalPlayer.WantedLevel;
            if (realWantedLevel != 0)//NativeFunction.Natives.GET_FAKE_WANTED_LEVEL<int>()) //if (PreviousWantedLevel != Game.LocalPlayer.WantedLevel)
            {
                if (!Settings.SettingsManager.PoliceSettings.TakeExclusiveControlOverWantedLevel)
                {
                    //this setting is new, allow the game and mods to set 2+ stars
                    if (Settings.SettingsManager.PoliceSettings.TakeExclusiveControlOverWantedLevelOneStarAndBelow)
                    {
                        if (realWantedLevel > 1)
                        {
                            PoliceResponse.RadioInWanted();
                            SetWantedLevel(realWantedLevel, $"Something Else Set, Allowed by settings (1) realWantedLevel{realWantedLevel}", true);
                            PoliceResponse.RadioInWanted();
                            PlacePoliceLastSeenPlayer = Position;

                        }
                    }
                    else//or is they want my mod to just accept any wanted level generated
                    {
                        PoliceResponse.RadioInWanted();
                        SetWantedLevel(realWantedLevel, $"Something Else Set, Allowed by settings (2) realWantedLevel{realWantedLevel}", true);
                        PoliceResponse.RadioInWanted();
                        PlacePoliceLastSeenPlayer = Position;
                    }
                }
                Game.LocalPlayer.WantedLevel = 0;
                //NativeFunction.CallByName<bool>("SET_MAX_WANTED_LEVEL", 0);
            }
            if (PreviousWantedLevel != wantedLevel)//NativeFunction.Natives.GET_FAKE_WANTED_LEVEL<int>()) //if (PreviousWantedLevel != Game.LocalPlayer.WantedLevel)
            {
                GameFiber.Yield();
                OnWantedLevelChanged();
            }
        }

        public void OnManuallyOpenedDoor()
        {
            EntryPoint.WriteToConsole("ADDING DONT CLOSE DOOR FLAG FROM PLAYER");
            ManuallyClosedDoor = false;
            if(IsRunningDoorCloseFlag)
            {
                EntryPoint.WriteToConsole("ADDING DONT CLOSE DOOR FLAG FROM PLAYER BUT I AM ALREADY RUNNING IT SO STOP");
                return;
            }
            GameFiber.StartNew(delegate
            {
                IsRunningDoorCloseFlag = true;
                while (IsInVehicle && !ManuallyClosedDoor)
                {
                    NativeFunction.Natives.SET_PED_RESET_FLAG(Character, 313, true);
                    GameFiber.Yield();
                }
                IsRunningDoorCloseFlag = false;
                EntryPoint.WriteToConsole("STOPPING DONT CLOSE DOOR FLAG");
            }, "Run Debug Logic");
        }
        public void OnManuallyClosedDoor()
        {
            ManuallyClosedDoor = true;
        }
        private void UpdateSleeping()
        {
            if(prevIsSleeping != IsSleeping)
            {
                if(IsSleeping)
                {
                    EntryPoint.WriteToConsole("SLEEPING STARTED, CLOSING EYES");
                }
                else
                {
                    SetFaceNormal();
                    EntryPoint.WriteToConsole("SLEEPING STOPPED, OPENING EYES");
                }
                prevIsSleeping = IsSleeping;
            }
            if (IsSleeping)
            {
                SetFaceSleeping();
            }
        }
        private void SetFaceSleeping()
        {
            string selectedDict = "facials@gen_male@base";
            string selectedAnim = "mood_sleeping_1";
            if(ModelName.ToLower() == "player_zero")
            {
                selectedDict = "facials@p_m_zero@base";
            }
            else if (ModelName.ToLower() == "player_one")
            {
                selectedDict = "facials@p_m_one@base";
            }
            else if (ModelName.ToLower() == "player_two")
            {
                selectedDict = "facials@p_m_two@base";
            }
            else if (Gender == "M")
            {
                selectedDict = "facials@gen_male@base";
            }
            else if (Gender == "F")
            {
                selectedDict = "facials@gen_female@base";
            }
            if (!AnimationDictionary.RequestAnimationDictionayResult(selectedDict))
            {
                Game.DisplaySubtitle("Could not load animation dictionary");
                return;
            }
            NativeFunction.Natives.PLAY_FACIAL_ANIM(Character, selectedAnim, selectedDict);
        }
        private void SetFaceNormal()
        {
            string selectedDict = "facials@gestures@";
            string selectedAnim = "mood_normal";
            if (!AnimationDictionary.RequestAnimationDictionayResult(selectedDict))
            {
                Game.DisplaySubtitle("Could not load animation dictionary");
                return;
            }
            NativeFunction.Natives.PLAY_FACIAL_ANIM(Character, selectedAnim, selectedDict);
        }

        public void OnInteractionMenuCreated(GameLocation gameLocation, MenuPool menuPool, UIMenu interactionMenu)
        {
            PlayerTasks.OnInteractionMenuCreated(gameLocation, menuPool, interactionMenu);
        }
    }
}