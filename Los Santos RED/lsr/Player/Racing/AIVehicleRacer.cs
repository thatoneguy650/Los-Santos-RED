using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AIVehicleRacer : VehicleRacer
{
    // Timing and State
    private uint GameTimeLastClearedFront;
    private bool hasBeenDisposed;
    private bool taskStarted;
    private bool reactionFinished;
    private int reactionDelay;

    // Burnout Logic
    private bool burnoutStarted;
    private bool burnoutFinished;
    private uint burnoutStartTime;
    private int burnoutDuration;
    private const int BURNOUT_SPREAD_TIME = 150;

    // Personality Traits
    private float SpeedOffset;
    private float PowerOffset;
    private float DriverAbility;
    private float DriverAggressiveness;
    public float GetDriverAbility() => DriverAbility;
    public float GetDriverAggressiveness() => DriverAggressiveness;

    // Stuck Detection
    private uint GameTimeLastMoved;
    private float LastDistanceToTarget;
    private const float StuckSpeedThreshold = 1.0f;
    private const int StuckTimeThreshold = 8000;


    private bool isManuallyDeleted;
    private bool isManualCleanup;

    public PedExt PedExt { get; set; }
    public bool WasSpawnedForRace { get; set; }
    public override string RacerName => PedExt == null ? base.RacerName : PedExt.Name;
    public bool IsManualDispose { get; set; } = false;
    public bool IsDisqualified { get; set; }

    public AIVehicleRacer(PedExt pedExt, VehicleExt vehicleExt) : base(vehicleExt)
    {
        PedExt = pedExt;
        Random rnd = new Random(Guid.NewGuid().GetHashCode());

        // Personality Initialization
        SpeedOffset = (float)(rnd.NextDouble() * 7.0 - 1.0); 
        PowerOffset = (float)(rnd.NextDouble() * 0.4 - 0.05); 
        DriverAbility = (float)(rnd.NextDouble() * 0.20 + 0.80);
        DriverAggressiveness = (float)(rnd.NextDouble() * 0.20 + 0.80);

        // Native Reaction Logic: Higher aggressiveness results in lower reaction delay
        reactionDelay = (int)((1.0f - DriverAggressiveness) * 500);
    }

    public override void Update(VehicleRace vehicleRace)
    {
        if (hasBeenDisposed || vehicleRace == null || PedExt == null || HasFinishedRace || IsDisqualified) return;

        Vehicle raceCar = VehicleExt?.Vehicle;
        Ped driver = PedExt?.Pedestrian;

        if (raceCar == null || !raceCar.Exists() || driver == null || !driver.Exists()) return;

        // PRE-RACE WAIT & REACTION DELAY
        if (!vehicleRace.HasRaceStarted)
        {
            ResetPreRaceState(raceCar);
            return;
        }

        // Apply Native Reaction Delay
        if (!reactionFinished)
        {
            if (Game.GameTime - GameTimeStartedRace < (uint)reactionDelay) return;
            reactionFinished = true;
        }

        // BURNOUT LOGIC
        HandleBurnout(vehicleRace, raceCar, driver);

        // RACE START TRIGGER 
        if (burnoutFinished && !taskStarted)
        {
            NativeFunction.Natives.SET_VEHICLE_HANDBRAKE(raceCar, false);
            PedExt.CurrentTask?.Start();
            taskStarted = true;
        }

        if (taskStarted) PedExt.CurrentTask?.Update();

        base.Update(vehicleRace);

        // Persistence and Failure Checks
        //NativeFunction.Natives.SET_ENTITY_LOD_DIST(raceCar, 1000);
        NativeFunction.Natives.SET_PED_KEEP_TASK(driver, true);

        if (driver.IsDead || !raceCar.IsDriveable)
        {
            HandleRacerFailure(vehicleRace);
            return;
        }

        // PROGRESSION & STUCK RECOVERY
        HandleStuckDetection(raceCar, driver, vehicleRace);

        // BEHAVIOR TUNING
        ApplyRubberBanding(vehicleRace, raceCar, driver);
        ApplyStabilityForce(raceCar);

        if (Game.GameTime - GameTimeLastClearedFront > 200 && vehicleRace.HasRaceStarted)
        {
            ClearFront();
            GameTimeLastClearedFront = Game.GameTime;
        }
    }

    private void ResetPreRaceState(Vehicle raceCar)
    {
        if (PedExt.CurrentTask != null && taskStarted)
        {
            PedExt.CurrentTask.Stop();
            taskStarted = false;
        }
        NativeFunction.Natives.SET_VEHICLE_FORWARD_SPEED(raceCar, 0f);
        NativeFunction.Natives.SET_VEHICLE_HANDBRAKE(raceCar, true);
        GameTimeLastMoved = Game.GameTime;
    }

    private void HandleBurnout(VehicleRace vehicleRace, Vehicle raceCar, Ped driver)
    {
        if (!burnoutFinished && !raceCar.IsBike)
        {
            if (!burnoutStarted)
            {
                int index = vehicleRace.VehicleRacers.IndexOf(this);
                burnoutDuration = (index + 1) * BURNOUT_SPREAD_TIME;
                burnoutStartTime = Game.GameTime;
                NativeFunction.Natives.SET_VEHICLE_HANDBRAKE(raceCar, false);
                // TEMP_ACTION 23 is native burnout
                NativeFunction.Natives.TASK_VEHICLE_TEMP_ACTION(driver, raceCar, 23, burnoutDuration);
                burnoutStarted = true;
            }
            if (Game.GameTime - burnoutStartTime >= burnoutDuration) burnoutFinished = true;
        }
        else burnoutFinished = true;
    }

    private void HandleStuckDetection(Vehicle raceCar, Ped driver, VehicleRace vehicleRace)
    {
        // Only run stuck detection if the race has at least two checkpoints 
        if (vehicleRace.VehicleRaceTrack.RaceCheckpoints.Count <= 1)
        {
            return;
        }

        float currentDist = DistanceToCheckpoint;
        bool isDriverInSeat = driver.IsInVehicle(raceCar, false);

        // 1. Progress Check
        // Progressing = In seat, on wheels, and either moving or closing distance to target
        bool isProgressing = isDriverInSeat && raceCar.IsOnAllWheels &&
                             (raceCar.Speed > StuckSpeedThreshold || Math.Abs(LastDistanceToTarget - currentDist) > 0.5f);

        if (isProgressing)
        {
            GameTimeLastMoved = Game.GameTime;
            LastDistanceToTarget = currentDist;
            return;
        }

        // 2. Recovery Logic
        // If they aren't progressing, check if the 8-second threshold has passed
        if (Game.GameTime - GameTimeLastMoved > StuckTimeThreshold)
        {
            // Check if there is a valid target and if the player is far enough away (60f)
            if (TargetCheckpoint != null && Game.LocalPlayer.Character.DistanceTo(TargetCheckpoint.Position) > 60f)
            {
                TeleportToTargetCheckpoint(raceCar);

                // Reset timers after teleport to prevent looping
                GameTimeLastMoved = Game.GameTime;
                LastDistanceToTarget = DistanceToCheckpoint;
            }
        }
    }

    public void AssignTask(VehicleRace vehicleRace, ITargetable Targetable, IEntityProvideable World, ISettingsProvideable Settings)
    {
        if (PedExt == null || !PedExt.Pedestrian.Exists()) return;
        Ped racePed = PedExt.Pedestrian;
        Vehicle raceCar = VehicleExt?.Vehicle;

        racePed.BlockPermanentEvents = true;

        if (raceCar != null && raceCar.Exists())
        {
            NativeFunction.Natives.SET_VEHICLE_HANDLING_OVERRIDE(raceCar, Game.GetHashKey("SPORTS_CAR"));
            NativeFunction.Natives.SET_VEHICLE_STRONG(raceCar, true);
            NativeFunction.Natives.SET_VEHICLE_HAS_STRONG_AXLES(raceCar, true);
            NativeFunction.Natives.SET_VEHICLE_TYRES_CAN_BURST(raceCar, false);
            NativeFunction.Natives.SET_ENTITY_ONLY_DAMAGED_BY_PLAYER(raceCar, true);

            if (NativeFunction.Natives.GET_NUM_VEHICLE_MODS<int>(raceCar, 15) > 0)
                NativeFunction.Natives.SET_VEHICLE_MOD(raceCar, 15, -1, true);
        }


            NativeFunction.Natives.SET_ENTITY_ONLY_DAMAGED_BY_PLAYER(racePed, true);
            NativeFunction.Natives.SET_PED_DIES_IN_WATER(racePed, false);
            NativeFunction.Natives.SET_PED_CAN_BE_KNOCKED_OFF_VEHICLE(racePed, 3);

            // Ped Config Flags
            NativeFunction.Natives.SET_PED_CONFIG_FLAG(racePed, 29, false); // PCF_GetOutUndriveableVehicle
            NativeFunction.Natives.SET_PED_CONFIG_FLAG(racePed, 32, false);  // PCF_WillFlyThroughWindscreen
            NativeFunction.Natives.SET_PED_CONFIG_FLAG(racePed, 118, false); // PCF_RunFromFiresAndExplosions
            NativeFunction.Natives.SET_PED_CONFIG_FLAG(racePed, 398, true);  // PCF_DontAllowToBeDraggedOutOfVehicle (PlayersDontDragMeOutOfCar)

        isManuallyDeleted = PedExt.IsManuallyDeleted;
        PedExt.IsManuallyDeleted = true;

        if (VehicleExt != null)
        {
            isManualCleanup = VehicleExt.IsManualCleanup;
            VehicleExt.IsManualCleanup = true;
        }

        PedExt.CurrentTask = new GeneralRace(PedExt, PedExt, Targetable, World, new List<VehicleExt>() { VehicleExt }, null, Settings, vehicleRace, this);
        PedExt.CurrentTask.Start();
    }

    private void ApplyRubberBanding(VehicleRace vehicleRace, Vehicle raceCar, Ped driver)
    {
        VehicleRacer playerRacer = vehicleRace.VehicleRacers.FirstOrDefault(r => r.IsPlayer);
        if (playerRacer?.VehicleExt?.Vehicle == null) return;

        float rb_distance = raceCar.DistanceTo(playerRacer.VehicleExt.Vehicle);
        bool isBehind = CheckIfBehind(playerRacer);

        float rb_base_speed = 62.0f + SpeedOffset; // Increased base speed from 56.0f (~125 mph) to 62.0f (~138 mph)

        // Native Aggression Formula: Highly aggressive drivers get a 20% speed boost
        if (DriverAggressiveness >= 0.85f) rb_base_speed *= 1.25f; // Lowered the requirement to 0.85 so more drivers qualify, and bumped the multiplier to 25%

        float rb_new_speed = rb_base_speed;
        float rb_cheat_power = 1.0f + PowerOffset;

        if (isBehind)
        {
            if (rb_distance > 100f) { rb_new_speed *= 1.6f; rb_cheat_power += 0.7f; }
            else if (rb_distance > 50f) { rb_new_speed *= 1.3f; rb_cheat_power += 0.4f; }
            else if (rb_distance < 15f) // Overtake/Drafting logic
            {
                rb_new_speed *= 1.25f;
                rb_cheat_power += 0.5f;
            }
            else { rb_new_speed *= 1.15f; rb_cheat_power += 0.2f; }
        }

        // Corner Exit Assist: Ensure they don't bog down after slow turns
        if (raceCar.Speed < 20f)
        {
            rb_cheat_power += 0.25f;
        }

        // Native Cornering Modifier: Scaling cornering efficiency (0.6 to 1.0) based on aggressiveness
        float cornerMod = 0.6f + (DriverAggressiveness * 0.4f);
        NativeFunction.Natives.SET_VEHICLE_STEERING_BIAS_SCALAR(raceCar, cornerMod);

        NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(driver, rb_new_speed);
        NativeFunction.Natives.MODIFY_VEHICLE_TOP_SPEED(raceCar, rb_new_speed);
        NativeFunction.Natives.SET_VEHICLE_CHEAT_POWER_INCREASE(raceCar, Math.Max(0.6f, rb_cheat_power));
        NativeFunction.Natives.SET_VEHICLE_IS_RACING(raceCar, true);
    }

    private void ApplyStabilityForce(Vehicle raceCar)
    {
        if (raceCar.Model.IsBike || raceCar.Model.IsQuadBike) return; // remove raceCar.Model.IsBike || raceCar.Model.IsQuadBike to cut down on air time for these vehicles.

        float speedFactor = raceCar.Speed * 0.0018f; // Scales the force based on current speed (tuning parameter) - 0.0018f allows for some natural air time
        NativeFunction.Natives.APPLY_FORCE_TO_ENTITY(raceCar, 3, 0f, 0f, -speedFactor, 0f, 0f, 0f, 0, false, true, true, false, true);
    }

    private bool CheckIfBehind(VehicleRacer playerRacer)
    {
        if (CurrentLap < playerRacer.CurrentLap) return true;
        if (CurrentLap > playerRacer.CurrentLap) return false;

        int myCP = TargetCheckpoint?.Order ?? 0;
        int playerCP = playerRacer.TargetCheckpoint?.Order ?? 0;

        if (myCP < playerCP) return true;
        if (myCP > playerCP) return false;

        return DistanceToCheckpoint > playerRacer.DistanceToCheckpoint;
    }

    private void TeleportToTargetCheckpoint(Vehicle raceCar)
    {
        if (TargetCheckpoint == null || hasBeenDisposed) return;
        Ped driver = PedExt.Pedestrian;

        if (!driver.IsInVehicle(raceCar, false)) driver.WarpIntoVehicle(raceCar, -1);

        raceCar.Position = new Vector3(TargetCheckpoint.Position.X, TargetCheckpoint.Position.Y, TargetCheckpoint.Position.Z + 0.5f);
        raceCar.Velocity = Vector3.Zero;

        if (AfterTargetCheckpoint != null)
        {
            Vector3 dir = AfterTargetCheckpoint.Position - TargetCheckpoint.Position;
            raceCar.Heading = (float)(Math.Atan2(-dir.X, dir.Y) * (180.0 / Math.PI));
        }

        NativeFunction.Natives.SET_VEHICLE_ON_GROUND_PROPERLY(raceCar);
        PedExt.CurrentTask?.Start();
    }

    public override void OnFinishedRace(int finalPosition, VehicleRace vehicleRace)
    {
        if (PedExt != null && VehicleExt != null && VehicleExt.Vehicle.Exists())
        {
            Vehicle raceCar = VehicleExt.Vehicle;
            Ped driver = PedExt.Pedestrian;

            // STOP THE RACE TASK
            PedExt.CurrentTask?.Stop();
            PedExt.CurrentTask = null;

            //  RESET PHYSICS & NATIVES
            // Return the car to standard performance so they don't wander at 150mph.
            NativeFunction.Natives.MODIFY_VEHICLE_TOP_SPEED(raceCar, -1.0f);
            NativeFunction.Natives.SET_VEHICLE_CHEAT_POWER_INCREASE(raceCar, 1.0f);
            NativeFunction.Natives.SET_VEHICLE_IS_RACING(raceCar, false);
            NativeFunction.Natives.SET_VEHICLE_HANDLING_OVERRIDE(raceCar, 0);
            NativeFunction.Natives.SET_VEHICLE_STRONG(raceCar, false);
            NativeFunction.Natives.SET_VEHICLE_HAS_STRONG_AXLES(raceCar, false);
            NativeFunction.Natives.SET_ENTITY_ONLY_DAMAGED_BY_PLAYER(raceCar, false);
            NativeFunction.Natives.SET_PED_CONFIG_FLAG(driver, 398, false); // allow drag out again

            //  START WANDERING
            // TASK_VEHICLE_DRIVE_WANDER(Ped ped, Vehicle vehicle, float speed, int drivingStyle)
            // Speed 20f is ~45mph. DrivingStyle 786603 is "Normal/Ignore Lights" which keeps them moving.
            NativeFunction.Natives.TASK_VEHICLE_DRIVE_WANDER(driver, raceCar, 20f, 786603);

            EntryPoint.WriteToConsole($"RACER {RacerName} finished and is now wandering.");
        }

        // Handle the winnings
        if (finalPosition == 1 && vehicleRace != null && vehicleRace.BetAmount > 0)
        {
            int totalWinAmount = vehicleRace.BetAmount * vehicleRace.VehicleRacers.Count();
            PedExt.Money += totalWinAmount;
        }

        base.OnFinishedRace(finalPosition, vehicleRace);
    }

    private void HandleRacerFailure(VehicleRace vehicleRace)
    {
        if (IsDisqualified) return;
        IsDisqualified = true;

        EntryPoint.WriteToConsole($"AI Racer {RacerName} is out of the race (Dead/Fell Off/Wrecked).");

        PedExt.CurrentTask?.Stop();
        PedExt.DeleteBlip();

        if (VehicleExt != null && VehicleExt.Vehicle.Exists())
        {

            if (vehicleRace.IsPinkSlipRace)
            {
                VehicleExt.Vehicle.IsPersistent = true;
                NativeFunction.Natives.SET_VEHICLE_DOORS_LOCKED(VehicleExt.Vehicle, 1);
                EntryPoint.WriteToConsole($"Pink Slips Enabled: Preserving vehicle {VehicleExt.Vehicle.Handle} for handover.");
            }

            NativeFunction.Natives.SET_VEHICLE_IS_RACING(VehicleExt.Vehicle, false);
            NativeFunction.Natives.SET_VEHICLE_HANDLING_OVERRIDE(VehicleExt.Vehicle, 0);
        }
    }

    public override void Dispose()
    {
        if (IsManualDispose)
        {
            return;
        }
        if (PedExt != null)
        {
            PedExt.SetNonPersistent();
            if (WasSpawnedForRace)
            {
                PedExt.DeleteBlip();
                if (PedExt.Pedestrian.Exists())
                {
                    PedExt.Pedestrian.IsPersistent = false;
                }
                PedExt.CanBeIdleTasked = true;
                PedExt.IsManuallyDeleted = false;
            }
            PedExt.IsManuallyDeleted = isManuallyDeleted;
            PedExt.CanBeAmbientTasked = true;
            PedExt.CanBeTasked = true;
            PedExt.CurrentTask?.Stop();
            PedExt.CurrentTask = null;
        }
        if (VehicleExt != null)
        {
            if (WasSpawnedForRace && !VehicleExt.IsOwnedByPlayer)
            {
                VehicleExt.RemoveBlip();
                if (VehicleExt.Vehicle.Exists())
                {
                    VehicleExt.Vehicle.IsPersistent = false;
                }
            }
            VehicleExt.IsManualCleanup = isManualCleanup;
        }
        base.Dispose();
    }

    public void ClearFront()
    {
        if (PedExt.IsInHelicopter || !PedExt.Pedestrian.Exists()) return;
        Vehicle raceCar = PedExt.Pedestrian.CurrentVehicle;
        if (!raceCar.Exists()) return;

        float length = raceCar.Model.Dimensions.Y;

        // --- ADJUSTABLE PARAMETERS ---

        // Increased distInFront: Moves the center of the 'eraser' further ahead of the bumper.
        // Increased range: Creates a larger diameter for the 'eraser' circle.
        float distInFront = (raceCar.Speed >= 27f || PedExt.DistanceToPlayer >= 120f) ? 15.0f : 6.0f;
        float range = (raceCar.Speed >= 27f || PedExt.DistanceToPlayer >= 120f) ? 18.0f : 10.0f;

        // --- END ADJUSTABLE PARAMETERS ---

        if (PedExt.DistanceToPlayer <= 25f || raceCar.IsOnScreen) // Increased distance check for player immersion
        {
            // Surgical deletion for nearby/visible racers
            Entity closest = Rage.World.GetClosestEntity(raceCar.GetOffsetPositionFront(length / 2f + distInFront), range, GetEntitiesFlags.ConsiderGroundVehicles | GetEntitiesFlags.ExcludePoliceCars | GetEntitiesFlags.ExcludePlayerVehicle);

            if (closest.Exists() && !closest.IsOnScreen && !closest.IsPersistent && closest.Handle != raceCar.Handle)
            {
                if (closest is Vehicle target) target.Delete();
            }
        }
        else
        {
            // Area-effect deletion for racers far from the player
            Vector3 pos = raceCar.GetOffsetPositionFront(length / 2f + distInFront);
            NativeFunction.Natives.CLEAR_AREA(pos.X, pos.Y, pos.Z, range, true, false, false, false);
        }
    }


    //Clear Traffic V2 - Math calculated in one used for both function,s instead of mathing twice
    //public void ClearFront()
    //{
    //    if (PedExt.IsInHelicopter || !PedExt.Pedestrian.Exists()) return;
    //    Vehicle raceCar = PedExt.Pedestrian.CurrentVehicle;
    //    if (!raceCar.Exists()) return;

    //    (float distInFront, float range) = GetClearanceBounds(raceCar);

    //    if (PedExt.DistanceToPlayer <= 20f || raceCar.IsOnScreen)
    //        CarefulFrontDelete(raceCar, distInFront, range);
    //    else
    //        LargeFrontDelete(raceCar, distInFront, range);
    //}

    //private (float dist, float range) GetClearanceBounds(Vehicle raceCar)
    //{
    //    if (raceCar.Speed >= 27f || PedExt.DistanceToPlayer >= 120f)
    //        return (9.0f, 12f);
    //    return (4.25f, 7.25f);
    //}

    //private void LargeFrontDelete(Vehicle raceCar, float distanceInFront, float range)
    //{
    //    float length = raceCar.Model.Dimensions.Y;
    //    Vector3 Position = raceCar.GetOffsetPositionFront(length / 2f + distanceInFront);
    //    NativeFunction.Natives.CLEAR_AREA(Position.X, Position.Y, Position.Z, range, true, false, false, false);
    //}

    //private void CarefulFrontDelete(Vehicle raceCar, float distanceInFront, float range)
    //{
    //    float length = raceCar.Model.Dimensions.Y;
    //    Entity ClosestCarEntity = Rage.World.GetClosestEntity(raceCar.GetOffsetPositionFront(length / 2f + distanceInFront), range, GetEntitiesFlags.ConsiderGroundVehicles | GetEntitiesFlags.ExcludePoliceCars | GetEntitiesFlags.ExcludePlayerVehicle);

    //    if (ClosestCarEntity.Exists() && !ClosestCarEntity.IsOnScreen && !ClosestCarEntity.IsPersistent)
    //    {
    //        if (ClosestCarEntity.Handle != raceCar.Handle)
    //        {
    //            if (ClosestCarEntity is Vehicle targetVeh)
    //            {
    //                foreach (Ped occupant in targetVeh.Occupants) occupant?.Delete();
    //                targetVeh.Delete();
    //            }
    //        }
    //    }
    //}



    //Previous Setup
    //public class AIVehicleRacer : VehicleRacer
    //{
    //    uint GameTimeLastClearedFront;
    //    private bool hasBeenDisposed;


    //    public AIVehicleRacer(PedExt pedExt, VehicleExt vehicleExt) : base(vehicleExt)
    //    {
    //        PedExt = pedExt;
    //    }
    //    public PedExt PedExt { get; set; }
    //    public bool WasSpawnedForRace { get; set; }
    //    public override string RacerName => PedExt == null ? base.RacerName : PedExt.Name;

    //    public bool IsManualDispose { get; set; } = false;
    //    public override void Update(VehicleRace vehicleRace)
    //    {
    //        if (vehicleRace == null)
    //        {
    //            return;
    //        }
    //        if (PedExt == null)
    //        {
    //            return;
    //        }
    //        if (HasFinishedRace)
    //        {
    //            return;
    //        }
    //        base.Update(vehicleRace);
    //        PedExt.CurrentTask?.Update();
    //        if (VehicleExt != null && VehicleExt.Vehicle.Exists())
    //        {
    //            NativeFunction.Natives.SET_VEHICLE_CHEAT_POWER_INCREASE(VehicleExt.Vehicle, 2.0f);
    //        }
    //        if (Game.GameTime - GameTimeLastClearedFront > 200 && vehicleRace.HasRaceStarted)
    //        {
    //            ClearFront();
    //            GameTimeLastClearedFront = Game.GameTime;
    //        }
    //    }
    //    public void AssignTask(VehicleRace vehicleRace, ITargetable Targetable, IEntityProvideable World, ISettingsProvideable Settings)
    //    {
    //        if (PedExt == null || !PedExt.Pedestrian.Exists())
    //        {
    //            return;
    //        }
    //        PedExt.Pedestrian.BlockPermanentEvents = true;
    //        PedExt.CurrentTask = new GeneralRace(PedExt, PedExt, Targetable, World, new List<VehicleExt>() { PedExt.AssignedVehicle }, null, Settings, vehicleRace, this);
    //        PedExt.CurrentTask.Start();




    //    }
    //    public override void Dispose()
    //    {
    //        if (IsManualDispose)
    //        {
    //            return;
    //        }
    //        if (PedExt != null)
    //        {
    //            PedExt.SetNonPersistent();
    //            if (WasSpawnedForRace)
    //            {
    //                PedExt.DeleteBlip();
    //                if (PedExt.Pedestrian.Exists())
    //                {
    //                    PedExt.Pedestrian.IsPersistent = false;
    //                }
    //                PedExt.CanBeIdleTasked = true;
    //                PedExt.IsManuallyDeleted = false;
    //            }
    //            PedExt.CanBeAmbientTasked = true;
    //            PedExt.CanBeTasked = true;
    //            PedExt.CurrentTask?.Stop();
    //            PedExt.CurrentTask = null;
    //        }
    //        if (VehicleExt != null)
    //        {
    //            if (WasSpawnedForRace && !VehicleExt.IsOwnedByPlayer)
    //            {
    //                VehicleExt.RemoveBlip();
    //                if (VehicleExt.Vehicle.Exists())
    //                {
    //                    VehicleExt.Vehicle.IsPersistent = false;
    //                }
    //            }
    //        }
    //        base.Dispose();
    //    }
    //    public override void OnFinishedRace(int finalPosition, VehicleRace vehicleRace)
    //    {
    //        if (finalPosition == 1 && vehicleRace != null && vehicleRace.BetAmount > 0)
    //        {
    //            int totalWinAmount = vehicleRace.BetAmount * vehicleRace.VehicleRacers.Count();
    //            PedExt.Money += totalWinAmount;
    //            EntryPoint.WriteToConsole($"OnFinishedRace AI {PedExt?.Handle} finalPosition:{finalPosition} totalWinAmount:{totalWinAmount}");
    //        }
    //        base.OnFinishedRace(finalPosition, vehicleRace);
    //    }
    //public void ClearFront()
    //{
    //    if (PedExt.IsInHelicopter || !PedExt.Pedestrian.Exists())
    //    {
    //        return;
    //    }
    //    Vehicle raceCar = PedExt.Pedestrian.CurrentVehicle;
    //    if (!raceCar.Exists())
    //    {
    //        return;
    //    }

    //    //EntryPoint.WriteToConsole("RACER CLEARED FRONT FOR ASSISTS");
    //    if (PedExt.DistanceToPlayer <= 20f || raceCar.IsOnScreen)//50f
    //    {
    //        CarefulFrontDelete(raceCar);
    //    }
    //    else
    //    {
    //        LargeFrontDelete(raceCar);
    //    }


    //}
    //private void LargeFrontDelete(Vehicle raceCar)
    //{
    //    if (!raceCar.Exists())
    //    {
    //        return;
    //    }
    //    float length = raceCar.Model.Dimensions.Y;
    //    float speed = raceCar.Speed;
    //    float distanceInFront = 4.25f;
    //    float range = 7.25f;// 4f;
    //    if (speed >= 27f || PedExt.DistanceToPlayer >= 120f)//if(speed >= 27f || Cop.DistanceToPlayer >= 150f)//~60mph//if (speed >= 27f || Cop.DistanceToPlayer >= 120f)//if(speed >= 27f || Cop.DistanceToPlayer >= 150f)//~60mph
    //    {
    //        distanceInFront = 9.0f;// 6.25f;
    //        range = 12f;// 10f;
    //    }
    //    Vector3 Position = raceCar.GetOffsetPositionFront(length / 2f + distanceInFront);
    //    NativeFunction.Natives.CLEAR_AREA(Position.X, Position.Y, Position.Z, range, true, false, false, false);
    //}
    //private void CarefulFrontDelete(Vehicle copCar)
    //{
    //    if (!copCar.Exists())
    //    {
    //        return;
    //    }
    //    float length = copCar.Model.Dimensions.Y;
    //    float speed = copCar.Speed;
    //    float distanceInFront = 4.25f;
    //    float range = 7.25f;// 4f;
    //    if (speed >= 27f || PedExt.DistanceToPlayer >= 120f)//if(speed >= 27f || Cop.DistanceToPlayer >= 150f)//~60mph//if (speed >= 27f || Cop.DistanceToPlayer >= 120f)//if(speed >= 27f || Cop.DistanceToPlayer >= 150f)//~60mph
    //    {
    //        distanceInFront = 9.0f;// 6.25f;
    //        range = 12f;// 10f;
    //    }
    //    Entity ClosestCarEntity = Rage.World.GetClosestEntity(copCar.GetOffsetPositionFront(length / 2f + distanceInFront), range, GetEntitiesFlags.ConsiderGroundVehicles | GetEntitiesFlags.ExcludePoliceCars | GetEntitiesFlags.ExcludePlayerVehicle);
    //    GameFiber.Yield();
    //    if (PedExt.Pedestrian.Exists() && ClosestCarEntity.Exists() && PedExt.Pedestrian.CurrentVehicle.Exists())
    //    {
    //        if (ClosestCarEntity != null && ClosestCarEntity.Handle != PedExt.Pedestrian.CurrentVehicle.Handle && !ClosestCarEntity.IsOnScreen && !ClosestCarEntity.IsPersistent)
    //        {
    //            Vehicle ClosestCar = (Vehicle)ClosestCarEntity;
    //            foreach (Ped carOccupant in ClosestCar.Occupants.ToList())
    //            {
    //                if (carOccupant.Exists())
    //                {
    //                    if (carOccupant.IsPersistent)
    //                    {
    //                        return;
    //                    }
    //                    carOccupant.Delete();
    //                }
    //            }
    //            if (ClosestCar.Exists())
    //            {
    //                ClosestCar.Delete();
    //            }
    //            GameFiber.Yield();
    //            EntryPoint.WriteToConsole($"DELETED CAR IN FRONT USING ASSIST MANAGER {PedExt.Handle}");
    //        }
    //    }
    //}

}