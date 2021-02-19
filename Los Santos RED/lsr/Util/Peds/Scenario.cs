using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Scenario
{
    public Scenario(string gameName, string name)
    {
        InternalName = gameName;
        Name = name;
    }
    public string InternalName { get; set; }
    public string Name { get; set; }
    public override string ToString()
    {
        return Name.ToString();
    }
}

