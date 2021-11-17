using LosSantosRED.lsr.Interface;
using RAGENativeUI;
using RAGENativeUI.Elements;

public class InventoryMenu : Menu
{
    private UIMenu inventoryMenu;
    private IActionable Player;
    private IConsumableSubstances ConsumableSubstances;

    public InventoryMenu(MenuPool menuPool, UIMenu parentMenu, IActionable player, IConsumableSubstances consumableSubstances)
    {
        Player = player;
        ConsumableSubstances = consumableSubstances;
        inventoryMenu = menuPool.AddSubMenu(parentMenu,"Inventory");
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
        foreach(ConsumableInventoryItem cii in Player.ConsumableItems)
        {
            inventoryMenu.AddItem(new UIMenuItem(cii.ConsumableSubstance?.Name, $"{cii.ConsumableSubstance?.Type} {cii.ConsumableSubstance?.Name} Total: {cii.Amount}") { Enabled = Player.CanPerformActivities });
        }        
    }
    private void OnActionItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        ConsumableSubstance selectedStuff = ConsumableSubstances.Get(selectedItem.Text);
        if(selectedStuff != null)
        {
            Player.StartConsumingActivity(selectedStuff);
            Player.RemoveFromInventory(selectedStuff, 1);
            EntryPoint.WriteToConsole($"Removed {selectedStuff.Name} ", 3);
        }
        inventoryMenu.Visible = false;
    }
}