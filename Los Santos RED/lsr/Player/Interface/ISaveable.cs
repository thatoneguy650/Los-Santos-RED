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
    public interface ISaveable
    {
        CellPhone CellPhone { get; }
        GangRelationships GangRelationships { get; }
        GunDealerRelationship GunDealerRelationship { get; }
        OfficerFriendlyRelationship OfficerFriendlyRelationship { get; }
        Licenses Licenses { get; }
        Properties Properties { get; }
        HumanState HumanState { get; }
        BankAccounts BankAccounts { get; }
        VehicleOwnership VehicleOwnership { get; }
        Inventory Inventory { get; set; }

        string PlayerName { get; }
        string ModelName { get; }
        PedVariation CurrentModelVariation { get; }
        bool IsMale { get; }
        Vector3 Position { get; }
        Ped Character { get; }
        int SpeechSkill { get; }
    }
}
