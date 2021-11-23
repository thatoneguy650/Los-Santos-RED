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
    public ModItem(string name, string modelName)
    {
        Name = name;
        ModelName = modelName;
    }
    public ModItem(string name, string modelName, int attachBoneIndex, Vector3 attachOffset, Rotator attachRotation) : this(name, modelName)
    {
        AttachBoneIndex = attachBoneIndex;
        AttachOffset = attachOffset;
        AttachRotation = attachRotation;
    }
    public int AttachBoneIndex { get; set; } = 57005;
    public Vector3 AttachOffset { get; set; } = new Vector3(0.12f, 0.0f, -0.06f);
    public Rotator AttachRotation { get; set; } = new Rotator(-77.0f, 23.0f, 0.0f);
    public string ModelName { get; set; }
    public string Name { get; set; }
    public bool CanStore { get; set; } = true;
    public bool IsIntoxicating { get; set; } = false;
    public int AmountPerPackage { get; set; } = 1;
    public int Price { get; set; } = 5;
    public bool HasPackage => PackageModel != "";
    public string PackageModel { get; set; } = "";
    public bool IsConsumable { get; set; } = false;
}

