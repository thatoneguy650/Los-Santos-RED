using Rage;
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
    public InteriorDoor(int modelHash, Vector3 position)
    {
        ModelHash = modelHash;
        Position = position;
    }
    public int ModelHash { get; set; }
    public Vector3 Position { get; set; } = Vector3.Zero;
    public bool IsLocked { get; set; } = true;
    public Rotator Rotation { get; set; } = new Rotator(0f, 0f, 0f);
}

