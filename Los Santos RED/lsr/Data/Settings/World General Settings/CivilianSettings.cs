using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CivilianSettings : ISettingsDefaultable
{

    [Description("Allows tasking of ambient civilians in the world.")]
    public bool ManageCivilianTasking { get; set; }
    [Description("Percentage of the time security guards will fight over flee. Percentage with a max of 100.")]
    public float SecurityFightPercentage { get; set; }
    [Description("Allows settings custom health values on ambient civilians.")]
    public bool OverrideHealth { get; set; }
    [Description("Minimum health value on ambient civilians.")]
    public int MinHealth { get; set; }
    [Description("Maximum health value on ambient civilians.")]
    public int MaxHealth { get; set; }
    [Description("Allows setting custom accuracy values on ambient civilians.")]
    public bool OverrideAccuracy { get; set; }
    [Description("Accuracy value to override. Percentage with a max of 100.")]
    public int GeneralAccuracy { get; set; }
    [Description("How far away in meters civilians will be able to see you. Maximum value of 90.")]
    public float SightDistance { get; set; }
    [Description("How far away in meters civilians will be able to hear non-suppressed shooting. Maximum value of 200.")]
    public float GunshotHearingDistance { get; set; }
    [Description("Allows tasking of mission or other mod spawned civilians in the world.")]
    public bool TaskMissionPeds { get; set; }
    [Description("Allows hold ups and talking of mission or other mod spawned civilians in the world.")]
    public bool AllowMissionPedsToInteract { get; set; }
    [Description("Allows random crimes to happen in the world.")]
    public bool AllowRandomCrimes { get; set; }
    [Description("Minumum time required between reandom crimes..")]
    public uint MinimumTimeBetweenRandomCrimes { get; set; }
    [Description("Place a blip on the criminal when a random crime is generated.")]
    public bool ShowRandomCriminalBlips { get; set; }
    [Description("Check and enforce crimes committed by ambient citizens. Required for police to react to civilian crimes.")]
    public bool CheckCivilianCrimes { get; set; }
    [Description("Allows civilians to be aware of crimes committed by other civilians and react accordingly. Required for civilians to react to other civilian crimes.")]
    public bool AllowCivilinsToCallPoliceOnOtherCivilians { get; set; }

    [Description("Percentage of civilians that will have illicit items to buy and sell in the rich zones (See Zones.xml). Maximum value of 100.")]
    public float DrugDealerPercentageRichZones { get; set; }
    [Description("Percentage of civilians that will have illicit items to buy and sell in the middle income zones (See Zones.xml). Maximum value of 100.")]
    public float DrugDealerPercentageMiddleZones { get; set; }
    [Description("Percentage of civilians that will have illicit items to buy and sell in the poor zones (See Zones.xml). Maximum value of 100.")]
    public float DrugDealerPercentagePoorZones { get; set; }
    [Description("Percentage of civilians that will have illicit items to buy in the rich zones (See Zones.xml). Maximum value of 100.")]
    public float DrugCustomerPercentageRichZones { get; set; }
    [Description("Percentage of civilians that will have illicit items to buy in the middle income zones (See Zones.xml). Maximum value of 100.")]
    public float DrugCustomerPercentageMiddleZones { get; set; }
    [Description("Percentage of civilians that will have illicit items to buy in the poor zones (See Zones.xml). Maximum value of 100.")]
    public float DrugCustomerPercentagePoorZones { get; set; }
    [Description("Percentage of civilians that will call the police on criminals in the rich zones (See Zones.xml). Maximum value of 100.")]
    public float CallPolicePercentageRichZones { get; set; }
    [Description("Percentage of civilians that will call the police on criminals in the middle income zones (See Zones.xml). Maximum value of 100.")]
    public float CallPolicePercentageMiddleZones { get; set; }
    [Description("Percentage of civilians that will call the police on criminals in the poor zones (See Zones.xml). Maximum value of 100.")]
    public float CallPolicePercentagePoorZones { get; set; }
    [Description("Percentage of civilians that will attack criminals in the rich zones (See Zones.xml). Maximum value of 100.")]
    public float FightPercentageRichZones { get; set; }
    [Description("Percentage of civilians that will attack criminals in the middle income zones (See Zones.xml). Maximum value of 100.")]
    public float FightPercentageMiddleZones { get; set; }
    [Description("Percentage of civilians that will attack criminals in the poor zones (See Zones.xml). Maximum value of 100.")]
    public float FightPercentagePoorZones { get; set; }
    [Description("Minumum amount of money a merchant will surrender upon mugging.")]
    public int MerchantMoneyMin { get; set; }
    [Description("Maximum amount of money a merchant will surrender upon mugging.")]
    public int MerchantMoneyMax { get; set; }
    [Description("Minumum amount of money a civilian will surrender upon mugging.")]
    public int MoneyMin { get; set; }
    [Description("Maximum amount of money a civilian will surrender upon mugging.")]
    public int MoneyMax { get; set; }
    [Description("Percentage of civilians that will follow you to do deals. Maximum value of 100.")]
    public float PercentageTrustingOfPlayer { get; set; }


    [Description("Percentage of time you will get random items when looting a random ped (weapons and vehicles excluded)")]
    public float PercentageToGetRandomItems { get; set; }
    [Description("Max number of random items to get when looting a random ped (weapons and vehicles excluded). Requires PercentageToGetRandomItems > 0")]
    public int MaxRandomItemsToGet { get; set; }
    [Description("Max amount to get for each random item when looting a random ped (weapons and vehicles excluded). Requires PercentageToGetRandomItems > 0")]
    public int MaxRandomItemsAmount { get; set; }

    public CivilianSettings()
    {
        SetDefault();
#if DEBUG
        ShowRandomCriminalBlips = true;
       // OverrideHealth = false;
#else

#endif
    }
    public void SetDefault()
    {
        ManageCivilianTasking = true;
        SecurityFightPercentage = 30f;//70f
        OverrideHealth = true;
        MinHealth = 70;
        MaxHealth = 100;
        OverrideAccuracy = true;
        GeneralAccuracy = 5;//10
        SightDistance = 80f;//70f;//90f
        GunshotHearingDistance = 125f;//100f
        TaskMissionPeds = false;
        AllowMissionPedsToInteract = false;
        AllowRandomCrimes = true;
        MinimumTimeBetweenRandomCrimes = 1200000;
        CheckCivilianCrimes = true;
        AllowCivilinsToCallPoliceOnOtherCivilians = true;
        ShowRandomCriminalBlips = false;
        DrugDealerPercentageRichZones = 1f;
        DrugDealerPercentageMiddleZones = 2f;
        DrugDealerPercentagePoorZones = 5f;

        DrugCustomerPercentageRichZones = 5f;
        DrugCustomerPercentageMiddleZones = 7f;
        DrugCustomerPercentagePoorZones = 10f;

        CallPolicePercentageRichZones = 50f;
        CallPolicePercentageMiddleZones = 30f;
        CallPolicePercentagePoorZones = 20f;

        FightPercentageRichZones = 0f;
        FightPercentageMiddleZones = 1f;
        FightPercentagePoorZones = 2f;

        MerchantMoneyMin = 500;
        MerchantMoneyMax = 2000;

        MoneyMin = 15;
        MoneyMax = 550;
        PercentageTrustingOfPlayer = 85f;
        PercentageToGetRandomItems = 80f;
        MaxRandomItemsToGet = 5;
        MaxRandomItemsAmount = 3;
    }

}