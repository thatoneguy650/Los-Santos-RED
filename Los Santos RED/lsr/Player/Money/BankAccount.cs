using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BankAccount
{
    private UIMenu BankAccountSubMenu;
    private UIMenuItem BankAccountSubMenuItem;
    public BankAccount()
    {

    }
    public BankAccount(string bankContactName,string accountName, int money)
    {
        BankContactName = bankContactName;
        Money = money;
        AccountName = accountName;
    }
    public string BankContactName { get; set; }
    public int Money { get; set; } = 0;
    public bool IsPrimary { get; set; } = false;
    public string AccountName { get; set; } 
    public string CashDisplay => $" ({(string.IsNullOrEmpty(AccountName) ? BankContactName : AccountName)} - ${Money})";
    public void SetSubMenu(UIMenu bankAccountSubMenu, UIMenuItem bankAccountSubMenuItem)
    {
        BankAccountSubMenu = bankAccountSubMenu;
        BankAccountSubMenuItem = bankAccountSubMenuItem;
    }
    public void UpdateMenus()
    {
        BankAccountSubMenuItem.Description = $"Balance: ${Money} {(IsPrimary ? " - ~r~Primary~s~" : "")}";
        BankAccountSubMenu.SubtitleText = $"Balance: ${Money} {(IsPrimary ? " - ~r~Primary~s~" : "")}";
    }
    public override string ToString()
    {
        return AccountName;
    }
}

