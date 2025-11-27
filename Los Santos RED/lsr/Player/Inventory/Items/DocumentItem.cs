using LSR.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable()]
public class DocumentItem : ValuableItem
{
    public string VehicleModel { get; set; }
    public LicensePlate LicensePlate { get; set; }
    public DocumentItem()
    {
        ItemSubType = ItemSubType.Identification;
    }

    public DocumentItem(string name, string description, ItemType itemType) : base(name, description, itemType)
    {
        ItemSubType = ItemSubType.Identification;
    }
    public override void AddToList(PossibleItems possibleItems)
    {
        possibleItems?.ValuableItems.RemoveAll(x => x.Name == Name);
        possibleItems?.ValuableItems.Add(this);
    }
}