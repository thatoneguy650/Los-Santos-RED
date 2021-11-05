
using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PedExt : IComplexTaskable
{ 
    private HealthState CurrentHealthState;
    private uint GameTimeLastExitedVehicle;
    private Entity Killer;
    private Entity LastHurtBy;
    private PedCrimes PedCrimes;
    private PlayerPerception PlayerPerception;
    private IPoliceRespondable PlayerToCheck;
    private ISettingsProvideable Settings;
    public PedExt(Ped _Pedestrian, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons)
    {
        Pedestrian = _Pedestrian;
        Handle = Pedestrian.Handle;
        Health = Pedestrian.Health;
        CurrentHealthState = new HealthState(this, settings);
        Settings = settings;
        PedCrimes = new PedCrimes(this, crimes, settings, weapons);
        PlayerPerception = new PlayerPerception(this, null, settings);
    }
    public PedExt(Ped _Pedestrian, ISettingsProvideable settings, bool _WillFight, bool _WillCallPolice, bool _IsGangMember, string _Name, PedGroup gameGroup, ICrimes crimes, IWeapons weapons) : this(_Pedestrian, settings, crimes, weapons)
    {
        WillFight = _WillFight;
        WillCallPolice = _WillCallPolice;
        IsGangMember = _IsGangMember;
        Name = _Name;
        PedGroup = gameGroup;
    }
    public bool CanBeAmbientTasked { get; set; } = true;
    public bool CanBeMugged => !IsCop && Pedestrian.Exists() && Pedestrian.IsAlive && !Pedestrian.IsStunned && !Pedestrian.IsRagdoll && (!Pedestrian.IsPersistent || Settings.SettingsManager.CivilianSettings.AllowMissionPedsToInteract);
    public bool CanBeTasked { get; set; } = true;
    public bool CanConverse => Pedestrian.Exists() && Pedestrian.IsAlive && !Pedestrian.IsFleeing && !Pedestrian.IsInCombat && !Pedestrian.IsSprinting && !Pedestrian.IsStunned && !Pedestrian.IsRagdoll && (!Pedestrian.IsPersistent || Settings.SettingsManager.CivilianSettings.AllowMissionPedsToInteract);
    public bool CanRecognizePlayer => PlayerPerception.CanRecognizeTarget;
    public bool CanRemove
    {
        get
        {
            if (!Pedestrian.Exists())
            {
                return true;
            }
            else if (Pedestrian.IsDead && CurrentHealthState.HasLoggedDeath)
            {
                return true;
            }
            return false;
        }
    }
    public bool CanSeePlayer => PlayerPerception.CanSeeTarget;
    public float ClosestDistanceToPlayer => PlayerPerception.ClosestDistanceToTarget;
    public List<Crime> CrimesCurrentlyViolating => PedCrimes.CrimesCurrentlyViolating;
    public List<Crime> CrimesWitnessed => PlayerPerception.CrimesWitnessed;
    public int CurrentlyViolatingWantedLevel => PedCrimes.CurrentlyViolatingWantedLevel;
    public ComplexTask CurrentTask { get; set; }
    public string DebugString => $"Handle: {Pedestrian.Handle} Distance {PlayerPerception.DistanceToTarget} See {PlayerPerception.CanSeeTarget} Md: {Pedestrian.Model.Name} Task: {CurrentTask?.Name} SubTask: {CurrentTask?.SubTaskName} InVeh {IsInVehicle}";
    public float DistanceToPlayer => PlayerPerception.DistanceToTarget;
    public bool EverSeenPlayer => PlayerPerception.EverSeenTarget;
    public string FormattedName => (HasSpokenWithPlayer ? Name : IsCop ? "Cop" : PedGroup?.MemberName);
    public uint GameTimeLastUpdated { get; private set; }
    public uint GameTimeLastUpdatedTask { get; set; }
    public uint Handle { get; private set; }
    public bool HasBeenHurtByPlayer { get; set; } = false;
    public bool HasBeenMugged { get; set; } = false;
    public bool HasSeenPlayerCommitCrime => PlayerPerception.CrimesWitnessed.Any();
    public bool HasSpokenWithPlayer { get; set; }
    public int Health { get; set; }
    public int InsultLimit => IsGangMember || IsCop ? 1 : 3;
    public bool IsCop { get; set; } = false;
    public bool IsCurrentlyViolatingAnyCrimes => PedCrimes.IsCurrentlyViolatingAnyCrimes;
    public bool IsDriver { get; private set; } = false;
    public bool IsFedUpWithPlayer => TimesInsultedByPlayer >= InsultLimit;
    public bool IsGangMember { get; set; } = false;
    public bool IsInBoat { get; private set; } = false;
    public bool IsInHelicopter { get; private set; } = false;
    public bool IsInVehicle { get; private set; } = false;
    public bool IsWanted => PedCrimes.IsWanted;
    public bool IsOnBike { get; private set; } = false;
    public bool IsRunningOwnFiber { get; set; } = false;
    public bool IsStill { get; private set; }
    public int LastSeatIndex { get; private set; }
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
    public bool NeedsTaskAssignmentCheck => Game.GameTime - GameTimeLastUpdatedTask >= 700;
    public Ped Pedestrian { get; private set; }
    public PedGroup PedGroup { get; private set; }
    public Vector3 PositionLastSeenCrime => PlayerPerception.PositionLastSeenCrime;
    public bool RecentlyGotOutOfVehicle => GameTimeLastExitedVehicle != 0 && Game.GameTime - GameTimeLastExitedVehicle <= 5000;
    public bool RecentlyUpdated => GameTimeLastUpdated != 0 && Game.GameTime - GameTimeLastUpdated < 2000;
    public uint TimeContinuoslySeenPlayer => PlayerPerception.TimeContinuoslySeenTarget;
    public int TimesInsultedByPlayer { get; set; }
    public VehicleExt VehicleLastSeenPlayerIn => PlayerPerception.VehicleLastSeenTargetIn;
    public string ViolationWantedLevelReason => PedCrimes.CurrentlyViolatingWantedLevelReason;
    public int WantedLevel => PedCrimes.WantedLevel;
    public bool IsDeadlyChase => PedCrimes.IsDeadlyChase;
    public WeaponInformation WeaponLastSeenPlayerWith => PlayerPerception.WeaponLastSeenTargetWith;
    public bool WillCallPolice { get; set; } = true;
    public bool WillFight { get; set; } = false;
    public bool WithinWeaponsAudioRange => PlayerPerception.WithinWeaponsAudioRange;
    private int FullUpdateInterval
    {
        get
        {
            if (IsCop)//IsCop)
            {
                if (PlayerPerception != null && PlayerPerception.DistanceToTarget >= 300)
                {
                    return 1500;
                }
                else
                {
                    return 500;//150
                }
            }
            else
            {
                if (PlayerPerception != null && PlayerPerception.DistanceToTarget >= 300)
                {
                    return 2000;
                }
                else
                {
                    return 750;//500// 750;//500
                }
            }
        }
    }
    public bool CheckHurtBy(Ped ToCheck)
    {
        if (LastHurtBy == ToCheck)
        {
            return true;
        }
        if (Pedestrian.Handle != ToCheck.Handle)
        {
            if (NativeFunction.CallByName<bool>("HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY", Pedestrian, ToCheck, true))
            {
                LastHurtBy = ToCheck;
                return true;

            }
            else if (ToCheck.IsInAnyVehicle(false) && NativeFunction.CallByName<bool>("HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY", Pedestrian, ToCheck.CurrentVehicle, true))
            {
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
            if (Pedestrian.Exists() && Pedestrian.IsDead && Pedestrian.Handle != ToCheck.Handle)
            {
                Killer = NativeFunction.Natives.GetPedSourceOfDeath<Entity>(Pedestrian);
                if (Killer.Handle == ToCheck.Handle || (ToCheck.IsInAnyVehicle(false) && ToCheck.Handle == Killer.Handle))
                {
                    return true;
                }
            }
            return false;
        }
        catch (Exception ex)
        {
            //EntryPoint.WriteToConsole($"KilledBy Error! Ped To Check: {Pedestrian.Handle}, assumeing you killed them if you hurt them");
            //return CheckHurtBy(ToCheck);
            return false;
        }

    }
    public bool SeenPlayerWithin(int msSince) => PlayerPerception.SeenTargetWithin(msSince);
    public void Update(IPerceptable perceptable, IPoliceRespondable policeRespondable, Vector3 placeLastSeen, IEntityProvideable world)
    {
        PlayerToCheck = policeRespondable;
        if (Pedestrian.Exists())
        {
            if (Pedestrian.IsAlive)
            {
                if (NeedsFullUpdate)
                {
                    PlayerPerception.Update(perceptable, placeLastSeen);
                    UpdateVehicleState();
                    if (!IsCop)
                    {
                        if (PlayerPerception.DistanceToTarget <= 150f)//only care in a bubble around the player, nothing to do with the player tho
                        {
                            PedCrimes.Update(world, policeRespondable);
                        }
                    }
                    GameTimeLastUpdated = Game.GameTime;
                }
            }
            CurrentHealthState.Update(policeRespondable);
        }
    }
    public void UpdateTask(List<PedExt> otherTargets)
    {
        if (CurrentTask != null)
        {
            CurrentTask.OtherTargets = otherTargets;
            CurrentTask.Update();
        }
    }
    private void UpdateVehicleState()
    {
        bool wasInVehicle = IsInVehicle;
        IsInVehicle = Pedestrian.IsInAnyVehicle(false);

        if (wasInVehicle != IsInVehicle)
        {
            if (IsInVehicle)//got in
            {

            }
            else//got out
            {
                GameTimeLastExitedVehicle = Game.GameTime;
            }
        }
        if (IsInVehicle && Pedestrian.CurrentVehicle.Exists())
        {
            IsDriver = Pedestrian.SeatIndex == -1;
            LastSeatIndex = Pedestrian.SeatIndex;
            IsInHelicopter = Pedestrian.IsInHelicopter;
            IsInBoat = Pedestrian.IsInBoat;
            if (!IsInHelicopter && !IsInBoat)
            {
                IsOnBike = Pedestrian.IsOnBike;
            }
        }
        else
        {
            IsInHelicopter = false;
            IsOnBike = false;
            IsDriver = false;
            IsInBoat = false;
        }
    }
}

