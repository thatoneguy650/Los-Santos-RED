using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PoliceSettings
{
    public bool SpawnedAmbientPoliceHaveBlip { get; set; } = true;
    public bool ShowPoliceRadarBlips { get; set; } = false;
    public bool OverridePoliceAccuracy { get; set; } = true;
    public int PoliceGeneralAccuracy { get; set; } = 10;

    public bool DispatchAudio { get; set; } = true;
    public int DispatchAudioVolume { get; set; } = 5;
    public bool DispatchSubtitles { get; set; } = false;
    public bool DispatchNotifications { get; set; } = true;

    public PoliceSettings()
    {

    }
}