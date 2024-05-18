using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

public class RadarDetectorSettings : ISettingsDefaultable
{
    [Description("Global enable disable for the entire radar detection system.")]
    public bool IsEnabled { get; set; }
    [Description("If enabled, will only alert when you are in a vehicle.")]
    public bool IsVehicleOnly { get; set; }
    [Description("If enabled, you will be required to have a radar detector item to hear radar detection.")]
    public bool DisableWithoutItem { get; set; }
    [Description("Enable or disable changing audio volume with the radar detector.")]
    public bool SetVolume { get; set; }
    [Description("Volume setting to use with the SetVolume setting. Min 0.0 Max 1.0")]
    public float AudioVolume { get; set; }
    public float MaxAlertDistance { get; set; }
    public string SoundFile { get; set; }

    public RadarDetectorSettings()
    {
        SetDefault();
    }

    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        SetDefault();
    }
    public void SetDefault()
    {
        IsEnabled = true;
        IsVehicleOnly = true;
        DisableWithoutItem = true;
        SetVolume = true;
        MaxAlertDistance = 300f;
        AudioVolume = 0.15f;// 0.45f;
        SoundFile = "radar\\CHIRP.mp3";
    }
}