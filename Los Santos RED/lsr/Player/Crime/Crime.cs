using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable()]
public class Crime
{
    private bool HasShownWarning = false;
    private uint GameTimeLastShownWarning;
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
    public float MaxReportingDistance { get; set; } = 999f;
    public bool RequiresCitation { get; set; } = false;
    public bool RequiresSearch { get; set; } = false;

    public bool ShowsWarning { get; set; } = false;
    public string WarningMessage { get; set; } = "";
    public uint TimeBetweenWarnings { get; set; } = 900000;
    public override string ToString()
    {
        return Name;
    }

    public void DisplayWarning()
    {
        if(ShowsWarning && (!HasShownWarning || (TimeBetweenWarnings > 0 && Game.GameTime - GameTimeLastShownWarning >= TimeBetweenWarnings)) && WarningMessage != "")
        {
            string fullWarningMessage = WarningMessage;
            if(CanBeReportedByCivilians)
            {
                fullWarningMessage += "~n~~o~Citizens~s~ can report this ~r~violation~s~";
            }
            Game.DisplayHelp(fullWarningMessage);
            GameTimeLastShownWarning = Game.GameTime;
            HasShownWarning = true;
        }
    }
}