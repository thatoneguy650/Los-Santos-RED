using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BankInteraction
{
    private ILocationInteractable Player;
    private UIMenu AccountSubMenu;
    private Bank Bank;
    private Texture BannerImage;
    private MenuPool MenuPool;
    private UIMenu InteractionMenu;

    private UIMenuListScrollerItem<int> incrementScroller;
    private UIMenuNumericScrollerItem<int> depositCashScroller;
    private UIMenuNumericScrollerItem<int> withDrawCashScroller;

    public BankInteraction(ILocationInteractable player, Bank bank)
    {
        Player = player;
        Bank = bank;
    }
    public void Start(MenuPool menuPool, UIMenu interactionMenu)
    {
        MenuPool = menuPool;
        InteractionMenu = interactionMenu;
        AddAccountsSubmenu();
        AddAccountItems();
        InteractionMenu.Visible = true;
        Player.IsTransacting = true;

    }

    private void AddAccountsSubmenu()
    {
        AccountSubMenu = MenuPool.AddSubMenu(InteractionMenu, $"Access Account");
        InteractionMenu.OnMenuClose += (sender) =>
        {
            Player.IsTransacting = false;
        };
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = $"Access account to deposit or withdraw money";
        if (Bank == null)
        {
            return;
        }
        if (Bank.HasBannerImage)
        {
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{Bank.BannerImagePath}");
            AccountSubMenu.SetBannerType(BannerImage);
        }
    }



    private void AddAccountItems()
    {

        incrementScroller = new UIMenuListScrollerItem<int>("Increment", "Set the scroll increment.", new List<int>() { 1, 5, 25, 100, 500, 1000, 10000, 100000 }) { SelectedItem = 1, Formatter = v => "$" + v.ToString("N0") };
        incrementScroller.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            depositCashScroller.Step = incrementScroller.SelectedItem;
            withDrawCashScroller.Step = incrementScroller.SelectedItem;
        };
        AccountSubMenu.AddItem(incrementScroller);

        UIMenuItem checkBalance = new UIMenuItem("Check Balance", "Check the current account balance.");
        checkBalance.Activated += (sender, e) =>
        {
            Game.DisplaySubtitle($"Current Balance: ${Player.BankAccounts.AccountMoney}");
        };
        AccountSubMenu.AddItem(checkBalance);


        EntryPoint.WriteToConsole($"Current Balance: ${Player.BankAccounts.Money} ${Player.BankAccounts.AccountMoney}");


        depositCashScroller = new UIMenuNumericScrollerItem<int>("Deposit", "Deposit the selected amount of money.", 0, Player.BankAccounts.Money, 1) { Value = Player.BankAccounts.Money, Formatter = v => "~r~$" + v.ToString("N0") + "~s~", };
        depositCashScroller.Activated += (sender, selectedItem) =>
        {
            if (depositCashScroller.Value <= Player.BankAccounts.Money)
            {
                Player.BankAccounts.GiveMoney(-1 * depositCashScroller.Value);
                Player.BankAccounts.AccountMoney += depositCashScroller.Value;
                depositCashScroller.Maximum = Player.BankAccounts.Money;
                withDrawCashScroller.Maximum = Player.BankAccounts.AccountMoney;
                depositCashScroller.Value = Player.BankAccounts.Money;
                withDrawCashScroller.Value = Player.BankAccounts.AccountMoney;
            }
        };
        AccountSubMenu.AddItem(depositCashScroller);
        
        withDrawCashScroller = new UIMenuNumericScrollerItem<int>("Withdraw", "Withdraw the selected amount of money.", 0, Player.BankAccounts.AccountMoney, 1) { Value = Player.BankAccounts.AccountMoney, Formatter = v => "~g~$" + v.ToString("N0") + "~s~", };
        withDrawCashScroller.Activated += (sender, selectedItem) =>
        {
            if (Player.BankAccounts.AccountMoney >= withDrawCashScroller.Value)
            {
                Player.BankAccounts.GiveMoney(withDrawCashScroller.Value);
                Player.BankAccounts.AccountMoney -= withDrawCashScroller.Value;
                depositCashScroller.Maximum = Player.BankAccounts.Money;
                withDrawCashScroller.Maximum = Player.BankAccounts.AccountMoney;
                depositCashScroller.Value = Player.BankAccounts.Money;
                withDrawCashScroller.Value = Player.BankAccounts.AccountMoney;
            }
        };        
        AccountSubMenu.AddItem(withDrawCashScroller);
    }
}

