using LSR.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IPlayerChaseable
{
    float DistanceToPlayer { get; }
    WeaponInventory WeaponInventory { get; }
    List<uint> BlackListedVehicles { get; }
    bool HasTaser { get; }
    uint Handle { get; }
    PlayerPerception PlayerPerception { get; }
}

