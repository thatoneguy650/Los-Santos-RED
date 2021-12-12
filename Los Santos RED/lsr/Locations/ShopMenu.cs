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
    public List<MenuItem> Items { get; set; }
    public string ID { get; set; }
    public string Name { get; set; }
    public void Randomize()
    {
        //foreach(MenuItem mi in Items)
        //{
        //    if(mi.Purchaseable)
        //    {
        //        mi.PurchasePrice += (int)((float)mi.PurchasePrice * RandomItems.GetRandomNumber(0.0f,0.2f));
        //    }
        //    if(mi.Sellable)
        //    {
        //        mi.SalesPrice -= (int)((float)mi.PurchasePrice * RandomItems.GetRandomNumber(0.2f, 0.4f));
        //    }
        //    if(mi.Purchaseable && mi.Sellable && mi.PurchasePrice <= mi.SalesPrice)
        //    {
        //        mi.PurchasePrice++;
        //        mi.PurchasePrice += (int)((float)mi.PurchasePrice * RandomItems.GetRandomNumber(0.1f, 0.4f));
        //    }
        //}
    }
}

