using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Crime
{
    public string ID { get; set; }
    public string Name { get; set; }
    public int ResultingWantedLevel { get; set; } = 0;
    public bool ResultsInLethalForce { get; set; } = false;
    public int Priority { get; set; } = 99;
    public bool CanBeReportedByCivilians { get; set; } = true;
    public bool CanReportBySound { get; set; } = false;
    public bool AngersCivilians { get; set; } = false;
    public bool ScaresCivilians { get; set; } = true;
    public bool IsCurrentlyViolating { get; set; } = false;
    public bool CanViolateWithoutPerception { get; set; } = false;
    public bool CanViolateMultipleTimes { get; set; } = true;
    public int PriorityGroup { get; set; } = 99;
    public bool IsTrafficViolation { get; set; }
    public Crime(string _ID, string _Name, int _ResultingWantedLevel, bool _ResultsInLethalForce, int priority, int priorityGroup)
    {
        ID = _ID;
        ResultsInLethalForce = _ResultsInLethalForce;
        ResultingWantedLevel = _ResultingWantedLevel;
        Name = _Name;
        Priority = priority;
        PriorityGroup = priorityGroup;
    }
    public Crime(string _ID, string _Name, int _ResultingWantedLevel, bool _ResultsInLethalForce, int priority, int priorityGroup, bool canBeReportedByCivilians, bool angersCivilians, bool scaresCivilians)
    {
        ID = _ID;
        ResultsInLethalForce = _ResultsInLethalForce;
        ResultingWantedLevel = _ResultingWantedLevel;
        Name = _Name;
        Priority = priority;
        PriorityGroup = priorityGroup;
        CanBeReportedByCivilians = canBeReportedByCivilians;
        AngersCivilians = angersCivilians;
        ScaresCivilians = scaresCivilians;
    }
    public Crime(string _ID, string _Name, int _ResultingWantedLevel, bool _ResultsInLethalForce, int priority, int priorityGroup, bool canBeReportedByCivilians)
    {
        ID = _ID;
        ResultsInLethalForce = _ResultsInLethalForce;
        ResultingWantedLevel = _ResultingWantedLevel;
        Name = _Name;
        Priority = priority;
        PriorityGroup = priorityGroup;
        CanBeReportedByCivilians = canBeReportedByCivilians;
    }
}