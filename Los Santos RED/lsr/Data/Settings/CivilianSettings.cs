using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CivilianSettings
{
    public bool ManageCivilianTasking { get; set; } = true;
    public float FightPercentage { get; set; } = 5f;
    public float CallPolicePercentage { get; set; } = 55f;
    public float SecurityFightPercentage { get; set; } = 70f;
    public float GangFightPercentage { get; set; } = 85f;
    public bool OverrideHealth { get; set; } = true;
    public int MinHealth { get; set; } = 70;
    public int MaxHealth { get; set; } = 100;
    public bool OverrideAccuracy { get; set; } = true;
    public int GeneralAccuracy { get; set; } = 10;

    public CivilianSettings()
    {

    }

}