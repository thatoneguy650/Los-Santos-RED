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
        void BecomeExistingPed(Ped TargetPed, string modelName, string fullName, int money, PedVariation variation);
        void BecomeSamePed(string modelName, string fullName, int money, PedVariation variation);





        void BecomeExistingPed(float Radius, bool Nearest, bool DeleteOld, bool ClearNearPolice, bool createRandomPedIfNoneReturned);
        void BecomeRandomPed();
        void BecomeRandomCop();
        void BecomeCustomPed();
        void BecomeSavedPed(string playerName, string modelName, int money, PedVariation variation);
        void RemoveOffset();
        void AddOffset();

    }
}
