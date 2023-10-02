using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class InteriorDoor
{
    private bool isLocked = true;

    //private bool lockState;
    //private float openRatio;

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

    public void AddDistanceOffset(Vector3 offsetToAdd)
    {
        Position += offsetToAdd;
    }

    private void ForceRotateOpenDoor()
    {
        Rage.Object doorEntity = NativeFunction.Natives.GET_CLOSEST_OBJECT_OF_TYPE<Rage.Object>(Position.X, Position.Y, Position.Z, 3.0f, ModelHash, true, false, true);
        if (!doorEntity.Exists())
        {
            return;
        }
        NativeFunction.Natives.FREEZE_ENTITY_POSITION(doorEntity, false);
        doorEntity.Rotation = new Rotator(0f, 0f, doorEntity.Rotation.Yaw - 100f);
        NativeFunction.Natives.FREEZE_ENTITY_POSITION(doorEntity, true);
    }
    private void ForceRotateCloseDoor()
    {
        Rage.Object doorEntity = NativeFunction.Natives.GET_CLOSEST_OBJECT_OF_TYPE<Rage.Object>(Position.X, Position.Y, Position.Z, 3.0f, ModelHash, true, false, true);
        if (!doorEntity.Exists())
        {
            return;
        }
        NativeFunction.Natives.FREEZE_ENTITY_POSITION(doorEntity, false);
        doorEntity.Rotation = new Rotator(0f, 0f, doorEntity.Rotation.Yaw + 100f);
        NativeFunction.Natives.FREEZE_ENTITY_POSITION(doorEntity, true);
    }
    //public void GetState()
    //{
    //    bool _lockState;
    //    float _openRatio;
    //    unsafe
    //    {
    //        NativeFunction.CallByName<bool>("GET_STATE_OF_CLOSEST_DOOR_OF_TYPE", ModelHash, Position.X, Position.Y, Position.Z, &_lockState, &_openRatio);
    //    }
    //    lockState = _lockState;
    //    openRatio = _openRatio;
    //}


    //public void LockGate()
    //{
    //    NativeFunction.Natives.x9B12F9A24FABEDB0(ModelHash, Position.X, Position.Y, Position.Z, true, 1.0f);
    //    isLocked = true;
    //}
    //public void UnLockGate()
    //{
    //    NativeFunction.Natives.x9B12F9A24FABEDB0(ModelHash, Position.X, Position.Y, Position.Z, false, 1.0f);
    //    isLocked = false;
    //}
}

