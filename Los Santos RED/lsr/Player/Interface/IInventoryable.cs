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
        RelationshipManager RelationshipManager { get; }
        CellPhone CellPhone { get; }
        Licenses Licenses { get; }
        Properties Properties { get; }
        HumanState HumanState { get; }
        VehicleOwnership VehicleOwnership { get; }
        Inventory Inventory { get; }
        BankAccounts BankAccounts { get; }
        ActivityManager ActivityManager { get; }
        Ped Character { get; }
        bool IsCop { get; set; }
        void DisplayPlayerNotification();
        void RemoveAgencyStatus();
        void SetAgencyStatus(Agency agency);
    }
}
