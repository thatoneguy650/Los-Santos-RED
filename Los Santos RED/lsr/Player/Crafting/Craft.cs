using LosSantosRED.lsr.Interface;
using Rage;
using System;

namespace Mod
{
    public class Crafting
    {
        private Player Player;
        private ICraftableItems CraftableItems;
        private IModItems ModItems;
        private ISettingsProvideable Settings;
        private ICrimes Crimes;

        public Crafting(Player player, ICraftableItems craftableItems, IModItems modItems, ISettingsProvideable settings, ICrimes crimes)
        {
            Player = player;
            CraftableItems = craftableItems;
            ModItems = modItems;
            Settings = settings;
            Crimes = crimes;
        }
        public void CraftItem(string productName, int quantity)
        {
            var itemToAdd = Player.Inventory.ItemsList.Find(item => item.ModItem.Name == productName);
            if(itemToAdd != null)
            {
                itemToAdd.AddAmount(quantity);
            }
            else
            {
                itemToAdd = new InventoryItem(ModItems.Get(productName), Settings);
                itemToAdd.RemainingPercent = quantity;
                Player.Inventory.ItemsList.Add(itemToAdd);
            }
        }
    }
}