using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Carrier
{
    public string CarrierID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Carrier()
    {
    }

    public Carrier(string carrierID, string name, string description)
    {
        CarrierID = carrierID;
        Name = name;
        Description = description;
    }
}

