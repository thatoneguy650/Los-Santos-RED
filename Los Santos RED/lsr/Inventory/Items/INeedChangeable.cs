using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface INeedChangeable
{
    bool ConsumeOnPurchase { get; set; }
    int HealthChangeAmount { get; set; }
    float HungerChangeAmount { get; set; }
    float ThirstChangeAmount { get; set; }
    float SleepChangeAmount { get; set; }
}

