using LosSantosRED.lsr;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Civilians
{
    public int PersistentCount
    {
        get
        {
            return Mod.World.CivilianList.Count(x => x.Pedestrian.IsPersistent);
        }
    }
    public void ResetWitnessedCrimes()
    {
        Mod.World.CivilianList.ForEach(x => x.CrimesWitnessed.Clear());
    }
    public void Update()
    {
        int PedsUpdated = 0;
        foreach (PedExt MyPed in Mod.World.CivilianList.OrderBy(x => x.Pedestrian.DistanceTo(Game.LocalPlayer.Character)))
        {
            MyPed.Update();
            PedsUpdated++;
            if(PedsUpdated > 10)//25
            {
                break;
            }
        }
    }
}
