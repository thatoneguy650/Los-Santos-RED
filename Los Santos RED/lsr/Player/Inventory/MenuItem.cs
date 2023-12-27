using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
[XmlInclude(typeof(VariablePriceMenuItem))]
public class MenuItem
{
    private int originalPurchasePrice;
    private int originalSalesPrice;
    private bool isSetFree = false;
    public int NumberOfItemsSoldToPlayer { get; set; }
    public int NumberOfItemsPurchasedByPlayer { get; set; }
    public MenuItem()
    {

    }
    public MenuItem(string modItemName)
    {
        ModItemName = modItemName;
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
    public bool Purchaseable => PurchasePrice > 0 || isSetFree || IsFree;
    public bool Sellable => SalesPrice > 0;
    public int PurchasePrice { get; set;} = 5;
    public int SalesPrice { get; set; } = -1;
    public bool IsIllicilt { get; set; } = false;
    public List<MenuItemExtra> Extras { get; set; } = new List<MenuItemExtra>();
    public int SubPrice { get; set; } = 1;
    public int SubAmount { get; set; } = 30;
    public int MinimumPurchaseAmount { get; set; } = 1;
    public int MaximumPurchaseAmount { get; set; } = 10;
    public int PurchaseIncrement { get; set; } = 1;
    public int NumberOfItemsToSellToPlayer { get; set; } = -1;
    public int NumberOfItemsToPurchaseFromPlayer { get; set; } = -1;
    public bool IsFree { get; set; } = false;

    [XmlIgnore]
    public UIMenuNumericScrollerItem<int> PurchaseScroller { get; set; }

    [XmlIgnore]
    public ModItem ModItem { get; set; }


    //public int MaxAmount => MinimumPurchaseAmount == 1 ? 10 : MinimumPurchaseAmount + (10 * PurchaseIncrement);
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
    public virtual void UpdatePrices()
    {

    }
    public virtual void UpdateStock()
    {
        NumberOfItemsPurchasedByPlayer = 0;
        NumberOfItemsSoldToPlayer = 0;
    }
}

