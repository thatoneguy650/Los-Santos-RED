using LosSantosRED.lsr.Player;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IArmorManageable
    {
        HumanState HumanState { get; }
        Ped Character { get; }
        bool CharacterModelIsFreeMode { get; }
        Inventory Inventory { get; }
    }
}