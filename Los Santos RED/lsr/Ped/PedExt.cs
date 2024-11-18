using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Mod;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

public class PedExt : IComplexTaskable, ISeatAssignable
{
    public IPoliceRespondable PlayerToCheck;
    protected ISettingsProvideable Settings;
    protected uint GameTimeSpawned;
    private uint GameTimeCreated = 0;
    private uint GameTimeLastEnteredVehicle;
    private uint GameTimeLastExitedVehicle;
    private uint GameTimeLastInsultedByPlayer;
    private uint GameTimeLastMovedFast;
    private bool hasCheckedWeapon = false;
    private Entity Killer;
    private uint KillerHandle;
    private Entity LastHurtBy;
    private Vector3 position;
    private uint UpdateJitter;
    private int TimeBetweenYelling = 5000;
    private uint GameTimeLastYelled;
    private RelationshipGroup originalGroup;
    private bool HasDrugAreaKnowledge;
    private bool HasGangAreaKnowledge;
    protected ICrimes Crimes;
    private uint GameTimeFirstSeenDead;
    private uint GameTimeFirstSeenUnconscious;
    private uint GameTimeLastReportedCrime;
    private uint GameTimeLastTooCloseToPlayer;
    protected uint GameTimeLastCollidedWithPlayer;
    protected uint GameTimePlayerLastDidBodilyFunctionsNear;
    protected bool HasGivenTooCloseWarning;
    protected uint GameTimePlayerLastDamagedCarOnFoot;
    protected uint GameTimePlayerLastStoodOnCar;

