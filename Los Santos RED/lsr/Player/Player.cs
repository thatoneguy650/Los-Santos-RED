using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Data;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Locations;
using LosSantosRED.lsr.Player;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Mod
{
    public class Player : IDispatchable, IActivityPerformable, IIntoxicatable, ITargetable, IPoliceRespondable, IInputable, IPedSwappable, IMuggable, IRespawnable, IViolateable, IWeaponDroppable, IDisplayable,
                          ICarStealable, IPlateChangeable, IActionable, IInteractionable, IInventoryable, IRespawning, ISaveable, IPerceptable, ILocateable, IDriveable, ISprintable, IWeatherReportable,
                          IBusRideable, IGangRelateable, IWeaponSwayable, IWeaponRecoilable, IWeaponSelectable, ICellPhoneable, ITaskAssignable, IContactInteractable, IGunDealerRelateable, ILicenseable, IPropertyOwnable, ILocationInteractable, IButtonPromptable, IHumanStateable, IStanceable,
                          IItemEquipable, IDestinateable, IVehicleOwnable, IBankAccountHoldable, IActivityManageable, IHealthManageable, IGroupManageable, IMeleeManageable, ISeatAssignable
    {
        public int UpdateState = 0;
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
        private uint GameTimeLastYelled;
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
        private bool isExcessiveSpeed;
        private bool isGettingIntoVehicle;
        private bool isHotwiring;
        private bool isInVehicle;
        private bool isJacking = false;
        private Vector3 position;
        private int PreviousWantedLevel;
        private int storedViewMode = -1;
        private uint targettingHandle;
        private int TimeBetweenYelling = 2500;
        private int wantedLevel = 0;
        private float CurrentVehicleRoll;
        private uint GameTimeLastClosedDoor;
        private bool isCheckingExcessSpeed;
        private bool isShooting;
        private uint prevCurrentLookedAtObjectHandle;

        private DynamicActivity LowerBodyActivity;
        private DynamicActivity UpperBodyActivity;

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
        private IGangTerritories GangTerritories;
        private IGameSaves GameSaves;
        private ISeats Seats;


        public Player(string modelName, bool isMale, string suspectsName, IEntityProvideable provider, ITimeControllable timeControllable, IStreets streets, IZones zones, ISettingsProvideable settings, IWeapons weapons, IRadioStations radioStations, IScenarios scenarios, ICrimes crimes
            , IAudioPlayable audio, IPlacesOfInterest placesOfInterest, IInteriors interiors, IModItems modItems, IIntoxicants intoxicants, IGangs gangs, IJurisdictions jurisdictions, IGangTerritories gangTerritories, IGameSaves gameSaves, INameProvideable names, IShopMenus shopMenus
            , IPedGroups pedGroups, IDances dances, ISpeeches speeches, ISeats seats)
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
            Scanner = new Scanner(provider, this, audio, Settings, TimeControllable);
            HealthState = new HealthState(new PedExt(Game.LocalPlayer.Character, Settings, Crimes, Weapons, PlayerName, "Person", World), Settings, true);
            if (CharacterModelIsFreeMode)
            {
                HealthState.MyPed.VoiceName = FreeModeVoice;
            }
            CurrentLocation = new LocationData(Game.LocalPlayer.Character, streets, zones, interiors);
            Surrendering = new SurrenderActivity(this, World);
            Violations = new Violations(this, TimeControllable, Crimes, Settings, Zones, GangTerritories);
            Investigation = new Investigation(this, Settings, provider);
            CriminalHistory = new CriminalHistory(this, Settings, TimeControllable);
            PoliceResponse = new PoliceResponse(this, Settings, TimeControllable, World);
            SearchMode = new SearchMode(this, Settings);
            Inventory = new Inventory(this, Settings);
            Sprinting = new Sprinting(this, Settings);
            Intoxication = new Intoxication(this);
            Respawning = new Respawning(TimeControllable, World, this, Weapons, PlacesOfInterest, Settings, this, this);
            RelationshipManager = new RelationshipManager(gangs, Settings, PlacesOfInterest, TimeControllable, this, this);
            CellPhone = new CellPhone(this, this, jurisdictions, Settings, TimeControllable, gangs, PlacesOfInterest, Zones, streets, GangTerritories, Crimes, World);
            PlayerTasks = new PlayerTasks(this, TimeControllable, gangs, PlacesOfInterest, Settings, World, Crimes, names, Weapons, shopMenus, ModItems, pedGroups);
            Licenses = new Licenses(this);
            Properties = new Properties(this, PlacesOfInterest, TimeControllable);
            ButtonPrompts = new ButtonPrompts(this, Settings);
            Injuries = new Injuries(this, Settings);
            Dances = dances;
            HumanState = new HumanState(this, TimeControllable, Settings);
            Speeches = speeches;
            Stance = new Stance(this, Settings);
            WeaponEquipment = new WeaponEquipment(this, this, Weapons, Settings, this, this, this);
            Destinations = new Destinations(this, World);
            VehicleOwnership = new VehicleOwnership(this,World);
            BankAccounts = new BankAccounts(this, Settings);
            ActivityManager = new ActivityManager(this);
            HealthManager = new HealthManager(this, Settings);
            GroupManager = new GroupManager(this, Settings, World, gangs, Weapons);
            MeleeManager = new MeleeManager(this, Settings);
        }
        public RelationshipManager RelationshipManager { get; private set; }
        public Destinations Destinations { get; private set; }
        public CriminalHistory CriminalHistory { get; private set; }
        public PlayerTasks PlayerTasks { get; private set; }
        public PoliceResponse PoliceResponse { get; private set; }
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
        public GroupManager GroupManager { get; private set; }
        public MeleeManager MeleeManager { get; private set; }
        public float ActiveDistance => Investigation.IsActive ? Investigation.Distance : 500f + (WantedLevel * 200f);
        public bool AnyGangMemberCanHearPlayer { get; set; }
        public bool AnyGangMemberCanSeePlayer { get; set; }
        public bool AnyGangMemberRecentlySeenPlayer { get; set; }
        public bool AnyHumansNear => World.Pedestrians.PoliceList.Any(x => x.DistanceToPlayer <= 10f) || World.Pedestrians.CivilianList.Any(x => x.DistanceToPlayer <= 10f) || World.Pedestrians.GangMemberList.Any(x => x.DistanceToPlayer <= 10f) || World.Pedestrians.MerchantList.Any(x => x.DistanceToPlayer <= 10f);
        public bool AnyPoliceCanHearPlayer { get; set; }
        public bool AnyPoliceCanRecognizePlayer { get; set; }
        public bool AnyPoliceCanSeePlayer { get; set; }
        public bool AnyPoliceRecentlySeenPlayer { get; set; }
        public bool AnyPoliceKnowInteriorLocation { get; set; }
        public Rage.Object AttachedProp { get; set; }
        public bool BeingArrested { get; private set; }
        public bool CanConverse => !IsIncapacitated && !IsVisiblyArmed && IsAliveAndFree && !IsMovingDynamically && ((IsInVehicle && VehicleSpeedMPH <= 5f) || !IsMovingFast) && !IsLootingBody && !IsDraggingBody && !IsHoldingHostage && !IsDancing;
        public bool CanConverseWithLookedAtPed => CurrentLookedAtPed != null && CurrentTargetedPed == null && CurrentLookedAtPed.CanConverse && !RelationshipManager.GangRelationships.IsHostile(CurrentLookedAtGangMember?.Gang) && (!CurrentLookedAtPed.IsCop || (IsNotWanted && !Investigation.IsActive)) && CanConverse;
        public bool CanExitCurrentInterior { get; set; } = false;
        public bool CanGrabLookedAtPed => CurrentLookedAtPed != null && CurrentTargetedPed == null && CanTakeHostage && !CurrentLookedAtPed.IsInVehicle && !CurrentLookedAtPed.IsUnconscious && !CurrentLookedAtPed.IsDead && CurrentLookedAtPed.DistanceToPlayer <= 3.0f && CurrentLookedAtPed.Pedestrian.Exists() && CurrentLookedAtPed.Pedestrian.IsThisPedInFrontOf(Character) && !Character.IsThisPedInFrontOf(CurrentLookedAtPed.Pedestrian);
        public bool CanHoldUpTargettedPed => CurrentTargetedPed != null && !IsCop && CurrentTargetedPed.CanBeMugged && !IsGettingIntoAVehicle && !IsBreakingIntoCar && !IsStunned && !IsRagdoll && IsVisiblyArmed && IsAliveAndFree && CurrentTargetedPed.DistanceToPlayer <= 15f;
        public bool CanLoot => !IsCop && !IsInVehicle && !IsIncapacitated && !IsMovingDynamically && !IsLootingBody && !IsDraggingBody && !IsConversing && !IsDancing;
        public bool CanLootLookedAtPed => CurrentLookedAtPed != null && CurrentTargetedPed == null && CanLoot && !CurrentLookedAtPed.HasBeenLooted && !CurrentLookedAtPed.IsInVehicle && (CurrentLookedAtPed.IsUnconscious || CurrentLookedAtPed.IsDead);
        public bool CanDrag => !IsInVehicle && !IsIncapacitated && !IsMovingDynamically && !IsLootingBody && !IsDraggingBody && !IsDancing;
        public bool CanDragLookedAtPed => CurrentLookedAtPed != null && CurrentTargetedPed == null && CanDrag && !CurrentLookedAtPed.IsInVehicle && (CurrentLookedAtPed.IsUnconscious || CurrentLookedAtPed.IsDead);
        public bool CanPerformActivities => (!IsMovingFast || IsInVehicle) && !IsIncapacitated && !IsDead && !IsBusted && !IsGettingIntoAVehicle && !IsMovingDynamically && !RecentlyGotOutOfVehicle;
        public bool CanTakeHostage => !IsCop && !IsInVehicle && !IsIncapacitated && !IsLootingBody && !IsDancing && WeaponEquipment.CurrentWeapon != null && WeaponEquipment.CurrentWeapon.CanPistolSuicide;
        public bool CanRecruitLookedAtGangMember => CurrentLookedAtGangMember != null && CurrentTargetedPed == null && RelationshipManager.GangRelationships.CurrentGang != null && CurrentLookedAtGangMember.Gang != null && RelationshipManager.GangRelationships.CurrentGang.ID == CurrentLookedAtGangMember.Gang.ID && !GroupManager.IsMember(CurrentLookedAtGangMember);
        public string ContinueCurrentActivityPrompt => UpperBodyActivity != null ? UpperBodyActivity.ContinuePrompt : LowerBodyActivity != null ? LowerBodyActivity.ContinuePrompt : "";
        public string CancelCurrentActivityPrompt => UpperBodyActivity != null ? UpperBodyActivity.CancelPrompt : LowerBodyActivity != null ? LowerBodyActivity.CancelPrompt : "";
        public string PauseCurrentActivityPrompt => UpperBodyActivity != null ? UpperBodyActivity.PausePrompt : LowerBodyActivity != null ? LowerBodyActivity.PausePrompt : "";
        public bool CanCancelCurrentActivity => UpperBodyActivity?.CanCancel == true || LowerBodyActivity?.CanCancel == true;
        public bool CanPauseCurrentActivity => UpperBodyActivity?.CanPause == true || LowerBodyActivity?.CanPause == true;
        public bool IsCurrentActivityPaused => UpperBodyActivity?.IsPaused() == true || LowerBodyActivity?.IsPaused() == true;
        public int CellX { get; private set; }
        public int CellY { get; private set; }
        public Ped Character => Game.LocalPlayer.Character;
        public bool CharacterModelIsFreeMode => ModelName.ToLower() == "mp_f_freemode_01" || ModelName.ToLower() == "mp_m_freemode_01";
        public bool CharacterModelIsPrimaryCharacter => ModelName.ToLower() == "player_zero" || ModelName.ToLower() == "player_one" || ModelName.ToLower() == "player_two";
        public Cop ClosestCopToPlayer { get; set; }
        public InteractableLocation ClosestInteractableLocation { get; private set; }
        public float ClosestPoliceDistanceToPlayer { get; set; }
        public Scenario ClosestScenario { get; private set; }
        public PedExt CurrentLookedAtPed { get; private set; }
        public Rage.Object CurrentLookedAtObject { get; private set; }
        public bool CanSitOnCurrentLookedAtObject { get; private set; }
        public GangMember CurrentLookedAtGangMember { get; private set; }
        public PedVariation CurrentModelVariation { get; set; }
        public VehicleExt CurrentSeenVehicle => CurrentVehicle ?? VehicleGettingInto;
        public PedExt CurrentTargetedPed { get; private set; }
        public VehicleExt CurrentVehicle { get;  set; }
        public bool CurrentVehicleIsRolledOver { get; set; }
        public bool CurrentVehicleIsInAir { get; set; }
        public bool DiedInVehicle { get; private set; }
        public string FreeModeVoice => IsMale ? Settings.SettingsManager.PlayerOtherSettings.MaleFreeModeVoice : Settings.SettingsManager.PlayerOtherSettings.FemaleFreeModeVoice;
        public int GroupID { get; set; }
        public bool HasBeenMoving => GameTimeStartedMoving != 0 && Game.GameTime - GameTimeStartedMoving >= 5000;
        public bool HasBeenMovingFast => GameTimeStartedMovingFast != 0 && Game.GameTime - GameTimeStartedMovingFast >= 2000;
        public bool HasCurrentActivity => UpperBodyActivity != null || LowerBodyActivity != null;
        public bool HasOnBodyArmor { get; private set; }
        public Interaction Interaction { get; private set; }
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
        public bool IsAttemptingToSurrender => Surrendering.HandsAreUp && !PoliceResponse.IsWeaponsFree;
        public bool IsBreakingIntoCar => IsCarJacking || IsLockPicking || IsHotWiring || isJacking;
        public bool IsBustable => IsAliveAndFree && PoliceResponse.HasBeenWantedFor >= 3000 && !Surrendering.IsCommitingSuicide && !IsHoldingHostage && !RecentlyBusted && !RecentlyResistedArrest && !PoliceResponse.IsWeaponsFree && (IsIncapacitated || (!IsMoving && !IsMovingDynamically)) && (!IsInVehicle || WantedLevel == 1 || IsIncapacitated);
        public bool IsBusted { get; private set; }
        public bool IsCarJacking { get; set; }
        public bool IsChangingLicensePlates { get; set; }
        public bool IsCommitingSuicide { get; set; }
        public bool IsConversing { get; set; }
        public bool IsCop { get; set; } = false;
        public bool IsAlive => !IsDead;
        public bool IsCrouched { get; set; }
        public bool IsCustomizingPed { get; set; }
        public bool IsDead { get; private set; }
        public bool IsDealingDrugs { get; set; } = false;
        public bool IsDealingIllegalGuns { get; set; } = false;
        public bool IsDisplayingCustomMenus => IsTransacting || IsCustomizingPed || IsConversing;
        public bool IsDoingSuspiciousActivity { get; set; } = false;
        public bool IsDriver { get; private set; }
        public bool IsDuckingInVehicle { get; set; } = false;
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
        public bool IsHoldingHostage { get; set; }
        public bool IsHoldingUp { get; set; }
        public bool IsHotWiring { get; private set; }
        public bool IsInAirVehicle { get; private set; }
        public bool IsInAutomobile { get; private set; }
        public bool IsIncapacitated => IsStunned || IsRagdoll;
        public bool IsInCover { get; private set; }
        public bool IsInSearchMode { get; set; }
        public bool IsInteracting => IsConversing || IsHoldingUp;
        public bool IsInteractingWithLocation { get; set; } = false;
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
        public bool IsLootingBody { get; set; }
        public bool IsDraggingBody { get; set; }
        public bool IsMakingInsultingGesture { get; set; }
        public bool IsMale { get; set; }
        public string Gender => IsMale ? "M" : "F";
        public bool IsMobileRadioEnabled { get; private set; }
        public bool IsMoveControlPressed { get; set; }
        public bool IsMoving => GameTimeLastMoved != 0 && Game.GameTime - GameTimeLastMoved <= 2000;
        public bool IsMovingDynamically { get; private set; }
        public bool IsMovingFast => GameTimeLastMovedFast != 0 && Game.GameTime - GameTimeLastMovedFast <= 2000;
        public bool IsNearScenario { get; private set; }
        public bool IsNotHoldingEnter { get; set; }
        public bool IsNotWanted => wantedLevel == 0;
        public bool IsOnMotorcycle { get; private set; }
        public bool IsOnMuscleRelaxants { get; set; }
        public bool IsPerformingActivity { get; set; }
        public bool IsPressingFireWeapon { get; set; }
        public bool IsRagdoll { get; private set; }
        public bool IsRidingBus { get; set; }
        public bool IsSitting { get; set; } = false;
        public bool IsLayingDown { get; set; } = false;
        public bool IsResting { get; set; } = false;
        public bool IsSleeping { get; set; } = false;
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
        public bool IsStill { get; private set; }
        public bool IsStunned { get; private set; }
        public bool IsTransacting { get; set; }
        public bool IsVisiblyArmed { get; set; }
        public bool IsDangerouslyArmed => WeaponEquipment.IsDangerouslyArmed;
        public bool IsWanted => wantedLevel > 0;
        public Vehicle LastFriendlyVehicle { get; set; }
        public GestureData LastGesture { get; set; }
        public DanceData LastDance { get; set; }
        public string ModelName { get; set; }
        public Vector3 PlacePoliceLastSeenPlayer { get; set; }
        public Vector3 PlacePoliceShouldSearchForPlayer { get; set; }
        public string PlayerName { get; set; }
        public Vector3 Position => position;
        public VehicleExt PreviousVehicle { get; private set; }
        public bool RecentlyBribedPolice => Respawning.RecentlyBribedPolice;
        public bool RecentlyBusted => GameTimeLastBusted != 0 && Game.GameTime - GameTimeLastBusted <= 5000;
        public bool RecentlyCrashedVehicle => GameTimeLastCrashedVehicle != 0 && Game.GameTime - GameTimeLastCrashedVehicle <= 5000;
        public bool RecentlyFedUpCop => GameTimeLastFedUpCop != 0 && Game.GameTime - GameTimeLastFedUpCop <= 5000;
        public bool RecentlyPaidFine => Respawning.RecentlyPaidFine;
        public bool RecentlyResistedArrest => Respawning.RecentlyResistedArrest;
        public bool RecentlyRespawned => Respawning.RecentlyRespawned;
        public bool RecentlySetWanted => GameTimeLastSetWanted != 0 && Game.GameTime - GameTimeLastSetWanted <= 5000;
        public bool RecentlyShot => GameTimeLastShot != 0 && !RecentlyStartedPlaying && Game.GameTime - GameTimeLastShot <= 3000;
        public bool RecentlyStartedPlaying => GameTimeStartedPlaying != 0 && Game.GameTime - GameTimeStartedPlaying <= 3000;
        public bool RecentlyGotOutOfVehicle => GameTimeGotOutOfVehicle != 0 && Game.GameTime - GameTimeGotOutOfVehicle <= 1000;
        public bool ReleasedFireWeapon { get; set; }
        public List<VehicleExt> ReportedStolenVehicles => TrackedVehicles.Where(x => x.NeedsToBeReportedStolen && !x.HasBeenDescribedByDispatch && !x.AddedToReportedStolenQueue).ToList();
        public float SearchModePercentage => SearchMode.SearchModePercentage;
        public bool ShouldCheckViolations => !Settings.SettingsManager.ViolationSettings.TreatAsCop && !IsCop && !RecentlyStartedPlaying;
        public int SpeechSkill { get; set; }
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
        public List<Crime> WantedCrimes => CriminalHistory.WantedCrimes;
        public int WantedLevel => wantedLevel;
        private bool CanYell => !IsYellingTimeOut;
        private bool IsYellingTimeOut => Game.GameTime - GameTimeLastYelled < TimeBetweenYelling;
        public bool IsInFirstPerson { get; private set; }
        public bool IsDancing { get; set; }
        public bool IsBeingANuisance { get; set; }
        public VehicleExt CurrentLookedAtVehicle { get; private set; }
        public float FootSpeed { get; set; }
        public bool WasDangerouslyArmedWhenBusted { get; private set; }





        public int AssignedSeat => -1;
        public List<uint> BlackListedVehicles => new List<uint>();
        public Ped Pedestrian => Game.LocalPlayer.Character;
        public int LastSeatIndex => -1;
        public uint Handle => Game.LocalPlayer.Character.Handle;

        public VehicleExt AssignedVehicle => null;






        //Required
        public void Setup()
        {
            Violations.Setup();
            Respawning.Setup();
            Scanner.Setup();
            RelationshipManager.Setup();
            CellPhone.Setup();
            PlayerTasks.Setup();
            Properties.Setup();
            ButtonPrompts.Setup();
            HumanState.Setup();
            Destinations.Setup();
            SetWantedLevel(0, "Initial", true);
            NativeFunction.CallByName<bool>("SET_MAX_WANTED_LEVEL", 0);
            WeaponEquipment.SetUnarmed();
            VehicleOwnership.Setup();
            BankAccounts.Setup();
            HealthManager.Setup();
            GroupManager.Setup();
            MeleeManager.Setup();

            SpareLicensePlates.Add(new LicensePlate(RandomItems.RandomString(8), 3, false));//random cali
            ModelName = Game.LocalPlayer.Character.Model.Name;
            CurrentModelVariation = NativeHelper.GetPedVariation(Game.LocalPlayer.Character);
            if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Exists())
            {
                UpdateCurrentVehicle();
                VehicleOwnership.TakeOwnershipOfVehicle(CurrentVehicle, false);
            }
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
            WeaponEquipment.Setup();
            GameFiber.StartNew(delegate
            {
                while (isActive)
                {
                    CellPhone.Update();
                    GameFiber.Yield();
                }
            }, "CellPhone");
            AnimationDictionary.RequestAnimationDictionay("facials@gen_female@base");
            AnimationDictionary.RequestAnimationDictionay("facials@gen_male@base");
            AnimationDictionary.RequestAnimationDictionay("facials@p_m_zero@base");
            AnimationDictionary.RequestAnimationDictionay("facials@p_m_one@base");
            AnimationDictionary.RequestAnimationDictionay("facials@p_m_two@base");
            if (Settings.SettingsManager.CellphoneSettings.TerminateVanillaCellphone)
            {
                NativeFunction.Natives.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME("cellphone_flashhand");
                NativeFunction.Natives.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME("cellphone_controller");
            }
            LastGesture = new GestureData("Thumbs Up Quick", "anim@mp_player_intselfiethumbs_up", "enter");
            LastDance = Dances.GetRandomDance();

            SpeechSkill = RandomItems.GetRandomNumberInt(Settings.SettingsManager.PlayerOtherSettings.PlayerSpeechSkill_Min, Settings.SettingsManager.PlayerOtherSettings.PlayerSpeechSkill_Max);
            // NativeFunction.Natives.SET_PED_COMPONENT_VARIATION(Game.LocalPlayer.Character, 11, 320, 0, 0); NativeFunction.Natives.SET_PED_COMPONENT_VARIATION(Game.LocalPlayer.Character, 10, 70, 0, 0);

        }
        public void Update()
        {
            UpdateVehicleData();
            GameFiber.Yield();
            UpdateWeaponData();
            GameFiber.Yield();
            UpdateStateData();
            GameFiber.Yield();
            bool IntoxicationIsPrimary = false;
            if (Intoxication.CurrentIntensity > Injuries.CurrentIntensity)
            {
                IntoxicationIsPrimary = true;
            }
            Intoxication.Update(IntoxicationIsPrimary);
            GameFiber.Yield();//TR Yield RemovedTest 1
            Injuries.Update(!IntoxicationIsPrimary);
            GameFiber.Yield();//TR Yield RemovedTest 1
            HumanState.Update();
            BankAccounts.Update();
            HealthManager.Update();
            GroupManager.Update();
            ButtonPrompts.Update();
            MeleeManager.Update();
        }
        public void Reset(bool resetWanted, bool resetTimesDied, bool resetWeapons, bool resetCriminalHistory, bool resetInventory, bool resetIntoxication, bool resetRelationships, bool resetOwnedVehicles, bool resetCellphone, bool resetActiveTasks, bool resetProperties, bool resetHealth, bool resetNeeds, bool resetGroup)
        {
            IsDead = false;
            IsBusted = false;
            Game.LocalPlayer.HasControl = true;
            BeingArrested = false;
            HealthState.Reset();
            IsPerformingActivity = false; 
            CurrentVehicle = null;
            Destinations.Reset();
            NativeFunction.Natives.SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY(false);
            IsMobileRadioEnabled = false;
            if (resetWanted)
            {
                GameTimeStartedPlaying = Game.GameTime;
                PoliceResponse.Reset();
                Investigation.Reset();
                Violations.Reset();            
                Scanner.Reset();
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
            }
            if (resetIntoxication)
            {
                Intoxication.Reset();
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
            if (resetActiveTasks)
            {
                PlayerTasks.Reset();
            }
            if (resetProperties)
            {
                Properties.Reset();
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
        }
        public void Dispose()
        {
            isActive = false;
            Scanner.Dispose();
            Investigation.Dispose(); //remove blip
            CriminalHistory.Dispose(); //remove blip
            PoliceResponse.Dispose(); //same ^
            Interaction?.Dispose();
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
            Destinations.Dispose();
            VehicleOwnership.Dispose();
            BankAccounts.Dispose();
            HealthManager.Dispose();
            GroupManager.Dispose();
            MeleeManager.Dispose();
            Violations.Dispose();
            NativeFunction.Natives.SET_PED_CONFIG_FLAG<bool>(Game.LocalPlayer.Character, (int)PedConfigFlags._PED_FLAG_PUT_ON_MOTORCYCLE_HELMET, true);
            NativeFunction.Natives.SET_PED_CONFIG_FLAG<bool>(Game.LocalPlayer.Character, (int)PedConfigFlags._PED_FLAG_DISABLE_STARTING_VEH_ENGINE, false);
            NativeFunction.Natives.SET_PED_IS_DRUNK<bool>(Game.LocalPlayer.Character, false);
            NativeFunction.Natives.RESET_PED_MOVEMENT_CLIPSET<bool>(Game.LocalPlayer.Character);
            NativeFunction.Natives.SET_PED_CONFIG_FLAG<bool>(Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, false);
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
            GameSave mySave = GameSaves.GetSave(this);
            if (mySave != null)
            {
                mySave.PlayerName = newName;
                GameSaves.UpdateSave(mySave);
                EntryPoint.WriteToConsole($"PLAYER EVENT: SAVED {newName}", 3);
            }
            PlayerName = newName;
            EntryPoint.WriteToConsole($"PLAYER EVENT: ChangeName {newName}", 3);
        }
        public void DisplayPlayerNotification()
        {
            string NotifcationText = "Warrants: ~g~None~s~";
            if (PoliceResponse.HasObservedCrimes)
            {
                NotifcationText = "Wanted For:" + PoliceResponse.PrintCrimes();
            }
            else if (CriminalHistory.HasHistory)
            {
                NotifcationText = "Wanted For:" + CriminalHistory.PrintCriminalHistory();
            }
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~b~Personal Info", $"~y~{PlayerName}", NotifcationText);
            // DisplayPlayerVehicleNotification();
        }
        public void SetDemographics(string modelName, bool isMale, string playerName, int money, int speechSkill)
        {
            ModelName = modelName;
            PlayerName = playerName;
            IsMale = isMale;
            BankAccounts.SetMoney(money);
            SpeechSkill = speechSkill;// 
            EntryPoint.WriteToConsole($"PLAYER EVENT: SetDemographics MoneyToSet {money} Current: {BankAccounts.Money} {NativeHelper.CashHash(Settings.SettingsManager.PedSwapSettings.MainCharacterToAlias)}", 3);
        }
        public void LocationUpdate()
        {
            CurrentLocation.Update(Character);
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
        public void SetDenStatus(Gang gang, bool v)
        {
            World.Places.StaticPlaces.SetGangLocationActive(gang.ID, v);
        }
        public void SetAngeredCop()
        {
            GameTimeLastFedUpCop = Game.GameTime;
        }
        public void SetBodyArmor(int Type)
        {
            if (CharacterModelIsFreeMode)
            {
                int NumberOfTextureVariations = NativeFunction.Natives.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS<int>(Character, 9, Type) - 1;
                int TextureID = 0;

                if (NumberOfTextureVariations > 0)
                {
                    RandomItems.GetRandomNumberInt(0, NumberOfTextureVariations);
                }
                NativeFunction.Natives.SET_PED_COMPONENT_VARIATION<bool>(Character, 9, Type, TextureID, 0);
                if (!HasOnBodyArmor)
                {
                    Character.Armor = 200;
                }
                HasOnBodyArmor = true;
            }
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
        public void ToggleBodyArmor(int Type)
        {
            if (CharacterModelIsFreeMode)
            {
                if (HasOnBodyArmor)
                {
                    NativeFunction.Natives.SET_PED_COMPONENT_VARIATION<bool>(Character, 9, 0, 0, 0);
                    HasOnBodyArmor = false;
                    Character.Armor = 0;
                }
                else
                {
                    int NumberOfTextureVariations = NativeFunction.Natives.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS<int>(Character, 9, Type) - 1;
                    int TextureID = 0;

                    if (NumberOfTextureVariations > 0)
                    {
                        RandomItems.GetRandomNumberInt(0, NumberOfTextureVariations);
                    }
                    NativeFunction.Natives.SET_PED_COMPONENT_VARIATION<bool>(Character, 9, Type, TextureID, 0);
                    HasOnBodyArmor = true;
                    Character.Armor = 200;
                }
            }
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
            EntryPoint.WriteToConsole($"PLAYER EVENT: OnGotOffFreeway (5 Second Delay)", 3);
        }
        public void OnGotOnFreeway()
        {
            GameFiber.Yield();
            if (IsWanted && AnyPoliceCanSeePlayer && TimeInCurrentVehicle >= 10000 && IsAliveAndFree)
            {
                Scanner.OnGotOnFreeway();
            }
            EntryPoint.WriteToConsole($"PLAYER EVENT: OnGotOnFreeway (5 Second Delay)", 3);
        }
        public void OnInvestigationExpire()
        {
            GameFiber.Yield();
            PoliceResponse.Reset();
            Scanner.OnInvestigationExpire();
            EntryPoint.WriteToConsole($"PLAYER EVENT: OnInvestigationExpire", 3);
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
                    Scanner.OnFIBHRTDeployed();
                }
                else if (vehicleType?.IsHelicopter == true)
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
            GameFiber.Yield();
            CriminalHistory.OnSuspectEluded(PoliceResponse.CrimesObserved.Select(x => x.AssociatedCrime).ToList(), PlacePoliceLastSeenPlayer);
            Scanner.OnSuspectEluded();
        }
        public void OnVehicleCrashed()
        {
            GameFiber.Yield();
            if (IsWanted && AnyPoliceRecentlySeenPlayer && IsInVehicle && TimeInCurrentVehicle >= 5000 && IsAliveAndFree)
            {
                GameTimeLastCrashedVehicle = Game.GameTime;
                Scanner.OnVehicleCrashed();
            }
            EntryPoint.WriteToConsole($"PLAYER EVENT: OnVehicleCrashed", 5);
        }
        public void OnVehicleEngineHealthDecreased(float amount, bool isCollision)
        {
            GameFiber.Yield();
            if (isCollision && IsWanted && AnyPoliceRecentlySeenPlayer && IsInVehicle && amount >= 50f && TimeInCurrentVehicle >= 5000)
            {
                GameTimeLastCrashedVehicle = Game.GameTime;
                Scanner.OnVehicleCrashed();
            }
            EntryPoint.WriteToConsole($"PLAYER EVENT: OnVehicleEngineHealthDecreased {amount} {isCollision}", 5);
        }
        public void OnVehicleHealthDecreased(int amount, bool isCollision)
        {
            GameFiber.Yield();
            if (isCollision && IsWanted && AnyPoliceRecentlySeenPlayer && IsInVehicle && amount >= 50 && TimeInCurrentVehicle >= 5000)
            {
                GameTimeLastCrashedVehicle = Game.GameTime;
                Scanner.OnVehicleCrashed();
            }
            EntryPoint.WriteToConsole($"PLAYER EVENT: OnVehicleHealthDecreased {amount} {isCollision}", 5);
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
        public void OnWantedActiveMode() => Scanner.OnWantedActiveMode();
        public void OnWantedSearchMode() => Scanner.OnWantedSearchMode();
        public void OnWeaponsFree() => Scanner.OnWeaponsFree();
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
                    EntryPoint.WriteToConsole($"CurrentWeapon {WeaponEquipment.CurrentWeapon.ModelName}", 5);
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
                    isCheckingExcessSpeed = true;
                    GameFiber.Sleep(5000);
                    if (isExcessiveSpeed)
                    {
                        Scanner.OnExcessiveSpeed();
                    }
                    isCheckingExcessSpeed = false;
                }, "FastForwardWatcher");
            }
            EntryPoint.WriteToConsole($"PLAYER EVENT: OnExcessiveSpeed", 3);
        }
        private void OnGettingIntoAVehicleChanged()
        {
            //GameFiber.Yield();//TR Yield RemovedTest 2
            if (IsGettingIntoAVehicle)
            {
                Vehicle VehicleTryingToEnter = Game.LocalPlayer.Character.VehicleTryingToEnter;
                int SeatTryingToEnter = Game.LocalPlayer.Character.SeatIndexTryingToEnter;
                if (VehicleTryingToEnter == null)
                {
                    return;
                }
                UpdateCurrentVehicle();
                //GameFiber.Yield();//TR Yield RemovedTest 2
                if (CurrentVehicle != null)
                {
                    Blip attachedBlip = CurrentVehicle.Vehicle.GetAttachedBlip();
                    VehicleGettingInto = CurrentVehicle;
                    if (VehicleOwnership.OwnedVehicles.Any(x => x.Handle == CurrentVehicle.Handle) && CurrentVehicle.Vehicle.Exists())//if (OwnedVehicle != null && CurrentVehicle.Handle == OwnedVehicle.Handle && CurrentVehicle.Vehicle.Exists())
                    {
                        CurrentVehicle.Vehicle.LockStatus = (VehicleLockStatus)1;
                        CurrentVehicle.Vehicle.MustBeHotwired = false;
                    }
                    else if (CurrentVehicle.Vehicle.Exists() && CurrentVehicle.Vehicle.IsPersistent && CurrentVehicle.Vehicle.GetAttachedBlip()?.Sprite == BlipSprite.GangVehicle)//vanilla owned vehicles?
                    {
                        CurrentVehicle.Vehicle.LockStatus = (VehicleLockStatus)1;
                        CurrentVehicle.Vehicle.MustBeHotwired = false;
                    }
                    else if (CurrentVehicle.Vehicle.Exists() && LastFriendlyVehicle.Exists() && CurrentVehicle.Vehicle.Handle == LastFriendlyVehicle.Handle)//vanilla owned vehicles?
                    {
                        CurrentVehicle.Vehicle.LockStatus = (VehicleLockStatus)1;
                        CurrentVehicle.Vehicle.MustBeHotwired = false;
                    }
                    else
                    {
                        if (CurrentVehicle.Vehicle.Exists() && CurrentVehicle.Vehicle.IsPersistent && !Settings.SettingsManager.VehicleSettings.AllowLockMissionVehicles)//vanilla owned vehicles?
                        {
                            CurrentVehicle.Vehicle.LockStatus = (VehicleLockStatus)1;
                            CurrentVehicle.Vehicle.MustBeHotwired = false;
                        }
                        if (!CurrentVehicle.HasBeenEnteredByPlayer && !IsCop)
                        {
                            CurrentVehicle.AttemptToLock();
                            //GameFiber.Yield();//TR Yield RemovedTest 2
                        }
                        bool hasScrewDriver = Inventory.HasTool(ToolTypes.Screwdriver);
                        if (Settings.SettingsManager.VehicleSettings.RequireScrewdriverForHotwire)
                        {
                            if (CurrentVehicle.Vehicle.MustBeHotwired)
                            {
                                CurrentVehicle.IsHotWireLocked = true;
                                CurrentVehicle.Vehicle.MustBeHotwired = false;
                            }
                            if (CurrentVehicle.IsHotWireLocked && hasScrewDriver)
                            {
                                CurrentVehicle.IsHotWireLocked = false;
                                CurrentVehicle.Vehicle.MustBeHotwired = true;
                            }
                        }

                        if (Settings.SettingsManager.VehicleSettings.RequireScrewdriverForLockPickEntry && !hasScrewDriver && IsNotHoldingEnter && VehicleTryingToEnter.Driver == null && VehicleTryingToEnter.LockStatus == (VehicleLockStatus)7 && !VehicleTryingToEnter.IsEngineOn)
                        {
                            Game.DisplayHelp("Screwdriver required to lockpick");
                        }


                        if (IsNotHoldingEnter && VehicleTryingToEnter.Driver == null && VehicleTryingToEnter.LockStatus == (VehicleLockStatus)7 && !VehicleTryingToEnter.IsEngineOn && (!Settings.SettingsManager.VehicleSettings.RequireScrewdriverForLockPickEntry || hasScrewDriver))//no driver && Unlocked
                        {
                            EntryPoint.WriteToConsole($"PLAYER EVENT: LockPick Start", 3);
                            CarLockPick MyLockPick = new CarLockPick(this, VehicleTryingToEnter, SeatTryingToEnter);
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
                        //else if (SeatTryingToEnter != -1)
                        //{
                        //    if (VehicleTryingToEnter.Exists() && VehicleTryingToEnter.Model.Name.ToLower().Contains("bus"))
                        //    {
                        //        EntryPoint.WriteToConsole($"PLAYER EVENT: BusRide Start LockStatus {VehicleTryingToEnter.LockStatus}", 3);
                        //        BusRide MyBusRide = new BusRide(this, VehicleTryingToEnter, World, PlacesOfInterest);
                        //        MyBusRide.Start();
                        //    }
                        //    else
                        //    {
                        //        EntryPoint.WriteToConsole($"PLAYER EVENT: Car Enter as Passenger {VehicleTryingToEnter.LockStatus}", 3);
                        //    }
                        //}
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

                if (!Settings.SettingsManager.PlayerOtherSettings.AllowMobileRadioOnFoot && IsMobileRadioEnabled && !IsDancing)
                {
                    IsMobileRadioEnabled = false;
                    NativeFunction.CallByName<bool>("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", false);
                }



            }
            //UpdateOwnedBlips();
            EntryPoint.WriteToConsole($"PLAYER EVENT: IsInVehicle to {IsInVehicle}", 3);
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
            if (Settings.SettingsManager.PlayerOtherSettings.SetSlowMoOnBusted)
            {
                Game.TimeScale = 0.4f;
            }
            Game.LocalPlayer.HasControl = false;



            Scanner.OnPlayerBusted();
            EntryPoint.WriteToConsole($"PLAYER EVENT: IsBusted Changed to: {IsBusted}", 3);
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
            if (Settings.SettingsManager.PlayerOtherSettings.SetSlowMoOnDeath)
            {
                Game.TimeScale = 0.4f;
            }
            Scanner.OnSuspectWasted();
            EntryPoint.WriteToConsole($"PLAYER EVENT: IsDead Changed to: {IsDead}", 3);
        }
        private void OnIsShootingChanged()
        {
            if (IsShooting)
            {
                if (IsWanted && WantedLevel <= 4 && AnyPoliceRecentlySeenPlayer)
                {
                    Scanner.OnSuspectShooting();
                }
                EntryPoint.WriteToConsole("PLAYER EVENT: Starting Shooting");
            }
            else
            {
                EntryPoint.WriteToConsole("PLAYER EVENT: Stopped Shooting");
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
                EntryPoint.WriteToConsole($"OnStartedDuckingInVehicle viewMode {viewMode} storedViewMode {storedViewMode}", 5);
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
                EntryPoint.WriteToConsole($"OnStoppedDuckingInVehicle storedViewMode {storedViewMode}", 5);
            }
        }
        private void OnTargettingHandleChanged()
        {
            if (TargettingHandle != 0)
            {
                CurrentTargetedPed = World.Pedestrians.GetPedExt(TargettingHandle);
                GameFiber.Yield();
                if (!IsInteracting && !IsInVehicle && CanHoldUpTargettedPed && CurrentTargetedPed != null && CurrentTargetedPed.CanBeMugged)//isinvehicle added here
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
            GameFiber.Yield();
            if (IsNotWanted && PreviousWantedLevel != 0)//Lost Wanted
            {
                if (!RecentlySetWanted)//only allow my process to set the wanted level
                {
                    if (Settings.SettingsManager.PoliceSettings.TakeExclusiveControlOverWantedLevel)
                    {
                        EntryPoint.WriteToConsole($"PLAYER EVENT: GAME AUTO SET WANTED TO {WantedLevel}, RESETTING TO {PreviousWantedLevel}", 3);
                        SetWantedLevel(PreviousWantedLevel, "GAME AUTO SET WANTED", true);
                    }
                }
                else
                {
                    CriminalHistory.OnLostWanted();
                    GameFiber.Yield();
                    PoliceResponse.OnLostWanted();
                    GameFiber.Yield();
                    RelationshipManager.GangRelationships.OnLostWanted();
                    World.Pedestrians.CivilianList.ForEach(x => x.PlayerCrimesWitnessed.Clear());
                    EntryPoint.WriteToConsole($"PLAYER EVENT: LOST WANTED", 3);
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
                    RelationshipManager.GangRelationships.OnBecameWanted();
                    EntryPoint.WriteToConsole($"PLAYER EVENT: BECAME WANTED", 3);
                }
            }
            else if (IsWanted && PreviousWantedLevel < WantedLevel)//Increased Wanted Level (can't decrease only remove for now.......)
            {
                PoliceResponse.OnWantedLevelIncreased();
                EntryPoint.WriteToConsole($"PLAYER EVENT: WANTED LEVEL INCREASED", 3);
                //BigMessage.ShowColoredShard("WANTED", $"{wantedLevel} stars", HudColor.Gold, HudColor.InGameBackground);
            }
            else if (IsWanted && PreviousWantedLevel > WantedLevel)
            {
                //PoliceResponse.OnWantedLevelDecreased();
                EntryPoint.WriteToConsole($"PLAYER EVENT: WANTED LEVEL DECREASED", 3);
            }
            EntryPoint.WriteToConsole($"Wanted Changed: {WantedLevel} Previous: {PreviousWantedLevel}", 3);
            PreviousWantedLevel = wantedLevel;// NativeFunction.Natives.GET_FAKE_WANTED_LEVEL<int>();//PreviousWantedLevel = Game.LocalPlayer.WantedLevel;
        }
        //Crimes
        public void AddCrime(Crime crimeObserved, bool isObservedByPolice, Vector3 Location, VehicleExt VehicleObserved, WeaponInformation WeaponObserved, bool HaveDescription, bool AnnounceCrime, bool isForPlayer)
        {
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
            CrimeSceneDescription description = new CrimeSceneDescription(!IsInVehicle, isObservedByPolice, Location, HaveDescription) { VehicleSeen = VehicleObserved, WeaponSeen = WeaponObserved, Speed = Game.LocalPlayer.Character.Speed };
            PoliceResponse.AddCrime(crimeObserved, description, isForPlayer);
            if (!isObservedByPolice && IsNotWanted)
            {
                Investigation.Start(Location, PoliceResponse.PoliceHaveDescription, true, false, false);
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
                    if (Settings.SettingsManager.PoliceSettings.UseFakeWantedLevelSystem)
                    {
                        NativeFunction.CallByName<bool>("SET_MAX_WANTED_LEVEL", 0);
                        NativeFunction.Natives.SET_FAKE_WANTED_LEVEL(desiredWantedLevel);
                    }
                    else
                    {
                        NativeFunction.CallByName<bool>("SET_MAX_WANTED_LEVEL", desiredWantedLevel);
                        Game.LocalPlayer.WantedLevel = desiredWantedLevel;
                    }

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
        //Vehicle Stuff
        public void ChangePlate(int Index)
        {
            if (!IsPerformingActivity && CanPerformActivities && !IsSitting && !IsInVehicle)
            {
                if (UpperBodyActivity != null)
                {
                    UpperBodyActivity.Cancel();
                }
                IsPerformingActivity = true;
                UpperBodyActivity = new PlateTheft(this, SpareLicensePlates[Index], Settings, World);
                UpperBodyActivity.Start();
            }
        }
        public void ChangePlate(LicensePlate toChange)
        {
            if (!IsPerformingActivity && CanPerformActivities && !IsSitting && !IsInVehicle)
            {
                if (UpperBodyActivity != null)
                {
                    UpperBodyActivity.Cancel();
                }
                IsPerformingActivity = true;
                UpperBodyActivity = new PlateTheft(this, toChange, Settings, World);
                UpperBodyActivity.Start();
            }
        }
        public void CloseDriverDoor()
        {
            if (Game.GameTime - GameTimeLastClosedDoor >= 1500)
            {
                if (!IsPerformingActivity && IsDriver && CurrentVehicle != null && CurrentVehicle.Vehicle.Exists())// Game.LocalPlayer.Character.CurrentVehicle.Exists() && )
                {
                    bool isValid = NativeFunction.Natives.x645F4B6E8499F632<bool>(CurrentVehicle.Vehicle, 0);
                    if (isValid)
                    {
                        float DoorAngle = NativeFunction.Natives.GET_VEHICLE_DOOR_ANGLE_RATIO<float>(CurrentVehicle.Vehicle, 0);

                        if (DoorAngle > 0.0f)
                        {
                            string toPlay = "";
                            int TimeToWait = 250;
                            if (DoorAngle >= 0.7)
                            {
                                toPlay = "d_close_in";
                                TimeToWait = 500;
                            }
                            else
                            {
                                toPlay = "d_close_in_near";
                            }
                            EntryPoint.WriteToConsole($"Player Event: Closing Door Manually Angle {DoorAngle} Dict veh@std@ds@enter_exit Animation {toPlay}", 5);
                            AnimationDictionary.RequestAnimationDictionay("veh@std@ds@enter_exit");
                            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Character, "veh@std@ds@enter_exit", toPlay, 4.0f, -4.0f, -1, 50, 0, false, false, false);//-1
                            GameFiber DoorWatcher = GameFiber.StartNew(delegate
                            {
                                GameFiber.Sleep(TimeToWait);
                                if (Game.LocalPlayer.Character.CurrentVehicle.Exists())
                                {
                                    NativeFunction.Natives.SET_VEHICLE_DOOR_SHUT(Game.LocalPlayer.Character.CurrentVehicle, 0, false);
                                    GameFiber.Sleep(250);
                                    NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Character);
                                }
                                else
                                {
                                    NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Character);
                                }
                            }, "DoorWatcher");
                        }
                        GameTimeLastClosedDoor = Game.GameTime;
                    }
                }
            }
        }
        public void ToggleLeftIndicator()
        {
            if (CurrentVehicle != null)
            {
                CurrentVehicle.Indicators.ToggleLeft();
            }
        }
        public void ToggleHazards()
        {
            if (CurrentVehicle != null)
            {
                CurrentVehicle.Indicators.ToggleHazards();
            }
        }
        public void ToggleRightIndicator()
        {
            if (CurrentVehicle != null)
            {
                CurrentVehicle.Indicators.ToggleRight();
            }
        }
        public void ToggleVehicleEngine()
        {
            if (CurrentVehicle != null)
            {
                CurrentVehicle.Engine.Toggle();
            }
        }
        public void ToggleDriverWindow()
        {
            if (CurrentVehicle != null)
            {
                CurrentVehicle.SetDriverWindow(!CurrentVehicle.ManuallyRolledDriverWindowDown);
            }
        }
        //Actions
        public void CommitSuicide()
        {
            if (!IsPerformingActivity && CanPerformActivities && !IsSitting && !IsInVehicle)
            {
                if (UpperBodyActivity != null)
                {
                    UpperBodyActivity.Cancel();
                }
                IsPerformingActivity = true;
                UpperBodyActivity = new SuicideActivity(this, Settings);
                UpperBodyActivity.Start();
            }
        }
        private void ConsumeItem(ModItem toAdd)
        {
            if (toAdd.CanConsume)
            {
                if (Settings.SettingsManager.NeedsSettings.ApplyNeeds)
                {
                    if (toAdd.ChangesHunger)
                    {
                        HumanState.Hunger.Change(toAdd.HungerChangeAmount, true);
                    }
                    if (toAdd.ChangesSleep)
                    {
                        HumanState.Sleep.Change(toAdd.SleepChangeAmount, true);
                    }
                    if (toAdd.ChangesThirst)
                    {
                        HumanState.Thirst.Change(toAdd.ThirstChangeAmount, true);
                    }
                }
                else
                {
                    if (toAdd.ChangesHealth)
                    {
                        HealthManager.ChangeHealth(toAdd.HealthChangeAmount);
                    }
                }

            }
        }
        public void ContinueCurrentActivity()
        {
            if (UpperBodyActivity != null && UpperBodyActivity.CanPause && UpperBodyActivity.IsPaused())
            {
                UpperBodyActivity.Continue();
            }
            else if (LowerBodyActivity != null && LowerBodyActivity.CanPause && LowerBodyActivity.IsPaused())
            {
                LowerBodyActivity.Continue();
            }
        }
        public void Gesture(GestureData gestureData)
        {
            EntryPoint.WriteToConsole($"Gesture Start 2 NO DATA?: {gestureData == null}");
            if (!IsPerformingActivity && CanPerformActivities)
            {
                if (UpperBodyActivity != null)
                {
                    UpperBodyActivity.Cancel();
                }
                IsPerformingActivity = true;
                LastGesture = gestureData;
                UpperBodyActivity = new GestureActivity(this, gestureData);
                UpperBodyActivity.Start();
            }
        }
        public void Gesture()
        {
            Gesture(LastGesture);
        }
        public void Dance(DanceData danceData)
        {
            EntryPoint.WriteToConsole($"Dance Start 2 NO DATA?: {danceData == null}");
            if (!IsPerformingActivity && CanPerformActivities && !IsInVehicle)
            {
                if (UpperBodyActivity != null)
                {
                    UpperBodyActivity.Cancel();
                }
                IsPerformingActivity = true;
                LastDance = danceData;
                UpperBodyActivity = new DanceActivity(this, danceData, RadioStations, Settings, Dances);
                UpperBodyActivity.Start();
            }
        }
        public void Dance()
        {
            StopDynamicActivity();
            LastDance = Dances.DanceLookups.PickRandom();
            Dance(LastDance);
        }
        public void EnterVehicleAsPassenger(bool withBlocking)
        {
            VehicleExt toEnter = World.Vehicles.GetClosestVehicleExt(Character.Position, false, 10f);
            if (toEnter != null && toEnter.Vehicle.Exists())
            {
                int? seatIndex = toEnter.Vehicle.GetFreePassengerSeatIndex();
                if (seatIndex != null)
                {
                    if (withBlocking)
                    {
                        foreach (Ped passenger in toEnter.Vehicle.Occupants)
                        {
                            if (passenger.Exists())
                            {
                                //passenger.CanBePulledOutOfVehicles = false;//when does this get turned off  ?
                                passenger.StaysInVehiclesWhenJacked = true;
                                passenger.BlockPermanentEvents = true;
                            }
                        }
                    }
                    LastFriendlyVehicle = toEnter.Vehicle;
                    NativeFunction.Natives.TASK_ENTER_VEHICLE(Character, toEnter.Vehicle, 5000, seatIndex, 1f, 9);
                }
            }
        }
        public void ForceErraticDriver()
        {
            if (IsInVehicle && !IsDriver && CurrentVehicle != null && CurrentVehicle.Vehicle.Exists())
            {
                Ped Driver = CurrentVehicle.Vehicle.Driver;
                if (Driver.Exists() && Driver.Handle != Character.Handle)
                {
                    PedExt DriverExt = World.Pedestrians.GetPedExt(Driver.Handle);
                    Driver.BlockPermanentEvents = true;
                    Driver.KeepTasks = true;
                    if (DriverExt != null)
                    {
                        DriverExt.CanBeAmbientTasked = false;
                        DriverExt.WillCallPolice = false;
                        DriverExt.WillCallPoliceIntense = false;
                        DriverExt.WillFight = false;
                        DriverExt.WillFightPolice = false;
                        DriverExt.CanBeTasked = false;
                    }
                    NativeFunction.Natives.SET_DRIVER_ABILITY(Driver, 100f);

                    unsafe
                    {
                        int lol = 0;
                        NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                        NativeFunction.CallByName<bool>("TASK_VEHICLE_MISSION_COORS_TARGET", 0, CurrentVehicle.Vehicle, 358.9726f, -1582.881f, 29.29195f, 8, 50f, (int)eCustomDrivingStyles.Code3, 0f, 2f, true);//8f
                        NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                        NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                        NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Driver, lol);
                        NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                    }

                    //unsafe
                    //{
                    //    int lol = 0;
                    //    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                    //    //NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", 0, CurrentVehicle.Vehicle, -1, -1, 15.0f, 9);
                    //    NativeFunction.CallByName<bool>("TASK_SMART_FLEE_COORD", 0, Position.X,Position.Y,Position.Z,5000f,-1, false, false);

                    //    //NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", 0, CurrentVehicle.Vehicle, 25f, (int)eCustomDrivingStyles.FastEmergency, 25f);
                    //    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                    //    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                    //    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Driver, lol);
                    //    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                    //}
                }
            }
        }
        public void GrabPed()
        {


            if (!IsPerformingActivity && CanPerformActivities && CanGrabLookedAtPed && !IsInVehicle)
            {
                if (UpperBodyActivity != null)
                {
                    UpperBodyActivity.Cancel();
                }
                if (LowerBodyActivity != null)
                {
                    LowerBodyActivity.Cancel();
                }
                LowerBodyActivity = new HumanShield(this, CurrentLookedAtPed, Settings, Crimes, ModItems);
                LowerBodyActivity.Start();
            }




            //if (!IsInteracting && CanLootLookedAtPed)
            //{
            //    if (Interaction != null)
            //    {
            //        Interaction.Dispose();
            //    }
            //    Interaction = new Loot(this, CurrentLookedAtPed, Settings, Crimes, ModItems);
            //    Interaction.Start();
            //}
        }
        public void LootPed()
        {
            if (!IsPerformingActivity && CanPerformActivities && CanLootLookedAtPed && !IsInVehicle)
            {
                if (UpperBodyActivity != null)
                {
                    UpperBodyActivity.Cancel();
                }
                if (LowerBodyActivity != null)
                {
                    LowerBodyActivity.Cancel();
                }
                LowerBodyActivity = new Loot(this, CurrentLookedAtPed, Settings, Crimes, ModItems);
                LowerBodyActivity.Start();
            }
        }
        public void DragPed()
        {
            if (!IsPerformingActivity && CanPerformActivities && CanDragLookedAtPed && !IsInVehicle)
            {
                if (UpperBodyActivity != null)
                {
                    UpperBodyActivity.Cancel();
                }
                if (LowerBodyActivity != null)
                {
                    LowerBodyActivity.Cancel();
                }
                LowerBodyActivity = new Drag(this, CurrentLookedAtPed, Settings, Crimes, ModItems, World);
                LowerBodyActivity.Start();
            }
        }
        public void PauseCurrentActivity()
        {
            if (UpperBodyActivity != null && UpperBodyActivity.CanPause)
            {
                UpperBodyActivity.Pause();
            }
            else if (LowerBodyActivity != null && LowerBodyActivity.CanPause)
            {
                LowerBodyActivity.Pause();
            }
        }
        public void CancelCurrentActivity()
        {
            if (UpperBodyActivity != null && UpperBodyActivity.CanCancel)
            {
                UpperBodyActivity.Cancel();
            }
            else if (LowerBodyActivity != null && LowerBodyActivity.CanCancel)
            {
                LowerBodyActivity.Cancel();
            }
        }
        public void PlaySpeech(string speechName, bool useMegaphone)
        {
            if (CharacterModelIsFreeMode && FreeModeVoice != "")
            {
                if (useMegaphone)
                {
                    Character.PlayAmbientSpeech(FreeModeVoice, speechName, 0, SpeechModifier.ForceMegaphone);

                }
                else
                {
                    Character.PlayAmbientSpeech(FreeModeVoice, speechName, 0, SpeechModifier.Force);
                }
                EntryPoint.WriteToConsole($"FREEMODE COP SPEAK {Character.Handle} freeModeVoice {FreeModeVoice} speechName {speechName}");
            }
            else
            {
                Character.PlayAmbientSpeech(speechName, useMegaphone);
            }
        }
        public void RemovePlate()
        {
            if (!IsPerformingActivity && CanPerformActivities)
            {
                if (UpperBodyActivity != null)
                {
                    UpperBodyActivity.Cancel();
                }
                IsPerformingActivity = true;
                UpperBodyActivity = new PlateTheft(this, Settings, World);
                UpperBodyActivity.Start();
            }
        }
        public void ShuffleToNextSeat()
        {
            if (CurrentVehicle != null && CurrentVehicle.Vehicle.Exists() && IsInVehicle && Character.IsInAnyVehicle(false) && Character.SeatIndex != -1 && NativeFunction.Natives.CAN_SHUFFLE_SEAT<bool>(CurrentVehicle.Vehicle, true))
            {
                NativeFunction.Natives.TASK_SHUFFLE_TO_NEXT_VEHICLE_SEAT(Character, CurrentVehicle.Vehicle, 0);
            }
        }
        public void StartConsumingActivity(ModItem modItem, bool performActivity)
        {
            if (((!IsPerformingActivity && CanPerformActivities) || !performActivity) && modItem.CanConsume)// modItem.Type != eConsumableType.None)
            {
                if (modItem.RequiresTool)
                {
                    if (!Inventory.UseTool(modItem.RequiredToolType))
                    {
                        Game.DisplayHelp($"Cannot Use Item {modItem.Name}, Requires {modItem.RequiredToolType}");
                        //Game.DisplayNotification($"Cannot Use Item {modItem.Name}, Requires {modItem.RequiredToolType}");
                        return;
                    }
                }
                if (modItem.PercentLostOnUse > 0.0f)
                {
                    Inventory.Use(modItem);
                }
                else
                {
                    Inventory.Remove(modItem, 1);
                }
                if (performActivity)
                {
                    if (UpperBodyActivity != null)
                    {
                        UpperBodyActivity.Cancel();
                    }
                    IsPerformingActivity = true;
                    if (modItem.Type == eConsumableType.Drink)
                    {
                        UpperBodyActivity = new DrinkingActivity(this, Settings, modItem, Intoxicants);
                    }
                    else if (modItem.Type == eConsumableType.Eat)
                    {
                        UpperBodyActivity = new EatingActivity(this, Settings, modItem, Intoxicants);
                    }
                    else if (modItem.Type == eConsumableType.Smoke)
                    {
                        UpperBodyActivity = new SmokingActivity(this, Settings, modItem, Intoxicants);
                    }
                    else if (modItem.Type == eConsumableType.Ingest)
                    {
                        UpperBodyActivity = new IngestActivity(this, Settings, modItem, Intoxicants);
                    }
                    else if (modItem.Type == eConsumableType.AltSmoke)
                    {
                        UpperBodyActivity = new PipeSmokingActivity(this, Settings, modItem, Intoxicants);
                    }
                    else if (modItem.Type == eConsumableType.Snort)
                    {
                        UpperBodyActivity = new InhaleActivity(this, Settings, modItem, Intoxicants);
                    }
                    else if (modItem.Type == eConsumableType.Inject)
                    {
                        UpperBodyActivity = new InjectActivity(this, Settings, modItem, Intoxicants);
                    }
                    UpperBodyActivity?.Start();
                }
                else
                {
                    TimeControllable.FastForward(TimeControllable.CurrentDateTime.AddMinutes(3));
                    ConsumeItem(modItem);
                    //ChangeHealth(modItem.HealthChangeAmount);
                }
            }
        }
        public void StartConversation()
        {
            if (!IsInteracting && CanConverseWithLookedAtPed)
            {
                if (Interaction != null)
                {
                    Interaction.Dispose();
                }
                //IsConversing = true;

                if (Settings.SettingsManager.ActivitySettings.UseSimpleConversation)
                {
                    Interaction = new Conversation_Simple(this, CurrentLookedAtPed, Settings, Crimes);
                    Interaction.Start();
                }
                else
                {
                    Interaction = new Conversation(this, CurrentLookedAtPed, Settings, Crimes, Speeches);
                    Interaction.Start();
                }

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
                Interaction = new HoldUp(this, CurrentTargetedPed, Settings, ModItems);
                Interaction.Start();
            }
        }
        public void StartHotwiring()
        {
            if (CurrentVehicle != null && CurrentVehicle.Vehicle.Exists() && CurrentVehicle.IsHotWireLocked)
            {
                CurrentVehicle.IsHotWireLocked = false;
                CurrentVehicle.Vehicle.MustBeHotwired = true;
            }
        }
        public void StartSleeping(bool FindSittingProp)
        {
            if (!IsPerformingActivity && CanPerformActivities && !IsSitting && !IsLayingDown)
            {
                if (UpperBodyActivity != null)
                {
                    UpperBodyActivity.Cancel();
                }
                if (LowerBodyActivity != null)
                {
                    LowerBodyActivity.Cancel();
                }

                if(HumanState.Sleep.IsNearMax)
                {
                    Game.DisplayHelp("You are not tired enough to sleep");
                }
                else
                {
                    LowerBodyActivity = new SleepingActivity(this, Settings);
                    LowerBodyActivity.Start();
                }


            }
        }
        public void StartLocationInteraction()
        {
            if (!IsInteracting && !IsInteractingWithLocation)
            {
                if (Interaction != null)
                {
                    Interaction.Dispose();
                }
                ClosestInteractableLocation.OnInteract(this, ModItems, World, Settings, Weapons, TimeControllable);
            }
        }
        public void StartScenario()
        {
            if (!IsPerformingActivity && CanPerformActivities && !IsSitting && !IsInVehicle)
            {
                if (UpperBodyActivity != null)
                {
                    UpperBodyActivity.Cancel();
                }
                IsPerformingActivity = true;
                UpperBodyActivity = new ScenarioActivity(this);
                UpperBodyActivity.Start();
            }
        }
        public void StartSimpleCellphoneActivity()
        {
            //for now just have the mneu come up, it is supposed to be simple,,,,
            //if (!IsPerformingActivity && CanPerformActivities)
            //{
            //    if (UpperBodyActivity != null)
            //    {
            //        UpperBodyActivity.Cancel();
            //    }
            //    UpperBodyActivity = new CellPhoneInteractionActivity(this, Settings, null, Intoxicants);
            //    UpperBodyActivity?.Start();
            //}
        }
        public void StartSimplePhone()
        {

        }
        public void StartSittingDown(bool findSittingProp, bool enterForward)
        {
            if (!IsPerformingActivity && CanPerformActivities && !IsSitting && !IsInVehicle)
            {
                if (UpperBodyActivity != null)
                {
                    UpperBodyActivity.Cancel();
                }
                if (LowerBodyActivity != null)
                {
                    LowerBodyActivity.Cancel();
                }
                LowerBodyActivity = new SittingActivity(this, Settings, findSittingProp, enterForward, Seats);
                LowerBodyActivity.Start();
            }
        }
        public void StopDynamicActivity()
        {
            if (IsPerformingActivity)
            {
                UpperBodyActivity?.Cancel();
                IsPerformingActivity = false;
            }
        }
        public void YellInPain()
        {
            if (CanYell)
            {
                if (RandomItems.RandomPercent(80))
                {
                    List<int> PossibleYells = new List<int>() { 8 };
                    int YellType = PossibleYells.PickRandom();
                    NativeFunction.Natives.PLAY_PAIN(Character, YellType, 0, 0);

                    List<string> PossibleAnimations = new List<string>() { "pain_6","pain_5","pain_4","pain_3","pain_2","pain_1",
      "electrocuted_1",
      "burning_1" };
                    string Animation = PossibleAnimations.PickRandom();
                    if (IsMale)
                    {
                        if (ModelName.ToLower() == "player_zero")
                        {
                            NativeFunction.Natives.PLAY_FACIAL_ANIM(Character, Animation, "facials@p_m_zero@base");
                        }
                        else if (ModelName.ToLower() == "player_one")
                        {
                            NativeFunction.Natives.PLAY_FACIAL_ANIM(Character, Animation, "facials@p_m_one@base");
                        }
                        else if (ModelName.ToLower() == "player_two")
                        {
                            NativeFunction.Natives.PLAY_FACIAL_ANIM(Character, Animation, "facials@p_m_two@base");
                        }
                        else
                        {
                            NativeFunction.Natives.PLAY_FACIAL_ANIM(Character, Animation, "facials@gen_male@base");
                        }
                    }
                    else
                    {
                        NativeFunction.Natives.PLAY_FACIAL_ANIM(Character, Animation, "facials@gen_female@base");
                    }
                    EntryPoint.WriteToConsole($"PLAYER YELL IN PAIN {Character.Handle} YellType {YellType} Animation {Animation}");
                }
                else
                {
                    PlaySpeech("GENERIC_FRIGHTENED_HIGH", false);
                    EntryPoint.WriteToConsole($"PLAYER CRY SPEECH FOR PAIN {Character.Handle}");
                }

                GameTimeLastYelled = Game.GameTime;
            }
        }
        //Interactions
        public void StartTransaction()
        {
            if (!IsInteracting && CanConverseWithLookedAtPed)
            {
                if (Interaction != null)
                {
                    Interaction.Dispose();
                }
                IsConversing = true;
                Merchant merchant = World.Pedestrians.Merchants.FirstOrDefault(x => x.Handle == CurrentLookedAtPed.Handle);
                if (merchant != null)
                {
                    EntryPoint.WriteToConsole("Transaction: 1 Start Ran", 5);
                    Interaction = new PersonTransaction(this, merchant, merchant.ShopMenu, ModItems, World, Settings, Weapons, TimeControllable) { AssociatedStore = merchant.AssociatedStore };// Settings, ModItems, TimeControllable, World, Weapons); 
                    Interaction.Start();
                }
                else
                {
                    EntryPoint.WriteToConsole("Transaction: 2 Start Ran", 5);
                    Interaction = new PersonTransaction(this, CurrentLookedAtPed, CurrentLookedAtPed.ShopMenu, ModItems, World, Settings, Weapons, TimeControllable);// Settings, ModItems, TimeControllable, World, Weapons);
                    Interaction.Start();
                }
            }
        }
        //Update Data
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
                            SetWantedLevel(realWantedLevel, "Something Else Set, Allowed by settings (1)", true);
                            PlacePoliceLastSeenPlayer = Position;

                        }
                    }
                    else//or is they want my mod to just accept any wanted level generated
                    {
                        SetWantedLevel(realWantedLevel, "Something Else Set, Allowed by settings (2)", true);
                        PlacePoliceLastSeenPlayer = Position;
                    }
                }

                Game.LocalPlayer.WantedLevel = 0;
                NativeFunction.CallByName<bool>("SET_MAX_WANTED_LEVEL", 0);
            }
            if (NativeFunction.Natives.GET_FAKE_WANTED_LEVEL<int>() != wantedLevel)
            {
                NativeFunction.Natives.SET_FAKE_WANTED_LEVEL(wantedLevel);
            }
            //if (Game.LocalPlayer.WantedLevel != 0)//NativeFunction.Natives.GET_FAKE_WANTED_LEVEL<int>()) //if (PreviousWantedLevel != Game.LocalPlayer.WantedLevel)
            //{
            //    Game.LocalPlayer.WantedLevel = 0;
            //    NativeFunction.CallByName<bool>("SET_MAX_WANTED_LEVEL", 0);
            //}

            if (PreviousWantedLevel != wantedLevel)//NativeFunction.Natives.GET_FAKE_WANTED_LEVEL<int>()) //if (PreviousWantedLevel != Game.LocalPlayer.WantedLevel)
            {
                GameFiber.Yield();
                OnWantedLevelChanged();
            }
            if (CurrentLocation.EntityToLocate.Exists() && CurrentLocation.EntityToLocate.Handle != Game.LocalPlayer.Character.Handle)
            {
                CurrentLocation.EntityToLocate = Game.LocalPlayer.Character;
            }
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
            IsMovingDynamically = IsInCover || Game.LocalPlayer.Character.IsInCombat || Game.LocalPlayer.Character.IsJumping || Game.LocalPlayer.Character.IsRunning;
            position = Game.LocalPlayer.Character.Position;
            // RootPosition = NativeFunction.Natives.GET_WORLD_POSITION_OF_ENTITY_BONE<Vector3>(Game.LocalPlayer.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, 57005));// if you are in a car, your position is the mioddle of the car, hopefully this fixes that
            //See which cell it is in now
            CellX = (int)(position.X / EntryPoint.CellSize);
            CellY = (int)(position.Y / EntryPoint.CellSize);
            EntryPoint.FocusCellX = CellX;
            EntryPoint.FocusCellY = CellY;
            EntryPoint.FocusZone = CurrentLocation?.CurrentZone;


            if (IsSleeping && IsNotWanted && !Investigation.IsActive && !Investigation.IsSuspicious)
            {
                if (!TimeControllable.IsFastForwarding)
                {
                    TimeControllable.FastForward(999);
                }
            }
            else
            {
                if (TimeControllable.IsFastForwarding)
                {
                    TimeControllable.StopFastForwarding();
                }
            }


            //GameFiber.Yield();//TR Yield RemovedTest 1
            ClosestInteractableLocation = null;
            if (!IsMovingFast && IsAliveAndFree && !IsConversing)
            {
                float ClosestDistance = 999f;
                ClosestInteractableLocation = null;
                ClosestDistance = 999f;
                foreach (InteractableLocation gl in World.Places.ActiveInteractableLocations)// PlacesOfInterest.GetAllStores())
                {
                    if (gl.IsOpen(TimeControllable.CurrentHour) && gl.DistanceToPlayer <= 3.0f && gl.CanInteract && !IsInteractingWithLocation)
                    {
                        if (gl.DistanceToPlayer < ClosestDistance)
                        {
                            ClosestInteractableLocation = gl;
                            ClosestDistance = gl.DistanceToPlayer;
                        }
                    }
                }
            }
            //GameFiber.Yield();//TR Yield RemovedTest 1

            Stance.Update();

            Sprinting.Update();
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
            if (IsMakingInsultingGesture && CurrentLookedAtPed != null)// && !CurrentLookedAtPed.IsFedUpWithPlayer)
            {
                CurrentLookedAtPed.InsultedByPlayer();
            }


            Destinations.Update();

            if (Surrendering.IsWavingHands)
            {
                if (Game.GameTime - GameTimeLastYelled >= 5000)
                {
                    if (!Investigation.IsActive && World.Pedestrians.Police.Any(x => x.DistanceToPlayer <= 100f) && World.Pedestrians.Civilians.Any(x => x.WantedLevel == 0 && x.CurrentlyViolatingWantedLevel > 0 && ((x.DistanceToPlayer <= 70f && x.CanSeePlayer) || x.DistanceToPlayer <= 30f)))
                    {
                        Investigation.Start(Position, false, true, false, false);
                    }
                    PlaySpeech("GENERIC_FRIGHTENED_HIGH", false);
                    GameTimeLastYelled = Game.GameTime;
                }
            }
            else if (Surrendering.HandsAreUp)
            {

                if (Game.GameTime - GameTimeLastYelled >= 10000)
                {
                    if (RandomItems.RandomPercent(50))
                    {
                        PlaySpeech("GENERIC_FRIGHTENED_MED", false);
                    }
                    else
                    {
                        PlaySpeech("GUN_BEG", false);
                    }
                    GameTimeLastYelled = Game.GameTime;
                }

            }
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
            PlayerTasks.Update();




            if (CurrentLookedAtObject != null && CurrentLookedAtObject.Exists())
            {
                if(CurrentLookedAtObject.Handle != prevCurrentLookedAtObjectHandle)
                {
                    if(Seats.CanSit(CurrentLookedAtObject))
                    {
                        CanSitOnCurrentLookedAtObject = true;
                    }
                    else
                    {
                        CanSitOnCurrentLookedAtObject = false;
                    }
                    prevCurrentLookedAtObjectHandle = CurrentLookedAtObject.Handle;
                }
            }
            else
            {
                CanSitOnCurrentLookedAtObject = false;
                prevCurrentLookedAtObjectHandle = 0;
            }


            //GameFiber.Yield();//TR Yield RemovedTest 1
        }
        public void UpdateVehicleData()
        {
            IsInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
            IsGettingIntoAVehicle = Game.LocalPlayer.Character.IsGettingIntoVehicle;
            if (IsInVehicle)
            {
                if (Character.CurrentVehicle.Exists() && VehicleOwnership.OwnedVehicles.Any(x => x.Vehicle.Exists() && x.Vehicle.Handle == Character.CurrentVehicle.Handle))//OwnedVehicle != null && OwnedVehicle.Vehicle.Exists() && Character.CurrentVehicle.Handle == OwnedVehicle.Vehicle.Handle)
                {
                    isJacking = false;
                }
                else if (Character.CurrentVehicle.Exists() && LastFriendlyVehicle.Exists() && LastFriendlyVehicle.Handle == Character.CurrentVehicle.Handle)
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
                GameFiber.Yield();
                if (CurrentVehicle != null && CurrentVehicle.Vehicle.Exists())
                {

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
                }
                else
                {
                    CurrentVehicleIsRolledOver = false;
                }

                if (Game.LocalPlayer.Character.CurrentVehicle.Exists())
                {
                    VehicleSpeed = Game.LocalPlayer.Character.CurrentVehicle.Speed;
                }
                else
                {
                    VehicleSpeed = 0f;
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

                if (Settings.SettingsManager.VehicleSettings.AllowRadioInPoliceVehicles && CurrentVehicle != null && CurrentVehicle.Vehicle.Exists() && CurrentVehicle.Vehicle.IsEngineOn && CurrentVehicle.Vehicle.IsPoliceVehicle)
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

                if (CurrentVehicle != null && CurrentVehicle.Vehicle.Exists() && CurrentVehicle.Vehicle.HasSiren && CurrentVehicle.Vehicle.IsSirenSilent)
                {
                    CurrentVehicle.Vehicle.IsSirenSilent = false;
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


                if (CurrentVehicle != null && CurrentVehicle.Vehicle.Exists() && Character.CurrentVehicle.Exists() && Character.SeatIndex != -1 && !IsRidingBus && CurrentVehicle.Vehicle.Model.Name.ToLower().Contains("bus"))
                {
                    IsRidingBus = true;
                    BusRide MyBusRide = new BusRide(this, CurrentVehicle.Vehicle, World, PlacesOfInterest, Settings);
                    MyBusRide.Start();
                }

            }
            else
            {
                CurrentVehicleIsRolledOver = false;
                IsDriver = false;
                IsOnMotorcycle = false;
                IsInAutomobile = false;
                PreviousVehicle = CurrentVehicle;
                CurrentVehicle = null;

                float PlayerSpeed = Game.LocalPlayer.Character.Speed;
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
                IsStill = Game.LocalPlayer.Character.IsStill;

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

                if (!Settings.SettingsManager.PlayerOtherSettings.AllowMobileRadioOnFoot && !IsDancing)
                {
                    NativeFunction.CallByName<bool>("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", false);
                }
                isJacking = Character.IsJacking;
            }
            TrackedVehicles.RemoveAll(x => !x.Vehicle.Exists());
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
            VehicleOwnership.Update();
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
                            World.Vehicles.AddEntity(createdVehicleExt, ResponseType.None);
                            TrackedVehicles.Add(createdVehicleExt);
                            existingVehicleExt = createdVehicleExt;
                            EntryPoint.WriteToConsole("New Vehicle Created in UpdateCurrentVehicle");
                        }
                        if (!TrackedVehicles.Any(x => x.Vehicle.Handle == vehicle.Handle))
                        {
                            TrackedVehicles.Add(existingVehicleExt);
                        }
                        if (IsInVehicle && !existingVehicleExt.HasBeenEnteredByPlayer)
                        {
                            existingVehicleExt.SetAsEntered();
                        }
                        existingVehicleExt.Engine.Synchronize();
                        existingVehicleExt.Update(this);
                        GameFiber.Yield();//TR removed 4
                        if (vehicle.Exists())
                        {
                            if (!existingVehicleExt.IsStolen)
                            {
                                if (IsDriver && !VehicleOwnership.OwnedVehicles.Any(x => x.Handle == existingVehicleExt.Handle))// == null || existingVehicleExt.Handle != OwnedVehicle.Handle))
                                {
                                    existingVehicleExt.IsStolen = true;
                                }
                            }
                            CurrentVehicle = existingVehicleExt;

                            EntryPoint.WriteToConsole("PLAYER VEHICLE UPDATE Needed to re look up vehicle", 5);
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
            if (Game.GameTime - GameTimeLastUpdatedLookedAtPed >= 750)//750)//750
            {
                GameFiber.Yield();
                Vector3 RayStart = Game.LocalPlayer.Character.GetBonePosition(PedBoneId.Head);
                Vector3 RayEnd = RayStart + NativeHelper.GetGameplayCameraDirection() * 6.0f;
                HitResult result = Rage.World.TraceCapsule(RayStart, RayEnd, 1f, TraceFlags.IntersectVehicles | TraceFlags.IntersectPeds | TraceFlags.IntersectObjects, Game.LocalPlayer.Character);
                if(result.Hit && result.HitEntity is Rage.Object)
                {
                    Rage.Object objectHit = (Rage.Object)result.HitEntity;
                    CurrentLookedAtObject = objectHit;
                    CurrentLookedAtVehicle = null;
                    CurrentLookedAtPed = null;
                    CurrentLookedAtGangMember = null;
                }
                else if (result.Hit && result.HitEntity is Ped)
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
                }
                else if (result.Hit && result.HitEntity is Vehicle)
                {
                    CurrentLookedAtObject = null;
                    Vehicle myCar = (Vehicle)result.HitEntity;
                    if (myCar.Exists())
                    {
                        CurrentLookedAtVehicle = World.Vehicles.GetVehicleExt(myCar);
                    }
                    else
                    {
                        CurrentLookedAtVehicle = null;
                    }
                    if (myCar.Exists() && myCar.Driver.Exists())
                    {
                        Ped closestPed = null;
                        float ClosestDistance = 999f;
                        foreach (Ped occupant in myCar.Occupants)
                        {
                            if (occupant.Exists())
                            {
                                float distanceTo = occupant.DistanceTo2D(Character);
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
                        CurrentLookedAtPed = null;
                        CurrentLookedAtGangMember = null;
                    }
                }
                else
                {
                    CurrentLookedAtVehicle = null;
                    CurrentLookedAtPed = null;
                    CurrentLookedAtGangMember = null;
                    CurrentLookedAtObject = null;
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
    }
}