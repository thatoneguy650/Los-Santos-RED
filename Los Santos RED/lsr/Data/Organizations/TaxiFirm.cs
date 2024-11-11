using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TaxiFirm : Organization
{
    public bool IsDefault { get; set; } = false;
    public int BaseFare { get; set; } = 10;
    public int PricePerMile { get; set; } = 5;
    public int FastSpeedFee { get; set; } = 20;
    public int CrazySpeedFee { get; set; } = 100;
    public float FightPercentage { get; set; } = 35f;
    public float FightPolicePercentage { get; set; } = 5f;
    public float AlwaysFightPolicePercentage { get; set; } = 1f;
    public float CallPolicePercentage { get; set; } = 35f;
    public float CallPoliceForSeriousCrimesPercentage { get; set; } = 70f;
    public float PercentageWithLongGuns { get; set; } = 5f;
    public float PercentageWithSidearms { get; set; } = 30f;
    public float PercentageWithMelee { get; set; } = 10f;
    public bool IsRideShare { get; set; } = false;
    public TaxiFirm()
    {
    }

    public TaxiFirm(string _ColorPrefix, string _ID, string _shortName, string _FullName, string _AgencyColorString, string _DispatchablePeropleGroupID, string _DispatchableVehicleGroupID, string _LicensePlatePrefix, string meleeWeaponsID, string sideArmsID, string longGunsID, string groupName) : base(_ColorPrefix, _ID, _shortName, _FullName, _AgencyColorString, _DispatchablePeropleGroupID, _DispatchableVehicleGroupID, _LicensePlatePrefix, meleeWeaponsID, sideArmsID, longGunsID, groupName)
    {

    }
    public int CalculateFare(float distance)
    {
        int totalFare = BaseFare;
        int AdditionalFare = (int)Math.Ceiling(distance * 3);
        return totalFare + AdditionalFare;
    }

    public void SetPedStats(TaxiDriver createdPedExt)
    {
        if(createdPedExt== null)
        {
            return;
        }
        createdPedExt.WillFight = RandomItems.RandomPercent(FightPercentage);
        createdPedExt.WillCallPolice = RandomItems.RandomPercent(CallPolicePercentage);
        createdPedExt.WillCallPoliceIntense = RandomItems.RandomPercent(CallPoliceForSeriousCrimesPercentage);
        createdPedExt.WillFightPolice = RandomItems.RandomPercent(FightPolicePercentage);
        createdPedExt.WillAlwaysFightPolice = RandomItems.RandomPercent(AlwaysFightPolicePercentage);
    }
}
