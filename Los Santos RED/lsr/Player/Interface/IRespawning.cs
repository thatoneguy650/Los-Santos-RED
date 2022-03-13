using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IRespawning//needs better name and to be moved
    {
        bool RecentlyRespawned { get; }
       // void RespawnAtGrave();
        void RespawnAtHospital(Hospital currentSelectedHospitalLocation);
        void RespawnAtCurrentLocation(bool withInvicibility, bool resetWanted, bool clearCriminalHistory, bool clearInventory);
        void SurrenderToPolice(PoliceStation currentSelectedSurrenderLocation);
        bool BribePolice(int bribeAmount);
        void ResistArrest();
        bool PayFine();
    }
}
