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
    public ModItem(string name, eConsumableType type, string modelName, int attachBoneIndex, Vector3 attachOffset, Rotator attachRotation) : this(name, type)
    {
        PhysicalItem = new PhysicalItem(modelName, "", attachBoneIndex, attachOffset, attachRotation);
    }
    public ModItem(string name, eConsumableType type, string modelName, string packageModelName, int attachBoneIndex, Vector3 attachOffset, Rotator attachRotation) : this(name, type)
    {
        PhysicalItem = new PhysicalItem(modelName, packageModelName, attachBoneIndex, attachOffset, attachRotation);
    }
    public ModItem(string name, eConsumableType type, string modelName,bool isLarge, string packageModelName, bool packageIsLarge, int attachBoneIndex, Vector3 attachOffset, Rotator attachRotation) : this(name, type)
    {
        PhysicalItem = new PhysicalItem(modelName, packageModelName, attachBoneIndex, attachOffset, attachRotation);
        PhysicalItem.PackageIsLarge = packageIsLarge;
        PhysicalItem.ItemIsLarge = isLarge;
    }
    public ModItem(string name, string modelName, string packageModelName, int attachBoneIndex, Vector3 attachOffset, Rotator attachRotation) : this(name, eConsumableType.None)
    {
        PhysicalItem = new PhysicalItem(modelName, packageModelName, attachBoneIndex, attachOffset, attachRotation);
    }
    public ModItem(string name, string modelName, bool isLarge, int attachBoneIndex, Vector3 attachOffset, Rotator attachRotation) : this(name, eConsumableType.None)
    {
        PhysicalItem = new PhysicalItem(modelName, "", attachBoneIndex, attachOffset, attachRotation);
        PhysicalItem.ItemIsLarge = isLarge;
    }
    public ModItem(string name, string modelName, int attachBoneIndex, Vector3 attachOffset, Rotator attachRotation) : this(name, eConsumableType.None)
    {
        PhysicalItem = new PhysicalItem(modelName, "", attachBoneIndex, attachOffset, attachRotation);
    }
    public PhysicalItem PhysicalItem { get; set; }
    public string Name { get; set; }
    public bool CanStore { get; set; } = true;
    public bool IsIntoxicating { get; set; } = false;
    public int AmountPerPackage { get; set; } = 1;
    public eConsumableType Type { get; set; } = eConsumableType.None;
    public bool CanConsume => Type == eConsumableType.Drink || Type == eConsumableType.Eat || Type == eConsumableType.Smoke || Type == eConsumableType.Ingest;
}

