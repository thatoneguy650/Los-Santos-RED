using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IPoliceTasking_Old
    {
        Vector3 PlaceLastSeenPlayer { get; }//tasking scanner
        bool AnyRecentlySeenPlayer { get; }//dispatcher scanner, search mode, tasking
        float ActiveDistance { get; }//tasking
    }
}
