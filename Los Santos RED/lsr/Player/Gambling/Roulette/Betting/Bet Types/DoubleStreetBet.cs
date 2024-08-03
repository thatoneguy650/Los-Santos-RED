using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette
{
    public class DoubleStreetBet : RouletteBet
    {
        public DoubleStreetBet(RoulettePocket primaryPocketID, RoulettePocket secondaryPocketID, RoulettePocket tertiaryPocketID, RoulettePocket quaternaryPocketID, RoulettePocket quinaryPocketID, RoulettePocket senaryPocketID, int amount) : base(amount)
        {
            PrimaryPocketID = primaryPocketID;
            SecondaryPocketID = secondaryPocketID;
            TeritaryPocketID = tertiaryPocketID;
            QuaternaryPocketID = quaternaryPocketID; 
            QuinaryPocketID = quinaryPocketID;
            SenaryPocketID = senaryPocketID;
        }
        public RoulettePocket PrimaryPocketID { get; set; }
        public RoulettePocket SecondaryPocketID { get; set; }
        public RoulettePocket TeritaryPocketID { get; set; }
        public RoulettePocket QuaternaryPocketID { get; set; }
        public RoulettePocket QuinaryPocketID { get; set; }
        public RoulettePocket SenaryPocketID { get; set; }
        public override string BetName => $"{PrimaryPocketID?.FullDisplay} {SecondaryPocketID?.FullDisplay} {TeritaryPocketID?.FullDisplay}  {QuaternaryPocketID?.FullDisplay} {QuinaryPocketID?.FullDisplay} {SenaryPocketID?.FullDisplay} Bet";
        public override bool IsWinner(RoulettePocket selectedPocket)
        {
            return selectedPocket.PocketID == PrimaryPocketID.PocketID || selectedPocket.PocketID == SecondaryPocketID.PocketID || selectedPocket.PocketID == TeritaryPocketID.PocketID
                || selectedPocket.PocketID == QuaternaryPocketID.PocketID || selectedPocket.PocketID == QuinaryPocketID.PocketID || selectedPocket.PocketID == SenaryPocketID.PocketID;
        }
        public override int WinAmount()
        {
            return (5 * Amount) + Amount;
        }
    }
}
