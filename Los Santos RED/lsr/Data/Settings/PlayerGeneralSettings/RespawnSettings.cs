using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RespawnSettings : ISettingsDefaultable
{

    [Description("Time when you are considered recently respawned. Used to delay some items that might happen immediately after you respawn.")]
    public uint RecentlyRespawnedTime { get; set; }
    [Description("Time when you are conisdered recently resisted arrest. Used so they dont immediately bust you before you have a chance to move.")]
    public uint RecentlyResistedArrestTime { get; set; }
    [Description("Gives a small invincibility to the player after respawning. Used so you have a chance of getting to cover.")]
    public bool InvincibilityOnRespawn { get; set; }
    [Description("Time invincibility lasts after respawn")]
    public int RespawnInvincibilityTime { get; set; }
    [Description("Allow you to undie and respawn at any time in the exact spot")]
    public bool AllowUndie { get; set; }
    [Description("Set a limit to the amount of undies you can do for a given chracter. Use 0 to allow unlimited.")]
    public int UndieLimit { get; set; }
    [Description("Used to disallow saving and undieing with a given chracter.")]
    public bool PermanentDeathMode { get; set; }
    [Description("Deduct the money without granting the bribe if the amount is too low.")]
    public bool DeductMoneyOnFailedBribe { get; set; }
    [Description("Minimum bribe amount required for each wanted level. Ex. a value of 500 would require a $1500 bribe at 3 stars.")]
    public int PoliceBribeWantedLevelScale { get; set; }
    [Description("Additional bribe amount for each police officer killed.")]
    public int PoliceBribePoliceKilledMultiplier { get; set; }
    [Description("Additional bribe amount for each police officer injured.")]
    public int PoliceBribePoliceInjuredMultiplier { get; set; }
    [Description("Deduct money on player after surrendering")]
    public bool DeductBailFee { get; set; }
    [Description("Minimum bail amount for each wanted level. Ex a value of 750 would require a $2250 bail fee at 3 stars.")]
    public int PoliceBailWantedLevelScale { get; set; }
    [Description("Additional bail amount for each police officer killed.")]
    public int PoliceBailPoliceKilledMultiplier { get; set; }
    [Description("Additional bail amount for each police officer injured.")]
    public int PoliceBailPoliceInjuredMultiplier { get; set; }
    [Description("Additional bail amount for each civilian killed.")]
    public int PoliceBailCiviliansKilledMultiplier { get; set; }
    [Description("Minimum bail duration (days) for each wanted level.")]
    public int PoliceBailDurationWantedLevelScale { get; set; }
    [Description("Additional bail duration (days) for each police officer killed.")]
    public int PoliceBailDurationPoliceKilledMultiplier { get; set; }
    [Description("Additional bail duration (days) for each police officer injured.")]
    public int PoliceBailDurationPoliceInjuredMultiplier { get; set; }
    [Description("Additional bail duration (days) for each civilian killed.")]
    public int PoliceBailDurationCiviliansKilledMultiplier { get; set; }
    [Description("Clears all illicit items from inventory when you surrender to police")]
    public bool ClearIllicitInventoryOnSurrender { get; set; }
    [Description("Remove weapons from player after surrendering")]
    public bool RemoveWeaponsOnSurrender { get; set; }
    [Description("Deduct money on respawn at hospital")]
    public bool DeductHospitalFee { get; set; }
    [Description("Amount to deduct when you respawn in the hospital")]
    public int HospitalFee { get; set; }
    [Description("Minimum Days to stay in the hospital")]
    public int HospitalStayMinDays { get; set; }
    [Description("Maximum Days to stay in the hospital")]
    public int HospitalStayMaxDays { get; set; }
    [Description("Clears all illicit items from inventory when you die")]
    public bool ClearIllicitInventoryOnDeath { get; set; }
    [Description("Remove weapons from player after respawning at a hostpital")]
    public bool RemoveWeaponsOnDeath { get; set; }

    public RespawnSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {

        RemoveWeaponsOnSurrender = true;



        DeductHospitalFee = true;
        HospitalStayMinDays = 2;
        HospitalStayMaxDays = 10;



        RemoveWeaponsOnDeath = true;

        RecentlyRespawnedTime = 1000;
        RecentlyResistedArrestTime = 5000;
        InvincibilityOnRespawn = true;
        RespawnInvincibilityTime = 5000;
        AllowUndie = true;
        UndieLimit = 0;


        DeductMoneyOnFailedBribe = true;
        PoliceBribeWantedLevelScale = 500;
        PoliceBribePoliceKilledMultiplier = 10000;
        PoliceBribePoliceInjuredMultiplier = 2000;



        DeductBailFee = true;
        PoliceBailWantedLevelScale = 2000;
        PoliceBailPoliceKilledMultiplier = 10000;
        PoliceBailPoliceInjuredMultiplier = 2000;
        PoliceBailCiviliansKilledMultiplier = 2000;


        PoliceBailDurationWantedLevelScale = 2;
        PoliceBailDurationPoliceKilledMultiplier = 3;
        PoliceBailDurationPoliceInjuredMultiplier = 1;
        PoliceBailDurationCiviliansKilledMultiplier = 2;


        HospitalFee = 5000;
        PermanentDeathMode = false;
        ClearIllicitInventoryOnDeath = true;
        ClearIllicitInventoryOnSurrender = true;



    }
}