using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TaxiFirm : Organization
{
    public TaxiFirm()
    {
    }

    public TaxiFirm(string _ColorPrefix, string _ID, string _shortName, string _FullName, string _AgencyColorString, string _DispatchablePeropleGroupID, string _DispatchableVehicleGroupID, string _LicensePlatePrefix, string meleeWeaponsID, string sideArmsID, string longGunsID, string groupName) : base(_ColorPrefix, _ID, _shortName, _FullName, _AgencyColorString, _DispatchablePeropleGroupID, _DispatchableVehicleGroupID, _LicensePlatePrefix, meleeWeaponsID, sideArmsID, longGunsID, groupName)
    {
    }
}
