using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IInteriors
    {
        PossibleInteriors PossibleInteriors { get; }

        Interior GetInteriorByInternalID(int interiorID);
        Interior GetInteriorByLocalID(int interiorID);
    }
}
