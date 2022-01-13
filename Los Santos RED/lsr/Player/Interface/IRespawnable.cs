using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IRespawnable
    {
        int Money { get; }
        PoliceResponse PoliceResponse { get; }
        int MaxWantedLastLife { get; set; }
        int WantedLevel { get; }
        bool DiedInVehicle { get; }
        bool CanUndie { get; }
        int TimesDied { get;  }
        string PlayerName { get; }
        string ModelName { get; }

        void GiveMoney(int v);
        void UnSetArrestedAnimation();
        void Reset(bool resetWanted, bool resetTimesDied, bool clearWeapons, bool clearCriminalHistory, bool clearInventory, bool clearIntoxication, bool resetGangRelationships);
        void RaiseHands();
        void SetWantedLevel(int maxWantedLastLife, string v1, bool v2);
        void ClearInventory();
    }
}
