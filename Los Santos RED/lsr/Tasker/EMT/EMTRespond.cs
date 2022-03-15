using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class EMTRespond : ComplexTask
{
    private uint GameTimeStartedTreatingVictim;
    private bool NeedsUpdates;
    private Vector3 CurrentTaskedPosition;
    private Task CurrentTask = Task.Nothing;
    private bool HasReachedReportedPosition;
    private bool HasReachedVictim;
    //public PedExt TargetPed { get; set; }
    private uint GameTimeLastSpoke;
    private enum Task
    {
        Wander,
        GoTo,
        Nothing,
        ExitVehicle,
        TreatVictim,
    }
    private Task CurrentTaskDynamic
    {
        get
        {
            if(HasReachedReportedPosition)
            {
                return Task.Wander;
            }
            return Task.GoTo;
        }
    }
    public EMTRespond(IComplexTaskable cop, ITargetable player) : base(player, cop, 1000)
    {
        Name = "EMTRespond";
        SubTaskName = "";
    }
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
            NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 10f);
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
                ExecuteCurrentSubTask();
            }
            else if (NeedsUpdates)
            {
                ExecuteCurrentSubTask();
            }
            SetSiren();
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
        else if (CurrentTask == Task.ExitVehicle)
        {
            RunInterval = 200;
            SubTaskName = "ExitVehicle";
            ExitVehicle();
        }
        GameTimeLastRan = Game.GameTime;
    }
    private void ExitVehicle()
    {
        NeedsUpdates = false;
        Ped.Pedestrian.BlockPermanentEvents = true;
        Ped.Pedestrian.KeepTasks = true;
        if (Ped.Pedestrian.Exists() && Ped.Pedestrian.CurrentVehicle.Exists() && OtherTarget != null && OtherTarget.Pedestrian.Exists())
        {
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", 0, Ped.Pedestrian.CurrentVehicle, 27, 1000);
                NativeFunction.CallByName<bool>("TASK_LEAVE_VEHICLE", 0, Ped.Pedestrian.CurrentVehicle, 64);
                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, OtherTarget.Pedestrian, -1, 3f, 4.4f, 1073741824, 1); //Original and works ok
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
    }
    private void Wander()
    {
        if (Ped.Pedestrian.Exists())
        {
            NeedsUpdates = false;
            if (Ped.Pedestrian.IsInAnyVehicle(false) && Ped.Pedestrian.CurrentVehicle.Exists())
            {
                Ped.Pedestrian.BlockPermanentEvents = true;
                Ped.Pedestrian.KeepTasks = true;
                //4 | 16 | 32 | 262144
                NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 12f, (int)eCustomDrivingStyles.FastEmergency, 10f);
            }
            else
            {
                Ped.Pedestrian.BlockPermanentEvents = true;
                Ped.Pedestrian.KeepTasks = true;
                //Ped.Pedestrian.Tasks.Wander();
                //NativeFunction.Natives.TASK_WANDER_STANDARD(Ped.Pedestrian, 0, 0);
                Vector3 Pos = Ped.Pedestrian.Position;
                NativeFunction.Natives.TASK_WANDER_IN_AREA(Ped.Pedestrian, Pos.X, Pos.Y, Pos.Z, 45f, 0f, 0f);
            }
            //EntryPoint.WriteToConsole(string.Format("TASKER: Investigation Began SearchingPosition: {0}", Ped.Pedestrian.Handle),5);
        }
    }
    private void GoTo()
    {
        if (Ped.Pedestrian.Exists())
        {
            NeedsUpdates = true;
            if (Player.Investigation.IsActive && Player.Investigation.RequiresEMS && CurrentTaskedPosition.DistanceTo2D(Player.Investigation.Position) >= 5f)
            {
                HasReachedReportedPosition = false;
                CurrentTaskedPosition = Player.Investigation.Position;
                UpdateGoTo();
                EntryPoint.WriteToConsole(string.Format("TASKER: EMTRespond Position Updated 1: {0}", Ped.Pedestrian.Handle), 5);
            }
            float DistanceTo = Ped.Pedestrian.DistanceTo2D(CurrentTaskedPosition);
            if (DistanceTo <= 25f)
            {
                HasReachedReportedPosition = true;
                EntryPoint.WriteToConsole(string.Format("TASKER: EMTRespond Position Reached: {0}", Ped.Pedestrian.Handle), 5);
            }
            else if (DistanceTo < 50f)
            {
                UpdateGoTo();
                EntryPoint.WriteToConsole(string.Format("TASKER: EMTRespond Position Near: {0}", Ped.Pedestrian.Handle), 5);
            }
        }
    }
    private void UpdateGoTo()
    {
        if (Ped.Pedestrian.Exists())
        {
            if (Ped.Pedestrian.IsInAnyVehicle(false))
            {
                if (Ped.IsDriver && Ped.Pedestrian.CurrentVehicle.Exists())// && Ped.Pedestrian.SeatIndex == -1)
                {
                    Ped.Pedestrian.BlockPermanentEvents = true;
                    Ped.Pedestrian.KeepTasks = true;
                    if (Ped.Pedestrian.DistanceTo2D(CurrentTaskedPosition) >= 50f)
                    {
                        NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 20f, (int)eCustomDrivingStyles.FastEmergency, 20f);
                    }
                    else
                    {
                        NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 12f, (int)eCustomDrivingStyles.FastEmergency, 20f); //NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 15f, (int)VehicleDrivingFlags.Normal, 20f);
                    }
                    EntryPoint.WriteToConsole(string.Format("TASKER: EMTRespond UpdateGoTo Driver: {0}", Ped.Pedestrian.Handle), 5);
                }
            }
            else
            {
                Ped.Pedestrian.BlockPermanentEvents = true;
                Ped.Pedestrian.KeepTasks = true;
                NativeFunction.Natives.TASK_FOLLOW_NAV_MESH_TO_COORD(Ped.Pedestrian, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 2.0f, -1, 5f, true, 0f);
            }
        }
    }
    private void SetSiren()
    {
        if (Ped.Pedestrian.Exists() && Ped.Pedestrian.CurrentVehicle.Exists() && Ped.Pedestrian.CurrentVehicle.HasSiren && !Ped.Pedestrian.CurrentVehicle.IsSirenOn)
        {
            Ped.Pedestrian.CurrentVehicle.IsSirenOn = true;
            Ped.Pedestrian.CurrentVehicle.IsSirenSilent = false;
        }
    }
    public override void Stop()
    {

    }

    private bool SayAvailableAmbient(Ped ToSpeak, List<string> Possibilities, bool WaitForComplete, bool isPlayer)
    {
        bool Spoke = false;

        foreach (string AmbientSpeech in Possibilities.OrderBy(x => RandomItems.MyRand.Next()))
        {

            if (Ped.VoiceName != "")
            {
                ToSpeak.PlayAmbientSpeech(Ped.VoiceName, AmbientSpeech, 0, SpeechModifier.Force);
            }
            else
            {
                ToSpeak.PlayAmbientSpeech(null, AmbientSpeech, 0, SpeechModifier.Force);
            }
            

            GameFiber.Sleep(300);//100
            if (ToSpeak.IsAnySpeechPlaying)
            {
                Spoke = true;
            }
            EntryPoint.WriteToConsole($"SAYAMBIENTSPEECH: {ToSpeak.Handle} Attempting {AmbientSpeech}, Result: {Spoke}", 5);
            if (Spoke)
            {
                break;
            }
        }
        GameFiber.Sleep(100);
        while (ToSpeak.IsAnySpeechPlaying && WaitForComplete )
        {
            Spoke = true;
            GameFiber.Yield();
        }
        if (!Spoke)
        {
            Game.DisplayNotification($"\"{Possibilities.FirstOrDefault()}\"");
        }
        
        return Spoke;
    }

}

