using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ILEDispatchableLocation : IAssaultSpawnable
{
    float DistanceToPlayer { get; }
    bool IsEnabled { get; }
    bool IsActivated { get; }
    Agency AssignedAgency { get; }
    int TotalAssaultSpawns { get; set; }

}

