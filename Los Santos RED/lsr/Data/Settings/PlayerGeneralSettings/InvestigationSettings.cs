﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class InvestigationSettings : ISettingsDefaultable
{
    [Description("Size of the investigation range when triggered")]
    public float ActiveDistance { get; set; }
    [Description("Time before an investigation times out. Will only happen when no active suspects are around the investigation area.")]
    public uint TimeLimit { get; set; }
    [Description("Cops outside of this range will ignore the investigation.")]
    public float MaxDistance { get; set; }
    [Description("How close you need to be to the center of the investigation position to trigger a suspicious person violation if they have your description.")]
    public float SuspiciousDistance { get; set; }
    [Description("Enable or disable the mini-map investigation circle")]
    public bool CreateBlip { get; set; }
    public InvestigationSettings()
    {
        SetDefault();
#if DEBUG

#endif
    }
    public void SetDefault()
    {
        ActiveDistance = 800f;
        TimeLimit = 60000;
        MaxDistance = 1500f;
        SuspiciousDistance = 250f;
        CreateBlip = true;
    }

}