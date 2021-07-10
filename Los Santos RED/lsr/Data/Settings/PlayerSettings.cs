using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerSettings
{
    public string AutoTuneRadioStation { get; set; } = "RADIO_19_USER";
    public bool KeepRadioStationAutoTuned { get; set; } = true;
    public uint Recognize_BaseTime { get; set; } = 2000;
    public uint Recognize_NightPenalty { get; set; } = 3500;
    public uint Recognize_VehiclePenalty { get; set; } = 750;
    public bool DisableAutoEngineStart { get; set; } = true;

    public PlayerSettings()
    {

    }

}