using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System.Collections.Generic;
using System.Linq;

public class TransactionMenu : Menu
{
    private UIMenu Menu;
    private IInteractionable Player;
    private List<ConsumableSubstance> ConsumableSubstances;
    public TransactionMenu(MenuPool menuPool, IInteractionable player, string MenuText, string MenuDescription, List<ConsumableSubstance> consumableSubstances)
    {
        Menu = new UIMenu(MenuText, MenuDescription);
        Player = player;
        ConsumableSubstances = consumableSubstances;
        menuPool.Add(Menu);
        Menu.OnItemSelect += OnItemSelect;
        CreateTransactionMenu();
    }
    public override void Hide()
    {
        Menu.Visible = false;
    }
    public override void Show()
    {
        if (!Menu.Visible)
        {
            CreateTransactionMenu();
            Menu.Visible = true;
        }
    }
    public override void Toggle()
    {
        if (!Menu.Visible)
        {
            CreateTransactionMenu();
            Menu.Visible = true;
        }
        else
        {
            Menu.Visible = false;
        }
    }
    private void CreateTransactionMenu()
    {
        Menu.Clear();
        foreach (ConsumableSubstance cii in ConsumableSubstances)
        {
            Menu.AddItem(new UIMenuItem(cii.Name, $"{cii.Name} ${cii.Price}"));
        }
        Menu.OnItemSelect += OnItemSelect;
    }
    private void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        ConsumableSubstance ToAdd = ConsumableSubstances.Where(x => x.Name == selectedItem.Text).FirstOrDefault();
        if(ToAdd != null && Player.Money >= ToAdd.Price)
        {
            Player.AddToInventory(ToAdd, ToAdd.AmountPerPackage);
            Player.GiveMoney(-1 * ToAdd.Price);
        }
        Menu.Visible = false;
    }
}