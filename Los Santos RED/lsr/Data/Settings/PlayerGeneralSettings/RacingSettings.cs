using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RacingSettings : ISettingsDefaultable
{
    [Description("")]
    public bool EnableOpponentRaceAssists { get; set; }
    public float AISpeedOffsetMin { get; set; }
    public float AISpeedOffsetMax { get;  set; }
    public float AIPowerOffsetMin { get; set; }
    public float AIPowerOffsetMax { get; set; }
    public float AIDriverAbilityMin { get; set; }
    public float AIDriverAbilityMax { get; set; }


    public float AIDriverAggressivenessMin { get; set; }
    public float AIDriverAggressivenessMax { get; set; }



    public float AIReactionDelayScalar { get; set; }
    public bool AIRubberBandingEnabled { get; set; }
    public bool AIStabilityForceAssist { get; set; }
    public bool AIClearFrontAssist { get; set; }
    public bool AIUnStuckAssist { get; set; }

    public RacingSettings()
    {
        SetDefault();

    }
    public void SetDefault()
    {
        EnableOpponentRaceAssists = true;
        AISpeedOffsetMin = -1.0f;
        AISpeedOffsetMax = 6.0f;


        AIPowerOffsetMin = -0.05f; 
        AIPowerOffsetMax = 0.35f;

        AIDriverAbilityMin = 0.8f; 
        AIDriverAbilityMax = 1.0f;

        AIDriverAggressivenessMin = 0.8f;
        AIDriverAggressivenessMax = 1.0f;
        AIReactionDelayScalar = 500f;
        AIRubberBandingEnabled = true;
        AIStabilityForceAssist = true;
        AIClearFrontAssist = true;
        AIUnStuckAssist = true;
    }

}