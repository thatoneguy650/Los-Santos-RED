using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class MenuItemExtra
{
    public MenuItemExtra()
    {

    }

    public MenuItemExtra(string modItemName, int purchasePrice)
    {
        ExtraName = modItemName;
        PurchasePrice = purchasePrice;
    }
    public MenuItemExtra(string extraName, int purchasePrice, int salesPrice)
    {
        ExtraName = extraName;
        PurchasePrice = purchasePrice;
        SalesPrice = salesPrice;
    }
    public string ExtraName { get; set; }
    public bool Purchaseable => PurchasePrice > 0;
    public bool Sellable => SalesPrice > 0;
    public int PurchasePrice { get; set; } = 5;
    public int SalesPrice { get; set; } = -1;
    public bool HasItem { get; set; } = false;

    public override string ToString()
    {
        if(HasItem)
        {
            return ExtraName + " - Equipped";
        }
        else if (Purchaseable)
        {
            return $"{ExtraName} - ${PurchasePrice}";
        }
        else
        {
            return ExtraName;
        }
    }
}

