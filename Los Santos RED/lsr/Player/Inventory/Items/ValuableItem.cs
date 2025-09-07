using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ValuableItem : ModItem
{
    public ValuableItem()
    {
    }

    public ValuableItem(string name, string description, ItemType itemType) : base(name, description, itemType)
    {

    }
    public override void AddToList(PossibleItems possibleItems)
    {
        possibleItems?.ValuableItems.RemoveAll(x => x.Name == Name);
        possibleItems?.ValuableItems.Add(this);
    }
}