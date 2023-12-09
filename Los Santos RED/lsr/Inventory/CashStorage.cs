using RAGENativeUI.Elements;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using LosSantosRED.lsr.Interface;
using System.Windows.Forms;
using System.Xml.Linq;
using Mod;

public class CashStorage
{
    private IInteractionable Player;
    private UIMenu cashStorageSubMenu;
    private UIMenuListScrollerItem<int> incrementScroller;
    private UIMenuNumericScrollerItem<int> storeCashScroller;
    private UIMenuNumericScrollerItem<int> removeCashScroller;
    private int MaxAccountValue = 5000000;
    private GameLocation GameLocation;
    private uint NotificationHandle;
    private UIMenuItem cashStorageSubMenuItem;
    private bool WithAnimation;
    public CashStorage()
    {

    }

    public int StoredCash { get; set; }

    public void Reset()
    {
        StoredCash = 0;
    }

    public void CreateInteractionMenu(IInteractionable player, MenuPool MenuPool, UIMenu InteractionMenu, GameLocation gameLocation, bool withAnimation, bool removeBanner)//, IMessageDisplayable messageDisplayable)
    {
        Player = player;
        GameLocation = gameLocation;
        WithAnimation = withAnimation;
        cashStorageSubMenu = MenuPool.AddSubMenu(InteractionMenu, "Stored Cash");
        cashStorageSubMenuItem = InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1];
        cashStorageSubMenuItem.Description = "Manage stored cash.";
        if (removeBanner)
        {
            cashStorageSubMenu.RemoveBanner();
        }
        else if (GameLocation == null || !GameLocation.HasBannerImage)
        {
            cashStorageSubMenu.SetBannerType(EntryPoint.LSRedColor);
        }
        UpdateStoredCash();
    }

    private void UpdateStoredCash()
    {
        cashStorageSubMenu.Clear();
        UpdateDescription();
        int defaultDelectedItem = 100;
        if(Player.BankAccounts.GetOnHandCashSafe() < defaultDelectedItem)
        {
            defaultDelectedItem = 1;
        }
        incrementScroller = new UIMenuListScrollerItem<int>("Increment", "Set the scroll increment.", new List<int>() { 1, 5, 25, 100, 500, 1000, 10000, 100000 }) 
        {
            SelectedItem = defaultDelectedItem,
            Formatter = v => "$"+ v.ToString("N0") 
        };
        storeCashScroller = new UIMenuNumericScrollerItem<int>("Store Cash", $"Store the selected amount of cash. Max of ${MaxAccountValue}", 0, Player.BankAccounts.GetOnHandCashSafe(), 100) 
        { 

            Value = 0, 
            Formatter = v => "~r~$" + v + "~s~", 
        };
        removeCashScroller = new UIMenuNumericScrollerItem<int>("Remove Cash", "Remove the selected amount of cash.", 0, StoredCash, 100) 
        { 
            Value = StoredCash, 
            Formatter = v => "~g~$" + v + "~s~", 
        };
        incrementScroller.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            storeCashScroller.Step = incrementScroller.SelectedItem;
            removeCashScroller.Step = incrementScroller.SelectedItem;
        };
        storeCashScroller.Activated += (sender, selectedItem) =>
        {
            int onHandCash = Player.BankAccounts.GetOnHandCashSafe();
            int newStoreValue = StoredCash + storeCashScroller.Value;
            if (newStoreValue >= Int32.MaxValue || newStoreValue >= MaxAccountValue)
            {
                Game.DisplaySubtitle("Account is at Maximum!");
                return;
            }
            int toStore = storeCashScroller.Value;
            if (storeCashScroller.Value <= Player.BankAccounts.GetOnHandCashSafe())
            {
                Player.BankAccounts.GiveMoney(-1 * storeCashScroller.Value, false);
                StoredCash += storeCashScroller.Value;
                storeCashScroller.Maximum = Player.BankAccounts.GetOnHandCashSafe();
                removeCashScroller.Maximum = StoredCash;
                storeCashScroller.Value = 0;// Player.BankAccounts.GetOnHandCashSafe();
                removeCashScroller.Value = 0;// StoredCash;



                if (WithAnimation)
                {
                    Player.ActivityManager.PerformCashAnimation(false);
                }



                if (GameLocation != null)
                {
                    GameLocation.DisplayMessage("~g~Stored~s~", $"You have stored ${toStore}.~n~Current Balance: ${StoredCash}");
                }
                else
                {
                    Game.RemoveNotification(NotificationHandle);
                    NotificationHandle = Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "Cash", "~g~Stored~s~", $"You have stored ${toStore}.~n~Current Balance: ${StoredCash}");
                }
                UpdateDescription();
            }
        };

        removeCashScroller.Activated += (sender, selectedItem) =>
        {
            if (removeCashScroller.Value + Player.BankAccounts.GetMoney(false) >= Int32.MaxValue)
            {
                Game.DisplaySubtitle("Money is at Maximum!");
                return;
            }
            int toRemove = removeCashScroller.Value;
            if (StoredCash >= removeCashScroller.Value)
            {
                Player.BankAccounts.GiveMoney(removeCashScroller.Value, false);
                StoredCash -= removeCashScroller.Value;
                storeCashScroller.Maximum = Player.BankAccounts.GetOnHandCashSafe();
                removeCashScroller.Maximum = StoredCash;
                storeCashScroller.Value = 0;// Player.BankAccounts.GetOnHandCashSafe();
                removeCashScroller.Value = 0;// StoredCash;


                if (WithAnimation)
                {
                    Player.ActivityManager.PerformCashAnimation(true);
                }


                if (GameLocation != null)
                {
                    GameLocation?.DisplayMessage("~g~Removed~s~", $"You have removed ${toRemove}.~n~Current Balance: ${StoredCash}");
                }
                else
                {
                    Game.RemoveNotification(NotificationHandle);
                    NotificationHandle = Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "Cash", "~g~Removed~s~", $"You have removed ${toRemove}.~n~Current Balance: ${StoredCash}");
                }
                UpdateDescription();
            }
        };
        cashStorageSubMenu.AddItem(incrementScroller);
        cashStorageSubMenu.AddItem(storeCashScroller);
        cashStorageSubMenu.AddItem(removeCashScroller);
    }
    private void UpdateDescription()
    {
        cashStorageSubMenuItem.Description = $"Balance: ${StoredCash}";
        cashStorageSubMenu.SubtitleText = $"Balance: ${StoredCash}";
    }

}

