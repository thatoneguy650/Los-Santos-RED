using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DispatchableVehicleModValue
{
    public DispatchableVehicleModValue()
    {
    }

    public DispatchableVehicleModValue(int value, int percentage)
    {
        Value = value;
        Percentage = percentage;
    }

    public int Value { get; set; }
    public int Percentage { get; set; }
}
