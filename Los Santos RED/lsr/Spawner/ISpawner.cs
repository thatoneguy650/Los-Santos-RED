using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface ISpawner
    {
        void Delete(Cop toDelete);
        void Spawn(PoliceSpawn policeSpawn);
    }
}
