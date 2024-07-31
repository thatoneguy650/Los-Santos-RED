using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette
{
    public class HalfBet : RouletteBet
    {
        public HalfBet(bool isFirst, int amount) : base(amount)
        {
            IsFirst = isFirst;
        }
        public bool IsFirst { get; private set; }
        public override string BetName => IsFirst ? "1 to 18" : "19 to 36";// $"{}";
        public override bool IsWinner(RoulettePocket selectedPocket)
        {
            if (IsFirst)
            {
                return selectedPocket.PocketID >= 1 && selectedPocket.PocketID <= 18;
            }
            else
            {
                return selectedPocket.PocketID >= 19 && selectedPocket.PocketID <= 36;
            }
        }
        public override int WinAmount()
        {
            return 2 * Amount;
        }
    }
}
