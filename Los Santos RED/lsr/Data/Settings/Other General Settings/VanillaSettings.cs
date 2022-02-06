using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class VanillaSettings : ISettingsDefaultable
{
    public bool TerminateRespawn { get; set; }
    public bool TerminateDispatch { get; set; }
    public bool TerminateHealthRecharge { get; set; }
    public bool TerminateWantedMusic { get; set; }
    public bool TerminateScanner { get; set; }
    public bool TerminateScenarioCops { get; set; }
    public VanillaSettings()
    {

        SetDefault();
    }
    public void SetDefault()
    {
        TerminateRespawn = true;
        TerminateDispatch = true;
        TerminateHealthRecharge = true;
        TerminateWantedMusic = true;
        TerminateScanner = true;
        TerminateScenarioCops = false;
    }

}