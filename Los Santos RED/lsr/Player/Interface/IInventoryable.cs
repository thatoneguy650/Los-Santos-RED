using LosSantosRED.lsr.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IInventoryable
    {
        Inventory Inventory { get; }
        bool IsPerformingActivity { get; }
        int Money { get; }

        bool RemoveFromInventory(ModItem toAdd, int v);
        void GiveMoney(int salesPrice);
        void AddToInventory(ModItem toAdd, int amountPerPackage);
        void StartServiceActivity(ModItem toAdd, GameLocation store);
    }
}
