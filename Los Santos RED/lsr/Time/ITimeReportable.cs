using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface ITimeReportable
    {
        bool IsNight { get; }
        bool IsFastForwarding { get; }
        int CurrentHour { get; }
        int CurrentMinute { get; }
        string CurrentTime { get; }
        int CurrentDay { get; }
        DateTime CurrentDateTime { get; }
        bool ForceShowClock { get; set; }
    }
}
