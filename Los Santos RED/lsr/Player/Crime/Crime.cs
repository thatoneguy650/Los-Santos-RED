using Rage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        CanBeReactedToByCivilians = canBeReportedByCivilians;
        AngersCivilians = angersCivilians;
        ScaresCivilians = scaresCivilians;
    }
    public Crime()
    {

    }

    public string ID { get; set; }
    public string Name { get; set; }
    public bool Enabled { get; set; } = true;
    public int Priority { get; set; } = 99;


    public int ResultingWantedLevel { get; set; } = 0;
    public bool ResultsInLethalForce { get; set; } = false;
    public bool AngersCivilians { get; set; } = false;
    public bool ScaresCivilians { get; set; } = true;

    public bool CanViolateMultipleTimes { get; set; } = true;


    public bool CanReportBySound { get; set; } = false;
    public bool CanBeReactedToByCivilians { get; set; } = true;
    public float MaxReportingDistance { get; set; } = 999f;

    public float MaxObservingDistance { get; set; } = 999f;

    public float MaxHearingDistance { get; set; } = 999f;

    public bool CanReleaseOnCite { get; set; } = false;
    public bool CanReleaseOnCleanSearch { get; set; } = false;
    public bool CanReleaseOnTalkItOut { get; set; } = false;





    public bool IsTrafficViolation { get; set; }

    public bool ShowsWarning { get; set; } = false;
    public string WarningMessage { get; set; } = "";
    public uint TimeBetweenWarnings { get; set; } = 1800000;
    public uint GracePeriod { get; set; } = 0;






    public override string ToString()
    {
        return Name;
    }
    public ReactionTier ReactionTier => IsIntense ? ReactionTier.Intense : IsAngerInducing || IsScary ? ReactionTier.Alerted : IsMundane ? ReactionTier.Mundane : ReactionTier.None;//maybe jus thvae this set on a per crime basis>?
    public bool IsScary => ScaresCivilians;
    public bool IsAngerInducing => AngersCivilians;
    public bool IsIntense => ResultingWantedLevel >= 3 || ResultsInLethalForce || Priority <= 10;
    public bool IsMundane => CanBeReactedToByCivilians && !ScaresCivilians && !AngersCivilians;



    public void DisplayWarning()
    {
        if(ShowsWarning && (!HasShownWarning || (TimeBetweenWarnings > 0 && Game.GameTime - GameTimeLastShownWarning >= TimeBetweenWarnings)) && WarningMessage != "")
        {
            string fullWarningMessage = WarningMessage;
            if(CanBeReactedToByCivilians)
            {
                fullWarningMessage += "~n~~o~Citizens~s~ can report this ~r~violation~s~";
            }
            if (ResultingWantedLevel >= 2)
            {
                fullWarningMessage += "~n~This is an ~r~Arrestable offense~s~";
            }
            else if (CanReleaseOnCite)
            {
                fullWarningMessage += "~n~This is a ~y~Citable offense~s~";
            }
            else if (CanReleaseOnCleanSearch)
            {
                fullWarningMessage += "~n~This is a ~o~Searchable offense~s~";
            }
            Game.DisplayHelp(fullWarningMessage);
            GameTimeLastShownWarning = Game.GameTime;
            HasShownWarning = true;
        }
    }
}