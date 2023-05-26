using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PedAlerts
{
    private PedExt PedExt;
    private ISettingsProvideable Settings;

    public List<PedExt> BodiesSeen { get; private set; } = new List<PedExt>();
    public bool IsAlerted => (BodiesSeen.Any() && Game.GameTime - GameTimeLastSeenDeadBody <= 60000) || Game.GameTime - GameTimeLastHeardGunshots <= 60000;
    public Vector3 PositionLastSeenUnconsciousPed { get; set; }
    public uint GameTimeLastSeenDeadBody { get; private set; }
    public uint GameTimeLastHeardGunshots { get; private set; }
    public bool HasSeenUnconsciousPed { get; set; } = false;
    public Vector3 AlertedPoint { get; set; }
    public PedAlerts(PedExt pedExt, ISettingsProvideable settings)
    {
        PedExt = pedExt;
        Settings = settings;
    }
    public void LookForUnconsciousPeds(IEntityProvideable world)
    {
        if (!PedExt.Pedestrian.Exists() || PedExt.IsUnconscious || HasSeenUnconsciousPed || PedExt.PlayerPerception.DistanceToTarget > 150f)//only care in a bubble around the player, nothing to do with the player tho
        {
            return;
        }
        foreach (PedExt distressedPed in world.Pedestrians.PedExts.Where(x => (x.IsUnconscious || x.IsInWrithe) && !x.HasStartedEMTTreatment && !x.HasBeenTreatedByEMTs && NativeHelper.IsNearby(PedExt.CellX, PedExt.CellY, x.CellX, x.CellY, 4) && x.Pedestrian.Exists()))
        {
            float distanceToBody = PedExt.Pedestrian.DistanceTo2D(distressedPed.Pedestrian);
            if (distanceToBody <= 15f || (distanceToBody <= 45f && distressedPed.Pedestrian.IsThisPedInFrontOf(PedExt.Pedestrian) && (distressedPed.HasBeenSeenUnconscious || NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", PedExt.Pedestrian, distressedPed.Pedestrian))))//if someone saw it assume ANYONE close saw it, only performance reason
            {
                AddSeenUnconsciousPed(distressedPed);
                break;
            }
        }
    }
    public void LookForBodiesAlert(IEntityProvideable world)
    {
        if (!PedExt.Pedestrian.Exists() || PedExt.IsUnconscious || PedExt.PlayerPerception.DistanceToTarget > 150f || world.TotalWantedLevel >= 3)
        {
            return;
        }
        foreach (PedExt deadBody in world.Pedestrians.DeadPeds.Where(x => !BodiesSeen.Any(y => y.Handle == x.Handle) && x.Pedestrian.Exists() && PedExt.Pedestrian.Exists()))
        {
            float distanceToBody = PedExt.Pedestrian.DistanceTo2D(deadBody.Pedestrian);
            bool CanSeeBody = false;
            if (!deadBody.WasKilledByPlayer && distanceToBody <= 15f)
            {
                CanSeeBody = true;
            }
            else if (distanceToBody <= 45f && deadBody.Pedestrian.IsThisPedInFrontOf(PedExt.Pedestrian) && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", PedExt.Pedestrian, deadBody.Pedestrian))
            {
                CanSeeBody = true;
                GameFiber.Yield();
            }
            if (CanSeeBody)
            {
                AddSeenBody(deadBody);
            }
        }
        BodiesSeen.RemoveAll(x => !x.Pedestrian.Exists());
    }
    public void AddSeenUnconsciousPed(PedExt unconsciousPed)
    {
        if (unconsciousPed == null || !unconsciousPed.Pedestrian.Exists())
        {
            return;
        }
        PositionLastSeenUnconsciousPed = unconsciousPed.Position;
        HasSeenUnconsciousPed = true;
        unconsciousPed.HasBeenSeenUnconscious = true;
        EntryPoint.WriteToConsole($"I AM PED {PedExt.Handle} AND I JUST SAW AN Unconscious BODY {unconsciousPed.Handle}");
    }
    public void AddSeenBody(PedExt deadBody)
    {
        if (deadBody == null || !deadBody.Pedestrian.Exists())
        {
            return;
        }
        BodiesSeen.Add(deadBody);
        GameTimeLastSeenDeadBody = Game.GameTime;
        AlertedPoint = deadBody.Pedestrian.Position;
        EntryPoint.WriteToConsole($"I AM PED {PedExt.Handle} AND I JUST SAW A DEAD BODY {deadBody.Handle}");
    }
    public void AddHeardGunfire(Vector3 locationHeard)
    {
        if (locationHeard == Vector3.Zero)
        {
            return;
        }
        GameTimeLastHeardGunshots = Game.GameTime;
        AlertedPoint = locationHeard;
        EntryPoint.WriteToConsole($"I AM PED {PedExt.Handle} AND I JUST HEARD GUNFIRE");
    }
}

