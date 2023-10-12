using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

[Serializable()]
public class GameCounty
{
    public string CountyID { get; set; }
    public string CountyName { get; set; }
    public string ColorPrefix { get; set; }
    public string ColorName => string.IsNullOrEmpty(ColorPrefix) ? CountyName : ColorPrefix + CountyName;
    public GameCounty()
    {
    }

    public GameCounty(string countyID, string countyName)
    {
        CountyID = countyID;
        CountyName = countyName;
    }
}

