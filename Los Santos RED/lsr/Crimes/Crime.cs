using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable()]
public class Crime
{
    public string Name { get; set; }
    public int ResultingWantedLevel { get; set; } = 0;
    public bool ResultsInLethalForce { get; set; } = false;
    public int Priority { get; set; } = 99;
    public bool CanBeReportedByCivilians { get; set; } = true;
    public bool CanReportBySound { get; set; } = false;
    public bool WillAngerCivilians { get; set; } = false;
    public bool WillScareCivilians { get; set; } = true;
    public bool IsCurrentlyViolating { get; set; } = false;
    public bool IsAlwaysFlagged { get; set; } = false;
    public int PriorityGroup { get; set; } = 99;
    public Crime()
    {

    }
    public Crime(string _Name, int _ResultingWantedLevel, bool _ResultsInLethalForce, int priority, int priorityGroup)
    {
        ResultsInLethalForce = _ResultsInLethalForce;
        ResultingWantedLevel = _ResultingWantedLevel;
        Name = _Name;
        Priority = priority;
        PriorityGroup = priorityGroup;
    }
}