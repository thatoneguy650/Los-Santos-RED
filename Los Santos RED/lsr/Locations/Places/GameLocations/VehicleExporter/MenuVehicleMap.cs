using LSR.Vehicles;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MenuVehicleMap
{
    public MenuVehicleMap(MenuItem menuItem, UIMenuItem uIMenuItem, VehicleExt vehicleExt)
    {
        MenuItem = menuItem;
        UIMenuItem = uIMenuItem;
        VehicleExt = vehicleExt;
    }

    public MenuItem MenuItem { get; set; }
    public UIMenuItem UIMenuItem { get; set; }
    public VehicleExt VehicleExt { get; set; }

}

