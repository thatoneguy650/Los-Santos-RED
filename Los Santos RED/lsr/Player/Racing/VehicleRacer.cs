using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class VehicleRacer
{
    public List<VehicleRaceCheckpoint> VehicleRaceCheckpoints = new List<VehicleRaceCheckpoint>();
    public VehicleRaceCheckpoint TargetCheckpoint;
    protected VehicleRaceCheckpoint AfterTargetCheckpoint;
    protected uint GameTimeStartedRace;
    protected uint GameTimeFinishedRace;
    protected VehicleRace VehicleRace;
    public VehicleRacer()
    {
    }

    public VehicleRacer(VehicleExt vehicleExt)
    {
        VehicleExt = vehicleExt;
    }
    public float DistanceToCheckpoint { get; private set; }

    public VehicleExt VehicleExt { get; set; }
    public virtual string RacerName => "Racer";
    public bool HasFinishedRace => GameTimeFinishedRace > 0;
    public virtual bool IsPlayer => false;
    public virtual void Dispose()
    {

    }

    public virtual void SetupRace(VehicleRace vehicleRace)
    {
        if(vehicleRace == null)
        {
            return;
        }
        VehicleRace = vehicleRace;
        TargetCheckpoint = vehicleRace.RaceCheckpoints.Where(x => x.Order == 0).FirstOrDefault();
        if(TargetCheckpoint == null)
        {
            return;
        }
        AfterTargetCheckpoint = vehicleRace.RaceCheckpoints.Where(x => x.Order == 1).FirstOrDefault();
    }
    public virtual void Update(VehicleRace vehicleRace)
    { 
        if(TargetCheckpoint == null || VehicleExt == null || !VehicleExt.Vehicle.Exists() || GameTimeFinishedRace > 0)
        {
            return;
        }
        DistanceToCheckpoint = TargetCheckpoint.Position.DistanceTo(VehicleExt.Vehicle);
        if (DistanceToCheckpoint <= 20f)
        {
            //EntryPoint.WriteToConsole($"{PedExt?.Handle} have reached checkpoint {TargetCheckpoint.Order} at {TargetCheckpoint.Position}");
            if (TargetCheckpoint.IsFinish)
            {
                //EntryPoint.WriteToConsole($"{PedExt?.Handle} HAVE FINISHED THE RACE");
                //OnFinishedRace();
                GameTimeFinishedRace = Game.GameTime;
                vehicleRace.OnRacerFinishedRace(this);
                //TargetCheckpoint = null;
            }
            else
            {
                //EntryPoint.WriteToConsole($"Looking for {TargetCheckpoint.Order + 1}");
                int nextOrder = TargetCheckpoint.Order + 1;
                TargetCheckpoint = vehicleRace.RaceCheckpoints.Where(x => x.Order == nextOrder).FirstOrDefault();
                AfterTargetCheckpoint = vehicleRace.RaceCheckpoints.Where(x => x.Order == nextOrder + 1).FirstOrDefault();
                //EntryPoint.WriteToConsole($"Assigned Next checkpoint {TargetCheckpoint.Order} at {TargetCheckpoint.Position}");
                OnReachedCheckpoint();
            }
        }        
    }
    public string GetTotalTimeAsString()
    {
        return ConvertMSToTime(GameTimeFinishedRace - GameTimeStartedRace);
    }
    private string ConvertMSToTime(uint TotalGameTime)
    {
        TimeSpan t = TimeSpan.FromMilliseconds(TotalGameTime);
        string answer = string.Format("{0:00}:{1:00}.{2:000}",
                                t.Minutes,
                                t.Seconds,
                                t.Milliseconds);

        return answer;
    }
    public virtual void OnReachedCheckpoint()
    {

    }
    public virtual void OnFinishedRace(int finalPosition, VehicleRace vehicleRace)
    {

    }
    public virtual void SetRaceStart(VehicleRace vehicleRace)
    {
        GameTimeStartedRace = Game.GameTime;
    }

    public virtual void HandleWinningBet(int betAmount)
    {
 
    }
}

