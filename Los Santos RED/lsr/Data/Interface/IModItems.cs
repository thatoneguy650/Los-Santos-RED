using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IModItems
    {
        PossibleItems PossibleItems { get; }
        WeaponItem GetWeapon(string modelName);
        WeaponItem GetWeapon(uint modelHash);
        ModItem Get(string text);
        ModItem GetRandomItem(bool allowIllegal, bool allowCellphones);
        List<ModItem> AllItems();
        List<ModItem> InventoryItems();
        void WriteToFile();
        WeaponItem GetRandomWeapon(bool allowIllegal);
        VehicleItem GetVehicle(string modelName);
        VehicleItem GetVehicle(uint modelHash);
    }
}
