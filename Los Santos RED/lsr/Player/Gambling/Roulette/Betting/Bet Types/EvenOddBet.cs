using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette
{
    public class EvenOddBet : RouletteBet
    {
        public EvenOddBet(bool isodd, int amount) : base(amount)
        {
            IsOdd = isodd;
        }
        public bool IsOdd { get; private set; }
        public override string BetName => IsOdd ? "Odd" : "Even";// $"{}";
        public override bool IsWinner(RoulettePocket selectedPocket)
        {
            if (IsOdd)
            {
                return selectedPocket.IsOdd;
            }
            else
            {
                return !selectedPocket.IsOdd;
            }
        }
        public override int WinAmount()
        {
            return 2 * Amount;
        }
    }
}
