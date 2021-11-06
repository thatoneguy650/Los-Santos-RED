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
        World.CivilianList.ForEach(x => x.PlayerCrimesWitnessed.Clear());
    }
    public void Update()
    {
        int PedsUpdated = 0;
        foreach (PedExt ped in World.CivilianList.OrderBy(x => x.GameTimeLastUpdated))
        {
            try
            {
                ped.Update(Perceptable, PoliceRespondable, Vector3.Zero, World);
            }
            catch (Exception e)
            {
                EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Updating Civilian Data");
            }
            PedsUpdated++;
            if (PedsUpdated > 4)//10)//3//10//25
            {
                PedsUpdated = 0;
                GameFiber.Yield();
            }
        }
    }
}
