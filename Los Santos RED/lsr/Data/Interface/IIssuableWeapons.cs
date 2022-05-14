using System.Collections.Generic;

namespace LosSantosRED.lsr.Interface
{
    public interface IIssuableWeapons
    {
        List<IssuableWeapon> GetWeaponData(string issuableWeaponsID);
    }
}