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
    public PhysicalItem(string modelName, string packageModelName, int attachBoneIndex, Vector3 attachOffset, Rotator attachRotation)
    {
        AttachBoneIndex = attachBoneIndex;
        AttachOffset = attachOffset;
        AttachRotation = attachRotation;
        ModelName = modelName;
        PackageModelName = packageModelName;
    }

    public int AttachBoneIndex { get; set; } = 57005;
    public Vector3 AttachOffset { get; set; } = new Vector3(0.12f, 0.0f, -0.06f);
    public Rotator AttachRotation { get; set; } = new Rotator(-77.0f, 23.0f, 0.0f);
    public string ModelName { get; set; } = "";
    public bool HasPackage => PackageModelName != "";
    public string PackageModelName { get; set; } = "";
}

