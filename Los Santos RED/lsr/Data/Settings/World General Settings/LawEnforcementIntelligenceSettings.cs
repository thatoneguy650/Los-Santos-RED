using System;
using System.ComponentModel;

[Serializable]
public class LawEnforcementIntelligenceSettings : ISettingsDefaultable
{
    [Category("Law Enforcement Intelligence")]
    [Description("Enable persistent warrant records and warrant lifecycle processing.")]
    public bool EnableWarrants { get; set; } = true;

    [Category("Law Enforcement Intelligence")]
    [Description("Enable BOLO records and automatic expiry processing.")]
    public bool EnableBOLOSystem { get; set; } = true;

    [Category("Law Enforcement Intelligence")]
    [Description("Enable background investigation progression from evidence already known to police.")]
    public bool EnableBackgroundInvestigation { get; set; } = true;

    [Category("Law Enforcement Intelligence")]
    [Description("Enable FIB surveillance assignments. Surveillance only records actual observations.")]
    public bool EnableFIBSurveillance { get; set; } = true;

    [Category("Law Enforcement Intelligence")]
    [Description("Maximum number of simultaneous FIB surveillance teams.")]
    public int MaxActiveFIBTeams { get; set; } = 2;

    [Category("Law Enforcement Intelligence")]
    [Description("Minimum distance in metres from the player when a surveillance team is created.")]
    public float SurveillanceStartDistance { get; set; } = 350f;

    [Category("Law Enforcement Intelligence")]
    [Description("Preferred minimum following distance in metres.")]
    public float SurveillanceMinDistance { get; set; } = 100f;

    [Category("Law Enforcement Intelligence")]
    [Description("Maximum following distance in metres before surveillance loses contact.")]
    public float SurveillanceMaxDistance { get; set; } = 450f;

    [Category("Law Enforcement Intelligence")]
    [Description("Chance per valid observation opportunity that surveillance is detected, from 0 to 100.")]
    public float SurveillanceDetectionChance { get; set; } = 8f;

    [Category("Law Enforcement Intelligence")]
    [Description("How often (in milliseconds of game time) background intelligence is processed.")]
    public int InvestigationUpdateInterval { get; set; } = 60000;

    [Category("Law Enforcement Intelligence")]
    [Description("Number of in-game days to retain individual observations.")]
    public int IntelligenceRetentionDays { get; set; } = 90;

    [Category("Law Enforcement Intelligence")]
    [Description("Default BOLO stop probability inside its designated zone, from 0 to 100.")]
    public float BOLOInZoneStopChance { get; set; } = 65f;

    [Category("Law Enforcement Intelligence")]
    [Description("Default BOLO recognition probability outside its designated zone, from 0 to 100.")]
    public float BOLOOutOfZoneStopChance { get; set; } = 15f;

    [Category("Law Enforcement Intelligence")]
    [Description("Minimum residence confidence required before a raid can be considered, from 0 to 100.")]
    public float RaidMinimumConfidence { get; set; } = 75f;

    public LawEnforcementIntelligenceSettings()
    {
        SetDefault();
    }

    public void SetDefault()
    {
        EnableWarrants = true;
        EnableBOLOSystem = true;
        EnableBackgroundInvestigation = true;
        EnableFIBSurveillance = true;
        MaxActiveFIBTeams = 2;
        SurveillanceStartDistance = 350f;
        SurveillanceMinDistance = 100f;
        SurveillanceMaxDistance = 450f;
        SurveillanceDetectionChance = 8f;
        InvestigationUpdateInterval = 60000;
        IntelligenceRetentionDays = 90;
        BOLOInZoneStopChance = 65f;
        BOLOOutOfZoneStopChance = 15f;
        RaidMinimumConfidence = 75f;
    }

    public void Normalize()
    {
        InvestigationUpdateInterval = Math.Max(1000, Math.Min(3600000, InvestigationUpdateInterval <= 0 ? 60000 : InvestigationUpdateInterval));
        MaxActiveFIBTeams = Math.Max(0, Math.Min(10, MaxActiveFIBTeams));
        SurveillanceStartDistance = ClampDistance(SurveillanceStartDistance, 350f);
        SurveillanceMinDistance = ClampDistance(SurveillanceMinDistance, 100f);
        SurveillanceMaxDistance = ClampDistance(SurveillanceMaxDistance, 450f);
        if (SurveillanceMaxDistance < SurveillanceMinDistance)
            SurveillanceMaxDistance = SurveillanceMinDistance;
        if (SurveillanceStartDistance < SurveillanceMinDistance)
            SurveillanceStartDistance = SurveillanceMinDistance;
        SurveillanceDetectionChance = ClampPercent(SurveillanceDetectionChance, 8f);
        IntelligenceRetentionDays = Math.Max(1, Math.Min(3650, IntelligenceRetentionDays <= 0 ? 90 : IntelligenceRetentionDays));
        BOLOInZoneStopChance = ClampPercent(BOLOInZoneStopChance, 65f);
        BOLOOutOfZoneStopChance = ClampPercent(BOLOOutOfZoneStopChance, 15f);
        RaidMinimumConfidence = ClampPercent(RaidMinimumConfidence, 75f);
    }

    private static float ClampDistance(float value, float fallback)
    {
        if (float.IsNaN(value) || float.IsInfinity(value)) return fallback;
        return Math.Max(10f, Math.Min(5000f, value));
    }

    private static float ClampPercent(float value, float fallback)
    {
        if (float.IsNaN(value) || float.IsInfinity(value)) return fallback;
        return Math.Max(0f, Math.Min(100f, value));
    }
}
