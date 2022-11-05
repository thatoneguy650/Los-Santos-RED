using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BongInventoryItem : InventoryItem
{
    public BongItem BongItem;
    public BongInventoryItem(BongItem bongItem)
    {
        BongItem = bongItem;
    }
    //public override void AddToInventory(IActionable actionable, float remainingPercent)
    //{
    //    BongInventoryItem existingInventoryItem = actionable.Inventory.BongInventoryItems.FirstOrDefault(x => x.BongItem.Name == this.BongItem.Name);
    //    if (existingInventoryItem != null)
    //    {
    //        existingInventoryItem.RemainingPercent += remainingPercent;
    //    }
    //    else
    //    {
    //        actionable.Inventory.BongInventoryItems.Add(new BongInventoryItem(BongItem) { RemainingPercent = remainingPercent });
    //    }
    //}
}

