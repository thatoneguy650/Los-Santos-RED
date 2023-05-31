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
    public bool HasSeenUnconsciousPed { get; set; } = false;
    public bool CanCallInUnconsciousPeds { get; set; } = false;
    public UnconsciousBodyAlert(PedExt pedExt, ISettingsProvideable settings) : base(pedExt, settings, ePedAlertType.UnconsciousBody)
    {
        Priority = 2;
        IsPositionAlert = false;
        IsPedAlert = true;
        TimeOutTime = Settings.SettingsManager.WorldSettings.DeadBodyAlertTimeout;
    }
    public override void Update(IPoliceRespondable policeRespondable, IEntityProvideable world)
    {
        if (!PedExt.PedAlertTypes.HasFlag(ePedAlertType.UnconsciousBody))
        {
            return;
        }
        if (!PedExt.Pedestrian.Exists() || PedExt.IsUnconscious || HasSeenUnconsciousPed || PedExt.PlayerPerception.DistanceToTarget > 150f)//only care in a bubble around the player, nothing to do with the player tho
        {
            return;
        }
        foreach (PedExt distressedPed in world.Pedestrians.PedExts.Where(x => (x.IsUnconscious || x.IsInWrithe) && !x.HasStartedEMTTreatment && !x.HasBeenTreatedByEMTs && NativeHelper.IsNearby(PedExt.CellX, PedExt.CellY, x.CellX, x.CellY, 4) && x.Pedestrian.Exists()))
        {
            float distanceToBody = PedExt.Pedestrian.DistanceTo2D(distressedPed.Pedestrian);
            if (distanceToBody <= 15f || (distanceToBody <= 45f && distressedPed.Pedestrian.IsThisPedInFrontOf(PedExt.Pedestrian) && (distressedPed.HasBeenSeenUnconscious || NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", PedExt.Pedestrian, distressedPed.Pedestrian))))//if someone saw it assume ANYONE close saw it, only performance reason
            {
                EntryPoint.WriteToConsole($"I AM PED {PedExt.Handle} AND I JUST SAW AN Unconscious BODY {distressedPed.Handle}");
                HasSeenUnconsciousPed = true;
                AddAlert(distressedPed);
                distressedPed.HasBeenSeenUnconscious = true;
                if(PedExt.AutoCallsInUnconsciousPeds)
                {
                    policeRespondable.AddMedicalEvent(distressedPed.Position);
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

