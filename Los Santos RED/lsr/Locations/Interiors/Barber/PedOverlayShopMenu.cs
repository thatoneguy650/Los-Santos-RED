using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PedOverlayShopMenu
{
    public PedOverlayShopMenu()
    {

    }

    public PedOverlayShopMenu(bool male, int overlayID, int index, int price)
    {
        Male = male;
        OverlayID = overlayID;
        Index = index;
        Price = price;
    }

    public bool Male { get; set; }
    public string DebugName { get; set; }
    public int OverlayID { get; set; }
    public int Index { get; set; }
    public int Price { get; set; }
}

