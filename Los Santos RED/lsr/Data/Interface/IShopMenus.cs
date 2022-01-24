using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IShopMenus
    {
        ShopMenu GetMenu(string v);
        ShopMenu GetRandomDrugDealerMenu();
        ShopMenu GetVendingMenu(string v);
        ShopMenu GetRandomDrugCustomerMenu();
    }
}
