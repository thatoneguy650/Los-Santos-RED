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
        GangRelationships GangRelationships { get; }
        CellPhone CellPhone { get; }
        GunDealerRelationship GunDealerRelationship { get; }
        Licenses Licenses { get; }
        Properties Properties { get; }
        OfficerFriendlyRelationship OfficerFriendlyRelationship { get; }
        HumanState HumanState { get; }
        VehicleOwnership VehicleOwnership { get; }
        Inventory Inventory { get; }
        BankAccounts BankAccounts { get; }
        bool IsPerformingActivity { get; }
        Ped Character { get; }
        void DisplayPlayerNotification();
    }
}
