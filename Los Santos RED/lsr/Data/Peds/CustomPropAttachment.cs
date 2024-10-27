using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class CustomPropAttachment
{
    public CustomPropAttachment()
    {
    }

    public CustomPropAttachment(string name, string boneName, Vector3 attachment, Rotator rotation)
    {
        PropName = name;
        BoneName = boneName;
        Attachment = attachment;
        Rotation = rotation;
    }
    public CustomPropAttachment(string name, Vector3 attachment)
    {
        PropName = name;
        Attachment = attachment;
    }

    public string PropName { get; set; }
    public string BoneName { get; set; }
    public Vector3 Attachment { get; set; }
    public Rotator Rotation { get; set; }
    public float SpawnChance { get; set; }
}

