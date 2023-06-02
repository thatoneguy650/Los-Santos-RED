using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

public class PedExt : IComplexTaskable, ISeatAssignable
{
    public IPoliceRespondable PlayerToCheck;
    protected ISettingsProvideable Settings;
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
    private ICrimes Crimes;

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




        PedViolations = new PedViolations(this, crimes, settings, weapons, world);
        PedPerception = new PedPerception(this, crimes, settings, weapons, world);
        PlayerPerception = new PlayerPerception(this, null, settings);
        PedReactions = new PedReactions(this);
        PedInventory = new SimpleInventory(Settings);
        PedBrain = new PedBrain(this, settings, world, weapons);
        PedDesires = new PedDesires(this, settings);
        PedAlerts = new PedAlerts(this,settings);
        //PedKnowledge = new PedKnowledge(this, settings);
        IsTrustingOfPlayer = RandomItems.RandomPercent(Settings.SettingsManager.CivilianSettings.PercentageTrustingOfPlayer);
        HasDrugAreaKnowledge = RandomItems.RandomPercent(Settings.SettingsManager.CivilianSettings.PercentageKnowsAnyDrugTerritory);
        HasGangAreaKnowledge = RandomItems.RandomPercent(Settings.SettingsManager.CivilianSettings.PercentageKnowsAnyGangTerritory);
        UpdateJitter = RandomItems.GetRandomNumber(100, 200);
    }
    public PedViolations PedViolations { get; private set; }
    public PedPerception PedPerception { get; private set; }
    public PlayerPerception PlayerPerception { get; private set; }
    public HealthState CurrentHealthState { get; private set; }
    public PedReactions PedReactions { get; set; }
    public SimpleInventory PedInventory { get; private set; }
    public PedBrain PedBrain { get; set; }
    public PedDesires PedDesires { get; private set; }
    public PedAlerts PedAlerts { get; private set; }
    public uint ArrestingPedHandle { get; set; } = 0;
    public List<Cop> AssignedCops { get; set; } = new List<Cop>();
    public int AssignedSeat { get; set; }
    public VehicleExt AssignedVehicle { get; set; }
    public Vector3 Position => position;
    public bool CanAttackPlayer => IsFedUpWithPlayer || HatesPlayer;
    public bool CanBeAmbientTasked { get; set; } = true;
    public bool CanBeMugged => !IsCop && Pedestrian.Exists() && !IsBusted && !IsUnconscious && !IsDead && !IsArrested && Pedestrian.IsAlive && !Pedestrian.IsStunned && !Pedestrian.IsRagdoll && !Pedestrian.IsInCombat && (!Pedestrian.IsPersistent || Settings.SettingsManager.CivilianSettings.AllowMissionPedsToInteract || IsMerchant || IsGangMember || WasModSpawned);
    public bool CanBeTasked { get; set; } = true;
    public virtual bool CanConverse => Pedestrian.Exists() && !IsBusted && !IsUnconscious && !IsDead && !IsArrested && Pedestrian.IsAlive && !Pedestrian.IsFleeing && !Pedestrian.IsInCombat && !Pedestrian.IsSprinting && !Pedestrian.IsStunned && !Pedestrian.IsRagdoll && (!Pedestrian.IsPersistent || Settings.SettingsManager.CivilianSettings.AllowMissionPedsToInteract || IsCop || IsMerchant || IsGangMember || WasModSpawned);
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
    public int CellX { get; set; }
    public int CellY { get; set; }
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
    public string InteractPrompt(IButtonPromptable player)
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

    public virtual ePedAlertType PedAlertTypes { get; set; } = ePedAlertType.UnconsciousBody;

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
    public int InsultLimit => IsGangMember || IsCop ? 2 : 3;
    public bool IsArrested { get; set; }
    public bool IsBusted { get; set; } = false;
    public bool IsCop { get; set; } = false;
    public bool IsDeadlyChase => PedViolations.IsDeadlyChase;
    public bool IsDealingDrugs { get; set; } = false;
    public bool IsDealingIllegalGuns { get; set; } = false;
    public bool IsDriver { get; private set; } = false;
    public bool IsDrunk { get; set; } = false;
    public bool IsFedUpWithPlayer => TimesInsultedByPlayer >= InsultLimit;
    public bool IsFreeModePed { get; set; } = false;
    public virtual bool IsGangMember { get; set; } = false;
    public bool IsInAPC { get; private set; }
    public bool IsInBoat { get; private set; } = false;
    public bool IsWaitingAtTrafficLight { get; set; }
    public bool IsTurningLeftAtTrafficLight { get; set; }
    public bool IsTurningRightAtTrafficLight { get; set; }
    public bool IsInHelicopter { get; private set; } = false;
    public bool IsInVehicle { get; private set; } = false;
    public bool IsInWrithe { get; set; } = false;
    public virtual bool IsMerchant { get; set; } = false;
    public bool IsMovingFast => GameTimeLastMovedFast != 0 && Game.GameTime - GameTimeLastMovedFast <= 2000;
    public bool IsNearSpawnPosition => Pedestrian.Exists() && Pedestrian.DistanceTo2D(SpawnPosition) <= 5f;////15f;//15f
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
    public int TimesInsultedByPlayer { get; private set; }
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
    public bool HasStartedEMTTreatment { get; set; } = false;
    public bool HasBeenLooted { get; set; } = false;
    public bool IsDead { get; set; } = false;
    public List<uint> BlackListedVehicles { get; set; } = new List<uint>();
    public bool WasModSpawned { get; set; } = false;
    public Vector3 SpawnPosition { get; set; }
    public float SpawnHeading { get; set; }
    public LocationTaskRequirements LocationTaskRequirements { get; set; } = new LocationTaskRequirements();
    public virtual bool KnowsDrugAreas => HasMenu || HasDrugAreaKnowledge;
    public virtual bool KnowsGangAreas => HasMenu || HasGangAreaKnowledge;
    public uint GameTimeReachedInvestigationPosition { get; set; }
    public bool HasFullBodyArmor { get; set; } = false;
    public virtual bool CanBeLooted { get; set; } = true;
    public virtual bool CanBeDragged { get; set; } = true;
    public virtual bool CanPlayRadioInAnimation => false;
    public bool AlwaysHasLongGun { get; set; } = false;
    public bool IsBeingHeldAsHostage { get; set; } = false;
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
    //protected virtual void UpdateAlerts(IPerceptable perceptable, IPoliceRespondable policeRespondable, IEntityProvideable world)
    //{
    //    if (Settings.SettingsManager.CivilianSettings.AllowCivilinsToCallEMTsOnBodies)
    //    {
    //        PedAlerts.LookForUnconsciousPeds(world);
    //    }
    //}
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
    public virtual void OnInsultedByPlayer(IInteractionable player)
    {
        if (GameTimeLastInsultedByPlayer == 0 || Game.GameTime - GameTimeLastInsultedByPlayer >= 1000)
        {
            TimesInsultedByPlayer += 1;
            GameTimeLastInsultedByPlayer = Game.GameTime;
            if(IsFedUpWithPlayer && player != null && Crimes != null)
            {
                AddWitnessedPlayerCrime(Crimes.CrimeList.FirstOrDefault(x => x.ID == "Harassment"), player.Character.Position);
                //EntryPoint.WriteToConsoleTestLong("Insulted by Player FED UP Adding Crime");
            }

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

                //EntryPoint.WriteToConsoleTestLong($"YELL IN PAIN {Pedestrian.Handle} YellType {YellType}");
            }
            else
            {
                PlaySpeech("GENERIC_FRIGHTENED_HIGH", false);
                //EntryPoint.WriteToConsoleTestLong($"CRY SPEECH FOR PAIN {Pedestrian.Handle}");
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
        PedDesires.OnItemsSoldToPlayer(modItem, numberPurchased);
    }
    public virtual void OnItemSold(ILocationInteractable player, ModItem modItem, int numberPurchased, int moneySpent)
    {
        PedDesires.OnItemsBoughtFromPlayer(modItem, numberPurchased);
    }
    public virtual void SetupTransactionItems(ShopMenu shopMenu)
    {
        // EntryPoint.WriteToConsole($"SetupTransactionItems START {Handle} HasMenu:{shopMenu == null} {shopMenu?.Name}");
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
        PedDesires.AddDesiredItem(shopMenu);
        //EntryPoint.WriteToConsole("SetupTransactionItems END");
    }
    public string LootInventory(IInteractionable player)
    {
        string ItemsFound = "";
        foreach (InventoryItem ii in PedInventory.ItemsList)
        {
            player.Inventory.Add(ii.ModItem, ii.RemainingPercent);
            ItemsFound += $"~n~~p~{ii.ModItem.Name}~s~ - {ii.RemainingPercent} {ii.ModItem.MeasurementName}(s)";
        }
        PedInventory.ItemsList.Clear();
        return ItemsFound;
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
        }

    }
    public void SetBaseStats(DispatchablePerson dispatchablePerson, IShopMenus shopMenus, IWeapons weapons, bool addBlip)
    {
        if (!Pedestrian.Exists())
        {
            return;
        }
        Pedestrian.Money = 0;
        IsTrustingOfPlayer = RandomItems.RandomPercent(Settings.SettingsManager.CivilianSettings.PercentageTrustingOfPlayer);// Gang.PercentageTrustingOfPlayer);
        Money = RandomItems.GetRandomNumberInt(Settings.SettingsManager.CivilianSettings.MoneyMin, Settings.SettingsManager.CivilianSettings.MoneyMax);
        WillFight = RandomItems.RandomPercent(CivilianFightPercentage());
        WillCallPolice = RandomItems.RandomPercent(CivilianCallPercentage());
        WillCallPoliceIntense = RandomItems.RandomPercent(CivilianSeriousCallPercentage());
        WillFightPolice = RandomItems.RandomPercent(CivilianFightPolicePercentage());
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
    }
    public void AddBlip()
    {
        if(!Pedestrian.Exists())
        {
            return;
        }
        Blip myBlip = Pedestrian.AttachBlip();
        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SET_BLIP_NAME("STRING");
        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(GroupName);
        NativeFunction.Natives.END_TEXT_COMMAND_SET_BLIP_NAME(myBlip);
        myBlip.Color = BlipColor;
        myBlip.Scale = BlipSize;
        AttachedLSRBlip = myBlip;
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
            Player.AddCrime(toReport.Crime, false, toReport.Location, toReport.Vehicle, toReport.Weapon, false, true, true);
        }
        OtherCrimesWitnessed.Clear();
    }



    //private void AddMedicalEventWitnessed(ITargetable Player)
    //{
    //    Player.AddMedicalEvent(PedAlerts.PositionLastSeenUnconsciousPed);
    //    PedAlerts.HasSeenUnconsciousPed = false;
    //}

    //public virtual void AddMedicalEventWitnessed(IPoliceRespondable Player, Vector3 position)
    //{
    //    Player.AddMedicalEvent(position);
    //    PedAlerts.HasSeenUnconsciousPed = false;
    //}

    private float CivilianCallPercentage()
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
    private float CivilianSeriousCallPercentage()
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
    private float CivilianFightPercentage()
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
    private float CivilianFightPolicePercentage()
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
        string Description = "";
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
        if (HasMenu)
        {
            Output += $"~n~Can Transact";
        }
        return Output;
    }
}
