using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class ShopMenu
{
    public ShopMenu()
    {

    }
    public ShopMenu(string iD, string name, List<MenuItem> items)
    {
        ID = iD;
        Name = name;
        Items = items;
    }
    public ShopMenu(string iD, string name, string groupName, List<MenuItem> items)
    {
        ID = iD;
        Name = name;
        Items = items;
        GroupName = groupName;
    }



    public ShopMenu(string iD, string name, List<MenuItem> items, string groupName)
    {
        ID = iD;
        Name = name;
        Items = items;
        GroupName = groupName;
    }
    public string ID { get; set; }
    public string Name { get; set; }
    public string GroupName { get; set; } = "";
    public string BannerOverride { get; set; } 
    public List<MenuItem> Items { get; set; }
    public void Randomize()
    {
        foreach (MenuItem mi in Items)
        {
            int existingPurchasePrice = mi.PurchasePrice;
            int existingSalesPrice = mi.SalesPrice;
            if (mi.Purchaseable && mi.PurchasePrice >= 15)
            {
                int purchaseModAmount = (int)Math.Ceiling((float)mi.PurchasePrice * RandomItems.GetRandomNumber(0.0f, 0.25f));
                if(RandomItems.RandomPercent(50))
                {
                    purchaseModAmount *= -1;
                }
                mi.PurchasePrice += purchaseModAmount;//add anywhere between 0 and 20 percent
            }
            if (mi.Sellable && mi.SalesPrice >= 15)
            {
                int salesModAmount = (int)Math.Ceiling((float)mi.SalesPrice * RandomItems.GetRandomNumber(0.0f, 0.25f));
                if (RandomItems.RandomPercent(50))
                {
                    salesModAmount *= -1;
                }
                mi.SalesPrice += salesModAmount;//add anywhere between 0 and 20 percent
            }
            EntryPoint.WriteToConsole($"MODIFIED {mi.ModItem?.DisplayName} FROM:{existingPurchasePrice}-{existingSalesPrice} TO:{mi.PurchasePrice}-{mi.SalesPrice}");
            if(mi.SalesPrice >= mi.PurchasePrice)
            {
                mi.PurchasePrice = existingPurchasePrice;
                mi.SalesPrice = existingSalesPrice;
                EntryPoint.WriteToConsole($"MODIFIED {mi.ModItem?.DisplayName} ERROR RESETTING");
            }
        }
    }
    public void SetFree()
    {
        foreach (MenuItem mi in Items)
        {
            mi.SetFree();
        }
    }
    public void ResetPrice()
    {
        foreach (MenuItem mi in Items)
        {
            mi.ResetPrice();
        }
    }
}

