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
    public interface IContactInteractable
    {
        BankAccounts BankAccounts { get; }
        GPSManager GPSManager { get; }
        RelationshipManager RelationshipManager { get; }
        PlayerTasks PlayerTasks { get; }
        CellPhone CellPhone { get; }
        PoliceResponse PoliceResponse { get; }
        LocationData CurrentLocation { get; }
        Investigation Investigation { get; }
        bool IsWanted { get; }
        bool IsNotWanted { get; }
        int WantedLevel { get; }
        Respawning Respawning { get; }
        Scanner Scanner { get; }
        Vector3 Position { get; }
        bool IsCop { get; }
        ButtonPrompts ButtonPrompts { get; }
        VehicleExt CurrentVehicle { get; }
        VehicleExt CurrentLookedAtVehicle { get; }
        CriminalHistory CriminalHistory { get; }
        TaxiManager TaxiManager { get; }
        PlayerVoice PlayerVoice { get; }
        Dispatcher Dispatcher { get; }
        GangBackupManager GangBackupManager { get; }

        void AddCrime(Crime toCallIn, bool v1, Vector3 placeSeen, VehicleExt vehicleSeen, WeaponInformation weaponSeen, bool v2, bool v3, bool v4);
        void SetWantedLevel(int v1, string v2, bool v3);
    }
}
