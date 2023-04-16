using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SecurityGuardBrain : PedBrain
{
    private SecurityGuard SecurityGuard;
    private uint GameTimeLastSeenCrime;
    private bool printDebug = true;
    private int debugLevel => printDebug ? 5 : 6;

    public SecurityGuardBrain(SecurityGuard securityGuard, ISettingsProvideable settings, IEntityProvideable world, IWeapons weapons) : base(securityGuard,settings,world, weapons)
    {
        SecurityGuard = securityGuard;
        Settings = settings;
        World = world;
    }
    public override void Setup()
    {

    }
    public override void Dispose()
    {

    }
    public override void Update(ITargetable player, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        PlacesOfInterest = placesOfInterest;

        if (PedExt.CanBeTasked && PedExt.CanBeAmbientTasked)
        {
            UpdateTask();
        }
        else if (!PedExt.CanBeTasked)
        {
            RemoveTask();
        }
    }
    private void UpdateTask()
    {
        if (PedExt.DistanceToPlayer >= 230f)
        {
            PedExt.CurrentTask = null;
            return;
        }
        if (PedExt.NeedsTaskAssignmentCheck)
        {
            if (PedExt.DistanceToPlayer <= 200f)
            {
                UpdateCurrentTask();//has yields if it does anything
            }
            else if (PedExt.CurrentTask != null)
            {
                PedExt.CurrentTask = null;
            }
        }
        if (PedExt.CurrentTask != null && PedExt.CurrentTask.ShouldUpdate)
        {
            PedExt.UpdateTask(null);
            GameFiber.Yield();
        }
    }
    private void RemoveTask()
    {
        if (PedExt.CurrentTask != null)
        {
            PedExt.CurrentTask = null;
        }
    }
    private void UpdateCurrentTask()
    {
        if (PedExt.DistanceToPlayer <= 100f)//50f
        {
            PedExt.PedReactions.Update(Player);
            if(PedExt.PedReactions.ReactionTier == ReactionTier.Intense)
            {




                if (SecurityGuard.WeaponInventory.IsArmed)
                {
                    if (SecurityGuard.PlayerPerception.HasSeenTargetWithin(30000))
                    {
                        SetFight();
                    }
                    else
                    {
                        SetLocate();
                    }
                }
                else
                {
                    SetFlee();
                }
            }
            else if (PedExt.PedReactions.ReactionTier == ReactionTier.Alerted)
            {

                if (SecurityGuard.PlayerPerception.HasSeenTargetWithin(30000))
                {
                    SetApprehend();
                }
                else
                {
                    SetLocate();
                }

                
            }
            else if (PedExt.CanAttackPlayer)
            {
                if (SecurityGuard.PlayerPerception.HasSeenTargetWithin(30000))
                {
                    SetChase();// SetFight();
                }
                else
                {
                    SetLocate();
                }
            }
            else if (PedExt.PedReactions.ReactionTier == ReactionTier.Mundane)
            {
                SetCalmCallIn();
            }
            else if (PedExt.WasModSpawned && PedExt.PedReactions.ReactionTier == ReactionTier.None)
            {
                SetIdle();
            }
            HandleCrimeReports();
        }
        PedExt.GameTimeLastUpdatedTask = Game.GameTime;
    }

    private void SetApprehend()
    {
        if(PedExt.PedReactions.PrimaryPedReaction?.IsReactingToPlayer == false)//  PedExt.PedReactions.HighestPriorityCrime?.Perpetrator != null)
        {
            SetAIApprehend();
        }
        else
        {
            SetChase();
        }
    }
    private void SetChase()
    {
        SecurityGuard.WeaponInventory.SetLessLethal();
        if (PedExt.CurrentTask?.Name == "Chase")
        {
            return;
        }
        SecurityGuard.CurrentTask = new Chase(SecurityGuard, Player, World, SecurityGuard, Settings) { UseWantedLevel = false };
        SecurityGuard.WeaponInventory.Reset();
        GameFiber.Yield();//TR Added back 4
        SecurityGuard.CurrentTask.Start();
       // EntryPoint.WriteToConsole($"SECURITY SET Chase {PedExt.Handle}", debugLevel);
    }
    private void SetAIApprehend()
    {
        SecurityGuard.WeaponInventory.SetLessLethal();
        if (PedExt.CurrentTask?.Name == "AIApprehend")
        {
            return;
        }
        SecurityGuard.CurrentTask = new AIApprehend(SecurityGuard, Player, SecurityGuard, Settings) { OtherTarget = PedExt.PedReactions.PrimaryPedReaction?.ReactingToPed, UseWantedLevel = false };
        SecurityGuard.WeaponInventory.Reset();
        GameFiber.Yield();//TR Added back 4
        SecurityGuard.CurrentTask.Start();
        //EntryPoint.WriteToConsole($"SECURITY SET AIApprehend {PedExt.Handle}", debugLevel);
    }
    private void HandleCrimeReports()
    {
        if (GameTimeLastSeenCrime == 0 && (PedExt.PlayerCrimesWitnessed.Any() || PedExt.OtherCrimesWitnessed.Any() || PedExt.HasSeenDistressedPed))
        {
            GameTimeLastSeenCrime = Game.GameTime;
            //EntryPoint.WriteToConsole("SECURITY SEEN FIRST CRIME", debugLevel);
        }
        if (GameTimeLastSeenCrime != 0 && Game.GameTime - GameTimeLastSeenCrime >= 10000 && PedExt.Pedestrian.Exists() && !PedExt.IsDead && !PedExt.IsUnconscious && !PedExt.Pedestrian.IsRagdoll && PedExt.DistanceToPlayer <= 40f)
        {
            GameTimeLastSeenCrime = 0;
            PedExt.ReportCrime(Player);
           // EntryPoint.WriteToConsole("SECURITY REPORTED CRIME", debugLevel);
        }
    }
    private void SetFlee()
    {
        if (PedExt.CurrentTask?.Name == "Flee")
        {
            return;
        }
        PedExt.CurrentTask = new Flee(PedExt, Player) { OtherTarget = PedExt.PedReactions.HighestPriorityCrime?.Perpetrator };
        GameFiber.Yield();//TR Added back 7
        PedExt.CurrentTask?.Start();
        //EntryPoint.WriteToConsole($"SECURITY SET FLEE {PedExt.Handle}", debugLevel);
    }

    private void SetLocate()
    {
        if (PedExt.CurrentTask?.Name == "GeneralLocate")
        {
            return;
        }
        PedExt.CurrentTask = new GeneralLocate(PedExt, PedExt, Player, World,null,PlacesOfInterest,Settings,true,null,true) { OtherTarget = PedExt.PedReactions.HighestPriorityCrime?.Perpetrator };
        GameFiber.Yield();//TR Added back 7
        PedExt.CurrentTask?.Start();
        //EntryPoint.WriteToConsole($"SECURITY SET GeneralLocate {PedExt.Handle}", debugLevel);
    }


    private void SetFight()
    {
        SecurityGuard.WeaponInventory.SetDeadly(false);
        if (PedExt.CurrentTask?.Name == "Fight")
        {
            return;
        }
        PedExt.CurrentTask?.Stop();
        PedExt.CurrentTask = new Fight(PedExt, Player, null) { OtherTarget = PedExt.PedReactions.HighestPriorityCrime?.Perpetrator };//gang memebrs already have guns
        //SecurityGuard.WeaponInventory.ShouldAutoSetWeaponState = false;
        //SecurityGuard.WeaponInventory.SetDeadly(false);
        GameFiber.Yield();//TR Added back 7
        PedExt.CurrentTask?.Start();
        //EntryPoint.WriteToConsole($"SECURITY SET FIGHT {PedExt.Handle}", debugLevel);
    }
    private void SetCalmCallIn()
    {
        if (PedExt.CurrentTask?.Name == "CalmCallIn")
        {
            return;
        }
        PedExt.CurrentTask = new CalmCallIn(PedExt, Player);
        GameFiber.Yield();//TR Added back 4
        PedExt.CurrentTask.Start();
        EntryPoint.WriteToConsole($"SECURITY SET CALM CALL IN {PedExt.Handle}", debugLevel);
    }
    private void SetIdle()
    {
        if (PedExt.CurrentTask?.Name == "Idle")
        {
            return;
        }
        PedExt.CurrentTask = new GeneralIdle(PedExt, PedExt, Player, World, new List<VehicleExt>() { PedExt.AssignedVehicle },  PlacesOfInterest, Settings,false,false,false, true);
        SecurityGuard.WeaponInventory.Reset();
        SecurityGuard.WeaponInventory.SetDefault();
        GameFiber.Yield();//TR Added back 4
        PedExt.CurrentTask.Start();
       // EntryPoint.WriteToConsole($"SECURITY SET IDLE {PedExt.Handle}", debugLevel);



        //if (PedExt.CurrentTask?.Name == "GenericIdle")
        //{
        //    return;
        //}
        //PedExt.CurrentTask = new GenericIdle(PedExt, Player, World, PlacesOfInterest);
        //SecurityGuard.WeaponInventory.SetDefault();
        //GameFiber.Yield();//TR Added back 4
        //PedExt.CurrentTask.Start();
        //EntryPoint.WriteToConsole($"SECURITY SET IDLE {PedExt.Handle}");
    }
}




