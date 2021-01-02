using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IActionable : IPlateChangeable
    {
        bool IsDead { get; }
        bool IsBusted { get; }
        bool IsInVehicle { get; }
        int TimesDied { get; }
        int WantedLevel { get; }

        void StartSmokingPot();
        void StartSmoking();
        void DrinkBeer();
        void CommitSuicide();
        void DisplayPlayerNotification();
        void GiveMoney(int v);
    }
}
