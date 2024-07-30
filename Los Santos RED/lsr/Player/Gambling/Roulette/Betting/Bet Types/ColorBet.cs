using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette
{
    public class ColorBet : RouletteBet
    {
        public ColorBet(bool isBlack, int amount) : base(amount)
        {
            IsBlack = isBlack;
        }
        public bool IsBlack { get; private set; }
        public override string BetName => IsBlack ? "Black" : "Red";// $"{}";
        public override bool IsWinner(RoulettePocket selectedPocket)
        {
            if(IsBlack)
            {
                return selectedPocket.Color == "Black";
            }
            else
            {
                return selectedPocket.Color == "Red";
            }
        }
        public override int WinAmount()
        {
            return 2 * Amount;
        }
    }
}
