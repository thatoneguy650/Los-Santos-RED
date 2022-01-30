using iFruitAddon2;
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
    public interface IGangRelateable
    {
        LocationData CurrentLocation { get; }
        bool IsWanted { get; }
        int WantedLevel { get; }
        List<GangReputation> GangReputations { get; }
        int Money { get; }
        string PlayerName { get; }
        List<Crime> WantedCrimes { get; }
       // VehicleExt OwnedVehicle { get; }
        PoliceResponse PoliceResponse { get; }
        List<VehicleExt> OwnedVehicles { get; }
       // List<iFruitText> TextList { get; }
        //List<iFruitContact> ContactList { get; }
        CellPhone CellPhone { get; }
        Blip CurrentGPSBlip { get; set; }

        void SetDenStatus(Gang gang, bool v);
        void AddGPSRoute(string name, Vector3 entrancePosition);
        void RemoveGPSRoute();
        //void AddContact(Gang gang);
        //void AddContact(string contactName, string contactIcon);
        //void DisableContact(string contactName);
        //bool IsContactEnabled(string contactName);
        //void AddScheduledText(string contactName, string contactIcon, string v);
        //void AddGangText(Gang gang, bool v);
    }
}
