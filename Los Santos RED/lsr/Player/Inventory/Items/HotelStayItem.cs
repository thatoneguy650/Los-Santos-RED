using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class HotelStayItem : ModItem
{
    public HotelStayItem()
    {
    }

    public HotelStayItem(string name, string description, ItemType itemType) : base(name, description, itemType)
    {

    }
    public override void AddToList(PossibleItems possibleItems)
    {
        possibleItems?.HotelStayItems.RemoveAll(x => x.Name == Name);
        possibleItems?.HotelStayItems.Add(this);
    }
}

