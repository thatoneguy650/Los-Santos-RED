using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

[Serializable]
public class InteriorDoor
{
    private Entity doorEntity;
    private bool isLocked = true;
    private float originalHeading = 0f;
    private bool HasOriginalHeading = false;
    //private bool WasForceRotatedOpen;

    public InteriorDoor()
    {

    }
    public InteriorDoor(long modelHash, Vector3 position)
    {
        ModelHash = modelHash;
        Position = position;
    }
    public long ModelHash { get; set; }
    public Vector3 Position { get; set; } = Vector3.Zero;
    public bool IsLocked => isLocked;
    public Rotator Rotation { get; set; } = new Rotator(0f, 0f, 0f);
    public bool ForceRotateOpen { get; set; } = false;
    public bool NeedsDefaultUnlock { get; set; } = false;
    public bool LockWhenClosed { get; set; } = false;

    [XmlIgnore]
    public bool HasBeenForceRotatedOpen { get; set; }
    public void LockDoor()
    {
        NativeFunction.Natives.x9B12F9A24FABEDB0(ModelHash, Position.X, Position.Y, Position.Z, true, 1.0f);
        if(ForceRotateOpen)
        {
            ForceRotateCloseDoor();
        }
        isLocked = true;
    }
    public void UnLockDoor()
    {
        NativeFunction.Natives.x9B12F9A24FABEDB0(ModelHash, Position.X, Position.Y, Position.Z, false, 1.0f);
        if(ForceRotateOpen)
        {
            ForceRotateOpenDoor();
        }
        isLocked = false;
    }
    public void Activate()
    {
        if(NeedsDefaultUnlock)
        {
            UnLockDoor();
        }
    }
    public void Deactivate()
    {
        if (ForceRotateOpen)
        {
            ForceRotateCloseDoor();
        }
    }
    public void AddDistanceOffset(Vector3 offsetToAdd)
    {
        Position += offsetToAdd;
    }
    private void ForceRotateOpenDoor()
    {
        doorEntity = NativeFunction.Natives.GET_CLOSEST_OBJECT_OF_TYPE<Rage.Object>(Position.X, Position.Y, Position.Z, 3.0f, ModelHash, true, false, true);
        if (!doorEntity.Exists())
        {
            //EntryPoint.WriteToConsole($"ForceRotateOpenDoor DOES NOT EXIST OPEN");
            return;
        }
       // doorEntity.Delete();
        if (!HasOriginalHeading)
        {
            originalHeading = doorEntity.Rotation.Yaw;
            HasOriginalHeading = true;
            //EntryPoint.WriteToConsole($"originalHeading{originalHeading}");
        }
        NativeFunction.Natives.FREEZE_ENTITY_POSITION(doorEntity, false);
        doorEntity.Rotation = new Rotator(0f, 0f, originalHeading - 100f);
        NativeFunction.Natives.FREEZE_ENTITY_POSITION(doorEntity, true);
        //doorEntity.IsPersistent = false;

        HasBeenForceRotatedOpen = true;

        //EntryPoint.WriteToConsole($"ForceRotateOpenDoor {originalHeading - 100f}");
    }
    private void ForceRotateCloseDoor()
    {
        //Rage.Object doorEntity = NativeFunction.Natives.GET_CLOSEST_OBJECT_OF_TYPE<Rage.Object>(Position.X, Position.Y, Position.Z, 3.0f, ModelHash, true, false, true);


        HasBeenForceRotatedOpen = false;

        if (!doorEntity.Exists())
        {
            //EntryPoint.WriteToConsole($"ForceRotateOpenDoor DOES NOT EXIST CLOSE");
            return;
        }
        if (!HasOriginalHeading)
        {
            originalHeading = doorEntity.Rotation.Yaw;
            HasOriginalHeading = true;
        }
        NativeFunction.Natives.FREEZE_ENTITY_POSITION(doorEntity, false);
        doorEntity.Rotation = new Rotator(0f, 0f, originalHeading);
        NativeFunction.Natives.FREEZE_ENTITY_POSITION(doorEntity, true);
        doorEntity.IsPersistent = false;

        //EntryPoint.WriteToConsole($"ForceRotateCloseDoor {originalHeading}");
    }
}

