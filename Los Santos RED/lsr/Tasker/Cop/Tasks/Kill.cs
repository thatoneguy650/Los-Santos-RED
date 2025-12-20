using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;


public class Kill : ComplexTask
{
    private bool TargettingCar = false;
    private uint GametimeLastRetasked;
    private bool IsGoingToBeforeAttacking = false;
    private ISettingsProvideable Settings;
    private bool CanSiege = false;
    private bool isSetCode3Close;
    private bool isSetRegularCode3;
    private HeliEngage HeliEngage;
    private bool ShouldGoToBeforeAttack => Ped.IsAnimal || (Settings.SettingsManager.PoliceTaskSettings.AllowSiegeMode && Player.CurrentLocation.IsInside && Player.AnyPoliceKnowInteriorLocation && !Player.IsInWantedActiveMode /*!Player.AnyPoliceRecentlySeenPlayer */ && CanSiege);
    public Kill(IComplexTaskable cop, IPlayerChaseable playerchaseable,IEntityProvideable world, ITargetable player, ISettingsProvideable settings) : base(player, cop, 1000)
    {
        Name = "Kill";
        SubTaskName = "";
        Settings = settings;
        HeliEngage = new HeliEngage(cop, playerchaseable, world, Settings, player);
    }
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
            //EntryPoint.WriteToConsole($"KILL STARTED {Ped.Handle} IsAnimal: {Ped.IsAnimal}");
            if(RandomItems.RandomPercent(Settings.SettingsManager.PoliceTaskSettings.SiegePercentage))
            {
                CanSiege = true;
            }
            else
            {
                CanSiege = false;
            }



            ClearTasks();


            NativeFunction.Natives.SET_PED_USING_ACTION_MODE(Ped.Pedestrian, true, -1, "DEFAULT_ACTION");

            NativeFunction.Natives.SET_PED_SHOULD_PLAY_IMMEDIATE_SCENARIO_EXIT(Ped.Pedestrian);

