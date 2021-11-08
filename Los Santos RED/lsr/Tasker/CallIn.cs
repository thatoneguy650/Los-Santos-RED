using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CallIn : ComplexTask
{
    private uint GameTimeStartedCallIn;
    public CallIn(IComplexTaskable ped, ITargetable player) : base(player, ped, 1000)
    {
        Name = "CallIn";
        SubTaskName = "";
        GameTimeStartedCallIn = Game.GameTime;
    }
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
            EntryPoint.WriteToConsole($"TASKER: CallIn Start: {Ped.Pedestrian.Handle}", 3);
            if (OtherTarget != null && OtherTarget.Pedestrian.Exists())
            {
                unsafe
                {
                    int lol = 0;
                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                    NativeFunction.CallByName<bool>("TASK_SMART_FLEE_PED", 0, OtherTarget.Pedestrian, 50f, 10000);//100f
                    NativeFunction.CallByName<bool>("TASK_USE_MOBILE_PHONE_TIMED", 0, 5000);
                    NativeFunction.CallByName<bool>("TASK_SMART_FLEE_PED", 0, OtherTarget.Pedestrian, 100f, -1);
                    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                }
            }
            else
            {
                unsafe
                {
                    int lol = 0;
                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                    NativeFunction.CallByName<bool>("TASK_SMART_FLEE_PED", 0, Player.Character, 50f, 10000);//100f
                    NativeFunction.CallByName<bool>("TASK_USE_MOBILE_PHONE_TIMED", 0, 5000);
                    NativeFunction.CallByName<bool>("TASK_SMART_FLEE_PED", 0, Player.Character, 100f, -1);
                    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                }
            }
        }
        GameTimeLastRan = Game.GameTime;
    }
    public override void Update()
    {
        if (Ped.Pedestrian.Exists())
        {
            if (Game.GameTime - GameTimeStartedCallIn >= 10000 && (Ped.PlayerCrimesWitnessed.Any() || Ped.OtherCrimesWitnessed.Any()))
            {
                ReportCrime();
            }
        }
        GameTimeLastRan = Game.GameTime;
    }
    public override void Stop()
    {

    }
    private void ReportCrime()
    {
        if (Ped.Pedestrian.Exists() && Ped.Pedestrian.IsAlive && !Ped.Pedestrian.IsRagdoll)
        {
            if(Ped.PlayerCrimesWitnessed.Any())
            {
                EntryPoint.WriteToConsole($"TASKER: CallIn ReportCrime Player: {Ped.Pedestrian.Handle}", 3);
                Crime ToReport = Ped.PlayerCrimesWitnessed.OrderBy(x => x.Priority).FirstOrDefault();
                foreach (Crime toReport in Ped.PlayerCrimesWitnessed)
                {
                    Player.AddCrime(ToReport, false, Ped.PositionLastSeenCrime, Ped.VehicleLastSeenPlayerIn, Ped.WeaponLastSeenPlayerWith, Ped.EverSeenPlayer && Ped.ClosestDistanceToPlayer <= 10f, true, true);
                }
                Ped.PlayerCrimesWitnessed.Clear();
            }
            else if (Ped.OtherCrimesWitnessed.Any())
            {
                EntryPoint.WriteToConsole($"TASKER: CallIn ReportCrime OtherCrimesWithnessed: {Ped.Pedestrian.Handle}", 3);
                WitnessedCrime toReport = Ped.OtherCrimesWitnessed.Where(x => x.Perpetrator.Pedestrian.Exists() && !x.Perpetrator.IsBusted && x.Perpetrator.Pedestrian.IsAlive).OrderBy(x => x.Crime.Priority).ThenByDescending(x => x.GameTimeLastWitnessed).FirstOrDefault();
                if (toReport != null)
                {
                    Player.AddCrime(toReport.Crime, false, toReport.Location, toReport.Vehicle, toReport.Weapon, false, true, true);
                }
                Ped.OtherCrimesWitnessed.Clear();
            }
            EntryPoint.WriteToConsole($"TASKER: CallIn ReportCrime: {Ped.Pedestrian.Handle}", 3);
        }     
    }
}

