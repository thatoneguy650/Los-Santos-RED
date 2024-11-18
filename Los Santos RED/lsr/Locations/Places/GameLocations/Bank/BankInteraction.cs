using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;


public class BankInteraction
{
    private ILocationInteractable Player;
    private UIMenu AccountsSubMenu;
    private UIMenuItem AccountsSubMenuItem;
    private Bank Bank;
    private Texture BannerImage;
    private MenuPool MenuPool;
    private UIMenu InteractionMenu;

    //private UIMenuListScrollerItem<int> incrementScroller;
    //private UIMenuNumericScrollerItem<int> depositCashScroller;
    //private UIMenuNumericScrollerItem<int> withDrawCashScroller;
    private int MaxAccountValue = 5000000;
    private uint NotificationHandle;
    private bool WithAnimation;

    public BankInteraction(ILocationInteractable player, Bank bank)
    {
        Player = player;
        Bank = bank;
    }
    public void Start(MenuPool menuPool, UIMenu interactionMenu, bool withAnimation)
    {
        MenuPool = menuPool;
        InteractionMenu = interactionMenu;
        WithAnimation = withAnimation;
        AddAccountsSubmenu();
        AddAccountItems();
        InteractionMenu.Visible = true;
        // Player.IsTransacting = true;
        EntryPoint.WriteToConsole("BANK INTERCATION SHOWING MENU");
        Player.OnInteractionMenuCreated(Bank, MenuPool, InteractionMenu);
        EntryPoint.WriteToConsole($"{Bank.Name} BankInteraction Created");
    }
    private void AddAccountsSubmenu()
    {
        AccountsSubMenu = MenuPool.AddSubMenu(InteractionMenu, $"Access Accounts");
        AccountsSubMenuItem = InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1];

        AccountsSubMenu.OnMenuOpen += (sender) =>
        {
            EntryPoint.WriteToConsole("BANK INTERACTION IS TRANSACTING SET TO TRUE");
            Player.IsTransacting = true;
        };

