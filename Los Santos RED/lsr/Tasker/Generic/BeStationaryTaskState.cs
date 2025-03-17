using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class BeStationaryTaskState : TaskState
{
    private PedExt PedGeneral;
    private IEntityProvideable World;
    private SeatAssigner SeatAssigner;

    private VehicleExt TaskedVehicle;
    private int TaskedSeat;
    private ISettingsProvideable Settings;
    private ITargetable Player;
    private bool BlockPermanentEvents = false;
    private Vector3 PlaceToDriveTo;
    private bool isSetCode3Close;
    private ILocationReachable LocationReachable;
    private uint GametimeLastRetasked;

    public BeStationaryTaskState(PedExt pedGeneral, ITargetable player, IEntityProvideable world, ISettingsProvideable settings, bool blockPermanentEvents)
    {
        PedGeneral = pedGeneral;
        Player = player;
        World = world;
        Settings = settings;
        BlockPermanentEvents = blockPermanentEvents;
    }
    public bool IsValid => PedGeneral != null && PedGeneral.Pedestrian.Exists() && PedGeneral.Pedestrian.Speed <= 0.5f;
    public string DebugName => $"BeStationaryTaskState";
    public void Dispose()
    {
        Stop();
    }
    public void Start()
    {
        PedGeneral.ClearTasks(true);
        TaskEntry();
    }
    public void Stop()
    {
        PedGeneral.ClearTasks(true);
    }
    public void Update()
    {
        if (!PedGeneral.Pedestrian.Exists())
        {
            return;
        }
        CheckTasks();
    }

    private void CheckTasks()
    {
        Rage.TaskStatus taskStatus = PedGeneral.Pedestrian.Tasks.CurrentTaskStatus;
        if (PedGeneral.IsDriver && (taskStatus == Rage.TaskStatus.NoTask || taskStatus == Rage.TaskStatus.Preparing) && Game.GameTime - GametimeLastRetasked >= 2000)
        {
            //PedGeneral.ClearTasks(true);
            TaskEntry();
            GametimeLastRetasked = Game.GameTime;
            EntryPoint.WriteToConsole($"BE STATIONARY TASKED {PedGeneral?.Handle} RETASKED");
        }
    }

    private void TaskEntry()
    {
        if (!PedGeneral.Pedestrian.Exists())
        {
            return;
        }
        if (BlockPermanentEvents)
        {
            PedGeneral.Pedestrian.BlockPermanentEvents = true;
            PedGeneral.Pedestrian.KeepTasks = true;
        }
        bool pedExists = PedGeneral != null && PedGeneral.Pedestrian.Exists();
        bool pedVehicleExists = PedGeneral != null && PedGeneral.Pedestrian.Exists() && PedGeneral.Pedestrian.CurrentVehicle.Exists();

        if(pedVehicleExists)
        {
            NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, 24, 90000);


            //NativeFunction.Natives.TASK_VEHICLE_DRIVE_TO_COORD(PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, PedGeneral.Pedestrian.Position.X, PedGeneral.Pedestrian.Position.Y, PedGeneral.Pedestrian.Position.Z, 1.0f, 0, "DUMMY_MODEL_FOR_SCRIPT", (int)eCustomDrivingStyles.RegularDriving, 5.0f, -1);

            EntryPoint.WriteToConsole("I AM ASSIGING THE BeStationaryTaskState ");
        }
        GametimeLastRetasked = Game.GameTime;
    }
 //   private enum TEMPACTION
 //   {
 //       TEMPACT_NONE = 0,
 //   TEMPACT_WAIT,
 //   TEMPACT_EMPTYTOBEREUSED,
 //   TEMPACT_REVERSE,
 //   TEMPACT_HANDBRAKETURNLEFT,
 //   TEMPACT_HANDBRAKETURNRIGHT,
 //   TEMPACT_HANDBRAKESTRAIGHT,
 //   TEMPACT_TURNLEFT,
 //   TEMPACT_TURNRIGHT,
 //   TEMPACT_GOFORWARD,
 //   TEMPACT_SWERVELEFT,
 //   TEMPACT_SWERVERIGHT,
 //   TEMPACT_EMPTYTOBEREUSED2,
 //   TEMPACT_REVERSE_LEFT,
 //   TEMPACT_REVERSE_RIGHT,
 //   TEMPACT_PLANE_FLY_UP,
 //   TEMPACT_PLANE_FLY_STRAIGHT,
 //   TEMPACT_PLANE_SHARP_LEFT,
 //   TEMPACT_PLANE_SHARP_RIGHT,
 //   TEMPACT_HEADON_COLLISION,
 //   TEMPACT_SWERVELEFT_STOP,
 //   TEMPACT_SWERVERIGHT_STOP,
 //   TEMPACT_REVERSE_STRAIGHT,
 //   TEMPACT_BOOST_USE_STEERING_ANGLE,
 //   TEMPACT_BRAKE,
 //   TEMPACT_HANDBRAKETURNLEFT_INTELLIGENT,
	//TEMPACT_HANDBRAKETURNRIGHT_INTELLIGENT,
	//TEMPACT_HANDBRAKESTRAIGHT_INTELLIGENT,
	//TEMPACT_REVERSE_STRAIGHT_HARD,
	//TEMPACT_EMPTYTOBEREUSED3,
	//TEMPACT_BURNOUT,
	//TEMPACT_REV_ENGINE,
	//TEMPACT_GOFORWARD_HARD,
	//TEMPACT_SURFACE_SUBMARINE
 //   }
}

