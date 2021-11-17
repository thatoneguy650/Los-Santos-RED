using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PlayerPerception
{
    private uint GameTimeLastSeenTargetCommitCrime;
    private uint GameTimeBehindTarget;
    private uint GameTimeContinuoslySeenTargetSince;
    private uint GameTimeLastDistanceCheck;
    private uint GameTimeLastLOSCheck;
    private uint GameTimeLastSeenTarget;
    private PedExt Originator;
    private ISettingsProvideable Settings;
    private IPerceptable Target;
    public PlayerPerception(PedExt originator, IPerceptable target, ISettingsProvideable settings)
    {
        Originator = originator;
        Target = target;
        Settings = settings;
    }
    public bool CanRecognizeTarget
    {
        get
        {

            if (TimeContinuoslySeenTarget >= 500)//1250
            {
                return true;
            }
            else if (CanSeeTarget && DistanceToTarget <= 8f && DistanceToTarget > 0.1f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public bool CanSeeTarget { get; private set; } = false;
    public float ClosestDistanceToTarget { get; private set; } = 2000f;
    public float DistanceToTarget { get; private set; } = 999f;
    public float DistanceToTargetLastSeen { get; private set; } = 999f;
    public bool EverSeenTarget => CanSeeTarget || GameTimeLastSeenTarget > 0;
    public bool HasSpokenWithTarget { get; set; }
    public bool IsFedUpWithTarget => TimesInsultedByTarget >= Originator.InsultLimit;
    public bool NeedsDistanceCheck
    {
        get
        {
            if (GameTimeLastDistanceCheck == 0)
            {
                return true;
            }
            else if (Game.GameTime > GameTimeLastDistanceCheck + DistanceUpdate)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public bool NeedsLOSCheck
    {
        get
        {
            if (DistanceToTarget >= 100)
            {
                return false;
            }
            else if (GameTimeLastLOSCheck == 0)
            {
                return true;
            }
            else if (Game.GameTime > GameTimeLastLOSCheck + LosUpdate)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public Vector3 PositionLastSeenTarget { get; private set; }
    public bool RecentlySeenTarget => CanSeeTarget || Game.GameTime - GameTimeLastSeenTarget <= 10000;
    public uint TimeContinuoslySeenTarget => GameTimeContinuoslySeenTargetSince == 0 ? 0 : Game.GameTime - GameTimeContinuoslySeenTargetSince;
    public int TimesInsultedByTarget { get; set; }
    public VehicleExt VehicleLastSeenTargetIn { get; set; }
    public WeaponInformation WeaponLastSeenTargetWith { get; set; }
    public bool WithinWeaponsAudioRange { get; private set; } = false;
    public List<Crime> CrimesWitnessed { get; private set; } = new List<Crime>();
    public bool HasSeenTargetCommitCrime => CrimesWitnessed.Any();
    public Vector3 PositionLastSeenCrime { get; private set; } = Vector3.Zero;
    private int DistanceUpdate
    {
        get
        {
            if (Originator.IsCop)//IsCop)
            {
                if (DistanceToTarget >= 300)
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
                if (DistanceToTarget >= 300)
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
    public bool NeedsUpdate
    {
        get
        {
            if ((NeedsDistanceCheck || NeedsLOSCheck) && Originator.Pedestrian.IsAlive)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
    private int LosUpdate => 750;
    public bool SeenTargetWithin(int msSince) => CanSeeTarget || Game.GameTime - GameTimeLastSeenTarget <= msSince;
    public void Update(IPerceptable target, Vector3 placeLastSeen)
    {
        Target = target;
        if (Originator != null && Originator.Pedestrian.Exists() && Originator.Pedestrian.IsAlive && Target != null && Target.Character.Exists())
        {
            UpdateTargetDistance(placeLastSeen);
            UpdateTargetLineOfSight(Target.IsWanted);
        }
        else
        {
            SetTargetSeen();
        }
        UpdateWitnessedCrimes();
    }
    private float GetDotVectorResult(Entity source, Entity target)
    {
        if (source.Exists() && target.Exists())
        {
            Vector3 dir = (target.Position - source.Position).ToNormalized();
            return Vector3.Dot(dir, source.ForwardVector);
        }
        else return -1.0f;
    }
    private bool IsBehind(Entity ToCheck)
    {
        if (GetDotVectorResult(ToCheck, Originator.Pedestrian) > 0)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    private bool IsInFrontOf(Ped ToCheck)
    {
        float Result = GetDotVectorResult(Originator.Pedestrian, ToCheck);
        if (Result > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool IsThisPedInFrontOf(Ped ToCheck)
    {
        float Result = GetDotVectorResult(ToCheck, Originator.Pedestrian);
        if (Result > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void SetTargetSeen()
    {
        CanSeeTarget = true;
        GameTimeLastSeenTarget = Game.GameTime;
        PositionLastSeenTarget = Target.Character.Position;
        VehicleLastSeenTargetIn = Target.CurrentSeenVehicle;
        WeaponLastSeenTargetWith = Target.CurrentSeenWeapon;
        if (GameTimeContinuoslySeenTargetSince == 0)
        {
            GameTimeContinuoslySeenTargetSince = Game.GameTime;
        }
    }
    private void SetTargetUnseen()
    {
        GameTimeContinuoslySeenTargetSince = 0;
        CanSeeTarget = false;
    }
    private void UpdateTargetDistance(Vector3 placeLastSeen)
    {
        if (!NeedsDistanceCheck || !Target.Character.Exists())
        {
            return;
        }
        Vector3 PositionToCheck = NativeFunction.Natives.GET_WORLD_POSITION_OF_ENTITY_BONE<Vector3>(Target.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Target.Character, 57005));// if you are in a car, your position is the mioddle of the car, hopefully this fixes that
        DistanceToTarget = Originator.Pedestrian.DistanceTo2D(PositionToCheck);
        if (Originator.IsCop)
        {
            DistanceToTargetLastSeen = Originator.Pedestrian.DistanceTo2D(placeLastSeen);
        }
        if (DistanceToTarget <= 0.1f)
        {
            DistanceToTarget = 999f;
        }
        if (DistanceToTarget <= ClosestDistanceToTarget)
        {
            ClosestDistanceToTarget = DistanceToTarget;
        }
        if (DistanceToTarget <= (Originator.IsCop ? Settings.SettingsManager.PoliceSettings.GunshotHearingDistance : Settings.SettingsManager.CivilianSettings.GunshotHearingDistance))//45f
        {
            WithinWeaponsAudioRange = true;
        }
        else
        {
            WithinWeaponsAudioRange = false;
        }
        if (!IsBehind(Target.Character))
        {
            if (GameTimeBehindTarget == 0)
            {
                GameTimeBehindTarget = Game.GameTime;
            }
        }
        else
        {
            GameTimeBehindTarget = 0;
        }
        GameTimeLastDistanceCheck = Game.GameTime;
    }
    private void UpdateTargetLineOfSight(bool IsWanted)
    {
        if (NeedsLOSCheck && Target.Character.Exists())
        {
            bool TargetInVehicle = Target.Character.IsInAnyVehicle(false);
            Entity ToCheck = TargetInVehicle ? (Entity)Target.Character.CurrentVehicle : (Entity)Target.Character;
            if (Originator.IsCop && !Originator.Pedestrian.IsInHelicopter)
            {
                if (DistanceToTarget <= Settings.SettingsManager.PoliceSettings.SightDistance && IsInFrontOf(Target.Character) && !Originator.Pedestrian.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Originator.Pedestrian, ToCheck))//55f
                {
                    SetTargetSeen();
                }
                else
                {
                    SetTargetUnseen();
                }
            }
            else if (Originator.Pedestrian.IsInHelicopter)
            {
                float DistanceToSee = Settings.SettingsManager.PoliceSettings.SightDistance_Helicopter;
                if (IsWanted)
                {
                    DistanceToSee += Settings.SettingsManager.PoliceSettings.SightDistance_Helicopter_AdditionalAtWanted;
                }
                if (DistanceToTarget <= DistanceToSee && !Originator.Pedestrian.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY", Originator.Pedestrian, ToCheck, 17))
                {
                    SetTargetSeen();
                }
                else
                {
                    SetTargetUnseen();
                }
            }
            else
            {
                if (DistanceToTarget <= Settings.SettingsManager.CivilianSettings.SightDistance && IsInFrontOf(Target.Character) && !Originator.Pedestrian.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Originator.Pedestrian, ToCheck))//55f
                {
                    SetTargetSeen();
                }
                else
                {
                    SetTargetUnseen();
                }
            }
            GameTimeLastLOSCheck = Game.GameTime;
        }
    }
    public void AddWitnessedCrime(Crime CrimeToAdd, Vector3 PositionToReport)
    {
        if (!CrimesWitnessed.Any(x => x.Name == CrimeToAdd.Name))
        {
            CrimesWitnessed.Add(CrimeToAdd);
            PositionLastSeenCrime = PositionToReport;
            GameTimeLastSeenTargetCommitCrime = Game.GameTime;
            EntryPoint.WriteToConsole($"AddWitnessedCrime Handle {Originator.Pedestrian.Handle} GameTimeLastReactedToCrime {GameTimeLastSeenTargetCommitCrime}, CrimeToAdd.Name {CrimeToAdd.Name}", 5);
        }
    }
    public void UpdateWitnessedCrimes()
    {
        foreach (Crime committing in Target.CivilianReportableCrimesViolating)
        {
            if (CanRecognizeTarget && !committing.CanReportBySound)
            {
                AddWitnessedCrime(committing, Originator.Pedestrian.Position);
            }
            else if (WithinWeaponsAudioRange && committing.CanReportBySound)
            {
                AddWitnessedCrime(committing, Originator.Pedestrian.Position);
            }
        }
    }
}

