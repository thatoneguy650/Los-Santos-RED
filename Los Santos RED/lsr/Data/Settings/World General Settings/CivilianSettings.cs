using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

public class CivilianSettings : ISettingsDefaultable
{
    [Description("Allows mod spawning of civilians (store owners, merchants, tellers, etc.) in the world.")]
    public bool ManageDispatching { get; set; }
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
    [Description("Minumum time required between random crimes..")]
    public uint MinimumTimeBetweenRandomCrimes { get; set; }

    [Description("Minumum time required between random traffic crimes..")]
    public uint MinimumTimeBetweenRandomTrafficCrimes { get; set; }


    [Description("Place a blip on the criminal when a random crime is generated.")]
    public bool ShowRandomCriminalBlips { get; set; }
    [Description("Check and enforce crimes committed by ambient citizens. Required for police to react to civilian crimes.")]
    public bool CheckCivilianCrimes { get; set; }
    [Description("Allows civilians to be aware of crimes committed by other civilians and react accordingly. Required for civilians to react to other civilian crimes.")]
    public bool AllowCivilinsToCallPoliceOnOtherCivilians { get; set; }


    [Description("Allows civilians to be aware of hurt peds in the world. Required for civilians to call EMS.")]
    public bool AllowAlerts { get; set; }


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


    [Description("Percentage of civilians that will call the police on intense criminal activity (shooting, killing, etc.) in the rich zones (See Zones.xml). Maximum value of 100.")]
    public float CallPoliceForSeriousCrimesPercentageRichZones { get; set; }
    [Description("Percentage of civilians that will call the police on intense criminal activity (shooting, killing, etc.) in the middle income zones (See Zones.xml). Maximum value of 100.")]
    public float CallPoliceForSeriousCrimesPercentageMiddleZones { get; set; }
    [Description("Percentage of civilians that will call the police on intense criminal activity (shooting, killing, etc.) in the poor zones (See Zones.xml). Maximum value of 100.")]
    public float CallPoliceForSeriousCrimesPercentagePoorZones { get; set; }



    [Description("Percentage of civilians that will attack criminals in the rich zones (See Zones.xml). Maximum value of 100.")]
    public float FightPercentageRichZones { get; set; }
    [Description("Percentage of civilians that will attack criminals in the middle income zones (See Zones.xml). Maximum value of 100.")]
    public float FightPercentageMiddleZones { get; set; }
    [Description("Percentage of civilians that will attack criminals in the poor zones (See Zones.xml). Maximum value of 100.")]
    public float FightPercentagePoorZones { get; set; }


    [Description("Percentage of civilians that will attack criminals in the rich zones (See Zones.xml). Maximum value of 100.")]
    public float FightPolicePercentageRichZones { get; set; }
    [Description("Percentage of civilians that will attack criminals in the middle income zones (See Zones.xml). Maximum value of 100.")]
    public float FightPolicePercentageMiddleZones { get; set; }
    [Description("Percentage of civilians that will attack criminals in the poor zones (See Zones.xml). Maximum value of 100.")]
    public float FightPolicePercentagePoorZones { get; set; }




    [Description("Percentage of civilians that will cower instead of flee  in the rich zones (See Zones.xml). Maximum value of 100.")]
    public float CowerPercentageRichZones { get; set; }
    [Description("Percentage of civilians that will cower instead of flee  in the middle income zones (See Zones.xml). Maximum value of 100.")]
    public float CowerPercentageMiddleZones { get; set; }
    [Description("Percentage of civilians that will cower instead of flee in the poor zones (See Zones.xml). Maximum value of 100.")]
    public float CowerPercentagePoorZones { get; set; }









    //[Description("Minumum amount of money a merchant will surrender upon mugging.")]
    //public int MerchantMoneyMin { get; set; }
    //[Description("Maximum amount of money a merchant will surrender upon mugging.")]
    //public int MerchantMoneyMax { get; set; }
    //[Description("Minumum amount of money a civilian will surrender upon mugging.")]
    //public int MoneyMin { get; set; }
    //[Description("Maximum amount of money a civilian will surrender upon mugging.")]
    //public int MoneyMax { get; set; }




    [Description("Minumum amount of money a merchant will surrender upon mugging in rich zones.")]
    public int MerchantMoneyMinRichZones { get; set; }
    [Description("Maximum amount of money a merchant will surrender upon mugging in rich zones.")]
    public int MerchantMoneyMaxRichZones { get; set; }

    [Description("Minumum amount of money a merchant will surrender upon mugging in middle zones.")]
    public int MerchantMoneyMinMiddleZones { get; set; }
    [Description("Maximum amount of money a merchant will surrender upon mugging in middle zones.")]
    public int MerchantMoneyMaxMiddleZones { get; set; }

    [Description("Minumum amount of money a merchant will surrender upon mugging in poor zones.")]
    public int MerchantMoneyMinPoorZones { get; set; }
    [Description("Maximum amount of money a merchant will surrender upon mugging in poor zones.")]
    public int MerchantMoneyMaxPoorZones { get; set; }




