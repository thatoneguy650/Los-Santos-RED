using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette
{
    public class TopLineBet : RouletteBet
    {
        public TopLineBet(int amount) : base(amount)
        {

        }
        public override string BetName { get; set; } = "Top Line";
        public override bool IsWinner(RoulettePocket selectedPocket)
        {
            List<int> winner = new List<int>() { -1,0,1,2,3 };
            return winner.Contains(selectedPocket.PocketID);         
        }
        public override int WinAmount()
        {
            return (6 * Amount) + Amount;
        }
    }
}
