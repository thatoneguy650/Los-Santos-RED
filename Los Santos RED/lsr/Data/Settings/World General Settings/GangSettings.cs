using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GangSettings : ISettingsDefaultable
{
    public bool ManageTasking { get; set; }
    public float FightPercentage { get; set; }
    public float CallPolicePercentage { get; set; }
    public float SecurityFightPercentage { get; set; }
    public float GangFightPercentage { get; set; }
    public bool CheckCivilianCrimes { get; set; }
    public bool ShowRandomCriminalBlips { get; set; }
    public float GangDrugDealPercentage { get; set; }
    public float RandomDrugDealPercent { get; set; }
    public bool ShowSpawnedGangBlip { get; set; }
    public bool RemoveVanillaGangs { get; set; }

    public GangSettings()
    {
        SetDefault();
#if DEBUG
        ShowSpawnedGangBlip = true;
        //RemoveVanillaGangs = true;
#else
               // ShowSpawnedBlips = false;
#endif
    }
    public void SetDefault()
    {
        ManageTasking = true;
        FightPercentage = 2f;//7f//5f//let
        CallPolicePercentage = 25f;//65f;//55f
        SecurityFightPercentage = 30f;//70f
        GangFightPercentage = 55f;//85f
        CheckCivilianCrimes = true;
        ShowRandomCriminalBlips = true;
        GangDrugDealPercentage = 40f;
        RandomDrugDealPercent = 5f;
        ShowSpawnedGangBlip = false;
        RemoveVanillaGangs = false;
    }

}