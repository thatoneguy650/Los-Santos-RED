using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable()]
public class InventoryItem
{
    public InventoryItem(ModItem modItem, int amount)
    {
        ModItem = modItem;
        Amount = amount;
    }
    public InventoryItem()
    {

    }
    public ModItem ModItem { get; set; }
    public int Amount { get; set; }
}

