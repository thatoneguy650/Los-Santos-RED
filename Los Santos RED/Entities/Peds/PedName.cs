using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable()]
public class PedName
{
    public string Name;
    public NameType Type;
    public PedName()
    {

    }
    public PedName(string name, NameType type)
    {
        Name = name;
        Type = type;
    }
}