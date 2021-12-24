using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ActivitySettings
{
    public float Alcohol_MaxEffectAllowed { get; set; } = 5.0f;
    public uint Alcohol_TimeToReachEachIntoxicatedLevel { get; set; } = 25000;
    public uint Alcohol_TimeToReachEachSoberLevel { get; set; } = 60000;
    public string Alcohol_Overlay { get; set; } = "Drunk";
    public List<string> Alcohol_PossibleProps { get; set; } = new List<string>() { "prop_cs_beer_bot_40oz", "prop_cs_beer_bot_40oz_02", "prop_cs_beer_bot_40oz_03" };
    public float Marijuana_MaxEffectAllowed { get; set; } = 3.0f;
    public uint Marijuana_TimeToReachEachIntoxicatedLevel { get; set; } = 60000;
    public uint Marijuana_TimeToReachEachSoberLevel { get; set; } = 120000;
    public string Marijuana_Overlay { get; set; } = "drug_wobbly";
    public List<string> Marijuana_PossibleProps { get; set; } = new List<string>() { "p_amb_joint_01" };
    public List<string> Cigarette_PossibleProps { get; set; } = new List<string>() { "ng_proc_cigarette01a" };
    public bool TeleportWhenSitting { get; set; } = false;

    public ActivitySettings()
    {

    }
}