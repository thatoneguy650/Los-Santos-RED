﻿using LosSantosRED.lsr.Interface;
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
    public SimpleInventory( ISettingsProvideable settings)
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
        if (Settings.SettingsManager.CivilianSettings.MaxRandomItemsToGet >= 1 && Settings.SettingsManager.CivilianSettings.MaxRandomItemsAmount >= 1)
        {
            int ItemsToGet = RandomItems.GetRandomNumberInt(1, Settings.SettingsManager.CivilianSettings.MaxRandomItemsToGet);
            for (int i = 0; i < ItemsToGet; i++)
            {
                ModItem toGet = modItems.GetRandomItem();
                int AmountToGet = RandomItems.GetRandomNumberInt(1, Settings.SettingsManager.CivilianSettings.MaxRandomItemsAmount);
                if (toGet != null)
                {
                    Add(toGet, AmountToGet * toGet.AmountPerPackage);
                }
            }
        }
    }

    public void CreateInteractionMenu(MenuPool menuPool, UIMenu VehicleInteractMenu)
    {

        UIMenu UnloadBodiesSubMenu = menuPool.AddSubMenu(VehicleInteractMenu, "Inventory");
        VehicleInteractMenu.MenuItems[VehicleInteractMenu.MenuItems.Count() - 1].Description = "Manage vehicle Inventory.";
        UnloadBodiesSubMenu.SetBannerType(EntryPoint.LSRedColor);


        foreach(InventoryItem modItem in ItemsList)
        {
            UIMenuItem unloadBody = new UIMenuItem($"Unload {storedBody.PedExt.Name}", $"Unload {storedBody.PedExt.Name} from {storedBody.VehicleDoorSeatData?.SeatName}");
            unloadBody.Activated += (menu, item) =>
            {

                UnloadBodiesSubMenu.Visible = false;
            };
            UnloadBodiesSubMenu.AddItem(unloadBody);
        }
    }



}
