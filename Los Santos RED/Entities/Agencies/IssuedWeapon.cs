using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class IssuedWeapon
{
    public string ModelName { get; set; }
    public bool IsPistol { get; set; }
    public WeaponVariation MyVariation { get; set; } = new WeaponVariation();
    public IssuedWeapon()
    {

    }
    public IssuedWeapon(string _ModelName, bool _IsPistol, WeaponVariation _MyVariation)
    {
        ModelName = _ModelName;
        IsPistol = _IsPistol;
        MyVariation = _MyVariation;
    }

}

