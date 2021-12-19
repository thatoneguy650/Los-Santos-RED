using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RespawnSettings
{


    public RespawnSettings()
    {

    }

    public bool DeductBailFee { get; set; } = true;
    public bool RemoveWeaponsOnSurrender { get; set; } = true;
    public bool DeductHospitalFee { get; set; } = true;
    public bool RemoveWeaponsOnDeath { get; set; } = true;
    public bool DeductMoneyOnFailedBribe { get; set; } = true;
    public uint RecentlyRespawnedTime { get; set; } = 1000;
    public uint RecentlyResistedArrestTime { get; set; } = 5000;
    public bool InvincibilityOnRespawn { get; set; } = true;
    public int RespawnInvincibilityTime { get; set; } = 5000;
    public bool AllowRandomGraveRespawn { get; set; } = true;
    public float RandomGraveRespawnPercentage { get; set; } = 10f;
    public bool AllowUndie { get; set; } = true;
    public int UndieLimit { get; set; } = 0;
    public int PoliceBribeWantedLevelScale { get; set; } = 500;
    public int PoliceBailWantedLevelScale { get; set; } = 750;
    public int HospitalFee { get; set; } = 5000;
    public bool PermanentDeathMode { get; set; } = false;
    public bool ClearInventoryOnDeath { get; set; } = true;
}