using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette
{
    public class ColumnBet : RouletteBet
    {
        public ColumnBet(int level, int amount) : base(amount)
        {
            Level = level;
        }
        public int Level { get; set; } = 1;
        public override string BetName => $"{Level}{(Level == 1 ? "st" : Level == 2 ? "nd" : Level == 3 ? "rd" : "")} Column";
        public override bool IsWinner(RoulettePocket selectedPocket)
        {
            if (Level == 1)
            {
                List<int> winner = new List<int>() { 1,4,7,10,13,16,19,22,25,28,31,34 };
                return winner.Contains(selectedPocket.PocketID);
            }
            else if (Level == 2)
            {
                List<int> winner = new List<int>() { 2, 5, 8, 11, 14, 17, 20, 23, 26, 29, 32, 35 };
                return winner.Contains(selectedPocket.PocketID);
            }
            else if (Level == 3)
            {
                List<int> winner = new List<int>() { 3, 6, 9, 12, 15, 18, 21, 24, 27, 30, 33, 36 };
                return winner.Contains(selectedPocket.PocketID);
            }
            return false;
        }
        public override int WinAmount()
        {
            return (2 * Amount) + Amount;
        }
    }
}
