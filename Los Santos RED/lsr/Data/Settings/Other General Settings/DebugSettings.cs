using Rage;
using System.Runtime.Serialization;
using System.Security.Policy;

public class DebugSettings : ISettingsDefaultable
{
    public int SpawnCarsTestLimit { get; set; }
    public bool ShowPoliceTaskArrows { get; set; }
    public bool ShowCivilianTaskArrows { get; set; }
    public bool ShowCivilianPerceptionArrows { get; set; }
    public bool ShowTrafficArrows { get; set; }

    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        SetDefault();
    }

    public DebugSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        SpawnCarsTestLimit = 90;
        ShowPoliceTaskArrows = false;
        ShowCivilianTaskArrows = false;
        ShowCivilianPerceptionArrows = false;
        ShowTrafficArrows = false;
    }
}