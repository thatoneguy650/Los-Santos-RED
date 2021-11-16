using LosSantosRED.lsr.Interface;
using RAGENativeUI;
using RAGENativeUI.Elements;

public class InventoryMenu : Menu
{
    private UIMenu Inventory;
    private IActionable Player;
    private IConsumableSubstances ConsumableSubstances;

    public InventoryMenu(MenuPool menuPool, UIMenu parentMenu, IActionable player, IConsumableSubstances consumableSubstances)
    {
        Player = player;
        ConsumableSubstances = consumableSubstances;
        Inventory = menuPool.AddSubMenu(parentMenu,"Inventory");
        Inventory.OnItemSelect += OnActionItemSelect;
        CreateInventoryMenu();
    }
    public int SelectedPlateIndex { get; set; }

    public override void Hide()
    {
        Inventory.Visible = false;
    }

    public override void Show()
    {
        Update();
        Inventory.Visible = true;
    }
    public override void Toggle()
    {
        if (!Inventory.Visible)
        {
            Update();
            Inventory.Visible = true;
        }
        else
        {
            Inventory.Visible = false;
        }
    }
    public void Update()
    {
        CreateInventoryMenu();
    }
    private void CreateInventoryMenu()
    {
        Inventory.Clear();
        foreach(ConsumableInventoryItem cii in Player.ConsumableItems)
        {
            Inventory.AddItem(new UIMenuItem(cii.ConsumableSubstance?.Name, $"{cii.ConsumableSubstance?.Type} {cii.ConsumableSubstance?.Name} Total: {cii.Amount}") { Enabled = Player.CanPerformActivities });
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
        Inventory.Visible = false;
    }
}