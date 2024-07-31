using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette
{
    public class CornerBet : RouletteBet
    {
        public CornerBet(RoulettePocket primaryPocketID, RoulettePocket secondaryPocketID, RoulettePocket tertiaryPocketID, RoulettePocket quaternaryPocketID, int amount) : base(amount)
        {
            PrimaryPocketID = primaryPocketID;
            SecondaryPocketID = secondaryPocketID;
            TeritaryPocketID = tertiaryPocketID;
            QuaternaryPocketID = quaternaryPocketID;
        }
        public RoulettePocket PrimaryPocketID { get; set; }
        public RoulettePocket SecondaryPocketID { get; set; }
        public RoulettePocket TeritaryPocketID { get; set; }
        public RoulettePocket QuaternaryPocketID { get; set; }
        public override string BetName => $"{PrimaryPocketID.FullDisplay} {SecondaryPocketID.FullDisplay} {TeritaryPocketID.FullDisplay} {QuaternaryPocketID.FullDisplay} Corner Bet";
        public override bool IsWinner(RoulettePocket selectedPocket)
        {
            return selectedPocket.PocketID == PrimaryPocketID.PocketID || selectedPocket.PocketID == SecondaryPocketID.PocketID || selectedPocket.PocketID == TeritaryPocketID.PocketID || selectedPocket.PocketID == QuaternaryPocketID.PocketID;
        }
        public override int WinAmount()
        {
            return (8 * Amount) + Amount;
        }
    }
}
