using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable()]
public class ConsumableModItem : ModItem
{
    public ConsumableModItem()
    {

    }
    public ConsumableModItem(string name, eConsumableType type, eConsumableCategory category, string modelName)
    {
        Name = name;
        Type = type;
        ModelName = modelName;
        ConsumableCategory = category;
    }
    public ConsumableModItem(string name, eConsumableType type, eConsumableCategory category, string modelName, int attachBoneIndex, Vector3 attachOffset, Rotator attachRotation) : this(name, type, category, modelName)
    {
        AttachBoneIndex = attachBoneIndex;
        AttachOffset = attachOffset;
        AttachRotation = attachRotation;
    }
    public eConsumableType Type { get; set; } = eConsumableType.None;
    public eConsumableCategory ConsumableCategory { get; set; } = eConsumableCategory.None;

}

