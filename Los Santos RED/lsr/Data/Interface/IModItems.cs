using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IModItems
    {
        ModItem Get(string text);
        ModItem GetRandomItem();
        List<ModItem> AllItems();
        List<ModItem> InventoryItems();
    }
}
