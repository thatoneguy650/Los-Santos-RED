using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IConsumeable
    {
        bool IsConsuming { get; set; }
        Ped Character { get; }
        string ModelName { get; }
        bool IsMale { get; }
    }
}
