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
    public BankAccount(string bankContactName, int money)
    {
        BankContactName = bankContactName;
        Money = money;
    }
    public string BankContactName { get; set; }
    public int Money { get; set; } = 0;
    public bool IsPrimary { get; set; } = false;

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
}

