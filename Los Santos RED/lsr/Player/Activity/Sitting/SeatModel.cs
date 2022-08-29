using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SeatModel
{
    public SeatModel()
    {

    }
    public SeatModel(uint hash)
    {
        ModelHash = hash;
    }
    public SeatModel(uint hash, float entryOffsetFront)
    {
        ModelHash = hash;
        EntryOffsetFront = entryOffsetFront;
    }
    public SeatModel(string modelName)
    {
        ModelName = modelName;
    }
    public SeatModel(string modelName, float entryOffsetFront)
    {
        ModelName = modelName;
        EntryOffsetFront = entryOffsetFront;
    }
    public string Name { get; set; } = "Unknown";
    public string ModelName { get; set; } = "";
    public uint ModelHash { get; set; } = 0;
    public float EntryOffsetFront { get; set; } = -0.5f;
}

