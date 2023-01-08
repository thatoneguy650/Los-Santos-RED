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









    [Description("Set slow mo when you are wasted and using the wasted menu.")]
    public bool SetSlowMoOnDeath { get; set; }
    [Description("Set slow mo when you are busted and using the busted menu.")]
    public bool SetSlowMoOnBusted { get; set; }
    [Description("Allow the weapon dropping system to drop weapons.")]
    public bool AllowWeaponDropping { get; set; }
    [Description("If enabled, recently dropped weapons will be uncollectable until the WeaponDroppingTimeToSuppress has elapsed.")]
    public bool WeaponDroppingSupressPickups { get; set; }
    [Description("Time to disable pickups after dropping weapon.")]
    public uint WeaponDroppingTimeToSuppress { get; set; }
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
    [Description("Attempts to disable being hassled by vanilla spawned gang peds.")]
    public bool DisableVanillaGangHassling { get; set; }
    [Description("If enabled, friendly gangs will not be able to be targetted or carjacked by you.")]
    public bool AllowAttackingFriendlyPeds { get; set; }
    [Description("If enabled, DLC vehicles will appear in the stores. You will need a trainer to stop the rockstar security measure that despawns DLC cars in SP.")]
    public bool AllowDLCVehiclesInStores { get; set; }
    [Description("Minimum Possible value for player speech skill. Speech skill is used to talk your way out of tickets and other things. Possible Values are from 0 (Low Skill) to 100 (Full Skill).")]
    public int PlayerSpeechSkill_Min { get; set; }
    [Description("Maximum Possible value for player speech skill. Speech skill is used to talk your way out of tickets and other things. Possible Values are from 0 (Low Skill) to 100 (Full Skill).")]
    public int PlayerSpeechSkill_Max { get; set; }

    public PlayerOtherSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        Recognize_BaseTime = 2000;
        Recognize_NightPenalty = 3500;
        Recognize_VehiclePenalty = 750;

        SetSlowMoOnDeath = true;
        SetSlowMoOnBusted = true;
        AllowWeaponDropping = true;
        WeaponDroppingSupressPickups = true;
        WeaponDroppingTimeToSuppress = 15000;
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
        AllowAttackingFriendlyPeds = false;
        AllowDLCVehiclesInStores = false;
        PlayerSpeechSkill_Min = 15;
        PlayerSpeechSkill_Max = 55;
    }

}