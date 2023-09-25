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
            ReTask();
            GameTimeStartedFlee = Game.GameTime;
            HasStartedPhoneTask = false;
        }
        GameTimeLastRan = Game.GameTime;
    }
    public override void Update()
    {
        if (!Ped.Pedestrian.Exists() && Settings.SettingsManager.CivilianSettings.AllowCallInIfPedDoesNotExist)
        {
            if (Settings.SettingsManager.CivilianSettings.AllowCallInIfPedDoesNotExist && Game.GameTime - GameTimeStartedCallIn >= Settings.SettingsManager.CivilianSettings.GameTimeToCallInIfPedDoesNotExist && (Ped.PlayerCrimesWitnessed.Any() || Ped.OtherCrimesWitnessed.Any() || Ped.PedAlerts.HasCrimeToReport))
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
            TaskCallIn();
        }
        if (HasStartedPhoneTask && Game.GameTime - GameTimeStartedCallIn >= GameTimeToCallIn + Settings.SettingsManager.CivilianSettings.GameTimeAfterCallInToReportCrime && (Ped.PlayerCrimesWitnessed.Any() || Ped.OtherCrimesWitnessed.Any() || Ped.PedAlerts.HasCrimeToReport))
        {
            Ped.ReportCrime(Player);
            EntryPoint.WriteToConsole($"{Ped.Handle} CALLED IN CRIME");
        }
        GameTimeLastRan = Game.GameTime;
    }
    public override void Stop()
    {

    }
    public override void ReTask()
    {

    } 
    private void TaskCallIn()
    {
        NativeFunction.CallByName<bool>("TASK_USE_MOBILE_PHONE_TIMED", Ped.Pedestrian, Settings.SettingsManager.CivilianSettings.GameTimeAfterCallInToReportCrime);
        HasStartedPhoneTask = true;
        Ped.PlaySpeech(new List<string>() { "GENERIC_SHOCKED_MED", "GENERIC_FRUSTRATED_HIGH", "GET_OUT_OF_HERE" }, false, false);
        EntryPoint.WriteToConsole($"{Ped.Handle} STARTED PHONE TASK");
    }
}

