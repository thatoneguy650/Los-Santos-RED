using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CarGeneratorBlock
{
    public Vector3 Position { get; set; }
    public float Distance { get; set; } = 2.0f;
    public string DebugName { get; set; }
    public CarGeneratorBlock()
    {

    }

    public CarGeneratorBlock(Vector3 position, string debugName)
    {
        Position = position;
        DebugName = debugName;
    }

    public void Block()
    {
        NativeFunction.Natives.SET_ALL_VEHICLE_GENERATORS_ACTIVE_IN_AREA(Position.X - Distance, Position.Y - Distance, Position.Z - Distance, Position.X + Distance, Position.Y + Distance, Position.Z + Distance, false, false);
        NativeFunction.Natives.REMOVE_VEHICLES_FROM_GENERATORS_IN_AREA(Position.X - Distance, Position.Y - Distance, Position.Z - Distance, Position.X + Distance, Position.Y + Distance, Position.Z + Distance, false);
    }
}

