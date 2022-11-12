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
    [Description("Clear the last damage after updating. Disable to increase compatibility with other mods. Leave enabled for best results.")]
    public bool ClearDamage { get; set; }
    [Description("Allows modification of damage to the player in the game world. Requires ModifyDamage to be enabled")]
    public bool ModifyPlayerDamage { get; set; }
    [Description("Percentage of hits that a determined to be normal. Maximum of 100.")]
    public float NormalDamagePercentPlayer { get; set; }
    [Description("Percentage of hits that a determined to be graze. Maximum of 100.")]
    public float GrazeDamagePercentPlayer { get; set; }
    [Description("Percentage of hits that a determined to be critical. Maximum of 100.")]
    public float CriticalDamagePercentPlayer { get; set; }
    [Description("Percentage of hits that a determined to be fatal. Maximum of 100.")]
    public float FatalDamagePercentPlayer { get; set; }
    [Description("Scalar to modify the base damage done with a normal hit on and armored target and area. Ex. Damage of -25 AP determined to be a normal hit with a Armor_NormalDamageModifier of 1 would be a -25 AP damage hit.")]
    public float Armor_NormalDamageModifierPlayer { get; set; }
    [Description("Scalar to modify the base damage done with a graze hit on and armored target and area. Ex. Damage of -25 AP determined to be a graze with a Armor_GrazeDamageModifier of 0.25 would be a -5 AP damage hit.")]
    public float Armor_GrazeDamageModifierPlayer { get; set; }
    [Description("Scalar to modify the base damage done with a critical hit on and armored target and area. Ex. Damage of -25 AP determined to be a critical with a Armor_CriticalDamageModifier of 2 would be a -50 AP damage hit.")]
    public float Armor_CriticalDamageModifierPlayer { get; set; }
    [Description("Scalar to modify the base damage done with a fatal hit. Ex. Damage of -25 HP determined to be a fatal hit with a Health_FatalDamageModifier of 10 would be a -250 HP damage hit.")]
    public float Health_FatalDamageModifierPlayer { get; set; }
    [Description("Scalar to modify the base damage done with a normal hit. Ex. Damage of -25 HP determined to be a normal hit with a Health_NormalDamageModifier of 2 would be a -50 HP damage hit.")]
    public float Health_NormalDamageModifierPlayer { get; set; }
    [Description("Scalar to modify the base damage done with a graze hit. Ex. Damage of -25 HP determined to be a graze with a Health_GrazeDamageModifier of 0.75 would be a -19 HP damage hit.")]
    public float Health_GrazeDamageModifierPlayer { get; set; }
    [Description("Scalar to modify the base damage done with a critical hit. Ex. Damage of -25 HP determined to be a critical with a Health_CriticalDamageModifier of 3 would be a -75 HP damage hit.")]
    public float Health_CriticalDamageModifierPlayer { get; set; }


    [Description("Allow injury effects (movement/overlays/etc) to play on the play when they get too injured")]
    public bool AllowInjuryEffects { get; set; }
    [Description("Health lost level at which the effects will start")]
    public int InjuryEffectHealthLostStart { get; set; }

    [Description("Intensity scalar for the health lost effects.")]
    public float InjuryEffectIntensityModifier { get; set; }


    [Description("Allow the player to yell when damaged.")]
    public bool AllowPlayerPainYells { get; set; }
    [Description("Health damage taken needed to trigger a pain yell for the player.")]
    public int PlayerPainYellsDamageNeeded { get; set; }



    [Description("Allows modification of damage to the AI in the game world. Requires ModifyDamage to be enabled")]
    public bool ModifyAIDamage { get; set; }



    [Description("Percentage of hits that a determined to be normal. Maximum of 100.")]
    public float NormalDamagePercentAI { get; set; }
    [Description("Percentage of hits that a determined to be graze. Maximum of 100.")]
    public float GrazeDamagePercentAI { get; set; }
    [Description("Percentage of hits that a determined to be critical. Maximum of 100.")]
    public float CriticalDamagePercentAI { get; set; }
    [Description("Percentage of hits that a determined to be fatal. Maximum of 100.")]
    public float FatalDamagePercentAI { get; set; }
    [Description("Scalar to modify the base damage done with a normal hit on and armored target and area. Ex. Damage of -25 AP determined to be a normal hit with a Armor_NormalDamageModifier of 1 would be a -25 AP damage hit.")]
    public float Armor_NormalDamageModifierAI { get; set; }
    [Description("Scalar to modify the base damage done with a graze hit on and armored target and area. Ex. Damage of -25 AP determined to be a graze with a Armor_GrazeDamageModifier of 0.25 would be a -5 AP damage hit.")]
    public float Armor_GrazeDamageModifierAI { get; set; }
    [Description("Scalar to modify the base damage done with a critical hit on and armored target and area. Ex. Damage of -25 AP determined to be a critical with a Armor_CriticalDamageModifier of 2 would be a -50 AP damage hit.")]
    public float Armor_CriticalDamageModifierAI { get; set; }
    [Description("Scalar to modify the base damage done with a fatal hit. Ex. Damage of -25 HP determined to be a fatal hit with a Health_FatalDamageModifier of 10 would be a -250 HP damage hit.")]
    public float Health_FatalDamageModifierAI { get; set; }
    [Description("Scalar to modify the base damage done with a normal hit. Ex. Damage of -25 HP determined to be a normal hit with a Health_NormalDamageModifier of 2 would be a -50 HP damage hit.")]
    public float Health_NormalDamageModifierAI { get; set; }
    [Description("Scalar to modify the base damage done with a graze hit. Ex. Damage of -25 HP determined to be a graze with a Health_GrazeDamageModifier of 0.75 would be a -19 HP damage hit.")]
    public float Health_GrazeDamageModifierAI { get; set; }
    [Description("Scalar to modify the base damage done with a critical hit. Ex. Damage of -25 HP determined to be a critical with a Health_CriticalDamageModifier of 3 would be a -75 HP damage hit.")]
    public float Health_CriticalDamageModifierAI { get; set; }



    [Description("Allow peds to yell when damaged.")]
    public bool AllowAIPainYells { get; set; }
    [Description("Health damage taken needed to trigger a pain yell for the AI.")]
    public int AIPainYellsDamageNeeded { get; set; }
    [Description("Allows AI to become unconscious when stunned.")]
    public bool AllowAIUnconsciousOnStun { get; set; }
    [Description("Percentage of time AI will become unconscious when stunned.")]
    public float AIUnconsciousOnStunPercentage { get; set; }
    [Description("Allows AI to become unconscious when damaged.")]
    public bool AllowAIUnconsciousOnDamage { get; set; }
    [Description("Percentage of time AI will become unconscious when damaged.")]
    public float AIUnconsciousOnDamagePercentage { get; set; }

    [Description("Set to lowest possible ped health. (Vanilla is 100)")]
    public int AIUnconsciousOnDamageAliveHealth { get; set; }

    [Description("Minimum health where unconsciousness can start.")]
    public int AIUnconsciousOnDamageMinimumHealth { get; set; }
    [Description("Minimum damage taken where unconsciousness can start.")]
    public int AIUnconsciousOnDamageMinimumHealthChange { get; set; }



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

        AllowInjuryEffects = true;
        InjuryEffectHealthLostStart = 60;
        InjuryEffectIntensityModifier = 0.6f;



        ModifyPlayerDamage = true;

        AllowPlayerPainYells = true;
        PlayerPainYellsDamageNeeded = 15;

        Armor_NormalDamageModifierPlayer = 1.0f;
        Armor_GrazeDamageModifierPlayer = 0.25f;
        Armor_CriticalDamageModifierPlayer = 2.0f;
        Health_FatalDamageModifierPlayer = 10.0f;
        Health_NormalDamageModifierPlayer = 1.5f;
        Health_GrazeDamageModifierPlayer = 0.5f;
        Health_CriticalDamageModifierPlayer = 2.0f;
        
        NormalDamagePercentPlayer = 70f;
        GrazeDamagePercentPlayer = 10f;
        CriticalDamagePercentPlayer = 15;
        FatalDamagePercentPlayer = 5f;





        ModifyAIDamage = true;

        Armor_NormalDamageModifierAI = 1.0f;
        Armor_GrazeDamageModifierAI = 0.25f;
        Armor_CriticalDamageModifierAI = 2.0f;
        Health_FatalDamageModifierAI = 10.0f;
        Health_NormalDamageModifierAI = 1.5f;
        Health_GrazeDamageModifierAI = 0.5f;
        Health_CriticalDamageModifierAI = 2.0f;

        NormalDamagePercentAI = 70f;
        GrazeDamagePercentAI = 10f;
        CriticalDamagePercentAI = 15;
        FatalDamagePercentAI = 5f;


        AllowAIUnconsciousOnStun = true;
        AIUnconsciousOnStunPercentage = 30f;

        AllowAIUnconsciousOnDamage = true;
        AIUnconsciousOnDamagePercentage = 30f;

        AIUnconsciousOnDamageAliveHealth = 100;

        AIUnconsciousOnDamageMinimumHealth = 130;
        AIUnconsciousOnDamageMinimumHealthChange = 20;

        AllowAIPainYells = true;
        AIPainYellsDamageNeeded = 15;




        AllowRagdoll = true;




    }

}