using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface ITimeControllable : ITimeReportable
    {
        bool IsNight { get; }
        void UnPauseTime();
        void PauseTime();
        void FastForward(int v);
    }
}
