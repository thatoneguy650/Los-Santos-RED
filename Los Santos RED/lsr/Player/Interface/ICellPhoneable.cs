using LosSantosRED.lsr.Locations;
using LosSantosRED.lsr.Player;
using LSR.Vehicles;
using Rage;
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

        GangRelationships GangRelationships { get; }

        void GiveMoney(int v);
        void AddGPSRoute(string name, Vector3 entrancePosition);
        //bool IsHostile(Gang myGang);
        //bool IsFriendly(Gang myGang);
        //int GetRepuationLevel(Gang gangLastCalled);
        //void GiveMoney(int v);
        //void SetReputation(Gang gangLastCalled, int v, bool sendNotification);
    }
}
