using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerOtherSettings : ISettingsDefaultable
{
    [Description("Time an NPC needs to have seen you to determine if they can recognize you. Used for BOLOs and some crimes.")]
    public uint Recognize_BaseTime { get; set; }
    [Description("Additional Time an NPC needs to have seen you to determine if they can recognize you when it is night")]
    public uint Recognize_NightPenalty { get; set; }
    [Description("Additional Time an NPC needs to have seen you to determine if they can recognize you when you are in an unknown vehicle")]
    public uint Recognize_VehiclePenalty { get; set; }
    [Description("Time in millisecond that each wanted level adds to the search time. Ex. SearchTimeMultiplier of 30000 at 2 Stars would take 60 seconds to expire. At 4 stars, 120 seconds.")]
    public uint SearchMode_SearchTimeMultiplier { get; set; }
    [Description("Enable random scenario start where available. Scenarios are the random things peds can do, sit down, smoke, chat, etc.")]
    public bool AllowStartRandomScenario { get; set; }
    [Description("Set slow mo when you are wasted and using the wasted menu.")]
    public bool SetSlowMoOnDeath { get; set; }
    [Description("Set slow mo when you are busted and using the busted menu.")]
    public bool SetSlowMoOnBusted { get; set; }
    [Description("Allow the weapon dropping system to drop weapons.")]
    public bool AllowWeaponDropping { get; set; }
    [Description("Ped voice to use as your own when you are playing as the male freemode ped.")]
    public string MaleFreeModeVoice { get; set; }
    [Description("Ped voice to use as your own when you are playing as the female freemode ped.")]
    public string FemaleFreeModeVoice { get; set; }
    [Description("If enabled the stores will give you a preview of the item you are about to buy.")]
    public bool GenerateStoreItemPreviews { get; set; }

    public PlayerOtherSettings()
    {
        SetDefault();
        #if DEBUG

        #endif
    }
    public void SetDefault()
    {
        Recognize_BaseTime = 2000;
        Recognize_NightPenalty = 3500;
        Recognize_VehiclePenalty = 750;
        SearchMode_SearchTimeMultiplier = 30000;
        AllowStartRandomScenario = false;
        SetSlowMoOnDeath = true;
        SetSlowMoOnBusted = true;
        AllowWeaponDropping = true;
        MaleFreeModeVoice = "A_M_M_BEVHILLS_01_WHITE_FULL_01";
        FemaleFreeModeVoice = "A_F_M_BEVHILLS_01_WHITE_FULL_01";
        GenerateStoreItemPreviews = true;
    }

}