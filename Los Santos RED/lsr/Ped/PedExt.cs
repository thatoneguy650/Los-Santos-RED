using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

public class PedExt : IComplexTaskable
{
    private HealthState CurrentHealthState;
    private uint GameTimeCreated = 0;
    private uint GameTimeLastEnteredVehicle;
    private uint GameTimeLastExitedVehicle;
    private uint GameTimeLastInsultedByPlayer;
    private uint GameTimeLastMovedFast;
    private bool hasCheckedWeapon = false;
    private Entity Killer;
    private uint KillerHandle;
    private Entity LastHurtBy;
    private PedCrimes PedCrimes;
    private PlayerPerception PlayerPerception;
    private IPoliceRespondable PlayerToCheck;
    private Vector3 position;
    private ISettingsProvideable Settings;
    private Vector3 SpawnPosition;
    private int TimeBetweenYelling = 5000;
    private uint GameTimeLastYelled;
    

    private bool IsYellingTimeOut => Game.GameTime - GameTimeLastYelled < TimeBetweenYelling;
    private bool CanYell => !IsYellingTimeOut;
    public PedExt(Ped _Pedestrian, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, string _Name, string groupName)
    {
        Pedestrian = _Pedestrian;
        Handle = Pedestrian.Handle;
        Health = Pedestrian.Health;
        SpawnPosition = Pedestrian.Position;
        Name = _Name;
        GameTimeCreated = Game.GameTime;
        GroupName = groupName;
        //if (PedGroup == null)
        //{
        //    PedGroup = new PedGroup(Pedestrian.RelationshipGroup.Name, Pedestrian.RelationshipGroup.Name, Pedestrian.RelationshipGroup.Name, false);
        //}
        CurrentHealthState = new HealthState(this, settings);
        Settings = settings;
        PedCrimes = new PedCrimes(this, crimes, settings, weapons);
        PlayerPerception = new PlayerPerception(this, null, settings);

        IsTrustingOfPlayer = RandomItems.RandomPercent(Settings.SettingsManager.CivilianSettings.PercentageTrustingOfPlayer);



    }
    public PedExt(Ped _Pedestrian, ISettingsProvideable settings, bool _WillFight, bool _WillCallPolice, bool _IsGangMember, bool isMerchant, string _Name, ICrimes crimes, IWeapons weapons, string groupName) : this(_Pedestrian, settings, crimes, weapons, _Name, groupName)
    {
        WillFight = _WillFight;
        WillCallPolice = _WillCallPolice;
        IsGangMember = _IsGangMember;
       // PedGroup = gameGroup;
        IsMerchant = isMerchant;
        Money = RandomItems.GetRandomNumberInt(Settings.SettingsManager.CivilianSettings.MoneyMin, Settings.SettingsManager.CivilianSettings.MoneyMax);
    }
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
    public int CellX { get; set; }
    public int CellY { get; set; }
    public float ClosestDistanceToPlayer => PlayerPerception.ClosestDistanceToTarget;
    public List<Crime> CrimesCurrentlyViolating => PedCrimes.CrimesCurrentlyViolating;
    public int CurrentlyViolatingWantedLevel => PedCrimes.CurrentlyViolatingWantedLevel;
    public ComplexTask CurrentTask { get; set; }
    public string DebugString => $"Handle: {Pedestrian.Handle} Distance {PlayerPerception.DistanceToTarget} See {PlayerPerception.CanSeeTarget} Md: {Pedestrian.Model.Name} Task: {CurrentTask?.Name} SubTask: {CurrentTask?.SubTaskName} InVeh {IsInVehicle}";
    public float DistanceToPlayer => PlayerPerception.DistanceToTarget;

    public float HeightToPlayer => PlayerPerception.HeightToTarget;


    public bool EverSeenPlayer => PlayerPerception.EverSeenTarget;




    public string FormattedName => (HasSpokenWithPlayer ? Name : GroupName);
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
    public bool HasSpokenWithPlayer { get; set; }
    public bool HatesPlayer { get; set; } = false;
    public int Health { get; set; }
    public int InsultLimit => IsGangMember || IsCop ? 2 : 3;
    public bool IsArrested { get; set; }
    public bool IsBusted { get; set; } = false;
    public bool IsCop { get; set; } = false;
    public bool IsCurrentlyViolatingAnyCivilianReportableCrimes => PedCrimes.IsCurrentlyViolatingAnyCrimes;
    public bool IsCurrentlyViolatingAnyCrimes => PedCrimes.IsCurrentlyViolatingAnyCrimes;


