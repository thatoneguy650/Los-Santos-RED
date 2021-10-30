using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class Perception
{
    private uint GameTimeLastSeen;
    public Vector3 PositionLastSeen { get; private set; }
    private uint GameTimeContinuoslySeenSince;
    private uint GameTimeBehind;
   // private bool WithinWeaponsAudioRange;
    private uint GameTimeLastDistanceCheck;
    public uint TimeContinuoslySeen
    {
        get
        {
            if (GameTimeContinuoslySeenSince == 0)
            {
                return 0;
            }
            else
            {
                return (Game.GameTime - GameTimeContinuoslySeenSince);
            }
        }
    }
    public Perception(Ped observedPed, Ped targetPed)
    {
        ObserverPed = observedPed;
        TargetPed = targetPed;
    }
    public bool CanRecognize
    {
        get
        {
            if (TimeContinuoslySeen >= 500)//1250
            {
                return true;
            }
            else if (CanSee && DistanceTo <= 8f && DistanceTo > 0.1f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public bool CanSee { get; private set; } = false;
    public float ClosestDistanceTo { get; private set; } = 2000f;
    public float DistanceTo { get; private set; } = 999f;
    public Ped ObserverPed { get; private set; }
    public Ped TargetPed { get; private set; }
    public bool RecentlySeen
    {
        get
        {
            if (CanSee)
                return true;
            else if (Game.GameTime - GameTimeLastSeen <= 10000)//Seen in last 10 seconds?
                return true;
            else
                return false;
        }
    }
    public void Update()
    {
        UpdateDistance();
        UpdateLineOfSight();
    }
    private void SetSeen()
    {
        CanSee = true;
        GameTimeLastSeen = Game.GameTime;
        PositionLastSeen = Game.LocalPlayer.Character.Position;
        if (GameTimeContinuoslySeenSince == 0)
        {
            GameTimeContinuoslySeenSince = Game.GameTime;
        }
    }
    private void SetUnseen()
    {
        GameTimeContinuoslySeenSince = 0;
        CanSee = false;
    }
    private void UpdateDistance()
    {
        if (TargetPed.Exists())
        {
            Vector3 PosToCheck = NativeFunction.Natives.GET_WORLD_POSITION_OF_ENTITY_BONE<Vector3>(TargetPed, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", TargetPed, 57005));
            DistanceTo = ObserverPed.DistanceTo2D(PosToCheck);
            if (DistanceTo <= 0.1f)
            {
                DistanceTo = 999f;
            }
            if (DistanceTo <= ClosestDistanceTo)
            {
                ClosestDistanceTo = DistanceTo;
            }
            //if (DistanceTo <= (Settings.SettingsManager.CivilianSettings.GunshotHearingDistance))//45f
            //{
            //    WithinWeaponsAudioRange = true;
            //}
            //else
            //{
            //    WithinWeaponsAudioRange = false;
            //}
            if (!IsBehind(TargetPed))
            {
                if (GameTimeBehind == 0)
                {
                    GameTimeBehind = Game.GameTime;
                }
            }
            else
            {
                GameTimeBehind = 0;
            }
        }
        GameTimeLastDistanceCheck = Game.GameTime;
    }
    private void UpdateLineOfSight()
    {
            bool InVehicle = TargetPed.IsInAnyVehicle(false);
            Entity ToCheck = InVehicle ? (Entity)TargetPed.CurrentVehicle : (Entity)TargetPed;
            if (DistanceTo <= 90f && IsInFrontOf(TargetPed) && !ObserverPed.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", ObserverPed, ToCheck))//55f
            {
                SetSeen();
            }
            else
            {
                SetUnseen();
            }
    }
    private bool IsBehind(Entity entity)
    {
        if (GetDotVectorResult(entity, ObserverPed) > 0)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    private bool IsInFrontOf(Ped ped)
    {
        float Result = GetDotVectorResult(ObserverPed, ped);
        if (Result > 0)
        {
            return true;
        }
        else
        {
            return false;
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


}