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
    public ModItem(string name, eConsumableType type)
    {
        Name = name;
        Type = type;
    }
    public PhysicalItem ModelItem { get; set; }
    public PhysicalItem PackageItem { get; set; }
    public string Name { get; set; }
    public int AmountPerPackage { get; set; } = 1;
    public bool CanConsume => Type == eConsumableType.Drink || Type == eConsumableType.Eat || Type == eConsumableType.Smoke || Type == eConsumableType.Ingest;
    public eConsumableType Type { get; set; } = eConsumableType.None;
    public string IntoxicantName { get; set; } = "";
}

