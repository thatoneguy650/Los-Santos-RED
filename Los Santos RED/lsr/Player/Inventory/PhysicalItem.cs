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
    public List<PropAttachment> Attachments { get; set; }
    public bool CleanupItemImmediately { get; set; } = false;
    public uint AliasWeaponHash { get; set; }
    public override string ToString()
    {
        return ID;
    }
}

