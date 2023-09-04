using NAudio.Wave;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DebugSettings : ISettingsDefaultable
{
    public bool ShowPoliceTaskArrows { get; set; }
    public bool ShowCivilianTaskArrows { get; set; }
    public bool ShowCivilianPerceptionArrows { get; set; }
    public bool ShowTrafficArrows { get; set; }
    public float CanineRunSpeed { get;  set; }
    public bool SetupCopFully { get; set; }
    //public float BongAnimStart { get; set; }
    //public float BongAnimEnd { get; set; }
    //public float BongAnimBlend { get; set; }
    //public float EquipmentAnimEnd { get; set; }
    //public float EquipmentAnimStart { get; set; }

    public DebugSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        ShowPoliceTaskArrows = false;
        ShowCivilianTaskArrows = false;
        ShowCivilianPerceptionArrows = false;
        ShowTrafficArrows = false;
        CanineRunSpeed = 10.0f;
        SetupCopFully = true;
        //BongAnimStart = 0.15f;
        //BongAnimEnd = 0.5f;
        //BongAnimStart = 0.3f;
        //BongAnimEnd = 0.55f;
        //BongAnimBlend = 2.0f;
        //EquipmentAnimStart = 0.25f;
        //EquipmentAnimEnd = 0.75f;
    }
}