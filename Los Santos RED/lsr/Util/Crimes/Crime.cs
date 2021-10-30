using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable()]
public class Crime
{
    public Crime(string _ID, string _Name, int _ResultingWantedLevel, bool _ResultsInLethalForce, int priority)
    {
        ID = _ID;
        ResultsInLethalForce = _ResultsInLethalForce;
        ResultingWantedLevel = _ResultingWantedLevel;
        Name = _Name;
        Priority = priority;
    }
    public Crime(string _ID, string _Name, int _ResultingWantedLevel, bool _ResultsInLethalForce, int priority, bool canBeReportedByCivilians, bool angersCivilians, bool scaresCivilians)
    {
        ID = _ID;
        ResultsInLethalForce = _ResultsInLethalForce;
        ResultingWantedLevel = _ResultingWantedLevel;
        Name = _Name;
        Priority = priority;
        CanBeReportedByCivilians = canBeReportedByCivilians;
        AngersCivilians = angersCivilians;
        ScaresCivilians = scaresCivilians;
    }
    public Crime(string _ID, string _Name, int _ResultingWantedLevel, bool _ResultsInLethalForce, int priority, bool canBeReportedByCivilians)
    {
        ID = _ID;
        ResultsInLethalForce = _ResultsInLethalForce;
        ResultingWantedLevel = _ResultingWantedLevel;
        Name = _Name;
        Priority = priority;
        CanBeReportedByCivilians = canBeReportedByCivilians;
    }
    public Crime()
    {

    }
    public string ID { get; set; }
    public string Name { get; set; }
    public bool CanReportBySound { get; set; } = false;
    public bool CanViolateMultipleTimes { get; set; } = true;
    public bool CanViolateWithoutPerception { get; set; } = false;
    public bool CanBeReportedByCivilians { get; set; } = true;
    public bool IsTrafficViolation { get; set; }
    public int ResultingWantedLevel { get; set; } = 0;
    public bool ResultsInLethalForce { get; set; } = false;
    public bool AngersCivilians { get; set; } = false;
    public bool ScaresCivilians { get; set; } = true;
    public int Priority { get; set; } = 99;
    public bool Enabled { get; set; } = true;
}