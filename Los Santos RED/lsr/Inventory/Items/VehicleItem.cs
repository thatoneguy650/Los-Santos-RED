using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class VehicleItem : ModItem
{
    public VehicleItem()
    {
    }

    public VehicleItem(string name, string description, ItemType itemType) : base(name, description, itemType)
    {

    }
    public VehicleItem(string name, ItemType itemType) : base(name, itemType)
    {

    }
    public VehicleItem(string name, bool requiresDLC, ItemType itemType) : base(name, requiresDLC, itemType)
    {

    }
    public VehicleItem(string name, string description, bool requiresDLC, ItemType itemType) : base(name, description, requiresDLC, itemType)
    {

    }

}

