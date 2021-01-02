using LosSantosRED.lsr.Data;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IZones
    {
        Zone GetZone(Vector3 ZonePosition);
        Zone GetZone(string InternalGameName);
    }
}
