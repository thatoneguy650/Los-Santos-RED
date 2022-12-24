using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SelectorSettings : ISettingsDefaultable
{
    [Description("Enable or disable the entire selector switch system")]
    public bool ApplySelector { get; set; }
    [Description("Enable or disable the selector system when using a controller.")]
    public bool ApplySelectorWithController { get; set; }
    public SelectorSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        ApplySelector = true;
        ApplySelectorWithController = false;
    }
}