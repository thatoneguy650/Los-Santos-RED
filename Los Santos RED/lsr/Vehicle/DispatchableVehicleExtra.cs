using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DispatchableVehicleExtra
{
    public DispatchableVehicleExtra()
    {
    }

    public DispatchableVehicleExtra(int extraID)
    {
        ExtraID = extraID;
        IsOn = true;
        Percentage = 100;
    }

    public DispatchableVehicleExtra(int extraID, bool isOn, int percentage)
    {
        ExtraID = extraID;
        IsOn = isOn;
        Percentage = percentage;
    }

    public int ExtraID { get; set; }
    public bool IsOn { get; set; }
    public int Percentage { get; set; }
}

