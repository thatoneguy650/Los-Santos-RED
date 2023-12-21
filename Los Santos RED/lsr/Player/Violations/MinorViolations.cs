using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MinorViolations
{
    private IViolateable Player;
    private Violations Violations;
    private ISettingsProvideable Settings;
    private ITimeReportable Time;
    private IEntityProvideable World;
    private IInteractionable Interactionable;
    private PedExt PreviousClosestPed;

    private uint GameTimeLastDamagedVehicleOnFoot;
    public bool RecentlyDamagedVehicleOnFoot => GameTimeLastDamagedVehicleOnFoot != 0 && Game.GameTime - GameTimeLastDamagedVehicleOnFoot <= 5000;
    public MinorViolations(IViolateable player, Violations violations, ISettingsProvideable settings, ITimeReportable time, IEntityProvideable world, IInteractionable interactionable)
        {
            Player = player;
            Violations = violations;
            Settings = settings;
            Time = time;
            World = world;
            Interactionable = interactionable;
        }

    public void Dispose()
    {

    }

    public void Reset()
    {

    }

    public void Setup()
    {

    }

    public void Update()
    {
        UpdateVehicleDamage();
        UpdateCollideItems();
        if (RecentlyDamagedVehicleOnFoot)
        {
            //EntryPoint.WriteToConsole("VIOLATING MaliciousVehicleDamageCrimeID");
            Violations.AddViolating(StaticStrings.MaliciousVehicleDamageCrimeID);
        }

    }
    private void UpdateVehicleDamage()
    {
        if (Player.IsInVehicle || RecentlyDamagedVehicleOnFoot || Player.IsWanted || Player.IsDead)//already checks crashes
        {
            return;
        }
        List<VehicleExt> CloseVehicles = World.Vehicles.AllVehicleList.Where(x => x.Vehicle.Exists() && x.Vehicle.DistanceTo(Player.Character) <= 5.0f).ToList();
        foreach (VehicleExt vehicle in CloseVehicles)
        {
            if (vehicle.CheckPlayerDamage(Interactionable))
            {
                return;
            }
        }
    }
    private void UpdateCollideItems()
    {
        if (Player.IsInVehicle || Player.IsWanted || Player.IsDead)//already checks crashes
        {
            return;
        }
        List<PedExt> closePeds = World.Pedestrians.LivingPeople.Where(x => x.Pedestrian.Exists() && x.DistanceToPlayer <= 5.0f).ToList();
        foreach (PedExt pedExt in closePeds)
        {
            if (!pedExt.Pedestrian.Exists())
            {
                continue;
            }
            if (NativeFunction.Natives.IS_ENTITY_TOUCHING_ENTITY<bool>(Player.Character, pedExt.Pedestrian))
            {
                float PlayerVelocity = Interactionable.Character.Velocity.Length();
                float PedVelocity = pedExt.Pedestrian.Velocity.Length();
                bool isPedRagdoll = pedExt.Pedestrian.IsRagdoll;
                if (PlayerVelocity > 3.0f || PedVelocity > 3.0f || isPedRagdoll)
                {
                    pedExt.OnTouchedByPlayer(Interactionable);
                    EntryPoint.WriteToConsole($"YOU ARE TOUCHING {pedExt.Handle}");
                }
#if DEBUG
                Game.DisplaySubtitle($"YOU ARE TOUCHING {pedExt.Handle} Velocity1: {Math.Round(PlayerVelocity, 2)} Velocity2: {Math.Round(PedVelocity, 2)} isPedRagdoll{isPedRagdoll}");
#endif
            }
        }
    }
    public void OnDamagedVehicle()
    {
        GameTimeLastDamagedVehicleOnFoot = Game.GameTime;
    }
}

