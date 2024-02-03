using ExtensionsMethods;
using LosSantosRED.lsr;
using Rage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class Intoxicant//THIS IS THE OBJECT, THC, ALOCOHL, ETC
{


    public Intoxicant()
    {

    }

    public Intoxicant(string name, uint intoxicatingIntervalTime, uint soberingIntervalTime, float maxEffectAllowed, string overLayEffect, IntoxicationEffect effects)
    {
        Name = name;
        IntoxicatingIntervalTime = intoxicatingIntervalTime;
        MaxEffectAllowed = maxEffectAllowed;
        SoberingIntervalTime = soberingIntervalTime;
        OverLayEffect = overLayEffect;
        Effects = effects;
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
    public string OverLayEffect { get; set; } = "";
    public bool HasOverlay => OverLayEffect != "";
    public float EffectIntoxicationLimit { get; set; } = 0.25f;
    public bool ContinuesWithoutCurrentUse { get; set; } = false;
    public bool NoCameraShake { get; set; }
    public IntoxicationEffect Effects { get; set; }
    public override string ToString()
    {
        return Name;
    }
    public string Description
    {
        get
        {
            string toReturn = "";
            if (HasOverlay)
            {

                if (Effects > 0)
                {
                    toReturn += $"~r~Impares Vision, ~s~";
                }
                else
                {
                    toReturn += $"~r~Impares Vision~s~";
                }
            }
            if (Effects > 0)
            {
                toReturn += $"~r~{Extensions.GetDisplayName(Effects)}~s~";
            }
            return toReturn;
        }
    }
}

