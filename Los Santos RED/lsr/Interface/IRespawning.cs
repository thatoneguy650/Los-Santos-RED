using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IRespawning
    {
        bool RecentlyBribedPolice { get; }
        bool RecentlyRespawned { get; }
        bool RecentlySurrenderedToPolice { get; }

        void RespawnAtGrave();
        void RespawnAtHospital(GameLocation currentSelectedHospitalLocation);
        void RespawnAtCurrentLocation(bool v1, bool v2);
        void SurrenderToPolice(GameLocation currentSelectedSurrenderLocation);
        void BribePolice(int bribeAmount);
        void ResistArrest();
    }
}
