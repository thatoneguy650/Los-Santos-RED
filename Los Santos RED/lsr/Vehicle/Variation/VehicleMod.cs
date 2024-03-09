using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class VehicleMod
{
    public VehicleMod()
    {
    }

    public VehicleMod(int iD, int output)
    {
        ID = iD;
        Output = output;
    }

    public int ID { get; set; }
    public int Output { get; set; }
}

