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
    [Description("Enable or disable the recoil system when in a vehicle only.")]
    public bool ApplySwayInVehicle { get; set; }
    [Description("Enable or disable the recoil system when on foot only.")]
    public bool ApplySwayOnFoot { get; set; }
    [Description("Global vertical recoil adjuster. Multiplier for the vertical intensity of the recoil. 1.0 is default, 2.0f would be double the felt recoil, 0.5 would be 1/2 of the felt recoil")]
    public float VeritcalSwayAdjuster { get; set; }
    [Description("Global vertical recoil adjuster. Multiplier for the horizontal intensity of the recoil. 1.0 is default, 2.0f would be double the felt recoil, 0.5 would be 1/2 of the felt recoil")]
    public float HorizontalSwayAdjuster { get; set; }
    [Description("On foot vertical recoil adjuster. Multiplier for the vertical intensity of the recoil only when on foot. Will stack with the global. 1.0 is default, 2.0f would be double the felt recoil, 0.5 would be 1/2 of the felt recoil")]
    public float VeritcalOnFootSwayAdjuster { get; set; }
    [Description("On foot horizontal recoil adjuster. Multiplier for the horizontal intensity of the recoil only when on foot. Will stack with the global. 1.0 is default, 2.0f would be double the felt recoil, 0.5 would be 1/2 of the felt recoil")]
    public float HorizontalOnFootSwayAdjuster { get; set; }
    [Description("In vehicle vertical recoil adjuster. Multiplier for the vertical intensity of the recoil only when in vehicle. Will stack with the global. 1.0 is default, 2.0f would be double the felt recoil, 0.5 would be 1/2 of the felt recoil")]
    public float VeritcalInVehicleSwayAdjuster { get; set; }
    [Description("In vehicle horizontal recoil adjuster. Multiplier for the horizontal intensity of the recoil only when in vehicle. Will stack with the global. 1.0 is default, 2.0f would be double the felt recoil, 0.5 would be 1/2 of the felt recoil")]
    public float HorizontalInVehicleSwayAdjuster { get; set; }

    public SwaySettings()
    {
        SetDefault();
#if DEBUG

#endif
    }
    public void SetDefault()
    {
        ApplySway = true;
        ApplySwayInVehicle = true;
        ApplySwayOnFoot = true;
        VeritcalSwayAdjuster = 1.0f;
        HorizontalSwayAdjuster = 1.0f;
        VeritcalOnFootSwayAdjuster = 1.0f;
        HorizontalOnFootSwayAdjuster = 1.0f;
        VeritcalInVehicleSwayAdjuster = 1.0f;
        HorizontalInVehicleSwayAdjuster = 1.0f;
    }

}