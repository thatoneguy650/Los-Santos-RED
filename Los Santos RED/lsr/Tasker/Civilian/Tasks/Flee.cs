using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Flee : ComplexTask
{
    private uint GameTimeStartedCallIn;
    private uint GameTimeStartedFlee;
    private bool HasStartedPhoneTask;
    private uint GameTimeToCallIn = 10000;
    private bool isInVehicle = false;
    private ITargetable Target;
    private bool isCowering = false;
    private ISettingsProvideable Settings;
    private uint GameTimeLastCheckedSurrender;

    private bool IsSetCower = false;

    private bool WillCowerBasedOnIntimidation = false;
    private uint GameTimeLastUpdatedCower;
    private bool IsWithinCowerDistance => Ped.DistanceToPlayer <= Ped.CowerDistance;
    //private bool ShouldCower => Ped.WillCower && IsWithinCowerDistance && !Player.RecentlyShot;
    private bool ShouldCallIn => Ped.HasCellPhone && (Ped.WillCallPolice || (Ped.WillCallPoliceIntense && Ped.PedReactions.HasSeenIntenseCrime));
    public Flee(IComplexTaskable ped, ITargetable player, ISettingsProvideable settings) : base(player, ped, 5000)
    {
        Name = "Flee";
        SubTaskName = "";
        Target = player;
        Settings = settings;
    }
    public override void Start()
    {
        if (!Ped.Pedestrian.Exists())
        {
            return;
        }
        NativeFunction.Natives.SET_PED_SHOULD_PLAY_IMMEDIATE_SCENARIO_EXIT(Ped.Pedestrian);
        isInVehicle = Ped.Pedestrian.IsInAnyVehicle(false);


        WillCowerBasedOnIntimidation = RandomItems.RandomPercent(Player.IntimidationManager.IntimidationPercent);

        EntryPoint.WriteToConsole($"FLEE START WillCower:{Ped.WillCower} WillCowerBasedOnIntimidation{WillCowerBasedOnIntimidation}");
        ReTask();
        GameTimeStartedFlee = Game.GameTime;
        GameTimeLastRan = Game.GameTime;    
        //if(ShouldCallIn)
        //{
            RunInterval = 1000;
        //}
    }
    public override void Update()
    {
        if (Ped.Pedestrian.Exists() && isInVehicle != Ped.Pedestrian.IsInAnyVehicle(false))
        {
            isInVehicle = Ped.Pedestrian.IsInAnyVehicle(false);
            ReTask();
        }
        if(isInVehicle && Ped.IsDriver)
        {
            NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.Vanilla_Alerted);
            NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 100f);//new
            NativeFunction.Natives.SET_DRIVER_ABILITY(Ped.Pedestrian, 1.0f);
            NativeFunction.Natives.SET_DRIVER_AGGRESSIVENESS(Ped.Pedestrian, 1.0f);
        }
        //if(ShouldCower != isCowering)
        //{
        //    ReTask();
        //}
        if (Game.GameTime - GameTimeLastUpdatedCower >= 4000)
        {
            WillCowerBasedOnIntimidation = RandomItems.RandomPercent(Player.IntimidationManager.IntimidationPercent);
            GameTimeLastUpdatedCower = Game.GameTime;
        }
        if (isCowering)
        {
            UpdateCowering();
        }
        else
        {
            UpdateFleeing();
        }
        CheckCallIn();      
        if(Ped.WantedLevel > 0)
        {
            HandleSurrendering();
        }  
        GameTimeLastRan = Game.GameTime;
    }

    private void UpdateFleeing()
    {
        if((Ped.WillCower || WillCowerBasedOnIntimidation) && IsWithinCowerDistance)
        {
            ReTask();
        }
    }

    private void UpdateCowering()
    {
        if(!IsWithinCowerDistance)
        {
            EntryPoint.WriteToConsole("PED FLEE: PED IS NOT WITHIN COWER DISTANCE RESETTING");
            ReTask();
           
        }
        if(!Ped.WillCower && Player.IntimidationManager.IntimidationPercent <= Settings.SettingsManager.PlayerOtherSettings.IntimidationMinBeforeCanFlee)
        {
            if (!RandomItems.RandomPercent(Player.IntimidationManager.IntimidationPercent))
            {
                WillCowerBasedOnIntimidation = false;
                EntryPoint.WriteToConsole("PED FLEE: INTIMIDATION LEVEL FELL TOO LOW RESETTING");
                ReTask();
            }
        }
    }

    private void HandleSurrendering()
    {
        if(!Ped.CanSurrender)
        {
            return;
        }
        if(Game.GameTime - GameTimeStartedFlee < 10000)
        {
            return;
        }
        if(!Ped.PedViolations.HasCopsAround)
        {
            return;
        }
        if(Game.GameTime - GameTimeLastCheckedSurrender >= 15000)
        {
            if(RandomItems.RandomPercent(Settings.SettingsManager.CivilianSettings.WantedPossibleSurrenderPercentage))
            {
                Ped.ShouldSurrender = true;
                EntryPoint.WriteToConsole("FLEEING PED SET TO SURRENDER");
            }
            GameTimeLastCheckedSurrender = Game.GameTime;
        }
    }
    public override void Stop()
    {

    }
    public override void ReTask()
    {
        if (!Ped.Pedestrian.Exists())
        {
            return;
        }
        Ped.Pedestrian.BlockPermanentEvents = true;
        Ped.Pedestrian.KeepTasks = true;
        isCowering = false;
        if (isInVehicle)
        {
            TaskVehicleFlee();
        }
        else
        {
            if ((Ped.WillCower || WillCowerBasedOnIntimidation) && IsWithinCowerDistance)
            {
                TaskCowerOnFoot();
                isCowering = true;
            }
            else
            {
                TaskFleeOnFoot();
            }
        }
    }
    private void CheckCallIn()
    {
       // EntryPoint.WriteToConsole("FLEE CHECK CALL IN!");
        if (!ShouldCallIn)
        {
           // EntryPoint.WriteToConsole("FLEE CHECK CALL IN! NOT CHECKING CALL IN");
            return;
        }
       // EntryPoint.WriteToConsole($"FLEE CHECK START1 {GameTimeStartedFlee} {GameTimeToCallIn}");
        if (!Ped.Pedestrian.Exists() && Settings.SettingsManager.CivilianSettings.AllowCallInIfPedDoesNotExist)
        {
            if (Settings.SettingsManager.CivilianSettings.AllowCallInIfPedDoesNotExist && Game.GameTime - GameTimeStartedCallIn >= Settings.SettingsManager.CivilianSettings.GameTimeToCallInIfPedDoesNotExist && (Ped.PlayerCrimesWitnessed.Any() || Ped.OtherCrimesWitnessed.Any() || Ped.PedAlerts.HasCrimeToReport))
            {
                //EntryPoint.WriteToConsole("NOT EXISTING PED CALLED IN CRIME");
                Ped.ReportCrime(Player);
            }
            return;
        }
        if (!Ped.Pedestrian.Exists() || !Ped.CanFlee || isCowering)
        {
            return;
        }
        if (!HasStartedPhoneTask && Game.GameTime - GameTimeStartedFlee >= GameTimeToCallIn)
        {
            EntryPoint.WriteToConsole($"FLEE START CALL {GameTimeStartedFlee} {GameTimeToCallIn}");
            TaskUsePhone();
        }
        if (HasStartedPhoneTask && Game.GameTime - GameTimeStartedCallIn >= GameTimeToCallIn + Settings.SettingsManager.CivilianSettings.GameTimeAfterCallInToReportCrime && (Ped.PlayerCrimesWitnessed.Any() || Ped.OtherCrimesWitnessed.Any() || Ped.PedAlerts.HasCrimeToReport))
        {
            EntryPoint.WriteToConsole("FLEE REPORT CRIME!");
            Ped.ReportCrime(Player);
            EntryPoint.WriteToConsole($"{Ped.Handle} CALLED IN CRIME");
            if (Ped.Pedestrian.Exists())
            {
                ReTask();
            }
        }
    }
    private void TaskUsePhone()
    {
        NativeFunction.CallByName<bool>("TASK_USE_MOBILE_PHONE_TIMED", Ped.Pedestrian, Settings.SettingsManager.CivilianSettings.GameTimeAfterCallInToReportCrime);
        HasStartedPhoneTask = true;
        Ped.PlaySpeech(new List<string>() { "GENERIC_SHOCKED_HIGH", "GENERIC_FRUSTRATED_HIGH", "GET_OUT_OF_HERE" }, false, false);
        EntryPoint.WriteToConsole($"{Ped.Handle} STARTED PHONE TASK");
    }
    private void TaskVehicleFlee()
    {
        Vector3 CurrentPos = Ped.Pedestrian.Position;
        Ped.IsCowering = false;
        if(Ped.Pedestrian.CurrentVehicle.Exists() && Ped.Pedestrian.CurrentVehicle.Handle == Player.CurrentVehicle?.Handle)
        {
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", 0, Ped.Pedestrian.CurrentVehicle, 27, 1000);
                NativeFunction.CallByName<bool>("TASK_LEAVE_VEHICLE", 0, Ped.Pedestrian.CurrentVehicle, Ped.DefaultEnterExitFlag | (int)eEnter_Exit_Vehicle_Flags.ECF_DONT_CLOSE_DOOR | (int)eEnter_Exit_Vehicle_Flags.ECF_DONT_WAIT_FOR_VEHICLE_TO_STOP);// 256);

                NativeFunction.CallByName<bool>("TASK_SMART_FLEE_COORD", 0, CurrentPos.X, CurrentPos.Y, CurrentPos.Z, 5000f, -1, true, false);

                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
        else
        {
            NativeFunction.CallByName<bool>("TASK_SMART_FLEE_COORD", Ped.Pedestrian, CurrentPos.X, CurrentPos.Y, CurrentPos.Z, 5000f, -1, true, false);
            NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.Vanilla_Alerted);
            NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 100f);//new
            NativeFunction.Natives.SET_DRIVER_ABILITY(Ped.Pedestrian, 1.0f);
            NativeFunction.Natives.SET_DRIVER_AGGRESSIVENESS(Ped.Pedestrian, 1.0f);
            EntryPoint.WriteToConsole("FLEE SET PED FLEE IN VEHICLE");
        }
        SubTaskName = "VehicleFlee";
    }
    private void TaskCowerOnFoot()
    {
        Ped.IsCowering = true;
        NativeFunction.Natives.TASK_COWER(Ped.Pedestrian, -1);
        EntryPoint.WriteToConsole("FLEE SET PED COWER");
        SubTaskName = "Cower";
    }
    private void TaskFleeOnFoot()
    {
        NativeFunction.Natives.SET_PED_SHOULD_PLAY_IMMEDIATE_SCENARIO_EXIT(Ped.Pedestrian);
        Ped.IsCowering = false;
        Vector3 CurrentPos = Ped.Pedestrian.Position;
        NativeFunction.CallByName<bool>("TASK_SMART_FLEE_COORD", Ped.Pedestrian, CurrentPos.X, CurrentPos.Y, CurrentPos.Z, 5000f, -1, true, false);
        EntryPoint.WriteToConsole("FLEE SET PED FLEE");
        SubTaskName = "FootFlee";
    }
}

