using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DamageSettings : ISettingsDefaultable
{
    public bool ModifyDamage { get; set; }
    public bool ClearDamage { get; set; }
    public float Armor_NormalDamageModifier { get; set; }
    public float Armor_GrazeDamageModifier { get; set; }
    public float Armor_CriticalDamageModifier { get; set; }
    public float Health_FatalDamageModifier { get; set; }
    public float Health_NormalDamageModifier { get; set; }
    public float Health_GrazeDamageModifier { get; set; }
    public float Health_CriticalDamageModifier { get; set; }
    public bool AllowRagdoll { get; set; }
    public float NormalDamagePercent { get; set; }
    public float GrazeDamagePercent { get; set; }
    public float CriticalDamagePercent { get; set; }
    public float FatalDamagePercent { get; set; }

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