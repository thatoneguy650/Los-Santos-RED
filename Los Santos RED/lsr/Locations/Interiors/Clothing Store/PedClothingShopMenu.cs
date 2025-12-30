using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PedClothingShopMenu
{
    public string ID { get; set; }
    public List<PedClothingShopMenuItem> PedClothingShopMenuItems { get; set; }
    public PedClothingShopMenu()
    {
    }

    public PedClothingShopMenu(string iD, List<PedClothingShopMenuItem> pedClothingShopMenuItems)
    {
        ID = iD;
        PedClothingShopMenuItems = pedClothingShopMenuItems;
    }
}

