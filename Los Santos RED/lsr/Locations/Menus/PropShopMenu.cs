using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PropShopMenu
{
    public PropShopMenu()
    {
    }

    public PropShopMenu(string modelName, string shopMenuID)
    {
        ModelName = modelName;
        ShopMenuID = shopMenuID;
    }

    public PropShopMenu(uint modelHash, string shopMenuID)
    {
        ModelHash = modelHash;
        ShopMenuID = shopMenuID;
    }

    public string ModelName { get; set; }
    public uint ModelHash { get; set; } 
    public string ShopMenuID { get; set; }

}

