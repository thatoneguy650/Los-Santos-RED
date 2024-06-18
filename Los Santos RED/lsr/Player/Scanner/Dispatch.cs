using Rage;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

public class Dispatch
{
    private uint GameTimeLastPlayed;
    public Dispatch()
    {
    }
    public bool AnyDispatchInterrupts { get; set; } = false;
    public bool CanAddExtras { get; set; } = true;
    public bool CanAlwaysBeInterrupted { get; set; }
    public bool CanAlwaysInterrupt { get; set; }
    public bool CanBeReportedMultipleTimes { get; set; } = true;
    public bool HasBeenPlayedThisWanted { get; set; }
    public bool HasPreamble => PreambleAudioSet.Any() || PreambleHelicopterAudioSet.Any();
    public bool HasEpilogue => EpilogueAudioSet.Any() || EpilogueHelicopterAudioSet.Any();
    public bool HasRecentlyBeenPlayed
    {
        get
        {
            uint TimeBetween = 25000;
            if (TimesPlayed > 0)
            {
                TimeBetween = 60000;
            }
            if(GameTimeLastPlayed == 0)
            {
                return false;
            }
            else if (Game.GameTime - GameTimeLastPlayed <= TimeBetween)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public bool HasVeryRecentlyBeenPlayed
    {
        get
        {
            if (GameTimeLastPlayed == 0)
            {
                return false;
            }
            else if (Game.GameTime - GameTimeLastPlayed <= 15000)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public bool HasntBeenPlayedForAWhile => GameTimeLastPlayed == 0 || Game.GameTime - GameTimeLastPlayed <= 90000;
    public bool IncludeAttentionAllUnits { get; set; }
    public bool IncludeCarryingWeapon { get; set; }
    public bool IncludeDrivingSpeed { get; set; }
    public bool IncludeDrivingVehicle { get; set; }
    public bool IncludeLicensePlate { get; set; }
    public bool IncludeRapSheet { get; set; }
    public bool IncludeReportedBy { get; set; } = true;
    public bool IsPoliceStatus { get; set; }

    public CrimeSceneDescription LatestInformation { get; set; } = new CrimeSceneDescription();
    public LocationSpecificity LocationDescription { get; set; } = LocationSpecificity.Nothing;
    public List<AudioSet> MainAudioSet { get; set; } = new List<AudioSet>();
    public List<AudioSet> MainMultiAudioSet { get; set; } = new List<AudioSet>();

    public List<AudioSet> MainHelicopterAudioSet { get; set; } = new List<AudioSet>();



    public bool MarkVehicleAsStolen { get; set; }
    public string Name { get; set; } = "Unknown";
    public string NotificationSubtitle { get; set; } = "";
    public string NotificationText
    {
        get
        {
            return Name;
        }
    }
    public string NotificationTitle { get; set; } = "Police Scanner";
    public List<AudioSet> PreambleAudioSet { get; set; } = new List<AudioSet>();





    public List<AudioSet> PreambleHelicopterAudioSet { get; set; } = new List<AudioSet>();






    public List<AudioSet> EpilogueAudioSet { get; set; } = new List<AudioSet>();


    public List<AudioSet> EpilogueHelicopterAudioSet { get; set; } = new List<AudioSet>();



    public int Priority { get; set; } = 99;
    public bool ResultsInLethalForce { get; set; }
    public List<AudioSet> SecondaryAudioSet { get; set; } = new List<AudioSet>();
    public int TimesPlayed { get; set; }
    public bool VehicleIncludesIn { get; set; }
    public bool IsAmbientAllowed { get; set; } = true;
    public void SetPlayed()
    {
        GameTimeLastPlayed = Game.GameTime;
        HasBeenPlayedThisWanted = true;
        TimesPlayed++;
    }
}