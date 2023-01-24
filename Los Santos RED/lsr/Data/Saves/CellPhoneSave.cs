using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CellPhoneSave
{
    public string CustomRingtone { get; set; } = "";
    public string CustomTextTone { get; set; } = "";
    public int CustomTheme { get; set; } = -1;
    public int CustomBackground { get; set; } = -1;
    public float CustomVolume { get; set; } = -1.0f;
    public bool SleepMode { get; set; } = false;
    public int CustomPhoneType { get; set; } = -1;
    public string CustomPhoneOS { get; set; } = "";

    public CellPhoneSave()
    {
    }

    public CellPhoneSave(string customRingtone, string customTextTone, int customTheme, int customBackground, float customVolume, bool sleepMode, int customPhoneType, string customPhoneOS)
    {
        CustomRingtone = customRingtone;
        CustomTextTone = customTextTone;
        CustomTheme = customTheme;
        CustomBackground = customBackground;
        CustomVolume = customVolume;
        SleepMode = sleepMode;
        CustomPhoneType = customPhoneType;
        CustomPhoneOS = customPhoneOS;
    }
}

