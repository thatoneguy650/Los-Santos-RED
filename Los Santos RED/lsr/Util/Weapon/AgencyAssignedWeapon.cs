using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AgencyAssignedWeapon
{
    public string ModelName { get; set; }
    public bool IsPistol { get; set; }
    public WeaponVariation Variation { get; set; } = new WeaponVariation();
    public AgencyAssignedWeapon()
    {

    }
    public AgencyAssignedWeapon(string _ModelName, bool _IsPistol, WeaponVariation _MyVariation)
    {
        ModelName = _ModelName;
        IsPistol = _IsPistol;
        Variation = _MyVariation;
    }

}

