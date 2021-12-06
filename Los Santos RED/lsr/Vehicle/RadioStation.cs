using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class RadioStation
{
    public RadioStation(string internalName, string name, int index)
    {
        InternalName = internalName;
        Name = name;
        Index = index;
    }
    public string InternalName { get; set; }
    public string Name { get; set; }
    public int Index { get; set; }
    public override string ToString()
    {
        return Name.ToString();
    }
}

