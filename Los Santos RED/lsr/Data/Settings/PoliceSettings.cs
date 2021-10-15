using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PoliceSettings
{
    public bool SpawnedAmbientPoliceHaveBlip { get; set; } = false;
    public bool ShowPoliceRadarBlips { get; set; } = false;
    public bool OverridePoliceAccuracy { get; set; } = true;
    public int PoliceGeneralAccuracy { get; set; } = 10;
    public float AutoRecognizeDistance { get; set; } = 20f;
    public float AlwaysRecognizeDistance { get; set; } = 7f;
    public int RecentlySeenTime { get; set; } = 17000;
    public bool AllowAmbientSpeech { get; set; } = true;
    public bool AllowChaseAssists { get; set; } = true;
    public bool ManagePoliceLoadout { get; set; } = true;
    public float BustDistance { get; set; } = 5f;
    public bool AllowRadioInAnimations { get; set; } = false;
    public bool TaskPolice { get; set; } = true;
    public int MinHealth { get; set; } = 85;
    public int MaxHealth { get; set; } = 125;
    public int MinArmor { get; set; } = 0;
    public int MaxArmor { get; set; } = 50;
    public PoliceSettings()
    {

    }
}