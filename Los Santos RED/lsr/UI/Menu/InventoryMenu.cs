using LosSantosRED.lsr.Interface;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

public class InventoryMenu : Menu
{
    private UIMenu inventoryMenu;
    private IActionable Player;
    private IModItems ModItems;

    public InventoryMenu(MenuPool menuPool, UIMenu parentMenu, IActionable player, IModItems modItems)
    {
        Player = player;
        ModItems = modItems;
        inventoryMenu = menuPool.AddSubMenu(parentMenu,"Inventory");
        parentMenu.MenuItems[parentMenu.MenuItems.Count() - 1].Description = "Access purchased items.";
        parentMenu.MenuItems[parentMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Heart;
        inventoryMenu.SetBannerType(EntryPoint.LSRedColor);
        inventoryMenu.OnItemSelect += OnActionItemSelect;
        CreateInventoryMenu();
    }
    public int SelectedPlateIndex { get; set; }

    public override void Hide()
    {
        inventoryMenu.Visible = false;
    }

    public override void Show()
    {
        Update();
        inventoryMenu.Visible = true;
    }
    public override void Toggle()
    {
        if (!inventoryMenu.Visible)
        {
            Update();
            inventoryMenu.Visible = true;
        }
        else
        {
            inventoryMenu.Visible = false;
        }
    }
    public void Update()
    {
        CreateInventoryMenu();
    }
    private void CreateInventoryMenu()
    {
        inventoryMenu.Clear();
        foreach(InventoryItem cii in Player.Inventory.Items)
        {
            if(cii.ModItem != null)
            {
               // inventoryMenu.AddItem(new UIMenuListScrollerItem<string>(cii.ModItem?.Name, cii.Description, new List<string>() { "Use", "Drop" }) { Enabled = Player.CanPerformActivities });
                inventoryMenu.AddItem(new UIMenuItem(cii.ModItem?.Name, cii.Description) { Enabled = Player.CanPerformActivities });
            }
        }        
    }
    private void OnActionItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        ModItem selectedStuff = ModItems.Get(selectedItem.Text);
        if (selectedStuff != null)
        {
            if (selectedStuff.CanConsume)
            {
                Player.StartConsumingActivity(selectedStuff);
              // Player.RemoveFromInventory(selectedStuff, 1);
                EntryPoint.WriteToConsole($"Removed {selectedStuff.Name} ", 3);  
            }
        }
        inventoryMenu.Visible = false;
    }
}