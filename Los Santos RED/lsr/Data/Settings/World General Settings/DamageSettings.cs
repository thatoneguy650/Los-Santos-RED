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









    [Description("Allow bleeding damage on some hits.")]
    public bool AllowBleedingPlayer { get; set; }
    public int BleedingPercentagePlayer { get; set; }
    public int BleedingMinDamageRequirementPlayer { get; set; }
    public int HealthLostEachBleedPlayer { get; set; }
    public uint MinGameTimeBetweenBleedsPlayer { get; set; }
    public uint MaxGameTimeBetweenBleedsPlayer { get; set; }
    public int BleedingStopPercentagePlayer { get; set; }  
    public int BleedingLightDamageAmountPlayer { get; set; }
    public int BleedingMediumDamageAmountPlayer { get; set; }
    public int BleedingHeavyDamageAmountPlayer { get; set; }
    public bool AllowBloodTrailsPlayer { get; set; }





    public bool AllowBleedingAI { get; set; }
    public int BleedingPercentageAI { get; set; }
    public int BleedingMinDamageRequirementAI { get; set; }
    public int HealthLostEachBleedAI { get; set; }
    public uint MinGameTimeBetweenBleedsAI { get; set; }
    public uint MaxGameTimeBetweenBleedsAI { get; set; }
    public int BleedingStopPercentageAI { get; set; }
    public int BleedingLightDamageAmountAI { get; set; }
    public int BleedingMediumDamageAmountAI { get; set; }
    public int BleedingHeavyDamageAmountAI { get; set; }
    public bool AllowBloodTrailsAI { get; set; }





    [Description("Allow additional ragdoll on critical and fatal hits.")]
    public bool AllowRagdollPlayer { get; set; }
    public int AlwaysRagdollDamageMinimumPlayer { get; set; }
    public int MediumRagdollDamageMinimumPlayer { get; set; }
    public int MediumRagdollDamagePercentagePlayer { get; set; }
    public int LowRagdollDamageMinimumPlayer { get; set; }
    public int LowRagdollDamagePercentagePlayer { get; set; }
    public int TinyRagdollDamageMinimumPlayer { get; set; }
    public int TinyRagdollDamagePercentagePlayer { get; set; }
    public uint AlwaysRagdollMinTimePlayer { get; set; }
    public uint AlwaysRagdollMaxTimePlayer { get; set; }
    public uint MediumRagdollMinTimePlayer { get; set; }
    public uint MediumRagdollMaxTimePlayer { get; set; }
    public uint LowRagdollMinTimePlayer { get; set; }
    public uint LowRagdollMaxTimePlayer { get; set; }
    public uint TinyRagdollMinTimePlayer { get; set; }
    public uint TinyRagdollMaxTimePlayer { get; set; }
    public bool EjectPedFromVehicleOnRagdollPlayer { get; set; }
    public int EjectPedFromVehicleOnRagdollPercentagePlayer { get; set; }



    public bool AllowRagdollAI { get; set; }
    public int AlwaysRagdollDamageMinimumAI { get; set; }
    public int MediumRagdollDamageMinimumAI { get; set; }
    public int MediumRagdollDamagePercentageAI { get; set; }
    public int LowRagdollDamageMinimumAI { get; set; }
    public int LowRagdollDamagePercentageAI { get; set; }
    public int TinyRagdollDamageMinimumAI { get; set; }
    public int TinyRagdollDamagePercentageAI { get; set; }
    public uint AlwaysRagdollMinTimeAI { get; set; }
    public uint AlwaysRagdollMaxTimeAI { get; set; }
    public uint MediumRagdollMinTimeAI { get; set; }
    public uint MediumRagdollMaxTimeAI { get; set; }
    public uint LowRagdollMinTimeAI { get; set; }
    public uint LowRagdollMaxTimeAI { get; set; }
    public uint TinyRagdollMinTimeAI { get; set; }
    public uint TinyRagdollMaxTimeAI { get; set; }
    public bool EjectPedFromVehicleOnRagdollAI { get; set; }
    public int EjectPedFromVehicleOnRagdollPercentageAI { get; set; }

    public DamageSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        ModifyDamage = true;
        ClearDamage = true;

        //Player UI
        AllowInjuryEffects = true;
        InjuryEffectHealthLostStart = 60;
        InjuryEffectIntensityModifier = 0.6f;


        //Player Damage
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

        //AI Damage
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

        //AI Other
        AllowAIUnconsciousOnStun = true;
        AIUnconsciousOnStunPercentage = 30f;
        AllowAIUnconsciousOnDamage = true;
        AIUnconsciousOnDamagePercentage = 30f;
        AIUnconsciousOnDamageAliveHealth = 100;
        AIUnconsciousOnDamageMinimumHealth = 130;
        AIUnconsciousOnDamageMinimumHealthChange = 20;

        AllowAIPainYells = true;
        AIPainYellsDamageNeeded = 15;


        //Bleeding Player
        AllowBleedingPlayer = true;
        BleedingPercentagePlayer = 40;
        BleedingMinDamageRequirementPlayer = 8;
        HealthLostEachBleedPlayer = 2;
        MinGameTimeBetweenBleedsPlayer = 2000;// 1200;
        MaxGameTimeBetweenBleedsPlayer = 9000;// 3500;
        BleedingStopPercentagePlayer = 2;
        BleedingLightDamageAmountPlayer = 10;
        BleedingMediumDamageAmountPlayer = 20;
        BleedingHeavyDamageAmountPlayer = 50;
        AllowBloodTrailsPlayer = true;

        //Bleeding AI
        AllowBleedingAI = true;
        BleedingPercentageAI = 15;
        BleedingMinDamageRequirementAI = 8;
        HealthLostEachBleedAI = 2;
        MinGameTimeBetweenBleedsAI = 2000;// 1200;
        MaxGameTimeBetweenBleedsAI = 9000;// 3500;
        BleedingStopPercentageAI = 15;
        BleedingLightDamageAmountAI = 10;
        BleedingMediumDamageAmountAI = 20;
        BleedingHeavyDamageAmountAI = 50;
        AllowBloodTrailsAI = true;

        //Ragdoll Player
        AllowRagdollPlayer = true;
        AlwaysRagdollDamageMinimumPlayer = 55;
        AlwaysRagdollMinTimePlayer = 5000;
        AlwaysRagdollMaxTimePlayer = 15000;
        MediumRagdollDamageMinimumPlayer = 25;
        MediumRagdollDamagePercentagePlayer = 55;
        MediumRagdollMinTimePlayer = 4000;
        MediumRagdollMaxTimePlayer = 10000;
        LowRagdollDamageMinimumPlayer = 15;
        LowRagdollDamagePercentagePlayer = 40;
        LowRagdollMinTimePlayer = 2000;
        LowRagdollMaxTimePlayer = 7000;
        TinyRagdollDamageMinimumPlayer = 10;
        TinyRagdollDamagePercentagePlayer = 2;
        TinyRagdollMinTimePlayer = 1000;
        TinyRagdollMaxTimePlayer = 3000;
        EjectPedFromVehicleOnRagdollPlayer = true;
        EjectPedFromVehicleOnRagdollPercentagePlayer = 20;

        //Ragdoll AI
        AllowRagdollAI = true;
        AlwaysRagdollDamageMinimumAI = 55;
        AlwaysRagdollMinTimeAI = 5000;
        AlwaysRagdollMaxTimeAI = 15000;
        MediumRagdollDamageMinimumAI = 25;
        MediumRagdollDamagePercentageAI = 25;
        MediumRagdollMinTimeAI = 4000;
        MediumRagdollMaxTimeAI = 10000;
        LowRagdollDamageMinimumAI = 15;
        LowRagdollDamagePercentageAI = 15;
        LowRagdollMinTimeAI = 2000;
        LowRagdollMaxTimeAI = 7000;
        TinyRagdollDamageMinimumAI = 10;
        TinyRagdollDamagePercentageAI = 1;
        TinyRagdollMinTimeAI = 1000;
        TinyRagdollMaxTimeAI = 3000;
        EjectPedFromVehicleOnRagdollAI = true;
        EjectPedFromVehicleOnRagdollPercentageAI = 15;
    }

}