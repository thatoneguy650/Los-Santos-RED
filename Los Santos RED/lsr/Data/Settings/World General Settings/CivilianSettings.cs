using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CivilianSettings : ISettingsDefaultable
{
    public bool ManageCivilianTasking { get; set; }
    public float FightPercentage { get; set; }
    public float CallPolicePercentage { get; set; }
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
    public float DrugDealerPercentage { get; set; }

    public CivilianSettings()
    {
        SetDefault();
#if DEBUG

#else

#endif
    }
    public void SetDefault()
    {
        ManageCivilianTasking = true;
        FightPercentage = 2f;//7f//5f//let
        CallPolicePercentage = 25f;//65f;//55f
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
        ShowRandomCriminalBlips = true;
        DrugDealerPercentage = 5f;
    }

}