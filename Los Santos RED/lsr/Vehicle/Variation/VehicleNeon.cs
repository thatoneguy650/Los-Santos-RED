using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[Serializable]
public class VehicleNeon
{
    public VehicleNeon()
    {
    }

    public VehicleNeon(int iD, bool isEnabled)
    {
        ID = iD;
        IsEnabled = isEnabled;
    }

    public int ID { get; set; }
    public bool IsEnabled { get; set; } 
}