    [Description("Minumum amount of money a civilian will surrender upon mugging in rich zones.")]
    public int MoneyMinRichZones { get; set; }
    [Description("Maximum amount of money a civilian will surrender upon mugging in rich zones.")]
    public int MoneyMaxRichZones { get; set; }
    [Description("Minumum amount of money a civilian will surrender upon mugging in middle zones.")]
    public int MoneyMinMiddleZones { get; set; }
    [Description("Maximum amount of money a civilian will surrender upon mugging in middle zones.")]
    public int MoneyMaxMiddleZones { get; set; }
    [Description("Minumum amount of money a civilian will surrender upon mugging in poor zones.")]
    public int MoneyMinPoorZones { get; set; }
    [Description("Maximum amount of money a civilian will surrender upon mugging in poor zones.")]
    public int MoneyMaxPoorZones { get; set; }






    //[Description("Minumum amount of money a merchant will surrender upon mugging.")]
    //public int MerchantMoneyMin { get; set; }



    [Description("Percentage of civilians that will follow you to do deals. Maximum value of 100.")]
    public float PercentageTrustingOfPlayer { get; set; }
















    public float PercentageKnowsAnyGangTerritory { get; set; }
    public float PercentageKnowsAnyDrugTerritory { get; set; }


    public bool AllowCallInIfPedDoesNotExist { get; set; }
    
    public uint GameTimeToCallInMinimum { get; set; }
    public uint GameTimeToCallInMaximum { get; set; }
    public uint GameTimeToCallInIfPedDoesNotExist { get; set; }


    public uint GameTimeAfterCallInToReportCrime { get; set; }
    public bool DisableWrithe { get; set; }
    public bool DisableWritheShooting { get; set; }




    [Description("Total limit of spawned service peds (teller, vendor, etc.).")]
    public int TotalSpawnedServiceMembersLimit { get; set; }


    public int PossibleSurrenderPercentage { get; set; } = 40;
    public int WantedPossibleSurrenderPercentage { get; set; } = 10;





    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        SetDefault();
    }

    public CivilianSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        ManageDispatching = true;
        ManageCivilianTasking = true;
        SecurityFightPercentage = 30f;//70f
        OverrideHealth = true;
        MinHealth = 70;
        MaxHealth = 100;
        OverrideAccuracy = true;
        GeneralAccuracy = 5;//10
        SightDistance = 60f;//70f;//90f
        GunshotHearingDistance = 125f;//100f
        TaskMissionPeds = false;
        AllowMissionPedsToInteract = false;
        AllowRandomCrimes = true;
        MinimumTimeBetweenRandomCrimes = 1200000;


        MinimumTimeBetweenRandomTrafficCrimes = 720000;

        CheckCivilianCrimes = true;
        AllowCivilinsToCallPoliceOnOtherCivilians = true;
        AllowAlerts = true;
        ShowRandomCriminalBlips = false;

        DrugDealerPercentageRichZones = 12f;// 1f;
        DrugDealerPercentageMiddleZones = 15f;// 2f;
        DrugDealerPercentagePoorZones = 25f;//5f;

        DrugCustomerPercentageRichZones = 20f;// 5f;
        DrugCustomerPercentageMiddleZones = 35f;// 7f;
        DrugCustomerPercentagePoorZones = 40f;// 10f;

        CallPolicePercentageRichZones = 25f;// 50f;
        CallPolicePercentageMiddleZones = 10f;// 30f;
        CallPolicePercentagePoorZones = 5f;// 20f;


        CallPoliceForSeriousCrimesPercentageRichZones = 45f;// 80f;
        CallPoliceForSeriousCrimesPercentageMiddleZones = 35f;// 70f;
        CallPoliceForSeriousCrimesPercentagePoorZones = 30f;// 50f;



        FightPercentageRichZones = 1f;// 0f;
        FightPercentageMiddleZones = 2f;// 1f;
        FightPercentagePoorZones = 4f;// 2f;


        FightPolicePercentageRichZones = 1f;
        FightPolicePercentageMiddleZones = 2f;
        FightPolicePercentagePoorZones = 5f;



        CowerPercentageRichZones = 10f;//HAS DESERIALIZED VALUES
        CowerPercentageMiddleZones = 3f;//HAS DESERIALIZED VALUES
        CowerPercentagePoorZones = 1f;//HAS DESERIALIZED VALUES


        MerchantMoneyMinRichZones = 100;
        MerchantMoneyMaxRichZones = 600;

        MerchantMoneyMinMiddleZones = 100;
        MerchantMoneyMaxMiddleZones = 400;
        MerchantMoneyMinPoorZones = 1;
        MerchantMoneyMaxPoorZones = 200;

        MoneyMinRichZones = 100;
        MoneyMaxRichZones = 750;
        MoneyMinMiddleZones = 25;
        MoneyMaxMiddleZones = 350; 
        MoneyMinPoorZones = 1;
        MoneyMaxPoorZones = 250;




        PercentageTrustingOfPlayer = 85f;


        PercentageKnowsAnyGangTerritory = 65f;
        PercentageKnowsAnyDrugTerritory = 65f;

        AllowCallInIfPedDoesNotExist = true;
        GameTimeToCallInIfPedDoesNotExist = 4000;
        GameTimeToCallInMinimum = 6000;
        GameTimeToCallInMaximum = 10000;

        GameTimeAfterCallInToReportCrime = 2000;
        DisableWrithe = true;
        DisableWritheShooting = true;

        TotalSpawnedServiceMembersLimit = 9;

        PossibleSurrenderPercentage = 40;
        WantedPossibleSurrenderPercentage = 10;
    }

}