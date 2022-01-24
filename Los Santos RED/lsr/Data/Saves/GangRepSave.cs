using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GangRepSave
{
    public GangRepSave()
    {
    }

    public GangRepSave(string gangID, int reputation)
    {
        GangID = gangID;
        Reputation = reputation;
    }

    public string GangID { get; set; }
    public int Reputation { get; set; }
}

