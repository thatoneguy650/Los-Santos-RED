using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Player
{
    [Serializable()]
    public class Inventory
    {
        private List<InventoryItem> ItemsList = new List<InventoryItem>();
        public List<InventoryItem> Items => ItemsList;
        public Inventory()
        {

        }
        public Inventory(IInventoryable player)
        {

        }
        public bool UseTool(ToolTypes tool)
        {
            foreach(InventoryItem ii in ItemsList.Where(x=> x.ModItem.ToolType == tool).OrderBy(x=> x.RemainingPercent))
            {
                if(Use(ii.ModItem))
                {
                    ItemsList.RemoveAll(x => x.RemainingPercent <= 0.005f);
                    return true;
                }
            }
            return false;
        }
        public bool HasTool(ToolTypes tool) => ItemsList.Any(x => x.ModItem.ToolType == tool);
        public void Add(ModItem modItem, float remainingPercent)
        {
            if (modItem != null)
            {
                InventoryItem ExistingItem = ItemsList.FirstOrDefault(x => x.ModItem.Name == modItem.Name);
                if (ExistingItem == null)
                {
                    ItemsList.Add(new InventoryItem(modItem) { RemainingPercent = remainingPercent });
                }
                else
                {
                    ExistingItem.RemainingPercent += remainingPercent;//ExistingItem.Amount += amount;
                }
            }
        }
        //public void Add(ModItem modItem, int amount)
        //{
        //    if (modItem != null)
        //    {
        //        InventoryItem ExistingItem = ItemsList.FirstOrDefault(x => x.ModItem.Name == modItem.Name);
        //        if (ExistingItem == null)
        //        {
        //            ItemsList.Add(new InventoryItem(modItem, amount));
        //        }
        //        else
        //        {
        //            ExistingItem.AddAmount(amount);//ExistingItem.Amount += amount;
        //        }
        //    }
        //}

        //public void Set(ModItem modItem, float remainingPercent)
        //{
        //    if (modItem != null)
        //    {
        //        InventoryItem ExistingItem = ItemsList.FirstOrDefault(x => x.ModItem.Name == modItem.Name);
        //        if (ExistingItem == null)
        //        {
        //            ItemsList.Add(new InventoryItem(modItem) { RemainingPercent = remainingPercent });
        //        }
        //        else
        //        {
        //            ExistingItem.RemainingPercent = remainingPercent;//ExistingItem.Amount += amount;
        //        }
        //    }
        //}


        public bool Remove(ModItem modItem)
        {
            if (modItem != null)
            {
                InventoryItem ExistingItem = ItemsList.FirstOrDefault(x => x.ModItem.Name == modItem.Name);
                if (ExistingItem != null)
                {
                    ItemsList.Remove(ExistingItem);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        public bool Remove(ModItem modItem, int amount)
        {
            if (modItem != null)
            {
                InventoryItem ExistingItem = ItemsList.FirstOrDefault(x => x.ModItem.Name == modItem.Name);
                if (ExistingItem != null)
                {
                    if (ExistingItem.Amount > amount)
                    {
                        ExistingItem.RemovePercent(amount);//ExistingItem.Amount -= amount;
                    }
                    else
                    {
                        ItemsList.Remove(ExistingItem);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        public bool Use(ModItem modItem)
        {
            if (modItem != null)
            {
                InventoryItem ExistingItem = ItemsList.FirstOrDefault(x => x.ModItem.Name == modItem.Name);
                if (ExistingItem != null)
                {
                    return ExistingItem.RemovePercent(modItem.PercentLostOnUse);
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        public InventoryItem Get(ModItem modItem)
        {
            return ItemsList.FirstOrDefault(x => x.ModItem.Name == modItem.Name);
        }
        public InventoryItem Get(string itemName)
        {
            return ItemsList.FirstOrDefault(x => x.ModItem.Name == itemName);
        }
        public bool HasItem(string itemName)
        {
            return ItemsList.FirstOrDefault(x => x.ModItem.Name == itemName)?.Amount > 0;
        }
        public int Amount(string itemName)
        {
            InventoryItem ii = ItemsList.FirstOrDefault(x => x.ModItem.Name == itemName);
            if(ii == null)
            {
                return 0;
            }
            else
            {
                return ii.Amount;
            }
        }
        public void Clear()
        {
            ItemsList.Clear();
        }
    }
}
