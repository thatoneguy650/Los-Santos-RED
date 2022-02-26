using LosSantosRED.lsr.Player;
using Rage;
using RAGENativeUI.Elements;
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
        Inventory Inventory { get; }
        CellPhone CellPhone { get; }
     //   BigMessageHandler BigMessage { get; }

        void GiveMoney(int v);
        void UnSetArrestedAnimation();
        void Reset(bool resetWanted, bool resetTimesDied, bool clearWeapons, bool clearCriminalHistory, bool clearInventory, bool clearIntoxication, bool resetGangRelationships, bool clearVehicleOwnership, bool resetCellphone, bool clearActiveTasks);
        void RaiseHands();
        void SetWantedLevel(int maxWantedLastLife, string v1, bool v2);
    }
}
