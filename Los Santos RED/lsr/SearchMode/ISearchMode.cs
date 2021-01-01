using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface ISearchMode
    {
        string SearchModeDebug { get; }
        bool StarsRecentlyGreyedOut { get; }
        bool StarsRecentlyActive { get; }
        uint TimeInSearchMode { get; }
    }
}
