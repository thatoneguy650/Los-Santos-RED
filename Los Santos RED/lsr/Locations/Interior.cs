using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable()]
public class Interior
{
    public Interior()
    {

    }
    public Interior(int iD, string name)
    {
        ID = iD;
        Name = name;
    }

    public int ID { get; set; }
    public string Name { get; set; }
    public bool IsMPOnly { get; set; } = false;
    public bool IsSPOnly { get; set; } = false;
}
