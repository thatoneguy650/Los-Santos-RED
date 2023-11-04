using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IGroupManageable
    {
        Ped Character { get; }
        PedExt CurrentLookedAtPed { get; }
        GangMember CurrentLookedAtGangMember { get; }
        RelationshipManager RelationshipManager { get; }
        WeaponEquipment WeaponEquipment { get; }
        int WantedLevel { get; }
        bool IsWanted { get; }
    }
}
