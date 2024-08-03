using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette
{
    public class StreetBet : RouletteBet
    {
        public StreetBet(RoulettePocket primaryPocketID, RoulettePocket secondaryPocketID, RoulettePocket tertiaryPocketID, int amount) : base(amount)
        {
            PrimaryPocketID = primaryPocketID;
            SecondaryPocketID = secondaryPocketID;
            TeritaryPocketID = tertiaryPocketID;
        }
        public RoulettePocket PrimaryPocketID { get; set; }
        public RoulettePocket SecondaryPocketID { get; set; }
        public RoulettePocket TeritaryPocketID { get; set; }
        public override string BetName => $"{PrimaryPocketID?.FullDisplay} {SecondaryPocketID?.FullDisplay} {TeritaryPocketID?.FullDisplay} Street Bet";
        public override bool IsWinner(RoulettePocket selectedPocket)
        {
            return selectedPocket.PocketID == PrimaryPocketID.PocketID || selectedPocket.PocketID == SecondaryPocketID.PocketID || selectedPocket.PocketID == TeritaryPocketID.PocketID;
        }
        public override int WinAmount()
        {
            return (11 * Amount) + Amount;
        }
    }
}
