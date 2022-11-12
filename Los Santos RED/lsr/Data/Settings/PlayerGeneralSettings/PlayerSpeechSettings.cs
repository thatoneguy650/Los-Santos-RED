using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerSpeechSettings : ISettingsDefaultable
{
    [Description("Enable or disable the entire auto speech system.")]
    public bool EnableSpeech { get; set; }

    [Description("Percentage of time to speak when active mode is triggered. Min 0 Max 100.")]
    public float OnWantedActiveModePercentage { get; set; }
    [Description("Percentage of time to speak when search mode is triggered. Min 0 Max 100.")]
    public float OnWantedSearchModePercentage { get; set; }
    [Description("Percentage of time to speak when youy become wanted. Min 0 Max 100.")]
    public float OnBecameWantedPercentage { get; set; }
    [Description("Percentage of time to speak when you lose wanted. Min 0 Max 100.")]
    public float OnLostWantedPercentage { get; set; }
    [Description("Percentage of time to speak when you have eluded the police. Min 0 Max 100.")]
    public float OnSuspectEludedPercentage { get; set; }
    [Description("Percentage of time to speak when you have crashed your car. Min 0 Max 100.")]
    public float OnCrashedCarPercentage { get; set; }
    [Description("Percentage of time to speak when you have shot a weapon. Min 0 Max 100.")]
    public float OnShotGunPercentage { get; set; }
    [Description("Percentage of time to speak when you kill a police officer. Min 0 Max 100.")]
    public float OnKilledCopPercentage { get; set; }
    [Description("Percentage of time to speak when you kill a civilian. Min 0 Max 100.")]
    public float OnKilledCivilianPercentage { get; set; }
    [Description("Minimum time (in ms) between speech when at 3 stars or more.")]
    public uint TimeBetweenSpeech_HighWanted { get; set; }
    [Description("Minimum time (in ms) between speech when at 1 or 2 stars.")]
    public uint TimeBetweenSpeech_LowWanted { get; set; }
    [Description("Minimum time (in ms) between speech when not wanted.")]
    public uint TimeBetweenSpeech { get; set; }
    [Description("Minimum time (in ms) to add as randomness between speaking.")]
    public uint TimeBetweenSpeechRandomizer_Min { get; set; }
    [Description("MAximum time (in ms) to add as randomness between speaking.")]
    public uint TimeBetweenSpeechRandomizer_Max { get; set; }

    public PlayerSpeechSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        EnableSpeech = true;
        OnWantedActiveModePercentage = 80f;
        OnWantedSearchModePercentage = 30f;
        OnBecameWantedPercentage = 100f;
        OnLostWantedPercentage = 100f;
        OnSuspectEludedPercentage = 30f;
        OnCrashedCarPercentage = 50f;
        OnShotGunPercentage = 30f;
        OnKilledCopPercentage = 80f;
        OnKilledCivilianPercentage = 80f;
        TimeBetweenSpeech = 25000;
        TimeBetweenSpeech_LowWanted = 20000;
        TimeBetweenSpeech_HighWanted = 15000;
        TimeBetweenSpeechRandomizer_Min = 2000;
        TimeBetweenSpeechRandomizer_Max = 10000;

    }
}