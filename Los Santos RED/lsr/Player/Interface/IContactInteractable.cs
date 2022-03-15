using LosSantosRED.lsr.Locations;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IContactInteractable
    {


        GangRelationships GangRelationships { get; }
        PlayerTasks PlayerTasks { get; }
        CellPhone CellPhone { get; }
        bool IsWanted { get; }
        bool IsNotWanted { get; }
        int WantedLevel { get; }
        int Money { get; }
        PoliceResponse PoliceResponse { get; }
        LocationData CurrentLocation { get; }
        Investigation Investigation { get; }

        void AddGPSRoute(string name, Vector3 entrancePosition);
        void GiveMoney(int p);
        void PayoffPolice();
        void SetWantedLevel(int v1, string v2, bool v3);
        void CallPolice();
        void CallEMS();
        void CallFire();
    }
}
