using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class VehicleExtra
{
    public VehicleExtra()
    {
    }

    public VehicleExtra(int iD, bool isTurnedOn)
    {
        ID = iD;
        IsTurnedOn = isTurnedOn;
    }

    public int ID { get; set; }
    public bool IsTurnedOn { get; set; }
}

