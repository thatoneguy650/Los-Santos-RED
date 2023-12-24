using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

public class ViolationSettings : ISettingsDefaultable
{
    [Description("If enabled, the game will assume you are a cop and not trigger violations for anything besides murding or hurting other cops")]
    public bool TreatAsCop { get; set; }
    [Description("If enabled, the game will not allow you to be busted")]
    public bool IsUnBustable { get; set; }

    [Description("If enabled, help text will be displayed the first time you violate a crime")]
    public bool ShowCrimeWarnings { get; set; }
    [Description("Distance the cops will consider you guilty if you killed the target and stand around them. Don't stand near people you murdered (common sense?)")]
    public float MurderDistance { get; set; }
    [Description("Additional time peds will react to you hurting a civilian. Used for when they only see you directly after you hurt a ped.")]
    public uint RecentlyHurtCivilianTime { get; set; }
    [Description("Additional time peds will react to you hurting a cop. Used for when they only see you directly after you hurt a ped.")]
    public uint RecentlyHurtPoliceTime { get; set; }
    [Description("Additional time peds will react to you killing a civilian. Used for when they only see you directly after you killed a ped.")]
    public uint RecentlyKilledCivilianTime { get; set; }
    [Description("Additional time peds will react to you killing a cop. Used for when they only see you directly after you killed a ped.")]
    public uint RecentlyKilledPoliceTime { get; set; }
    [Description("Time you can violate driving against traffic before the cops react. Used to ignore small traffic errors.")]
    public uint RecentlyDrivingAgainstTrafficTime { get; set; }
    [Description("Time you can violate driving on pavement before the cops react. Used to ignore small traffic errors.")]
    public uint RecentlyDrivingOnPavementTime { get; set; }
    [Description("Additional time peds will react to you hitting a ped with a car. Used for when they only see you directly after you hit a ped.")]
    public uint RecentlyHitPedTime { get; set; }
    [Description("Additional time peds will react to you hitting a peds car with a car. Used for when they only see you directly after you hit a ped.")]
    public uint RecentlyHitVehicleTime { get; set; }
    [Description("Additional time peds will react to you running a red light. Used for when they only see you directly after you ran the light.")]
    public uint RecentlyRanRedLightTime { get; set; }
    [Description("Time resisting arrest will trigger when you are moving fast.")]
    public uint ResistingArrestFastTriggerTime { get; set; }
    [Description("Time resisting arrest will trigger when you are moving at a medium pace.")]
    public uint ResistingArrestMediumTriggerTime { get; set; }
    [Description("Time resisting arrest will trigger when you are barely moving or walking")]
    public uint ResistingArrestSlowTriggerTime { get; set; }
    [Description("Time you can violate speeding before the cops react. Used to ignore small traffic errors.")]
    public uint RecentlySpeedingTime { get; set; }
    [Description("Minimum speed required (MPH) to trigger some traffic violations (driving against traffic, driving on pavement, running a red light)")]
    public float MinTrafficViolationSpeed { get; set; }
    [Description("Speed required (MPH) over the speed limit to trigger felong speeding")]
    public float OverLimitFelonySpeedingAmount { get; set; }
    [Description("Speed required (MPH) over the speed limit to trigger speeding")]
    public float OverLimitSpeedingAmount { get; set; }
    public bool AllowCollisionReactions { get; set; }
    public bool AllowVehicleStandingReactions { get; set; }
    public bool AllowBodilyFunctionReactions { get; set; }
    public bool AllowVehicleDamageReactions { get; set; }
    public bool AllowStandingCloseReactions { get; set; }
    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        SetDefault();
    }
    public ViolationSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        TreatAsCop = false;
        IsUnBustable = false;
        RecentlyHurtCivilianTime = 5000;
        RecentlyHurtPoliceTime = 5000;
        RecentlyKilledCivilianTime = 5000;
        RecentlyKilledPoliceTime = 5000;
        MurderDistance = 9f;
        RecentlyDrivingAgainstTrafficTime = 1000;
        RecentlyDrivingOnPavementTime = 1000;
        RecentlySpeedingTime = 2000;
        RecentlyHitPedTime = 1500;
        RecentlyHitVehicleTime = 1500;
        RecentlyRanRedLightTime = 1500;
        ResistingArrestFastTriggerTime = 2000;
        ResistingArrestMediumTriggerTime = 10000;
        ResistingArrestSlowTriggerTime = 25000;

        MinTrafficViolationSpeed = 25f;
        OverLimitFelonySpeedingAmount = 20f;
        OverLimitSpeedingAmount = 10f;
        ShowCrimeWarnings = true;

        AllowCollisionReactions = true;
        AllowVehicleStandingReactions = true;
        AllowBodilyFunctionReactions = true;
        AllowVehicleDamageReactions = true;
        AllowStandingCloseReactions = true;
    }
}