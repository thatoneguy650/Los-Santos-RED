using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;


public class VehicleRace
{
    private uint GameTimeStartedCountdown;
    private List<Blip> SpawnedBlips = new List<Blip>();
    private uint GameTimeStartedRace;
    private uint GameTimeFinishedRace;
    private bool HasWinner;
    private List<VehicleRacer> Finishers = new List<VehicleRacer>();
    public VehicleRace(string name, List<VehicleRaceCheckpoint> raceCheckpoints, List<VehicleRaceStartingPosition> vehicleRaceStartingPositions)
    {
        Name = name;
        RaceCheckpoints = raceCheckpoints;
        VehicleRaceStartingPositions = vehicleRaceStartingPositions;
    }
    public VehicleRace()
    {
    }

    public string Name { get; set; }

    public bool IsCountdownEnabled { get; set; } = true;
    public bool AreRacerBlipsEnabled { get; set; } = true;
    public List<VehicleRaceCheckpoint> RaceCheckpoints { get; set; }
    public List<VehicleRaceStartingPosition> VehicleRaceStartingPositions { get; set; }

    public bool HasRaceStarted => GameTimeStartedRace > 0;

    [XmlIgnore]
    public bool IsActive { get; set; }
    [XmlIgnore]
    public List<VehicleRacer> VehicleRacers
    {
        get
        {
            List<VehicleRacer> toReturn = new List<VehicleRacer>();
            toReturn.AddRange(AIVehicleRacers);
            toReturn.Add(PlayerRacer);
            return toReturn;
        }
    }
    [XmlIgnore]
    public List<AIVehicleRacer> AIVehicleRacers { get; set; }
    [XmlIgnore]
    public PlayerVehicleRacer PlayerRacer { get; set; }
    public void Setup(List<AIVehicleRacer> aivehicleRacers, PlayerVehicleRacer playerRacer)
    {
        VehicleRaceCheckpoint finishCheckpoint = RaceCheckpoints.OrderByDescending(x => x.Order).FirstOrDefault();
        if(finishCheckpoint != null)
        {
            finishCheckpoint.IsFinish = true;
        }
        AIVehicleRacers = aivehicleRacers;
        PlayerRacer = playerRacer;
    }
    public void Start(ITargetable targetable, IEntityProvideable World, ISettingsProvideable Settings)
    {
        GameFiber raceGameFiber = GameFiber.StartNew(delegate
        {
            ResetItems();
            IsActive = true;
            foreach (VehicleRacer vre in VehicleRacers)
            {
                vre.SetupRace(this);
            }
            RacePrelimiary();
            foreach (AIVehicleRacer vre in AIVehicleRacers)
            {
                vre.AssignTask(this, targetable, World, Settings);
            }
            foreach (VehicleRacer vre in VehicleRacers)
            {
                vre.SetRaceStart(this);
            }
            while (IsActive)
            {
                foreach (VehicleRacer vre in VehicleRacers)
                {
                    vre.Update(this);
                }
                if (Game.IsKeyDownRightNow(Keys.L) || !EntryPoint.ModController.IsRunning)
                {
                    IsActive = false;
                }
                if(GameTimeFinishedRace > 0 && Game.GameTime - GameTimeFinishedRace >= 15000)
                {
                    IsActive = false;
                }
                GameFiber.Yield();
            }
            Dispose();
        }, "RaceGameFiber");
    }

    private void ResetItems()
    {
        GameTimeStartedRace = 0;
        GameTimeFinishedRace = 0;
        HasWinner = false;
        GameTimeStartedCountdown = 0;
    }

    public void Dispose()
    {
        PlayerRacer.Dispose();
        foreach (AIVehicleRacer vre in AIVehicleRacers)
        {
            vre.Dispose();     
        }
    }
    public void AddBlip(Blip checkpointBlip)
    {
        SpawnedBlips.Add(checkpointBlip);
    }
    private void RacePrelimiary()
    {
        if(IsCountdownEnabled)
        {
            DoCountdown();
        }     
        GameTimeStartedRace = Game.GameTime;
        PlayerRacer.ShowMessage("GO!");
    }
    private void DoCountdown()
    {
       //EntryPoint.WriteToConsole($"COUNTDOWN START {Game.GameTime - GameTimeStartedCountdown}");
        GameTimeStartedCountdown = Game.GameTime;
        string toShow = "";
        string showing = "";
        while (Game.GameTime - GameTimeStartedCountdown <= 3000)
        {
            NativeHelper.DisablePlayerMovementControl();
            if (Game.GameTime - GameTimeStartedCountdown < 1000)
            {
                toShow = "3";
            }
            else if (Game.GameTime - GameTimeStartedCountdown < 2000)
            {
                toShow = "2";
            }
            else if (Game.GameTime - GameTimeStartedCountdown < 3000)
            {
                toShow = "1";
            }
            if(toShow != showing)
            {
                PlayerRacer.ShowMessage(toShow);
                showing = toShow;
            }
            GameFiber.Yield();
        }
        //EntryPoint.WriteToConsole("COUNTDOWN END");
    }
    public void OnRacerFinishedRace(VehicleRacer vehicleRacer)
    {
        if(GameTimeFinishedRace == 0)
        {
            GameTimeFinishedRace = Game.GameTime;
        }
        Finishers.Add(vehicleRacer);
        int finalPosition = Finishers.Count();
        vehicleRacer.OnFinishedRace(finalPosition);
        if(!PlayerRacer.HasFinishedRace && finalPosition == 1)
        {
            PlayerRacer.ShowMessage($"Winner: {vehicleRacer.RacerName}", vehicleRacer.GetTotalTimeAsString());
        }
    }
    public void EndRace()
    {
        GameTimeFinishedRace = Game.GameTime;
        //IsActive = false;
    }
}

