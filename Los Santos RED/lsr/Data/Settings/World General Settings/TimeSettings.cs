using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TimeSettings : ISettingsDefaultable
{
    [Description("Scale time with player speed. When stationary game time will progress the same as real time. When moving quickly it will progress more rapidly that vanilla GTA time.")]
    public bool ScaleTime { get; set; }
    [Description("Fast forward speed value. A value of 25 means 25 seconds will elapse each time an interval is reached.")]
    public int FastForwardMultiplier { get; set; }
    [Description("Fast forward speed interval. A value of 10 means each 10 ms the FastForwardMultiplier will be added to the current time.")]
    public int FastForwardInterval { get; set; }
    public bool SetRealTime { get; set; }

    public TimeSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        ScaleTime = true;
        FastForwardMultiplier = 25;//300
        FastForwardInterval = 10;
        SetRealTime = false;
    }

}