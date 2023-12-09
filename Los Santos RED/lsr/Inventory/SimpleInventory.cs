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
    protected ISettingsProvideable Settings;
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
    public void AddRandomItems(IModItems modItems, bool allowCellphones, bool allowIllegal)
    {
        if (Settings.SettingsManager.PlayerOtherSettings.MaxRandomItemsToGet >= 1 && Settings.SettingsManager.PlayerOtherSettings.MaxRandomItemsAmount >= 1)
        {
            int ItemsToGet = RandomItems.GetRandomNumberInt(1, Settings.SettingsManager.PlayerOtherSettings.MaxRandomItemsToGet);
            for (int i = 0; i < ItemsToGet; i++)
            {
                ModItem toGet = modItems.GetRandomItem(allowIllegal, allowCellphones);
                int AmountToGet = RandomItems.GetRandomNumberInt(1, Settings.SettingsManager.PlayerOtherSettings.MaxRandomItemsAmount);
                if (toGet != null)
                {
                    Add(toGet, AmountToGet * toGet.AmountPerPackage);
                }
            }
        }
    }
    public void CreateInteractionMenu(IInteractionable player, MenuPool menuPool, UIMenu menuToAdd, bool withAnimations, List<ItemType> AllowedItemTypes, List<ItemType> DisallowedItemTypes, bool removeBanner, string overrideTitle, string overrideDescription)
    {
        string title = "Stored Inventory";
        string description = "Manage stored inventory. Take or deposit items.";


        if(!string.IsNullOrEmpty(overrideTitle))
        {
            title = overrideTitle;
        }
        if (!string.IsNullOrEmpty(overrideDescription))
        {
            description = overrideDescription;
        }

        UIMenu VehicleInventoryItem = menuPool.AddSubMenu(menuToAdd, title);
        menuToAdd.MenuItems[menuToAdd.MenuItems.Count() - 1].Description = description;
        if (removeBanner)
        {
            VehicleInventoryItem.RemoveBanner();
        }
        else
        {
            VehicleInventoryItem.SetBannerType(EntryPoint.LSRedColor);
        }
        UIMenuItem TakeAllItems = new UIMenuItem("Take All","Take all stored items.");
        TakeAllItems.Activated += (sender, selectedItem) =>
        {
            TakeAll(player, AllowedItemTypes, DisallowedItemTypes);
            UpdateInventoryScrollers(player, AllowedItemTypes, DisallowedItemTypes);
        };
        VehicleInventoryItem.AddItem(TakeAllItems);
        UIMenuItem DepositAll = new UIMenuItem("Store All", "Store all player items.");
        DepositAll.Activated += (sender, selectedItem) =>
        {
            StoreAll(player, AllowedItemTypes, DisallowedItemTypes);
            UpdateInventoryScrollers(player, AllowedItemTypes, DisallowedItemTypes);
        };
        VehicleInventoryItem.AddItem(DepositAll);

        List<InventoryItem> storedItems = GetAllowedItems(ItemsList.ToList(), AllowedItemTypes, DisallowedItemTypes);
        foreach (InventoryItem inventoryItem in storedItems)
        {
            inventoryItem.ModItem.CreateInventoryManageMenu(player, menuPool, this, VehicleInventoryItem, withAnimations, Settings, removeBanner);
        }
        List<InventoryItem> playerItems = GetAllowedItems(player.Inventory.ItemsList.ToList(), AllowedItemTypes, DisallowedItemTypes);
        if (playerItems == null)
        {
            return;
        }
        foreach (InventoryItem inventoryItem in playerItems)
        {
            if (!storedItems.Any(x => x.ModItem.Name == inventoryItem.ModItem.Name))
            {
                inventoryItem.ModItem.CreateInventoryManageMenu(player, menuPool, this, VehicleInventoryItem, withAnimations, Settings, removeBanner);
            }
        }
    }
    private void UpdateInventoryScrollers(IInteractionable player, List<ItemType> AllowedItemTypes, List<ItemType> DisallowedItemTypes)
    {
        List<InventoryItem> storedItems = GetAllowedItems(ItemsList.ToList(), AllowedItemTypes, DisallowedItemTypes);
        foreach (InventoryItem inventoryItem in storedItems)
        {
            inventoryItem.ModItem.UpdateInventoryScrollers(player, this, Settings);
        }
        List<InventoryItem> playerItems = GetAllowedItems(player.Inventory.ItemsList.ToList(), AllowedItemTypes, DisallowedItemTypes);
        foreach (InventoryItem inventoryItem in playerItems)
        {
            if (!ItemsList.Any(x => x.ModItem.Name == inventoryItem.ModItem.Name))
            {
                inventoryItem.ModItem.UpdateInventoryScrollers(player, this, Settings);
            }
        }
    }
    private List<InventoryItem> GetAllowedItems(List<InventoryItem> toCheck, List<ItemType> AllowedItemTypes, List<ItemType> DisallowedItemTypes)
    {
        List<InventoryItem> allowedItems = null;
        if (AllowedItemTypes != null && AllowedItemTypes.Any())
        {
            allowedItems = toCheck.Where(x => x.ModItem != null && AllowedItemTypes.Contains(x.ModItem.ItemType)).ToList();
        }
        else if (DisallowedItemTypes != null && DisallowedItemTypes.Any())
        {
            allowedItems = toCheck.Where(x => x.ModItem != null && !DisallowedItemTypes.Contains(x.ModItem.ItemType)).ToList();
        }
        else
        {
            allowedItems = toCheck.ToList();
        }
        return allowedItems;
    }

    private void TakeAll(IInteractionable player, List<ItemType> AllowedItemTypes, List<ItemType> DisallowedItemTypes)
    {
        List<InventoryItem> storedItems = GetAllowedItems(ItemsList.ToList(), AllowedItemTypes, DisallowedItemTypes);
        foreach (InventoryItem inventoryItem in storedItems)
        {
            int amount = inventoryItem.Amount;
            if (Remove(inventoryItem.ModItem, amount))
            {
                player.Inventory.Add(inventoryItem.ModItem, amount);
            }
        }
    }
    private void StoreAll(IInteractionable player, List<ItemType> AllowedItemTypes, List<ItemType> DisallowedItemTypes)
    {
        List<InventoryItem> playerItems = GetAllowedItems(player.Inventory.ItemsList.ToList(), AllowedItemTypes, DisallowedItemTypes);
        foreach (InventoryItem inventoryItem in playerItems)
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

    public void OnImpounded()
    {
        RemoveIllegalItems();
    }
    public void RemoveIllegalItems()
    {
        foreach (InventoryItem inventoryItem in ItemsList.ToList())
        {
            if (inventoryItem.ModItem == null || inventoryItem.ModItem.IsPossessionIllicit)
            {
                Remove(inventoryItem.ModItem, inventoryItem.Amount);
            }
        }
    }

    public List<ModItem> GetIllegalItems()
    {
        List<ModItem> items = new List<ModItem>();
        foreach (InventoryItem ii in ItemsList.ToList())
        {
            if (ii.ModItem != null && ii.ModItem.IsPossessionIllicit)
            {
                items.Add(ii.ModItem);
            }
        }
        return items;
    }
}

