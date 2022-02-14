using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CivilianSettings : ISettingsDefaultable
{
    public bool ManageCivilianTasking { get; set; }
  //  public float FightPercentage { get; set; }
   // public float CallPolicePercentage { get; set; }
    public float SecurityFightPercentage { get; set; }
    public bool OverrideHealth { get; set; }
    public int MinHealth { get; set; }
    public int MaxHealth { get; set; }
    public bool OverrideAccuracy { get; set; }
    public int GeneralAccuracy { get; set; }
    public float SightDistance { get; set; }
    public float GunshotHearingDistance { get; set; }
    public bool TaskMissionPeds { get; set; }
    public bool AllowMissionPedsToInteract { get; set; }
    public bool AllowRandomCrimes { get; set; }
    public uint MinimumTimeBetweenRandomCrimes { get; set; }
    public bool CheckCivilianCrimes { get; set; }
    public bool AllowCivilinsToCallPoliceOnOtherCivilians { get; set; }
    public bool ShowRandomCriminalBlips { get; set; }
    //public float DrugDealerPercentage { get; set; }


    public float DrugDealerPercentageRichZones { get; set; }
    public float DrugDealerPercentageMiddleZones { get; set; }
    public float DrugDealerPercentagePoorZones { get; set; }

    public float DrugCustomerPercentageRichZones { get; set; }
    public float DrugCustomerPercentageMiddleZones { get; set; }
    public float DrugCustomerPercentagePoorZones { get; set; }

    public float CallPolicePercentageRichZones { get; set; }
    public float CallPolicePercentageMiddleZones { get; set; }
    public float CallPolicePercentagePoorZones { get; set; }

    public float FightPercentageRichZones { get; set; }
    public float FightPercentageMiddleZones { get; set; }
    public float FightPercentagePoorZones { get; set; }
    public int MerchantMoneyMin { get; set; }
    public int MerchantMoneyMax { get; set; }
    public int MoneyMin { get; set; }
    public int MoneyMax { get; set; }

    public CivilianSettings()
    {
        SetDefault();
#if DEBUG
        ShowRandomCriminalBlips = true;
#else

#endif
    }
    public void SetDefault()
    {
        ManageCivilianTasking = true;
        //FightPercentage = 2f;//7f//5f//let
        //CallPolicePercentage = 25f;//65f;//55f
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
        MinimumTimeBetweenRandomCrimes = 300000;
        CheckCivilianCrimes = true;
        AllowCivilinsToCallPoliceOnOtherCivilians = true;
        ShowRandomCriminalBlips = false;
       // DrugDealerPercentage = 5f;
        DrugDealerPercentageRichZones = 1f;
        DrugDealerPercentageMiddleZones = 2f;
        DrugDealerPercentagePoorZones = 5f;

        DrugCustomerPercentageRichZones = 5f;
        DrugCustomerPercentageMiddleZones = 7f;
        DrugCustomerPercentagePoorZones = 10f;




        CallPolicePercentageRichZones = 55f;
        CallPolicePercentageMiddleZones = 25f;
        CallPolicePercentagePoorZones = 10f;

        FightPercentageRichZones = 0f;
        FightPercentageMiddleZones = 1f;
        FightPercentagePoorZones = 2f;

        MerchantMoneyMin = 500;
        MerchantMoneyMax = 2000;

        MoneyMin = 15;
        MoneyMax = 100;


    }

}