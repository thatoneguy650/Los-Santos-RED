using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable()]
public class GameCounty
{
    public string CountyID { get; set; }
    public string CountyName { get; set; }
    public GameCounty()
    {
    }

    public GameCounty(string countyID, string countyName)
    {
        CountyID = countyID;
        CountyName = countyName;
    }
}

