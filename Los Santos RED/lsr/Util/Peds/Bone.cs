using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Bone
{
    public string Name { get; set; }
    public int Type { get; set; }
    public int Tag { get; set; }
    public BodyLocation Location { get; set; }
    public Bone(string name, int type, int tag)
    {
        Name = name;
        Type = type;
        Tag = tag;
    }
    public Bone(string name, int type, int tag, BodyLocation location)
    {
        Name = name;
        Type = type;
        Tag = tag;
        Location = location;
    }
}