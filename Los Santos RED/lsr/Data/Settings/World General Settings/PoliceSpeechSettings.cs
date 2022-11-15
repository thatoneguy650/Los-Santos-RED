using System.ComponentModel;

public class PoliceSpeechSettings : ISettingsDefaultable
{

    [Description("Enable or disable ambient speech from police during chases")]
    public bool AllowAmbientSpeech { get; set; }
    [Description("Minimum time (in ms) between cops speaking when not armed or in a deadly chase.")]
    public int TimeBetweenCopSpeak_General_Min { get; set; }
    [Description("Minimum additional time (in ms) between cops speaking when not armed or in a deadly chase.")]
    public int TimeBetweenCopSpeak_General_Randomizer_Min { get; set; }
    [Description("Maximum additional time (in ms) between cops speaking when not armed or in a deadly chase.")]
    public int TimeBetweenCopSpeak_General_Randomizer_Max { get; set; }
    [Description("Minimum time (in ms) between cops speaking when you are armed.")]
    public int TimeBetweenCopSpeak_Armed_Min { get; set; }
    [Description("Minimum additional time (in ms) between cops speaking when you are armed.")]
    public int TimeBetweenCopSpeak_Armed_Randomizer_Min { get; set; }
    [Description("Maximum additional time (in ms) between cops speaking when you are armed.")]
    public int TimeBetweenCopSpeak_Armed_Randomizer_Max { get; set; }
    [Description("Minimum time (in ms) between cops speaking when in a deadly chase.")]
    public int TimeBetweenCopSpeak_Deadly_Min { get; set; }
    [Description("Minimum additional time (in ms) between cops speaking when in a deadly chase.")]
    public int TimeBetweenCopSpeak_Deadly_Randomizer_Min { get; set; }
    [Description("Maximum additional time (in ms) between cops speaking when in a deadly chase.")]
    public int TimeBetweenCopSpeak_Deadly_Randomizer_Max { get; set; }
    [Description("Minimum time (in ms) between cops speaking when in a weapons free mode.")]
    public int TimeBetweenCopSpeak_WeaponsFree_Min { get; set; }
    [Description("Minimum additional time (in ms) between cops speaking when in weapons free mode.")]
    public int TimeBetweenCopSpeak_WeaponsFree_Randomizer_Min { get; set; }
    [Description("Maximum additional time (in ms) between cops speaking when in weapons free mode.")]
    public int TimeBetweenCopSpeak_WeaponsFree_Randomizer_Max { get; set; }


    public PoliceSpeechSettings()
    {
        SetDefault();


    }
    public void SetDefault()
    {
     
        AllowAmbientSpeech = true;

        TimeBetweenCopSpeak_Armed_Min = 15000;
        TimeBetweenCopSpeak_Armed_Randomizer_Min = 5000;
        TimeBetweenCopSpeak_Armed_Randomizer_Max = 10000;


        TimeBetweenCopSpeak_General_Min = 20000;
        TimeBetweenCopSpeak_General_Randomizer_Min = 10000;
        TimeBetweenCopSpeak_General_Randomizer_Max = 19000;


        TimeBetweenCopSpeak_Deadly_Min = 15000;
        TimeBetweenCopSpeak_Deadly_Randomizer_Min = 7000;
        TimeBetweenCopSpeak_Deadly_Randomizer_Max = 15000;

        TimeBetweenCopSpeak_WeaponsFree_Min = 12000;
        TimeBetweenCopSpeak_WeaponsFree_Randomizer_Min = 3000;
        TimeBetweenCopSpeak_WeaponsFree_Randomizer_Max = 7000;

    }
}