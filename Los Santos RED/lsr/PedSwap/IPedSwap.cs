using LosSantosRED.lsr.Player;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IPedSwap
    {
        void BecomeExistingPed(float Radius, bool Nearest, bool DeleteOld, bool ClearNearPolice, bool createRandomPedIfNoneReturned);
        void BecomeRandomPed();
        void BecomeSavedPed(string playerName, bool isMale, int money, string modelName, PedVariation currentModelVariation);
        void BecomeRandomCop();
        void BecomeCustomPed();
    }
}
