using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PossibleLocations
{
    

    public PossibleLocations()
    {

    }
    public List<GameLocation> LocationsList { get; private set; } = new List<GameLocation>();
    public List<DeadDrop> DeadDrops { get; private set; } = new List<DeadDrop>();
    public List<ScrapYard> ScrapYards { get; private set; } = new List<ScrapYard>();
    public List<GangDen> GangDens { get; private set; } = new List<GangDen>();
}

