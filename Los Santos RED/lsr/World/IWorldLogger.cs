using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IWorldLogger
    {
      //  void AddToList(PedExt toAdd);
        void AddToList(VehicleExt toAdd);
        void AddBlip(Blip myBlip);
        void AddCop(Cop myNewCop);
    }
}