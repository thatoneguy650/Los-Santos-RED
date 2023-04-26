using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using RAGENativeUI.Elements;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SimpleInventory
{
    private ISettingsProvideable Settings;
    public List<InventoryItem> ItemsList { get; set; } = new List<InventoryItem>();
    public SimpleInventory(ISettingsProvideable settings)
    {
        Settings = settings;
    }
    public void Add(ModItem modItem, float remainingPercent)
    {
        if (modItem != null)
        {
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
    public void AddRandomItems(IModItems modItems)
    {
        if (Settings.SettingsManager.PlayerOtherSettings.MaxRandomItemsToGet >= 1 && Settings.SettingsManager.PlayerOtherSettings.MaxRandomItemsAmount >= 1)
        {
            int ItemsToGet = RandomItems.GetRandomNumberInt(1, Settings.SettingsManager.PlayerOtherSettings.MaxRandomItemsToGet);
            for (int i = 0; i < ItemsToGet; i++)
            {
                ModItem toGet = modItems.GetRandomItem(true);
                int AmountToGet = RandomItems.GetRandomNumberInt(1, Settings.SettingsManager.PlayerOtherSettings.MaxRandomItemsAmount);
                if (toGet != null)
                {
                    Add(toGet, AmountToGet * toGet.AmountPerPackage);
                }
            }
        }
    }




    public void CreateInteractionMenu(IInteractionable player, MenuPool menuPool, UIMenu menuToAdd, bool withAnimations)
    {
        UIMenu VehicleInventoryItem = menuPool.AddSubMenu(menuToAdd, "Stored Inventory");
        menuToAdd.MenuItems[menuToAdd.MenuItems.Count() - 1].Description = "Manage stored inventory. Take or deposit items.";
        VehicleInventoryItem.SetBannerType(EntryPoint.LSRedColor);
        UIMenuItem TakeAllItems = new UIMenuItem("Take All","Take all stored items.");
        TakeAllItems.Activated += (sender, selectedItem) =>
        {
            TakeAll(player);
            UpdateInventoryScrollers(player);
        };
        VehicleInventoryItem.AddItem(TakeAllItems);
        UIMenuItem DepositAll = new UIMenuItem("Store All", "Store all player items.");
        DepositAll.Activated += (sender, selectedItem) =>
        {
            StoreAll(player);
            UpdateInventoryScrollers(player);
        };
        VehicleInventoryItem.AddItem(DepositAll);



        foreach (InventoryItem inventoryItem in ItemsList)
        {
            inventoryItem.ModItem.CreateInventoryManageMenu(player, menuPool, this, VehicleInventoryItem, withAnimations, Settings);
        }
        foreach (InventoryItem inventoryItem in player.Inventory.ItemsList.ToList())
        {
            if (!ItemsList.Any(x => x.ModItem.Name == inventoryItem.ModItem.Name))
            {
                inventoryItem.ModItem.CreateInventoryManageMenu(player, menuPool, this, VehicleInventoryItem, withAnimations, Settings);
            }
        }

    }
    private void UpdateInventoryScrollers(IInteractionable player)
    {
        foreach (InventoryItem inventoryItem in ItemsList)
        {
            inventoryItem.ModItem.UpdateInventoryScrollers(player, this, Settings);
        }
        foreach (InventoryItem inventoryItem in player.Inventory.ItemsList.ToList())
        {
            if (!ItemsList.Any(x => x.ModItem.Name == inventoryItem.ModItem.Name))
            {
                inventoryItem.ModItem.UpdateInventoryScrollers(player, this, Settings);
            }
        }
    }

    private void TakeAll(IInteractionable player)
    {
        foreach(InventoryItem inventoryItem in ItemsList.ToList())
        {
            int amount = inventoryItem.Amount;
            if (Remove(inventoryItem.ModItem, amount))
            {
                player.Inventory.Add(inventoryItem.ModItem, amount);
            }
        }
    }
    private void StoreAll(IInteractionable player)
    {
        foreach (InventoryItem inventoryItem in player.Inventory.ItemsList.ToList())
        {
            int amount = inventoryItem.Amount;
            if (player.Inventory.Remove(inventoryItem.ModItem, amount))
            {
                Add(inventoryItem.ModItem, amount);
            }
        }
    }
    public void Reset()
    {
        ItemsList.Clear();
    }
}

