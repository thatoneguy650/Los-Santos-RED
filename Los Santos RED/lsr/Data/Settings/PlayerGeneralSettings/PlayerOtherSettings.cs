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
    [Description("Modifier to melee damage on the player. If set Less than 1 the melee damage the player do will be reduced by that percent (0.1 = 10% damage). If set higher than 1 the melee damage will be scaled by that about (2.5 = 250% damage). Set at 1 for default.")]
    public float MeleeDamageModifier { get; set; }
    [Description("Cost to remove an investigation using the corrupt cop interaction.")]
    public int CorruptCopInvestigationClearCost { get; set; }
    [Description("Cost to remove your wanted level using the corrupt cop interaction. Number is multiplied by your current wanted level to get the total cost. Ex. 5000 at 2 starts would cost $10000")]
    public int CorruptCopWantedClearCostScalar { get; set; }
    [Description("Hours needed to wait between contact tasks after completing the previous task.")]
    public double HoursBetweenTasksWhenCompleted { get; set; }
    [Description("Hours needed to wait between contact tasks after failing the previous task.")]
    public double HoursBetweenTasksWhenFailed { get; set; }
    [Description("Allows mobile radio when on foot.")]
    public bool AllowMobileRadioOnFoot { get; set; }
    public bool DisableVanillaGangHassling { get; set; }
    public bool AllowAttackingFriendlyPeds { get; set; }

    public bool OverwriteHandOffset { get; set; }
    public float HandOffsetX { get; set; }
    public float HandOffsetY { get; set; }
    public float HandOffsetZ { get; set; }

    public float HandRotateX { get; set; }
    public float HandRotateY { get; set; }
    public float HandRotateZ { get; set; }




    public bool OverwriteMouthOffset { get; set; }
    public float MouthOffsetX { get; set; }
    public float MouthOffsetY { get; set; }
    public float MouthOffsetZ { get; set; }

    public float MouthRotateX { get; set; }
    public float MouthRotateY { get; set; }
    public float MouthRotateZ { get; set; }
    public bool AllowDLCVehiclesInStores { get; set; }

    public PlayerOtherSettings()
    {
        SetDefault();
#if DEBUG
        MeleeDamageModifier = 1.0f;
        //SetSlowMoOnBusted = false;
        //SetSlowMoOnDeath = false;
    #endif
    }
    public void SetDefault()
    {
        Recognize_BaseTime = 2000;
        Recognize_NightPenalty = 3500;
        Recognize_VehiclePenalty = 750;
        SearchMode_SearchTimeMultiplier = 30000;
        SetSlowMoOnDeath = true;
        SetSlowMoOnBusted = true;
        AllowWeaponDropping = true;
        MaleFreeModeVoice = "A_M_M_BEVHILLS_01_WHITE_FULL_01";
        FemaleFreeModeVoice = "A_F_M_BEVHILLS_01_WHITE_FULL_01";
        GenerateStoreItemPreviews = true;

        MeleeDamageModifier = 1.0f;


        CorruptCopInvestigationClearCost = 2000;
        CorruptCopWantedClearCostScalar = 5000;
        HoursBetweenTasksWhenCompleted = 6;
        HoursBetweenTasksWhenFailed = 24;
        AllowMobileRadioOnFoot = false;
        DisableVanillaGangHassling = true;
        AllowAttackingFriendlyPeds = true;

        OverwriteHandOffset = false;
        HandOffsetX = 0.0f;
        HandOffsetY = 0.0f;
        HandOffsetZ = 0.0f;
        HandRotateX = 0.0f;
        HandRotateY = 0.0f;
        HandRotateZ = 0.0f;



        OverwriteMouthOffset = false;
        MouthOffsetX = 0.0f;
        MouthOffsetY = 0.0f;
        MouthOffsetZ = 0.0f;
        MouthRotateX = 0.0f;
        MouthRotateY = 0.0f;
        MouthRotateZ = 0.0f;

        AllowDLCVehiclesInStores = false;

    }

}