            //NativeFunction.Natives.SET_PED_SHOOT_RATE(Ped.Pedestrian, 100);//30
            NativeFunction.Natives.SET_PED_ALERTNESS(Ped.Pedestrian, 3);//very altert
                                                                        // NativeFunction.Natives.SET_PED_COMBAT_ABILITY(Ped.Pedestrian, 2);//professional
                                                                        // NativeFunction.Natives.SET_PED_COMBAT_RANGE(Ped.Pedestrian, 2);//far
                                                                        //  NativeFunction.Natives.SET_PED_COMBAT_MOVEMENT(Ped.Pedestrian, 2);//offensinve
            if (Ped.IsInVehicle)
            {
                NativeFunction.Natives.SET_DRIVER_ABILITY(Ped.Pedestrian, Settings.SettingsManager.PoliceTaskSettings.DriverAbility);
                NativeFunction.Natives.SET_DRIVER_AGGRESSIVENESS(Ped.Pedestrian, Settings.SettingsManager.PoliceTaskSettings.DriverAggressiveness);
                if (Settings.SettingsManager.PoliceTaskSettings.DriverRacing > 0f)
                {
                    NativeFunction.Natives.SET_DRIVER_RACING_MODIFIER(Ped.Pedestrian, Settings.SettingsManager.PoliceTaskSettings.DriverRacing);
                }
                NativeFunction.Natives.SET_COMBAT_FLOAT(Ped.Pedestrian, 17, 2.0f);
            }
            if (Settings.SettingsManager.PoliceTaskSettings.BlockEventsDuringKill)
            {
                Ped.Pedestrian.BlockPermanentEvents = true;
            }
            else
            {
                Ped.Pedestrian.BlockPermanentEvents = false;
            }
            Ped.Pedestrian.KeepTasks = true;
            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_AlwaysFight, true);
            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_CanFightArmedPedsWhenNotArmed, true);
            //New
            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_WillDragInjuredPedsToSafety, true);

            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_Aggressive, true);


            if (Ped.IsDriver)
            {
                if (Ped.IsInHelicopter)
                {
                    HeliEngage.AssignTask();
                    //Vector3 pedPos = Player.Character.Position;
                    //if (Ped.Pedestrian.CurrentVehicle.Exists())
                    //{
                    //    HeliEngage.AssignTask();


                    //NativeFunction.Natives.TASK_HELI_CHASE(Ped.Pedestrian, Player.Character, RandomItems.RandomPercent(50) ? -40f : 40f, RandomItems.RandomPercent(50) ? -40f : 40f, RandomItems.GetRandomNumber(40f, 60f)); //NativeFunction.Natives.TASK_HELI_CHASE(Ped.Pedestrian, Player.Character, RandomItems.RandomPercent(50) ? -35f : 35f, RandomItems.RandomPercent(50) ? -35f : 35f, RandomItems.GetRandomNumber(50f, 70f));



                    //NativeFunction.Natives.TASK_HELI_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle,
                    //    0, Player.Character,//target vehicle and ped
                    //    0f, 0f, 0f,//coordinated, shouldnt be needed
                    //    Settings.SettingsManager.DebugSettings.HeliMission,//MISSION
                    //    100f,//Cruise SPeed
                    //    50f,//Target Reached DIst
                    //    -1f,//Heli Orientation
                    //    50,//flight height
                    //    50, //min hiehg tabove terrain
                    //    -1.0f,//slowdown distance
                    //    0//HELIMODE heli flags, 0 is none
                    //    );

                    
                    /*
                    ENUM VEHICLE_MISSION
                        MISSION_NONE = 0,               // 0
                        MISSION_CRUISE,                 // 1
                        MISSION_RAM,                    // 2
                        MISSION_BLOCK,                  // 3
                        MISSION_GOTO,                   // 4
                        MISSION_STOP,                   // 5
                        MISSION_ATTACK,                 // 6
                        MISSION_FOLLOW,                 // 7
                        MISSION_FLEE,                   // 8
                        MISSION_CIRCLE,                 // 9
                        MISSION_ESCORT_LEFT,            // 10
                        MISSION_ESCORT_RIGHT,           // 11
                        MISSION_ESCORT_REAR,            // 12
                        MISSION_ESCORT_FRONT,           // 13
                        MISSION_GOTO_RACING,            // 14
                        MISSION_FOLLOW_RECORDING,       // 15
                        MISSION_POLICE_BEHAVIOUR,       // 16
                        MISSION_PARK_PERPENDICULAR,     // 17
                        MISSION_PARK_PARALLEL,          // 18
                        MISSION_LAND,                   // 19
                        MISSION_LAND_AND_WAIT,          // 20
                        MISSION_CRASH,                  // 21
                        MISSION_PULL_OVER,               // 22
                        MISSION_PROTECT					// 23
                    ENDENUM */


                    //if (Player.Character.CurrentVehicle.Exists())
                    //{
                    //    NativeFunction.Natives.TASK_HELI_CHASE(Ped.Pedestrian, Player.Character, 0.0f, 0.0f, RandomItems.GetRandomNumber(70f, 80f));//NativeFunction.Natives.TASK_HELI_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, Player.Character.CurrentVehicle, Player.Character, pedPos.X, pedPos.Y, pedPos.Z, 9, 50f, 150f, -1f, -1, 30, -1.0f, 0);//NativeFunction.Natives.TASK_HELI_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, Player.Character.CurrentVehicle, Player.Character, pedPos.X, pedPos.Y, pedPos.Z, 9, 50f, 150f, -1f, -1, 30, -1.0f, 0);
                    //}
                    //else
                    //{
                    //    NativeFunction.Natives.TASK_HELI_CHASE(Ped.Pedestrian, Player.Character, 0.0f, 0.0f, RandomItems.GetRandomNumber(70f, 80f)); //NativeFunction.Natives.TASK_HELI_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, Player.Character, pedPos.X, pedPos.Y, pedPos.Z, 9, 50f, 150f, -1f, -1, 30, -1.0f, 0);//NativeFunction.Natives.TASK_HELI_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, Player.Character, pedPos.X, pedPos.Y, pedPos.Z, 9, 50f, 150f, -1f, -1, 30, -1.0f, 0);
                    //}
                    //}
                }
                else if (Ped.IsInPlane)
                {
                    NativeFunction.Natives.TASK_PLANE_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, Player.Character, 0f, 0f, 0f, 6, 100f, 40, -1.0f, 40, 20, true);//THIS KINDA WORKS//NativeFunction.Natives.TASK_PLANE_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, Player.Character, 0f,0f, 0f, 6, 70f, 40, -1.0f, 40, 20, true);
                }
                else if (Ped.IsInBoat)
                {
                    NativeFunction.Natives.TASK_VEHICLE_CHASE(Ped.Pedestrian, Player.Character);
                }
                else
                {
                    //NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.FullContact, true);

                    NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_DisableCruiseInFrontDuringBlockDuringVehicleChase, false);
                    NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_DisableSpinOutDuringVehicleChase, false);
                    NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_DisableBlockFromPursueDuringVehicleChase, false);


                    ////NEW CHASE STUFF
                    //NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.CantPullAlongsideInFront, false);
                    //NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.UseContinuousRam, true);
                    //NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.CantPullAlongside, false);



                    NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.FullContact, true);
                    NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.MediumContact, false);
                    NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.LowContact, false);
                    NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.PIT, false);
                    NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.NoContact, false);
                    NativeFunction.Natives.SET_DRIVE_TASK_MAX_CRUISE_SPEED(Ped.Pedestrian, 150f);

                    NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE(Ped.Pedestrian, 0f);
                    //int DesiredStyle = (int)eDrivingStyles.AvoidEmptyVehicles | (int)eDrivingStyles.AvoidPeds | (int)eDrivingStyles.AvoidObject | (int)eDrivingStyles.AllowWrongWay | (int)eDrivingStyles.ShortestPath;
                    NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.Code3);
                    //AssignCombat();

                    NativeFunction.Natives.TASK_VEHICLE_CHASE(Ped.Pedestrian, Player.Character);
                    //NativeFunction.Natives.TASK_COMBAT_HATED_TARGETS_AROUND_PED(Ped.Pedestrian, 300f, 0);
                }
            }
            else
            {
                AssignCombat();
            }
            if (!Ped.IsInHelicopter && !Ped.IsInPlane)
            {
                UpdateCombat();
            }
        }  
    }
    public override void Update()
    {
        if(!Ped.Pedestrian.Exists())
        {
            return;
        }

        if(Ped.IsDriver && (Ped.IsInHelicopter || Ped.IsInPlane))
        {
            Ped.ControlLandingGear();
            //if(Ped.IsDriver)
            //{
            //    if (Ped.DistanceToPlayer <= 100f && Player.Character.Speed < 32f)//70 mph
            //    {
            //        NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 10f);
            //    }
            //    else
            //    {
            //        NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 100f);
            //    }
            //}
        }
        if(Ped.IsDriver && !Ped.IsInHelicopter && !Ped.IsInPlane)
        {
            //NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.FullContact, true);
            //NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE(Ped.Pedestrian, 0f);
            NativeFunction.Natives.SET_DRIVER_ABILITY(Ped.Pedestrian, Settings.SettingsManager.PoliceTaskSettings.DriverAbility);
            NativeFunction.Natives.SET_DRIVER_AGGRESSIVENESS(Ped.Pedestrian, Settings.SettingsManager.PoliceTaskSettings.DriverAggressiveness);
            if (Settings.SettingsManager.PoliceTaskSettings.DriverRacing > 0f)
            {
                NativeFunction.Natives.SET_DRIVER_RACING_MODIFIER(Ped.Pedestrian, Settings.SettingsManager.PoliceTaskSettings.DriverRacing);
            }


            if ((Ped.RecentlySeenPlayer && Ped.DistanceToPlayer <= Settings.SettingsManager.PoliceTaskSettings.DriveBySightDuringChaseDistance) && Settings.SettingsManager.PoliceTaskSettings.AllowDriveBySightDuringChase)
            {
                if (!isSetCode3Close)
                {
                    NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.Code3Close);
                    isSetCode3Close = true;
                    isSetRegularCode3 = false;
                }
            }
            else
            {
                if (isSetRegularCode3)
                {
                    NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.Code3);
                    isSetRegularCode3 = true;
                    isSetCode3Close = false;
                }
            }


        }
        if(!Ped.IsDriver && Ped.DistanceToPlayer <= 100f && Ped.Pedestrian.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask && Game.GameTime - GametimeLastRetasked >= 1000)
        {
            NativeFunction.Natives.SET_PED_ALERTNESS(Ped.Pedestrian, 3);//very altert
            //NativeFunction.Natives.SET_DRIVER_ABILITY(Ped.Pedestrian, 100f);
            if (Settings.SettingsManager.PoliceTaskSettings.BlockEventsDuringKill)
            {
                Ped.Pedestrian.BlockPermanentEvents = true;
            }
            else
            {
                Ped.Pedestrian.BlockPermanentEvents = false;
            }
            Ped.Pedestrian.KeepTasks = true;
            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_AlwaysFight, true);
            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_CanFightArmedPedsWhenNotArmed, true);
            //NativeFunction.Natives.TASK_COMBAT_HATED_TARGETS_AROUND_PED(Ped.Pedestrian, 300f, 0);
            AssignCombat();
            //EntryPoint.WriteToConsole($"KillTask: {Ped.Pedestrian.Handle} Reset Combat", 5);
            GametimeLastRetasked = Game.GameTime;
        }
        if(!Ped.IsInVehicle)
        {
            if(ShouldGoToBeforeAttack != IsGoingToBeforeAttacking)
            {
                UpdateCombat();
                //EntryPoint.WriteToConsoleTestLong($"KILL Task Target Changed to {Player.CurrentLocation.IsInside}");
            }
        }
        if(Ped.IsAnimal)
        {
            NativeFunction.Natives.SET_PED_MOVE_RATE_OVERRIDE<uint>(Ped.Pedestrian, Settings.SettingsManager.PoliceTaskSettings.K9RunSpeed);
        }
        if (Ped.IsInHelicopter)
        {
            HeliEngage.UpdateTask();
        }
        if (IsGoingToBeforeAttacking && Ped.PlayerPerception.CanSeeTarget)
        {
            EntryPoint.WriteToConsole("YOU HAVE BEEN SEEN TRANSITIONING TO REGULAR COMBAT");
            IsGoingToBeforeAttacking = false;
            AssignCombat();
        }
    }
    public override void ReTask()
    {

    }
    public void ClearTasks()//temp public
    {
        if (Ped.Pedestrian.Exists())
        {
            //int seatIndex = 0;
            //Vehicle CurrentVehicle = null;
            //bool WasInVehicle = false;
            //if (Ped.Pedestrian.IsInAnyVehicle(false))
            //{
            //    WasInVehicle = true;
            //    CurrentVehicle = Ped.Pedestrian.CurrentVehicle;
            //    seatIndex = Ped.Pedestrian.SeatIndex;
            //}
            ////Ped.Pedestrian.Tasks.Clear();
            //NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
            ////Ped.Pedestrian.BlockPermanentEvents = false;
            ////Ped.Pedestrian.KeepTasks = false;
            ////Ped.Pedestrian.RelationshipGroup.SetRelationshipWith(RelationshipGroup.Player, Relationship.Neutral);
            //if (WasInVehicle && !Ped.Pedestrian.IsInAnyVehicle(false) && CurrentVehicle.Exists())
            //{
            //    Ped.Pedestrian.WarpIntoVehicle(CurrentVehicle, seatIndex);
            //}            
            //EntryPoint.WriteToConsole(string.Format("     ClearedTasks: {0}", Ped.Pedestrian.Handle));
        }
    }
    public override void Stop()
    {

    }
    private void UpdateCombat()
    {
        if (ShouldGoToBeforeAttack)
        {
            IsGoingToBeforeAttacking = true;
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);

                if(Ped.IsAnimal)
                {
                    NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Player.Character, -1, 7f, 500f, 1073741824, 1); //Original and works ok
                    NativeFunction.CallByName<bool>("TASK_PUT_PED_DIRECTLY_INTO_MELEE", 0, Player.Character, 0.0f, -1.0f, 0.0f, 134217728);
                }
                else
                {
                    //NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Player.Character, -1, 0.01f, 500f, 1073741824, 1); //Original and works ok
                    NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", 0, Player.Character, Settings.SettingsManager.PoliceTaskSettings.SiegeGotoDistance, Settings.SettingsManager.PoliceTaskSettings.SiegeAimDistance);
                }
                //NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY_WHILE_AIMING_AT_ENTITY", 0, Player.Character, Player.Character, 200f, true, 10.0f, 200f, false, false, (uint)FiringPattern.DelayFireByOneSecond);
                // NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Player.Character, -1, 7f, 500f, 1073741824, 1); //Original and works ok
                NativeFunction.CallByName<bool>("TASK_COMBAT_PED", 0, Player.Character, Ped.DefaultCombatFlag, 16); ;// NativeFunction.CallByName<bool>("TASK_COMBAT_PED", 0, Player.Character, Ped.DefaultCombatFlag == 0 ? 134217728 : Ped.DefaultCombatFlag, 16);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
        else
        {
            IsGoingToBeforeAttacking = false;
            AssignCombat();
            //NativeFunction.Natives.TASK_COMBAT_PED(Ped.Pedestrian, Player.Character, 0, 16);
        }
    }
    private void AssignCombat()
    {
        unsafe
        {
            int lol = 0;
            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            NativeFunction.CallByName<bool>("TASK_COMBAT_PED", 0, Player.Character, Ped.DefaultCombatFlag, 16);
            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        }

        //NativeFunction.Natives.TASK_COMBAT_PED(Ped.Pedestrian, Player.Character, Ped.DefaultCombatFlag, 16);//

        EntryPoint.WriteToConsole($"ASSIGNED COMBAT TASK TO {Ped.Handle} DefaultCombatFlag:{Ped.DefaultCombatFlag}");



        


    }

    private enum COMBAT_ATTRIBUTE_FLOATS
    {
        CCF_BLIND_FIRE_CHANCE,              // Chance to blind fire from cover, range is 0.0-1.0 (default is 0.05 for civilians, law doesn't blind fire)
        CCF_BURST_DURATION_IN_COVER,        // How long each burst from cover should last (default is 2.0)
        CCF_MAX_SHOOTING_DISTANCE,          // The maximum distance the ped will try to shoot from (will override weapon range if set to anything > 0.0, default is -1.0)
        CCF_TIME_BETWEEN_BURSTS_IN_COVER,   // How long to wait, in cover, between firing bursts (< 0.0 will disable firing, unless cover fire is requested, default is 1.25)
        CCF_TIME_BETWEEN_PEEKS,             // How long to wait before attempting to peek again (default is 10.0)
        CCF_STRAFE_WHEN_MOVING_CHANCE,      // A chance to strafe to cover, range is 0.0-1.0 (0.0 will force them to run, 1.0 will force strafe and shoot, default is 1.0)
        CCF_WEAPON_ACCURACY,                // default is 0.4
        CCF_FIGHT_PROFICIENCY,              // How well an opponent can melee fight, range is 0.0-1.0 (default is 0.5)
        CCF_WALK_WHEN_STRAFING_CHANCE,      // The possibility of a ped walking while strafing rather than jog/run, range is 0.0-1.0 (default is 0.0)
        CCF_HELI_SPEED_MODIFIER,            // The speed modifier when driving a heli in combat
        CCF_HELI_SENSES_RANGE,              // The range of the ped's senses (sight, identification, hearing) when in a heli
        CCF_ATTACK_WINDOW_DISTANCE_FOR_COVER, // The distance we'll use for cover based behaviour in attack windows Default is -1.0 (disabled), range is -1.0 to 150.0
        CCF_TIME_TO_INVALIDATE_INJURED_TARGET,  // How long to stop combat an injured target if there is no other valid target, if target is player in singleplayer
                                                // this will happen indefinitely unless explicitly disabled by setting to 0.0, default = 10.0 range = 0-50
        CCF_MIN_DISTANCE_TO_TARGET,         // Min distance the ped will use if CA_MAINTAIN_MIN_DISTANCE_TO_TARGET is set, default 5.0 (currently only for cover search + usage)
        CCF_BULLET_IMPACT_DETECTION_RANGE,  // The range at which the ped will detect the bullet impact event
        CCF_AIM_TURN_THRESHOLD,             // The threshold at which the ped will perform an aim turn
        CCF_OPTIMAL_COVER_DISTANCE,         //
        CCF_AUTOMOBILE_SPEED_MODIFIER,      // The speed modifier when driving an automobile in combat
        CCF_SPEED_TO_FLEE_IN_VEHICLE,       //
        CCF_TRIGGER_CHARGE_TIME_NEAR,       // How long to wait before charging a close target hiding in cover
        CCF_TRIGGER_CHARGE_TIME_FAR,        // How long to wait before charging a distant target hiding in cover
        CCF_MAX_DISTANCE_TO_HEAR_EVENTS, // Max distance peds can hear an event from, even if the sound is louder
        CCF_MAX_DISTANCE_TO_HEAR_EVENTS_USING_LOS, // Max distance peds can hear an event from, even if the sound is louder if the ped is using LOS to hear events (CPED_CONFIG_FLAG_CheckLoSForSoundEvents)				
        CCF_HOMING_ROCKET_BREAK_LOCK_ANGLE,             // Angle between the rocket and target where lock-on will stop, range is 0.0-1.0, (default is 0.2), the bigger the number the easier to break lock
        CCF_HOMING_ROCKET_BREAK_LOCK_ANGLE_CLOSE,       // Angle between the rocket and target where lock-on will stop, when rocket is within CCF_HOMING_ROCKET_BREAK_LOCK_CLOSE_DISTANCE, range is 0.0-1.0, (default is 0.6), the bigger the number the easier to break lock
        CCF_HOMING_ROCKET_BREAK_LOCK_CLOSE_DISTANCE,    // Distance at which we check CCF_HOMING_ROCKET_BREAK_LOCK_ANGLE_CLOSE rather than CCF_HOMING_ROCKET_BREAK_LOCK_ANGLE
        CCF_HOMING_ROCKET_TURN_RATE_MODIFIER,           // Alters homing characteristics defined for the weapon (1.0 is default, <1.0 slow turn rates, >1.0 speed them up
        CCF_TIME_BETWEEN_AGGRESSIVE_MOVES_DURING_VEHICLE_CHASE, // Sets the time delay between aggressive moves during vehicle chases. -1.0 means use random values, 0.0 means never
        CCF_MAX_VEHICLE_TURRET_FIRING_RANGE,    // Max firing range for a ped in vehicle turret seat
        CCF_WEAPON_DAMAGE_MODIFIER,             // Multiplies the weapon damage dealt by the ped, range is 0.0-10.0 (default is 1.0)
        MAX_COMBAT_FLOATS
    }
                        //*/

}





