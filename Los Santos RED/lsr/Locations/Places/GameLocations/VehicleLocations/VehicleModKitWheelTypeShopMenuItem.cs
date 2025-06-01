using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class VehicleModKitWheelTypeShopMenuItem
{
    public int WheelTypeID { get; set; }
    public int ExtraAmount { get; set; }
    public VehicleModKitWheelTypeShopMenuItem()
    {
    }

    public VehicleModKitWheelTypeShopMenuItem(int wheelTypeID, int extraAmount)
    {
        WheelTypeID = wheelTypeID;
        ExtraAmount = extraAmount;
    }
}

