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
        List<Zone> ZoneList { get; }

        Zone GetZone(Vector3 ZonePosition);
        Zone GetZone(string InternalGameName);
        List<Zone> GetZoneByItem(ModItem selectedItem, IShopMenus shopMenus, bool v);
        //string GetZoneName(Vector3 entrancePosition);
    }
}
