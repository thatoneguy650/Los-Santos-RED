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
    private IPoliceRespondable Player;

    public Civilians(IEntityProvideable world, IPoliceRespondable currentPlayer)
    {
        World = world;
        Player = currentPlayer;
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
            ped.Update(Player,Vector3.Zero);
            PedsUpdated++;
            if(PedsUpdated > 10)//25
            {
                break;
            }
        }
    }
}
