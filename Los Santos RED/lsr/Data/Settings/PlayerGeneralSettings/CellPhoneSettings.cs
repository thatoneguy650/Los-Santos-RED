using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

public class CellphoneSettings : ISettingsDefaultable
{
    [Description("Allow temporary termination of vanilla cellphone while burner is active.")]
    public bool AllowTerminateVanillaCellphoneScripts { get; set; }
    [Description("Terminate vanilla cell phone while LSR is active")]
    public bool TerminateVanillaCellphone { get; set; }
    [Description("Allow the use of a custom burner phone to interact with contacts and text messages.")]
    public bool AllowBurnerPhone { get; set; }
    [Description("Burner cell position on screen (X - Horizontal).")]
    public float BurnerCellPositionX { get; set; }
    [Description("Burner cell position on screen (Y - Vertical).")]
    public float BurnerCellPositionY { get; set; }
    [Description("Burner cell position on screen (Z - ???).")]
    public float BurnerCellPositionZ { get; set; }
    [Description("Burner cell scale.")]
    public float BurnerCellScale { get; set; }
    [Description("Type of phone to use as the burner. Choose 0-4. 0 - Default phone / Michael's phone, 1 - Trevor's phone, 2 - Franklin's phone, 3 - Unused police phone, 4 - Prologue phone.")]
    public int BurnerCellPhoneTypeID { get; set; }
    [Description("Choose burner phone scaleform. Choices: cellphone_ifruit, cellphone_facade, or cellphone_badger")]
    public string BurnerCellScaleformName { get; set; }
    [Description("Choose burner phone theme (1-8)")]
    public int DefaultBurnerCellThemeID { get; set; }
    [Description("Choose burner phone background (0-17)")]
    public int DefaultBurnerCellBackgroundID { get; set; }
    [Description("Enable or disable custom ringtones.")]
    public bool UseCustomRingtone { get; set; }
    [Description("Set the default custom ringtone filename. Use the filename from the LosSantosRED\audio\tones folder. Ex. STTHOMAS.mp3")]
    public string DefaultCustomRingtoneNameNew { get; set; }
    [Description("Enable or disable custom texttones.")]
    public bool UseCustomTexttone { get; set; }
    [Description("Set the default custom texttone filename. Use the filename from the LosSantosRED\audio\tones folder. Ex. STTHOMAS.mp3")]
    public string DefaultCustomTexttoneNameNew { get; set; }
    [Description("Set volume of custom tones. Min 0.0 Max 1.0")]
    public float DefaultCustomToneVolume { get; set; }

    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        SetDefault();
    }

    public CellphoneSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {

        AllowBurnerPhone = true;
        BurnerCellPositionX = 99.62f;
        BurnerCellPositionY = -45.305f;
        BurnerCellPositionZ = -113f;
        BurnerCellScale = 500f;

        BurnerCellPhoneTypeID = 0;
        AllowTerminateVanillaCellphoneScripts = true;
        BurnerCellScaleformName = "cellphone_ifruit";
        TerminateVanillaCellphone = false;


        DefaultBurnerCellThemeID = 1;
        DefaultBurnerCellBackgroundID = 0;

        UseCustomRingtone = true;
        DefaultCustomRingtoneNameNew = "GTA5TONE2.mp3";
        UseCustomTexttone = true;
        DefaultCustomTexttoneNameNew = "GTA5TONE7.mp3";
        DefaultCustomToneVolume = 0.25f;

    }
}