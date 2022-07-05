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
                          IBusRideable, IGangRelateable, IWeaponSwayable, IWeaponRecoilable, IWeaponSelectable, ICellPhoneable, ITaskAssignable, IContactInteractable, IGunDealerRelateable, ILicenseable, IPropertyOwnable, ILocationInteractable, IButtonPromptable, IHumanStateable
    {
        public int UpdateState = 0;
        //private BigMessageThread BigMessageThread;




        private int money = 0;

        private ICrimes Crimes;
        private CriminalHistory CriminalHistory;

        private IGameSaves GameSaves;
        private uint GameTimeGotInVehicle;
        private uint GameTimeGotOutOfVehicle;
        private uint GameTimeLastBusted;
        private uint GameTimeLastChangedMoney;
        private uint GameTimeLastCheckedRouteBlip;
        private uint GameTimeLastCrashedVehicle;
        private uint GameTimeLastDied;
        private uint GameTimeLastFedUpCop;
        private uint GameTimeLastMoved;
        private uint GameTimeLastMovedFast;
        private uint GameTimeLastSetMeleeModifier;
        private uint GameTimeLastSetWanted;
        private uint GameTimeLastShot;
        private uint GameTimeLastUpdatedLookedAtPed;
        private uint GameTimeLastYelled;
        private uint GameTimeStartedHotwiring;
        private uint GameTimeStartedMoving;
        private uint GameTimeStartedMovingFast;
        private uint GameTimeStartedPlaying;
        private uint GameTimeWantedLevelStarted;
        private uint GangNotificationID = 0;
        private IGangTerritories GangTerritories;
        private bool HasThrownGotOffFreeway;
        private bool HasThrownGotOnFreeway;
        private HealthState HealthState;
        private IIntoxicants Intoxicants;
        
        private bool isActive = true;
        private bool isAiming;
        private bool isAimingInVehicle;
        private bool isExcessiveSpeed;
        private bool isGettingIntoVehicle;
        private bool isHotwiring;
        private bool isInVehicle;
        private bool isJacking = false;
        private uint LookedAtPedButtonPromptHandle;
        private DynamicActivity LowerBodyActivity;
        private IModItems ModItems;
        private INameProvideable Names;
        private IPlacesOfInterest PlacesOfInterest;
        private Vector3 position;
        private int PreviousWantedLevel;
        private IRadioStations RadioStations;
        private Respawning Respawning;
        private Scanner Scanner;
        private IScenarios Scenarios;
        private SearchMode SearchMode;
        private ISettingsProvideable Settings;
        
        private int storedViewMode = -1;
        private SurrenderActivity Surrendering;
        private uint targettingHandle;
        private int TimeBetweenYelling = 2500;
        private ITimeControllable TimeControllable;
        private DynamicActivity UpperBodyActivity;
        private int wantedLevel = 0;
        private WeaponDropping WeaponDropping;
        private WeaponRecoil WeaponRecoil;
        private IWeapons Weapons;
        private WeaponSelector WeaponSelector;
        private WeaponSway WeaponSway;
        private IEntityProvideable World;
        private IZones Zones;
        private IDances Dances;
        private float CurrentVehicleRoll;
        private uint GameTimeLastClosedDoor;
        private uint GameTimeLastToggledSurrender;
        private bool isCheckingExcessSpeed;
        private bool isShooting;
        private bool isGettingOutOfAVehicle;

        public Player(string modelName, bool isMale, string suspectsName, IEntityProvideable provider, ITimeControllable timeControllable, IStreets streets, IZones zones, ISettingsProvideable settings, IWeapons weapons, IRadioStations radioStations, IScenarios scenarios, ICrimes crimes
            , IAudioPlayable audio, IPlacesOfInterest placesOfInterest, IInteriors interiors, IModItems modItems, IIntoxicants intoxicants, IGangs gangs, IJurisdictions jurisdictions, IGangTerritories gangTerritories, IGameSaves gameSaves, INameProvideable names, IShopMenus shopMenus, IPedGroups pedGroups,IDances dances)
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
            Scanner = new Scanner(provider, this, audio, Settings, TimeControllable);
            HealthState = new HealthState(new PedExt(Game.LocalPlayer.Character, Settings, Crimes, Weapons, PlayerName, "Person"), Settings, true);
            if (CharacterModelIsFreeMode)
            {
                HealthState.MyPed.VoiceName = FreeModeVoice;
            }
            CurrentLocation = new LocationData(Game.LocalPlayer.Character, streets, zones, interiors);
            WeaponDropping = new WeaponDropping(this, Weapons, Settings);
            Surrendering = new SurrenderActivity(this, World);
            Violations = new Violations(this, TimeControllable, Crimes, Settings, Zones, GangTerritories);
            Investigation = new Investigation(this, Settings, provider);
            CriminalHistory = new CriminalHistory(this, Settings, TimeControllable);
            PoliceResponse = new PoliceResponse(this, Settings, TimeControllable, World);
            SearchMode = new SearchMode(this, Settings);
            Inventory = new Inventory(this);
            Sprinting = new Sprinting(this, Settings);
            Intoxication = new Intoxication(this);
            Respawning = new Respawning(TimeControllable, World, this, Weapons, PlacesOfInterest, Settings);
            GangRelationships = new GangRelationships(gangs, this, Settings);
            WeaponSway = new WeaponSway(this, Settings);
            WeaponRecoil = new WeaponRecoil(this, Settings);
            WeaponSelector = new WeaponSelector(this, Settings);
            CellPhone = new CellPhone(this, this, jurisdictions, Settings, TimeControllable, gangs, PlacesOfInterest, Zones, streets, GangTerritories);
            PlayerTasks = new PlayerTasks(this, TimeControllable, gangs, PlacesOfInterest, Settings, World, Crimes, names, Weapons, shopMenus, ModItems, pedGroups);
            GunDealerRelationship = new GunDealerRelationship(this, PlacesOfInterest);
            OfficerFriendlyRelationship = new OfficerFriendlyRelationship(this, PlacesOfInterest);
            Licenses = new Licenses(this);
            Properties = new Properties(this, PlacesOfInterest, TimeControllable);
            ButtonPrompts = new ButtonPrompts(this, Settings);
            Injuries = new Injuries(this, Settings);
            Dances = dances;
            HumanState = new HumanState(this);
        }
        public float ActiveDistance => Investigation.IsActive ? Investigation.Distance : 500f + (WantedLevel * 200f);
        public Cop AliasedCop { get; set; }
        public bool AnyGangMemberCanHearPlayer { get; set; }
        public bool AnyGangMemberCanSeePlayer { get; set; }
        public bool AnyGangMemberRecentlySeenPlayer { get; set; }
        public bool AnyHumansNear => World.Pedestrians.PoliceList.Any(x => x.DistanceToPlayer <= 10f) || World.Pedestrians.CivilianList.Any(x => x.DistanceToPlayer <= 10f) || World.Pedestrians.GangMemberList.Any(x => x.DistanceToPlayer <= 10f) || World.Pedestrians.MerchantList.Any(x => x.DistanceToPlayer <= 10f);
        public bool AnyPoliceCanHearPlayer { get; set; }
        public bool AnyPoliceCanRecognizePlayer { get; set; }
        public bool AnyPoliceCanSeePlayer { get; set; }
        public bool AnyPoliceRecentlySeenPlayer { get; set; }
        public Rage.Object AttachedProp { get; set; }
        public bool BeingArrested { get; private set; }
        public List<ButtonPrompt> ButtonPromptList { get; private set; } = new List<ButtonPrompt>();
        public ButtonPrompts ButtonPrompts { get; private set; }
        public bool CanConverse => !IsIncapacitated && !IsVisiblyArmed && IsAliveAndFree && !IsMovingDynamically && ((IsInVehicle && VehicleSpeedMPH <= 15f) || !IsMovingFast) && !IsLootingBody && !IsDraggingBody && !IsHoldingHostage && !IsDancing;
        public bool CanConverseWithLookedAtPed => CurrentLookedAtPed != null && CurrentTargetedPed == null && CurrentLookedAtPed.CanConverse && CanConverse;
        public bool CanDropWeapon => CanPerformActivities && WeaponDropping.CanDropWeapon;
        public bool CanExitCurrentInterior { get; set; } = false;
        public bool CanGrabLookedAtPed => CurrentLookedAtPed != null && CurrentTargetedPed == null && CanTakeHostage && !CurrentLookedAtPed.IsInVehicle && !CurrentLookedAtPed.IsUnconscious && !CurrentLookedAtPed.IsDead && CurrentLookedAtPed.DistanceToPlayer <= 3.0f && CurrentLookedAtPed.Pedestrian.Exists() && CurrentLookedAtPed.Pedestrian.IsThisPedInFrontOf(Character) && !Character.IsThisPedInFrontOf(CurrentLookedAtPed.Pedestrian);
        public bool CanHoldUpTargettedPed => CurrentTargetedPed != null && !IsCop && CurrentTargetedPed.CanBeMugged && !IsGettingIntoAVehicle && !IsBreakingIntoCar && !IsStunned && !IsRagdoll && IsVisiblyArmed && IsAliveAndFree && CurrentTargetedPed.DistanceToPlayer <= 10f;
        public bool CanLoot => !IsInVehicle && !IsIncapacitated && !IsMovingDynamically && !IsLootingBody && !IsDraggingBody && !IsConversing && !IsDancing;
        public bool CanLootLookedAtPed => CurrentLookedAtPed != null && CurrentTargetedPed == null && CanLoot && !CurrentLookedAtPed.HasBeenLooted && !CurrentLookedAtPed.IsInVehicle && (CurrentLookedAtPed.IsUnconscious || CurrentLookedAtPed.IsDead);
        public bool CanDrag => !IsInVehicle && !IsIncapacitated && !IsMovingDynamically && !IsLootingBody && !IsDraggingBody && !IsDancing;
        public bool CanDragLookedAtPed => CurrentLookedAtPed != null && CurrentTargetedPed == null && CanDrag && !CurrentLookedAtPed.IsInVehicle && (CurrentLookedAtPed.IsUnconscious || CurrentLookedAtPed.IsDead);
        public bool CanPerformActivities => (!IsMovingFast || IsInVehicle) && !IsIncapacitated && !IsDead && !IsBusted && !IsGettingIntoAVehicle && !IsMovingDynamically && !RecentlyGotOutOfVehicle;
        public bool CanSurrender => Surrendering.CanSurrender;
        public bool CanTakeHostage => !IsInVehicle && !IsIncapacitated && !IsLootingBody && !IsDancing && CurrentWeapon != null && CurrentWeapon.CanPistolSuicide;
        public bool CanUndie => Respawning.CanUndie;
        public bool CanWaveHands => Surrendering.CanWaveHands;
        public bool CanCancelCurrentActivity => UpperBodyActivity?.CanCancel == true || LowerBodyActivity?.CanCancel == true;
        public bool CanPauseCurrentActivity => UpperBodyActivity?.CanPause == true || LowerBodyActivity?.CanPause == true;
        public bool IsCurrentActivityPaused => UpperBodyActivity?.IsPaused() == true || LowerBodyActivity?.IsPaused() == true;
        public CellPhone CellPhone { get; private set; }
        public int CellX { get; private set; }
        public int CellY { get; private set; }
        public Ped Character => Game.LocalPlayer.Character;
        public bool CharacterModelIsFreeMode => ModelName.ToLower() == "mp_f_freemode_01" || ModelName.ToLower() == "mp_m_freemode_01";
        public bool CharacterModelIsPrimaryCharacter => ModelName.ToLower() == "player_zero" || ModelName.ToLower() == "player_one" || ModelName.ToLower() == "player_two";
        public Cop ClosestCopToPlayer { get; set; }
        public InteractableLocation ClosestInteractableLocation { get; private set; }
        public float ClosestPoliceDistanceToPlayer { get; set; }
        public Scenario ClosestScenario { get; private set; }
        public int CriminalHistoryMaxWantedLevel => CriminalHistory.MaxWantedLevel;
        public Blip CurrentGPSBlip { get; set; }
        public LocationData CurrentLocation { get; set; }
        public PedExt CurrentLookedAtPed { get; private set; }
        public PedVariation CurrentModelVariation { get; set; }
        public VehicleExt CurrentSeenVehicle => CurrentVehicle ?? VehicleGettingInto;
        public WeaponInformation CurrentSeenWeapon => !IsInVehicle ? CurrentWeapon : null;
        public SelectorOptions CurrentSelectorSetting => WeaponSelector.CurrentSelectorSetting;
        public PedExt CurrentTargetedPed { get; private set; }
        public ComplexTask CurrentTask { get; set; }
        public VehicleExt CurrentVehicle { get; private set; }
        public bool CurrentVehicleIsRolledOver { get; set; }
        public bool CurrentVehicleIsInAir { get; set; }
        public WeaponInformation CurrentWeapon { get; private set; }
        public WeaponCategory CurrentWeaponCategory => CurrentWeapon != null ? CurrentWeapon.Category : WeaponCategory.Unknown;
        public WeaponHash CurrentWeaponHash { get; set; }
        public bool CurrentWeaponIsOneHanded { get; private set; }
        public short CurrentWeaponMagazineSize { get; private set; }
        public string DebugLine1 => $"Player: {ModelName},{Game.LocalPlayer.Character.Handle} RcntStrPly: {RecentlyStartedPlaying} IsMovingDynam: {IsMovingDynamically} IsIntoxicated: {IsIntoxicated} {CurrentLocation?.CurrentZone?.InternalGameName}";
        public string DebugLine2 => $"Vio: {Violations.LawsViolatingDisplay}";
        public string DebugLine3 => $"Rep: {PoliceResponse.ReportedCrimesDisplay}";
        public string DebugLine4 { get; set; }
        public string DebugLine5 => $"{CurrentLookedAtPed?.Handle} CanLootLookedAtPed {CanLootLookedAtPed} CanGrabLookedAtPed {CanGrabLookedAtPed} DIstance {CurrentLookedAtPed?.DistanceToPlayer}  {Violations?.LawsViolatingDisplay}";
        public string DebugLine6 => $"RolledOver: {CurrentVehicleIsRolledOver} CurrentVehicleRoll: {CurrentVehicleRoll} CurrentVehicleIsInAir {CurrentVehicleIsInAir}";
        public string DebugLine7 => $"AnyPolice: CanSee: {AnyPoliceCanSeePlayer}, RecentlySeen: {AnyPoliceRecentlySeenPlayer}, CanHear: {AnyPoliceCanHearPlayer}, CanRecognize {AnyPoliceCanRecognizePlayer}";
        public string DebugLine8 => SearchMode.DebugString;
        public string DebugLine9 => "";
        public bool DiedInVehicle { get; private set; }
        public string FreeModeVoice => IsMale ? Settings.SettingsManager.PlayerOtherSettings.MaleFreeModeVoice : Settings.SettingsManager.PlayerOtherSettings.FemaleFreeModeVoice;
        public GangRelationships GangRelationships { get; private set; }
        public int GroupID { get; set; }
        public GunDealerRelationship GunDealerRelationship { get; private set; }
        public bool HandsAreUp { get; set; }
        public bool HasBeenMoving => GameTimeStartedMoving != 0 && Game.GameTime - GameTimeStartedMoving >= 5000;
        public bool HasBeenMovingFast => GameTimeStartedMovingFast != 0 && Game.GameTime - GameTimeStartedMovingFast >= 2000;
        public bool HasCriminalHistory => CriminalHistory.HasHistory;
        public bool HasCurrentActivity => UpperBodyActivity != null;
        public bool HasDeadlyCriminalHistory => CriminalHistory.HasDeadlyHistory;
        public bool HasOnBodyArmor { get; private set; }
        public Injuries Injuries { get; private set; }
        public Interaction Interaction { get; private set; }
        public float IntoxicatedIntensity { get; set; }
        public float IntoxicatedIntensityPercent { get; set; } = 0.0f;
        public Intoxication Intoxication { get; private set; }
        public Inventory Inventory { get; set; }
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
        public bool IsDisplayingCustomMenus => IsTransacting || IsCustomizingPed;
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
        public bool IsLootingBody { get; set; }
        public bool IsDraggingBody { get; set; }
        public bool IsMakingInsultingGesture { get; set; }
        public bool IsMale { get; set; }
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
        public bool IsSpeeding => Violations.IsSpeeding;
        public bool IsStill { get; private set; }
        public bool IsStunned { get; private set; }
        public bool IsTransacting { get; set; }
        public bool IsVisiblyArmed { get; private set; }
        public bool IsWanted => wantedLevel > 0;
        public bool IsWavingHands { get; set; }
        public Vehicle LastFriendlyVehicle { get; set; }
        public GestureData LastGesture { get; set; }
        public DanceData LastDance { get; set; }
        public WeaponHash LastWeaponHash { get; set; }
        public Licenses Licenses { get; private set; }
        public int MaxWantedLastLife { get; set; }
        public string ModelName { get; set; }
        public int LastChangeMoneyAmount { get; set; }
        public int Money
        {
            get
            {
                int CurrentCash;
                uint PlayerCashHash;
                if (CharacterModelIsPrimaryCharacter)
                {
                    PlayerCashHash = NativeHelper.CashHash(ModelName.ToLower());
                }
                else
                {
                    PlayerCashHash = NativeHelper.CashHash(Settings.SettingsManager.PedSwapSettings.MainCharacterToAlias);
                }

                if (Settings.SettingsManager.PedSwapSettings.AliasPedAsMainCharacter || CharacterModelIsPrimaryCharacter)
                {

                    unsafe
                    {
                        NativeFunction.CallByName<int>("STAT_GET_INT", PlayerCashHash, &CurrentCash, -1);
                    }
                    return CurrentCash;
                }
                else
                {
                    return money;
                }
            }
        }
        public OfficerFriendlyRelationship OfficerFriendlyRelationship { get; private set; }
        public List<VehicleExt> OwnedVehicles { get; set; } = new List<VehicleExt>();
        public Vector3 PlacePoliceLastSeenPlayer { get; set; }
        public string PlayerName { get; set; }
        public PlayerTasks PlayerTasks { get; private set; }
        public PoliceResponse PoliceResponse { get; private set; }
        public Vector3 Position => position;
        public VehicleExt PreviousVehicle { get; private set; }
        public Properties Properties { get; private set; }
        public bool RecentlyBribedPolice => Respawning.RecentlyBribedPolice;
        public bool RecentlyBusted => GameTimeLastBusted != 0 && Game.GameTime - GameTimeLastBusted <= 5000;
        public bool RecentlyChangedMoney => GameTimeLastChangedMoney != 0 && Game.GameTime - GameTimeLastChangedMoney <= 7000;
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
        public List<LicensePlate> SpareLicensePlates { get; private set; } = new List<LicensePlate>();
        public Sprinting Sprinting { get; private set; }
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
        public uint TimeOnFoot => GameTimeGotOutOfVehicle == 0 || IsInVehicle ? 0 : Game.GameTime - GameTimeGotOutOfVehicle;
        public int TimesDied => Respawning.TimesDied;
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
        public Violations Violations { get; private set; }
        public List<Crime> WantedCrimes => CriminalHistory.WantedCrimes;
        public int WantedLevel => wantedLevel;
        private bool CanYell => !IsYellingTimeOut;
        private bool IsYellingTimeOut => Game.GameTime - GameTimeLastYelled < TimeBetweenYelling;
        public bool IsInFirstPerson { get; private set; }
        public bool IsDancing { get; set; }
        public bool IsBeingANuisance { get; set; }
        public VehicleExt CurrentLookedAtVehicle { get; private set; }
        public float FootSpeed { get; set; }
        public HumanState HumanState { get; set; }
        public void Setup()
        {
            Violations.Setup();
            Respawning.Setup();
            GangRelationships.Setup();
            CellPhone.Setup();
            PlayerTasks.Setup();
            GunDealerRelationship.Setup();
            OfficerFriendlyRelationship.Setup();
            Properties.Setup();
            ButtonPrompts.Setup();
            HumanState.Setup();


            SetWantedLevel(0, "Initial", true);
            NativeFunction.CallByName<bool>("SET_MAX_WANTED_LEVEL", 0);
            SetUnarmed();
            SpareLicensePlates.Add(new LicensePlate(RandomItems.RandomString(8), 3, false));//random cali
            ModelName = Game.LocalPlayer.Character.Model.Name;
            CurrentModelVariation = NativeHelper.GetPedVariation(Game.LocalPlayer.Character);
            if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Exists())
            {
                UpdateCurrentVehicle();
                TakeOwnershipOfVehicle(CurrentVehicle, false);
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

            GameFiber.StartNew(delegate
            {
                while (isActive)
                {
                    if (Game.LocalPlayer.Character.IsShooting)
                    {
                        WeaponRecoil.Update();
                        GameTimeLastShot = Game.GameTime;
                    }
                    else if (Game.LocalPlayer.IsFreeAiming || Game.LocalPlayer.Character.IsAiming)
                    {
                        WeaponSway.Update();
                    }
                    GameFiber.Yield();
                }
            }, "IsShootingChecker");
            GameFiber.StartNew(delegate
            {
                while (isActive)
                {
                    WeaponSelector.Update();
                    GameFiber.Yield();
                }
            }, "IsShootingChecker2");

            GameFiber.StartNew(delegate
            {
                while (isActive)
                {
                    CellPhone.Update();
                    GameFiber.Yield();
                }
            }, "CellPhone");

            //BigMessageThread = new BigMessageThread(true);
            //BigMessage = BigMessageThread.MessageInstance;



            AnimationDictionary.RequestAnimationDictionay("facials@gen_female@base");
            AnimationDictionary.RequestAnimationDictionay("facials@gen_male@base");

            AnimationDictionary.RequestAnimationDictionay("facials@p_m_zero@base");
            AnimationDictionary.RequestAnimationDictionay("facials@p_m_one@base");
            AnimationDictionary.RequestAnimationDictionay("facials@p_m_two@base");


            if (Settings.SettingsManager.CellphoneSettings.TerminateVanillaCellphone)
            {
                Tools.Scripts.TerminateScript("cellphone_flashhand");
                Tools.Scripts.TerminateScript("cellphone_controller");
            }
            LastGesture = new GestureData("Thumbs Up Quick", "anim@mp_player_intselfiethumbs_up", "enter");
            LastDance = Dances.GetRandomDance();
        }
        public void Update()
        {
            UpdateData();
            ButtonPrompts.Update();
        }
        public void Reset(bool resetWanted, bool resetTimesDied, bool clearWeapons, bool clearCriminalHistory, bool clearInventory, bool clearIntoxication, bool resetGangRelationships, bool clearOwnedVehicles, bool clearCellphone, bool clearActiveTasks, bool clearProperties, bool resetHealth)
        {
            IsDead = false;
            IsBusted = false;
            Game.LocalPlayer.HasControl = true;
            BeingArrested = false;
            HealthState.Reset();
            IsPerformingActivity = false;
            CurrentVehicle = null;
            RemoveGPSRoute();


            NativeFunction.CallByName<bool>("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", false);
            IsMobileRadioEnabled = false;

            // IsIntoxicated = false;
            if (resetWanted)
            {
                PoliceResponse.Reset();
                Investigation.Reset();
                Violations.Reset();
                MaxWantedLastLife = 0;
                GameTimeStartedPlaying = Game.GameTime;
                Scanner.Reset();
                Update();
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
            if (clearInventory)
            {
                Inventory.Clear();
            }
            if (clearIntoxication)
            {
                Intoxication.Dispose();
            }
            else if (IsIntoxicated)
            {
                Intoxication.Restart();
            }
            if (resetGangRelationships)
            {
                GangRelationships.ResetAllReputations();
                foreach (GangDen gd in PlacesOfInterest.PossibleLocations.GangDens)
                {
                    gd.Reset();
                }
                foreach (DeadDrop gd in PlacesOfInterest.PossibleLocations.DeadDrops)
                {
                    gd.Reset();
                }
                OfficerFriendlyRelationship.Reset(false);
                GunDealerRelationship.Reset(false);
            }
            if (clearOwnedVehicles)
            {
                ClearVehicleOwnership();
            }
            if (clearCellphone)
            {
                CellPhone.Reset();
            }
            if (clearActiveTasks)
            {
                PlayerTasks.Clear();
            }
            if (clearProperties)
            {
                Properties.Dispose();
            }
            if (resetHealth)
            {
                Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
                NativeFunction.Natives.RESET_PED_VISIBLE_DAMAGE(Game.LocalPlayer.Character);
                Injuries.Dispose();

                HumanState.Reset();

            }
            else if (Injuries.IsInjured)
            {
                Injuries.Restart();
            }
        }
        public void Dispose()
        {
            Investigation.Dispose(); //remove blip
            CriminalHistory.Dispose(); //remove blip
            PoliceResponse.Dispose(); //same ^
            Interaction?.Dispose();
            SearchMode.Dispose();
            GangRelationships.Dispose();
            CellPhone.Dispose();
            PlayerTasks.Dispose();
            Properties.Dispose();
            ButtonPrompts.Dispose();
            Intoxication.Dispose();
            Injuries.Dispose();

            GunDealerRelationship.Dispose();
            OfficerFriendlyRelationship.Dispose();
            HumanState.Dispose();

            isActive = false;
            RemoveGPSRoute();
            NativeFunction.Natives.SET_PED_CONFIG_FLAG<bool>(Game.LocalPlayer.Character, (int)PedConfigFlags._PED_FLAG_PUT_ON_MOTORCYCLE_HELMET, true);


            NativeFunction.Natives.SET_PED_CONFIG_FLAG<bool>(Game.LocalPlayer.Character, (int)PedConfigFlags._PED_FLAG_DISABLE_STARTING_VEH_ENGINE, false);
            // NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags._PED_FLAG_DISABLE_STARTING_VEH_ENGINE, false);

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
            NativeFunction.CallByName<bool>("SET_MAX_WANTED_LEVEL", 6);

            NativeFunction.Natives.SET_PED_AS_COP(Game.LocalPlayer.Character, false);
            ClearVehicleOwnership();
            if (Settings.SettingsManager.PlayerOtherSettings.SetSlowMoOnDeath)
            {
                Game.TimeScale = 1f;
            }
            NativeFunction.Natives.ENABLE_ALL_CONTROL_ACTIONS(0);//enable all controls in case we left some disabled

            NativeFunction.Natives.SET_CAN_ATTACK_FRIENDLY(Character, false, false);
            NativeFunction.Natives.SET_PLAYER_CAN_BE_HASSLED_BY_GANGS(Game.LocalPlayer, true);

            //Game.DisableControlAction(0, GameControl.Attack, false);
        }
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
                Investigation.Start(Location, PoliceResponse.PoliceHaveDescription,true,false,false);
            }



            if (AnnounceCrime)
            {
                Scanner.AnnounceCrime(crimeObserved, description);
            }


        }
        public void AddCrimeToHistory(Crime crime) => CriminalHistory.AddCrime(crime);
        public void AddDistressedPed(Vector3 position)
        {
            if (Settings.SettingsManager.EMSSettings.ManageDispatching && Settings.SettingsManager.EMSSettings.ManageTasking && World.TotalWantedLevel <= 1 && World.Pedestrians.PedExts.Any(x => (x.IsUnconscious || x.IsInWrithe) && !x.IsDead && !x.HasStartedEMTTreatment))
            {
                //Scanner.Reset();
                Investigation.Start(position, false, false, true, false);
                Scanner.OnMedicalServicesRequested();
            }
        }
        public void AddGPSRoute(string Name, Vector3 position)
        {
            if (CurrentGPSBlip.Exists())
            {
                NativeFunction.Natives.SET_BLIP_ROUTE(CurrentGPSBlip, false);
                CurrentGPSBlip.Delete();
            }
            if (position != Vector3.Zero)
            {
                Blip MyLocationBlip = new Blip(position)
                {
                    Name = Name
                };
                if (MyLocationBlip.Exists())
                {
                    MyLocationBlip.Color = Color.LightYellow;
                    NativeFunction.Natives.SET_BLIP_AS_SHORT_RANGE(MyLocationBlip, false);
                    NativeFunction.Natives.BEGIN_TEXT_COMMAND_SET_BLIP_NAME("STRING");
                    NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(Name);
                    NativeFunction.Natives.END_TEXT_COMMAND_SET_BLIP_NAME(MyLocationBlip);
                    NativeFunction.Natives.SET_BLIP_ROUTE(MyLocationBlip, true);
                    CurrentGPSBlip = MyLocationBlip;
                    World.AddBlip(MyLocationBlip);
                    Game.DisplaySubtitle($"Adding GPS To {Name}");
                    GameTimeLastCheckedRouteBlip = Game.GameTime;
                }
            }
        }
        public void AnnounceCrime(Crime crimeObserved, bool isObservedByPolice, Vector3 Location, VehicleExt VehicleObserved, WeaponInformation WeaponObserved)
        {
            CrimeSceneDescription description = new CrimeSceneDescription(false, isObservedByPolice, Location, false) { VehicleSeen = VehicleObserved, WeaponSeen = WeaponObserved };
            Scanner.AnnounceCrime(crimeObserved, description);
        }
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
        public void CallEMS()
        {
            if (Settings.SettingsManager.EMSSettings.ManageDispatching && Settings.SettingsManager.EMSSettings.ManageTasking && World.TotalWantedLevel <= 1)
            {
                Scanner.Reset();
                Investigation.Start(Position, false, false, true, false);
                Scanner.OnMedicalServicesRequested();
            }
        }
        public void CallFire()
        {
            if (World.TotalWantedLevel <= 1)
            {
                Scanner.Reset();
                Investigation.Start(Position, false, false, false, true);
                Scanner.OnFirefightingServicesRequested();
            }
        }
        public void CallPolice()
        {
            Crime ToCallIn = Crimes.CrimeList.FirstOrDefault(x => x.ID == "OfficersNeeded");
            PedExt violatingCiv = World.Pedestrians.Citizens.Where(x => x.DistanceToPlayer <= 200f).OrderByDescending(x => x.CurrentlyViolatingWantedLevel).FirstOrDefault();
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
                Investigation.Start(Position, false, true, false, false);
            }
            else
            {
                AddCrime(ToCallIn, false, description.PlaceSeen, description.VehicleSeen, description.WeaponSeen, false, true, false);
            }
        }
        public void ChangeHealth(int ToAdd)
        {
            if (ToAdd > 0)
            {
                if (Character.Health < Character.MaxHealth)
                {
                    if (Character.MaxHealth - Character.Health < ToAdd)
                    {
                        ToAdd = Character.MaxHealth - Character.Health;
                    }
                    Character.Health += ToAdd;
                    EntryPoint.WriteToConsole($"PLAYER EVENT: Added Health {ToAdd}", 5);
                }
            }
            else if(ToAdd < 0)
            {
                if(Character.Health > 0 && Character.Health + ToAdd >= 0)
                {
                    Character.Health += ToAdd;
                }
                else
                {
                    Character.Health = 0;
                }
            }
        }
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
        public void ClearVehicleOwnership()
        {
            foreach (VehicleExt car in OwnedVehicles)
            {
                if (car.Vehicle.Exists())
                {
                    Blip attachedBlip = car.Vehicle.GetAttachedBlip();
                    if (attachedBlip.Exists())
                    {
                        attachedBlip.Delete();
                    }
                    if (car.AttachedBlip.Exists())
                    {
                        car.AttachedBlip.Delete();
                    }
                    car.Vehicle.IsPersistent = false;
                }
            }
            OwnedVehicles.Clear();
            EntryPoint.WriteToConsole($"PLAYER EVENT: OWNED VEHICLEs CLEARED", 5);
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
            if(CurrentVehicle != null)
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
        public void ConsumeItem(ModItem toAdd)
        {
            if (toAdd.CanConsume)
            {
                if (toAdd.ChangesHealth)
                {
                    ChangeHealth(toAdd.HealthChangeAmount);
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
        public void Crouch()
        {
            string CrouchSet = "move_crouch_proto";
            CrouchSet = "move_ped_crouched";
            CrouchSet = "move_ped_crouched";


            //if(CurrentWeapon != null && !CurrentWeapon.CanPistolSuicide)
            //{
            //    CrouchSet = "move_aim_strafe_crouch_2h";
            //}

            if (!IsInVehicle)
            {
                if (IsCrouched)
                {
                    NativeFunction.Natives.RESET_PED_MOVEMENT_CLIPSET<bool>(Character);
                    IsCrouched = false;
                }
                else
                {
                    if (!NativeFunction.Natives.HAS_ANIM_SET_LOADED<bool>(CrouchSet))
                    {
                        NativeFunction.Natives.REQUEST_ANIM_SET(CrouchSet);
                    }
                    NativeFunction.Natives.SET_PED_MOVEMENT_CLIPSET(Character, CrouchSet, 0.5f);
                    IsCrouched = true;
                }
            }
        }
        public void DeleteTrackedVehicles()
        {
            TrackedVehicles.Clear();
        }
        public void DisplayPlayerGangNotification()
        {
            Game.RemoveNotification(GangNotificationID);
            string NotifcationText = GangRelationships.PrintRelationships();
            if (NotifcationText != "")
            {
                GangNotificationID = Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Gang Info", $"~y~{PlayerName}", NotifcationText);
            }
            else
            {
                GangNotificationID = Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Gang Info", $"~y~{PlayerName}", "~s~Gangs: N/A");
            }
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
            // DisplayPlayerVehicleNotification();
        }
        public void DisplayPlayerVehicleNotification(VehicleExt toDescribe)
        {
            string NotifcationText = "";
            VehicleExt VehicleToDescribe = toDescribe;
            bool usingOwned = true;
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

                //string VehicleNameColor = "~p~";
                //string VehicleString = "";
                if (usingOwned)
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
        public void DropWeapon()
        {
            if (CanDropWeapon)
            {
                WeaponDropping.DropWeapon();
            }
        }
        public void EnterLocation()
        {
            //if (ClosestTeleportEntrance != null)
            //{
            //    Game.FadeScreenOut(1500, true);
            //    CurrentInteriorLocation = ClosestTeleportEntrance;
            //    Character.Position = ClosestTeleportEntrance.TeleportEnterPosition;
            //    Character.Heading = ClosestTeleportEntrance.TeleportEnterHeading;
            //    Game.FadeScreenIn(1500, true);
            //}
            //else if (ClosestPurchaseLocation != null && ClosestPurchaseLocation.IsPurchased)
            //{
            //    Game.DisplayNotification("ENTERED BRO");
            //}
        }
        public void EnterVehicleAsPassenger()
        {
            VehicleExt toEnter = World.Vehicles.GetClosestVehicleExt(Character.Position, false, 10f);
            if (toEnter != null && toEnter.Vehicle.Exists())
            {
                int? seatIndex = toEnter.Vehicle.GetFreePassengerSeatIndex();
                if (seatIndex != null)
                {
                    foreach (Ped passenger in toEnter.Vehicle.Occupants)
                    {
                        if (passenger.Exists())
                        {
                            passenger.StaysInVehiclesWhenJacked = true;
                            passenger.BlockPermanentEvents = true;
                        }
                    }
                    LastFriendlyVehicle = toEnter.Vehicle;
                    NativeFunction.Natives.TASK_ENTER_VEHICLE(Character, toEnter.Vehicle, 5000, seatIndex, 1f, 9);
                }
            }
        }
        public void ExitLocation()
        {
            //if (CurrentInteriorLocation != null)
            //{
            //    Game.FadeScreenOut(1500, true);
            //    Character.Position = CurrentInteriorLocation.EntrancePosition;
            //    Character.Heading = CurrentInteriorLocation.EntranceHeading;
            //    CurrentInteriorLocation = null;
            //    Game.FadeScreenIn(1500, true);
            //}
        }
        public int FineAmount()
        {
            int InitialAmount = Settings.SettingsManager.PoliceSettings.GeneralFineAmount;
            if (PoliceResponse.PlayerSeenInVehicleDuringWanted)
            {
                if (!Licenses.HasDriversLicense || !Licenses.DriversLicense.IsValid(TimeControllable))
                {
                    InitialAmount += Settings.SettingsManager.PoliceSettings.DrivingWithoutLicenseFineAmount;
                }
            }
            return InitialAmount;
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
        public void GangRelationshipsUpdate() => GangRelationships.Update();
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
        public void GiveMoney(int Amount)
        {
            if (Amount != 0)
            {
                LastChangeMoneyAmount = Amount;
                GameTimeLastChangedMoney = Game.GameTime;
            }
            int CurrentCash;
            uint PlayerCashHash;
            if (CharacterModelIsPrimaryCharacter)
            {
                PlayerCashHash = NativeHelper.CashHash(ModelName.ToLower());
            }
            else
            {
                PlayerCashHash = NativeHelper.CashHash(Settings.SettingsManager.PedSwapSettings.MainCharacterToAlias);
            }

            EntryPoint.WriteToConsole($"PlayerCashHash {PlayerCashHash} ModelName {ModelName}");

            //uint PlayerCashHash = NativeHelper.CashHash(Settings.SettingsManager.PedSwapSettings.MainCharacterToAlias);

            if (Settings.SettingsManager.PedSwapSettings.AliasPedAsMainCharacter || CharacterModelIsPrimaryCharacter)
            {

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
            else
            {
                if(money + Amount < 0)
                {
                    money = 0;
                }
                else
                {
                    money += Amount;
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
        public void LowerHands() => Surrendering.LowerHands();
        public void OnAppliedWantedStats(int wantedLevel) => Scanner.OnAppliedWantedStats(wantedLevel);
        public void OnGotOffFreeway()
        {
            GameFiber.Yield();
            if (IsWanted && AnyPoliceCanSeePlayer && TimeInCurrentVehicle >= 10000)
            {
                Scanner.OnGotOffFreeway();
            }
            EntryPoint.WriteToConsole($"PLAYER EVENT: OnGotOffFreeway (5 Second Delay)", 3);
        }
        public void OnGotOnFreeway()
        {
            GameFiber.Yield();
            if (IsWanted && AnyPoliceCanSeePlayer && TimeInCurrentVehicle >= 10000)
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
            if (IsWanted && AnyPoliceRecentlySeenPlayer && IsInVehicle && TimeInCurrentVehicle >= 5000)
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
            if (IsWanted && AnyPoliceRecentlySeenPlayer && IsInVehicle && TimeInCurrentVehicle >= 5000)
            {
                Scanner.OnVehicleStartedFire();
            }
            //EntryPoint.WriteToConsole($"PLAYER EVENT: OnVehicleStartedFire", 5);
        }
        public void OnWantedActiveMode() => Scanner.OnWantedActiveMode();
        public void OnWantedSearchMode() => Scanner.OnWantedSearchMode();
        public void OnWeaponsFree() => Scanner.OnWeaponsFree();
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
            if(UpperBodyActivity != null && UpperBodyActivity.CanCancel)
            {
                UpperBodyActivity.Cancel();
            }
            else if(LowerBodyActivity != null && LowerBodyActivity.CanCancel)
            {
                LowerBodyActivity.Cancel();
            }
        }
        public bool PayFine()
        {
            bool toReturn = Respawning.PayFine();
            if (toReturn)
            {
                Scanner.OnPaidFine();
            }
            return toReturn;
        }
        public void PayoffPolice()
        {
            Respawning.PayoffPolice();
            Scanner.OnBribedPolice();
        }
        public void PlayDispatchDebug(Crime toPlay, CrimeSceneDescription toAnnounce)
        {
            Scanner.Reset();
            Scanner.AnnounceCrime(toPlay, toAnnounce);
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
        public string PrintCriminalHistory() => CriminalHistory.PrintCriminalHistory();
        public void RaiseHands() => Surrendering.RaiseHands();
        public void ToggleSurrender()
        {
            if (Game.GameTime - GameTimeLastToggledSurrender >= 1000)
            {
                if (HandsAreUp)
                {
                    if (!IsBusted)
                    {
                        Surrendering.LowerHands();
                    }
                }
                else if (IsWavingHands)
                {
                    Surrendering.LowerHands();
                }
                else
                {
                    if (CanSurrender)
                    {
                        Surrendering.RaiseHands();
                    }
                    else if (CanWaveHands)
                    {
                        Surrendering.WaveHands();
                    }
                }
                GameTimeLastToggledSurrender = Game.GameTime;
            }
        }
        public void RemoveGPSRoute()
        {
            if (CurrentGPSBlip.Exists())
            {
                NativeFunction.Natives.SET_BLIP_ROUTE(CurrentGPSBlip, false);
                CurrentGPSBlip.Delete();
            }
        }
        public void RemoveOwnershipOfNearestCar()
        {
            VehicleExt toTakeOwnershipOf;
            if (CurrentVehicle != null && CurrentVehicle.Vehicle.Exists())
            {
                toTakeOwnershipOf = CurrentVehicle;
            }
            else
            {
                toTakeOwnershipOf = World.Vehicles.GetClosestVehicleExt(Character.Position, false, 10f);
            }
            if (toTakeOwnershipOf != null && toTakeOwnershipOf.Vehicle.Exists())
            {
                RemoveOwnershipOfVehicle(toTakeOwnershipOf);
                //DisplayPlayerNotification();
            }
            else
            {
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~b~Personal Info", string.Format("~y~{0}", PlayerName), "No Vehicle Found");
            }
        }
        public void RemoveOwnershipOfVehicle(VehicleExt toOwn)
        {
            if (toOwn != null && toOwn.Vehicle.Exists())
            {
                Blip attachedBlip = toOwn.Vehicle.GetAttachedBlip();
                if (attachedBlip.Exists())
                {
                    attachedBlip.Delete();
                }

                if (toOwn.AttachedBlip.Exists())
                {
                    toOwn.AttachedBlip.Delete();
                }

                toOwn.Vehicle.IsPersistent = false;
            }
            if (OwnedVehicles.Any(x => x.Handle == toOwn.Handle))
            {
                OwnedVehicles.Remove(toOwn);
            }
            UpdateOwnedBlips();
            EntryPoint.WriteToConsole($"PLAYER EVENT: OWNED VEHICLE REMOVED {toOwn.Vehicle.Handle}", 5);
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
        public void ResetScanner() => Scanner.Reset();
        public void ResetScannerDebug()
        {
            Scanner.Reset();
        }
        public void ResistArrest() => Respawning.ResistArrest();
        public void RespawnAtCurrentLocation(bool withInvicibility, bool resetWanted, bool clearCriminalHistory, bool clearInventory) => Respawning.RespawnAtCurrentLocation(withInvicibility, resetWanted, clearCriminalHistory, clearInventory);
        public void RespawnAtHospital(Hospital currentSelectedHospitalLocation) => Respawning.RespawnAtHospital(currentSelectedHospitalLocation);
        public void ScannerPlayDebug() => Scanner.DebugPlayDispatch();
        public void ScannerUpdate() => Scanner.Tick();
        public void SearchModeUpdate() => SearchMode.UpdateWanted();
        public void SetAngeredCop()
        {
            GameTimeLastFedUpCop = Game.GameTime;
        }
        public void SetArrestedAnimation(bool stayStanding) => Surrendering.SetArrestedAnimation(stayStanding);
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
        public void SetDemographics(string modelName, bool isMale, string playerName, int money)
        {
            ModelName = modelName;
            PlayerName = playerName;
            IsMale = isMale;
            SetMoney(money);
            EntryPoint.WriteToConsole($"PLAYER EVENT: SetDemographics MoneyToSet {money} Current: {Money} {NativeHelper.CashHash(Settings.SettingsManager.PedSwapSettings.MainCharacterToAlias)}", 3);
        }
        public void SetDenStatus(Gang gang, bool v)
        {
            World.Places.SetGangLocationActive(gang.ID, v);
        }
        public void SetMoney(int Amount)
        {
            uint PlayerCashHash;
            if (CharacterModelIsPrimaryCharacter)
            {
                PlayerCashHash = NativeHelper.CashHash(ModelName.ToLower());
            }
            else
            {
                PlayerCashHash = NativeHelper.CashHash(Settings.SettingsManager.PedSwapSettings.MainCharacterToAlias);
            }
           
            EntryPoint.WriteToConsole($"PlayerCashHash {PlayerCashHash} ModelName {ModelName}");
            if (Settings.SettingsManager.PedSwapSettings.AliasPedAsMainCharacter || CharacterModelIsPrimaryCharacter)
            {
                NativeFunction.CallByName<int>("STAT_SET_INT", PlayerCashHash, Amount, 1);
            }
            else
            {
                money = Amount;
            }
        }
        public void SetPlayerToLastWeapon()
        {
            if (Game.LocalPlayer.Character.Inventory.EquippedWeapon != null && LastWeaponHash != 0)
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)LastWeaponHash, true);
                //EntryPoint.WriteToConsole("SetPlayerToLastWeapon" + LastWeaponHash.ToString());
            }
        }
        public void SetSelector(SelectorOptions eSelectorSetting) => WeaponSelector.SetSelectorSetting(eSelectorSetting);
        public void SetUnarmed()
        {
            if (!(Game.LocalPlayer.Character.Inventory.EquippedWeapon == null))
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)2725352035, true); //Unequip weapon so you don't get shot
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
        public void ShootAt(Vector3 TargetCoordinate)
        {
            NativeFunction.CallByName<bool>("SET_PED_SHOOTS_AT_COORD", Game.LocalPlayer.Character, TargetCoordinate.X, TargetCoordinate.Y, TargetCoordinate.Z, true);
            GameTimeLastShot = Game.GameTime;
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
                        Game.DisplayNotification($"Cannot Use Item {modItem.Name}, Requires {modItem.RequiredToolType}");
                        return;
                    }
                }

                if (modItem.PercentLostOnUse > 0.0f)
                {
                    UseInventoryItem(modItem);
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
                    ChangeHealth(modItem.HealthChangeAmount);
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
        public void StartLayingDown(bool FindSittingProp)
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
                LowerBodyActivity = new LayingActivity(this, Settings, FindSittingProp);
                LowerBodyActivity.Start();
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
                LowerBodyActivity = new SittingActivity(this, Settings, findSittingProp, enterForward);
                LowerBodyActivity.Start();
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
        public void StopDynamicActivity()
        {
            if (IsPerformingActivity)
            {
                UpperBodyActivity?.Cancel();
                IsPerformingActivity = false;
            }
        }
        public void SurrenderToPolice(PoliceStation currentSelectedSurrenderLocation) => Respawning.SurrenderToPolice(currentSelectedSurrenderLocation);
        public void TakeOwnershipOfNearestCar()
        {
            VehicleExt toTakeOwnershipOf;
            if (CurrentVehicle != null && CurrentVehicle.Vehicle.Exists())
            {
                toTakeOwnershipOf = CurrentVehicle;
            }
            else
            {
                toTakeOwnershipOf = World.Vehicles.GetClosestVehicleExt(Character.Position, false, 10f);
            }
            if (toTakeOwnershipOf != null && toTakeOwnershipOf.Vehicle.Exists())
            {
                TakeOwnershipOfVehicle(toTakeOwnershipOf, true);
                //DisplayPlayerNotification();
            }
            else
            {
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~b~Personal Info", string.Format("~y~{0}", PlayerName), "No Vehicle Found");
            }
        }
        public void TakeOwnershipOfVehicle(VehicleExt toOwn, bool showNotification)
        {
            if (toOwn != null && toOwn.Vehicle.Exists() && !OwnedVehicles.Any(x => x.Handle == toOwn.Handle))
            {
                toOwn.SetNotWanted();
                toOwn.Vehicle.IsStolen = false;
                toOwn.Vehicle.IsPersistent = true;

                OwnedVehicles.Add(toOwn);
                UpdateOwnedBlips();
                if (showNotification)
                {
                    DisplayPlayerVehicleNotification(toOwn);
                }
                EntryPoint.WriteToConsole($"PLAYER EVENT: OWNED VEHICLE ADDED {toOwn.Vehicle.Handle}", 5);
            }
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
        public void ToggleSelector() => WeaponSelector.ToggleSelector();
        public void ToggleActionMode()
        {
            bool isUsingActionMode = NativeFunction.Natives.IS_PED_USING_ACTION_MODE<bool>(Character);
            NativeFunction.Natives.SET_PED_USING_ACTION_MODE(Character, !isUsingActionMode, -1, "DEFAULT_ACTION");
        }
        public void ToggleStealthMode()
        {
            bool isUsingStealthMode = NativeFunction.Natives.GET_PED_STEALTH_MOVEMENT<bool>(Character);
            NativeFunction.Natives.SET_PED_STEALTH_MOVEMENT(Character, !isUsingStealthMode, "DEFAULT_ACTION");
        }
        public void TrafficViolationsUpdate() => Violations.UpdateTraffic();
        public void UnSetArrestedAnimation()
        {
            //if (HandsAreUp)
            //{
                Surrendering.UnSetArrestedAnimation();
            //}
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
                HealthState.MyPed = new PedExt(Game.LocalPlayer.Character, Settings, Crimes, Weapons, PlayerName, "Person");
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

            //GameFiber.Yield();//TR Yield RemovedTest 1
            if (!IsMovingFast && IsAliveAndFree && !IsConversing)
            {
                float ClosestDistance = 999f;
                ClosestInteractableLocation = null;
                ClosestDistance = 999f;
                foreach (InteractableLocation gl in World.Places.ActiveInteractableLocations)// PlacesOfInterest.GetAllStores())
                {
                    if (gl.DistanceToPlayer <= 3.0f && gl.CanInteract && !IsInteractingWithLocation)
                    {
                        if (gl.DistanceToPlayer < ClosestDistance)
                        {
                            ClosestInteractableLocation = gl;
                            ClosestDistance = gl.DistanceToPlayer;
                        }
                    }
                }



                //if(ClosestInteractableLocation != null && ClosestInteractableLocation.HasVendor)
                //{
                //    float VendorPositionDistanceToPlayer = ClosestInteractableLocation.VendorPosition.DistanceTo(Character);
                //    if (VendorPositionDistanceToPlayer <= 3.0f && (ClosestInteractableLocation.Merchant?.IsDead == true || ClosestInteractableLocation.Merchant?.IsUnconscious == true || ClosestInteractableLocation.Merchant?.HasBeenMugged == true || ClosestInteractableLocation.Merchant?.DistanceToPlayer >= 50f))
                //    {
                //        ClosestInteractableLocation.CanInteract = true;
                //        ClosestInteractableLocation.VendorAbandoned = true;
                //    }
                //    else
                //    {
                //        ClosestInteractableLocation.CanInteract = false;
                //        ClosestInteractableLocation.VendorAbandoned = false;
                //    }
                //}
            }
            //GameFiber.Yield();//TR Yield RemovedTest 1
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
            if (CurrentGPSBlip.Exists())
            {
                if (CurrentGPSBlip.DistanceTo2D(Position) <= 30f)
                {
                    NativeFunction.Natives.SET_BLIP_ROUTE(CurrentGPSBlip, false);
                    CurrentGPSBlip.Delete();
                }
                else
                {
                    if (GameTimeLastCheckedRouteBlip == 0 || Game.GameTime - GameTimeLastCheckedRouteBlip >= 10000)
                    {
                        NativeFunction.Natives.SET_BLIP_ROUTE(CurrentGPSBlip, true);
                        GameTimeLastCheckedRouteBlip = Game.GameTime;
                    }
                }
            }
            if (IsWavingHands)
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
            else if (HandsAreUp)
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



            if(IsInVehicle)
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
            //GameFiber.Yield();//TR Yield RemovedTest 1
        }
        public void UpdateVehicleData()
        {
            IsInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
            IsGettingIntoAVehicle = Game.LocalPlayer.Character.IsGettingIntoVehicle;
            //IsGettingOutOfAVehicle = NativeFunction.Natives.IS_PED_JUMPING_OUT_OF_VEHICLE<bool>(Character);
            if (IsInVehicle)
            {
                if (Character.CurrentVehicle.Exists() && OwnedVehicles.Any(x => x.Vehicle.Exists() && x.Vehicle.Handle == Character.CurrentVehicle.Handle))//OwnedVehicle != null && OwnedVehicle.Vehicle.Exists() && Character.CurrentVehicle.Handle == OwnedVehicle.Vehicle.Handle)
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


                    //bool isValid = NativeFunction.Natives.x645F4B6E8499F632<bool>(CurrentVehicle.Vehicle, 0);
                    //if (isValid)
                    //{
                    //    CurrentVehicle.Vehicle.HasBone("door_dside_f");
                    //    float DoorAngle = NativeFunction.Natives.GET_VEHICLE_DOOR_ANGLE_RATIO<float>(CurrentVehicle.Vehicle, 0);
                    //    if(DoorAngle >= 0.0001f)
                    //    {
                    //        DriverDoorOpen = true;
                    //    }
                    //    else
                    //    {
                    //        DriverDoorOpen = false;
                    //    }
                    //}
                    //else
                    //{
                    //    DriverDoorOpen = false;
                    //}
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
            UpdateOwnedBlips();






            //if(IsHotWiring && CurrentVehicle != null && CurrentVehicle.Vehicle.Exists() && Game.GameTime - GameTimeStartedHotwiring <= 15000)
            //{
            //    CurrentVehicle.Vehicle.MustBeHotwired = true;
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
            //GameFiber.Yield();//TR Yield RemovedTest 1
            WeaponDescriptor PlayerCurrentWeapon = Game.LocalPlayer.Character.Inventory.EquippedWeapon;

            if (PlayerCurrentWeapon != null)
            {
                CurrentWeaponHash = PlayerCurrentWeapon.Hash;
                CurrentWeapon = Weapons.GetCurrentWeapon(Game.LocalPlayer.Character);
                GameFiber.Yield();
                if (CurrentWeapon != null && CurrentWeapon.Category != WeaponCategory.Melee)
                {
                    CurrentWeaponMagazineSize = PlayerCurrentWeapon.MagazineSize;
                }
                else
                {
                    CurrentWeaponMagazineSize = 0;
                }
            }
            else
            {
                CurrentWeaponMagazineSize = 0;
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

            if (Settings.SettingsManager.PlayerOtherSettings.MeleeDamageModifier != 1.0f && (GameTimeLastSetMeleeModifier == 0 || Game.GameTime - GameTimeLastSetMeleeModifier >= 5000))
            {
                NativeFunction.Natives.SET_PLAYER_MELEE_WEAPON_DAMAGE_MODIFIER(Game.LocalPlayer, Settings.SettingsManager.PlayerOtherSettings.MeleeDamageModifier, true);
                GameTimeLastSetMeleeModifier = Game.GameTime;
            }

            if (Settings.SettingsManager.PlayerOtherSettings.AllowWeaponDropping)
            {
                WeaponDropping.Update();
            }
            UpdateTargetedPed();
            GameFiber.Yield();
            UpdateLookedAtPed();
            GameFiber.Yield();


            IsShooting = RecentlyShot;


        }
        public bool UseInventoryItem(ModItem modItem) => Inventory.Use(modItem);
        public void ViolationsUpdate() => Violations.Update();
        public void WaveHands() => Surrendering.WaveHands();
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
                        if(ModelName.ToLower() == "player_zero")
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
                //if (CurrentVehicle != null)
                //{
                //    CurrentVehicle.SetDriverWindow(true);
                //}

                if (CurrentWeapon == null)
                {
                    IsMakingInsultingGesture = true;
                }
                else
                {
                    EntryPoint.WriteToConsole($"CurrentWeapon {CurrentWeapon.ModelName}", 5);
                }
            }
            else
            {
                //if (CurrentVehicle != null)
                //{
                //    CurrentVehicle.SetDriverWindow(false);
                //}

                IsMakingInsultingGesture = false;
            }
            //EntryPoint.WriteToConsole($"PLAYER EVENT: IsAimingInVehicle Changed to: {IsAimingInVehicle}", 5);
        }
        private void OnExcessiveSpeed()
        {
            GameFiber.Yield();
            if (IsWanted && VehicleSpeedMPH >= 75f && AnyPoliceCanSeePlayer && TimeInCurrentVehicle >= 10000 && !isCheckingExcessSpeed)
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
                    if (OwnedVehicles.Any(x => x.Handle == CurrentVehicle.Handle) && CurrentVehicle.Vehicle.Exists())//if (OwnedVehicle != null && CurrentVehicle.Handle == OwnedVehicle.Handle && CurrentVehicle.Vehicle.Exists())
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

                        if(Settings.SettingsManager.VehicleSettings.RequireScrewdriverForLockPickEntry && !hasScrewDriver && IsNotHoldingEnter && VehicleTryingToEnter.Driver == null && VehicleTryingToEnter.LockStatus == (VehicleLockStatus)7 && !VehicleTryingToEnter.IsEngineOn)
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
                            Violations.AddCarJacked(jackedPed);
                            CarJack MyJack = new CarJack(this, CurrentVehicle, jackedPed, SeatTryingToEnter, CurrentWeapon);
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
                if (IsWanted && AnyPoliceCanSeePlayer)
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
                if (IsWanted && AnyPoliceCanSeePlayer && !IsRagdoll)
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
            HandsAreUp = false;
            if (WantedLevel > 1)
            {
                Surrendering.SetArrestedAnimation(WantedLevel <= 2);//needs to move
            }

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
                    GangRelationships.OnLostWanted();
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
                    GangRelationships.OnBecameWanted();
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
                                if (IsDriver && !OwnedVehicles.Any(x => x.Handle == existingVehicleExt.Handle))// == null || existingVehicleExt.Handle != OwnedVehicle.Handle))
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
        private void UpdateData()
        {
            UpdateVehicleData();
            GameFiber.Yield();
            UpdateWeaponData();
            GameFiber.Yield();
            UpdateStateData();
            GameFiber.Yield();

            bool IntoxicationIsPrimary = false;
            if(Intoxication.CurrentIntensity > Injuries.CurrentIntensity)
            {
                IntoxicationIsPrimary = true;
            }


            Intoxication.Update(IntoxicationIsPrimary);
            GameFiber.Yield();//TR Yield RemovedTest 1
            Injuries.Update(!IntoxicationIsPrimary);
            GameFiber.Yield();//TR Yield RemovedTest 1
            HumanState.Update();




        }
        private void UpdateLookedAtPed()
        {
            if (Game.GameTime - GameTimeLastUpdatedLookedAtPed >= 750)//750)//750
            {
                GameFiber.Yield();

                //Works fine just going simpler
                Vector3 RayStart = Game.LocalPlayer.Character.GetBonePosition(PedBoneId.Head);
                // Vector3 RayStart = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
                Vector3 RayEnd = RayStart + NativeHelper.GetGameplayCameraDirection() * 6.0f;
                //Vector3 RayStart = Game.LocalPlayer.Character.GetBonePosition(PedBoneId.Head);
                //Vector3 RayEnd = RayStart + Game.LocalPlayer.Character.Direction * 5.0f;



                HitResult result = Rage.World.TraceCapsule(RayStart, RayEnd, 1f, TraceFlags.IntersectVehicles | TraceFlags.IntersectPeds, Game.LocalPlayer.Character);//2 meter wide cylinder out 10 meters that ignores the player charater going from the head in the players direction
                                                                                                                                                                                     //  Rage.Debug.DrawArrowDebug(RayStart, Game.LocalPlayer.Character.Direction, Rotator.Zero, 1f, Color.White);
                                                                                                                                                                                     //  Rage.Debug.DrawArrowDebug(RayEnd, Game.LocalPlayer.Character.Direction, Rotator.Zero, 1f, Color.Red);


                //HitResult result = Rage.World.TraceCapsule(RayStart, RayEnd, 1f, TraceFlags.IntersectVehicles | TraceFlags.IntersectPedsSimpleCollision, Game.LocalPlayer.Character);//2 meter wide cylinder out 10 meters that ignores the player charater going from the head in the players direction
                //                                                                                                                                                                     //  Rage.Debug.DrawArrowDebug(RayStart, Game.LocalPlayer.Character.Direction, Rotator.Zero, 1f, Color.White);
                //                                                                                                                                                                     //  Rage.Debug.DrawArrowDebug(RayEnd, Game.LocalPlayer.Character.Direction, Rotator.Zero, 1f, Color.Red);
                if (result.Hit && result.HitEntity is Ped)
                {
                    //EntryPoint.WriteToConsole("Hit Ped in Looked At");
                    // Rage.Debug.DrawArrowDebug(result.HitPosition, Game.LocalPlayer.Character.Direction, Rotator.Zero, 1f, Color.Green);
                    CurrentLookedAtPed = World.Pedestrians.GetPedExt(result.HitEntity.Handle);
                    CurrentLookedAtVehicle = null;
                }
                else if (result.Hit && result.HitEntity is Vehicle)
                {
                    Vehicle myCar = (Vehicle)result.HitEntity;
                    if(myCar.Exists())
                    {
                        CurrentLookedAtVehicle = World.Vehicles.GetVehicleExt(myCar);
                    }
                    else
                    {
                        CurrentLookedAtVehicle = null;
                    }
                    if(myCar.Exists() && myCar.Driver.Exists())
                    {
                        Ped closestPed = null;
                        float ClosestDistance = 999f;
                        foreach(Ped occupant in myCar.Occupants)
                        {
                            if(occupant.Exists())
                            {
                                float distanceTo = occupant.DistanceTo2D(Character);
                                if (distanceTo <= ClosestDistance)
                                {
                                    closestPed = occupant;
                                    ClosestDistance = distanceTo;
                                }
                            }
                                
                        }
                        if(closestPed.Exists())
                        {
                            CurrentLookedAtPed = World.Pedestrians.GetPedExt(closestPed.Handle);
                        }

                        
                    }
                    else
                    {
                        CurrentLookedAtPed = null;
                    }
                    // Rage.Debug.DrawArrowDebug(result.HitPosition, Game.LocalPlayer.Character.Direction, Rotator.Zero, 1f, Color.Green);
                    
                }
                else
                {
                    CurrentLookedAtVehicle = null;
                    CurrentLookedAtPed = null;
                }

                //CurrentLookedAtPed = EntityProvider.CivilianList.Where(x => x.DistanceToPlayer <= 4f && !x.IsBehindPlayer).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
                GameTimeLastUpdatedLookedAtPed = Game.GameTime;

                GameFiber.Yield();
            }
        }
        private void UpdateOwnedBlips()
        {
            //EntryPoint.WriteToConsole($"PLAYER EVENT: UpdateOwnedBlips CurrentVehicle {CurrentVehicle != null}", 5);
            foreach (VehicleExt car in OwnedVehicles)
            {
                if (car.Vehicle.Exists())
                {
                    if (CurrentVehicle?.Handle == car.Handle)
                    {
                        if (car.AttachedBlip.Exists())
                        {
                            car.AttachedBlip.Delete();
                        }
                    }
                    else
                    {
                        if (!car.AttachedBlip.Exists())
                        {
                            car.AttachedBlip = car.Vehicle.AttachBlip();
                            car.AttachedBlip.Sprite = BlipSprite.GetawayCar;
                            car.AttachedBlip.Color = System.Drawing.Color.Red;
                        }
                    }
                }
            }
            //}
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
                || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)0x23C9F95C//weapon_ball

                || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)2508868239//bat

                || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)4192643659//bottle
                || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)2227010557//crowbar
                || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)1141786504//golf club
                || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)1317494643//hammer

                || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)0x94117305//pool cue
                || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)0x19044EE0//wrench

                )//weapon_ball

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