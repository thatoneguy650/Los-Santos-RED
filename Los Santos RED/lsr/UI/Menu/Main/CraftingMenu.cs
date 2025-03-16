using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

public class CraftingMenu : ModUIMenu
{
    const string UNCATEGORIZED = "Uncategorized";
    private UIMenu Menu;
    private MenuPool MenuPool;
    private ICraftableItems CraftableItems;
    private Mod.Crafting Crafting;
    private ILocationInteractable LocationInteractablePlayer;

    public CraftingMenu(MenuPool menuPool, ICraftableItems craftableItems, Mod.Crafting crafting, ILocationInteractable locationInteractablePlayer)
    {
        MenuPool = menuPool;
        CraftableItems = craftableItems;
        Crafting = crafting;
        LocationInteractablePlayer = locationInteractablePlayer;
    }
    public void Setup()
    {
        Menu = new UIMenu("Crafting", string.Empty);
        Crafting.CraftingMenu = this;
        Menu.SetBannerType(System.Drawing.Color.FromArgb(181, 48, 48));
        MenuPool.Add(Menu);
    }
    public override void Hide()
    {
        Menu.Visible = false;
    }
    public override void Show()
    {
        if (!Menu.Visible)
        {
            Create();
            if (Menu.MenuItems.Count > 0)
            {
                Menu.Visible = true;
            }
            else
            {
                Game.DisplayNotification($"You have no clue what to make with this stuff here.");
            }
        }
    }
    public void Show(string craftingFlag)
    {
        if (!Menu.Visible)
        {
            Menu.Clear();
            CreateCraftableItems(craftingFlag);
            if (Menu.MenuItems.Count > 0)
            {
                Menu.Visible = true;
            }
            else
            {
                Game.DisplayNotification($"You have no clue what to make with this stuff here.");
            }
        }
    }
    public override void Toggle()
    {
        if (!Menu.Visible)
        {
            Show();
        }
        else
        {
            Menu.Visible = false;
        }
    }
    private void Create()
    {
        Menu.Clear();
        CreateCraftableItems();
    }
    private void CreateCraftableItems(string craftingFlag = null)
    {
        HashSet<string> craftableItemsChecked = new HashSet<string>();
        Dictionary<string, ModItem> itemsToRemove = new Dictionary<string, ModItem>();
        Dictionary<string, UIMenu> categoryMenus = new Dictionary<string, UIMenu>();
        if(craftingFlag == null && LocationInteractablePlayer.IsInVehicle)
        {
            craftingFlag = LocationInteractablePlayer.CurrentVehicle.VehicleModelName;
        }
        foreach (InventoryItem item in LocationInteractablePlayer.Inventory.ItemsList)
        {
            if (Crafting.CraftableItems.IngredientCraftableLookup.ContainsKey(item.ModItem.Name))
            {
                foreach (string craftableItem in Crafting.CraftableItems.IngredientCraftableLookup[item.ModItem.Name])
                {
                    if (craftableItemsChecked.Contains(craftableItem))
                    {
                        continue;
                    }
                    int quantity = Crafting.GetQuantityOfCraftable(CraftableItems.CraftablesLookup[craftableItem], itemsToRemove, craftingFlag);
                    if (quantity > 0)
                    {
                        craftableItemsChecked.Add(craftableItem);
                        CraftableItem craftableItemObject = Crafting.CraftableItems.CraftablesLookup[craftableItem].CraftableItem;

                        UIMenu uIMenu = GetSubMenuForCraftableItem(craftableItemObject.Category, categoryMenus);
                        if (craftableItemObject.SingleUnit)
                        {
                            UIMenuItem itemMenu = new UIMenuItem(craftableItemObject.Name, craftableItemObject.IngredientList);
                            itemMenu.Activated += (s, e) =>
                            {
                                Crafting.CraftItem(itemMenu.Text, itemsToRemove, craftingFlag: craftingFlag);
                            };
                            uIMenu.AddItem(itemMenu);
                        }
                        else
                        {
                            UIMenuNumericScrollerItem<int> itemMenu = new UIMenuNumericScrollerItem<int>(craftableItemObject.Name, craftableItemObject.IngredientList, 1, quantity, 1);
                            itemMenu.Value = 1;
                            itemMenu.IndexChanged += (s, oldIndex, newIndex) =>
                            {
                                itemMenu.Description = craftableItemObject.GetIngredients(newIndex+1);
                            };
                            itemMenu.Activated += (s, e) =>
                            {
                                Crafting.CraftItem(itemMenu.Text, itemsToRemove, itemMenu.Value, craftingFlag: craftingFlag);
                            };
                            uIMenu.AddItem(itemMenu);
                        }
                    }
                }
            }
        }
    }


    private UIMenu GetSubMenuForCraftableItem(string category,Dictionary<string, UIMenu> categoryMenus)
    {
        if(string.IsNullOrEmpty(category))
        {
            if(categoryMenus.ContainsKey(UNCATEGORIZED))
            {
                return categoryMenus[UNCATEGORIZED];
            }
            else
            {
                UIMenu subMenu = MenuPool.AddSubMenu(Menu, UNCATEGORIZED);
                categoryMenus.Add(UNCATEGORIZED, subMenu);
                return subMenu;
            }
        }
        if(categoryMenus.ContainsKey(category))
        {
            return categoryMenus[category];
        }
        else
        {
            UIMenu subMenu = MenuPool.AddSubMenu(Menu, category);
            categoryMenus.Add(category, subMenu);
            return subMenu;
        }
    }
}