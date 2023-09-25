//using LosSantosRED.lsr.Interface;
//using Rage;
//using Rage.Native;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


//public class ScaredCallIn : ComplexTask
//{
//    private uint GameTimeStartedCallIn;
//    private uint GameTimeStartedFlee;
//    private bool HasStartedPhoneTask;
//    private uint GameTimeToCallIn = 10000;
//    private ISettingsProvideable Settings;
//    private bool isCowering = false;
//    private bool IsWithinCowerDistance => Ped.DistanceToPlayer <= Ped.CowerDistance;
//    private bool ShouldCower => Ped.WillCower && IsWithinCowerDistance && !Player.RecentlyShot;
//    public ScaredCallIn(IComplexTaskable ped, ITargetable player, ISettingsProvideable settings) : base(player, ped, 1000)
//    {
//        Name = "ScaredCallIn";
//        SubTaskName = "";
//        GameTimeStartedCallIn = Game.GameTime;
//        Settings = settings;
//    }
//    public override void Start()
//    {
//        if (Ped.Pedestrian.Exists())
//        {
//            GameTimeToCallIn = RandomItems.GetRandomNumber(Settings.SettingsManager.CivilianSettings.GameTimeToCallInMinimum, Settings.SettingsManager.CivilianSettings.GameTimeToCallInMaximum);
//            ReTask();
//            GameTimeStartedFlee = Game.GameTime;
//            HasStartedPhoneTask = false;
//        }
//        GameTimeLastRan = Game.GameTime;
//    }

//    public override void Update()
//    {
//        CheckCallIn();
//        NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.Code3);
//        if (ShouldCower != isCowering)
//        {
//            ReTask();
//        }
//        GameTimeLastRan = Game.GameTime;
//    }

//    private void CheckCallIn()
//    {
//        if (!Ped.Pedestrian.Exists() && Settings.SettingsManager.CivilianSettings.AllowCallInIfPedDoesNotExist)
//        {
//            if (Settings.SettingsManager.CivilianSettings.AllowCallInIfPedDoesNotExist && Game.GameTime - GameTimeStartedCallIn >= Settings.SettingsManager.CivilianSettings.GameTimeToCallInIfPedDoesNotExist && (Ped.PlayerCrimesWitnessed.Any() || Ped.OtherCrimesWitnessed.Any() || Ped.PedAlerts.HasCrimeToReport))
//            {
//                EntryPoint.WriteToConsole("NOT EXISTING PED CALLED IN CRIME");
//                Ped.ReportCrime(Player);
//            }
//            return;
//        }
//        if (!Ped.Pedestrian.Exists() || !Ped.CanFlee)
//        {
//            return;
//        }
//        if (!HasStartedPhoneTask && Game.GameTime - GameTimeStartedFlee >= GameTimeToCallIn)
//        {
//            NativeFunction.CallByName<bool>("TASK_USE_MOBILE_PHONE_TIMED", Ped.Pedestrian, Settings.SettingsManager.CivilianSettings.GameTimeAfterCallInToReportCrime);
//            HasStartedPhoneTask = true;
//            Ped.PlaySpeech(new List<string>() { "GENERIC_SHOCKED_HIGH", "GENERIC_FRUSTRATED_HIGH", "GET_OUT_OF_HERE" }, false, false);
//            EntryPoint.WriteToConsole($"{Ped.Handle} STARTED PHONE TASK");
//        }
//        if (HasStartedPhoneTask && Game.GameTime - GameTimeStartedCallIn >= GameTimeToCallIn + Settings.SettingsManager.CivilianSettings.GameTimeAfterCallInToReportCrime && (Ped.PlayerCrimesWitnessed.Any() || Ped.OtherCrimesWitnessed.Any() || Ped.PedAlerts.HasCrimeToReport))
//        {
//            Ped.ReportCrime(Player);
//            EntryPoint.WriteToConsole($"{Ped.Handle} CALLED IN CRIME");
//            if (Ped.Pedestrian.Exists())
//            {
//                TaskFlee();
//            }
//        }
//    }
//    public override void Stop()
//    {

//    }
//    public override void ReTask()
//    {
//        if (OtherTarget != null && OtherTarget.Pedestrian.Exists())
//        {
//            NativeFunction.CallByName<bool>("TASK_SMART_FLEE_PED", 0, OtherTarget.Pedestrian, 1500f, -1);
//            //unsafe
//            //{
//            //    int lol = 0;
//            //    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
//            //    NativeFunction.CallByName<bool>("TASK_SMART_FLEE_PED", 0, OtherTarget.Pedestrian, 50f, 10000);//100f
//            //    NativeFunction.CallByName<bool>("TASK_USE_MOBILE_PHONE_TIMED", 0, 5000);
//            //    NativeFunction.CallByName<bool>("TASK_SMART_FLEE_PED", 0, OtherTarget.Pedestrian, 1500f, -1);
//            //    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
//            //    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
//            //    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
//            //    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
//            //}
//        }
//        else
//        {
//            NativeFunction.CallByName<bool>("TASK_SMART_FLEE_PED", Ped.Pedestrian, Player.Character, 1500f, -1);
//            //unsafe
//            //{
//            //    int lol = 0;
//            //    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
//            //    NativeFunction.CallByName<bool>("TASK_SMART_FLEE_PED", 0, Player.Character, 50f, 10000);//100f
//            //    NativeFunction.CallByName<bool>("TASK_USE_MOBILE_PHONE_TIMED", 0, 5000);
//            //    NativeFunction.CallByName<bool>("TASK_SMART_FLEE_PED", 0, Player.Character, 1500f, -1);
//            //    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
//            //    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
//            //    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
//            //    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
//            //}
//        }
//    }


//    private void TaskFlee()
//    {

//    }

    
//}

