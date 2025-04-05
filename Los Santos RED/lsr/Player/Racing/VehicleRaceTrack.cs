using RAGENativeUI.Elements;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class VehicleRaceTrack
{
    public VehicleRaceTrack()
    {
    }

    public VehicleRaceTrack(string name, List<VehicleRaceCheckpoint> raceCheckpoints, List<VehicleRaceStartingPosition> vehicleRaceStartingPositions)
    {
        Name = name;
        RaceCheckpoints = raceCheckpoints;
        VehicleRaceStartingPositions = vehicleRaceStartingPositions;
    }
    public string Name { get; set; }
    public List<VehicleRaceCheckpoint> RaceCheckpoints { get; set; }
    public List<VehicleRaceStartingPosition> VehicleRaceStartingPositions { get; set; }



}

