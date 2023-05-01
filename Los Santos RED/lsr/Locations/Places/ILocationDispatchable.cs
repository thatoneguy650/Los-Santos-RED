using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface ILocationDispatchable
{
    List<ConditionalLocation> PossiblePedSpawns { get; }
    List<ConditionalLocation> PossibleVehicleSpawns { get; }
    Agency AssignedAgency { get; set; }
    Vector3 EntrancePosition { get; }
    bool IsDispatchFilled { get; set; }
    bool IsEnabled { get; }
    float DistanceToPlayer { get; }
    bool IsNearby { get; }
    string AssignedAssociationID { get; }
    bool IsActivated { get; }
}

