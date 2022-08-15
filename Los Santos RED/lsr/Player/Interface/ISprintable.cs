using LosSantosRED.lsr.Locations;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface ISprintable
    {
        Ped Character { get; }
        HumanState HumanState { get; }
        Injuries Injuries { get; }
    }
}
