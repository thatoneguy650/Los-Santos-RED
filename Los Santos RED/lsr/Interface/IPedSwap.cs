using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IPedSwap
    {
        void TakeoverPed(float selectedTakeoverRadius, bool v1, bool v2, bool v3);
    }
}
