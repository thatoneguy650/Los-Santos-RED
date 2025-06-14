using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class VehicleModKitShopMenuItem
{
    public VehicleModKitShopMenuItem()
    {
    }

    public VehicleModKitShopMenuItem(int modTypeID, int basePrice, int priceScale)
    {
        ModTypeID = modTypeID;
        BasePrice = basePrice;
        PriceScale = priceScale;
    }

    public int ModTypeID { get; set; }
    public int BasePrice { get; set; } = 500;
    public int PriceScale { get; set; } = 100;   
}

