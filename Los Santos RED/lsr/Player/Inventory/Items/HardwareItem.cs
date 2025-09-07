using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[Serializable]
public class HardwareItem : ModItem
{
    public HardwareItem()
    {
    }
    public HardwareItem(string name, ItemType itemType) : base(name, itemType)
    {

    }
    public HardwareItem(string name, string description, ItemType itemType) : base(name, description, itemType)
    {

    }
    public override void AddToList(PossibleItems possibleItems)
    {
        possibleItems?.HardwareItems.RemoveAll(x => x.Name == Name);
        possibleItems?.HardwareItems.Add(this);
    }
}

