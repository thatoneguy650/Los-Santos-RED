using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette
{
    public class SplitBet : RouletteBet
    {
        public SplitBet(RoulettePocket primaryPocketID, RoulettePocket secondaryPocketID, int amount) : base(amount)
        {
            PrimaryPocketID = primaryPocketID;
            SecondaryPocketID = secondaryPocketID;
        }
        public RoulettePocket PrimaryPocketID { get; set; }
        public RoulettePocket SecondaryPocketID { get; set; }
        public override string BetName => $"{PrimaryPocketID.PocketDisplay} {SecondaryPocketID.PocketDisplay} Split Bet";
        public override bool IsWinner(RoulettePocket selectedPocket)
        {
            return selectedPocket.PocketID == PrimaryPocketID.PocketID || selectedPocket.PocketID == SecondaryPocketID.PocketID;
        }
        public override int WinAmount()
        {
            return (17 * Amount) + Amount;
        }
    }
}
