using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette
{
    public class RowBet : RouletteBet
    {
        public RowBet(int amount) : base(amount)
        {

        }
        public override string BetName { get; set; } = "Row Bet";
        public override bool IsWinner(RoulettePocket selectedPocket)
        {
            List<int> winner = new List<int>() { -1, 0 };
            return winner.Contains(selectedPocket.PocketID);
        }
        public override int WinAmount()
        {
            return (17 * Amount) + Amount;
        }
    }
}
