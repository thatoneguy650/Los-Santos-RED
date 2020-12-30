using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IPlayerIntoxicationState
    {
        bool IsDrunk { get; set; }
        bool IsImbibing { get; set; }
    }
}
