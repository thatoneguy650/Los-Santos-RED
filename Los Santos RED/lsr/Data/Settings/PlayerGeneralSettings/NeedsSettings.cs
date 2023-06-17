using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class NeedsSettings : ISettingsDefaultable
{
    [Description("Enable or disable the entire needs system")]
    public bool ApplyNeeds { get; set; }
    [Description("Enable or diable the thirst component of the needs system")]
    public bool ApplyThirst { get; set; }
    [Description("Change the intensity of the drain and recovery for thirst. Default 1.0")]
    public float ThirstChangeScalar { get; set; }
    [Description("Enable or diable the hunger component of the needs system")]
    public bool ApplyHunger { get; set; }
    [Description("Change the intensity of the drain and recovery for hunger. Default 1.0")]
    public float HungerChangeScalar { get; set; }
    [Description("Enable or diable the sleep component of the needs system")]
    public bool ApplySleep { get; set; }
    [Description("Change the intensity of the drain and recovery for sleep. Default 1.0")]
    public float SleepChangeScalar { get; set; }
    [Description("Changes the amount of digits seen on the hunger ui")]
    public int HungerDisplayDigits { get; set; }
    [Description("Changes the amount of digits seen on the thirst ui")]
    public int ThirstDisplayDigits { get; set; }
    [Description("Changes the amount of digits seen on the sleep ui")]
    public int SleepDisplayDigits { get; set; }
    [Description("Allows health regen when all needs are met (greater than 75%).")]
    public bool AllowHealthRegen { get; set; }
    [Description("Allows health drain when you have pressing needs (less than 25%) up to a set amount.")]
    public bool AllowHealthDrain { get; set; }
    [Description("Interval between adding health when all needs are met.")]
    public uint HealthRegenInterval { get; set; }
    [Description("Interval between draining health when you have pressing needs.")]
    public uint HealthDrainInterval { get; set; }
    [Description("Amount of health added each interval when all needs are met.")]
    public int HealthRegenAmount { get; set; }
    [Description("Amount of health drained each interval when you have pressing needs.")]
    public int HealthDrainAmount { get; set; }
    [Description("Minimum health value you can drain to.")]
    public int HealthDrainMinHealth { get; set; }
    [Description("Allows a melee damage decrease when you have pressing needs (less than 25%) up to a set amount.")]
    public bool AllowMeleeDamageDecrease { get; set; }
    [Description("Time (in ms) between needs being gained. Lower values increase needs more rapidly.")]
    public uint TimeBetweenGain { get; set; }

    public NeedsSettings()
    {
        SetDefault();

    }
    public void SetDefault()
    {
        ApplyNeeds = true;
        ApplyThirst = true;
        ThirstChangeScalar = 1.0f;
        ThirstDisplayDigits = 0;
        ApplyHunger = true;
        HungerChangeScalar = 1.0f;
        HungerDisplayDigits = 0;
        ApplySleep = true;
        SleepChangeScalar = 1.0f;
        SleepDisplayDigits = 0;
        AllowHealthRegen = true;
        AllowHealthDrain = true;
        HealthRegenInterval = 35000;
        HealthDrainInterval = 65000;
        HealthRegenAmount = 1;
        HealthDrainAmount = 1;
        HealthDrainMinHealth = 140;
        AllowMeleeDamageDecrease = true;
        TimeBetweenGain = 750;
    }

}