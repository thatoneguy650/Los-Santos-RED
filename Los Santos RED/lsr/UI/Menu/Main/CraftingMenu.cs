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
    private UIMenu Menu;
    private MenuPool MenuPool;
    private ICraftableItems CraftableItems;
    private Mod.Crafting Crafting;

    public CraftingMenu(MenuPool menuPool, ICraftableItems craftableItems, Mod.Crafting crafting)
    {
        MenuPool = menuPool;
        CraftableItems = craftableItems;
        Crafting = crafting;
    }
    public void Setup()
    {
        Menu = new UIMenu("Crafting",string.Empty);
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
            Menu.Visible = true;
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
    private void CreateCraftableItems()
    {
        foreach (var craftableItem in CraftableItems.Items)
        {
            if(craftableItem.SingleUnit)
            {
                UIMenuItem itemMenu = new UIMenuItem(craftableItem.Name, craftableItem.IngredientList);
                itemMenu.Activated += (s, e) =>
                {
                    Crafting.CraftItem(itemMenu.Text);
                };
                Menu.AddItem(itemMenu);
            }
            else
            {
                UIMenuNumericScrollerItem<int> itemMenu = new UIMenuNumericScrollerItem<int>(craftableItem.Name, craftableItem.IngredientList, 1, 10, 1);
                itemMenu.Value = 1;
                itemMenu.Activated += (s, e) =>
                {
                    Crafting.CraftItem(itemMenu.Text, itemMenu.Value);
                };
                Menu.AddItem(itemMenu);
            }
        }
    }
}