using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DamageSettings : ISettingsDefaultable
{
    [Description("Allows modification of damage in the game world.")]
    public bool ModifyDamage { get; set; }
    [Description("Clear the last damage after updating. Disable to increase compatibility with other mods.")]
    public bool ClearDamage { get; set; }
    [Description("Percentage of hits that a determined to be normal. Maximum of 100.")]
    public float NormalDamagePercent { get; set; }
    [Description("Percentage of hits that a determined to be graze. Maximum of 100.")]
    public float GrazeDamagePercent { get; set; }
    [Description("Percentage of hits that a determined to be critical. Maximum of 100.")]
    public float CriticalDamagePercent { get; set; }
    [Description("Percentage of hits that a determined to be fatal. Maximum of 100.")]
    public float FatalDamagePercent { get; set; }
    [Description("Scalar to modify the base damage done with a normal hit on and armored target and area. Ex. Damage of -25 AP determined to be a normal hit with a Armor_NormalDamageModifier of 1 would be a -25 AP damage hit.")]
    public float Armor_NormalDamageModifier { get; set; }
    [Description("Scalar to modify the base damage done with a graze hit on and armored target and area. Ex. Damage of -25 AP determined to be a graze with a Armor_GrazeDamageModifier of 0.25 would be a -5 AP damage hit.")]
    public float Armor_GrazeDamageModifier { get; set; }
    [Description("Scalar to modify the base damage done with a critical hit on and armored target and area. Ex. Damage of -25 AP determined to be a critical with a Armor_CriticalDamageModifier of 2 would be a -50 AP damage hit.")]
    public float Armor_CriticalDamageModifier { get; set; }
    [Description("Scalar to modify the base damage done with a fatal hit. Ex. Damage of -25 HP determined to be a fatal hit with a Health_FatalDamageModifier of 10 would be a -250 HP damage hit.")]
    public float Health_FatalDamageModifier { get; set; }
    [Description("Scalar to modify the base damage done with a normal hit. Ex. Damage of -25 HP determined to be a normal hit with a Health_NormalDamageModifier of 2 would be a -50 HP damage hit.")]
    public float Health_NormalDamageModifier { get; set; }
    [Description("Scalar to modify the base damage done with a graze hit. Ex. Damage of -25 HP determined to be a graze with a Health_GrazeDamageModifier of 0.75 would be a -19 HP damage hit.")]
    public float Health_GrazeDamageModifier { get; set; }
    [Description("Scalar to modify the base damage done with a critical hit. Ex. Damage of -25 HP determined to be a critical with a Health_CriticalDamageModifier of 3 would be a -75 HP damage hit.")]
    public float Health_CriticalDamageModifier { get; set; }
    [Description("Allow additional ragdoll on critical and fatal hits. (Currently Unusued)")]
    public bool AllowRagdoll { get; set; }
    public DamageSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        ModifyDamage = true;
        ClearDamage = true;
        Armor_NormalDamageModifier = 1.0f;
        Armor_GrazeDamageModifier = 0.25f;
        Armor_CriticalDamageModifier = 2.0f;
        Health_FatalDamageModifier = 10.0f;
        Health_NormalDamageModifier = 2.0f;
        Health_GrazeDamageModifier = 0.75f;
        Health_CriticalDamageModifier = 3.0f;
        AllowRagdoll = true;
        NormalDamagePercent = 60f;
        GrazeDamagePercent = 10f;
        CriticalDamagePercent = 22f;
        FatalDamagePercent = 8f;
    }

}