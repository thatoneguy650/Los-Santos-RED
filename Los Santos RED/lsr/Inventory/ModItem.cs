using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable()]
public class ModItem
{
    public ModItem()
    {

    }
    public ModItem(string name)
    {
        Name = name;
    }
    public ModItem(string name, string description)
    {
        Name = name;
        Description = description;
    }
    public ModItem(string name, eConsumableType type)
    {
        Name = name;
        Type = type;
    }
    public ModItem(string name, string description, eConsumableType type)
    {
        Name = name;
        Description = description;
        Type = type;
    }
    public PhysicalItem ModelItem { get; set; }
    public PhysicalItem PackageItem { get; set; }
    public string Name { get; set; }
    public string Description { get; set; } = "";
    public int AmountPerPackage { get; set; } = 1;
    public bool CanConsume => Type == eConsumableType.Drink || Type == eConsumableType.Eat || Type == eConsumableType.Smoke || Type == eConsumableType.Ingest;
    public eConsumableType Type { get; set; } = eConsumableType.None;
    public string IntoxicantName { get; set; } = "";
    public bool IsIntoxicating => IntoxicantName != "";
    public bool RestoresHealth => HealthGained > 0;
    public int HealthGained { get; set; } = 0;
    public string FormattedItemType
    {
        get
        {
            if(Type == eConsumableType.Drink)
            {
                return "Drinkable";
            }
            else if (Type == eConsumableType.Eat)
            {
                return "Edible";
            }
            else if (Type == eConsumableType.Smoke)
            {
                return "Smokeable";
            }
            else if (Type == eConsumableType.Ingest)
            {
                return "Ingestable";
            }
            else if (Type == eConsumableType.Service)
            {
                return "Service";
            }
            else if (Type == eConsumableType.None)
            {
                return "Other";
            }
            else
            {
                return Type.ToString();
            }
        }
    }
}

