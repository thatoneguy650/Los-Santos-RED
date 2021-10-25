using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DamageSettings
{
    public bool ModifyDamage { get; set; } = true;
    public bool ClearDamage { get; set; } = true;
    public float Armor_NormalDamageModifier { get; set; } = 1.0f;
    public float Armor_GrazeDamageModifier { get; set; } = 0.25f;
    public float Armor_CriticalDamageModifier { get; set; } = 2.0f;
    public float Health_FatalDamageModifier { get; set; } = 10.0f;
    public float Health_NormalDamageModifier { get; set; } = 2.0f;
    public float Health_GrazeDamageModifier { get; set; } = 0.75f;
    public float Health_CriticalDamageModifier { get; set; } = 3.0f;
    public bool AllowRagdoll { get; set; } = true;
    public float NormalDamagePercent { get; set; } = 60f;
    public float GrazeDamagePercent { get; set; } = 10f;
    public float CriticalDamagePercent { get; set; } = 22f;
    public float FatalDamagePercent { get; set; } = 8f;

    public DamageSettings()
    {

    }

}