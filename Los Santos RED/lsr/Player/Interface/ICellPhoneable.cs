using LosSantosRED.lsr.Locations;
using LosSantosRED.lsr.Player;
using LSR.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface ICellPhoneable
    {
        LocationData CurrentLocation { get; }
        int Money { get; }

        void CallPolice();
        bool IsHostile(Gang myGang);
        bool IsFriendly(Gang myGang);
        int GetRepuationLevel(Gang gangLastCalled);
        void GiveMoney(int v);
        void SetReputation(Gang gangLastCalled, int v);
    }
}
