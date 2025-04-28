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
    const string UNCATEGORIZED = "General Items";//"Uncategorized";
    private UIMenu Menu;
    private MenuPool MenuPool;
    private ICraftableItems CraftableItems;
    private Mod.Crafting Crafting;
    private ILocationInteractable LocationInteractablePlayer;
    private IModItems ModItems;

    public CraftingMenu(MenuPool menuPool, ICraftableItems craftableItems, Mod.Crafting crafting, ILocationInteractable locationInteractablePlayer, IModItems modItems)
    {
        MenuPool = menuPool;
        CraftableItems = craftableItems;
        Crafting = crafting;
        LocationInteractablePlayer = locationInteractablePlayer;
        ModItems = modItems;
    }
    public void Setup()
    {
        Menu = new UIMenu("Crafting", "Select crafting category");
        Crafting.CraftingMenu = this;
        Menu.SetBannerType( EntryPoint.LSRedColor);
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
                Game.DisplayHelp("No craftable items.");// $"You have no clue what to make with this stuff here.");
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
                Game.DisplayHelp("No craftable items.");// $"You have no clue what to make with this stuff here.");
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
        Dictionary<string, UIMenu> categoryMenus = new Dictionary<string, UIMenu>();
        if (craftingFlag == null && LocationInteractablePlayer.IsInVehicle)
        {
            craftingFlag = LocationInteractablePlayer.CurrentVehicle.VehicleModelName;
        }
        foreach(CraftableItem craftableItem in CraftableItems.Items)
        {
            if (!string.IsNullOrEmpty(craftingFlag) && craftableItem.CraftingFlags != null && !craftableItem.CraftingFlags.Contains(craftingFlag))
            {
                continue;
            }
            if (craftableItem.CraftingFlags != null && craftableItem.CraftingFlags.Count > 0 && !craftableItem.CraftingFlags.Contains(craftingFlag))
            {
                continue;
            }
            int quantity = 0;
            int ingredientsSatisfied = 0;
            int ingredientsToSatisfy = craftableItem.Ingredients.Count;
            foreach(Ingredient ingredient in craftableItem.Ingredients)
            {
                InventoryItem ingredientInInventory = LocationInteractablePlayer.Inventory.ItemsList.Find(x => x.ModItem.Name == ingredient.IngredientName);
                if(ingredientInInventory == null)
                {
                    break;
                }
                int instancesCraftable = ingredientInInventory.Amount / ingredient.Quantity;
                if (instancesCraftable == 0)
                {
                    quantity = 0;
                    break;
                }
                else
                {
                    if (ingredient.IsConsumed)
                    {
                        quantity = quantity == 0 ? instancesCraftable : Math.Min(quantity, instancesCraftable);
                    }
                    ingredientsSatisfied++;
                }
            }
            if (ingredientsSatisfied != ingredientsToSatisfy || quantity==0)
            {
                UIMenu uIMenu = GetSubMenuForCraftableItem(craftableItem.Category, categoryMenus);
                UIMenuItem itemMenu = new UIMenuItem(craftableItem.Name, craftableItem.GetIngredientDescription(1,ModItems));
                itemMenu.Enabled = false;
                uIMenu.AddItem(itemMenu);
                continue;
            }
            if (quantity > 0)
            {
                UIMenu uIMenu = GetSubMenuForCraftableItem(craftableItem.Category, categoryMenus);
                if (craftableItem.SingleUnit)
                {
                    UIMenuItem itemMenu = new UIMenuItem(craftableItem.Name, craftableItem.GetIngredientDescription(1,ModItems));
                    itemMenu.Activated += (s, e) =>
                    {
                        Crafting.CraftItem(itemMenu.Text, craftingFlag: craftingFlag);
                    };
                    uIMenu.AddItem(itemMenu);
                }
                else
                {
                    UIMenuNumericScrollerItem<int> itemMenu = new UIMenuNumericScrollerItem<int>(craftableItem.Name, craftableItem.GetIngredientDescription(1,ModItems), 1, quantity, 1);
                    itemMenu.Value = 1;
                    itemMenu.IndexChanged += (s, oldIndex, newIndex) =>
                    {
                        itemMenu.Description = craftableItem.GetIngredientDescription(newIndex + 1, ModItems);
                    };
                    itemMenu.Activated += (s, e) =>
                    {
                        Crafting.CraftItem(itemMenu.Text, itemMenu.Value, craftingFlag: craftingFlag);
                    };
                    uIMenu.AddItem(itemMenu);
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
                subMenu.SetBannerType(EntryPoint.LSRedColor);
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
            subMenu.SetBannerType(EntryPoint.LSRedColor);
            categoryMenus.Add(category, subMenu);
            return subMenu;
        }
    }
    internal void RedrawCraftingMenu(string craftingFlag)
    {
        foreach(UIMenu m in Menu.Children.Values)
        {
            m.Clear();
            MenuPool.Remove(m);
        }
        Show(craftingFlag);
    }
}