using LosSantosRED.lsr.Interface;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

public class InventoryMenu : Menu
{
    private UIMenu inventoryMenu;
    private ILocationInteractable ActionablePlayer;
    private IActivityPerformable ActivityPerformablePlayer;
    private IModItems ModItems;
    private bool IsInside;

    public InventoryMenu(MenuPool menuPool, UIMenu parentMenu, ILocationInteractable player, IModItems modItems, bool isInside)
    {
        ActionablePlayer = player;
        ModItems = modItems;
        IsInside = isInside;
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
        foreach(InventoryItem cii in ActionablePlayer.Inventory.Items)
        {
            if(cii.ModItem != null)
            {
               // inventoryMenu.AddItem(new UIMenuListScrollerItem<string>(cii.ModItem?.Name, cii.Description, new List<string>() { "Use", "Drop" }) { Enabled = Player.CanPerformActivities });


                if(IsInside)
                {
                    inventoryMenu.AddItem(new UIMenuItem(cii.ModItem?.Name, cii.Description) { RightLabel = cii.RightLabel, Enabled = (cii.ModItem?.ChangesHealth == true) });
                }
                else
                {
                    inventoryMenu.AddItem(new UIMenuItem(cii.ModItem?.Name, cii.Description) { RightLabel = cii.RightLabel, Enabled = ActionablePlayer.CanPerformActivities });
                }

                
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
                if(IsInside)
                {
                    ActionablePlayer.StartConsumingActivity(selectedStuff, false);
                    InventoryItem ii = ActionablePlayer.Inventory.Get(selectedStuff);
                    if (ii != null)
                    {
                        if(ii.Amount > 0)
                        {
                            selectedItem.Description = ii.Description;
                            selectedItem.RightLabel = ii.RightLabel;
                        }
                        else
                        {
                            sender.RemoveItemAt(index);
                            sender.RefreshIndex();
                        }
                        
                    }
                    else
                    {
                        selectedItem.Enabled = false;
                    }
                    

                }
                else
                {
                    ActionablePlayer.StartConsumingActivity(selectedStuff, true);
                    inventoryMenu.Visible = false;
                }
              // Player.RemoveFromInventory(selectedStuff, 1);
                EntryPoint.WriteToConsole($"Removed {selectedStuff.Name} ", 3);  
            }
        }
        
    }
}