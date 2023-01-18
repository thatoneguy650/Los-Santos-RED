using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PercentageSelectShopMenu
{
    public PercentageSelectShopMenu()
    {
    }

    public PercentageSelectShopMenu(ShopMenu shopMenu, int selectChance)
    {
        ShopMenu = shopMenu;
        SelectChance = selectChance;
    }
    public ShopMenu ShopMenu { get; set; }
    public int SelectChance { get; set; } = 0;
}

