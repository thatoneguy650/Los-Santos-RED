using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

public class InvestigationSettings : ISettingsDefaultable
{
    [Description("Size of the investigation range when triggered")]
    public float ActiveDistance { get; set; }
    [Description("Time before an investigation times out. Will only happen when no active suspects are around the investigation area.")]
    public uint TimeLimit { get; set; }
    [Description("Min Time before an investigation times out. Will only happen when no active suspects are around the investigation area.")]
    public uint MinTimeLimit { get; set; }
    [Description("Cops outside of this range will ignore the investigation.")]
    public float MaxDistance { get; set; }
    [Description("How close you need to be to the center of the investigation position to trigger a suspicious person violation if they have your description.")]
    public float SuspiciousDistance { get; set; }
    [Description("Enable or disable the mini-map investigation circle")]
    public bool CreateBlip { get; set; }

    [Description("Extra time after a cop arrives at the center before the investigation can expire.")]
    public uint ExtraTimeAfterReachingInvestigationCenterBeforeExpiring { get; set; }


    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        SetDefault();
    }

    public InvestigationSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        ActiveDistance = 700f;// 800f;
        TimeLimit = 90000;//60000;
        MinTimeLimit = 70000;// 20000;
        MaxDistance = 1000f;// 1500f;
        SuspiciousDistance = 250f;
        CreateBlip = true;
        ExtraTimeAfterReachingInvestigationCenterBeforeExpiring = 15000;
    }

}