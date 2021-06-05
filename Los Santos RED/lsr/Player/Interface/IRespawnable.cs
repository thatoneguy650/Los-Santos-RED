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
        int TimesDied { get; set; }
        bool DiedInVehicle { get; }
        bool CanUndie { get; }

        void GiveMoney(int v);
        void UnSetArrestedAnimation(Ped character);
        void Reset(bool resetWanted, bool resetTimesDied, bool clearWeapons, bool clearCriminalHistory);
        void RaiseHands();
        void SetWantedLevel(int maxWantedLastLife, string v1, bool v2);
    }
}
