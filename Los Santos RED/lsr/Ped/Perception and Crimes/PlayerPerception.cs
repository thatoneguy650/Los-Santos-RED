using LosSantosRED.lsr.Helper;
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
    //private uint GameTimeBehindTarget;
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
            //if (DistanceToTarget >= 100)
            //{
            //    return false;
            //}
            //else 
            if (GameTimeLastLOSCheck == 0)
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
    private int DistanceUpdate//also need to change the full update interval for this to work
    {
        get
        {
            if (Originator.IsCop)//IsCop)
            {
                if (DistanceToTarget >= 300)
                {
                    return 1500;
                }
                if (DistanceToTarget >= 80F)
                {
                    return 300;
                }
                else
                {
                    return 250;//150
                }
            }
            else
            {

                if (DistanceToTarget >= 300)
                {
                    return 2000;
                }
                if (DistanceToTarget >= 80F)
                {
                    return 300;
                }
                else
                {
                    return 250;// 750;//500// 750;//500
                }
            }
        }
    }
    private int LosUpdate//also need to change the full update interval for this to work
    {
        get
        {
            if (DistanceToTarget >= 300)
            {
                return 2000;
            }
            if (DistanceToTarget >= 80F)
            {
                return 750;
            }
            else
            {
                return 350;//500// 750;//500
            }
        }
    }
    //private int DistanceUpdate
    //{
    //    get
    //    {
    //        if (Originator.IsCop)//IsCop)
    //        {
    //            if (DistanceToTarget >= 300)
    //            {
    //                return 1500;
    //            }
    //            else
    //            {
    //                return 500;//150
    //            }
    //        }
    //        else
    //        {

    //            if (DistanceToTarget >= 300)
    //            {
    //                return 2000;
    //            }
    //            else
    //            {
    //                return 500;// 750;//500// 750;//500
    //            }
    //        }
    //    }
    //}
    //private int LosUpdate
    //{
    //    get
    //    {
    //        if (DistanceToTarget >= 300)
    //        {
    //            return 2000;
    //        }
    //        else
    //        {
    //            return 750;//500// 750;//500
    //        }
    //    }
    //}
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
   // private int LosUpdate => 750;
    public bool SeenTargetWithin(int msSince) => CanSeeTarget || Game.GameTime - GameTimeLastSeenTarget <= msSince;
    public void Update(IPerceptable target, Vector3 placeLastSeen)
    {
        Target = target;
        if (Originator != null && Originator.Pedestrian.Exists() && Originator.Pedestrian.IsAlive && Target != null && Target.Character.Exists())
        {
            bool distanceRan = UpdateTargetDistance(placeLastSeen, target.Position);
            bool losRan = UpdateTargetLineOfSight(Target.IsWanted);
            if(distanceRan || losRan)
            {
               // GameFiber.Yield();//TR, 2 was getting rid of both running, this is just running one if either are done, maybe just do vision?
            }

            if(losRan && Originator.IsCop)
            {
                GameFiber.Yield();//TR
            }

            UpdateWitnessedCrimes();
            //GameFiber.Yield();
        }
        else
        {
            SetTargetUnseen();// SetTargetSeen();//maybe unseen here? not sure....
        }
        //UpdateWitnessedCrimes();
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
    private bool UpdateTargetDistance(Vector3 placeLastSeen, Vector3 posToCheck)
    {
        if (!NeedsDistanceCheck || !Target.Character.Exists())
        {
            return false;
        }
        Vector3 PositionToCheck = posToCheck;
        if(Originator.IsCop)
        {
            DistanceToTarget = Originator.Pedestrian.DistanceTo2D(PositionToCheck);
            //GameFiber.Yield();//TR is new, doesnt seem to do much....
        }
        else
        {
            DistanceToTarget = Originator.Pedestrian.DistanceTo2D(PositionToCheck);
            //int maxCellsAway =  NativeHelper.MaxCellsAway(Target.CellX, Target.CellY, Originator.CellX, Originator.CellY);
            //if (maxCellsAway <= 2)
            //{
            //    DistanceToTarget = Originator.Pedestrian.DistanceTo2D(PositionToCheck);
            //}
            //else
            //{
            //    DistanceToTarget = maxCellsAway * 70f;
            //}
        }
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
        GameTimeLastDistanceCheck = Game.GameTime;
        //GameFiber.Yield();//TR Yield RemovedTest 2
        return true;
    }
    private bool UpdateTargetLineOfSight(bool IsWanted)
    {
        if (DistanceToTarget >= 100f)//this is new
        {
            SetTargetUnseen();
            return false;
        }
        if (NeedsLOSCheck && Target.Character.Exists() && Originator.Pedestrian.Exists())
        {     
            bool TargetInVehicle = Target.Character.IsInAnyVehicle(false);
            Entity ToCheck = TargetInVehicle ? (Entity)Target.Character.CurrentVehicle : (Entity)Target.Character;
            if(TargetInVehicle && DistanceToTarget <= 20f && !Originator.Pedestrian.IsDead)//this is new...., cops should be able to see behind themselves a short distance
            {
                SetTargetSeen();
            }
            else if (!TargetInVehicle && DistanceToTarget <= 8f && !Originator.Pedestrian.IsDead)//this is new...., cops should be able to see behind themselves a short distance
            {
                SetTargetSeen();
            }
            else if (Originator.IsCop && !Originator.Pedestrian.IsInHelicopter)
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
            //GameFiber.Yield();//TR Yield RemovedTest 2
            return true;
        }
        return false;
    }
    public void AddWitnessedCrime(Crime CrimeToAdd, Vector3 PositionToReport)
    {
        if (!CrimesWitnessed.Any(x => x.Name == CrimeToAdd.Name))
        {
            CrimesWitnessed.Add(CrimeToAdd);
            PositionLastSeenCrime = PositionToReport;
            GameTimeLastSeenTargetCommitCrime = Game.GameTime;
            //EntryPoint.WriteToConsole($"AddWitnessedCrime Handle {Originator.Pedestrian.Handle} GameTimeLastReactedToCrime {GameTimeLastSeenTargetCommitCrime}, CrimeToAdd.Name {CrimeToAdd.Name}", 5);
        }
    }
    public void UpdateWitnessedCrimes()
    {
        if (Originator.Pedestrian.Exists())
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
}

