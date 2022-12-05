using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PedFashionAlias
{
    public int ID { get; set; }
    public string Name { get; set; }
    
    public PedFashionAlias()
    {

    }

    public PedFashionAlias(int iD, string name)
    {
        ID = iD;
        Name = name;
    }
    public override string ToString()
    {
        return Name;
    }
}

