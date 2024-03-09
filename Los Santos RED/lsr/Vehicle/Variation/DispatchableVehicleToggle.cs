using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[Serializable]
public class DispatchableVehicleToggle
{
    public DispatchableVehicleToggle()
    {
    }

    public DispatchableVehicleToggle(int id)
    {
        ID = id;
        IsTurnedOn = true;
        Percentage = 100;
    }

    public DispatchableVehicleToggle(int id, bool isTurnedOn, int percentage)
    {
        ID = id;
        IsTurnedOn = isTurnedOn;
        Percentage = percentage;
    }


    public int ID { get; set; }
    public bool IsTurnedOn { get; set; }
    public int Percentage { get; set; }
}

