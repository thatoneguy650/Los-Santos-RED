using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class FlashlightInventoryItem : InventoryItem
{
    public FlashlightItem FlashlightItem;
    public FlashlightInventoryItem(FlashlightItem flashlightItem)
    {
        FlashlightItem = flashlightItem;
    }
    //public override void AddToInventory(IActionable actionable, float remainingPercent)
    //{
    //    FlashlightInventoryItem existingInventoryItem = actionable.Inventory.FlashlightInventoryItems.FirstOrDefault(x => x.FlashlightItem.Name == this.FlashlightItem.Name);
    //    if(existingInventoryItem != null)
    //    {
    //        existingInventoryItem.RemainingPercent += remainingPercent;
    //    }
    //    else
    //    {
    //        actionable.Inventory.FlashlightInventoryItems.Add(new FlashlightInventoryItem(FlashlightItem) {RemainingPercent = remainingPercent });
    //    }
    //}
}

