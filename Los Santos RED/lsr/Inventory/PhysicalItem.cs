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
    public PhysicalItem(string modelName, ePhysicalItemType type)
    {
        ModelName = modelName;
        Type = type;
    }
    public PhysicalItem(string modelName, List<PropAttachment> attachments)
    {
        ModelName = modelName;
        Attachments = attachments;
    }
    //public PhysicalItem(string modelName, Vector3 attachOffset, Rotator attachRotation)
    //{
    //    AttachOffsetOverride = attachOffset;
    //    AttachRotationOverride = attachRotation;
    //    ModelName = modelName;
    //}
    //public PhysicalItem(string modelName, Vector3 attachOffsetOverride, Rotator attachRotationOverride, Vector3 secondaryAttachOffsetOverride, Rotator secondaryAttachRotationOverride)
    //{
    //    AttachOffsetOverride = attachOffsetOverride;
    //    AttachRotationOverride = attachRotationOverride;
    //    SecondaryAttachOffsetOverride = secondaryAttachOffsetOverride;
    //    SecondaryAttachRotationOverride = secondaryAttachRotationOverride;
    //    ModelName = modelName;
    //}
    public PhysicalItem(string modelName, uint modelHash, ePhysicalItemType type)
    {
        ModelName = modelName;
        ModelHash = modelHash;
        Type = type;
    }
    public string ID => ModelName.ToLower();
    public string ModelName { get; set; } = "";
    public uint ModelHash { get; set; } = 0;
    public bool IsLarge { get; set; } = false;
    public ePhysicalItemType Type { get; set; } = ePhysicalItemType.Prop;



    //public Vector3 AttachOffsetOverride { get; set; } = Vector3.Zero;
    //public Rotator AttachRotationOverride { get; set; } = Rotator.Zero;



    //public Vector3 SecondaryAttachOffsetOverride { get; set; } = Vector3.Zero;
    //public Rotator SecondaryAttachRotationOverride { get; set; } = Rotator.Zero;


    //public Vector3 SecondaryAttachOffsetFemaleOverride { get; set; } = Vector3.Zero;
    //public Rotator SecondaryAttachRotationFemaleOverride { get; set; } = Rotator.Zero;


    public List<PropAttachment> Attachments { get; set; }


}

