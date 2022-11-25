using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class IssuableWeaponsGroup
{
    public IssuableWeaponsGroup()
    {
    }

    public IssuableWeaponsGroup(string issuableWeaponsID, List<IssuableWeapon> issuableWeapons)
    {
        IssuableWeaponsID = issuableWeaponsID;
        IssuableWeapons = issuableWeapons;
    }

    public string IssuableWeaponsID { get; set; }
    public List<IssuableWeapon> IssuableWeapons { get; set; }

}