    private bool IsYellingTimeOut => Game.GameTime - GameTimeLastYelled < TimeBetweenYelling;
    private bool CanYell => !IsYellingTimeOut;
    public PedExt(Ped _Pedestrian, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, string _Name, string groupName, IEntityProvideable world)
    {
        Pedestrian = _Pedestrian;
        Handle = Pedestrian.Handle;
        Health = Pedestrian.Health;
        SpawnPosition = Pedestrian.Position;
        Name = _Name;
        GameTimeCreated = Game.GameTime;
        GroupName = groupName;
        Crimes = crimes;
        CurrentHealthState = new HealthState(this, settings, false);
        Settings = settings;

        //if (WasModSpawned)
        //{
            GameTimeSpawned = Game.GameTime;
        //}

        DistanceChecker = new DistanceChecker(Settings);
        PedViolations = new PedViolations(this, crimes, settings, weapons, world);
        PedPerception = new PedPerception(this, crimes, settings, weapons, world);
        PlayerPerception = new PlayerPerception(this, null, settings);
        PedReactions = new PedReactions(this);
        PedInventory = new SimpleInventory(Settings);
        PedBrain = new PedBrain(this, settings, world, weapons);
        ItemDesires = new ItemDesires();
        PedAlerts = new PedAlerts(this,settings);
        //PedKnowledge = new PedKnowledge(this, settings);
        IsTrustingOfPlayer = RandomItems.RandomPercent(Settings.SettingsManager.CivilianSettings.PercentageTrustingOfPlayer);
        HasDrugAreaKnowledge = RandomItems.RandomPercent(Settings.SettingsManager.CivilianSettings.PercentageKnowsAnyDrugTerritory);
        HasGangAreaKnowledge = RandomItems.RandomPercent(Settings.SettingsManager.CivilianSettings.PercentageKnowsAnyGangTerritory);
        UpdateJitter = RandomItems.GetRandomNumber(100, 200);
        CowerDistance = RandomItems.GetRandomNumber(20f, 45f);
    }
    public PedViolations PedViolations { get; private set; }
    public PedPerception PedPerception { get; private set; }
    public PlayerPerception PlayerPerception { get; private set; }
    public HealthState CurrentHealthState { get; private set; }
    public PedReactions PedReactions { get; set; }
    public SimpleInventory PedInventory { get; private set; }
    public PedBrain PedBrain { get; set; }
    public ItemDesires ItemDesires { get; private set; }
    public PedAlerts PedAlerts { get; private set; }
    public DistanceChecker DistanceChecker { get; private set; }
    public uint ArrestingPedHandle { get; set; } = 0;
    public List<Cop> AssignedCops { get; set; } = new List<Cop>();
    public int AssignedSeat { get; set; }
    public VehicleExt AssignedVehicle { get; set; }
    public Vector3 Position => position;
    public List<uint> BlackListedVehicles { get; set; } = new List<uint>();
    public bool HasCalledInCrimesRecently => GameTimeLastReportedCrime != 0 && Game.GameTime - GameTimeLastReportedCrime <= 15000;
    public uint HasBeenSpawnedFor => Game.GameTime - GameTimeSpawned;
    public bool CanAttackPlayer => IsFedUpWithPlayer || HatesPlayer;
    public bool CanBeAmbientTasked { get; set; } = true;
    public bool CanBeMugged => !IsCop && Pedestrian.Exists() && !IsBusted && !IsUnconscious && !IsDead && !IsArrested && Pedestrian.IsAlive && !Pedestrian.IsStunned && !Pedestrian.IsRagdoll && !Pedestrian.IsInCombat && (!Pedestrian.IsPersistent || Settings.SettingsManager.CivilianSettings.AllowMissionPedsToInteract || IsMerchant || IsGangMember || WasModSpawned);
    public bool CanBeTasked { get; set; } = true;
    public virtual bool CanConverse => Pedestrian.Exists() && !IsBusted && !IsUnconscious && !IsDead && !IsArrested && !IsCowering && Pedestrian.IsAlive && !Pedestrian.IsFleeing && !Pedestrian.IsInCombat && !Pedestrian.IsSprinting && !Pedestrian.IsStunned && !Pedestrian.IsRagdoll 
        && (!Pedestrian.IsPersistent || Settings.SettingsManager.CivilianSettings.AllowMissionPedsToInteract || IsCop || IsMerchant || IsGangMember || WasModSpawned);
    public virtual bool CanFlee => Pedestrian.Exists() && CanBeTasked && CanBeAmbientTasked && !IsBusted && !IsUnconscious && !IsDead && !IsArrested && Pedestrian.IsAlive && !Pedestrian.IsStunned && !Pedestrian.IsRagdoll;
    public bool CanRecognizePlayer => PlayerPerception.CanRecognizeTarget;
    public bool CanRemove
    {
        get
        {
            if (!Pedestrian.Exists())
            {
                return true;
            }
            else if (Pedestrian.IsDead && CurrentHealthState.HasLoggedDeath)// && DistanceToPlayer >= 250f)
            {
                return true;
            }
            return false;
        }
    }
    public virtual bool IsTrustingOfPlayer { get; set; } = true;
    public virtual bool CanTransact => HasMenu;
    public bool CanSeePlayer => PlayerPerception.CanSeeTarget;
    public bool RecentlySeenPlayer => PlayerPerception.RecentlySeenTarget;
    public bool IsCowering { get; set; }
    public bool IsLSRFleeing => IsCowering || (Pedestrian.Exists() && Pedestrian.IsFleeing);
    public int CellX { get; set; }
    public int CellY { get; set; }
    public bool CanSurrender { get; set; } = false;
    public float ClosestDistanceToPlayer => PlayerPerception.ClosestDistanceToTarget;
    public List<Crime> CrimesCurrentlyViolating => PedViolations.CrimesCurrentlyViolating;
    public int CurrentlyViolatingWantedLevel => PedViolations.CurrentlyViolatingWantedLevel;
    public ComplexTask CurrentTask { get; set; }
    public string DebugString => $"Handle: {Pedestrian.Handle} Distance {PlayerPerception.DistanceToTarget} See {PlayerPerception.CanSeeTarget} Md: {Pedestrian.Model.Name} Task: {CurrentTask?.Name} SubTask: {CurrentTask?.SubTaskName} InVeh {IsInVehicle}";
    public float DistanceToPlayer => PlayerPerception.DistanceToTarget;
    public float HeightToPlayer => PlayerPerception.HeightToTarget;
    public bool EverSeenPlayer => PlayerPerception.EverSeenTarget;
    public string FormattedName => (PlayerKnownsName ? Name : GroupName);
    public virtual int ShootRate { get; set; } = 400;
    public virtual int Accuracy { get; set; } = 5;
    public virtual int CombatAbility { get; set; } = 0;
    public virtual int CombatRange { get; set; } = -1;
    public virtual int CombatMovement { get; set; } = -1;
    public virtual int TaserAccuracy { get; set; } = 10;
    public virtual int TaserShootRate { get; set; } = 100;
    public virtual int VehicleAccuracy { get; set; } = 10;
    public virtual int VehicleShootRate { get; set; } = 100;
    public virtual int TurretAccuracy { get; set; } = 10;
    public virtual int TurretShootRate { get; set; } = 1000;
    public virtual bool IsAnimal { get; set; } = false;
    public virtual int DefaultCombatFlag { get; set; } = 0;
    public virtual int DefaultEnterExitFlag { get; set; } = 0;
    public virtual string InteractPrompt(IButtonPromptable player)
    {
        bool toSell = false;
        bool toSellPlayerHas = false;
        bool toBuy = false;
        if (HasMenu)
        {
            toSell = ShopMenu.Items.Any(x => x.Sellable);
            toBuy = ShopMenu.Items.Any(x => x.Purchaseable);
            toSellPlayerHas = ShopMenu.Items.Any(x => x.Sellable && player.Inventory.Get(x.ModItem) != null && x.NumberOfItemsToPurchaseFromPlayer > 0);
        }
        string promptText;
        if (toSell && toBuy)
        {
            promptText = $"Transact with";
        }
        else if (toBuy)
        {
            promptText = $"Buy from";
        }
        else if (toSell)
        {
            promptText = $"Sell to";
        }
        else
        {
            promptText = $"Talk to";
        }
        promptText += $" {FormattedName}";
        if (toSellPlayerHas && Settings.SettingsManager.ActivitySettings.ShowInPromptWhenPedsWantToBuyItemsYouHave)
        {
            promptText += " (!)";
        }
        return promptText;
    }
    public string TransactionPrompt(IButtonPromptable player)
    {
        bool toSell = false;
        bool toSellPlayerHas = false;
        bool toBuy = false;
        if (HasMenu)
        {
            toSell = ShopMenu.Items.Any(x => x.Sellable);
            toBuy = ShopMenu.Items.Any(x => x.Purchaseable);
            toSellPlayerHas = ShopMenu.Items.Any(x => x.Sellable && player.Inventory.Get(x.ModItem) != null && x.NumberOfItemsToPurchaseFromPlayer > 0);
        }
        string promptText;
        if (toSell && toBuy)
        {
            promptText = $"Transact with";
        }
        else if (toBuy)
        {
            promptText = $"Buy from";
        }
        else if (toSell)
        {
            promptText = $"Sell to";
        }
        else
        {
            promptText = $"Transact with";
        }
        promptText += $" {FormattedName}";
        if (toSellPlayerHas && Settings.SettingsManager.ActivitySettings.ShowInPromptWhenPedsWantToBuyItemsYouHave)
        {
            promptText += " (!)";
        }
        return promptText;
    }
    public string GroupName { get; set; } = "Person";
    public uint GameTimeLastUpdated { get; set; }
    public uint GameTimeLastUpdatedTask { get; set; }
    public uint Handle { get; private set; }
    public bool ShouldSurrender { get; set; }
    public virtual bool ShowsItemPreviewsWhenTransacting { get; set; }
    public virtual ePedAlertType PedAlertTypes { get; set; } = ePedAlertType.UnconsciousBody;
    public virtual bool GenerateUnconsciousAlerts { get; set; } = true;
    public bool HasCellPhone { get; set; } = true;
    public bool HasIdentification { get; set; } = true;
    public bool HasBeenCarJackedByPlayer { get; set; } = false;
    public bool HasBeenHurtByPlayer { get; set; } = false;
    public bool WasKilledByPlayer { get; set; } = false;
    public bool HasBeenMugged { get; set; } = false;
    public uint HasExistedFor => Game.GameTime - GameTimeCreated;
    public bool HasLoggedDeath => CurrentHealthState.HasLoggedDeath;
    public bool HasMenu => ShopMenu != null && ShopMenu.Items != null && ShopMenu.Items.Any();// TransactionMenu != null && TransactionMenu.Any();
    public bool HasSeenPlayerCommitCrime => PlayerPerception.PlayerCrimesWitnessed.Any();
    public bool HasBeenTreatedByEMTs { get; set; }
    public bool HasSeenPlayerCommitMajorCrime => PlayerPerception.PlayerCrimesWitnessed.Any(x => x.Crime.AngersCivilians || x.Crime.ScaresCivilians);
    public bool HasSeenPlayerCommitTrafficCrime => PlayerPerception.PlayerCrimesWitnessed.Any(x => x.Crime.IsTrafficViolation);
    public bool PlayerKnownsName { get; set; }
    public bool HatesPlayer { get; set; } = false;
    public int Health { get; set; }
    public virtual int InsultLimit => 3;
    public virtual int CollideWithPlayerLimit => 2;
    public virtual int PlayerStandTooCloseLimit => 2;
    public bool IsFedUpWithPlayer => TimesInsultedByPlayer >= InsultLimit || TimesCollidedWithPlayer > CollideWithPlayerLimit || TimesPlayerStoodTooClose > PlayerStandTooCloseLimit;
    public int TimesInsultedByPlayer { get; protected set; }
    public int TimesCollidedWithPlayer { get; private set; }
    public int TimesPlayerStoodTooClose { get; private set; }
    public bool IsArrested { get; set; }
    public bool IsBusted { get; set; } = false;
    public bool IsCop { get; set; } = false;
    public bool IsDeadlyChase => PedViolations.IsDeadlyChase;
    public bool IsDealingDrugs { get; set; } = false;
    public bool IsDealingIllegalGuns { get; set; } = false;
    public bool IsSpeeding { get; set; } = false;
    public bool IsDrivingRecklessly { get; set; } = false;
    public bool IsDriver { get; private set; } = false;
    public bool IsDrunk { get; set; } = false;
    public bool IsFreeModePed { get; set; } = false;
    public virtual bool IsGangMember { get; set; } = false;
    public bool IsInAPC { get; private set; }
    public bool IsInBoat { get; private set; } = false;
    public bool IsWaitingAtTrafficLight { get; set; }
    public bool IsTurningLeftAtTrafficLight { get; set; }
    public bool IsTurningRightAtTrafficLight { get; set; }
    public bool IsInHelicopter { get; private set; } = false;
    public bool IsInPlane { get; private set; } = false;
    public bool IsInAirVehicle => IsInPlane || IsInHelicopter;
    public bool IsInVehicle { get; private set; } = false;
    public bool IsInWrithe { get; set; } = false;
    public virtual bool IsMerchant { get; set; } = false;
    public bool IsMovingFast => GameTimeLastMovedFast != 0 && Game.GameTime - GameTimeLastMovedFast <= 2000;
    public bool IsNearSpawnPosition => Pedestrian.Exists() && Pedestrian.DistanceTo2D(SpawnPosition) <= 10f;////15f;//15f
    public bool IsOnBike { get; private set; } = false;
    public bool IsRunningOwnFiber { get; set; } = false;
    public bool IsStill { get; private set; }
    public bool IsSuicidal { get; set; } = false;
    public bool IsUnconscious { get; set; }
    public virtual bool AutoCallsInUnconsciousPeds { get; set; } = false;
    public bool IsLocationSpawned { get; set; } = false;
    public bool IsSuspicious { get; set; } = false;
    public bool IsWanted => PedViolations.IsWanted;
    public bool IsNotWanted => PedViolations.IsNotWanted;
    public bool IsZombie { get; set; } = false;
    public int LastSeatIndex { get; private set; } = -1;
    public int Money { get; set; } = 10;
    public bool StayInVehicle { get; set; } = false;
    public string Name { get; set; } = "Unknown";
    public virtual bool NeedsFullUpdate
    {
        get
        {
            if (GameTimeLastUpdated == 0)
            {
                return true;
            }
            else if (Game.GameTime > GameTimeLastUpdated + FullUpdateInterval)// + UpdateJitter)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public virtual System.Drawing.Color BlipColor => System.Drawing.Color.White;
    public virtual float BlipSize => 0.6f;
    public bool NeedsTaskAssignmentCheck => Game.GameTime - GameTimeLastUpdatedTask >= Settings.SettingsManager.PerformanceSettings.TaskAssignmentCheckFrequency;// (IsCop ? 500 : 700);
    public List<WitnessedCrime> OtherCrimesWitnessed => PedPerception.NPCCrimesWitnessed;
    public Ped Pedestrian { get; set; }
    public virtual float CowerDistance { get; set; } = 40f;
    public List<WitnessedCrime> PlayerCrimesWitnessed => PlayerPerception.PlayerCrimesWitnessed;
    public List<WitnessedCrime> CrimesWitnessed
    {
        get
        {
            List<WitnessedCrime> myList = new List<WitnessedCrime>();
            myList.AddRange(PlayerPerception.PlayerCrimesWitnessed);
            myList.AddRange(PedPerception.NPCCrimesWitnessed);
            return myList;
        }
    }
    public Vector3 PositionLastSeenCrime => PlayerPerception.PositionLastSeenCrime;
    public bool RecentlyGotInVehicle => GameTimeLastEnteredVehicle != 0 && Game.GameTime - GameTimeLastEnteredVehicle <= 2000;//was 4000//was 1000
    public bool RecentlyGotOutOfVehicle => GameTimeLastExitedVehicle != 0 && Game.GameTime - GameTimeLastExitedVehicle <= 2000;//was 4000//was 1000
    public bool RecentlyUpdated => GameTimeLastUpdated != 0 && Game.GameTime - GameTimeLastUpdated < 2000;
    public int RelationShipFromPlayer { get; set; } = 255;
    public int RelationShipToPlayer { get; set; } = 255;
    public bool ShouldCheckCrimes
    {
        get
        {
            if (Pedestrian.Exists())
            {
                string RelationshipGroupName = Pedestrian.RelationshipGroup.Name;//weirdness withthis bullshit
                if (RelationshipGroupName == string.Empty)
                {
                    //EntryPoint.WriteToConsoleTestLong($" PedExt.Pedestrian {Pedestrian.Handle} RelationshipGroupName {RelationshipGroupName} RelationshipGroupName2 A{RelationshipGroupName}A");
                    RelationshipGroupName = RelationshipGroupName.ToUpper();
                }
                if (RelationshipGroupName == "SECURITY_GUARD" || RelationshipGroupName == "SECURITY_GUARDS" || RelationshipGroupName == "PRIVATE_SECURITY" || RelationshipGroupName == "FIREMAN" || RelationshipGroupName == "MEDIC" || RelationshipGroupName == "RANGE_IGNORE" || RelationshipGroupName == "range_IGNORE")
                {
                    return false;
                }
                else if (RelationshipGroupName == "")
                {
                    return true;
                }
                else if (RelationshipGroupName == "ZOMBIE")
                {
                    return true;
                }
                return true;
            }
            return false;
            // return PedExt != null && (PedExt.PedGroup == null || PedExt.PedGroup?.InternalName.ToUpper() == "ZOMBIE" || (PedExt.PedGroup != null && PedExt.PedGroup?.InternalName.ToUpper() != "SECURITY_GUARD" && PedExt.PedGroup?.InternalName.ToUpper() != "PRIVATE_SECURITY" && PedExt.PedGroup?.InternalName.ToUpper() != "FIREMAN" && PedExt.PedGroup?.InternalName.ToUpper() != "MEDIC"));
        }
    }
    public uint TimeContinuoslySeenPlayer => PlayerPerception.TimeContinuoslySeenTarget;
    public ShopMenu ShopMenu { get; private set; }
    public VehicleExt VehicleLastSeenPlayerIn => PlayerPerception.VehicleLastSeenTargetIn;
    public int WantedLevel => PedViolations.WantedLevel;
    public bool WasEverSetPersistent { get; set; }
    public bool WasPersistentOnCreate { get; set; } = false;
    public bool WasSetCriminal { get; set; } = false;
    public WeaponInformation WeaponLastSeenPlayerWith => PlayerPerception.WeaponLastSeenTargetWith;
    public Blip AttachedLSRBlip { get; set; }
    public virtual bool WillCallPolice { get; set; } = false;
    public virtual bool WillCallPoliceIntense { get; set; } = false;
    public virtual bool WillFight { get; set; } = false;
    public virtual bool WillFightPolice { get; set; } = false;
    public virtual bool WillAlwaysFightPolice { get; set; } = false;
    public virtual bool WillCower { get; set; } = false;
    public bool IsGroupMember { get; set; } = false;
    public bool WithinWeaponsAudioRange => PlayerPerception.WithinWeaponsAudioRange;
    public string VoiceName { get; set; } = "";
    private int FullUpdateInterval//dont forget distance and LOS in here
    {
        get
        {
            if (IsCop)//IsCop)
            {
                if (PlayerPerception?.DistanceToTarget >= 300)
                {
                    return Settings.SettingsManager.PerformanceSettings.CopUpdateIntervalVeryFar;
                }
                else if (PlayerPerception?.DistanceToTarget >= 200)
                {
                    return Settings.SettingsManager.PerformanceSettings.CopUpdateIntervalFar;
                }
                else if (PlayerPerception?.DistanceToTarget >= 50f)
                {
                    return Settings.SettingsManager.PerformanceSettings.CopUpdateIntervalMedium;
                }
                else
                {
                    return Settings.SettingsManager.PerformanceSettings.CopUpdateIntervalClose;
                }
            }
            else
            {
                if (PlayerPerception?.DistanceToTarget >= 100f)//300
                {
                    return Settings.SettingsManager.PerformanceSettings.OtherUpdateIntervalVeryFar;
                }
                else if (PlayerPerception?.DistanceToTarget >= 50f)//200
                {
                    return Settings.SettingsManager.PerformanceSettings.OtherUpdateIntervalFar;
                }
                else if (IsWanted)
                {
                    return Settings.SettingsManager.PerformanceSettings.OtherUpdateIntervalWanted;
                }
                else if (PlayerPerception?.DistanceToTarget >= 25f)//50f
                {
                    return Settings.SettingsManager.PerformanceSettings.OtherUpdateIntervalMedium;//2000
                }
                else
                {
                    return Settings.SettingsManager.PerformanceSettings.OtherUpdateIntervalClose;
                }
            }
        }
    }
    public uint GameTimeKilled { get; set; }
    public uint GameTimeLastInjured { get; set; }
    public bool RecentlyInjured => GameTimeLastInjured != 0 && Game.GameTime - GameTimeLastInjured <= 3000;
    public bool HasBeenSeenUnconscious { get; set; } = false;
    public bool HasBeenSeenDead { get; set; } = false;   
    public bool HasStartedEMTTreatment { get; set; } = false;
    public bool WasSeenInDistressByServicePed { get; set; } = false;
    public uint GameTimeSeenDead => Game.GameTime - GameTimeFirstSeenDead;
    public uint GameTimeSeenUnconscious => Game.GameTime - GameTimeFirstSeenUnconscious;
    public bool HasBeenLooted { get; set; } = false;
    public bool IsDead { get; set; } = false;
    public bool WasModSpawned { get; set; } = false;
    public Vector3 SpawnPosition { get; set; }
    public float SpawnHeading { get; set; }
    public bool IsPlayerControlled { get; set; } = false;
    public LocationTaskRequirements LocationTaskRequirements { get; set; } = new LocationTaskRequirements();
    public virtual bool KnowsDrugAreas => HasMenu || HasDrugAreaKnowledge;
    public virtual bool KnowsGangAreas => HasMenu || HasGangAreaKnowledge;
    public uint GameTimeReachedInvestigationPosition { get; set; }
    public bool HasFullBodyArmor { get; set; } = false;
    public virtual bool CanBeLooted { get; set; } = true;
    public virtual bool CanBeDragged { get; set; } = true;
    public virtual bool CanPlayRadioInAnimation => false;
    public virtual string BlipName => "Person";
    public bool AlwaysHasLongGun { get; set; } = false;
    public bool IsBeingHeldAsHostage { get; set; } = false;
    public bool GeneratesBodyAlerts { get; set; } = true;
    public bool WasCrushed { get; set; }
    public virtual bool CanBeIdleTasked { get; set; } = true;
    public bool IsManuallyDeleted { get; set; } = false;
    public bool CanBeBuried => IsUnconscious || IsDead;
    public bool IsLoadedInTrunk { get; set; }
    public virtual bool HasWeapon => false;

    public virtual void Update(IPerceptable perceptable, IPoliceRespondable policeRespondable, Vector3 placeLastSeen, IEntityProvideable world)
    {
        PlayerToCheck = policeRespondable;
        if (!Pedestrian.Exists())
        {
            return;
        }
        if (Pedestrian.IsAlive)
        {
            if (NeedsFullUpdate)
            {
                IsInWrithe = Pedestrian.IsInWrithe;
                UpdatePositionData();
                PlayerPerception.Update(perceptable, placeLastSeen);
                UpdateVehicleState();
                if (!IsUnconscious && PlayerPerception.DistanceToTarget <= 200f)//was 150 only care in a bubble around the player, nothing to do with the player tho
                {
                    if (!PlayerPerception.RanSightThisUpdate) 
                    { 
                        GameFiber.Yield(); 
                    }
                    if (ShouldCheckCrimes)
                    {
                        PedViolations.Update(policeRespondable);//possible yield in here!, REMOVED FOR NOW
                    }
                    PedPerception.Update();
                    if (policeRespondable.CanBustPeds)
                    {
                        CheckPlayerBusted();
                    }
                    if (Settings.SettingsManager.CivilianSettings.AllowAlerts)
                    {
                        PedAlerts.Update(policeRespondable, world);
                    }
                    //UpdateAlerts(perceptable, policeRespondable, world);           
                }
                GameTimeLastUpdated = Game.GameTime;
            }
        }
        CurrentHealthState.Update(policeRespondable);//has a yield if they get damaged, seems ok
        
    }
    public virtual void OnBecameWanted()
    {
        if (Pedestrian.Exists())
        {
            if (!Pedestrian.IsPersistent)
            {
                Pedestrian.IsPersistent = true;
            }
            originalGroup = Pedestrian.RelationshipGroup;
            RelationshipGroup CriminalsRG = new RelationshipGroup("CRIMINALS");
            Pedestrian.RelationshipGroup = CriminalsRG;
            RelationshipGroup.Cop.SetRelationshipWith(CriminalsRG, Relationship.Hate);
            CriminalsRG.SetRelationshipWith(RelationshipGroup.Cop, Relationship.Hate);
            RelationshipGroup.SecurityGuard.SetRelationshipWith(CriminalsRG, Relationship.Hate);
            CriminalsRG.SetRelationshipWith(RelationshipGroup.SecurityGuard, Relationship.Hate);
            //EntryPoint.WriteToConsoleTestLong($"{Pedestrian.Handle} BECAME WANTED (CIVILIAN) SET TO CRIMINALS");
        }
    }
    public virtual void OnLostWanted()
    {
        if (Pedestrian.Exists())
        {
            Pedestrian.RelationshipGroup = originalGroup;
            PedViolations.Reset();
            //EntryPoint.WriteToConsoleTestLong($"{Pedestrian.Handle} LOST WANTED (CIVILIAN) SET TO ORIGINAL GROUP {originalGroup.Name}");
        }
    }
    public void AddWitnessedPlayerCrime(Crime CrimeToAdd, Vector3 PositionToReport) => PlayerPerception.AddWitnessedCrime(CrimeToAdd, PositionToReport);
    public void ApolgizedToByPlayer()
    {
        if ((GameTimeLastInsultedByPlayer == 0 || Game.GameTime - GameTimeLastInsultedByPlayer >= 1000) && TimesInsultedByPlayer > 0)
        {
            TimesInsultedByPlayer -= 1;
            GameTimeLastInsultedByPlayer = Game.GameTime;
            if (this.GetType() == typeof(GangMember))
            {
                GangMember gm = (GangMember)this;
                PlayerToCheck.RelationshipManager.GangRelationships.ChangeReputation(gm.Gang, 100, true);
            }
        }
    }
    public bool CheckHurtBy(Ped ToCheck, bool OnlyLast)
    {
        if (LastHurtBy == ToCheck)
        {
            //EntryPoint.WriteToConsole($"PEDEXT: CheckHurtBy Ped To Check: {Pedestrian.Handle} (LAST) Hurt by {ToCheck.Handle}");
            return true;
        }
        if (!OnlyLast && Pedestrian.Handle != ToCheck.Handle)
        {
            if (NativeFunction.CallByName<bool>("HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY", Pedestrian, ToCheck, true))
            {
                //EntryPoint.WriteToConsole($"PEDEXT: CheckHurtBy Ped To Check: {Pedestrian.Handle} (NEW PERSON) Hurt by {ToCheck.Handle}");
                LastHurtBy = ToCheck;
                return true;
            }
            else if (ToCheck.IsInAnyVehicle(false) && NativeFunction.CallByName<bool>("HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY", Pedestrian, ToCheck.CurrentVehicle, true))
            {
                //EntryPoint.WriteToConsole($"PEDEXT: CheckHurtBy Ped To Check: {Pedestrian.Handle} (NEW CAR) Hurt by {ToCheck.Handle}");
                LastHurtBy = ToCheck;
                return true;
            }
        }
        return false;
    }
    public bool CheckKilledBy(Ped ToCheck)
    {
        try
        {
            if (Pedestrian.Exists() && ToCheck.Exists() && Pedestrian.IsDead && Pedestrian.Handle != ToCheck.Handle)
            {
                //Killer = NativeFunction.Natives.GetPedSourceOfDeath<Entity>(Pedestrian);
                if (KillerHandle != 0 && KillerHandle == ToCheck.Handle || (ToCheck.IsInAnyVehicle(false) && ToCheck.CurrentVehicle.Handle == KillerHandle))//if (Killer.Exists() && Killer.Handle == ToCheck.Handle || (ToCheck.IsInAnyVehicle(false) && ToCheck.CurrentVehicle.Handle == Killer.Handle))
                {
                    //EntryPoint.WriteToConsole($"PEDEXT: CheckKilledBy Ped To Check: {Pedestrian.Handle} Killed by {KillerHandle}");
                    return true;
                }
                else if (KillerHandle == 0 && CheckHurtBy(ToCheck, true))
                {
                    //EntryPoint.WriteToConsole($"PEDEXT: CheckKilledBy Ped To Check: {Pedestrian.Handle} Killer is 0 Last Hurt By {ToCheck.Handle}");
                    return true;
                }
            }
            return false;
        }
        catch (Exception ex)
        {
            //EntryPoint.WriteToConsole($"KilledBy Error! Ped To Check: {Pedestrian.Handle}, assumeing you killed them if you hurt them");
            EntryPoint.WriteToConsole($"PEDEXT: CheckKilledBy Ped To Check: {Pedestrian.Handle} ERROR Killer is 0 Last Hurt By {ToCheck.Handle}", 5);
            return CheckHurtBy(ToCheck, true);//turned back on for now.......
            //return false;
        }
    }
    public void LogSourceOfDeath()
    {
        if (Pedestrian.Exists() && Pedestrian.IsDead)
        {
            try
            {
                KillerHandle = NativeFunction.Natives.GetPedSourceOfDeath<uint>(Pedestrian);
                EntryPoint.WriteToConsole($"PEDEXT: LogSourceOfDeath Ped To Check: {Pedestrian.Handle} killed by {KillerHandle}");
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole($"PEDEXT: LogSourceOfDeath Error! Ped To Check: {Pedestrian.Handle} {ex.Message} {ex.StackTrace}", 5);
            }
        }
    }
    public bool SeenPlayerWithin(int msSince) => PlayerPerception.SeenTargetWithin(msSince);
    public void SetWantedLevel(int toSet)
    {
        if (PedViolations.WantedLevel < toSet)
        {
            PedViolations.WantedLevel = toSet;
        }
        //if (toSet == 0)
        //{
        //    IsArrested = true;
        //    PedCrimes.Reset();
        //}
    }
    public void SetBusted()
    {
        IsBusted = true;
        CanBeAmbientTasked = false;
        CanBeTasked = false;

    }
    public void UpdateTask(PedExt otherTarget)
    {
        if (CurrentTask != null)
        {
            CurrentTask.OtherTarget = otherTarget;
            CurrentTask.Update();
        }
    }
    public void UpdateTask()
    {
        if (CurrentTask != null)
        {
            CurrentTask.Update();
        }
    }
    public void CheckPlayerBusted()
    {
        if (IsWanted && PlayerPerception.DistanceToTarget <= Settings.SettingsManager.PoliceSettings.BustDistance)
        {
            if (Pedestrian.Exists() && (Pedestrian.IsStunned || Pedestrian.IsRagdoll) && !IsBusted)
            {
                SetBusted();
               //EntryPoint.WriteToConsole($"PEDEXT: Player bust {Pedestrian.Handle}");
            }
        }
    }
    public void UpdatePositionData()
    {
        position = Pedestrian.Position;//See which cell it is in now
        CellX = (int)(position.X / EntryPoint.CellSize);
        CellY = (int)(position.Y / EntryPoint.CellSize);
    }
    public void PlaySpeech(List<string> Possibilities, bool useMegaphone, bool isShouted)
    {
        if(!Pedestrian.Exists())
        {
            return;
        }
        bool Spoke = false;
        foreach (string AmbientSpeech in Possibilities.OrderBy(x => RandomItems.MyRand.Next()).Take(2))
        {
            string voiceName = null;
            bool IsOverWrittingVoice = false;
            if (VoiceName != "")
            {
                voiceName = VoiceName;
                IsOverWrittingVoice = true;
            }
            bool hasContext = NativeFunction.Natives.DOES_CONTEXT_EXIST_FOR_THIS_PED<bool>(Pedestrian, AmbientSpeech, false);
            SpeechModifier speechModifier = SpeechModifier.Force;
            if (useMegaphone)
            {
                speechModifier = SpeechModifier.ForceMegaphone;
            }
            else if (isShouted)
            {
                speechModifier = SpeechModifier.ForceShouted;
            }

            if (IsOverWrittingVoice)
            {
                Pedestrian.PlayAmbientSpeech(voiceName, AmbientSpeech, 0, speechModifier);
            }
            else
            {
                Pedestrian.PlayAmbientSpeech(AmbientSpeech, useMegaphone);
            }
            GameFiber.Sleep(300);//100
            if (Pedestrian.Exists() && Pedestrian.IsAnySpeechPlaying)
            {
                Spoke = true;
            }
            //EntryPoint.WriteToConsole($"SAYAMBIENTSPEECH: {Cop.Pedestrian.Handle} voiceName {voiceName} Attempting {AmbientSpeech}, Result: {Spoke} IsOverWrittingVoice {IsOverWrittingVoice}");
            if (Spoke)
            {
                break;
            }
        }
    }
    public void UpdateVehicleState()
    {
        bool wasInVehicle = IsInVehicle;
        IsInVehicle = Pedestrian.IsInAnyVehicle(false);
        if (wasInVehicle != IsInVehicle)
        {
            if (IsInVehicle)//got in
            {
                //EntryPoint.WriteToConsole($"PedExt {Pedestrian.Handle} Got In Vehicle", 5);
                GameTimeLastEnteredVehicle = Game.GameTime;
            }
            else//got out
            {
                //EntryPoint.WriteToConsole($"PedExt {Pedestrian.Handle} Go Out of Vehicle", 5);
                GameTimeLastExitedVehicle = Game.GameTime;
            }
        }
        if (IsInVehicle && Pedestrian.CurrentVehicle.Exists())
        {
            IsDriver = Pedestrian.SeatIndex == -1;
            LastSeatIndex = Pedestrian.SeatIndex;
            IsInHelicopter = Pedestrian.IsInHelicopter;
            IsInBoat = Pedestrian.IsInBoat;
            IsInPlane = Pedestrian.IsInPlane;

            IsWaitingAtTrafficLight = NativeFunction.Natives.IS_VEHICLE_STOPPED_AT_TRAFFIC_LIGHTS<bool>(Pedestrian.CurrentVehicle);
            IsTurningLeftAtTrafficLight = false;
            IsTurningRightAtTrafficLight = false;
            if (Pedestrian.CurrentVehicle.Model.Name.ToLower() == "rhino")
            {
                IsInAPC = true;
            }
            else
            {
                IsInAPC = false;
            }
            if (!IsInHelicopter && !IsInBoat)
            {
                IsOnBike = Pedestrian.IsOnBike;
            }
            if (Pedestrian.CurrentVehicle.Speed >= 2.0f)
            {
                GameTimeLastMovedFast = Game.GameTime;
            }
            else
            {
                GameTimeLastMovedFast = 0;
            }
        }
        else
        {
            IsInHelicopter = false;
            IsOnBike = false;
            IsDriver = false;
            IsInBoat = false;
            IsInAPC = false;
            IsWaitingAtTrafficLight = false;
            IsTurningLeftAtTrafficLight = false;
            IsTurningRightAtTrafficLight = false;
            if (Pedestrian.Exists() && Pedestrian.Speed >= 7.0f)
            {
                GameTimeLastMovedFast = Game.GameTime;
            }
            else
            {
                GameTimeLastMovedFast = 0;
            }
        }
    }
    public void YellInPain(bool force)
    {
        if (CanYell || force)
        {
            if (RandomItems.RandomPercent(80))
            {
                List<int> PossibleYells = new List<int>() { 6, 7, 8 };
                int YellType = PossibleYells.PickRandom();
                NativeFunction.Natives.PLAY_PAIN(Pedestrian, YellType, 0, 0);

                EntryPoint.WriteToConsole($"YELL IN PAIN {Pedestrian.Handle} YellType {YellType}");
            }
            else
            {
                PlaySpeech("GENERIC_FRIGHTENED_HIGH", false);
                EntryPoint.WriteToConsole($"CRY SPEECH FOR PAIN {Pedestrian.Handle}");
            }
            GameTimeLastYelled = Game.GameTime;
        }
    }
    public void ResetPlayerCrimes()
    {
        PlayerPerception.PlayerCrimesWitnessed.Clear();
    }
    public void ResetCrimes()
    {
        PedViolations.Reset();
        PedPerception.Reset();
    }
    public void ClearTasks(bool resetAlertness)
    {
        if (!Pedestrian.Exists())
        {
            return;
        }
        int seatIndex = 0;
        Vehicle CurrentVehicle = null;
        bool WasInVehicle = false;
        if (Pedestrian.IsInAnyVehicle(false))
        {
            WasInVehicle = true;
            CurrentVehicle = Pedestrian.CurrentVehicle;
            seatIndex = Pedestrian.SeatIndex;
        }
        NativeFunction.Natives.CLEAR_PED_TASKS(Pedestrian);
        Pedestrian.BlockPermanentEvents = false;
        Pedestrian.KeepTasks = false;
        NativeFunction.Natives.CLEAR_PED_TASKS(Pedestrian);
        if (resetAlertness)
        {
            NativeFunction.Natives.SET_PED_ALERTNESS(Pedestrian, 0);
        }
        if (WasInVehicle && !Pedestrian.IsInAnyVehicle(false) && CurrentVehicle != null)
        {
            Pedestrian.WarpIntoVehicle(CurrentVehicle, seatIndex);
        }
        
        //EntryPoint.WriteToConsoleTestLong($"PED {Pedestrian.Handle} CLEAR TASKS RAN");
    }
    public void PlaySpeech(string speechName, bool useMegaphone)
    {
        if (VoiceName != "")
        {
            if (useMegaphone)
            {
                Pedestrian.PlayAmbientSpeech(VoiceName, speechName, 0, SpeechModifier.ForceMegaphone);
            }
            else
            {
                Pedestrian.PlayAmbientSpeech(VoiceName, speechName, 0, SpeechModifier.Force);

            }
            //EntryPoint.WriteToConsoleTestLong($"FREEMODE COP SPEAK {Pedestrian.Handle} freeModeVoice {VoiceName} speechName {speechName}");
        }
        else
        {
            Pedestrian.PlayAmbientSpeech(speechName, useMegaphone);
        }
    }
    public virtual void OnItemPurchased(ILocationInteractable player, ModItem modItem, int numberPurchased, int moneySpent)
    {
        int preMoney = Money;
        Money += Math.Abs(moneySpent);
        ItemDesires.OnItemsSoldToPlayer(modItem, numberPurchased);
        EntryPoint.WriteToConsole($"OnItemPurchased moneySpent:{moneySpent} PreMoney: {preMoney} PostMoney: {Money}");
    }
    public virtual void OnItemSold(ILocationInteractable player, ModItem modItem, int numberPurchased, int moneySpent)
    {
        int preMoney = Money;
        Money -= Math.Abs(moneySpent);
        if (Money < 0)
        {
            Money = 0;
        }
        ItemDesires.OnItemsBoughtFromPlayer(modItem, numberPurchased);
        EntryPoint.WriteToConsole($"OnItemSold moneySpent:{moneySpent} PreMoney: {preMoney} PostMoney: {Money}");
    }
    public virtual void SetupTransactionItems(ShopMenu shopMenu, bool matchWithMenu)
    {
         //EntryPoint.WriteToConsole($"SetupTransactionItems START {Handle} HasMenu:{shopMenu == null} {shopMenu?.Name}");
        ShopMenu = shopMenu;
        if (shopMenu == null)
        {
            return;
        }
        foreach (MenuItem menuItem in ShopMenu.Items)
        {
            if (menuItem.NumberOfItemsToSellToPlayer > 0)
            {
                PedInventory.Add(menuItem.ModItem, menuItem.NumberOfItemsToSellToPlayer);
            }
        }
        ItemDesires.AddDesiredItem(shopMenu, matchWithMenu);
       // EntryPoint.WriteToConsole("SetupTransactionItems END");
    }
    public string LootInventory(IInteractionable player, IModItems modItems, ICellphones cellphones)
    {
        HasBeenLooted = true;
        string ItemsFound = "";
        ItemsFound += StealPhone(player, modItems, cellphones);
        ItemsFound += StealID(player, modItems);
        foreach (InventoryItem ii in PedInventory.ItemsList)
        {
            player.Inventory.Add(ii.ModItem, ii.RemainingPercent);
            ItemsFound += $"~n~~p~{ii.ModItem.Name}~s~ - {ii.RemainingPercent} {ii.ModItem.MeasurementName}(s)";
        }
        PedInventory.ItemsList.Clear();
        return ItemsFound;
    }
    private string StealPhone(IInteractionable player, IModItems modItems, ICellphones cellphones)
    {
        EntryPoint.WriteToConsole($"STEAL PHONE START");
        if(!HasCellPhone)
        {
            EntryPoint.WriteToConsole($"STEAL PHONE NO PHONE");
            return "";
        }
        HasCellPhone = false;
        CellphoneData phone = cellphones.GetRandomRegular();
        if(phone == null)
        {
            EntryPoint.WriteToConsole($"STEAL PHONE NO RANDOM");
            return "";
        }
        EntryPoint.WriteToConsole($"STEAL PHONE CellphoneData {phone.ModItemName}");
        CellphoneItem cellphoneItem = modItems.PossibleItems.CellphoneItems.FirstOrDefault(x => x.Name.ToLower() == phone.ModItemName.ToLower());
        if(cellphoneItem == null)
        {
            EntryPoint.WriteToConsole($"STEAL PHONE NO MOD ITEM");
            return "";
        }
        player.Inventory.Add(cellphoneItem,1.0f);
        return $"~n~~p~{cellphoneItem.Name}~s~";
    }
    protected virtual string StealID(IInteractionable player, IModItems modItems)
    {
        EntryPoint.WriteToConsole($"STEAL ID START");
        if (!HasIdentification)
        {
            EntryPoint.WriteToConsole($"STEAL ID NO ID");
            return "";
        }
        HasIdentification = false;
        ValuableItem idITem = modItems.PossibleItems.ValuableItems.FirstOrDefault(x => x.Name.ToLower() == "drivers license");
        if (idITem == null)
        {
            EntryPoint.WriteToConsole($"STEAL ID NO MOD ITEM");
            return "";
        }
        player.Inventory.Add(idITem, 1.0f);
        return $"~n~~p~{idITem.Name}~s~";
    }
    public virtual void ReportCrime(ITargetable Player)
    {
        if (Pedestrian.Exists() && Pedestrian.IsAlive && !Pedestrian.IsRagdoll)
        {
            if (PlayerCrimesWitnessed.Any())
            {
                AddPlayerCrimeWitnessed(Player);
            }
            else if (OtherCrimesWitnessed.Any())
            {
                AddOtherCrimesWitnessed(Player);
            }
            PedAlerts.OnReportedCrime(PlayerToCheck);
            //else if (PedAlerts.HasSeenUnconsciousPed)
            //{
            //    AddMedicalEventWitnessed(Player);
            //}
            GameTimeLastReportedCrime = Game.GameTime;

        }
        else if (!Pedestrian.Exists())
        {
            if (PlayerCrimesWitnessed.Any())
            {
                AddPlayerCrimeWitnessed(Player);
            }
            else if (OtherCrimesWitnessed.Any())
            {
                AddOtherCrimesWitnessed(Player);
            }
            PedAlerts.OnReportedCrime(PlayerToCheck);
            //else if (PedAlerts.HasSeenUnconsciousPed)
            //{
            //    AddMedicalEventWitnessed(Player);
            //}
            GameTimeLastReportedCrime = Game.GameTime;
        }
        
    }
    public virtual void SetPersistent()
    {

    }
    public virtual void SetNonPersistent()
    {

    }
    public void SetBaseStats(DispatchablePerson dispatchablePerson, IShopMenus shopMenus, IWeapons weapons, bool addBlip)
    {
        if (!Pedestrian.Exists())
        {
            return;
        }
        Pedestrian.Money = 0;
        IsTrustingOfPlayer = RandomItems.RandomPercent(Settings.SettingsManager.CivilianSettings.PercentageTrustingOfPlayer);// Gang.PercentageTrustingOfPlayer);
        Money = RandomItems.GetRandomNumberInt(CivilianMoneyMin(), CivilianMoneyMax());
        WillFight = RandomItems.RandomPercent(CivilianFightPercentage());
        WillCallPolice = RandomItems.RandomPercent(CivilianCallPercentage());
        WillCallPoliceIntense = RandomItems.RandomPercent(CivilianSeriousCallPercentage());
        WillFightPolice = RandomItems.RandomPercent(CivilianFightPolicePercentage());
        WillCower = RandomItems.RandomPercent(CivilianCowerPercentage());
        CanSurrender = RandomItems.RandomPercent(Settings.SettingsManager.CivilianSettings.PossibleSurrenderPercentage);
        if (addBlip)
        {
            AddBlip();
        }
        if (dispatchablePerson == null)
        {
            return;
        }
        dispatchablePerson.SetPedExtPermanentStats(this, Settings.SettingsManager.CivilianSettings.OverrideHealth, false, Settings.SettingsManager.CivilianSettings.OverrideAccuracy);//has a yield
        if (!Pedestrian.Exists())
        {
            return;
        }
        if (Settings.SettingsManager.CivilianSettings.SightDistance > 60f)
        {
            NativeFunction.Natives.SET_PED_SEEING_RANGE(Pedestrian, Settings.SettingsManager.CivilianSettings.SightDistance);
        }
    }
    public void AddBlip()
    {
        if(!Pedestrian.Exists() || AttachedLSRBlip.Exists())
        {
            return;
        }
        Blip myBlip = Pedestrian.AttachBlip();

        EntryPoint.WriteToConsole($"PEDEXT BLIP CREATED");

        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SET_BLIP_NAME("STRING");
        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(BlipName);
        NativeFunction.Natives.END_TEXT_COMMAND_SET_BLIP_NAME(myBlip);
        myBlip.Color = BlipColor;
        myBlip.Scale = BlipSize;

        NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)myBlip.Handle, true);


        AttachedLSRBlip = myBlip;
    }
    public virtual void MatchPlayerPedType(IPedSwappable Player)
    {
        Player.RemoveAgencyStatus();
    }
    public void DeleteBlip()
    {
        if(AttachedLSRBlip.Exists())
        {
            AttachedLSRBlip.Delete();
        }
        if (!Pedestrian.Exists())
        {
            return;
        }
        Blip attachedBlip = Pedestrian.GetAttachedBlip();
        if (attachedBlip.Exists())
        {
            attachedBlip.Delete();
        }
    }
    public virtual void FullyDelete()
    {
        DeleteBlip();
        if(Pedestrian.Exists())
        {
            Pedestrian.Delete();
        }
    }
    private void AddPlayerCrimeWitnessed(ITargetable Player)
    {
        foreach(WitnessedCrime witnessedCrime in PlayerCrimesWitnessed.ToList())
        {
            Player.AddCrime(witnessedCrime.Crime, false, PositionLastSeenCrime, VehicleLastSeenPlayerIn, WeaponLastSeenPlayerWith, EverSeenPlayer && ClosestDistanceToPlayer <= 10f, true, true);
        }
        PlayerCrimesWitnessed.Clear();
    }
    private void AddOtherCrimesWitnessed(ITargetable Player)
    {
        WitnessedCrime toReport = OtherCrimesWitnessed.Where(x => x.Perpetrator.Pedestrian.Exists() && !x.Perpetrator.IsBusted && x.Perpetrator.Pedestrian.IsAlive).OrderBy(x => x.Crime.Priority).ThenByDescending(x => x.GameTimeLastWitnessed).FirstOrDefault();
        if (toReport != null)
        {
            Player.AddCrime(toReport.Crime, false, toReport.Location, toReport.Vehicle, toReport.Weapon, false, true, false);
        }
        OtherCrimesWitnessed.Clear();
    }
    public virtual void AddSpecificInteraction(ILocationInteractable player, MenuPool menuPool, UIMenu headerMenu, AdvancedConversation advancedConversation)
    {
        UIMenuItem transactionInteract = new UIMenuItem("Start Transaction", "Buy or sell with the current ped.");
        transactionInteract.Activated += (menu, item) =>
        {
            menu.Visible = false;
            advancedConversation.StartTransactionWithPed();
        };
        if (HasMenu)
        {
            headerMenu.AddItem(transactionInteract);
        }
    }
    public virtual void ShowPedInfoNotification(uint pedHeadshotHandle)
    {
        string Description = $"~p~{GroupName}~s~";
        if (IsFedUpWithPlayer)
        {
            Description += "~n~~r~Fed Up~s~";
        }
        else if (TimesInsultedByPlayer > 0)
        {
            Description += $"~n~~o~Insulted {TimesInsultedByPlayer} time(s)~s~";
        }
        if (HasMenu)
        {
            Description += $"~n~~g~Can Transact~s~";
        }
        if (pedHeadshotHandle != 0 && NativeFunction.Natives.IsPedheadshotReady<bool>(pedHeadshotHandle))
        {
            string str = NativeFunction.Natives.GetPedheadshotTxdString<string>(pedHeadshotHandle);
            Game.DisplayNotification(str, str, "~b~Ped Info", $"~y~{Name}", Description);
        }
        else
        {
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~b~Ped Info", $"~y~{Name}", Description);
        }
    }
    protected float CivilianCallPercentage()
    {
        if (EntryPoint.FocusZone != null)
        {
            if (EntryPoint.FocusZone.Economy == eLocationEconomy.Rich)
            {
                return Settings.SettingsManager.CivilianSettings.CallPolicePercentageRichZones;
            }
            else if (EntryPoint.FocusZone.Economy == eLocationEconomy.Middle)
            {
                return Settings.SettingsManager.CivilianSettings.CallPolicePercentageMiddleZones;
            }
            else if (EntryPoint.FocusZone.Economy == eLocationEconomy.Poor)
            {
                return Settings.SettingsManager.CivilianSettings.CallPolicePercentagePoorZones;
            }
            else
            {
                return Settings.SettingsManager.CivilianSettings.CallPolicePercentageMiddleZones;
            }
        }
        else
        {
            return Settings.SettingsManager.CivilianSettings.CallPolicePercentageMiddleZones;
        }
    }
    protected float CivilianSeriousCallPercentage()
    {
        if (EntryPoint.FocusZone != null)
        {
            if (EntryPoint.FocusZone.Economy == eLocationEconomy.Rich)
            {
                return Settings.SettingsManager.CivilianSettings.CallPoliceForSeriousCrimesPercentageRichZones;
            }
            else if (EntryPoint.FocusZone.Economy == eLocationEconomy.Middle)
            {
                return Settings.SettingsManager.CivilianSettings.CallPoliceForSeriousCrimesPercentageMiddleZones;
            }
            else if (EntryPoint.FocusZone.Economy == eLocationEconomy.Poor)
            {
                return Settings.SettingsManager.CivilianSettings.CallPoliceForSeriousCrimesPercentagePoorZones;
            }
            else
            {
                return Settings.SettingsManager.CivilianSettings.CallPoliceForSeriousCrimesPercentageMiddleZones;
            }
        }
        else
        {
            return Settings.SettingsManager.CivilianSettings.CallPoliceForSeriousCrimesPercentageMiddleZones;
        }
    }
    protected int CivilianMoneyMax()
    {
        if (EntryPoint.FocusZone != null)
        {
            if (EntryPoint.FocusZone.Economy == eLocationEconomy.Rich)
            {
                return Settings.SettingsManager.CivilianSettings.MoneyMaxRichZones;
            }
            else if (EntryPoint.FocusZone.Economy == eLocationEconomy.Middle)
            {
                return Settings.SettingsManager.CivilianSettings.MoneyMaxMiddleZones;
            }
            else if (EntryPoint.FocusZone.Economy == eLocationEconomy.Poor)
            {
                return Settings.SettingsManager.CivilianSettings.MoneyMaxPoorZones;
            }
            else
            {
                return Settings.SettingsManager.CivilianSettings.MoneyMaxMiddleZones;
            }
        }
        else
        {
            return Settings.SettingsManager.CivilianSettings.MoneyMaxMiddleZones;
        }
    }
    protected int CivilianMoneyMin()
    {
        if (EntryPoint.FocusZone != null)
        {
            if (EntryPoint.FocusZone.Economy == eLocationEconomy.Rich)
            {
                return Settings.SettingsManager.CivilianSettings.MoneyMinRichZones;
            }
            else if (EntryPoint.FocusZone.Economy == eLocationEconomy.Middle)
            {
                return Settings.SettingsManager.CivilianSettings.MoneyMinMiddleZones;
            }
            else if (EntryPoint.FocusZone.Economy == eLocationEconomy.Poor)
            {
                return Settings.SettingsManager.CivilianSettings.MoneyMinPoorZones;
            }
            else
            {
                return Settings.SettingsManager.CivilianSettings.MoneyMinMiddleZones;
            }
        }
        else
        {
            return Settings.SettingsManager.CivilianSettings.MoneyMinMiddleZones;
        }
    }
    protected int MerchantMoneyMax()
    {
        if (EntryPoint.FocusZone != null)
        {
            if (EntryPoint.FocusZone.Economy == eLocationEconomy.Rich)
            {
                return Settings.SettingsManager.CivilianSettings.MerchantMoneyMaxRichZones;
            }
            else if (EntryPoint.FocusZone.Economy == eLocationEconomy.Middle)
            {
                return Settings.SettingsManager.CivilianSettings.MerchantMoneyMaxMiddleZones;
            }
            else if (EntryPoint.FocusZone.Economy == eLocationEconomy.Poor)
            {
                return Settings.SettingsManager.CivilianSettings.MerchantMoneyMaxPoorZones;
            }
            else
            {
                return Settings.SettingsManager.CivilianSettings.MerchantMoneyMaxMiddleZones;
            }
        }
        else
        {
            return Settings.SettingsManager.CivilianSettings.MerchantMoneyMaxMiddleZones;
        }
    }
    protected int MerchantMoneyMin()
    {
        if (EntryPoint.FocusZone != null)
        {
            if (EntryPoint.FocusZone.Economy == eLocationEconomy.Rich)
            {
                return Settings.SettingsManager.CivilianSettings.MerchantMoneyMinRichZones;
            }
            else if (EntryPoint.FocusZone.Economy == eLocationEconomy.Middle)
            {
                return Settings.SettingsManager.CivilianSettings.MerchantMoneyMinMiddleZones;
            }
            else if (EntryPoint.FocusZone.Economy == eLocationEconomy.Poor)
            {
                return Settings.SettingsManager.CivilianSettings.MerchantMoneyMinPoorZones;
            }
            else
            {
                return Settings.SettingsManager.CivilianSettings.MerchantMoneyMinMiddleZones;
            }
        }
        else
        {
            return Settings.SettingsManager.CivilianSettings.MerchantMoneyMinMiddleZones;
        }
    }
    protected float CivilianFightPercentage()
    {
        if (EntryPoint.FocusZone != null)
        {
            if (EntryPoint.FocusZone.Economy == eLocationEconomy.Rich)
            {
                return Settings.SettingsManager.CivilianSettings.FightPercentageRichZones;
            }
            else if (EntryPoint.FocusZone.Economy == eLocationEconomy.Middle)
            {
                return Settings.SettingsManager.CivilianSettings.FightPercentageMiddleZones;
            }
            else if (EntryPoint.FocusZone.Economy == eLocationEconomy.Poor)
            {
                return Settings.SettingsManager.CivilianSettings.FightPercentagePoorZones;
            }
            else
            {
                return Settings.SettingsManager.CivilianSettings.FightPercentageMiddleZones;
            }
        }
        else
        {
            return Settings.SettingsManager.CivilianSettings.FightPercentageMiddleZones;
        }
    }
    protected float CivilianFightPolicePercentage()
    {
        if (EntryPoint.FocusZone != null)
        {
            if (EntryPoint.FocusZone.Economy == eLocationEconomy.Rich)
            {
                return Settings.SettingsManager.CivilianSettings.FightPolicePercentageRichZones;
            }
            else if (EntryPoint.FocusZone.Economy == eLocationEconomy.Middle)
            {
                return Settings.SettingsManager.CivilianSettings.FightPolicePercentageMiddleZones;
            }
            else if (EntryPoint.FocusZone.Economy == eLocationEconomy.Poor)
            {
                return Settings.SettingsManager.CivilianSettings.FightPolicePercentagePoorZones;
            }
            else
            {
                return Settings.SettingsManager.CivilianSettings.FightPolicePercentageMiddleZones;
            }
        }
        else
        {
            return Settings.SettingsManager.CivilianSettings.FightPolicePercentageMiddleZones;
        }
    }
    protected float CivilianCowerPercentage()
    {
        if (EntryPoint.FocusZone != null)
        {
            if (EntryPoint.FocusZone.Economy == eLocationEconomy.Rich)
            {
                return Settings.SettingsManager.CivilianSettings.CowerPercentageRichZones;
            }
            else if (EntryPoint.FocusZone.Economy == eLocationEconomy.Middle)
            {
                return Settings.SettingsManager.CivilianSettings.CowerPercentageMiddleZones;
            }
            else if (EntryPoint.FocusZone.Economy == eLocationEconomy.Poor)
            {
                return Settings.SettingsManager.CivilianSettings.CowerPercentagePoorZones;
            }
            else
            {
                return Settings.SettingsManager.CivilianSettings.CowerPercentageMiddleZones;
            }
        }
        else
        {
            return Settings.SettingsManager.CivilianSettings.CowerPercentageMiddleZones;
        }
    }
    public virtual void OnKilledByPlayer(IViolateable Player, IZones Zones, IGangTerritories GangTerritories)
    {

    }
    public virtual void OnInjuredByPlayer(IViolateable Player, IZones Zones, IGangTerritories GangTerritories)
    {

    }
    public virtual void OnCarjackedByPlayer(IViolateable Player, IZones Zones, IGangTerritories GangTerritories)
    {
        HasBeenCarJackedByPlayer = true;
    }
    public virtual void OnDeath(IPoliceRespondable policeRespondable)
    {
        PlayerPerception.Reset();
    }
    public virtual void OnUnconscious(IPoliceRespondable policeRespondable)
    {

    }
    public virtual bool OnTreatedByEMT(float revivePercentage)
    {
        HasBeenTreatedByEMTs = true;
        HasStartedEMTTreatment = false;
        if(!Pedestrian.Exists())
        {
            return false;
        }
        EntryPoint.WriteToConsole($"EMT TREATED VICTIM {Handle}");
        if (RandomItems.RandomPercent(revivePercentage))//Settings.SettingsManager.EMSSettings.RevivePercentage))
        {
            Pedestrian.IsRagdoll = false;
            IsUnconscious = false;
            HasBeenSeenUnconscious = false;
            CanBeAmbientTasked = true;
            CanBeTasked = true;
            PlaySpeech("GENERIC_THANKS", false);
            NativeFunction.CallByName<bool>("SET_PED_MOVEMENT_CLIPSET", Pedestrian, "move_m@drunk@verydrunk", 0x3E800000);
            return false;
        }
        else
        {
            YellInPain(true);
            Pedestrian.Kill();
            IsUnconscious = false;
            return true;
        }    
    }
    public void SetUnconscious(IPoliceRespondable policeRespondable)
    {
        
    }
    public virtual void ShowCustomDisplay(uint headshotID,string Title, string Description)
    {
        string pic1 = "CHAR_BLANK_ENTRY";
        string pic2 = "CHAR_BLANK_ENTRY";
        if (NativeFunction.Natives.IsPedheadshotReady<bool>(headshotID))
        {
            string str = NativeFunction.Natives.GetPedheadshotTxdString<string>(headshotID);
            pic1 = str;
            pic2 = str;
        }
        Game.DisplayNotification(pic1, pic2, Title, $"~y~{Name}", Description);
    }
    public virtual void ShowInfoDisplay(uint headshotID)
    {
        string pic1 = "CHAR_BLANK_ENTRY";
        string pic2 = "CHAR_BLANK_ENTRY";
        if (NativeFunction.Natives.IsPedheadshotReady<bool>(headshotID))
        {
            string str = NativeFunction.Natives.GetPedheadshotTxdString<string>(headshotID);
            pic1 = str;
            pic2 = str;
        }
        Game.DisplayNotification(pic1, pic2, "~o~Information", $"~y~{Name}", GetPedInfoForDisplay());
    }
    protected virtual string GetPedInfoForDisplay()
    {
        string Output = "";
        string Status = "Normal";
        if(IsDead)
        {
            Status = "Dead";
        }
        else if(IsUnconscious)
        {
            Status = "Unconscious";
        }
        Output = $"Status: {Status}";
        if (HasMenu && !IsDead && !IsUnconscious)
        {
            Output += $"~n~Can Transact";
        }
        return Output;
    }
    public virtual void SetSeenDead(PedExt deadBody)
    {
        //if (GameTimeLastSeenDead != 0)
        //{
        //    GameTimeLastSeenDead = Game.GameTime;
        //}
    }
    public virtual void SetSeenUnconscious(PedExt distressedPed)
    {
        //GameTimeLastSeenUnconscious = Game.GameTime;
    }
    public void SetWasSeenDead()
    {
        if(GameTimeFirstSeenDead != 0)
        {
            return;
        }
        GameTimeFirstSeenDead = Game.GameTime;
    }
    public void SetWasSeenUnconscious()
    {
        if (GameTimeFirstSeenUnconscious != 0)
        {
            return;
        }
        GameTimeFirstSeenUnconscious = Game.GameTime;
        EntryPoint.WriteToConsole($"{Handle} first time seen unconscious");
    }
    public void ResetPlayerStoodTooClose()
    {
        GameTimeLastTooCloseToPlayer = 0;
    }
    public virtual void OnPlayerStoodOnCar(IInteractionable player)
    {
        if (Game.GameTime - GameTimePlayerLastStoodOnCar < 3000)
        {
            return;
        }
        PlayerPerception.SetFakeSeen();
        GameTimePlayerLastStoodOnCar = Game.GameTime;
        PlaySpeech(new List<string>() { "GENERIC_SHOCKED_HIGH", "GENERIC_FRUSTRATED_HIGH", "GET_OUT_OF_HERE" }, false, false);
        AddWitnessedPlayerCrime(Crimes.CrimeList.FirstOrDefault(x => x.ID == StaticStrings.HarassmentCrimeID), player.Character.Position);
        EntryPoint.WriteToConsole($"OnHitInsultLimit triggered {Handle}");
    }
    public virtual void OnInsultedByPlayer(IInteractionable player)
    {
        if (GameTimeLastInsultedByPlayer == 0 || Game.GameTime - GameTimeLastInsultedByPlayer >= 1000)
        {
            TimesInsultedByPlayer += 1;
            GameTimeLastInsultedByPlayer = Game.GameTime;

            if(TimesInsultedByPlayer >= InsultLimit)
            {
                OnHitInsultLimit(player);
            }
        }
    }
    protected virtual void OnHitInsultLimit(IInteractionable player)
    {
        PlayerPerception.SetFakeSeen();
        PlaySpeech(new List<string>() { "GENERIC_SHOCKED_HIGH", "GENERIC_FRUSTRATED_HIGH", "GET_OUT_OF_HERE" }, false, false);
        AddWitnessedPlayerCrime(Crimes.CrimeList.FirstOrDefault(x => x.ID == StaticStrings.HarassmentCrimeID), player.Character.Position);
        EntryPoint.WriteToConsole($"OnHitInsultLimit triggered {Handle}");
    }
    protected virtual void OnHitPlayerStoodTooCloseLimit(IInteractionable player)
    {
        PlayerPerception.SetFakeSeen();
        PlaySpeech(new List<string>() { "GENERIC_SHOCKED_HIGH", "GENERIC_FRUSTRATED_HIGH", "GET_OUT_OF_HERE" }, false, false);
        AddWitnessedPlayerCrime(Crimes.CrimeList.FirstOrDefault(x => x.ID == StaticStrings.HarassmentCrimeID), player.Character.Position);
        EntryPoint.WriteToConsole($"OnHitPlayerStoodTooCloseLimit triggered {Handle}");
    }
    protected virtual void OnHitCollideWithPlayerLimit(IInteractionable player)
    {
        PlayerPerception.SetFakeSeen();
        PlaySpeech(new List<string>() { "GENERIC_SHOCKED_HIGH", "GENERIC_FRUSTRATED_HIGH", "GET_OUT_OF_HERE" }, false, false);
        AddWitnessedPlayerCrime(Crimes.CrimeList.FirstOrDefault(x => x.ID == StaticStrings.HarassmentCrimeID), player.Character.Position);
        EntryPoint.WriteToConsole($"OnHitCollideWithPlayerLimit triggered {Handle}");
    }
    public virtual void OnPlayerIsClose(IInteractionable player)
    {
        if(GameTimeLastTooCloseToPlayer == 0)
        {
           // HasGivenTooCloseWarning = false;
            GameTimeLastTooCloseToPlayer = Game.GameTime;
        }
        if (Game.GameTime - GameTimeLastTooCloseToPlayer >= 5000 && !HasGivenTooCloseWarning)
        {
            OnPlayerStandingTooClose(player);
            HasGivenTooCloseWarning = true;
        }
        if (Game.GameTime - GameTimeLastTooCloseToPlayer >= 10000)
        {
            GameTimeLastTooCloseToPlayer = Game.GameTime;
            HasGivenTooCloseWarning = false;
            OnPlayerStoodTooClose(player);
        }
    }
    private void OnPlayerStandingTooClose(IInteractionable player)
    {
        PlaySpeech(new List<string>() { "GENERIC_SHOCKED_HIGH", "GENERIC_FRUSTRATED_HIGH", "GET_OUT_OF_HERE" }, false, false);
        EntryPoint.WriteToConsole($"OnPlayerStandingTooClose  triggered {Handle}");
        Game.DisplayHelp("You are crowding. Back off to avoid issues");
    }
    private void OnPlayerStoodTooClose(IInteractionable player)
    {
        EntryPoint.WriteToConsole($"OnPlayerStoodTooClose triggered {Handle}");
        TimesPlayerStoodTooClose++;
        if(TimesPlayerStoodTooClose > PlayerStandTooCloseLimit)
        {
            OnHitPlayerStoodTooCloseLimit(player);
        }
    }
    public virtual void OnCollidedWithPlayer(IInteractionable player)
    {
        if(Game.GameTime - GameTimeLastCollidedWithPlayer < 3000)
        {
            return;
        }
        GameTimeLastCollidedWithPlayer = Game.GameTime;
        TimesCollidedWithPlayer++;
        EntryPoint.WriteToConsole($"OnCollidedWithPlayer triggered {Handle} TimeCollidedWithPlayer:{TimesCollidedWithPlayer} ");
        if (TimesCollidedWithPlayer > CollideWithPlayerLimit)
        {
            OnHitCollideWithPlayerLimit(player);
        }
    }
    public virtual void OnPlayerDamagedCarOnFoot(IInteractionable player)
    {
        if (Game.GameTime - GameTimePlayerLastDamagedCarOnFoot < 3000)
        {
            return;
        }
        PlayerPerception.SetFakeSeen();
        GameTimePlayerLastDamagedCarOnFoot = Game.GameTime;
        AddWitnessedPlayerCrime(Crimes.CrimeList.FirstOrDefault(x => x.ID == StaticStrings.HarassmentCrimeID), player.Character.Position);
        EntryPoint.WriteToConsole($"OnPlayerDamagedCarOnFoot triggered {Handle}");
    }
    public virtual void ControlLandingGear()
    {
        if(!(IsInHelicopter || IsInPlane))
        {
            return;
        }
        if(!Pedestrian.Exists() || !Pedestrian.CurrentVehicle.Exists())
        {
            return;
        }
        bool hasLandingGear = NativeFunction.Natives.GET_VEHICLE_HAS_LANDING_GEAR<bool>(Pedestrian.CurrentVehicle);
        if(!hasLandingGear)
        {
            return;
        }
        int landingGearState = NativeFunction.Natives.GET_LANDING_GEAR_STATE<int>(Pedestrian.CurrentVehicle);
        float height = Pedestrian.HeightAboveGround;
        EntryPoint.WriteToConsole($"I AM PED {Handle} IsInHelicopter{IsInHelicopter} IsInPlane{IsInPlane} landingGearState{landingGearState} height{height}");
        if (height >= 15f && landingGearState == 0)//locked down
        {
            NativeFunction.Natives.CONTROL_LANDING_GEAR(Pedestrian.CurrentVehicle, 1);//retract
            EntryPoint.WriteToConsole($"I AM PED {Handle} IsInHelicopter{IsInHelicopter} IsInPlane{IsInPlane} AND I AM RETRACTING MY LANDING GEAR");
        }


        /*ENUM LANDING_GEAR_STATE
LGS_LOCKED_DOWN,
LGS_RETRACTING,
LGS_RETRACTING_INSTANT,
LGS_DEPLOYING,
LGS_LOCKED_UP,
LGS_BROKEN
ENDENUM
        
         ENUM LANDING_GEAR_COMMAND
    LGC_DEPLOY = 0,
    LGC_RETRACT,
    LGC_DEPLOY_INSTANT,
    LGC_RETRACT_INSTANT,
    LGC_BREAK
ENDENUM
         
         
         */


    }
    public virtual void OnPlayerDidBodilyFunctionsNear(IInteractionable player)
    {
        if (Game.GameTime - GameTimePlayerLastDidBodilyFunctionsNear < 3000)
        {
            return;
        }
        GameTimePlayerLastDidBodilyFunctionsNear = Game.GameTime;
        AddWitnessedPlayerCrime(Crimes.CrimeList.FirstOrDefault(x => x.ID == StaticStrings.HarassmentCrimeID), player.Character.Position);
        EntryPoint.WriteToConsole($"OnPlayerDidBodilyFunctionsNear triggered {Handle}");
    }
    public virtual void OnHeardGunfire(IPoliceRespondable policeRespondable)
    {

    }
    public virtual void OnSeenDeadBody(IPoliceRespondable policeRespondable)
    {

    }
}
