using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ScenarioBlock
{
    public Vector3 Position { get; set; }
    public float Distance { get; set; } = 2.0f;
    public string DebugName { get; set; }
    public ScenarioBlock()
    {

    }

    public ScenarioBlock(Vector3 position, string debugName)
    {
        Position = position;
        DebugName = debugName;
    }

    public virtual void Block()
    {
        NativeFunction.Natives.ADD_SCENARIO_BLOCKING_AREA<int>(Position.X - Distance, Position.Y - Distance, Position.Z - Distance, Position.X + Distance, Position.Y + Distance, Position.Z + Distance, false, true, true, true);
    }
}

