using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RecoilSettings : ISettingsDefaultable
{
    [Description("Enable or disable the entire recoil system.")]
    public bool ApplyRecoil { get; set; }
    [Description("Enable or disable the recoil system when in a vehicle only.")]
    public bool ApplyRecoilInVehicle { get; set; }
    [Description("Enable or disable the recoil system when on foot only.")]
    public bool ApplyRecoilOnFoot { get; set; }
    [Description("Global vertical recoil adjuster. Multiplier for the vertical intensity of the recoil. 1.0 is default, 2.0f would be double the felt recoil, 0.5 would be 1/2 of the felt recoil")]
    public float VerticalRecoilAdjuster { get; set; }
    [Description("Global vertical recoil adjuster. Multiplier for the horizontal intensity of the recoil. 1.0 is default, 2.0f would be double the felt recoil, 0.5 would be 1/2 of the felt recoil")]
    public float HorizontalRecoilAdjuster { get; set; }
    [Description("On foot vertical recoil adjuster. Multiplier for the vertical intensity of the recoil only when on foot. Will stack with the global. 1.0 is default, 2.0f would be double the felt recoil, 0.5 would be 1/2 of the felt recoil")]
    public float VerticalOnFootRecoilAdjuster { get; set; }
    [Description("On foot horizontal recoil adjuster. Multiplier for the horizontal intensity of the recoil only when on foot. Will stack with the global. 1.0 is default, 2.0f would be double the felt recoil, 0.5 would be 1/2 of the felt recoil")]
    public float HorizontalOnFootRecoilAdjuster { get; set; }
    [Description("In vehicle vertical recoil adjuster. Multiplier for the vertical intensity of the recoil only when in vehicle. Will stack with the global. 1.0 is default, 2.0f would be double the felt recoil, 0.5 would be 1/2 of the felt recoil")]
    public float VerticalInVehicleRecoilAdjuster { get; set; }
    [Description("In vehicle horizontal recoil adjuster. Multiplier for the horizontal intensity of the recoil only when in vehicle. Will stack with the global. 1.0 is default, 2.0f would be double the felt recoil, 0.5 would be 1/2 of the felt recoil")]
    public float HorizontalInVehicleRecoilAdjuster { get; set; }
    public RecoilSettings()
    {
        SetDefault();
#if DEBUG

#endif
    }
    public void SetDefault()
    {
        ApplyRecoil = true;
        ApplyRecoilInVehicle = true;
        ApplyRecoilOnFoot = true;
        VerticalRecoilAdjuster = 1.0f;
        HorizontalRecoilAdjuster = 1.0f;
        VerticalOnFootRecoilAdjuster = 1.0f;
        HorizontalOnFootRecoilAdjuster = 1.0f;
        VerticalInVehicleRecoilAdjuster = 1.0f;
        HorizontalInVehicleRecoilAdjuster = 1.0f;
    }
}