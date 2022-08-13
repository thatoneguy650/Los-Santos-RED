using LosSantosRED.lsr.Interface;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

public class InventoryMenu : Menu
{
    private MenuPool MenuPool;
    private UIMenu ParentMenu;

    private UIMenu inventoryMenu;
    private List<UIMenu> CategoryMenus = new List<UIMenu>();

    private ILocationInteractable ActionablePlayer;
    private IModItems ModItems;

    private bool IsInside;
    public int SelectedPlateIndex { get; set; }
    public InventoryMenu(MenuPool menuPool, UIMenu parentMenu, ILocationInteractable player, IModItems modItems, bool isInside)
    {
        MenuPool = menuPool;
        ParentMenu = parentMenu;
        ActionablePlayer = player;
        ModItems = modItems;
        IsInside = isInside;
        MenuPool = menuPool;
    }
    public void Setup()
    {
        inventoryMenu = MenuPool.AddSubMenu(ParentMenu, "Inventory");
        ParentMenu.MenuItems[ParentMenu.MenuItems.Count() - 1].Description = "Access purchased items.";
        ParentMenu.MenuItems[ParentMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Heart;
        inventoryMenu.SetBannerType(EntryPoint.LSRedColor);
        inventoryMenu.OnItemSelect += OnActionItemSelect;
    }
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
        CategoryMenus.Clear();
        foreach (ItemType itemType in (ItemType[])Enum.GetValues(typeof(ItemType)))
        {
            int totalItems = ActionablePlayer.Inventory.Items.Count(x => x.ModItem?.ItemType == itemType && x.ModItem?.CanConsume == true);
            if (totalItems > 0)
            {
                UIMenu itemsubMenu = MenuPool.AddSubMenu(inventoryMenu, itemType.ToString());
                inventoryMenu.MenuItems[inventoryMenu.MenuItems.Count() - 1].Description = itemType.ToString();
                //inventoryMenu.MenuItems[inventoryMenu.MenuItems.Count() - 1].RightLabel = $"{totalItems} Item(s)";
                itemsubMenu.SetBannerType(EntryPoint.LSRedColor);
                itemsubMenu.OnItemSelect += OnActionItemSelect;
                CategoryMenus.Add(itemsubMenu);
            }
        }
        foreach (InventoryItem cii in ActionablePlayer.Inventory.Items)
        {
            if(cii.ModItem != null)
            {
                // inventoryMenu.AddItem(new UIMenuListScrollerItem<string>(cii.ModItem?.Name, cii.Description, new List<string>() { "Use", "Drop" }) { Enabled = Player.CanPerformActivities });
                UIMenu SubMenu = CategoryMenus.FirstOrDefault(x => x.SubtitleText == cii.ModItem.ItemType.ToString());
                if (SubMenu != null)
                {
                    if (IsInside)
                    {
                        SubMenu.AddItem(new UIMenuItem(cii.ModItem?.Name, cii.Description) { RightLabel = cii.RightLabel, Enabled = (cii.ModItem?.ChangesHealth == true || cii.ModItem?.ChangesNeeds == true) });
                    }
                    else
                    {
                        SubMenu.AddItem(new UIMenuItem(cii.ModItem?.Name, cii.Description) { RightLabel = cii.RightLabel, Enabled = true });
                    }
                }
                else
                {
                    if (IsInside)
                    {
                        inventoryMenu.AddItem(new UIMenuItem(cii.ModItem?.Name, cii.Description) { RightLabel = cii.RightLabel, Enabled = (cii.ModItem?.ChangesHealth == true || cii.ModItem?.ChangesNeeds == true)});
                    }
                    else
                    {
                        inventoryMenu.AddItem(new UIMenuItem(cii.ModItem?.Name, cii.Description) { RightLabel = cii.RightLabel, Enabled = true });
                    }
                }
                
            }
        }        
    }
    private void OnActionItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (ActionablePlayer.CanPerformActivities)
        {
            EntryPoint.WriteToConsole($"Inventory On Action Item Selected selectedItem.Text: {selectedItem.Text} sender.SubtitleText: {sender.SubtitleText} index: {index}");
            ModItem selectedModItem = ModItems.Get(selectedItem.Text);
            if (selectedModItem != null)
            {
                if (selectedModItem.CanConsume)
                {
                    if (IsInside)
                    {
                        ActionablePlayer.StartConsumingActivity(selectedModItem, false);

                    }
                    else
                    {
                        ActionablePlayer.StartConsumingActivity(selectedModItem, true);
                    }
                    InventoryItem ii = ActionablePlayer.Inventory.Get(selectedModItem);
                    if (ii != null)
                    {
                        if (ii.Amount > 0)
                        {
                            selectedItem.Description = ii.Description;
                            selectedItem.RightLabel = ii.RightLabel;
                        }
                        else
                        {
                            selectedItem.Enabled = false;
                            selectedItem.RightLabel = "None";
                        }
                    }
                    else
                    {
                        selectedItem.Enabled = false;
                        selectedItem.RightLabel = "None";
                        selectedItem.Description = "";
                    }
                    EntryPoint.WriteToConsole($"Removed {selectedModItem.Name} ", 3);
                }
            }
        }
    }
}