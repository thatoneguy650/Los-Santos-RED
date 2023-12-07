using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IOutfitManageable
    {
        string ModelName { get; }
        PedVariation CurrentModelVariation { get; set; }
        Ped Character { get; }
        string PlayerName { get; }
    }
}
