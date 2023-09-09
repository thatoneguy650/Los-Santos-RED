using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BankAccount
{
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
}

