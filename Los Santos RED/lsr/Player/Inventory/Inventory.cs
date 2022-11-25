using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LosSantosRED.lsr.Player
{
    [Serializable()]
    public class Inventory
    {
        private IInventoryable Player;
        private ISettingsProvideable Settings;
        private IModItems ModItems;
        public List<InventoryItem> ItemsList { get; set; } = new List<InventoryItem>();

     //   public List<LicensePlateInventoryItem> LicensePlateItems { get; set; } = new List<LicensePlateInventoryItem>();




        // public List<InventoryItem> Items => ItemsList;
        public Inventory()
        {

        }
        public Inventory(IInventoryable player, ISettingsProvideable settings, IModItems modItems)
        {
            Player = player;
            Settings = settings;
            ModItems = modItems;
        }
        public void Reset()
        {
            Clear();
        }
        public void Clear()
        {
            ItemsList.Clear();
        }
        public bool Has(Type type)
        {
            return ItemsList.FirstOrDefault(x => x.ModItem.GetType() == type) != null;
        }
        public InventoryItem Get(Type type)
        {
            return ItemsList.Where(x => x.ModItem.GetType() == type).OrderBy(x=> x.RemainingPercent).FirstOrDefault();
        }
        public void Add(ModItem modItem, float remainingPercent)
        {
            if (modItem != null)
            {
                if (ModItems.Get(modItem.Name) == null)
                {
                    modItem.AddNewItem(ModItems);
                }
                InventoryItem ExistingItem = Get(modItem);
                if (ExistingItem == null)
                {
                    ItemsList.Add(new InventoryItem(modItem, Settings) { RemainingPercent = remainingPercent });
                }
                else
                {
                    ExistingItem.RemainingPercent += remainingPercent;
                }
            }
        }
        public InventoryItem Get(ModItem modItem)
        {
            return ItemsList.FirstOrDefault(x => x.ModItem.Name == modItem.Name);
        }
        public void Use(ModItem modItem)
        {
            if (modItem != null)
            {
                if (modItem.PercentLostOnUse > 0.0f)
                {
                    Get(modItem)?.RemovePercent(modItem.PercentLostOnUse);                
                }
                else
                {
                    Remove(modItem, 1);
                }
            }
        }
        public bool Remove(ModItem modItem)
        {
            if (modItem != null)
            {
                InventoryItem ExistingItem = Get(modItem);
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
                InventoryItem ExistingItem = Get(modItem);
                if (ExistingItem != null)
                {
                    if (ExistingItem.Amount > amount)
                    {
                        ExistingItem.RemovePercent(amount);
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
    }
}
