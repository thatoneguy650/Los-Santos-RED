using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class WeaponItem : ModItem
{
    public WeaponItem()
    {
    }
    public WeaponItem(string name, string description, bool requiresDLC, ItemType itemType) : base(name, description, requiresDLC, itemType)
    {

    }
}

