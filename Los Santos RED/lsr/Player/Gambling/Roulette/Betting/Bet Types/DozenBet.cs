using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette
{
    public class DozenBet : RouletteBet
    {
        public DozenBet(int level, int amount) : base(amount)
        {
            Level = level;
        }
        public int Level { get; set; } = 1;
        public override string BetName => $"{Level}{(Level == 1 ? "st" : Level == 2 ? "nd" : Level == 3 ? "rd" : "")} Dozen";
        public override bool IsWinner(RoulettePocket selectedPocket)
        {
            if(Level == 1)
            {
                return selectedPocket.PocketID >= 1 && selectedPocket.PocketID <= 12;
            }
            else if (Level == 2)
            {
                return selectedPocket.PocketID >= 13 && selectedPocket.PocketID <= 24;
            }
            else if (Level == 3)
            {
                return selectedPocket.PocketID >= 25 && selectedPocket.PocketID <= 36;
            }
            return false;
        }
        public override int WinAmount()
        {
            return (2 * Amount) + Amount;
        }
    }
}
