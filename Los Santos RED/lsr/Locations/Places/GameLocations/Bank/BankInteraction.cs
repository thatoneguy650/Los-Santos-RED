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
    private UIMenuItem AccountSubMenuItem;
    private Bank Bank;
    private Texture BannerImage;
    private MenuPool MenuPool;
    private UIMenu InteractionMenu;

    private UIMenuListScrollerItem<int> incrementScroller;
    private UIMenuNumericScrollerItem<int> depositCashScroller;
    private UIMenuNumericScrollerItem<int> withDrawCashScroller;
    private int MaxAccountValue = 5000000;

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

        AccountSubMenuItem = InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1];


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
        AccountSubMenu.Clear();
        if (Bank == null)
        {
            return;
        }
        BankAccount bankAccount = Player.BankAccounts.GetAccount(Bank.Name);
        if(bankAccount == null)
        {
            AccountSubMenuItem.Description = "No Account";
            UIMenuItem addAccount = new UIMenuItem("Create Account", $"Create a new checking account with {Bank.Name}.");
            addAccount.Activated += (sender, e) =>
            {
                Player.BankAccounts.BankAccountList.Add(new BankAccount(Bank.Name, 0));
                Bank.DisplayMessage("~g~Account Created",$"You have successfully create an account at {Bank.Name}");
                AddAccountItems();
            };
            AccountSubMenu.AddItem(addAccount);
            return;
        }


        UIMenuItem setPrimary = new UIMenuItem("Set Primary", $"Set the account as primary. Electronic payments will first be debited from this account, then others as needed.") { RightLabel = bankAccount.IsPrimary ? "Primary" :"" };
        setPrimary.Activated += (sender, e) =>
        {
            Player.BankAccounts.BankAccountList.ForEach(x => x.IsPrimary = false);
            bankAccount.IsPrimary = true;
            Bank.DisplayMessage("~g~Primary Account", $"Your account at {Bank.Name} is now set as primary.");
            AddAccountItems();
        };
        AccountSubMenu.AddItem(setPrimary);

        UpdateDescription(bankAccount);
        incrementScroller = new UIMenuListScrollerItem<int>("Increment", "Set the scroll increment.", new List<int>() { 1, 5, 25, 100, 500, 1000, 10000, 100000 }) { SelectedItem = 100, Formatter = v => "$" + v.ToString("N0") };
        incrementScroller.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            depositCashScroller.Step = incrementScroller.SelectedItem;
            withDrawCashScroller.Step = incrementScroller.SelectedItem;
        };
        AccountSubMenu.AddItem(incrementScroller);

        AccountSubMenu.RefreshIndex();

        int onHandCashNow = GetOnHandCash();


        depositCashScroller = new UIMenuNumericScrollerItem<int>("Deposit", $"Deposit the selected amount of money. ~n~Max of ${MaxAccountValue}", 0, GetOnHandCash(), 100) { Value = 0, Formatter = v => "~r~$" + v.ToString("N0") + "~s~", };
        depositCashScroller.Activated += (sender, selectedItem) =>
        {
            int onHandCash = GetOnHandCash();
            int newAccountValue = bankAccount.Money + depositCashScroller.Value;
            if (newAccountValue >= Int32.MaxValue || newAccountValue >= MaxAccountValue)
            {
                Game.DisplaySubtitle("Account is at Maximum!");
                return;
            }
            int toDeposit = depositCashScroller.Value;
            if (depositCashScroller.Value <= onHandCash)
            {
                Player.BankAccounts.GiveMoney(-1 * depositCashScroller.Value, false);
                bankAccount.Money += depositCashScroller.Value;
                depositCashScroller.Maximum = onHandCash;
                withDrawCashScroller.Maximum = bankAccount.Money;



                depositCashScroller.Value = 0;// onHandCash;
                withDrawCashScroller.Value = 0;//bankAccount.Money;




                UpdateDescription(bankAccount);
                Bank.DisplayMessage("~g~Deposit~s~", $"You have successfully deposited ${toDeposit}.~n~Account Balance: ${bankAccount.Money}");
            }
        };
        AccountSubMenu.AddItem(depositCashScroller);
        withDrawCashScroller = new UIMenuNumericScrollerItem<int>("Withdraw", "Withdraw the selected amount of money.", 0, bankAccount.Money, 100) { Value = 0, Formatter = v => "~g~$" + v.ToString("N0") + "~s~", };
        withDrawCashScroller.Activated += (sender, selectedItem) =>
        {
            if(withDrawCashScroller.Value + Player.BankAccounts.GetMoney(false) > Int32.MaxValue)
            {
                Game.DisplaySubtitle("Money is at Maximum!");
                return;
            }
            int towithdraw = withDrawCashScroller.Value;
            if (bankAccount != null && bankAccount.Money >= withDrawCashScroller.Value)
            {
                Player.BankAccounts.GiveMoney(withDrawCashScroller.Value, false);
                bankAccount.Money -= withDrawCashScroller.Value;
                depositCashScroller.Maximum = GetOnHandCash();
                withDrawCashScroller.Maximum = bankAccount.Money;




                depositCashScroller.Value = 0;//GetOnHandCash();
                withDrawCashScroller.Value = 0;// bankAccount.Money;



                UpdateDescription(bankAccount);
                Bank.DisplayMessage("~g~Withdrawl~s~", $"You successfully withdrew ${towithdraw}.~n~Account Balance: ${bankAccount.Money}");
            }
        };
        AccountSubMenu.AddItem(withDrawCashScroller);
        AccountSubMenu.RefreshIndex();
    }
    private void UpdateDescription(BankAccount bankAccount)
    {
        AccountSubMenuItem.Description = $"Balance: ${bankAccount.Money} {(bankAccount.IsPrimary ? " - ~r~Primary~s~" : "")}";
        AccountSubMenu.SubtitleText = $"Balance: ${bankAccount.Money} {(bankAccount.IsPrimary ? " - ~r~Primary~s~" : "")}";
    }
    private int GetOnHandCash()
    {
        int money = Player.BankAccounts.GetMoney(false);
        if (money >= 2147483647)
        {
            money = 2147483646;
        }
        return money;
    }
}

