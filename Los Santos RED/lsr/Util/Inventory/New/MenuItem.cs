using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MenuItem
{
    public MenuItem()
    {

    }

    public MenuItem(string modItemName, int purchasePrice)
    {
        ModItemName = modItemName;
        PurchasePrice = purchasePrice;
    }
    public MenuItem(string modItemName, int purchasePrice, int salesPrice)
    {
        ModItemName = modItemName;
        PurchasePrice = purchasePrice;
        SalesPrice = salesPrice;
    }
    public string ModItemName { get; set; }
    public bool Purchaseable => PurchasePrice > 0;
    public bool Sellable => SalesPrice > 0;
    public int PurchasePrice { get; set;} = 5;
    public int SalesPrice { get; set; } = -1;
}

