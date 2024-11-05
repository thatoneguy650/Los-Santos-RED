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
    public string CountyNameShort { get; set; }
    //public string ColorName => 


    public string ColorName(bool useShort)
    { 
        if(useShort && !string.IsNullOrEmpty(CountyNameShort))
        {
            return string.IsNullOrEmpty(ColorPrefix) ? CountyNameShort : ColorPrefix + CountyNameShort;
        }
        return string.IsNullOrEmpty(ColorPrefix) ? CountyName : ColorPrefix + CountyName;
    }
    public GameCounty()
    {
    }

    public GameCounty(string countyID, string countyName)
    {
        CountyID = countyID;
        CountyName = countyName;
    }

    public GameCounty(string countyID, string countyName, string countyNameShort) : this(countyID, countyName)
    {
        CountyNameShort = countyNameShort;
    }
}

