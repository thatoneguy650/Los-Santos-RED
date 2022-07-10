using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ScaredCallIn : ComplexTask
{
    private uint GameTimeStartedCallIn;
    public ScaredCallIn(IComplexTaskable ped, ITargetable player) : base(player, ped, 1000)
    {
        Name = "ScaredCallIn";
        SubTaskName = "";
        GameTimeStartedCallIn = Game.GameTime;
    }
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
            if (OtherTarget != null && OtherTarget.Pedestrian.Exists())
            {
                unsafe
                {
                    int lol = 0;
                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                    NativeFunction.CallByName<bool>("TASK_SMART_FLEE_PED", 0, OtherTarget.Pedestrian, 50f, 10000);//100f
                    NativeFunction.CallByName<bool>("TASK_USE_MOBILE_PHONE_TIMED", 0, 5000);
                    NativeFunction.CallByName<bool>("TASK_SMART_FLEE_PED", 0, OtherTarget.Pedestrian, 1500f, -1);
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
                    NativeFunction.CallByName<bool>("TASK_SMART_FLEE_PED", 0, Player.Character, 1500f, -1);
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
            if (Game.GameTime - GameTimeStartedCallIn >= 10000 && (Ped.PlayerCrimesWitnessed.Any() || Ped.OtherCrimesWitnessed.Any() || Ped.HasSeenDistressedPed))
            {
                ReportCrime();
            }
        }
        else
        {
            if (Game.GameTime - GameTimeStartedCallIn >= 4000 && (Ped.PlayerCrimesWitnessed.Any() || Ped.OtherCrimesWitnessed.Any() || Ped.HasSeenDistressedPed))
            {
                ReportCrime();
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
    private void ReportCrime()
    {
        if (Ped.Pedestrian.Exists() && Ped.Pedestrian.IsAlive && !Ped.Pedestrian.IsRagdoll)
        {
            if(Ped.PlayerCrimesWitnessed.Any())
            {
                Crime ToReport = Ped.PlayerCrimesWitnessed.OrderBy(x => x.Priority).FirstOrDefault();
                List<Crime> toCheck = Ped.PlayerCrimesWitnessed.Copy();
                foreach (Crime toReport in toCheck)
                {
                    Player.AddCrime(ToReport, false, Ped.PositionLastSeenCrime, Ped.VehicleLastSeenPlayerIn, Ped.WeaponLastSeenPlayerWith, Ped.EverSeenPlayer && Ped.ClosestDistanceToPlayer <= 10f, true, true);

                }
                Ped.PlayerCrimesWitnessed.Clear();
            }
            else if (Ped.OtherCrimesWitnessed.Any())
            {
                WitnessedCrime toReport = Ped.OtherCrimesWitnessed.Where(x => x.Perpetrator.Pedestrian.Exists() && !x.Perpetrator.IsBusted && x.Perpetrator.Pedestrian.IsAlive).OrderBy(x => x.Crime.Priority).ThenByDescending(x => x.GameTimeLastWitnessed).FirstOrDefault();
                if (toReport != null)
                {
                    Player.AddCrime(toReport.Crime, false, toReport.Location, toReport.Vehicle, toReport.Weapon, false, true, false);// true);//why was this set to true?
                }
                Ped.OtherCrimesWitnessed.Clear();
            }
            else if (Ped.HasSeenDistressedPed)
            {
                Player.AddDistressedPed(Ped.PositionLastSeenDistressedPed);
                Ped.HasSeenDistressedPed = false;
            }
        }
        else if(!Ped.Pedestrian.Exists())
        {
            if (Ped.PlayerCrimesWitnessed.Any())
            {
                Crime ToReport = Ped.PlayerCrimesWitnessed.OrderBy(x => x.Priority).FirstOrDefault();
                List<Crime> toCheck = Ped.PlayerCrimesWitnessed.Copy();
                foreach (Crime toReport in toCheck)
                {
                    Player.AddCrime(ToReport, false, Ped.PositionLastSeenCrime, Ped.VehicleLastSeenPlayerIn, Ped.WeaponLastSeenPlayerWith, Ped.EverSeenPlayer && Ped.ClosestDistanceToPlayer <= 10f, true, true);
                }
                Ped.PlayerCrimesWitnessed.Clear();
            }
            else if (Ped.OtherCrimesWitnessed.Any())
            {
                WitnessedCrime toReport = Ped.OtherCrimesWitnessed.Where(x => x.Perpetrator.Pedestrian.Exists() && !x.Perpetrator.IsBusted && x.Perpetrator.Pedestrian.IsAlive).OrderBy(x => x.Crime.Priority).ThenByDescending(x => x.GameTimeLastWitnessed).FirstOrDefault();
                if (toReport != null)
                {
                    Player.AddCrime(toReport.Crime, false, toReport.Location, toReport.Vehicle, toReport.Weapon, false, true, true);
                }
                Ped.OtherCrimesWitnessed.Clear();
            }
            else if (Ped.HasSeenDistressedPed)
            {
                Player.AddDistressedPed(Ped.PositionLastSeenDistressedPed);
                Ped.HasSeenDistressedPed = false;
            }
        }
        
    }
}

