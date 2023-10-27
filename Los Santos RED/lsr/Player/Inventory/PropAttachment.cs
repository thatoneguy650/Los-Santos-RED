using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PropAttachment
{
    public PropAttachment()
    {
    }

    public PropAttachment(string name, string boneName, Vector3 attachment, Rotator rotation)
    {
        Name = name;
        BoneName = boneName;
        Attachment = attachment;
        Rotation = rotation;
    }
    public PropAttachment(string name, Vector3 attachment)
    {
        Name = name;
        Attachment = attachment;
    }

    public string Name { get; set; }
    public string BoneName { get; set; }
    public Vector3 Attachment { get; set; }
    public Rotator Rotation { get; set; }
    public string Gender { get; set; } = "U";
    public bool IsMP { get; set; } = false;
}

