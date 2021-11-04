using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Civilians
{
    private IEntityProvideable World;
    private IPoliceRespondable PoliceRespondable;
    private IPerceptable Perceptable;
    public Civilians(IEntityProvideable world, IPoliceRespondable policeRespondable, IPerceptable perceptable)
    {
        World = world;
        PoliceRespondable = policeRespondable;
        Perceptable = perceptable;
    }
    public int PersistentCount
    {
        get
        {
            return World.CivilianList.Count(x => x.Pedestrian.IsPersistent);
        }
    }
    public void ResetWitnessedCrimes()
    {
        World.CivilianList.ForEach(x => x.CrimesWitnessed.Clear());
    }
    public void Update()
    {
        int PedsUpdated = 0;
        foreach (PedExt ped in World.CivilianList.OrderBy(x => x.GameTimeLastUpdated))
        {
            ped.Update(Perceptable, PoliceRespondable, Vector3.Zero, World);
            PedsUpdated++;
            if (PedsUpdated > 4)//10)//3//10//25
            {
                PedsUpdated = 0;
                GameFiber.Yield();
            }
        }
    }
}
