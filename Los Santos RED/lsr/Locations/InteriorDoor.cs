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
    public bool IsLocked { get; set; } = true;
    public Rotator Rotation { get; set; } = new Rotator(0f, 0f, 0f);

    public void LockDoor()
    {
        NativeFunction.Natives.x9B12F9A24FABEDB0(2811495845, Position.X, Position.Y, Position.Z, true, 1.0f);
    }
    public void UnLockDoor()
    {
        NativeFunction.Natives.x9B12F9A24FABEDB0(2811495845, Position.X, Position.Y, Position.Z, false, 1.0f);
    }
}

