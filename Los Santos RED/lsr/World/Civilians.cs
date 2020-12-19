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
            return Mod.World.Pedestrians.Civilians.Count(x => x.Pedestrian.Exists() && x.Pedestrian.IsPersistent);
        }
    }
    public void Tick()
    {
        Update();
    }
    public void ResetWitnessedCrimes()
    {
        Mod.World.Pedestrians.Civilians.ForEach(x => x.CrimesWitnessed.Clear());
    }
    private void Update()
    {
        foreach (PedExt MyPed in Mod.World.Pedestrians.Civilians.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsAlive))
        {
            MyPed.Update();
        }
    }
}
