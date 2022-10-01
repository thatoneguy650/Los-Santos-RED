using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ScannerSettings : ISettingsDefaultable
{
    [Description("Global enable disable for the entire scanner system.")]
    public bool IsEnabled { get; set; }
    [Description("Enable or disable audio alerts from the scanner")]
    public bool EnableAudio { get; set; }
    [Description("Enable or disable changing audio volume with the scanner. Not recommened unless you have issues.")]
    public bool SetVolume { get; set; }

    [Description("Volume setting to use with the SetVolume setting. Min 0.0 Max 1.0")]
    public float AudioVolume { get; set; }


    //[Description("Volume setting to use with the SetVolume setting. Min 0 Max 10")]
    //public int AudioVolume_Old { get; set; }




    [Description("Enable or disable subtitles from the scanner.")]
    public bool EnableSubtitles { get; set; }
    [Description("Enable or disable notifications from the scanner.")]
    public bool EnableNotifications { get; set; }
    [Description("Minimum time before the crime/statues happens and the dispatcher makes the announcement. Smaller values are more reactive, but less realistic. Vanilla is basically instant.")]
    public int DelayMinTime { get; set; }
    [Description("Maximum time before the crime/statues happens and the dispatcher makes the announcement. Smaller values are more reactive, but less realistic. Vanilla is basically instant.")]
    public int DelayMaxTime { get; set; }
    [Description("Enable or disable ambient status announcements like suspect spotter or search for suspect. All crimes will still be reported. Used if you want only the most basic scanner system.")]
    public bool AllowStatusAnnouncements { get; set; }
    [Description("Changes the grammer of the dispatcher a bit to be more realstic in some places. Disable to be more consistent.")]
    public bool UseNearForLocations { get; set; }
    [Description("Number of units to include attention audio for during a citizen reported crime. Requires Call Signs.")]
    public int NumberOfUnitsToAnnounce { get; set; }
    [Description("Applies low and high pass filters to the audio to make it sound more like actual radio.")]
    public bool ApplyFilter { get; set; }

    public ScannerSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        IsEnabled = true;
        EnableAudio = true;
        SetVolume = true;
        AudioVolume = 0.3f;
        ApplyFilter = true;
        EnableSubtitles = false;
        EnableNotifications = true;
        DelayMinTime = 1500;
        DelayMaxTime = 2500;
        AllowStatusAnnouncements  = true;
        UseNearForLocations  = false;
        NumberOfUnitsToAnnounce = 2;

    }

}