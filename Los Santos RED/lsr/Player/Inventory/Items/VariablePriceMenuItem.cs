using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class VariablePriceMenuItem : MenuItem
{
    public VariablePriceMenuItem()
    {

    }
    public VariablePriceMenuItem(string modItemName,int minPurchasePrice, int maxPurchasePrice) : base(modItemName)
    {
        MinPurchasePrice = minPurchasePrice;
        MaxPurchasePrice = maxPurchasePrice;
    }
    //public VariablePriceMenuItem(string modItemName, int minSalesPrice, int maxSalesPrice) : base(modItemName)
    //{
    //    MinSalesPrice = minSalesPrice;
    //    MaxSalesPrice = maxSalesPrice;
    //}
    public VariablePriceMenuItem(string modItemName, int minPurchasePrice, int maxPurchasePrice, int minSalesPrice, int maxSalesPrice) : base(modItemName)
    {
        MinSalesPrice = minSalesPrice;
        MaxSalesPrice = maxSalesPrice;
        MinPurchasePrice = minPurchasePrice;
        MaxPurchasePrice = maxPurchasePrice;
    }
    public int MinSalesPrice { get; set; }
    public int MaxSalesPrice { get; set; }
    public int MinPurchasePrice { get; set; }
    public int MaxPurchasePrice { get; set; }
    public override void UpdatePrices()
    {
        if(MinSalesPrice > 0 && MaxSalesPrice > 0)
        {
            SalesPrice = RandomItems.GetRandomNumberInt(MinSalesPrice, MaxSalesPrice);
        }
        if(MinPurchasePrice > 0 && MaxPurchasePrice > 0)
        {
            PurchasePrice = RandomItems.GetRandomNumberInt(MinPurchasePrice, MaxPurchasePrice);
        }
        base.UpdatePrices();
    }
}

