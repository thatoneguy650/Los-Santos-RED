using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CalmCallIn : ComplexTask
{
    private uint GameTimeStartedCallIn;
    private uint GameTimeStartedFlee;
    private bool HasStartedPhoneTask;
    private uint GameTimeToCallIn = 10000;
    private ISettingsProvideable Settings;
    public CalmCallIn(IComplexTaskable ped, ITargetable player, ISettingsProvideable settings) : base(player, ped, 1000)
    {
        Name = "CalmCallIn";
        SubTaskName = "";
        Settings = settings;
        GameTimeStartedCallIn = Game.GameTime;
    }
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
            GameTimeToCallIn = RandomItems.GetRandomNumber(Settings.SettingsManager.CivilianSettings.GameTimeToCallInMinimum, Settings.SettingsManager.CivilianSettings.GameTimeToCallInMaximum);
            TaskMoveAway();
            GameTimeStartedFlee = Game.GameTime;
            HasStartedPhoneTask = false;
        }
        GameTimeLastRan = Game.GameTime;
    }
    private void TaskMoveAway()
    {
        //EntryPoint.WriteToConsole($"TASKER: CalmCallIn Start: {Ped.Pedestrian.Handle}", 3);
        //unsafe
        //{
        //    int lol = 0;
        //    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
        //    NativeFunction.CallByName<bool>("TASK_USE_MOBILE_PHONE_TIMED", 0, 10000);
        //    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
        //    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
        //    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
        //    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        //}
    }
    public override void Update()
    {

        if (!Ped.Pedestrian.Exists() && Settings.SettingsManager.CivilianSettings.AllowCallInIfPedDoesNotExist)
        {
            if (Settings.SettingsManager.CivilianSettings.AllowCallInIfPedDoesNotExist && Game.GameTime - GameTimeStartedCallIn >= Settings.SettingsManager.CivilianSettings.GameTimeToCallInIfPedDoesNotExist && (Ped.PlayerCrimesWitnessed.Any() || Ped.OtherCrimesWitnessed.Any() || Ped.HasSeenDistressedPed))
            {
                EntryPoint.WriteToConsole("NOT EXISTING PED CALLED IN CRIME");
                Ped.ReportCrime(Player);
            }
            return;
        }
        if (!Ped.Pedestrian.Exists() || !Ped.CanFlee)
        {
            return;
        }
        if (!HasStartedPhoneTask && Game.GameTime - GameTimeStartedFlee >= GameTimeToCallIn)
        {
            NativeFunction.CallByName<bool>("TASK_USE_MOBILE_PHONE_TIMED", Ped.Pedestrian, Settings.SettingsManager.CivilianSettings.GameTimeAfterCallInToReportCrime);
            HasStartedPhoneTask = true;
            Ped.PlaySpeech(new List<string>() { "GENERIC_SHOCKED_MED", "GENERIC_FRUSTRATED_HIGH", "GET_OUT_OF_HERE" }, false, false);
            EntryPoint.WriteToConsole($"{Ped.Handle} STARTED PHONE TASK");
        }
        if (HasStartedPhoneTask && Game.GameTime - GameTimeStartedCallIn >= GameTimeToCallIn + Settings.SettingsManager.CivilianSettings.GameTimeAfterCallInToReportCrime && (Ped.PlayerCrimesWitnessed.Any() || Ped.OtherCrimesWitnessed.Any() || Ped.HasSeenDistressedPed))
        {
            Ped.ReportCrime(Player);
            EntryPoint.WriteToConsole($"{Ped.Handle} CALLED IN CRIME");
        }




        //if (Ped.Pedestrian.Exists())
        //{
        //    if (Game.GameTime - GameTimeStartedCallIn >= 10000 && (Ped.PlayerCrimesWitnessed.Any() || Ped.OtherCrimesWitnessed.Any() || Ped.HasSeenDistressedPed))
        //    {
        //        Ped.ReportCrime(Player);
        //    }
        //}
        //else
        //{
        //    if (Game.GameTime - GameTimeStartedCallIn >= 4000 && (Ped.PlayerCrimesWitnessed.Any() || Ped.OtherCrimesWitnessed.Any() || Ped.HasSeenDistressedPed))
        //    {
        //        Ped.ReportCrime(Player);
        //    }
        //}





        GameTimeLastRan = Game.GameTime;
    }
    public override void Stop()
    {

    }
    public override void ReTask()
    {

    }
 
}

