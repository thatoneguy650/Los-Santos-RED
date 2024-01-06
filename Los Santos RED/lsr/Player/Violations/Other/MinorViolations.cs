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
using Mod;


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
    private List<VehicleExt> CloseVehicles;

    public bool RecentlyDamagedVehicleOnFoot => GameTimeLastDamagedVehicleOnFoot != 0 && Game.GameTime - GameTimeLastDamagedVehicleOnFoot <= 3000;
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
    public void UpdateData()
    {
        CloseVehicles = World.Vehicles.AllVehicleList.Where(x => x.Vehicle.Exists() && x.Vehicle.DistanceTo(Player.Character) <= 5.0f).ToList();
        UpdateVehicleStanding();
        UpdateVehicleDamage();
        UpdateCollideItems();
        UpdateStandingClose();
        UpdateBodilyFunctionsAroundOthers();
    }

    private void UpdateVehicleStanding()
    {
        if (Player.IsInVehicle)
        {
            Player.IsStandingOnNonTrainVehicle = false;
            Player.IsRidingOnTrain = false;
            return;
        }
        if (!Settings.SettingsManager.ViolationSettings.AllowVehicleStandingReactions)
        {
            Player.IsStandingOnNonTrainVehicle = false;
            Player.IsRidingOnTrain = false;
            return;
        }
        bool isStandingOnVehicle = NativeFunction.Natives.IS_PED_ON_VEHICLE<bool>(Player.Character);
        if(!isStandingOnVehicle)
        {
            Player.IsStandingOnNonTrainVehicle = false;
            Player.IsRidingOnTrain = false;
            return;
        }
        if(CloseVehicles == null)
        {
            Player.IsStandingOnNonTrainVehicle = false;
            Player.IsRidingOnTrain = false;
            return;
        }
        bool isStandingOnTop = false;
        bool isOnTrain = false;
        foreach (VehicleExt vehicle in CloseVehicles)
        {
            if(NativeFunction.Natives.IS_PED_ON_SPECIFIC_VEHICLE<bool>(Player.Character,vehicle.Vehicle))
            {
                if(vehicle.Vehicle.IsTrain)
                {
                    isOnTrain = true;
                    isStandingOnTop = false;
                }
                else if (!vehicle.IsOwnedByPlayer && !vehicle.IsBoat && !vehicle.IsMotorcycle && !vehicle.IsBicycle)
                {
                    Player.IsRidingOnTrain = false;
                    EntryPoint.WriteToConsole($"YOU ARE STANDING ON TOP OF {vehicle.Handle}");
                    isStandingOnTop = true;
                    if(vehicle.Vehicle.Exists() && vehicle.Vehicle.Driver.Exists())
                    {
                        PedExt driver = World.Pedestrians.GetPedExt(vehicle.Vehicle.Driver.Handle);
                        driver?.OnPlayerStoodOnCar(Interactionable);
                    }
                }
                break;
            }
        }
        Player.IsRidingOnTrain = isOnTrain;
        Player.IsStandingOnNonTrainVehicle = isStandingOnTop;
    }

    private void UpdateBodilyFunctionsAroundOthers()
    {
        if (!Settings.SettingsManager.ViolationSettings.AllowBodilyFunctionReactions)
        {
            return;
        }
        if (!Player.ActivityManager.IsUrinatingDefecting || Player.ActivityManager.IsUrinatingDefectingOnToilet)
        {
            return;
        }
        List<PedExt> closePeds = World.Pedestrians.LivingPeople.Where(x => x.Pedestrian.Exists() && !x.IsUnconscious && !x.IsDead && x.DistanceToPlayer <= 15.0f).ToList();
        foreach (PedExt pedExt in closePeds)
        {
            if (!pedExt.Pedestrian.Exists())
            {
                continue;
            }
            if (!pedExt.CanSeePlayer || pedExt.DistanceToPlayer > 10.0f)
            {
                continue;
            }
            pedExt.OnPlayerDidBodilyFunctionsNear(Interactionable);    
        }
    }

    public void Update()
    {
        if (RecentlyDamagedVehicleOnFoot)
        {
            Violations.AddViolating(StaticStrings.MaliciousVehicleDamageCrimeID);
        }
        if (Player.IsStandingOnNonTrainVehicle)
        {
            Violations.AddViolating(StaticStrings.StandingOnVehicleCrimeID);
        }
    }
    private void UpdateVehicleDamage()
    {
        if (!Settings.SettingsManager.ViolationSettings.AllowVehicleDamageReactions)
        {
            return;
        }
        if (Player.IsInVehicle || RecentlyDamagedVehicleOnFoot || Player.IsWanted || Player.IsDead || CloseVehicles == null || Player.RecentlyGotOutOfVehicle || Player.Character.IsInAnyVehicle(false))//already checks crashes
        {
            return;
        }
        foreach (VehicleExt vehicle in CloseVehicles)
        {
            if (vehicle.CheckPlayerDamage(Interactionable, World))
            {
                GameTimeLastDamagedVehicleOnFoot = Game.GameTime;
                return;
            }
        }
    }
    private void UpdateCollideItems()
    {
        if(!Settings.SettingsManager.ViolationSettings.AllowCollisionReactions)
        {
            return;
        }
        if (Player.IsDead)//already checks crashes
        {
            return;
        }
        List<PedExt> closePeds = World.Pedestrians.LivingPeople.Where(x => x.Pedestrian.Exists() && !x.IsUnconscious && !x.IsDead && x.DistanceToPlayer <= 5.0f).ToList();
        foreach (PedExt pedExt in closePeds)
        {
            if (!pedExt.Pedestrian.Exists() || pedExt.Pedestrian.IsInAnyVehicle(false))
            {
                continue;
            }
            if (NativeFunction.Natives.IS_ENTITY_TOUCHING_ENTITY<bool>(Player.Character, pedExt.Pedestrian))
            {
                float PlayerVelocity = Interactionable.Character.Velocity.Length();
                float PedVelocity = pedExt.Pedestrian.Velocity.Length();
                bool isPedRagdoll = pedExt.Pedestrian.IsRagdoll;
                if (PlayerVelocity > 4.0f || PedVelocity > 4.0f || isPedRagdoll)
                {
                    pedExt.OnCollidedWithPlayer(Interactionable);
                    EntryPoint.WriteToConsole($"YOU ARE COLLIDING WITH {pedExt.Handle}");
//#if DEBUG
//                    Game.DisplaySubtitle($"YOU ARE TOUCHING {pedExt.Handle} Velocity1: {Math.Round(PlayerVelocity, 2)} Velocity2: {Math.Round(PedVelocity, 2)} isPedRagdoll{isPedRagdoll}");
//#endif
                }
            }
        }
    }
    private void UpdateStandingClose()
    {
        if (!Settings.SettingsManager.ViolationSettings.AllowStandingCloseReactions)
        {
            return;
        }
        if (Player.IsInVehicle)
        {
            return;
        }
        PedExt closestPedExt = World.Pedestrians.PedExts.Where(x => !x.IsDead && !x.IsUnconscious && x.DistanceToPlayer <= 0.65f).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
        if (closestPedExt == null)
        {
            PreviousClosestPed?.ResetPlayerStoodTooClose();
            return;
        }
        if (PreviousClosestPed != null && closestPedExt.Handle != PreviousClosestPed.Handle)
        {
            PreviousClosestPed.ResetPlayerStoodTooClose();
        }
        if (closestPedExt.IsGroupMember)
        {
            return;
        }
        if(1==1)
        {
            return;
        }
        if (closestPedExt.DistanceToPlayer <= 0.65f)
        {
            closestPedExt.OnPlayerIsClose(Interactionable);
            //EntryPoint.WriteToConsole("TOO CLOSE TO PED, MAKING THEM ANGRY");
        }
    }

    public void OnGotOutOfVehicle()
    {
        if(Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
        {
            NativeFunction.Natives.CLEAR_ENTITY_LAST_DAMAGE_ENTITY(Player.CurrentVehicle.Vehicle);
            EntryPoint.WriteToConsole("GOUT OUT CLEAR DAMAGE1");
        }
        else if (Player.Character.LastVehicle.Exists())
        {
            NativeFunction.Natives.CLEAR_ENTITY_LAST_DAMAGE_ENTITY(Player.Character.LastVehicle);
            EntryPoint.WriteToConsole("GOUT OUT CLEAR DAMAGE2");
        }
    }
}

