using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class MenuItem
{
    private int originalPurchasePrice;
    private int originalSalesPrice;
    private bool isSetFree = false;
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
    public bool Purchaseable => PurchasePrice > 0 || isSetFree;
    public bool Sellable => SalesPrice > 0;
    public int PurchasePrice { get; set;} = 5;
    public int SalesPrice { get; set; } = -1;
    public bool IsIllicilt { get; set; } = false;
    public List<MenuItemExtra> Extras { get; set; } = new List<MenuItemExtra>();
    public int SubPrice { get; set; } = 1;
    public int SubAmount { get; set; } = 30;
    public int NumberOfItemsToSellToPlayer { get; set; } = -1;
    public int NumberOfItemsToPurchaseFromPlayer { get; set; } = -1;


    [XmlIgnore]
    public int ItemsSoldToPlayer { get; set; } = 0;
    [XmlIgnore]
    public int ItemsBoughtFromPlayer { get; set; } = 0;

    public void SetFree()
    {
        if (!isSetFree)
        {
            originalPurchasePrice = PurchasePrice;
            originalSalesPrice = SalesPrice;
            isSetFree = true;
            PurchasePrice = 0;
            SalesPrice = 0;
        }
    }
    public void ResetPrice()
    {
        if (isSetFree)
        {
            PurchasePrice = originalPurchasePrice;
            SalesPrice = originalSalesPrice;
            isSetFree = false;
        }
    }
}