//if(PedExt.Pedestrian.Exists())
//{
//    NativeFunction.Natives.SET_PED_COMBAT_RANGE(PedExt.Pedestrian, 0);//CR_Near
//    NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(PedExt.Pedestrian, 31, true);//CA_MAINTAIN_MIN_DISTANCE_TO_TARGET
//    NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(PedExt.Pedestrian, 0, false);//CA_USE_COVER



//    NativeFunction.Natives.SET_COMBAT_FLOAT(PedExt.Pedestrian, 2, 5.0f);//CCF_MAX_SHOOTING_DISTANCE
//    NativeFunction.Natives.SET_COMBAT_FLOAT(PedExt.Pedestrian, 13, 2.0f);//CCF_MIN_DISTANCE_TO_TARGET
//    //CCF_MIN_DISTANCE_TO_TARGET
//    //set combat float   CCF_MAX_SHOOTING_DISTANCE
//}

//public enum COMBAT_ATTRIBUTE_FLOATS
//{ 
//    CCF_BLIND_FIRE_CHANCE,				// Chance to blind fire from cover, range is 0.0-1.0 (default is 0.05 for civilians, law doesn't blind fire)
//    CCF_BURST_DURATION_IN_COVER,		// How long each burst from cover should last (default is 2.0)
//    CCF_MAX_SHOOTING_DISTANCE,			// The maximum distance the ped will try to shoot from (will override weapon range if set to anything > 0.0, default is -1.0)
//    CCF_TIME_BETWEEN_BURSTS_IN_COVER,	// How long to wait, in cover, between firing bursts (< 0.0 will disable firing, unless cover fire is requested, default is 1.25)
//    CCF_TIME_BETWEEN_PEEKS,				// How long to wait before attempting to peek again (default is 10.0)
//    CCF_STRAFE_WHEN_MOVING_CHANCE,		// A chance to strafe to cover, range is 0.0-1.0 (0.0 will force them to run, 1.0 will force strafe and shoot, default is 1.0)
//    CCF_WEAPON_ACCURACY,				// default is 0.4
//    CCF_FIGHT_PROFICIENCY,				// How well an opponent can melee fight, range is 0.0-1.0 (default is 0.5)
//    CCF_WALK_WHEN_STRAFING_CHANCE,		// The possibility of a ped walking while strafing rather than jog/run, range is 0.0-1.0 (default is 0.0)
//    CCF_HELI_SPEED_MODIFIER,			// The speed modifier when driving a heli in combat
//    CCF_HELI_SENSES_RANGE,				// The range of the ped's senses (sight, identification, hearing) when in a heli
//    CCF_ATTACK_WINDOW_DISTANCE_FOR_COVER, // The distance we'll use for cover based behaviour in attack windows Default is -1.0 (disabled), range is -1.0 to 150.0
//    CCF_TIME_TO_INVALIDATE_INJURED_TARGET,	// How long to stop combat an injured target if there is no other valid target, if target is player in singleplayer
//                                            // this will happen indefinitely unless explicitly disabled by setting to 0.0, default = 10.0 range = 0-50
//    CCF_MIN_DISTANCE_TO_TARGET,			// Min distance the ped will use if CA_MAINTAIN_MIN_DISTANCE_TO_TARGET is set, default 5.0 (currently only for cover search + usage)
//    CCF_BULLET_IMPACT_DETECTION_RANGE,	// The range at which the ped will detect the bullet impact event
//    CCF_AIM_TURN_THRESHOLD,				// The threshold at which the ped will perform an aim turn
//    CCF_OPTIMAL_COVER_DISTANCE,			//
//    CCF_AUTOMOBILE_SPEED_MODIFIER,		// The speed modifier when driving an automobile in combat
// CCF_SPEED_TO_FLEE_IN_VEHICLE,		//
// CCF_TRIGGER_CHARGE_TIME_NEAR,		// How long to wait before charging a close target hiding in cover
// CCF_TRIGGER_CHARGE_TIME_FAR,		// How long to wait before charging a distant target hiding in cover
// CCF_MAX_DISTANCE_TO_HEAR_EVENTS, // Max distance peds can hear an event from, even if the sound is louder
// CCF_MAX_DISTANCE_TO_HEAR_EVENTS_USING_LOS, // Max distance peds can hear an event from, even if the sound is louder if the ped is using LOS to hear events (CPED_CONFIG_FLAG_CheckLoSForSoundEvents)				
// CCF_HOMING_ROCKET_BREAK_LOCK_ANGLE,				// Angle between the rocket and target where lock-on will stop, range is 0.0-1.0, (default is 0.2), the bigger the number the easier to break lock
// CCF_HOMING_ROCKET_BREAK_LOCK_ANGLE_CLOSE,		// Angle between the rocket and target where lock-on will stop, when rocket is within CCF_HOMING_ROCKET_BREAK_LOCK_CLOSE_DISTANCE, range is 0.0-1.0, (default is 0.6), the bigger the number the easier to break lock
// CCF_HOMING_ROCKET_BREAK_LOCK_CLOSE_DISTANCE,	// Distance at which we check CCF_HOMING_ROCKET_BREAK_LOCK_ANGLE_CLOSE rather than CCF_HOMING_ROCKET_BREAK_LOCK_ANGLE
// CCF_HOMING_ROCKET_TURN_RATE_MODIFIER,			// Alters homing characteristics defined for the weapon (1.0 is default, <1.0 slow turn rates, >1.0 speed them up
// CCF_TIME_BETWEEN_AGGRESSIVE_MOVES_DURING_VEHICLE_CHASE,	// Sets the time delay between aggressive moves during vehicle chases. -1.0 means use random values, 0.0 means never
// CCF_MAX_VEHICLE_TURRET_FIRING_RANGE,	// Max firing range for a ped in vehicle turret seat
// CCF_WEAPON_DAMAGE_MODIFIER,				// Multiplies the weapon damage dealt by the ped, range is 0.0-10.0 (default is 1.0)
//    MAX_COMBAT_FLOATS
//};



//enum eCombatRange // 0xB69160F5
//{
//    CR_Near,
//    CR_Medium,
//    CR_Far,
//    CR_VeryFar,
//    CR_NumRanges
//};

