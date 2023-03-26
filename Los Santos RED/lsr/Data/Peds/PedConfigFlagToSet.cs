using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PedConfigFlagToSet
{
    public PedConfigFlagToSet()
    {
    }

    public PedConfigFlagToSet(int configFlag, bool enabled)
    {
        ConfigFlag = configFlag;
        Enabled = enabled;
    }


    public bool Enabled { get; set; }
    public int ConfigFlag { get; set; }
    public void ApplyToPed(Ped ped)
    {
        if(!ped.Exists())
        {
            return;
        }
        NativeFunction.Natives.SET_PED_CONFIG_FLAG(ped, ConfigFlag, Enabled);
    }
}

