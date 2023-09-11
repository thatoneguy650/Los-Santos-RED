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



    public int StreetDisplayStyleIndex { get; set; }
    public int StreetDisplayColorIndex { get; set; }
    public int StreetDisplayFontIndex { get; set; }



    public float StreetDisplayOffsetX { get; set; }
    public float StreetDisplayOffsetY { get; set; }
    public float StreetDisplayOffsetZ { get; set; }


    public float StreetDisplayRotationX { get; set; }
    public float StreetDisplayRotationY { get; set; }
    public float StreetDisplayRotationZ { get; set; }

    public float StreetDisplayScaleX { get; set; }
    public float StreetDisplayScaleY { get; set; }
    public float StreetDisplayScaleZ { get; set; }
    public bool StreetDisplayUseCalc { get; set; }
    public float StreetDisplayNodeOffsetFront { get; set; }

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
        StreetDisplayStyleIndex = 0;
        StreetDisplayColorIndex = 2;
        StreetDisplayFontIndex = 5;

        StreetDisplayOffsetZ = 2f;

        StreetDisplayScaleX = 5.0f;
        StreetDisplayScaleY = 1.0f;
        StreetDisplayScaleZ = 1.0f;
        StreetDisplayUseCalc = true;
        StreetDisplayNodeOffsetFront = 40f;
    }
}