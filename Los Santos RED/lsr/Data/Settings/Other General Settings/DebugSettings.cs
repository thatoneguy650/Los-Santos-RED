using NAudio.Wave;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DebugSettings : ISettingsDefaultable
{
    public bool ShowPoliceTaskArrows { get; set; }
    public bool ShowCivilianTaskArrows { get; set; }
    public bool ShowCivilianPerceptionArrows { get; set; }
    public bool ShowTrafficArrows { get; set; }
    public float CanineRunSpeed { get;  set; }

    public DebugSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        ShowPoliceTaskArrows = false;
        ShowCivilianTaskArrows = false;
        ShowCivilianPerceptionArrows = false;
        ShowTrafficArrows = false;
        CanineRunSpeed = 10.0f;
    }
}