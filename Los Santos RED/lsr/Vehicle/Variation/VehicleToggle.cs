using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class VehicleToggle
{
    public VehicleToggle()
    {
    }

    public VehicleToggle(int iD, bool isTurnedOn)
    {
        ID = iD;
        IsTurnedOn = isTurnedOn;
    }

    public int ID { get; set; }
    public bool IsTurnedOn { get; set; }
}

