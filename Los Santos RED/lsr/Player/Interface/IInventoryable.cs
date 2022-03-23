using LosSantosRED.lsr.Player;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IInventoryable
    {
        Inventory Inventory { get; }
        bool IsPerformingActivity { get; }
        int Money { get; }
        Ped Character { get; }
        void GiveMoney(int salesPrice);
        void TakeOwnershipOfVehicle(VehicleExt myNewCar, bool showNotification);
        void DisplayPlayerNotification();
        GangRelationships GangRelationships { get; }
        CellPhone CellPhone { get; }
        GunDealerRelationship GunDealerRelationship { get; }
        Licenses Licenses { get; }
        Properties Properties { get; }
        OfficerFriendlyRelationship OfficerFriendlyRelationship { get; }

        void ClearVehicleOwnership();
    }
}
