using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class WantedLevel  
{
    public WantedLevel()
    {
    }

    public WantedLevel(int level)
    {
        Level = level;
    }

    public int Level { get; set; }
    public bool RequiresLethalForce { get; set; } = false;
    public int PoliceKillLimit { get; set; } = -1;
    public int CivilianKillLimit { get; set; } = -1;


    public int PedSpawnLimit { get; set; }
    public int VehicleSpawnLimit { get; set; }
    public int BoatSpawnLimit { get; set; }
    public int HeliSpawnLimit { get; set; }


    public int LikelyhoodOfAnySpawn { get; set; }
    public int LikelyHoodOfCountySpawn { get; set; }


    public int InvestigationRespondingOfficers { get; set; }


    public float WorldPedSpawnLimit { get; set; }//set the world to spawn less vanilla peds at 4-6 wanted level, what do, needs better name
}

