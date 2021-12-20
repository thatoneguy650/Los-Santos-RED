using LosSantosRED.lsr.Interface;
using RAGENativeUI;
using RAGENativeUI.Elements;

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
        foreach(InventoryItem cii in Player.InventoryItems)
        {
            if(cii.ModItem != null)
            {
                inventoryMenu.AddItem(new UIMenuItem(cii.ModItem?.Name, $"{cii.ModItem.Type} {cii.ModItem?.Name} Total: {cii.Amount}") { Enabled = Player.CanPerformActivities });
            }
        }        
    }
    private void OnActionItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        ModItem selectedStuff = ModItems.Get(selectedItem.Text);
        if(selectedStuff != null && selectedStuff.CanConsume)
        {
            Player.StartConsumingActivity(selectedStuff);
            Player.RemoveFromInventory(selectedStuff, 1);
            EntryPoint.WriteToConsole($"Removed {selectedStuff.Name} ", 3);
        }
        inventoryMenu.Visible = false;
    }
}