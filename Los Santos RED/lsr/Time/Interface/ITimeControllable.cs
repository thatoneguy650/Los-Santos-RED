using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface ITimeControllable//probably get right off, has smell
    {
        bool IsNight { get; }
        void UnPauseTime();
        void PauseTime();
    }
}
