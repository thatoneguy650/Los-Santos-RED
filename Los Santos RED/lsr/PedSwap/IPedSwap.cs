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
        void BecomeExistingPed(Ped TargetPed, string modelName, string fullName, int money, PedVariation variation, int speechSkill, string voiceName);
        void BecomeSamePed(string modelName, string fullName, int money, PedVariation variation, string voiceName);


        void BecomeKnownPed(PedExt toBecome, bool deleteOld, bool clearNearPolice);


        void BecomeExistingPed(float Radius, bool Nearest, bool DeleteOld, bool ClearNearPolice, bool createRandomPedIfNoneReturned);
        void BecomeRandomPed();
        void BecomeCop(Agency agency);
       // void BecomeCustomPed();
        void BecomeSavedPed(string playerName, string modelName, int money, PedVariation variation, int speechSkill, string voiceName);
        void RemoveOffset();
        void AddOffset();
        void TreatAsCivilian();
        void TreatAsCop();
        void BecomeGangMember(Gang selectedItem);
        void BecomeCreatorPed();
        void BecomeEMT(Agency selectedItem);
        void BecomeFireFighter(Agency selectedItem);
        void BecomeSecurity(Agency selectedItem);
    }
}
