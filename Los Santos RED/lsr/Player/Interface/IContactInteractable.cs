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
        BankAccounts BankAccounts { get; }
        Destinations Destinations { get; }
        GangRelationships GangRelationships { get; }
        PlayerTasks PlayerTasks { get; }
        CellPhone CellPhone { get; }
        PoliceResponse PoliceResponse { get; }
        LocationData CurrentLocation { get; }
        Investigation Investigation { get; }
        bool IsWanted { get; }
        bool IsNotWanted { get; }
        int WantedLevel { get; }
        Respawning Respawning { get; }

        void SetWantedLevel(int v1, string v2, bool v3);
    }
}
