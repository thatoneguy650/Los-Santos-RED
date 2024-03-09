using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[Serializable]
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
    public DispatchableVehicleExtra(int extraID, bool isOn, int percentage, int order)
    {
        ExtraID = extraID;
        IsOn = isOn;
        Percentage = percentage;
        Order = order;
    }
    public int ExtraID { get; set; }
    public bool IsOn { get; set; }
    public int Percentage { get; set; }
    public int Order { get; set; }
}

