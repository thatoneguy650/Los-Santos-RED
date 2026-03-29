using System.ComponentModel;
using System.Runtime.Serialization;

public class DragSettings : ISettingsDefaultable
{
    [Description("")]
    public bool FadeOut { get; set; }
    public float HeadingSpeedMultiplier { get; set; }
    public float DraggingSpeedMultiplier { get; set; }
    public bool QuickAttachEnabled { get; set; }

    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        SetDefault();
    }
    public DragSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        FadeOut = true;
        HeadingSpeedMultiplier = 1.0f;
        DraggingSpeedMultiplier = 1.0f;
        QuickAttachEnabled = false;
    }
}