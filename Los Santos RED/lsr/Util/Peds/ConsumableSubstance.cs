using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable()]
public class ConsumableSubstance
{
    public ConsumableSubstance()
    {

    }
    public ConsumableSubstance(string name, eConsumableType type, eConsumableCategory category, string modelName)
    {
        Name = name;
        Type = type;
        ModelName = modelName;
        ConsumableCategory = category;
    }

    public ConsumableSubstance(string name, eConsumableType type, eConsumableCategory category, string modelName, int attachBoneIndex, Vector3 attachOffset, Rotator attachRotation) : this(name, type, category, modelName)
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
    public eConsumableType Type { get; set; } = eConsumableType.None;
    public eConsumableCategory ConsumableCategory { get; set; } = eConsumableCategory.None;
    public bool CanStore { get; set; } = true;
    public bool IsIntoxicating { get; set; } = false;
    public int AmountPerPackage { get; set; } = 1;
    public int Price { get; set; } = 5;
    public bool HasPackage => PackageModel != "";
    public string PackageModel { get; set; } = "";

}

