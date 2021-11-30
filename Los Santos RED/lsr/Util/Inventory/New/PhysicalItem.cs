using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PhysicalItem
{
    public PhysicalItem()
    {

    }
    public PhysicalItem(string modelName)
    {
        ModelName = modelName;
    }
    public PhysicalItem(string modelName, int attachBoneIndex, Vector3 attachOffset, Rotator attachRotation)
    {
        AttachBoneIndex = attachBoneIndex;
        AttachOffset = attachOffset;
        AttachRotation = attachRotation;
        ModelName = modelName;
    }
    public int AttachBoneIndex { get; set; } = 57005;
    public Vector3 AttachOffset { get; set; } = Vector3.Zero;
    public Rotator AttachRotation { get; set; } = Rotator.Zero;
    public string ModelName { get; set; } = "";
    public bool IsLarge { get; set; } = false;
    public ePhysicalItemType Type { get; set; } = ePhysicalItemType.Prop;
}

