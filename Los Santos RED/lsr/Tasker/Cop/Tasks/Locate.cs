using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Locate : ComplexTask
{
    private bool NeedsUpdates;
    private Vector3 CurrentTaskedPosition;
    private Task CurrentTask = Task.Nothing;
    private bool HasReachedReportedPosition;
    private bool isSetCode3Close;
    private bool hasSixthSense = false;
    private ISettingsProvideable Settings;
    private Vector3 PlaceToGoTo => hasSixthSense ? Player.PlacePoliceShouldSearchForPlayer : Player.PlacePoliceLastSeenPlayer;

    private enum Task
    {
        Wander,
        GoTo,
        Nothing,
    }
    private Task CurrentTaskDynamic
    {
        get
        {
            if (HasReachedReportedPosition)
            {
                return Task.Wander;
            }
            else
            {
                return Task.GoTo;
            }
        }
    }
    public Locate(IComplexTaskable cop, ITargetable player, ISettingsProvideable settings) : base(player, cop, 1000)
    {
        Name = "Locate";
        SubTaskName = "";
        Settings = settings;
    }
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
            NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 10f);
            //EntryPoint.WriteToConsole($"TASKER: Locate Start: {Ped.Pedestrian.Handle}");
            //Ped.Pedestrian.BlockPermanentEvents = false;


            //if (Settings.SettingsManager.PoliceSettings.BlockEventsDuringLocate)
            //{
            //    Ped.Pedestrian.BlockPermanentEvents = true;
            //}
            //else
            //{
            //    Ped.Pedestrian.BlockPermanentEvents = false;
            //}
            //Ped.Pedestrian.KeepTasks = true;


            hasSixthSense = RandomItems.RandomPercent(Ped.IsInHelicopter ? Settings.SettingsManager.PoliceTaskSettings.SixthSenseHelicopterPercentage : Settings.SettingsManager.PoliceTaskSettings.SixthSensePercentage);
            if (!hasSixthSense && Ped.DistanceToPlayer <= 40f && RandomItems.RandomPercent(Settings.SettingsManager.PoliceTaskSettings.SixthSensePercentageClose))
            {
                hasSixthSense = true;
            }

            EntryPoint.WriteToConsole($"LOCATE TASK: Cop {Ped.Handle} hasSixthSense {hasSixthSense}");
            Update();
        }
    }
    public override void Update()
    {
        if (Ped.Pedestrian.Exists() && ShouldUpdate)
        {
            if (CurrentTask != CurrentTaskDynamic)
            {
                CurrentTask = CurrentTaskDynamic;
                //EntryPoint.WriteToConsole($"      Locate SubTask Changed: {Ped.Pedestrian.Handle} to {CurrentTask} {CurrentDynamic}");
                ExecuteCurrentSubTask();
            }
            else if (NeedsUpdates)
            {
                ExecuteCurrentSubTask();
            }
            SetVehicle();
        }
    }
    public override void ReTask()
    {

    }
    private void ExecuteCurrentSubTask()
    {
        if (CurrentTask == Task.Wander)
        {
            SubTaskName = "Wander";
            Wander();
        }
        else if (CurrentTask == Task.GoTo)
        {
            SubTaskName = "GoTo";
            GoTo();
        }
        GameTimeLastRan = Game.GameTime;
    }
    private void Wander()
    {
        NeedsUpdates = false;
        if (Ped.Pedestrian.Exists())
        {
            if (Ped.Pedestrian.IsInAnyVehicle(false) && Ped.Pedestrian.CurrentVehicle.Exists())
            {
                if (Ped.IsDriver)
                {
                    if (Settings.SettingsManager.PoliceTaskSettings.BlockEventsDuringVehicleLocate)
                    {
                        Ped.Pedestrian.BlockPermanentEvents = true;
                    }
                    else
                    {
                        Ped.Pedestrian.BlockPermanentEvents = false;
                    }
                    Ped.Pedestrian.KeepTasks = true;
                    NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 30f, (int)eCustomDrivingStyles.Code3, 10f);
                }
            }
            else
            {
                //Ped.Pedestrian.Tasks.Wander();
                if (Settings.SettingsManager.PoliceTaskSettings.BlockEventsDuringLocate)
                {
                    Ped.Pedestrian.BlockPermanentEvents = true;
                }
                else
                {
                    Ped.Pedestrian.BlockPermanentEvents = false;
                }
                Ped.Pedestrian.KeepTasks = true;
                NativeFunction.Natives.TASK_WANDER_STANDARD(Ped.Pedestrian, 0, 0);
            }
            //EntryPoint.WriteToConsole(string.Format("Locate Began SearchingPosition: {0}", Ped.Pedestrian.Handle),5);
        }
    }
    private void GoTo()
    {
        if (Ped.Pedestrian.Exists())
        {
            NeedsUpdates = true;
            if (CurrentTaskedPosition.DistanceTo2D(PlaceToGoTo) >= 5f && !HasReachedReportedPosition) //if (CurrentTaskedPosition.DistanceTo2D(Player.PlacePoliceLastSeenPlayer) >= 5f && !HasReachedReportedPosition)
            {
                HasReachedReportedPosition = false;
                CurrentTaskedPosition = PlaceToGoTo;// Player.PlacePoliceLastSeenPlayer;
                if (Ped.Pedestrian.IsInAnyVehicle(false))
                {
                    if (Ped.IsDriver)
                    {
                        if (Settings.SettingsManager.PoliceTaskSettings.BlockEventsDuringVehicleLocate)
                        {
                            Ped.Pedestrian.BlockPermanentEvents = true;
                        }
                        else
                        {
                            Ped.Pedestrian.BlockPermanentEvents = false;
                        }
                        Ped.Pedestrian.KeepTasks = true;
                        if (Ped.IsInHelicopter)
                        {
                            NativeFunction.Natives.TASK_HELI_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, 0, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 4, 50f, 150f, -1f, -1, 30, -1.0f, 0);//NativeFunction.Natives.TASK_HELI_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, 0, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 4, 50f, 10f, 0f, -1, -1, -1, 0);
                        }
                        else if (Ped.IsInBoat)
                        {
                            NativeFunction.Natives.TASK_BOAT_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, 0, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 4, 50f, (int)eCustomDrivingStyles.Code3, -1.0f, 7);
                        }
                        else
                        {
                            NativeFunction.Natives.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 70f, (int)eCustomDrivingStyles.Code3, 10f); //30f speed
                        }
                    }
                }
                else
                {
                    //Ped.Pedestrian.Tasks.GoStraightToPosition(CurrentTaskedPosition, 15f, 0f, 2f, 0);
                    if (Settings.SettingsManager.PoliceTaskSettings.BlockEventsDuringLocate)
                    {
                        Ped.Pedestrian.BlockPermanentEvents = true;
                    }
                    else
                    {
                        Ped.Pedestrian.BlockPermanentEvents = false;
                    }
                    Ped.Pedestrian.KeepTasks = true;
                    NativeFunction.Natives.TASK_GO_STRAIGHT_TO_COORD(Ped.Pedestrian, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 15f, -1, 0f, 0f);
                }
                //EntryPoint.WriteToConsole(string.Format("Locate Position Updated: {0}", Ped.Pedestrian.Handle),5);
            }
            float DistanceToCoordinates = Ped.Pedestrian.DistanceTo2D(CurrentTaskedPosition);
            if (Ped.Pedestrian.IsInAirVehicle)
            {
                if (DistanceToCoordinates <= 150f)
                {
                   NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 10f);
                }
                else
                {
                    NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 50f);
                    
                }
            }
            if (DistanceToCoordinates <= 25f)
            {
                if(hasSixthSense && Player.SearchMode.IsInStartOfSearchMode)
                {

                }
                else
                {
                    HasReachedReportedPosition = true;
                }
                
                EntryPoint.WriteToConsole($"LOCATE TASK: Cop {Ped.Handle} HAS REACHED POSITION");
            }
            if (Ped.IsDriver && !Ped.IsInHelicopter && !Ped.IsInBoat && Ped.DistanceToPlayer <= Settings.SettingsManager.PoliceTaskSettings.DriveBySightDuringLocateDistance && Settings.SettingsManager.PoliceTaskSettings.AllowDriveBySightDuringLocate)// && Player.CurrentLocation.IsOffroad && Player.CurrentLocation.HasBeenOffRoad)
            {
                if (!isSetCode3Close)
                {
                    NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.Code3Close);
                    isSetCode3Close = true;
                }
            }
            else
            {
                if (isSetCode3Close)
                {
                    NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.Code3);
                    isSetCode3Close = false;
                }
            }
        }
    }
    private void SetVehicle()
    {
        if (Ped.Pedestrian.Exists() && Ped.Pedestrian.CurrentVehicle.Exists() && Ped.Pedestrian.CurrentVehicle.HasSiren && !Ped.Pedestrian.CurrentVehicle.IsSirenOn)
        {
            Ped.Pedestrian.CurrentVehicle.IsSirenOn = true;
            Ped.Pedestrian.CurrentVehicle.IsSirenSilent = false;
        }
        if (Ped.IsInVehicle)
        {
            NativeFunction.Natives.SET_DRIVER_ABILITY(Ped.Pedestrian, Settings.SettingsManager.PoliceTaskSettings.DriverAbility);
            NativeFunction.Natives.SET_DRIVER_AGGRESSIVENESS(Ped.Pedestrian, Settings.SettingsManager.PoliceTaskSettings.DriverAggressiveness);
            if (Settings.SettingsManager.PoliceTaskSettings.DriverRacing > 0f)
            {
                NativeFunction.Natives.SET_DRIVER_RACING_MODIFIER(Ped.Pedestrian, Settings.SettingsManager.PoliceTaskSettings.DriverRacing);
            }
        }
    }
    public override void Stop()
    {

    }
}