        AccountsSubMenu.OnMenuClose += (sender) =>
        {
            EntryPoint.WriteToConsole("BANK INTERACTION IS TRANSACTING SET TO FALSE");
            Player.IsTransacting = false;
        };
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = $"Access accounts to deposit or withdraw money";
        if (Bank == null)
        {
            return;
        }
        if (Bank.HasBannerImage)
        {
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{Bank.BannerImagePath}");
            AccountsSubMenu.SetBannerType(BannerImage);
            Game.RawFrameRender += (s, e) => MenuPool.DrawBanners(e.Graphics);
            EntryPoint.WriteToConsole("BANK INTERACTION AccountsSubMenu.SetBannerType(BannerImage) RAN");
        }
    }
    private void AddAccountItems()
    {
        AccountsSubMenu.Clear();
        if (Bank == null)
        {
            AddGenericAccounts();
        }
        else
        {
            AddSpecificAccounts();
        }     
    }
    private void AddSpecificAccounts()
    {
        bool added = false;
        foreach (BankAccount bankAccount in Player.BankAccounts.BankAccountList.Where(x=> x.BankContactName == Bank.Name))
        {
            AddAccountSubMenu(bankAccount);
            added = true;
        }
        if(added)
        {
            return;
        }
        AddNewAccount();
    }
    private void AddGenericAccounts()
    {
        foreach(BankAccount bankAccount in Player.BankAccounts.BankAccountList)
        {
            AddAccountSubMenu(bankAccount);
        }
    }
    private void AddAccountSubMenu(BankAccount bankAccount)
    {
        UIMenu BankAccountSubMenu = MenuPool.AddSubMenu(AccountsSubMenu, bankAccount.BankContactName);
        if (Bank?.HasBannerImage == true)
        {
            BankAccountSubMenu.SetBannerType(BannerImage);
        }
        bankAccount.SetSubMenu(BankAccountSubMenu, AccountsSubMenu.MenuItems[AccountsSubMenu.MenuItems.Count() - 1]);
        UIMenuItem setPrimary = new UIMenuItem("Set Primary", $"Set the account as primary. Electronic payments will first be debited from this account, then others as needed.") { RightLabel = bankAccount.IsPrimary ? "Primary" : "" };
        setPrimary.Activated += (sender, e) =>
        {
            Player.BankAccounts.BankAccountList.ForEach(x => x.IsPrimary = false);
            bankAccount.IsPrimary = true;
            DisplayMessage("~g~Primary Account", $"Your account {bankAccount.BankContactName} is now set as primary.");      
            AddAccountItems();
        };
        BankAccountSubMenu.AddItem(setPrimary);
        UpdateDescription(bankAccount);


        UIMenuNumericScrollerItem<int> depositCashScroller = new UIMenuNumericScrollerItem<int>("Deposit", $"Deposit the selected amount of money. ~n~Max of ${MaxAccountValue}", 0, GetOnHandCash(), 100)
        {
            Value = 0,
            Formatter = v => "~r~$" + v.ToString("N0") + "~s~",
        };
        UIMenuNumericScrollerItem<int> withDrawCashScroller = new UIMenuNumericScrollerItem<int>("Withdraw", "Withdraw the selected amount of money.", 0, bankAccount.Money, 100) 
        { 
            Value = 0, 
            Formatter = v => "~g~$" + v.ToString("N0") + "~s~", 
        };

        UIMenuListScrollerItem<int> incrementScroller = new UIMenuListScrollerItem<int>("Increment", "Set the deposit/withdrawl increment.", new List<int>() { 1, 5, 25, 100, 500, 1000, 10000, 100000 }) 
        { 
            SelectedItem = 100, 
            Formatter = v => "$" + v.ToString("N0") 
        };
        incrementScroller.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            depositCashScroller.Step = incrementScroller.SelectedItem;
            withDrawCashScroller.Step = incrementScroller.SelectedItem;
        };
        BankAccountSubMenu.AddItem(incrementScroller);
        BankAccountSubMenu.RefreshIndex();
        int onHandCashNow = GetOnHandCash();

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

                if (WithAnimation)
                {
                    Player.ActivityManager.PerformCashAnimation(true);
                }


                UpdateDescription(bankAccount);
                DisplayMessage("~g~Deposit~s~", $"Deposit Amount: ${toDeposit}.~n~Balance: ${bankAccount.Money}");    
            }
        };
        BankAccountSubMenu.AddItem(depositCashScroller);

        withDrawCashScroller.Activated += (sender, selectedItem) =>
        {
            if (withDrawCashScroller.Value + Player.BankAccounts.GetMoney(false) > Int32.MaxValue)
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

                if (WithAnimation)
                {
                    Player.ActivityManager.PerformCashAnimation(true);
                }

                UpdateDescription(bankAccount);
                DisplayMessage("~g~Withdrawl~s~", $"Withdrawl Amount: ${towithdraw}.~n~Balance: ${bankAccount.Money}");
            }
        };
        BankAccountSubMenu.AddItem(withDrawCashScroller);
        BankAccountSubMenu.RefreshIndex();
    }
    private void AddNewAccount()
    {
        //AccountsSubMenuItem.Description = "No Account";
        UIMenuItem addAccount = new UIMenuItem("Create Account", $"Create a new account with {Bank.Name}.");
        addAccount.Activated += (sender, e) =>
        {
            Player.BankAccounts.CreateNewAccount(Bank);// .BankAccountList.Add(new BankAccount(Bank.Name,Bank.ShortName, 0));
            Bank.DisplayMessage("~g~Account Created", $"You have successfully create an account at {Bank.Name}");
            AddAccountItems();
        };
        AccountsSubMenu.AddItem(addAccount);      
    }
    private void DisplayMessage(string header, string message)
    {
        if(Bank != null)
        {
            Bank.DisplayMessage(header, message);
            return;
        }
        Game.RemoveNotification(NotificationHandle);
        NotificationHandle = Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "Bank Account", header, message);
    }
    private void UpdateDescription(BankAccount bankAccount)
    {
        bankAccount.UpdateMenus();
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

    public void Dispose()
    {
        Game.RawFrameRender -= (s, e) => MenuPool.DrawBanners(e.Graphics);
    }
}

