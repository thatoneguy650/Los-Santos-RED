using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Mod;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class UnconsciousBodyAlert : PedAlert
{
    public List<BodyWatcher> UnconsciousPedsSeen { get; private set; } = new List<BodyWatcher>();
    public bool HasSeenUnconsciousPed { get; set; } = false;
    public bool CanCallInUnconsciousPeds { get; set; } = false;

    public UnconsciousBodyAlert(PedExt pedExt, ISettingsProvideable settings) : base(pedExt, settings, ePedAlertType.UnconsciousBody)
    {
        Priority = 2;
        IsPositionAlert = false;
        IsPedAlert = true;
        TimeOutTime = Settings.SettingsManager.WorldSettings.UnconsciousBodyAlertTimeout;
    }
    public override void Update(IPoliceRespondable policeRespondable, IEntityProvideable world)
    {
        if (!PedExt.PedAlertTypes.HasFlag(ePedAlertType.UnconsciousBody))
        {
            return;
        }
        if (!PedExt.Pedestrian.Exists() || PedExt.IsUnconscious || PedExt.PlayerPerception.DistanceToTarget > 150f)//only care in a bubble around the player, nothing to do with the player tho
        {
            return;
        }
        if (HasReachedPosition)
        {
            UnconsciousPedsSeen.ForEach(x => x.DisableAlerts());
        }
        if (HasSeenUnconsciousPed)
        {
            return;
        }
        foreach (PedExt unconsciousPed in world.Pedestrians.PedExts.Where(x => !x.IsDead && (x.IsUnconscious || x.IsInWrithe) && !x.IsLoadedInTrunk && !x.HasStartedEMTTreatment && !x.HasBeenTreatedByEMTs && !UnconsciousPedsSeen.Any(y => y.PedBody?.Handle == x.Handle) && NativeHelper.IsNearby(PedExt.CellX, PedExt.CellY, x.CellX, x.CellY, 4) && x.Pedestrian.Exists()))
        {
            float distanceToBody = PedExt.Pedestrian.DistanceTo2D(unconsciousPed.Pedestrian);
            bool CanSeeBody = false;
            if (distanceToBody <= 15f)
            {
                CanSeeBody = true;
            }
            else if (distanceToBody <= 45f && world.TotalWantedLevel >= 3)
            {
                CanSeeBody = true;
            }
            else if (distanceToBody <= 45f && unconsciousPed.Pedestrian.IsThisPedInFrontOf(PedExt.Pedestrian) && (unconsciousPed.HasBeenSeenUnconscious || NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", PedExt.Pedestrian, unconsciousPed.Pedestrian)))
            {
                CanSeeBody = true;
            }
            if(CanSeeBody)
            {
                EntryPoint.WriteToConsole($"I AM PED {PedExt.Handle} AND I JUST SAW AN Unconscious BODY {unconsciousPed.Handle} GenerateUnconsciousAlerts{PedExt.GenerateUnconsciousAlerts} GeneratesBodyAlerts:{unconsciousPed.GeneratesBodyAlerts}");
                HasSeenUnconsciousPed = true;        
                PedExt.SetSeenUnconscious(unconsciousPed);
                unconsciousPed.HasBeenSeenUnconscious = true;
                UnconsciousPedsSeen.Add(new BodyWatcher(unconsciousPed));
                if (PedExt.GenerateUnconsciousAlerts && unconsciousPed.GeneratesBodyAlerts)
                {
                    AddAlert(unconsciousPed); 
                }
                if (PedExt.AutoCallsInUnconsciousPeds)
                {
                    policeRespondable.AddMedicalEvent(unconsciousPed.Position);
                }
                break;
            }
        }
        base.Update(policeRespondable, world);

    }
    public override void OnReportedCrime(IPoliceRespondable policeRespondable)
    {
        if (HasSeenUnconsciousPed && AlertTarget != null && AlertTarget.Pedestrian.Exists())
        {
            policeRespondable.AddMedicalEvent(AlertTarget.Position);
            EntryPoint.WriteToConsole($"Ped Alerts OnReportedCrime Added Medical Flag");
        }
        HasSeenUnconsciousPed = false;
        base.OnReportedCrime(policeRespondable);
    }
}

