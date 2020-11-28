using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CrimeEvent
{
    private uint GameTimeLastOccurred;
    private uint InstanceDuration = 20000;
    public CrimeEvent(Crime crimeToReport)
    {
        AssociatedCrime = crimeToReport;
        GameTimeLastOccurred = Game.GameTime;
    }
    public Crime AssociatedCrime { get; set; }
    public int Instances { get; set; } = 1;
    public bool CanAddInstance
    {
        get
        {
            if (Game.GameTime - GameTimeLastOccurred >= InstanceDuration)
                return true;
            else
                return false;
        }
    }
    public bool RecentlyOccurred(uint TimeSince)
    {
        if (Game.GameTime - GameTimeLastOccurred <= TimeSince)
            return true;
        else
            return false;
    }
    public void AddInstance()
    {
        if (CanAddInstance)
        {
            GameTimeLastOccurred = Game.GameTime;
            Instances++;
        }
    }
}