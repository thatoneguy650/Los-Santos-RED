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
    private IPoliceRespondable PlayerToCheck;
    private ISettingsProvideable Settings;
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

    public Vector3 SpawnPosition { get; set; }
    private int TimeBetweenYelling = 5000;
    private uint GameTimeLastYelled;
    private RelationshipGroup originalGroup;

    private bool ShouldCheckCrimes
    {
        get
        {
            if (Pedestrian.Exists())
            {
                string RelationshipGroupName = Pedestrian.RelationshipGroup.Name;//weirdness withthis bullshit
                if (RelationshipGroupName == string.Empty)
                {
                    EntryPoint.WriteToConsole($" PedExt.Pedestrian {Pedestrian.Handle} RelationshipGroupName {RelationshipGroupName} RelationshipGroupName2 A{RelationshipGroupName}A");
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
        CurrentHealthState = new HealthState(this, settings, false);
        Settings = settings;
        PedViolations = new PedViolations(this, crimes, settings, weapons, world);
        PedPerception = new PedPerception(this, crimes, settings, weapons, world);
        PlayerPerception = new PlayerPerception(this, null, settings);
        PedReactions = new PedReactions(this);
        IsTrustingOfPlayer = RandomItems.RandomPercent(Settings.SettingsManager.CivilianSettings.PercentageTrustingOfPlayer);


        UpdateJitter = RandomItems.GetRandomNumber(100, 200);

    }
    public PedExt(Ped _Pedestrian, ISettingsProvideable settings, bool _WillFight, bool _WillCallPolice, bool _IsGangMember, bool isMerchant, string _Name, ICrimes crimes, IWeapons weapons, string groupName, IEntityProvideable world, bool willFightPolice) : this(_Pedestrian, settings, crimes, weapons, _Name, groupName, world)
    {
        WillFight = _WillFight;
        WillFightPolice = willFightPolice;
        WillCallPolice = _WillCallPolice;
        IsGangMember = _IsGangMember;
        IsMerchant = isMerchant;
        Money = RandomItems.GetRandomNumberInt(Settings.SettingsManager.CivilianSettings.MoneyMin, Settings.SettingsManager.CivilianSettings.MoneyMax);
    }
    public PedViolations PedViolations { get; private set; }
    public PedPerception PedPerception { get; private set; }
    public PlayerPerception PlayerPerception { get; private set; }
    public HealthState CurrentHealthState { get; private set; }

    public PedReactions PedReactions { get; set; }
    public uint ArrestingPedHandle { get; set; } = 0;
    public List<Cop> AssignedCops { get; set; } = new List<Cop>();
    public int AssignedSeat { get; set; }
    public VehicleExt AssignedVehicle { get; set; }
    public bool CanAttackPlayer => IsFedUpWithPlayer || HatesPlayer;
    public bool CanBeAmbientTasked { get; set; } = true;
    public bool CanBeMugged => !IsCop && Pedestrian.Exists() && !IsBusted && !IsUnconscious && !IsDead && !IsArrested && Pedestrian.IsAlive && !Pedestrian.IsStunned && !Pedestrian.IsRagdoll && (!Pedestrian.IsPersistent || Settings.SettingsManager.CivilianSettings.AllowMissionPedsToInteract || IsMerchant || IsGangMember || WasModSpawned);
    public bool CanBeTasked { get; set; } = true;
    public bool CanConverse => Pedestrian.Exists() && !IsBusted && !IsUnconscious && !IsDead && !IsArrested && Pedestrian.IsAlive && !Pedestrian.IsFleeing && !Pedestrian.IsInCombat && !Pedestrian.IsSprinting && !Pedestrian.IsStunned && !Pedestrian.IsRagdoll && (!Pedestrian.IsPersistent || Settings.SettingsManager.CivilianSettings.AllowMissionPedsToInteract || IsCop || IsMerchant || IsGangMember || WasModSpawned);
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
    public bool IsTrustingOfPlayer { get; set; } = true;
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
    public string GroupName { get; set; } = "Person";
    public uint GameTimeLastUpdated { get; private set; }
    public uint GameTimeLastUpdatedTask { get; set; }
    public uint Handle { get; private set; }
    public bool HasBeenCarJackedByPlayer { get; set; } = false;
    public bool HasBeenHurtByPlayer { get; set; } = false;
    public bool HasBeenMugged { get; set; } = false;
    public uint HasExistedFor => Game.GameTime - GameTimeCreated;
    public bool HasLoggedDeath => CurrentHealthState.HasLoggedDeath;
    public bool HasMenu => ShopMenu != null && ShopMenu.Items.Any();// TransactionMenu != null && TransactionMenu.Any();
    public bool HasSeenPlayerCommitCrime => PlayerPerception.CrimesWitnessed.Any();
    public bool HasBeenTreatedByEMTs { get; set; }
    public bool HasSeenPlayerCommitMajorCrime => PlayerPerception.CrimesWitnessed.Any(x=> x.AngersCivilians || x.ScaresCivilians);
    public bool HasSeenPlayerCommitTrafficCrime => PlayerPerception.CrimesWitnessed.Any(x => x.IsTrafficViolation);
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
    public bool IsGangMember { get; set; } = false;
    public bool IsInAPC { get; private set; }
    public bool IsInBoat { get; private set; } = false;
    public bool IsWaitingAtTrafficLight { get; set; }
    public bool IsTurningLeftAtTrafficLight { get; set; }
    public bool IsTurningRightAtTrafficLight { get; set; }
    public bool IsInHelicopter { get; private set; } = false;
    public bool IsInVehicle { get; private set; } = false;
    public bool IsInWrithe { get; private set; } = false;
    public bool IsMerchant { get; set; } = false;
    public bool IsMovingFast => GameTimeLastMovedFast != 0 && Game.GameTime - GameTimeLastMovedFast <= 2000;
    public bool IsNearSpawnPosition =>  Pedestrian.DistanceTo2D(SpawnPosition) <= 15f;//15f
    public bool IsOnBike { get; private set; } = false;
    public bool IsRunningOwnFiber { get; set; } = false;
    public bool IsStill { get; private set; }
    public bool IsSuicidal { get; set; } = false;
    public bool IsUnconscious { get; set; }
    public bool IsAmbientSpawn { get; set; } = false;
    public bool IsSuspicious { get; set; } = false;
    public bool IsWanted => PedViolations.IsWanted;
    public bool IsNotWanted => PedViolations.IsNotWanted;
    public bool IsZombie { get; set; } = false;
    public int LastSeatIndex { get; private set; } = -1;
    public int Money { get; set; } = 10;
    public string Name { get; set; }
    public bool NeedsFullUpdate
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
    public bool NeedsTaskAssignmentCheck => Game.GameTime - GameTimeLastUpdatedTask >= Settings.SettingsManager.DebugSettings.TaskAssignmentCheckFrequency;// (IsCop ? 500 : 700);
    public List<WitnessedCrime> OtherCrimesWitnessed => PedPerception.OtherCrimesWitnessed;
    public Ped Pedestrian { get; set; }
    public List<Crime> PlayerCrimesWitnessed => PlayerPerception.CrimesWitnessed;
    public Vector3 PositionLastSeenCrime => PlayerPerception.PositionLastSeenCrime;
    public bool RecentlyGotInVehicle => GameTimeLastEnteredVehicle != 0 && Game.GameTime - GameTimeLastEnteredVehicle <= 2000;//was 4000//was 1000
    public bool RecentlyGotOutOfVehicle => GameTimeLastExitedVehicle != 0 && Game.GameTime - GameTimeLastExitedVehicle <= 2000;//was 4000//was 1000
    public bool RecentlyUpdated => GameTimeLastUpdated != 0 && Game.GameTime - GameTimeLastUpdated < 2000;
    public int RelationShipFromPlayer { get; set; } = 255;
    public int RelationShipToPlayer { get; set; } = 255;
    public uint TimeContinuoslySeenPlayer => PlayerPerception.TimeContinuoslySeenTarget;
    public int TimesInsultedByPlayer { get; private set; }
    public Vector3 PositionLastSeenDistressedPed { get; set; }
    public ShopMenu ShopMenu { get; set; }
    public VehicleExt VehicleLastSeenPlayerIn => PlayerPerception.VehicleLastSeenTargetIn;
    public int WantedLevel => PedViolations.WantedLevel;
    public bool WasEverSetPersistent { get; set; }
    public bool WasPersistentOnCreate { get; set; } = false;
    public bool WasSetCriminal { get; set; } = false;
    public WeaponInformation WeaponLastSeenPlayerWith => PlayerPerception.WeaponLastSeenTargetWith;
    public bool WillCallPolice { get; set; } = false;//true;
    public bool WillCallPoliceIntense { get; set; } = false;//true;
    public bool WillFight { get; set; } = false;


    public bool IsGroupMember { get; set; } = false;
    public bool WillFightPolice { get; set; } = false;

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
                    return Settings.SettingsManager.DebugSettings.CopUpdateIntervalVeryFar;
                }
                else if (PlayerPerception?.DistanceToTarget >= 200)
                {
                    return Settings.SettingsManager.DebugSettings.CopUpdateIntervalFar;
                }
                else if (PlayerPerception?.DistanceToTarget >= 50f)
                {
                    return Settings.SettingsManager.DebugSettings.CopUpdateIntervalMedium;
                }
                else
                {
                    return Settings.SettingsManager.DebugSettings.CopUpdateIntervalClose;
                }
            }
            else
            {
                if (PlayerPerception?.DistanceToTarget >= 100f)//300
                {
                    return Settings.SettingsManager.DebugSettings.OtherUpdateIntervalVeryFar;
                }
                else if (PlayerPerception?.DistanceToTarget >= 50f)//200
                {
                    return Settings.SettingsManager.DebugSettings.OtherUpdateIntervalFar;
                }
                else if (IsWanted)
                {
                    return Settings.SettingsManager.DebugSettings.OtherUpdateIntervalWanted;
                }
                else if (PlayerPerception?.DistanceToTarget >= 25f)//50f
                {
                    return Settings.SettingsManager.DebugSettings.OtherUpdateIntervalMedium;//2000
                }
                else
                {
                    return Settings.SettingsManager.DebugSettings.OtherUpdateIntervalClose;
                }
            }
        }
    }
    public uint GameTimeLastInjured { get; set; }
    public bool RecentlyInjured => GameTimeLastInjured != 0 && Game.GameTime - GameTimeLastInjured <= 3000;
    public bool HasSeenDistressedPed { get; set; } = false;
    public bool HasBeenSeenInDistress { get; set; } = false;
    public bool HasStartedEMTTreatment { get; set; } = false;
    public bool HasBeenLooted { get; set; } = false;
    public bool IsDead { get; set; } = false;
    public bool WasModSpawned { get; set; } = false;
    public List<uint> BlackListedVehicles { get; set; } = new List<uint>();
    public void Update(IPerceptable perceptable, IPoliceRespondable policeRespondable, Vector3 placeLastSeen, IEntityProvideable world)
    {
        PlayerToCheck = policeRespondable;
        if (Pedestrian.Exists())
        {
            if (Pedestrian.IsAlive)
            {
                if (NeedsFullUpdate)
                {
                    IsInWrithe = Pedestrian.IsInWrithe;
                    UpdatePositionData();
                    PlayerPerception.Update(perceptable, placeLastSeen);
                    UpdateVehicleState();
                    if (!IsCop && !IsUnconscious)
                    {
                        if (PlayerPerception.DistanceToTarget <= 200f && ShouldCheckCrimes)//was 150 only care in a bubble around the player, nothing to do with the player tho
                        {
                            // if (IsGangMember)
                            //{


                            if (Settings.SettingsManager.DebugSettings.PedUpdatePerformanceMode && (!PlayerPerception.RanSightThisUpdate || IsGangMember))
                            {

                                GameFiber.Yield();//TR TEST 28
                            }
                            //}
                            PedViolations.Update(policeRespondable);//possible yield in here!, REMOVED FOR NOW
                            //if(IsGangMember)
                            //{
                              //  GameFiber.Yield();//TR TEST 28
                            //}
                            PedPerception.Update();
                            if (Settings.SettingsManager.DebugSettings.PedUpdatePerformanceMode2 && (!PlayerPerception.RanSightThisUpdate || IsGangMember))
                            {

                                GameFiber.Yield();//TR TEST 28
                            }
                        }
                        if (Pedestrian.Exists() && policeRespondable.IsCop && !policeRespondable.IsIncapacitated)
                        {
                            CheckPlayerBusted();
                        }
                    }
                    if (Pedestrian.Exists() && Settings.SettingsManager.CivilianSettings.AllowCivilinsToCallPoliceOnBodies && !IsUnconscious && !HasSeenDistressedPed && PlayerPerception.DistanceToTarget <= 150f)//only care in a bubble around the player, nothing to do with the player tho
                    {
                        LookForDistressedPeds(world);
                    }
                    if (IsCop && HasSeenDistressedPed)
                    {
                        perceptable.AddMedicalEvent(PositionLastSeenDistressedPed);
                        HasSeenDistressedPed = false;
                    }
                    GameTimeLastUpdated = Game.GameTime;
                }
            }
            CurrentHealthState.Update(policeRespondable);//has a yield if they get damaged, seems ok
        }
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
            EntryPoint.WriteToConsole($"{Pedestrian.Handle} BECAME WANTED (CIVILIAN) SET TO CRIMINALS");     
        }
    }

    public virtual void OnLostWanted()
    {
        if (Pedestrian.Exists())
        {
            Pedestrian.RelationshipGroup = originalGroup;
            PedViolations.Reset();
            EntryPoint.WriteToConsole($"{Pedestrian.Handle} LOST WANTED (CIVILIAN) SET TO ORIGINAL GROUP {originalGroup.Name}");
        }   
    }
    public void AddWitnessedPlayerCrime(Crime CrimeToAdd, Vector3 PositionToReport) => PlayerPerception.AddWitnessedCrime(CrimeToAdd, PositionToReport);
    public void ApolgizedToPlayer()
    {
        if (GameTimeLastInsultedByPlayer == 0 || Game.GameTime - GameTimeLastInsultedByPlayer >= 1000)
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
            EntryPoint.WriteToConsole($"PEDEXT: CheckHurtBy Ped To Check: {Pedestrian.Handle} (LAST) Hurt by {ToCheck.Handle}", 5);
            return true;
        }
        if (!OnlyLast && Pedestrian.Handle != ToCheck.Handle)
        {
            if (NativeFunction.CallByName<bool>("HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY", Pedestrian, ToCheck, true))
            {
                EntryPoint.WriteToConsole($"PEDEXT: CheckHurtBy Ped To Check: {Pedestrian.Handle} (NEW PERSON) Hurt by {ToCheck.Handle}", 5);
                LastHurtBy = ToCheck;
                return true;
            }
            else if (ToCheck.IsInAnyVehicle(false) && NativeFunction.CallByName<bool>("HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY", Pedestrian, ToCheck.CurrentVehicle, true))
            {
                EntryPoint.WriteToConsole($"PEDEXT: CheckHurtBy Ped To Check: {Pedestrian.Handle} (NEW CAR) Hurt by {ToCheck.Handle}", 5);
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
                    EntryPoint.WriteToConsole($"PEDEXT: CheckKilledBy Ped To Check: {Pedestrian.Handle} Killed by {KillerHandle}", 5);
                    return true;
                }
                else if (KillerHandle == 0 && CheckHurtBy(ToCheck, true))
                {
                    EntryPoint.WriteToConsole($"PEDEXT: CheckKilledBy Ped To Check: {Pedestrian.Handle} Killer is 0 Last Hurt By {ToCheck.Handle}", 5);
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
    public void InsultedByPlayer()
    {
        if (GameTimeLastInsultedByPlayer == 0 || Game.GameTime - GameTimeLastInsultedByPlayer >= 1000)
        {
            TimesInsultedByPlayer += 1;
            GameTimeLastInsultedByPlayer = Game.GameTime;
            if (this.GetType() == typeof(GangMember))
            {
                GangMember gm = (GangMember)this;
                PlayerToCheck.RelationshipManager.GangRelationships.ChangeReputation(gm.Gang, -100, true);
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
                EntryPoint.WriteToConsole($"PEDEXT: LogSourceOfDeath Ped To Check: {Pedestrian.Handle} killed by {KillerHandle}", 5);
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole($"PEDEXT: LogSourceOfDeath Error! Ped To Check: {Pedestrian.Handle} {ex.Message} {ex.StackTrace}", 5);
            }
        }
    }
    public void LogSourceOfDeathOld()
    {
        if (Pedestrian.Exists() && Pedestrian.IsDead)
        {
            try
            {
                Killer = NativeFunction.Natives.GetPedSourceOfDeath<Entity>(Pedestrian);
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
    private void LookForDistressedPeds(IEntityProvideable world)
    {
        foreach(PedExt distressedPed in world.Pedestrians.PedExts.Where(x=> (x.IsUnconscious || x.IsInWrithe) && !x.HasStartedEMTTreatment && !x.HasBeenTreatedByEMTs && NativeHelper.IsNearby(CellX, CellY, x.CellX, x.CellY, 4) && x.Pedestrian.Exists()))
        {
            float distanceToBody = Pedestrian.DistanceTo2D(distressedPed.Pedestrian);
            if (distanceToBody <= 15f || (distanceToBody <= 45f && distressedPed.Pedestrian.IsThisPedInFrontOf(Pedestrian) && (distressedPed.HasBeenSeenInDistress || NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Pedestrian, distressedPed.Pedestrian))))//if someone saw it assume ANYONE close saw it, only performance reason
            {
                PositionLastSeenDistressedPed = distressedPed.position;
                HasSeenDistressedPed = true;
                distressedPed.HasBeenSeenInDistress = true;
                break;
            }   
        }
    }
    private void CheckPlayerBusted()
    {
        if (PlayerPerception.DistanceToTarget <= Settings.SettingsManager.PoliceSettings.BustDistance)
        {
            if (Pedestrian.Exists() && (Pedestrian.IsStunned || Pedestrian.IsRagdoll) && !IsBusted)
            {
                SetBusted();
                EntryPoint.WriteToConsole($"PEDEXT: Player bust {Pedestrian.Handle}", 3);
            }
        }
    }
    private void UpdatePositionData()
    {
        position = Pedestrian.Position;//See which cell it is in now
        CellX = (int)(position.X / EntryPoint.CellSize);
        CellY = (int)(position.Y / EntryPoint.CellSize);
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
        PlayerPerception.CrimesWitnessed.Clear();
    }
    public void ResetCrimes()
    {
        PedViolations.Reset();
        PedPerception.Reset();
    }
    private void PlaySpeech(string speechName, bool useMegaphone)
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
            EntryPoint.WriteToConsole($"FREEMODE COP SPEAK {Pedestrian.Handle} freeModeVoice {VoiceName} speechName {speechName}");
        }
        else
        {
            Pedestrian.PlayAmbientSpeech(speechName, useMegaphone);
        }
    }
}