    public Crime WorstObservedCrime => PedCrimes.CrimesObservedViolating.OrderBy(x=> x.Priority).FirstOrDefault();
    public bool IsDeadlyChase => PedCrimes.IsDeadlyChase;
    public bool IsDealingDrugs { get; set; } = false;
    public bool IsDealingIllegalGuns { get; set; } = false;
    public bool IsDriver { get; private set; } = false;
    public bool IsDrunk { get; set; } = false;
    public bool IsFedUpWithPlayer => TimesInsultedByPlayer >= InsultLimit;
    public bool IsFreeModePed { get; set; } = false;
    public bool IsGangMember { get; set; } = false;
    public bool IsInAPC { get; private set; }
    public bool IsInBoat { get; private set; } = false;
    public bool IsInHelicopter { get; private set; } = false;
    public bool IsInVehicle { get; private set; } = false;
    public bool IsInWrithe { get; private set; } = false;
    public bool IsMerchant { get; set; } = false;
    public bool IsMovingFast => GameTimeLastMovedFast != 0 && Game.GameTime - GameTimeLastMovedFast <= 2000;
    public bool IsNearSpawnPosition => Pedestrian.DistanceTo2D(SpawnPosition) <= 10f;
    public bool IsOnBike { get; private set; } = false;
    public bool IsRunningOwnFiber { get; set; } = false;
    public bool IsStill { get; private set; }
    public bool IsSuicidal { get; set; } = false;

    public bool IsUnconscious { get; set; }

    public bool IsSuspicious { get; set; } = false;
    public bool IsWanted => PedCrimes.IsWanted;
    public bool IsNotWanted => PedCrimes.IsNotWanted;
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
            else if (Game.GameTime > GameTimeLastUpdated + FullUpdateInterval)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public bool NeedsTaskAssignmentCheck => Game.GameTime - GameTimeLastUpdatedTask >= (IsCop ? 500 : 700);
    public List<WitnessedCrime> OtherCrimesWitnessed => PedCrimes.OtherCrimesWitnessed;
    public Ped Pedestrian { get; set; }
   // public PedGroup PedGroup { get; private set; }
    public List<Crime> PlayerCrimesWitnessed => PlayerPerception.CrimesWitnessed;
    public Vector3 PositionLastSeenCrime => PlayerPerception.PositionLastSeenCrime;
    public bool RecentlyGotInVehicle => GameTimeLastEnteredVehicle != 0 && Game.GameTime - GameTimeLastEnteredVehicle <= 1000;
    public bool RecentlyGotOutOfVehicle => GameTimeLastExitedVehicle != 0 && Game.GameTime - GameTimeLastExitedVehicle <= 1000;
    public bool RecentlyUpdated => GameTimeLastUpdated != 0 && Game.GameTime - GameTimeLastUpdated < 2000;
    public int RelationShipFromPlayer { get; set; } = 255;
    public int RelationShipToPlayer { get; set; } = 255;
    public uint TimeContinuoslySeenPlayer => PlayerPerception.TimeContinuoslySeenTarget;
    public int TimesInsultedByPlayer { get; private set; }
    //public List<MenuItem> TransactionMenu { get; set; }

    public Vector3 PositionLastSeenDistressedPed { get; set; }

    public ShopMenu ShopMenu { get; set; }

    public VehicleExt VehicleLastSeenPlayerIn => PlayerPerception.VehicleLastSeenTargetIn;
    public string ViolationWantedLevelReason => PedCrimes.CurrentlyViolatingWantedLevelReason;
    public int WantedLevel => PedCrimes.WantedLevel;
    public bool WasEverSetPersistent { get; set; }
    public bool WasSetCriminal { get; set; } = false;
    public WeaponInformation WeaponLastSeenPlayerWith => PlayerPerception.WeaponLastSeenTargetWith;
    public bool WillCallPolice { get; set; } = false;//true;
    public bool WillFight { get; set; } = false;
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
                    return 3000;
                }
                else if (PlayerPerception?.DistanceToTarget >= 200)
                {
                    return 2000;
                }
                else if (PlayerPerception?.DistanceToTarget >= 50f)
                {
                    return 750;//1000
                }
                else
                {
                    return 500;
                }
            }
            else
            {
                if (PlayerPerception?.DistanceToTarget >= 300)
                {
                    return 4000;
                }
                else if (PlayerPerception?.DistanceToTarget >= 200)
                {
                    return 3000;
                }
                else if (PlayerPerception?.DistanceToTarget >= 50f)
                {
                    return 1000;//2000
                }
                else
                {
                    return 750;
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
                PlayerToCheck.GangRelationships.ChangeReputation(gm.Gang, 100, true);
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
                PlayerToCheck.GangRelationships.ChangeReputation(gm.Gang, -100, true);
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
        if (PedCrimes.WantedLevel < toSet)
        {
            PedCrimes.WantedLevel = toSet;
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
                        if (PlayerPerception.DistanceToTarget <= 150f)//only care in a bubble around the player, nothing to do with the player tho
                        {
                            PedCrimes.Update(world, policeRespondable);//possible yield in here!
                        }
                        if (Pedestrian.Exists() && policeRespondable.IsCop)
                        {
                            CheckPlayerBusted();
                        }
                    }
                    if(Pedestrian.Exists() && !IsUnconscious && !HasSeenDistressedPed && PlayerPerception.DistanceToTarget <= 150f)//only care in a bubble around the player, nothing to do with the player tho
                    {
                        LookForDistressedPeds(world);
                    }
                    if(IsCop && HasSeenDistressedPed)
                    {
                        perceptable.AddDistressedPed(PositionLastSeenDistressedPed);
                        HasSeenDistressedPed = false;
                    }

                    GameTimeLastUpdated = Game.GameTime;
                }
            }
            CurrentHealthState.Update(policeRespondable);
        }
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
        if (PlayerPerception.DistanceToTarget <= 5f)
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
        PedCrimes.Reset();
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