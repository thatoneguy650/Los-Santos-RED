using RAGENativeUI.Elements;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;


public class VehicleRaceTrack
{
    public VehicleRaceTrack()
    {
    }

    public VehicleRaceTrack(string id, string name, string description, List<VehicleRaceCheckpoint> raceCheckpoints, List<VehicleRaceStartingPosition> vehicleRaceStartingPositions)
    {
        ID = id;
        Name = name;
        Description = description;
        RaceCheckpoints = raceCheckpoints;
        VehicleRaceStartingPositions = vehicleRaceStartingPositions;
    }
    public string ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<VehicleRaceCheckpoint> RaceCheckpoints { get; set; }
    public List<VehicleRaceStartingPosition> VehicleRaceStartingPositions { get; set; }

    internal void AddDistanceOffset(Vector3 vector3)
    {
        foreach(VehicleRaceCheckpoint vehicleRaceCheckpoint in RaceCheckpoints)
        {
            vehicleRaceCheckpoint.Position += vector3;
        }
        foreach(VehicleRaceStartingPosition vehicleRaceStartingPosition in VehicleRaceStartingPositions)
        {
            vehicleRaceStartingPosition.Position += vector3;
        }
    }
}

