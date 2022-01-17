using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TimeSettings : ISettingsDefaultable
{
    public bool ScaleTime { get; set; }
    public TimeSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        ScaleTime = true;
    }

}