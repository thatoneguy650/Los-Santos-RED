using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Player
{
    public class Inventory
    {
        private List<ConsumableInventoryItem> ConsumableList = new List<ConsumableInventoryItem>();
        private IInventoryable Player;
        public List<ConsumableInventoryItem> Consumables => ConsumableList;
        public Inventory(IInventoryable player)
        {
            Player = player;
        }
        public void Add(ConsumableSubstance consumableSubstance, int amount)
        {
            ConsumableInventoryItem ExistingItem = ConsumableList.FirstOrDefault(x => x.ConsumableSubstance.Name == consumableSubstance.Name);
            if (ExistingItem == null)
            {
                ConsumableList.Add(new ConsumableInventoryItem(consumableSubstance,amount));
            }
            else
            {
                ExistingItem.Amount += amount;
            }
        }
        public void Remove(ConsumableSubstance consumableSubstance)
        {
            ConsumableInventoryItem ExistingItem = ConsumableList.FirstOrDefault(x => x.ConsumableSubstance.Name == consumableSubstance.Name);
            if (ExistingItem != null)
            {
                ConsumableList.Remove(ExistingItem);
            }
        }
        public void Remove(ConsumableSubstance consumableSubstance, int amount)
        {
            ConsumableInventoryItem ExistingItem = ConsumableList.FirstOrDefault(x => x.ConsumableSubstance.Name == consumableSubstance.Name);
            if (ExistingItem != null)
            {
                if (ExistingItem.Amount > amount)
                {
                    ExistingItem.Amount -= amount;
                }
                else
                {
                    ConsumableList.Remove(ExistingItem);
                }
            }
        }
        public ConsumableInventoryItem Get(ConsumableSubstance consumableSubstance)
        {
            return ConsumableList.FirstOrDefault(x => x.ConsumableSubstance.Name == consumableSubstance.Name);
        }
        public void PrintInventory()
        {
            EntryPoint.WriteToConsole("PLAYER PrintInventory", 5);
            foreach (ConsumableInventoryItem cii in ConsumableList)
            {
                EntryPoint.WriteToConsole($"{cii.ConsumableSubstance.Name} {cii.ConsumableSubstance.Type} {cii.ConsumableSubstance.ModelName} {cii.Amount}", 5);
            }
            EntryPoint.WriteToConsole("PLAYER PrintInventory", 5);
        }
    }
}
