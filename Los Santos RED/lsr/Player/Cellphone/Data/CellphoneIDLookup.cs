using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CellphoneIDLookup
{
    public CellphoneIDLookup()
    {
    }

    public CellphoneIDLookup(int iD, string name)
    {
        ID = iD;
        Name = name;
    }

    public int ID { get; set; }
    public string Name { get; set; }    
}

