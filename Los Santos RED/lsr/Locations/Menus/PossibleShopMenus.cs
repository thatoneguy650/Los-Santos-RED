using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PossibleShopMenus
{
    public PossibleShopMenus()
    {
    }

    public List<ShopMenu> ShopMenuList { get; private set; } = new List<ShopMenu>();
    public List<ShopMenuGroup> ShopMenuGroups { get; private set; } = new List<ShopMenuGroup>();
    public List<PropShopMenu> PropShopMenus { get; private set; } = new List<PropShopMenu>();
}

