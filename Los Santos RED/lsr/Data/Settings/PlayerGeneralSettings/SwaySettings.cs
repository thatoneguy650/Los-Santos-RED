using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SwaySettings : ISettingsDefaultable
{
    [Description("Enable or disable the entire sway system.")]
    public bool ApplySway { get; set; }
    [Description("Enable or disable the sway system when in a vehicle only.")]
    public bool ApplySwayInVehicle { get; set; }
    [Description("Enable or disable the sway system when on foot only.")]
    public bool ApplySwayOnFoot { get; set; }

    [Description("Enable or disable the sway system when using a controller.")]
    public bool ApplySwayWithController { get; set; }
    [Description("Global vertical sway adjuster. Multiplier for the vertical intensity of the sway. 1.0 is default, 2.0f would be double the felt sway, 0.5 would be 1/2 of the felt sway")]
    public float VeritcalSwayAdjuster { get; set; }
    [Description("Global horizonal sway adjuster. Multiplier for the horizontal intensity of the sway. 1.0 is default, 2.0f would be double the felt sway, 0.5 would be 1/2 of the felt sway")]
    public float HorizontalSwayAdjuster { get; set; }
    [Description("On foot vertical sway adjuster. Multiplier for the vertical intensity of the sway only when on foot. Will stack with the global. 1.0 is default, 2.0f would be double the felt sway, 0.5 would be 1/2 of the felt sway")]
    public float VeritcalOnFootSwayAdjuster { get; set; }
    [Description("On foot horizontal sway adjuster. Multiplier for the horizontal intensity of the sway only when on foot. Will stack with the global. 1.0 is default, 2.0f would be double the felt sway, 0.5 would be 1/2 of the felt sway")]
    public float HorizontalOnFootSwayAdjuster { get; set; }
    [Description("In vehicle vertical sway adjuster. Multiplier for the vertical intensity of the sway only when in vehicle. Will stack with the global. 1.0 is default, 2.0f would be double the felt sway, 0.5 would be 1/2 of the felt sway")]
    public float VeritcalInVehicleSwayAdjuster { get; set; }
    [Description("In vehicle horizontal sway adjuster. Multiplier for the horizontal intensity of the sway only when in vehicle. Will stack with the global. 1.0 is default, 2.0f would be double the felt sway, 0.5 would be 1/2 of the felt sway")]
    public float HorizontalInVehicleSwayAdjuster { get; set; }
    [Description("Enable or disable the sway system when in first person.")]
    public bool ApplySwayInFirstPerson { get; set; }
    [Description("First person vertical sway adjuster. Multiplier for the vertical intensity of the sway. 1.0 is default, 2.0f would be double the felt sway, 0.5 would be 1/2 of the felt sway")]
    public float VeritcalFirstPersonSwayAdjuster { get; set; }
    [Description("First person horizonal sway adjuster. Multiplier for the horizontal intensity of the sway. 1.0 is default, 2.0f would be double the felt sway, 0.5 would be 1/2 of the felt sway")]
    public float HorizontalFirstPersonSwayAdjuster { get; set; }
    [Description("Enable or disable the sway system when using a sniper rifle.")]
    public bool ApplySwayToSnipers { get; set; }
    [Description("Frames to wait after recoil to apply sway. DEBUG SETTING")]
    public int FramesBetweenRecoil { get; set; }
    public float SmoothRate { get; set; }
    public float ExcessivePitch { get; set; }

    public SwaySettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        ApplySway = true;
        ApplySwayInVehicle = false;
        ApplySwayOnFoot = true;
        VeritcalSwayAdjuster = 1.0f;
        HorizontalSwayAdjuster = 1.0f;
        VeritcalOnFootSwayAdjuster = 1.0f;
        HorizontalOnFootSwayAdjuster = 1.0f;
        VeritcalInVehicleSwayAdjuster = 1.0f;
        HorizontalInVehicleSwayAdjuster = 1.0f;
        ApplySwayInFirstPerson = false;
        VeritcalFirstPersonSwayAdjuster = 1.0f;
        HorizontalFirstPersonSwayAdjuster = 1.0f;
//#if DEBUG
//        ApplySwayInFirstPerson = true;
//#endif
        ApplySwayToSnipers = false;
        FramesBetweenRecoil = 30;
        ApplySwayWithController = false;
        SmoothRate = 1.0f;
        ExcessivePitch = 0.001f;
    }

}