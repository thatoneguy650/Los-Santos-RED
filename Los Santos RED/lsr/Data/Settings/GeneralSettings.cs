using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GeneralSettings
{
    public bool PedSwap_AliasPedAsMainCharacter { get; set; } = true;
    public string PedSwap_MainCharacterToAlias { get; set; } = "Michael";
    public bool PedSwap_SetRandomMoney { get; set; } = true;
    public int PedSwap_RandomMoneyMin { get; set; } = 500;
    public int PedSwap_RandomMoneyMax { get; set; } = 5000;
    public int PedSwap_PercentageToGetRandomWeapon { get; set; } = 40;
    public int PedSwap_PercentageToGetCriminalHistory { get; set; } = 20;

    public string PedSwap_AliasModelName
    {
        get
        {
            if (PedSwap_MainCharacterToAlias == "Michael")
                return "player_zero";
            else if (PedSwap_MainCharacterToAlias == "Franklin")
                return "player_one";
            else if (PedSwap_MainCharacterToAlias == "Trevor")
                return "player_two";
            else
                return "player_zero";
        }
    }

    public bool HealthState_ModifyDamage { get; set; } = true;
    public bool HealthState_ClearDamage { get; set; } = true;
    public float HealthState_Armor_NormalDamageModifier { get; set; } = 1.0f;
    public float HealthState_Armor_GrazeDamageModifier { get; set; } = 0.25f;
    public float HealthState_Armor_CriticalDamageModifier { get; set; } = 2.0f;
    public float HealthState_Health_FatalDamageModifier { get; set; } = 10.0f;
    public float HealthState_Health_NormalDamageModifier { get; set; } = 2.0f;
    public float HealthState_Health_GrazeDamageModifier { get; set; } = 0.75f;
    public float HealthState_Health_CriticalDamageModifier { get; set; } = 3.0f;
    public bool HealthState_AllowRagdoll { get; set; } = true;
    public float HealthState_NormalDamagePercent { get; set; } = 60f;
    public float HealthState_GrazeDamagePercent { get; set; } = 10f;
    public float HealthState_CriticalDamagePercent { get; set; } = 22f;
    public float HealthState_FatalDamagePercent { get; set; } = 8f;

    public bool Vanilla_TerminateRespawn { get; set; } = true;
    public bool Vanilla_TerminateDispatch { get; set; } = true;
    public bool Vanilla_TerminateHealthRecharge { get; set; } = true;
    public bool Vanilla_TerminateWantedMusic { get; set; } = true;
    public bool Vanilla_TerminateScanner { get; set; } = true;
    public bool Vanilla_TerminateScenarioCops { get; set; } = false;

    public bool Tasker_TaskCivilians { get; set; } = true;

    public bool Time_ScaleTime { get; set; } = true;

    public float Civilians_FightPercentage { get; set; } = 5f;
    public float Civilians_CallPolicePercentage { get; set; } = 55f;
    public bool Civilians_SecurityAlwaysFights { get; set; } = true;
    public bool Civilians_GangAlwaysFights { get; set; } = true;
    public int Civilians_MinHealth { get; set; } = 70;
    public int Civilians_MaxHealth { get; set; } = 100;

    public bool World_AddPOIBlipsToMap { get; set; } = true;
    public bool World_UpdateVehiclePlates { get; set; } = true;
    public bool World_CleanupVehicles { get; set; } = true;

    public bool Dispatch_DispatchLE { get; set; } = true;
    public bool Dispatch_DispatchEMS { get; set; } = true;
    public bool Dispatch_DispatchFire { get; set; } = true;
    public int Dispatch_PoliceMax_Default { get; set; } = 5;
    public int Dispatch_PoliceMax_Investigation { get; set; } = 6;
    public int Dispatch_PoliceMax_Wanted1 { get; set; } = 7;
    public int Dispatch_PoliceMax_Wanted2 { get; set; } = 10;
    public int Dispatch_PoliceMax_Wanted3 { get; set; } = 18;
    public int Dispatch_PoliceMax_Wanted4 { get; set; } = 25;
    public int Dispatch_PoliceMax_Wanted5 { get; set; } = 35;

    //need distance, and delete criteria settings as well
    public GeneralSettings()
    {

    }

}