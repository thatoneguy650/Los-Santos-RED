using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class VehicleColorShopMenuItem
{
    public VehicleColorShopMenuItem()
    {
    }

    public VehicleColorShopMenuItem(int colorID, int price)
    {
        ColorID = colorID;
        Price = price;
    }

    public int ColorID { get; set; }
    public int Price { get; set; }
}

