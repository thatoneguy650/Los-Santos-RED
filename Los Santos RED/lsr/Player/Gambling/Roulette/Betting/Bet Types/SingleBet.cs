using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette
{
    public class SingleBet : RouletteBet
    {
        public SingleBet(RoulettePocket roulettePocket, int amount) : base(amount)
        {
            RoulettePocket = roulettePocket;
        }
        public RoulettePocket RoulettePocket { get; set; }
        public override string BetName => $"{RoulettePocket.FullDisplay} Single Bet";
        public override bool IsWinner(RoulettePocket selectedPocket)
        {
            return selectedPocket.PocketID == RoulettePocket.PocketID;
        }
        public override int WinAmount()
        {
            return (35 * Amount) + Amount;
        }
    }
}
