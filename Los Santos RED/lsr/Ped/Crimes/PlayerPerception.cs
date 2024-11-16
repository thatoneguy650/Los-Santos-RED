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
    private uint GameTimeContinuoslySeenTargetSince;
    private uint GameTimeLastDistanceCheck;
    private uint GameTimeLastLOSCheck;
    private uint GameTimeLastSeenTarget;
    private PedExt Originator;
    private ISettingsProvideable Settings;
    private IPerceptable Target;
    private uint GameTimeLastSetFakeSeen;
    private bool IsFakeSeen => GameTimeLastSetFakeSeen != 0 && Game.GameTime - GameTimeLastSetFakeSeen <= 5000;

    private float DistanceToTargetInVehicle => Settings.SettingsManager.PlayerOtherSettings.SeeBehindDistanceVehicle;
    private float DistanceToTargetOnFoot => Target.Stance.IsBeingStealthy ? Settings.SettingsManager.PlayerOtherSettings.SeeBehindDistanceStealth : Settings.SettingsManager.PlayerOtherSettings.SeeBehindDistanceRegular; //0.25f : 4f;
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
    public float HeightToTarget { get; private set; } = 999f;
    public float DistanceToTargetLastSeen { get; private set; } = 999f;
    public bool EverSeenTarget => CanSeeTarget || GameTimeLastSeenTarget > 0;
    //public bool EverRecognizedTarget => CanRecognizeTarget || GameTimeLastSeenTarget > 0;
    public bool HasSpokenWithTarget { get; set; }
    public bool IsFedUpWithTarget => TimesInsultedByTarget >= Originator.InsultLimit;
    public bool NeedsDistanceCheck => true;
    public bool NeedsLOSCheck => true;
    public Vector3 PositionLastSeenTarget { get; private set; }
    public bool RecentlySeenTarget => CanSeeTarget || Game.GameTime - GameTimeLastSeenTarget <= 10000;
    public uint TimeContinuoslySeenTarget => GameTimeContinuoslySeenTargetSince == 0 ? 0 : Game.GameTime - GameTimeContinuoslySeenTargetSince;
    public int TimesInsultedByTarget { get; set; }
    public VehicleExt VehicleLastSeenTargetIn { get; set; }
    public WeaponInformation WeaponLastSeenTargetWith { get; set; }
    public bool WithinWeaponsAudioRange { get; private set; } = false;
    public bool HasSeenTargetWithin(uint time) => CanSeeTarget || Game.GameTime - GameTimeLastSeenTarget <= time;
    public List<WitnessedCrime> PlayerCrimesWitnessed { get; private set; } = new List<WitnessedCrime>();
    public bool HasSeenTargetCommitCrime => PlayerCrimesWitnessed.Any();
    public Vector3 PositionLastSeenCrime { get; private set; } = Vector3.Zero;
    public bool NeedsUpdate => Originator.Pedestrian.Exists() && Originator.Pedestrian.IsAlive;
    public bool RanSightThisUpdate { get; private set; }
    public bool SeenTargetWithin(int msSince) => CanSeeTarget || Game.GameTime - GameTimeLastSeenTarget <= msSince;
    public void Update(IPerceptable target, Vector3 placeLastSeen)
    {
        RanSightThisUpdate = false;
        Target = target;
        if (Originator != null &&  Originator.Pedestrian.Exists() && Originator.Pedestrian.IsAlive && Target != null && Target.Character.Exists())
        {
            UpdateTargetDistance(placeLastSeen, target.Position);
            if (!Originator.IsUnconscious)
            {
                if(!IsFakeSeen)
                {
                    UpdateTargetLineOfSight(Target.IsWanted);
                }
                UpdateWitnessedCrimes();
            }
            else
            {
                SetTargetUnseen();
            }
        }
        else
        {
            SetTargetUnseen();// SetTargetSeen();//maybe unseen here? not sure....
        }
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
        WeaponLastSeenTargetWith = Target.WeaponEquipment.CurrentSeenWeapon;
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
        DistanceToTarget = Originator.Pedestrian.DistanceTo(PositionToCheck);//TR 2D CHANGE
        HeightToTarget = Math.Abs(Originator.Pedestrian.Position.Z - PositionToCheck.Z);//is new
        if (Originator.IsCop && Originator.Pedestrian.Exists())
        {
            DistanceToTargetLastSeen = Originator.Pedestrian.DistanceTo(placeLastSeen);//TR 2D CHANGE
        }
        if (DistanceToTarget <= 0.1f)
        {
            if(Originator.Pedestrian.CurrentVehicle?.Handle == Target.Character.CurrentVehicle?.Handle)
            {
                DistanceToTarget = DistanceToTarget;
            }
            else
            {
                DistanceToTarget = 100f;// 999f;
            }
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

        Originator.DistanceChecker.UpdateMovement(DistanceToTarget);


        GameTimeLastDistanceCheck = Game.GameTime;
        return true;
    }
    private bool UpdateTargetLineOfSight(bool IsWanted)
    {
        float expectedSightDistance = 60f;
        if(Originator.IsInAirVehicle)
        {
            expectedSightDistance = Settings.SettingsManager.PoliceSettings.SightDistance_Aircraft;
            if (IsWanted)
            {
                expectedSightDistance += Settings.SettingsManager.PoliceSettings.SightDistance_Aircraft_AdditionalAtWanted;
            }
        }
        else if (Originator.IsCop)
        {
            expectedSightDistance = Settings.SettingsManager.PoliceSettings.SightDistance;
        }
        else
        {
            expectedSightDistance = Settings.SettingsManager.CivilianSettings.SightDistance;
        }
        expectedSightDistance += 50f;//safety margin

        if (DistanceToTarget >= expectedSightDistance || Originator.IsUnconscious || !Target.Character.IsVisible || Originator.IsDead)//this is new, was 100f
        {
            SetTargetUnseen();
            return false;
        }

        if (NeedsLOSCheck && Target.Character.Exists() && Originator.Pedestrian.Exists())
        {     
            bool TargetInVehicle = Target.Character.IsInAnyVehicle(false);
            Entity ToCheck = TargetInVehicle ? (Entity)Target.Character.CurrentVehicle : (Entity)Target.Character;

            bool isInFront = IsInFrontOf(Target.Character); 
            if (Originator.IsCop && !Originator.IsInHelicopter && !Originator.IsInPlane)//!Originator.Pedestrian.IsInHelicopter)
            {
                if (DistanceToTarget <= Settings.SettingsManager.PoliceSettings.SightDistance && isInFront)//55f
                {
                    if(NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Originator.Pedestrian, ToCheck))
                    {
                        SetTargetSeen();
                    }
                    else
                    {
                        SetTargetUnseen();
                    }
                    GameFiber.Yield();//TR TEST 28
                    RanSightThisUpdate = true;
                }
                else
                {
                    if(!isInFront && ((TargetInVehicle && DistanceToTarget <= DistanceToTargetInVehicle) || (!TargetInVehicle && DistanceToTarget <= DistanceToTargetOnFoot)))
                    {
                        if (NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY", Originator.Pedestrian, ToCheck, 17))
                        {
                            SetTargetSeen();
                        }
                        else
                        {
                            SetTargetUnseen();
                        }
                        GameFiber.Yield();//TR TEST 28
                        RanSightThisUpdate = true;
                    }
                    else
                    {
                        SetTargetUnseen();
                    }
                    //GameFiber.Yield();//TR TEST 28
                    //RanSightThisUpdate = true;
                }
            }
            else if (Originator.IsInHelicopter || Originator.IsInPlane)
            {
                float DistanceToSee = Settings.SettingsManager.PoliceSettings.SightDistance_Aircraft;
                if (IsWanted)
                {
                    DistanceToSee += Settings.SettingsManager.PoliceSettings.SightDistance_Aircraft_AdditionalAtWanted;
                }
                if (DistanceToTarget <= DistanceToSee)
                {
                    if (NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY", Originator.Pedestrian, ToCheck, 17))
                    {
                        SetTargetSeen();
                    }
                    else
                    {
                        SetTargetUnseen();
                    }
                    GameFiber.Yield();//TR TEST 28
                    RanSightThisUpdate = true;
                }
                else
                {
                    SetTargetUnseen();
                }
            }
            else
            {
                if (DistanceToTarget <= Settings.SettingsManager.CivilianSettings.SightDistance && isInFront)//55f
                {
                    if (NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Originator.Pedestrian, ToCheck))
                    {
                        SetTargetSeen();
                    }
                    else
                    {
                        SetTargetUnseen();
                    }
                    GameFiber.Yield();//TR TEST 28
                    RanSightThisUpdate = true;
                }
                else
                {
                    if (!isInFront && ((TargetInVehicle && DistanceToTarget <= DistanceToTargetInVehicle) || (!TargetInVehicle && DistanceToTarget <= DistanceToTargetOnFoot)))
                    {
                        if (NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY", Originator.Pedestrian, ToCheck, 17))
                        {
                            SetTargetSeen();
                        }
                        else
                        {
                            SetTargetUnseen();
                        }
                        GameFiber.Yield();//TR TEST 28
                        RanSightThisUpdate = true;
                    }
                    else
                    {
                        SetTargetUnseen();
                    }
                    //GameFiber.Yield();//TR TEST 28
                    //RanSightThisUpdate = true;
                }
            }
            GameTimeLastLOSCheck = Game.GameTime;
            return true;
        }
        return false;
    }
    public void AddWitnessedCrime(Crime CrimeToAdd, Vector3 PositionToReport)
    {
        PositionLastSeenCrime = PositionToReport;
        GameTimeLastSeenTargetCommitCrime = Game.GameTime;
        WitnessedCrime ExistingEvent = PlayerCrimesWitnessed.FirstOrDefault(x => x.Crime?.ID == CrimeToAdd.ID);
        if (ExistingEvent == null)
        {
            PlayerCrimesWitnessed.Add(new WitnessedCrime(CrimeToAdd, null, VehicleLastSeenTargetIn, WeaponLastSeenTargetWith, PositionToReport));
        }
        else
        {
            ExistingEvent.UpdateWitnessed(VehicleLastSeenTargetIn, WeaponLastSeenTargetWith, PositionToReport);
        }

        //EntryPoint.WriteToConsole($"AddWitnessedCrime {Originator.Handle} {CrimeToAdd.Name}");

    }
    public void UpdateWitnessedCrimes()
    {
        if(!Originator.Pedestrian.Exists() || Originator.IsUnconscious)
        {
            return;
        }
        //if(Originator.IsMerchant)
        //{
            //EntryPoint.WriteToConsole($"UpdateWitnessedCrimes RAN FOR MERCHANT CRIMES VIOLATING {Originator.Handle} {string.Join(",",Target.Violations.CivilianReportableCrimesViolating)}");
        //}

        foreach (Crime committing in Target.Violations.CivilianReportableCrimesViolating)
        {
            if(DistanceToTarget > committing.MaxReportingDistance)
            {
                continue;
            }
            if (CanRecognizeTarget)
            {
                AddWitnessedCrime(committing, Originator.Pedestrian.Position);
            }
            else if (WithinWeaponsAudioRange && committing.CanReportBySound && DistanceToTarget <= committing.MaxHearingDistance)
            {
                AddWitnessedCrime(committing, Originator.Pedestrian.Position);
            }         
        }     
    }
    public void Reset()
    {
        SetTargetUnseen();
    }
    public void SetFakeSeen()
    {
        GameTimeLastSetFakeSeen = Game.GameTime;
        SetTargetSeen();
    }
}

