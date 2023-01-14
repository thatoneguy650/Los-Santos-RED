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
    public CalmCallIn(IComplexTaskable ped, ITargetable player) : base(player, ped, 1000)
    {
        Name = "CalmCallIn";
        SubTaskName = "";
        GameTimeStartedCallIn = Game.GameTime;
    }
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
            //EntryPoint.WriteToConsole($"TASKER: CalmCallIn Start: {Ped.Pedestrian.Handle}", 3);
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_USE_MOBILE_PHONE_TIMED", 0, 10000);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }     
        }
        GameTimeLastRan = Game.GameTime;
    }
    public override void Update()
    {
        if (Ped.Pedestrian.Exists())
        {
            if (Game.GameTime - GameTimeStartedCallIn >= 10000 && (Ped.PlayerCrimesWitnessed.Any() || Ped.OtherCrimesWitnessed.Any() || Ped.HasSeenDistressedPed))
            {
                Ped.ReportCrime(Player);
            }
        }
        else
        {
            if (Game.GameTime - GameTimeStartedCallIn >= 4000 && (Ped.PlayerCrimesWitnessed.Any() || Ped.OtherCrimesWitnessed.Any() || Ped.HasSeenDistressedPed))
            {
                Ped.ReportCrime(Player);
            }
        }
        GameTimeLastRan = Game.GameTime;
    }
    public override void Stop()
    {

    }
    public override void ReTask()
    {

    }
 
}

