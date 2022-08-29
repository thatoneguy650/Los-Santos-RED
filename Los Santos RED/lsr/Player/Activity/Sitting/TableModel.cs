using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TableModel
{
    public TableModel()
    {

    }
    public TableModel(uint hash)
    {
        Hash = hash;
    }
    public TableModel(uint hash, float entryOffsetFront)
    {
        Hash = hash;
        EntryOffsetFront = entryOffsetFront;
    }
    public TableModel(string modelName)
    {
        ModelName = modelName;
    }
    public TableModel(string modelName, float entryOffsetFront)
    {
        ModelName = modelName;
        EntryOffsetFront = entryOffsetFront;
    }
    public string Name { get; set; } = "Unknown";
    public string ModelName { get; set; }
    public uint Hash { get; set; }
    public float EntryOffsetFront { get; set; } = -0.5f;
}