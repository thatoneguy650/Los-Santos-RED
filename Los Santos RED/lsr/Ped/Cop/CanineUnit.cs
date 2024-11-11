using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CanineUnit : Cop
{
    private bool IsRunningMoveOverride = false;
    private bool hasSetSitAnimation = false;
    private bool cancelMoveRateOverride = false;
    public override string UnitType { get; set; } = "K9";
    public override bool ShouldBustPlayer => false;
    public override bool IsAnimal { get; set; } = true;
    public override int DefaultCombatFlag { get; set; } = 134217728;//disable aim intro
    public override int DefaultEnterExitFlag { get; set; } = (int)eEnter_Exit_Vehicle_Flags.ECF_WARP_PED;
    public override bool IsTrustingOfPlayer { get; set; } = false;
    public override bool CanConverse => false;
    public override bool CanTransact => false;
    public override bool CanBeLooted { get; set; } = false;
    public override bool CanBeDragged { get; set; } = false;
    public CanineUnit(Ped pedestrian, ISettingsProvideable settings, int health, Agency agency, bool wasModSpawned, ICrimes crimes, IWeapons weapons, string name, string modelName, IEntityProvideable world) : base(pedestrian, settings, health, agency, wasModSpawned, crimes, weapons, name, modelName, world)
    {

    }
    public override void Update(IPerceptable perceptable, IPoliceRespondable policeRespondable, Vector3 placeLastSeen, IEntityProvideable world)
    {
        base.Update(perceptable, policeRespondable, placeLastSeen, world);

        if(!Pedestrian.Exists() || !Pedestrian.IsAlive)
        {
            return;
        }
        if(IsInVehicle)
        {
            if (!hasSetSitAnimation)
            {
                SetVehicleSitAnimation();
                hasSetSitAnimation = true;
            }
        }
        else
        {
            if (hasSetSitAnimation)
            {
                hasSetSitAnimation = false;
            }
        }
        UpdateMoveRateOverride();
    }
    private void UpdateMoveRateOverride()
    {
        if (PlayerPerception.DistanceToTarget <= 75f && IsRespondingToWanted)
        {
            if(IsRunningMoveOverride)
            {
                return;
            }
            StartMoveRateOverride();
        }
        else
        {
            if(!IsRunningMoveOverride)
            {
                return;
            }
            StopMoveRateOverride();
        }
    }
    private void SetVehicleSitAnimation()
    {
        string PlayingDict = "creatures@rottweiler@in_vehicle@low_car";
        string PlayingAnim = "sit";
        AnimationDictionary.RequestAnimationDictionay(PlayingDict);
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Pedestrian, PlayingDict, PlayingAnim, 8.0f, -8.0f, -1, 1, 0, false, false, false);
    }
    public void StartMoveRateOverride()
    {
        if(!IsRespondingToWanted)
        {
            return;
        }
        if(IsRunningMoveOverride)
        {
            return;
        }
        cancelMoveRateOverride = false;
        IsRunningMoveOverride = true;
        GameFiber.StartNew(delegate
        {
            try
            {
                EntryPoint.WriteToConsole("Move Rate Override STARTED ON ANIMAL PED");
                while (Pedestrian.Exists() && EntryPoint.ModController.IsRunning && !IsDead && !IsUnconscious)
                {
                    if (Pedestrian.Exists())
                    {
                        NativeFunction.Natives.SET_PED_MOVE_RATE_OVERRIDE<uint>(Pedestrian, Settings.SettingsManager.PoliceTaskSettings.K9RunSpeed);
                    }
                    if(cancelMoveRateOverride)
                    {
                        break;
                    }
                    GameFiber.Yield();
                }
                IsRunningMoveOverride = false;
                EntryPoint.WriteToConsole("Move Rate Override STOPPED ON ANIMAL PED");
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
            }
        }, "Run Move Rate Logic");

    }
    private void StopMoveRateOverride()
    {
        cancelMoveRateOverride = true;
        EntryPoint.WriteToConsole("Move Rate Override SENT STOP COMMAND");
    }
}

