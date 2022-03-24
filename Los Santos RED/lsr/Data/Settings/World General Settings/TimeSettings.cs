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
    public int FastForwardMultiplier { get; set; }
    public int FastForwardInterval { get; set; }

    public TimeSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        ScaleTime = true;
        FastForwardMultiplier = 100;//300
        FastForwardInterval = 10;
    }

}