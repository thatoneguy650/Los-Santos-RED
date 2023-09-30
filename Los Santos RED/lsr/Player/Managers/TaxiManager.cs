using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TaxiManager
{
    public TaxiFirm RequestedFirm { get; set; }
    public Vector3 PickupLocation { get; set; }
    public bool RequestService(TaxiFirm taxiFirm)
    {
        if(taxiFirm == null)
        {
            return false;
        }
        return true;
    }
}

