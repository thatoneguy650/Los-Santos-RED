using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IHealthManageable
    {
        HumanState HumanState { get; }
        Ped Character { get; }
    }
}
