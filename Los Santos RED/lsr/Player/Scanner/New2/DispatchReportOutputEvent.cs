using Rage;
using System.Collections.Generic;

public class DispatchReportOutputEvent
{
    public DispatchReportOutputEvent()
    {
    }
    public bool AnyDispatchInterrupts { get; set; } = false;
    public bool CanBeInterrupted { get; set; } = true;
    public bool CanInterrupt { get; set; } = true;
    public bool HasStreetAudio { get; set; }
    public bool HasUnitAudio { get; set; }
    public bool HasLocationAudio { get; set; }
    public bool HasZoneAudio { get; set; }
    public string NotificationSubtitle { get; set; } = "Status";
    public string NotificationText { get; set; } = "~b~Scanner Audio";
    public string NotificationTitle { get; set; } = "Police Scanner";
    public Vector3 PositionToReport { get; set; }
    public int Priority { get; set; } = 99;
    public List<string> SoundsToPlay { get; set; } = new List<string>();
    public string Subtitles { get; set; }
    public int UnitAudioAmount { get; set; }





}