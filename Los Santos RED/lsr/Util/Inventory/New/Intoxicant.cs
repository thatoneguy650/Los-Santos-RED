using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class Intoxicant
{
    public Intoxicant()
    {

    }

    public Intoxicant(string name, uint intoxicatingIntervalTime, uint soberingIntervalTime, float maxEffectAllowed, string overLayEffect)
    {
        Name = name;
        IntoxicatingIntervalTime = intoxicatingIntervalTime;
        MaxEffectAllowed = maxEffectAllowed;
        SoberingIntervalTime = soberingIntervalTime;
        OverLayEffect = overLayEffect;
    }

    public string Name { get; set; }
    public uint IntoxicatingIntervalTime { get; set; }
    public float MaxEffectAllowed { get; set; }
    public uint SoberingIntervalTime { get; set; }
    public string OverLayEffect { get; set; }
}

