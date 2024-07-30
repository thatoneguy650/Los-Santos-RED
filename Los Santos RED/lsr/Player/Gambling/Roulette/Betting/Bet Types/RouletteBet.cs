using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette
{
    public class RouletteBet
    {
        public RouletteBet(int amount)
        {
            Amount = amount;
        }
        public virtual int Amount { get; set; }
        public virtual string BetName { get; set; } = "Bet";
        public virtual bool IsWinner(RoulettePocket selectedPocket)
        {
            return false;
        }
        public virtual int WinAmount()
        {
            return Amount;
        }

    }
}
