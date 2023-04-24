using LosSantosRED.lsr.Data;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IWeapons
    {
        WeaponInformation GetWeapon(string modelName);
        WeaponInformation GetWeapon(ulong WeaponHash);
        WeaponVariation GetWeaponVariation(Ped character, uint hash);
        WeaponInformation GetCurrentWeapon(Ped character);
        WeaponInformation GetRandomRegularWeapon(WeaponCategory randomWeaponCategory);
        WeaponInformation GetRandomRegularWeapon();
        List<WeaponInformation> GetAllWeapons();
        WeaponVariation GetRandomVariation(uint hash, float v);
    }
}
