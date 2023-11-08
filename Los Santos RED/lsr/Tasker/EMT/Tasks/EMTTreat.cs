using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class EMTTreat : ComplexTask
{
    private uint GameTimeStartedTreatingVictim;
    private bool NeedsUpdates;
    private Vector3 CurrentTaskedPosition;
    private Task CurrentTask = Task.Nothing;
    private bool HasReachedReportedPosition;
    private bool HasReachedVictim;
    //public PedExt TargetPed { get; set; }
    private uint GameTimeLastSpoke;
    private int TimeToSpeak = 5000;
    private uint GameTimeFinishedTreatingVictim;
    private ISettingsProvideable Settings;

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
            if (OtherTarget != null && OtherTarget.Pedestrian.Exists())
            {
                if (HasReachedVictim && !Ped.IsInVehicle)
                {
                    return Task.TreatVictim;
                }
                else if (HasReachedReportedPosition)
                {
                    return Task.ExitVehicle;
                }
                else
                {
                    return Task.GoTo;
                }
            }
            else
            {
                return Task.Wander;
            }
        }
    }
    public EMTTreat(IComplexTaskable cop, ITargetable player, PedExt targetPed, ISettingsProvideable settings) : base(player, cop, 1000)
    {
        Name = "EMTTreat";
        SubTaskName = "";
        OtherTarget = targetPed;
        Settings = settings;
    }
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
            if (!NativeFunction.CallByName<bool>("HAS_ANIM_SET_LOADED", "move_m@drunk@verydrunk"))
            {
                NativeFunction.CallByName<bool>("REQUEST_ANIM_SET", "move_m@drunk@verydrunk");
            }
            //NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 10f);////tr cruise speed test
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
        if (!HasReachedVictim && !Ped.IsInVehicle && Ped.Pedestrian.Exists() && OtherTarget != null && OtherTarget.Pedestrian.Exists())
        {
            if (Ped.Pedestrian.DistanceTo2D(OtherTarget.Pedestrian.Position) <= 5f)
            {
                OtherTarget.HasStartedEMTTreatment = true;
                //EntryPoint.WriteToConsoleTestLong("EMT REACHED VICTIM");
                HasReachedVictim = true;
                GameTimeStartedTreatingVictim = Game.GameTime;
            }
        }
        if (HasReachedVictim && !Ped.IsInVehicle && GameTimeStartedTreatingVictim != 0 && OtherTarget != null && !OtherTarget.HasBeenTreatedByEMTs)
        {
            if (Game.GameTime - GameTimeLastSpoke >= TimeToSpeak)
            {
                TimeToSpeak = RandomItems.GetRandomNumberInt(5000, 10000);
                SayAvailableAmbient(Ped.Pedestrian, new List<string>() { "GENERIC_SHOCKED_MED" }, false, false);
                GameTimeLastSpoke = Game.GameTime;
            }

        }
        if (Ped.Pedestrian.Exists() && HasReachedVictim && Game.GameTime - GameTimeStartedTreatingVictim >= 15000 && OtherTarget != null && GameTimeFinishedTreatingVictim == 0)
        {
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "amb@medic@standing@tendtodead@exit", "exit", 8.0f, -8.0f, -1, 0, 0, false, false, false);
            GameTimeFinishedTreatingVictim = Game.GameTime;

        }
        else if(GameTimeFinishedTreatingVictim != 0 && Game.GameTime - GameTimeFinishedTreatingVictim >= 2000)
        {
            if (OtherTarget != null)
            {
                if(OtherTarget.OnTreatedByEMT(Settings.SettingsManager.EMSSettings.RevivePercentage))//true if died, w/.e
                {
                    Ped.PlaySpeech("GENERIC_SHOCKED_HIGH", false);
                }
            }
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
        else if (CurrentTask == Task.TreatVictim)
        {
            RunInterval = 200;
            SubTaskName = "TreatVictim";
            TreatVictim();
        }
        GameTimeLastRan = Game.GameTime;
    }

    private void GoTo()
    {
        if (Ped.Pedestrian.Exists())
        {
            NeedsUpdates = true;
            if (OtherTarget != null && OtherTarget.Pedestrian.Exists() && CurrentTaskedPosition.DistanceTo2D(OtherTarget.Pedestrian.Position) >= 5f)
            {
                HasReachedReportedPosition = false;
                CurrentTaskedPosition = OtherTarget.Pedestrian.Position;
                UpdateGoTo();
               // EntryPoint.WriteToConsole(string.Format("TASKER: EMTRespond Position Updated 2: {0}", Ped.Pedestrian.Handle), 5);
            }
            float DistanceTo = Ped.Pedestrian.DistanceTo2D(CurrentTaskedPosition);
            if (DistanceTo <= 25f)//25f
            {
                HasReachedReportedPosition = true;
               // EntryPoint.WriteToConsole(string.Format("TASKER: EMTRespond Position Reached: {0}", Ped.Pedestrian.Handle), 5);
            }
            else if (DistanceTo < 50f)
            {
                UpdateGoTo();
               // EntryPoint.WriteToConsole(string.Format("TASKER: EMTRespond Position Near: {0}", Ped.Pedestrian.Handle), 5);
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
                        NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 20f, (int)eCustomDrivingStyles.Code3, 20f);
                    }
                    else
                    {
                        NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 12f, (int)eCustomDrivingStyles.Code3, 20f); //NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 15f, (int)VehicleDrivingFlags.Normal, 20f);
                    }
                   // EntryPoint.WriteToConsole(string.Format("TASKER: EMTRespond UpdateGoTo Driver: {0}", Ped.Pedestrian.Handle), 5);
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
                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, OtherTarget.Pedestrian, -1, 5f, 2.0f, 1073741824, 1); //Original and works ok
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
    }
    private void TreatVictim()
    {

        NeedsUpdates = false;
        Ped.Pedestrian.BlockPermanentEvents = true;
        Ped.Pedestrian.KeepTasks = true;
        if (Ped.Pedestrian.Exists() && OtherTarget != null && OtherTarget.Pedestrian.Exists())
        {
            //EntryPoint.WriteToConsoleTestLong("EMT TREAT VICTIM RAN");
            AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@enter");
            AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@base");
            AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@exit");
            AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@idle_a");

            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                 NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, OtherTarget.Pedestrian, -1, 1.75f, 0.75f, 1073741824, 1); //Original and works ok
                NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, OtherTarget.Pedestrian, 1000);
                //NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, OtherTarget.Pedestrian, 500, 0, 2);
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "amb@medic@standing@tendtodead@enter", "enter", 8.0f, -8.0f, -1, 0, 0, false, false, false);
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "amb@medic@standing@tendtodead@idle_a", "idle_a", 8.0f, -8.0f, -1, 0, 0, false, false, false);
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "amb@medic@standing@tendtodead@idle_a", "idle_b", 8.0f, -8.0f, -1, 0, 0, false, false, false);
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "amb@medic@standing@tendtodead@idle_a", "idle_c", 8.0f, -8.0f, -1, 0, 0, false, false, false);
                //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "amb@medic@standing@tendtodead@exit", "exit", 8.0f, -8.0f, -1, 0, 0, false, false, false);
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
                NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 12f, (int)eCustomDrivingStyles.Code3, 10f);
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

    private void SetSiren()
    {
        if (Ped.Pedestrian.Exists() && Ped.Pedestrian.CurrentVehicle.Exists() && Ped.IsDriver && Ped.Pedestrian.CurrentVehicle.HasSiren && !Ped.Pedestrian.CurrentVehicle.IsSirenOn)
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
            //EntryPoint.WriteToConsole($"SAYAMBIENTSPEECH: {ToSpeak.Handle} Attempting {AmbientSpeech}, Result: {Spoke}", 5);
            if (Spoke)
            {
                break;
            }
        }
        GameFiber.Sleep(100);
        while (ToSpeak.IsAnySpeechPlaying && WaitForComplete)
        {
            Spoke = true;
            GameFiber.Yield();
        }
        //if (!Spoke)
        //{
        //    Game.DisplayNotification($"\"{Possibilities.FirstOrDefault()}\"");
        //}

        return Spoke;
    }

}

