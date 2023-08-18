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

    public bool NeedsDefaultUnlock { get; set; } = false;
    //public bool LockState => LockState; 
    //public float OpenRatio => openRatio;
    public void LockDoor()
    {
        NativeFunction.Natives.x9B12F9A24FABEDB0(ModelHash, Position.X, Position.Y, Position.Z, true, 1.0f);
        isLocked = true;
    }
    public void UnLockDoor()
    {
        NativeFunction.Natives.x9B12F9A24FABEDB0(ModelHash, Position.X, Position.Y, Position.Z, false, 1.0f);
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

