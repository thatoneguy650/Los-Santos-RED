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
        List<Consumeable> ConsumableList = new List<Consumeable>();
        private IInventoryable Player;
        public Inventory(IInventoryable player)
        {
            Player = player;
        }
        public void Add(Consumeable consumeable)
        {
            Consumeable ExistingItem = ConsumableList.FirstOrDefault(x => x.Name == consumeable.Name);
            if (ExistingItem == null)
            {
                ConsumableList.Add(consumeable);
            }
            else
            {
                ExistingItem.Amount += consumeable.Amount;
            }
        }
        public void Remove(string Name)
        {
            Consumeable ExistingItem = ConsumableList.FirstOrDefault(x => x.Name == Name);
            if (ExistingItem != null)
            {
                ConsumableList.Remove(ExistingItem);
            }
        }
        public void Remove(string Name, float amount)
        {
            Consumeable ExistingItem = ConsumableList.FirstOrDefault(x => x.Name == Name);
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
    }
}
