using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class VehicleVariationShopMenu
{
    public VehicleVariationShopMenu()
    {
    }

    public VehicleVariationShopMenu(string iD, List<VehicleModKitShopMenuItem> vehicleModKitShopMenuItem)
    {
        ID = iD;
        VehicleModKitShopMenuItems = vehicleModKitShopMenuItem;
    }

    public string ID { get; set; }
    public List<VehicleModKitShopMenuItem> VehicleModKitShopMenuItems { get; set; } = new List<VehicleModKitShopMenuItem>();
    public List<VehicleModKitWheelTypeShopMenuItem> VehicleModKitWheelTypeShopMenuItems { get; set; } = new List<VehicleModKitWheelTypeShopMenuItem>();
    public List<VehicleColorShopMenuItem> VehicleColorShopMenuItems { get; set; } = new List<VehicleColorShopMenuItem>();
}

