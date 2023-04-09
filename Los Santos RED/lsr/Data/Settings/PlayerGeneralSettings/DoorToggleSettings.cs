using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DoorToggleSettings : ISettingsDefaultable
{
    [Description("")]
    public float CloseHoodAnimationTime { get; set; }
    public float OpenHoodAnimationTime { get; set; }
    public float DefaultAnimationTime { get; set; }
    public float TrunkHeading { get; set; }
    public float TrunkOffset { get; set; }
    public float HoodHeading { get; set; }
    public float HoodOffset { get; set; }
    public bool ShowMarker { get; set; }
    public DoorToggleSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        CloseHoodAnimationTime = 0.25f;
        OpenHoodAnimationTime = 0.5f;
        DefaultAnimationTime = 0.25f;
        TrunkHeading = 90f;
        TrunkOffset = -0.5f;
        HoodHeading = 90f;
        HoodOffset = 0.5f;
        ShowMarker = false;
    }
}